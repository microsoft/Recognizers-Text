from abc import ABC, abstractproperty
from typing import Dict, Pattern, Optional, List
import regex

from recognizers_text.extractor import ExtractResult
from recognizers_text.parser import Parser, ParseResult

class NumberParserConfiguration(ABC):
    @abstractproperty
    def cardinal_number_map(self) -> Dict[str, int]: pass
    @abstractproperty
    def ordinal_number_map(self) -> Dict[str, int]: pass
    @abstractproperty
    def round_number_map(self) -> Dict[str, int]: pass
    @abstractproperty
    def culture_info(self) -> str: pass
    @abstractproperty
    def digital_number_regex(self) -> Pattern: pass
    @abstractproperty
    def fraction_marker_token(self) -> str: pass
    @abstractproperty
    def negative_number_sign_regex(self) -> Pattern: pass
    @abstractproperty
    def half_a_dozen_regex(self) -> Pattern: pass
    @abstractproperty
    def half_a_dozen_text(self) -> str: pass
    @abstractproperty
    def lang_marker(self) -> str: pass
    @abstractproperty
    def non_decimal_separator_char(self) -> str: pass
    @abstractproperty
    def decimal_separator_char(self) -> str: pass
    @abstractproperty
    def word_separator_token(self) -> str: pass
    @abstractproperty
    def written_decimal_separator_texts(self) -> List[str]: pass
    @abstractproperty
    def written_group_separator_texts(self) -> List[str]: pass
    @abstractproperty
    def written_integer_separator_texts(self) -> List[str]: pass
    @abstractproperty
    def written_fraction_separator_texts(self) -> List[str]: pass
    @abstractproperty
    def normalize_token_set(self, tokens: List[str], context: ParseResult) -> List[str]: pass
    @abstractproperty
    def resolve_composite_number(self, number_str: str) -> int: pass
    
class BaseNumberParser(Parser):
    def __init__(self, config: NumberParserConfiguration):
        self.config: NumberParserConfiguration = config
        self.supported_types: List[str] = list()

        single_int_frac = f"{self.config.word_separator_token}| -|{self._get_key_regex(self.config.cardinal_number_map.keys())}|{self._get_key_regex(self.config.ordinal_number_map.keys())}"
        self.text_number_regex: Pattern = regex.compile(fr"(?=\b)(${single_int_frac})(?=\b)", flags=regex.I | regex.S)
        self.arabic_number_regex: Pattern = regex.compile(r"\d+", flags=regex.I | regex.S)
        self.round_number_set: List[str] = list(self.config.round_number_map.keys())

    def parse(self, source: ExtractResult) -> Optional[ParseResult]:
        # check if the parser is configured to support specific types
        if source.type not in self.supported_types:
            return None
        ret: Optional[ParseResult] = None
        extra = source.data
        if not extra:
            if self.arabic_number_regex.search(source.text):
                extra = "Num"
            else:
                extra = self.config.lang_marker
        
        #Resolve symbol prefix
        is_negative = False
        match_negative = source.text.search(self.config.negative_number_sign_regex)

        if match_negative:
            is_negative = True
            source.text = source.text[len(match_negative[1]):]
        
        if "Num" in extra:
            ret = self._digit_number_parse(source)
        elif regex.search(fr'Frac${self.config.langMarker}', extra): # Frac is a special number, parse via another method
            ret = self._frac_like_number_parse(source)
        elif self.config.lang_marker in extra:
            ret = self._text_number_parse(source)
        elif "Pow" in extra:
            ret = self._power_number_parse(source)
        
        if ret and ret.value:
            if is_negative:
                # Recover to the original extracted Text
                ret.text = match_negative[1] + source.text
                # Check if ret.value is a BigNumber
                if type(ret.value) is int:
                    ret.value = -ret.value
                else:
                    ret.value.s = -1
        
            ret.resolution_str = self.config.culture_info.format(ret.value) if self.config.culture_info else str(ret.value)

        return ret

    def _get_key_regex(self, keys: List[str]) -> str:
        return str.join('|', sorted(keys, key = len, reverse=True))

    def _digit_number_parse(self, ext_result: ExtractResult) -> ParseResult:
        result =  ParseResult()
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

        matches = regex.search(self.config.digital_number_regex, handle)
        if matches:
            for match in matches:
                # TODO chek if it is necessary in python HACK: Matching regex may be buggy, may include a digit before the unit
                # match.value = match.value.replace(/\d/g, '')
                # match.length = match.value.length

                rep = self.config.round_number_map.get(match.value)
                # \\s+ for filter the spaces.
                power *= rep

                # tslint:disable-next-line:no-conditional-assignment
                tmp_index = handle.index(match.value, start_index)
                while tmp_index >= 0:
                    front = handle[0:tmp_index].rstrip()
                    start_index = len(front)
                    handle = front + handle[tmp_index + len(match):]
                    tmp_index = handle.index(match.value, start_index)

        # scale used in the calculate of double
        result.value = self.__get_digital_value(handle, power)

        return result
    
    def _is_digit(self, c: str) -> bool:
        return c.isdigit()
    
    def _frac_like_number_parse(self, ext_result: ExtractResult) -> ParseResult:
        pass
    
    def _text_number_parse(self, ext_result: ExtractResult) -> ParseResult:
        pass

    def _power_number_parse(self, ext_result: ExtractResult) -> ParseResult:
        pass

    def __split_multi(self, source: str, tokens: List[str]) -> List[str]:
        # We can use the first token as a temporary join character
        tmp = tokens[0]
        for token in tokens:
            source = source.split(token).join(tmp)
        return source.split(tmp)

    def __get_matches(self, source: str) -> List[str]:
        matches = list(regex.finditer(self._text_number_parse, source))
        return filter(None, map(lambda m : m.group().lower(), matches))

    # Test if big and combine with small.
    # e.g. "hundred" can combine with "thirty" but "twenty" can't combine with "thirty".
    def __is_composable(self, big: int, small: int) -> bool:
        base_num = 100 if small > 10 else 10
        return big % base_num == 0 and big / base_num >=1

    def __get_int_value(self, matches: List[str]) -> int:
        is_end = [False] * len(matches)

        tmp_val = 0
        end_flag = 1

        # Scan from end to start, find the end word
        for i in range(len(matches)-1, 0, -1):
            if matches[i] in self.round_number_set:
                # if false,then continue, you will meet hundred first, then thousand.
                if (end_flag > self.config.round_number_map[matches[i]]):
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
                    match_value = self.config.cardinal_number_map.get(match, self.config.ordinal_number_map[match])

                    #This is just for ordinal now. Not for fraction ever.
                    if ordinal:
                        frac_part = self.config.ordinal_number_map[match]
                        if len(tmp_stack) > 0:
                            int_part = tmp_stack.pop()
                            # if intPart >= fracPart, it means it is an ordinal number
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
            # Calculate the part like "thirty-one"
            mul_value = 1
            if last_index != len(is_end):
                part_value = self.__get_int_value(matches[last_index:len(is_end)])
                tmp_val += mul_value * part_value
        return tmp_val

    def __get_point_value(self, matches: List[str]) -> float:
        result = 0
        first_match = matches[0]
        if first_match in self.config.cardinal_number_map and self.config.cardinal_number_map[first_match] >= 10:
            prefix = '0.'
            tmp_int = self.__get_int_value(matches)
            result = float(prefix + str(tmp_int))
        else:
            scale = 0.1
            for match in matches:
                result += scale * self.config.cardinal_number_map[match]
                scale *= 0.1

        return result

    def __get_digital_value(self, digitstr: str, power: int) -> float:
        tmp: float = 0
        scale: float = 10
        dot: bool = False
        negative: bool = False
        fraction: bool = False

        call_stack: List[int] = list()

        for c in digitstr:
            if c == '/':
                fraction = True

            if c == ' ' or c == '/':
                call_stack.append(tmp)
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
                negative = True

        call_stack.append(tmp)

        # is the number is a fraction.
        cal_result = 0
        if fraction:
            deno = call_stack.pop()
            mole = call_stack.pop()
            cal_result += mole / deno
        
        for n in call_stack:
            cal_result += n

        cal_result *= power

        return cal_result if not negative else -cal_result

class BasePercentageParser(BaseNumberParser):
    def parse(self, source: ExtractResult) -> Optional[ParseResult]:
        original = source.text

        # do replace text & data from extended info
        if type(source.data) is list:
            source.text = source.data[0]
            source.data = source.data[1].data
        
        result: ParseResult() = super().parse(source)

        if len(result.resolution_str) > 0:
            if not result.resolution_str.strip().endswith('%'):
                result.resolution_str = result.resolution_str.strip() + '%'

        result.data = source.text
        result.text = original

        return result