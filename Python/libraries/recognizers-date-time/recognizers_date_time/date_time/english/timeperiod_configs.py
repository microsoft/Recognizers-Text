from typing import List, Pattern, Dict

from recognizers_text.utilities import RegExpUtility
from recognizers_text.extractor import Extractor
from recognizers_number.number.english.extractors import EnglishIntegerExtractor
from recognizers_date_time.date_time.extractors import DateTimeExtractor
from recognizers_date_time.date_time.base_timeperiod import TimePeriodExtractorConfiguration, MatchedIndex, TimePeriodParserConfiguration, MatchedTimeRegex
from recognizers_date_time.date_time.base_time import BaseTimeExtractor, BaseTimeParser
from recognizers_date_time.date_time.english.time_configs import EnglishTimeExtractorConfiguration, EnglishTimeParserConfiguration
from recognizers_date_time.date_time.english.base_configs import EnglishDateTimeUtilityConfiguration
from recognizers_date_time.date_time.utilities import DateTimeUtilityConfiguration
from recognizers_date_time.resources.english_date_time import EnglishDateTime

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

class EnglishTimePeriodParserConfiguration(TimePeriodParserConfiguration):
    @property
    def time_extractor(self) -> BaseTimeExtractor:
        return self._time_extractor

    @property
    def time_parser(self) -> BaseTimeParser:
        return self._time_parser

    @property
    def integer_extractor(self) -> Extractor:
        return self._integer_extractor

    @property
    def pure_number_from_to_regex(self) -> Pattern:
        return self._pure_number_from_to_regex

    @property
    def pure_number_between_and_regex(self) -> Pattern:
        return self._pure_number_between_and_regex

    @property
    def time_of_day_regex(self) -> Pattern:
        return self._time_of_day_regex

    @property
    def till_regex(self) -> Pattern:
        return self._till_regex

    @property
    def numbers(self) -> Dict[str, int]:
        return self._numbers

    @property
    def utility_configuration(self) -> DateTimeUtilityConfiguration:
        return self._utility_configuration

    def __init__(self):
        self._time_extractor = BaseTimeExtractor(EnglishTimeExtractorConfiguration())
        self._time_parser = BaseTimeParser(EnglishTimeParserConfiguration())
        self._integer_extractor = EnglishIntegerExtractor()
        self._pure_number_from_to_regex = RegExpUtility.get_safe_reg_exp(EnglishDateTime.PureNumFromTo)
        self._pure_number_between_and_regex = RegExpUtility.get_safe_reg_exp(EnglishDateTime.PureNumBetweenAnd)
        self._time_of_day_regex = RegExpUtility.get_safe_reg_exp(EnglishDateTime.TimeOfDayRegex)
        self._till_regex = RegExpUtility.get_safe_reg_exp(EnglishDateTime.TillRegex)
        self._numbers = EnglishDateTime.Numbers
        self._utility_configuration = EnglishDateTimeUtilityConfiguration()


    def get_matched_timex_range(self, source: str) -> MatchedTimeRegex:
        source = source.strip().lower()
        if source.endswith('s'):
            source = source[:-1]
        timex = ''
        begin_hour = 0
        end_hour = 0
        end_min = 0
        if source.endswith('morning'):
            timex = 'TMO'
            begin_hour = 8
            end_hour = 12
        elif source.endswith('afternoon'):
            timex = 'TAF'
            begin_hour = 12
            end_hour = 16
        elif source.endswith('evening'):
            timex = 'TEV'
            begin_hour = 16
            end_hour = 20
        elif source.endswith('daytime'):
            timex = 'TDT'
            begin_hour = 8
            end_hour = 18
        elif source.endswith('night'):
            timex = 'TNI'
            begin_hour = 20
            end_hour = 23
            end_min = 59
        else:
            timex = None
            matched = False
            return MatchedTimeRegex(
                matched=False,
                timex='',
                begin_hour=0,
                end_hour=0,
                end_min=0
            )
        return MatchedTimeRegex(
                matched=True,
                timex=timex,
                begin_hour=begin_hour,
                end_hour=end_hour,
                end_min=end_min
            )