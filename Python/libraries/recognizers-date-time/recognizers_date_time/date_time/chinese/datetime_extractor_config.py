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
    def date_point_extractor(self) -> DateTimeExtractor:
        return self._date_point_extractor

    @property
    def time_point_extractor(self) -> DateTimeExtractor:
        return self._time_point_extractor

    @property
    def duration_extractor(self) -> DateTimeExtractor:
        return None

    @property
    def suffix_regex(self) -> Pattern:
        return None

    @property
    def now_regex(self) -> Pattern:
        return self._now_regex

    @property
    def time_of_today_after_regex(self) -> Pattern:
        return None

    @property
    def simple_time_of_today_after_regex(self) -> Pattern:
        return None

    @property
    def night_regex(self) -> Pattern:
        return self._night_regex

    @property
    def time_of_today_before_regex(self) -> Pattern:
        return self._time_of_today_before_regex

    @property
    def simple_time_of_today_before_regex(self) -> Pattern:
        return None

    @property
    def the_end_of_regex(self) -> Pattern:
        return None

    @property
    def unit_regex(self) -> Pattern:
        return None

    @property
    def preposition_regex(self) -> Pattern:
        return self._preposition_regex

    @property
    def utility_configuration(self) -> any:
        return None

    def __init__(self):
        self._date_point_extractor = ChineseDateExtractor()
        self._time_point_extractor = ChineseTimeExtractor()
        self._now_regex = RegExpUtility.get_safe_reg_exp(ChineseDateTime.NowRegex)
        self._night_regex = RegExpUtility.get_safe_reg_exp(ChineseDateTime.NightRegex)
        self._time_of_today_before_regex = RegExpUtility.get_safe_reg_exp(ChineseDateTime.TimeOfTodayRegex)
        self._preposition_regex = RegExpUtility.get_safe_reg_exp(ChineseDateTime.PrepositionRegex)

    def is_connector_token(self, source: str) -> bool:
        return not source.strip() or source == ',' or regex.search(self.preposition_regex, source)
