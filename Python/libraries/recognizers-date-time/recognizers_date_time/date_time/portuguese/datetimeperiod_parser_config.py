#  Copyright (c) Microsoft Corporation. All rights reserved.
#  Licensed under the MIT License.

from typing import Pattern, Dict
import regex

from recognizers_text.utilities import RegExpUtility
from ...resources.portuguese_date_time import PortugueseDateTime
from ..base_datetimeperiod import DateTimePeriodParserConfiguration, MatchedTimeRange
from ..constants import Constants, TimeTypeConstants
from ..extractors import DateTimeExtractor
from ..parsers import DateTimeParser
from ..base_configs import BaseDateParserConfiguration


class PortugueseDateTimePeriodParserConfiguration(DateTimePeriodParserConfiguration):

    @property
    def time_of_day_regex(self) -> Pattern:
        return self._time_of_day_regex

    @property
    def future_suffix_regex(self):
        return self._future_suffix_regex

    @property
    def within_next_prefix_regex(self):
        return self._within_next_prefix_regex

    def __init__(self, config: BaseDateParserConfiguration):
        self._time_of_day_regex = RegExpUtility.get_safe_reg_exp(PortugueseDateTime.TimeOfDayRegex)
        self._future_suffix_regex = RegExpUtility.get_safe_reg_exp(PortugueseDateTime.FutureSuffixRegex)
        self._within_next_prefix_regex = RegExpUtility.get_safe_reg_exp(PortugueseDateTime.WithinNextPrefixRegex)
        self._previous_prefix_regex = RegExpUtility.get_safe_reg_exp(PortugueseDateTime.PreviousPrefixRegex)
        self._cardinal_extractor = config.cardinal_extractor
        self._am_desc_regex = RegExpUtility.get_safe_reg_exp(PortugueseDateTime.AmDescRegex)
        self._pm_desc_regex = RegExpUtility.get_safe_reg_exp(PortugueseDateTime.PmDescRegex)
        self._token_before_date = PortugueseDateTime.TokenBeforeDate
        self._token_before_time = PortugueseDateTime.TokenBeforeTime
        self._check_both_before_after = PortugueseDateTime.CheckBothBeforeAfter
        self._pure_number_from_to_regex = RegExpUtility.get_safe_reg_exp(
            PortugueseDateTime.PureNumFromTo)
        self._pure_number_between_and_regex = RegExpUtility.get_safe_reg_exp(
            PortugueseDateTime.PureNumBetweenAnd)
        self._period_time_of_day_with_date_regex = RegExpUtility.get_safe_reg_exp(
            PortugueseDateTime.PeriodTimeOfDayWithDateRegex)
        self._specific_time_of_day_regex = RegExpUtility.get_safe_reg_exp(
            PortugueseDateTime.SpecificTimeOfDayRegex)
        self._past_regex = RegExpUtility.get_safe_reg_exp(
            PortugueseDateTime.PreviousPrefixRegex)
        self._future_regex = RegExpUtility.get_safe_reg_exp(
            PortugueseDateTime.NextPrefixRegex)
        self._relative_time_unit_regex = RegExpUtility.get_safe_reg_exp(
            PortugueseDateTime.RelativeTimeUnitRegex)
        self._rest_of_date_time_regex = RegExpUtility.get_safe_reg_exp(
            PortugueseDateTime.RestOfDateTimeRegex)
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
        self._time_zone_parser = config.time_zone_parser
        self._prefix_day_regex = RegExpUtility.get_safe_reg_exp(
            PortugueseDateTime.PrefixDayRegex)
        self._after_regex = RegExpUtility.get_safe_reg_exp(
            PortugueseDateTime.AfterRegex)
        self._before_regex = RegExpUtility.get_safe_reg_exp(
            PortugueseDateTime.BeforeRegex)

    @property
    def previous_prefix_regex(self):
        return self._previous_prefix_regex

    @property
    def cardinal_extractor(self):
        return self._cardinal_extractor

    @property
    def am_desc_regex(self):
        return self._am_desc_regex

    @property
    def pm_desc_regex(self):
        return self._pm_desc_regex

    @property
    def before_regex(self):
        return self._before_regex

    @property
    def after_regex(self):
        return self._after_regex

    @property
    def prefix_day_regex(self):
        return self._prefix_day_regex

    @property
    def token_before_date(self) -> str:
        return self._token_before_date

    @property
    def token_before_time(self):
        return self._token_before_time

    @property
    def check_both_before_after(self) -> bool:
        return self._check_both_before_after

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

    @property
    def time_zone_parser(self) -> DateTimeParser:
        return self._time_zone_parser

    def get_matched_time_range(self, source: str):
        trimmed_source = source.strip()

        begin_hour = 0
        end_hour = 0
        end_min = 0

        time_str = ''
        return MatchedTimeRange(time_str, begin_hour, end_hour, end_min, False)

    def get_swift_prefix(self, source: str) -> int:
        if source.startswith('next'):
            return 1

        if source.startswith('last'):
            return -1

        return 0
