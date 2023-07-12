#  Copyright (c) Microsoft Corporation. All rights reserved.
#  Licensed under the MIT License.

from typing import Pattern, Dict

from recognizers_number import JapaneseIntegerExtractor, CJKNumberParser
from recognizers_text import RegExpUtility
from recognizers_date_time.date_time.CJK.base_date import BaseCJKDateExtractor, BaseCJKDateParser
from recognizers_date_time.date_time.japanese.date_extractor_config import JapaneseDateExtractorConfiguration
from recognizers_date_time.date_time.CJK.base_dateperiod import CJKDatePeriodParserConfiguration
from recognizers_date_time.date_time.japanese.dateperiod_extractor_config import \
    JapaneseDatePeriodExtractorConfiguration
from recognizers_date_time.resources.japanese_date_time import JapaneseDateTime


class JapaneseDatePeriodParserConfiguration(CJKDatePeriodParserConfiguration):

    @property
    def wom_last_regex(self) -> Pattern:
        return self._wom_last_regex

    @property
    def wom_previous_regex(self) -> Pattern:
        return self._wom_previous_regex

    @property
    def wom_next_regex(self) -> Pattern:
        return self._wom_next_regex

    @property
    def month_of_year(self) -> Dict[str, int]:
        return self._month_of_year

    @property
    def next_month_regex(self) -> Pattern:
        return self._next_month_regex

    @property
    def after_next_month_regex(self) -> Pattern:
        return self._after_next_month_regex

    @property
    def last_month_regex(self) -> Pattern:
        return self._last_month_regex

    @property
    def next_year_regex(self) -> Pattern:
        return self._next_year_regex

    @property
    def after_next_year_regex(self) -> Pattern:
        return self._after_next_year_regex

    @property
    def last_year_regex(self) -> Pattern:
        return self._last_year_regex

    @property
    def this_year_regex(self) -> Pattern:
        return self._this_year_regex

    @property
    def date_extractor(self) -> BaseCJKDateExtractor:
        return self._date_extractor

    @property
    def duration_extractor(self):
        return self._duration_extractor

    @property
    def cardinal_extractor(self):
        return self._cardinal_extractor

    @property
    def duration_parser(self):
        raise self._duration_parser

    @property
    def date_parser(self) -> BaseCJKDateParser:
        return self._date_parser

    @property
    def integer_extractor(self) -> JapaneseIntegerExtractor:
        return self._integer_extractor

    @property
    def number_parser(self) -> CJKNumberParser:
        return self._number_parser

    @property
    def dynasty_year_map(self) -> Dict[str, int]:
        return self._dynasty_year_map

    @property
    def unit_map(self) -> Dict[str, int]:
        return self._unit_map

    @property
    def cardinal_map(self) -> Dict[str, int]:
        return self._cardinal_map

    @property
    def day_of_month(self) -> Dict[str, int]:
        return self._day_of_month

    @property
    def season_map(self) -> Dict[str, int]:
        return self._season_map

    @property
    def dynasty_start_year(self) -> Pattern:
        return self._dynasty_start_year

    @property
    def token_before_date(self) -> str:
        return self._token_before_date

    @property
    def dynasty_year_regex(self) -> Pattern:
        return self._dynasty_year_regex

    @property
    def simple_cases_regex(self) -> Pattern:
        return self._simple_cases_regex

    @property
    def this_regex(self) -> Pattern:
        return self._this_regex

    @property
    def next_regex(self) -> Pattern:
        return self._next_regex

    @property
    def last_regex(self) -> Pattern:
        return self._last_regex

    @property
    def year_to_year(self) -> Pattern:
        return self._year_to_year

    @property
    def year_to_year_suffix_required(self) -> Pattern:
        return self._year_to_year_suffix_required

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
    def later_early_period_regex(self) -> Pattern:
        return self._later_early_period_regex

    @property
    def date_period_with_ago_and_later(self) -> Pattern:
        return self._date_period_with_ago_and_later

    @property
    def reference_date_period_regex(self) -> Pattern:
        return self._reference_date_period_regex

    @property
    def complex_date_period_regex(self) -> Pattern:
        return self._complex_date_period_regex

    @property
    def duration_relative_duration_unit_regex(self) -> Pattern:
        return self._duration_relative_duration_unit_regex

    @property
    def year_in_cjk_regex(self) -> Pattern:
        return self._year_in_cjk_regex

    @property
    def month_to_month(self) -> Pattern:
        return self._month_to_month

    @property
    def month_to_month_suffix_required(self) -> Pattern:
        return self._month_to_month_suffix_required

    @property
    def month_regex(self) -> Pattern:
        return self._month_regex

    @property
    def year_and_month(self) -> Pattern:
        return self._year_and_month

    @property
    def pure_num_year_and_month(self) -> Pattern:
        return self._pure_num_year_and_month

    @property
    def one_word_period_regex(self) -> Pattern:
        return self._one_word_period_regex

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
    def week_with_week_day_range_regex(self) -> Pattern:
        return self._week_with_week_day_range_regex

    @property
    def unit_regex(self) -> Pattern:
        return self._unit_regex

    @property
    def duration_unit_regex(self) -> Pattern:
        return self._duration_unit_regex

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
    def first_last_of_year_regex(self) -> Pattern:
        return self._first_last_of_year_regex

    @property
    def season_with_year(self) -> Pattern:
        return self._season_with_year

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
    def day_to_day(self) -> Pattern:
        return self._day_to_day

    @property
    def month_day_range(self) -> Pattern:
        return self._month_day_range

    @property
    def day_regex_for_period(self) -> Pattern:
        return self._day_regex_for_period

    @property
    def simple_year_and_month(self) -> Pattern:
        return self._simple_year_and_month

    @property
    def special_month_regex(self) -> Pattern:
        return self._special_month_regex

    @property
    def special_year_regex(self) -> Pattern:
        return self._special_year_regex

    @property
    def two_num_year(self) -> int:
        return self._two_num_year

    @property
    def date_point_with_ago_and_later(self) -> Pattern:
        return self.date_period_with_ago_and_later

    def __init__(self, config):
        super().__init__()
        self._integer_extractor = config.integer_extractor
        self._number_parser = config.number_parser
        self._date_extractor = config.date_extractor
        self._duration = config.duration_extractor
        self._cardinal_extractor = config.cardinal_extractor
        self._date_parser = config.date_parser

        self._dynasty_year_regex = JapaneseDateExtractorConfiguration().dynasty_year_regex
        self._dynasty_start_year = JapaneseDateExtractorConfiguration().dynasty_start_year
        self._dynasty_year_map = JapaneseDateExtractorConfiguration().dynasty_year_map
        self._simple_cases_regex = JapaneseDatePeriodExtractorConfiguration().simple_cases_regex
        self._this_regex = JapaneseDatePeriodExtractorConfiguration().this_regex
        self._next_regex = JapaneseDatePeriodExtractorConfiguration().next_regex
        self._last_regex = JapaneseDatePeriodExtractorConfiguration().last_regex

        self._year_to_year = JapaneseDatePeriodExtractorConfiguration().year_to_year
        self._year_to_year_suffix_required = JapaneseDatePeriodExtractorConfiguration().year_to_year_suffix_required
        self._year_regex = JapaneseDatePeriodExtractorConfiguration().year_regex
        self._year_in_cjk_regex = JapaneseDatePeriodExtractorConfiguration().year_in_cjk_regex
        self._month_to_month = JapaneseDatePeriodExtractorConfiguration().month_to_month
        self._month_to_month_suffix_required = JapaneseDatePeriodExtractorConfiguration().month_to_month_suffix_required
        self._day_to_day = JapaneseDatePeriodExtractorConfiguration().day_to_day
        self._month_day_range = JapaneseDatePeriodExtractorConfiguration().month_day_range
        self._day_regex_for_period = JapaneseDatePeriodExtractorConfiguration().day_regex_for_period
        self._month_regex = JapaneseDatePeriodExtractorConfiguration().month_regex
        self._special_month_regex = JapaneseDatePeriodExtractorConfiguration().special_month_regex
        self._special_year_regex = JapaneseDatePeriodExtractorConfiguration().special_year_regex
        self._year_and_month = JapaneseDatePeriodExtractorConfiguration().year_and_month
        self._pure_num_year_and_month = JapaneseDatePeriodExtractorConfiguration().pure_num_year_and_month
        self._simple_year_and_month = JapaneseDatePeriodExtractorConfiguration().simple_year_and_month
        self._one_word_period_regex = JapaneseDatePeriodExtractorConfiguration().one_word_period_regex
        self._number_combined_with_unit = JapaneseDatePeriodExtractorConfiguration().number_combined_with_unit
        self._past_regex = JapaneseDatePeriodExtractorConfiguration().past_regex
        self._future_regex = JapaneseDatePeriodExtractorConfiguration().future_regex
        self._week_with_week_day_range_regex = JapaneseDatePeriodExtractorConfiguration().week_with_weekday_range_regex
        self._unit_regex = JapaneseDatePeriodExtractorConfiguration().unit_regex
        self._duration_unit_regex = JapaneseDatePeriodExtractorConfiguration().duration_unit_regex
        self._week_of_month_regex = JapaneseDatePeriodExtractorConfiguration().week_of_month_regex
        self._week_of_year_regex = JapaneseDatePeriodExtractorConfiguration().week_of_year_regex
        self._week_of_date_regex = JapaneseDatePeriodExtractorConfiguration().week_of_date_regex
        self._month_of_date_regex = JapaneseDatePeriodExtractorConfiguration().month_of_date_regex
        self._which_week_regex = JapaneseDatePeriodExtractorConfiguration().which_week_regex
        self._first_last_of_year_regex = JapaneseDatePeriodExtractorConfiguration().first_last_of_year_regex
        self._season_with_year = JapaneseDatePeriodExtractorConfiguration().season_with_year_regex
        self._quarter_regex = JapaneseDatePeriodExtractorConfiguration().quarter_regex
        self._decade_regex = JapaneseDatePeriodExtractorConfiguration().decade_regex
        self._century_regex = JapaneseDatePeriodExtractorConfiguration().century_regex
        self._relative_regex = JapaneseDateExtractorConfiguration().relative_regex
        self._relative_month_regex = JapaneseDateExtractorConfiguration().relative_month_regex
        self._later_early_period_regex = JapaneseDatePeriodExtractorConfiguration().later_early_period_regex
        self._date_period_with_ago_and_later = JapaneseDatePeriodExtractorConfiguration().date_point_with_ago_and_later
        self._reference_date_period_regex = JapaneseDatePeriodExtractorConfiguration().reference_date_period_regex
        self._complex_date_period_regex = JapaneseDatePeriodExtractorConfiguration().complex_date_period_regex
        self._duration_relative_duration_unit_regex = JapaneseDateExtractorConfiguration().\
            duration_relative_duration_unit_regex
        self._unit_map = JapaneseDateTime.ParserConfigurationUnitMap
        self._cardinal_map = JapaneseDateTime.ParserConfigurationCardinalMap
        self._day_of_month = JapaneseDateTime.ParserConfigurationDayOfMonth
        self._season_map = JapaneseDateTime.ParserConfigurationSeasonMap
        self._month_of_year = JapaneseDateTime.ParserConfigurationMonthOfYear
        self._token_before_date = ''

        self._wom_previous_regex = RegExpUtility.get_safe_reg_exp(JapaneseDateTime.WoMPreviousRegex)
        self._wom_next_regex = RegExpUtility.get_safe_reg_exp(JapaneseDateTime.WoMNextRegex)
        self._wom_last_regex = RegExpUtility.get_safe_reg_exp(JapaneseDateTime.WoMLastRegex)
        self._next_prefix_regex = RegExpUtility.get_safe_reg_exp(JapaneseDateTime.NextPrefixRegex)
        self._after_regex = RegExpUtility.get_safe_reg_exp(JapaneseDateTime.AfterRegex)
        self._next_month_regex = RegExpUtility.get_safe_reg_exp(JapaneseDateTime.ParserConfigurationNextMonthRegex)
        self._after_next_month_regex = RegExpUtility.get_safe_reg_exp(
            JapaneseDateTime.ParserConfigurationAfterNextMonthRegex)
        self._last_month_regex = RegExpUtility.get_safe_reg_exp(JapaneseDateTime.ParserConfigurationLastMonthRegex)
        self._next_year_regex = RegExpUtility.get_safe_reg_exp(JapaneseDateTime.ParserConfigurationNextYearRegex)
        self._after_next_year_regex = RegExpUtility.get_safe_reg_exp(
            JapaneseDateTime.ParserConfigurationAfterNextYearRegex)
        self._last_year_regex = RegExpUtility.get_safe_reg_exp(JapaneseDateTime.ParserConfigurationLastYearRegex)
        self._this_year_regex = RegExpUtility.get_safe_reg_exp(JapaneseDateTime.ParserConfigurationThisYearRegex)
        self._two_num_year = int(JapaneseDateTime.TwoNumYear)
        # TODO When the implementation for these properties is added, change the None values to their respective Regexps

        self._duration_extractor = None
        self._duration_parser = None

    def to_month_number(self, month_str: str) -> int:
        return self.month_of_year[month_str] % 12 if self.month_of_year[month_str] > 12 \
            else self.month_of_year[month_str]

    def is_month_only(self, source: str) -> bool:
        trimmed_source = source.strip().lower()
        return any(trimmed_source.endswith(o) for o in JapaneseDateTime.MonthTerms)

    def is_weekend(self, source: str) -> bool:
        trimmed_source = source.strip().lower()
        return any(trimmed_source.endswith(o) for o in JapaneseDateTime.WeekendTerms)

    def is_week_only(self, source: str) -> bool:
        trimmed_source = source.strip().lower()
        return any(trimmed_source.endswith(o) for o in JapaneseDateTime.WeekTerms)

    def is_year_only(self, source: str) -> bool:
        trimmed_source = source.strip().lower()
        return any(trimmed_source.startswith(o) or trimmed_source.endswith(o) for o in JapaneseDateTime.YearTerms)

    def is_this_year(self, source: str) -> bool:
        trimmed_source = source.strip().lower()
        return any(trimmed_source == o for o in JapaneseDateTime.ThisYearTerms)

    def is_year_to_date(self, source: str) -> bool:
        trimmed_source = source.strip().lower()
        return any(trimmed_source == o for o in JapaneseDateTime.YearToDateTerms)

    def is_last_year(self, source: str) -> bool:
        trimmed_source = source.strip().lower()
        return any(trimmed_source == o for o in JapaneseDateTime.LastYearTerms)

    def is_next_year(self, source: str) -> bool:
        trimmed_source = source.strip().lower()
        return any(trimmed_source == o for o in JapaneseDateTime.NextYearTerms)

    def is_year_after_next(self, source: str) -> bool:
        trimmed_source = source.strip().lower()
        return any(trimmed_source == o for o in JapaneseDateTime.YearAfterNextTerms)

    def is_year_before_last(self, source: str) -> bool:
        trimmed_source = source.strip().lower()
        return any(trimmed_source == o for o in JapaneseDateTime.YearBeforeLastTerms)

    def get_swift_month(self, source: str) -> int:
        # Current month: 今月
        value = 0

        if self.next_month_regex.search(source):
            value = 1
        elif self.last_month_regex.search(source):
            value = -1
        elif self.after_next_month_regex.search(source):
            value = 2

        return value

    def get_swift_year(self, source: str) -> int:
        value = -10

        if self.after_next_year_regex.search(source):
            value = 2
        elif self.next_year_regex.search(source):
            value = 1
        elif self.last_year_regex.search(source):
            value = -1
        elif self.this_year_regex.search(source):
            # Current year: 今年
            value = 0

        return value
