from typing import List, Pattern

from recognizers_text.extractor import Extractor
from recognizers_text.utilities import RegExpUtility
from recognizers_number.number import BaseNumberParser, BaseNumberExtractor
from recognizers_number.number.english.extractors import EnglishIntegerExtractor
from recognizers_number.number.english.parsers import EnglishNumberParserConfiguration
from ...resources.base_date_time import BaseDateTime
from ...resources.english_date_time import EnglishDateTime
from ..extractors import DateTimeExtractor
from ..base_duration import BaseDurationExtractor
from ..base_date import BaseDateExtractor
from ..base_dateperiod import DatePeriodExtractorConfiguration, MatchedIndex
from .duration_extractor_config import EnglishDurationExtractorConfiguration
from .date_extractor_config import EnglishDateExtractorConfiguration
from .common_configs import EnglishOrdinalExtractor, EnglishCardinalExtractor


class EnglishDatePeriodExtractorConfiguration(DatePeriodExtractorConfiguration):
    @property
    def simple_cases_regexes(self) -> List[Pattern]:
        return self._simple_cases_regexes

    @property
    def illegal_year_regex(self) -> Pattern:
        return self._illegal_year_regex

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
    def time_unit_regex(self) -> Pattern:
        return self._time_unit_regex

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

    @property
    def range_connector_regex(self) -> Pattern:
        return self._range_connector_regex

    @property
    def ordinal_extractor(self) -> BaseNumberExtractor:
        return self._ordinal_extractor

    @property
    def cardinal_extractor(self) -> Extractor:
        return self._cardinal_extractor

    @property
    def now_regex(self) -> Pattern:
        return self._now_regex

    @property
    def within_next_prefix_regex(self) -> Pattern:
        return self._within_next_prefix_regex

    @property
    def future_suffix_regex(self) -> Pattern:
        return self._future_suffix_regex

    @property
    def ago_regex(self) -> Pattern:
        return self._ago_regex

    @property
    def later_regex(self) -> Pattern:
        return self._later_regex

    @property
    def less_than_regex(self) -> Pattern:
        return self._less_than_regex

    @property
    def more_than_regex(self) -> Pattern:
        return self._more_than_regex

    @property
    def duration_date_restrictions(self) -> [str]:
        return self._duration_date_restrictions

    @property
    def year_period_regex(self) -> Pattern:
        return self._year_period_regex

    @property
    def month_num_regex(self) -> Pattern:
        return self._month_num_regex

    @property
    def century_suffix_regex(self) -> Pattern:
        return self._century_suffix_regex

    def __init__(self):
        self._simple_cases_regexes = [
            RegExpUtility.get_safe_reg_exp(EnglishDateTime.SimpleCasesRegex),
            RegExpUtility.get_safe_reg_exp(EnglishDateTime.BetweenRegex),
            RegExpUtility.get_safe_reg_exp(EnglishDateTime.OneWordPeriodRegex),
            RegExpUtility.get_safe_reg_exp(EnglishDateTime.MonthWithYear),
            RegExpUtility.get_safe_reg_exp(EnglishDateTime.MonthNumWithYear),
            RegExpUtility.get_safe_reg_exp(EnglishDateTime.YearRegex),
            RegExpUtility.get_safe_reg_exp(EnglishDateTime.WeekOfMonthRegex),
            RegExpUtility.get_safe_reg_exp(EnglishDateTime.WeekOfYearRegex),
            RegExpUtility.get_safe_reg_exp(
                EnglishDateTime.MonthFrontBetweenRegex),
            RegExpUtility.get_safe_reg_exp(
                EnglishDateTime.MonthFrontSimpleCasesRegex),
            RegExpUtility.get_safe_reg_exp(EnglishDateTime.QuarterRegex),
            RegExpUtility.get_safe_reg_exp(
                EnglishDateTime.QuarterRegexYearFront),
            RegExpUtility.get_safe_reg_exp(EnglishDateTime.AllHalfYearRegex),
            RegExpUtility.get_safe_reg_exp(EnglishDateTime.SeasonRegex),
            RegExpUtility.get_safe_reg_exp(EnglishDateTime.WhichWeekRegex),
            RegExpUtility.get_safe_reg_exp(EnglishDateTime.RestOfDateRegex),
            RegExpUtility.get_safe_reg_exp(
                EnglishDateTime.LaterEarlyPeriodRegex),
            RegExpUtility.get_safe_reg_exp(
                EnglishDateTime.WeekWithWeekDayRangeRegex)
        ]
        self._illegal_year_regex = RegExpUtility.get_safe_reg_exp(
            BaseDateTime.IllegalYearRegex)
        self._year_regex = RegExpUtility.get_safe_reg_exp(
            EnglishDateTime.YearRegex)
        self._till_regex = RegExpUtility.get_safe_reg_exp(
            EnglishDateTime.TillRegex)
        self._followed_unit = RegExpUtility.get_safe_reg_exp(
            EnglishDateTime.FollowedDateUnit)
        self._number_combined_with_unit = RegExpUtility.get_safe_reg_exp(
            EnglishDateTime.NumberCombinedWithDateUnit)
        self._past_regex = RegExpUtility.get_safe_reg_exp(
            EnglishDateTime.PreviousPrefixRegex)
        self._future_regex = RegExpUtility.get_safe_reg_exp(
            EnglishDateTime.NextPrefixRegex)
        self._week_of_regex = RegExpUtility.get_safe_reg_exp(
            EnglishDateTime.WeekOfRegex)
        self._month_of_regex = RegExpUtility.get_safe_reg_exp(
            EnglishDateTime.MonthOfRegex)
        self._date_unit_regex = RegExpUtility.get_safe_reg_exp(
            EnglishDateTime.DateUnitRegex)
        self._in_connector_regex = RegExpUtility.get_safe_reg_exp(
            EnglishDateTime.InConnectorRegex)
        self._range_unit_regex = RegExpUtility.get_safe_reg_exp(
            EnglishDateTime.RangeUnitRegex)
        self._date_point_extractor = BaseDateExtractor(
            EnglishDateExtractorConfiguration())
        self._integer_extractor = EnglishIntegerExtractor()
        self._number_parser = BaseNumberParser(
            EnglishNumberParserConfiguration())
        self._duration_extractor = BaseDurationExtractor(
            EnglishDurationExtractorConfiguration())
        self._range_connector_regex = RegExpUtility.get_safe_reg_exp(
            EnglishDateTime.RangeConnectorRegex)
        self._now_regex = RegExpUtility.get_safe_reg_exp(
            EnglishDateTime.NowRegex
        )
        self._within_next_prefix_regex = RegExpUtility.get_safe_reg_exp(
            EnglishDateTime.WithinNextPrefixRegex
        )
        self._time_unit_regex = RegExpUtility.get_safe_reg_exp(
            EnglishDateTime.TimeUnitRegex
        )
        self._future_suffix_regex = RegExpUtility.get_safe_reg_exp(
            EnglishDateTime.FutureSuffixRegex
        )
        self._ago_regex = RegExpUtility.get_safe_reg_exp(
            EnglishDateTime.AgoRegex
        )
        self._later_regex = RegExpUtility.get_safe_reg_exp(
            EnglishDateTime.LaterRegex
        )
        self._less_than_regex = RegExpUtility.get_safe_reg_exp(
            EnglishDateTime.LessThanRegex
        )
        self._more_than_regex = RegExpUtility.get_safe_reg_exp(
            EnglishDateTime.MoreThanRegex
        )
        self._duration_date_restrictions = EnglishDateTime.DurationDateRestrictions
        self._year_period_regex = RegExpUtility.get_safe_reg_exp(
            EnglishDateTime.YearPeriodRegex
        )
        self._month_num_regex = RegExpUtility.get_safe_reg_exp(
            EnglishDateTime.MonthNumRegex
        )
        self._century_suffix_regex = RegExpUtility.get_safe_reg_exp(
            EnglishDateTime.CenturySuffixRegex
        )
        self._ordinal_extractor = EnglishOrdinalExtractor()
        # TODO When the implementation for these properties is added, change the None values to their respective Regexps
        self._cardinal_extractor = EnglishCardinalExtractor()

    def get_from_token_index(self, source: str) -> MatchedIndex:
        return MatchedIndex(True, source.rfind('from')) if source.endswith('from') else MatchedIndex(False, -1)

    def get_between_token_index(self, source: str) -> MatchedIndex:
        return MatchedIndex(True, source.rfind('between')) if source.endswith('between') else MatchedIndex(False, -1)

    def has_connector_token(self, source: str) -> bool:
        match = self.range_connector_regex.search(source)
        return len(match.group()) == len(source) if match else None
