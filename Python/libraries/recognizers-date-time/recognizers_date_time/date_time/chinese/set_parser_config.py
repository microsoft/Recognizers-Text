from typing import Pattern, Dict

from recognizers_text.utilities import RegExpUtility
from ...resources.chinese_date_time import ChineseDateTime
from ..extractors import DateTimeExtractor
from ..parsers import DateTimeParser
from ..base_set import SetParserConfiguration, MatchedTimex
from .date_extractor import ChineseDateExtractor
from .time_extractor import ChineseTimeExtractor
from .duration_extractor import ChineseDurationExtractor
from .datetime_extractor import ChineseDateTimeExtractor
from .date_parser import ChineseDateParser
from .time_parser import ChineseTimeParser
from .duration_parser import ChineseDurationParser
from .datetime_parser import ChineseDateTimeParser

class ChineseSetParserConfiguration(SetParserConfiguration):
    @property
    def duration_extractor(self) -> DateTimeExtractor:
        return self._duration_extractor

    @property
    def duration_parser(self) -> DateTimeParser:
        return self._duration_parser

    @property
    def time_extractor(self) -> DateTimeExtractor:
        return self._time_extractor

    @property
    def time_parser(self) -> DateTimeParser:
        return self._time_parser

    @property
    def date_extractor(self) -> DateTimeExtractor:
        return self._date_extractor

    @property
    def date_parser(self) -> DateTimeParser:
        return self._date_parser

    @property
    def date_time_extractor(self) -> DateTimeExtractor:
        return self._date_time_extractor

    @property
    def date_time_parser(self) -> DateTimeParser:
        return self._date_time_parser

    @property
    def date_period_extractor(self) -> DateTimeExtractor:
        raise NotImplementedError()

    @property
    def date_period_parser(self) -> DateTimeParser:
        raise NotImplementedError()

    @property
    def time_period_extractor(self) -> DateTimeExtractor:
        raise NotImplementedError()

    @property
    def time_period_parser(self) -> DateTimeParser:
        raise NotImplementedError()

    @property
    def date_time_period_extractor(self) -> DateTimeExtractor:
        raise NotImplementedError()

    @property
    def date_time_period_parser(self) -> DateTimeParser:
        raise NotImplementedError()

    @property
    def unit_map(self) -> Dict[str, str]:
        return self._unit_map

    @property
    def each_prefix_regex(self) -> Pattern:
        return self._each_prefix_regex

    @property
    def periodic_regex(self) -> Pattern:
        raise NotImplementedError()

    @property
    def each_unit_regex(self) -> Pattern:
        return self._each_unit_regex

    @property
    def each_day_regex(self) -> Pattern:
        return self._each_day_regex

    @property
    def set_week_day_regex(self) -> Pattern:
        raise NotImplementedError()

    @property
    def set_each_regex(self) -> Pattern:
        raise NotImplementedError()

    def __init__(self):
        self._date_extractor = ChineseDateExtractor()
        self._time_extractor = ChineseTimeExtractor()
        self._duration_extractor = ChineseDurationExtractor()
        self._date_time_extractor = ChineseDateTimeExtractor()
        self._date_parser = ChineseDateParser()
        self._time_parser = ChineseTimeParser()
        self._duration_parser = ChineseDurationParser()
        self._date_time_parser = ChineseDateTimeParser()
        self._unit_map = ChineseDateTime.ParserConfigurationUnitMap
        self._each_unit_regex = RegExpUtility.get_safe_reg_exp(ChineseDateTime.SetEachUnitRegex)
        self._each_day_regex = RegExpUtility.get_safe_reg_exp(ChineseDateTime.SetEachDayRegex)
        self._each_prefix_regex = RegExpUtility.get_safe_reg_exp(ChineseDateTime.SetEachPrefixRegex)

    def get_matched_daily_timex(self, text: str) -> MatchedTimex:
        return None

    def get_matched_unit_timex(self, text: str) -> MatchedTimex:
        timex = ''

        if text == '天' or text == '日':
            timex = 'P1D'
        elif text == '周' or text == '星期':
            timex = 'P1W'
        elif text == '月':
            timex = 'P1M'
        elif text == '年':
            timex = 'P1Y'

        return MatchedTimex(not timex == '', timex)
