from typing import List, Pattern

from recognizers_text.extractor import Extractor
from recognizers_text.utilities import RegExpUtility
from recognizers_number import EnglishIntegerExtractor
from ...resources.english_date_time import EnglishDateTime
from ..extractors import DateTimeExtractor
from ..base_merged import MergedExtractorConfiguration
from ..base_date import BaseDateExtractor
from ..base_time import BaseTimeExtractor
from ..base_duration import BaseDurationExtractor
from ..base_dateperiod import BaseDatePeriodExtractor
from ..base_timeperiod import BaseTimePeriodExtractor
from ..base_datetime import BaseDateTimeExtractor
from ..base_datetimeperiod import BaseDateTimePeriodExtractor
from ..base_set import BaseSetExtractor
from ..base_holiday import BaseHolidayExtractor
from .date_extractor_config import EnglishDateExtractorConfiguration
from .time_extractor_config import EnglishTimeExtractorConfiguration
from .duration_extractor_config import EnglishDurationExtractorConfiguration
from .dateperiod_extractor_config import EnglishDatePeriodExtractorConfiguration
from .timeperiod_extractor_config import EnglishTimePeriodExtractorConfiguration
from .datetime_extractor_config import EnglishDateTimeExtractorConfiguration
from .datetimeperiod_extractor_config import EnglishDateTimePeriodExtractorConfiguration
from .set_extractor_config import EnglishSetExtractorConfiguration
from .holiday_extractor_config import EnglishHolidayExtractorConfiguration

class EnglishMergedExtractorConfiguration(MergedExtractorConfiguration):
    @property
    def date_extractor(self) -> DateTimeExtractor:
        return self._date_extractor

    @property
    def time_extractor(self) -> DateTimeExtractor:
        return self._time_extractor

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
    def holiday_extractor(self) -> DateTimeExtractor:
        return self._holiday_extractor

    @property
    def duration_extractor(self) -> DateTimeExtractor:
        return self._duration_extractor

    @property
    def set_extractor(self) -> DateTimeExtractor:
        return self._set_extractor

    @property
    def integer_extractor(self) -> Extractor:
        return self._integer_extractor

    @property
    def after_regex(self) -> Pattern:
        return self._after_regex

    @property
    def before_regex(self) -> Pattern:
        return self._before_regex

    @property
    def since_regex(self) -> Pattern:
        return self._since_regex

    @property
    def from_to_regex(self) -> Pattern:
        return self._from_to_regex

    @property
    def single_ambiguous_month_regex(self) -> Pattern:
        return self._single_ambiguous_month_regex

    @property
    def preposition_suffix_regex(self) -> Pattern:
        return self._preposition_suffix_regex

    @property
    def number_ending_pattern(self) -> Pattern:
        return self._number_ending_pattern

    @property
    def filter_word_regex_list(self) -> List[Pattern]:
        return self._filter_word_regex_list

    def __init__(self):
        self._integer_extractor = EnglishIntegerExtractor()
        self._date_extractor = BaseDateExtractor(EnglishDateExtractorConfiguration())
        self._time_extractor = BaseTimeExtractor(EnglishTimeExtractorConfiguration())
        self._duration_extractor = BaseDurationExtractor(EnglishDurationExtractorConfiguration())
        self._date_period_extractor = BaseDatePeriodExtractor(EnglishDatePeriodExtractorConfiguration())
        self._time_period_extractor = BaseTimePeriodExtractor(EnglishTimePeriodExtractorConfiguration())
        self._date_time_extractor = BaseDateTimeExtractor(EnglishDateTimeExtractorConfiguration())
        self._date_time_period_extractor = BaseDateTimePeriodExtractor(EnglishDateTimePeriodExtractorConfiguration())
        self._set_extractor = BaseSetExtractor(EnglishSetExtractorConfiguration())
        self._holiday_extractor = BaseHolidayExtractor(EnglishHolidayExtractorConfiguration())
        self._after_regex = RegExpUtility.get_safe_reg_exp(EnglishDateTime.AfterRegex)
        self._before_regex = RegExpUtility.get_safe_reg_exp(EnglishDateTime.BeforeRegex)
        self._since_regex = RegExpUtility.get_safe_reg_exp(EnglishDateTime.SinceRegex)
        self._from_to_regex = RegExpUtility.get_safe_reg_exp(EnglishDateTime.FromToRegex)
        self._single_ambiguous_month_regex = RegExpUtility.get_safe_reg_exp(EnglishDateTime.SingleAmbiguousMonthRegex)
        self._preposition_suffix_regex = RegExpUtility.get_safe_reg_exp(EnglishDateTime.PrepositionSuffixRegex)
        self._number_ending_pattern = RegExpUtility.get_safe_reg_exp(EnglishDateTime.NumberEndingPattern)
        self._filter_word_regex_list = [
            RegExpUtility.get_safe_reg_exp(EnglishDateTime.OneOnOneRegex)
        ]
