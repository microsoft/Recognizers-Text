from typing import List, Pattern, Dict

from recognizers_text.utilities import RegExpUtility
from recognizers_number.number import BaseNumberParser, BaseNumberExtractor
from recognizers_number.number.english.extractors import EnglishIntegerExtractor
from recognizers_number.number.english.parsers import EnglishNumberParserConfiguration
from recognizers_date_time.date_time import DateTimeExtractor, BaseDateParser
from recognizers_date_time.date_time.base_dateperiod import DatePeriodExtractorConfiguration, DatePeriodParserConfiguration, MatchedIndex
from recognizers_date_time.date_time.base_date import BaseDateExtractor
from recognizers_date_time.date_time.base_duration import BaseDurationExtractor, BaseDurationParser
#from recognizers_date_time.date_time.english.parsers import EnglishCommonDateTimeParserConfiguration
from recognizers_date_time.date_time.english.duration_configs import EnglishDurationExtractorConfiguration
from recognizers_date_time.date_time.english.date_configs import EnglishDateExtractorConfiguration
from recognizers_date_time.resources.english_date_time import EnglishDateTime

class EnglishDatePeriodExtractorConfiguration(DatePeriodExtractorConfiguration):
    @property
    def simple_cases_regexes(self) -> List[Pattern]:
        return self._simple_cases_regexes

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
    def range_connector_regex(self) -> Pattern:
        return self._range_connector_regex

    def __init__(self):
        self._simple_cases_regexes = [
            RegExpUtility.get_safe_reg_exp(EnglishDateTime.SimpleCasesRegex),
            RegExpUtility.get_safe_reg_exp(EnglishDateTime.BetweenRegex),
            RegExpUtility.get_safe_reg_exp(EnglishDateTime.OneWordPeriodRegex),
            RegExpUtility.get_safe_reg_exp(EnglishDateTime.MonthWithYear),
            RegExpUtility.get_safe_reg_exp(EnglishDateTime.MonthNumWithYear),
            RegExpUtility.get_safe_reg_exp(EnglishDateTime.YearRegex),
            RegExpUtility.get_safe_reg_exp(EnglishDateTime.WeekOfMonthRegex),
            RegExpUtility.get_safe_reg_exp(EnglishDateTime.WeekOfYearRegex),
            RegExpUtility.get_safe_reg_exp(EnglishDateTime.MonthFrontBetweenRegex),
            RegExpUtility.get_safe_reg_exp(EnglishDateTime.MonthFrontSimpleCasesRegex),
            RegExpUtility.get_safe_reg_exp(EnglishDateTime.QuarterRegex),
            RegExpUtility.get_safe_reg_exp(EnglishDateTime.QuarterRegexYearFront),
            RegExpUtility.get_safe_reg_exp(EnglishDateTime.AllHalfYearRegex),
            RegExpUtility.get_safe_reg_exp(EnglishDateTime.SeasonRegex),
            RegExpUtility.get_safe_reg_exp(EnglishDateTime.WhichWeekRegex),
            RegExpUtility.get_safe_reg_exp(EnglishDateTime.RestOfDateRegex),
            RegExpUtility.get_safe_reg_exp(EnglishDateTime.LaterEarlyPeriodRegex),
            RegExpUtility.get_safe_reg_exp(EnglishDateTime.WeekWithWeekDayRangeRegex)
        ]
        self._year_regex = RegExpUtility.get_safe_reg_exp(EnglishDateTime.YearRegex)
        self._till_regex = RegExpUtility.get_safe_reg_exp(EnglishDateTime.TillRegex)
        self._followed_unit = RegExpUtility.get_safe_reg_exp(EnglishDateTime.FollowedDateUnit)
        self._number_combined_with_unit = RegExpUtility.get_safe_reg_exp(EnglishDateTime.NumberCombinedWithDateUnit)
        self._past_regex = RegExpUtility.get_safe_reg_exp(EnglishDateTime.PastPrefixRegex)
        self._future_regex = RegExpUtility.get_safe_reg_exp(EnglishDateTime.NextPrefixRegex)
        self._week_of_regex = RegExpUtility.get_safe_reg_exp(EnglishDateTime.WeekOfRegex)
        self._month_of_regex = RegExpUtility.get_safe_reg_exp(EnglishDateTime.MonthOfRegex)
        self._date_unit_regex = RegExpUtility.get_safe_reg_exp(EnglishDateTime.DateUnitRegex)
        self._in_connector_regex = RegExpUtility.get_safe_reg_exp(EnglishDateTime.InConnectorRegex)
        self._range_unit_regex = RegExpUtility.get_safe_reg_exp(EnglishDateTime.RangeUnitRegex)
        self._date_point_extractor = BaseDateExtractor(EnglishDateExtractorConfiguration())
        self._integer_extractor = EnglishIntegerExtractor()
        self._number_parser = BaseNumberParser(EnglishNumberParserConfiguration())
        self._duration_extractor = BaseDurationExtractor(EnglishDurationExtractorConfiguration())
        self._range_connector_regex = RegExpUtility.get_safe_reg_exp(EnglishDateTime.RangeConnectorRegex)

    def get_from_token_index(self, source: str) -> MatchedIndex:
        return MatchedIndex(True, source.rfind('from')) if source.endswith('from') else MatchedIndex(False, -1)

    def get_between_token_index(self, source: str) -> MatchedIndex:
        return MatchedIndex(True, source.rfind('between')) if source.endswith('between') else MatchedIndex(False, -1)

    def has_connector_token(self, source: str) -> bool:
        match = self.range_connector_regex.search(source)
        return len(match.group()) == len(source) if match else None

class EnglishDatePeriodParserConfiguration(DatePeriodParserConfiguration):
    @property
    def date_extractor(self) -> DateTimeExtractor:
        return self._date_extractor

    @property
    def date_parser(self) -> BaseDateParser:
        return self._date_parser

    @property
    def duration_extractor(self) -> DateTimeExtractor:
        return self._duration_extractor

    @property
    def duration_parser(self) -> BaseDurationParser:
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

    def __init__(self, config: None):
        self._date_extractor = config
        self._date_parser = config
        self._duration_extractor = config
        self._duration_parser = config
        self._month_front_between_regex = config
        self._between_regex = config
        self._month_front_simple_cases_regex = config
        self._simple_cases_regex = config
        self._one_word_period_regex = config
        self._month_with_year = config
        self._month_num_with_year = config
        self._year_regex = config
        self._past_regex = config
        self._future_regex = config
        self._in_connector_regex = config
        self._week_of_month_regex = config
        self._week_of_year_regex = config
        self._quarter_regex = config
        self._quarter_regex_year_front = config
        self._all_half_year_regex = config
        self._season_regex = config
        self._week_of_regex = config
        self._month_of_regex = config
        self._which_week_regex = config
        self._rest_of_date_regex = config
        self._later_early_period_regex = config
        self._week_with_week_day_range_regex = config
        self._token_before_date = config
        self._day_of_month = config
        self._month_of_year = config
        self._cardinal_map = config
        self._season_map = config
        self._unit_map = config

    def get_swift_day_or_month(self, source: str) -> int:
        return  None

    def get_swift_year(self, source: str) -> int:
        return  None

    def is_future(self, source: str) -> bool:
        return  None

    def is_year_to_date(self, source: str) -> bool:
        return  None

    def is_month_to_date(self, source: str) -> bool:
        return  None

    def is_week_only(self, source: str) -> bool:
        return  None

    def is_weekend(self, source: str) -> bool:
        return  None

    def is_month_only(self, source: str) -> bool:
        return  None

    def is_last_cardinal(self, source: str) -> bool:
        return  None

    def is_year_only(self, source: str) -> bool:
        return  None
