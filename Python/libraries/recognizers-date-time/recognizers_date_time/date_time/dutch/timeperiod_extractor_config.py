#  Copyright (c) Microsoft Corporation. All rights reserved.
#  Licensed under the MIT License.

from typing import List, Pattern

from recognizers_text.utilities import RegExpUtility
from recognizers_text.extractor import Extractor
from recognizers_number.number.dutch.extractors import DutchIntegerExtractor
from ...resources.dutch_date_time import DutchDateTime
from ..extractors import DateTimeExtractor
from ..base_timeperiod import TimePeriodExtractorConfiguration, MatchedIndex
from ..base_time import BaseTimeExtractor
from ..base_timezone import BaseTimeZoneExtractor
from .time_extractor_config import DutchTimeExtractorConfiguration
from .base_configs import DutchDateTimeUtilityConfiguration
from .timezone_extractor_config import DutchTimeZoneExtractorConfiguration


class DutchTimePeriodExtractorConfiguration(TimePeriodExtractorConfiguration):
    @property
    def check_both_before_after(self) -> bool:
        return self._check_both_before_after

    @property
    def simple_cases_regex(self) -> List[Pattern]:
        return self._simple_cases_regex

    @property
    def till_regex(self) -> Pattern:
        return self._till_regex

    @property
    def time_of_day_regex(self) -> Pattern:
        return self._time_of_day_regex

    @property
    def general_ending_regex(self) -> Pattern:
        return self._general_ending_regex

    @property
    def single_time_extractor(self) -> DateTimeExtractor:
        return self._single_time_extractor

    @property
    def integer_extractor(self) -> Extractor:
        return self._integer_extractor

    @property
    def token_before_date(self) -> str:
        return self._token_before_date

    @property
    def pure_number_regex(self) -> List[Pattern]:
        return self._pure_number_regex

    @property
    def time_zone_extractor(self) -> DateTimeExtractor:
        return self._time_zone_extractor

    @property
    def hour_regex(self) -> Pattern:
        return self._hour_regex

    @property
    def period_hour_num_regex(self) -> Pattern:
        return self._period_hour_num_regex

    @property
    def period_desc_regex(self) -> Pattern:
        return self._period_desc_regex

    @property
    def time_date_from_to_regex(self):
        return self._time_date_from_to_regex

    @property
    def specific_time_from_to(self):
        return self._specific_time_from_to_regex

    @property
    def specific_time_between_and(self):
        return self._specific_time_between_and_regex

    @property
    def preposition_regex(self):
        return self._preposition_regex

    @property
    def specific_time_of_day_regex(self):
        return self._specific_time_of_day_regex

    @property
    def time_unit_regex(self):
        return self._time_unit_regex

    def __init__(self):
        super().__init__()
        self._check_both_before_after = DutchDateTime.CheckBothBeforeAfter
        self._single_time_extractor = BaseTimeExtractor(
            DutchTimeExtractorConfiguration())
        self._integer_extractor = DutchIntegerExtractor()
        self.utility_configuration = DutchDateTimeUtilityConfiguration()

        self._simple_cases_regex: List[Pattern] = [
            RegExpUtility.get_safe_reg_exp(DutchDateTime.PureNumFromTo),
            RegExpUtility.get_safe_reg_exp(DutchDateTime.PureNumBetweenAnd),
            RegExpUtility.get_safe_reg_exp(DutchDateTime.PmRegex),
            RegExpUtility.get_safe_reg_exp(DutchDateTime.AmRegex)
        ]

        self._till_regex: Pattern = RegExpUtility.get_safe_reg_exp(
            DutchDateTime.TillRegex)
        self._time_of_day_regex: Pattern = RegExpUtility.get_safe_reg_exp(
            DutchDateTime.TimeOfDayRegex)
        self._general_ending_regex: Pattern = RegExpUtility.get_safe_reg_exp(
            DutchDateTime.GeneralEndingRegex)
        self._hour_regex: Pattern = RegExpUtility.get_safe_reg_exp(DutchDateTime.HourRegex)
        self._period_desc_regex: Pattern = RegExpUtility.get_safe_reg_exp(
            DutchDateTime.DescRegex)
        self._period_hour_num_regex: Pattern = RegExpUtility.get_safe_reg_exp(
            DutchDateTime.PeriodHourNumRegex)
        self._specific_time_of_day_regex: Pattern = RegExpUtility.get_safe_reg_exp(
            DutchDateTime.SpecificTimeOfDayRegex)
        self.time_followed_unit: Pattern = RegExpUtility.get_safe_reg_exp(
            DutchDateTime.TimeFollowedUnit)
        self.time_number_combined_with_unit: Pattern = RegExpUtility.get_safe_reg_exp(
            DutchDateTime.TimeNumberCombinedWithUnit)

        self.from_regex = RegExpUtility.get_safe_reg_exp(
            DutchDateTime.FromRegex)
        self.between_regex = RegExpUtility.get_safe_reg_exp(
            DutchDateTime.BetweenRegex)
        self._time_unit_regex = RegExpUtility.get_safe_reg_exp(
            DutchDateTime.TimeUnitRegex)
        self.range_connector_regex = RegExpUtility.get_safe_reg_exp(
            DutchDateTime.RangeConnectorRegex)
        self.before_regex = RegExpUtility.get_safe_reg_exp(
            DutchDateTime.BeforeRegex)
        self._time_date_from_to_regex = RegExpUtility.get_safe_reg_exp(
            DutchDateTime.TimeDateFromTo)
        self._specific_time_from_to_regex = RegExpUtility.get_safe_reg_exp(
            DutchDateTime.SpecificTimeFromTo)
        self._specific_time_between_and_regex = RegExpUtility.get_safe_reg_exp(
            DutchDateTime.SpecificTimeBetweenAnd)
        self._preposition_regex = RegExpUtility.get_safe_reg_exp(
            DutchDateTime.PrepositionRegex)
        self._token_before_date = DutchDateTime.TokenBeforeDate
        self._pure_number_regex = [DutchDateTime.PureNumFromTo, DutchDateTime.PureNumFromTo]
        self._time_zone_extractor = BaseTimeZoneExtractor(
            DutchTimeZoneExtractorConfiguration())

    def get_from_token_index(self, source: str) -> MatchedIndex:
        match = self.from_regex.search(source)
        if match:
            return MatchedIndex(True, match.start())

        return MatchedIndex(False, -1)

    def get_between_token_index(self, source: str) -> MatchedIndex:
        match = self.before_regex.search(source)
        if match:
            return MatchedIndex(True, match.start())

        return MatchedIndex(False, -1)

    def is_connector_token(self, source: str):
        return self.range_connector_regex.match(source)

