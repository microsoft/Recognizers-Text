from typing import Pattern, Dict
import regex

from recognizers_text.utilities import RegExpUtility
from ...resources.english_date_time import EnglishDateTime
from ..base_datetimeperiod import DateTimePeriodParserConfiguration, MatchedTimeRange
from ..extractors import DateTimeExtractor
from ..parsers import DateTimeParser
from ..base_configs import BaseDateParserConfiguration

class EnglishDateTimePeriodParserConfiguration(DateTimePeriodParserConfiguration):

    def __init__(self, config: BaseDateParserConfiguration):
        self._pure_number_from_to_regex = RegExpUtility.get_safe_reg_exp(EnglishDateTime.PureNumFromTo)
        self._pure_number_between_and_regex = RegExpUtility.get_safe_reg_exp(EnglishDateTime.PureNumBetweenAnd)
        self._period_time_of_day_with_date_regex = RegExpUtility.get_safe_reg_exp(EnglishDateTime.PeriodTimeOfDayWithDateRegex)
        self._specific_time_of_day_regex = RegExpUtility.get_safe_reg_exp(EnglishDateTime.SpecificTimeOfDayRegex)
        self._past_regex = RegExpUtility.get_safe_reg_exp(EnglishDateTime.PastPrefixRegex)
        self._future_regex = RegExpUtility.get_safe_reg_exp(EnglishDateTime.NextPrefixRegex)
        self._relative_time_unit_regex = RegExpUtility.get_safe_reg_exp(EnglishDateTime.RelativeTimeUnitRegex)
        self._rest_of_date_time_regex = RegExpUtility.get_safe_reg_exp(EnglishDateTime.RestOfDateTimeRegex)
        self._numbers = config.numbers
        self._unit_map = config.unit_map
        self._date_extractor = config.date_extractor
        self._time_extractor = config.time_extractor
        self._date_time_extractor = config.date_time_extractor
        self._time_period_extractor = config.time_period_extractor
        self._duration_extractor = config.duration_extractor
        self._date_parser = config.date_parser
        self._time_parser = config.time_parser
        self._date_time_parser = config.date_time_parser
        self._time_period_parser = config.time_period_parser
        self._duration_parser = config.duration_parser
        self.morning_start_end_regex = RegExpUtility.get_safe_reg_exp(EnglishDateTime.MorningStartEndRegex)
        self.afternoon_start_end_regex = RegExpUtility.get_safe_reg_exp(EnglishDateTime.AfternoonStartEndRegex)
        self.evening_start_end_regex = RegExpUtility.get_safe_reg_exp(EnglishDateTime.EveningStartEndRegex)
        self.night_start_end_regex = RegExpUtility.get_safe_reg_exp(EnglishDateTime.NightStartEndRegex)

    @property
    def pure_number_from_to_regex(self) -> Pattern:
        return self._pure_number_from_to_regex

    @property
    def pure_number_between_and_regex(self) -> Pattern:
        return self._pure_number_between_and_regex

    @property
    def period_time_of_day_with_date_regex(self) -> Pattern:
        return self._period_time_of_day_with_date_regex

    @property
    def specific_time_of_day_regex(self) -> Pattern:
        return self._specific_time_of_day_regex

    @property
    def past_regex(self) -> Pattern:
        return self._past_regex

    @property
    def future_regex(self) -> Pattern:
        return self._future_regex

    @property
    def relative_time_unit_regex(self) -> Pattern:
        return self._relative_time_unit_regex

    @property
    def rest_of_date_time_regex(self) -> Pattern:
        return self._rest_of_date_time_regex

    @property
    def numbers(self) -> Dict[str, int]:
        return self._numbers

    @property
    def unit_map(self) -> Dict[str, str]:
        return self._unit_map

    @property
    def date_extractor(self) -> DateTimeExtractor:
        return self._date_extractor

    @property
    def time_extractor(self) -> DateTimeExtractor:
        return self._time_extractor

    @property
    def date_time_extractor(self) -> DateTimeExtractor:
        return self._date_time_extractor

    @property
    def time_period_extractor(self) -> DateTimeExtractor:
        return self._time_period_extractor

    @property
    def duration_extractor(self) -> DateTimeExtractor:
        return self._duration_extractor

    @property
    def date_parser(self) -> DateTimeParser:
        return self._date_parser

    @property
    def time_parser(self) -> DateTimeParser:
        return self._time_parser

    @property
    def date_time_parser(self) -> DateTimeParser:
        return self._date_time_parser

    @property
    def time_period_parser(self) -> DateTimeParser:
        return self._time_period_parser

    @property
    def duration_parser(self) -> DateTimeParser:
        return self._duration_parser

    def get_matched_time_range(self, source: str) -> MatchedTimeRange:
        if regex.search(self.morning_start_end_regex, source):
            return MatchedTimeRange('TMO', 8, 12, 0, True)

        if regex.search(self.afternoon_start_end_regex, source):
            return MatchedTimeRange('TAF', 12, 16, 0, True)

        if regex.search(self.evening_start_end_regex, source):
            return MatchedTimeRange('TEV', 16, 20, 0, True)

        if regex.search(self.night_start_end_regex, source):
            return MatchedTimeRange('TNI', 20, 23, 59, True)

        return MatchedTimeRange(None, 0, 0, 0, False)

    def get_swift_prefix(self, source: str) -> int:
        if source.startswith('next'):
            return 1

        if source.startswith('last'):
            return -1

        return 0
