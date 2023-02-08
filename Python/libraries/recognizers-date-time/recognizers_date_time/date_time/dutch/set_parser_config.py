#  Copyright (c) Microsoft Corporation. All rights reserved.
#  Licensed under the MIT License.

from typing import Pattern, Dict

from recognizers_text.utilities import RegExpUtility
from ...resources.dutch_date_time import DutchDateTime
from ..extractors import DateTimeExtractor
from ..parsers import DateTimeParser
from ..base_set import SetParserConfiguration, MatchedTimex
from ..base_configs import BaseDateParserConfiguration


class DutchSetParserConfiguration(SetParserConfiguration):
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

    @property
    def day_type_regex(self) -> Pattern:
        return self._day_type_regex

    @property
    def week_type_regex(self) -> Pattern:
        return self._week_type_regex

    @property
    def bi_week_type_regex(self) -> Pattern:
        return self._bi_week_type_regex

    @property
    def month_type_regex(self) -> Pattern:
        return self._month_type_regex

    @property
    def quarter_type_regex(self) -> Pattern:
        return self._quarter_type_regex

    @property
    def year_type_regex(self) -> Pattern:
        return self._year_type_regex

    @property
    def semi_year_type_regex(self) -> Pattern:
        return self._semi_year_type_regex

    @property
    def weekend_type_regex(self) -> Pattern:
        return self._weekend_type_regex

    def __init__(self, config: BaseDateParserConfiguration):
        self._duration_extractor = config.duration_extractor
        self._time_extractor = config.time_extractor
        self._date_extractor = config.date_extractor
        self._date_time_extractor = config.date_time_extractor
        self._date_period_extractor = config.date_period_extractor
        self._time_period_extractor = config.time_period_extractor
        self._date_time_period_extractor = config.date_time_period_extractor
        self._duration_parser = config.duration_parser
        self._time_parser = config.time_parser
        self._date_parser = config.date_parser
        self._date_time_parser = config.date_time_parser
        self._date_period_parser = config.date_period_parser
        self._time_period_parser = config.time_period_parser
        self._date_time_period_parser = config.date_time_period_parser
        self._unit_map = DutchDateTime.UnitMap

        self._day_type_regex = RegExpUtility.get_safe_reg_exp(DutchDateTime.DayTypeRegex)
        self._week_type_regex = RegExpUtility.get_safe_reg_exp(DutchDateTime.WeekTypeRegex)
        self._bi_week_type_regex = RegExpUtility.get_safe_reg_exp(DutchDateTime.BiWeekTypeRegex)
        self._month_type_regex = RegExpUtility.get_safe_reg_exp(DutchDateTime.MonthTypeRegex)
        self._quarter_type_regex = RegExpUtility.get_safe_reg_exp(DutchDateTime.QuarterTypeRegex)
        self._year_type_regex = RegExpUtility.get_safe_reg_exp(DutchDateTime.YearTypeRegex)
        self._semi_year_type_regex = RegExpUtility.get_safe_reg_exp(DutchDateTime.SemiYearTypeRegex)
        self._weekend_type_regex = RegExpUtility.get_safe_reg_exp(DutchDateTime.WeekendTypeRegex)
        self._each_prefix_regex = RegExpUtility.get_safe_reg_exp(
            DutchDateTime.EachPrefixRegex)
        self._periodic_regex = RegExpUtility.get_safe_reg_exp(
            DutchDateTime.PeriodicRegex)
        self._each_unit_regex = RegExpUtility.get_safe_reg_exp(
            DutchDateTime.EachUnitRegex)
        self._each_day_regex = RegExpUtility.get_safe_reg_exp(
            DutchDateTime.EachDayRegex)
        self._set_week_day_regex = RegExpUtility.get_safe_reg_exp(
            DutchDateTime.SetWeekDayRegex)
        self._set_each_regex = RegExpUtility.get_safe_reg_exp(
            DutchDateTime.SetEachRegex)

    def get_matched_daily_timex(self, text: str) -> MatchedTimex:
        trimmed_source = text.strip().lower()
        timex = ''

        if self._day_type_regex.search(trimmed_source):
            timex = 'P1D'
        elif self._week_type_regex.search(trimmed_source):
            timex = 'P1W'
        elif self._bi_week_type_regex.search(trimmed_source):
            timex = 'P2W'
        elif self._month_type_regex.search(trimmed_source):
            timex = 'P1M'
        elif self._year_type_regex.search(trimmed_source):
            timex = 'P1Y'
        elif self._semi_year_type_regex.search(trimmed_source):
            timex = 'P0.5Y'
        elif self._quarter_type_regex.search(trimmed_source):
            timex = 'P3M'
        elif self._weekend_type_regex.search(trimmed_source):
            timex = 'XXXX-WXX-WE'
        else:
            return MatchedTimex(False, None)

        return MatchedTimex(True, timex)

    def get_matched_unit_timex(self, text: str) -> MatchedTimex:

        return self.get_matched_daily_timex(text)

