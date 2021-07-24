#  Copyright (c) Microsoft Corporation. All rights reserved.
#  Licensed under the MIT License.

from typing import List, Pattern

from recognizers_text import Extractor
from recognizers_text.utilities import RegExpUtility
from recognizers_number.number import BaseNumberParser, BaseNumberExtractor
from recognizers_number.number.spanish.extractors import SpanishIntegerExtractor,\
    SpanishCardinalExtractor, SpanishOrdinalExtractor
from recognizers_number.number.spanish.parsers import SpanishNumberParserConfiguration
from ...resources.base_date_time import BaseDateTime
from ...resources.spanish_date_time import SpanishDateTime
from ..extractors import DateTimeExtractor
from ..base_duration import BaseDurationExtractor
from ..base_date import BaseDateExtractor
from ..base_dateperiod import DatePeriodExtractorConfiguration, MatchedIndex
from .duration_extractor_config import SpanishDurationExtractorConfiguration
from .date_extractor_config import SpanishDateExtractorConfiguration


class SpanishDatePeriodExtractorConfiguration(DatePeriodExtractorConfiguration):
    @property
    def year_period_regex(self) -> Pattern:
        return self._year_period_regex

    @property
    def all_half_year_regex(self):
        return self._all_half_year_regex

    @property
    def previous_prefix_regex(self) -> Pattern:
        return self._previous_prefix_regex

    @property
    def check_both_before_after(self) -> Pattern:
        return self._check_both_before_after

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
    def simple_cases_regexes(self) -> List[Pattern]:
        return self._simple_cases_regexes

    @property
    def illegal_year_regex(self) -> Pattern:
        return self._illegal_year_regex

    @property
    def year_regex(self) -> Pattern:
        return self._year_regex

    @property
    def decade_with_century_regex(self) -> Pattern:
        return self._decade_with_century_regex

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
    def month_num_regex(self) -> Pattern:
        return self._month_num_regex

    @property
    def century_suffix_regex(self) -> Pattern:
        return self._century_suffix_regex

    def __init__(self):
        self._year_period_regex = RegExpUtility.get_safe_reg_exp(SpanishDateTime.YearPeriodRegex)
        self._previous_prefix_regex = RegExpUtility.get_safe_reg_exp(SpanishDateTime.PastRegex)
        self._simple_cases_regexes = [
            self._year_period_regex,
            RegExpUtility.get_safe_reg_exp(SpanishDateTime.SimpleCasesRegex),
            RegExpUtility.get_safe_reg_exp(SpanishDateTime.DayBetweenRegex),
            RegExpUtility.get_safe_reg_exp(SpanishDateTime.OneWordPeriodRegex),
            RegExpUtility.get_safe_reg_exp(SpanishDateTime.MonthWithYearRegex),
            RegExpUtility.get_safe_reg_exp(SpanishDateTime.MonthNumWithYearRegex),
            RegExpUtility.get_safe_reg_exp(SpanishDateTime.YearRegex),
            RegExpUtility.get_safe_reg_exp(SpanishDateTime.WeekOfMonthRegex),
            RegExpUtility.get_safe_reg_exp(SpanishDateTime.WeekOfYearRegex),
            RegExpUtility.get_safe_reg_exp(SpanishDateTime.MonthFrontBetweenRegex),
            RegExpUtility.get_safe_reg_exp(SpanishDateTime.MonthFrontSimpleCasesRegex),
            RegExpUtility.get_safe_reg_exp(SpanishDateTime.QuarterRegex),
            RegExpUtility.get_safe_reg_exp(SpanishDateTime.QuarterRegexYearFront),
            RegExpUtility.get_safe_reg_exp(SpanishDateTime.SeasonRegex),
            RegExpUtility.get_safe_reg_exp(SpanishDateTime.RestOfDateRegex),
            RegExpUtility.get_safe_reg_exp(SpanishDateTime.LaterEarlyPeriodRegex),
            RegExpUtility.get_safe_reg_exp(SpanishDateTime.WeekWithWeekDayRangeRegex),
            RegExpUtility.get_safe_reg_exp(SpanishDateTime.YearPlusNumberRegex),
            RegExpUtility.get_safe_reg_exp(SpanishDateTime.WhichWeekRegex),
            RegExpUtility.get_safe_reg_exp(SpanishDateTime.ReferenceDatePeriodRegex),
        ]
        self._check_both_before_after = SpanishDateTime.CheckBothBeforeAfter
        self._time_unit_regex = RegExpUtility.get_safe_reg_exp(
            SpanishDateTime.TimeUnitRegex)
        self._within_next_prefix_regex = RegExpUtility.get_safe_reg_exp(
            SpanishDateTime.WithinNextPrefixRegex
        )
        self._illegal_year_regex = RegExpUtility.get_safe_reg_exp(
            BaseDateTime.IllegalYearRegex)
        self._year_regex = RegExpUtility.get_safe_reg_exp(
            SpanishDateTime.YearRegex)
        self._till_regex = RegExpUtility.get_safe_reg_exp(
            SpanishDateTime.TillRegex)
        self._followed_unit = RegExpUtility.get_safe_reg_exp(
            SpanishDateTime.FollowedDateUnit)
        self._number_combined_with_unit = RegExpUtility.get_safe_reg_exp(
            SpanishDateTime.NumberCombinedWithDateUnit)
        self._past_regex = RegExpUtility.get_safe_reg_exp(
            SpanishDateTime.PastRegex)
        self._future_regex = RegExpUtility.get_safe_reg_exp(
            SpanishDateTime.FutureRegex)
        self._week_of_regex = RegExpUtility.get_safe_reg_exp(
            SpanishDateTime.WeekOfRegex)
        self._month_of_regex = RegExpUtility.get_safe_reg_exp(
            SpanishDateTime.MonthOfRegex)
        self._date_unit_regex = RegExpUtility.get_safe_reg_exp(
            SpanishDateTime.DateUnitRegex)
        self._in_connector_regex = RegExpUtility.get_safe_reg_exp(
            SpanishDateTime.InConnectorRegex)
        self._range_unit_regex = RegExpUtility.get_safe_reg_exp(
            SpanishDateTime.RangeUnitRegex)
        self._all_half_year_regex = RegExpUtility.get_safe_reg_exp(
            SpanishDateTime.AllHalfYearRegex)
        self.from_regex = RegExpUtility.get_safe_reg_exp(
            SpanishDateTime.FromRegex)
        self.range_connector_regex = RegExpUtility.get_safe_reg_exp(
            SpanishDateTime.RangeConnectorRegex)
        self.between_regex = RegExpUtility.get_safe_reg_exp(
            SpanishDateTime.BetweenRegex)

        self._date_point_extractor = BaseDateExtractor(
            SpanishDateExtractorConfiguration())
        self._integer_extractor = SpanishIntegerExtractor()
        self._number_parser = BaseNumberParser(
            SpanishNumberParserConfiguration())
        self._duration_extractor = BaseDurationExtractor(
            SpanishDurationExtractorConfiguration())
        self._now_regex = RegExpUtility.get_safe_reg_exp(
            SpanishDateTime.NowRegex)
        self._future_suffix_regex = RegExpUtility.get_safe_reg_exp(
            SpanishDateTime.FutureSuffixRegex
        )
        self._ago_regex = RegExpUtility.get_safe_reg_exp(
            SpanishDateTime.AgoRegex
        )
        self._later_regex = RegExpUtility.get_safe_reg_exp(
            SpanishDateTime.LaterRegex
        )
        self._less_than_regex = RegExpUtility.get_safe_reg_exp(
            SpanishDateTime.LessThanRegex
        )
        self._more_than_regex = RegExpUtility.get_safe_reg_exp(
            SpanishDateTime.MoreThanRegex
        )
        self._duration_date_restrictions = SpanishDateTime.DurationDateRestrictions
        self._month_num_regex = RegExpUtility.get_safe_reg_exp(
            SpanishDateTime.MonthNumRegex
        )
        self._century_suffix_regex = RegExpUtility.get_safe_reg_exp(
            SpanishDateTime.CenturySuffixRegex
        )
        self._check_both_before_after = False
        self._cardinal_extractor = SpanishCardinalExtractor()
        self._ordinal_extractor = SpanishOrdinalExtractor()
        self._decade_with_century_regex = RegExpUtility.get_safe_reg_exp(
            SpanishDateTime.DecadeWithCenturyRegex)
        self._previous_prefix_regex = RegExpUtility.get_safe_reg_exp(
            SpanishDateTime.PreviousPrefixRegex
        )

    def get_from_token_index(self, source: str) -> MatchedIndex:
        match = self.from_regex.search(source)
        if match:
            return MatchedIndex(True, match.start())

        return MatchedIndex(False, -1)

    def get_between_token_index(self, source: str) -> MatchedIndex:
        match = self.between_regex.search(source)
        if match:
            return MatchedIndex(True, match.start())

        return MatchedIndex(False, -1)

    def has_connector_token(self, source: str) -> bool:
        return not self.range_connector_regex.search(source) is None
