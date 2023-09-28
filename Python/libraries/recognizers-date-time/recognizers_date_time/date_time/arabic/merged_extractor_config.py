from typing import List, Pattern

from recognizers_text.extractor import Extractor
from recognizers_text.utilities import RegExpUtility, DefinitionLoader
from recognizers_number import ArabicIntegerExtractor
from recognizers_date_time.resources.arabic_date_time import ArabicDateTime
from recognizers_date_time.date_time.extractors import DateTimeExtractor
from recognizers_date_time.date_time.base_merged import MergedExtractorConfiguration
from recognizers_date_time.date_time.base_date import BaseDateExtractor
from recognizers_date_time.date_time.base_time import BaseTimeExtractor
from recognizers_date_time.date_time.base_duration import BaseDurationExtractor
from recognizers_date_time.date_time.base_dateperiod import BaseDatePeriodExtractor
from recognizers_date_time.date_time.base_timeperiod import BaseTimePeriodExtractor
from recognizers_date_time.date_time.base_datetime import BaseDateTimeExtractor
from recognizers_date_time.date_time.base_datetimeperiod import BaseDateTimePeriodExtractor
from recognizers_date_time.date_time.base_set import BaseSetExtractor
from recognizers_date_time.date_time.base_holiday import BaseHolidayExtractor
from recognizers_date_time.date_time.arabic.date_extractor_config import ArabicDateExtractorConfiguration
from recognizers_date_time.date_time.arabic.time_extractor_config import ArabicTimeExtractorConfiguration
from recognizers_date_time.date_time.arabic.duration_extractor_config import ArabicDurationExtractorConfiguration
from recognizers_date_time.date_time.arabic.dateperiod_extractor_config import ArabicDatePeriodExtractorConfiguration
from recognizers_date_time.date_time.arabic.timeperiod_extractor_config import ArabicTimePeriodExtractorConfiguration
from recognizers_date_time.date_time.arabic.datetime_extractor_config import ArabicDateTimeExtractorConfiguration
from recognizers_date_time.date_time.arabic.datetimeperiod_extractor_config import ArabicDateTimePeriodExtractorConfiguration
from recognizers_date_time.date_time.arabic.set_extractor_config import ArabicSetExtractorConfiguration
from recognizers_date_time.date_time.arabic.holiday_extractor_config import ArabicHolidayExtractorConfiguration
from recognizers_date_time.resources.base_date_time import BaseDateTime


class ArabicMergedExtractorConfiguration(MergedExtractorConfiguration):
    @property
    def check_both_before_after(self):
        return self._check_both_before_after

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
        return self._superfluous_word_matcher

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
    def number_ending_pattern(self) -> Pattern:
        return self._number_ending_pattern

    @property
    def filter_word_regex_list(self) -> List[Pattern]:
        return self._filter_word_regex_list

    @property
    def ambiguous_range_modifier_prefix(self) -> Pattern:
        return None

    @property
    def ambiguity_filters_dict(self) -> Pattern:
        return self._ambiguity_filters_dict

    @property
    def potential_ambiguous_range_regex(self) -> Pattern:
        return self._potential_ambiguous_range_regex

    @property
    def year_regex(self) -> Pattern:
        return self._year_regex

    def __init__(self):
        self._before_regex = RegExpUtility.get_safe_reg_exp(
            ArabicDateTime.BeforeRegex)
        self._after_regex = RegExpUtility.get_safe_reg_exp(
            ArabicDateTime.AfterRegex)
        self._since_regex = RegExpUtility.get_safe_reg_exp(
            ArabicDateTime.SinceRegex)
        self._from_to_regex = RegExpUtility.get_safe_reg_exp(
            ArabicDateTime.FromToRegex)
        self._year_regex = RegExpUtility.get_safe_reg_exp(
            ArabicDateTime.YearRegex)
        self._single_ambiguous_month_regex = RegExpUtility.get_safe_reg_exp(
            ArabicDateTime.SingleAmbiguousMonthRegex)
        self._preposition_suffix_regex = RegExpUtility.get_safe_reg_exp(
            ArabicDateTime.PrepositionSuffixRegex)
        self._ambiguous_range_modifier_prefix = RegExpUtility.get_safe_reg_exp(
            ArabicDateTime.AmbiguousRangeModifierPrefix)
        self._number_ending_pattern = RegExpUtility.get_safe_reg_exp(
            ArabicDateTime.NumberEndingPattern)

        self._date_extractor = BaseDateExtractor(
            ArabicDateExtractorConfiguration())
        self._time_extractor = BaseTimeExtractor(
            ArabicTimeExtractorConfiguration())
        self._date_time_extractor = BaseDateTimeExtractor(
            ArabicDateTimeExtractorConfiguration())
        self._date_period_extractor = BaseDatePeriodExtractor(
            ArabicDatePeriodExtractorConfiguration())
        self._time_period_extractor = BaseTimePeriodExtractor(
            ArabicTimePeriodExtractorConfiguration())
        self._date_time_period_extractor = BaseDateTimePeriodExtractor(
            ArabicDateTimePeriodExtractorConfiguration())
        self._duration_extractor = BaseDurationExtractor(
            ArabicDurationExtractorConfiguration())
        self._set_extractor = BaseSetExtractor(
            ArabicSetExtractorConfiguration())
        self._holiday_extractor = BaseHolidayExtractor(
            ArabicHolidayExtractorConfiguration())
        self._integer_extractor = ArabicIntegerExtractor()
        self._filter_word_regex_list = []
        self._unspecified_date_period_regex = RegExpUtility.get_safe_reg_exp(
            ArabicDateTime.UnspecificDatePeriodRegex
        )
        self._around_regex = ArabicDateTime.AroundRegex
        self._equal_regex = BaseDateTime.EqualRegex
        self._suffix_after_regex = RegExpUtility.get_safe_reg_exp(
            ArabicDateTime.SuffixAfterRegex
        )
        self._potential_ambiguous_range_regex = RegExpUtility.get_safe_reg_exp(
            ArabicDateTime.FromToRegex
        )
        self._check_both_before_after = ArabicDateTime.CheckBothBeforeAfter
        self._term_filter_regexes = [
            RegExpUtility.get_safe_reg_exp(ArabicDateTime.OneOnOneRegex),
            RegExpUtility.get_safe_reg_exp(ArabicDateTime.SingleAmbiguousTermsRegex),
        ]
        # TODO When the implementation for these properties is added, change the None values to their respective Regexps
        self._superfluous_word_matcher = None
        self._fail_fast_regex = None
        self._datetime_alt_extractor = None
        self._time_zone_extractor = None
        self._ambiguity_filters_dict = DefinitionLoader.load_ambiguity_filters(ArabicDateTime.AmbiguityFiltersDict)
