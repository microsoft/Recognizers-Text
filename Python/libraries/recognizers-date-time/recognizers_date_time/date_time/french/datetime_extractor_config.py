from typing import Pattern
import regex

from recognizers_text.utilities import RegExpUtility
from ...resources.french_date_time import FrenchDateTime
from ..extractors import DateTimeExtractor
from ..utilities import DateTimeUtilityConfiguration
from ..base_date import BaseDateExtractor
from ..base_time import BaseTimeExtractor
from ..base_duration import BaseDurationExtractor
from ..base_datetime import DateTimeExtractorConfiguration
from .base_configs import FrenchDateTimeUtilityConfiguration
from .date_extractor_config import FrenchDateExtractorConfiguration
from .time_extractor_config import FrenchTimeExtractorConfiguration
from .duration_extractor_config import FrenchDurationExtractorConfiguration

class FrenchDateTimeExtractorConfiguration(DateTimeExtractorConfiguration):
    @property
    def date_point_extractor(self) -> DateTimeExtractor:
        return self._date_point_extractor

    @property
    def time_point_extractor(self) -> DateTimeExtractor:
        return self._time_point_extractor

    @property
    def duration_extractor(self) -> DateTimeExtractor:
        return self._duration_extractor

    @property
    def suffix_regex(self) -> Pattern:
        return self._suffix_regex

    @property
    def now_regex(self) -> Pattern:
        return self._now_regex

    @property
    def time_of_today_after_regex(self) -> Pattern:
        return self._time_of_today_after_regex

    @property
    def time_of_day_regex(self) -> Pattern:
        return self._time_of_day_regex

    @property
    def specific_time_of_day_regex(self) -> Pattern:
        return self._specific_time_of_day_regex

    @property
    def simple_time_of_today_after_regex(self) -> Pattern:
        return self._simple_time_of_today_after_regex

    @property
    def night_regex(self) -> Pattern:
        return self._night_regex

    @property
    def time_of_today_before_regex(self) -> Pattern:
        return self._time_of_today_before_regex

    @property
    def simple_time_of_today_before_regex(self) -> Pattern:
        return self._simple_time_of_today_before_regex

    @property
    def the_end_of_regex(self) -> Pattern:
        return self._the_end_of_regex

    @property
    def unit_regex(self) -> Pattern:
        return self._unit_regex

    @property
    def utility_configuration(self) -> DateTimeUtilityConfiguration:
        return self._utility_configuration

    def __init__(self):
        self.preposition_regex = RegExpUtility.get_safe_reg_exp(FrenchDateTime.PrepositionRegex)
        self._now_regex = RegExpUtility.get_safe_reg_exp(FrenchDateTime.NowRegex)
        self._suffix_regex = RegExpUtility.get_safe_reg_exp(FrenchDateTime.SuffixRegex)

        self._time_of_day_regex = RegExpUtility.get_safe_reg_exp(FrenchDateTime.TimeOfDayRegex)
        self._specific_time_of_day_regex = RegExpUtility.get_safe_reg_exp(FrenchDateTime.SpecificTimeOfDayRegex)
        self._time_of_today_after_regex = RegExpUtility.get_safe_reg_exp(FrenchDateTime.TimeOfTodayAfterRegex)
        self._time_of_today_before_regex = RegExpUtility.get_safe_reg_exp(FrenchDateTime.TimeOfTodayBeforeRegex)
        self._simple_time_of_today_after_regex = RegExpUtility.get_safe_reg_exp(FrenchDateTime.SimpleTimeOfTodayAfterRegex)
        self._simple_time_of_today_before_regex = RegExpUtility.get_safe_reg_exp(FrenchDateTime.SimpleTimeOfTodayBeforeRegex)
        self._the_end_of_regex = RegExpUtility.get_safe_reg_exp(FrenchDateTime.TheEndOfRegex)
        self._unit_regex = RegExpUtility.get_safe_reg_exp(FrenchDateTime.TimeUnitRegex)
        self.connector_regex = RegExpUtility.get_safe_reg_exp(FrenchDateTime.ConnectorRegex)
        self._night_regex = RegExpUtility.get_safe_reg_exp(FrenchDateTime.NightRegex)

        self._date_point_extractor = BaseDateExtractor(FrenchDateExtractorConfiguration())
        self._time_point_extractor = BaseTimeExtractor(FrenchTimeExtractorConfiguration())
        self._duration_extractor = BaseDurationExtractor(FrenchDurationExtractorConfiguration())
        self._utility_configuration = FrenchDateTimeUtilityConfiguration()

    def is_connector_token(self, source: str) -> bool:
        return (
            source == '' or source == ',' or
            regex.search(self.preposition_regex, source) is not None or
            source == 't' or
            source == 'pour' or
            source == 'vers'
            )
