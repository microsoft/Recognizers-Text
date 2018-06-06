from typing import List, Pattern

from recognizers_text.utilities import RegExpUtility
from recognizers_text.extractor import Extractor
from recognizers_number.number.english.extractors import EnglishIntegerExtractor
from ...resources.english_date_time import EnglishDateTime
from ..extractors import DateTimeExtractor
from ..base_timeperiod import TimePeriodExtractorConfiguration, MatchedIndex
from ..base_time import BaseTimeExtractor
from .time_extractor_config import EnglishTimeExtractorConfiguration

class EnglishTimePeriodExtractorConfiguration(TimePeriodExtractorConfiguration):
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

    def __init__(self):
        self._simple_cases_regex: List[Pattern] = [
            RegExpUtility.get_safe_reg_exp(EnglishDateTime.PureNumFromTo),
            RegExpUtility.get_safe_reg_exp(EnglishDateTime.PureNumBetweenAnd)
        ]
        self._till_regex: Pattern = RegExpUtility.get_safe_reg_exp(EnglishDateTime.TillRegex)
        self._time_of_day_regex: Pattern = RegExpUtility.get_safe_reg_exp(EnglishDateTime.TimeOfDayRegex)
        self._general_ending_regex: Pattern = RegExpUtility.get_safe_reg_exp(EnglishDateTime.GeneralEndingRegex)
        self._single_time_extractor = BaseTimeExtractor(EnglishTimeExtractorConfiguration())
        self._integer_extractor = EnglishIntegerExtractor()

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
