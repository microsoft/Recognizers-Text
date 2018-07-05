from typing import List, Pattern

from recognizers_number import BaseNumberExtractor, SpanishCardinalExtractor
from recognizers_text.utilities import RegExpUtility
from ...resources.spanish_date_time import SpanishDateTime
from ..extractors import DateTimeExtractor
from ..base_datetimeperiod import DateTimePeriodExtractorConfiguration, MatchedIndex
from ..base_date import BaseDateExtractor
from ..base_time import BaseTimeExtractor
from ..base_duration import BaseDurationExtractor
from ..base_timeperiod import BaseTimePeriodExtractor
from ..base_datetime import BaseDateTimeExtractor
from .date_extractor_config import SpanishDateExtractorConfiguration
from .time_extractor_config import SpanishTimeExtractorConfiguration
from .duration_extractor_config import SpanishDurationExtractorConfiguration
from .timeperiod_extractor_config import SpanishTimePeriodExtractorConfiguration
from .datetime_extractor_config import SpanishDateTimeExtractorConfiguration

class SpanishDateTimePeriodExtractorConfiguration(DateTimePeriodExtractorConfiguration):
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
    def past_prefix_regex(self) -> Pattern:
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

    def __init__(self):
        self._simple_cases_regexes = [
            RegExpUtility.get_safe_reg_exp(SpanishDateTime.PureNumFromTo),
            RegExpUtility.get_safe_reg_exp(SpanishDateTime.PureNumBetweenAnd)
        ]

        self._preposition_regex = RegExpUtility.get_safe_reg_exp(SpanishDateTime.PrepositionRegex)
        self._till_regex = RegExpUtility.get_safe_reg_exp(SpanishDateTime.TillRegex)
        self._specific_time_of_day_regex = RegExpUtility.get_safe_reg_exp(SpanishDateTime.SpecificTimeOfDayRegex)
        self._time_of_day_regex = RegExpUtility.get_safe_reg_exp(SpanishDateTime.TimeOfDayRegex)
        self._followed_unit = RegExpUtility.get_safe_reg_exp(SpanishDateTime.FollowedUnit)
        self._time_unit_regex = RegExpUtility.get_safe_reg_exp(SpanishDateTime.UnitRegex)
        self._past_prefix_regex = RegExpUtility.get_safe_reg_exp(SpanishDateTime.PastRegex)
        self._next_prefix_regex = RegExpUtility.get_safe_reg_exp(SpanishDateTime.FutureRegex)
        self._number_combined_with_unit = RegExpUtility.get_safe_reg_exp(SpanishDateTime.DateTimePeriodNumberCombinedWithUnit)
        self._week_day_regex = RegExpUtility.get_safe_reg_exp(SpanishDateTime.WeekDayRegex)
        self._period_time_of_day_with_date_regex = RegExpUtility.get_safe_reg_exp(SpanishDateTime.PeriodTimeOfDayWithDateRegex)
        self._relative_time_unit_regex = RegExpUtility.get_safe_reg_exp(SpanishDateTime.RelativeTimeUnitRegex)
        self._rest_of_date_time_regex = RegExpUtility.get_safe_reg_exp(SpanishDateTime.RestOfDateTimeRegex)
        self._general_ending_regex = RegExpUtility.get_safe_reg_exp(SpanishDateTime.GeneralEndingRegex)
        self._middle_pause_regex = RegExpUtility.get_safe_reg_exp(SpanishDateTime.MiddlePauseRegex)

        self.from_regex = RegExpUtility.get_safe_reg_exp(SpanishDateTime.FromRegex)
        self.connector_and_regex = RegExpUtility.get_safe_reg_exp(SpanishDateTime.ConnectorAndRegex)
        self.between_regex = RegExpUtility.get_safe_reg_exp(SpanishDateTime.BetweenRegex)

        self._cardinal_extractor = SpanishCardinalExtractor()

        self._single_date_extractor = BaseDateExtractor(SpanishDateExtractorConfiguration())
        self._single_time_extractor = BaseTimeExtractor(SpanishTimeExtractorConfiguration())
        self._single_date_time_extractor = BaseDateTimeExtractor(SpanishDateTimeExtractorConfiguration())
        self._duration_extractor = BaseDurationExtractor(SpanishDurationExtractorConfiguration())
        self._time_period_extractor = BaseTimePeriodExtractor(SpanishTimePeriodExtractorConfiguration())

    def get_from_token_index(self, source: str) -> MatchedIndex:
        match = self.from_regex.search(source)
        if match:
            return MatchedIndex(True, match.start())

        return MatchedIndex(False, -1)

    def get_between_token_index(self, source: str) -> MatchedIndex:
        match = self.between_regex.search(source)
        if match:
            return MatchedIndex(True, match.start())

        return MatchedIndex(False, -1)

    def has_connector_token(self, source: str) -> bool:
        match = self.connector_and_regex.search(source)
        if match:
            return MatchedIndex(True, match.start())

        return MatchedIndex(False, -1)
