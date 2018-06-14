from abc import ABC, abstractmethod
from typing import List, Optional, Pattern, Callable, Dict, Match
from datetime import datetime
from calendar import Calendar

from recognizers_text.extractor import ExtractResult
from ..resources.base_date_time import BaseDateTime
from .constants import Constants, TimeTypeConstants
from .extractors import DateTimeExtractor
from .parsers import DateTimeParser, DateTimeParseResult
from .utilities import Token, merge_all_tokens, FormatUtil, DayOfWeek, DateTimeResolutionResult, DateUtils

class HolidayExtractorConfiguration(ABC):
    @property
    @abstractmethod
    def holiday_regexes(self) -> List[Pattern]:
        raise NotImplementedError

class BaseHolidayExtractor(DateTimeExtractor):
    @property
    def extractor_type_name(self) -> str:
        return Constants.SYS_DATETIME_DATE

    def __init__(self, config: HolidayExtractorConfiguration):
        self.config = config

    def extract(self, source: str, reference: datetime = None) -> List[ExtractResult]:
        if not reference:
            reference = datetime.now()

        tokens = []
        tokens += self.__holiday_match(source)
        result = merge_all_tokens(tokens, source, self.extractor_type_name)
        return result

    def __holiday_match(self, source: str) -> List[Token]:
        tokens = []

        for regexp in self.config.holiday_regexes:
            matches = regexp.finditer(source)
            for match in matches:
                tokens.append(Token(match.start(), match.end()))

        return tokens

class HolidayParserConfiguration(ABC):
    @property
    @abstractmethod
    def variable_holidays_timex_dictionary(self) -> Dict[str, str]:
        raise NotImplementedError

    @property
    @abstractmethod
    def holiday_func_dictionary(self) -> Dict[str, Callable[[int], datetime]]:
        raise NotImplementedError

    @property
    @abstractmethod
    def holiday_names(self) -> Dict[str, List[str]]:
        raise NotImplementedError

    @property
    @abstractmethod
    def holiday_regex_list(self) -> List[Pattern]:
        raise NotImplementedError

    @abstractmethod
    def get_swift_year(self, text: str) -> int:
        raise NotImplementedError

    @abstractmethod
    def sanitize_holiday_token(self, holiday: str) -> str:
        raise NotImplementedError

class BaseHolidayParser(DateTimeParser):
    @property
    def parser_type_name(self) -> str:
        return Constants.SYS_DATETIME_DATE

    def __init__(self, config: HolidayParserConfiguration):
        self.config = config

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
                value = inner_result

        result = DateTimeParseResult(source)
        result.value = value
        result.timex_str = value.timex if value else ''
        result.resolution_str = ''

        return result

    def _parse_holiday_regex_match(self, text: str, reference: datetime) -> DateTimeResolutionResult:
        trimmed_text = text.strip()

        for pattern in self.config.holiday_regex_list:
            match = pattern.search(trimmed_text)
            if match and match.pos == 0 and match.endpos == len(trimmed_text):
                result = self._match2date(match, reference)
                return result

        return DateTimeResolutionResult()

    def _match2date(self, match: Match, reference: datetime) -> DateTimeResolutionResult:
        result = DateTimeResolutionResult()
        holiday_str = self.config.sanitize_holiday_token(match.group('holiday').lower())

        year_str = match.group('year')
        order_str = match.group('order')
        year = None
        has_year = False

        if year_str:
            year = int(year_str)
            has_year = True
        elif order_str:
            swift = self.config.get_swift_year(order_str)
            if swift < -1:
                return result
            year = reference.year + swift
            has_year = True
        else:
            year = reference.year

        holiday_key = next(iter([key for key, values in self.config.holiday_names.items() if holiday_str in values]), None)

        if holiday_key:
            timex_str = ''
            value = reference
            func = self.config.holiday_func_dictionary.get(holiday_key)

            if func:
                value = func(year)
                timex_str = self.config.variable_holidays_timex_dictionary.get(holiday_key)
                if not timex_str:
                    timex_str = f'-{FormatUtil.to_str(value.month, 2)}-{FormatUtil.to_str(value.day, 2)}'
            else:
                return result

            if value == DateUtils.min_value:
                result.timex = ''
                result.future_value = DateUtils.min_value
                result.past_value = DateUtils.min_value
                result.success = True
                return result

            if has_year:
                result.timex = FormatUtil.to_str(year, 4) + timex_str
                result.future_value = datetime(year, value.month, value.day)
                result.past_value = result.future_value
                result.success = True
                return result

            result.timex = 'XXXX' + timex_str
            result.future_value = self.__get_future_value(value, reference, holiday_key)
            result.past_value = self.__get_past_value(value, reference, holiday_key)
            result.success = True
            return result

        return result

    def __get_future_value(self, value: datetime, reference: datetime, holiday: str) -> datetime:
        if value < reference:
            func = self.config.holiday_func_dictionary.get(holiday)
            if func:
                return func(value.year+1)
        return value

    def __get_past_value(self, value: datetime, reference: datetime, holiday: str) -> datetime:
        if value >= reference:
            func = self.config.holiday_func_dictionary.get(holiday)
            if func:
                return func(value.year-1)
        return value

class BaseHolidayParserConfiguration(HolidayParserConfiguration):
    @property
    def variable_holidays_timex_dictionary(self) -> Dict[str, str]:
        return self._variable_holidays_timex_dictionary

    @property
    def holiday_func_dictionary(self) -> Dict[str, Callable[[int], datetime]]:
        return self._holiday_func_dictionary

    @property
    @abstractmethod
    def holiday_names(self) -> Dict[str, List[str]]:
        raise NotImplementedError

    @property
    @abstractmethod
    def holiday_regex_list(self) -> List[Pattern]:
        raise NotImplementedError

    @abstractmethod
    def get_swift_year(self, text: str) -> int:
        raise NotImplementedError

    @abstractmethod
    def sanitize_holiday_token(self, holiday: str) -> str:
        raise NotImplementedError

    def __init__(self):
        self._variable_holidays_timex_dictionary = BaseDateTime.VariableHolidaysTimexDictionary
        self._holiday_func_dictionary = self._init_holiday_funcs()

    def _init_holiday_funcs(self) -> Dict[str, Callable[[int], datetime]]:
        return dict([
            ('fathers', BaseHolidayParserConfiguration.fathers_day),
            ('Mothers', BaseHolidayParserConfiguration.mothers_day),
            ('thanksgivingday', BaseHolidayParserConfiguration.thanksgiving_day),
            ('thanksgiving', BaseHolidayParserConfiguration.thanksgiving_day),
            ('martinlutherking', BaseHolidayParserConfiguration.martin_luther_king_day),
            ('washingtonsbirthday', BaseHolidayParserConfiguration.washingtons_birthday),
            ('labour', BaseHolidayParserConfiguration.labour_day),
            ('canberra', BaseHolidayParserConfiguration.canberra_day),
            ('colombus', BaseHolidayParserConfiguration.colombus_day),
            ('memorial', BaseHolidayParserConfiguration.memorial_day)
        ])

    @staticmethod
    def mothers_day(year: int) -> datetime:
        return datetime(year, 5, BaseHolidayParserConfiguration.get_day(year, 5, 1, DayOfWeek.Sunday))

    @staticmethod
    def fathers_day(year: int) -> datetime:
        return datetime(year, 6, BaseHolidayParserConfiguration.get_day(year, 6, 2, DayOfWeek.Sunday))

    @staticmethod
    def martin_luther_king_day(year: int) -> datetime:
        return datetime(year, 1, BaseHolidayParserConfiguration.get_day(year, 1, 2, DayOfWeek.Monday))

    @staticmethod
    def washingtons_birthday(year: int) -> datetime:
        return datetime(year, 2, BaseHolidayParserConfiguration.get_day(year, 2, 2, DayOfWeek.Monday))

    @staticmethod
    def canberra_day(year: int) -> datetime:
        return datetime(year, 3, BaseHolidayParserConfiguration.get_day(year, 3, 0, DayOfWeek.Monday))

    @staticmethod
    def memorial_day(year: int) -> datetime:
        return datetime(year, 5, BaseHolidayParserConfiguration.get_last_day(year, 5, DayOfWeek.Monday))

    @staticmethod
    def labour_day(year: int) -> datetime:
        return datetime(year, 9, BaseHolidayParserConfiguration.get_day(year, 9, 0, DayOfWeek.Monday))

    @staticmethod
    def colombus_day(year: int) -> datetime:
        return datetime(year, 10, BaseHolidayParserConfiguration.get_day(year, 10, 1, DayOfWeek.Monday))

    @staticmethod
    def thanksgiving_day(year: int) -> datetime:
        return datetime(year, 11, BaseHolidayParserConfiguration.get_day(year, 11, 3, DayOfWeek.Thursday))

    @staticmethod
    def get_day(year: int, month: int, week: int, day_of_week: DayOfWeek) -> int:
        calendar = Calendar()
        return [d for d in calendar.itermonthdays2(year, month) if d[0] and d[1] == day_of_week-1][week][0]

    @staticmethod
    def get_last_day(year: int, month: int, day_of_week: DayOfWeek) -> int:
        return BaseHolidayParserConfiguration.get_day(year, month, -1, day_of_week)
