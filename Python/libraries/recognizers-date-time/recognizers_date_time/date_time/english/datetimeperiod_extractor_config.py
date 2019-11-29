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
from ..utilities import DateTimeOptions


class EnglishDateTimePeriodExtractorConfiguration(DateTimePeriodExtractorConfiguration):

    @property
    def check_both_before_after(self) -> Pattern:
        return self._check_both_before_after

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
    def general_ending_regex(self) -> Pattern:
        return self._general_ending_regex

    @property
    def middle_pause_regex(self) -> Pattern:
        return self._middle_pause_regex

    @property
    def token_before_date(self) -> str:
        return self._token_before_date

    @property
    def within_next_prefix_regex(self) -> Pattern:
        return self._within_next_prefix_regex

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

    @property
    def suffix_regex(self) -> Pattern:
        return self._suffix_regex

    @property
    def week_day_regex(self) -> Pattern:
        return self._week_day_regex

    def __init__(self):
        super().__init__()
        self._week_day_regex = RegExpUtility.get_safe_reg_exp(
            EnglishDateTime.WeekDayRegex
        )
        self._check_both_before_after = EnglishDateTime.CheckBothBeforeAfter
        self._cardinal_extractor = EnglishCardinalExtractor()
        self._single_date_extractor = BaseDateExtractor(
            EnglishDateExtractorConfiguration())
        self._single_time_extractor = BaseTimeExtractor(
            EnglishTimeExtractorConfiguration())
        self._single_date_time_extractor = BaseDateTimeExtractor(
            EnglishDateTimeExtractorConfiguration())
        self._duration_extractor = BaseDurationExtractor(
            EnglishDurationExtractorConfiguration())
        self._time_period_extractor = BaseTimePeriodExtractor(
            EnglishTimePeriodExtractorConfiguration())
        self._simple_cases_regexes = [
            RegExpUtility.get_safe_reg_exp(EnglishDateTime.PureNumFromTo),
            RegExpUtility.get_safe_reg_exp(EnglishDateTime.PureNumBetweenAnd)
        ]
        self._preposition_regex = RegExpUtility.get_safe_reg_exp(
            EnglishDateTime.PrepositionRegex)
        self._till_regex = RegExpUtility.get_safe_reg_exp(
            EnglishDateTime.TillRegex)
        self._specific_time_of_day_regex = RegExpUtility.get_safe_reg_exp(
            EnglishDateTime.PeriodSpecificTimeOfDayRegex)
        self._time_of_day_regex = RegExpUtility.get_safe_reg_exp(
            EnglishDateTime.PeriodTimeOfDayRegex)
        self._period_time_of_day_with_date_regex = RegExpUtility.get_safe_reg_exp(
            EnglishDateTime.PeriodTimeOfDayWithDateRegex)
        self._followed_unit = RegExpUtility.get_safe_reg_exp(
            EnglishDateTime.TimeFollowedUnit)
        self._number_combined_with_unit = RegExpUtility.get_safe_reg_exp(
            EnglishDateTime.TimeNumberCombinedWithUnit)
        self._time_unit_regex = RegExpUtility.get_safe_reg_exp(
            EnglishDateTime.TimeUnitRegex)
        self._past_prefix_regex = RegExpUtility.get_safe_reg_exp(
            EnglishDateTime.PreviousPrefixRegex)
        self._next_prefix_regex = RegExpUtility.get_safe_reg_exp(
            EnglishDateTime.NextPrefixRegex)
        self._relative_time_unit_regex = RegExpUtility.get_safe_reg_exp(
            EnglishDateTime.RelativeTimeUnitRegex)
        self._rest_of_date_time_regex = RegExpUtility.get_safe_reg_exp(
            EnglishDateTime.RestOfDateTimeRegex)
        self._general_ending_regex = RegExpUtility.get_safe_reg_exp(
            EnglishDateTime.GeneralEndingRegex)
        self._middle_pause_regex = RegExpUtility.get_safe_reg_exp(
            EnglishDateTime.MiddlePauseRegex)
        self.range_connector_regex = RegExpUtility.get_safe_reg_exp(
            EnglishDateTime.RangeConnectorRegex)
        self._token_before_date = EnglishDateTime.TokenBeforeDate
        self._within_next_prefix_regex = RegExpUtility.get_safe_reg_exp(
            EnglishDateTime.WithinNextPrefixRegex
        )
        self._future_suffix_regex = RegExpUtility.get_safe_reg_exp(
            EnglishDateTime.FutureSuffixRegex
        )
        self._date_unit_regex = RegExpUtility.get_safe_reg_exp(
            EnglishDateTime.DateUnitRegex
        )
        self._am_desc_regex = RegExpUtility.get_safe_reg_exp(
            EnglishDateTime.AmDescRegex
        )
        self._pm_desc_regex = RegExpUtility.get_safe_reg_exp(
            EnglishDateTime.PmDescRegex
        )
        self._prefix_day_regex = RegExpUtility.get_safe_reg_exp(
            EnglishDateTime.PrefixDayRegex
        )
        self._before_regex = RegExpUtility.get_safe_reg_exp(
            EnglishDateTime.BeforeRegex
        )
        self._after_regex = RegExpUtility.get_safe_reg_exp(
            EnglishDateTime.AfterRegex
        )
        self._suffix_regex = RegExpUtility.get_safe_reg_exp(
            EnglishDateTime.SuffixRegex
        )
        self._options = DateTimeOptions.NONE
        self._check_both_before_after = EnglishDateTime.CheckBothBeforeAfter

    def get_from_token_index(self, source: str) -> MatchedIndex:
        if source.endswith('from'):
            return MatchedIndex(matched=True, index=source.rfind('from'))

        return MatchedIndex(matched=False, index=-1)

    def get_between_token_index(self, source: str) -> MatchedIndex:
        if source.endswith('between'):
            return MatchedIndex(matched=True, index=source.rfind('between'))

        return MatchedIndex(matched=False, index=-1)

    def has_connector_token(self, source: str) -> bool:
        return regex.fullmatch(self.range_connector_regex, source)
