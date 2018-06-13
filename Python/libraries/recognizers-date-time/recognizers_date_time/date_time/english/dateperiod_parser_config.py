from typing import Pattern, Dict

from recognizers_text.utilities import RegExpUtility
from ...resources.english_date_time import EnglishDateTime
from ..extractors import DateTimeExtractor
from ..parsers import DateTimeParser
from ..base_configs import BaseDateParserConfiguration
from ..base_dateperiod import DatePeriodParserConfiguration

class EnglishDatePeriodParserConfiguration(DatePeriodParserConfiguration):
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
    def past_prefix_regex(self) -> Pattern:
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

    def __init__(self, config: BaseDateParserConfiguration):
        self._date_extractor = config.date_extractor
        self._date_parser = config.date_parser
        self._duration_extractor = config.duration_extractor
        self._duration_parser = config.duration_parser
        self._month_front_between_regex = RegExpUtility.get_safe_reg_exp(EnglishDateTime.MonthFrontBetweenRegex)
        self._between_regex = RegExpUtility.get_safe_reg_exp(EnglishDateTime.BetweenRegex)
        self._month_front_simple_cases_regex = RegExpUtility.get_safe_reg_exp(EnglishDateTime.MonthFrontSimpleCasesRegex)
        self._simple_cases_regex = RegExpUtility.get_safe_reg_exp(EnglishDateTime.SimpleCasesRegex)
        self._one_word_period_regex = RegExpUtility.get_safe_reg_exp(EnglishDateTime.OneWordPeriodRegex)
        self._month_with_year = RegExpUtility.get_safe_reg_exp(EnglishDateTime.MonthWithYear)
        self._month_num_with_year = RegExpUtility.get_safe_reg_exp(EnglishDateTime.MonthNumWithYear)
        self._year_regex = RegExpUtility.get_safe_reg_exp(EnglishDateTime.YearRegex)
        self._past_regex = RegExpUtility.get_safe_reg_exp(EnglishDateTime.PastPrefixRegex)
        self._future_regex = RegExpUtility.get_safe_reg_exp(EnglishDateTime.NextPrefixRegex)
        self._in_connector_regex = config.utility_configuration.in_connector_regex
        self._week_of_month_regex = RegExpUtility.get_safe_reg_exp(EnglishDateTime.WeekOfMonthRegex)
        self._week_of_year_regex = RegExpUtility.get_safe_reg_exp(EnglishDateTime.WeekOfYearRegex)
        self._quarter_regex = RegExpUtility.get_safe_reg_exp(EnglishDateTime.QuarterRegex)
        self._quarter_regex_year_front = RegExpUtility.get_safe_reg_exp(EnglishDateTime.QuarterRegexYearFront)
        self._all_half_year_regex = RegExpUtility.get_safe_reg_exp(EnglishDateTime.AllHalfYearRegex)
        self._season_regex = RegExpUtility.get_safe_reg_exp(EnglishDateTime.SeasonRegex)
        self._week_of_regex = RegExpUtility.get_safe_reg_exp(EnglishDateTime.WeekOfRegex)
        self._month_of_regex = RegExpUtility.get_safe_reg_exp(EnglishDateTime.MonthOfRegex)
        self._which_week_regex = RegExpUtility.get_safe_reg_exp(EnglishDateTime.WhichWeekRegex)
        self._next_prefix_regex = RegExpUtility.get_safe_reg_exp(EnglishDateTime.NextPrefixRegex)
        self._past_prefix_regex = RegExpUtility.get_safe_reg_exp(EnglishDateTime.PastPrefixRegex)
        self._this_prefix_regex = RegExpUtility.get_safe_reg_exp(EnglishDateTime.ThisPrefixRegex)
        self._rest_of_date_regex = RegExpUtility.get_safe_reg_exp(EnglishDateTime.RestOfDateRegex)
        self._later_early_period_regex = RegExpUtility.get_safe_reg_exp(EnglishDateTime.LaterEarlyPeriodRegex)
        self._week_with_week_day_range_regex = RegExpUtility.get_safe_reg_exp(EnglishDateTime.WeekWithWeekDayRangeRegex)
        self._token_before_date = EnglishDateTime.TokenBeforeDate
        self._day_of_month = config.day_of_month
        self._month_of_year = config.month_of_year
        self._cardinal_map = config.cardinal_map
        self._season_map = config.season_map
        self._unit_map = config.unit_map

    def get_swift_day_or_month(self, source: str) -> int:
        trimmed_source = source.strip().lower()
        swift = 0

        if self.next_prefix_regex.search(trimmed_source):
            swift = 1
        elif self.past_prefix_regex.search(trimmed_source):
            swift = -1

        return swift

    def get_swift_year(self, source: str) -> int:
        trimmed_source = source.strip().lower()
        swift = -10

        if self.next_prefix_regex.search(trimmed_source):
            swift = 1
        elif self.past_prefix_regex.search(trimmed_source):
            swift = -1
        elif self.this_prefix_regex.search(trimmed_source):
            swift = 0

        return swift

    def is_future(self, source: str) -> bool:
        trimmed_source = source.strip().lower()
        return trimmed_source.startswith('this') or trimmed_source.startswith('next')

    def is_year_to_date(self, source: str) -> bool:
        trimmed_source = source.strip().lower()
        return trimmed_source == 'year to date'

    def is_month_to_date(self, source: str) -> bool:
        trimmed_source = source.strip().lower()
        return trimmed_source == 'month to date'

    def is_week_only(self, source: str) -> bool:
        trimmed_source = source.strip().lower()
        return trimmed_source.endswith('week')

    def is_weekend(self, source: str) -> bool:
        trimmed_source = source.strip().lower()
        return trimmed_source.endswith('weekend')

    def is_month_only(self, source: str) -> bool:
        trimmed_source = source.strip().lower()
        return trimmed_source.endswith('month')

    def is_last_cardinal(self, source: str) -> bool:
        trimmed_source = source.strip().lower()
        return trimmed_source == 'last'

    def is_year_only(self, source: str) -> bool:
        trimmed_source = source.strip().lower()
        return trimmed_source.endswith('year')
