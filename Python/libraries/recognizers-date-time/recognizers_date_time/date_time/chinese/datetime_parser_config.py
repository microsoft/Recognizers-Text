from typing import Pattern, Dict

from recognizers_text.utilities import RegExpUtility
from recognizers_number.number.extractors import BaseNumberExtractor
from recognizers_number.number.parsers import BaseNumberParser
from ...resources.chinese_date_time import ChineseDateTime
from ..extractors import DateTimeExtractor
from ..parsers import DateTimeParser
from ..utilities import DateTimeUtilityConfiguration
from ..base_configs import BaseDateParserConfiguration
from ..base_datetime import DateTimeParserConfiguration, MatchedTimex
from .date_extractor import ChineseDateExtractor
from .time_extractor import ChineseTimeExtractor
from .date_parser import ChineseDateParser
from .time_parser import ChineseTimeParser

class ChineseDateTimeParserConfiguration():
    @property
    def token_before_date(self) -> str:
        raise NotImplementedError()

    @property
    def token_before_time(self) -> str:
        raise NotImplementedError()

    @property
    def date_extractor(self) -> DateTimeExtractor:
        return self._date_extractor

    @property
    def time_extractor(self) -> DateTimeExtractor:
        return self._time_extractor

    @property
    def date_parser(self) -> DateTimeParser:
        return self._date_parser

    @property
    def time_parser(self) -> DateTimeParser:
        return self._time_parser

    @property
    def cardinal_extractor(self) -> BaseNumberExtractor:
        raise NotImplementedError()

    @property
    def number_parser(self) -> BaseNumberParser:
        raise NotImplementedError()

    @property
    def duration_extractor(self) -> DateTimeExtractor:
        raise NotImplementedError()

    @property
    def duration_parser(self) -> DateTimeParser:
        raise NotImplementedError()

    @property
    def now_regex(self) -> Pattern:
        return self._now_regex

    @property
    def am_time_regex(self) -> Pattern:
        return self._am_time_regex

    @property
    def pm_time_regex(self) -> Pattern:
        return self._pm_time_regex

    @property
    def simple_time_of_today_after_regex(self) -> Pattern:
        raise NotImplementedError()

    @property
    def simple_time_of_today_before_regex(self) -> Pattern:
        raise NotImplementedError()

    @property
    def specific_time_of_day_regex(self) -> Pattern:
        return self._specific_time_of_day_regex

    @property
    def the_end_of_regex(self) -> Pattern:
        raise NotImplementedError()

    @property
    def unit_regex(self) -> Pattern:
        raise NotImplementedError()

    @property
    def unit_map(self) -> Dict[str, str]:
        raise NotImplementedError()

    @property
    def numbers(self) -> Dict[str, int]:
        raise NotImplementedError()

    @property
    def utility_configuration(self) -> DateTimeUtilityConfiguration:
        raise NotImplementedError()

    def __init__(self):
        self._date_extractor = ChineseDateExtractor()
        self._time_extractor = ChineseTimeExtractor()
        self._date_parser = ChineseDateParser()
        self._time_parser = ChineseTimeParser()
        self._pm_time_regex = RegExpUtility.get_safe_reg_exp(ChineseDateTime.DateTimeSimplePmRegex)
        self._am_time_regex = RegExpUtility.get_safe_reg_exp(ChineseDateTime.DateTimeSimpleAmRegex)
        self._specific_time_of_day_regex = RegExpUtility.get_safe_reg_exp(ChineseDateTime.TimeOfTodayRegex)
        self._now_regex = RegExpUtility.get_safe_reg_exp(ChineseDateTime.NowRegex)

    def have_ambiguous_token(self, source: str, matched_text: str) -> bool:
        return False

    def get_matched_now_timex(self, source: str) -> MatchedTimex:
        source = source.strip().lower()

        if source.endswith('现在'):
            return MatchedTimex(True, 'PRESENT_REF')
        elif source in ['刚刚才', '刚刚', '刚才']:
            return MatchedTimex(True, 'PAST_REF')
        elif source in ['立刻', '马上']:
            return MatchedTimex(True, 'FUTURE_REF')

        return MatchedTimex(False, None)

    def get_swift_day(self, source: str) -> int:
        source = source.strip().lower()

        if source in ['明晚', '明早', '明晨']:
            return 1
        elif source in ['昨晚']:
            return -1

        return 0

    def get_hour(self, source: str, hour: int) -> int:
        source = source.strip().lower()
        result = hour

        if hour < 12 and source in ['今晚', '明晚', '昨晚']:
            result += 12
        elif hour >= 12 and source in ['今早', '今晨', '明早', '明晨']:
            result -= 12

        return result
