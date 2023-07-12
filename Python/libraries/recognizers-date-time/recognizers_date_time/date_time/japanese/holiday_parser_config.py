from datetime import datetime
from typing import List, Pattern, Dict, Callable

from recognizers_text.utilities import RegExpUtility
from recognizers_number import BaseNumberParser, BaseNumberExtractor
from recognizers_date_time.date_time.CJK import CJKHolidayParserConfiguration, CJKCommonDateTimeParserConfiguration
from recognizers_date_time.resources.japanese_date_time import JapaneseDateTime


class JapaneseHolidayParserConfiguration(CJKHolidayParserConfiguration):

    @property
    def fixed_holidays_dict(self) -> Dict[str, Callable[[int], datetime]]:
        return self._fixed_holidays_dict

    @property
    def holiday_func_dict(self) -> Dict[str, Callable[[int], datetime]]:
        return self._holiday_func_dictionary

    @property
    def no_fixed_timex(self) -> Dict[str, str]:
        return self._no_fixed_timex

    @property
    def integer_extractor(self) -> BaseNumberExtractor:
        return self._integer_extractor

    @property
    def number_parser(self) -> BaseNumberParser:
        return self._number_parser

    @property
    def holiday_regex_list(self) -> List[Pattern]:
        return self._holiday_regex_list

    @property
    def lunar_holiday_regex(self) -> Pattern:
        return self._lunar_holiday_regex

    def __init__(self, config: CJKCommonDateTimeParserConfiguration):
        super().__init__()
        self._no_fixed_timex = JapaneseDateTime.HolidayNoFixedTimex
        self._integer_extractor = config.integer_extractor
        self._number_parser = config.number_parser

        self._lunar_holiday_regex = RegExpUtility.get_safe_reg_exp(JapaneseDateTime.LunarHolidayRegex)
        self._holiday_regex_list = self._holiday_regex_list = [
            RegExpUtility.get_safe_reg_exp(JapaneseDateTime.HolidayRegexList1),
            RegExpUtility.get_safe_reg_exp(JapaneseDateTime.HolidayRegexList2),
            self.lunar_holiday_regex
        ]

        self._holiday_func_dictionary = self._init_holiday_funcs()
        self._fixed_holidays_dict = self._init_fixed_holiday_funcs()

    def _init_holiday_funcs(self) -> Dict[str, Callable[[int], datetime]]:
        return dict([
            ('父亲节', CJKHolidayParserConfiguration.fathers_day),
            ('父の日', CJKHolidayParserConfiguration.fathers_day),
            ('母亲节', CJKHolidayParserConfiguration.mothers_day),
            ('母の日', CJKHolidayParserConfiguration.mothers_day),
            ('感恩节', CJKHolidayParserConfiguration.thanksgiving_day),
            ('感謝祭の日', CJKHolidayParserConfiguration.thanksgiving_day),
            ('感謝祭', CJKHolidayParserConfiguration.thanksgiving_day),
            ('キング牧師記念日', CJKHolidayParserConfiguration.martin_luther_king_day),
        ])

    def _init_fixed_holiday_funcs(self) -> Dict[str, Callable[[int], datetime]]:
        return dict([
            ("元旦", CJKHolidayParserConfiguration.new_year),
            ("旧正月", CJKHolidayParserConfiguration.new_year),
            ("元旦节", CJKHolidayParserConfiguration.new_year),
            ("お正月", CJKHolidayParserConfiguration.new_year),
            ("独立記念日", CJKHolidayParserConfiguration.usa_independence_day),
            ("旧暦の正月初一", CJKHolidayParserConfiguration.spring_day),
            ("教师节", CJKHolidayParserConfiguration.teacher_day),
            ("教師の日", CJKHolidayParserConfiguration.teacher_day),
            ("青年节", CJKHolidayParserConfiguration.youth_day),
            ("青年の日", CJKHolidayParserConfiguration.youth_day),
            ("儿童节", CJKHolidayParserConfiguration.children_day),
            ("子供の日", CJKHolidayParserConfiguration.children_day),
            ("妇女节", CJKHolidayParserConfiguration.female_day),
            ("国際婦人デー", CJKHolidayParserConfiguration.female_day),
            ("植树节", CJKHolidayParserConfiguration.tree_plant_day),
            ("植樹祭", CJKHolidayParserConfiguration.tree_plant_day),
            ("情人节", CJKHolidayParserConfiguration.lover_day),
            ("バレンタインデー", CJKHolidayParserConfiguration.lover_day),
            ("圣诞节", CJKHolidayParserConfiguration.christmas_day),
            ("クリスマスの日", CJKHolidayParserConfiguration.christmas_day),
            ("クリスマス", CJKHolidayParserConfiguration.christmas_day),
            ("クリスマスイブ", CJKHolidayParserConfiguration.christmas_eve),
            ("新年", CJKHolidayParserConfiguration.new_year),
            ("復活祭", CJKHolidayParserConfiguration.easter_day),
            ("愚人节", CJKHolidayParserConfiguration.fool_day),
            ("エイプリルフール", CJKHolidayParserConfiguration.fool_day),
            ("五一", CJKHolidayParserConfiguration.labor_day),
            ("劳动节", CJKHolidayParserConfiguration.labor_day),
            ("メーデー", CJKHolidayParserConfiguration.labor_day),
            ("国際的な労働者の日", CJKHolidayParserConfiguration.labor_day),
            ("万圣节", CJKHolidayParserConfiguration.halloween_day),
            ("ハロウィン", CJKHolidayParserConfiguration.halloween_day),
            ("中秋节", CJKHolidayParserConfiguration.mid_autumn_day),
            ("中秋", CJKHolidayParserConfiguration.mid_autumn_day),
            ("中秋節", CJKHolidayParserConfiguration.mid_autumn_day),
            ("春节", CJKHolidayParserConfiguration.spring_day),
            ("除夕", CJKHolidayParserConfiguration.new_year_eve),
            ("大晦日", CJKHolidayParserConfiguration.new_year_eve),
            ("元宵节", CJKHolidayParserConfiguration.lantern_day),
            ("元宵節", CJKHolidayParserConfiguration.lantern_day),
            ("清明节", CJKHolidayParserConfiguration.qing_ming_day),
            ("清明節", CJKHolidayParserConfiguration.qing_ming_day),
            ("清明", CJKHolidayParserConfiguration.qing_ming_day),
            ("端午节", CJKHolidayParserConfiguration.dragon_boat_day),
            ("端午の節句", CJKHolidayParserConfiguration.boys_festival),
            ("端午", CJKHolidayParserConfiguration.dragon_boat_day),
            ("国庆节", CJKHolidayParserConfiguration.jap_national_day),
            ("国慶節", CJKHolidayParserConfiguration.jap_national_day),
            ("建军节", CJKHolidayParserConfiguration.jap_mil_build_day),
            ("建軍節", CJKHolidayParserConfiguration.jap_mil_build_day),
            ("女生节", CJKHolidayParserConfiguration.girls_day),
            ("ガールズデー", CJKHolidayParserConfiguration.girls_day),
            ("光棍节", CJKHolidayParserConfiguration.singles_day),
            ("双十一", CJKHolidayParserConfiguration.singles_day),
            ("ダブル十一", CJKHolidayParserConfiguration.singles_day),
            ("シングルデー", CJKHolidayParserConfiguration.singles_day),
            ("重阳节", CJKHolidayParserConfiguration.chong_yang_day),
            ("重陽節", CJKHolidayParserConfiguration.chong_yang_day),
        ])

    def get_swift_year(self, source: str) -> int:
        # TODO move hardcoded values to resource file
        trimmed_text = source.strip().lower()
        swift = -10

        if trimmed_text.endswith('前年') or trimmed_text.endswith('先年'):
            swift = -1
        if trimmed_text.endswith('来年'):
            swift = +1

        return swift

    def sanitize_year_token(self, source: str) -> str:
        # TODO move hardcoded values to resource file
        if source.endswith('年'):
            return source[0:-1]
        return source
