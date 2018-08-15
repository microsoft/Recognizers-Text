from abc import ABC, abstractmethod
from typing import Dict, Pattern, Optional, List
from decimal import Decimal, getcontext
import regex
from recognizers_text.utilities import RegExpUtility
from recognizers_text.extractor import ExtractResult
from recognizers_text.parser import Parser, ParseResult
from recognizers_number.culture import CultureInfo
from recognizers_number.number.constants import Constants

getcontext().prec = 15

class NumberParserConfiguration(ABC):
    @property
    @abstractmethod
    def cardinal_number_map(self) -> Dict[str, int]:
        pass

    @property
    @abstractmethod
    def ordinal_number_map(self) -> Dict[str, int]:
        pass

    @property
    @abstractmethod
    def round_number_map(self) -> Dict[str, int]:
        pass

    @property
    @abstractmethod
    def culture_info(self) -> CultureInfo:
        pass

    @property
    @abstractmethod
    def digital_number_regex(self) -> Pattern:
        pass

    @property
    @abstractmethod
    def fraction_marker_token(self) -> str:
        pass

    @property
    @abstractmethod
    def negative_number_sign_regex(self) -> Pattern:
        pass

    @property
    @abstractmethod
    def half_a_dozen_regex(self) -> Pattern:
        pass

    @property
    @abstractmethod
    def half_a_dozen_text(self) -> str:
        pass

    @property
    @abstractmethod
    def lang_marker(self) -> str:
        pass

    @property
    @abstractmethod
    def non_decimal_separator_char(self) -> str:
        pass

    @property
    @abstractmethod
    def decimal_separator_char(self) -> str:
        pass

    @property
    @abstractmethod
    def word_separator_token(self) -> str:
        pass

    @property
    @abstractmethod
    def written_decimal_separator_texts(self) -> List[str]:
        pass

    @property
    @abstractmethod
    def written_group_separator_texts(self) -> List[str]:
        pass

    @property
    @abstractmethod
    def written_integer_separator_texts(self) -> List[str]:
        pass

    @property
    @abstractmethod
    def written_fraction_separator_texts(self) -> List[str]:
        pass

    @property
    @abstractmethod
    def normalize_token_set(self, tokens: List[str], context: ParseResult) -> List[str]:
        pass

    @property
    @abstractmethod
    def resolve_composite_number(self, number_str: str) -> int:
        pass

class BaseNumberParser(Parser):
    def __init__(self, config: NumberParserConfiguration):
        self.config: NumberParserConfiguration = config
        self.supported_types: List[str] = list()

        single_int_frac = f'{self.config.word_separator_token}| -|{self._get_key_regex(self.config.cardinal_number_map.keys())}|{self._get_key_regex(self.config.ordinal_number_map.keys())}'
        self.text_number_regex: Pattern = RegExpUtility.get_safe_reg_exp(fr'(?=\b)({single_int_frac})(?=\b)', flags=regex.I | regex.S)
        self.arabic_number_regex: Pattern = RegExpUtility.get_safe_reg_exp(r'\d+', flags=regex.I | regex.S)
        self.round_number_set: List[str] = list(self.config.round_number_map.keys())

    def parse(self, source: ExtractResult) -> Optional[ParseResult]:
        # check if the parser is configured to support specific types
        if self.supported_types and source.type not in self.supported_types:
            return None
        ret: Optional[ParseResult] = None
        extra = source.data if isinstance(source.data, str) else None
        if not extra:
            if self.arabic_number_regex.search(source.text):
                extra = 'Num'
            else:
                extra = self.config.lang_marker

        #Resolve symbol prefix
        is_negative = False
        match_negative = regex.search(self.config.negative_number_sign_regex, source.text)

        if match_negative:
            is_negative = True
            source.text = source.text[len(match_negative[1]):]

        if 'Num' in extra:
            ret = self._digit_number_parse(source)
        elif regex.search(fr'Frac{self.config.lang_marker}', extra): # Frac is a special number, parse via another method
            ret = self._frac_like_number_parse(source)
        elif self.config.lang_marker in extra:
            ret = self._text_number_parse(source)
        elif 'Pow' in extra:
            ret = self._power_number_parse(source)

        if ret and ret.value is not None:
            if is_negative:
                # Recover to the original extracted Text
                ret.text = match_negative[1] + source.text
                ret.value = ret.value * -1
            # Use culture_info to format values
            ret.resolution_str = self.config.culture_info.format(ret.value) if self.config.culture_info is not None else repr(ret.value)

        return ret

    def _get_key_regex(self, keys: List[str]) -> str:
        return str.join('|', sorted(keys, key=len, reverse=True))

    def _digit_number_parse(self, ext_result: ExtractResult) -> ParseResult:
        result = ParseResult()
        result.start = ext_result.start
        result.length = ext_result.length
        result.text = ext_result.text
        result.type = ext_result.type

        # [1] 24
        # [2] 12 32/33
        # [3] 1,000,000
        # [4] 234.567
        # [5] 44/55
        # [6] 2 hundred
        # dot occured.

        power = 1
        tmp_index = -1
        start_index = 0
        handle = ext_result.text.lower()

        matches = list(regex.finditer(self.config.digital_number_regex, handle))
        if matches:
            for match in matches:
                rep = self.config.round_number_map.get(match.group())
                # \\s+ for filter the spaces.
                power *= rep

                tmp_index = handle.find(match.group(), start_index)
                while tmp_index >= 0:
                    front = handle[0:tmp_index].rstrip()
                    start_index = len(front)
                    handle = front + handle[tmp_index + len(match):]
                    tmp_index = handle.find(match.group(), start_index)

        # scale used in the calculate of double
        result.value = self._get_digital_value(handle, power)

        return result

    def _is_digit(self, c: str) -> bool:
        return c.isdigit()

    def _frac_like_number_parse(self, ext_result: ExtractResult) -> ParseResult:
        result = ParseResult()
        result.start = ext_result.start
        result.length = ext_result.length
        result.text = ext_result.text
        result.type = ext_result.type

        result_text = ext_result.text.lower()
        if regex.search(self.config.fraction_marker_token, result_text):
            over_index = result_text.find(self.config.fraction_marker_token)
            small_part = result_text[0:over_index].strip()
            big_part = result_text[over_index + len(self.config.fraction_marker_token):len(result_text)].strip()
            small_value = self._get_digital_value(small_part, 1) if self._is_digit(small_part[0]) else self.__get_int_value(self.__get_matches(small_part))
            big_value = self._get_digital_value(big_part, 1) if self._is_digit(big_part[0]) else self.__get_int_value(self.__get_matches(big_part))

            result.value = small_value / big_value
        else:
            words = list(filter(lambda x: x, result_text.split(' ')))
            frac_words = self.config.normalize_token_set(words, result)

            # Split fraction with integer
            split_index = len(frac_words) - 1
            current_value = self.config.resolve_composite_number(frac_words[split_index])
            round_value = 1

            for split_index in range(len(frac_words) - 2, -1, -1):
                if (frac_words[split_index] in self.config.written_fraction_separator_texts
                        or frac_words[split_index] in self.config.written_integer_separator_texts):
                    continue
                previous_value = current_value
                current_value = self.config.resolve_composite_number(frac_words[split_index])

                sm_hundreds = 100

                # previous: hundred
                # current: one
                if ((previous_value >= sm_hundreds and previous_value > current_value)
                        or (previous_value < sm_hundreds and self.__is_composable(current_value, previous_value))):
                    if (previous_value < sm_hundreds and current_value >= round_value):
                        round_value = current_value
                    elif (previous_value < sm_hundreds and current_value < round_value):
                        split_index += 1
                        break

                    # current is the first word
                    if split_index == 0:
                        # scan, skip the first word
                        split_index = 1
                        while split_index <= len(frac_words) - 2:
                            # e.g. one hundred thousand
                            # frac[i+1] % 100 and frac[i] % 100 = 0
                            if (self.config.resolve_composite_number(frac_words[split_index]) >= sm_hundreds
                                    and not frac_words[split_index + 1] in self.config.written_fraction_separator_texts
                                    and self.config.resolve_composite_number(frac_words[split_index + 1]) < sm_hundreds):
                                split_index += 1
                                break
                            split_index += 1
                        break
                    continue
                split_index += 1
                break

            frac_part = []
            for i in range(split_index, len(frac_words)):
                if frac_words[i].find('-') > -1:
                    split = frac_words[i].split('-')
                    frac_part.append(split[0])
                    frac_part.append('-')
                    frac_part.append(split[1])
                else:
                    frac_part.append(frac_words[i])

            frac_words = frac_words[:split_index]

            # denomi = denominator
            denomi_value = self.__get_int_value(frac_part)
            # Split mixed number with fraction
            numer_value = 0
            int_value = 0

            mixed_index = len(frac_words)
            for i in range(len(frac_words) - 1, -1, -1):
                if (i < len(frac_words) - 1 and frac_words[i] in self.config.written_fraction_separator_texts):
                    numer_str = ' '.join(frac_words[i + 1:len(frac_words)])
                    numer_value = self.__get_int_value(self.__get_matches(numer_str))
                    mixed_index = i + 1
                    break

            int_str = ' '.join(frac_words[0:mixed_index])
            int_value = self.__get_int_value(self.__get_matches(int_str))

            # Find mixed number
            if (mixed_index != len(frac_words) and numer_value < denomi_value):
                # int_value + numer_value / denomi_value
                result.value = int_value + numer_value / denomi_value
            else:
                # (int_value + numer_value) / denomi_value
                result.value = (int_value + numer_value) / denomi_value

            # Convert to float for fixed float point vs. exponential notation consistency /w C#/TS/JS
            result.value = float(result.value)
        return result

    def _text_number_parse(self, ext_result: ExtractResult) -> ParseResult:
        result = ParseResult(ext_result)

        handle = regex.sub(self.config.half_a_dozen_regex, self.config.half_a_dozen_text, ext_result.text.lower())
        num_group = self.__split_multi(handle, list(filter(lambda x: x is not None, self.config.written_decimal_separator_texts)))

        int_part = num_group[0]

        matches = list(map(lambda x: x.group().lower(), list(regex.finditer(self.text_number_regex, int_part)))) if int_part else list()

        int_part_real = self.__get_int_value(matches)

        point_part_real = Decimal(0)
        if len(num_group) == 2:
            point_part = num_group[1]
            matches = list(map(lambda x: x.group().lower(), list(regex.finditer(self.text_number_regex, point_part))))
            point_part_real += self.__get_point_value(matches)

        result.value = int_part_real + Decimal(point_part_real)
        return result

    def _power_number_parse(self, ext_result: ExtractResult) -> ParseResult:
        result = ParseResult(ext_result)

        handle = ext_result.text.upper()
        exponent = '^' not in ext_result.text

        # [1] 1e10
        # [2] 1.1^-23
        call_stack = list()
        scale = 10
        dot = False
        negative = False
        tmp = 0

        for i in range(len(handle)):
            c = handle[i]
            if c in ['^', 'E']:
                if negative:
                    call_stack.append(-tmp)
                else:
                    call_stack.append(tmp)
                tmp = 0
                scale = 10
                dot = False
                negative = False
            elif c.isdigit():
                if dot:
                    tmp = tmp + scale * int(c)
                    scale *= 0.1
                else:
                    tmp = tmp * scale + int(c)
            elif c == self.config.decimal_separator_char:
                dot = True
                scale = 0.1
            elif c == '-':
                negative = not negative
            elif c == '+':
                continue
            if i == len(handle)-1:
                if negative:
                    call_stack.append(-tmp)
                else:
                    call_stack.append(tmp)
        result_value = 0
        a = Decimal(call_stack.pop(0))
        b = Decimal(call_stack.pop(0))
        if exponent:
            result_value = getcontext().multiply(a, getcontext().power(Decimal(10), b))
        else:
            result_value = getcontext().power(a, b)

        result.value = result_value
        result.resolution_str = str(result_value)

        return result

    def __split_multi(self, source: str, tokens: List[str]) -> List[str]:
        # We can use the first token as a temporary join character
        tmp = tokens[0]
        for token in tokens:
            source = tmp.join(source.split(token))
        return source.split(tmp)

    def __get_matches(self, source: str) -> List[str]:
        matches = list(regex.finditer(self.text_number_regex, source))
        return list(filter(None, map(lambda m: m.group().lower(), matches)))

    # Test if big and combine with small.
    # e.g. 'hundred' can combine with 'thirty' but 'twenty' can't combine with 'thirty'.
    def __is_composable(self, big: int, small: int) -> bool:
        base_num = 100 if small > 10 else 10
        return big % base_num == 0 and big / base_num >= 1

    def __get_int_value(self, matches: List[str]) -> Decimal:
        is_end = [False] * len(matches)

        tmp_val = 0
        end_flag = 1

        # Scan from end to start, find the end word
        for i in range(len(matches)-1, 0, -1):
            if matches[i] in self.round_number_set:
                # if false,then continue, you will meet hundred first, then thousand.
                if end_flag > self.config.round_number_map[matches[i]]:
                    continue
                is_end[i] = True
                end_flag = self.config.round_number_map[matches[i]]

        if end_flag == 1:
            tmp_stack = list()
            old_sym = ''
            for match in matches:
                cardinal = match in self.config.cardinal_number_map
                ordinal = match in self.config.ordinal_number_map
                if cardinal or ordinal:
                    match_value = self.config.cardinal_number_map.get(match, None)
                    if match_value is None:
                        match_value = self.config.ordinal_number_map.get(match, None)

                    #This is just for ordinal now. Not for fraction ever.
                    if ordinal:
                        frac_part = self.config.ordinal_number_map[match]
                        if tmp_stack:
                            int_part = tmp_stack.pop()
                            # if intPart >= frac_part, it means it is an ordinal number
                            # it begins with an integer, ends with an ordinal
                            # e.g. ninety-ninth
                            if int_part >= frac_part:
                                tmp_stack.append(int_part + frac_part)
                            else:
                                for val in tmp_stack:
                                    int_part += val
                                tmp_stack.clear()
                                tmp_stack.append(int_part * frac_part)
                        else:
                            tmp_stack.append(frac_part)
                    elif match in self.config.cardinal_number_map:
                        if old_sym == '-':
                            tmp = tmp_stack.pop() + match_value
                            tmp_stack.append(tmp)
                        elif old_sym == self.config.written_integer_separator_texts[0] or len(tmp_stack) < 2:
                            tmp_stack.append(match_value)
                        elif len(tmp_stack) >= 2:
                            tmp = tmp_stack.pop() + match_value
                            tmp += tmp_stack.pop()
                            tmp_stack.append(tmp)
                else:
                    complex_val = self.config.resolve_composite_number(match)
                    if complex_val != 0:
                        tmp_stack.append(complex_val)
                old_sym = match
            for tmp in tmp_stack:
                tmp_val += tmp
        else:
            last_index = 0
            part_value = 1
            mul_value = 1
            for i in range(len(is_end)):
                if is_end[i]:
                    mul_value = self.config.round_number_map[matches[i]]
                    part_value = 1
                    if i != 0:
                        part_value = self.__get_int_value(matches[last_index:i])
                    tmp_val += mul_value * part_value
                    last_index = i + 1
            # Calculate the part like 'thirty-one'
            mul_value = 1
            if last_index != len(is_end):
                part_value = self.__get_int_value(matches[last_index:len(is_end)])
                tmp_val += mul_value * part_value
        return Decimal(tmp_val)

    def __get_point_value(self, matches: List[str]) -> Decimal:
        result = 0
        first_match = matches[0]
        if first_match in self.config.cardinal_number_map and self.config.cardinal_number_map[first_match] >= 10:
            prefix = '0.'
            tmp_int = self.__get_int_value(matches)
            result = Decimal(prefix + str(tmp_int))
        else:
            scale = Decimal(0.1)
            for match in matches:
                result += scale * Decimal(self.config.cardinal_number_map[match])
                scale *= Decimal(0.1)

        return result

    def _get_digital_value(self, digitstr: str, power: int) -> Decimal:
        tmp: Decimal = Decimal(0)
        scale: Decimal = Decimal(10)
        dot: bool = False
        negative: bool = False
        fraction: bool = '/' in digitstr

        call_stack: List[Decimal] = list()

        for c in digitstr:
            if not fraction and (c == self.config.non_decimal_separator_char or c == ' ' or c == Constants.NO_BREAK_SPACE):
                continue

            if c == ' ' or c == '/':
                call_stack.append(tmp)
                tmp = Decimal(0)
            elif c.isdigit():
                if dot:
                    tmp = getcontext().add(tmp, getcontext().multiply(scale, Decimal(c)))
                    scale = getcontext().multiply(scale, Decimal(0.1))
                else:
                    tmp = getcontext().add(getcontext().multiply(tmp, scale), Decimal(c))
            elif c == self.config.decimal_separator_char:
                dot = True
                scale = Decimal(0.1)
            elif c == '-':
                negative = True

        call_stack.append(tmp)

        # is the number is a fraction.
        cal_result = Decimal(0)
        if fraction:
            deno = call_stack.pop()
            mole = call_stack.pop()
            cal_result = getcontext().add(cal_result, getcontext().divide(mole, deno))

        for n in call_stack:
            cal_result = getcontext().add(cal_result, n)

        cal_result = getcontext().multiply(cal_result, Decimal(power))

        return cal_result if not negative else cal_result * -1

class BasePercentageParser(BaseNumberParser):
    def parse(self, source: ExtractResult) -> Optional[ParseResult]:
        original = source.text

        # do replace text & data from extended info
        if isinstance(source.data, list):
            source.text = source.data[0]
            source.data = source.data[1].data

        result: ParseResult = super().parse(source)

        if not result.resolution_str is None and result.resolution_str:
            if not result.resolution_str.strip().endswith('%'):
                result.resolution_str = result.resolution_str.strip() + '%'

        result.data = source.text
        result.text = original

        return result
