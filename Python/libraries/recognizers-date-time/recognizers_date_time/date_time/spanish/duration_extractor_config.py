from typing import Pattern

from recognizers_text.utilities import RegExpUtility
from recognizers_number.number.extractors import BaseNumberExtractor
from recognizers_number.number.spanish.extractors import SpanishCardinalExtractor
from ...resources.spanish_date_time import SpanishDateTime
from ..base_duration import DurationExtractorConfiguration


class SpanishDurationExtractorConfiguration(DurationExtractorConfiguration):

    @property
    def dmy_date_format(self) -> bool:
        return self._dmy_date_format

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
    def inexact_number_unit_regex(self) -> Pattern:
        return self._inexact_number_unit_regex

    @property
    def suffix_and_regex(self) -> Pattern:
        return self._suffix_and_regex

    @property
    def relative_duration_unit_regex(self) -> Pattern:
        return self._relative_duration_unit_regex

    @property
    def more_than_regex(self) -> BaseNumberExtractor:
        return self._more_than_regex

    @property
    def less_than_regex(self) -> BaseNumberExtractor:
        return self._less_than_regex

    @property
    def cardinal_extractor(self) -> BaseNumberExtractor:
        return self._cardinal_extractor

    @property
    def during_regex(self) -> Pattern:
        return self._during_regex

    @property
    def unit_map(self) -> Pattern:
        return self._unit_map

    @property
    def unit_value_map(self) -> {}:
        return self._unit_value_map

    @property
    def duration_unit_regex(self) -> {}:
        return self._duration_unit_regex

    @property
    def duration_connector_regex(self) -> Pattern:
        return self._duration_connector_regex

    @property
    def more_than_regex(self) -> Pattern:
        return self._more_than_regex

    @property
    def less_than_regex(self) -> Pattern:
        return self._less_than_regex

    def __init__(self):
        super().__init__()
        self._all_regex: Pattern = RegExpUtility.get_safe_reg_exp(
            SpanishDateTime.AllRegex)
        self._half_regex: Pattern = RegExpUtility.get_safe_reg_exp(
            SpanishDateTime.HalfRegex)
        self._followed_unit: Pattern = RegExpUtility.get_safe_reg_exp(
            SpanishDateTime.FollowedUnit)
        self._number_combined_with_unit: Pattern = RegExpUtility.get_safe_reg_exp(
            SpanishDateTime.DurationNumberCombinedWithUnit)
        self._an_unit_regex: Pattern = RegExpUtility.get_safe_reg_exp(
            SpanishDateTime.AnUnitRegex)
        self._inexact_number_unit_regex: Pattern = RegExpUtility.get_safe_reg_exp(
            SpanishDateTime.InexactNumberUnitRegex)
        self._suffix_and_regex: Pattern = RegExpUtility.get_safe_reg_exp(
            SpanishDateTime.SuffixAndRegex)
        self._relative_duration_unit_regex: Pattern = RegExpUtility.get_safe_reg_exp(
            SpanishDateTime.RelativeDurationUnitRegex)
        self._more_than_regex: Pattern = RegExpUtility.get_safe_reg_exp(
            SpanishDateTime.MoreThanRegex)
        self._less_than_regex: Pattern = RegExpUtility.get_safe_reg_exp(
            SpanishDateTime.LessThanOneHour)
        self._cardinal_extractor: BaseNumberExtractor = SpanishCardinalExtractor()
        self._during_regex: Pattern = RegExpUtility.get_safe_reg_exp(
            SpanishDateTime.DuringRegex
        )
        self._unit_map = SpanishDateTime.UnitMap
        self._unit_value_map = SpanishDateTime.UnitValueMap
        self._duration_unit_regex = RegExpUtility.get_safe_reg_exp(
            SpanishDateTime.DurationUnitRegex
        )
        self._duration_connector_regex = RegExpUtility.get_safe_reg_exp(
            SpanishDateTime.DurationConnectorRegex
        )
        self._more_than_regex = RegExpUtility.get_safe_reg_exp(
            SpanishDateTime.MoreThanRegex
        )
        self._less_than_regex = RegExpUtility.get_safe_reg_exp(
            SpanishDateTime.LessThanRegex
        )
