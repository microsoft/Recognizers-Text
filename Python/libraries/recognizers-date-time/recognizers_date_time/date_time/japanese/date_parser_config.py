from typing import List, Pattern, Dict

from recognizers_date_time.date_time.extractors import DateTimeExtractor
from recognizers_date_time.date_time.parsers import DateTimeParser
from recognizers_date_time.date_time.CJK.base_configs import CJKCommonDateTimeParserConfiguration
from recognizers_number import BaseNumberExtractor, BaseNumberParser
from recognizers_text import RegExpUtility
from recognizers_date_time.date_time.japanese.date_extractor_config import JapaneseDateExtractorConfiguration
from recognizers_date_time.date_time.CJK.base_date import CJKDateParserConfiguration

from recognizers_date_time.resources.japanese_date_time import JapaneseDateTime


class JapaneseDateParserConfiguration(CJKDateParserConfiguration):

    @property
    def plus_one_day_regex(self) -> Pattern:
        return self._plus_one_day_regex

    @property
    def minus_one_day_regex(self) -> Pattern:
        return self._minus_one_day_regex

    @property
    def plus_two_day_regex(self) -> Pattern:
        return self._plus_two_day_regex

    @property
    def minus_two_day_regex(self) -> Pattern:
        return self._minus_two_day_regex

    @property
    def plus_three_day_regex(self) -> Pattern:
        return self._plus_three_day_regex

    @property
    def minus_three_day_regex(self) -> Pattern:
        return self._minus_three_day_regex

    @property
    def plus_four_day_regex(self) -> Pattern:
        return self._plus_four_day_regex

    @property
    def integer_extractor(self) -> BaseNumberExtractor:
        return self._integer_extractor

    @property
    def ordinal_extractor(self) -> BaseNumberExtractor:
        return self._ordinal_extractor

    @property
    def number_parser(self) -> BaseNumberParser:
        return self._number_parser

    @property
    def date_extractor(self) -> DateTimeExtractor:
        return self._date_extractor

    @property
    def duration_extractor(self) -> DateTimeExtractor:
        return self._duration_extractor

    @property
    def duration_parser(self) -> DateTimeParser:
        return self._duration_parser

    @property
    def date_regex_list(self) -> List[Pattern]:
        return self._date_regex_list

    @property
    def special_date(self) -> Pattern:
        return self._special_date

    @property
    def next_re(self) -> Pattern:
        return self._next_re

    @property
    def last_re(self) -> Pattern:
        return self._last_re

    @property
    def special_day_regex(self) -> Pattern:
        return self._special_day_regex

    @property
    def strict_week_day_regex(self) -> Pattern:
        return self._strict_week_day_regex

    @property
    def lunar_regex(self) -> Pattern:
        return self._lunar_regex

    @property
    def unit_regex(self) -> Pattern:
        return self._unit_regex

    @property
    def before_regex(self) -> Pattern:
        return self._before_regex

    @property
    def after_regex(self) -> Pattern:
        return self._after_regex

    @property
    def next_regex(self) -> Pattern:
        return self._next_regex

    @property
    def this_regex(self) -> Pattern:
        return self._this_regex

    @property
    def last_regex(self) -> Pattern:
        return self._last_regex

    @property
    def week_day_of_month_regex(self) -> any:
        return self._week_day_of_month_regex

    @property
    def week_day_and_day_regex(self) -> Pattern:
        return self._week_day_and_day_regex

    @property
    def duration_relative_duration_unit_regex(self) -> Pattern:
        return self._duration_relative_duration_unit_regex

    @property
    def special_day_with_num_regex(self) -> Pattern:
        return self._special_day_with_num_regex

    @property
    def dynasty_year_regex(self) -> Pattern:
        return self._dynasty_year_regex

    @property
    def dynasty_year_map(self) -> Dict[str, int]:
        return self._dynasty_year_map

    @property
    def cardinal_map(self) -> Dict[str, int]:
        return self._cardinal_map

    @property
    def unit_map(self) -> Dict[str, str]:
        return self._unit_map

    @property
    def day_of_month(self) -> Dict[str, int]:
        return self._day_of_month

    @property
    def day_of_week(self) -> Dict[str, int]:
        return self._day_of_week

    @property
    def month_of_year(self) -> Dict[str, int]:
        return self._month_of_year

    @property
    def dynasty_start_year(self) -> Pattern:
        return self._dynasty_start_year

    @property
    def last_week_day_regex(self) -> Pattern:
        return self._last_week_day_regex

    @property
    def next_month_regex(self) -> Pattern:
        return self._next_month_regex

    @property
    def last_month_regex(self) -> Pattern:
        return self._last_month_regex

    def get_swift_day(self, source: str) -> int:
        source = source.strip().lower()
        swift = 0

        if RegExpUtility.match_begin(self.plus_one_day_regex, source, trim=True):
            swift = 1
        elif RegExpUtility.match_begin(self.minus_one_day_regex, source, trim=True):
            swift = -1

        if RegExpUtility.is_exact_match(self.plus_one_day_regex, source, trim=False):
            swift = 1
        elif RegExpUtility.is_exact_match(self.plus_three_day_regex, source, trim=False):
            swift = 3
        elif RegExpUtility.is_exact_match(self.plus_four_day_regex, source, trim=False):
            swift = 4
        elif RegExpUtility.is_exact_match(self.minus_three_day_regex, source, trim=False):
            swift = -3
        elif RegExpUtility.is_exact_match(self.minus_one_day_regex, source, trim=False):
            swift = -1
        elif RegExpUtility.is_exact_match(self.plus_two_day_regex, source, trim=False):
            swift = 2
        elif RegExpUtility.is_exact_match(self.minus_two_day_regex, source, trim=False):
            swift = -2
        return swift

    def __init__(self, config: CJKCommonDateTimeParserConfiguration):
        super().__init__()
        self._integer_extractor = config.integer_extractor
        self._ordinal_extractor = config.ordinal_extractor

        self._number_parser = config.number_parser

        self._date_extractor = config.date_extractor
        self._duration_extractor = config.duration_extractor
        self._duration_parser = config.duration_parser

        self._date_regex_list = JapaneseDateExtractorConfiguration().date_regex_list
        self._special_date = RegExpUtility.get_safe_reg_exp(JapaneseDateTime.SpecialDate)
        self._next_re = JapaneseDateExtractorConfiguration().next_prefix_regex
        self._last_re = JapaneseDateExtractorConfiguration().last_prefix_regex
        self._special_day_regex = JapaneseDateExtractorConfiguration().special_day_regex
        self._strict_week_day_regex = JapaneseDateExtractorConfiguration().week_day_regex
        self._lunar_regex = JapaneseDateExtractorConfiguration().lunar_regex

        self._unit_regex = JapaneseDateExtractorConfiguration().date_unit_regex
        self._before_regex = JapaneseDateExtractorConfiguration().before_regex
        self._after_regex = JapaneseDateExtractorConfiguration().after_regex
        self._dynasty_year_regex = JapaneseDateExtractorConfiguration().dynasty_year_regex
        self._dynasty_start_year = JapaneseDateExtractorConfiguration().dynasty_start_year
        self._dynasty_year_map = JapaneseDateExtractorConfiguration().dynasty_year_map
        self._next_regex = JapaneseDateExtractorConfiguration().next_regex
        self._this_regex = JapaneseDateExtractorConfiguration().this_regex
        self._last_regex = JapaneseDateExtractorConfiguration().last_regex
        self._week_day_of_month_regex = JapaneseDateExtractorConfiguration().week_day_of_month_regex
        self._week_day_and_day_regex = JapaneseDateExtractorConfiguration().week_day_and_day_regex
        self._duration_relative_duration_unit_regex = \
            JapaneseDateExtractorConfiguration().duration_relative_duration_unit_regex
        self._special_day_with_num_regex = JapaneseDateExtractorConfiguration().special_day_with_num_regex

        self._cardinal_map = config.cardinal_map
        self._unit_map = config.unit_map
        self._day_of_month = config.day_of_month
        self._day_of_week = config.day_of_week
        self._month_of_year = config.month_of_year

        self._plus_one_day_regex = RegExpUtility.get_safe_reg_exp(JapaneseDateTime.PlusOneDayRegex)
        self._minus_one_day_regex = RegExpUtility.get_safe_reg_exp(JapaneseDateTime.MinusOneDayRegex)
        self._plus_two_day_regex = RegExpUtility.get_safe_reg_exp(JapaneseDateTime.PlusTwoDayRegex)
        self._minus_two_day_regex = RegExpUtility.get_safe_reg_exp(JapaneseDateTime.MinusTwoDayRegex)
        self._plus_three_day_regex = RegExpUtility.get_safe_reg_exp(JapaneseDateTime.PlusThreeDayRegex)
        self._minus_three_day_regex = RegExpUtility.get_safe_reg_exp(JapaneseDateTime.MinusThreeDayRegex)
        self._plus_four_day_regex = RegExpUtility.get_safe_reg_exp(JapaneseDateTime.PlusFourDayRegex)
        self._next_month_regex = RegExpUtility.get_safe_reg_exp(JapaneseDateTime.ParserConfigurationNextMonthRegex)
        self._last_month_regex = RegExpUtility.get_safe_reg_exp(JapaneseDateTime.ParserConfigurationLastMonthRegex)
        self._last_week_day_regex = RegExpUtility.get_safe_reg_exp(JapaneseDateTime.ParserConfigurationLastWeekDayRegex)
