from typing import List, Pattern, Dict

from recognizers_text import RegExpUtility, Extractor, Parser
from recognizers_number import ChineseIntegerExtractor, ChineseNumberParser, ChineseNumberParserConfiguration

from ...resources.chinese_date_time import ChineseDateTime
from ..constants import Constants
from ..base_date import DateParserConfiguration

class ChineseDateParserConfiguration(DateParserConfiguration):
    @property
    def ordinal_extractor(self) -> any:
        return None

    @property
    def integer_extractor(self) -> Extractor:
        return self._integer_extractor

    @property
    def cardinal_extractor(self) -> any:
        return None

    @property
    def duration_extractor(self) -> any:
        return None

    @property
    def number_parser(self) -> Parser:
        return self._number_parser

    @property
    def duration_parser(self) -> any:
        return None

    @property
    def month_of_year(self) -> Dict[str, int]:
        return self._month_of_year

    @property
    def day_of_month(self) -> Dict[str, int]:
        return self._day_of_month

    @property
    def day_of_week(self) -> Dict[str, int]:
        return self._day_of_week

    @property
    def unit_map(self) -> any:
        return None

    @property
    def cardinal_map(self) -> any:
        return None

    @property
    def date_regex(self) -> List[Pattern]:
        return self._date_regex

    @property
    def on_regex(self) -> any:
        return None

    @property
    def special_day_regex(self) -> Pattern:
        return self._special_day_regex

    @property
    def special_day_with_num_regex(self) -> Pattern:
        return self._special_day_with_num_regex

    @property
    def next_regex(self) -> Pattern:
        return self._next_regex

    @property
    def unit_regex(self) -> any:
        return None

    @property
    def month_regex(self) -> any:
        return None

    @property
    def week_day_regex(self) -> Pattern:
        return self._week_day_regex

    @property
    def last_regex(self) -> Pattern:
        return self._last_regex

    @property
    def this_regex(self) -> Pattern:
        return self._this_regex

    @property
    def week_day_of_month_regex(self) -> any:
        return None

    @property
    def for_the_regex(self) -> any:
        return None

    @property
    def week_day_and_day_of_month_regex(self) -> any:
        return None

    @property
    def relative_month_regex(self) -> any:
        return None

    @property
    def relative_week_day_regex(self) -> any:
        return None

    @property
    def utility_configuration(self) -> any:
        return None

    @property
    def date_token_prefix(self) -> any:
        return None

    def get_swift_day(self, source: str) -> int:
        source = source.strip().lower()
        swift = 0
        if source in ['今天', '今日', '最近']:
            swift = 0
        elif source in ['明天', '明日']:
            swift = 1
        elif source in ['昨天']:
            swift = -1
        elif source.endswith('大后天'):
            swift = 3
        elif source.endswith('大前天'):
            swift = -3
        elif source.endswith('后天'):
            swift = 2
        elif source.endswith('前天'):
            swift = -2
        return swift

    def get_swift_month(self, source: str) -> int:
        source = source.strip().lower()
        swift = 0
        if source.startswith(ChineseDateTime.ParserConfigurationNextMonthToken):
            swift = 1
        elif source.startswith(ChineseDateTime.ParserConfigurationLastMonthToken):
            swift = -1
        return swift

    def is_cardinal_last(self, source: str) -> bool:
        return source == ChineseDateTime.ParserConfigurationLastWeekDayToken

    def __init__(self):
        self._date_regex = [
            RegExpUtility.get_safe_reg_exp(ChineseDateTime.DateRegexList1),
            RegExpUtility.get_safe_reg_exp(ChineseDateTime.DateRegexList2),
            RegExpUtility.get_safe_reg_exp(ChineseDateTime.DateRegexList3),
            RegExpUtility.get_safe_reg_exp(ChineseDateTime.DateRegexList4),
            RegExpUtility.get_safe_reg_exp(ChineseDateTime.DateRegexList5)
        ]

        if ChineseDateTime.DefaultLanguageFallback == Constants.DEFAULT_LANGUAGE_FALLBACK_DMY:
            self._date_regex.append(RegExpUtility.get_safe_reg_exp(ChineseDateTime.DateRegexList7))
            self._date_regex.append(RegExpUtility.get_safe_reg_exp(ChineseDateTime.DateRegexList6))
        else:
            self._date_regex.append(RegExpUtility.get_safe_reg_exp(ChineseDateTime.DateRegexList6))
            self._date_regex.append(RegExpUtility.get_safe_reg_exp(ChineseDateTime.DateRegexList7))

        self._date_regex.append(RegExpUtility.get_safe_reg_exp(ChineseDateTime.DateRegexList8))

        self._month_of_year = ChineseDateTime.ParserConfigurationMonthOfYear
        self._day_of_month = ChineseDateTime.ParserConfigurationDayOfMonth
        self._day_of_week = ChineseDateTime.ParserConfigurationDayOfWeek
        self._special_day_regex = RegExpUtility.get_safe_reg_exp(ChineseDateTime.SpecialDayRegex)
        self._special_day_with_num_regex = RegExpUtility.get_safe_reg_exp(ChineseDateTime.SpecialDayWithNumRegex)
        self._this_regex = RegExpUtility.get_safe_reg_exp(ChineseDateTime.DateThisRegex)
        self._next_regex = RegExpUtility.get_safe_reg_exp(ChineseDateTime.DateNextRegex)
        self._last_regex = RegExpUtility.get_safe_reg_exp(ChineseDateTime.DateLastRegex)
        self._week_day_regex = RegExpUtility.get_safe_reg_exp(ChineseDateTime.WeekDayRegex)
        self._integer_extractor = ChineseIntegerExtractor()
        self._number_parser = ChineseNumberParser(ChineseNumberParserConfiguration())
