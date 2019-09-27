from typing import Pattern, List, Dict
from recognizers_number import (BaseNumberExtractor, BaseNumberParser,
                                FrenchOrdinalExtractor, FrenchIntegerExtractor, FrenchNumberParserConfiguration)
from recognizers_text.utilities import RegExpUtility
from ...resources.french_date_time import FrenchDateTime
from ..extractors import DateTimeExtractor
from ..base_duration import BaseDurationExtractor
from ..base_date import DateExtractorConfiguration
from ..utilities import DateTimeUtilityConfiguration
from .duration_extractor_config import FrenchDurationExtractorConfiguration
from .base_configs import FrenchDateTimeUtilityConfiguration
from ..constants import Constants
from ...resources.base_date_time import BaseDateTime
from ..utilities import DateTimeOptions


class FrenchDateExtractorConfiguration(DateExtractorConfiguration):

    @property
    def date_regex_list(self) -> List[Pattern]:
        return self._date_regex_list

    @property
    def implicit_date_list(self) -> List[Pattern]:
        return self._implicit_date_list

    @property
    def month_end(self) -> Pattern:
        return self._month_end

    @property
    def week_day_end(self) -> Pattern:
        return self._week_day_end

    @property
    def of_month(self) -> Pattern:
        return self._of_month

    @property
    def date_unit_regex(self) -> Pattern:
        return self._date_unit_regex

    @property
    def for_the_regex(self) -> Pattern:
        return self._for_the_regex

    @property
    def week_day_and_day_of_month_regex(self) -> Pattern:
        return self._week_day_and_day_of_month_regex

    @property
    def relative_month_regex(self) -> Pattern:
        return self._relative_month_regex

    @property
    def week_day_regex(self) -> Pattern:
        return self._week_day_regex

    @property
    def prefix_article_regex(self) -> Pattern:
        return self._prefix_article_regex

    @property
    def day_of_week(self) -> Dict[str, int]:
        return self._day_of_week

    @property
    def month_of_year(self) -> Dict[str, int]:
        return self._month_of_year

    @property
    def ordinal_extractor(self) -> BaseNumberExtractor:
        return self._ordinal_extractor

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
    def utility_configuration(self) -> DateTimeUtilityConfiguration:
        return self._utility_configuration

    @property
    def strict_relative_regex(self) -> Pattern:
        return self._strict_relative_regex

    @property
    def range_connector_symbol_regex(self) -> Pattern:
        return self._range_connector_symbol_regex

    @property
    def year_suffix(self) -> Pattern:
        return self._year_suffix

    @property
    def more_than_regex(self) -> Pattern:
        return self._more_than_regex

    @property
    def less_than_regex(self) -> Pattern:
        return self._less_than_regex

    @property
    def in_connector_regex(self) -> Pattern:
        return self._in_connector_regex

    @property
    def range_unit_regex(self) -> Pattern:
        return self._range_unit_regex

    @property
    def since_year_suffix_regex(self) -> Pattern:
        return self._since_year_suffix_regex

    @property
    def week_day_and_day_regex(self) -> Pattern:
        return self._week_day_and_day_regex

    def __init__(self):
        if FrenchDateTime.DefaultLanguageFallback == Constants.DEFAULT_LANGUAGE_FALLBACK_DMY:
            date_extractor_4 = FrenchDateTime.DateExtractor5
            date_extractor_5 = FrenchDateTime.DateExtractor4
        else:
            date_extractor_4 = FrenchDateTime.DateExtractor4
            date_extractor_5 = FrenchDateTime.DateExtractor5

        self._date_regex_list = [
            RegExpUtility.get_safe_reg_exp(FrenchDateTime.DateExtractor1),
            RegExpUtility.get_safe_reg_exp(FrenchDateTime.DateExtractor2),
            RegExpUtility.get_safe_reg_exp(FrenchDateTime.DateExtractor3),
            RegExpUtility.get_safe_reg_exp(date_extractor_4),
            RegExpUtility.get_safe_reg_exp(date_extractor_5),
            RegExpUtility.get_safe_reg_exp(FrenchDateTime.DateExtractor6),
            RegExpUtility.get_safe_reg_exp(FrenchDateTime.DateExtractor7),
            RegExpUtility.get_safe_reg_exp(FrenchDateTime.DateExtractor8),
            RegExpUtility.get_safe_reg_exp(FrenchDateTime.DateExtractor9),
            RegExpUtility.get_safe_reg_exp(FrenchDateTime.DateExtractorA),
        ]

        self._implicit_date_list = [
            RegExpUtility.get_safe_reg_exp(FrenchDateTime.OnRegex),
            RegExpUtility.get_safe_reg_exp(FrenchDateTime.RelaxedOnRegex),
            RegExpUtility.get_safe_reg_exp(FrenchDateTime.SpecialDayRegex),
            RegExpUtility.get_safe_reg_exp(FrenchDateTime.ThisRegex),
            RegExpUtility.get_safe_reg_exp(FrenchDateTime.LastDateRegex),
            RegExpUtility.get_safe_reg_exp(FrenchDateTime.NextDateRegex),
            RegExpUtility.get_safe_reg_exp(FrenchDateTime.StrictWeekDay),
            RegExpUtility.get_safe_reg_exp(FrenchDateTime.WeekDayOfMonthRegex),
            RegExpUtility.get_safe_reg_exp(FrenchDateTime.SpecialDate),
        ]
        self._month_end = RegExpUtility.get_safe_reg_exp(
            FrenchDateTime.MonthEnd)
        self._of_month = RegExpUtility.get_safe_reg_exp(FrenchDateTime.OfMonth)
        self._date_unit_regex = RegExpUtility.get_safe_reg_exp(
            FrenchDateTime.DateUnitRegex)
        self._for_the_regex = RegExpUtility.get_safe_reg_exp(
            FrenchDateTime.ForTheRegex)
        self._week_day_and_day_of_month_regex = RegExpUtility.get_safe_reg_exp(
            FrenchDateTime.WeekDayAndDayOfMonthRegex)
        self._relative_month_regex = RegExpUtility.get_safe_reg_exp(
            FrenchDateTime.RelativeMonthRegex)
        self._week_day_regex = RegExpUtility.get_safe_reg_exp(
            FrenchDateTime.WeekDayRegex)
        self._day_of_week = FrenchDateTime.DayOfWeek
        self._ordinal_extractor = FrenchOrdinalExtractor()
        self._integer_extractor = FrenchIntegerExtractor()
        self._number_parser = BaseNumberParser(
            FrenchNumberParserConfiguration())
        self._duration_extractor = BaseDurationExtractor(
            FrenchDurationExtractorConfiguration())
        self._utility_configuration = FrenchDateTimeUtilityConfiguration()
        self._range_connector_symbol_regex = RegExpUtility.get_safe_reg_exp(
            BaseDateTime.RangeConnectorSymbolRegex
        )
        self._strict_relative_regex = RegExpUtility.get_safe_reg_exp(
            FrenchDateTime.StrictRelativeRegex
        )
        self._year_suffix = RegExpUtility.get_safe_reg_exp(
            FrenchDateTime.YearSuffix
        )
        self._month_of_year = FrenchDateTime.MonthOfYear
        self._prefix_article_regex = RegExpUtility.get_safe_reg_exp(
            FrenchDateTime.PrefixArticleRegex
        )
        self._week_day_end = RegExpUtility.get_safe_reg_exp(
            FrenchDateTime.WeekDayEnd
        )
        self._more_than_regex = RegExpUtility.get_safe_reg_exp(
            FrenchDateTime.MoreThanRegex
        )
        self._less_than_regex = RegExpUtility.get_safe_reg_exp(
            FrenchDateTime.LessThanRegex
        )
        self._in_connector_regex = RegExpUtility.get_safe_reg_exp(
            FrenchDateTime.InConnectorRegex
        )
        self._range_unit_regex = RegExpUtility.get_safe_reg_exp(
            FrenchDateTime.RangeUnitRegex
        )
        self._since_year_suffix_regex = RegExpUtility.get_safe_reg_exp(
            FrenchDateTime.SinceYearSuffixRegex
        )
        self._week_day_and_day_regex = RegExpUtility.get_safe_reg_exp(
            FrenchDateTime.WeekDayAndDayRegex
        )
