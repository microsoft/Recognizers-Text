from typing import List, Dict, Callable
import re
from datetime import datetime

from recognizers_text.utilities import RegExpUtility
from recognizers_date_time.date_time.utilities import DateUtils
from recognizers_date_time.date_time.base_holiday import BaseHolidayParserConfiguration
from recognizers_date_time.resources.arabic_date_time import ArabicDateTime


class ArabicHolidayParserConfiguration(BaseHolidayParserConfiguration):
    @property
    def holiday_names(self) -> Dict[str, List[str]]:
        return self._holiday_names

    @property
    def holiday_regex_list(self) -> List[str]:
        return self._holiday_regexes

    @property
    def holiday_func_dictionary(self) -> Dict[str, Callable[[int], datetime]]:
        return self._holiday_func_dictionary

    def get_swift_year(self, text: str) -> int:
        trimmed_text = text.strip().lower()
        swift = -10

        if self.next_prefix_regex.search(trimmed_text):
            swift = 1

        if self.previous_prefix_regex.search(trimmed_text):
            swift = -1

        if self.this_prefix_regex.search(trimmed_text):
            swift = 0

        return swift

    def sanitize_holiday_token(self, holiday: str) -> str:
        return holiday.replace(' ', '').replace('\'', '')

    def __init__(self, config):
        super().__init__()
        self._holiday_regexes = [
            RegExpUtility.get_safe_reg_exp(ArabicDateTime.HolidayRegex)
        ]
        self._holiday_names = ArabicDateTime.HolidayNames

        self.next_prefix_regex = RegExpUtility.get_safe_reg_exp(
            ArabicDateTime.NextPrefixRegex)
        self.previous_prefix_regex = RegExpUtility.get_safe_reg_exp(
            ArabicDateTime.PreviousPrefixRegex)
        self.this_prefix_regex = RegExpUtility.get_safe_reg_exp(
            ArabicDateTime.ThisPrefixRegex)

    def _init_holiday_funcs(self) -> Dict[str, Callable[[int], datetime]]:
        local = dict([
            ('maosbirthday', ArabicHolidayParserConfiguration.mao_birthday),
            ('yuandan', ArabicHolidayParserConfiguration.new_year),
            ('teachersday', ArabicHolidayParserConfiguration.teacher_day),
            ('singleday', ArabicHolidayParserConfiguration.singles_day),
            ('allsaintsday', ArabicHolidayParserConfiguration.halloween_day),
            ('youthday', ArabicHolidayParserConfiguration.youth_day),
            ('childrenday', ArabicHolidayParserConfiguration.children_day),
            ('femaleday', ArabicHolidayParserConfiguration.female_day),
            ('treeplantingday', ArabicHolidayParserConfiguration.tree_plant_day),
            ('arborday', ArabicHolidayParserConfiguration.tree_plant_day),
            ('girlsday', ArabicHolidayParserConfiguration.girls_day),
            ('whiteloverday', ArabicHolidayParserConfiguration.white_lover_day),
            ('loverday', ArabicHolidayParserConfiguration.valentines_day),
            ('christmas', ArabicHolidayParserConfiguration.christmas_day),
            ('xmas', ArabicHolidayParserConfiguration.christmas_day),
            ('newyear', ArabicHolidayParserConfiguration.new_year),
            ('newyearday', ArabicHolidayParserConfiguration.new_year),
            ('newyearsday', ArabicHolidayParserConfiguration.new_year),
            ('inaugurationday', ArabicHolidayParserConfiguration.inauguration_day),
            ('groundhougday', ArabicHolidayParserConfiguration.groundhog_day),
            ('valentinesday', ArabicHolidayParserConfiguration.valentines_day),
            ('stpatrickday', ArabicHolidayParserConfiguration.st_patrick_day),
            ('aprilfools', ArabicHolidayParserConfiguration.fool_day),
            ('stgeorgeday', ArabicHolidayParserConfiguration.st_george_day),
            ('mayday', ArabicHolidayParserConfiguration.may_day),
            ('cincodemayoday', ArabicHolidayParserConfiguration.cinco_de_mayo_day),
            ('baptisteday', ArabicHolidayParserConfiguration.baptiste_day),
            ('usindependenceday', ArabicHolidayParserConfiguration.usa_independence_day),
            ('independenceday', ArabicHolidayParserConfiguration.usa_independence_day),
            ('bastilleday', ArabicHolidayParserConfiguration.bastille_day),
            ('halloweenday', ArabicHolidayParserConfiguration.halloween_day),
            ('allhallowday', ArabicHolidayParserConfiguration.all_hallow_day),
            ('allsoulsday', ArabicHolidayParserConfiguration.all_souls_day),
            ('guyfawkesday', ArabicHolidayParserConfiguration.guy_fawkes_day),
            ('veteransday', ArabicHolidayParserConfiguration.veterans_day),
            ('christmaseve', ArabicHolidayParserConfiguration.christmas_eve),
            ('newyeareve', ArabicHolidayParserConfiguration.new_year_eve),
            ('easterday', ArabicHolidayParserConfiguration.easter_day),
            ('juneteenth', ArabicHolidayParserConfiguration.juneteenth),
        ])

        return {**super()._init_holiday_funcs(), **local}

    @staticmethod
    def mao_birthday(year: int) -> datetime:
        return datetime(year, 12, 26)

    @staticmethod
    def new_year(year: int) -> datetime:
        return datetime(year, 1, 1)

    @staticmethod
    def teacher_day(year: int) -> datetime:
        return datetime(year, 9, 10)

    @staticmethod
    def singles_day(year: int) -> datetime:
        return datetime(year, 11, 11)

    @staticmethod
    def halloween_day(year: int) -> datetime:
        return datetime(year, 10, 31)

    @staticmethod
    def youth_day(year: int) -> datetime:
        return datetime(year, 5, 4)

    @staticmethod
    def children_day(year: int) -> datetime:
        return datetime(year, 6, 1)

    @staticmethod
    def female_day(year: int) -> datetime:
        return datetime(year, 3, 8)

    @staticmethod
    def tree_plant_day(year: int) -> datetime:
        return datetime(year, 3, 12)

    @staticmethod
    def girls_day(year: int) -> datetime:
        return datetime(year, 3, 7)

    @staticmethod
    def white_lover_day(year: int) -> datetime:
        return datetime(year, 3, 14)

    @staticmethod
    def valentines_day(year: int) -> datetime:
        return datetime(year, 2, 14)

    @staticmethod
    def christmas_day(year: int) -> datetime:
        return datetime(year, 12, 25)

    @staticmethod
    def inauguration_day(year: int) -> datetime:
        return datetime(year, 1, 20)

    @staticmethod
    def groundhog_day(year: int) -> datetime:
        return datetime(year, 2, 2)

    @staticmethod
    def st_patrick_day(year: int) -> datetime:
        return datetime(year, 3, 17)

    @staticmethod
    def fool_day(year: int) -> datetime:
        return datetime(year, 4, 1)

    @staticmethod
    def st_george_day(year: int) -> datetime:
        return datetime(year, 4, 23)

    @staticmethod
    def may_day(year: int) -> datetime:
        return datetime(year, 5, 1)

    @staticmethod
    def cinco_de_mayo_day(year: int) -> datetime:
        return datetime(year, 5, 5)

    @staticmethod
    def baptiste_day(year: int) -> datetime:
        return datetime(year, 6, 24)

    @staticmethod
    def usa_independence_day(year: int) -> datetime:
        return datetime(year, 7, 4)

    @staticmethod
    def bastille_day(year: int) -> datetime:
        return datetime(year, 7, 14)

    @staticmethod
    def all_hallow_day(year: int) -> datetime:
        return datetime(year, 11, 1)

    @staticmethod
    def all_souls_day(year: int) -> datetime:
        return datetime(year, 11, 2)

    @staticmethod
    def guy_fawkes_day(year: int) -> datetime:
        return datetime(year, 11, 5)

    @staticmethod
    def veterans_day(year: int) -> datetime:
        return datetime(year, 11, 11)

    @staticmethod
    def christmas_eve(year: int) -> datetime:
        return datetime(year, 12, 24)

    @staticmethod
    def new_year_eve(year: int) -> datetime:
        return datetime(year, 12, 31)

    @staticmethod
    def easter_day(year: int) -> datetime:
        return DateUtils.min_value

    @staticmethod
    def juneteenth(year: int) -> datetime:
        return datetime(year, 6, 19)
