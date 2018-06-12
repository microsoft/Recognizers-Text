from typing import List, Dict, Pattern, Optional
from collections import namedtuple
from decimal import Decimal, getcontext
import copy
import regex

from recognizers_text.utilities import RegExpUtility
from recognizers_text.culture import Culture
from recognizers_text.extractor import ExtractResult
from recognizers_text.parser import ParseResult
from recognizers_number.resources.chinese_numeric import ChineseNumeric
from recognizers_number.number.parsers import BaseNumberParser, NumberParserConfiguration
from recognizers_number.culture import CultureInfo

getcontext().prec = 15

class ChineseNumberParserConfiguration(NumberParserConfiguration):
    @property
    def cardinal_number_map(self) -> Dict[str, int]:
        return dict()

    @property
    def ordinal_number_map(self) -> Dict[str, int]:
        return dict()

    @property
    def round_number_map(self) -> Dict[str, int]:
        return self._round_number_map

    @property
    def culture_info(self) -> CultureInfo:
        return self._culture_info

    @property
    def digital_number_regex(self) -> Pattern:
        return self._digital_number_regex

    @property
    def fraction_marker_token(self) -> str:
        return self._fraction_marker_token

    @property
    def negative_number_sign_regex(self) -> Pattern:
        return self._negative_number_sign_regex

    @property
    def half_a_dozen_regex(self) -> Pattern:
        return None

    @property
    def half_a_dozen_text(self) -> str:
        return self._half_a_dozen_text

    @property
    def lang_marker(self) -> str:
        return self._lang_marker

    @property
    def non_decimal_separator_char(self) -> str:
        return self._non_decimal_separator_char

    @property
    def decimal_separator_char(self) -> str:
        return self._decimal_separator_char

    @property
    def word_separator_token(self) -> str:
        return self._word_separator_token

    @property
    def written_decimal_separator_texts(self) -> List[str]:
        return list()

    @property
    def written_group_separator_texts(self) -> List[str]:
        return list()

    @property
    def written_integer_separator_texts(self) -> List[str]:
        return list()

    @property
    def written_fraction_separator_texts(self) -> List[str]:
        return list()

    def __init__(self, culture_info=None):
        if culture_info is None:
            culture_info = CultureInfo(Culture.Chinese)

        self._culture_info = culture_info
        self._lang_marker = ChineseNumeric.LangMarker
        self._decimal_separator_char = ChineseNumeric.DecimalSeparatorChar
        self._fraction_marker_token = ChineseNumeric.FractionMarkerToken
        self._non_decimal_separator_char = ChineseNumeric.NonDecimalSeparatorChar
        self._half_a_dozen_text = ChineseNumeric.HalfADozenText
        self._word_separator_token = ChineseNumeric.WordSeparatorToken

        self._round_number_map = ChineseNumeric.RoundNumberMap
        self._digital_number_regex = RegExpUtility.get_safe_reg_exp(ChineseNumeric.DigitalNumberRegex)

        self.zero_to_nine_map_chs = ChineseNumeric.ZeroToNineMap
        self.round_number_map_chs = ChineseNumeric.RoundNumberMapChar
        self.full_to_half_map_chs = ChineseNumeric.FullToHalfMap
        self.trato_sim_map_chs = ChineseNumeric.TratoSimMap
        self.unit_map_chs = ChineseNumeric.UnitMap
        self.round_direct_list_chs = ChineseNumeric.RoundDirectList

        self.digit_num_regex = ChineseNumeric.DigitNumRegex
        self.dozen_regex = ChineseNumeric.DozenRegex
        self.percentage_regex = ChineseNumeric.PercentageRegex
        self.double_and_round_chs_regex = RegExpUtility.get_safe_reg_exp(ChineseNumeric.DoubleAndRoundRegex)
        self.frac_split_regex = RegExpUtility.get_safe_reg_exp(ChineseNumeric.FracSplitRegex)
        self._negative_number_sign_regex = RegExpUtility.get_safe_reg_exp(ChineseNumeric.NegativeNumberSignRegex)
        self.point_regex_chs = ChineseNumeric.PointRegex
        self.spe_get_number_regex = RegExpUtility.get_safe_reg_exp(ChineseNumeric.SpeGetNumberRegex)
        self.pair_regex = RegExpUtility.get_safe_reg_exp(ChineseNumeric.PairRegex)

    def normalize_token_set(self, tokens: List[str], context: ParseResult) -> List[str]:
        return tokens

    def resolve_composite_number(self, number_str: str) -> int:
        return 0

class ChineseNumberParser(BaseNumberParser):
    def __init__(self, config: ChineseNumberParserConfiguration):
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
            result = self.per_parse_chs(simplified_source)
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
            result = self.frac_parse_chs(simplified_source)
        elif 'Dou' in extra:
            result = self.dou_parse_chs(simplified_source)
        elif 'Integer' in extra:
            result = self.int_parse_chs(simplified_source)
        elif 'Ordinal' in extra:
            result = self.ord_parse_chs(simplified_source)

        if result is not None:
            result.text = source.text

        return result

    def replace_trad_with_simplified(self, source: str) -> str:
        if source is None or not source.strip():
            return source
        return ''.join(map(lambda c: self.config.trato_sim_map_chs.get(c, c), source))

    def replace_full_with_half(self, source: str) -> str:
        if source is None or not source.strip():
            return source
        return ''.join(map(lambda c: self.config.full_to_half_map_chs.get(c, c), source))

    def replace_unit(self, source: str) -> str:
        if source is None or not source.strip():
            return source
        for (k, v) in self.config.unit_map_chs.items():
            source = source.replace(k, v)
        return source

    def per_parse_chs(self, source: ExtractResult) -> ParseResult:
        result = ParseResult(source)
        source_text = source.text
        power = 1

        if 'Spe' in source.data:
            source_text = self.replace_full_with_half(source_text)
            source_text = self.replace_unit(source_text)

            if source_text == '半折':
                result.value = 50
            elif source_text == '10成':
                result.value = 100
            else:
                matches = list(regex.finditer(self.config.spe_get_number_regex, source_text))
                int_number: int
                if len(matches) == 2:
                    int_number_char = matches[0].group()[0]
                    if int_number_char == '对':
                        int_number = 5
                    elif int_number_char == '十' or int_number_char == '拾':
                        int_number = 10
                    else:
                        int_number = self.config.zero_to_nine_map_chs[int_number_char]

                    point_number_char = matches[1].group()[0]
                    point_number: float
                    if point_number_char == '半':
                        point_number = 0.5
                    else:
                        point_number = self.config.zero_to_nine_map_chs[point_number_char] * 0.1

                    result.value = (int_number + point_number) * 10
                else:
                    int_number_char = matches[0].group()[0]
                    if int_number_char == '对':
                        int_number = 5
                    elif int_number_char == '十' or int_number_char == '拾':
                        int_number = 10
                    else:
                        int_number = self.config.zero_to_nine_map_chs[int_number_char]
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
            result.value = self.get_digit_value_chs(double_text, power)

        else:
            double_match = regex.search(self.config.percentage_regex, source_text)
            double_text = self.replace_unit(double_match.group())

            split_result = regex.split(self.config.point_regex_chs, double_text)
            if split_result[0] == '':
                split_result[0] = '零'

            double_value = self.get_int_value_chs(split_result[0])
            if len(split_result) == 2:
                if regex.search(self.config.negative_number_sign_regex, split_result[0]) is not None:
                    double_value -= self.get_point_value_chs(split_result[1])
                else:
                    double_value += self.get_point_value_chs(split_result[1])
            result.value = double_value

        result.resolution_str = self.__format(result.value) + '%'
        return result

    def frac_parse_chs(self, source: ExtractResult) -> ParseResult:
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
        if self.is_digit_chs(part):
            return self.get_digit_value_chs(part, 1.0)
        return self.get_int_value_chs(part)

    def dou_parse_chs(self, source: ExtractResult) -> ParseResult:
        result = ParseResult(source)

        source_text = self.replace_unit(source.text)

        if (regex.search(self.config.double_and_round_chs_regex, source.text)) is not None:
            power = self.config.round_number_map_chs[source_text[-1:]]
            result.value = self.get_digit_value_chs(source_text[:-1], power)
        else:
            split_result = regex.split(self.config.point_regex_chs, source_text)
            if split_result[0] == '':
                split_result[0] = '零'
            if regex.search(self.config.negative_number_sign_regex, split_result[0]) is not None:
                result.value = self.get_int_value_chs(split_result[0]) - self.get_point_value_chs(split_result[1])
            else:
                result.value = self.get_int_value_chs(split_result[0]) + self.get_point_value_chs(split_result[1])

        result.resolution_str = self.__format(result.value)
        return result

    def int_parse_chs(self, source: ExtractResult) -> ParseResult:
        result = ParseResult(source)
        result.value = self.get_int_value_chs(source.text)
        result.resolution_str = self.__format(result.value)
        return result

    def ord_parse_chs(self, source: ExtractResult) -> ParseResult:
        result = ParseResult(source)
        source_text = source.text[1:]

        if regex.search(self.config.digit_num_regex, source_text) is not None:
            result.value = self.get_digit_value_chs(source_text, 1)
        else:
            result.value = self.get_int_value_chs(source_text)

        result.resolution_str = self.__format(result.value)
        return result

    def get_digit_value_chs(self, source: str, power: float) -> float:
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

    def get_int_value_chs(self, source: str) -> float:
        result_str = source
        dozen = False
        pair = False

        if regex.search(self.config.dozen_regex, result_str) is not None:
            dozen = True
            result_str = result_str[:-1]
        elif regex.search(self.config.pair_regex, result_str) is not None:
            pair = True
            result_str = result_str[:-1]

        result_str = self.replace_unit(result_str)
        int_value = 0
        part_value = 0
        before_value = 1
        is_round_before = False
        round_before = -1
        round_default = 1
        negative = False

        if regex.search(self.config.negative_number_sign_regex, result_str) is not None:
            negative = True
            result_str = result_str[1:]

        for i in range(len(result_str)):
            c = result_str[i]
            if c in self.config.round_number_map_chs:
                round_recent = self.config.round_number_map_chs[c]
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
                    if i == len(result_str)-1 or c in self.config.round_direct_list_chs:
                        int_value += part_value
                        part_value = 0
                round_default = round_recent / 10
            elif c in self.config.zero_to_nine_map_chs:
                if i != len(result_str)-1:
                    if c == '零' and result_str[i+1] not in self.config.round_number_map_chs:
                        before_value = 1
                        round_default = 1
                    else:
                        before_value = self.config.zero_to_nine_map_chs[c]
                        is_round_before = False
                else:
                    part_value += self.config.zero_to_nine_map_chs[c] * round_default
                    int_value += part_value
                    part_value = 0
        if negative:
            int_value = - int_value
        if dozen:
            int_value = int_value * 12
        if pair:
            int_value = int_value * 2

        return int_value

    def get_point_value_chs(self, source: str) -> float:
        result: float = 0
        scale: float = 0.1
        for c in source:
            result += scale * self.config.zero_to_nine_map_chs[c]
            scale *= 0.1
        return result

    def is_digit_chs(self, source: str) -> bool:
        return (source is not None
                and len(source.strip()) > 0
                and regex.search(self.config.digit_num_regex, source) is not None)
