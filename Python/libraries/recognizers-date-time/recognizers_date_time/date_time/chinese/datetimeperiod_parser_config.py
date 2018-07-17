from typing import Dict, Pattern

from recognizers_text import RegExpUtility

from ...resources.chinese_date_time import ChineseDateTime
from ..extractors import DateTimeExtractor
from ..parsers import DateTimeParser
from ..base_datetimeperiod import DateTimePeriodParserConfiguration, MatchedTimeRange
from .date_extractor import ChineseDateExtractor
from .time_extractor import ChineseTimeExtractor
from .timeperiod_extractor import ChineseTimePeriodExtractor
from .datetime_extractor import ChineseDateTimeExtractor
from .date_parser import ChineseDateParser
from .time_parser import ChineseTimeParser
from .timeperiod_parser import ChineseTimePeriodParser
from .datetime_parser import ChineseDateTimeParser

class ChineseDateTimePeriodParserConfiguration(DateTimePeriodParserConfiguration):
    @property
    def pure_number_from_to_regex(self) -> any:
        return None

    @property
    def pure_number_between_and_regex(self) -> any:
        return None

    @property
    def period_time_of_day_with_date_regex(self) -> any:
        return None

    @property
    def specific_time_of_day_regex(self) -> Pattern:
        return self._specific_time_of_day_regex

    @property
    def past_regex(self) -> Pattern:
        return self._past_regex

    @property
    def future_regex(self) -> Pattern:
        return self._future_regex

    @property
    def relative_time_unit_regex(self) -> Pattern:
        return self._relative_time_unit_regex

    @property
    def rest_of_date_time_regex(self) -> any:
        return None

    @property
    def numbers(self) -> any:
        return None

    @property
    def unit_map(self) -> Dict[str, str]:
        return self._unit_map

    @property
    def date_extractor(self) -> DateTimeExtractor:
        return self._date_extractor

    @property
    def time_extractor(self) -> DateTimeExtractor:
        return self._time_extractor

    @property
    def date_time_extractor(self) -> DateTimeExtractor:
        return self._date_time_extractor

    @property
    def time_period_extractor(self) -> DateTimeExtractor:
        return self._time_period_extractor

    @property
    def duration_extractor(self) -> any:
        return None

    @property
    def date_parser(self) -> DateTimeParser:
        return self._date_parser

    @property
    def time_parser(self) -> DateTimeParser:
        return self._time_parser

    @property
    def date_time_parser(self) -> DateTimeParser:
        return self._date_time_parser

    @property
    def time_period_parser(self) -> DateTimeParser:
        return self._time_period_parser

    @property
    def duration_parser(self) -> any:
        return None

    def __init__(self):
        self._specific_time_of_day_regex = RegExpUtility.get_safe_reg_exp(ChineseDateTime.SpecificTimeOfDayRegex)
        self._past_regex = RegExpUtility.get_safe_reg_exp(ChineseDateTime.PastRegex)
        self._future_regex = RegExpUtility.get_safe_reg_exp(ChineseDateTime.FutureRegex)
        self._relative_time_unit_regex = RegExpUtility.get_safe_reg_exp(ChineseDateTime.TimeOfDayRegex)
        self._unit_map = ChineseDateTime.ParserConfigurationUnitMap
        self._date_extractor = ChineseDateExtractor()
        self._time_extractor = ChineseTimeExtractor()
        self._date_time_extractor = ChineseDateTimeExtractor()
        self._time_period_extractor = ChineseTimePeriodExtractor()
        self._date_parser = ChineseDateParser()
        self._time_parser = ChineseTimeParser()
        self._date_time_parser = ChineseDateTimeParser()
        self._time_period_parser = ChineseTimePeriodParser()

    def get_matched_time_range(self, source: str) -> MatchedTimeRange:
        source = source.strip().lower()
        if source in ['今晚']:
            return MatchedTimeRange('TEV', 16, 20, 0, True, 0)
        elif source in ['今早', '今晨']:
            return MatchedTimeRange('TMO', 8, 12, 0, True, 0)
        elif source in ['明晚']:
            return MatchedTimeRange('TEV', 16, 20, 0, True, 1)
        elif source in ['明早', '明晨']:
            return MatchedTimeRange('TMO', 8, 12, 0,True, 1)
        elif source in ['昨晚']:
            return MatchedTimeRange('TEV', 16, 20, 0, True, -1)
        return MatchedTimeRange('', 0, 0, 0, False, 0)

    def get_swift_prefix(self, source: str) -> int:
        return None
