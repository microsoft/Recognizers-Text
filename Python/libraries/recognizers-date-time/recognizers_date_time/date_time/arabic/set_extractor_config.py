from typing import Pattern

from recognizers_text.utilities import RegExpUtility
from recognizers_date_time.resources.arabic_date_time import ArabicDateTime
from recognizers_date_time.date_time.extractors import DateTimeExtractor
from recognizers_date_time.date_time.base_set import SetExtractorConfiguration
from recognizers_date_time.date_time.base_date import BaseDateExtractor
from recognizers_date_time.date_time.base_time import BaseTimeExtractor
from recognizers_date_time.date_time.base_duration import BaseDurationExtractor
from recognizers_date_time.date_time.base_dateperiod import BaseDatePeriodExtractor
from recognizers_date_time.date_time.base_timeperiod import BaseTimePeriodExtractor
from recognizers_date_time.date_time.base_datetime import BaseDateTimeExtractor
from recognizers_date_time.date_time.base_datetimeperiod import BaseDateTimePeriodExtractor
from recognizers_date_time.date_time.arabic.date_extractor_config import ArabicDateExtractorConfiguration
from recognizers_date_time.date_time.arabic.time_extractor_config import ArabicTimeExtractorConfiguration
from recognizers_date_time.date_time.arabic.duration_extractor_config import ArabicDurationExtractorConfiguration
from recognizers_date_time.date_time.arabic.dateperiod_extractor_config import ArabicDatePeriodExtractorConfiguration
from recognizers_date_time.date_time.arabic.timeperiod_extractor_config import ArabicTimePeriodExtractorConfiguration
from recognizers_date_time.date_time.arabic.datetime_extractor_config import ArabicDateTimeExtractorConfiguration
from recognizers_date_time.date_time.arabic.datetimeperiod_extractor_config import ArabicDateTimePeriodExtractorConfiguration


class ArabicSetExtractorConfiguration(SetExtractorConfiguration):
    @property
    def last_regex(self) -> Pattern:
        return self._last_regex

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
    def before_each_day_regex(self) -> Pattern:
        return self._before_each_day_regex

    @property
    def set_week_day_regex(self) -> Pattern:
        return self._set_week_day_regex

    @property
    def set_each_regex(self) -> Pattern:
        return self._set_each_regex

    @property
    def duration_extractor(self) -> DateTimeExtractor:
        return self._duration_extractor

    @property
    def time_extractor(self) -> DateTimeExtractor:
        return self._time_extractor

    @property
    def date_extractor(self) -> DateTimeExtractor:
        return self._date_extractor

    @property
    def date_time_extractor(self) -> DateTimeExtractor:
        return self._date_time_extractor

    @property
    def date_period_extractor(self) -> DateTimeExtractor:
        return self._date_period_extractor

    @property
    def time_period_extractor(self) -> DateTimeExtractor:
        return self._time_period_extractor

    @property
    def date_time_period_extractor(self) -> DateTimeExtractor:
        return self._date_time_period_extractor

    @property
    def duration_unit_regex(self) -> Pattern:
        return self._duration_unit_regex

    def __init__(self, dmyDateFormat=False):
        self._duration_extractor = BaseDurationExtractor(
            ArabicDurationExtractorConfiguration())
        self._time_extractor = BaseTimeExtractor(
            ArabicTimeExtractorConfiguration())
        self._date_extractor = BaseDateExtractor(
            ArabicDateExtractorConfiguration())
        self._date_time_extractor = BaseDateTimeExtractor(
            ArabicDateTimeExtractorConfiguration(dmyDateFormat))
        self._date_period_extractor = BaseDatePeriodExtractor(
            ArabicDatePeriodExtractorConfiguration())
        self._time_period_extractor = BaseTimePeriodExtractor(
            ArabicTimePeriodExtractorConfiguration())
        self._date_time_period_extractor = BaseDateTimePeriodExtractor(
            ArabicDateTimePeriodExtractorConfiguration(dmyDateFormat))
        self._last_regex = RegExpUtility.get_safe_reg_exp(
            ArabicDateTime.SetLastRegex)
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
        self._duration_unit_regex = RegExpUtility.get_safe_reg_exp(
            ArabicDateTime.DurationUnitRegex
        )
        self._before_each_day_regex = None
