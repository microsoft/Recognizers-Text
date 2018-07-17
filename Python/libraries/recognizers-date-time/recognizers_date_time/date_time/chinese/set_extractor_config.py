from typing import Pattern

from recognizers_text import RegExpUtility

from ...resources.chinese_date_time import ChineseDateTime
from ..base_set import SetExtractorConfiguration
from .duration_extractor import ChineseDurationExtractor
from .time_extractor import ChineseTimeExtractor
from .date_extractor import ChineseDateExtractor
from .datetime_extractor import ChineseDateTimeExtractor

class ChineseSetExtractorConfiguration(SetExtractorConfiguration):
    @property
    def last_regex(self) -> Pattern:
        return self._last_regex

    @property
    def each_prefix_regex(self) -> Pattern:
        return self._each_prefix_regex

    @property
    def periodic_regex(self) -> any:
        return None

    @property
    def each_unit_regex(self) -> Pattern:
        return self._each_unit_regex

    @property
    def each_day_regex(self) -> Pattern:
        return self._each_day_regex

    @property
    def before_each_day_regex(self) -> Pattern:
        return self._before_each_day_regex

    @property
    def set_week_day_regex(self) -> any:
        return None

    @property
    def set_each_regex(self) -> any:
        return None

    @property
    def duration_extractor(self) -> ChineseDurationExtractor:
        return self._duration_extractor

    @property
    def time_extractor(self) -> ChineseTimeExtractor:
        return self._time_extractor

    @property
    def date_extractor(self) -> ChineseDateExtractor:
        return self._date_extractor

    @property
    def date_time_extractor(self) -> ChineseDateTimeExtractor:
        return self._date_time_extractor

    @property
    def date_period_extractor(self) -> any:
        return None

    @property
    def time_period_extractor(self) -> any:
        return None

    @property
    def date_time_period_extractor(self) -> any:
        return None

    def __init__(self):
        self._last_regex = RegExpUtility.get_safe_reg_exp(ChineseDateTime.SetLastRegex)
        self._each_prefix_regex = RegExpUtility.get_safe_reg_exp(ChineseDateTime.SetEachPrefixRegex)
        self._each_unit_regex = RegExpUtility.get_safe_reg_exp(ChineseDateTime.SetEachUnitRegex)
        self._each_day_regex = RegExpUtility.get_safe_reg_exp(ChineseDateTime.SetEachDayRegex)
        self._before_each_day_regex = RegExpUtility.get_safe_reg_exp(ChineseDateTime.SetEachDayRegex)
        self._duration_extractor = ChineseDurationExtractor()
        self._time_extractor = ChineseTimeExtractor()
        self._date_extractor = ChineseDateExtractor()
        self._date_time_extractor = ChineseDateTimeExtractor()
