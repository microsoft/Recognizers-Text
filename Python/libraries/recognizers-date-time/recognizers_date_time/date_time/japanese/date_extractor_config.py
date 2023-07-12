#  Copyright (c) Microsoft Corporation. All rights reserved.
#  Licensed under the MIT License.

from typing import List, Pattern, Dict

from recognizers_text import RegExpUtility
from recognizers_number import JapaneseIntegerExtractor, CJKNumberParser, JapaneseNumberParserConfiguration
from recognizers_date_time.date_time.CJK.base_date import CJKDateExtractorConfiguration
from recognizers_date_time.date_time.constants import Constants
from recognizers_date_time.date_time.extractors import DateTimeExtractor
from recognizers_date_time.resources import JapaneseDateTime
from recognizers_date_time.resources.base_date_time import BaseDateTime


class JapaneseDateExtractorConfiguration(CJKDateExtractorConfiguration):

    @property
    def week_day_regex(self) -> Pattern:
        return self._week_day_regex

    @property
    def lunar_regex(self) -> Pattern:
        return self._lunar_regex

    @property
    def this_regex(self) -> Pattern:
        return self._this_regex

    @property
    def last_regex(self) -> Pattern:
        return self._last_regex

    @property
    def next_regex(self) -> Pattern:
        return self._next_regex

    @property
    def special_day_regex(self) -> Pattern:
        return self._special_day_regex

    @property
    def week_day_of_month_regex(self) -> Pattern:
        return self._week_day_of_month

    @property
    def special_date_regex(self) -> Pattern:
        return self._special_date_regex

    @property
    def special_day_with_num_regex(self) -> Pattern:
        return self._special_day_with_num_regex

    @property
    def before_regex(self) -> Pattern:
        return self._before_regex

    @property
    def after_regex(self) -> Pattern:
        return self._after_regex

    @property
    def week_day_start_end(self) -> Pattern:
        return self._week_day_start_end

    @property
    def datetime_period_unit_regex(self) -> Pattern:
        return self._datetime_period_unit_regex

    @property
    def range_connector_symbol_regex(self) -> Pattern:
        return self._range_connector_symbol_regex

    @property
    def month_regex(self) -> Pattern:
        return self._month_regex

    @property
    def day_regex(self):
        return self._day_regex

    @property
    def date_day_regex_in_cjk(self) -> Pattern:
        return self._date_day_regex_in_CJK

    @property
    def day_regex_num_in_cjk(self) -> Pattern:
        return self._day_regex_num_in_CJK

    @property
    def month_num_regex(self) -> Pattern:
        return self._month_num_regex

    @property
    def week_day_and_day_regex(self) -> Pattern:
        return self._week_day_and_day_regex

    @property
    def duration_relative_duration_unit_regex(self) -> Pattern:
        return self._duration_relative_duration_unit_regex

    @property
    def year_regex(self) -> Pattern:
        return self._year_regex

    @property
    def relative_regex(self) -> Pattern:
        return self._relative_regex

    @property
    def relative_month_regex(self) -> Pattern:
        return self._relative_month_regex

    @property
    def zero_to_nine_integer_regex_cjk(self) -> Pattern:
        return self._zero_to_nine_integer_regex_cjk

    @property
    def date_year_in_cjk_regex(self) -> Pattern:
        return self._date_year_in_cjk_regex

    @property
    def this_prefix_regex(self) -> Pattern:
        return self._this_prefix_regex

    @property
    def last_prefix_regex(self) -> Pattern:
        return self._last_prefix_regex

    @property
    def next_prefix_regex(self) -> Pattern:
        return self._next_prefix_regex

    @property
    def date_unit_regex(self) -> Pattern:
        return self._date_unit_regex

    @property
    def dynasty_year_regex(self) -> Pattern:
        return self._dynasty_year_regex

    @property
    def dynasty_start_year(self) -> str:
        return self._dynasty_start_year

    @property
    def dynasty_year_map(self) -> Dict[str, int]:
        return self._dynasty_year_map

    @property
    def date_regex_list(self) -> List[Pattern]:
        return self._date_regex_list

    @property
    def implicit_date_list(self) -> List[Pattern]:
        return self._implicit_date_list

    @property
    def duration_extractor(self) -> DateTimeExtractor:
        return self._duration_extractor

    @property
    def ambiguity_date_filters_dict(self) -> Dict[Pattern, Pattern]:
        return self._ambiguity_date_filters_dict

    @property
    def number_parser(self) -> CJKNumberParser:
        return self._number_parser

    def __init__(self):
        self._duration_extractor = None

        # ２０１６年１２月１日
        self._date_regex_list_1 = RegExpUtility.get_safe_reg_exp(JapaneseDateTime.DateRegexList1)

        # 金曜日 6月 15日
        self._date_regex_list_2 = RegExpUtility.get_safe_reg_exp(JapaneseDateTime.DateRegexList2)

        # (2015年)?(农历)?十月二十(星期三)?
        self._date_regex_list_3 = RegExpUtility.get_safe_reg_exp(JapaneseDateTime.DateRegexList3)

        # 2015-12-23
        self._date_regex_list_8 = RegExpUtility.get_safe_reg_exp(JapaneseDateTime.DateRegexList8)

        # 23/7
        self._date_regex_list_5 = RegExpUtility.get_safe_reg_exp(JapaneseDateTime.DateRegexList5)

        # 7/23
        self._date_regex_list_4 = RegExpUtility.get_safe_reg_exp(JapaneseDateTime.DateRegexList4)

        # 23-3-2017
        self._date_regex_list_7 = RegExpUtility.get_safe_reg_exp(JapaneseDateTime.DateRegexList7)

        # 3-23-2015
        self._date_regex_list_6 = RegExpUtility.get_safe_reg_exp(JapaneseDateTime.DateRegexList6)

        # １２月１日
        self._date_regex_list_9 = RegExpUtility.get_safe_reg_exp(JapaneseDateTime.DateRegexList9)

        # 2015/12/23
        self._date_regex_list_10 = RegExpUtility.get_safe_reg_exp(JapaneseDateTime.DateRegexList10)

        # 2016/12 (this is not a Date)
        # self._date_regex_list_11 = RegExpUtility.get_safe_reg_exp(JapaneseDateTime.DateRegexList11)

        self._date_regex_list = [
            self._date_regex_list_1, self._date_regex_list_10, self._date_regex_list_2, self._date_regex_list_9,
            self._date_regex_list_3, self._date_regex_list_4, self._date_regex_list_5
        ]

        # Regex precedence where the order between D and M varies is controlled by DefaultLanguageFallback

        if JapaneseDateTime.DefaultLanguageFallback == Constants.DEFAULT_LANGUAGE_FALLBACK_DMY:
            self._date_regex_list.extend([self._date_regex_list_7, self._date_regex_list_6, self._date_regex_list_8])
        else:
            self._date_regex_list.extend([self._date_regex_list_6, self._date_regex_list_7, self._date_regex_list_8])

        self._integer_extractor = JapaneseIntegerExtractor()
        self._number_parser = CJKNumberParser(JapaneseNumberParserConfiguration())

        self._week_day_regex = RegExpUtility.get_safe_reg_exp(JapaneseDateTime.WeekDayRegex)
        self._lunar_regex = RegExpUtility.get_safe_reg_exp(JapaneseDateTime.LunarRegex)
        self._this_regex = RegExpUtility.get_safe_reg_exp(JapaneseDateTime.DateThisRegex)
        self._last_regex = RegExpUtility.get_safe_reg_exp(JapaneseDateTime.DateLastRegex)
        self._next_regex = RegExpUtility.get_safe_reg_exp(JapaneseDateTime.DateNextRegex)
        self._special_day_regex = RegExpUtility.get_safe_reg_exp(JapaneseDateTime.SpecialDayRegex)
        self._week_day_of_month = RegExpUtility.get_safe_reg_exp(JapaneseDateTime.WeekDayOfMonthRegex)
        self._special_date_regex = RegExpUtility.get_safe_reg_exp(JapaneseDateTime.SpecialDate)
        self._special_day_with_num_regex = RegExpUtility.get_safe_reg_exp(JapaneseDateTime.SpecialDayWithNumRegex)
        self._month_regex = RegExpUtility.get_safe_reg_exp(JapaneseDateTime.MonthRegex)
        self._day_regex = RegExpUtility.get_safe_reg_exp(JapaneseDateTime.DayRegex)
        self._date_day_regex_in_CJK = RegExpUtility.get_safe_reg_exp(JapaneseDateTime.DateDayRegexInCJK)
        self._day_regex_num_in_CJK = RegExpUtility.get_safe_reg_exp(JapaneseDateTime.DayRegexNumInCJK)
        self._month_num_regex = RegExpUtility.get_safe_reg_exp(JapaneseDateTime.MonthNumRegex)
        self._week_day_and_day_regex = RegExpUtility.get_safe_reg_exp(JapaneseDateTime.WeekDayAndDayRegex)
        self._duration_relative_duration_unit_regex = RegExpUtility.get_safe_reg_exp(
            JapaneseDateTime.DurationRelativeDurationUnitRegex)
        self._year_regex = RegExpUtility.get_safe_reg_exp(JapaneseDateTime.YearRegex)
        self._relative_regex = RegExpUtility.get_safe_reg_exp(JapaneseDateTime.RelativeRegex)
        self._relative_month_regex = RegExpUtility.get_safe_reg_exp(JapaneseDateTime.RelativeMonthRegex)
        self._zero_to_nine_integer_regex_cjk = RegExpUtility.get_safe_reg_exp(
            JapaneseDateTime.ZeroToNineIntegerRegexCJK)
        self._date_year_in_cjk_regex = RegExpUtility.get_safe_reg_exp(JapaneseDateTime.DateYearInCJKRegex)
        self._this_prefix_regex = RegExpUtility.get_safe_reg_exp(JapaneseDateTime.ThisPrefixRegex)
        self._last_prefix_regex = RegExpUtility.get_safe_reg_exp(JapaneseDateTime.LastPrefixRegex)
        self._next_prefix_regex = RegExpUtility.get_safe_reg_exp(JapaneseDateTime.NextPrefixRegex)
        self._date_unit_regex = RegExpUtility.get_safe_reg_exp(JapaneseDateTime.UnitRegex)
        self._dynasty_year_regex = RegExpUtility.get_safe_reg_exp(JapaneseDateTime.DynastyYearRegex)
        self._dynasty_start_year = JapaneseDateTime.DynastyStartYear
        self._dynasty_year_map = JapaneseDateTime.DynastyYearMap

        self._datetime_period_unit_regex = RegExpUtility.get_safe_reg_exp(JapaneseDateTime.DateTimePeriodUnitRegex)
        self._before_regex = RegExpUtility.get_safe_reg_exp(JapaneseDateTime.BeforeRegex)
        self._after_regex = RegExpUtility.get_safe_reg_exp(JapaneseDateTime.AfterRegex)
        self._week_day_start_end = RegExpUtility.get_safe_reg_exp(JapaneseDateTime.WeekDayStartEnd)
        self._range_connector_symbol_regex = RegExpUtility.get_safe_reg_exp(BaseDateTime.RangeConnectorSymbolRegex)
        self._ambiguity_date_filters_dict = JapaneseDateTime.AmbiguityDateTimeFiltersDict

        self._implicit_date_list = [
            self.special_day_with_num_regex, self.special_day_regex, self.this_regex, self.last_regex, self.next_regex,
            self.week_day_regex, self.week_day_of_month_regex, self.special_date_regex, self.week_day_and_day_regex
        ]
