from abc import abstractmethod
from calendar import Calendar
from datetime import datetime
from datedelta import datedelta
import regex
from typing import List, Pattern, Callable, Dict, Optional, Match

from recognizers_text import Metadata
from recognizers_text.extractor import ExtractResult
from recognizers_text.utilities import RegExpUtility
from recognizers_number import Constants as NumberConstants
from recognizers_date_time.date_time.constants import Constants, TimeTypeConstants
from recognizers_date_time.date_time.extractors import DateTimeExtractor
from recognizers_date_time.date_time.parsers import DateTimeParseResult, DateTimeParser
from recognizers_date_time.date_time.utilities import DateTimeOptionsConfiguration, Token, merge_all_tokens, \
    DateTimeFormatUtil, DateTimeResolutionResult, DateUtils, DayOfWeek, HolidayFunctions
from recognizers_number import BaseNumberExtractor, BaseNumberParser


class CJKHolidayExtractorConfiguration(DateTimeOptionsConfiguration):
    @property
    @abstractmethod
    def holiday_regexes(self) -> List[Pattern]:
        raise NotImplementedError


class BaseCJKHolidayExtractor(DateTimeExtractor):

    @property
    def extractor_type_name(self) -> str:
        return Constants.SYS_DATETIME_DATE  # "Date"

    def __init__(self, config: CJKHolidayExtractorConfiguration):
        self.config = config

    def extract(self, text: str) -> List[ExtractResult]:
        return self.extract(text, datetime.now())

    def extract(self, source: str, reference: datetime = None) -> List[ExtractResult]:
        tokens: List[Token] = list()
        tokens.extend(self.holiday_match(source))
        ers: List[ExtractResult] = merge_all_tokens(tokens, source, self.extractor_type_name)

        for er in ers:
            er.metadata = Metadata()
            er.metadata.is_holiday = True

        return ers

    def holiday_match(self, text: str) -> List[Token]:
        ret: List[Token] = list()
        for pattern in self.config.holiday_regexes:
            matches = list(regex.finditer(pattern, text))
            ret.extend(map(lambda x: Token(x.start(), x.end()), matches))
        return ret


class CJKHolidayParserConfiguration(DateTimeOptionsConfiguration):

    @property
    @abstractmethod
    def integer_extractor(self) -> BaseNumberExtractor:
        return NotImplementedError

    @property
    @abstractmethod
    def number_parser(self) -> BaseNumberParser:
        raise NotImplementedError

    @property
    @abstractmethod
    def fixed_holidays_dict(self) -> Dict[str, Callable[[int], datetime]]:
        raise NotImplementedError

    @property
    @abstractmethod
    def holiday_func_dict(self) -> Dict[str, Callable[[int], datetime]]:
        raise NotImplementedError

    @property
    @abstractmethod
    def no_fixed_timex(self) -> Dict[str, str]:
        raise NotImplementedError

    @property
    @abstractmethod
    def holiday_regex_list(self) -> List[Pattern]:
        raise NotImplementedError

    @property
    @abstractmethod
    def lunar_holiday_regex(self) -> Pattern:
        raise NotImplementedError

    @abstractmethod
    def get_swift_year(self, source: str) -> str:
        raise NotImplementedError

    @abstractmethod
    def sanitize_year_token(self, source: str) -> str:
        raise NotImplementedError

    @staticmethod
    def new_year(year: int) -> datetime:
        return datetime(year, 1, 1)

    @staticmethod
    def usa_independence_day(year: int) -> datetime:
        return datetime(year, 7, 4)

    @staticmethod
    def teacher_day(year: int) -> datetime:
        return datetime(year, 9, 10)

    @staticmethod
    def mao_birthday(year: int) -> datetime:
        return datetime(year, 12, 26)

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
    def lover_day(year: int) -> datetime:
        return datetime(year, 2, 14)

    @staticmethod
    def christmas_day(year: int) -> datetime:
        return datetime(year, 12, 25)

    @staticmethod
    def christmas_eve(year: int) -> datetime:
        return datetime(year, 12, 24)

    @staticmethod
    def easter_day(year: int) -> datetime:
        return HolidayFunctions.calculate_holiday_by_easter(year)

    @staticmethod
    def fool_day(year: int) -> datetime:
        return datetime(year, 4, 1)

    @staticmethod
    def labor_day(year: int) -> datetime:
        return datetime(year, 5, 1)

    @staticmethod
    def halloween_day(year: int) -> datetime:
        return datetime(year, 10, 31)

    @staticmethod
    def mid_autumn_day(year: int) -> datetime:
        return datetime(year, 8, 15)

    @staticmethod
    def spring_day(year: int) -> datetime:
        return datetime(year, 1, 1)

    @staticmethod
    def new_year_eve(year: int) -> datetime:
        return datetime(year, 12, 31)

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
    def boys_festival(year: int) -> datetime:
        return datetime(year, 5, 5)

    @staticmethod
    def jap_national_day(year: int) -> datetime:
        return datetime(year, 10, 1)

    @staticmethod
    def jap_mil_build_day(year: int) -> datetime:
        return datetime(year, 8, 1)

    @staticmethod
    def girls_day(year: int) -> datetime:
        return datetime(year, 3, 7)

    @staticmethod
    def singles_day(year: int) -> datetime:
        return datetime(year, 11, 11)

    @staticmethod
    def chong_yang_day(year: int) -> datetime:
        return datetime(year, 9, 9)

    @staticmethod
    def get_day(year: int, month: int, week: int, day_of_week: DayOfWeek) -> int:
        calendar = Calendar()
        return [d for d in calendar.itermonthdays2(year, month) if d[0] and d[1] == day_of_week - 1][week][0]

    @staticmethod
    def get_last_day(year: int, month: int, day_of_week: DayOfWeek) -> int:
        return CJKHolidayParserConfiguration.get_day(year, month, -1, day_of_week)

    @staticmethod
    def mothers_day(year: int) -> datetime:
        return datetime(year, 5, CJKHolidayParserConfiguration.get_day(year, 5, 1, DayOfWeek.SUNDAY))

    @staticmethod
    def fathers_day(year: int) -> datetime:
        return datetime(year, 6, CJKHolidayParserConfiguration.get_day(year, 6, 2, DayOfWeek.SUNDAY))

    @staticmethod
    def martin_luther_king_day(year: int) -> datetime:
        return datetime(year, 1, CJKHolidayParserConfiguration.get_day(year, 1, 2, DayOfWeek.MONDAY))

    @staticmethod
    def washingtons_birthday(year: int) -> datetime:
        return datetime(year, 2, CJKHolidayParserConfiguration.get_day(year, 2, 2, DayOfWeek.MONDAY))

    @staticmethod
    def canberra_day(year: int) -> datetime:
        return datetime(year, 3, CJKHolidayParserConfiguration.get_day(year, 3, 0, DayOfWeek.MONDAY))

    @staticmethod
    def memorial_day(year: int) -> datetime:
        return datetime(year, 5, CJKHolidayParserConfiguration.get_last_day(year, 5, DayOfWeek.MONDAY))

    @staticmethod
    def labour_day(year: int) -> datetime:
        return datetime(year, 9, CJKHolidayParserConfiguration.get_day(year, 9, 0, DayOfWeek.MONDAY))

    @staticmethod
    def international_workers_day(year: int) -> datetime:
        return datetime(year, 5, 1)

    @staticmethod
    def columbus_day(year: int) -> datetime:
        return datetime(year, 10, CJKHolidayParserConfiguration.get_day(year, 10, 1, DayOfWeek.MONDAY))

    @staticmethod
    def thanksgiving_day(year: int) -> datetime:
        return datetime(year, 11, CJKHolidayParserConfiguration.get_day(year, 11, 3, DayOfWeek.THURSDAY))


class BaseCJKHolidayParser(DateTimeParser):
    @property
    def parser_type_name(self) -> str:
        return Constants.SYS_DATETIME_DATE

    def __init__(self, config: CJKHolidayParserConfiguration):
        self.config = config

    def parse(self, source: ExtractResult, reference: datetime = None) -> Optional[DateTimeParseResult]:
        reference_date = reference if reference is not None else datetime.now()
        value = None

        if source.type == self.parser_type_name:
            inner_result = self.parse_holiday_regex_match(source.text, reference_date)

            if inner_result.success:
                inner_result.future_resolution = {
                    TimeTypeConstants.DATE: DateTimeFormatUtil.format_date(inner_result.future_value)
                }
                inner_result.past_resolution = {
                    TimeTypeConstants.DATE: DateTimeFormatUtil.format_date(inner_result.past_value)
                }
                inner_result.is_lunar = self.is_lunar_calendar(source.text)
                value = inner_result

        result = DateTimeParseResult(source)
        result.value = value
        result.timex_str = value.timex if value else ''
        result.resolution_str = ''

        return result

    def filter_result(self, query: str, candidate_results: List[DateTimeParseResult]):
        return candidate_results

    def get_future_value(self, value: datetime, reference_date: datetime, holiday: str) -> datetime:
        if value < reference_date:
            if holiday in self.config.fixed_holidays_dict:
                return value + datedelta(years=1)
            if holiday in self.config.holiday_func_dict:
                value = self.config.holiday_func_dict[holiday](reference_date.year + 1)
        return value

    def get_past_value(self, value: datetime, reference_date: datetime, holiday: str) -> datetime:
        if value >= reference_date:
            if holiday in self.config.fixed_holidays_dict:
                return value + datedelta(years=-1)
            if holiday in self.config.holiday_func_dict:
                value = self.config.holiday_func_dict[holiday](reference_date.year - 1)
        return value

    def parse_holiday_regex_match(self, text: str, reference_date: datetime) -> DateTimeResolutionResult:
        for pattern in self.config.holiday_regex_list:
            match = pattern.match(text)
            if match and match.pos == 0 and match.endpos == len(text):
                # Value string will be set in Match2Date method
                result = self.match_to_date(match, reference_date)
                return result
        return DateTimeResolutionResult()

    def match_to_date(self, match: Match, reference_date: datetime) -> DateTimeResolutionResult:
        result = DateTimeResolutionResult()
        holiday_str = match.group('holiday')

        year = reference_date.year
        has_year = False
        year_num = match.group('year')
        year_cjk = match.group(Constants.YEAR_CJK_GROUP_NAME)
        year_relative = match.group('yearrel')

        if year_num:
            has_year = True
            year_num = self.config.sanitize_year_token(year_num)
            year = int(year_num)

        elif year_cjk:
            has_year = True
            year_cjk = self.config.sanitize_year_token(year_cjk)
            year = self.convert_to_integer(year_cjk)

        elif year_relative:
            has_year = True
            swift = self.config.get_swift_year(year_relative)
            if swift >= -1:
                year += swift

        if 100 > year >= 90:
            year += Constants.BASE_YEAR_PAST_CENTURY
        elif year < 20:
            year += Constants.BASE_YEAR_CURRENT_CENTURY

        if holiday_str:
            value: datetime = None
            timex = ''
            if holiday_str in self.config.fixed_holidays_dict:
                value = self.config.fixed_holidays_dict[holiday_str](year)
                timex = f'-{DateTimeFormatUtil.to_str(value.month, 2)}-{DateTimeFormatUtil.to_str(value.day, 2)}'
            else:
                if holiday_str in self.config.holiday_func_dict:
                    value = self.config.holiday_func_dict[holiday_str](year)
                    timex = self.config.no_fixed_timex[holiday_str]
                else:
                    return result

            if has_year:
                result.timex = DateTimeFormatUtil.to_str(year, 4) + timex
                result.future_value = DateUtils.safe_create_from_min_value(year, value.month, value.day)
                result.past_value = result.future_value
                result.success = True
                return result

            result.timex = 'XXXX' + timex
            result.future_value = self.get_future_value(value, reference_date, holiday_str)
            result.past_value = self.get_past_value(value, reference_date, holiday_str)
            result.success = True
            return result

        return result

    def convert_to_integer(self, year_cjk: str):
        year = 0
        num = 0

        er = self.config.integer_extractor.extract(year_cjk)
        if er and er[0].type == NumberConstants.SYS_NUM_INTEGER:
            num = self.config.number_parser.parse(er[0]).value

        if num < 10:
            num = 0
            for ch in year_cjk:
                num *= 10
                er = self.config.integer_extractor.extract(ch)
                if er and er[0].type == NumberConstants.SYS_NUM_INTEGER:
                    num += self.config.number_parser.parse(er[0]).value

            year = num
        else:
            year = num
        return -1 if year == 0 else year

    def is_lunar_calendar(self, text: str):
        source = text.strip().lower()
        match = RegExpUtility.get_matches(self.config.lunar_holiday_regex, source)
        if match:
            return True
        return False
