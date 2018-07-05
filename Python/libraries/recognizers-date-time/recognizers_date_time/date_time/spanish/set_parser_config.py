from typing import Pattern, Dict

from recognizers_text.utilities import RegExpUtility
from ...resources.spanish_date_time import SpanishDateTime
from ..extractors import DateTimeExtractor
from ..parsers import DateTimeParser
from ..base_set import SetParserConfiguration, MatchedTimex
from ..base_configs import BaseDateParserConfiguration

class SpanishSetParserConfiguration(SetParserConfiguration):
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
        self._unit_map = SpanishDateTime.UnitMap

        self._each_prefix_regex = RegExpUtility.get_safe_reg_exp(SpanishDateTime.EachPrefixRegex)
        self._periodic_regex = RegExpUtility.get_safe_reg_exp(SpanishDateTime.PeriodicRegex)
        self._each_unit_regex = RegExpUtility.get_safe_reg_exp(SpanishDateTime.EachUnitRegex)
        self._each_day_regex = RegExpUtility.get_safe_reg_exp(SpanishDateTime.EachDayRegex)
        self._set_week_day_regex = RegExpUtility.get_safe_reg_exp(SpanishDateTime.SetWeekDayRegex)
        self._set_each_regex = RegExpUtility.get_safe_reg_exp(SpanishDateTime.SetEachRegex)

    def get_matched_daily_timex(self, text: str) -> MatchedTimex:
        trimmed_text = text.strip().lower()
        timex = ''

        if trimmed_text.endswith('diario') or trimmed_text.endswith('diariamente'):
            timex = 'P1D'
        elif trimmed_text == 'semanalmente':
            timex = 'P1W'
        elif trimmed_text == 'quincenalmente':
            timex = 'P2W'
        elif trimmed_text == 'mensualmente':
            timex = 'P1M'
        elif trimmed_text == 'anualmente':
            timex = 'P1Y'
        else:
            return MatchedTimex(False, None)

        return MatchedTimex(True, timex)

    def get_matched_unit_timex(self, text: str) -> MatchedTimex:
        trimmed_text = text.strip().lower()
        timex = ''

        if (
                trimmed_text == 'día' or trimmed_text == 'dia' or
                trimmed_text == 'días' or trimmed_text == 'dias'
            ):
            timex = 'P1D'
        elif trimmed_text == 'semana' or trimmed_text == 'semanas':
            timex = 'P1W'
        elif trimmed_text == 'mes' or trimmed_text == 'meses':
            timex = 'P1M'
        elif trimmed_text == 'año' or trimmed_text == 'años':
            timex = 'P1Y'
        else:
            return MatchedTimex(False, None)

        return MatchedTimex(True, timex)
