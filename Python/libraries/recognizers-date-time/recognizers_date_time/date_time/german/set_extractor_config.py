#  Copyright (c) Microsoft Corporation. All rights reserved.
#  Licensed under the MIT License.

from typing import Pattern

from recognizers_text.utilities import RegExpUtility
from ...resources.german_date_time import germanDateTime
from ..extractors import DateTimeExtractor
from ..base_set import SetExtractorConfiguration
from ..base_date import BaseDateExtractor
from ..base_time import BaseTimeExtractor
from ..base_duration import BaseDurationExtractor
from ..base_dateperiod import BaseDatePeriodExtractor
from ..base_timeperiod import BaseTimePeriodExtractor
from ..base_datetime import BaseDateTimeExtractor
from ..base_datetimeperiod import BaseDateTimePeriodExtractor
from .date_extractor_config import germanDateExtractorConfiguration
from .time_extractor_config import germanTimeExtractorConfiguration
from .duration_extractor_config import germanDurationExtractorConfiguration
from .dateperiod_extractor_config import germanDatePeriodExtractorConfiguration
from .timeperiod_extractor_config import germanTimePeriodExtractorConfiguration
from .datetime_extractor_config import germanDateTimeExtractorConfiguration
from .datetimeperiod_extractor_config import germanDateTimePeriodExtractorConfiguration


class germanSetExtractorConfiguration(SetExtractorConfiguration):
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

    def __init__(self):
        self._duration_extractor = BaseDurationExtractor(
            germanDurationExtractorConfiguration())
        self._time_extractor = BaseTimeExtractor(
            germanTimeExtractorConfiguration())
        self._date_extractor = BaseDateExtractor(
            germanDateExtractorConfiguration())
        self._date_time_extractor = BaseDateTimeExtractor(
            germanDateTimeExtractorConfiguration())
        self._date_period_extractor = BaseDatePeriodExtractor(
            germanDatePeriodExtractorConfiguration())
        self._time_period_extractor = BaseTimePeriodExtractor(
            germanTimePeriodExtractorConfiguration())
        self._date_time_period_extractor = BaseDateTimePeriodExtractor(
            germanDateTimePeriodExtractorConfiguration())
        self._last_regex = RegExpUtility.get_safe_reg_exp(
            germanDateTime.SetLastRegex)
        self._each_prefix_regex = RegExpUtility.get_safe_reg_exp(
            germanDateTime.EachPrefixRegex)
        self._periodic_regex = RegExpUtility.get_safe_reg_exp(
            germanDateTime.PeriodicRegex)
        self._each_unit_regex = RegExpUtility.get_safe_reg_exp(
            germanDateTime.EachUnitRegex)
        self._each_day_regex = RegExpUtility.get_safe_reg_exp(
            germanDateTime.EachDayRegex)
        self._set_week_day_regex = RegExpUtility.get_safe_reg_exp(
            germanDateTime.SetWeekDayRegex)
        self._set_each_regex = RegExpUtility.get_safe_reg_exp(
            germanDateTime.SetEachRegex)
        self._duration_unit_regex = RegExpUtility.get_safe_reg_exp(
            germanDateTime.DurationUnitRegex
        )
        self._before_each_day_regex = None
