#  Copyright (c) Microsoft Corporation. All rights reserved.
#  Licensed under the MIT License.

from typing import Pattern, Dict

from recognizers_text.utilities import RegExpUtility
from recognizers_number.number.extractors import BaseNumberExtractor
from recognizers_number.number.parsers import BaseNumberParser
from recognizers_number.number.english.extractors import EnglishCardinalExtractor
from recognizers_number.number.english.parsers import EnglishNumberParserConfiguration
from ...resources.english_date_time import EnglishDateTime
from ..extractors import DateTimeExtractor
from ..base_duration import DurationParserConfiguration, BaseDurationExtractor
from .duration_extractor_config import EnglishDurationExtractorConfiguration


class EnglishDurationParserConfiguration(DurationParserConfiguration):
    @property
    def options(self):
        return self._cardinal_extractor

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

    @property
    def duration_extractor(self) -> DateTimeExtractor:
        return self._duration_extractor

    def __init__(self, config):
        self._duration_extractor = BaseDurationExtractor(
            EnglishDurationExtractorConfiguration(), False)
        self._cardinal_extractor: BaseNumberExtractor = EnglishCardinalExtractor()
        self._number_parser: BaseNumberParser = BaseNumberParser(
            EnglishNumberParserConfiguration())
        self._followed_unit: Pattern = RegExpUtility.get_safe_reg_exp(
            EnglishDateTime.DurationFollowedUnit)
        self._suffix_and_regex: Pattern = RegExpUtility.get_safe_reg_exp(
            EnglishDateTime.SuffixAndRegex)
        self._number_combined_with_unit: Pattern = RegExpUtility.get_safe_reg_exp(
            EnglishDateTime.NumberCombinedWithDurationUnit)
        self._an_unit_regex: Pattern = RegExpUtility.get_safe_reg_exp(
            EnglishDateTime.AnUnitRegex)
        self._all_date_unit_regex: Pattern = RegExpUtility.get_safe_reg_exp(
            EnglishDateTime.AllRegex)
        self._half_date_unit_regex: Pattern = RegExpUtility.get_safe_reg_exp(
            EnglishDateTime.HalfRegex)
        self._inexact_number_unit_regex: Pattern = RegExpUtility.get_safe_reg_exp(
            EnglishDateTime.InexactNumberUnitRegex)
        self._unit_map: Dict[str, str] = EnglishDateTime.UnitMap
        self._unit_value_map: Dict[str, int] = EnglishDateTime.UnitValueMap
        self._double_numbers: Dict[str, float] = EnglishDateTime.DoubleNumbers
