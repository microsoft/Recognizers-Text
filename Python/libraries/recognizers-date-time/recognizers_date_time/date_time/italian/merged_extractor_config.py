#  Copyright (c) Microsoft Corporation. All rights reserved.
#  Licensed under the MIT License.

from typing import List, Pattern

from recognizers_text.extractor import Extractor
from recognizers_text.utilities import RegExpUtility, DefinitionLoader
from recognizers_number import ItalianIntegerExtractor
from ...resources.italian_date_time import ItalianDateTime
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
from .date_extractor_config import ItalianDateExtractorConfiguration
from .time_extractor_config import ItalianTimeExtractorConfiguration
from .duration_extractor_config import ItalianDurationExtractorConfiguration
from .dateperiod_extractor_config import ItalianDatePeriodExtractorConfiguration
from .timeperiod_extractor_config import ItalianTimePeriodExtractorConfiguration
from .datetime_extractor_config import ItalianDateTimeExtractorConfiguration
from .datetimeperiod_extractor_config import ItalianDateTimePeriodExtractorConfiguration
from .set_extractor_config import ItalianSetExtractorConfiguration
from .holiday_extractor_config import ItalianHolidayExtractorConfiguration
from ...resources.base_date_time import BaseDateTime


class ItalianMergedExtractorConfiguration(MergedExtractorConfiguration):
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
    def potential_ambiguous_range_regex(self) -> Pattern:
        return None

    @property
    def ambiguity_filters_dict(self) -> Pattern:
        return self._ambiguity_filters_dict

    def __init__(self):
        self._before_regex = RegExpUtility.get_safe_reg_exp(
            ItalianDateTime.BeforeRegex)
        self._after_regex = RegExpUtility.get_safe_reg_exp(
            ItalianDateTime.AfterRegex)
        self._since_regex = RegExpUtility.get_safe_reg_exp(
            ItalianDateTime.SinceRegex)
        self._from_to_regex = RegExpUtility.get_safe_reg_exp(
            ItalianDateTime.FromToRegex)
        self._single_ambiguous_month_regex = RegExpUtility.get_safe_reg_exp(
            ItalianDateTime.SingleAmbiguousMonthRegex)
        self._preposition_suffix_regex = RegExpUtility.get_safe_reg_exp(
            ItalianDateTime.PrepositionSuffixRegex)
        self._ambiguous_range_modifier_prefix = RegExpUtility.get_safe_reg_exp(
            ItalianDateTime.AmbiguousRangeModifierPrefix)
        self._number_ending_pattern = RegExpUtility.get_safe_reg_exp(
            ItalianDateTime.NumberEndingPattern)

        self._date_extractor = BaseDateExtractor(
            ItalianDateExtractorConfiguration())
        self._time_extractor = BaseTimeExtractor(
            ItalianTimeExtractorConfiguration())
        self._date_time_extractor = BaseDateTimeExtractor(
            ItalianDateTimeExtractorConfiguration())
        self._date_period_extractor = BaseDatePeriodExtractor(
            ItalianDatePeriodExtractorConfiguration())
        self._time_period_extractor = BaseTimePeriodExtractor(
            ItalianTimePeriodExtractorConfiguration())
        self._date_time_period_extractor = BaseDateTimePeriodExtractor(
            ItalianDateTimePeriodExtractorConfiguration())
        self._duration_extractor = BaseDurationExtractor(
            ItalianDurationExtractorConfiguration())
        self._set_extractor = BaseSetExtractor(
            ItalianSetExtractorConfiguration())
        self._holiday_extractor = BaseHolidayExtractor(
            ItalianHolidayExtractorConfiguration())
        self._integer_extractor = ItalianIntegerExtractor()
        self._filter_word_regex_list = []
        self._unspecified_date_period_regex = RegExpUtility.get_safe_reg_exp(
            ItalianDateTime.UnspecificDatePeriodRegex
        )
        self._around_regex = ItalianDateTime.AroundRegex
        self._equal_regex = BaseDateTime.EqualRegex
        self._suffix_after_regex = RegExpUtility.get_safe_reg_exp(
            ItalianDateTime.SuffixAfterRegex
        )
        self._check_both_before_after = ItalianDateTime.CheckBothBeforeAfter
        # TODO When the implementation for these properties is added, change the None values to their respective Regexps
        self._superfluous_word_matcher = None
        self._fail_fast_regex = None
        self._term_filter_regexes = None
        self._datetime_alt_extractor = None
        self._time_zone_extractor = None
        self._ambiguity_filters_dict = DefinitionLoader.load_ambiguity_filters(ItalianDateTime.AmbiguityFiltersDict)
