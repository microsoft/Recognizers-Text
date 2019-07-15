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
from recognizers_number.number.cjk_parsers import CJKNumberParser
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
    def zero_char(self) -> str:
        return self._zero_char

    @property
    def pair_char(self) -> str:
        return self._pair_char

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

    @property
    def zero_to_nine_map(self) -> Dict[str, int]:
        return self._zero_to_nine_map

    @property
    def round_number_map_char(self) -> Dict[str, int]:
        return self._round_number_map_char

    @property
    def full_to_half_map(self) -> Dict[str, int]:
        return self._full_to_half_map

    @property
    def trato_sim_map(self) -> Dict[str, int]:
        return self._trato_sim_map

    @property
    def unit_map(self) -> Dict[str, int]:
        return self._unit_map

    @property
    def round_direct_list(self) -> List[str]:
        return self._round_direct_list

    @property
    def ten_chars(self) -> List[str]:
        return self._ten_chars

    @property
    def digit_num_regex(self) -> Pattern:
        return self._digit_num_regex

    @property
    def dozen_regex(self) -> Pattern:
        return self._dozen_regex

    @property
    def percentage_regex(self) -> Pattern:
        return self._percentage_regex

    @property
    def double_and_round_regex(self) -> Pattern:
        return self._double_and_round_regex

    @property
    def frac_split_regex(self) -> Pattern:
        return self._frac_split_regex

    @property
    def point_regex(self) -> Pattern:
        return self._point_regex

    @property
    def spe_get_number_regex(self) -> Pattern:
        return self._spe_get_number_regex

    @property
    def pair_regex(self) -> Pattern:
        return self._pair_regex

    @property
    def round_number_integer_regex(self) -> Pattern:
        return self._round_number_integer_regex

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
        self._zero_char = ChineseNumeric.ZeroChar
        self._pair_char = ChineseNumeric.PairChar

        self._round_number_map = ChineseNumeric.RoundNumberMap
        self._digital_number_regex = RegExpUtility.get_safe_reg_exp(
            ChineseNumeric.DigitalNumberRegex)

        self._zero_to_nine_map = ChineseNumeric.ZeroToNineMap
        self._round_number_map_char = ChineseNumeric.RoundNumberMapChar
        self._full_to_half_map = ChineseNumeric.FullToHalfMap
        self._trato_sim_map = ChineseNumeric.TratoSimMap
        self._unit_map = ChineseNumeric.UnitMap
        self._round_direct_list = ChineseNumeric.RoundDirectList
        self._ten_chars = ChineseNumeric.TenChars

        self._digit_num_regex = ChineseNumeric.DigitNumRegex
        self._dozen_regex = ChineseNumeric.DozenRegex
        self._percentage_regex = ChineseNumeric.PercentageRegex
        self._double_and_round_regex = RegExpUtility.get_safe_reg_exp(
            ChineseNumeric.DoubleAndRoundRegex)
        self._frac_split_regex = RegExpUtility.get_safe_reg_exp(
            ChineseNumeric.FracSplitRegex)
        self._negative_number_sign_regex = RegExpUtility.get_safe_reg_exp(
            ChineseNumeric.NegativeNumberSignRegex)
        self._point_regex = ChineseNumeric.PointRegex
        self._spe_get_number_regex = RegExpUtility.get_safe_reg_exp(
            ChineseNumeric.SpeGetNumberRegex)
        self._pair_regex = RegExpUtility.get_safe_reg_exp(
            ChineseNumeric.PairRegex)

    def normalize_token_set(self, tokens: List[str], context: ParseResult) -> List[str]:
        return tokens

    def resolve_composite_number(self, number_str: str) -> int:
        return 0
