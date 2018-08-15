from typing import List, Dict, Optional
from datedelta import datedelta
from datetime import datetime, timedelta
import regex

from recognizers_text import RegExpUtility, ExtractResult
from recognizers_number import Constants as NumberConstants

from ...resources.chinese_date_time import ChineseDateTime
from ..constants import TimeTypeConstants, Constants
from ..utilities import DateTimeResolutionResult, FormatUtil, DateUtils, DayOfWeek
from ..parsers import DateTimeParseResult
from ..base_date import BaseDateParser
from .date_parser_config import ChineseDateParserConfiguration

class ChineseDateParser(BaseDateParser):
    def __init__(self):
        super().__init__(ChineseDateParserConfiguration())
        self.lunar_regex = RegExpUtility.get_safe_reg_exp(ChineseDateTime.LunarRegex)
        self.special_date_regex = RegExpUtility.get_safe_reg_exp(ChineseDateTime.SpecialDate)
        self.token_next_regex = RegExpUtility.get_safe_reg_exp(ChineseDateTime.DateNextRe)
        self.token_last_regex = RegExpUtility.get_safe_reg_exp(ChineseDateTime.DateLastRe)
        self.month_max_days: List[int] = [ 31, 29, 31, 30, 31, 30, 31, 31, 30, 31, 30, 31 ]


    def parse(self, source: ExtractResult, reference: datetime = None) -> Optional[DateTimeParseResult]:
        if reference is None:
            reference = datetime.now()

        result_value: DateTimeParseResult = None

        if source.type is self.parser_type_name:
            source_text = source.text.lower()
            inner_result = self.parse_basic_regex_match(source_text, reference)

            if not inner_result.success:
                inner_result = self.parse_implicit_date(source_text, reference)

            if not inner_result.success:
                inner_result = self.parse_weekday_of_month(source_text, reference)

            if not inner_result.success:
                inner_result = self.parser_duration_with_ago_and_later(source_text, reference)

            if inner_result.success:
                inner_result.future_resolution: Dict[str, str] = dict()
                inner_result.future_resolution[TimeTypeConstants.DATE] = FormatUtil.format_date(inner_result.future_value)
                inner_result.past_resolution: Dict[str, str] = dict()
                inner_result.past_resolution[TimeTypeConstants.DATE] = FormatUtil.format_date(inner_result.past_value)
                inner_result.is_lunar = self.__parse_lunar_calendar(source_text)
                result_value = inner_result

        result = DateTimeParseResult(source)
        result.value = result_value
        result.timex_str = result_value.timex if result_value is not None else ''
        result.resolution_str = ''

        return result

    def __parse_lunar_calendar(self, source: str) -> bool:
        return regex.match(self.lunar_regex, source.strip()) != None

    def parse_basic_regex_match(self, source: str, reference: datetime) -> DateTimeParseResult:
        trimmed_source = source.strip()
        result = DateTimeResolutionResult()

        for regexp in self.config.date_regex:
            match = regex.search(regexp, trimmed_source)
            if match and match.start() == 0 and len(match.group()) == len(trimmed_source):
                result = self.match_to_date(match, reference)
                break

        return result

    def parse_implicit_date(self, source: str, reference: datetime) -> DateTimeParseResult:
        trimmed_source = source.strip()
        result = DateTimeResolutionResult()

        # handle "on 12"
        match = regex.search(self.special_date_regex, trimmed_source)
        if match and len(match.group()) == len(trimmed_source):
            day = 0
            month = reference.month
            year = reference.year
            year_str = RegExpUtility.get_group(match, 'thisyear')
            month_str = RegExpUtility.get_group(match, 'thismonth')
            day_str = RegExpUtility.get_group(match, 'day')
            day = self.config.day_of_month.get(day_str, -1)

            has_year = year_str.strip() != ''
            has_month = month_str.strip() != ''

            if has_month:
                if regex.search(self.token_next_regex, month_str):
                    month += 1
                    if month == 12:
                        month = 0
                        year += 1
                elif regex.search(self.token_last_regex, month_str):
                    month -= 1
                    if month == -1:
                        month = 12
                        year -= 1

                if has_year:
                    if regex.search(self.token_next_regex, year_str):
                        year += 1
                    elif regex.search(self.token_last_regex, year_str):
                        year -= 1

            result.timex = FormatUtil.luis_date(year if has_year else -1, month if has_month else -1, day)

            future_date: datetime
            past_date: datetime

            if day > self.month_max_days[month]:
                future_date = DateUtils.safe_create_from_min_value(year, month + 1, day)
                past_date = DateUtils.safe_create_from_min_value(year, month - 1, day)
            else:
                future_date = DateUtils.safe_create_from_min_value(year, month, day)
                past_date = DateUtils.safe_create_from_min_value(year, month, day)

                if not has_month:
                    if future_date < reference:
                        future_date += datedelta(months=1)
                    if past_date >= reference:
                        past_date += datedelta(months=-1)
                elif has_month and not has_year:
                    if future_date < reference:
                        future_date += datedelta(years=1)
                    if past_date >= reference:
                        past_date += datedelta(years=-1)

            result.future_value = future_date
            result.past_value = past_date
            result.success = True
            return result

        # handle "today", "the day before yesterday"
        match = regex.match(self.config.special_day_regex, trimmed_source)
        if match and match.start() == 0 and len(match.group()) == len(trimmed_source):
            swift = self.config.get_swift_day(match.group())
            value = reference + timedelta(days=swift)

            result.timex = FormatUtil.luis_date_from_datetime(value)
            result.future_value = value
            result.past_value = value
            result.success = True
            return result

        # handle "this Friday"
        match = regex.match(self.config.this_regex, trimmed_source)
        if match and match.start() == 0 and len(match.group()) == len(trimmed_source):
            weekday_str = RegExpUtility.get_group(match, 'weekday')
            value = DateUtils.this(reference, self.config.day_of_week.get(weekday_str))

            result.timex = FormatUtil.luis_date_from_datetime(value)
            result.future_value = value
            result.past_value = value
            result.success = True
            return result

        # handle "next Sunday"
        match = regex.match(self.config.next_regex, trimmed_source)
        if match and match.start() == 0 and len(match.group()) == len(trimmed_source):
            weekday_str = RegExpUtility.get_group(match, 'weekday')
            value = DateUtils.next(reference, self.config.day_of_week.get(weekday_str))

            result.timex = FormatUtil.luis_date_from_datetime(value)
            result.future_value = value
            result.past_value = value
            result.success = True
            return result

        # handle "last Friday", "last mon"
        match = regex.match(self.config.last_regex, trimmed_source)
        if match and match.start() == 0 and len(match.group()) == len(trimmed_source):
            weekday_str = RegExpUtility.get_group(match, 'weekday')
            value = DateUtils.last(reference, self.config.day_of_week.get(weekday_str))

            result.timex = FormatUtil.luis_date_from_datetime(value)
            result.future_value = value
            result.past_value = value
            result.success = True
            return result

        # handle "Friday"
        match = regex.match(self.config.week_day_regex, trimmed_source)
        if match and match.start() == 0 and len(match.group()) == len(trimmed_source):
            weekday_str = RegExpUtility.get_group(match, 'weekday')
            weekday = self.config.day_of_week.get(weekday_str)
            value = DateUtils.this(reference, weekday)

            if weekday == 0:
                weekday = 7

            if weekday < reference.isoweekday():
                value = DateUtils.next(reference, weekday)

            result.timex = 'XXXX-WXX-' + str(weekday)
            future_date = value
            past_date = value

            if future_date < reference:
                future_date += timedelta(weeks=1)

            if past_date >= reference:
                past_date -= timedelta(weeks=1)

            result.future_value = future_date
            result.past_value = past_date
            result.success = True
            return result

        return result

    def match_to_date(self, match, reference: datetime)-> DateTimeResolutionResult:
        result = DateTimeResolutionResult()
        year_str = RegExpUtility.get_group(match, 'year')
        year_chs = RegExpUtility.get_group(match, 'yearchs')
        month_str = RegExpUtility.get_group(match, 'month')
        day_str = RegExpUtility.get_group(match, 'day')
        month = 0
        day = 0
        year_tmp = self.convert_chinese_year_to_number(year_chs)
        year = 0 if year_tmp == -1 else year_tmp

        if month_str in self.config.month_of_year and day_str in self.config.day_of_month:
            month = self.get_month_of_year(month_str)
            day = self.get_day_of_month(day_str)

            if year_str.strip():
                year = int(year_str) if year_str.isnumeric() else 0

                if year < 100 and year >= Constants.MinTwoDigitYearPastNum:
                    year += 1900
                elif year < 100 and year < Constants.MaxTwoDigitYearFutureNum:
                    year += 2000

        no_year = False

        if year == 0:
            year = reference.year
            result.timex = FormatUtil.luis_date(-1, month, day)
            no_year = True
        else:
            result.timex = FormatUtil.luis_date(year, month, day)

        future_date = DateUtils.safe_create_from_min_value(year, month, day)
        past_date = DateUtils.safe_create_from_min_value(year, month, day)

        if no_year and future_date < reference:
            future_date = DateUtils.safe_create_from_min_value(year + 1, month, day)

        if no_year and past_date >= reference:
            past_date = DateUtils.safe_create_from_min_value(year - 1, month, day)

        result.future_value = future_date
        result.past_value = past_date
        result.success = True
        return result

    def convert_chinese_year_to_number(self, source: str) -> int:
        year = 0
        er: ExtractResult = next(iter(self.config.integer_extractor.extract(source)), None)
        if er and er.type == NumberConstants.SYS_NUM_INTEGER:
            year = int(self.config.number_parser.parse(er).value)

        if year < 10:
            year = 0
            for char in source:
                year = year * 10
                er = next(iter(self.config.integer_extractor.extract(char)), None)
                if er and er.type == NumberConstants.SYS_NUM_INTEGER:
                    year = year + int(self.config.number_parser.parse(er).value)

        return -1 if year < 10 else year

    def get_month_of_year(self, source: str) -> int:
        if self.config.month_of_year[source] > 12:
            return self.config.month_of_year[source] % 12
        return self.config.month_of_year[source]

    def get_day_of_month(self, source: str) -> int:
        if self.config.day_of_month[source] > 31:
            return self.config.day_of_month[source] % 31
        return self.config.day_of_month[source]
