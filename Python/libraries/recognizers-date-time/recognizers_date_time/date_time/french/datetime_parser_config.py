from typing import Pattern, Dict

from recognizers_text.utilities import RegExpUtility
from recognizers_number.number.extractors import BaseNumberExtractor
from recognizers_number.number.parsers import BaseNumberParser
from ...resources.french_date_time import FrenchDateTime
from ..extractors import DateTimeExtractor
from ..parsers import DateTimeParser
from ..utilities import DateTimeUtilityConfiguration
from ..base_configs import BaseDateParserConfiguration
from ..base_datetime import DateTimeParserConfiguration, MatchedTimex

class FrenchDateTimeParserConfiguration(DateTimeParserConfiguration):
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
        self._token_before_date = FrenchDateTime.TokenBeforeDate
        self._token_before_time = FrenchDateTime.TokenBeforeTime
        self._now_regex = RegExpUtility.get_safe_reg_exp(FrenchDateTime.NowRegex)
        self._am_time_regex = RegExpUtility.get_safe_reg_exp(FrenchDateTime.AMTimeRegex)
        self._pm_time_regex = RegExpUtility.get_safe_reg_exp(FrenchDateTime.PMTimeRegex)
        self._simple_time_of_today_after_regex = RegExpUtility.get_safe_reg_exp(FrenchDateTime.SimpleTimeOfTodayAfterRegex)
        self._simple_time_of_today_before_regex = RegExpUtility.get_safe_reg_exp(FrenchDateTime.SimpleTimeOfTodayBeforeRegex)
        self._specific_time_of_day_regex = RegExpUtility.get_safe_reg_exp(FrenchDateTime.SpecificTimeOfDayRegex)
        self._the_end_of_regex = RegExpUtility.get_safe_reg_exp(FrenchDateTime.TheEndOfRegex)
        self._unit_regex = RegExpUtility.get_safe_reg_exp(FrenchDateTime.TimeUnitRegex)

        self._date_extractor = config.date_extractor
        self._time_extractor = config.time_extractor
        self._date_parser = config.date_parser
        self._time_parser = config.time_parser
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
        timex = ''

        if source.endswith('maintenant'):
            timex = 'PRESENT_REF'
        elif (
                source == 'récemment' or 
                source == 'précédemment' or 
                source == 'auparavant'
            ):
            timex = 'PAST_REF'
        elif source == 'dès que possible' or source == 'dqp':
            timex = 'FUTURE_REF'
        else:
            return MatchedTimex(False, None)

        return MatchedTimex(True, timex)

    def get_swift_day(self, source: str) -> int:
        source = source.strip().lower()
        swift = 0

        if (
                source.startswith('prochain') or
                source.endswith('prochain') or
                source.startswith('prochaine') or
                source.endswith('prochaine')
            ):
            swift = -1
        elif (
                source.startswith('dernier') or
                source.startswith('dernière') or
                source.endswith('dernier') or
                source.endswith('dernière')
            ):
            swift = 1

        return swift

    def get_hour(self, source: str, hour: int) -> int:
        source = source.strip().lower()
        result = hour

        # TODO: replace with a regex
        if source.endswith('matin') and hour >= 12:
            result -= 12
        elif not source.endswith('matin') and hour < 12:
            result += 12

        return result
