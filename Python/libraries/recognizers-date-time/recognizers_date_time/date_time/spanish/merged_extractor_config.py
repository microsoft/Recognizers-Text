from typing import List, Pattern

from recognizers_text.extractor import Extractor
from recognizers_text.utilities import RegExpUtility
from recognizers_number import SpanishIntegerExtractor
from ...resources.spanish_date_time import SpanishDateTime
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
from .date_extractor_config import SpanishDateExtractorConfiguration
from .time_extractor_config import SpanishTimeExtractorConfiguration
from .duration_extractor_config import SpanishDurationExtractorConfiguration
from .dateperiod_extractor_config import SpanishDatePeriodExtractorConfiguration
from .timeperiod_extractor_config import SpanishTimePeriodExtractorConfiguration
from .datetime_extractor_config import SpanishDateTimeExtractorConfiguration
from .datetimeperiod_extractor_config import SpanishDateTimePeriodExtractorConfiguration
from .set_extractor_config import SpanishSetExtractorConfiguration
from .holiday_extractor_config import SpanishHolidayExtractorConfiguration
from ...resources.base_date_time import BaseDateTime


class SpanishMergedExtractorConfiguration(MergedExtractorConfiguration):
    @property
    def time_zone_extractor(self):
        return self._time_zone_extractor

    @property
    def datetime_alt_extractor(self):
        return self._datetime_alt_extractor

    @property
    def term_filter_regexes(self) -> List[Pattern]:
        return self._term_filter_regexes

    @property
    def fail_fast_regex(self) -> Pattern:
        return self._fail_fast_regex

    @property
    def superfluous_word_matcher(self) -> Pattern:
        return self._superfluous_work_matcher

    @property
    def ambiguity_filters_dict(self) -> Pattern:
        return self._ambiguity_filters_dict

    @property
    def unspecified_date_period_regex(self) -> Pattern:
        return self._unspecified_date_period_regex

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
    def around_regex(self) -> Pattern:
        return self._around_regex

    @property
    def equal_regex(self) -> Pattern:
        return self._equal_regex

    @property
    def suffix_after_regex(self) -> Pattern:
        return self._suffix_after_regex

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
    def ambiguous_range_modifier_prefix(self) -> None:
        return None

    @property
    def potential_ambiguous_range_regex(self) -> None:
        return None

    @property
    def number_ending_pattern(self) -> Pattern:
        return self._number_ending_pattern

    @property
    def filter_word_regex_list(self) -> List[Pattern]:
        return self._filter_word_regex_list

    def __init__(self):
        self._ambiguity_filters_dict = None
        self._superfluous_work_matcher = None
        self._fail_fast_regex = None
        self._term_filter_regexes = None
        self._datetime_alt_extractor = None
        self._time_zone_extractor = None
        self._before_regex = RegExpUtility.get_safe_reg_exp(
            SpanishDateTime.BeforeRegex)
        self._after_regex = RegExpUtility.get_safe_reg_exp(
            SpanishDateTime.AfterRegex)
        self._since_regex = RegExpUtility.get_safe_reg_exp(
            SpanishDateTime.SinceRegex)
        self._from_to_regex = RegExpUtility.get_safe_reg_exp(
            SpanishDateTime.FromToRegex)
        self._single_ambiguous_month_regex = RegExpUtility.get_safe_reg_exp(
            SpanishDateTime.SingleAmbiguousMonthRegex)
        self._preposition_suffix_regex = RegExpUtility.get_safe_reg_exp(
            SpanishDateTime.PrepositionSuffixRegex)
        self._ambiguous_range_modifier_prefix = RegExpUtility.get_safe_reg_exp(
            SpanishDateTime.AmbiguousRangeModifierPrefix)
        self._number_ending_pattern = RegExpUtility.get_safe_reg_exp(
            SpanishDateTime.NumberEndingPattern)

        self._date_extractor = BaseDateExtractor(
            SpanishDateExtractorConfiguration())
        self._time_extractor = BaseTimeExtractor(
            SpanishTimeExtractorConfiguration())
        self._date_time_extractor = BaseDateTimeExtractor(
            SpanishDateTimeExtractorConfiguration())
        self._date_period_extractor = BaseDatePeriodExtractor(
            SpanishDatePeriodExtractorConfiguration())
        self._time_period_extractor = BaseTimePeriodExtractor(
            SpanishTimePeriodExtractorConfiguration())
        self._date_time_period_extractor = BaseDateTimePeriodExtractor(
            SpanishDateTimePeriodExtractorConfiguration())
        self._duration_extractor = BaseDurationExtractor(
            SpanishDurationExtractorConfiguration())
        self._set_extractor = BaseSetExtractor(
            SpanishSetExtractorConfiguration())
        self._holiday_extractor = BaseHolidayExtractor(
            SpanishHolidayExtractorConfiguration())
        self._integer_extractor = SpanishIntegerExtractor()
        self._filter_word_regex_list = []
        self._unspecified_date_period_regex = RegExpUtility.get_safe_reg_exp(
            SpanishDateTime.UnspecificDatePeriodRegex
        )
        self._around_regex = SpanishDateTime.AroundRegex
        self._equal_regex = BaseDateTime.EqualRegex
        self._suffix_after_regex = RegExpUtility.get_safe_reg_exp(
            SpanishDateTime.SuffixAfterRegex
        )
