from typing import Pattern
import regex

from recognizers_text.utilities import RegExpUtility
from recognizers_date_time.resources.arabic_date_time import ArabicDateTime
from recognizers_date_time.date_time.extractors import DateTimeExtractor
from recognizers_date_time.date_time.base_date import BaseDateExtractor
from recognizers_date_time.date_time.base_time import BaseTimeExtractor
from recognizers_date_time.date_time.base_duration import BaseDurationExtractor
from recognizers_date_time.date_time.base_datetime import DateTimeExtractorConfiguration
from recognizers_date_time.date_time.arabic.base_configs import ArabicDateTimeUtilityConfiguration
from recognizers_date_time.date_time.arabic.date_extractor_config import ArabicDateExtractorConfiguration
from recognizers_date_time.date_time.arabic.time_extractor_config import ArabicTimeExtractorConfiguration
from recognizers_date_time.date_time.arabic.duration_extractor_config import ArabicDurationExtractorConfiguration


class ArabicDateTimeExtractorConfiguration(DateTimeExtractorConfiguration):
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
    def specific_end_of_regex(self) -> Pattern:
        return self._specific_end_of_regex

    @property
    def unspecific_end_of_regex(self) -> Pattern:
        return self._unspecific_end_of_regex

    @property
    def unit_regex(self) -> Pattern:
        return self._unit_regex

    @property
    def utility_configuration(self) -> ArabicDateTimeUtilityConfiguration:
        return self._utility_configuration

    @property
    def number_as_time_regex(self) -> Pattern:
        return self._number_as_time_regex

    @property
    def date_number_connector_regex(self) -> Pattern:
        return self._date_number_connector_regex

    @property
    def suffix_after_regex(self) -> Pattern:
        return self._suffix_after_regex

    @property
    def year_suffix(self) -> Pattern:
        return self._year_suffix

    @property
    def year_regex(self) -> Pattern:
        return self._year_regex

    @property
    def dmy_date_format(self) -> bool:
        return self._dmy_date_format

    @property
    def specific_time_of_day_regex(self) -> Pattern:
        return self._specific_time_of_day_regex

    @property
    def prefix_day_regex(self) -> Pattern:
        return self._prefix_day_regex

    def __init__(self, dmyDateFormat=True):
        super().__init__()
        self._date_point_extractor = BaseDateExtractor(
            ArabicDateExtractorConfiguration())
        self._time_point_extractor = BaseTimeExtractor(
            ArabicTimeExtractorConfiguration())
        self._duration_extractor = BaseDurationExtractor(
            ArabicDurationExtractorConfiguration())
        self._utility_configuration = ArabicDateTimeUtilityConfiguration()
        self.preposition_regex = RegExpUtility.get_safe_reg_exp(
            ArabicDateTime.PrepositionRegex)
        self._now_regex = RegExpUtility.get_safe_reg_exp(
            ArabicDateTime.NowRegex)
        self._suffix_regex = RegExpUtility.get_safe_reg_exp(
            ArabicDateTime.SuffixRegex)
        self._time_of_today_after_regex = RegExpUtility.get_safe_reg_exp(
            ArabicDateTime.TimeOfTodayAfterRegex)
        self._simple_time_of_today_after_regex = RegExpUtility.get_safe_reg_exp(
            ArabicDateTime.SimpleTimeOfTodayAfterRegex)
        self._night_regex = RegExpUtility.get_safe_reg_exp(
            ArabicDateTime.TimeOfDayRegex)
        self._time_of_today_before_regex = RegExpUtility.get_safe_reg_exp(
            ArabicDateTime.TimeOfTodayBeforeRegex)
        self._simple_time_of_today_before_regex = RegExpUtility.get_safe_reg_exp(
            ArabicDateTime.SimpleTimeOfTodayBeforeRegex)
        self._specific_end_of_regex = RegExpUtility.get_safe_reg_exp(
            ArabicDateTime.SpecificEndOfRegex)
        self._unspecific_end_of_regex = RegExpUtility.get_safe_reg_exp(
            ArabicDateTime.UnspecificEndOfRegex)
        self._unit_regex = RegExpUtility.get_safe_reg_exp(
            ArabicDateTime.TimeUnitRegex)
        self.connector_regex = RegExpUtility.get_safe_reg_exp(
            ArabicDateTime.ConnectorRegex)
        self._number_as_time_regex = RegExpUtility.get_safe_reg_exp(
            ArabicDateTime.NumberAsTimeRegex)
        self._date_number_connector_regex = RegExpUtility.get_safe_reg_exp(
            ArabicDateTime.DateNumberConnectorRegex
        )
        self._suffix_after_regex = RegExpUtility.get_safe_reg_exp(
            ArabicDateTime.SuffixAfterRegex
        )
        self._year_suffix = RegExpUtility.get_safe_reg_exp(
            ArabicDateTime.YearSuffix
        )
        self._year_regex = RegExpUtility.get_safe_reg_exp(
            ArabicDateTime.YearRegex
        )
        self._specific_time_of_day_regex = RegExpUtility.get_safe_reg_exp(
            ArabicDateTime.SpecificTimeOfDayRegex
        )
        self._prefix_day_regex = RegExpUtility.get_safe_reg_exp(
            ArabicDateTime.PrefixDayRegex)

    def is_connector_token(self, source: str) -> bool:
        return source.strip() == '' or regex.search(self.connector_regex, source) is not None or regex.search(self.preposition_regex, source) is not None
