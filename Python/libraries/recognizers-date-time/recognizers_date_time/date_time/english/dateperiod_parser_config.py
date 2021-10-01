#  Copyright (c) Microsoft Corporation. All rights reserved.
#  Licensed under the MIT License.

from typing import Pattern, Dict

from recognizers_text.utilities import RegExpUtility
from ...resources.english_date_time import EnglishDateTime
from ..extractors import DateTimeExtractor
from ..parsers import DateTimeParser
from ..base_configs import BaseDateParserConfiguration
from ..base_dateperiod import DatePeriodParserConfiguration


class EnglishDatePeriodParserConfiguration(DatePeriodParserConfiguration):
    @property
    def less_than_regex(self) -> Pattern:
        return self._less_than_regex

    @property
    def reference_date_period_regex(self) -> Pattern:
        return self._reference_date_period_regex

    @property
    def date_extractor(self) -> DateTimeExtractor:
        return self._date_extractor

    @property
    def date_parser(self) -> DateTimeParser:
        return self._date_parser

    @property
    def duration_extractor(self) -> DateTimeExtractor:
        return self._duration_extractor

    @property
    def duration_parser(self) -> DateTimeParser:
        return self._duration_parser

    @property
    def month_front_between_regex(self) -> Pattern:
        return self._month_front_between_regex

    @property
    def relative_regex(self) -> Pattern:
        return self._relative_regex

    @property
    def between_regex(self) -> Pattern:
        return self._between_regex

    @property
    def month_front_simple_cases_regex(self) -> Pattern:
        return self._month_front_simple_cases_regex

    @property
    def simple_cases_regex(self) -> Pattern:
        return self._simple_cases_regex

    @property
    def one_word_period_regex(self) -> Pattern:
        return self._one_word_period_regex

    @property
    def month_with_year(self) -> Pattern:
        return self._month_with_year

    @property
    def month_num_with_year(self) -> Pattern:
        return self._month_num_with_year

    @property
    def year_regex(self) -> Pattern:
        return self._year_regex

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
    def in_connector_regex(self) -> Pattern:
        return self._in_connector_regex

    @property
    def week_of_month_regex(self) -> Pattern:
        return self._week_of_month_regex

    @property
    def week_of_year_regex(self) -> Pattern:
        return self._week_of_year_regex

    @property
    def quarter_regex(self) -> Pattern:
        return self._quarter_regex

    @property
    def quarter_regex_year_front(self) -> Pattern:
        return self._quarter_regex_year_front

    @property
    def all_half_year_regex(self) -> Pattern:
        return self._all_half_year_regex

    @property
    def season_regex(self) -> Pattern:
        return self._season_regex

    @property
    def week_of_regex(self) -> Pattern:
        return self._week_of_regex

    @property
    def month_of_regex(self) -> Pattern:
        return self._month_of_regex

    @property
    def which_week_regex(self) -> Pattern:
        return self._which_week_regex

    @property
    def next_prefix_regex(self) -> Pattern:
        return self._next_prefix_regex

    @property
    def previous_prefix_regex(self) -> Pattern:
        return self._past_prefix_regex

    @property
    def this_prefix_regex(self) -> Pattern:
        return self._this_prefix_regex

    @property
    def rest_of_date_regex(self) -> Pattern:
        return self._rest_of_date_regex

    @property
    def later_early_period_regex(self) -> Pattern:
        return self._later_early_period_regex

    @property
    def week_with_week_day_range_regex(self) -> Pattern:
        return self._week_with_week_day_range_regex

    @property
    def unspecific_end_of_range_regex(self) -> Pattern:
        return self._unspecific_end_of_range_regex

    @property
    def token_before_date(self) -> str:
        return self._token_before_date

    @property
    def day_of_month(self) -> Dict[str, int]:
        return self._day_of_month

    @property
    def month_of_year(self) -> Dict[str, int]:
        return self._month_of_year

    @property
    def cardinal_map(self) -> Dict[str, int]:
        return self._cardinal_map

    @property
    def season_map(self) -> Dict[str, str]:
        return self._season_map

    @property
    def unit_map(self) -> Dict[str, str]:
        return self._unit_map

    @property
    def now_regex(self) -> Pattern:
        return self._now_regex

    @property
    def ago_regex(self) -> Pattern:
        return self._ago_regex

    @property
    def later_regex(self) -> Pattern:
        return self._later_regex

    @property
    def complex_dateperiod_regex(self) -> Pattern:
        return self._complex_dateperiod_regex

    @property
    def relative_decade_regex(self) -> Pattern:
        return self._relative_decade_regex

    @property
    def check_both_before_after(self) -> bool:
        return self._check_both_before_after

    def __init__(self, config: BaseDateParserConfiguration):
        self._check_both_before_after = EnglishDateTime.CheckBothBeforeAfter
        self._later_regex = RegExpUtility.get_safe_reg_exp(
            EnglishDateTime.LaterRegex)
        self._ago_regex = RegExpUtility.get_safe_reg_exp(
            EnglishDateTime.AgoRegex)
        self._date_extractor = config.date_extractor
        self._date_parser = config.date_parser
        self._duration_extractor = config.duration_extractor
        self._duration_parser = config.duration_parser
        self._month_front_between_regex = RegExpUtility.get_safe_reg_exp(
            EnglishDateTime.MonthFrontBetweenRegex)
        self._between_regex = RegExpUtility.get_safe_reg_exp(
            EnglishDateTime.BetweenRegex)
        self._month_front_simple_cases_regex = RegExpUtility.get_safe_reg_exp(
            EnglishDateTime.MonthFrontSimpleCasesRegex)
        self._simple_cases_regex = RegExpUtility.get_safe_reg_exp(
            EnglishDateTime.SimpleCasesRegex)
        self._one_word_period_regex = RegExpUtility.get_safe_reg_exp(
            EnglishDateTime.OneWordPeriodRegex)
        self._month_with_year = RegExpUtility.get_safe_reg_exp(
            EnglishDateTime.MonthWithYear)
        self._month_num_with_year = RegExpUtility.get_safe_reg_exp(
            EnglishDateTime.MonthNumWithYear)
        self._year_regex = RegExpUtility.get_safe_reg_exp(
            EnglishDateTime.YearRegex)
        self._past_regex = RegExpUtility.get_safe_reg_exp(
            EnglishDateTime.PreviousPrefixRegex)
        self._future_regex = RegExpUtility.get_safe_reg_exp(
            EnglishDateTime.NextPrefixRegex)
        self._in_connector_regex = config.utility_configuration.in_connector_regex
        self._week_of_month_regex = RegExpUtility.get_safe_reg_exp(
            EnglishDateTime.WeekOfMonthRegex)
        self._week_of_year_regex = RegExpUtility.get_safe_reg_exp(
            EnglishDateTime.WeekOfYearRegex)
        self._quarter_regex = RegExpUtility.get_safe_reg_exp(
            EnglishDateTime.QuarterRegex)
        self._quarter_regex_year_front = RegExpUtility.get_safe_reg_exp(
            EnglishDateTime.QuarterRegexYearFront)
        self._all_half_year_regex = RegExpUtility.get_safe_reg_exp(
            EnglishDateTime.AllHalfYearRegex)
        self._season_regex = RegExpUtility.get_safe_reg_exp(
            EnglishDateTime.SeasonRegex)
        self._week_of_regex = RegExpUtility.get_safe_reg_exp(
            EnglishDateTime.WeekOfRegex)
        self._month_of_regex = RegExpUtility.get_safe_reg_exp(
            EnglishDateTime.MonthOfRegex)
        self._which_week_regex = RegExpUtility.get_safe_reg_exp(
            EnglishDateTime.WhichWeekRegex)
        self._next_prefix_regex = RegExpUtility.get_safe_reg_exp(
            EnglishDateTime.NextPrefixRegex)
        self._past_prefix_regex = RegExpUtility.get_safe_reg_exp(
            EnglishDateTime.PreviousPrefixRegex)
        self._this_prefix_regex = RegExpUtility.get_safe_reg_exp(
            EnglishDateTime.ThisPrefixRegex)
        self._rest_of_date_regex = RegExpUtility.get_safe_reg_exp(
            EnglishDateTime.RestOfDateRegex)
        self._later_early_period_regex = RegExpUtility.get_safe_reg_exp(
            EnglishDateTime.LaterEarlyPeriodRegex)
        self._week_with_week_day_range_regex = RegExpUtility.get_safe_reg_exp(
            EnglishDateTime.WeekWithWeekDayRangeRegex)
        self._unspecific_end_of_range_regex = RegExpUtility.get_safe_reg_exp(
            EnglishDateTime.UnspecificEndOfRangeRegex)
        self._token_before_date = EnglishDateTime.TokenBeforeDate
        self._day_of_month = config.day_of_month
        self._month_of_year = config.month_of_year
        self._cardinal_map = config.cardinal_map
        self._season_map = config.season_map
        self._unit_map = config.unit_map
        self._now_regex = RegExpUtility.get_safe_reg_exp(
            EnglishDateTime.NowRegex)
        self._relative_regex = RegExpUtility.get_safe_reg_exp(
            EnglishDateTime.RelativeRegex)
        self._decade_with_century_regex = RegExpUtility.get_safe_reg_exp(
            EnglishDateTime.DecadeWithCenturyRegex)
        self._complex_dateperiod_regex = RegExpUtility.get_safe_reg_exp(
            EnglishDateTime.ComplexDatePeriodRegex
        )
        self._relative_decade_regex = RegExpUtility.get_safe_reg_exp(
            EnglishDateTime.RelativeDecadeRegex
        )
        self._reference_date_period_regex = RegExpUtility.get_safe_reg_exp(
            EnglishDateTime.ReferenceDatePeriodRegex
        )
        self._less_than_regex = RegExpUtility.get_safe_reg_exp(
            EnglishDateTime.LessThanRegex
        )

    def get_swift_day_or_month(self, source: str) -> int:
        trimmed_source = source.strip().lower()
        swift = 0

        if self.next_prefix_regex.search(trimmed_source):
            swift = 1
        elif self.previous_prefix_regex.search(trimmed_source):
            swift = -1

        return swift

    def get_swift_year(self, source: str) -> int:
        trimmed_source = source.strip().lower()
        swift = -10

        if self.next_prefix_regex.search(trimmed_source):
            swift = 1
        elif self.previous_prefix_regex.search(trimmed_source):
            swift = -1
        elif self.this_prefix_regex.search(trimmed_source):
            swift = 0

        return swift

    def is_future(self, source: str) -> bool:
        trimmed_source = source.strip().lower()
        return any(trimmed_source.startswith(o) for o in EnglishDateTime.FutureTerms)

    def is_year_to_date(self, source: str) -> bool:
        trimmed_source = source.strip().lower()
        return any(trimmed_source == o for o in EnglishDateTime.YearToDateTerms)

    def is_month_to_date(self, source: str) -> bool:
        trimmed_source = source.strip().lower()
        return any(trimmed_source == o for o in EnglishDateTime.MonthToDateTerms)

    def is_week_only(self, source: str) -> bool:
        trimmed_source = source.strip().lower()
        return any(trimmed_source.endswith(o) for o in EnglishDateTime.WeekTerms)

    def is_weekend(self, source: str) -> bool:
        trimmed_source = source.strip().lower()
        return any(trimmed_source.endswith(o) for o in EnglishDateTime.WeekendTerms)

    def is_month_only(self, source: str) -> bool:
        trimmed_source = source.strip().lower()
        return any(trimmed_source.endswith(o) for o in EnglishDateTime.MonthTerms)

    def is_last_cardinal(self, source: str) -> bool:
        trimmed_source = source.strip().lower()
        return any(trimmed_source == o for o in EnglishDateTime.LastCardinalTerms)

    def is_year_only(self, source: str) -> bool:
        trimmed_source = source.strip().lower()
        return any(trimmed_source.endswith(o) for o in EnglishDateTime.YearTerms) or\
            (any(trimmed_source.endswith(o) for o in EnglishDateTime.GenericYearTerms)
             and self.unspecific_end_of_range_regex.match(trimmed_source))
