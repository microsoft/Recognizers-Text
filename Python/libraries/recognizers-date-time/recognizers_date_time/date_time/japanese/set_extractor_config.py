from typing import Pattern

from recognizers_date_time.date_time.extractors import DateTimeExtractor
from recognizers_date_time.date_time.CJK import BaseCJKDurationExtractor, BaseCJKTimeExtractor, BaseCJKDateExtractor, \
    BaseCJKDateTimeExtractor, BaseCJKDatePeriodExtractor, BaseCJKTimePeriodExtractor, BaseCJKDateTimePeriodExtractor, \
    CJKSetExtractorConfiguration
from recognizers_date_time.resources.japanese_date_time import JapaneseDateTime
from recognizers_date_time.date_time.japanese.time_extractor_config import JapaneseTimeExtractorConfiguration
from recognizers_date_time.date_time.japanese.timeperiod_extractor_config import \
    JapaneseTimePeriodExtractorConfiguration
from recognizers_date_time.date_time.japanese.date_extractor_config import JapaneseDateExtractorConfiguration
from recognizers_date_time.date_time.japanese.dateperiod_extractor_config import \
    JapaneseDatePeriodExtractorConfiguration
from recognizers_date_time.date_time.japanese.duration_extractor_config import JapaneseDurationExtractorConfiguration
from recognizers_date_time.date_time.japanese.datetime_extractor_config import JapaneseDateTimeExtractorConfiguration
from recognizers_date_time.date_time.japanese.datetimeperiod_extractor_config import \
    JapaneseDateTimePeriodExtractorConfiguration
from recognizers_text import RegExpUtility


class JapaneseSetExtractorConfiguration(CJKSetExtractorConfiguration):

    @property
    def unit_regex(self) -> Pattern:
        return self._unit_regex

    @property
    def each_unit_regex(self) -> Pattern:
        return self._each_unit_regex

    @property
    def each_prefix_regex(self) -> Pattern:
        return self._each_prefix_regex

    @property
    def each_suffix_regex(self) -> Pattern:
        return self._each_suffix_regex

    @property
    def last_regex(self) -> Pattern:
        return self._last_regex
    
    @property
    def each_day_regex(self) -> Pattern:
        return self._each_day_regex

    @property
    def each_date_unit_regex(self) -> Pattern:
        return self._each_date_unit_regex

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
        super().__init__()
        self._unit_regex = RegExpUtility.get_safe_reg_exp(JapaneseDateTime.SetUnitRegex)
        self._each_unit_regex = RegExpUtility.get_safe_reg_exp(JapaneseDateTime.SetEachUnitRegex)
        self._each_prefix_regex = RegExpUtility.get_safe_reg_exp(JapaneseDateTime.SetEachPrefixRegex)
        self._each_suffix_regex = RegExpUtility.get_safe_reg_exp(JapaneseDateTime.SetEachSuffixRegex)
        self._last_regex = RegExpUtility.get_safe_reg_exp(JapaneseDateTime.SetLastRegex)
        self._each_day_regex = RegExpUtility.get_safe_reg_exp(JapaneseDateTime.SetEachDayRegex)
        self._each_date_unit_regex = RegExpUtility.get_safe_reg_exp(JapaneseDateTime.SetEachDateUnitRegex)
        self._duration_extractor = BaseCJKDurationExtractor(JapaneseDurationExtractorConfiguration())
        self._time_extractor = BaseCJKTimeExtractor(JapaneseTimeExtractorConfiguration())
        self._date_extractor = BaseCJKDateExtractor(JapaneseDateExtractorConfiguration())
        self._date_time_extractor = BaseCJKDateTimeExtractor(JapaneseDateTimeExtractorConfiguration())
        self._date_period_extractor = BaseCJKDatePeriodExtractor(JapaneseDatePeriodExtractorConfiguration())
        self._time_period_extractor = BaseCJKTimePeriodExtractor(JapaneseTimePeriodExtractorConfiguration())
        self._date_time_period_extractor = BaseCJKDateTimePeriodExtractor(
            JapaneseDateTimePeriodExtractorConfiguration())
    
    