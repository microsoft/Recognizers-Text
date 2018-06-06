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

class EnglishDateExtractorConfiguration(DateExtractorConfiguration):
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
        self._date_regex_list = [
            RegExpUtility.get_safe_reg_exp(EnglishDateTime.DateExtractor1),
            RegExpUtility.get_safe_reg_exp(EnglishDateTime.DateExtractor2),
            RegExpUtility.get_safe_reg_exp(EnglishDateTime.DateExtractor3),
            RegExpUtility.get_safe_reg_exp(EnglishDateTime.DateExtractor4),
            RegExpUtility.get_safe_reg_exp(EnglishDateTime.DateExtractor5),
            RegExpUtility.get_safe_reg_exp(EnglishDateTime.DateExtractor6),
            RegExpUtility.get_safe_reg_exp(EnglishDateTime.DateExtractor7),
            RegExpUtility.get_safe_reg_exp(EnglishDateTime.DateExtractor8),
            RegExpUtility.get_safe_reg_exp(EnglishDateTime.DateExtractor9),
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
            RegExpUtility.get_safe_reg_exp(EnglishDateTime.WeekDayOfMonthRegex),
            RegExpUtility.get_safe_reg_exp(EnglishDateTime.SpecialDate),
        ]
        self._month_end = RegExpUtility.get_safe_reg_exp(EnglishDateTime.MonthEnd)
        self._of_month = RegExpUtility.get_safe_reg_exp(EnglishDateTime.OfMonth)
        self._date_unit_regex = RegExpUtility.get_safe_reg_exp(EnglishDateTime.DateUnitRegex)
        self._for_the_regex = RegExpUtility.get_safe_reg_exp(EnglishDateTime.ForTheRegex)
        self._week_day_and_day_of_month_regex = RegExpUtility.get_safe_reg_exp(
            EnglishDateTime.WeekDayAndDayOfMonthRegex)
        self._relative_month_regex = RegExpUtility.get_safe_reg_exp(EnglishDateTime.RelativeMonthRegex)
        self._week_day_regex = RegExpUtility.get_safe_reg_exp(EnglishDateTime.WeekDayRegex)
        self._day_of_week = EnglishDateTime.DayOfWeek
        self._ordinal_extractor = EnglishOrdinalExtractor()
        self._integer_extractor = EnglishIntegerExtractor()
        self._number_parser = BaseNumberParser(EnglishNumberParserConfiguration())
        self._duration_extractor = BaseDurationExtractor(EnglishDurationExtractorConfiguration())
        self._utility_configuration = EnglishDateTimeUtilityConfiguration()
