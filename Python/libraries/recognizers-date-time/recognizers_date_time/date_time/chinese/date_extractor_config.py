from typing import List, Pattern, Dict

from recognizers_text import RegExpUtility
from recognizers_number import BaseNumberExtractor
from ...resources import ChineseDateTime
from ..constants import Constants
from ..extractors import DateTimeExtractor
from ..base_date import DateTimeUtilityConfiguration
from ..base_date import DateExtractorConfiguration
from ...resources.base_date_time import BaseDateTime


class ChineseDateExtractorConfiguration(DateExtractorConfiguration):
    @property
    def week_day_start(self) -> Pattern:
        pass

    @property
    def check_both_before_after(self) -> Pattern:
        pass

    @property
    def week_day_end(self) -> Pattern:
        pass

    @property
    def number_parser(self):
        pass

    @property
    def month_regex(self) -> Pattern:
        return self._month_regex

    @property
    def day_regex(self):
        return self._day_regex

    @property
    def date_regex_list(self) -> List[Pattern]:
        return self._date_regex_list

    @property
    def implicit_date_list(self) -> List[Pattern]:
        return self._implicit_date_list

    @property
    def range_connector_symbol_regex(self) -> Pattern:
        return self._range_connector_symbol_regex

    @property
    def date_day_regex_in_chinese(self) -> Pattern:
        return self._date_day_regex_in_chinese

    @property
    def day_regex_num_in_chinese(self) -> Pattern:
        return self._day_regex_num_in_chinese

    @property
    def month_num_regex(self) -> Pattern:
        return self._month_num_regex

    @property
    def year_regex(self) -> Pattern:
        return self._year_regex

    @property
    def relative_regex(self) -> Pattern:
        return self._relative_regex

    @property
    def zero_to_nine_integer_regex_chinese(self) -> Pattern:
        return self._zero_to_nine_integer_regex_chinese

    @property
    def date_year_in_chinese_regex(self) -> Pattern:
        return self._date_year_in_chinese_regex

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
    def before_regex(self) -> Pattern:
        return self._before_regex

    @property
    def after_regex(self) -> Pattern:
        return self._after_regex

    @property
    def datetime_period_unit_regex(self) -> Pattern:
        return self._datetime_period_unit_regex

    @property
    def week_day_and_day_regex(self) -> Pattern:
        return self._week_day_and_day_regex

    @property
    def prefix_article_regex(self) -> Pattern:
        return self._prefix_article_regex

    @property
    def month_of_year(self) -> Dict[str, int]:
        return self._month_of_year

    @property
    def year_suffix(self) -> Pattern:
        return self._year_suffix

    @property
    def more_than_regex(self) -> Pattern:
        return self._more_than_regex

    @property
    def less_than_regex(self) -> Pattern:
        return self._less_than_regex

    @property
    def in_connector_regex(self) -> Pattern:
        return self._in_connector_regex

    @property
    def range_unit_regex(self) -> Pattern:
        return self._range_unit_regex

    @property
    def since_year_suffix_regex(self) -> Pattern:
        return self._since_year_suffix_regex

    @property
    def month_end(self) -> Pattern:
        return None

    @property
    def of_month(self) -> Pattern:
        return None

    @property
    def for_the_regex(self) -> Pattern:
        return None

    @property
    def week_day_and_day_of_month_regex(self) -> Pattern:
        return None

    @property
    def relative_month_regex(self) -> Pattern:
        return None

    @property
    def week_day_regex(self) -> Pattern:
        return None

    @property
    def day_of_week(self) -> Dict[str, int]:
        return None

    @property
    def ordinal_extractor(self) -> BaseNumberExtractor:
        return None

    @property
    def integer_extractor(self) -> BaseNumberExtractor:
        return None

    @property
    def duration_extractor(self) -> DateTimeExtractor:
        return None

    @property
    def strict_relative_regex(self) -> Pattern:
        return None

    @property
    def utility_configuration(self) -> DateTimeUtilityConfiguration:
        return None

    @property
    def week_day_end(self) -> Pattern:
        pass

    def __init__(self):
        self._datetime_period_unit_regex = RegExpUtility.get_safe_reg_exp(
            ChineseDateTime.DateTimePeriodUnitRegex
        )
        self._after_regex = RegExpUtility.get_safe_reg_exp(
            ChineseDateTime.AfterRegex
        )
        self._before_regex = RegExpUtility.get_safe_reg_exp(
            ChineseDateTime.BeforeRegex
        )
        self._date_unit_regex = RegExpUtility.get_safe_reg_exp(
            ChineseDateTime.UnitRegex
        )
        self._next_prefix_regex = RegExpUtility.get_safe_reg_exp(
            ChineseDateTime.NextPrefixRegex
        )
        self._last_prefix_regex = RegExpUtility.get_safe_reg_exp(
            ChineseDateTime.LastPrefixRegex
        )
        self._this_prefix_regex = RegExpUtility.get_safe_reg_exp(
            ChineseDateTime.ThisPrefixRegex
        )
        self._date_year_in_chinese_regex = RegExpUtility.get_safe_reg_exp(
            ChineseDateTime.DateYearInChineseRegex
        )
        self._zero_to_nine_integer_regex_chinese = RegExpUtility.get_safe_reg_exp(
            ChineseDateTime.ZeroToNineIntegerRegexChs
        )
        self._relative_regex = RegExpUtility.get_safe_reg_exp(
            ChineseDateTime.RelativeRegex
        )
        self._year_regex = RegExpUtility.get_safe_reg_exp(
            ChineseDateTime.YearRegex
        )
        self._month_num_regex = RegExpUtility.get_safe_reg_exp(
            ChineseDateTime.MonthNumRegex
        )
        self._day_regex_num_in_chinese = RegExpUtility.get_safe_reg_exp(
            ChineseDateTime.DayRegexNumInChinese
        )
        self._date_day_regex_in_chinese = RegExpUtility.get_safe_reg_exp(
            ChineseDateTime.DateDayRegexInChinese
        )
        self._day_regex = RegExpUtility.get_safe_reg_exp(
            ChineseDateTime.DayRegex
        )
        self._month_regex = RegExpUtility.get_safe_reg_exp(
            ChineseDateTime.MonthRegex
        )
        self._date_regex_list = [
            RegExpUtility.get_safe_reg_exp(ChineseDateTime.DateRegexList1),
            RegExpUtility.get_safe_reg_exp(ChineseDateTime.DateRegexList2),
            RegExpUtility.get_safe_reg_exp(ChineseDateTime.DateRegexList3),
            RegExpUtility.get_safe_reg_exp(ChineseDateTime.DateRegexList4),
            RegExpUtility.get_safe_reg_exp(ChineseDateTime.DateRegexList5)
        ]
        if ChineseDateTime.DefaultLanguageFallback == Constants.DEFAULT_LANGUAGE_FALLBACK_DMY:
            self._date_regex_list.append(
                RegExpUtility.get_safe_reg_exp(ChineseDateTime.DateRegexList7))
            self._date_regex_list.append(
                RegExpUtility.get_safe_reg_exp(ChineseDateTime.DateRegexList6))
        else:
            self._date_regex_list.append(
                RegExpUtility.get_safe_reg_exp(ChineseDateTime.DateRegexList6))
            self._date_regex_list.append(
                RegExpUtility.get_safe_reg_exp(ChineseDateTime.DateRegexList7))
        self._date_regex_list.append(
            RegExpUtility.get_safe_reg_exp(ChineseDateTime.DateRegexList8))
        self._implicit_date_list = [
            RegExpUtility.get_safe_reg_exp(ChineseDateTime.LunarRegex),
            RegExpUtility.get_safe_reg_exp(ChineseDateTime.SpecialDayRegex),
            RegExpUtility.get_safe_reg_exp(ChineseDateTime.DateThisRegex),
            RegExpUtility.get_safe_reg_exp(ChineseDateTime.DateLastRegex),
            RegExpUtility.get_safe_reg_exp(ChineseDateTime.DateNextRegex),
            RegExpUtility.get_safe_reg_exp(ChineseDateTime.WeekDayRegex),
            RegExpUtility.get_safe_reg_exp(
                ChineseDateTime.WeekDayOfMonthRegex),
            RegExpUtility.get_safe_reg_exp(ChineseDateTime.SpecialDate)
        ]
        self._range_connector_symbol_regex = RegExpUtility.get_safe_reg_exp(
            BaseDateTime.RangeConnectorSymbolRegex
        )
        self._check_both_before_after = False
        # TODO When the implementation for these properties is added, change the None values to their respective Regexps
        self._since_year_suffix_regex = None
        self._range_unit_regex = None
        self._in_connector_regex = None
        self._less_than_regex = None
        self._more_than_regex = None
        self._year_suffix = None
        self._month_of_year = None
        self._prefix_article_regex = None
        self._week_day_and_day_regex = None
