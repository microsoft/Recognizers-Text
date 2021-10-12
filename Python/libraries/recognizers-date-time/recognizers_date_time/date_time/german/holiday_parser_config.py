#  Copyright (c) Microsoft Corporation. All rights reserved.
#  Licensed under the MIT License.

from typing import List, Dict, Callable
import re
from datetime import datetime

from recognizers_text.utilities import RegExpUtility
from ..utilities import DateUtils
from ..base_holiday import BaseHolidayParserConfiguration
from ...resources.german_date_time import germanDateTime


class germanHolidayParserConfiguration(BaseHolidayParserConfiguration):
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
            RegExpUtility.get_safe_reg_exp(germanDateTime.HolidayRegex)
        ]
        self._holiday_names = germanDateTime.HolidayNames

    def _init_holiday_funcs(self) -> Dict[str, Callable[[int], datetime]]:
        local = dict([
            ('maosbirthday', germanHolidayParserConfiguration.mao_birthday),
            ('yuandan', germanHolidayParserConfiguration.new_year),
            ('teachersday', germanHolidayParserConfiguration.teacher_day),
            ('singleday', germanHolidayParserConfiguration.singles_day),
            ('allsaintsday', germanHolidayParserConfiguration.halloween_day),
            ('youthday', germanHolidayParserConfiguration.youth_day),
            ('childrenday', germanHolidayParserConfiguration.children_day),
            ('femaleday', germanHolidayParserConfiguration.female_day),
            ('treeplantingday', germanHolidayParserConfiguration.tree_plant_day),
            ('arborday', germanHolidayParserConfiguration.tree_plant_day),
            ('girlsday', germanHolidayParserConfiguration.girls_day),
            ('whiteloverday', germanHolidayParserConfiguration.white_lover_day),
            ('loverday', germanHolidayParserConfiguration.valentines_day),
            ('christmas', germanHolidayParserConfiguration.christmas_day),
            ('xmas', germanHolidayParserConfiguration.christmas_day),
            ('newyear', germanHolidayParserConfiguration.new_year),
            ('newyearday', germanHolidayParserConfiguration.new_year),
            ('newyearsday', germanHolidayParserConfiguration.new_year),
            ('inaugurationday', germanHolidayParserConfiguration.inauguration_day),
            ('groundhougday', germanHolidayParserConfiguration.groundhog_day),
            ('valentinesday', germanHolidayParserConfiguration.valentines_day),
            ('stpatrickday', germanHolidayParserConfiguration.st_patrick_day),
            ('aprilfools', germanHolidayParserConfiguration.fool_day),
            ('stgeorgeday', germanHolidayParserConfiguration.st_george_day),
            ('mayday', germanHolidayParserConfiguration.may_day),
            ('cincodemayoday', germanHolidayParserConfiguration.cinco_de_mayo_day),
            ('baptisteday', germanHolidayParserConfiguration.baptiste_day),
            ('usindependenceday', germanHolidayParserConfiguration.usa_independence_day),
            ('independenceday', germanHolidayParserConfiguration.usa_independence_day),
            ('bastilleday', germanHolidayParserConfiguration.bastille_day),
            ('halloweenday', germanHolidayParserConfiguration.halloween_day),
            ('allhallowday', germanHolidayParserConfiguration.all_hallow_day),
            ('allsoulsday', germanHolidayParserConfiguration.all_souls_day),
            ('guyfawkesday', germanHolidayParserConfiguration.guy_fawkes_day),
            ('veteransday', germanHolidayParserConfiguration.veterans_day),
            ('christmaseve', germanHolidayParserConfiguration.christmas_eve),
            ('newyeareve', germanHolidayParserConfiguration.new_year_eve),
            ('easterday', germanHolidayParserConfiguration.easter_day),
            ('juneteenth', germanHolidayParserConfiguration.juneteenth),
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
