from typing import Pattern, Dict

from recognizers_text.utilities import RegExpUtility
from recognizers_date_time.resources.arabic_date_time import ArabicDateTime
from recognizers_date_time.date_time.extractors import DateTimeExtractor
from recognizers_date_time.date_time.parsers import DateTimeParser
from recognizers_date_time.date_time.base_set import SetParserConfiguration, MatchedTimex
from recognizers_date_time.date_time.base_configs import BaseDateParserConfiguration


class ArabicSetParserConfiguration(SetParserConfiguration):
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
        self._unit_map = ArabicDateTime.UnitMap
        self._each_prefix_regex = RegExpUtility.get_safe_reg_exp(
            ArabicDateTime.EachPrefixRegex)
        self._periodic_regex = RegExpUtility.get_safe_reg_exp(
            ArabicDateTime.PeriodicRegex)
        self._each_unit_regex = RegExpUtility.get_safe_reg_exp(
            ArabicDateTime.EachUnitRegex)
        self._each_day_regex = RegExpUtility.get_safe_reg_exp(
            ArabicDateTime.EachDayRegex)
        self._set_week_day_regex = RegExpUtility.get_safe_reg_exp(
            ArabicDateTime.SetWeekDayRegex)
        self._set_each_regex = RegExpUtility.get_safe_reg_exp(
            ArabicDateTime.SetEachRegex)

        self._day_type_regex = RegExpUtility.get_safe_reg_exp(ArabicDateTime.DayTypeRegex)
        self._week_type_regex = RegExpUtility.get_safe_reg_exp(ArabicDateTime.WeekTypeRegex)
        self._month_type_regex = RegExpUtility.get_safe_reg_exp(ArabicDateTime.MonthTypeRegex)
        self._quarter_type_regex = RegExpUtility.get_safe_reg_exp(ArabicDateTime.QuarterTypeRegex)
        self._year_type_regex = RegExpUtility.get_safe_reg_exp(ArabicDateTime.YearTypeRegex)
        self._weekend_type_regex = RegExpUtility.get_safe_reg_exp(ArabicDateTime.WeekendTypeRegex)

    def get_matched_daily_timex(self, text: str) -> MatchedTimex:
        trimmed_source = text.strip().lower()

        if self._day_type_regex.search(trimmed_source):
            timex = 'P1D'
        elif self._week_type_regex.search(trimmed_source):
            timex = 'P1W'
        elif self._month_type_regex.search(trimmed_source):
            timex = 'P1M'
        elif self._year_type_regex.search(trimmed_source):
            timex = 'P1Y'
        elif self._quarter_type_regex.search(trimmed_source):
            timex = 'P3M'
        elif self._weekend_type_regex.search(trimmed_source):
            timex = 'XXXX-WXX-WE'
        else:
            return MatchedTimex(False, None)

        return MatchedTimex(True, timex)

    def get_matched_unit_timex(self, text: str) -> MatchedTimex:
        return self.get_matched_daily_timex(text)
