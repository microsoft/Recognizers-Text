#  Copyright (c) Microsoft Corporation. All rights reserved.
#  Licensed under the MIT License.

from typing import List, Dict, Callable
import re
from datetime import datetime

from recognizers_text.utilities import RegExpUtility
from ..utilities import DateUtils
from ..base_holiday import BaseHolidayParserConfiguration
from ...resources.german_date_time import GermanDateTime


class GermanHolidayParserConfiguration(BaseHolidayParserConfiguration):
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
            RegExpUtility.get_safe_reg_exp(GermanDateTime.HolidayRegex1),
            RegExpUtility.get_safe_reg_exp(GermanDateTime.HolidayRegex2),
            RegExpUtility.get_safe_reg_exp(GermanDateTime.HolidayRegex3)
        ]
        self._holiday_names = GermanDateTime.HolidayNames

    def _init_holiday_funcs(self) -> Dict[str, Callable[[int], datetime]]:
        local = dict([
            ('maosbirthday', GermanHolidayParserConfiguration.mao_birthday),
            ('yuandan', GermanHolidayParserConfiguration.new_year),
            ('teachersday', GermanHolidayParserConfiguration.teacher_day),
            ('singleday', GermanHolidayParserConfiguration.singles_day),
            ('allsaintsday', GermanHolidayParserConfiguration.halloween_day),
            ('youthday', GermanHolidayParserConfiguration.youth_day),
            ('childrenday', GermanHolidayParserConfiguration.children_day),
            ('femaleday', GermanHolidayParserConfiguration.female_day),
            ('treeplantingday', GermanHolidayParserConfiguration.tree_plant_day),
            ('arborday', GermanHolidayParserConfiguration.tree_plant_day),
            ('girlsday', GermanHolidayParserConfiguration.girls_day),
            ('whiteloverday', GermanHolidayParserConfiguration.white_lover_day),
            ('loverday', GermanHolidayParserConfiguration.valentines_day),
            ('christmas', GermanHolidayParserConfiguration.christmas_day),
            ('xmas', GermanHolidayParserConfiguration.christmas_day),
            ('newyear', GermanHolidayParserConfiguration.new_year),
            ('newyearday', GermanHolidayParserConfiguration.new_year),
            ('newyearsday', GermanHolidayParserConfiguration.new_year),
            ('inaugurationday', GermanHolidayParserConfiguration.inauguration_day),
            ('groundhougday', GermanHolidayParserConfiguration.groundhog_day),
            ('valentinesday', GermanHolidayParserConfiguration.valentines_day),
            ('stpatrickday', GermanHolidayParserConfiguration.st_patrick_day),
            ('aprilfools', GermanHolidayParserConfiguration.fool_day),
            ('stgeorgeday', GermanHolidayParserConfiguration.st_george_day),
            ('mayday', GermanHolidayParserConfiguration.may_day),
            ('cincodemayoday', GermanHolidayParserConfiguration.cinco_de_mayo_day),
            ('baptisteday', GermanHolidayParserConfiguration.baptiste_day),
            ('usindependenceday', GermanHolidayParserConfiguration.usa_independence_day),
            ('independenceday', GermanHolidayParserConfiguration.usa_independence_day),
            ('bastilleday', GermanHolidayParserConfiguration.bastille_day),
            ('halloweenday', GermanHolidayParserConfiguration.halloween_day),
            ('allhallowday', GermanHolidayParserConfiguration.all_hallow_day),
            ('allsoulsday', GermanHolidayParserConfiguration.all_souls_day),
            ('guyfawkesday', GermanHolidayParserConfiguration.guy_fawkes_day),
            ('veteransday', GermanHolidayParserConfiguration.veterans_day),
            ('christmaseve', GermanHolidayParserConfiguration.christmas_eve),
            ('newyeareve', GermanHolidayParserConfiguration.new_year_eve),
            ('easterday', GermanHolidayParserConfiguration.easter_day),
            ('juneteenth', GermanHolidayParserConfiguration.juneteenth),
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
