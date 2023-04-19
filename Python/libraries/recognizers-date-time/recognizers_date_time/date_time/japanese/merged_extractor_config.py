#  Copyright (c) Microsoft Corporation. All rights reserved.
#  Licensed under the MIT License.

from typing import Pattern, List

from recognizers_text import RegExpUtility
from ...resources.japanese_date_time import JapaneseDateTime, BaseDateTime

from ..extractors import DateTimeExtractor
from ..base_merged import MergedExtractorConfiguration
from ..base_holiday import BaseHolidayExtractor
# from .duration_extractor import JapaneseDurationExtractor
from .time_extractor import JapaneseTimeExtractor
# from .date_extractor import JapaneseDateExtractor
# from .datetime_extractor import JapaneseDateTimeExtractor
from .timeperiod_extractor import JapaneseTimePeriodExtractor
# from .dateperiod_extractor import JapaneseDatePeriodExtractor
# from .datetimeperiod_extractor import JapaneseDateTimePeriodExtractor
# from .set_extractor import JapaneseSetExtractor
# from .holiday_extractor_config import JapaneseHolidayExtractorConfiguration


class JapaneseMergedExtractorConfiguration(MergedExtractorConfiguration):

    @property
    def time_zone_extractor(self) -> any:
        return self._time_zone_extractor

    @property
    def datetime_alt_extractor(self) -> any:
        return self._datetime_alt_extractor

    @property
    def unspecified_date_period_regex(self) -> Pattern:
        return self._unspecified_date_period_regex

    @property
    def term_filter_regexes(self) -> List[Pattern]:
        return self._term_filter_regexes

    @property
    def around_regex(self) -> Pattern:
        return self._around_regex

    @property
    def ambiguous_range_modifier_prefix(self) -> Pattern:
        return self._ambiguous_range_modifier_prefix

    @property
    def potential_ambiguous_range_regex(self) -> Pattern:
        return self._potential_ambiguous_range_regex

    @property
    def suffix_after_regex(self) -> Pattern:
        return self._suffix_after_regex

    @property
    def fail_fast_regex(self) -> Pattern:
        return self._fail_fast_regex

    @property
    def superfluous_word_matcher(self) -> Pattern:
        return self._superfluous_word_matcher

    @property
    def ambiguity_filters_dict(self) -> {}:
        return self._ambiguity_filters_dict

    # @property
    # def date_extractor(self) -> DateTimeExtractor:
    #     return self._date_extractor

    @property
    def time_extractor(self) -> DateTimeExtractor:
        return self._time_extractor

    # @property
    # def date_time_extractor(self) -> DateTimeExtractor:
    #     return self._date_time_extractor
    #
    # @property
    # def date_period_extractor(self) -> DateTimeExtractor:
    #     return self._date_period_extractor

    @property
    def time_period_extractor(self) -> DateTimeExtractor:
        return self._time_period_extractor

    # @property
    # def date_time_period_extractor(self) -> DateTimeExtractor:
    #     return self._date_time_period_extractor
    #
    # @property
    # def holiday_extractor(self) -> DateTimeExtractor:
    #     return self._holiday_extractor
    #
    # @property
    # def duration_extractor(self) -> DateTimeExtractor:
    #     return self._duration_extractor
    #
    # @property
    # def set_extractor(self) -> DateTimeExtractor:
    #     return self._set_extractor

    @property
    def integer_extractor(self) -> any:
        return None

    @property
    def after_regex(self) -> Pattern:
        return self._after_regex

    @property
    def since_regex(self) -> any:
        return None

    @property
    def before_regex(self) -> Pattern:
        return self._before_regex

    @property
    def from_to_regex(self) -> any:
        return None

    @property
    def single_ambiguous_month_regex(self) -> any:
        return None

    @property
    def preposition_suffix_regex(self) -> any:
        return None

    @property
    def number_ending_pattern(self) -> any:
        return None

    @property
    def filter_word_regex_list(self) -> any:
        return None

    @property
    def until_regex(self) -> Pattern:
        return self._until_regex

    @property
    def since_prefix_regex(self) -> Pattern:
        return self._since_prefix_regex

    @property
    def since_suffix_regex(self) -> Pattern:
        return self._since_suffix_regex

    @property
    def equal_regex(self) -> Pattern:
        return self._equal_regex

    def __init__(self):
        self._equal_regex = RegExpUtility.get_safe_reg_exp(
            BaseDateTime.EqualRegex
        )
        self._since_suffix_regex = RegExpUtility.get_safe_reg_exp(
            JapaneseDateTime.ParserConfigurationSinceSuffix
        )
        self._since_prefix_regex = RegExpUtility.get_safe_reg_exp(
            JapaneseDateTime.ParserConfigurationSincePrefix
        )
        self._until_regex = RegExpUtility.get_safe_reg_exp(
            JapaneseDateTime.ParserConfigurationUntil
        )
        self._after_regex = RegExpUtility.get_safe_reg_exp(
            JapaneseDateTime.AfterRegex
        )
        self._before_regex = RegExpUtility.get_safe_reg_exp(
            JapaneseDateTime.BeforeRegex
        )
        self._ambiguity_filters_dict = JapaneseDateTime.AmbiguityFiltersDict
        # self._date_extractor = JapaneseDateExtractor()
        self._time_extractor = JapaneseTimeExtractor()
        # self._date_time_extractor = JapaneseDateTimeExtractor()
        # self._date_period_extractor = JapaneseDatePeriodExtractor()
        self._time_period_extractor = JapaneseTimePeriodExtractor()
        # self._date_time_period_extractor = JapaneseDateTimePeriodExtractor()
        # self._holiday_extractor = BaseHolidayExtractor(
        #     JapaneseHolidayExtractorConfiguration())
        # self._duration_extractor = JapaneseDurationExtractor()
        # self._set_extractor = JapaneseSetExtractor()
        # TODO When the implementation for these properties is added, change the None values to their respective Regexps
        self._superfluous_word_matcher = None
        self._fail_fast_regex = None
        self._unspecified_date_period_regex = None
        self._suffix_after_regex = None
        self._potential_ambiguous_range_regex = None
        self._ambiguous_range_modifier_prefix = None
        self._around_regex = None
        self._term_filter_regexes = None
        self._datetime_alt_extractor = None
        self._time_zone_extractor = None
