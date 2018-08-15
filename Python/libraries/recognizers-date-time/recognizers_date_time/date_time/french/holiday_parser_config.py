from typing import List, Dict, Callable
from datetime import datetime

from recognizers_text.utilities import RegExpUtility
from ..utilities import DateUtils
from ..base_holiday import BaseHolidayParserConfiguration
from ...resources.french_date_time import FrenchDateTime

class FrenchHolidayParserConfiguration(BaseHolidayParserConfiguration):
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
            RegExpUtility.get_safe_reg_exp(FrenchDateTime.HolidayRegex1),
            RegExpUtility.get_safe_reg_exp(FrenchDateTime.HolidayRegex2),
            RegExpUtility.get_safe_reg_exp(FrenchDateTime.HolidayRegex3),
            RegExpUtility.get_safe_reg_exp(FrenchDateTime.HolidayRegex4)
        ]
        self._holiday_names = FrenchDateTime.HolidayNames
        #self._variable_holidays_timex_dictionary = FrenchDateTime.VariableHolidaysTimexDictionary

    def _init_holiday_funcs(self) -> Dict[str, Callable[[int], datetime]]:
        local = dict([
            ('maosbirthday', FrenchHolidayParserConfiguration.mao_birthday),
            ('yuandan', FrenchHolidayParserConfiguration.new_year),
            ('teachersday', FrenchHolidayParserConfiguration.teacher_day),
            ('singleday', FrenchHolidayParserConfiguration.singles_day),
            ('allsaintsday', FrenchHolidayParserConfiguration.halloween_day),
            ('youthday', FrenchHolidayParserConfiguration.youth_day),
            ('childrenday', FrenchHolidayParserConfiguration.children_day),
            ('femaleday', FrenchHolidayParserConfiguration.female_day),
            ('treeplantingday', FrenchHolidayParserConfiguration.tree_plant_day),
            ('arborday', FrenchHolidayParserConfiguration.tree_plant_day),
            ('girlsday', FrenchHolidayParserConfiguration.girls_day),
            ('whiteloverday', FrenchHolidayParserConfiguration.white_lover_day),
            ('loverday', FrenchHolidayParserConfiguration.valentines_day),
            ('christmas', FrenchHolidayParserConfiguration.christmas_day),
            ('xmas', FrenchHolidayParserConfiguration.christmas_day),
            ('newyear', FrenchHolidayParserConfiguration.new_year),
            ('newyearday', FrenchHolidayParserConfiguration.new_year),
            ('newyearsday', FrenchHolidayParserConfiguration.new_year),
            ('inaugurationday', FrenchHolidayParserConfiguration.inauguration_day),
            ('groundhougday', FrenchHolidayParserConfiguration.groundhog_day),
            ('valentinesday', FrenchHolidayParserConfiguration.valentines_day),
            ('stpatrickday', FrenchHolidayParserConfiguration.st_patrick_day),
            ('aprilfools', FrenchHolidayParserConfiguration.fool_day),
            ('stgeorgeday', FrenchHolidayParserConfiguration.st_george_day),
            ('mayday', FrenchHolidayParserConfiguration.mayday),
            ('cincodemayoday', FrenchHolidayParserConfiguration.cinco_de_mayo_day),
            ('baptisteday', FrenchHolidayParserConfiguration.baptiste_day),
            ('usindependenceday', FrenchHolidayParserConfiguration.usa_independence_day),
            ('independenceday', FrenchHolidayParserConfiguration.usa_independence_day),
            ('bastilleday', FrenchHolidayParserConfiguration.bastille_day),
            ('halloweenday', FrenchHolidayParserConfiguration.halloween_day),
            ('allhallowday', FrenchHolidayParserConfiguration.all_hallow_day),
            ('allsoulsday', FrenchHolidayParserConfiguration.all_souls_day),
            ('guyfawkesday', FrenchHolidayParserConfiguration.guyfawkes_day),
            ('veteransday', FrenchHolidayParserConfiguration.veterans_day),
            ('christmaseve', FrenchHolidayParserConfiguration.christmas_eve),
            ('newyeareve', FrenchHolidayParserConfiguration.new_year_eve),
            ('fathersday', FrenchHolidayParserConfiguration.fathers_day),
            ('mothersday', FrenchHolidayParserConfiguration.mothers_day),
            ('labourday', FrenchHolidayParserConfiguration.labour_day)
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
    def easter_day(year: int) -> datetime:
        return DateUtils.min_value

    @staticmethod
    def valentines_day(year: int) -> datetime:
        return datetime(year, 2, 14)

    @staticmethod
    def white_lover_day(year: int) -> datetime:
        return datetime(year, 3, 14)

    @staticmethod
    def fool_day(year: int) -> datetime:
        return datetime(year, 4, 1)

    @staticmethod
    def girls_day(year: int) -> datetime:
        return datetime(year, 3, 7)

    @staticmethod
    def tree_plant_day(year: int) -> datetime:
        return datetime(year, 3, 12)

    @staticmethod
    def youth_day(year: int) -> datetime:
        return datetime(year, 5, 4)

    @staticmethod
    def teacher_day(year: int) -> datetime:
        return datetime(year, 9, 10)

    @staticmethod
    def singles_day(year: int) -> datetime:
        return datetime(year, 11, 11)

    @staticmethod
    def mao_birthday(year: int) -> datetime:
        return datetime(year, 12, 26)

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
    def st_george_day(year: int) -> datetime:
        return datetime(year, 4, 23)

    @staticmethod
    def mayday(year: int) -> datetime:
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
    def guyfawkes_day(year: int) -> datetime:
        return datetime(year, 11, 5)

    @staticmethod
    def veterans_day(year: int) -> datetime:
        return datetime(year, 11, 11)

    @staticmethod
    def fathers_day(year: int) -> datetime:
        return datetime(year, 6, 17)

    @staticmethod
    def mothers_day(year: int) -> datetime:
        return datetime(year, 5, 27)

    @staticmethod
    def labour_day(year: int) -> datetime:
        return datetime(year, 5, 1)

    def get_swift_year(self, text: str) -> int:
        trimmed_text = text.strip().lower()
        swift = -10

        if trimmed_text.endswith('prochain'): # next - 'l'annee prochain'
            swift = 1

        if trimmed_text.endswith('dernier'): # last - 'l'annee dernier'
            swift = -1

        if trimmed_text.startswith('cette'): # this - 'cette annees'
            swift = 0

        return swift

    def sanitize_holiday_token(self, holiday: str) -> str:
        return holiday.replace(' ', '').replace('\'', '')
