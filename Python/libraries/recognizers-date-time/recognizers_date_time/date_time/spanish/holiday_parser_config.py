from typing import List, Dict, Callable
from datetime import datetime

from recognizers_text.utilities import RegExpUtility
from ..utilities import DateUtils
from ..base_holiday import BaseHolidayParserConfiguration
from ...resources.spanish_date_time import SpanishDateTime

class SpanishHolidayParserConfiguration(BaseHolidayParserConfiguration):
    @property
    def holiday_names(self) -> Dict[str, List[str]]:
        return self._holiday_names

    @property
    def holiday_regex_list(self) -> List[str]:
        return self._holiday_regexes

    @property
    def holiday_func_dictionary(self) -> Dict[str, Callable[[int], datetime]]:
        return self._holiday_func_dictionary

    def __init__(self, config):
        super().__init__()
        self._holiday_regexes = [
            RegExpUtility.get_safe_reg_exp(SpanishDateTime.HolidayRegex1),
            RegExpUtility.get_safe_reg_exp(SpanishDateTime.HolidayRegex2),
            RegExpUtility.get_safe_reg_exp(SpanishDateTime.HolidayRegex3)
        ]
        self._holiday_names = SpanishDateTime.HolidayNames
        self._variable_holidays_timex_dictionary = SpanishDateTime.VariableHolidaysTimexDictionary

        self.next_prefix_regex = RegExpUtility.get_safe_reg_exp(SpanishDateTime.NextPrefixRegex)
        self.past_prefix_regex = RegExpUtility.get_safe_reg_exp(SpanishDateTime.PastPrefixRegex)
        self.this_prefix_regex = RegExpUtility.get_safe_reg_exp(SpanishDateTime.ThisPrefixRegex)

    def _init_holiday_funcs(self) -> Dict[str, Callable[[int], datetime]]:
        local = dict([
            ('padres', SpanishHolidayParserConfiguration.fathers_day),
            ('madres', SpanishHolidayParserConfiguration.mothers_day),
            ('acciondegracias', SpanishHolidayParserConfiguration.thanksgiving_day),
            ('trabajador', SpanishHolidayParserConfiguration.labour_day),
            ('delaraza', SpanishHolidayParserConfiguration.columbus_day),
            ('memoria', SpanishHolidayParserConfiguration.memorial_day),
            ('pascuas', SpanishHolidayParserConfiguration.easter_day),
            ('navidad', SpanishHolidayParserConfiguration.christmas_day),
            ('nochebuena', SpanishHolidayParserConfiguration.christmas_eve),
            ('añonuevo', SpanishHolidayParserConfiguration.new_year),
            ('nochevieja', SpanishHolidayParserConfiguration.new_year_eve),
            ('yuandan', SpanishHolidayParserConfiguration.new_year),
            ('maestro', SpanishHolidayParserConfiguration.teacher_day),
            ('todoslossantos', SpanishHolidayParserConfiguration.halloween_day),
            ('niño', SpanishHolidayParserConfiguration.children_day),
            ('mujer', SpanishHolidayParserConfiguration.female_day)
        ])

        return {**super()._init_holiday_funcs(), **local}

    @staticmethod
    def new_year(year: int) -> datetime:
        return datetime(year, 1, 1)

    @staticmethod
    def new_year_eve(year: int) -> datetime:
        return datetime(year, 12, 31)

    @staticmethod
    def christmas_day(year: int) -> datetime:
        return datetime(year, 12, 25)

    @staticmethod
    def christmas_eve(year: int) -> datetime:
        return datetime(year, 12, 24)

    @staticmethod
    def female_day(year: int) -> datetime:
        return datetime(year, 3, 8)

    @staticmethod
    def children_day(year: int) -> datetime:
        return datetime(year, 6, 1)

    @staticmethod
    def halloween_day(year: int) -> datetime:
        return datetime(year, 10, 31)

    @staticmethod
    def teacher_day(year: int) -> datetime:
        return datetime(year, 9, 11)

    @staticmethod
    def easter_day(year: int) -> datetime:
        return DateUtils.min_value

    def get_swift_year(self, text: str) -> int:
        trimmed_text = text.strip().lower()
        swift = -10

        if self.next_prefix_regex.search(trimmed_text):
            swift = 1

        if self.past_prefix_regex.search(trimmed_text):
            swift = -1

        if self.this_prefix_regex.search(trimmed_text):
            swift = 0

        return swift

    def sanitize_holiday_token(self, holiday: str) -> str:
        return holiday.replace(' ', '').replace('á', 'a').replace('é', 'e').replace('í', 'i').replace('ó', 'o').replace('ú', 'u')
