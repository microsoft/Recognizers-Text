
from typing import Pattern, Dict
from recognizers_text.utilities import RegExpUtility

from recognizers_date_time.date_time.base_set import SetParserConfiguration, MatchedTimex
from recognizers_date_time.date_time.extractors import DateTimeExtractor
from recognizers_date_time.date_time.english.common_configs import EnglishCommonDateTimeParserConfiguration
from recognizers_date_time.date_time.base_duration import BaseDurationParser
from recognizers_date_time.date_time.base_timeperiod import BaseTimePeriodParser
from recognizers_date_time.date_time.base_time import BaseTimeParser
from recognizers_date_time.date_time.base_date import BaseDateParser
from recognizers_date_time.date_time.base_datetime import BaseDateTimeParser
from recognizers_date_time.date_time.base_dateperiod import BaseDatePeriodParser
from recognizers_date_time.date_time.base_datetimeperiod import BaseDateTimePeriodParser

from recognizers_date_time.resources.english_date_time import EnglishDateTime

class EnglishSetParserConfiguration(SetParserConfiguration):
    @property
    def duration_extractor(self) -> DateTimeExtractor:
        return self._duration_extractor

    @property
    def duration_parser(self) -> BaseDurationParser:
        return self._duration_parser

    @property
    def time_extractor(self) -> DateTimeExtractor:
        return self._time_extractor

    @property
    def time_parser(self) -> BaseTimeParser:
        return self._time_parser

    @property
    def date_extractor(self) -> DateTimeExtractor:
        return self._date_extractor

    @property
    def date_parser(self) -> BaseDateParser:
        return self._date_parser

    @property
    def date_time_extractor(self) -> DateTimeExtractor:
        return self._date_time_extractor

    @property
    def date_time_parser(self) -> BaseDateTimeParser:
        return self._date_time_parser

    @property
    def date_period_extractor(self) -> DateTimeExtractor:
        return self._date_period_extractor

    @property
    def date_period_parser(self) -> BaseDatePeriodParser:
        return self._date_period_parser

    @property
    def time_period_extractor(self) -> DateTimeExtractor:
        return self._time_period_extractor

    @property
    def time_period_parser(self) -> BaseTimePeriodParser:
        return self._time_period_parser

    @property
    def date_time_period_extractor(self) -> DateTimeExtractor:
        return self._date_time_period_extractor

    @property
    def date_time_period_parser(self) -> BaseDateTimePeriodParser:
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

    def __init__(self, config: EnglishCommonDateTimeParserConfiguration):
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
        self._unit_map = EnglishDateTime.UnitMap
        self._each_prefix_regex = RegExpUtility.get_safe_reg_exp(EnglishDateTime.EachPrefixRegex)
        self._periodic_regex = RegExpUtility.get_safe_reg_exp(EnglishDateTime.PeriodicRegex)
        self._each_unit_regex = RegExpUtility.get_safe_reg_exp(EnglishDateTime.EachUnitRegex)
        self._each_day_regex = RegExpUtility.get_safe_reg_exp(EnglishDateTime.EachDayRegex)
        self._set_week_day_regex = RegExpUtility.get_safe_reg_exp(EnglishDateTime.SetWeekDayRegex)
        self._set_each_regex = RegExpUtility.get_safe_reg_exp(EnglishDateTime.SetEachRegex)

    def get_matched_daily_timex(self, text: str) -> MatchedTimex:
        trimmed_text = text.strip().lower()
        if trimmed_text == 'daily':
            timex = 'P1D'
        elif trimmed_text == 'weekly':
            timex = 'P1W'
        elif trimmed_text == 'biweekly':
            timex = 'P2W'
        elif trimmed_text == 'monthly':
            timex = 'P1M'
        elif trimmed_text in ('yearly', 'annually', 'annual'):
            timex = 'P1Y'
        else:
            return MatchedTimex(False, None)

        return MatchedTimex(True, timex)

    def get_matched_unit_timex(self, text: str) -> MatchedTimex:
        trimmed_text = text.strip().lower()
        if trimmed_text == 'day':
            timex = 'P1D'
        elif trimmed_text == 'week':
            timex = 'P1W'
        elif trimmed_text == 'month':
            timex = 'P1M'
        elif trimmed_text == 'year':
            timex = 'P1Y'
        else:
            return MatchedTimex(False, None)

        return MatchedTimex(True, timex)
