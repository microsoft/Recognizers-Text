from typing import List, Pattern

from recognizers_text.utilities import RegExpUtility
from recognizers_number.number import BaseNumberParser, BaseNumberExtractor
from recognizers_number.number.french.extractors import FrenchIntegerExtractor
from recognizers_number.number.french.parsers import FrenchNumberParserConfiguration
from ...resources.french_date_time import FrenchDateTime
from ..extractors import DateTimeExtractor
from ..base_duration import BaseDurationExtractor
from ..base_date import BaseDateExtractor
from ..base_dateperiod import DatePeriodExtractorConfiguration, MatchedIndex
from .duration_extractor_config import FrenchDurationExtractorConfiguration
from .date_extractor_config import FrenchDateExtractorConfiguration

class FrenchDatePeriodExtractorConfiguration(DatePeriodExtractorConfiguration):
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
        return self._week_of_regex

    @property
    def month_of_regex(self) -> Pattern:
        return self._month_of_regex

    @property
    def date_unit_regex(self) -> Pattern:
        return self._date_unit_regex

    @property
    def in_connector_regex(self) -> Pattern:
        return self._in_connector_regex

    @property
    def range_unit_regex(self) -> Pattern:
        return self._range_unit_regex

    @property
    def date_point_extractor(self) -> DateTimeExtractor:
        return self._date_point_extractor

    @property
    def integer_extractor(self) -> BaseNumberExtractor:
        return self._integer_extractor

    @property
    def number_parser(self) -> BaseNumberParser:
        return self._number_parser

    @property
    def duration_extractor(self) -> DateTimeExtractor:
        return self._duration_extractor

    def __init__(self):
        self._simple_cases_regexes = [
            RegExpUtility.get_safe_reg_exp(FrenchDateTime.SimpleCasesRegex),
            RegExpUtility.get_safe_reg_exp(FrenchDateTime.BetweenRegex),
            RegExpUtility.get_safe_reg_exp(FrenchDateTime.OneWordPeriodRegex),
            RegExpUtility.get_safe_reg_exp(FrenchDateTime.MonthWithYear),
            RegExpUtility.get_safe_reg_exp(FrenchDateTime.MonthNumWithYear),
            RegExpUtility.get_safe_reg_exp(FrenchDateTime.YearRegex),
            RegExpUtility.get_safe_reg_exp(FrenchDateTime.WeekDayOfMonthRegex),
            RegExpUtility.get_safe_reg_exp(FrenchDateTime.WeekOfYearRegex),
            RegExpUtility.get_safe_reg_exp(FrenchDateTime.MonthFrontBetweenRegex),
            RegExpUtility.get_safe_reg_exp(FrenchDateTime.MonthFrontSimpleCasesRegex),
            RegExpUtility.get_safe_reg_exp(FrenchDateTime.QuarterRegex),
            RegExpUtility.get_safe_reg_exp(FrenchDateTime.QuarterRegexYearFront),
            RegExpUtility.get_safe_reg_exp(FrenchDateTime.AllHalfYearRegex),
            RegExpUtility.get_safe_reg_exp(FrenchDateTime.SeasonRegex),
            RegExpUtility.get_safe_reg_exp(FrenchDateTime.PastSuffixRegex),
            RegExpUtility.get_safe_reg_exp(FrenchDateTime.NextSuffixRegex),
            RegExpUtility.get_safe_reg_exp(FrenchDateTime.ThisPrefixRegex),
            RegExpUtility.get_safe_reg_exp(FrenchDateTime.LaterEarlyPeriodRegex),
            RegExpUtility.get_safe_reg_exp(FrenchDateTime.WeekWithWeekDayRangeRegex)
        ]
        self._year_regex = RegExpUtility.get_safe_reg_exp(FrenchDateTime.YearRegex)
        self._till_regex = RegExpUtility.get_safe_reg_exp(FrenchDateTime.TillRegex)
        self._followed_unit = RegExpUtility.get_safe_reg_exp(FrenchDateTime.FollowedDateUnit)
        self._number_combined_with_unit = RegExpUtility.get_safe_reg_exp(FrenchDateTime.NumberCombinedWithDateUnit)
        self._past_regex = RegExpUtility.get_safe_reg_exp(FrenchDateTime.PastSuffixRegex)
        self._future_regex = RegExpUtility.get_safe_reg_exp(FrenchDateTime.NextSuffixRegex)
        self._week_of_regex = RegExpUtility.get_safe_reg_exp(FrenchDateTime.WeekOfRegex)
        self._month_of_regex = RegExpUtility.get_safe_reg_exp(FrenchDateTime.MonthOfRegex)
        self._date_unit_regex = RegExpUtility.get_safe_reg_exp(FrenchDateTime.DateUnitRegex)
        self._in_connector_regex = RegExpUtility.get_safe_reg_exp(FrenchDateTime.InConnectorRegex)
        self._range_unit_regex = RegExpUtility.get_safe_reg_exp(FrenchDateTime.RangeUnitRegex)

        self.from_regex = RegExpUtility.get_safe_reg_exp(FrenchDateTime.FromRegex)
        self.connector_and_regex = RegExpUtility.get_safe_reg_exp(FrenchDateTime.ConnectorAndRegex)
        self.before_regex = RegExpUtility.get_safe_reg_exp(FrenchDateTime.BeforeRegex2)

        self._date_point_extractor = BaseDateExtractor(FrenchDateExtractorConfiguration())
        self._integer_extractor = FrenchIntegerExtractor()
        self._number_parser = BaseNumberParser(FrenchNumberParserConfiguration())
        self._duration_extractor = BaseDurationExtractor(FrenchDurationExtractorConfiguration())

    def get_from_token_index(self, source: str) -> MatchedIndex:
        match = self.from_regex.search(source)
        if match:
            return MatchedIndex(True, match.start())

        return MatchedIndex(False, -1)

    def get_between_token_index(self, source: str) -> MatchedIndex:
        match = self.before_regex.search(source)
        if match:
            return MatchedIndex(True, match.start())

        return MatchedIndex(False, -1)

    def has_connector_token(self, source: str) -> bool:
        return not self.connector_and_regex.search(source) is None
