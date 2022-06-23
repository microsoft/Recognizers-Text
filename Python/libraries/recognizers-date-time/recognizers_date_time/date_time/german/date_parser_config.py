#  Copyright (c) Microsoft Corporation. All rights reserved.
#  Licensed under the MIT License.

from typing import Pattern, List, Dict
import regex

from recognizers_text.utilities import RegExpUtility
from recognizers_number import BaseNumberExtractor, BaseNumberParser
from ...resources.german_date_time import GermanDateTime
from ..extractors import DateTimeExtractor
from ..parsers import DateTimeParser
from ..utilities import DateTimeUtilityConfiguration
from ..base_date import DateParserConfiguration
from ..base_configs import BaseDateParserConfiguration
from .date_extractor_config import GermanDateExtractorConfiguration


class GermanDateParserConfiguration(DateParserConfiguration):
    @property
    def check_both_before_after(self) -> bool:
        return self._check_both_before_after

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
    def date_extractor(self) -> DateTimeExtractor:
        return self._date_extractor

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
    def special_day_with_num_regex(self) -> Pattern:
        return self._special_day_with_num_regex

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
    def week_day_and_day_regex(self) -> Pattern:
        return self._week_day_and_day_regex

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

    _relative_day_regex = RegExpUtility.get_safe_reg_exp(
        GermanDateTime.RelativeDayRegex)
    _next_prefix_regex = RegExpUtility.get_safe_reg_exp(
        GermanDateTime.NextPrefixRegex)
    _past_prefix_regex = RegExpUtility.get_safe_reg_exp(
        GermanDateTime.PreviousPrefixRegex)

    def __init__(self, config: BaseDateParserConfiguration):
        self._ordinal_extractor = config.ordinal_extractor
        self._integer_extractor = config.integer_extractor
        self._cardinal_extractor = config.cardinal_extractor
        self._date_extractor = config.date_extractor
        self._duration_extractor = config.duration_extractor
        self._number_parser = config.number_parser
        self._duration_parser = config.duration_parser
        self._month_of_year = config.month_of_year
        self._day_of_month = config.day_of_month
        self._day_of_week = config.day_of_week
        self._unit_map = config.unit_map
        self._cardinal_map = config.cardinal_map
        self._date_regex = (GermanDateExtractorConfiguration()).date_regex_list
        self._on_regex = RegExpUtility.get_safe_reg_exp(GermanDateTime.OnRegex)
        self._special_day_regex = RegExpUtility.get_safe_reg_exp(
            GermanDateTime.SpecialDayRegex)
        self._special_day_with_num_regex = RegExpUtility.get_safe_reg_exp(
            GermanDateTime.SpecialDayWithNumRegex)
        self._next_regex = RegExpUtility.get_safe_reg_exp(
            GermanDateTime.NextDateRegex)
        self._unit_regex = RegExpUtility.get_safe_reg_exp(
            GermanDateTime.DateUnitRegex)
        self._month_regex = RegExpUtility.get_safe_reg_exp(
            GermanDateTime.MonthRegex)
        self._week_day_regex = RegExpUtility.get_safe_reg_exp(
            GermanDateTime.WeekDayRegex)
        self._last_regex = RegExpUtility.get_safe_reg_exp(
            GermanDateTime.LastDateRegex)
        self._this_regex = RegExpUtility.get_safe_reg_exp(
            GermanDateTime.ThisRegex)
        self._week_day_of_month_regex = RegExpUtility.get_safe_reg_exp(
            GermanDateTime.WeekDayOfMonthRegex)
        self._for_the_regex = RegExpUtility.get_safe_reg_exp(
            GermanDateTime.ForTheRegex)
        self._week_day_and_day_of_month_regex = RegExpUtility.get_safe_reg_exp(
            GermanDateTime.WeekDayAndDayOfMonthRegex)
        self._week_day_and_day_regex = RegExpUtility.get_safe_reg_exp(
            GermanDateTime.WeekDayAndDayRegex)
        self._relative_month_regex = RegExpUtility.get_safe_reg_exp(
            GermanDateTime.RelativeMonthRegex)
        self._relative_week_day_regex = RegExpUtility.get_safe_reg_exp(
            GermanDateTime.RelativeWeekDayRegex)
        self._utility_configuration = config.utility_configuration
        self._date_token_prefix = GermanDateTime.DateTokenPrefix
        self._check_both_before_after = GermanDateTime.CheckBothBeforeAfter

    def get_swift_day(self, source: str) -> int:
        trimmed_text = source.strip().lower()
        swift = 0
        matches = regex.search(
            GermanDateParserConfiguration._relative_day_regex, source)
        if trimmed_text == 'heute':
            swift = 0
        elif trimmed_text == 'morgen':
            swift = 1
        elif trimmed_text == 'gestern':
            swift = -1
        elif trimmed_text.endswith('übermorgen'):
            swift = 2
        elif trimmed_text.endswith('vorgestern'):
            swift = -2
        elif trimmed_text.endswith('tag danach'):
            swift = 1
        elif trimmed_text.endswith('der tag zuvor'):
            swift = -1
        elif matches:
            swift = self.get_swift(source)

        return swift

    def get_swift_month(self, source: str) -> int:
        return self.get_swift(source)

    def get_swift(self, source: str) -> int:
        trimmed_text = source.strip().lower()
        swift = 0
        next_prefix_matches = regex.search(
            GermanDateParserConfiguration._next_prefix_regex, trimmed_text)
        past_prefix_matches = regex.search(
            GermanDateParserConfiguration._past_prefix_regex, trimmed_text)
        if next_prefix_matches:
            swift = 1
        elif past_prefix_matches:
            swift = -1

        return swift

    def is_cardinal_last(self, source: str) -> bool:
        trimmed_text = source.strip().lower()
        return not regex.search(GermanDateParserConfiguration._past_prefix_regex, trimmed_text) is None
