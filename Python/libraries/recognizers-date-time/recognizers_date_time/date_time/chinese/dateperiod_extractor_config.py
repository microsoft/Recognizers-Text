from typing import List, Pattern

from recognizers_text import Extractor, Parser, RegExpUtility
from recognizers_number import ChineseNumberExtractor, ChineseNumberParserConfiguration, BaseNumberParser
from ...resources.chinese_date_time import ChineseDateTime
from ..extractors import DateTimeExtractor
from ..base_dateperiod import DatePeriodExtractorConfiguration, MatchedIndex
from .date_extractor import ChineseDateExtractor

class ChineseDatePeriodExtractorConfiguration(DatePeriodExtractorConfiguration):
    @property
    def simple_cases_regexes(self) -> List[Pattern]:
        return self._simple_cases_regexes

    @property
    def year_regex(self) -> Pattern:
        return self._year_regex

    @property
    def till_regex(self) -> Pattern:
        return self._till_regex

    @property
    def followed_unit(self) -> Pattern:
        return self._followed_unit

    @property
    def number_combined_with_unit(self) -> Pattern:
        return self._number_combined_with_unit

    @property
    def past_regex(self) -> Pattern:
        return self._past_regex

    @property
    def future_regex(self) -> Pattern:
        return self._future_regex

    @property
    def week_of_regex(self) -> Pattern:
        return None

    @property
    def month_of_regex(self) -> Pattern:
        return None

    @property
    def date_unit_regex(self) -> Pattern:
        return None

    @property
    def in_connector_regex(self) -> Pattern:
        return None

    @property
    def range_unit_regex(self) -> Pattern:
        return None

    @property
    def date_point_extractor(self) -> DateTimeExtractor:
        return self._date_point_extractor

    @property
    def integer_extractor(self) -> Extractor:
        return self._integer_extractor

    @property
    def number_parser(self) -> Parser:
        return self._number_parser

    @property
    def duration_extractor(self) -> DateTimeExtractor:
        return None

    @property
    def range_connector_regex(self) -> Pattern:
        return None

    def __init__(self):
        self._simple_cases_regexes = [
            RegExpUtility.get_safe_reg_exp(ChineseDateTime.SimpleCasesRegex),
            RegExpUtility.get_safe_reg_exp(ChineseDateTime.OneWordPeriodRegex),
            RegExpUtility.get_safe_reg_exp(ChineseDateTime.StrictYearRegex),
            RegExpUtility.get_safe_reg_exp(ChineseDateTime.YearToYear),
            RegExpUtility.get_safe_reg_exp(ChineseDateTime.YearAndMonth),
            RegExpUtility.get_safe_reg_exp(ChineseDateTime.PureNumYearAndMonth),
            RegExpUtility.get_safe_reg_exp(ChineseDateTime.DatePeriodYearInChineseRegex),
            RegExpUtility.get_safe_reg_exp(ChineseDateTime.WeekOfMonthRegex),
            RegExpUtility.get_safe_reg_exp(ChineseDateTime.SeasonWithYear),
            RegExpUtility.get_safe_reg_exp(ChineseDateTime.QuarterRegex),
        ]
        self._year_regex = RegExpUtility.get_safe_reg_exp(ChineseDateTime.YearRegex)
        self._till_regex = RegExpUtility.get_safe_reg_exp(ChineseDateTime.DatePeriodTillRegex)
        self._followed_unit = RegExpUtility.get_safe_reg_exp(ChineseDateTime.FollowedUnit)
        self._number_combined_with_unit = RegExpUtility.get_safe_reg_exp(ChineseDateTime.NumberCombinedWithUnit)
        self._past_regex = RegExpUtility.get_safe_reg_exp(ChineseDateTime.PastRegex)
        self._future_regex = RegExpUtility.get_safe_reg_exp(ChineseDateTime.FutureRegex)
        self._date_point_extractor = ChineseDateExtractor()
        self._integer_extractor = ChineseNumberExtractor()
        self._number_parser = BaseNumberParser(ChineseNumberParserConfiguration())

    def get_from_token_index(self, source: str) -> MatchedIndex:
        if source.endswith('从'):
            return MatchedIndex(True, source.rindex('从'))
        return MatchedIndex(False, -1)

    def get_between_token_index(self, source: str) -> MatchedIndex:
        return MatchedIndex(False, -1)

    def has_connector_token(self, source: str) -> bool:
        return False
