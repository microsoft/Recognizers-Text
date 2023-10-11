from typing import List, Pattern

from recognizers_text.extractor import Extractor
from recognizers_text.utilities import RegExpUtility
from recognizers_number import CatalanIntegerExtractor
from ...resources.catalan_date_time import CatalanDateTime
from ..extractors import DateTimeExtractor
from ..base_minimal_merged import MinimalMergedExtractorConfiguration
from ..base_date import BaseDateExtractor
from ..base_time import BaseTimeExtractor
from .date_extractor_config import CatalanDateExtractorConfiguration
from .time_extractor_config import CatalanTimeExtractorConfiguration
from ...resources.base_date_time import BaseDateTime


class CatalanMergedExtractorConfiguration(MinimalMergedExtractorConfiguration):
    @property
    def check_both_before_after(self):
        return self._check_both_before_after

    @property
    def date_extractor(self) -> DateTimeExtractor:
        return self._date_extractor

    @property
    def time_extractor(self) -> DateTimeExtractor:
        return self._time_extractor

    @property
    def integer_extractor(self) -> Extractor:
        return self._integer_extractor

    @property
    def equal_regex(self) -> Pattern:
        return self._equal_regex

    @property
    def ambiguous_range_modifier_prefix(self) -> None:
        return None

    @property
    def potential_ambiguous_range_regex(self) -> None:
        return None

    @property
    def number_ending_pattern(self) -> Pattern:
        return self._number_ending_pattern

    @property
    def filter_word_regex_list(self) -> List[Pattern]:
        return self._filter_word_regex_list

    def __init__(self):
        self._ambiguous_range_modifier_prefix = RegExpUtility.get_safe_reg_exp(
            CatalanDateTime.AmbiguousRangeModifierPrefix)
        self._number_ending_pattern = RegExpUtility.get_safe_reg_exp(
            CatalanDateTime.NumberEndingPattern)

        self._date_extractor = BaseDateExtractor(
            CatalanDateExtractorConfiguration())
        self._time_extractor = BaseTimeExtractor(CatalanTimeExtractorConfiguration())
        self._integer_extractor = CatalanIntegerExtractor()
        self._filter_word_regex_list = []
        self._equal_regex = BaseDateTime.EqualRegex
        self._check_both_before_after = CatalanDateTime.CheckBothBeforeAfter