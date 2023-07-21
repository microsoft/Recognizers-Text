from typing import Pattern

from recognizers_text.utilities import RegExpUtility
from recognizers_number.number.japanese.extractors import JapaneseCardinalExtractor
from recognizers_date_time.date_time.extractors import DateTimeExtractor
from recognizers_date_time.date_time.CJK import CJKDateTimePeriodExtractorConfiguration, BaseCJKDateExtractor, \
    BaseCJKTimeExtractor, BaseCJKDateTimeExtractor, BaseCJKDurationExtractor, BaseCJKTimePeriodExtractor
from recognizers_date_time.date_time.japanese.date_extractor_config import JapaneseDateExtractorConfiguration
from recognizers_date_time.date_time.japanese.time_extractor_config import JapaneseTimeExtractorConfiguration
from recognizers_date_time.date_time.japanese.datetime_extractor_config import JapaneseDateTimeExtractorConfiguration
from recognizers_date_time.date_time.japanese.duration_extractor_config import JapaneseDurationExtractorConfiguration
from recognizers_date_time.date_time.japanese.timeperiod_extractor_config import \
    JapaneseTimePeriodExtractorConfiguration
from recognizers_date_time.date_time.utilities import MatchedIndex
from recognizers_date_time.resources.japanese_date_time import JapaneseDateTime
from recognizers_text import Extractor


class JapaneseDateTimePeriodExtractorConfiguration(CJKDateTimePeriodExtractorConfiguration):
    @property
    def till_regex(self) -> Pattern:
        return self._till_regex

    @property
    def from_prefix_regex(self) -> Pattern:
        return self._from_prefix_regex

    @property
    def from_suffix_regex(self) -> Pattern:
        return self._from_suffix_regex

    @property
    def connector_regex(self) -> Pattern:
        return self._connector_regex

    @property
    def preposition_regex(self) -> Pattern:
        return self._preposition_regex

    @property
    def zhijian_regex(self) -> Pattern:
        return self._zhijian_regex

    @property
    def time_of_day_regex(self) -> Pattern:
        return self._time_of_day_regex

    @property
    def specific_time_of_day_regex(self) -> Pattern:
        return self._specific_time_of_day_regex

    @property
    def unit_regex(self) -> Pattern:
        return self._unit_regex

    @property
    def followed_unit(self) -> Pattern:
        return self._followed_unit

    @property
    def past_regex(self) -> Pattern:
        return self._past_regex

    @property
    def future_regex(self) -> Pattern:
        return self._future_regex

    @property
    def weekday_regex(self) -> Pattern:
        return self._weekday_regex

    @property
    def time_period_left_regex(self) -> Pattern:
        return self._time_period_left_regex

    @property
    def relative_regex(self) -> Pattern:
        return self._relative_regex

    @property
    def rest_of_date_regex(self) -> Pattern:
        return self._rest_of_date_regex

    @property
    def am_pm_desc_regex(self) -> Pattern:
        return self._am_pm_desc_regex

    @property
    def before_after_regex(self) -> Pattern:
        return self._before_after_regex

    @property
    def hour_regex(self) -> Pattern:
        return self._hour_regex

    @property
    def hour_num_regex(self) -> Pattern:
        return self._hour_num_regex

    @property
    def this_regex(self) -> Pattern:
        return self._this_regex

    @property
    def last_regex(self) -> Pattern:
        return self._last_regex

    @property
    def next_regex(self) -> Pattern:
        return self._next_regex

    @property
    def number_combined_with_unit(self) -> Pattern:
        return self._number_combined_with_unit

    @property
    def cardinal_extractor(self) -> Extractor:
        return self._cardinal_extractor

    @property
    def single_date_extractor(self) -> DateTimeExtractor:
        return self._single_date_extractor

    @property
    def single_time_extractor(self) -> DateTimeExtractor:
        return self._single_time_extractor

    @property
    def single_date_time_extractor(self) -> DateTimeExtractor:
        return self._single_date_time_extractor

    @property
    def duration_extractor(self) -> DateTimeExtractor:
        return self._duration_extractor

    @property
    def time_period_extractor(self) -> DateTimeExtractor:
        return self._time_period_extractor

    def __init__(self):
        super().__init__()

        self._cardinal_extractor = JapaneseCardinalExtractor()

        self._single_date_extractor = BaseCJKDateExtractor(JapaneseDateExtractorConfiguration())
        self._single_time_extractor = BaseCJKTimeExtractor(JapaneseTimeExtractorConfiguration())
        self._single_date_time_extractor = BaseCJKDateTimeExtractor(JapaneseDateTimeExtractorConfiguration())
        self._duration_extractor = BaseCJKDurationExtractor(JapaneseDurationExtractorConfiguration())
        self._time_period_extractor = BaseCJKTimePeriodExtractor(JapaneseTimePeriodExtractorConfiguration())

        self._from_prefix_regex = RegExpUtility.get_safe_reg_exp(JapaneseDateTime.DateTimePeriodFromPrefixRegex)
        self._from_suffix_regex = RegExpUtility.get_safe_reg_exp(JapaneseDateTime.DateTimePeriodFromSuffixRegex)
        self._till_regex = RegExpUtility.get_safe_reg_exp(JapaneseDateTime.DateTimePeriodTillRegex)
        self._connector_regex = RegExpUtility.get_safe_reg_exp(JapaneseDateTime.DateTimePeriodConnectorRegex)
        self._preposition_regex = RegExpUtility.get_safe_reg_exp(JapaneseDateTime.DateTimePeriodPrepositionRegex)
        self._zhijian_regex = RegExpUtility.get_safe_reg_exp(JapaneseDateTime.ZhijianRegex)
        self._time_of_day_regex = RegExpUtility.get_safe_reg_exp(JapaneseDateTime.TimeOfDayRegex)
        self._specific_time_of_day_regex = RegExpUtility.get_safe_reg_exp(JapaneseDateTime.SpecificTimeOfDayRegex)
        self._unit_regex = RegExpUtility.get_safe_reg_exp(JapaneseDateTime.DateTimePeriodUnitRegex)
        self._followed_unit = RegExpUtility.get_safe_reg_exp(JapaneseDateTime.DateTimePeriodFollowedUnit)
        self._past_regex = RegExpUtility.get_safe_reg_exp(JapaneseDateTime.PastRegex)
        self._future_regex = RegExpUtility.get_safe_reg_exp(JapaneseDateTime.FutureRegex)
        self._weekday_regex = RegExpUtility.get_safe_reg_exp(JapaneseDateTime.WeekDayRegex)
        self._time_period_left_regex = RegExpUtility.get_safe_reg_exp(JapaneseDateTime.TimePeriodLeftRegex)
        self._relative_regex = RegExpUtility.get_safe_reg_exp(JapaneseDateTime.RelativeRegex)
        self._rest_of_date_regex = RegExpUtility.get_safe_reg_exp(JapaneseDateTime.RestOfDateRegex)
        self._am_pm_desc_regex = RegExpUtility.get_safe_reg_exp(JapaneseDateTime.AmPmDescRegex)
        self._before_after_regex = RegExpUtility.get_safe_reg_exp(JapaneseDateTime.BeforeAfterRegex)

        self._hour_regex = RegExpUtility.get_safe_reg_exp(JapaneseDateTime.HourRegex)
        self._hour_num_regex = RegExpUtility.get_safe_reg_exp(JapaneseDateTime.HourNumRegex)
        self._this_regex = RegExpUtility.get_safe_reg_exp(JapaneseDateTime.DatePeriodThisRegex)
        self._last_regex = RegExpUtility.get_safe_reg_exp(JapaneseDateTime.DatePeriodLastRegex)
        self._next_regex = RegExpUtility.get_safe_reg_exp(JapaneseDateTime.DatePeriodNextRegex)
        self._number_combined_with_unit = RegExpUtility.get_safe_reg_exp(JapaneseDateTime.NumberCombinedWithUnit)

    def get_from_token_index(self, text: str) -> MatchedIndex:
        index = -1
        match = RegExpUtility.match_end(self.from_prefix_regex, text, True)
        if match.success:
            return MatchedIndex(True, match.index)
        else:
            match = RegExpUtility.match_end(self.from_suffix_regex, text, True)
            if match.success:
                return MatchedIndex(True, match.index)
        return MatchedIndex(False, index)

    def has_connector_token(self, text: str) -> MatchedIndex:
        return RegExpUtility.exact_match(self.connector_regex, text, True)

    def get_between_token_index(self, text: str) -> MatchedIndex:
        index = -1
        match = RegExpUtility.get_matches(self.zhijian_regex, text)
        if match and match[0].success:
            return MatchedIndex(True, match.index)
        return MatchedIndex(False, index)
