from typing import Pattern
import regex

from recognizers_text import RegExpUtility

from ...resources.chinese_date_time import ChineseDateTime
from ..extractors import DateTimeExtractor
from ..base_datetime import DateTimeExtractorConfiguration
from .date_extractor import ChineseDateExtractor
from .time_extractor import ChineseTimeExtractor


class ChineseDateTimeExtractorConfiguration(DateTimeExtractorConfiguration):
    @property
    def number_as_time_regex(self) -> Pattern:
        return self._number_as_time_regex

    @property
    def date_number_connector_regex(self) -> Pattern:
        return self._date_number_connector_regex

    @property
    def suffix_after_regex(self) -> Pattern:
        return self._suffix_after_regex

    @property
    def year_suffix(self) -> Pattern:
        return self._year_suffix

    @property
    def year_regex(self) -> Pattern:
        return self._year_regex

    @property
    def date_point_extractor(self) -> DateTimeExtractor:
        return self._date_point_extractor

    @property
    def time_point_extractor(self) -> DateTimeExtractor:
        return self._time_point_extractor

    @property
    def night_regex(self) -> Pattern:
        return self._night_regex

    @property
    def time_of_today_before_regex(self) -> Pattern:
        return self._time_of_today_before_regex

    @property
    def preposition_regex(self) -> Pattern:
        return self._preposition_regex

    @property
    def now_regex(self) -> Pattern:
        return self._now_regex

    @property
    def duration_extractor(self) -> DateTimeExtractor:
        return None

    @property
    def suffix_regex(self) -> Pattern:
        return None

    @property
    def time_of_today_after_regex(self) -> Pattern:
        return None

    @property
    def simple_time_of_today_after_regex(self) -> Pattern:
        return None

    @property
    def simple_time_of_today_before_regex(self) -> Pattern:
        return None

    @property
    def specific_end_of_regex(self) -> Pattern:
        return None

    @property
    def unspecific_end_of_regex(self) -> Pattern:
        return None

    @property
    def unit_regex(self) -> Pattern:
        return None

    @property
    def utility_configuration(self) -> any:
        return None

    def __init__(self):
        super().__init__()
        self._date_point_extractor = ChineseDateExtractor()
        self._time_point_extractor = ChineseTimeExtractor()
        self._now_regex = RegExpUtility.get_safe_reg_exp(
            ChineseDateTime.NowRegex)
        self._night_regex = RegExpUtility.get_safe_reg_exp(
            ChineseDateTime.NightRegex)
        self._time_of_today_before_regex = RegExpUtility.get_safe_reg_exp(
            ChineseDateTime.TimeOfTodayRegex)
        self._preposition_regex = RegExpUtility.get_safe_reg_exp(
            ChineseDateTime.PrepositionRegex)
        # TODO When the implementation for these properties is added, change the None values to their respective Regexps
        self._year_regex = None
        self._year_suffix = None
        self._suffix_after_regex = None
        self._date_number_connector_regex = None
        self._number_as_time_regex = None

    def is_connector_token(self, source: str) -> bool:
        return not source.strip() or source == ',' or regex.search(self.preposition_regex, source)
