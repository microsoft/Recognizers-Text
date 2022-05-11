#  Copyright (c) Microsoft Corporation. All rights reserved.
#  Licensed under the MIT License.

from typing import Pattern, List, Dict
from recognizers_number import (BaseNumberExtractor, BaseNumberParser,
                                ItalianOrdinalExtractor, ItalianIntegerExtractor, ItalianNumberParserConfiguration)
from recognizers_text.utilities import RegExpUtility
from ...resources.italian_date_time import ItalianDateTime
from ..extractors import DateTimeExtractor
from ..base_duration import BaseDurationExtractor
from ..base_date import DateExtractorConfiguration
from ..utilities import DateTimeUtilityConfiguration
from .duration_extractor_config import ItalianDurationExtractorConfiguration
from .base_configs import ItalianDateTimeUtilityConfiguration
from ..constants import Constants
from ...resources.base_date_time import BaseDateTime
from ..utilities import DateTimeOptions


class ItalianDateExtractorConfiguration(DateExtractorConfiguration):

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
        self._check_both_before_after = False
        if ItalianDateTime.DefaultLanguageFallback == Constants.DEFAULT_LANGUAGE_FALLBACK_DMY:
            date_extractor_4 = ItalianDateTime.DateExtractor5
            date_extractor_5 = ItalianDateTime.DateExtractor4
            date_extractor_6 = ItalianDateTime.DateExtractor7
            date_extractor_7 = ItalianDateTime.DateExtractor6
        else:
            date_extractor_4 = ItalianDateTime.DateExtractor4
            date_extractor_5 = ItalianDateTime.DateExtractor5
            date_extractor_6 = ItalianDateTime.DateExtractor6
            date_extractor_7 = ItalianDateTime.DateExtractor7

        self._date_regex_list = [
            RegExpUtility.get_safe_reg_exp(ItalianDateTime.DateExtractor1),
            RegExpUtility.get_safe_reg_exp(ItalianDateTime.DateExtractor2),
            RegExpUtility.get_safe_reg_exp(ItalianDateTime.DateExtractor3),
            RegExpUtility.get_safe_reg_exp(date_extractor_4),
            RegExpUtility.get_safe_reg_exp(date_extractor_5),
            RegExpUtility.get_safe_reg_exp(date_extractor_6),
            RegExpUtility.get_safe_reg_exp(date_extractor_7),
            RegExpUtility.get_safe_reg_exp(ItalianDateTime.DateExtractor8),
            RegExpUtility.get_safe_reg_exp(ItalianDateTime.DateExtractor9),
            RegExpUtility.get_safe_reg_exp(ItalianDateTime.DateExtractorA),
        ]

        self._implicit_date_list = [
            RegExpUtility.get_safe_reg_exp(ItalianDateTime.OnRegex),
            RegExpUtility.get_safe_reg_exp(ItalianDateTime.RelaxedOnRegex),
            RegExpUtility.get_safe_reg_exp(ItalianDateTime.SpecialDayRegex),
            RegExpUtility.get_safe_reg_exp(ItalianDateTime.ThisRegex),
            RegExpUtility.get_safe_reg_exp(ItalianDateTime.LastDateRegex),
            RegExpUtility.get_safe_reg_exp(ItalianDateTime.NextDateRegex),
            RegExpUtility.get_safe_reg_exp(ItalianDateTime.StrictWeekDay),
            RegExpUtility.get_safe_reg_exp(ItalianDateTime.WeekDayOfMonthRegex),
            RegExpUtility.get_safe_reg_exp(ItalianDateTime.SpecialDate),
        ]
        self._month_end = RegExpUtility.get_safe_reg_exp(
            ItalianDateTime.MonthEnd)
        self._of_month = RegExpUtility.get_safe_reg_exp(ItalianDateTime.OfMonth)
        self._date_unit_regex = RegExpUtility.get_safe_reg_exp(
            ItalianDateTime.DateUnitRegex)
        self._for_the_regex = RegExpUtility.get_safe_reg_exp(
            ItalianDateTime.ForTheRegex)
        self._week_day_and_day_of_month_regex = RegExpUtility.get_safe_reg_exp(
            ItalianDateTime.WeekDayAndDayOfMonthRegex)
        self._relative_month_regex = RegExpUtility.get_safe_reg_exp(
            ItalianDateTime.RelativeMonthRegex)
        self._week_day_regex = RegExpUtility.get_safe_reg_exp(
            ItalianDateTime.WeekDayRegex)
        self._day_of_week = ItalianDateTime.DayOfWeek
        self._ordinal_extractor = ItalianOrdinalExtractor()
        self._integer_extractor = ItalianIntegerExtractor()
        self._number_parser = BaseNumberParser(
            ItalianNumberParserConfiguration())
        self._duration_extractor = BaseDurationExtractor(
            ItalianDurationExtractorConfiguration())
        self._utility_configuration = ItalianDateTimeUtilityConfiguration()
        self._range_connector_symbol_regex = RegExpUtility.get_safe_reg_exp(
            BaseDateTime.RangeConnectorSymbolRegex
        )
        self._strict_relative_regex = RegExpUtility.get_safe_reg_exp(
            ItalianDateTime.StrictRelativeRegex
        )
        self._year_suffix = RegExpUtility.get_safe_reg_exp(
            ItalianDateTime.YearSuffix
        )
        self._month_of_year = ItalianDateTime.MonthOfYear
        self._prefix_article_regex = RegExpUtility.get_safe_reg_exp(
            ItalianDateTime.PrefixArticleRegex
        )
        self._week_day_end = RegExpUtility.get_safe_reg_exp(
            ItalianDateTime.WeekDayEnd
        )
        self._more_than_regex = RegExpUtility.get_safe_reg_exp(
            ItalianDateTime.MoreThanRegex
        )
        self._less_than_regex = RegExpUtility.get_safe_reg_exp(
            ItalianDateTime.LessThanRegex
        )
        self._in_connector_regex = RegExpUtility.get_safe_reg_exp(
            ItalianDateTime.InConnectorRegex
        )
        self._range_unit_regex = RegExpUtility.get_safe_reg_exp(
            ItalianDateTime.RangeUnitRegex
        )
        self._since_year_suffix_regex = RegExpUtility.get_safe_reg_exp(
            ItalianDateTime.SinceYearSuffixRegex
        )
        self._week_day_and_day_regex = RegExpUtility.get_safe_reg_exp(
            ItalianDateTime.WeekDayAndDayRegex
        )
        self._week_day_start = RegExpUtility.get_safe_reg_exp(
            ItalianDateTime.WeekDayStart
        )
        self._check_both_before_after = ItalianDateTime.CheckBothBeforeAfter
