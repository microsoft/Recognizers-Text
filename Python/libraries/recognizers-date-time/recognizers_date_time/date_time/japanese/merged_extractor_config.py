#  Copyright (c) Microsoft Corporation. All rights reserved.
#  Licensed under the MIT License.

from typing import Pattern

from recognizers_text.utilities import RegExpUtility, DefinitionLoader
from recognizers_date_time.date_time.extractors import DateTimeExtractor
from recognizers_date_time.date_time.CJK import CJKMergedExtractorConfiguration, BaseCJKTimeExtractor, \
    BaseCJKTimePeriodExtractor

from recognizers_date_time.date_time.japanese.time_extractor_config import JapaneseTimeExtractorConfiguration
from recognizers_date_time.date_time.japanese.timeperiod_extractor_config import \
    JapaneseTimePeriodExtractorConfiguration
from recognizers_date_time.date_time.japanese.date_extractor_config import JapaneseDateExtractorConfiguration
from recognizers_date_time.date_time.japanese.dateperiod_extractor_config import \
    JapaneseDatePeriodExtractorConfiguration
from recognizers_date_time.date_time.CJK.base_dateperiod import BaseCJKDatePeriodExtractor
from recognizers_date_time.date_time.CJK.base_date import BaseCJKDateExtractor

from recognizers_date_time.resources.base_date_time import BaseDateTime
from recognizers_date_time.resources.japanese_date_time import JapaneseDateTime


class JapaneseMergedExtractorConfiguration(CJKMergedExtractorConfiguration):

    @property
    def before_regex(self) -> Pattern:
        return self._before_regex

    @property
    def unspecified_date_period_regex(self) -> Pattern:
        return self._unspecified_date_period_regex

    @property
    def after_regex(self) -> Pattern:
        return self._after_regex

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
    def around_prefix_regex(self) -> Pattern:
        return self._around_prefix_regex

    @property
    def around_suffix_regex(self) -> Pattern:
        return self._around_suffix_regex

    @property
    def equal_regex(self) -> Pattern:
        return self._equal_regex

    @property
    def potential_ambiguous_range_regex(self) -> Pattern:
        return self._potential_ambiguous_range_regex

    @property
    def ambiguous_range_modifier_prefix(self) -> Pattern:
        return self._ambiguous_range_modifier_prefix

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
    def duration_extractor(self) -> DateTimeExtractor:
        return self._duration_extractor

    @property
    def set_extractor(self) -> DateTimeExtractor:
        return self._set_extractor

    @property
    def holiday_extractor(self) -> DateTimeExtractor:
        return self._holiday_extractor

    @property
    def ambiguity_filters_dict(self) -> {}:
        return self._ambiguity_filters_dict

    def __init__(self):
        super().__init__()
        self._ambiguity_filters_dict = DefinitionLoader.load_ambiguity_filters(JapaneseDateTime.AmbiguityFiltersDict)
        self._date_extractor = BaseCJKDateExtractor(JapaneseDateExtractorConfiguration())
        self._time_extractor = BaseCJKTimeExtractor(JapaneseTimeExtractorConfiguration())
        self._date_time_extractor = None
        self._date_period_extractor = BaseCJKDatePeriodExtractor(JapaneseDatePeriodExtractorConfiguration())
        self._time_period_extractor = BaseCJKTimePeriodExtractor(JapaneseTimePeriodExtractorConfiguration())
        self._date_time_period_extractor = None
        self._duration_extractor = None
        self._set_extractor = None
        self._holiday_extractor = None

        self._before_regex = RegExpUtility.get_safe_reg_exp(JapaneseDateTime.BeforeRegex)
        self._unspecified_date_period_regex = RegExpUtility.get_safe_reg_exp(JapaneseDateTime.UnspecificDatePeriodRegex)
        self._after_regex = RegExpUtility.get_safe_reg_exp(JapaneseDateTime.AfterRegex)
        self._until_regex = RegExpUtility.get_safe_reg_exp(JapaneseDateTime.ParserConfigurationUntil)
        self._since_prefix_regex = RegExpUtility.get_safe_reg_exp(JapaneseDateTime.ParserConfigurationSincePrefix)
        self._since_suffix_regex = RegExpUtility.get_safe_reg_exp(JapaneseDateTime.ParserConfigurationSinceSuffix)
        self._around_prefix_regex = RegExpUtility.get_safe_reg_exp(JapaneseDateTime.ParserConfigurationAroundPrefix)
        self._around_suffix_regex = RegExpUtility.get_safe_reg_exp(JapaneseDateTime.ParserConfigurationAroundSuffix)

        self._equal_regex = RegExpUtility.get_safe_reg_exp(BaseDateTime.EqualRegex)
        self._potential_ambiguous_range_regex = RegExpUtility.get_safe_reg_exp(JapaneseDateTime.FromToRegex)
        self._ambiguous_range_modifier_prefix = RegExpUtility.get_safe_reg_exp(
            JapaneseDateTime.AmbiguousRangeModifierPrefix)
