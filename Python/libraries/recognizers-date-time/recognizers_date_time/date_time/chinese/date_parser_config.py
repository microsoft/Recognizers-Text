#  Copyright (c) Microsoft Corporation. All rights reserved.
#  Licensed under the MIT License.

from typing import List, Pattern, Dict

from recognizers_text import RegExpUtility, Extractor, Parser
from recognizers_number import CJKNumberParser, ChineseIntegerExtractor, ChineseNumberParserConfiguration

from ...resources.chinese_date_time import ChineseDateTime
from ..constants import Constants
from ..base_date import DateParserConfiguration
from ..extractors import DateTimeExtractor


class ChineseDateParserConfiguration(DateParserConfiguration):

    @property
    def check_both_before_after(self) -> bool:
        pass

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
    def date_extractor(self) -> DateTimeExtractor:
        return self._date_extractor

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
        return ChineseDateTime.ParserConfigurationUnitMap

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
        return self._week_day_of_month_regex

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

    @property
    def dynasty_year_regex(self) -> Pattern:
        return self._dynasty_year_regex

    @property
    def dynasty_year_map(self) -> Dict[str, int]:
        return self._dynasty_year_map

    @property
    def dynasty_start_year(self) -> str:
        return self._dynasty_start_year

    def get_swift_day(self, source: str) -> int:
        source = source.strip().lower()
        swift = 0
        if source in ['今天', '今日', '最近']:
            swift = 0
        elif source.startswith('明'):
            swift = 1
        elif source.startswith('昨'):
            swift = -1
        elif source == '大后天' or source == '大後天':
            swift = 3
        elif source == '大前天':
            swift = -3
        elif source == '后天' or source == '後天':
            swift = 2
        elif source == '前天':
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
            # 2015-12-23 - This regex represents the standard format in Chinese dates (YMD) and has precedence over other orderings
            RegExpUtility.get_safe_reg_exp(ChineseDateTime.DateRegexList8)
        ]

        # Regex precedence where the order between D and M varies is controlled by DefaultLanguageFallback
        if ChineseDateTime.DefaultLanguageFallback == Constants.DEFAULT_LANGUAGE_FALLBACK_DMY:
            order_regex_list = [ChineseDateTime.DateRegexList5, ChineseDateTime.DateRegexList4]
        else:
            order_regex_list = [ChineseDateTime.DateRegexList4, ChineseDateTime.DateRegexList5]

        if ChineseDateTime.DefaultLanguageFallback in [Constants.DEFAULT_LANGUAGE_FALLBACK_DMY, Constants.DEFAULT_LANGUAGE_FALLBACK_YMD]:
            order_regex_list.extend([ChineseDateTime.DateRegexList7, ChineseDateTime.DateRegexList6])
        else:
            order_regex_list.extend([ChineseDateTime.DateRegexList6, ChineseDateTime.DateRegexList7])
        self._date_regex.extend([RegExpUtility.get_safe_reg_exp(ii) for ii in order_regex_list])

        self._month_of_year = ChineseDateTime.ParserConfigurationMonthOfYear
        self._day_of_month = ChineseDateTime.ParserConfigurationDayOfMonth
        self._day_of_week = ChineseDateTime.ParserConfigurationDayOfWeek
        self._special_day_regex = RegExpUtility.get_safe_reg_exp(
            ChineseDateTime.SpecialDayRegex)
        self._special_day_with_num_regex = RegExpUtility.get_safe_reg_exp(
            ChineseDateTime.SpecialDayWithNumRegex)
        self._this_regex = RegExpUtility.get_safe_reg_exp(
            ChineseDateTime.DateThisRegex)
        self._next_regex = RegExpUtility.get_safe_reg_exp(
            ChineseDateTime.DateNextRegex)
        self._last_regex = RegExpUtility.get_safe_reg_exp(
            ChineseDateTime.DateLastRegex)
        self._unit_regex = RegExpUtility.get_safe_reg_exp(
            ChineseDateTime.DateUnitRegex)
        self._unit_map = ChineseDateTime.ParserConfigurationUnitMap
        self._week_day_of_month_regex = RegExpUtility.get_safe_reg_exp(
            ChineseDateTime.WeekDayOfMonthRegex)
        self._week_day_regex = RegExpUtility.get_safe_reg_exp(
            ChineseDateTime.WeekDayRegex)
        self._dynasty_year_regex = RegExpUtility.get_safe_reg_exp(
            ChineseDateTime.DynastyYearRegex)
        self._dynasty_year_map = ChineseDateTime.DynastyYearMap
        self._integer_extractor = ChineseIntegerExtractor()
        self._number_parser = CJKNumberParser(ChineseNumberParserConfiguration())
        self._date_extractor = None
        self._dynasty_start_year = ChineseDateTime.DynastyStartYear
