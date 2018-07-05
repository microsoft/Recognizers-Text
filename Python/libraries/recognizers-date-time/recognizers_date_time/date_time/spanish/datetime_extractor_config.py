from typing import Pattern
import regex

from recognizers_text.utilities import RegExpUtility
from ...resources.spanish_date_time import SpanishDateTime
from ..extractors import DateTimeExtractor
from ..utilities import DateTimeUtilityConfiguration
from ..base_date import BaseDateExtractor
from ..base_time import BaseTimeExtractor
from ..base_duration import BaseDurationExtractor
from ..base_datetime import DateTimeExtractorConfiguration
from .base_configs import SpanishDateTimeUtilityConfiguration
from .date_extractor_config import SpanishDateExtractorConfiguration
from .time_extractor_config import SpanishTimeExtractorConfiguration
from .duration_extractor_config import SpanishDurationExtractorConfiguration

class SpanishDateTimeExtractorConfiguration(DateTimeExtractorConfiguration):
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
        self.preposition_regex = RegExpUtility.get_safe_reg_exp(SpanishDateTime.PrepositionRegex)
        self._now_regex = RegExpUtility.get_safe_reg_exp(SpanishDateTime.NowRegex)
        self._suffix_regex = RegExpUtility.get_safe_reg_exp(SpanishDateTime.SuffixRegex)

        self._time_of_day_regex = RegExpUtility.get_safe_reg_exp(SpanishDateTime.TimeOfDayRegex)
        self._specific_time_of_day_regex = RegExpUtility.get_safe_reg_exp(SpanishDateTime.SpecificTimeOfDayRegex)
        self._time_of_today_after_regex = RegExpUtility.get_safe_reg_exp(SpanishDateTime.TimeOfTodayAfterRegex)
        self._time_of_today_before_regex = RegExpUtility.get_safe_reg_exp(SpanishDateTime.TimeOfTodayBeforeRegex)
        self._simple_time_of_today_after_regex = RegExpUtility.get_safe_reg_exp(SpanishDateTime.SimpleTimeOfTodayAfterRegex)
        self._simple_time_of_today_before_regex = RegExpUtility.get_safe_reg_exp(SpanishDateTime.SimpleTimeOfTodayBeforeRegex)
        self._the_end_of_regex = RegExpUtility.get_safe_reg_exp(SpanishDateTime.TheEndOfRegex)
        self._unit_regex = RegExpUtility.get_safe_reg_exp(SpanishDateTime.TimeUnitRegex)
        self.connector_regex = RegExpUtility.get_safe_reg_exp(SpanishDateTime.ConnectorRegex)
        self._night_regex = RegExpUtility.get_safe_reg_exp(SpanishDateTime.TimeOfDayRegex)

        self._date_point_extractor = BaseDateExtractor(SpanishDateExtractorConfiguration())
        self._time_point_extractor = BaseTimeExtractor(SpanishTimeExtractorConfiguration())
        self._duration_extractor = BaseDurationExtractor(SpanishDurationExtractorConfiguration())
        self._utility_configuration = SpanishDateTimeUtilityConfiguration()

    def is_connector_token(self, source: str) -> bool:
        return source.strip() == '' or regex.search(self.connector_regex, source) is not None or regex.search(self.preposition_regex, source) is not None
