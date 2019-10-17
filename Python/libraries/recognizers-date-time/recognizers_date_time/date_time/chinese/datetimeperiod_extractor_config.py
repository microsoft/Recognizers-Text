from typing import Pattern

from recognizers_text import Extractor, RegExpUtility
from recognizers_number import ChineseCardinalExtractor

from ...resources.chinese_date_time import ChineseDateTime
from ..extractors import DateTimeExtractor
from ..base_datetimeperiod import DateTimePeriodExtractorConfiguration, MatchedIndex
from .date_extractor import ChineseDateExtractor
from .time_extractor import ChineseTimeExtractor
from .datetime_extractor import ChineseDateTimeExtractor


class ChineseDateTimePeriodExtractorConfiguration(DateTimePeriodExtractorConfiguration):
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
    def cardinal_extractor(self) -> Extractor:
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
    def followed_unit(self) -> Pattern:
        return self._followed_unit

    @property
    def time_unit_regex(self) -> Pattern:
        return self._time_unit_regex

    @property
    def period_time_of_day_with_date_regex(self) -> Pattern:
        return None

    @property
    def number_combined_with_unit(self) -> Pattern:
        return None

    @property
    def previous_prefix_regex(self) -> Pattern:
        return None

    @property
    def next_prefix_regex(self) -> Pattern:
        return None

    @property
    def range_connector_regex(self) -> Pattern:
        return None

    @property
    def relative_time_unit_regex(self) -> Pattern:
        return None

    @property
    def rest_of_date_time_regex(self) -> Pattern:
        return None

    @property
    def general_ending_regex(self) -> Pattern:
        return None

    @property
    def middle_pause_regex(self) -> Pattern:
        return None

    @property
    def duration_extractor(self) -> DateTimeExtractor:
        return None

    @property
    def time_period_extractor(self) -> DateTimeExtractor:
        return None

    @property
    def simple_cases_regexes(self) -> Pattern:
        return None

    def __init__(self):
        super().__init__()
        self._cardinal_extractor = ChineseCardinalExtractor()
        self._single_date_extractor = ChineseDateExtractor()
        self._single_time_extractor = ChineseTimeExtractor()
        self._single_date_time_extractor = ChineseDateTimeExtractor()
        self._preposition_regex = RegExpUtility.get_safe_reg_exp(
            ChineseDateTime.DateTimePeriodPrepositionRegex)
        self._till_regex = RegExpUtility.get_safe_reg_exp(
            ChineseDateTime.DateTimePeriodTillRegex)
        self._specific_time_of_day_regex = RegExpUtility.get_safe_reg_exp(
            ChineseDateTime.SpecificTimeOfDayRegex)
        self._time_of_day_regex = RegExpUtility.get_safe_reg_exp(
            ChineseDateTime.TimeOfDayRegex)
        self._followed_unit = RegExpUtility.get_safe_reg_exp(
            ChineseDateTime.DateTimePeriodFollowedUnit)
        self._time_unit_regex = RegExpUtility.get_safe_reg_exp(
            ChineseDateTime.DateTimePeriodUnitRegex)
        # TODO When the implementation for these properties is added, change the None values to their respective Regexps
        self._suffix_regex = None
        self._after_regex = None
        self._before_regex = None
        self._prefix_day_regex = None
        self._pm_desc_regex = None
        self._am_desc_regex = None
        self._date_unit_regex = None
        self._future_suffix_regex = None
        self._within_next_prefix_regex = None
        self._token_before_date = None

    def get_from_token_index(self, source: str) -> MatchedIndex:
        if source.endswith('从'):
            return MatchedIndex(True, source.rindex('从'))
        return MatchedIndex(False, -1)

    def get_between_token_index(self, source: str) -> MatchedIndex:
        return MatchedIndex(False, -1)

    def has_connector_token(self, source: str) -> bool:
        return source in ['和', '与', '到']
