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
    def day_of_week(self) -> Dict[str, int]:
        return self._day_of_week

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

    def __init__(self):
        if SpanishDateTime.DefaultLanguageFallback == Constants.DEFAULT_LANGUAGE_FALLBACK_DMY:
            date_extractor_4 = SpanishDateTime.DateExtractor5
            date_extractor_5 = SpanishDateTime.DateExtractor4
        else:
            date_extractor_4 = SpanishDateTime.DateExtractor4
            date_extractor_5 = SpanishDateTime.DateExtractor5

        self._date_regex_list = [
            RegExpUtility.get_safe_reg_exp(SpanishDateTime.DateExtractor1),
            RegExpUtility.get_safe_reg_exp(SpanishDateTime.DateExtractor2),
            RegExpUtility.get_safe_reg_exp(SpanishDateTime.DateExtractor3),
            RegExpUtility.get_safe_reg_exp(date_extractor_4),
            RegExpUtility.get_safe_reg_exp(date_extractor_5),
            RegExpUtility.get_safe_reg_exp(SpanishDateTime.DateExtractor6),
            RegExpUtility.get_safe_reg_exp(SpanishDateTime.DateExtractor7),
            RegExpUtility.get_safe_reg_exp(SpanishDateTime.DateExtractor8),
            RegExpUtility.get_safe_reg_exp(SpanishDateTime.DateExtractor9),
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
            RegExpUtility.get_safe_reg_exp(SpanishDateTime.WeekDayOfMonthRegex),
            RegExpUtility.get_safe_reg_exp(SpanishDateTime.SpecialDateRegex),
        ]
        self._month_end = RegExpUtility.get_safe_reg_exp(SpanishDateTime.MonthEndRegex)
        self._of_month = RegExpUtility.get_safe_reg_exp(SpanishDateTime.OfMonthRegex)
        self._date_unit_regex = RegExpUtility.get_safe_reg_exp(SpanishDateTime.DateUnitRegex)
        self._for_the_regex = RegExpUtility.get_safe_reg_exp(SpanishDateTime.ForTheRegex)
        self._week_day_and_day_of_month_regex = RegExpUtility.get_safe_reg_exp(
            SpanishDateTime.WeekDayAndDayOfMonthRegex)
        self._relative_month_regex = RegExpUtility.get_safe_reg_exp(SpanishDateTime.RelativeMonthRegex)
        self._week_day_regex = RegExpUtility.get_safe_reg_exp(SpanishDateTime.WeekDayRegex)
        self._day_of_week = SpanishDateTime.DayOfWeek
        self._ordinal_extractor = SpanishOrdinalExtractor()
        self._integer_extractor = SpanishIntegerExtractor()
        self._number_parser = BaseNumberParser(SpanishNumberParserConfiguration())
        self._duration_extractor = BaseDurationExtractor(SpanishDurationExtractorConfiguration())
        self._utility_configuration = SpanishDateTimeUtilityConfiguration()
