#  Copyright (c) Microsoft Corporation. All rights reserved.
#  Licensed under the MIT License.

from typing import Pattern, List, Dict
from recognizers_number import (BaseNumberExtractor, BaseNumberParser,
                                EnglishOrdinalExtractor, EnglishIntegerExtractor, EnglishNumberParserConfiguration)
from recognizers_text.utilities import RegExpUtility
from ...resources.english_date_time import EnglishDateTime
from ..extractors import DateTimeExtractor
from ..base_duration import BaseDurationExtractor
from ..base_date import DateExtractorConfiguration
from ..utilities import DateTimeUtilityConfiguration
from .duration_extractor_config import EnglishDurationExtractorConfiguration
from .base_configs import EnglishDateTimeUtilityConfiguration
from ...resources.base_date_time import BaseDateTime


class EnglishDateExtractorConfiguration(DateExtractorConfiguration):
    @property
    def week_day_start(self) -> Pattern:
        return self._week_day_start

    @property
    def check_both_before_after(self) -> Pattern:
        return self._check_both_before_after

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
    def week_day_start(self) -> Pattern:
        return self._week_day_start

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

    @property
    def month_regex(self) -> Pattern:
        return self._month_regex

    @property
    def month_num_regex(self) -> Pattern:
        return self._month_num_regex

    @property
    def year_regex(self) -> Pattern:
        return self._year_regex

    @property
    def written_month_regex(self) -> Pattern:
        return self._written_month_regex

    @property
    def month_suffix_regex(self) -> Pattern:
        return self._month_suffix_regex

    def __init__(self):
        self._check_both_before_after = EnglishDateTime.CheckBothBeforeAfter
        self._date_regex_list = [
            RegExpUtility.get_safe_reg_exp(EnglishDateTime.DateExtractor1),
            RegExpUtility.get_safe_reg_exp(EnglishDateTime.DateExtractor3),
            RegExpUtility.get_safe_reg_exp(EnglishDateTime.DateExtractor4),
            RegExpUtility.get_safe_reg_exp(EnglishDateTime.DateExtractor5),
            RegExpUtility.get_safe_reg_exp(EnglishDateTime.DateExtractor6),
            RegExpUtility.get_safe_reg_exp(EnglishDateTime.DateExtractor7L),
            RegExpUtility.get_safe_reg_exp(EnglishDateTime.DateExtractor7S),
            RegExpUtility.get_safe_reg_exp(EnglishDateTime.DateExtractor8),
            RegExpUtility.get_safe_reg_exp(EnglishDateTime.DateExtractor9L),
            RegExpUtility.get_safe_reg_exp(EnglishDateTime.DateExtractor9S),
            RegExpUtility.get_safe_reg_exp(EnglishDateTime.DateExtractorA),
        ]
        self._implicit_date_list = [
            RegExpUtility.get_safe_reg_exp(EnglishDateTime.OnRegex),
            RegExpUtility.get_safe_reg_exp(EnglishDateTime.RelaxedOnRegex),
            RegExpUtility.get_safe_reg_exp(EnglishDateTime.SpecialDayRegex),
            RegExpUtility.get_safe_reg_exp(EnglishDateTime.ThisRegex),
            RegExpUtility.get_safe_reg_exp(EnglishDateTime.LastDateRegex),
            RegExpUtility.get_safe_reg_exp(EnglishDateTime.NextDateRegex),
            RegExpUtility.get_safe_reg_exp(EnglishDateTime.SingleWeekDayRegex),
            RegExpUtility.get_safe_reg_exp(
                EnglishDateTime.WeekDayOfMonthRegex),
            RegExpUtility.get_safe_reg_exp(EnglishDateTime.SpecialDate),
            RegExpUtility.get_safe_reg_exp(EnglishDateTime.SpecialDayWithNumRegex),
            RegExpUtility.get_safe_reg_exp(EnglishDateTime.RelativeWeekDayRegex)
        ]
        self._month_end = RegExpUtility.get_safe_reg_exp(
            EnglishDateTime.MonthEnd)
        self._of_month = RegExpUtility.get_safe_reg_exp(
            EnglishDateTime.OfMonth)
        self._date_unit_regex = RegExpUtility.get_safe_reg_exp(
            EnglishDateTime.DateUnitRegex)
        self._for_the_regex = RegExpUtility.get_safe_reg_exp(
            EnglishDateTime.ForTheRegex)
        self._week_day_and_day_of_month_regex = RegExpUtility.get_safe_reg_exp(
            EnglishDateTime.WeekDayAndDayOfMonthRegex)
        self._relative_month_regex = RegExpUtility.get_safe_reg_exp(
            EnglishDateTime.RelativeMonthRegex)
        self._week_day_regex = RegExpUtility.get_safe_reg_exp(
            EnglishDateTime.WeekDayRegex)
        self._day_of_week = EnglishDateTime.DayOfWeek
        self._ordinal_extractor = EnglishOrdinalExtractor()
        self._integer_extractor = EnglishIntegerExtractor()
        self._number_parser = BaseNumberParser(
            EnglishNumberParserConfiguration())
        self._duration_extractor = BaseDurationExtractor(
            EnglishDurationExtractorConfiguration())
        self._utility_configuration = EnglishDateTimeUtilityConfiguration()
        self._range_connector_symbol_regex = RegExpUtility.get_safe_reg_exp(
            BaseDateTime.RangeConnectorSymbolRegex
        )
        self._strict_relative_regex = RegExpUtility.get_safe_reg_exp(
            EnglishDateTime.StrictRelativeRegex
        )
        self._year_suffix = RegExpUtility.get_safe_reg_exp(
            EnglishDateTime.YearSuffix
        )
        self._month_of_year = EnglishDateTime.MonthOfYear
        self._prefix_article_regex = RegExpUtility.get_safe_reg_exp(
            EnglishDateTime.PrefixArticleRegex
        )
        self._week_day_end = RegExpUtility.get_safe_reg_exp(
            EnglishDateTime.WeekDayEnd
        )
        self._week_day_start = RegExpUtility.get_safe_reg_exp(
            EnglishDateTime.WeekDayStart
        )
        self._more_than_regex = RegExpUtility.get_safe_reg_exp(
            EnglishDateTime.MoreThanRegex
        )
        self._less_than_regex = RegExpUtility.get_safe_reg_exp(
            EnglishDateTime.LessThanRegex
        )
        self._in_connector_regex = RegExpUtility.get_safe_reg_exp(
            EnglishDateTime.InConnectorRegex
        )
        self._range_unit_regex = RegExpUtility.get_safe_reg_exp(
            EnglishDateTime.RangeUnitRegex
        )
        self._since_year_suffix_regex = RegExpUtility.get_safe_reg_exp(
            EnglishDateTime.SinceYearSuffixRegex
        )
        self._week_day_and_day_regex = RegExpUtility.get_safe_reg_exp(
            EnglishDateTime.WeekDayAndDayRegex
        )
        self._week_day_start = RegExpUtility.get_safe_reg_exp(
            EnglishDateTime.WeekDayStart
        )
        self._check_both_before_after = EnglishDateTime.CheckBothBeforeAfter

        self._month_regex = RegExpUtility.get_safe_reg_exp(
            EnglishDateTime.MonthRegex
        )
        self._month_num_regex = RegExpUtility.get_safe_reg_exp(
            EnglishDateTime.MonthNumRegex
        )
        self._year_regex = RegExpUtility.get_safe_reg_exp(
            EnglishDateTime.YearRegex
        )
        self._day_regex = RegExpUtility.get_safe_reg_exp(
            EnglishDateTime.DayRegex
        )
        self._written_month_regex = RegExpUtility.get_safe_reg_exp(
            EnglishDateTime.WrittenMonthRegex
        )
        self._month_suffix_regex = RegExpUtility.get_safe_reg_exp(
            EnglishDateTime.MonthSuffixRegex
        )
