#  Copyright (c) Microsoft Corporation. All rights reserved.
#  Licensed under the MIT License.

from typing import Pattern

from recognizers_text.utilities import RegExpUtility
from recognizers_number.number.extractors import BaseNumberExtractor
from recognizers_number.number.french.extractors import FrenchCardinalExtractor
from ...resources.french_date_time import FrenchDateTime
from ..base_duration import DurationExtractorConfiguration


class FrenchDurationExtractorConfiguration(DurationExtractorConfiguration):

    @property
    def check_both_before_after(self):
        return self._check_both_before_after

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
    def more_than_regex(self) -> Pattern:
        return self._more_than_regex

    @property
    def less_than_regex(self) -> Pattern:
        return self._less_than_regex

    @property
    def cardinal_extractor(self) -> BaseNumberExtractor:
        return self._cardinal_extractor

    @property
    def during_regex(self) -> Pattern:
        return self._during_regex

    @property
    def unit_map(self) -> {}:
        return self._unit_map

    @property
    def unit_value_map(self) -> {}:
        return self._unit_value_map

    @property
    def duration_unit_regex(self) -> Pattern:
        return self._duration_unit_regex

    @property
    def duration_connector_regex(self) -> Pattern:
        return self._duration_connector_regex

    @property
    def conjunction_regex(self) -> Pattern:
        return self._conjunction_regex

    @property
    def inexact_number_regex(self):
        return self._inexact_number_regex

    @property
    def special_number_unit_regex(self):
        return self._special_number_unit_regex

    def __init__(self):
        super().__init__()
        self._check_both_before_after = FrenchDateTime.CheckBothBeforeAfter
        self._inexact_number_regex = RegExpUtility.get_safe_reg_exp(FrenchDateTime.InexactNumberRegex)
        self._conjunction_regex = RegExpUtility.get_safe_reg_exp(FrenchDateTime.ConjunctionRegex)
        self._all_regex: Pattern = RegExpUtility.get_safe_reg_exp(
            FrenchDateTime.AllRegex)
        self._half_regex: Pattern = RegExpUtility.get_safe_reg_exp(
            FrenchDateTime.HalfRegex)
        self._followed_unit: Pattern = RegExpUtility.get_safe_reg_exp(
            FrenchDateTime.DurationFollowedUnit)
        self._number_combined_with_unit: Pattern = RegExpUtility.get_safe_reg_exp(
            FrenchDateTime.NumberCombinedWithDurationUnit)
        self._an_unit_regex: Pattern = RegExpUtility.get_safe_reg_exp(
            FrenchDateTime.AnUnitRegex)
        self._inexact_number_unit_regex: Pattern = RegExpUtility.get_safe_reg_exp(
            FrenchDateTime.InexactNumberUnitRegex)
        self._suffix_and_regex: Pattern = RegExpUtility.get_safe_reg_exp(
            FrenchDateTime.SuffixAndRegex)
        self._relative_duration_unit_regex: Pattern = RegExpUtility.get_safe_reg_exp(
            FrenchDateTime.RelativeDurationUnitRegex)
        self._more_than_regex: Pattern = RegExpUtility.get_safe_reg_exp(
            FrenchDateTime.MoreThanRegex)
        self._less_than_regex: Pattern = RegExpUtility.get_safe_reg_exp(
            FrenchDateTime.LessThanOneHour)
        self._cardinal_extractor: BaseNumberExtractor = FrenchCardinalExtractor()
        self._during_regex: Pattern = RegExpUtility.get_safe_reg_exp(
            FrenchDateTime.DuringRegex
        )
        self._unit_map = FrenchDateTime.UnitMap
        self._unit_value_map = FrenchDateTime.UnitValueMap
        self._duration_unit_regex = RegExpUtility.get_safe_reg_exp(
            FrenchDateTime.DurationUnitRegex
        )
        self._duration_connector_regex = RegExpUtility.get_safe_reg_exp(
            FrenchDateTime.DurationConnectorRegex
        )
        self._more_than_regex = RegExpUtility.get_safe_reg_exp(
            FrenchDateTime.MoreThanRegex
        )
        self._less_than_regex = RegExpUtility.get_safe_reg_exp(
            FrenchDateTime.LessThanRegex
        )
        self._check_both_before_after = FrenchDateTime.CheckBothBeforeAfter
        self._special_number_unit_regex = None
