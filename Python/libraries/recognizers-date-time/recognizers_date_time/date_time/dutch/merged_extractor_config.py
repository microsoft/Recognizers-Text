#  Copyright (c) Microsoft Corporation. All rights reserved.
#  Licensed under the MIT License.

from typing import List, Pattern

from recognizers_text.extractor import Extractor
from recognizers_text.utilities import RegExpUtility, DefinitionLoader
from recognizers_number import DutchIntegerExtractor
from ...resources.dutch_date_time import DutchDateTime
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
from .date_extractor_config import DutchDateExtractorConfiguration
from .time_extractor_config import DutchTimeExtractorConfiguration
from .duration_extractor_config import DutchDurationExtractorConfiguration
from .dateperiod_extractor_config import DutchDatePeriodExtractorConfiguration
from .timeperiod_extractor_config import DutchTimePeriodExtractorConfiguration
from .datetime_extractor_config import DutchDateTimeExtractorConfiguration
from .datetimeperiod_extractor_config import DutchDateTimePeriodExtractorConfiguration
from .set_extractor_config import DutchSetExtractorConfiguration
from .holiday_extractor_config import DutchHolidayExtractorConfiguration
from ...resources.base_date_time import BaseDateTime


class DutchMergedExtractorConfiguration(MergedExtractorConfiguration):
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
            DutchDateTime.BeforeRegex)
        self._after_regex = RegExpUtility.get_safe_reg_exp(
            DutchDateTime.AfterRegex)
        self._since_regex = RegExpUtility.get_safe_reg_exp(
            DutchDateTime.SinceRegex)
        self._from_to_regex = RegExpUtility.get_safe_reg_exp(
            DutchDateTime.FromToRegex)
        self._year_regex = RegExpUtility.get_safe_reg_exp(
            DutchDateTime.YearRegex)
        self._single_ambiguous_month_regex = RegExpUtility.get_safe_reg_exp(
            DutchDateTime.SingleAmbiguousMonthRegex)
        self._preposition_suffix_regex = RegExpUtility.get_safe_reg_exp(
            DutchDateTime.PrepositionSuffixRegex)
        self._ambiguous_range_modifier_prefix = RegExpUtility.get_safe_reg_exp(
            DutchDateTime.AmbiguousRangeModifierPrefix)
        self._number_ending_pattern = RegExpUtility.get_safe_reg_exp(
            DutchDateTime.NumberEndingPattern)

        self._date_extractor = BaseDateExtractor(
            DutchDateExtractorConfiguration())
        self._time_extractor = BaseTimeExtractor(
            DutchTimeExtractorConfiguration())
        self._date_time_extractor = BaseDateTimeExtractor(
            DutchDateTimeExtractorConfiguration())
        self._date_period_extractor = BaseDatePeriodExtractor(
            DutchDatePeriodExtractorConfiguration())
        self._time_period_extractor = BaseTimePeriodExtractor(
            DutchTimePeriodExtractorConfiguration())
        self._date_time_period_extractor = BaseDateTimePeriodExtractor(
            DutchDateTimePeriodExtractorConfiguration())
        self._duration_extractor = BaseDurationExtractor(
            DutchDurationExtractorConfiguration())
        self._set_extractor = BaseSetExtractor(
            DutchSetExtractorConfiguration())
        self._holiday_extractor = BaseHolidayExtractor(
            DutchHolidayExtractorConfiguration())
        self._integer_extractor = DutchIntegerExtractor()
        self._filter_word_regex_list = []
        self._unspecified_date_period_regex = RegExpUtility.get_safe_reg_exp(
            DutchDateTime.UnspecificDatePeriodRegex
        )
        self._around_regex = DutchDateTime.AroundRegex
        self._equal_regex = BaseDateTime.EqualRegex
        self._suffix_after_regex = RegExpUtility.get_safe_reg_exp(
            DutchDateTime.SuffixAfterRegex
        )
        self._potential_ambiguous_range_regex = RegExpUtility.get_safe_reg_exp(
            DutchDateTime.PotentialAmbiguousRangeRegex
        )
        self._check_both_before_after = DutchDateTime.CheckBothBeforeAfter
        self._term_filter_regexes = [
            RegExpUtility.get_safe_reg_exp(DutchDateTime.OneOnOneRegex),
            RegExpUtility.get_safe_reg_exp(DutchDateTime.SingleAmbiguousTermsRegex),
        ]
        # TODO When the implementation for these properties is added, change the None values to their respective Regexps
        self._superfluous_word_matcher = None
        self._fail_fast_regex = None
        self._datetime_alt_extractor = None
        self._time_zone_extractor = None
        self._ambiguity_filters_dict = DefinitionLoader.load_ambiguity_filters(DutchDateTime.AmbiguityFiltersDict)
