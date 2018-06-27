from typing import Pattern, Dict

from recognizers_text.utilities import RegExpUtility
from recognizers_number.number.extractors import BaseNumberExtractor
from recognizers_number.number.parsers import BaseNumberParser
from ...resources.spanish_date_time import SpanishDateTime
from ..base_duration import DurationParserConfiguration

class SpanishDurationParserConfiguration(DurationParserConfiguration):
    @property
    def cardinal_extractor(self) -> BaseNumberExtractor:
        return self._cardinal_extractor

    @property
    def number_parser(self) -> BaseNumberParser:
        return self._number_parser

    @property
    def followed_unit(self) -> Pattern:
        return self._followed_unit

    @property
    def suffix_and_regex(self) -> Pattern:
        return self._suffix_and_regex

    @property
    def number_combined_with_unit(self) -> Pattern:
        return self._number_combined_with_unit

    @property
    def an_unit_regex(self) -> Pattern:
        return self._an_unit_regex

    @property
    def all_date_unit_regex(self) -> Pattern:
        return self._all_date_unit_regex

    @property
    def half_date_unit_regex(self) -> Pattern:
        return self._half_date_unit_regex

    @property
    def inexact_number_unit_regex(self) -> Pattern:
        return self._inexact_number_unit_regex

    @property
    def unit_map(self) -> Dict[str, str]:
        return self._unit_map

    @property
    def unit_value_map(self) -> Dict[str, int]:
        return self._unit_value_map

    @property
    def double_numbers(self) -> Dict[str, float]:
        return self._double_numbers

    def __init__(self, config):
        self._cardinal_extractor: BaseNumberExtractor = config.cardinal_extractor
        self._number_parser: BaseNumberParser = config.number_parser
        self._followed_unit: Pattern = RegExpUtility.get_safe_reg_exp(SpanishDateTime.FollowedUnit)
        self._suffix_and_regex: Pattern = RegExpUtility.get_safe_reg_exp(SpanishDateTime.SuffixAndRegex)
        self._number_combined_with_unit: Pattern = RegExpUtility.get_safe_reg_exp(SpanishDateTime.DurationNumberCombinedWithUnit)
        self._an_unit_regex: Pattern = RegExpUtility.get_safe_reg_exp(SpanishDateTime.AnUnitRegex)
        self._all_date_unit_regex: Pattern = RegExpUtility.get_safe_reg_exp(SpanishDateTime.AllRegex)
        self._half_date_unit_regex: Pattern = RegExpUtility.get_safe_reg_exp(SpanishDateTime.HalfRegex)
        self._inexact_number_unit_regex: Pattern = RegExpUtility.get_safe_reg_exp(SpanishDateTime.InexactNumberUnitRegex)
        self._unit_map: Dict[str, int] = SpanishDateTime.UnitMap
        self._unit_value_map: Dict[str, int] = SpanishDateTime.UnitValueMap
        self._double_numbers: Dict[str, float] = SpanishDateTime.DoubleNumbers
