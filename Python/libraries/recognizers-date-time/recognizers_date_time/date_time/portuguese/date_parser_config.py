#  Copyright (c) Microsoft Corporation. All rights reserved.
#  Licensed under the MIT License.

from typing import Pattern, List, Dict
import regex

from recognizers_text.utilities import RegExpUtility
from recognizers_number import BaseNumberExtractor, BaseNumberParser
from ...resources.portuguese_date_time import PortugueseDateTime
from ..extractors import DateTimeExtractor
from ..parsers import DateTimeParser
from ..utilities import DateTimeUtilityConfiguration
from ..base_date import DateParserConfiguration
from ..base_configs import BaseDateParserConfiguration
from .date_extractor_config import PortugueseDateExtractorConfiguration


class PortugueseDateParserConfiguration(DateParserConfiguration):
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
    def for_the_regex(self) -> Pattern:
        return self._for_the_regex

    @property
    def week_day_and_day_of_month_regex(self) -> Pattern:
        return self._week_day_and_day_of_month_regex

    @property
    def week_day_and_day_regex(self) -> Pattern:
        return self._week_day_and_day_regex

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

    # The following three regexes only used in this configuration
    # They are not used in the base parser, therefore they are not extracted
    # If the spanish date parser need the same regexes, they should be extracted
    _relative_day_regex = RegExpUtility.get_safe_reg_exp(
        PortugueseDateTime.RelativeDayRegex)
    _next_prefix_regex = RegExpUtility.get_safe_reg_exp(
        PortugueseDateTime.NextPrefixRegex)
    _past_prefix_regex = RegExpUtility.get_safe_reg_exp(
        PortugueseDateTime.PreviousPrefixRegex)

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
        self._date_regex = (
            PortugueseDateExtractorConfiguration()).date_regex_list
        self._on_regex = RegExpUtility.get_safe_reg_exp(
            PortugueseDateTime.OnRegex)
        self._special_day_regex = RegExpUtility.get_safe_reg_exp(
            PortugueseDateTime.SpecialDayRegex)
        self._special_day_with_num_regex = RegExpUtility.get_safe_reg_exp(
            PortugueseDateTime.SpecialDayWithNumRegex)
        self._next_regex = RegExpUtility.get_safe_reg_exp(
            PortugueseDateTime.NextDateRegex)
        self._unit_regex = RegExpUtility.get_safe_reg_exp(
            PortugueseDateTime.DateUnitRegex)
        self._month_regex = RegExpUtility.get_safe_reg_exp(
            PortugueseDateTime.MonthRegex)
        self._week_day_regex = RegExpUtility.get_safe_reg_exp(
            PortugueseDateTime.WeekDayRegex)
        self._last_regex = RegExpUtility.get_safe_reg_exp(
            PortugueseDateTime.LastDateRegex)
        self._this_regex = RegExpUtility.get_safe_reg_exp(
            PortugueseDateTime.ThisRegex)
        self._week_day_of_month_regex = RegExpUtility.get_safe_reg_exp(
            PortugueseDateTime.WeekDayOfMonthRegex)
        self._for_the_regex = RegExpUtility.get_safe_reg_exp(
            PortugueseDateTime.ForTheRegex)
        self._week_day_and_day_of_month_regex = RegExpUtility.get_safe_reg_exp(
            PortugueseDateTime.WeekDayAndDayOfMonthRegex)
        self._week_day_and_day_regex = RegExpUtility.get_safe_reg_exp(
            PortugueseDateTime.WeekDayAndDayRegex)
        self._relative_month_regex = RegExpUtility.get_safe_reg_exp(
            PortugueseDateTime.RelativeMonthRegex)
        self._relative_week_day_regex = RegExpUtility.get_safe_reg_exp(
            PortugueseDateTime.RelativeWeekDayRegex)
        self._utility_configuration = config.utility_configuration
        self._date_token_prefix = PortugueseDateTime.DateTokenPrefix
        self._check_both_before_after = PortugueseDateTime.CheckBothBeforeAfter

    def get_swift_day(self, source: str) -> int:
        trimmed_text = self.__normalize(source.strip().lower())
        swift = 0

        # TODO: add the relative day logic if needed. If yes, the whole method should be abstracted.
        if trimmed_text == 'hoje' or trimmed_text == 'este dia':
            swift = 0
        elif trimmed_text == 'amanha' or trimmed_text == 'de amanha' or trimmed_text.endswith('dia seguinte') \
                or trimmed_text.endswith('o dia de amanha') or trimmed_text.endswith('proximo dia'):
            swift = 1
        elif trimmed_text == 'ontem':
            swift = -1
        elif trimmed_text.endswith('dia depois de amanha') or trimmed_text.endswith('depois de amanha'):
            swift = 2
        elif trimmed_text.endswith('anteontem') or trimmed_text.endswith('dia antes de ontem'):
            swift = -2
        elif trimmed_text.endswith('ultimo dia'):
            swift = -1

        return swift

    def get_swift_month(self, source: str) -> int:
        return self.get_swift(source)

    def get_swift(self, source: str) -> int:
        trimmed_text = source.strip().lower()
        swift = 0
        next_prefix_matches = regex.search(
            PortugueseDateParserConfiguration._next_prefix_regex, trimmed_text)
        past_prefix_matches = regex.search(
            PortugueseDateParserConfiguration._past_prefix_regex, trimmed_text)
        if next_prefix_matches:
            swift = 1
        elif past_prefix_matches:
            swift = -1

        return swift

    def is_cardinal_last(self, source: str) -> bool:
        trimmed_text = source.strip().lower()
        return trimmed_text == 'last'

    def __normalize(self, source: str) -> str:
        return source.replace('á', 'a').replace('é', 'e').replace('í', 'i').replace('ó', 'o').replace('ú', 'u')\
            .replace('ê', 'e').replace('ô', 'o').replace('ü', 'u').replace('ã', 'a').replace('õ', 'o').replace('ç', 'c')
