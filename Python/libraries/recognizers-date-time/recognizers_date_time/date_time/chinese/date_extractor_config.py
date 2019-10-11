from typing import List, Pattern, Dict

from recognizers_text import RegExpUtility
from recognizers_number import BaseNumberExtractor, BaseNumberParser
from ...resources import ChineseDateTime
from ..constants import Constants
from ..extractors import DateTimeExtractor
from ..base_date import DateTimeUtilityConfiguration
from ..base_date import DateExtractorConfiguration
from ...resources.base_date_time import BaseDateTime


class ChineseDateExtractorConfiguration(DateExtractorConfiguration):
    @property
    def week_day_end(self) -> Pattern:
        pass

    @property
    def week_day_and_day_regex(self) -> Pattern:
        pass

    @property
    def prefix_article_regex(self) -> Pattern:
        pass

    @property
    def month_of_year(self) -> Dict[str, int]:
        pass

    @property
    def year_suffix(self) -> Pattern:
        pass

    @property
    def more_than_regex(self) -> Pattern:
        pass

    @property
    def less_than_regex(self) -> Pattern:
        pass

    @property
    def in_connector_regex(self) -> Pattern:
        pass

    @property
    def range_unit_regex(self) -> Pattern:
        pass

    @property
    def since_year_suffix_regex(self) -> Pattern:
        pass

    @property
    def date_regex_list(self) -> List[Pattern]:
        return self._date_regex_list

    @property
    def implicit_date_list(self) -> List[Pattern]:
        return self._implicit_date_list

    @property
    def month_end(self) -> Pattern:
        return None

    @property
    def of_month(self) -> Pattern:
        return None

    @property
    def date_unit_regex(self) -> Pattern:
        return None

    @property
    def for_the_regex(self) -> Pattern:
        return None

    @property
    def week_day_and_day_of_month_regex(self) -> Pattern:
        return None

    @property
    def relative_month_regex(self) -> Pattern:
        return None

    @property
    def week_day_regex(self) -> Pattern:
        return None

    @property
    def day_of_week(self) -> Dict[str, int]:
        return None

    @property
    def ordinal_extractor(self) -> BaseNumberExtractor:
        return None

    @property
    def integer_extractor(self) -> BaseNumberExtractor:
        return None

    @property
    def number_parser(self) -> BaseNumberParser:
        return None

    @property
    def duration_extractor(self) -> DateTimeExtractor:
        return None

    @property
    def strict_relative_regex(self) -> Pattern:
        return None

    @property
    def range_connector_symbol_regex(self) -> Pattern:
        return self._range_connector_symbol_regex

    @property
    def utility_configuration(self) -> DateTimeUtilityConfiguration:
        return None

    def __init__(self):
        self._date_regex_list = [
            RegExpUtility.get_safe_reg_exp(ChineseDateTime.DateRegexList1),
            RegExpUtility.get_safe_reg_exp(ChineseDateTime.DateRegexList2),
            RegExpUtility.get_safe_reg_exp(ChineseDateTime.DateRegexList3),
            RegExpUtility.get_safe_reg_exp(ChineseDateTime.DateRegexList4),
            RegExpUtility.get_safe_reg_exp(ChineseDateTime.DateRegexList5)
        ]

        if ChineseDateTime.DefaultLanguageFallback == Constants.DEFAULT_LANGUAGE_FALLBACK_DMY:
            self._date_regex_list.append(
                RegExpUtility.get_safe_reg_exp(ChineseDateTime.DateRegexList7))
            self._date_regex_list.append(
                RegExpUtility.get_safe_reg_exp(ChineseDateTime.DateRegexList6))
        else:
            self._date_regex_list.append(
                RegExpUtility.get_safe_reg_exp(ChineseDateTime.DateRegexList6))
            self._date_regex_list.append(
                RegExpUtility.get_safe_reg_exp(ChineseDateTime.DateRegexList7))

        self._date_regex_list.append(
            RegExpUtility.get_safe_reg_exp(ChineseDateTime.DateRegexList8))

        self._implicit_date_list = [
            RegExpUtility.get_safe_reg_exp(ChineseDateTime.LunarRegex),
            RegExpUtility.get_safe_reg_exp(ChineseDateTime.SpecialDayRegex),
            RegExpUtility.get_safe_reg_exp(ChineseDateTime.DateThisRegex),
            RegExpUtility.get_safe_reg_exp(ChineseDateTime.DateLastRegex),
            RegExpUtility.get_safe_reg_exp(ChineseDateTime.DateNextRegex),
            RegExpUtility.get_safe_reg_exp(ChineseDateTime.WeekDayRegex),
            RegExpUtility.get_safe_reg_exp(
                ChineseDateTime.WeekDayOfMonthRegex),
            RegExpUtility.get_safe_reg_exp(ChineseDateTime.SpecialDate)
        ]
        self._range_connector_symbol_regex = RegExpUtility.get_safe_reg_exp(
            BaseDateTime.RangeConnectorSymbolRegex
        )
