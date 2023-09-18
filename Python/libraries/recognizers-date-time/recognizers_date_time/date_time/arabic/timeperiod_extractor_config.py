from typing import List, Pattern

from recognizers_text.utilities import RegExpUtility
from recognizers_text.extractor import Extractor
from recognizers_number.number.arabic.extractors import ArabicIntegerExtractor
from recognizers_date_time.resources.arabic_date_time import ArabicDateTime
from recognizers_date_time.date_time.extractors import DateTimeExtractor
from recognizers_date_time.date_time.base_timeperiod import TimePeriodExtractorConfiguration, MatchedIndex
from recognizers_date_time.date_time.base_time import BaseTimeExtractor
from recognizers_date_time.date_time.arabic.time_extractor_config import ArabicTimeExtractorConfiguration
from recognizers_date_time.date_time.arabic.base_configs import ArabicDateTimeUtilityConfiguration


class ArabicTimePeriodExtractorConfiguration(TimePeriodExtractorConfiguration):
    @property
    def till_regex(self) -> Pattern:
        return self._till_regex

    @property
    def hour_regex(self) -> Pattern:
        return self._hour_regex

    @property
    def period_hour_num_regex(self) -> Pattern:
        return self._period_hour_num_regex

    @property
    def period_desc_regex(self) -> Pattern:
        return self._period_desc_regex

    @property
    def pm_regex(self) -> Pattern:
        return self._pm_regex

    @property
    def am_regex(self) -> Pattern:
        return self._am_regex

    @property
    def pure_num_from_to(self) -> Pattern:
        return self._pure_num_from_to

    @property
    def pure_num_between_and(self):
        return self._pure_num_between_and

    @property
    def specific_time_from_to(self):
        return self._specific_time_from_to_regex

    @property
    def specific_time_between_and(self):
        return self._specific_time_between_and_regex

    @property
    def preposition_regex(self):
        return self._preposition_regex

    @property
    def time_of_day_regex(self) -> Pattern:
        return self._time_of_day_regex

    @property
    def specific_time_of_day_regex(self):
        return self._specific_time_of_day_regex

    @property
    def time_unit_regex(self):
        return self._time_unit_regex

    @property
    def time_follow_unit(self) -> Pattern:
        return self._time_followed_unit

    @property
    def time_number_combined_with_unit(self) -> Pattern:
        return self._time_number_combined_with_unit

    @property
    def general_ending_regex(self) -> Pattern:
        return self._general_ending_regex

    @property
    def token_before_date(self) -> str:
        return self._token_before_date

    @property
    def utility_configuration(self):
        return self._utility_configuration

    @property
    def single_time_extractor(self) -> DateTimeExtractor:
        return self._single_time_extractor

    @property
    def time_zone_extractor(self) -> DateTimeExtractor:
        return self._time_zone_extractor

    @property
    def integer_extractor(self) -> Extractor:
        return self._integer_extractor

    @property
    def simple_cases_regex(self) -> List[Pattern]:
        return self._simple_cases_regex

    @property
    def pure_number_regex(self) -> List[Pattern]:
        return self._pure_number_regex

    @property
    def check_both_before_after(self) -> bool:
        return self._check_both_before_after

    @property
    def from_regex(self) -> Pattern:
        return self._from_regex

    @property
    def between_token_regex(self) -> Pattern:
        return self._between_token_regex

    @property
    def range_connector_regex(self) -> Pattern:
        return self._range_connector_regex

    def __init__(self):
        super().__init__()

        self._token_before_date = ArabicDateTime.TokenBeforeDate
        self._single_time_extractor = BaseTimeExtractor(ArabicTimeExtractorConfiguration())
        self._utility_configuration = ArabicDateTimeUtilityConfiguration()

        self._integer_extractor = ArabicIntegerExtractor()
        self._check_both_before_after = ArabicDateTime.CheckBothBeforeAfter

        self._till_regex: Pattern = RegExpUtility.get_safe_reg_exp(ArabicDateTime.TillRegex)
        self._time_of_day_regex: Pattern = RegExpUtility.get_safe_reg_exp(ArabicDateTime.TimeOfDayRegex)
        self._general_ending_regex: Pattern = RegExpUtility.get_safe_reg_exp( ArabicDateTime.GeneralEndingRegex)
        self._hour_regex: Pattern = RegExpUtility.get_safe_reg_exp(ArabicDateTime.HourRegex)
        self._period_desc_regex: Pattern = RegExpUtility.get_safe_reg_exp(ArabicDateTime.DescRegex)
        self._period_hour_num_regex: Pattern = RegExpUtility.get_safe_reg_exp(ArabicDateTime.PeriodHourNumRegex)
        self._specific_time_of_day_regex: Pattern = RegExpUtility.get_safe_reg_exp(
            ArabicDateTime.SpecificTimeOfDayRegex)
        self._time_followed_unit: Pattern = RegExpUtility.get_safe_reg_exp(ArabicDateTime.TimeFollowedUnit)
        self._time_number_combined_with_unit: Pattern = RegExpUtility.get_safe_reg_exp(
            ArabicDateTime.TimeNumberCombinedWithUnit)

        self._time_unit_regex = RegExpUtility.get_safe_reg_exp(ArabicDateTime.TimeUnitRegex)
        self._specific_time_from_to_regex = RegExpUtility.get_safe_reg_exp(ArabicDateTime.SpecificTimeFromTo)
        self._specific_time_between_and_regex = RegExpUtility.get_safe_reg_exp(ArabicDateTime.SpecificTimeBetweenAnd)
        self._preposition_regex = RegExpUtility.get_safe_reg_exp(ArabicDateTime.PrepositionRegex)

        self._time_zone_extractor = None
        self._pure_num_between_and = RegExpUtility.get_safe_reg_exp(ArabicDateTime.PureNumBetweenAnd)
        self._pure_num_from_to = RegExpUtility.get_safe_reg_exp(ArabicDateTime.PureNumFromTo)
        self._am_regex = RegExpUtility.get_safe_reg_exp(ArabicDateTime.AmRegex)
        self._pm_regex = RegExpUtility.get_safe_reg_exp(ArabicDateTime.PmRegex)

        self._simple_cases_regex: List[Pattern] = [
            self.pure_num_from_to,
            self.pure_num_between_and,
            self.specific_time_from_to,
            self.specific_time_between_and
        ]
        self._pure_number_regex = [self.pure_num_from_to, self.pure_num_between_and]

        self._from_regex = RegExpUtility.get_safe_reg_exp(ArabicDateTime.FromRegex)
        self._between_token_regex = RegExpUtility.get_safe_reg_exp(ArabicDateTime.BetweenTokenRegex)
        self._range_connector_regex = RegExpUtility.get_safe_reg_exp(ArabicDateTime.RangeConnectorRegex)

    def get_from_token_index(self, source: str) -> MatchedIndex:
        match = self.from_regex.search(source)
        if match:
            return MatchedIndex(True, match.start())

        return MatchedIndex(False, -1)

    def get_between_token_index(self, source: str) -> MatchedIndex:
        match = self.between_token_regex.search(source)
        if match:
            return MatchedIndex(True, match.start())

        return MatchedIndex(False, -1)

    def is_connector_token(self, source: str):
        return self.range_connector_regex.match(source)
