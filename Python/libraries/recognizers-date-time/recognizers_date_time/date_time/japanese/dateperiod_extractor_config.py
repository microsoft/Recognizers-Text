#  Copyright (c) Microsoft Corporation. All rights reserved.
#  Licensed under the MIT License.

from typing import List, Pattern, Dict

from recognizers_text import Extractor, RegExpUtility
from recognizers_number import JapaneseNumberExtractor
from recognizers_date_time.date_time.japanese.date_extractor_config import JapaneseDateExtractorConfiguration
from recognizers_date_time.date_time.CJK.base_date import BaseCJKDateExtractor
from recognizers_date_time.date_time.CJK.base_dateperiod import CJKDatePeriodExtractorConfiguration
from recognizers_date_time.resources.japanese_date_time import JapaneseDateTime
from recognizers_date_time.date_time.extractors import DateTimeExtractor


class JapaneseDatePeriodExtractorConfiguration(CJKDatePeriodExtractorConfiguration):

    @property
    def till_regex(self) -> Pattern:
        return self._till_regex

    @property
    def range_prefix_regex(self) -> Pattern:
        return self._range_prefix_regex

    @property
    def range_suffix_regex(self) -> Pattern:
        return self._range_suffix_regex

    @property
    def strict_year_regex(self) -> Pattern:
        return self._strict_year_regex

    @property
    def year_in_cjk_regex(self) -> Pattern:
        return self._year_in_cjk_regex

    # for case "（より）？（2017）？12日に1月10日"
    @property
    def simple_cases_regex(self) -> Pattern:
        return self._simple_cases_regex

    @property
    def year_and_month(self) -> Pattern:
        return self._year_and_month

    # 2017.12, 2017-12, 2017/12, 12/2017
    @property
    def pure_num_year_and_month(self) -> Pattern:
        return self._pure_num_year_and_month

    @property
    def simple_year_and_month(self) -> Pattern:
        return self._simple_year_and_month

    @property
    def one_word_period_regex(self) -> Pattern:
        return self._one_word_period_regex

    @property
    def week_of_month_regex(self) -> Pattern:
        return self._week_of_month_regex

    @property
    def week_of_year_regex(self) -> Pattern:
        return self._week_of_year_regex

    @property
    def week_of_date_regex(self) -> Pattern:
        return self._week_of_date_regex

    @property
    def month_of_date_regex(self) -> Pattern:
        return self._month_of_date_regex

    @property
    def which_week_regex(self) -> Pattern:
        return self._which_week_regex

    @property
    def followed_unit(self) -> Pattern:
        return self._followed_unit

    @property
    def number_combined_with_unit(self) -> Pattern:
        return self._number_combined_with_unit

    @property
    def year_to_year(self) -> Pattern:
        return self._year_to_year

    @property
    def year_to_year_suffix_required(self) -> Pattern:
        return self._year_to_year_suffix_required

    @property
    def month_to_month(self) -> Pattern:
        return self._month_to_month

    @property
    def month_to_month_suffix_required(self) -> Pattern:
        return self._month_to_month_suffix_required

    @property
    def day_to_day(self) -> Pattern:
        return self._day_to_day

    @property
    def day_regex_for_period(self) -> Pattern:
        return self._day_regex_for_period

    @property
    def month_day_range(self) -> Pattern:
        return self._month_day_range

    @property
    def year_month_range(self) -> Pattern:
        return self._year_month_range

    @property
    def year_month_day_range(self) -> Pattern:
        return self._year_month_day_range

    @property
    def past_regex(self) -> Pattern:
        return self._past_regex

    @property
    def future_regex(self) -> Pattern:
        return self._future_regex

    @property
    def week_with_weekday_range_regex(self) -> Pattern:
        return self._week_with_weekday_range_regex

    @property
    def first_last_of_year_regex(self) -> Pattern:
        return self._first_last_of_year_regex

    @property
    def season_with_year_regex(self) -> Pattern:
        return self._season_with_year_regex

    @property
    def quarter_regex(self) -> Pattern:
        return self._quarter_regex

    @property
    def decade_regex(self) -> Pattern:
        return self._decade_regex

    @property
    def century_regex(self) -> Pattern:
        return self._century_regex

    @property
    def special_month_regex(self) -> Pattern:
        return self._special_month_regex

    @property
    def special_year_regex(self) -> Pattern:
        return self._special_year_regex

    @property
    def day_regex(self) -> Pattern:
        return self._day_regex

    @property
    def day_regex_in_cjk(self) -> Pattern:
        return self._day_regex_in_cjk

    @property
    def month_num_regex(self) -> Pattern:
        return self._month_num_regex

    @property
    def this_regex(self) -> Pattern:
        return self._this_regex

    @property
    def date_unit_regex(self) -> Pattern:
        return self._date_unit_regex

    @property
    def last_regex(self) -> Pattern:
        return self._last_regex

    @property
    def next_regex(self) -> Pattern:
        return self._next_regex

    @property
    def relative_month_regex(self) -> Pattern:
        return self._relative_month_regex

    @property
    def later_early_period_regex(self) -> Pattern:
        return self._later_early_period_regex

    @property
    def date_point_with_ago_and_later(self) -> Pattern:
        return self._date_point_with_ago_and_later

    @property
    def reference_date_period_regex(self) -> Pattern:
        return self._reference_date_period_regex

    @property
    def complex_date_period_regex(self) -> Pattern:
        return self._complex_date_period_regex

    @property
    def month_regex(self) -> Pattern:
        return self._month_regex

    @property
    def year_regex(self) -> Pattern:
        return self._year_regex

    @property
    def year_regex_in_number(self) -> Pattern:
        return self._year_regex_in_number

    @property
    def zero_to_nine_integer_regex_cjk(self) -> Pattern:
        return self._zero_to_nine_integer_regex_cjk

    @property
    def month_suffix_regex(self) -> Pattern:
        return self._month_suffix_regex

    @property
    def unit_regex(self) -> Pattern:
        return self._unit_regex

    @property
    def duration_unit_regex(self) -> Pattern:
        return self._duration_unit_regex

    @property
    def season_regex(self):
        return self._season_regex

    @property
    def date_point_extractor(self) -> DateTimeExtractor:
        return self._date_point_extractor

    @property
    def duration_extractor(self):
        return self._duration_extractor

    @property
    def integer_extractor(self) -> Extractor:
        return self._integer_extractor

    @property
    def simple_cases_regexes(self) -> List[Pattern]:
        return self._simple_cases_regexes

    @property
    def ambiguity_filters_dict(self) -> Dict[Pattern, Pattern]:
        return self._ambiguity_filters_dict

    def __init__(self):
        super().__init__()
        self._till_regex = RegExpUtility.get_safe_reg_exp(JapaneseDateTime.DatePeriodTillRegex)
        self._range_prefix_regex = RegExpUtility.get_safe_reg_exp(JapaneseDateTime.DatePeriodRangePrefixRegex)
        self._range_suffix_regex = RegExpUtility.get_safe_reg_exp(JapaneseDateTime.DatePeriodRangeSuffixRegex)
        self._strict_year_regex = RegExpUtility.get_safe_reg_exp(JapaneseDateTime.StrictYearRegex)
        self._year_in_cjk_regex = RegExpUtility.get_safe_reg_exp(JapaneseDateTime.DatePeriodYearInCJKRegex)
        self._simple_cases_regex = RegExpUtility.get_safe_reg_exp(JapaneseDateTime.SimpleCasesRegex)
        self._year_and_month = RegExpUtility.get_safe_reg_exp(JapaneseDateTime.YearAndMonth)
        self._pure_num_year_and_month = RegExpUtility.get_safe_reg_exp(JapaneseDateTime.PureNumYearAndMonth)
        self._simple_year_and_month = RegExpUtility.get_safe_reg_exp(JapaneseDateTime.SimpleYearAndMonth)
        self._one_word_period_regex = RegExpUtility.get_safe_reg_exp(JapaneseDateTime.OneWordPeriodRegex)
        self._week_of_month_regex = RegExpUtility.get_safe_reg_exp(JapaneseDateTime.WeekOfMonthRegex)
        self._week_of_year_regex = RegExpUtility.get_safe_reg_exp(JapaneseDateTime.WeekOfYearRegex)
        self._week_of_date_regex = RegExpUtility.get_safe_reg_exp(JapaneseDateTime.WeekOfDateRegex)
        self._month_of_date_regex = RegExpUtility.get_safe_reg_exp(JapaneseDateTime.MonthOfDateRegex)
        self._which_week_regex = RegExpUtility.get_safe_reg_exp(JapaneseDateTime.WhichWeekRegex)
        self._followed_unit = RegExpUtility.get_safe_reg_exp(JapaneseDateTime.FollowedUnit)
        self._number_combined_with_unit = RegExpUtility.get_safe_reg_exp(JapaneseDateTime.NumberCombinedWithUnit)
        self._year_to_year = RegExpUtility.get_safe_reg_exp(JapaneseDateTime.YearToYear)
        self._year_to_year_suffix_required = RegExpUtility.get_safe_reg_exp(
            JapaneseDateTime.YearToYearSuffixRequired)
        self._month_to_month = RegExpUtility.get_safe_reg_exp(JapaneseDateTime.MonthToMonth)
        self._month_to_month_suffix_required = RegExpUtility.get_safe_reg_exp(
            JapaneseDateTime.MonthToMonthSuffixRequired)
        self._day_to_day = RegExpUtility.get_safe_reg_exp(JapaneseDateTime.DayToDay)
        self._day_regex_for_period = RegExpUtility.get_safe_reg_exp(JapaneseDateTime.DayRegexForPeriod)
        self._month_day_range = RegExpUtility.get_safe_reg_exp(JapaneseDateTime.MonthDayRange)
        self._year_month_range = RegExpUtility.get_safe_reg_exp(JapaneseDateTime.YearMonthRange)
        self._year_month_day_range = RegExpUtility.get_safe_reg_exp(JapaneseDateTime.YearMonthDayRange)
        self._past_regex = RegExpUtility.get_safe_reg_exp(JapaneseDateTime.PastRegex)
        self._future_regex = RegExpUtility.get_safe_reg_exp(JapaneseDateTime.FutureRegex)
        self._week_with_weekday_range_regex = RegExpUtility.get_safe_reg_exp(JapaneseDateTime.WeekWithWeekDayRangeRegex)
        self._first_last_of_year_regex = RegExpUtility.get_safe_reg_exp(JapaneseDateTime.WeekWithWeekDayRangeRegex)
        self._season_with_year_regex = RegExpUtility.get_safe_reg_exp(JapaneseDateTime.SeasonWithYear)
        self._quarter_regex = RegExpUtility.get_safe_reg_exp(JapaneseDateTime.QuarterRegex)
        self._decade_regex = RegExpUtility.get_safe_reg_exp(JapaneseDateTime.DecadeRegex)
        self._century_regex = RegExpUtility.get_safe_reg_exp(JapaneseDateTime.CenturyRegex)
        self._special_month_regex = RegExpUtility.get_safe_reg_exp(JapaneseDateTime.SpecialMonthRegex)
        self._special_year_regex = RegExpUtility.get_safe_reg_exp(JapaneseDateTime.SpecialYearRegex)

        self._day_regex = RegExpUtility.get_safe_reg_exp(JapaneseDateTime.DayRegex)
        self._day_regex_in_cjk = RegExpUtility.get_safe_reg_exp(JapaneseDateTime.DatePeriodDayRegexInCJK)
        self._month_num_regex = RegExpUtility.get_safe_reg_exp(JapaneseDateTime.MonthNumRegex)
        self._this_regex = RegExpUtility.get_safe_reg_exp(JapaneseDateTime.DatePeriodThisRegex)
        self._date_unit_regex = RegExpUtility.get_safe_reg_exp(JapaneseDateTime.DateUnitRegex)
        self._last_regex = RegExpUtility.get_safe_reg_exp(JapaneseDateTime.DatePeriodLastRegex)
        self._next_regex = RegExpUtility.get_safe_reg_exp(JapaneseDateTime.DatePeriodNextRegex)
        self._relative_month_regex = RegExpUtility.get_safe_reg_exp(JapaneseDateTime.RelativeMonthRegex)
        self._later_early_period_regex = RegExpUtility.get_safe_reg_exp(JapaneseDateTime.LaterEarlyPeriodRegex)
        self._date_point_with_ago_and_later = RegExpUtility.get_safe_reg_exp(JapaneseDateTime.DatePointWithAgoAndLater)
        self._reference_date_period_regex = RegExpUtility.get_safe_reg_exp(JapaneseDateTime.ReferenceDatePeriodRegex)
        self._complex_date_period_regex = RegExpUtility.get_safe_reg_exp(JapaneseDateTime.ComplexDatePeriodRegex)
        self._month_regex = RegExpUtility.get_safe_reg_exp(JapaneseDateTime.MonthRegex)
        self._year_regex = RegExpUtility.get_safe_reg_exp(JapaneseDateTime.YearRegex)
        self._year_regex_in_number = RegExpUtility.get_safe_reg_exp(JapaneseDateTime.YearRegexInNumber)
        self._zero_to_nine_integer_regex_cjk = RegExpUtility.get_safe_reg_exp(
            JapaneseDateTime.ZeroToNineIntegerRegexCJK)
        self._month_suffix_regex = RegExpUtility.get_safe_reg_exp(JapaneseDateTime.MonthSuffixRegex)
        self._unit_regex = RegExpUtility.get_safe_reg_exp(JapaneseDateTime.UnitRegex)
        self._duration_unit_regex = RegExpUtility.get_safe_reg_exp(JapaneseDateTime.DurationUnitRegex)
        self._season_regex = JapaneseDateTime.SeasonRegex

        self._simple_cases_regexes = [
            self._simple_cases_regex, self._one_word_period_regex, self._strict_year_regex, self._year_to_year,
            self._year_to_year_suffix_required, self._month_to_month, self._day_to_day, self._year_month_range,
            self._month_day_range, self._year_month_day_range, self._month_to_month_suffix_required, self._year_and_month,
            self._pure_num_year_and_month, self._year_in_cjk_regex, self._special_month_regex, self._special_year_regex,
            self._week_of_month_regex, self._week_of_year_regex, self._week_of_date_regex, self._month_of_date_regex,
            self._which_week_regex, self._later_early_period_regex, self._season_with_year_regex, self._quarter_regex,
            self._decade_regex, self._century_regex, self._reference_date_period_regex, self._date_point_with_ago_and_later]

        self._date_point_extractor = BaseCJKDateExtractor(JapaneseDateExtractorConfiguration())
        self._duration_extractor = None
        self._integer_extractor = JapaneseNumberExtractor()
        self._ambiguity_filters_dict = JapaneseDateTime.AmbiguityFiltersDict
