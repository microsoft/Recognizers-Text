from typing import Pattern, Dict

from recognizers_text.utilities import RegExpUtility
from recognizers_text.extractor import Extractor
from recognizers_number.number.french.extractors import FrenchIntegerExtractor
from ...resources.french_date_time import FrenchDateTime
from ..extractors import DateTimeExtractor
from ..parsers import DateTimeParser
from ..base_configs import BaseDateParserConfiguration, DateTimeUtilityConfiguration
from ..base_timeperiod import TimePeriodParserConfiguration, MatchedTimeRegex
from ..constants import Constants
from ..utilities import TimexUtil

class FrenchTimePeriodParserConfiguration(TimePeriodParserConfiguration):
    @property
    def time_extractor(self) -> DateTimeExtractor:
        return self._time_extractor

    @property
    def time_parser(self) -> DateTimeParser:
        return self._time_parser

    @property
    def integer_extractor(self) -> Extractor:
        return self._integer_extractor

    @property
    def pure_number_from_to_regex(self) -> Pattern:
        return self._pure_number_from_to_regex

    @property
    def pure_number_between_and_regex(self) -> Pattern:
        return self._pure_number_between_and_regex

    @property
    def time_of_day_regex(self) -> Pattern:
        return self._time_of_day_regex

    @property
    def till_regex(self) -> Pattern:
        return self._till_regex

    @property
    def numbers(self) -> Dict[str, int]:
        return self._numbers

    @property
    def utility_configuration(self) -> DateTimeUtilityConfiguration:
        return self._utility_configuration

    def __init__(self, config: BaseDateParserConfiguration):
        self._time_extractor = config.time_extractor
        self._time_parser = config.time_parser
        self._integer_extractor = config.integer_extractor
        self._numbers = config.numbers
        self._utility_configuration = config.utility_configuration
        self._pure_number_from_to_regex = RegExpUtility.get_safe_reg_exp(FrenchDateTime.PureNumFromTo)
        self._pure_number_between_and_regex = RegExpUtility.get_safe_reg_exp(FrenchDateTime.PureNumBetweenAnd)
        self._time_of_day_regex = RegExpUtility.get_safe_reg_exp(FrenchDateTime.TimeOfDayRegex)
        self._till_regex = RegExpUtility.get_safe_reg_exp(FrenchDateTime.TillRegex)

    def get_matched_timex_range(self, source: str) -> MatchedTimeRegex:
        source = source.strip().lower()
        if source.endswith('s'):
            source = source[:-1]

        timex = ''
        begin_hour = 0
        end_hour = 0
        end_min = 0

        time_of_day = ""
        if source.endswith(FrenchDateTime.MorningTerm1) or source.endswith(FrenchDateTime.MorningTerm2) \
                or source.endswith(FrenchDateTime.MorningTerm3):
            time_of_day = Constants.Morning
        elif source.endswith(FrenchDateTime.AfternoonTerm1) or source.endswith(FrenchDateTime.AfternoonTerm2) \
                or source.endswith(FrenchDateTime.AfternoonTerm3) or source.endswith(FrenchDateTime.AfternoonTerm4):
            time_of_day = Constants.Afternoon
        elif source.endswith(FrenchDateTime.EveningTerm1) or source.endswith(FrenchDateTime.EveningTerm2) \
                or source.endswith(FrenchDateTime.EveningTerm3):
            time_of_day = Constants.Evening
        elif source == FrenchDateTime.DaytimeTerm1 or source.endswith(FrenchDateTime.DaytimeTerm2) \
                or source.endswith(FrenchDateTime.DaytimeTerm3):
            time_of_day = Constants.Daytime
        elif source.endswith(FrenchDateTime.NightTerm):
            time_of_day = Constants.Night
        else:
            return MatchedTimeRegex(
                matched=False,
                timex='',
                begin_hour=0,
                end_hour=0,
                end_min=0
            )

        parse_result = TimexUtil.parse_time_of_day(time_of_day)
        timex = parse_result.timex
        begin_hour = parse_result.begin_hour
        end_hour = parse_result.end_hour
        end_min = parse_result.end_min

        return MatchedTimeRegex(
            matched=True,
            timex=timex,
            begin_hour=begin_hour,
            end_hour=end_hour,
            end_min=end_min
        )
