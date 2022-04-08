#  Copyright (c) Microsoft Corporation. All rights reserved.
#  Licensed under the MIT License.

from typing import Pattern

from recognizers_text.utilities import RegExpUtility
from ...resources.german_date_time import GermanDateTime
from ..extractors import DateTimeExtractor
from ..base_set import SetExtractorConfiguration
from ..base_date import BaseDateExtractor
from ..base_time import BaseTimeExtractor
from ..base_duration import BaseDurationExtractor
from ..base_dateperiod import BaseDatePeriodExtractor
from ..base_timeperiod import BaseTimePeriodExtractor
from ..base_datetime import BaseDateTimeExtractor
from ..base_datetimeperiod import BaseDateTimePeriodExtractor
from .date_extractor_config import GermanDateExtractorConfiguration
from .time_extractor_config import GermanTimeExtractorConfiguration
from .duration_extractor_config import GermanDurationExtractorConfiguration
from .dateperiod_extractor_config import GermanDatePeriodExtractorConfiguration
from .timeperiod_extractor_config import GermanTimePeriodExtractorConfiguration
from .datetime_extractor_config import GermanDateTimeExtractorConfiguration
from .datetimeperiod_extractor_config import GermanDateTimePeriodExtractorConfiguration


class GermanSetExtractorConfiguration(SetExtractorConfiguration):
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

    def __init__(self):
        self._last_regex = RegExpUtility.get_safe_reg_exp(
            GermanDateTime.SetLastRegex)
        self._periodic_regex = RegExpUtility.get_safe_reg_exp(
            GermanDateTime.PeriodicRegex)
        self._each_unit_regex = RegExpUtility.get_safe_reg_exp(
            GermanDateTime.EachUnitRegex)
        self._each_prefix_regex = RegExpUtility.get_safe_reg_exp(
            GermanDateTime.EachPrefixRegex)
        self._each_day_regex = RegExpUtility.get_safe_reg_exp(
            GermanDateTime.EachDayRegex)
        self._before_each_day_regex = None
        self._set_each_regex = RegExpUtility.get_safe_reg_exp(
            GermanDateTime.SetEachRegex)
        self._set_week_day_regex = RegExpUtility.get_safe_reg_exp(
            GermanDateTime.SetWeekDayRegex)

        self._duration_extractor = BaseDurationExtractor(
            GermanDurationExtractorConfiguration())
        self._time_extractor = BaseTimeExtractor(
            GermanTimeExtractorConfiguration())
        self._date_extractor = BaseDateExtractor(
            GermanDateExtractorConfiguration())
        self._date_time_extractor = BaseDateTimeExtractor(
            GermanDateTimeExtractorConfiguration())
        self._date_period_extractor = BaseDatePeriodExtractor(
            GermanDatePeriodExtractorConfiguration())
        self._time_period_extractor = BaseTimePeriodExtractor(
            GermanTimePeriodExtractorConfiguration())
        self._date_time_period_extractor = BaseDateTimePeriodExtractor(
            GermanDateTimePeriodExtractorConfiguration())
