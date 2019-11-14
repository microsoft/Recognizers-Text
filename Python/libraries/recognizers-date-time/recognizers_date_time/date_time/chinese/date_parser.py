from numbers import Number
from typing import List, Dict, Optional
from datedelta import datedelta
from datetime import datetime, timedelta
import regex

from recognizers_text import RegExpUtility, ExtractResult
from recognizers_number import Constants as NumberConstants, ChineseIntegerExtractor
from .date_extractor import ChineseDateExtractor
from .duration_extractor import ChineseDurationExtractor

from ...resources.chinese_date_time import ChineseDateTime
from ..constants import TimeTypeConstants, Constants
from ..utilities import DateTimeResolutionResult, DateTimeFormatUtil, DateUtils, DayOfWeek
from ..parsers import DateTimeParseResult
from ..base_date import BaseDateParser
from .date_parser_config import ChineseDateParserConfiguration


class ChineseDateParser(BaseDateParser):
    integer_extractor = ChineseIntegerExtractor()

    def __init__(self):
        super().__init__(ChineseDateParserConfiguration())
        self.lunar_regex = RegExpUtility.get_safe_reg_exp(
            ChineseDateTime.LunarRegex)
        self.special_date_regex = RegExpUtility.get_safe_reg_exp(
            ChineseDateTime.SpecialDate)
        self.token_next_regex = RegExpUtility.get_safe_reg_exp(
            ChineseDateTime.NextPrefixRegex)
        self.token_last_regex = RegExpUtility.get_safe_reg_exp(
            ChineseDateTime.LastPrefixRegex)
        self.month_max_days: List[int] = [
            31, 29, 31, 30, 31, 30, 31, 31, 30, 31, 30, 31]
        self.duration_extractor = ChineseDurationExtractor()

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
                inner_result = self.parse_weekday_of_month(
                    source_text, reference)

            if not inner_result.success:
                inner_result = self.parser_duration_with_ago_and_later(
                    source_text, reference)

            if inner_result.success:
                inner_result.future_resolution: Dict[str, str] = dict()
                inner_result.future_resolution[TimeTypeConstants.DATE] = DateTimeFormatUtil.format_date(
                    inner_result.future_value)
                inner_result.past_resolution: Dict[str, str] = dict()
                inner_result.past_resolution[TimeTypeConstants.DATE] = DateTimeFormatUtil.format_date(
                    inner_result.past_value)
                inner_result.is_lunar = self.__parse_lunar_calendar(
                    source_text)
                result_value = inner_result

        result = DateTimeParseResult(source)
        result.value = result_value
        result.timex_str = result_value.timex if result_value is not None else ''
        result.resolution_str = ''

        return result

    def __parse_lunar_calendar(self, source: str) -> bool:
        return regex.match(self.lunar_regex, source.strip()) is not None

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
                    if month == Constants.MAX_MONTH + 1:
                        month = Constants.MIN_MONTH
                        year += 1
                elif regex.search(self.token_last_regex, month_str):
                    month -= 1
                    if month == Constants.MIN_MONTH - 1:
                        month = Constants.MAX_MONTH
                        year -= 1

                if has_year:
                    if regex.search(self.token_next_regex, year_str):
                        year += 1
                    elif regex.search(self.token_last_regex, year_str):
                        year -= 1

            result.timex = DateTimeFormatUtil.luis_date(
                year if has_year else -1, month if has_month else -1, day)

            future_date: datetime
            past_date: datetime

            if day > self.get_month_max_day(year, month):
                future_month = month + 1
                past_month = month - 1
                future_year = year
                past_year = year

                if future_month == Constants.MAX_MONTH + 1:
                    future_month = Constants.MIN_MONTH
                    future_year = year + 1

                if past_month == Constants.MIN_MONTH - 1:
                    past_month = Constants.MAX_MONTH
                    past_year = year - 1

                is_future_valid = DateUtils.is_valid_date(
                    future_year, future_month, day)
                is_past_valid = DateUtils.is_valid_date(past_year, past_month, day)

                if is_future_valid and is_past_valid:
                    future_date = DateUtils.safe_create_from_min_value(
                        future_year, future_month, day)
                    past_date = DateUtils.safe_create_from_min_value(
                        past_year, past_month, day)
                elif is_future_valid and not is_past_valid:
                    future_date = past_date = DateUtils.safe_create_from_min_value(
                        future_year, future_month, day)
                elif not is_future_valid and not is_past_valid:
                    future_date = past_date = DateUtils.safe_create_from_min_value(
                        past_year, past_month, day)
                else:
                    future_date = past_date = DateUtils.safe_create_from_min_value(
                        year, month, day)
            else:
                future_date = DateUtils.safe_create_from_min_value(
                    year, month, day)
                past_date = DateUtils.safe_create_from_min_value(
                    year, month, day)

                if not has_month:
                    if future_date < reference:
                        if self.is_valid_date(year, month + 1, day):
                            future_date += datedelta(months=1)
                    if past_date >= reference:
                        if self.is_valid_date(year, month - 1, day):
                            past_date += datedelta(months=-1)
                        elif self.is_non_leap_year_Feb_29th(year, month - 1, day):
                            past_date += datedelta(months=-2)
                elif has_month and not has_year:
                    if future_date < reference:
                        if self.is_valid_date(year + 1, month, day):
                            future_date += datedelta(years=1)
                    if past_date >= reference:
                        if self.is_valid_date(year - 1, month, day):
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

            result.timex = DateTimeFormatUtil.luis_date_from_datetime(value)
            result.future_value = value
            result.past_value = value
            result.success = True
            return result

        # handle "this Friday"
        match = regex.match(self.config.this_regex, trimmed_source)
        if match and match.start() == 0 and len(match.group()) == len(trimmed_source):
            weekday_str = RegExpUtility.get_group(match, 'weekday')
            value = DateUtils.this(
                reference, self.config.day_of_week.get(weekday_str))

            result.timex = DateTimeFormatUtil.luis_date_from_datetime(value)
            result.future_value = value
            result.past_value = value
            result.success = True
            return result

        # handle "next Sunday"
        match = regex.match(self.config.next_regex, trimmed_source)
        if match and match.start() == 0 and len(match.group()) == len(trimmed_source):
            weekday_str = RegExpUtility.get_group(match, 'weekday')
            value = DateUtils.next(
                reference, self.config.day_of_week.get(weekday_str))

            result.timex = DateTimeFormatUtil.luis_date_from_datetime(value)
            result.future_value = value
            result.past_value = value
            result.success = True
            return result

        # handle "last Friday", "last mon"
        match = regex.match(self.config.last_regex, trimmed_source)
        if match and match.start() == 0 and len(match.group()) == len(trimmed_source):
            weekday_str = RegExpUtility.get_group(match, 'weekday')
            value = DateUtils.last(
                reference, self.config.day_of_week.get(weekday_str))

            result.timex = DateTimeFormatUtil.luis_date_from_datetime(value)
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

    def match_to_date(self, match, reference: datetime) -> DateTimeResolutionResult:
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

                if year < 100 and year >= Constants.MIN_TWO_DIGIT_YEAR_PAST_NUM:
                    year += 1900
                elif year < 100 and year < Constants.MAX_TWO_DIGIT_YEAR_FUTURE_NUM:
                    year += 2000

        no_year = False

        if year == 0:
            year = reference.year
            result.timex = DateTimeFormatUtil.luis_date(-1, month, day)
            no_year = True
        else:
            result.timex = DateTimeFormatUtil.luis_date(year, month, day)

        future_date = DateUtils.safe_create_from_min_value(year, month, day)
        past_date = DateUtils.safe_create_from_min_value(year, month, day)

        if no_year and future_date < reference:
            future_date = DateUtils.safe_create_from_min_value(
                year + 1, month, day)

        if no_year and past_date >= reference:
            past_date = DateUtils.safe_create_from_min_value(
                year - 1, month, day)

        result.future_value = future_date
        result.past_value = past_date
        result.success = True
        return result

    # convert Chinese Number to Integer
    def parse_chinese_written_number_to_value(self, source: str) -> int:
        num = -1
        er: ExtractResult = next(
            iter(self.integer_extractor.extract(source)), None)
        if er and er.type == NumberConstants.SYS_NUM_INTEGER:
            num = int(self.config.number_parser.parse(er).value)

        return num

    def convert_chinese_year_to_number(self, source: str) -> int:
        year = 0
        er: ExtractResult = next(
            iter(self.config.integer_extractor.extract(source)), None)
        if er and er.type == NumberConstants.SYS_NUM_INTEGER:
            year = int(self.config.number_parser.parse(er).value)

        if year < 10:
            year = 0
            for char in source:
                year = year * 10
                er = next(
                    iter(self.config.integer_extractor.extract(char)), None)
                if er and er.type == NumberConstants.SYS_NUM_INTEGER:
                    year = year + \
                        int(self.config.number_parser.parse(er).value)

        return -1 if year < 10 else year

    def get_month_of_year(self, source: str) -> int:
        if self.config.month_of_year[source] > 12:
            return self.config.month_of_year[source] % 12
        return self.config.month_of_year[source]

    def get_day_of_month(self, source: str) -> int:
        if self.config.day_of_month[source] > 31:
            return self.config.day_of_month[source] % 31
        return self.config.day_of_month[source]

    def is_leap_year(self, year) -> bool:
        return (year % 4 == 0) and (year % 100 != 0) or (year % 400 == 0)

    def get_month_max_day(self, year, month) -> int:
        max_day = self.month_max_days[month - 1]

        if not self.is_leap_year(year) and month == 2:
            max_day -= 1
        return max_day

    def is_valid_date(self, year, month, day):
        if month < Constants.MIN_MONTH:
            year -= 1
            month = Constants.MAX_MONTH

        if month > Constants.MAX_MONTH:
            year += 1
            month = Constants.MIN_MONTH

        return DateUtils.is_valid_date(year, month, day)

    def is_non_leap_year_Feb_29th(self, year, month, day):
        return not self.is_leap_year(year) and month == 2 and day == 29

    # Handle cases like "三天前"
    def parser_duration_with_ago_and_later(self, source: str, reference: datetime) -> DateTimeResolutionResult:
        result = DateTimeResolutionResult()
        duration_res = self.duration_extractor.extract(source, reference).pop()

        if duration_res:
            match = self.config._unit_regex.search(source)
            if match:
                suffix = source[duration_res.start + duration_res.length:]
                src_unit = RegExpUtility.get_group(match, 'unit')

                number_str = source[duration_res.start:match.lastindex - duration_res.start + 1]
                number = self.parse_chinese_written_number_to_value(number_str)

                if src_unit in self.config.unit_map:
                    unit_str = self.config.unit_map.get(src_unit)

                    before_match = RegExpUtility.get_matches(ChineseDateExtractor.before_regex, suffix)
                    if before_match and suffix.startswith(before_match[0]):
                        if unit_str == Constants.TIMEX_DAY:
                            date = reference + timedelta(days=-number)
                        elif unit_str == Constants.TIMEX_WEEK:
                            date = reference + timedelta(days=-7 * number)
                        elif unit_str == Constants.TIMEX_MONTH_FULL:
                            date = reference.replace(month=reference.month-1)
                        elif unit_str == Constants.TIMEX_YEAR:
                            date = reference.replace(year=reference.year-1)
                        else:
                            return result

                        result.timex = DateTimeFormatUtil.luis_date_from_datetime(date)
                        result.future_value = result.past_value = date
                        result.success = True
                        return result

                    after_match = RegExpUtility.get_matches(ChineseDateExtractor.after_regex, suffix)
                    if after_match and suffix.startswith(after_match[0]):
                        if unit_str == Constants.TIMEX_DAY:
                            date = reference + timedelta(days=number)
                        elif unit_str == Constants.TIMEX_WEEK:
                            date = reference + timedelta(days=7 * number)
                        elif unit_str == Constants.TIMEX_MONTH_FULL:
                            date = reference.replace(month=reference.month+1)
                        elif unit_str == Constants.TIMEX_YEAR:
                            date = reference.replace(year=reference.year+1)
                        else:
                            return result

                        result.timex = DateTimeFormatUtil.luis_date_from_datetime(date)
                        result.future_value = result.past_value = date
                        result.success = True
                        return result

        return result
