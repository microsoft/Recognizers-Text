from typing import Pattern, List

from recognizers_text import RegExpUtility
from ...resources.chinese_date_time import ChineseDateTime, BaseDateTime

from ..extractors import DateTimeExtractor
from ..base_merged import MergedExtractorConfiguration
from ..base_holiday import BaseHolidayExtractor
from .duration_extractor import ChineseDurationExtractor
from .time_extractor import ChineseTimeExtractor
from .date_extractor import ChineseDateExtractor
from .datetime_extractor import ChineseDateTimeExtractor
from .timeperiod_extractor import ChineseTimePeriodExtractor
from .dateperiod_extractor import ChineseDatePeriodExtractor
from .datetimeperiod_extractor import ChineseDateTimePeriodExtractor
from .set_extractor import ChineseSetExtractor
from .holiday_extractor_config import ChineseHolidayExtractorConfiguration


class ChineseMergedExtractorConfiguration(MergedExtractorConfiguration):

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
            ChineseDateTime.ParserConfigurationSinceSuffix
        )
        self._since_prefix_regex = RegExpUtility.get_safe_reg_exp(
            ChineseDateTime.ParserConfigurationSincePrefix
        )
        self._until_regex = RegExpUtility.get_safe_reg_exp(
            ChineseDateTime.ParserConfigurationUntil
        )
        self._after_regex = RegExpUtility.get_safe_reg_exp(
            ChineseDateTime.AfterRegex
        )
        self._before_regex = RegExpUtility.get_safe_reg_exp(
            ChineseDateTime.BeforeRegex
        )
        self._ambiguity_filters_dict = ChineseDateTime.AmbiguityFiltersDict
        self._date_extractor = ChineseDateExtractor()
        self._time_extractor = ChineseTimeExtractor()
        self._date_time_extractor = ChineseDateTimeExtractor()
        self._date_period_extractor = ChineseDatePeriodExtractor()
        self._time_period_extractor = ChineseTimePeriodExtractor()
        self._date_time_period_extractor = ChineseDateTimePeriodExtractor()
        self._holiday_extractor = BaseHolidayExtractor(
            ChineseHolidayExtractorConfiguration())
        self._duration_extractor = ChineseDurationExtractor()
        self._set_extractor = ChineseSetExtractor()
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
