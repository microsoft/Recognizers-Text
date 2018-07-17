from typing import Pattern, Dict

from recognizers_text.utilities import RegExpUtility
from ...resources.french_date_time import FrenchDateTime
from ..base_datetimeperiod import DateTimePeriodParserConfiguration, MatchedTimeRange
from ..extractors import DateTimeExtractor
from ..parsers import DateTimeParser
from ..base_configs import BaseDateParserConfiguration

class FrenchDateTimePeriodParserConfiguration(DateTimePeriodParserConfiguration):

    def __init__(self, config: BaseDateParserConfiguration):
        self._date_extractor = config.date_extractor
        self._time_extractor = config.time_extractor
        self._date_time_extractor = config.date_time_extractor
        self._time_period_extractor = config.time_period_extractor
        self.cardinal_extractor = config.cardinal_extractor
        self._duration_extractor = config.duration_extractor
        self.number_parser = config.number_parser
        self._date_parser = config.date_parser
        self._time_parser = config.time_parser
        self._date_time_parser = config.date_time_parser
        self._time_period_parser = config.time_period_parser
        self._duration_parser = config.duration_parser
        self._unit_map = config.unit_map
        self._numbers = config.numbers

        self.next_prefix_regex = RegExpUtility.get_safe_reg_exp(FrenchDateTime.NextSuffixRegex)
        self.past_prefix_regex = RegExpUtility.get_safe_reg_exp(FrenchDateTime.PastSuffixRegex)
        self.this_prefix_regex = RegExpUtility.get_safe_reg_exp(FrenchDateTime.ThisPrefixRegex)

        self.morning_start_end_regex = RegExpUtility.get_safe_reg_exp(FrenchDateTime.MorningStartEndRegex)
        self.afternoon_start_end_regex = RegExpUtility.get_safe_reg_exp(FrenchDateTime.AfternoonStartEndRegex)
        self.evening_start_end_regex = RegExpUtility.get_safe_reg_exp(FrenchDateTime.EveningStartEndRegex)
        self.night_start_end_regex = RegExpUtility.get_safe_reg_exp(FrenchDateTime.NightStartEndRegex)

        self._pure_number_from_to_regex = RegExpUtility.get_safe_reg_exp(FrenchDateTime.PureNumFromTo)
        self._pure_number_between_and_regex = RegExpUtility.get_safe_reg_exp(FrenchDateTime.PureNumBetweenAnd)
        self._specific_time_of_day_regex = RegExpUtility.get_safe_reg_exp(FrenchDateTime.SpecificTimeOfDayRegex)
        self.time_of_day_regex = RegExpUtility.get_safe_reg_exp(FrenchDateTime.TimeOfDayRegex)
        self._past_regex = RegExpUtility.get_safe_reg_exp(FrenchDateTime.PastSuffixRegex)
        self._future_regex = RegExpUtility.get_safe_reg_exp(FrenchDateTime.NextSuffixRegex)
        self.number_combined_with_unit_regex = RegExpUtility.get_safe_reg_exp(FrenchDateTime.TimeNumberCombinedWithUnit)
        self.unit_regex = RegExpUtility.get_safe_reg_exp(FrenchDateTime.TimeUnitRegex)
        self._period_time_of_day_with_date_regex = RegExpUtility.get_safe_reg_exp(FrenchDateTime.PeriodTimeOfDayWithDateRegex)
        self._relative_time_unit_regex = RegExpUtility.get_safe_reg_exp(FrenchDateTime.RelativeTimeUnitRegex)
        self._rest_of_date_time_regex = RegExpUtility.get_safe_reg_exp(FrenchDateTime.RestOfDateTimeRegex)

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
        trimmed_source = source.strip().lower()
        time_str = ''
        begin_hour = 0
        end_hour = 0
        end_min = 0

        if self.morning_start_end_regex.search(trimmed_source):
            time_str = 'TMO'
            begin_hour = 8
            end_hour = 12
        elif self.afternoon_start_end_regex.search(trimmed_source):
            time_str = 'TAF'
            begin_hour = 12
            end_hour = 16
        elif self.evening_start_end_regex.search(trimmed_source):
            time_str = 'TEV'
            begin_hour = 16
            end_hour = 20
        elif self.night_start_end_regex.search(trimmed_source):
            time_str = 'TNI'
            begin_hour = 20
            end_hour = 23
            end_min = 59
        else:
            return MatchedTimeRange(time_str, begin_hour, end_hour, end_min, False)

        return MatchedTimeRange(time_str, begin_hour, end_hour, end_min, True)

    def get_swift_prefix(self, source: str) -> int:
        trimmed_source = source.strip().lower()
        swift = 0

        # TODO: replace with regex
        if (
            trimmed_source.startswith('prochain') or
            trimmed_source.endswith('prochain') or
            trimmed_source.startswith('prochaine') or
            trimmed_source.endswith('prochaine')
            ):
            swift = 1
        elif (
            trimmed_source.startswith('derniere') or
            trimmed_source.startswith('dernier') or
            trimmed_source.endswith('derniere') or
            trimmed_source.endswith('dernier')
            ):
            swift = -1

        return swift
