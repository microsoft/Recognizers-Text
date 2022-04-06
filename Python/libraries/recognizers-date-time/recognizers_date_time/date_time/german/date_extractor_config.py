#  Copyright (c) Microsoft Corporation. All rights reserved.
#  Licensed under the MIT License.

from typing import Pattern, List, Dict
from recognizers_number import (BaseNumberExtractor, BaseNumberParser,
                                GermanOrdinalExtractor, GermanIntegerExtractor, GermanNumberParserConfiguration)
from recognizers_text.utilities import RegExpUtility
from ...resources.german_date_time import GermanDateTime
from ..extractors import DateTimeExtractor
from ..base_duration import BaseDurationExtractor
from ..base_date import DateExtractorConfiguration
from ..utilities import DateTimeUtilityConfiguration
from .duration_extractor_config import GermanDurationExtractorConfiguration
from .base_configs import GermanDateTimeUtilityConfiguration
from ..constants import Constants
from ...resources.base_date_time import BaseDateTime


class GermanDateExtractorConfiguration(DateExtractorConfiguration):

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
        if GermanDateTime.DefaultLanguageFallback == Constants.DEFAULT_LANGUAGE_FALLBACK_DMY:
            date_extractor_4 = GermanDateTime.DateExtractor5
            date_extractor_5 = GermanDateTime.DateExtractor4
            date_extractor_6 = GermanDateTime.DateExtractor7
            date_extractor_7 = GermanDateTime.DateExtractor6
        else:
            date_extractor_4 = GermanDateTime.DateExtractor4
            date_extractor_5 = GermanDateTime.DateExtractor5
            date_extractor_6 = GermanDateTime.DateExtractor6
            date_extractor_7 = GermanDateTime.DateExtractor7

        self._date_regex_list = [
            RegExpUtility.get_safe_reg_exp(GermanDateTime.DateExtractor1),
            RegExpUtility.get_safe_reg_exp(GermanDateTime.DateExtractor2),
            RegExpUtility.get_safe_reg_exp(GermanDateTime.DateExtractor3),
            RegExpUtility.get_safe_reg_exp(date_extractor_4),
            RegExpUtility.get_safe_reg_exp(date_extractor_5),
            RegExpUtility.get_safe_reg_exp(date_extractor_6),
            RegExpUtility.get_safe_reg_exp(date_extractor_7),
            RegExpUtility.get_safe_reg_exp(GermanDateTime.DateExtractor8),
            RegExpUtility.get_safe_reg_exp(GermanDateTime.DateExtractor9),
            RegExpUtility.get_safe_reg_exp(GermanDateTime.DateExtractorA),
        ]

        self._implicit_date_list = [
            RegExpUtility.get_safe_reg_exp(GermanDateTime.OnRegex),
            RegExpUtility.get_safe_reg_exp(GermanDateTime.RelaxedOnRegex),
            RegExpUtility.get_safe_reg_exp(GermanDateTime.SpecialDayRegex),
            RegExpUtility.get_safe_reg_exp(GermanDateTime.ThisRegex),
            RegExpUtility.get_safe_reg_exp(GermanDateTime.LastDateRegex),
            RegExpUtility.get_safe_reg_exp(GermanDateTime.NextDateRegex),
            RegExpUtility.get_safe_reg_exp(GermanDateTime.SingleWeekDayRegex),
            RegExpUtility.get_safe_reg_exp(GermanDateTime.WeekDayOfMonthRegex),
            RegExpUtility.get_safe_reg_exp(GermanDateTime.SpecialDate),
        ]
        self._month_end = RegExpUtility.get_safe_reg_exp(
            GermanDateTime.MonthEnd)
        self._of_month = RegExpUtility.get_safe_reg_exp(GermanDateTime.OfMonth)
        self._date_unit_regex = RegExpUtility.get_safe_reg_exp(
            GermanDateTime.DateUnitRegex)
        self._for_the_regex = RegExpUtility.get_safe_reg_exp(
            GermanDateTime.ForTheRegex)
        self._week_day_and_day_of_month_regex = RegExpUtility.get_safe_reg_exp(
            GermanDateTime.WeekDayAndDayOfMonthRegex)
        self._relative_month_regex = RegExpUtility.get_safe_reg_exp(
            GermanDateTime.RelativeMonthRegex)
        self._week_day_regex = RegExpUtility.get_safe_reg_exp(
            GermanDateTime.WeekDayRegex)
        self._day_of_week = GermanDateTime.DayOfWeek
        self._ordinal_extractor = GermanOrdinalExtractor()
        self._integer_extractor = GermanIntegerExtractor()
        self._number_parser = BaseNumberParser(
            GermanNumberParserConfiguration())
        self._duration_extractor = BaseDurationExtractor(
            GermanDurationExtractorConfiguration())
        self._utility_configuration = GermanDateTimeUtilityConfiguration()
        self._range_connector_symbol_regex = RegExpUtility.get_safe_reg_exp(
            BaseDateTime.RangeConnectorSymbolRegex
        )
        self._strict_relative_regex = RegExpUtility.get_safe_reg_exp(
            GermanDateTime.StrictRelativeRegex
        )
        self._year_suffix = RegExpUtility.get_safe_reg_exp(
            GermanDateTime.YearSuffix
        )
        self._month_of_year = GermanDateTime.MonthOfYear
        self._prefix_article_regex = RegExpUtility.get_safe_reg_exp(
            GermanDateTime.PrefixArticleRegex
        )
        self._week_day_end = RegExpUtility.get_safe_reg_exp(
            GermanDateTime.WeekDayEnd
        )
        self._more_than_regex = RegExpUtility.get_safe_reg_exp(
            GermanDateTime.MoreThanRegex
        )
        self._less_than_regex = RegExpUtility.get_safe_reg_exp(
            GermanDateTime.LessThanRegex
        )
        self._in_connector_regex = RegExpUtility.get_safe_reg_exp(
            GermanDateTime.InConnectorRegex
        )
        self._range_unit_regex = RegExpUtility.get_safe_reg_exp(
            GermanDateTime.RangeUnitRegex
        )
        self._since_year_suffix_regex = RegExpUtility.get_safe_reg_exp(
            GermanDateTime.SinceYearSuffixRegex
        )
        self._week_day_and_day_regex = RegExpUtility.get_safe_reg_exp(
            GermanDateTime.WeekDayAndDayRegex
        )
        self._week_day_start = RegExpUtility.get_safe_reg_exp(
            GermanDateTime.WeekDayStart
        )
        self._check_both_before_after = GermanDateTime.CheckBothBeforeAfter
