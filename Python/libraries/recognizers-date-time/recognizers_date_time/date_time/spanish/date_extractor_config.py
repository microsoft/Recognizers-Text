from typing import Pattern, List, Dict
from recognizers_number import (BaseNumberExtractor, BaseNumberParser,
                                SpanishOrdinalExtractor, SpanishIntegerExtractor, SpanishNumberParserConfiguration)
from recognizers_text.utilities import RegExpUtility
from ...resources.spanish_date_time import SpanishDateTime
from ..extractors import DateTimeExtractor
from ..base_duration import BaseDurationExtractor
from ..base_date import DateExtractorConfiguration
from ..utilities import DateTimeUtilityConfiguration
from .duration_extractor_config import SpanishDurationExtractorConfiguration
from .base_configs import SpanishDateTimeUtilityConfiguration
from ..constants import Constants
from ...resources.base_date_time import BaseDateTime


class SpanishDateExtractorConfiguration(DateExtractorConfiguration):
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
    def strict_relative_regex(self) -> Pattern:
        return self._strict_relative_regex

    @property
    def range_connector_symbol_regex(self) -> Pattern:
        return self._range_connector_symbol_regex

    @property
    def utility_configuration(self) -> DateTimeUtilityConfiguration:
        return self._utility_configuration

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
        if SpanishDateTime.DefaultLanguageFallback == Constants.DEFAULT_LANGUAGE_FALLBACK_DMY:
            date_extractor_4 = SpanishDateTime.DateExtractor5
            date_extractor_5 = SpanishDateTime.DateExtractor4
            date_extractor_6 = SpanishDateTime.DateExtractor8
            date_extractor_8 = SpanishDateTime.DateExtractor6
            date_extractor_7 = SpanishDateTime.DateExtractor9
            date_extractor_9 = SpanishDateTime.DateExtractor7
        else:
            date_extractor_4 = SpanishDateTime.DateExtractor4
            date_extractor_5 = SpanishDateTime.DateExtractor5
            date_extractor_6 = SpanishDateTime.DateExtractor6
            date_extractor_8 = SpanishDateTime.DateExtractor8
            date_extractor_7 = SpanishDateTime.DateExtractor7
            date_extractor_9 = SpanishDateTime.DateExtractor9

        self._date_regex_list = [
            RegExpUtility.get_safe_reg_exp(SpanishDateTime.DateExtractor1),
            RegExpUtility.get_safe_reg_exp(SpanishDateTime.DateExtractor2),
            RegExpUtility.get_safe_reg_exp(SpanishDateTime.DateExtractor3),
            RegExpUtility.get_safe_reg_exp(date_extractor_4),
            RegExpUtility.get_safe_reg_exp(date_extractor_5),
            RegExpUtility.get_safe_reg_exp(date_extractor_6),
            RegExpUtility.get_safe_reg_exp(date_extractor_7),
            RegExpUtility.get_safe_reg_exp(date_extractor_8),
            RegExpUtility.get_safe_reg_exp(date_extractor_9),
            RegExpUtility.get_safe_reg_exp(SpanishDateTime.DateExtractor10),
        ]

        self._implicit_date_list = [
            RegExpUtility.get_safe_reg_exp(SpanishDateTime.OnRegex),
            RegExpUtility.get_safe_reg_exp(SpanishDateTime.RelaxedOnRegex),
            RegExpUtility.get_safe_reg_exp(SpanishDateTime.SpecialDayRegex),
            RegExpUtility.get_safe_reg_exp(SpanishDateTime.ThisRegex),
            RegExpUtility.get_safe_reg_exp(SpanishDateTime.LastDateRegex),
            RegExpUtility.get_safe_reg_exp(SpanishDateTime.NextDateRegex),
            RegExpUtility.get_safe_reg_exp(SpanishDateTime.WeekDayRegex),
            RegExpUtility.get_safe_reg_exp(
                SpanishDateTime.WeekDayOfMonthRegex),
            RegExpUtility.get_safe_reg_exp(SpanishDateTime.SpecialDateRegex),
        ]
        self._month_end = RegExpUtility.get_safe_reg_exp(
            SpanishDateTime.MonthEndRegex)
        self._of_month = RegExpUtility.get_safe_reg_exp(
            SpanishDateTime.OfMonthRegex)
        self._date_unit_regex = RegExpUtility.get_safe_reg_exp(
            SpanishDateTime.DateUnitRegex)
        self._for_the_regex = RegExpUtility.get_safe_reg_exp(
            SpanishDateTime.ForTheRegex)
        self._week_day_and_day_of_month_regex = RegExpUtility.get_safe_reg_exp(
            SpanishDateTime.WeekDayAndDayOfMonthRegex)
        self._relative_month_regex = RegExpUtility.get_safe_reg_exp(
            SpanishDateTime.RelativeMonthRegex)
        self._week_day_regex = RegExpUtility.get_safe_reg_exp(
            SpanishDateTime.WeekDayRegex)
        self._day_of_week = SpanishDateTime.DayOfWeek
        self._ordinal_extractor = SpanishOrdinalExtractor()
        self._integer_extractor = SpanishIntegerExtractor()
        self._number_parser = BaseNumberParser(
            SpanishNumberParserConfiguration())
        self._duration_extractor = BaseDurationExtractor(
            SpanishDurationExtractorConfiguration())
        self._utility_configuration = SpanishDateTimeUtilityConfiguration()
        self._range_connector_symbol_regex = RegExpUtility.get_safe_reg_exp(
            BaseDateTime.RangeConnectorSymbolRegex
        )
        self._strict_relative_regex = RegExpUtility.get_safe_reg_exp(
            SpanishDateTime.StrictRelativeRegex
        )
        self._year_suffix = RegExpUtility.get_safe_reg_exp(
            SpanishDateTime.YearSuffix
        )
        self._month_of_year = SpanishDateTime.MonthOfYear
        self._prefix_article_regex = RegExpUtility.get_safe_reg_exp(
            SpanishDateTime.PrefixArticleRegex
        )
        self._week_day_end = RegExpUtility.get_safe_reg_exp(
            SpanishDateTime.WeekDayEnd
        )
        self._more_than_regex = RegExpUtility.get_safe_reg_exp(
            SpanishDateTime.MoreThanRegex
        )
        self._less_than_regex = RegExpUtility.get_safe_reg_exp(
            SpanishDateTime.LessThanRegex
        )
        self._in_connector_regex = RegExpUtility.get_safe_reg_exp(
            SpanishDateTime.InConnectorRegex
        )
        self._range_unit_regex = RegExpUtility.get_safe_reg_exp(
            SpanishDateTime.RangeUnitRegex
        )
        self._since_year_suffix_regex = RegExpUtility.get_safe_reg_exp(
            SpanishDateTime.SinceYearSuffixRegex
        )
        self._week_day_and_day_regex = RegExpUtility.get_safe_reg_exp(
            SpanishDateTime.WeekDayAndDayRegex
        )
