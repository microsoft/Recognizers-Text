from typing import Pattern, Dict

from recognizers_text.utilities import RegExpUtility
from recognizers_number.number.extractors import BaseNumberExtractor
from recognizers_number.number.parsers import BaseNumberParser
from ...resources.english_date_time import EnglishDateTime
from ..extractors import DateTimeExtractor
from ..parsers import DateTimeParser
from ..utilities import DateTimeUtilityConfiguration
from ..base_configs import BaseDateParserConfiguration
from ..base_datetime import DateTimeParserConfiguration, MatchedTimex

class EnglishDateTimeParserConfiguration(DateTimeParserConfiguration):
    @property
    def token_before_date(self) -> str:
        return self._token_before_date

    @property
    def token_before_time(self) -> str:
        return self._token_before_time

    @property
    def date_extractor(self) -> DateTimeExtractor:
        return self._date_extractor

    @property
    def time_extractor(self) -> DateTimeExtractor:
        return self._time_extractor

    @property
    def date_parser(self) -> DateTimeParser:
        return self._date_parser

    @property
    def time_parser(self) -> DateTimeParser:
        return self._time_parser

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
    def duration_parser(self) -> DateTimeParser:
        return self._duration_parser

    @property
    def now_regex(self) -> Pattern:
        return self._now_regex

    @property
    def am_time_regex(self) -> Pattern:
        return self._am_time_regex

    @property
    def pm_time_regex(self) -> Pattern:
        return self._pm_time_regex

    @property
    def simple_time_of_today_after_regex(self) -> Pattern:
        return self._simple_time_of_today_after_regex

    @property
    def simple_time_of_today_before_regex(self) -> Pattern:
        return self._simple_time_of_today_before_regex

    @property
    def specific_time_of_day_regex(self) -> Pattern:
        return self._specific_time_of_day_regex

    @property
    def the_end_of_regex(self) -> Pattern:
        return self._the_end_of_regex

    @property
    def unit_regex(self) -> Pattern:
        return self._unit_regex

    @property
    def unit_map(self) -> Dict[str, str]:
        return self._unit_map

    @property
    def numbers(self) -> Dict[str, int]:
        return self._numbers

    @property
    def utility_configuration(self) -> DateTimeUtilityConfiguration:
        return self._utility_configuration

    def __init__(self, config: BaseDateParserConfiguration):
        self._token_before_date = EnglishDateTime.TokenBeforeDate
        self._token_before_time = EnglishDateTime.TokenBeforeTime
        self._date_extractor = config.date_extractor
        self._time_extractor = config.time_extractor
        self._date_parser = config.date_parser
        self._time_parser = config.time_parser
        self._now_regex = RegExpUtility.get_safe_reg_exp(EnglishDateTime.NowRegex)
        self._am_time_regex = RegExpUtility.get_safe_reg_exp(EnglishDateTime.AMTimeRegex)
        self._pm_time_regex = RegExpUtility.get_safe_reg_exp(EnglishDateTime.PMTimeRegex)
        self._simple_time_of_today_after_regex = RegExpUtility.get_safe_reg_exp(EnglishDateTime.SimpleTimeOfTodayAfterRegex)
        self._simple_time_of_today_before_regex = RegExpUtility.get_safe_reg_exp(EnglishDateTime.SimpleTimeOfTodayBeforeRegex)
        self._specific_time_of_day_regex = RegExpUtility.get_safe_reg_exp(EnglishDateTime.SpecificTimeOfDayRegex)
        self._the_end_of_regex = RegExpUtility.get_safe_reg_exp(EnglishDateTime.TheEndOfRegex)
        self._unit_regex = RegExpUtility.get_safe_reg_exp(EnglishDateTime.TimeUnitRegex)
        self._numbers = config.numbers
        self._cardinal_extractor = config.cardinal_extractor
        self._number_parser = config.number_parser
        self._duration_extractor = config.duration_extractor
        self._duration_parser = config.duration_parser
        self._unit_map = config.unit_map
        self._utility_configuration = config.utility_configuration

    def have_ambiguous_token(self, source: str, matched_text: str) -> bool:
        return False

    def get_matched_now_timex(self, source: str) -> MatchedTimex:
        source = source.strip().lower()

        if source.endswith('now'):
            return MatchedTimex(True, 'PRESENT_REF')
        elif source in ['recently', 'previously']:
            return MatchedTimex(True, 'PAST_REF')
        elif source in ['as soon as possible', 'asap']:
            return MatchedTimex(True, 'FUTURE_REF')

        return MatchedTimex(False, None)

    def get_swift_day(self, source: str) -> int:
        source = source.strip().lower()

        if source.startswith('next'):
            return 1
        elif source.startswith('last'):
            return -1

        return 0

    def get_hour(self, source: str, hour: int) -> int:
        source = source.strip().lower()

        if source.endswith('morning') and hour >= 12:
            return hour - 12
        elif not source.endswith('morning') and hour < 12:
            return hour + 12

        return hour
