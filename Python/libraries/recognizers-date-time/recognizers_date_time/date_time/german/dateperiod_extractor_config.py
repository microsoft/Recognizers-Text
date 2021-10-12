#  Copyright (c) Microsoft Corporation. All rights reserved.
#  Licensed under the MIT License.

from typing import List, Pattern

from recognizers_text.extractor import Extractor
from recognizers_text.utilities import RegExpUtility
from recognizers_number.number import BaseNumberParser, BaseNumberExtractor
from recognizers_number.number.german.extractors import germanIntegerExtractor
from recognizers_number.number.german.parsers import germanNumberParserConfiguration
from ...resources.base_date_time import BaseDateTime
from ...resources.german_date_time import germanDateTime
from ..extractors import DateTimeExtractor
from ..base_duration import BaseDurationExtractor
from ..base_date import BaseDateExtractor
from ..base_dateperiod import DatePeriodExtractorConfiguration, MatchedIndex
from .duration_extractor_config import germanDurationExtractorConfiguration
from .date_extractor_config import germanDateExtractorConfiguration
from .common_configs import germanOrdinalExtractor, germanCardinalExtractor


class germanDatePeriodExtractorConfiguration(DatePeriodExtractorConfiguration):
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
    def time_unit_regex(self) -> Pattern:
        return self._time_unit_regex

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
    def range_connector_regex(self) -> Pattern:
        return self._range_connector_regex

    @property
    def ordinal_extractor(self) -> BaseNumberExtractor:
        return self._ordinal_extractor

    @property
    def cardinal_extractor(self) -> Extractor:
        return self._cardinal_extractor

    @property
    def now_regex(self) -> Pattern:
        return self._now_regex

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
    def decade_with_century_regex(self) -> Pattern:
        return self._decade_with_century_regex

    @property
    def month_num_regex(self) -> Pattern:
        return self._month_num_regex

    @property
    def century_suffix_regex(self) -> Pattern:
        return self._century_suffix_regex

    def __init__(self):
        self._previous_prefix_regex = RegExpUtility.get_safe_reg_exp(
            germanDateTime.PreviousPrefixRegex)
        self._check_both_before_after = germanDateTime.CheckBothBeforeAfter
        self._simple_cases_regexes = [
            RegExpUtility.get_safe_reg_exp(germanDateTime.SimpleCasesRegex),
            RegExpUtility.get_safe_reg_exp(germanDateTime.BetweenRegex),
            RegExpUtility.get_safe_reg_exp(germanDateTime.OneWordPeriodRegex),
            RegExpUtility.get_safe_reg_exp(germanDateTime.MonthWithYear),
            RegExpUtility.get_safe_reg_exp(germanDateTime.MonthNumWithYear),
            RegExpUtility.get_safe_reg_exp(germanDateTime.YearRegex),
            RegExpUtility.get_safe_reg_exp(germanDateTime.DecadeWithCenturyRegex),
            RegExpUtility.get_safe_reg_exp(germanDateTime.WeekOfMonthRegex),
            RegExpUtility.get_safe_reg_exp(germanDateTime.WeekOfYearRegex),
            RegExpUtility.get_safe_reg_exp(
                germanDateTime.MonthFrontBetweenRegex),
            RegExpUtility.get_safe_reg_exp(
                germanDateTime.MonthFrontSimpleCasesRegex),
            RegExpUtility.get_safe_reg_exp(germanDateTime.QuarterRegex),
            RegExpUtility.get_safe_reg_exp(
                germanDateTime.QuarterRegexYearFront),
            RegExpUtility.get_safe_reg_exp(germanDateTime.AllHalfYearRegex),
            RegExpUtility.get_safe_reg_exp(germanDateTime.SeasonRegex),
            RegExpUtility.get_safe_reg_exp(germanDateTime.WhichWeekRegex),
            RegExpUtility.get_safe_reg_exp(germanDateTime.RestOfDateRegex),
            RegExpUtility.get_safe_reg_exp(
                germanDateTime.LaterEarlyPeriodRegex),
            RegExpUtility.get_safe_reg_exp(
                germanDateTime.WeekWithWeekDayRangeRegex),
            RegExpUtility.get_safe_reg_exp(germanDateTime.YearPlusNumberRegex),
            RegExpUtility.get_safe_reg_exp(germanDateTime.DecadeWithCenturyRegex),
            RegExpUtility.get_safe_reg_exp(germanDateTime.RelativeDecadeRegex),
            RegExpUtility.get_safe_reg_exp(germanDateTime.ReferenceDatePeriodRegex)
        ]
        self._check_both_before_after = germanDateTime.CheckBothBeforeAfter
        self._illegal_year_regex = RegExpUtility.get_safe_reg_exp(
            BaseDateTime.IllegalYearRegex)
        self._year_regex = RegExpUtility.get_safe_reg_exp(
            germanDateTime.YearRegex)
        self._till_regex = RegExpUtility.get_safe_reg_exp(
            germanDateTime.TillRegex)
        self._followed_unit = RegExpUtility.get_safe_reg_exp(
            germanDateTime.FollowedDateUnit)
        self._number_combined_with_unit = RegExpUtility.get_safe_reg_exp(
            germanDateTime.NumberCombinedWithDateUnit)
        self._past_regex = RegExpUtility.get_safe_reg_exp(
            germanDateTime.PreviousPrefixRegex)
        self._future_regex = RegExpUtility.get_safe_reg_exp(
            germanDateTime.NextPrefixRegex)
        self._week_of_regex = RegExpUtility.get_safe_reg_exp(
            germanDateTime.WeekOfRegex)
        self._month_of_regex = RegExpUtility.get_safe_reg_exp(
            germanDateTime.MonthOfRegex)
        self._date_unit_regex = RegExpUtility.get_safe_reg_exp(
            germanDateTime.DateUnitRegex)
        self._in_connector_regex = RegExpUtility.get_safe_reg_exp(
            germanDateTime.InConnectorRegex)
        self._range_unit_regex = RegExpUtility.get_safe_reg_exp(
            germanDateTime.RangeUnitRegex)
        self._date_point_extractor = BaseDateExtractor(
            germanDateExtractorConfiguration())
        self._integer_extractor = germanIntegerExtractor()
        self._number_parser = BaseNumberParser(
            germanNumberParserConfiguration())
        self._duration_extractor = BaseDurationExtractor(
            germanDurationExtractorConfiguration())
        self._range_connector_regex = RegExpUtility.get_safe_reg_exp(
            germanDateTime.RangeConnectorRegex)
        self._now_regex = RegExpUtility.get_safe_reg_exp(
            germanDateTime.NowRegex
        )
        self._within_next_prefix_regex = RegExpUtility.get_safe_reg_exp(
            germanDateTime.WithinNextPrefixRegex
        )
        self._time_unit_regex = RegExpUtility.get_safe_reg_exp(
            germanDateTime.TimeUnitRegex
        )
        self._future_suffix_regex = RegExpUtility.get_safe_reg_exp(
            germanDateTime.FutureSuffixRegex
        )
        self._ago_regex = RegExpUtility.get_safe_reg_exp(
            germanDateTime.AgoRegex
        )
        self._later_regex = RegExpUtility.get_safe_reg_exp(
            germanDateTime.LaterRegex
        )
        self._less_than_regex = RegExpUtility.get_safe_reg_exp(
            germanDateTime.LessThanRegex
        )
        self._more_than_regex = RegExpUtility.get_safe_reg_exp(
            germanDateTime.MoreThanRegex
        )
        self._duration_date_restrictions = germanDateTime.DurationDateRestrictions
        self._year_period_regex = RegExpUtility.get_safe_reg_exp(
            germanDateTime.YearPeriodRegex
        )
        self._decade_with_century_regex = RegExpUtility.get_safe_reg_exp(
            germanDateTime.DecadeWithCenturyRegex
        )
        self._month_num_regex = RegExpUtility.get_safe_reg_exp(
            germanDateTime.MonthNumRegex
        )
        self._century_suffix_regex = RegExpUtility.get_safe_reg_exp(
            germanDateTime.CenturySuffixRegex
        )
        self._ordinal_extractor = germanOrdinalExtractor()
        self._previous_prefix_regex = RegExpUtility.get_safe_reg_exp(
            germanDateTime.PreviousPrefixRegex
        )
        self._cardinal_extractor = germanCardinalExtractor()

    def get_from_token_index(self, source: str) -> MatchedIndex:
        return MatchedIndex(True, source.rfind('from')) if source.endswith('from') else MatchedIndex(False, -1)

    def get_between_token_index(self, source: str) -> MatchedIndex:
        return MatchedIndex(True, source.rfind('between')) if source.endswith('between') else MatchedIndex(False, -1)

    def has_connector_token(self, source: str) -> bool:
        match = self.range_connector_regex.search(source)
        return len(match.group()) == len(source) if match else None
