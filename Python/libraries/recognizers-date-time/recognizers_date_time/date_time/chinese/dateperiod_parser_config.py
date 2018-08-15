from typing import Pattern, Dict
import regex

from recognizers_text import RegExpUtility

from ...resources.chinese_date_time import ChineseDateTime
from ..extractors import DateTimeExtractor
from ..parsers import DateTimeParser
from ..base_dateperiod import DatePeriodParserConfiguration
from .duration_extractor import ChineseDurationExtractor
from .date_extractor import ChineseDateExtractor
from .date_parser import ChineseDateParser

class ChineseDatePeriodParserConfiguration(DatePeriodParserConfiguration):
    @property
    def date_extractor(self) -> DateTimeExtractor:
        return self._date_extractor

    @property
    def date_parser(self) -> DateTimeParser:
        return self._date_parser

    @property
    def duration_extractor(self) -> DateTimeExtractor:
        return self._duration_extractor

    @property
    def duration_parser(self) -> any:
        return None

    @property
    def month_front_between_regex(self) -> any:
        return None

    @property
    def between_regex(self) -> any:
        return None

    @property
    def month_front_simple_cases_regex(self) -> any:
        return None

    @property
    def simple_cases_regex(self) -> Pattern:
        return self._simple_cases_regex

    @property
    def one_word_period_regex(self) -> Pattern:
        return self._one_word_period_regex

    @property
    def month_with_year(self) -> any:
        return None

    @property
    def month_num_with_year(self) -> any:
        return None

    @property
    def year_regex(self) -> Pattern:
        return self._year_regex

    @property
    def past_regex(self) -> Pattern:
        return self._past_regex

    @property
    def future_regex(self) -> Pattern:
        return self._future_regex

    @property
    def in_connector_regex(self) -> any:
        return None

    @property
    def week_of_month_regex(self) -> Pattern:
        return self._week_of_month_regex

    @property
    def week_of_year_regex(self) -> any:
        return None

    @property
    def quarter_regex(self) -> Pattern:
        return self._quarter_regex

    @property
    def quarter_regex_year_front(self) -> any:
        return None

    @property
    def all_half_year_regex(self) -> any:
        return None

    @property
    def season_regex(self) -> Pattern:
        return self._season_regex

    @property
    def week_of_regex(self) -> any:
        return None

    @property
    def month_of_regex(self) -> any:
        return None

    @property
    def which_week_regex(self) -> any:
        return None

    @property
    def next_prefix_regex(self) -> Pattern:
        return self._next_prefix_regex

    @property
    def past_prefix_regex(self) -> Pattern:
        return self._past_prefix_regex

    @property
    def this_prefix_regex(self) -> Pattern:
        return self._this_prefix_regex

    @property
    def rest_of_date_regex(self) -> any:
        return None

    @property
    def later_early_period_regex(self) -> Pattern:
        return self._later_early_period_regex

    @property
    def week_with_week_day_range_regex(self) -> Pattern:
        return self._week_with_week_day_range_regex

    @property
    def token_before_date(self) -> str:
        return self._token_before_date

    @property
    def day_of_month(self) -> Dict[str, int]:
        return self._day_of_month

    @property
    def month_of_year(self) -> Dict[str, int]:
        return self._month_of_year

    @property
    def cardinal_map(self) -> Dict[str, int]:
        return self._cardinal_map

    @property
    def season_map(self) -> Dict[str, str]:
        return self._season_map

    @property
    def unit_map(self) -> Dict[str, str]:
        return self._unit_map

    def __init__(self):
        self._date_extractor = ChineseDateExtractor()
        self._date_parser = ChineseDateParser()
        self._duration_extractor = ChineseDurationExtractor()
        self._simple_cases_regex = RegExpUtility.get_safe_reg_exp(ChineseDateTime.SimpleCasesRegex)
        self._one_word_period_regex = RegExpUtility.get_safe_reg_exp(ChineseDateTime.OneWordPeriodRegex)
        self._year_regex = RegExpUtility.get_safe_reg_exp(ChineseDateTime.DatePeriodYearRegex)
        self._past_regex = RegExpUtility.get_safe_reg_exp(ChineseDateTime.PastRegex)
        self._future_regex = RegExpUtility.get_safe_reg_exp(ChineseDateTime.FutureRegex)
        self._week_of_month_regex = RegExpUtility.get_safe_reg_exp(ChineseDateTime.WeekOfMonthRegex)
        self._quarter_regex = RegExpUtility.get_safe_reg_exp(ChineseDateTime.QuarterRegex)
        self._season_regex = RegExpUtility.get_safe_reg_exp(ChineseDateTime.SeasonRegex)
        self._next_prefix_regex = RegExpUtility.get_safe_reg_exp(ChineseDateTime.DatePeriodNextRegex)
        self._past_prefix_regex = RegExpUtility.get_safe_reg_exp(ChineseDateTime.DatePeriodLastRegex)
        self._this_prefix_regex = RegExpUtility.get_safe_reg_exp(ChineseDateTime.DatePeriodThisRegex)
        self._later_early_period_regex = RegExpUtility.get_safe_reg_exp(r'\0')
        self._week_with_week_day_range_regex = RegExpUtility.get_safe_reg_exp(r'\0')
        self._token_before_date = ' on '
        self._day_of_month = ChineseDateTime.ParserConfigurationDayOfMonth
        self._month_of_year = ChineseDateTime.ParserConfigurationMonthOfYear
        self._cardinal_map = ChineseDateTime.ParserConfigurationCardinalMap
        self._season_map = ChineseDateTime.ParserConfigurationSeasonMap
        self._unit_map = ChineseDateTime.ParserConfigurationUnitMap

    def get_swift_day_or_month(self, source: str) -> int:
        source = source.strip().lower()
        if source.endswith('去年'):
            return -1
        if source.endswith('明年'):
            return 1
        if source.endswith('前年'):
            return -2
        if source.endswith('后年'):
            return 2
        if source.startswith('下个'):
            return 1
        if source.startswith('上个'):
            return -1
        if regex.search(self.this_prefix_regex, source):
            return 0
        if regex.search(self.next_prefix_regex, source):
            return 1
        if regex.search(self.past_prefix_regex, source):
            return -1
        return 0

    def get_swift_year(self, source: str) -> int:
        source = source.strip().lower()
        swift = -10
        if source.startswith('明年'):
            swift = 1
        elif source.startswith('去年'):
            swift = -1
        elif source.startswith('今年'):
            swift = 0
        return swift

    def is_future(self, source: str) -> bool:
        return regex.search(self.this_prefix_regex, source) or regex.search(self.next_prefix_regex, source)

    def is_year_to_date(self, source: str) -> bool:
        return source.strip().lower() == '今年'

    def is_month_to_date(self, source: str) -> bool:
        return False

    def is_week_only(self, source: str) -> bool:
        return source.strip().lower().endswith('周') or source.strip().lower().endswith('星期')

    def is_weekend(self, source: str) -> bool:
        return source.strip().lower().endswith('周末')

    def is_month_only(self, source: str) -> bool:
        return source.strip().lower().endswith('月')

    def is_last_cardinal(self, source: str) -> bool:
        return source.strip().lower() == '最后一'

    def is_year_only(self, source: str) -> bool:
        return source.strip().lower().endswith('年')
