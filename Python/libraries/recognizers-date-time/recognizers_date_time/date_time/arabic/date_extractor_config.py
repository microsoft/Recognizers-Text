from typing import Pattern, List, Dict
from recognizers_number import (BaseNumberExtractor, BaseNumberParser,
                                ArabicOrdinalExtractor, ArabicIntegerExtractor, ArabicNumberParserConfiguration)
from recognizers_text.utilities import RegExpUtility
from recognizers_date_time.resources import ArabicDateTime, BaseDateTime
from recognizers_date_time.date_time.extractors import DateTimeExtractor
from recognizers_date_time.date_time.base_date import DateExtractorConfiguration
from recognizers_date_time.date_time.base_duration import BaseDurationExtractor
from recognizers_date_time.date_time.utilities import DateTimeUtilityConfiguration
from recognizers_date_time.date_time.arabic.base_configs import ArabicDateTimeUtilityConfiguration
from recognizers_date_time.date_time.arabic.duration_extractor_config import ArabicDurationExtractorConfiguration
from recognizers_date_time.date_time.constants import Constants


class ArabicDateExtractorConfiguration(DateExtractorConfiguration):

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
    def week_day_regex(self) -> Pattern:
        return self._week_day_regex

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
    def week_day_and_day_regex(self) -> Pattern:
        return self._week_day_and_day_regex

    @property
    def relative_month_regex(self) -> Pattern:
        return self._relative_month_regex

    @property
    def strict_relative_regex(self) -> Pattern:
        return self._strict_relative_regex

    @property
    def prefix_article_regex(self) -> Pattern:
        return self._prefix_article_regex

    @property
    def of_month(self) -> Pattern:
        return self._of_month

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
    def year_suffix(self) -> Pattern:
        return self._year_suffix

    @property
    def less_than_regex(self) -> Pattern:
        return self._less_than_regex

    @property
    def more_than_regex(self) -> Pattern:
        return self._more_than_regex

    @property
    def in_connector_regex(self) -> Pattern:
        return self._in_connector_regex

    @property
    def range_unit_regex(self) -> Pattern:
        return self._range_unit_regex

    @property
    def range_connector_symbol_regex(self) -> Pattern:
        return self._range_connector_symbol_regex

    @property
    def before_after_regex(self) -> Pattern:
        return self._before_after_regex

    @property
    def day_of_week(self) -> Dict[str, int]:
        return self._day_of_week

    @property
    def month_of_year(self) -> Dict[str, int]:
        return self._month_of_year

    @property
    def since_year_suffix_regex(self) -> Pattern:
        return self._since_year_suffix_regex

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
        self._year_regex = RegExpUtility.get_safe_reg_exp(ArabicDateTime.YearRegex)
        self._before_after_regex = RegExpUtility.get_safe_reg_exp(ArabicDateTime.BeforeAfterRegex)
        self._month_num_regex = RegExpUtility.get_safe_reg_exp(ArabicDateTime.MonthNumRegex)
        self._month_regex = RegExpUtility.get_safe_reg_exp(ArabicDateTime.MonthRegex)

        self._ordinal_extractor = ArabicOrdinalExtractor()
        self._integer_extractor = ArabicIntegerExtractor()

        self._number_parser = BaseNumberParser(
            ArabicNumberParserConfiguration())
        self._duration_extractor = self._duration_extractor = BaseDurationExtractor(
            ArabicDurationExtractorConfiguration())
        self._utility_configuration = ArabicDateTimeUtilityConfiguration()

        self._implicit_date_list = [
            RegExpUtility.get_safe_reg_exp(ArabicDateTime.OnRegex),
            RegExpUtility.get_safe_reg_exp(ArabicDateTime.RelaxedOnRegex),
            RegExpUtility.get_safe_reg_exp(ArabicDateTime.SpecialDayRegex),
            RegExpUtility.get_safe_reg_exp(ArabicDateTime.ThisRegex),
            RegExpUtility.get_safe_reg_exp(ArabicDateTime.LastDateRegex),
            RegExpUtility.get_safe_reg_exp(ArabicDateTime.NextDateRegex),
            RegExpUtility.get_safe_reg_exp(ArabicDateTime.SingleWeekDayRegex),
            RegExpUtility.get_safe_reg_exp(ArabicDateTime.WeekDayOfMonthRegex),
            RegExpUtility.get_safe_reg_exp(ArabicDateTime.SpecialDate),
            RegExpUtility.get_safe_reg_exp(ArabicDateTime.SpecialDayWithNumRegex),
            RegExpUtility.get_safe_reg_exp(ArabicDateTime.RelativeWeekDayRegex),
        ]

        if ArabicDateTime.DefaultLanguageFallback == Constants.DEFAULT_LANGUAGE_FALLBACK_DMY:
            date_extractor_4 = ArabicDateTime.DateExtractor5
            date_extractor_5 = ArabicDateTime.DateExtractor8
            date_extractor_6 = ArabicDateTime.DateExtractor7L
            date_extractor_7 = ArabicDateTime.DateExtractor9S
            date_extractor_8 = ArabicDateTime.DateExtractor4
            date_extractor_9 = ArabicDateTime.DateExtractor6
            date_extractor_10 = ArabicDateTime.DateExtractor9L
            date_extractor_11 = ArabicDateTime.DateExtractor7S
        else:
            date_extractor_4 = ArabicDateTime.DateExtractor4
            date_extractor_5 = ArabicDateTime.DateExtractor6
            date_extractor_6 = ArabicDateTime.DateExtractor7L
            date_extractor_7 = ArabicDateTime.DateExtractor7S
            date_extractor_8 = ArabicDateTime.DateExtractor5
            date_extractor_9 = ArabicDateTime.DateExtractor8
            date_extractor_10 = ArabicDateTime.DateExtractor9L
            date_extractor_11 = ArabicDateTime.DateExtractor9S

        self._date_regex_list = [
            RegExpUtility.get_safe_reg_exp(ArabicDateTime.DateExtractor1),
            RegExpUtility.get_safe_reg_exp(ArabicDateTime.DateExtractor3),
            RegExpUtility.get_safe_reg_exp(date_extractor_4),
            RegExpUtility.get_safe_reg_exp(date_extractor_5),
            RegExpUtility.get_safe_reg_exp(date_extractor_6),
            RegExpUtility.get_safe_reg_exp(date_extractor_7),
            RegExpUtility.get_safe_reg_exp(date_extractor_8),
            RegExpUtility.get_safe_reg_exp(date_extractor_9),
            RegExpUtility.get_safe_reg_exp(date_extractor_10),
            RegExpUtility.get_safe_reg_exp(date_extractor_11),
            RegExpUtility.get_safe_reg_exp(ArabicDateTime.DateExtractorA),
        ]

        self._day_of_week = ArabicDateTime.DayOfWeek
        self._month_of_year = ArabicDateTime.MonthOfYear

        self._check_both_before_after = ArabicDateTime.CheckBothBeforeAfter

        self._of_month = RegExpUtility.get_safe_reg_exp(ArabicDateTime.OfMonth)
        self._month_end = RegExpUtility.get_safe_reg_exp(ArabicDateTime.MonthEnd)
        self._week_day_end = RegExpUtility.get_safe_reg_exp(ArabicDateTime.WeekDayEnd)
        self._week_day_start = RegExpUtility.get_safe_reg_exp(ArabicDateTime.WeekDayStart)
        self._date_unit_regex = RegExpUtility.get_safe_reg_exp(ArabicDateTime.DateUnitRegex)
        self._for_the_regex = RegExpUtility.get_safe_reg_exp(ArabicDateTime.ForTheRegex)
        self._week_day_and_day_of_month_regex = RegExpUtility.get_safe_reg_exp(ArabicDateTime.WeekDayAndDayOfMonthRegex)
        self._week_day_and_day_regex = RegExpUtility.get_safe_reg_exp(ArabicDateTime.WeekDayAndDayRegex)
        self._relative_month_regex = RegExpUtility.get_safe_reg_exp(ArabicDateTime.RelativeMonthRegex)
        self._strict_relative_regex = RegExpUtility.get_safe_reg_exp(ArabicDateTime.StrictRelativeRegex)
        self._week_day_regex = RegExpUtility.get_safe_reg_exp(ArabicDateTime.WeekDayRegex)
        self._prefix_article_regex = RegExpUtility.get_safe_reg_exp(ArabicDateTime.PrefixArticleRegex)
        self._year_suffix = RegExpUtility.get_safe_reg_exp(ArabicDateTime.YearSuffix)
        self._less_than_regex = RegExpUtility.get_safe_reg_exp(ArabicDateTime.LessThanRegex)
        self._more_than_regex = RegExpUtility.get_safe_reg_exp(ArabicDateTime.MoreThanRegex)
        self._in_connector_regex = RegExpUtility.get_safe_reg_exp(ArabicDateTime.InConnectorRegex)
        self._since_year_suffix_regex = RegExpUtility.get_safe_reg_exp(ArabicDateTime.SinceYearSuffixRegex)
        self._range_unit_regex = RegExpUtility.get_safe_reg_exp(ArabicDateTime.RangeUnitRegex)
        self._range_connector_symbol_regex = RegExpUtility.get_safe_reg_exp(BaseDateTime.RangeConnectorSymbolRegex)
