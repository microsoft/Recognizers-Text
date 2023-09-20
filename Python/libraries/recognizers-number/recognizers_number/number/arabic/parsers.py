from re import Pattern
from typing import List, Dict, Optional, Any


from recognizers_text.utilities import RegExpUtility
from recognizers_text.culture import Culture
from recognizers_number.culture import CultureInfo
from recognizers_number.number.parsers import BaseNumberParserConfiguration
from recognizers_number.resources.arabic_numeric import ArabicNumeric


class ArabicNumberParserConfiguration(BaseNumberParserConfiguration):
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

    def __init__(self, culture_info: Optional[CultureInfo]=None):
        if culture_info is None:
            culture_info = CultureInfo(Culture.Arabic)

        self._culture_info = culture_info
        self._lang_marker = ArabicNumeric.LangMarker
        self._is_compound_number_language = ArabicNumeric.CompoundNumberLanguage
        self._is_multi_decimal_separator_culture = ArabicNumeric.MultiDecimalSeparatorCulture

        self._decimal_separator_char = ArabicNumeric.DecimalSeparatorChar
        self._fraction_marker_token = ArabicNumeric.FractionMarkerToken
        self._non_decimal_separator_char = ArabicNumeric.NonDecimalSeparatorChar
        self._half_a_dozen_text = ArabicNumeric.HalfADozenText
        self._word_separator_token = ArabicNumeric.WordSeparatorToken
        self._non_standard_separator_variants = []

        self._written_decimal_separator_texts = ArabicNumeric.WrittenDecimalSeparatorTexts
        self._written_group_separator_texts = ArabicNumeric.WrittenGroupSeparatorTexts
        self._written_integer_separator_texts = ArabicNumeric.WrittenIntegerSeparatorTexts
        self._written_fraction_separator_texts = ArabicNumeric.WrittenFractionSeparatorTexts

        self._cardinal_number_map = ArabicNumeric.CardinalNumberMap
        self._ordinal_number_map = ArabicNumeric.OrdinalNumberMap
        self._relative_reference_offset_map = ArabicNumeric.RelativeReferenceOffsetMap
        self._relative_reference_relative_to_map = ArabicNumeric.RelativeReferenceRelativeToMap
        self._round_number_map = ArabicNumeric.RoundNumberMap

        self._half_a_dozen_regex = RegExpUtility.get_safe_reg_exp(ArabicNumeric.HalfADozenRegex)
        self._digital_number_regex = RegExpUtility.get_safe_reg_exp(ArabicNumeric.DigitalNumberRegex)
        self._negative_number_sign_regex = RegExpUtility.get_safe_reg_exp(ArabicNumeric.NegativeNumberSignRegex)
        self._fraction_preposition_regex = RegExpUtility.get_safe_reg_exp(ArabicNumeric.FractionPrepositionRegex)

        self.non_decimal_separator_text = ''

    @staticmethod
    def get_lang_specific_int_value(match_strs: List[str]) -> (bool, int):
        result = (False, 0)

        # @TODO "و" should be moved to Arabic YAML file.

        #    Workaround to solve "و" which means "and" before rounded number in Arabic.
        #    ألف و مائة = one thousand and one hundred #
        #    But in Arabic there is no integer before hundred, because it's 100 by default.
        if len(match_strs) == 1 and match_strs[0] == "و":
            result = (True, 1)

        return result
