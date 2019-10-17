from typing import List, Pattern

from recognizers_number import BaseNumberExtractor, FrenchCardinalExtractor
from recognizers_text.utilities import RegExpUtility
from ...resources.french_date_time import FrenchDateTime
from ..extractors import DateTimeExtractor
from ..base_datetimeperiod import DateTimePeriodExtractorConfiguration, MatchedIndex
from ..base_date import BaseDateExtractor
from ..base_time import BaseTimeExtractor
from ..base_duration import BaseDurationExtractor
from ..base_timeperiod import BaseTimePeriodExtractor
from ..base_datetime import BaseDateTimeExtractor
from .date_extractor_config import FrenchDateExtractorConfiguration
from .time_extractor_config import FrenchTimeExtractorConfiguration
from .duration_extractor_config import FrenchDurationExtractorConfiguration
from .timeperiod_extractor_config import FrenchTimePeriodExtractorConfiguration
from .datetime_extractor_config import FrenchDateTimeExtractorConfiguration


class FrenchDateTimePeriodExtractorConfiguration(DateTimePeriodExtractorConfiguration):

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
        self._simple_cases_regexes = [
            RegExpUtility.get_safe_reg_exp(FrenchDateTime.PureNumFromTo),
            RegExpUtility.get_safe_reg_exp(FrenchDateTime.PureNumBetweenAnd),
            RegExpUtility.get_safe_reg_exp(
                FrenchDateTime.SpecificTimeOfDayRegex)
        ]

        self._preposition_regex = RegExpUtility.get_safe_reg_exp(
            FrenchDateTime.PrepositionRegex)
        self._till_regex = RegExpUtility.get_safe_reg_exp(
            FrenchDateTime.TillRegex)
        self._specific_time_of_day_regex = RegExpUtility.get_safe_reg_exp(
            FrenchDateTime.PeriodSpecificTimeOfDayRegex)
        self._time_of_day_regex = RegExpUtility.get_safe_reg_exp(
            FrenchDateTime.PeriodTimeOfDayRegex)
        self._followed_unit = RegExpUtility.get_safe_reg_exp(
            FrenchDateTime.TimeFollowedUnit)
        self._time_unit_regex = RegExpUtility.get_safe_reg_exp(
            FrenchDateTime.TimeUnitRegex)
        self._past_prefix_regex = RegExpUtility.get_safe_reg_exp(
            FrenchDateTime.PastSuffixRegex)
        self._next_prefix_regex = RegExpUtility.get_safe_reg_exp(
            FrenchDateTime.NextSuffixRegex)
        self._number_combined_with_unit = RegExpUtility.get_safe_reg_exp(
            FrenchDateTime.TimeNumberCombinedWithUnit)
        self._week_day_regex = RegExpUtility.get_safe_reg_exp(
            FrenchDateTime.WeekDayRegex)
        self._period_time_of_day_with_date_regex = RegExpUtility.get_safe_reg_exp(
            FrenchDateTime.PeriodTimeOfDayWithDateRegex)
        self._relative_time_unit_regex = RegExpUtility.get_safe_reg_exp(
            FrenchDateTime.RelativeTimeUnitRegex)
        self._rest_of_date_time_regex = RegExpUtility.get_safe_reg_exp(
            FrenchDateTime.RestOfDateTimeRegex)
        self._general_ending_regex = RegExpUtility.get_safe_reg_exp(
            FrenchDateTime.GeneralEndingRegex)
        self._middle_pause_regex = RegExpUtility.get_safe_reg_exp(
            FrenchDateTime.MiddlePauseRegex)

        self.from_regex = RegExpUtility.get_safe_reg_exp(
            FrenchDateTime.FromRegex2)
        self.connector_and_regex = RegExpUtility.get_safe_reg_exp(
            FrenchDateTime.ConnectorAndRegex)

        self._cardinal_extractor = FrenchCardinalExtractor()

        self._single_date_extractor = BaseDateExtractor(
            FrenchDateExtractorConfiguration())
        self._single_time_extractor = BaseTimeExtractor(
            FrenchTimeExtractorConfiguration())
        self._single_date_time_extractor = BaseDateTimeExtractor(
            FrenchDateTimeExtractorConfiguration())
        self._duration_extractor = BaseDurationExtractor(
            FrenchDurationExtractorConfiguration())
        self._time_period_extractor = BaseTimePeriodExtractor(
            FrenchTimePeriodExtractorConfiguration())
        self._within_next_prefix_regex = RegExpUtility.get_safe_reg_exp(
            FrenchDateTime.WithinNextPrefixRegex
        )
        self._time_unit_regex = RegExpUtility.get_safe_reg_exp(
            FrenchDateTime.TimeUnitRegex
        )
        self._token_before_date = FrenchDateTime.TokenBeforeDate
        self._future_suffix_regex = RegExpUtility.get_safe_reg_exp(
            FrenchDateTime.FutureSuffixRegex
        )
        self._date_unit_regex = RegExpUtility.get_safe_reg_exp(
            FrenchDateTime.DateUnitRegex
        )
        self._am_desc_regex = RegExpUtility.get_safe_reg_exp(
            FrenchDateTime.AmDescRegex
        )
        self._pm_desc_regex = RegExpUtility.get_safe_reg_exp(
            FrenchDateTime.PmDescRegex
        )
        self._prefix_day_regex = RegExpUtility.get_safe_reg_exp(
            FrenchDateTime.PrefixDayRegex
        )
        self._before_regex = RegExpUtility.get_safe_reg_exp(
            FrenchDateTime.BeforeRegex
        )
        self._after_regex = RegExpUtility.get_safe_reg_exp(
            FrenchDateTime.AfterRegex
        )
        self._suffix_regex = RegExpUtility.get_safe_reg_exp(
            FrenchDateTime.SuffixRegex
        )

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
