from typing import List, Pattern

from recognizers_text.utilities import RegExpUtility
from recognizers_text.extractor import Extractor
from recognizers_number.number.english.extractors import EnglishIntegerExtractor
from ...resources.english_date_time import EnglishDateTime
from ..extractors import DateTimeExtractor
from ..base_timeperiod import TimePeriodExtractorConfiguration, MatchedIndex
from ..base_time import BaseTimeExtractor
from .time_extractor_config import EnglishTimeExtractorConfiguration
from ..utilities import DateTimeOptions


class EnglishTimePeriodExtractorConfiguration(TimePeriodExtractorConfiguration):

    @property
    def dmy_date_format(self) -> bool:
        return self._dmy_date_format

    @property
    def simple_cases_regex(self) -> List[Pattern]:
        return self._simple_cases_regex

    @property
    def till_regex(self) -> Pattern:
        return self._till_regex

    @property
    def time_of_day_regex(self) -> Pattern:
        return self._time_of_day_regex

    @property
    def general_ending_regex(self) -> Pattern:
        return self._general_ending_regex

    @property
    def single_time_extractor(self) -> DateTimeExtractor:
        return self._single_time_extractor

    @property
    def integer_extractor(self) -> Extractor:
        return self._integer_extractor

    @property
    def token_before_date(self) -> str:
        return self._token_before_date

    @property
    def pure_number_regex(self) -> List[Pattern]:
        return self._pure_number_regex

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
    def preposition_regex(self) -> Pattern:
        return self._preposition_regex

    @property
    def specific_time_of_day_regex(self) -> Pattern:
        return self._specific_time_of_day_regex

    @property
    def time_unit_regex(self) -> Pattern:
        return self._time_unit_regex
    
    @property
    def time_followed_unit(self) -> Pattern:
        return self._time_followed_unit
    @property
    def time_number_combined_with_unit(self):
        return self._time_number_combined_with_unit

    def __init__(self):
        super().__init__()
        self._time_number_combined_with_unit = RegExpUtility.get_safe_reg_exp(
            EnglishDateTime.TimeNumberCombinedWithUnit
        )
        self._time_followed_unit = RegExpUtility.get_safe_reg_exp(
            EnglishDateTime.TimeFollowedUnit
        )
        self._time_unit_regex = RegExpUtility.get_safe_reg_exp(
            EnglishDateTime.TimeUnitRegex
        )
        self._hour_regex = RegExpUtility.get_safe_reg_exp(
            EnglishDateTime.HourRegex
        )
        self._period_hour_num_regex = RegExpUtility.get_safe_reg_exp(
            EnglishDateTime.PeriodHourNumRegex
        )
        self._period_desc_regex = RegExpUtility.get_safe_reg_exp(
            EnglishDateTime.DescRegex
        )
        self._pm_regex = RegExpUtility.get_safe_reg_exp(
            EnglishDateTime.PmRegex
        )
        self._am_regex = RegExpUtility.get_safe_reg_exp(
            EnglishDateTime.AmRegex
        )
        self._preposition_regex = RegExpUtility.get_safe_reg_exp(
            EnglishDateTime.PrepositionRegex
        )
        self._specific_time_of_day_regex = RegExpUtility.get_safe_reg_exp(
            EnglishDateTime.SpecificTimeOfDayRegex
        )
        self._simple_cases_regex: List[Pattern] = [
            RegExpUtility.get_safe_reg_exp(EnglishDateTime.SpecificTimeBetweenAnd),
            RegExpUtility.get_safe_reg_exp(EnglishDateTime.SpecificTimeFromTo),
            RegExpUtility.get_safe_reg_exp(EnglishDateTime.PureNumFromTo),
            RegExpUtility.get_safe_reg_exp(EnglishDateTime.PureNumBetweenAnd)
        ]
        self._till_regex: Pattern = RegExpUtility.get_safe_reg_exp(
            EnglishDateTime.TillRegex)
        self._time_of_day_regex: Pattern = RegExpUtility.get_safe_reg_exp(
            EnglishDateTime.TimeOfDayRegex)
        self._general_ending_regex: Pattern = RegExpUtility.get_safe_reg_exp(
            EnglishDateTime.GeneralEndingRegex)
        self._single_time_extractor = BaseTimeExtractor(
            EnglishTimeExtractorConfiguration())
        self._integer_extractor = EnglishIntegerExtractor()
        self._token_before_date = EnglishDateTime.TokenBeforeDate
        self._pure_number_regex = [EnglishDateTime.PureNumFromTo, EnglishDateTime.PureNumFromTo]
        self._options = DateTimeOptions.NONE

    def get_from_token_index(self, source: str) -> MatchedIndex:
        index = -1
        if source.endswith('from'):
            index = source.rfind('from')
            return MatchedIndex(matched=True, index=index)
        return MatchedIndex(matched=False, index=index)

    def get_between_token_index(self, source: str) -> MatchedIndex:
        index = -1
        if source.endswith('between'):
            index = source.rfind('between')
            return MatchedIndex(matched=True, index=index)
        return MatchedIndex(matched=False, index=index)

    def has_connector_token(self, source: str) -> bool:
        return source == 'and'
