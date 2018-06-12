from typing import Pattern

from recognizers_text.utilities import RegExpUtility
from recognizers_number.number.extractors import BaseNumberExtractor
from recognizers_number.number.english.extractors import EnglishCardinalExtractor
from ...resources.english_date_time import EnglishDateTime
from ..base_duration import DurationExtractorConfiguration

class EnglishDurationExtractorConfiguration(DurationExtractorConfiguration):
    @property
    def all_regex(self) -> Pattern:
        return self._all_regex

    @property
    def half_regex(self) -> Pattern:
        return self._half_regex

    @property
    def followed_unit(self) -> Pattern:
        return self._followed_unit

    @property
    def number_combined_with_unit(self) -> Pattern:
        return self._number_combined_with_unit

    @property
    def an_unit_regex(self) -> Pattern:
        return self._an_unit_regex

    @property
    def in_exact_number_unit_regex(self) -> Pattern:
        return self._in_exact_number_unit_regex

    @property
    def suffix_and_regex(self) -> Pattern:
        return self._suffix_and_regex

    @property
    def relative_duration_unit_regex(self) -> Pattern:
        return self._relative_duration_unit_regex

    @property
    def cardinal_extractor(self) -> BaseNumberExtractor:
        return self._cardinal_extractor

    def __init__(self):
        self._all_regex: Pattern = RegExpUtility.get_safe_reg_exp(EnglishDateTime.AllRegex)
        self._half_regex: Pattern = RegExpUtility.get_safe_reg_exp(EnglishDateTime.HalfRegex)
        self._followed_unit: Pattern = RegExpUtility.get_safe_reg_exp(EnglishDateTime.DurationFollowedUnit)
        self._number_combined_with_unit: Pattern = RegExpUtility.get_safe_reg_exp(EnglishDateTime.NumberCombinedWithDurationUnit)
        self._an_unit_regex: Pattern = RegExpUtility.get_safe_reg_exp(EnglishDateTime.AnUnitRegex)
        self._in_exact_number_unit_regex: Pattern = RegExpUtility.get_safe_reg_exp(EnglishDateTime.InexactNumberUnitRegex)
        self._suffix_and_regex: Pattern = RegExpUtility.get_safe_reg_exp(EnglishDateTime.SuffixAndRegex)
        self._relative_duration_unit_regex: Pattern = RegExpUtility.get_safe_reg_exp(EnglishDateTime.RelativeDurationUnitRegex)
        self._cardinal_extractor: BaseNumberExtractor = EnglishCardinalExtractor()
