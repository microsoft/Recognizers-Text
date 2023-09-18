from typing import Pattern, Dict

from recognizers_text.utilities import RegExpUtility
from recognizers_text.extractor import Extractor
from recognizers_date_time.resources.arabic_date_time import ArabicDateTime
from recognizers_date_time.date_time.extractors import DateTimeExtractor
from recognizers_date_time.date_time.parsers import DateTimeParser
from recognizers_date_time.date_time.base_configs import BaseDateParserConfiguration, DateTimeUtilityConfiguration
from recognizers_date_time.date_time.base_timeperiod import TimePeriodParserConfiguration, MatchedTimeRegex
from recognizers_date_time.date_time.constants import Constants
from recognizers_date_time.date_time.utilities import TimexUtil


class ArabicTimePeriodParserConfiguration(TimePeriodParserConfiguration):
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
    def time_zone_parser(self) -> DateTimeParser:
        return self._time_zone_parser

    @property
    def specific_time_from_to_regex(self) -> Pattern:
        return self._specific_time_from_to_regex

    @property
    def specific_time_between_and_regex(self) -> Pattern:
        return self._specific_time_between_and_regex

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
    def general_ending_regex(self):
        return self._general_ending_regex

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
        self._general_ending_regex = RegExpUtility.get_safe_reg_exp(
            ArabicDateTime.GeneralEndingRegex)
        self._time_extractor = config.time_extractor
        self._time_parser = config.time_parser
        self._integer_extractor = config.integer_extractor
        self._numbers = config.numbers
        self._utility_configuration = config.utility_configuration
        self._pure_number_from_to_regex = RegExpUtility.get_safe_reg_exp(
            ArabicDateTime.PureNumFromTo)
        self._pure_number_between_and_regex = RegExpUtility.get_safe_reg_exp(
            ArabicDateTime.PureNumBetweenAnd)
        self._specific_time_from_to_regex = RegExpUtility.get_safe_reg_exp(
            ArabicDateTime.SpecificTimeFromTo)
        self._specific_time_between_and_regex = RegExpUtility.get_safe_reg_exp(
            ArabicDateTime.SpecificTimeBetweenAnd)
        self._time_of_day_regex = RegExpUtility.get_safe_reg_exp(
            ArabicDateTime.TimeOfDayRegex)
        self._till_regex = RegExpUtility.get_safe_reg_exp(
            ArabicDateTime.TillRegex)
        self._time_zone_parser = config.time_zone_parser

    def get_matched_timex_range(self, source: str) -> MatchedTimeRegex:
        trimmed_text = source.strip().lower()
        if trimmed_text.endswith('s'):
            trimmed_text = trimmed_text[:-1]

        if any(trimmed_text.endswith(o) for o in ArabicDateTime.MorningTermList):
            time_of_day = Constants.MORNING
        elif any(trimmed_text.endswith(o) for o in ArabicDateTime.AfternoonTermList):
            time_of_day = Constants.AFTERNOON
        elif any(trimmed_text.endswith(o) for o in ArabicDateTime.EveningTermList):
            time_of_day = Constants.EVENING
        elif any(trimmed_text.endswith(o) for o in ArabicDateTime.DaytimeTermList):
            time_of_day = Constants.DAYTIME
        elif any(trimmed_text.endswith(o) for o in ArabicDateTime.NightTermList):
            time_of_day = Constants.NIGHT
        elif all(trimmed_text.endswith(o) for o in ArabicDateTime.BusinessHourSplitStrings):
            time_of_day = Constants.BUSINESS_HOUR
        elif any(trimmed_text.endswith(o) for o in ArabicDateTime.MealtimeBreakfastTermList):
            time_of_day = Constants.MEALTIME_BREAKFAST
        elif any(trimmed_text.endswith(o) for o in ArabicDateTime.MealtimeBrunchTermList):
            time_of_day = Constants.MEALTIME_BRUNCH
        elif any(trimmed_text.endswith(o) for o in ArabicDateTime.MealtimeLunchTermList):
            time_of_day = Constants.MEALTIME_LUNCH
        elif any(trimmed_text.endswith(o) for o in ArabicDateTime.MealtimeDinnerTermList):
            time_of_day = Constants.MEALTIME_DINNER
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
