#  Copyright (c) Microsoft Corporation. All rights reserved.
#  Licensed under the MIT License.

from typing import List, Pattern

from recognizers_text.extractor import Extractor
from recognizers_text.utilities import RegExpUtility
from recognizers_number.number import BaseNumberParser, BaseNumberExtractor
from recognizers_number.number.portuguese.extractors import PortugueseIntegerExtractor
from recognizers_number.number.portuguese.parsers import PortugueseNumberParserConfiguration
from ...resources.base_date_time import BaseDateTime
from ...resources.portuguese_date_time import PortugueseDateTime
from ..extractors import DateTimeExtractor
from ..base_duration import BaseDurationExtractor
from ..base_date import BaseDateExtractor
from ..base_dateperiod import DatePeriodExtractorConfiguration, MatchedIndex
from .duration_extractor_config import PortugueseDurationExtractorConfiguration
from .date_extractor_config import PortugueseDateExtractorConfiguration
from .common_configs import PortugueseOrdinalExtractor, PortugueseCardinalExtractor


class PortugueseDatePeriodExtractorConfiguration(DatePeriodExtractorConfiguration):
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
            PortugueseDateTime.PreviousPrefixRegex)
        self._check_both_before_after = PortugueseDateTime.CheckBothBeforeAfter
        self._simple_cases_regexes = [
            RegExpUtility.get_safe_reg_exp(PortugueseDateTime.SimpleCasesRegex),
            RegExpUtility.get_safe_reg_exp(PortugueseDateTime.BetweenRegex),
            RegExpUtility.get_safe_reg_exp(PortugueseDateTime.OneWordPeriodRegex),
            RegExpUtility.get_safe_reg_exp(PortugueseDateTime.MonthWithYearRegex),
            RegExpUtility.get_safe_reg_exp(PortugueseDateTime.MonthNumWithYearRegex),
            RegExpUtility.get_safe_reg_exp(PortugueseDateTime.YearRegex),
            RegExpUtility.get_safe_reg_exp(PortugueseDateTime.DecadeWithCenturyRegex),
            RegExpUtility.get_safe_reg_exp(PortugueseDateTime.WeekOfMonthRegex),
            RegExpUtility.get_safe_reg_exp(PortugueseDateTime.WeekOfYearRegex),
            RegExpUtility.get_safe_reg_exp(
                PortugueseDateTime.MonthFrontBetweenRegex),
            RegExpUtility.get_safe_reg_exp(
                PortugueseDateTime.MonthFrontSimpleCasesRegex),
            RegExpUtility.get_safe_reg_exp(PortugueseDateTime.QuarterRegex),
            RegExpUtility.get_safe_reg_exp(
                PortugueseDateTime.QuarterRegexYearFront),
            RegExpUtility.get_safe_reg_exp(PortugueseDateTime.AllHalfYearRegex),
            RegExpUtility.get_safe_reg_exp(PortugueseDateTime.SeasonRegex),
            RegExpUtility.get_safe_reg_exp(PortugueseDateTime.WhichWeekRegex),
            RegExpUtility.get_safe_reg_exp(PortugueseDateTime.RestOfDateRegex),
            RegExpUtility.get_safe_reg_exp(
                PortugueseDateTime.LaterEarlyPeriodRegex),
            RegExpUtility.get_safe_reg_exp(
                PortugueseDateTime.WeekWithWeekDayRangeRegex),
            RegExpUtility.get_safe_reg_exp(PortugueseDateTime.YearPlusNumberRegex),
            RegExpUtility.get_safe_reg_exp(PortugueseDateTime.DecadeWithCenturyRegex),
            RegExpUtility.get_safe_reg_exp(PortugueseDateTime.RelativeDecadeRegex),
            RegExpUtility.get_safe_reg_exp(PortugueseDateTime.ReferenceDatePeriodRegex)
        ]
        self._check_both_before_after = PortugueseDateTime.CheckBothBeforeAfter
        self._illegal_year_regex = RegExpUtility.get_safe_reg_exp(
            BaseDateTime.IllegalYearRegex)
        self._year_regex = RegExpUtility.get_safe_reg_exp(
            PortugueseDateTime.YearRegex)
        self._till_regex = RegExpUtility.get_safe_reg_exp(
            PortugueseDateTime.TillRegex)
        self._followed_unit = RegExpUtility.get_safe_reg_exp(
            PortugueseDateTime.FollowedDateUnit)
        self._number_combined_with_unit = RegExpUtility.get_safe_reg_exp(
            PortugueseDateTime.NumberCombinedWithDateUnit)
        self._past_regex = RegExpUtility.get_safe_reg_exp(
            PortugueseDateTime.PreviousPrefixRegex)
        self._future_regex = RegExpUtility.get_safe_reg_exp(
            PortugueseDateTime.NextPrefixRegex)
        self._week_of_regex = RegExpUtility.get_safe_reg_exp(
            PortugueseDateTime.WeekOfRegex)
        self._month_of_regex = RegExpUtility.get_safe_reg_exp(
            PortugueseDateTime.MonthOfRegex)
        self._date_unit_regex = RegExpUtility.get_safe_reg_exp(
            PortugueseDateTime.DateUnitRegex)
        self._in_connector_regex = RegExpUtility.get_safe_reg_exp(
            PortugueseDateTime.InConnectorRegex)
        self._range_unit_regex = RegExpUtility.get_safe_reg_exp(
            PortugueseDateTime.RangeUnitRegex)
        self._date_point_extractor = BaseDateExtractor(
            PortugueseDateExtractorConfiguration())
        self._integer_extractor = PortugueseIntegerExtractor()
        self._number_parser = BaseNumberParser(
            PortugueseNumberParserConfiguration())
        self._duration_extractor = BaseDurationExtractor(
            PortugueseDurationExtractorConfiguration())
        self._range_connector_regex = RegExpUtility.get_safe_reg_exp(
            PortugueseDateTime.RangeConnectorRegex)
        self._now_regex = RegExpUtility.get_safe_reg_exp(
            PortugueseDateTime.NowRegex
        )
        self._within_next_prefix_regex = RegExpUtility.get_safe_reg_exp(
            PortugueseDateTime.WithinNextPrefixRegex
        )
        self._time_unit_regex = RegExpUtility.get_safe_reg_exp(
            PortugueseDateTime.TimeUnitRegex
        )
        self._future_suffix_regex = RegExpUtility.get_safe_reg_exp(
            PortugueseDateTime.FutureSuffixRegex
        )
        self._ago_regex = RegExpUtility.get_safe_reg_exp(
            PortugueseDateTime.AgoRegex
        )
        self._later_regex = RegExpUtility.get_safe_reg_exp(
            PortugueseDateTime.LaterRegex
        )
        self._less_than_regex = RegExpUtility.get_safe_reg_exp(
            PortugueseDateTime.LessThanRegex
        )
        self._more_than_regex = RegExpUtility.get_safe_reg_exp(
            PortugueseDateTime.MoreThanRegex
        )
        self._duration_date_restrictions = PortugueseDateTime.DurationDateRestrictions
        self._year_period_regex = RegExpUtility.get_safe_reg_exp(
            PortugueseDateTime.YearPeriodRegex
        )
        self._decade_with_century_regex = RegExpUtility.get_safe_reg_exp(
            PortugueseDateTime.DecadeWithCenturyRegex
        )
        self._month_num_regex = RegExpUtility.get_safe_reg_exp(
            PortugueseDateTime.MonthNumRegex
        )
        self._century_suffix_regex = RegExpUtility.get_safe_reg_exp(
            PortugueseDateTime.CenturySuffixRegex
        )
        self._ordinal_extractor = PortugueseOrdinalExtractor()
        self._previous_prefix_regex = RegExpUtility.get_safe_reg_exp(
            PortugueseDateTime.PreviousPrefixRegex
        )
        self._cardinal_extractor = PortugueseCardinalExtractor()

    def get_from_token_index(self, source: str) -> MatchedIndex:
        return MatchedIndex(True, source.rfind('from')) if source.endswith('from') else MatchedIndex(False, -1)

    def get_between_token_index(self, source: str) -> MatchedIndex:
        return MatchedIndex(True, source.rfind('between')) if source.endswith('between') else MatchedIndex(False, -1)

    def has_connector_token(self, source: str) -> bool:
        match = self.range_connector_regex.search(source)
        return len(match.group()) == len(source) if match else None
