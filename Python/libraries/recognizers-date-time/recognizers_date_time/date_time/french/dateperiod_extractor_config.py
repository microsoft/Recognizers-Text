#  Copyright (c) Microsoft Corporation. All rights reserved.
#  Licensed under the MIT License.

from typing import List, Pattern

from recognizers_text.utilities import RegExpUtility
from recognizers_number.number import BaseNumberParser
from recognizers_number.number.french.extractors import FrenchIntegerExtractor, FrenchCardinalExtractor
from recognizers_number.number.french.parsers import FrenchNumberParserConfiguration
from ...resources.base_date_time import BaseDateTime
from ...resources.french_date_time import FrenchDateTime
from ..extractors import DateTimeExtractor
from ..base_duration import BaseDurationExtractor
from ..base_date import BaseDateExtractor
from ..base_dateperiod import DatePeriodExtractorConfiguration, MatchedIndex
from .duration_extractor_config import FrenchDurationExtractorConfiguration
from .date_extractor_config import FrenchDateExtractorConfiguration
from recognizers_text.extractor import Extractor
from recognizers_number import FrenchOrdinalExtractor, BaseNumberExtractor, FrenchCardinalExtractor


class FrenchDatePeriodExtractorConfiguration(DatePeriodExtractorConfiguration):
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
    def written_month_regex(self) -> Pattern:
        return self._written_month_regex

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

    def __init__(self):
        self._all_half_year_regex = RegExpUtility.get_safe_reg_exp(FrenchDateTime.AllHalfYearRegex)
        self._week_day_of_month_regex = RegExpUtility.get_safe_reg_exp(FrenchDateTime.WeekDayOfMonthRegex)
        self._complex_date_period_regex = RegExpUtility.get_safe_reg_exp(FrenchDateTime.ComplexDatePeriodRegex)
        self._rest_of_date_regex = RegExpUtility.get_safe_reg_exp(FrenchDateTime.RestOfDateRegex)
        self._which_week_regex = RegExpUtility.get_safe_reg_exp(FrenchDateTime.WhichWeekRegex)
        self._this_prefix_regex = RegExpUtility.get_safe_reg_exp(FrenchDateTime.ThisPrefixRegex)
        self._next_prefix_regex = RegExpUtility.get_safe_reg_exp(FrenchDateTime.NextSuffixRegex)
        self._past_prefix_regex = RegExpUtility.get_safe_reg_exp(FrenchDateTime.PastSuffixRegex)
        self._month_suffix_regex = RegExpUtility.get_safe_reg_exp(FrenchDateTime.MonthSuffixRegex)
        self._written_month_regex = RegExpUtility.get_safe_reg_exp(FrenchDateTime.WrittenMonthRegex)
        self._relative_month_regex = RegExpUtility.get_safe_reg_exp(FrenchDateTime.RelativeMonthRegex)
        self._week_day_regex = RegExpUtility.get_safe_reg_exp(FrenchDateTime.WeekDayRegex)
        self._day_regex = RegExpUtility.get_safe_reg_exp(FrenchDateTime.DayRegex)
        self._range_connector_regex = RegExpUtility.get_safe_reg_exp(FrenchDateTime.RangeConnectorRegex)
        self._time_unit_regex = RegExpUtility.get_safe_reg_exp(FrenchDateTime.TimeUnitRegex)
        self._previous_prefix_regex = RegExpUtility.get_safe_reg_exp(
            FrenchDateTime.PastSuffixRegex)
        self._check_both_before_after = FrenchDateTime.CheckBothBeforeAfter
        self._simple_cases_regexes = [
            RegExpUtility.get_safe_reg_exp(FrenchDateTime.SimpleCasesRegex),
            RegExpUtility.get_safe_reg_exp(FrenchDateTime.BetweenRegex),
            RegExpUtility.get_safe_reg_exp(FrenchDateTime.OneWordPeriodRegex),
            RegExpUtility.get_safe_reg_exp(FrenchDateTime.MonthWithYear),
            RegExpUtility.get_safe_reg_exp(FrenchDateTime.MonthNumWithYear),
            RegExpUtility.get_safe_reg_exp(FrenchDateTime.YearRegex),
            RegExpUtility.get_safe_reg_exp(FrenchDateTime.YearPeriodRegex),
            RegExpUtility.get_safe_reg_exp(FrenchDateTime.WeekOfYearRegex),
            RegExpUtility.get_safe_reg_exp(FrenchDateTime.WeekDayOfMonthRegex),
            RegExpUtility.get_safe_reg_exp(
                FrenchDateTime.MonthFrontBetweenRegex),
            RegExpUtility.get_safe_reg_exp(
                FrenchDateTime.MonthFrontSimpleCasesRegex),
            RegExpUtility.get_safe_reg_exp(FrenchDateTime.QuarterRegex),
            RegExpUtility.get_safe_reg_exp(
                FrenchDateTime.QuarterRegexYearFront),
            RegExpUtility.get_safe_reg_exp(FrenchDateTime.SeasonRegex),
            RegExpUtility.get_safe_reg_exp(
                FrenchDateTime.LaterEarlyPeriodRegex),
            RegExpUtility.get_safe_reg_exp(
                FrenchDateTime.WeekWithWeekDayRangeRegex),
            RegExpUtility.get_safe_reg_exp(FrenchDateTime.YearPlusNumberRegex),
            RegExpUtility.get_safe_reg_exp(FrenchDateTime.DecadeWithCenturyRegex),
            RegExpUtility.get_safe_reg_exp(FrenchDateTime.RelativeDecadeRegex)
        ]
        self._check_both_before_after = FrenchDateTime.CheckBothBeforeAfter
        self._illegal_year_regex = RegExpUtility.get_safe_reg_exp(
            BaseDateTime.IllegalYearRegex)
        self._year_regex = RegExpUtility.get_safe_reg_exp(
            FrenchDateTime.YearRegex)
        self._till_regex = RegExpUtility.get_safe_reg_exp(
            FrenchDateTime.TillRegex)
        self._followed_unit = RegExpUtility.get_safe_reg_exp(
            FrenchDateTime.FollowedDateUnit)
        self._number_combined_with_unit = RegExpUtility.get_safe_reg_exp(
            FrenchDateTime.NumberCombinedWithDateUnit)
        self._past_regex = RegExpUtility.get_safe_reg_exp(
            FrenchDateTime.PastSuffixRegex)
        self._future_regex = RegExpUtility.get_safe_reg_exp(
            FrenchDateTime.NextSuffixRegex)
        self._week_of_regex = RegExpUtility.get_safe_reg_exp(
            FrenchDateTime.WeekOfRegex)
        self._month_of_regex = RegExpUtility.get_safe_reg_exp(
            FrenchDateTime.MonthOfRegex)
        self._date_unit_regex = RegExpUtility.get_safe_reg_exp(
            FrenchDateTime.DateUnitRegex)
        self._within_next_prefix_regex = RegExpUtility.get_safe_reg_exp(
            FrenchDateTime.WithinNextPrefixRegex)
        self._in_connector_regex = RegExpUtility.get_safe_reg_exp(
            FrenchDateTime.InConnectorRegex)
        self._range_unit_regex = RegExpUtility.get_safe_reg_exp(
            FrenchDateTime.RangeUnitRegex)

        self.from_regex = RegExpUtility.get_safe_reg_exp(
            FrenchDateTime.FromRegex)
        self.connector_and_regex = RegExpUtility.get_safe_reg_exp(
            FrenchDateTime.ConnectorAndRegex)
        self.before_regex = RegExpUtility.get_safe_reg_exp(
            FrenchDateTime.BeforeRegex2)

        self._date_point_extractor = BaseDateExtractor(
            FrenchDateExtractorConfiguration())
        self._integer_extractor = FrenchIntegerExtractor()
        self._number_parser = BaseNumberParser(
            FrenchNumberParserConfiguration())
        self._duration_extractor = BaseDurationExtractor(
            FrenchDurationExtractorConfiguration())
        self._now_regex = RegExpUtility.get_safe_reg_exp(
            FrenchDateTime.NowRegex)
        self._future_suffix_regex = RegExpUtility.get_safe_reg_exp(
            FrenchDateTime.FutureSuffixRegex
        )
        self._ago_regex = RegExpUtility.get_safe_reg_exp(
            FrenchDateTime.AgoRegex
        )
        self._later_regex = RegExpUtility.get_safe_reg_exp(
            FrenchDateTime.LaterRegex
        )
        self._less_than_regex = RegExpUtility.get_safe_reg_exp(
            FrenchDateTime.LessThanRegex
        )
        self._more_than_regex = RegExpUtility.get_safe_reg_exp(
            FrenchDateTime.MoreThanRegex
        )
        self._duration_date_restrictions = FrenchDateTime.DurationDateRestrictions
        self._year_period_regex = RegExpUtility.get_safe_reg_exp(
            FrenchDateTime.YearPeriodRegex
        )
        self._month_num_regex = RegExpUtility.get_safe_reg_exp(
            FrenchDateTime.MonthNumRegex
        )
        self._century_suffix_regex = RegExpUtility.get_safe_reg_exp(
            FrenchDateTime.CenturySuffixRegex
        )
        self._ordinal_extractor = FrenchOrdinalExtractor()
        self._cardinal_extractor = FrenchCardinalExtractor()
        self._previous_prefix_regex = RegExpUtility.get_safe_reg_exp(
            FrenchDateTime.PreviousPrefixRegex
        )
        self._cardinal_extractor = FrenchCardinalExtractor()
        # TODO When the implementation for these properties is added, change the None values to their respective Regexps
        self._time_unit_regex = None

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
        return not self.connector_and_regex.search(source) is None
