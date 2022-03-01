#  Copyright (c) Microsoft Corporation. All rights reserved.
#  Licensed under the MIT License.

from typing import Pattern, List, Dict
from recognizers_number import (BaseNumberExtractor, BaseNumberParser,
                                PortugueseOrdinalExtractor, PortugueseIntegerExtractor, PortugueseNumberParserConfiguration)
from recognizers_text.utilities import RegExpUtility
from ...resources.portuguese_date_time import PortugueseDateTime
from ..extractors import DateTimeExtractor
from ..base_duration import BaseDurationExtractor
from ..base_date import DateExtractorConfiguration
from ..utilities import DateTimeUtilityConfiguration
from .duration_extractor_config import PortugueseDurationExtractorConfiguration
from .base_configs import PortugueseDateTimeUtilityConfiguration
from ..constants import Constants
from ...resources.base_date_time import BaseDateTime


class PortugueseDateExtractorConfiguration(DateExtractorConfiguration):
    @property
    def week_day_start(self) -> Pattern:
        return self._week_day_start

    @property
    def check_both_before_after(self) -> bool:
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
        self._check_both_before_after = False
        if PortugueseDateTime.DefaultLanguageFallback == Constants.DEFAULT_LANGUAGE_FALLBACK_DMY:
            date_extractor_4 = PortugueseDateTime.DateExtractor5
            date_extractor_5 = PortugueseDateTime.DateExtractor4
            date_extractor_6 = PortugueseDateTime.DateExtractor8
            date_extractor_8 = PortugueseDateTime.DateExtractor6
            date_extractor_7 = PortugueseDateTime.DateExtractor9
            date_extractor_9 = PortugueseDateTime.DateExtractor7
        else:
            date_extractor_4 = PortugueseDateTime.DateExtractor4
            date_extractor_5 = PortugueseDateTime.DateExtractor5
            date_extractor_6 = PortugueseDateTime.DateExtractor6
            date_extractor_8 = PortugueseDateTime.DateExtractor8
            date_extractor_7 = PortugueseDateTime.DateExtractor7
            date_extractor_9 = PortugueseDateTime.DateExtractor9

        self._date_regex_list = [
            RegExpUtility.get_safe_reg_exp(PortugueseDateTime.DateExtractor1),
            RegExpUtility.get_safe_reg_exp(PortugueseDateTime.DateExtractor2),
            RegExpUtility.get_safe_reg_exp(PortugueseDateTime.DateExtractor3),
            RegExpUtility.get_safe_reg_exp(date_extractor_4),
            RegExpUtility.get_safe_reg_exp(date_extractor_5),
            RegExpUtility.get_safe_reg_exp(date_extractor_6),
            RegExpUtility.get_safe_reg_exp(date_extractor_7),
            RegExpUtility.get_safe_reg_exp(date_extractor_8),
            RegExpUtility.get_safe_reg_exp(date_extractor_9),
            RegExpUtility.get_safe_reg_exp(PortugueseDateTime.DateExtractor10),
        ]

        self._implicit_date_list = [
            RegExpUtility.get_safe_reg_exp(PortugueseDateTime.OnRegex),
            RegExpUtility.get_safe_reg_exp(PortugueseDateTime.RelaxedOnRegex),
            RegExpUtility.get_safe_reg_exp(PortugueseDateTime.SpecialDayRegex),
            RegExpUtility.get_safe_reg_exp(PortugueseDateTime.ThisRegex),
            RegExpUtility.get_safe_reg_exp(PortugueseDateTime.LastDateRegex),
            RegExpUtility.get_safe_reg_exp(PortugueseDateTime.NextDateRegex),
            RegExpUtility.get_safe_reg_exp(PortugueseDateTime.WeekDayRegex),
            RegExpUtility.get_safe_reg_exp(
                PortugueseDateTime.WeekDayOfMonthRegex),
            RegExpUtility.get_safe_reg_exp(PortugueseDateTime.SpecialDateRegex),
        ]
        self._month_end = RegExpUtility.get_safe_reg_exp(
            PortugueseDateTime.MonthEndRegex)
        self._of_month = RegExpUtility.get_safe_reg_exp(
            PortugueseDateTime.OfMonthRegex)
        self._date_unit_regex = RegExpUtility.get_safe_reg_exp(
            PortugueseDateTime.DateUnitRegex)
        self._for_the_regex = RegExpUtility.get_safe_reg_exp(
            PortugueseDateTime.ForTheRegex)
        self._week_day_and_day_of_month_regex = RegExpUtility.get_safe_reg_exp(
            PortugueseDateTime.WeekDayAndDayOfMonthRegex)
        self._relative_month_regex = RegExpUtility.get_safe_reg_exp(
            PortugueseDateTime.RelativeMonthRegex)
        self._week_day_regex = RegExpUtility.get_safe_reg_exp(
            PortugueseDateTime.WeekDayRegex)
        self._day_of_week = PortugueseDateTime.DayOfWeek
        self._ordinal_extractor = PortugueseOrdinalExtractor()
        self._integer_extractor = PortugueseIntegerExtractor()
        self._number_parser = BaseNumberParser(
            PortugueseNumberParserConfiguration())
        self._duration_extractor = BaseDurationExtractor(
            PortugueseDurationExtractorConfiguration())
        self._utility_configuration = PortugueseDateTimeUtilityConfiguration()
        self._range_connector_symbol_regex = RegExpUtility.get_safe_reg_exp(
            BaseDateTime.RangeConnectorSymbolRegex
        )
        self._strict_relative_regex = RegExpUtility.get_safe_reg_exp(
            PortugueseDateTime.StrictRelativeRegex
        )
        self._year_suffix = RegExpUtility.get_safe_reg_exp(
            PortugueseDateTime.YearSuffix
        )
        self._month_of_year = PortugueseDateTime.MonthOfYear
        self._prefix_article_regex = RegExpUtility.get_safe_reg_exp(
            PortugueseDateTime.PrefixArticleRegex
        )
        self._week_day_end = RegExpUtility.get_safe_reg_exp(
            PortugueseDateTime.WeekDayEnd
        )
        self._more_than_regex = RegExpUtility.get_safe_reg_exp(
            PortugueseDateTime.MoreThanRegex
        )
        self._less_than_regex = RegExpUtility.get_safe_reg_exp(
            PortugueseDateTime.LessThanRegex
        )
        self._in_connector_regex = RegExpUtility.get_safe_reg_exp(
            PortugueseDateTime.InConnectorRegex
        )
        self._range_unit_regex = RegExpUtility.get_safe_reg_exp(
            PortugueseDateTime.RangeUnitRegex
        )
        self._since_year_suffix_regex = RegExpUtility.get_safe_reg_exp(
            PortugueseDateTime.SinceYearSuffixRegex
        )
        self._week_day_and_day_regex = RegExpUtility.get_safe_reg_exp(
            PortugueseDateTime.WeekDayAndDayRegex
        )
        self._week_day_start = RegExpUtility.get_safe_reg_exp(
            PortugueseDateTime.WeekDayStart
        )
        self._check_both_before_after = PortugueseDateTime.CheckBothBeforeAfter
