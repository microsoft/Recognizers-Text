from typing import Pattern, List, Dict
import regex

from recognizers_text.utilities import RegExpUtility
from recognizers_number import BaseNumberExtractor, BaseNumberParser
from recognizers_date_time.resources.arabic_date_time import ArabicDateTime
from ..extractors import DateTimeExtractor
from recognizers_date_time.date_time.parsers import DateTimeParser
from recognizers_date_time.date_time.utilities import DateTimeUtilityConfiguration
from recognizers_date_time.date_time.base_date import DateParserConfiguration
from recognizers_date_time.date_time.base_configs import BaseDateParserConfiguration
from recognizers_date_time.date_time.arabic.date_extractor_config import ArabicDateExtractorConfiguration


class ArabicDateParserConfiguration(DateParserConfiguration):
    @property
    def date_token_prefix(self) -> str:
        return self._date_token_prefix

    @property
    def integer_extractor(self) -> BaseNumberExtractor:
        return self._integer_extractor

    @property
    def ordinal_extractor(self) -> BaseNumberExtractor:
        return self._ordinal_extractor

    @property
    def cardinal_extractor(self) -> BaseNumberExtractor:
        return self._cardinal_extractor

    @property
    def number_parser(self) -> BaseNumberParser:
        return self._number_parser

    @property
    def duration_extractor(self) -> DateTimeExtractor:
        return self._duration_extractor

    @property
    def date_extractor(self) -> DateTimeExtractor:
        return self._date_extractor

    @property
    def duration_parser(self) -> DateTimeParser:
        return self._duration_parser

    @property
    def date_regex(self) -> List[Pattern]:
        return self._date_regex

    @property
    def unit_map(self) -> Dict[str, str]:
        return self._unit_map

    @property
    def on_regex(self) -> Pattern:
        return self._on_regex

    @property
    def special_day_regex(self) -> Pattern:
        return self._special_day_regex

    @property
    def special_day_with_num_regex(self) -> Pattern:
        return self._special_day_with_num_regex

    @property
    def next_regex(self) -> Pattern:
        return self._next_regex

    @property
    def this_regex(self) -> Pattern:
        return self._this_regex

    @property
    def last_regex(self) -> Pattern:
        return self._last_regex

    @property
    def unit_regex(self) -> Pattern:
        return self._unit_regex

    @property
    def week_day_regex(self) -> Pattern:
        return self._week_day_regex

    @property
    def month_regex(self) -> Pattern:
        return self._month_regex

    @property
    def week_day_of_month_regex(self) -> Pattern:
        return self._week_day_of_month_regex

    @property
    def for_the_regex(self) -> Pattern:
        return self._for_the_regex

    @property
    def week_day_and_day_of_month_regex(self) -> Pattern:
        return self._week_day_and_day_of_month_regex

    @property
    def week_day_and_day_regex(self) -> Pattern:
        return self._week_day_and_day_regex

    @property
    def relative_month_regex(self) -> Pattern:
        return self._relative_month_regex

    @property
    def strict_relative_regex(self) -> Pattern:
        return self._strict_relative_regex

    @property
    def year_suffix(self) -> Pattern:
        return self._year_suffix

    @property
    def relative_week_day_regex(self) -> Pattern:
        return self._relative_week_day_regex

    @property
    def relative_day_regex(self) -> Pattern:
        return self._relative_day_regex

    @property
    def next_prefix_regex(self) -> Pattern:
        return self._next_prefix_regex

    @property
    def previous_prefix_regex(self) -> Pattern:
        return self._previous_prefix_regex

    @property
    def upcoming_prefix_regex(self) -> Pattern:
        return self._upcoming_prefix_regex

    @property
    def past_prefix_regex(self) -> Pattern:
        return self._past_prefix_regex

    @property
    def before_after_regex(self) -> Pattern:
        return self._before_after_regex

    @property
    def tasks_mode_duration_to_date_patterns(self) -> Pattern:
        return self._tasks_mode_duration_to_date_patterns

    @property
    def day_of_month(self) -> Dict[str, int]:
        return self._day_of_month

    @property
    def day_of_week(self) -> Dict[str, int]:
        return self._day_of_week

    @property
    def month_of_year(self) -> Dict[str, int]:
        return self._month_of_year

    @property
    def cardinal_map(self) -> Dict[str, int]:
        return self._cardinal_map

    @property
    def same_day_terms(self) -> List[str]:
        return self._same_day_terms

    @property
    def plus_one_day_terms(self) -> List[str]:
        return self._plus_one_day_terms

    @property
    def minus_one_day_terms(self) -> List[str]:
        return self._minus_one_day_terms

    @property
    def plus_two_day_terms(self) -> List[str]:
        return self._plus_two_day_terms

    @property
    def minus_two_day_terms(self) -> List[str]:
        return self._minus_two_day_terms

    @property
    def check_both_before_after(self) -> bool:
        return self._check_both_before_after

    @property
    def utility_configuration(self) -> DateTimeUtilityConfiguration:
        return self._utility_configuration

    def __init__(self, config: BaseDateParserConfiguration):
        self._date_token_prefix = ArabicDateTime.DateTokenPrefix

        self._integer_extractor = config.integer_extractor
        self._ordinal_extractor = config.ordinal_extractor
        self._cardinal_extractor = config.cardinal_extractor
        self._number_parser = config.number_parser
        self._date_extractor = config.date_extractor
        self._duration_extractor = config.duration_extractor
        self._duration_parser = config.duration_parser

        self._date_regex = ArabicDateExtractorConfiguration().date_regex_list

        self._month_of_year = config.month_of_year
        self._day_of_month = config.day_of_month
        self._day_of_week = config.day_of_week
        self._unit_map = config.unit_map
        self._cardinal_map = config.cardinal_map

        self._on_regex = RegExpUtility.get_safe_reg_exp(ArabicDateTime.OnRegex)
        self._special_day_regex = RegExpUtility.get_safe_reg_exp(
            ArabicDateTime.SpecialDayRegex)
        self._special_day_with_num_regex = RegExpUtility.get_safe_reg_exp(
            ArabicDateTime.SpecialDayWithNumRegex)
        self._next_regex = RegExpUtility.get_safe_reg_exp(
            ArabicDateTime.NextDateRegex)
        self._last_regex = RegExpUtility.get_safe_reg_exp(
            ArabicDateTime.LastDateRegex)
        self._this_regex = RegExpUtility.get_safe_reg_exp(
            ArabicDateTime.ThisRegex)
        self._unit_regex = RegExpUtility.get_safe_reg_exp(
            ArabicDateTime.DateUnitRegex)
        self._month_regex = RegExpUtility.get_safe_reg_exp(
            ArabicDateTime.MonthRegex)
        self._week_day_regex = RegExpUtility.get_safe_reg_exp(
            ArabicDateTime.WeekDayRegex)
        self._week_day_of_month_regex = RegExpUtility.get_safe_reg_exp(
            ArabicDateTime.WeekDayOfMonthRegex)
        self._for_the_regex = RegExpUtility.get_safe_reg_exp(
            ArabicDateTime.ForTheRegex)
        self._week_day_and_day_of_month_regex = RegExpUtility.get_safe_reg_exp(
            ArabicDateTime.WeekDayAndDayOfMonthRegex)
        self._week_day_and_day_regex = RegExpUtility.get_safe_reg_exp(
            ArabicDateTime.WeekDayAndDayRegex)
        self._relative_month_regex = RegExpUtility.get_safe_reg_exp(
            ArabicDateTime.RelativeMonthRegex)
        self._strict_relative_regex = RegExpUtility.get_safe_reg_exp(ArabicDateTime.StrictRelativeRegex)
        self._year_suffix = RegExpUtility.get_safe_reg_exp(ArabicDateTime.YearSuffix)
        self._relative_week_day_regex = RegExpUtility.get_safe_reg_exp(
            ArabicDateTime.RelativeWeekDayRegex)
        self._before_after_regex = RegExpUtility.get_safe_reg_exp(ArabicDateTime.BeforeAfterRegex)

        self._utility_configuration = config.utility_configuration
        self._date_token_prefix = ArabicDateTime.DateTokenPrefix
        self._check_both_before_after = ArabicDateTime.CheckBothBeforeAfter

        self._tasks_mode_duration_to_date_patterns = None

        self._past_prefix_regex = RegExpUtility.get_safe_reg_exp(ArabicDateTime.PastPrefixRegex)
        self._upcoming_prefix_regex = RegExpUtility.get_safe_reg_exp(ArabicDateTime.UpcomingPrefixRegex)
        self._previous_prefix_regex = RegExpUtility.get_safe_reg_exp(ArabicDateTime.PreviousPrefixRegex)
        self._next_prefix_regex = RegExpUtility.get_safe_reg_exp(ArabicDateTime.NextPrefixRegex)
        self._relative_day_regex = RegExpUtility.get_safe_reg_exp(ArabicDateTime.RelativeDayRegex)

        self._minus_two_day_terms = ArabicDateTime.MinusTwoDayTerms
        self._plus_two_day_terms = ArabicDateTime.PlusTwoDayTerms
        self._minus_one_day_terms = ArabicDateTime.MinusOneDayTerms
        self._plus_one_day_terms = ArabicDateTime.PlusOneDayTerms
        self._same_day_terms = ArabicDateTime.SameDayTerms

    def get_swift_month_or_year(self, source: str) -> int:
        trimmed_text = source.strip().lower()
        swift = 0

        if regex.search(self._next_prefix_regex, trimmed_text):
            swift = 1
        elif regex.search(self._previous_prefix_regex, trimmed_text):
            swift = -1

        return swift

    def is_cardinal_last(self, source: str) -> bool:
        trimmed_text = source.strip().lower()
        return trimmed_text == "last"

    def normalise(self, text: str) -> str:
        return text
