from typing import List, Dict, Callable
import re
from datetime import datetime

from recognizers_text.utilities import RegExpUtility
from ..utilities import DateUtils
from ..base_holiday import BaseHolidayParserConfiguration
from ...resources.english_date_time import EnglishDateTime

class EnglishHolidayParserConfiguration(BaseHolidayParserConfiguration):
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
        if trimmed_text.startswith('next'):
            swift = 1
        if trimmed_text.startswith('last'):
            swift = -1
        if trimmed_text.startswith('this'):
            swift = 0
        return swift

    def sanitize_holiday_token(self, holiday: str) -> str:
        return re.sub('[ \']', '', holiday)

    def __init__(self, config):
        super().__init__()
        self._holiday_regexes = [
            RegExpUtility.get_safe_reg_exp(EnglishDateTime.HolidayRegex1),
            RegExpUtility.get_safe_reg_exp(EnglishDateTime.HolidayRegex2),
            RegExpUtility.get_safe_reg_exp(EnglishDateTime.HolidayRegex3)
        ]
        self._holiday_names = EnglishDateTime.HolidayNames

    def _init_holiday_funcs(self) -> Dict[str, Callable[[int], datetime]]:
        local = dict([
            ('maosbirthday', EnglishHolidayParserConfiguration.mao_birthday),
            ('yuandan', EnglishHolidayParserConfiguration.new_year),
            ('teachersday', EnglishHolidayParserConfiguration.teacher_day),
            ('singleday', EnglishHolidayParserConfiguration.singles_day),
            ('allsaintsday', EnglishHolidayParserConfiguration.halloween_day),
            ('youthday', EnglishHolidayParserConfiguration.youth_day),
            ('childrenday', EnglishHolidayParserConfiguration.children_day),
            ('femaleday', EnglishHolidayParserConfiguration.female_day),
            ('treeplantingday', EnglishHolidayParserConfiguration.tree_plant_day),
            ('arborday', EnglishHolidayParserConfiguration.tree_plant_day),
            ('girlsday', EnglishHolidayParserConfiguration.girls_day),
            ('whiteloverday', EnglishHolidayParserConfiguration.white_lover_day),
            ('loverday', EnglishHolidayParserConfiguration.valentines_day),
            ('christmas', EnglishHolidayParserConfiguration.christmas_day),
            ('xmas', EnglishHolidayParserConfiguration.christmas_day),
            ('newyear', EnglishHolidayParserConfiguration.new_year),
            ('newyearday', EnglishHolidayParserConfiguration.new_year),
            ('newyearsday', EnglishHolidayParserConfiguration.new_year),
            ('inaugurationday', EnglishHolidayParserConfiguration.inauguration_day),
            ('groundhougday', EnglishHolidayParserConfiguration.groundhog_day),
            ('valentinesday', EnglishHolidayParserConfiguration.valentines_day),
            ('stpatrickday', EnglishHolidayParserConfiguration.st_patrick_day),
            ('aprilfools', EnglishHolidayParserConfiguration.fool_day),
            ('stgeorgeday', EnglishHolidayParserConfiguration.st_george_day),
            ('mayday', EnglishHolidayParserConfiguration.may_day),
            ('cincodemayoday', EnglishHolidayParserConfiguration.cinco_de_mayo_day),
            ('baptisteday', EnglishHolidayParserConfiguration.baptiste_day),
            ('usindependenceday', EnglishHolidayParserConfiguration.usa_independence_day),
            ('independenceday', EnglishHolidayParserConfiguration.usa_independence_day),
            ('bastilleday', EnglishHolidayParserConfiguration.bastille_day),
            ('halloweenday', EnglishHolidayParserConfiguration.halloween_day),
            ('allhallowday', EnglishHolidayParserConfiguration.all_hallow_day),
            ('allsoulsday', EnglishHolidayParserConfiguration.all_souls_day),
            ('guyfawkesday', EnglishHolidayParserConfiguration.guy_fawkes_day),
            ('veteransday', EnglishHolidayParserConfiguration.veterans_day),
            ('christmaseve', EnglishHolidayParserConfiguration.christmas_eve),
            ('newyeareve', EnglishHolidayParserConfiguration.new_year_eve),
            ('easterday', EnglishHolidayParserConfiguration.easter_day)
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
