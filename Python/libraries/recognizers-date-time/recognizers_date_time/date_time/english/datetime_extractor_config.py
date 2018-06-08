from typing import Pattern
import regex

from recognizers_text.utilities import RegExpUtility
from ...resources.english_date_time import EnglishDateTime
from ..extractors import DateTimeExtractor
from ..utilities import DateTimeUtilityConfiguration
from ..base_date import BaseDateExtractor
from ..base_time import BaseTimeExtractor
from ..base_duration import BaseDurationExtractor
from ..base_datetime import DateTimeExtractorConfiguration
from .base_configs import EnglishDateTimeUtilityConfiguration
from .date_extractor_config import EnglishDateExtractorConfiguration
from .time_extractor_config import EnglishTimeExtractorConfiguration
from .duration_extractor_config import EnglishDurationExtractorConfiguration

class EnglishDateTimeExtractorConfiguration(DateTimeExtractorConfiguration):
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
        self._date_point_extractor = BaseDateExtractor(EnglishDateExtractorConfiguration())
        self._time_point_extractor = BaseTimeExtractor(EnglishTimeExtractorConfiguration())
        self._duration_extractor = BaseDurationExtractor(EnglishDurationExtractorConfiguration())
        self._suffix_regex = RegExpUtility.get_safe_reg_exp(EnglishDateTime.SuffixRegex)
        self._now_regex = RegExpUtility.get_safe_reg_exp(EnglishDateTime.NowRegex)
        self._time_of_today_after_regex = RegExpUtility.get_safe_reg_exp(EnglishDateTime.TimeOfTodayAfterRegex)
        self._simple_time_of_today_after_regex = RegExpUtility.get_safe_reg_exp(EnglishDateTime.SimpleTimeOfTodayAfterRegex)
        self._night_regex = RegExpUtility.get_safe_reg_exp(EnglishDateTime.TimeOfDayRegex)
        self._time_of_today_before_regex = RegExpUtility.get_safe_reg_exp(EnglishDateTime.TimeOfTodayBeforeRegex)
        self._simple_time_of_today_before_regex = RegExpUtility.get_safe_reg_exp(EnglishDateTime.SimpleTimeOfTodayBeforeRegex)
        self._the_end_of_regex = RegExpUtility.get_safe_reg_exp(EnglishDateTime.TheEndOfRegex)
        self._unit_regex = RegExpUtility.get_safe_reg_exp(EnglishDateTime.TimeUnitRegex)
        self._utility_configuration = EnglishDateTimeUtilityConfiguration()
        self.connector_regex = RegExpUtility.get_safe_reg_exp(EnglishDateTime.ConnectorRegex)
        self.preposition_regex = RegExpUtility.get_safe_reg_exp(EnglishDateTime.PrepositionRegex)

    def is_connector_token(self, source: str) -> bool:
        return source.strip() == '' or regex.search(self.connector_regex, source) is not None or regex.search(self.preposition_regex, source) is not None
