from typing import Optional, Match
from datetime import datetime, timedelta
from datedelta import datedelta
from recognizers_number import ChineseIntegerExtractor, AgnosticNumberParserFactory, ParserType as AgnosticNumberParserType, ChineseNumberParserConfiguration, Constants as NumberConstants
from recognizers_text.extractor import ExtractResult

from ...resources.chinese_date_time import ChineseDateTime
from ..base_holiday import BaseHolidayParser
from ..parsers import DateTimeParseResult
from .holiday_parser_config import ChineseHolidayParserConfiguration
from ..constants import TimeTypeConstants
from ..utilities import FormatUtil, DateTimeResolutionResult, RegExpUtility

class ChineseHolidayParser(BaseHolidayParser):
    def __init__(self):
        config = ChineseHolidayParserConfiguration()
        BaseHolidayParser.__init__(self, config)
        self.__lunar_holiday_regex = RegExpUtility.get_safe_reg_exp(ChineseDateTime.LunarHolidayRegex)
        self.__integer_extractor = ChineseIntegerExtractor()
        self.__number_parser = AgnosticNumberParserFactory.get_parser(AgnosticNumberParserType.INTEGER, ChineseNumberParserConfiguration())
        self.__fixed_holiday_dictionary = dict([
            ('元旦', ChineseHolidayParser.new_year),
            ('元旦节', ChineseHolidayParser.new_year),
            ('教师节', ChineseHolidayParser.teacher_day),
            ('青年节', ChineseHolidayParser.youth_day),
            ('儿童节', ChineseHolidayParser.children_day),
            ('妇女节', ChineseHolidayParser.female_day),
            ('植树节', ChineseHolidayParser.tree_plant_day),
            ('情人节', ChineseHolidayParser.lover_day),
            ('圣诞节', ChineseHolidayParser.christmas_day),
            ('新年', ChineseHolidayParser.new_year),
            ('愚人节', ChineseHolidayParser.fool_day),
            ('五一', ChineseHolidayParser.labor_day),
            ('劳动节', ChineseHolidayParser.labor_day),
            ('万圣节', ChineseHolidayParser.halloween_day),
            ('中秋节', ChineseHolidayParser.midautumn_day),
            ('中秋', ChineseHolidayParser.midautumn_day),
            ('春节', ChineseHolidayParser.spring_day),
            ('除夕', ChineseHolidayParser.new_year_eve),
            ('元宵节', ChineseHolidayParser.lantern_day),
            ('清明节', ChineseHolidayParser.qing_ming_day),
            ('清明', ChineseHolidayParser.qing_ming_day),
            ('端午节', ChineseHolidayParser.dragon_boat_day),
            ('端午', ChineseHolidayParser.dragon_boat_day),
            ('国庆节', ChineseHolidayParser.chs_national_day),
            ('建军节', ChineseHolidayParser.chs_mil_build_day),
            ('女生节', ChineseHolidayParser.girls_day),
            ('光棍节', ChineseHolidayParser.singles_day),
            ('双十一', ChineseHolidayParser.singles_day),
            ('重阳节', ChineseHolidayParser.chong_yang_day)
        ])

    @staticmethod
    def new_year(year: int) -> datetime:
        return datetime(year, 1, 1)

    @staticmethod
    def chs_national_day(year: int) -> datetime:
        return datetime(year, 10, 1)

    @staticmethod
    def labor_day(year: int) -> datetime:
        return datetime(year, 5, 1)

    @staticmethod
    def christmas_day(year: int) -> datetime:
        return datetime(year, 12, 25)

    @staticmethod
    def lover_day(year: int) -> datetime:
        return datetime(year, 2, 14)

    @staticmethod
    def chs_mil_build_day(year: int) -> datetime:
        return datetime(year, 8, 1)

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
    def female_day(year: int) -> datetime:
        return datetime(year, 3, 8)

    @staticmethod
    def children_day(year: int) -> datetime:
        return datetime(year, 6, 1)

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
    def halloween_day(year: int) -> datetime:
        return datetime(year, 10, 31)

    @staticmethod
    def midautumn_day(year: int) -> datetime:
        return datetime(year, 8, 15)

    @staticmethod
    def spring_day(year: int) -> datetime:
        return datetime(year, 1, 1)

    @staticmethod
    def new_year_eve(year: int) -> datetime:
        return datetime(year, 1, 1) + timedelta(days=-1)

    @staticmethod
    def lantern_day(year: int) -> datetime:
        return datetime(year, 1, 15)

    @staticmethod
    def qing_ming_day(year: int) -> datetime:
        return datetime(year, 4, 4)

    @staticmethod
    def dragon_boat_day(year: int) -> datetime:
        return datetime(year, 5, 5)

    @staticmethod
    def chong_yang_day(year: int) -> datetime:
        return datetime(year, 9, 9)

    def parse(self, source: ExtractResult, reference: datetime = None) -> Optional[DateTimeParseResult]:
        if not reference:
            reference = datetime.now()

        value = None

        if source.type == self.parser_type_name:
            inner_result = self._parse_holiday_regex_match(source.text, reference)

            if inner_result.success:
                inner_result.future_resolution = {
                    TimeTypeConstants.DATE: FormatUtil.format_date(inner_result.future_value)
                }
                inner_result.past_resolution = {
                    TimeTypeConstants.DATE: FormatUtil.format_date(inner_result.past_value)
                }
                inner_result.is_lunar = self.__is_lunar(source.text)
                value = inner_result

        result = DateTimeParseResult(source)
        result.value = value
        result.timex_str = value.timex if value else ''
        result.resolution_str = ''

        return result

    def __is_lunar(self, source: str) -> bool:
        return self.__lunar_holiday_regex.search(source) is not None

    def _match2date(self, match: Match, reference: datetime) -> DateTimeResolutionResult:
        result = DateTimeResolutionResult()
        holiday_str = self.config.sanitize_holiday_token(match.group('holiday').lower())

        if not holiday_str:
            return result

        year = reference.year
        year_num = match.group('year')
        year_chinese = match.group('yearchs')
        year_relative = match.group('yearrel')
        has_year = False

        if year_num:
            has_year = True
            if self.config.get_swift_year(year_num) == 0:
                year_num = year_num[0:len(year_num) - 1]
            year = self.__convert_year(year_num, False)
        elif year_chinese:
            has_year = True
            if self.config.get_swift_year(year_chinese) == 0:
                year_chinese = year_chinese[0:len(year_chinese) - 1]
            year = self.__convert_year(year_chinese, True)
        elif year_relative:
            has_year = True
            year += self.config.get_swift_year(reference.year)

        if year < 100 and year >= 90:
            year += 1900
        elif year < 100 and year < 20:
            year += 2000

        timex = ''
        date = reference
        if holiday_str in self.__fixed_holiday_dictionary:
            date = self.__fixed_holiday_dictionary[holiday_str](year)
            timex = f'-{FormatUtil.to_str(date.month, 2)}-{FormatUtil.to_str(date.day, 2)}'
        elif holiday_str in self.config.holiday_func_dictionary:
            date = self.config.holiday_func_dictionary[holiday_str](year)
            timex = self.config.variable_holidays_timex_dictionary[holiday_str]
        else:
            return result

        if has_year:
            result.timex = FormatUtil.to_str(year, 4) + timex
            result.future_value = datetime(year, date.month, date.day)
            result.past_value = datetime(year, date.month, date.day)
        else:
            result.timex = 'XXXX' + timex
            result.future_value = self.__get_date_value(date, reference, holiday_str, 1, lambda d, r: d < r)
            result.past_value = self.__get_date_value(date, reference, holiday_str, -1, lambda d, r: d >= r)

        result.success = True

        return result

    def __convert_year(self, year_str: str, is_chinese: bool) -> int:
        year = -1
        if is_chinese:
            year_num = 0
            ers = self.__integer_extractor.extract(year_str)
            if ers and ers[-1].type == NumberConstants.SYS_NUM_INTEGER:
                year_num = int(self.__number_parser.parse(ers[-1]).value)

            if year_num < 10:
                year_num = 0
                for char in year_str:
                    year_num *= 10
                    ers = self.__integer_extractor.extract(char)
                    if ers and ers[-1].type == NumberConstants.SYS_NUM_INTEGER:
                        year_num += int(self.__number_parser.parse(ers[-1]).value)
            else:
                year = year_num
        else:
            year = int(year_str)

        return -1 if year == 0 else year

    def __get_date_value(self, date: datetime, reference: datetime, holiday: str, swift: int, comparer) -> datetime:
        result = date
        if comparer(date, reference):
            if holiday in self.__fixed_holiday_dictionary:
                return date + datedelta(years=swift)

            if holiday in self.config.holiday_func_dictionary:
                result = self.config.holiday_func_dictionary[holiday](reference.year + swift)

        return result
