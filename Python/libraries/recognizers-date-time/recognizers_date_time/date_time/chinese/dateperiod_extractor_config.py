from typing import List, Pattern

from recognizers_text import Extractor, Parser, RegExpUtility
from recognizers_number import ChineseNumberExtractor, ChineseNumberParserConfiguration, BaseNumberParser, \
    ChineseCardinalExtractor, ChineseOrdinalExtractor
from ...resources.base_date_time import BaseDateTime
from ...resources.chinese_date_time import ChineseDateTime
from ..extractors import DateTimeExtractor
from ..base_dateperiod import DatePeriodExtractorConfiguration, MatchedIndex
from .date_extractor import ChineseDateExtractor


class ChineseDatePeriodExtractorConfiguration(DatePeriodExtractorConfiguration):
    @property
    def previous_prefix_regex(self) -> Pattern:
        return self._previous_prefix_regex

    @property
    def check_both_before_after(self) -> Pattern:
        return self._check_both_before_after

    @property
    def time_unit_regex(self) -> Pattern:
        return self._time_unit_regex

    @property
    def ordinal_extractor(self) -> Extractor:
        return self._ordinal_extractor

    @property
    def cardinal_extractor(self) -> Extractor:
        return self._cardinal_extractor

    @property
    def within_next_prefix_regex(self) -> Pattern:
        return self._within_next_prefix_regex

    @property
    def future_suffix_regex(self) -> Pattern:
        return self._future_suffix_regex

    @property
    def ago_regex(self) -> Pattern:
        return self._ago_regex

    @property
    def later_regex(self) -> Pattern:
        return self._later_regex

    @property
    def less_than_regex(self) -> Pattern:
        return self._less_than_regex

    @property
    def more_than_regex(self) -> Pattern:
        return self._more_than_regex

    @property
    def duration_date_restrictions(self) -> [str]:
        return self._duration_date_restrictions

    @property
    def year_period_regex(self) -> Pattern:
        return self._year_period_regex

    @property
    def century_suffix_regex(self) -> Pattern:
        return self._century_suffix_regex

    @property
    def month_num_regex(self) -> Pattern:
        return self._month_num_regex

    @property
    def simple_cases_regexes(self) -> List[Pattern]:
        return self._simple_cases_regexes

    @property
    def illegal_year_regex(self) -> Pattern:
        return self._illegal_year_regex

    @property
    def year_regex(self) -> Pattern:
        return self._year_regex

    @property
    def till_regex(self) -> Pattern:
        return self._till_regex

    @property
    def followed_unit(self) -> Pattern:
        return self._followed_unit

    @property
    def number_combined_with_unit(self) -> Pattern:
        return self._number_combined_with_unit

    @property
    def past_regex(self) -> Pattern:
        return self._past_regex

    @property
    def future_regex(self) -> Pattern:
        return self._future_regex

    @property
    def date_point_extractor(self) -> DateTimeExtractor:
        return self._date_point_extractor

    @property
    def integer_extractor(self) -> Extractor:
        return self._integer_extractor

    @property
    def number_parser(self) -> Parser:
        return self._number_parser

    @property
    def now_regex(self) -> Pattern:
        return self._now_regex

    @property
    def day_regex(self):
        return self._day_regex

    @property
    def day_regex_in_chinese(self) -> Pattern:
        return self._day_regex_in_chinese

    @property
    def relative_month_regex(self) -> Pattern:
        return self._relative_month_regex

    @property
    def zero_to_nine_integer_regex_chinese(self) -> Pattern:
        return self._zero_to_nine_integer_regex_chinese

    @property
    def month_regex(self) -> Pattern:
        return self._month_regex

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
    def strict_year_regex(self) -> Pattern:
        return self._strict_year_regex

    @property
    def year_regex_in_number(self) -> Pattern:
        return self._year_regex_in_number

    @property
    def month_suffix_regex(self) -> Pattern:
        return self._month_suffix_regex

    @property
    def season_regex(self) -> Pattern:
        return self._season_regex

    @property
    def week_of_regex(self) -> Pattern:
        return None

    @property
    def month_of_regex(self) -> Pattern:
        return None

    @property
    def date_unit_regex(self) -> Pattern:
        return None

    @property
    def in_connector_regex(self) -> Pattern:
        return None

    @property
    def range_unit_regex(self) -> Pattern:
        return None

    @property
    def duration_extractor(self) -> DateTimeExtractor:
        return None

    @property
    def range_connector_regex(self) -> Pattern:
        return None

    def __init__(self):
        self._season_regex = RegExpUtility.get_safe_reg_exp(
            ChineseDateTime.SeasonRegex
        )
        self._month_suffix_regex = RegExpUtility.get_safe_reg_exp(
            ChineseDateTime.MonthSuffixRegex
        )
        self._year_regex_in_number = RegExpUtility.get_safe_reg_exp(
            ChineseDateTime.YearRegexInNumber
        )
        self._strict_year_regex = RegExpUtility.get_safe_reg_exp(
            ChineseDateTime.StrictYearRegex
        )
        self._last_regex = RegExpUtility.get_safe_reg_exp(
            ChineseDateTime.DatePeriodLastRegex
        )
        self._next_regex = RegExpUtility.get_safe_reg_exp(
            ChineseDateTime.DatePeriodNextRegex
        )
        self._this_regex = RegExpUtility.get_safe_reg_exp(
            ChineseDateTime.DatePeriodThisRegex
        )
        self._month_regex = RegExpUtility.get_safe_reg_exp(
            ChineseDateTime.MonthRegex
        )
        self._zero_to_nine_integer_regex_chinese = RegExpUtility.get_safe_reg_exp(
            ChineseDateTime.ZeroToNineIntegerRegexChs
        )
        self._relative_month_regex = RegExpUtility.get_safe_reg_exp(
            ChineseDateTime.RelativeMonthRegex
        )
        self._day_regex_in_chinese = RegExpUtility.get_safe_reg_exp(
            ChineseDateTime.DatePeriodDayRegexInChinese
        )
        self._day_regex = RegExpUtility.get_safe_reg_exp(
            ChineseDateTime.DayRegex
        )
        self._simple_cases_regexes = [
            RegExpUtility.get_safe_reg_exp(ChineseDateTime.SimpleCasesRegex),
            RegExpUtility.get_safe_reg_exp(ChineseDateTime.OneWordPeriodRegex),
            RegExpUtility.get_safe_reg_exp(ChineseDateTime.StrictYearRegex),
            RegExpUtility.get_safe_reg_exp(ChineseDateTime.YearToYear),
            RegExpUtility.get_safe_reg_exp(
                ChineseDateTime.YearToYearSuffixRequired),
            RegExpUtility.get_safe_reg_exp(ChineseDateTime.MonthToMonth),
            RegExpUtility.get_safe_reg_exp(ChineseDateTime.MonthToMonthSuffixRequired),
            RegExpUtility.get_safe_reg_exp(ChineseDateTime.YearAndMonth),
            RegExpUtility.get_safe_reg_exp(
                ChineseDateTime.PureNumYearAndMonth),
            RegExpUtility.get_safe_reg_exp(
                ChineseDateTime.DatePeriodYearInChineseRegex),
            RegExpUtility.get_safe_reg_exp(ChineseDateTime.WeekOfMonthRegex),
            RegExpUtility.get_safe_reg_exp(ChineseDateTime.SeasonWithYear),
            RegExpUtility.get_safe_reg_exp(ChineseDateTime.QuarterRegex),
            RegExpUtility.get_safe_reg_exp(ChineseDateTime.DecadeRegex)
        ]
        self._illegal_year_regex = RegExpUtility.get_safe_reg_exp(
            BaseDateTime.IllegalYearRegex)
        self._year_regex = RegExpUtility.get_safe_reg_exp(
            ChineseDateTime.YearRegex)
        self._till_regex = RegExpUtility.get_safe_reg_exp(
            ChineseDateTime.DatePeriodTillRegex)
        self._followed_unit = RegExpUtility.get_safe_reg_exp(
            ChineseDateTime.FollowedUnit)
        self._number_combined_with_unit = RegExpUtility.get_safe_reg_exp(
            ChineseDateTime.NumberCombinedWithUnit)
        self._past_regex = RegExpUtility.get_safe_reg_exp(
            ChineseDateTime.PastRegex)
        self._future_regex = RegExpUtility.get_safe_reg_exp(
            ChineseDateTime.FutureRegex)
        self._date_point_extractor = ChineseDateExtractor()
        self._integer_extractor = ChineseNumberExtractor()
        self._number_parser = BaseNumberParser(
            ChineseNumberParserConfiguration())
        self._now_regex = RegExpUtility.get_safe_reg_exp(
            ChineseDateTime.NowRegex)
        self._month_num_regex = RegExpUtility.get_safe_reg_exp(
            ChineseDateTime.MonthNumRegex)
        self._cardinal_extractor = ChineseCardinalExtractor()
        self._ordinal_extractor = ChineseOrdinalExtractor()

        # TODO When the implementation for these properties is added, change the None values to their respective Regexps
        self._previous_prefix_regex = None
        self._check_both_before_after = None
        self._century_suffix_regex = None
        self._year_period_regex = None
        self._duration_date_restrictions = None
        self._more_than_regex = None
        self._less_than_regex = None
        self._later_regex = None
        self._ago_regex = None
        self._future_suffix_regex = None
        self._within_next_prefix_regex = None
        self._time_unit_regex = None
        self._previous_prefix_regex = None

    def get_from_token_index(self, source: str) -> MatchedIndex:
        if source.endswith('从'):
            return MatchedIndex(True, source.rindex('从'))
        return MatchedIndex(False, -1)

    def get_between_token_index(self, source: str) -> MatchedIndex:
        return MatchedIndex(False, -1)

    def has_connector_token(self, source: str) -> bool:
        return False
