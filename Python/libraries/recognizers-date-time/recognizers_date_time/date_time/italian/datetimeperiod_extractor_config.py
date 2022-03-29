#  Copyright (c) Microsoft Corporation. All rights reserved.
#  Licensed under the MIT License.

from typing import List, Pattern

from recognizers_number import BaseNumberExtractor, ItalianCardinalExtractor
from recognizers_text.utilities import RegExpUtility
from ...resources.italian_date_time import ItalianDateTime
from ..extractors import DateTimeExtractor
from ..base_datetimeperiod import DateTimePeriodExtractorConfiguration, MatchedIndex
from ..base_date import BaseDateExtractor
from ..base_time import BaseTimeExtractor
from ..base_duration import BaseDurationExtractor
from ..base_timeperiod import BaseTimePeriodExtractor
from ..base_datetime import BaseDateTimeExtractor
from ..base_timezone import BaseTimeZoneExtractor
from .date_extractor_config import ItalianDateExtractorConfiguration
from .time_extractor_config import ItalianTimeExtractorConfiguration
from .duration_extractor_config import ItalianDurationExtractorConfiguration
from .timeperiod_extractor_config import ItalianTimePeriodExtractorConfiguration
from .timezone_extractor_config import ItalianTimeZoneExtractorConfiguration
from .datetime_extractor_config import ItalianDateTimeExtractorConfiguration


class ItalianDateTimePeriodExtractorConfiguration(DateTimePeriodExtractorConfiguration):
    @property
    def check_both_before_after(self) -> Pattern:
        return self._check_both_before_after

    @property
    def suffix_regex(self) -> Pattern:
        return self._suffix_regex

    @property
    def cardinal_extractor(self) -> BaseNumberExtractor:
        return self._cardinal_extractor

    @property
    def single_date_extractor(self) -> DateTimeExtractor:
        return self._single_date_extractor

    @property
    def single_time_extractor(self) -> DateTimeExtractor:
        return self._single_time_extractor

    @property
    def single_date_time_extractor(self) -> DateTimeExtractor:
        return self._single_date_time_extractor

    @property
    def duration_extractor(self) -> DateTimeExtractor:
        return self._duration_extractor

    @property
    def time_period_extractor(self) -> DateTimeExtractor:
        return self._time_period_extractor

    @property
    def time_zone_extractor(self) -> DateTimeExtractor:
        return self._time_zone_extractor

    @property
    def simple_cases_regexes(self) -> List[Pattern]:
        return self._simple_cases_regexes

    @property
    def preposition_regex(self) -> Pattern:
        return self._preposition_regex

    @property
    def till_regex(self) -> Pattern:
        return self._till_regex

    @property
    def specific_time_of_day_regex(self) -> Pattern:
        return self._specific_time_of_day_regex

    @property
    def time_of_day_regex(self) -> Pattern:
        return self._time_of_day_regex

    @property
    def period_time_of_day_with_date_regex(self) -> Pattern:
        return self._period_time_of_day_with_date_regex

    @property
    def followed_unit(self) -> Pattern:
        return self._followed_unit

    @property
    def number_combined_with_unit(self) -> Pattern:
        return self._number_combined_with_unit

    @property
    def time_unit_regex(self) -> Pattern:
        return self._time_unit_regex

    @property
    def previous_prefix_regex(self) -> Pattern:
        return self._past_prefix_regex

    @property
    def next_prefix_regex(self) -> Pattern:
        return self._next_prefix_regex

    @property
    def relative_time_unit_regex(self) -> Pattern:
        return self._relative_time_unit_regex

    @property
    def rest_of_date_time_regex(self) -> Pattern:
        return self._rest_of_date_time_regex

    @property
    def week_day_regex(self) -> Pattern:
        return self._week_day_regex

    @property
    def general_ending_regex(self) -> Pattern:
        return self._general_ending_regex

    @property
    def middle_pause_regex(self) -> Pattern:
        return self._middle_pause_regex

    @property
    def within_next_prefix_regex(self) -> Pattern:
        return self._within_next_prefix_regex

    @property
    def token_before_date(self) -> str:
        return self._token_before_date

    @property
    def future_suffix_regex(self) -> Pattern:
        return self._future_suffix_regex

    @property
    def date_unit_regex(self) -> Pattern:
        return self._date_unit_regex

    @property
    def am_desc_regex(self) -> Pattern:
        return self._am_desc_regex

    @property
    def pm_desc_regex(self) -> Pattern:
        return self._pm_desc_regex

    @property
    def prefix_day_regex(self) -> Pattern:
        return self._prefix_day_regex

    @property
    def before_regex(self) -> Pattern:
        return self._before_regex

    @property
    def after_regex(self) -> Pattern:
        return self._after_regex

    def __init__(self):
        super().__init__()
        self._check_both_before_after = ItalianDateTime.CheckBothBeforeAfter
        self._simple_cases_regexes = [
            RegExpUtility.get_safe_reg_exp(ItalianDateTime.PureNumFromTo),
            RegExpUtility.get_safe_reg_exp(ItalianDateTime.PureNumBetweenAnd),
            RegExpUtility.get_safe_reg_exp(
                ItalianDateTime.SpecificTimeOfDayRegex)
        ]

        self._preposition_regex = RegExpUtility.get_safe_reg_exp(
            ItalianDateTime.PrepositionRegex)
        self._till_regex = RegExpUtility.get_safe_reg_exp(
            ItalianDateTime.TillRegex)
        self._specific_time_of_day_regex = RegExpUtility.get_safe_reg_exp(
            ItalianDateTime.PeriodSpecificTimeOfDayRegex)
        self._time_of_day_regex = RegExpUtility.get_safe_reg_exp(
            ItalianDateTime.PeriodTimeOfDayRegex)
        self._followed_unit = RegExpUtility.get_safe_reg_exp(
            ItalianDateTime.TimeFollowedUnit)
        self._time_unit_regex = RegExpUtility.get_safe_reg_exp(
            ItalianDateTime.TimeUnitRegex)
        self._past_prefix_regex = RegExpUtility.get_safe_reg_exp(
            ItalianDateTime.PastSuffixRegex)
        self._next_prefix_regex = RegExpUtility.get_safe_reg_exp(
            ItalianDateTime.NextSuffixRegex)
        self._number_combined_with_unit = RegExpUtility.get_safe_reg_exp(
            ItalianDateTime.TimeNumberCombinedWithUnit)
        self._week_day_regex = RegExpUtility.get_safe_reg_exp(
            ItalianDateTime.WeekDayRegex)
        self._period_time_of_day_with_date_regex = RegExpUtility.get_safe_reg_exp(
            ItalianDateTime.PeriodTimeOfDayWithDateRegex)
        self._relative_time_unit_regex = RegExpUtility.get_safe_reg_exp(
            ItalianDateTime.RelativeTimeUnitRegex)
        self._rest_of_date_time_regex = RegExpUtility.get_safe_reg_exp(
            ItalianDateTime.RestOfDateTimeRegex)
        self._general_ending_regex = RegExpUtility.get_safe_reg_exp(
            ItalianDateTime.GeneralEndingRegex)
        self._middle_pause_regex = RegExpUtility.get_safe_reg_exp(
            ItalianDateTime.MiddlePauseRegex)

        self.from_regex = RegExpUtility.get_safe_reg_exp(
            ItalianDateTime.FromRegex2)
        self.connector_and_regex = RegExpUtility.get_safe_reg_exp(
            ItalianDateTime.ConnectorAndRegex)

        self._cardinal_extractor = ItalianCardinalExtractor()

        self._single_date_extractor = BaseDateExtractor(
            ItalianDateExtractorConfiguration())
        self._single_time_extractor = BaseTimeExtractor(
            ItalianTimeExtractorConfiguration())
        self._single_date_time_extractor = BaseDateTimeExtractor(
            ItalianDateTimeExtractorConfiguration())
        self._duration_extractor = BaseDurationExtractor(
            ItalianDurationExtractorConfiguration())
        self._time_period_extractor = BaseTimePeriodExtractor(
            ItalianTimePeriodExtractorConfiguration())
        self._time_zone_extractor = BaseTimeZoneExtractor(
            ItalianTimeZoneExtractorConfiguration())
        self._within_next_prefix_regex = RegExpUtility.get_safe_reg_exp(
            ItalianDateTime.WithinNextPrefixRegex
        )
        self._time_unit_regex = RegExpUtility.get_safe_reg_exp(
            ItalianDateTime.TimeUnitRegex
        )
        self._token_before_date = ItalianDateTime.TokenBeforeDate
        self._future_suffix_regex = RegExpUtility.get_safe_reg_exp(
            ItalianDateTime.FutureSuffixRegex
        )
        self._date_unit_regex = RegExpUtility.get_safe_reg_exp(
            ItalianDateTime.DateUnitRegex
        )
        self._am_desc_regex = RegExpUtility.get_safe_reg_exp(
            ItalianDateTime.AmDescRegex
        )
        self._pm_desc_regex = RegExpUtility.get_safe_reg_exp(
            ItalianDateTime.PmDescRegex
        )
        self._prefix_day_regex = RegExpUtility.get_safe_reg_exp(
            ItalianDateTime.PrefixDayRegex
        )
        self._before_regex = RegExpUtility.get_safe_reg_exp(
            ItalianDateTime.BeforeRegex
        )
        self._after_regex = RegExpUtility.get_safe_reg_exp(
            ItalianDateTime.AfterRegex
        )
        self._suffix_regex = RegExpUtility.get_safe_reg_exp(
            ItalianDateTime.SuffixRegex
        )
        self._check_both_before_after = ItalianDateTime.CheckBothBeforeAfter

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

    def has_connector_token(self, source: str) -> bool:
        match = self.connector_and_regex.search(source)
        if match:
            return MatchedIndex(True, match.start())

        return MatchedIndex(False, -1)
