from typing import Pattern, List, Dict
import regex

from recognizers_text.utilities import RegExpUtility
from recognizers_number import BaseNumberExtractor, BaseNumberParser
from ...resources.french_date_time import FrenchDateTime
from ..extractors import DateTimeExtractor
from ..parsers import DateTimeParser
from ..utilities import DateTimeUtilityConfiguration
from ..base_date import DateParserConfiguration
from ..base_configs import BaseDateParserConfiguration
from .date_extractor_config import FrenchDateExtractorConfiguration

class FrenchDateParserConfiguration(DateParserConfiguration):
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
        self._date_regex = (FrenchDateExtractorConfiguration()).date_regex_list
        self._on_regex = RegExpUtility.get_safe_reg_exp(FrenchDateTime.OnRegex)
        self._special_day_regex = RegExpUtility.get_safe_reg_exp(FrenchDateTime.SpecialDayRegex)
        self._special_day_regex_with_num_regex = RegExpUtility.get_safe_reg_exp(FrenchDateTime.SpecialDayWithNumRegex)
        self._next_regex = RegExpUtility.get_safe_reg_exp(FrenchDateTime.NextDateRegex)
        self._unit_regex = RegExpUtility.get_safe_reg_exp(FrenchDateTime.DateUnitRegex)
        self._month_regex = RegExpUtility.get_safe_reg_exp(FrenchDateTime.MonthRegex)
        self._week_day_regex = RegExpUtility.get_safe_reg_exp(FrenchDateTime.WeekDayRegex)
        self._strict_week_day = RegExpUtility.get_safe_reg_exp(FrenchDateTime.StrictWeekDay)
        self._last_regex = RegExpUtility.get_safe_reg_exp(FrenchDateTime.LastDateRegex)
        self._this_regex = RegExpUtility.get_safe_reg_exp(FrenchDateTime.ThisRegex)
        self._week_day_of_month_regex = RegExpUtility.get_safe_reg_exp(FrenchDateTime.WeekDayOfMonthRegex)
        self._for_the_regex = RegExpUtility.get_safe_reg_exp(FrenchDateTime.ForTheRegex)
        self._week_day_and_day_of_month_regex = RegExpUtility.get_safe_reg_exp(FrenchDateTime.WeekDayAndDayOfMonthRegex)
        self._relative_month_regex = RegExpUtility.get_safe_reg_exp(FrenchDateTime.RelativeMonthRegex)
        self._relative_week_day_regex = RegExpUtility.get_safe_reg_exp(FrenchDateTime.RelativeWeekDayRegex)
        self._utility_configuration = config.utility_configuration
        self._date_token_prefix = FrenchDateTime.DateTokenPrefix

    def get_swift_day(self, source: str) -> int:
        trimmed_text = source.strip().lower()
        swift = 0

        if trimmed_text == 'aujourd\'hui' or trimmed_text == 'auj':
            swift = 0
        elif trimmed_text == 'demain' or trimmed_text.endswith('a2m1') or trimmed_text.endswith('lendemain') or trimmed_text.endswith('jour suivant'):
            swift = 1
        elif trimmed_text == 'hier':
            swift = -1
        elif trimmed_text.endswith('après demain') or trimmed_text.endswith('après-demain'):
            swift = 2
        elif trimmed_text.endswith('avant-hier') or trimmed_text.endswith('avant hier'):
            swift = -2
        elif trimmed_text.endswith('dernier'):
            swift = -1

        return swift

    def get_swift_month(self, source: str) -> int:
        trimmed_text = source.strip().lower()
        swift = 0

        if trimmed_text.endswith('prochaine') or trimmed_text.endswith('prochain'):
            swift = 1

        if trimmed_text == 'dernière' or trimmed_text.endswith('dernières') or trimmed_text.endswith('derniere') or trimmed_text.endswith('dernieres'):
            swift = -1

        return swift

    def is_cardinal_last(self, source: str) -> bool:
        trimmed_text = source.strip().lower()
        return trimmed_text.endswith('dernière') or trimmed_text.endswith('dernières') or trimmed_text.endswith('derniere') or trimmed_text.endswith('dernieres')
