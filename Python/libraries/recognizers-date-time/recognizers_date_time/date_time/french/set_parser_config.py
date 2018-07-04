from typing import Pattern, Dict

from recognizers_text.utilities import RegExpUtility
from ...resources.french_date_time import FrenchDateTime
from ..extractors import DateTimeExtractor
from ..parsers import DateTimeParser
from ..base_set import SetParserConfiguration, MatchedTimex
from ..base_configs import BaseDateParserConfiguration

class FrenchSetParserConfiguration(SetParserConfiguration):
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
        self._unit_map = FrenchDateTime.UnitMap

        self._each_prefix_regex = RegExpUtility.get_safe_reg_exp(FrenchDateTime.EachPrefixRegex)
        self._periodic_regex = RegExpUtility.get_safe_reg_exp(FrenchDateTime.PeriodicRegex)
        self._each_unit_regex = RegExpUtility.get_safe_reg_exp(FrenchDateTime.EachUnitRegex)
        self._each_day_regex = RegExpUtility.get_safe_reg_exp(FrenchDateTime.EachDayRegex)
        self._set_week_day_regex = RegExpUtility.get_safe_reg_exp(FrenchDateTime.SetWeekDayRegex)
        self._set_each_regex = RegExpUtility.get_safe_reg_exp(FrenchDateTime.SetEachRegex)

    def get_matched_daily_timex(self, text: str) -> MatchedTimex:
        trimmed_text = text.strip().lower()
        timex = ''

        if (
                trimmed_text == 'quotidien' or trimmed_text == 'quotidienne' or
                trimmed_text == 'jours' or trimmed_text == 'journellement'
            ):
            timex = 'P1D'
        elif trimmed_text == 'hebdomadaire':
            timex = 'P1W'
        elif trimmed_text == 'bihebdomadaire':
            timex = 'P2W'
        elif trimmed_text == 'mensuel' or trimmed_text == 'mensuelle':
            timex = 'P1M'
        elif trimmed_text == 'annuel' or trimmed_text == 'annuellement':
            timex = 'P1Y'
        else:
            return MatchedTimex(False, None)

        return MatchedTimex(True, timex)

    def get_matched_unit_timex(self, text: str) -> MatchedTimex:
        trimmed_text = text.strip().lower()
        timex = ''

        if trimmed_text == 'jour' or trimmed_text == 'journee':
            timex = 'P1D'
        elif trimmed_text == 'semaine':
            timex = 'P1W'
        elif trimmed_text == 'mois':
            timex = 'P1M'
        elif trimmed_text == 'an' or trimmed_text == 'annee':
            timex = 'P1Y'
        else:
            return MatchedTimex(False, None)

        return MatchedTimex(True, timex)
