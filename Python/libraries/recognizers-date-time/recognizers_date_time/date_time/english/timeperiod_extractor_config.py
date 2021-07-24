#  Copyright (c) Microsoft Corporation. All rights reserved.
#  Licensed under the MIT License.

from typing import List, Pattern

from recognizers_text.utilities import RegExpUtility
from recognizers_text.extractor import Extractor
from recognizers_number.number.english.extractors import EnglishIntegerExtractor
from ...resources.english_date_time import EnglishDateTime
from ..extractors import DateTimeExtractor
from ..base_timeperiod import TimePeriodExtractorConfiguration, MatchedIndex
from ..base_time import BaseTimeExtractor
from ..base_timezone import BaseTimeZoneExtractor
from .time_extractor_config import EnglishTimeExtractorConfiguration
from .timezone_extractor_config import EnglishTimeZoneExtractorConfiguration
from ..utilities import DateTimeOptions


class EnglishTimePeriodExtractorConfiguration(TimePeriodExtractorConfiguration):

    @property
    def check_both_before_after(self) -> bool:
        return self._check_both_before_after

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
    def time_zone_extractor(self) -> DateTimeExtractor:
        return self._time_zone_extractor

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
        self._check_both_before_after = EnglishDateTime.CheckBothBeforeAfter
        self._simple_cases_regex: List[Pattern] = [
            RegExpUtility.get_safe_reg_exp(EnglishDateTime.PureNumFromTo),
            RegExpUtility.get_safe_reg_exp(EnglishDateTime.PureNumBetweenAnd),
            RegExpUtility.get_safe_reg_exp(EnglishDateTime.SpecificTimeFromTo),
            RegExpUtility.get_safe_reg_exp(EnglishDateTime.SpecificTimeBetweenAnd)
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
        self._time_zone_extractor = BaseTimeZoneExtractor(
            EnglishTimeZoneExtractorConfiguration())
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

    def is_connector_token(self, source: str):
        return source == "and"
