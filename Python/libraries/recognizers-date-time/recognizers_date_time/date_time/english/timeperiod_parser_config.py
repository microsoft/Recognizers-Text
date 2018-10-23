from typing import Pattern, Dict

from recognizers_text.utilities import RegExpUtility
from recognizers_text.extractor import Extractor
from recognizers_number.number.english.extractors import EnglishIntegerExtractor
from ...resources.english_date_time import EnglishDateTime
from ..extractors import DateTimeExtractor
from ..parsers import DateTimeParser
from ..base_configs import BaseDateParserConfiguration, DateTimeUtilityConfiguration
from ..base_timeperiod import TimePeriodParserConfiguration, MatchedTimeRegex
from ..constants import Constants

class EnglishTimePeriodParserConfiguration(TimePeriodParserConfiguration):
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
        self._integer_extractor = EnglishIntegerExtractor()
        self._pure_number_from_to_regex = RegExpUtility.get_safe_reg_exp(EnglishDateTime.PureNumFromTo)
        self._pure_number_between_and_regex = RegExpUtility.get_safe_reg_exp(EnglishDateTime.PureNumBetweenAnd)
        self._time_of_day_regex = RegExpUtility.get_safe_reg_exp(EnglishDateTime.TimeOfDayRegex)
        self._till_regex = RegExpUtility.get_safe_reg_exp(EnglishDateTime.TillRegex)
        self._numbers = EnglishDateTime.Numbers
        self._utility_configuration = config.utility_configuration


    def get_matched_timex_range(self, source: str) -> MatchedTimeRegex:
        source = source.strip().lower()
        if source.endswith('s'):
            source = source[:-1]

        timex = ''
        begin_hour = 0
        end_hour = 0
        end_min = 0

        if source.endswith(Constants.EN_MORNING):
            timex = EnglishDateTime.TimeOfDayTimex[Constants.EN_MORNING]
            begin_hour = EnglishDateTime.TimeOfDayBeginHour[Constants.EN_MORNING]
            end_hour = EnglishDateTime.TimeOfDayEndHour[Constants.EN_MORNING]
        elif source.endswith(Constants.EN_AFTERNOON):
            timex = EnglishDateTime.TimeOfDayTimex[Constants.EN_AFTERNOON]
            begin_hour = EnglishDateTime.TimeOfDayBeginHour[Constants.EN_AFTERNOON]
            end_hour = EnglishDateTime.TimeOfDayEndHour[Constants.EN_AFTERNOON]
        elif source.endswith(Constants.EN_EVENING):
            timex = EnglishDateTime.TimeOfDayTimex[Constants.EN_EVENING]
            begin_hour = EnglishDateTime.TimeOfDayBeginHour[Constants.EN_EVENING]
            end_hour = EnglishDateTime.TimeOfDayEndHour[Constants.EN_EVENING]
        elif source.endswith(Constants.EN_DAYTIME):
            timex = EnglishDateTime.TimeOfDayTimex[Constants.EN_DAYTIME]
            begin_hour = EnglishDateTime.TimeOfDayBeginHour[Constants.EN_DAYTIME]
            end_hour = EnglishDateTime.TimeOfDayEndHour[Constants.EN_DAYTIME]
        elif source.endswith(Constants.EN_NIGHT):
            timex = EnglishDateTime.TimeOfDayTimex[Constants.EN_NIGHT]
            begin_hour = EnglishDateTime.TimeOfDayBeginHour[Constants.EN_NIGHT]
            end_hour = EnglishDateTime.TimeOfDayEndHour[Constants.EN_NIGHT]
            end_min = EnglishDateTime.TimeOfDayEndMin[Constants.EN_NIGHT]
        else:
            return MatchedTimeRegex(
                matched=False,
                timex='',
                begin_hour=0,
                end_hour=0,
                end_min=0
            )

        return MatchedTimeRegex(
            matched=True,
            timex=timex,
            begin_hour=begin_hour,
            end_hour=end_hour,
            end_min=end_min
        )
