from typing import Pattern

from recognizers_text.utilities import RegExpUtility
from ...resources.french_date_time import FrenchDateTime
from ..extractors import DateTimeExtractor
from ..base_set import SetExtractorConfiguration
from ..base_date import BaseDateExtractor
from ..base_time import BaseTimeExtractor
from ..base_duration import BaseDurationExtractor
from ..base_dateperiod import BaseDatePeriodExtractor
from ..base_timeperiod import BaseTimePeriodExtractor
from ..base_datetime import BaseDateTimeExtractor
from ..base_datetimeperiod import BaseDateTimePeriodExtractor
from .date_extractor_config import FrenchDateExtractorConfiguration
from .time_extractor_config import FrenchTimeExtractorConfiguration
from .duration_extractor_config import FrenchDurationExtractorConfiguration
from .dateperiod_extractor_config import FrenchDatePeriodExtractorConfiguration
from .timeperiod_extractor_config import FrenchTimePeriodExtractorConfiguration
from .datetime_extractor_config import FrenchDateTimeExtractorConfiguration
from .datetimeperiod_extractor_config import FrenchDateTimePeriodExtractorConfiguration

class FrenchSetExtractorConfiguration(SetExtractorConfiguration):
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
        self._last_regex = RegExpUtility.get_safe_reg_exp(FrenchDateTime.SetLastRegex)
        self._periodic_regex = RegExpUtility.get_safe_reg_exp(FrenchDateTime.PeriodicRegex)
        self._each_unit_regex = RegExpUtility.get_safe_reg_exp(FrenchDateTime.EachUnitRegex)
        self._each_prefix_regex = RegExpUtility.get_safe_reg_exp(FrenchDateTime.EachPrefixRegex)
        self._each_day_regex = RegExpUtility.get_safe_reg_exp(FrenchDateTime.EachDayRegex)
        self._before_each_day_regex = None
        self._set_each_regex = RegExpUtility.get_safe_reg_exp(FrenchDateTime.SetEachRegex)
        self._set_week_day_regex = RegExpUtility.get_safe_reg_exp(FrenchDateTime.SetWeekDayRegex)

        self._duration_extractor = BaseDurationExtractor(FrenchDurationExtractorConfiguration())
        self._time_extractor = BaseTimeExtractor(FrenchTimeExtractorConfiguration())
        self._date_extractor = BaseDateExtractor(FrenchDateExtractorConfiguration())
        self._date_time_extractor = BaseDateTimeExtractor(FrenchDateTimeExtractorConfiguration())
        self._date_period_extractor = BaseDatePeriodExtractor(FrenchDatePeriodExtractorConfiguration())
        self._time_period_extractor = BaseTimePeriodExtractor(FrenchTimePeriodExtractorConfiguration())
        self._date_time_period_extractor = BaseDateTimePeriodExtractor(FrenchDateTimePeriodExtractorConfiguration())
