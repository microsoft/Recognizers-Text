#  Copyright (c) Microsoft Corporation. All rights reserved.
#  Licensed under the MIT License.

from typing import Pattern, Dict

from recognizers_text.utilities import RegExpUtility
from ...resources.german_date_time import GermanDateTime
from ..extractors import DateTimeExtractor
from ..parsers import DateTimeParser
from ..base_set import SetParserConfiguration, MatchedTimex
from ..base_configs import BaseDateParserConfiguration


class GermanSetParserConfiguration(SetParserConfiguration):
    @property
    def duration_extractor(self) -> DateTimeExtractor:
        return self._duration_extractor

    @property
    def duration_parser(self) -> DateTimeParser:
        return self._duration_parser

    @property
    def time_extractor(self) -> DateTimeExtractor:
        return self._time_extractor

    @property
    def time_parser(self) -> DateTimeParser:
        return self._time_parser

    @property
    def date_extractor(self) -> DateTimeExtractor:
        return self._date_extractor

    @property
    def date_parser(self) -> DateTimeParser:
        return self._date_parser

    @property
    def date_time_extractor(self) -> DateTimeExtractor:
        return self._date_time_extractor

    @property
    def date_time_parser(self) -> DateTimeParser:
        return self._date_time_parser

    @property
    def date_period_extractor(self) -> DateTimeExtractor:
        return self._date_period_extractor

    @property
    def date_period_parser(self) -> DateTimeParser:
        return self._date_period_parser

    @property
    def time_period_extractor(self) -> DateTimeExtractor:
        return self._time_period_extractor

    @property
    def time_period_parser(self) -> DateTimeParser:
        return self._time_period_parser

    @property
    def date_time_period_extractor(self) -> DateTimeExtractor:
        return self._date_time_period_extractor

    @property
    def date_time_period_parser(self) -> DateTimeParser:
        return self._date_time_period_parser

    @property
    def unit_map(self) -> Dict[str, str]:
        return self._unit_map

    @property
    def each_prefix_regex(self) -> Pattern:
        return self._each_prefix_regex

    @property
    def periodic_regex(self) -> Pattern:
        return self._periodic_regex

    @property
    def each_unit_regex(self) -> Pattern:
        return self._each_unit_regex

    @property
    def each_day_regex(self) -> Pattern:
        return self._each_day_regex

    @property
    def set_week_day_regex(self) -> Pattern:
        return self._set_week_day_regex

    @property
    def set_each_regex(self) -> Pattern:
        return self._set_each_regex

    def __init__(self, config: BaseDateParserConfiguration):
        self._duration_extractor = config.duration_extractor
        self._duration_parser = config.duration_parser
        self._time_extractor = config.time_extractor
        self._time_parser = config.time_parser
        self._date_extractor = config.date_extractor
        self._date_parser = config.date_parser
        self._date_time_extractor = config.date_time_extractor
        self._date_time_parser = config.date_time_parser
        self._date_period_extractor = config.date_period_extractor
        self._date_period_parser = config.date_period_parser
        self._time_period_extractor = config.time_period_extractor
        self._time_period_parser = config.time_period_parser
        self._date_time_period_extractor = config.date_time_period_extractor
        self._date_time_period_parser = config.date_time_period_parser
        self._unit_map = GermanDateTime.UnitMap
        self._each_prefix_regex = RegExpUtility.get_safe_reg_exp(
            GermanDateTime.EachPrefixRegex)
        self._periodic_regex = RegExpUtility.get_safe_reg_exp(
            GermanDateTime.PeriodicRegex)
        self._each_unit_regex = RegExpUtility.get_safe_reg_exp(
            GermanDateTime.EachUnitRegex)
        self._each_day_regex = RegExpUtility.get_safe_reg_exp(
            GermanDateTime.EachDayRegex)
        self._set_week_day_regex = RegExpUtility.get_safe_reg_exp(
            GermanDateTime.SetWeekDayRegex)
        self._set_each_regex = RegExpUtility.get_safe_reg_exp(
            GermanDateTime.SetEachRegex)

    def get_matched_daily_timex(self, text: str) -> MatchedTimex:
        trimmed_text = text.strip().lower()
        if trimmed_text in ['täglich', "täglicher", "tägliches", "tägliche", "täglichen", "alltäglich", "alltäglicher", "alltägliches", "alltägliche", "alltäglichen", "jeden Tag"]:
            timex = 'P1D'
        elif trimmed_text in ['wöchentlich', "wöchentliche", "wöchentliches", "wöchentlicher", "wöchentlichen"]:
            timex = 'P1W'
        elif trimmed_text in ['zweiwöchentlich', "zweiwöchentlichen", "zweiwöchentliche", "zweiwöchentliches"]:
            timex = 'P2W'
        elif trimmed_text in ['monatlich', "monatliche", "monatlicher", "monatlichen", "monatliches"]:
            timex = 'P1M'
        elif trimmed_text in ['quartalsmäßig', "quartalsmäßige", "quartalsmäßigen", "quartalsmäßiges", "quartalsmäßiger"]:
            timex = 'P3M'
        elif trimmed_text in ["jährlich", "jährliche", "jährliches", "jährlichen", "jährlicher"]:
            timex = 'P1Y'
        else:
            return MatchedTimex(False, None)

        return MatchedTimex(True, timex)

    def get_matched_unit_timex(self, text: str) -> MatchedTimex:
        trimmed_text = text.strip().lower()
        if trimmed_text == 'tag':
            timex = 'P1D'
        elif trimmed_text == 'woche':
            timex = 'P1W'
        elif trimmed_text == 'monat':
            timex = 'P1M'
        elif trimmed_text == 'jahr':
            timex = 'P1Y'
        else:
            return MatchedTimex(False, None)

        return MatchedTimex(True, timex)

    def week_day_group_match_string(match):
        # TO DO
        pass
