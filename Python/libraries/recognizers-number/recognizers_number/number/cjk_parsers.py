from abc import abstractmethod
from typing import List, Dict, Pattern, Optional
from collections import namedtuple
from decimal import Decimal, getcontext
import copy
import regex

from recognizers_text.utilities import RegExpUtility
from recognizers_text.culture import Culture
from recognizers_text.extractor import ExtractResult
from recognizers_text.parser import ParseResult
from recognizers_number.number.parsers import BaseNumberParser, NumberParserConfiguration
from recognizers_number.culture import CultureInfo

getcontext().prec = 15

class CJKNumberParserConfiguration(NumberParserConfiguration):
    @property
    @abstractmethod
    def zero_to_nine_map(self) -> Dict[str, int]:
        pass

    @property
    @abstractmethod
    def round_number_map_char(self) -> Dict[str, int]:
        pass
    
    @property
    @abstractmethod
    def full_to_half_map(self) -> Dict[str, int]:
        pass
    
    @property
    @abstractmethod
    def trato_sim_map(self) -> Dict[str, int]:
        pass
    
    @property
    @abstractmethod
    def unit_map(self) -> Dict[str, int]:
        pass
    
    @property
    @abstractmethod
    def round_direct_list(self) -> List[str]:
        pass
    
    @property
    @abstractmethod
    def digit_num_regex(self) -> Pattern:
        pass
    
    @property
    @abstractmethod
    def dozen_regex(self) -> Pattern:
        pass

    @property
    @abstractmethod
    def percentage_regex(self) -> Pattern:
        pass
    
    @property
    @abstractmethod
    def double_and_round_regex(self) -> Pattern:
        pass
    
    @property
    @abstractmethod
    def frac_split_regex(self) -> Pattern:
        pass

    @property
    @abstractmethod
    def point_regex(self) -> Pattern:
        pass

    @property
    @abstractmethod
    def spe_get_number_regex(self) -> Pattern:
        pass
    
    @property
    @abstractmethod
    def pair_regex(self) -> Pattern:
        pass

    @property
    @abstractmethod
    def round_number_integer_regex(self) -> Pattern:
        pass

class CJKNumberParser(BaseNumberParser):
    def __init__(self, config: CJKNumberParserConfiguration):
        super().__init__(config)
        self.config = config

    def __format(self, value: object) -> str:
        if self.config.culture_info is None:
            return str(value)
        return self.config.culture_info.format(value)

    def parse(self, source: ExtractResult) -> Optional[ParseResult]:
        result: ParseResult
        extra: str = source.data
        simplified_source: ExtractResult = copy.deepcopy(source)
        simplified_source.text = self.replace_trad_with_simplified(source.text)

        if not extra:
            return result

        if 'Per' in extra:
            result = self.per_parse(simplified_source)
        elif 'Num' in extra:
            simplified_source.text = self.replace_full_with_half(simplified_source.text)
            result = self._digit_number_parse(simplified_source)
            if regex.search(self.config.negative_number_sign_regex, simplified_source.text) and result.value > 0:
                result.value = result.value * -1
            result.resolution_str = self.__format(result.value)
        elif 'Pow' in extra:
            simplified_source.text = self.replace_full_with_half(simplified_source.text)
            result = self._power_number_parse(simplified_source)
            result.resolution_str = self.__format(result.value)
        elif 'Frac' in extra:
            result = self.frac_parse(simplified_source)
        elif 'Dou' in extra:
            result = self.dou_parse(simplified_source)
        elif 'Integer' in extra:
            result = self.int_parse(simplified_source)
        elif 'Ordinal' in extra:
            result = self.ord_parse(simplified_source)

        if result is not None:
            result.text = source.text

        return result

    def replace_trad_with_simplified(self, source: str) -> str:
        if source is None or not source.strip():
            return source
        if self.config.trato_sim_map is None:
            return source
        return ''.join(map(lambda c: self.config.trato_sim_map.get(c, c), source))

    def replace_full_with_half(self, source: str) -> str:
        if source is None or not source.strip():
            return source
        return ''.join(map(lambda c: self.config.full_to_half_map.get(c, c), source))

    def replace_unit(self, source: str) -> str:
        if source is None or not source.strip():
            return source
        for (k, v) in self.config.unit_map.items():
            source = source.replace(k, v)
        return source

    def per_parse(self, source: ExtractResult) -> ParseResult:
        result = ParseResult(source)
        source_text = source.text
        power = 1

        if 'Spe' in source.data:
            source_text = self.replace_full_with_half(source_text)
            source_text = self.replace_unit(source_text)

            if source_text == '半額' or source_text == '半折':
                result.value = 50
            elif source_text == '10成' or source_text == '10割' or source_text == '十割':
                result.value = 100
            else:
                matches = list(regex.finditer(self.config.spe_get_number_regex, source_text))
                int_number: int
                if len(matches) == 2:
                    int_number_char = matches[0].group()[0]
                    if int_number_char == '対' or int_number_char == '对':
                        int_number = 5
                    elif int_number_char == '十' or int_number_char == '拾':
                        int_number = 10
                    else:
                        int_number = self.config.zero_to_nine_map[int_number_char]

                    point_number_char = matches[1].group()[0]
                    point_number: float
                    if point_number_char == '半':
                        point_number = 0.5
                    else:
                        point_number = self.config.zero_to_nine_map[point_number_char] * 0.1

                    result.value = (int_number + point_number) * 10
                elif len(matches) == 5:
                    # Deal the Japanese percentage case like "xxx割xxx分xxx厘", get the integer value and convert into result.
                    int_number_char = matches[0].group()[0]
                    point_number_char = matches[1].group()[0]
                    dot_number_char = matches[3].group()[0]

                    point_number = self.config.zero_to_nine_map[point_number_char] * 0.1
                    dot_number = self.config.zero_to_nine_map[dot_number_char] * 0.01

                    int_number = self.config.zero_to_nine_map[int_number_char]

                    result.value = (int_number + point_number + dot_number) * 10
                else:
                    int_number_char = matches[0].group()[0]
                    if int_number_char == '对' or int_number_char == '対':
                        int_number = 5
                    elif int_number_char == '十' or int_number_char == '拾':
                        int_number = 10
                    else:
                        int_number = self.config.zero_to_nine_map[int_number_char]
                    result.value = int_number * 10

        elif 'Num' in source.data:
            double_match = regex.search(self.config.percentage_regex, source_text)
            double_text = double_match.group()

            if any(x for x in ['k', 'K', 'ｋ', 'Ｋ'] if x in double_text):
                power = 1000
            elif any(x for x in ['M', 'Ｍ'] if x in double_text):
                power = 1000000
            elif any(x for x in ['G', 'Ｇ'] if x in double_text):
                power = 1000000000
            elif any(x for x in ['T', 'Ｔ'] if x in double_text):
                power = 1000000000000
            result.value = self.get_digit_value(double_text, power)

        else:
            double_match = regex.search(self.config.percentage_regex, source_text)
            double_text = self.replace_unit(double_match.group())

            split_result = regex.split(self.config.point_regex, double_text)
            if split_result[0] == '':
                split_result[0] = '零'

            double_value = self.get_int_value(split_result[0])
            if len(split_result) == 2:
                if regex.search(self.config.negative_number_sign_regex, split_result[0]) is not None:
                    double_value -= self.get_point_value(split_result[1])
                else:
                    double_value += self.get_point_value(split_result[1])
            result.value = double_value

        result.resolution_str = self.__format(result.value) + '%'
        return result

    def frac_parse(self, source: ExtractResult) -> ParseResult:
        result = ParseResult(source)

        source_text = source.text
        split_result = regex.split(self.config.frac_split_regex, source_text)

        parts = namedtuple('parts', ['intval', 'demo', 'num'])

        result_part: parts

        if len(split_result) == 3:
            result_part = parts(
                intval=split_result[0],
                demo=split_result[1],
                num=split_result[2]
            )
        else:
            result_part = parts(
                intval='零',
                demo=split_result[0],
                num=split_result[1]
            )

        int_value = Decimal(self.get_value_from_part(result_part.intval))
        num_value = Decimal(self.get_value_from_part(result_part.num))
        demo_value = Decimal(self.get_value_from_part(result_part.demo))

        if regex.search(self.config.negative_number_sign_regex, result_part.intval) is not None:
            result.value = int_value - num_value / demo_value
        else:
            result.value = int_value + num_value / demo_value

        result.resolution_str = self.__format(result.value)
        return result

    def get_value_from_part(self, part: str) -> float:
        if self.is_digit(part):
            return self.get_digit_value(part, 1.0)
        return self.get_int_value(part)

    def dou_parse(self, source: ExtractResult) -> ParseResult:
        result = ParseResult(source)

        source_text = self.replace_unit(source.text)

        if (regex.search(self.config.double_and_round_regex, source.text)) is not None:
            power = self.config.round_number_map_char[source_text[-1:]]
            result.value = self.get_digit_value(source_text[:-1], power)
        else:
            split_result = regex.split(self.config.point_regex, source_text)
            if split_result[0] == '':
                split_result[0] = '零'
            if regex.search(self.config.negative_number_sign_regex, split_result[0]) is not None:
                result.value = self.get_int_value(split_result[0]) - self.get_point_value(split_result[1])
            else:
                result.value = self.get_int_value(split_result[0]) + self.get_point_value(split_result[1])

        result.resolution_str = self.__format(result.value)
        return result

    def int_parse(self, source: ExtractResult) -> ParseResult:
        result = ParseResult(source)
        result.value = self.get_int_value(source.text)
        result.resolution_str = self.__format(result.value)
        return result

    def ord_parse(self, source: ExtractResult) -> ParseResult:
        result = ParseResult(source)
        source_text = source.text[1:]

        if regex.search(self.config.digit_num_regex, source_text) is not None:
            result.value = self.get_digit_value(source_text, 1)
        else:
            result.value = self.get_int_value(source_text)

        result.resolution_str = self.__format(result.value)
        return result

    def get_digit_value(self, source: str, power: float) -> float:
        negative: bool = False
        result_str = source
        if regex.search(self.config.negative_number_sign_regex, result_str) is not None:
            negative = True
            result_str = result_str[1:]
        result_str = self.replace_full_with_half(result_str)
        result = float(super()._get_digital_value(result_str, power))
        if negative:
            result = - result
        return result

    def get_int_value(self, source: str) -> float:
        result_str = source
        dozen = False
        pair = False

        if regex.search(self.config.dozen_regex, result_str) is not None:
            dozen = True
            if self.config.culture_info.code == Culture.Chinese:
#            if isinstance(self.config, ChineseNumberParserConfiguration):
               result_str = result_str[:-1]
            elif self.config.culture_info.code == Culture.Japanese:
#            if isinstance(self.config, CJKNumberParserConfiguration):
                result_str = result_str[:-3]
        elif regex.search(self.config.pair_regex, result_str) is not None:
            pair = True
            result_str = result_str[:-1]

        result_str = self.replace_unit(result_str)
        int_value = 0
        part_value = 0
        before_value = 0
        is_round_before = False
        round_before = -1
        round_default = 1
        negative = False
        has_number = False

        if regex.search(self.config.negative_number_sign_regex, result_str) is not None:
            negative = True
            result_str = result_str[1:]

        for i in range(len(result_str)):
            c = result_str[i]
            if c in self.config.round_number_map_char:
                round_recent = self.config.round_number_map_char[c]
                if not has_number:
                    before_value = 1
                if round_before != -1 and round_recent > round_before:
                    if is_round_before:
                        int_value += part_value * round_recent
                        is_round_before = False
                    else:
                        part_value += before_value * round_default
                        int_value += part_value * round_recent
                    round_before = -1
                    part_value = 0
                else:
                    is_round_before = True
                    part_value += before_value * round_recent
                    round_before = round_recent
                    if i == len(result_str)-1 or c in self.config.round_direct_list:
                        int_value += part_value
                        part_value = 0

                has_number = False
                before_value = 0
                round_default = round_recent / 10
            elif c in self.config.zero_to_nine_map:
                has_number = True
                if i != len(result_str)-1:
                    if c == '零' and result_str[i+1] not in self.config.round_number_map_char:
                        round_default = 1
                    else:
                        before_value = before_value * 10 + self.config.zero_to_nine_map[c]
                        is_round_before = False
                else:
                    if i == len(result_str)-1 and self.config.culture_info.code == Culture.Japanese:
                        round_default = 1
                    part_value += before_value * 10
                    part_value += self.config.zero_to_nine_map[c] * round_default
                    int_value += part_value
                    part_value = 0
        if negative:
            int_value = - int_value
        if dozen:
            int_value = int_value * 12
        if pair:
            int_value = int_value * 2

        return int_value

    def get_point_value(self, source: str) -> float:
        result: float = 0
        scale: float = 0.1
        for c in source:
            result += scale * self.config.zero_to_nine_map[c]
            scale *= 0.1
        return result

    def is_digit(self, source: str) -> bool:
        return (source is not None
                and len(source.strip()) > 0
                and regex.search(self.config.digit_num_regex, source) is not None)
