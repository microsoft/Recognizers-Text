#  Copyright (c) Microsoft Corporation. All rights reserved.
#  Licensed under the MIT License.

from typing import Pattern

from recognizers_text.utilities import RegExpUtility
from recognizers_number.number.extractors import BaseNumberExtractor
from recognizers_number.number.dutch.extractors import DutchCardinalExtractor
from ...resources.dutch_date_time import DutchDateTime
from ..base_duration import DurationExtractorConfiguration


class DutchDurationExtractorConfiguration(DurationExtractorConfiguration):

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
    def mod_prefix_regex(self) -> Pattern:
        return self._mod_prefix_regex

    @property
    def mod_suffix_regex(self) -> Pattern:
        return self._mod_suffix_regex

    @property
    def inexact_number_regex(self):
        return self._inexact_number_regex

    @property
    def special_number_unit_regex(self):
        return self._special_number_unit_regex

    def __init__(self):
        super().__init__()
        self._check_both_before_after = DutchDateTime.CheckBothBeforeAfter
        self._inexact_number_regex = RegExpUtility.get_safe_reg_exp(DutchDateTime.InexactNumberRegex)
        self._conjunction_regex = RegExpUtility.get_safe_reg_exp(DutchDateTime.ConjunctionRegex)
        self._all_regex: Pattern = RegExpUtility.get_safe_reg_exp(
            DutchDateTime.AllRegex)
        self._half_regex: Pattern = RegExpUtility.get_safe_reg_exp(
            DutchDateTime.HalfRegex)
        self._followed_unit: Pattern = RegExpUtility.get_safe_reg_exp(
            DutchDateTime.DurationFollowedUnit)
        self._number_combined_with_unit: Pattern = RegExpUtility.get_safe_reg_exp(
            DutchDateTime.NumberCombinedWithDurationUnit)
        self._an_unit_regex: Pattern = RegExpUtility.get_safe_reg_exp(
            DutchDateTime.AnUnitRegex)
        self._inexact_number_unit_regex: Pattern = RegExpUtility.get_safe_reg_exp(
            DutchDateTime.InexactNumberUnitRegex)
        self._suffix_and_regex: Pattern = RegExpUtility.get_safe_reg_exp(
            DutchDateTime.SuffixAndRegex)
        self._relative_duration_unit_regex: Pattern = RegExpUtility.get_safe_reg_exp(
            DutchDateTime.RelativeDurationUnitRegex)
        self._more_than_regex: Pattern = RegExpUtility.get_safe_reg_exp(
            DutchDateTime.MoreThanRegex)
        self._less_than_regex: Pattern = RegExpUtility.get_safe_reg_exp(
            DutchDateTime.LessThanOneHour)
        self._cardinal_extractor: BaseNumberExtractor = DutchCardinalExtractor()
        self._during_regex: Pattern = RegExpUtility.get_safe_reg_exp(
            DutchDateTime.DuringRegex
        )
        self._unit_map = DutchDateTime.UnitMap
        self._unit_value_map = DutchDateTime.UnitValueMap
        self._duration_unit_regex = RegExpUtility.get_safe_reg_exp(
            DutchDateTime.DurationUnitRegex
        )
        self._duration_connector_regex = RegExpUtility.get_safe_reg_exp(
            DutchDateTime.DurationConnectorRegex
        )
        self._more_than_regex = RegExpUtility.get_safe_reg_exp(
            DutchDateTime.MoreThanRegex
        )
        self._less_than_regex = RegExpUtility.get_safe_reg_exp(
            DutchDateTime.LessThanRegex
        )
        self._mod_prefix_regex = RegExpUtility.get_safe_reg_exp(
            DutchDateTime.ModPrefixRegex
        )
        self._mod_suffix_regex = RegExpUtility.get_safe_reg_exp(
            DutchDateTime.ModSuffixRegex
        )
        self._check_both_before_after = DutchDateTime.CheckBothBeforeAfter
        self._special_number_unit_regex = None
