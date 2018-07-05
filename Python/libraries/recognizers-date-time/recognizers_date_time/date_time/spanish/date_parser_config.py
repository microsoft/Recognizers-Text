from typing import Pattern, List, Dict
import regex

from recognizers_text.utilities import RegExpUtility
from recognizers_number import BaseNumberExtractor, BaseNumberParser
from ...resources.spanish_date_time import SpanishDateTime
from ..extractors import DateTimeExtractor
from ..parsers import DateTimeParser
from ..utilities import DateTimeUtilityConfiguration
from ..base_date import DateParserConfiguration
from ..base_configs import BaseDateParserConfiguration
from .date_extractor_config import SpanishDateExtractorConfiguration

class SpanishDateParserConfiguration(DateParserConfiguration):
    @property
    def ordinal_extractor(self) -> BaseNumberExtractor:
        return self._ordinal_extractor

    @property
    def integer_extractor(self) -> BaseNumberExtractor:
        return self._integer_extractor

    @property
    def cardinal_extractor(self) -> BaseNumberExtractor:
        return self._cardinal_extractor

    @property
    def duration_extractor(self) -> DateTimeExtractor:
        return self._duration_extractor

    @property
    def duration_parser(self) -> DateTimeParser:
        return self._duration_parser

    @property
    def number_parser(self) -> BaseNumberParser:
        return self._number_parser

    @property
    def month_of_year(self) -> Dict[str, int]:
        return self._month_of_year

    @property
    def day_of_month(self) -> Dict[str, int]:
        return self._day_of_month

    @property
    def day_of_week(self) -> Dict[str, int]:
        return self._day_of_week

    @property
    def unit_map(self) -> Dict[str, str]:
        return self._unit_map

    @property
    def cardinal_map(self) -> Dict[str, int]:
        return self._cardinal_map

    @property
    def date_regex(self) -> List[Pattern]:
        return self._date_regex

    @property
    def on_regex(self) -> Pattern:
        return self._on_regex

    @property
    def special_day_regex(self) -> Pattern:
        return self._special_day_regex

    @property
    def next_regex(self) -> Pattern:
        return self._next_regex

    @property
    def unit_regex(self) -> Pattern:
        return self._unit_regex

    @property
    def month_regex(self) -> Pattern:
        return self._month_regex

    @property
    def week_day_regex(self) -> Pattern:
        return self._week_day_regex

    @property
    def last_regex(self) -> Pattern:
        return self._last_regex

    @property
    def this_regex(self) -> Pattern:
        return self._this_regex

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
    def relative_month_regex(self) -> Pattern:
        return self._relative_month_regex

    @property
    def relative_week_day_regex(self) -> Pattern:
        return self._relative_week_day_regex

    @property
    def utility_configuration(self) -> DateTimeUtilityConfiguration:
        return self._utility_configuration

    @property
    def date_token_prefix(self) -> str:
        return self._date_token_prefix

    # The following three regexes only used in this configuration
    # They are not used in the base parser, therefore they are not extracted
    # If the spanish date parser need the same regexes, they should be extracted
    _relative_day_regex = RegExpUtility.get_safe_reg_exp(SpanishDateTime.RelativeDayRegex)
    _next_prefix_regex = RegExpUtility.get_safe_reg_exp(SpanishDateTime.NextPrefixRegex)
    _past_prefix_regex = RegExpUtility.get_safe_reg_exp(SpanishDateTime.PastPrefixRegex)

    def __init__(self, config: BaseDateParserConfiguration):
        self._ordinal_extractor = config.ordinal_extractor
        self._integer_extractor = config.integer_extractor
        self._cardinal_extractor = config.cardinal_extractor
        self._duration_extractor = config.duration_extractor
        self._number_parser = config.number_parser
        self._duration_parser = config.duration_parser
        self._month_of_year = config.month_of_year
        self._day_of_month = config.day_of_month
        self._day_of_week = config.day_of_week
        self._unit_map = config.unit_map
        self._cardinal_map = config.cardinal_map
        self._date_regex = (SpanishDateExtractorConfiguration()).date_regex_list
        self._on_regex = RegExpUtility.get_safe_reg_exp(SpanishDateTime.OnRegex)
        self._special_day_regex = RegExpUtility.get_safe_reg_exp(SpanishDateTime.SpecialDayRegex)
        self._next_regex = RegExpUtility.get_safe_reg_exp(SpanishDateTime.NextDateRegex)
        self._unit_regex = RegExpUtility.get_safe_reg_exp(SpanishDateTime.DateUnitRegex)
        self._month_regex = RegExpUtility.get_safe_reg_exp(SpanishDateTime.MonthRegex)
        self._week_day_regex = RegExpUtility.get_safe_reg_exp(SpanishDateTime.WeekDayRegex)
        self._last_regex = RegExpUtility.get_safe_reg_exp(SpanishDateTime.LastDateRegex)
        self._this_regex = RegExpUtility.get_safe_reg_exp(SpanishDateTime.ThisRegex)
        self._week_day_of_month_regex = RegExpUtility.get_safe_reg_exp(SpanishDateTime.WeekDayOfMonthRegex)
        self._for_the_regex = RegExpUtility.get_safe_reg_exp(SpanishDateTime.ForTheRegex)
        self._week_day_and_day_of_month_regex = RegExpUtility.get_safe_reg_exp(SpanishDateTime.WeekDayAndDayOfMonthRegex)
        self._relative_month_regex = RegExpUtility.get_safe_reg_exp(SpanishDateTime.RelativeMonthRegex)
        self._relative_week_day_regex = RegExpUtility.get_safe_reg_exp(SpanishDateTime.RelativeWeekDayRegex)
        self._utility_configuration = config.utility_configuration
        self._date_token_prefix = SpanishDateTime.DateTokenPrefix

    def get_swift_day(self, source: str) -> int:
        trimmed_text = self.__normalize(source.strip().lower())
        swift = 0

        #TODO: add the relative day logic if needed. If yes, the whole method should be abstracted.
        if trimmed_text == 'hoy' or trimmed_text == 'el dia':
            swift = 0
        elif trimmed_text == 'mañana' or trimmed_text.endswith('dia siguiente') or trimmed_text.endswith('el dia de mañana') or trimmed_text.endswith('proximo dia'):
            swift = 1
        elif trimmed_text == 'ayer':
            swift = -1
        elif trimmed_text.endswith('pasado mañana') or trimmed_text.endswith('dia despues de mañana'):
            swift = 2
        elif trimmed_text.endswith('anteayer') or trimmed_text.endswith('dia antes de ayer'):
            swift = -2
        elif trimmed_text.endswith('ultimo dia'):
            swift = -1

        return swift

    def get_swift_month(self, source: str) -> int:
        trimmed_text = source.strip().lower()
        swift = 0

        if regex.search(SpanishDateParserConfiguration._next_prefix_regex, trimmed_text):
            swift = 1

        if regex.search(SpanishDateParserConfiguration._past_prefix_regex, trimmed_text):
            swift = -1

        return swift

    def is_cardinal_last(self, source: str) -> bool:
        trimmed_text = source.strip().lower()
        return not regex.search(SpanishDateParserConfiguration._past_prefix_regex, trimmed_text) is None

    def __normalize(self, source: str) -> str:
        return source.replace('á', 'a').replace('é', 'e').replace('í', 'i').replace('ó', 'o').replace('ú', 'u')
