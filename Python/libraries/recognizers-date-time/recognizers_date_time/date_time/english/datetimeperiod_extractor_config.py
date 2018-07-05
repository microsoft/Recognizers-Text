from typing import List, Pattern
import regex

from recognizers_number import BaseNumberExtractor, EnglishCardinalExtractor
from recognizers_text.utilities import RegExpUtility
from ...resources.english_date_time import EnglishDateTime
from ..extractors import DateTimeExtractor
from ..base_datetimeperiod import DateTimePeriodExtractorConfiguration, MatchedIndex
from ..base_date import BaseDateExtractor
from ..base_time import BaseTimeExtractor
from ..base_duration import BaseDurationExtractor
from ..base_timeperiod import BaseTimePeriodExtractor
from ..base_datetime import BaseDateTimeExtractor
from .date_extractor_config import EnglishDateExtractorConfiguration
from .time_extractor_config import EnglishTimeExtractorConfiguration
from .duration_extractor_config import EnglishDurationExtractorConfiguration
from .timeperiod_extractor_config import EnglishTimePeriodExtractorConfiguration
from .datetime_extractor_config import EnglishDateTimeExtractorConfiguration

class EnglishDateTimePeriodExtractorConfiguration(DateTimePeriodExtractorConfiguration):
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
    def general_ending_regex(self) -> Pattern:
        return self._general_ending_regex

    @property
    def middle_pause_regex(self) -> Pattern:
        return self._middle_pause_regex

    def __init__(self):
        self._cardinal_extractor = EnglishCardinalExtractor()
        self._single_date_extractor = BaseDateExtractor(EnglishDateExtractorConfiguration())
        self._single_time_extractor = BaseTimeExtractor(EnglishTimeExtractorConfiguration())
        self._single_date_time_extractor = BaseDateTimeExtractor(EnglishDateTimeExtractorConfiguration())
        self._duration_extractor = BaseDurationExtractor(EnglishDurationExtractorConfiguration())
        self._time_period_extractor = BaseTimePeriodExtractor(EnglishTimePeriodExtractorConfiguration())
        self._simple_cases_regexes = [
            RegExpUtility.get_safe_reg_exp(EnglishDateTime.PureNumFromTo),
            RegExpUtility.get_safe_reg_exp(EnglishDateTime.PureNumBetweenAnd)
        ]
        self._preposition_regex = RegExpUtility.get_safe_reg_exp(EnglishDateTime.PrepositionRegex)
        self._till_regex = RegExpUtility.get_safe_reg_exp(EnglishDateTime.TillRegex)
        self._specific_time_of_day_regex = RegExpUtility.get_safe_reg_exp(EnglishDateTime.PeriodSpecificTimeOfDayRegex)
        self._time_of_day_regex = RegExpUtility.get_safe_reg_exp(EnglishDateTime.PeriodTimeOfDayRegex)
        self._period_time_of_day_with_date_regex = RegExpUtility.get_safe_reg_exp(EnglishDateTime.PeriodTimeOfDayWithDateRegex)
        self._followed_unit = RegExpUtility.get_safe_reg_exp(EnglishDateTime.TimeFollowedUnit)
        self._number_combined_with_unit = RegExpUtility.get_safe_reg_exp(EnglishDateTime.TimeNumberCombinedWithUnit)
        self._time_unit_regex = RegExpUtility.get_safe_reg_exp(EnglishDateTime.TimeUnitRegex)
        self._past_prefix_regex = RegExpUtility.get_safe_reg_exp(EnglishDateTime.PastPrefixRegex)
        self._next_prefix_regex = RegExpUtility.get_safe_reg_exp(EnglishDateTime.NextPrefixRegex)
        self._relative_time_unit_regex = RegExpUtility.get_safe_reg_exp(EnglishDateTime.RelativeTimeUnitRegex)
        self._rest_of_date_time_regex = RegExpUtility.get_safe_reg_exp(EnglishDateTime.RestOfDateTimeRegex)
        self._general_ending_regex = RegExpUtility.get_safe_reg_exp(EnglishDateTime.GeneralEndingRegex)
        self._middle_pause_regex = RegExpUtility.get_safe_reg_exp(EnglishDateTime.MiddlePauseRegex)
        self.range_connector_regex = RegExpUtility.get_safe_reg_exp(EnglishDateTime.RangeConnectorRegex)

    def get_from_token_index(self, source: str) -> MatchedIndex:
        if source.endswith('from'):
            return MatchedIndex(matched=True, index=source.rfind('from'))

        return MatchedIndex(matched=False, index=-1)

    def get_between_token_index(self, source: str) -> MatchedIndex:
        if source.endswith('between'):
            return MatchedIndex(matched=True, index=source.rfind('between'))

        return MatchedIndex(matched=False, index=-1)

    def has_connector_token(self, source: str) -> bool:
        return regex.search(self.range_connector_regex, source)
