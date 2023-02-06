#  Copyright (c) Microsoft Corporation. All rights reserved.
#  Licensed under the MIT License.

from typing import Pattern, List, Dict
from recognizers_number import (BaseNumberExtractor, BaseNumberParser,
                                DutchOrdinalExtractor, DutchIntegerExtractor, DutchNumberParserConfiguration)
from recognizers_text.utilities import RegExpUtility
from ...resources.dutch_date_time import DutchDateTime
from ..extractors import DateTimeExtractor
from ..base_duration import BaseDurationExtractor
from ..base_date import DateExtractorConfiguration
from ..utilities import DateTimeUtilityConfiguration
from .duration_extractor_config import DutchDurationExtractorConfiguration
from .base_configs import DutchDateTimeUtilityConfiguration
from ..constants import Constants
from ...resources.base_date_time import BaseDateTime


class DutchDateExtractorConfiguration(DateExtractorConfiguration):

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
        if DutchDateTime.DefaultLanguageFallback == Constants.DEFAULT_LANGUAGE_FALLBACK_DMY:
            date_extractor_4 = DutchDateTime.DateExtractor5
            date_extractor_5 = DutchDateTime.DateExtractor8
            date_extractor_6 = DutchDateTime.DateExtractor9L
            date_extractor_7 = DutchDateTime.DateExtractor9S
            date_extractor_8 = DutchDateTime.DateExtractor4
            date_extractor_9 = DutchDateTime.DateExtractor6
            date_extractor_10 = DutchDateTime.DateExtractor7L
            date_extractor_11 = DutchDateTime.DateExtractor7S
        else:
            date_extractor_4 = DutchDateTime.DateExtractor4
            date_extractor_5 = DutchDateTime.DateExtractor6
            date_extractor_6 = DutchDateTime.DateExtractor7L
            date_extractor_7 = DutchDateTime.DateExtractor7S
            date_extractor_8 = DutchDateTime.DateExtractor5
            date_extractor_9 = DutchDateTime.DateExtractor8
            date_extractor_10 = DutchDateTime.DateExtractor9L
            date_extractor_11 = DutchDateTime.DateExtractor9S

        self._date_regex_list = [
            RegExpUtility.get_safe_reg_exp(DutchDateTime.DateExtractor1),
            RegExpUtility.get_safe_reg_exp(DutchDateTime.DateExtractor3),
            RegExpUtility.get_safe_reg_exp(date_extractor_4),
            RegExpUtility.get_safe_reg_exp(date_extractor_5),
            RegExpUtility.get_safe_reg_exp(date_extractor_6),
            RegExpUtility.get_safe_reg_exp(date_extractor_7),
            RegExpUtility.get_safe_reg_exp(date_extractor_8),
            RegExpUtility.get_safe_reg_exp(date_extractor_9),
            RegExpUtility.get_safe_reg_exp(date_extractor_10),
            RegExpUtility.get_safe_reg_exp(date_extractor_11),
            RegExpUtility.get_safe_reg_exp(DutchDateTime.DateExtractorA),
        ]

        self._implicit_date_list = [
            RegExpUtility.get_safe_reg_exp(DutchDateTime.OnRegex),
            RegExpUtility.get_safe_reg_exp(DutchDateTime.RelaxedOnRegex),
            RegExpUtility.get_safe_reg_exp(DutchDateTime.SpecialDayRegex),
            RegExpUtility.get_safe_reg_exp(DutchDateTime.ThisRegex),
            RegExpUtility.get_safe_reg_exp(DutchDateTime.LastDateRegex),
            RegExpUtility.get_safe_reg_exp(DutchDateTime.NextDateRegex),
            RegExpUtility.get_safe_reg_exp(DutchDateTime.SingleWeekDayRegex),
            RegExpUtility.get_safe_reg_exp(DutchDateTime.WeekDayOfMonthRegex),
            RegExpUtility.get_safe_reg_exp(DutchDateTime.SpecialDate),
            RegExpUtility.get_safe_reg_exp(DutchDateTime.SpecialDayWithNumRegex),
            RegExpUtility.get_safe_reg_exp(DutchDateTime.RelativeWeekDayRegex),
        ]
        self._month_end = RegExpUtility.get_safe_reg_exp(
            DutchDateTime.MonthEnd)
        self._of_month = RegExpUtility.get_safe_reg_exp(DutchDateTime.OfMonth)
        self._date_unit_regex = RegExpUtility.get_safe_reg_exp(
            DutchDateTime.DateUnitRegex)
        self._for_the_regex = RegExpUtility.get_safe_reg_exp(
            DutchDateTime.ForTheRegex)
        self._week_day_and_day_of_month_regex = RegExpUtility.get_safe_reg_exp(
            DutchDateTime.WeekDayAndDayOfMonthRegex)
        self._relative_month_regex = RegExpUtility.get_safe_reg_exp(
            DutchDateTime.RelativeMonthRegex)
        self._week_day_regex = RegExpUtility.get_safe_reg_exp(
            DutchDateTime.WeekDayRegex)
        self._day_of_week = DutchDateTime.DayOfWeek
        self._ordinal_extractor = DutchOrdinalExtractor()
        self._integer_extractor = DutchIntegerExtractor()
        self._number_parser = BaseNumberParser(
            DutchNumberParserConfiguration())
        self._duration_extractor = BaseDurationExtractor(
            DutchDurationExtractorConfiguration())
        self._utility_configuration = DutchDateTimeUtilityConfiguration()
        self._range_connector_symbol_regex = RegExpUtility.get_safe_reg_exp(
            BaseDateTime.RangeConnectorSymbolRegex
        )
        self._strict_relative_regex = RegExpUtility.get_safe_reg_exp(
            DutchDateTime.StrictRelativeRegex
        )
        self._year_suffix = RegExpUtility.get_safe_reg_exp(
            DutchDateTime.YearSuffix
        )
        self._month_of_year = DutchDateTime.MonthOfYear
        self._prefix_article_regex = RegExpUtility.get_safe_reg_exp(
            DutchDateTime.PrefixArticleRegex
        )
        self._week_day_end = RegExpUtility.get_safe_reg_exp(
            DutchDateTime.WeekDayEnd
        )
        self._more_than_regex = RegExpUtility.get_safe_reg_exp(
            DutchDateTime.MoreThanRegex
        )
        self._less_than_regex = RegExpUtility.get_safe_reg_exp(
            DutchDateTime.LessThanRegex
        )
        self._in_connector_regex = RegExpUtility.get_safe_reg_exp(
            DutchDateTime.InConnectorRegex
        )
        self._range_unit_regex = RegExpUtility.get_safe_reg_exp(
            DutchDateTime.RangeUnitRegex
        )
        self._since_year_suffix_regex = RegExpUtility.get_safe_reg_exp(
            DutchDateTime.SinceYearSuffixRegex
        )
        self._week_day_and_day_regex = RegExpUtility.get_safe_reg_exp(
            DutchDateTime.WeekDayAndDayRegex
        )
        self._week_day_start = RegExpUtility.get_safe_reg_exp(
            DutchDateTime.WeekDayStart
        )
        self._check_both_before_after = DutchDateTime.CheckBothBeforeAfter
