#  Copyright (c) Microsoft Corporation. All rights reserved.
#  Licensed under the MIT License.

from typing import List, Dict, Callable, Pattern
from datetime import datetime

from recognizers_text.utilities import RegExpUtility
from ..utilities import DateUtils
from ..base_holiday import BaseHolidayParserConfiguration
from ...resources.dutch_date_time import DutchDateTime


class DutchHolidayParserConfiguration(BaseHolidayParserConfiguration):
    @property
    def holiday_names(self) -> Dict[str, List[str]]:
        return self._holiday_names

    @property
    def holiday_regex_list(self) -> List[Pattern]:
        return self._holiday_regexes

    @property
    def holiday_func_dictionary(self) -> Dict[str, Callable[[int], datetime]]:
        return self._holiday_func_dictionary

    def __init__(self, config):
        super().__init__()
        self._holiday_regexes = [
            RegExpUtility.get_safe_reg_exp(DutchDateTime.HolidayRegex),
        ]
        self._holiday_names = DutchDateTime.HolidayNames

        self.next_prefix_regex = RegExpUtility.get_safe_reg_exp(
            DutchDateTime.NextPrefixRegex)
        self.previous_prefix_regex = RegExpUtility.get_safe_reg_exp(
            DutchDateTime.PreviousPrefixRegex)
        self.this_prefix_regex = RegExpUtility.get_safe_reg_exp(
            DutchDateTime.ThisPrefixRegex)

    def _init_holiday_funcs(self) -> Dict[str, Callable[[int], datetime]]:
        local = dict([
            ("maosbirthday", DutchHolidayParserConfiguration.mao_birthday),
            ("teachersday", DutchHolidayParserConfiguration.teacher_day),
            ("singleday", DutchHolidayParserConfiguration.singles_day),
            ("allsaintsday", DutchHolidayParserConfiguration.halloween_day),
            ("youthday", DutchHolidayParserConfiguration.youth_day),
            ("childrenday", DutchHolidayParserConfiguration.children_day),
            ("femaleday", DutchHolidayParserConfiguration.female_day),
            ("treeplantingday", DutchHolidayParserConfiguration.tree_plant_day),
            ("arborday", DutchHolidayParserConfiguration.tree_plant_day),
            ("girlsday", DutchHolidayParserConfiguration.girls_day),
            ("whiteloverday", DutchHolidayParserConfiguration.white_lover_day),
            ("loverday", DutchHolidayParserConfiguration.valentines_day),
            ("christmas", DutchHolidayParserConfiguration.christmas_day),
            ("xmas", DutchHolidayParserConfiguration.children_day),
            ("newyear", DutchHolidayParserConfiguration.new_year),
            ("newyearday", DutchHolidayParserConfiguration.new_year),
            ("newyearsday", DutchHolidayParserConfiguration.new_year),
            ("groundhougday", DutchHolidayParserConfiguration.groundhog_day),
            ("valentinesday", DutchHolidayParserConfiguration.valentines_day),
            ("stpatrickday", DutchHolidayParserConfiguration.st_patrick_day),
            ("aprilfools", DutchHolidayParserConfiguration.fool_day),
            ("stgeorgeday", DutchHolidayParserConfiguration.st_george_day),
            ("mayday", DutchHolidayParserConfiguration.mayday),
            ("cincodemayoday", DutchHolidayParserConfiguration.cinco_de_mayo_day),
            ("baptisteday", DutchHolidayParserConfiguration.baptiste_day),
            ("usindependenceday", DutchHolidayParserConfiguration.usa_independence_day),
            ("independenceday", DutchHolidayParserConfiguration.usa_independence_day),
            ("bastilleday", DutchHolidayParserConfiguration.bastille_day),
            ("halloweenday", DutchHolidayParserConfiguration.halloween_day),
            ("allhallowday", DutchHolidayParserConfiguration.all_hallow_day),
            ("allsoulsday", DutchHolidayParserConfiguration.all_souls_day),
            ("guyfawkesday", DutchHolidayParserConfiguration.guyfawkes_day),
            ("veteransday", DutchHolidayParserConfiguration.veterans_day),
            ("christmaseve", DutchHolidayParserConfiguration.christmas_eve),
            ("newyeareve", DutchHolidayParserConfiguration.new_year_eve),
            ("fathersday", DutchHolidayParserConfiguration.fathers_day),
            ("mothersday", DutchHolidayParserConfiguration.mothers_day),
            ("labourday", DutchHolidayParserConfiguration.international_workers_day),
            ("memorialday", DutchHolidayParserConfiguration.memorial_day),
            ("easterday", DutchHolidayParserConfiguration.easter_day),
            ("eastermonday", DutchHolidayParserConfiguration.easter_day),
            ("mardigras", DutchHolidayParserConfiguration.easter_day),
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

        if self.next_prefix_regex.search(trimmed_text):
            swift = 1

        if self.previous_prefix_regex.search(trimmed_text):
            swift = -1

        if self.this_prefix_regex.search(trimmed_text):
            swift = 0

        return swift

    def sanitize_holiday_token(self, holiday: str) -> str:
        return holiday\
            .replace(' ', '')\
            .replace("'", '') \
            .replace('-', '')
