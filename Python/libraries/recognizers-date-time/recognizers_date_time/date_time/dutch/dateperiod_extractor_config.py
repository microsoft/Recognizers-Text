#  Copyright (c) Microsoft Corporation. All rights reserved.
#  Licensed under the MIT License.

from typing import List, Pattern

from recognizers_text.utilities import RegExpUtility
from recognizers_number.number import BaseNumberParser
from recognizers_number.number.Dutch.extractors import DutchIntegerExtractor, DutchCardinalExtractor
from recognizers_number.number.Dutch.parsers import DutchNumberParserConfiguration
from ...resources.base_date_time import BaseDateTime
from ...resources.dutch_date_time import DutchDateTime
from ..extractors import DateTimeExtractor
from ..base_duration import BaseDurationExtractor
from ..base_date import BaseDateExtractor
from ..base_dateperiod import DatePeriodExtractorConfiguration, MatchedIndex
from .duration_extractor_config import DutchDurationExtractorConfiguration
from .date_extractor_config import DutchDateExtractorConfiguration
from recognizers_text.extractor import Extractor
from recognizers_number import DutchOrdinalExtractor, BaseNumberExtractor, DutchCardinalExtractor


class DutchDatePeriodExtractorConfiguration(DatePeriodExtractorConfiguration):
    @property
    def previous_prefix_regex(self) -> Pattern:
        return self._previous_prefix_regex

    @property
    def check_both_before_after(self) -> bool:
        return self._check_both_before_after

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
    def decade_with_century_regex(self) -> Pattern:
        return self._decade_with_century_regex

    @property
    def future_regex(self) -> Pattern:
        return self._future_regex

    @property
    def week_of_regex(self) -> Pattern:
        return self._week_of_regex

    @property
    def month_of_regex(self) -> Pattern:
        return self._month_of_regex

    @property
    def date_unit_regex(self) -> Pattern:
        return self._date_unit_regex

    @property
    def in_connector_regex(self) -> Pattern:
        return self._in_connector_regex

    @property
    def range_unit_regex(self) -> Pattern:
        return self._range_unit_regex

    @property
    def date_point_extractor(self) -> DateTimeExtractor:
        return self._date_point_extractor

    @property
    def integer_extractor(self) -> BaseNumberExtractor:
        return self._integer_extractor

    @property
    def number_parser(self) -> BaseNumberParser:
        return self._number_parser

    @property
    def duration_extractor(self) -> DateTimeExtractor:
        return self._duration_extractor

    @property
    def now_regex(self) -> Pattern:
        return self._now_regex

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
    def month_num_regex(self) -> Pattern:
        return self._month_num_regex

    @property
    def century_suffix_regex(self) -> Pattern:
        return self._century_suffix_regex

    @property
    def ordinal_extractor(self) -> BaseNumberExtractor:
        return self._ordinal_extractor

    @property
    def cardinal_extractor(self) -> Extractor:
        return self._cardinal_extractor

    @property
    def time_unit_regex(self) -> Pattern:
        return self._time_unit_regex

    @property
    def within_next_prefix_regex(self) -> Pattern:
        return self._within_next_prefix_regex

    @property
    def range_connector_regex(self) -> Pattern:
        return self._range_connector_regex

    @property
    def day_regex(self) -> Pattern:
        return self._day_regex

    @property
    def week_day_regex(self) -> Pattern:
        return self._week_day_regex

    @property
    def relative_month_regex(self) -> Pattern:
        return self._relative_month_regex

    @property
    def month_suffix_regex(self) -> Pattern:
        return self._month_suffix_regex

    @property
    def past_prefix_regex(self) -> Pattern:
        return self._past_prefix_regex

    @property
    def next_prefix_regex(self) -> Pattern:
        return self._next_prefix_regex

    @property
    def this_prefix_regex(self) -> Pattern:
        return self._this_prefix_regex

    @property
    def which_week_regex(self) -> Pattern:
        return self._which_week_regex

    @property
    def rest_of_date_regex(self) -> Pattern:
        return self._rest_of_date_regex

    @property
    def complex_date_period_regex(self) -> Pattern:
        return self._complex_date_period_regex

    @property
    def week_day_of_month_regex(self) -> Pattern:
        return self._week_day_of_month_regex

    @property
    def all_half_year_regex(self) -> Pattern:
        return self._all_half_year_regex

    @property
    def week_of_month_regex(self) -> Pattern:
        return self._week_of_month_regex

    @property
    def written_month_regex(self) -> Pattern:
        return self._written_month_regex

    @property
    def first_last_regex(self) -> Pattern:
        return self._first_last_regex

    @property
    def between_token_regex(self) -> Pattern:
        return self._between_token_regex

    def __init__(self):
        self._all_half_year_regex = RegExpUtility.get_safe_reg_exp(DutchDateTime.AllHalfYearRegex)
        self._week_day_of_month_regex = RegExpUtility.get_safe_reg_exp(DutchDateTime.WeekDayOfMonthRegex)
        self._complex_date_period_regex = RegExpUtility.get_safe_reg_exp(DutchDateTime.ComplexDatePeriodRegex)
        self._rest_of_date_regex = RegExpUtility.get_safe_reg_exp(DutchDateTime.RestOfDateRegex)
        self._which_week_regex = RegExpUtility.get_safe_reg_exp(DutchDateTime.WhichWeekRegex)
        self._this_prefix_regex = RegExpUtility.get_safe_reg_exp(DutchDateTime.ThisPrefixRegex)
        self._next_prefix_regex = RegExpUtility.get_safe_reg_exp(DutchDateTime.NextPrefixRegex)
        self._past_prefix_regex = RegExpUtility.get_safe_reg_exp(DutchDateTime.PastSuffixRegex)
        self._month_suffix_regex = RegExpUtility.get_safe_reg_exp(DutchDateTime.MonthSuffixRegex)
        self._relative_month_regex = RegExpUtility.get_safe_reg_exp(DutchDateTime.RelativeMonthRegex)
        self._written_month_regex = RegExpUtility.get_safe_reg_exp(DutchDateTime.WrittenMonthRegex)
        self._week_day_regex = RegExpUtility.get_safe_reg_exp(DutchDateTime.WeekDayRegex)
        self._day_regex = RegExpUtility.get_safe_reg_exp(DutchDateTime.DayRegex)
        self._range_connector_regex = RegExpUtility.get_safe_reg_exp(DutchDateTime.RangeConnectorRegex)
        self._time_unit_regex = RegExpUtility.get_safe_reg_exp(DutchDateTime.TimeUnitRegex)
        self._first_last_regex = RegExpUtility.get_safe_reg_exp(DutchDateTime.FirstLastRegex)
        self.between_token_regex = RegExpUtility.get_safe_reg_exp(DutchDateTime.BetweenTokenRegex)
        self._previous_prefix_regex = RegExpUtility.get_safe_reg_exp(
            DutchDateTime.PastSuffixRegex)
        self._check_both_before_after = DutchDateTime.CheckBothBeforeAfter
        self._week_of_month_regex = DutchDateTime.WeekOfMonthRegex
        self._simple_cases_regexes = [
            RegExpUtility.get_safe_reg_exp(DutchDateTime.SimpleCasesRegex),
            RegExpUtility.get_safe_reg_exp(DutchDateTime.BetweenRegex),
            RegExpUtility.get_safe_reg_exp(DutchDateTime.OneWordPeriodRegex),
            RegExpUtility.get_safe_reg_exp(DutchDateTime.MonthWithYear),
            RegExpUtility.get_safe_reg_exp(DutchDateTime.MonthNumWithYear),
            RegExpUtility.get_safe_reg_exp(DutchDateTime.YearRegex),
            RegExpUtility.get_safe_reg_exp(DutchDateTime.WeekOfMonthRegex),
            RegExpUtility.get_safe_reg_exp(DutchDateTime.WeekOfYearRegex),
            RegExpUtility.get_safe_reg_exp(
                DutchDateTime.MonthFrontBetweenRegex),
            RegExpUtility.get_safe_reg_exp(
                DutchDateTime.MonthFrontSimpleCasesRegex),
            RegExpUtility.get_safe_reg_exp(DutchDateTime.QuarterRegex),
            RegExpUtility.get_safe_reg_exp(
                DutchDateTime.QuarterRegexYearFront),
            RegExpUtility.get_safe_reg_exp(DutchDateTime.AllHalfYearRegex),
            RegExpUtility.get_safe_reg_exp(DutchDateTime.SeasonRegex),
            RegExpUtility.get_safe_reg_exp(DutchDateTime.WhichWeekRegex),
            RegExpUtility.get_safe_reg_exp(DutchDateTime.RestOfDateRegex),
            RegExpUtility.get_safe_reg_exp(
                DutchDateTime.LaterEarlyPeriodRegex),
            RegExpUtility.get_safe_reg_exp(
                DutchDateTime.WeekWithWeekDayRangeRegex),
            RegExpUtility.get_safe_reg_exp(DutchDateTime.YearPlusNumberRegex),
            RegExpUtility.get_safe_reg_exp(DutchDateTime.DecadeWithCenturyRegex),
            RegExpUtility.get_safe_reg_exp(DutchDateTime.RelativeDecadeRegex),
            RegExpUtility.get_safe_reg_exp(DutchDateTime.ReferenceDatePeriodRegex)
        ]
        self._check_both_before_after = DutchDateTime.CheckBothBeforeAfter
        self._illegal_year_regex = RegExpUtility.get_safe_reg_exp(
            BaseDateTime.IllegalYearRegex)
        self._year_regex = RegExpUtility.get_safe_reg_exp(
            DutchDateTime.YearRegex)
        self._till_regex = RegExpUtility.get_safe_reg_exp(
            DutchDateTime.TillRegex)
        self._followed_unit = RegExpUtility.get_safe_reg_exp(
            DutchDateTime.FollowedDateUnit)
        self._number_combined_with_unit = RegExpUtility.get_safe_reg_exp(
            DutchDateTime.NumberCombinedWithDateUnit)
        self._past_regex = RegExpUtility.get_safe_reg_exp(
            DutchDateTime.PastSuffixRegex)
        self._future_regex = RegExpUtility.get_safe_reg_exp(
            DutchDateTime.NextPrefixRegex)
        self._week_of_regex = RegExpUtility.get_safe_reg_exp(
            DutchDateTime.WeekOfRegex)
        self._month_of_regex = RegExpUtility.get_safe_reg_exp(
            DutchDateTime.MonthOfRegex)
        self._date_unit_regex = RegExpUtility.get_safe_reg_exp(
            DutchDateTime.DateUnitRegex)
        self._within_next_prefix_regex = RegExpUtility.get_safe_reg_exp(
            DutchDateTime.WithinNextPrefixRegex)
        self._in_connector_regex = RegExpUtility.get_safe_reg_exp(
            DutchDateTime.InConnectorRegex)
        self._range_unit_regex = RegExpUtility.get_safe_reg_exp(
            DutchDateTime.RangeUnitRegex)

        self.from_regex = RegExpUtility.get_safe_reg_exp(
            DutchDateTime.FromRegex)
        self.before_regex = RegExpUtility.get_safe_reg_exp(
            DutchDateTime.BeforeRegex)

        self._date_point_extractor = BaseDateExtractor(
            DutchDateExtractorConfiguration())
        self._integer_extractor = DutchIntegerExtractor()
        self._number_parser = BaseNumberParser(
            DutchNumberParserConfiguration())
        self._duration_extractor = BaseDurationExtractor(
            DutchDurationExtractorConfiguration())
        self._now_regex = RegExpUtility.get_safe_reg_exp(
            DutchDateTime.NowRegex)
        self._future_suffix_regex = RegExpUtility.get_safe_reg_exp(
            DutchDateTime.FutureSuffixRegex
        )
        self._ago_regex = RegExpUtility.get_safe_reg_exp(
            DutchDateTime.AgoRegex
        )
        self._later_regex = RegExpUtility.get_safe_reg_exp(
            DutchDateTime.LaterRegex
        )
        self._less_than_regex = RegExpUtility.get_safe_reg_exp(
            DutchDateTime.LessThanRegex
        )
        self._more_than_regex = RegExpUtility.get_safe_reg_exp(
            DutchDateTime.MoreThanRegex
        )
        self._duration_date_restrictions = DutchDateTime.DurationDateRestrictions
        self._year_period_regex = RegExpUtility.get_safe_reg_exp(
            DutchDateTime.YearPeriodRegex
        )
        self._month_num_regex = RegExpUtility.get_safe_reg_exp(
            DutchDateTime.MonthNumRegex
        )
        self._century_suffix_regex = RegExpUtility.get_safe_reg_exp(
            DutchDateTime.CenturySuffixRegex
        )
        self._decade_with_century_regex = RegExpUtility.get_safe_reg_exp(
            DutchDateTime.DecadeWithCenturyRegex
        )
        self._ordinal_extractor = DutchOrdinalExtractor()
        self._cardinal_extractor = DutchCardinalExtractor()
        self._previous_prefix_regex = RegExpUtility.get_safe_reg_exp(
            DutchDateTime.PreviousPrefixRegex
        )
        self._cardinal_extractor = DutchCardinalExtractor()
        self._time_unit_regex = RegExpUtility.get_safe_reg_exp(DutchDateTime.TimeUnitRegex)

    def get_from_token_index(self, source: str) -> MatchedIndex:
        match = self.from_regex.search(source)
        if match:
            return MatchedIndex(True, match.start())

        return MatchedIndex(False, -1)

    def get_between_token_index(self, source: str) -> MatchedIndex:
        match = self.before_regex.search(source)
        if match:
            return MatchedIndex(True, match.start())

        return MatchedIndex(False, -1)

    def has_connector_token(self, source: str) -> bool:
        return not self.range_connector_regex.search(source) is None
