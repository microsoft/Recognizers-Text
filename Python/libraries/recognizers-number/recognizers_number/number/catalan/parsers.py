from typing import Dict, Pattern, List
import regex

from recognizers_text.utilities import RegExpUtility
from recognizers_text.culture import Culture
from recognizers_text.parser import ParseResult
from recognizers_number.culture import CultureInfo
from recognizers_number.number.parsers import BaseNumberParserConfiguration
from recognizers_number.resources.catalan_numeric import CatalanNumeric


class CatalanNumberParserConfiguration(BaseNumberParserConfiguration):
    @property
    def cardinal_number_map(self) -> Dict[str, int]:
        return self._cardinal_number_map

    @property
    def ordinal_number_map(self) -> Dict[str, int]:
        return self._ordinal_number_map

    @property
    def round_number_map(self) -> Dict[str, int]:
        return self._round_number_map

    @property
    def culture_info(self):
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
        return self._half_a_dozen_regex

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
        return self._written_decimal_separator_texts

    @property
    def written_group_separator_texts(self) -> List[str]:
        return self._written_group_separator_texts

    @property
    def written_integer_separator_texts(self) -> List[str]:
        return self._written_integer_separator_texts

    @property
    def written_fraction_separator_texts(self) -> List[str]:
        return self._written_fraction_separator_texts

    @property
    def non_standard_separator_variants(self) -> List[str]:
        return self._non_standard_separator_variants

    @property
    def is_multi_decimal_separator_culture(self) -> bool:
        return self._is_multi_decimal_separator_culture

    @property
    def round_multiplier_regex(self) -> Pattern:
        return self._round_multiplier_regex

    def __init__(self, culture_info=None):
        if culture_info is None:
            culture_info = CultureInfo(Culture.Catalan)

        self._culture_info = culture_info
        self._lang_marker = CatalanNumeric.LangMarker
        self._decimal_separator_char = CatalanNumeric.DecimalSeparatorChar
        self._fraction_marker_token = None
        self._non_decimal_separator_char = CatalanNumeric.NonDecimalSeparatorChar
        self._half_a_dozen_text = CatalanNumeric.HalfADozenText
        self._word_separator_token = CatalanNumeric.WordSeparatorToken

        self._written_decimal_separator_texts = CatalanNumeric.WrittenDecimalSeparatorTexts
        self._written_group_separator_texts = CatalanNumeric.WrittenGroupSeparatorTexts
        self._written_integer_separator_texts = CatalanNumeric.WrittenIntegerSeparatorTexts
        self._written_fraction_separator_texts = None
        self._non_standard_separator_variants = CatalanNumeric.NonStandardSeparatorVariants
        self._is_multi_decimal_separator_culture = CatalanNumeric.MultiDecimalSeparatorCulture

        ordinal_number_map: Dict[str, int] = dict(
            CatalanNumeric.OrdinalNumberMap)
        self._cardinal_number_map = CatalanNumeric.CardinalNumberMap
        self._ordinal_number_map = ordinal_number_map
        self._round_number_map = CatalanNumeric.RoundNumberMap
        self._negative_number_sign_regex = RegExpUtility.get_safe_reg_exp(
            CatalanNumeric.NegativeNumberSignRegex)
        self._half_a_dozen_regex = RegExpUtility.get_safe_reg_exp(
            CatalanNumeric.HalfADozenRegex)
        self._digital_number_regex = RegExpUtility.get_safe_reg_exp(
            CatalanNumeric.DigitalNumberRegex)
        self._round_multiplier_regex = None
