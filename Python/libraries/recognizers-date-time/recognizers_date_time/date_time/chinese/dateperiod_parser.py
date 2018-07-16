from typing import Optional
from datetime import datetime, timedelta
from datedelta import datedelta
import regex

from recognizers_text import RegExpUtility, ExtractResult
from recognizers_number import ChineseIntegerExtractor, ChineseNumberParser, ChineseNumberParserConfiguration, Constants as NumberConstants

from ...resources.chinese_date_time import ChineseDateTime
from ..constants import TimeTypeConstants
from ..utilities import FormatUtil, DateTimeResolutionResult, DateUtils
from ..parsers import DateTimeParseResult
from ..base_dateperiod import BaseDatePeriodParser
from .dateperiod_parser_config import ChineseDatePeriodParserConfiguration

class ChineseDatePeriodParser(BaseDatePeriodParser):
    def __init__(self):
        super().__init__(ChineseDatePeriodParserConfiguration())
        self.integer_extractor = ChineseIntegerExtractor()
        self.number_parser = ChineseNumberParser(ChineseNumberParserConfiguration())
        self.year_in_chinese_regex = RegExpUtility.get_safe_reg_exp(ChineseDateTime.DatePeriodYearInChineseRegex)
        self.number_combined_with_unit_regex = RegExpUtility.get_safe_reg_exp(ChineseDateTime.NumberCombinedWithUnit)
        self.unit_regex = RegExpUtility.get_safe_reg_exp(ChineseDateTime.UnitRegex)
        self.year_and_month_regex = RegExpUtility.get_safe_reg_exp(ChineseDateTime.YearAndMonth)
        self.pure_number_year_and_month_regex = RegExpUtility.get_safe_reg_exp(ChineseDateTime.PureNumYearAndMonth)
        self.year_to_year_regex = RegExpUtility.get_safe_reg_exp(ChineseDateTime.YearToYear)
        self.chinese_year_regex = RegExpUtility.get_safe_reg_exp(ChineseDateTime.DatePeriodYearInChineseRegex)
        self.season_with_year_regex = RegExpUtility.get_safe_reg_exp(ChineseDateTime.SeasonWithYear)

    def parse(self, source: ExtractResult, reference: datetime = None) -> Optional[DateTimeParseResult]:
        if not reference:
            reference = datetime.now()

        if source.type == self.parser_type_name:
            source_text = source.text.strip().lower()

            inner_result = self._parse_simple_cases(source_text, reference)
            if not inner_result.success:
                inner_result = self._parse_one_world_period(source_text, reference)

            if not inner_result.success:
                inner_result = self._merge_two_times_points(source_text, reference)

            if not inner_result.success:
                inner_result = self._parse_number_with_unit(source_text, reference)

            if not inner_result.success:
                inner_result = self._parse_duration(source_text, reference)

            if not inner_result.success:
                inner_result = self._parse_year_and_month(source_text, reference)

            if not inner_result.success:
                inner_result = self._parse_year_to_year(source_text, reference)

            if not inner_result.success:
                inner_result = self._parse_year(source_text, reference)

            if not inner_result.success:
                inner_result = self._parse_week_of_month(source_text, reference)

            if not inner_result.success:
                inner_result = self._parse_season(source_text, reference)

            if not inner_result.success:
                inner_result = self._parse_quarter(source_text, reference)

            if inner_result.success:
                if inner_result.future_value and inner_result.past_value:
                    inner_result.future_resolution = {
                        TimeTypeConstants.START_DATE: FormatUtil.format_date(inner_result.future_value[0]),
                        TimeTypeConstants.END_DATE: FormatUtil.format_date(inner_result.future_value[1])
                    }
                    inner_result.past_resolution = {
                        TimeTypeConstants.START_DATE: FormatUtil.format_date(inner_result.past_value[0]),
                        TimeTypeConstants.END_DATE: FormatUtil.format_date(inner_result.past_value[1])
                    }
                else:
                    inner_result.future_resolution = {}
                    inner_result.past_resolution = {}
                result_value = inner_result

        result = DateTimeParseResult(source)
        result.value = result_value
        result.timex_str = result_value.timex if result_value else ''
        result.resolution_str = ''

        return result

    def _parse_simple_cases(self, source: str, reference: datetime) -> DateTimeResolutionResult:
        result = DateTimeResolutionResult()
        year = reference.year
        month = reference.month
        no_year = False
        input_year = False

        match = regex.search(self.config.simple_cases_regex, source)

        if not match or match.start() != 0 or len(match.group()) != len(source):
            return result

        days = RegExpUtility.get_group_list(match, 'day')
        begin_day = self.config.day_of_month[days[0]]
        end_day = self.config.day_of_month[days[1]]

        month_str = RegExpUtility.get_group(match, 'month')

        if month_str.strip() != '':
            month = self.config.month_of_year[month_str]
        else:
            month_str = RegExpUtility.get_group(match, 'relmonth')
            month += self.config.get_swift_day_or_month(month_str)

            if month < 0:
                month = 0
                year -= 1
            elif month > 11:
                month = 11
                year += 1

        year_str = RegExpUtility.get_group(match, 'year')
        if year_str.strip() != '':
            year = int(year_str)
            input_year = True
        else:
            no_year = True

        begin_date_luis = FormatUtil.luis_date(year if input_year or self.config.is_future(month_str) else -1, month, begin_day)
        end_date_luis = FormatUtil.luis_date(year if input_year or self.config.is_future(month_str) else -1, month, end_day)

        future_year = year
        past_year = year

        start_date = DateUtils.safe_create_from_min_value(year, month, begin_day)

        if no_year and start_date < reference:
            future_year += 1

        if no_year and start_date >= reference:
            past_year -= 1

        result.timex = f'({begin_date_luis},{end_date_luis},P{end_day - begin_day}D)'

        result.future_value = [
            DateUtils.safe_create_from_min_value(future_year, month, begin_day),
            DateUtils.safe_create_from_min_value(future_year, month, end_day)
        ]
        result.past_value = [
            DateUtils.safe_create_from_min_value(past_year, month, begin_day),
            DateUtils.safe_create_from_min_value(past_year, month, end_day)
        ]

        result.success = True
        return result

    def _parse_number_with_unit(self, source: str, reference: datetime) -> DateTimeResolutionResult:
        result = DateTimeResolutionResult()

        # if there are NO spaces between number and unit
        match = regex.search(self.number_combined_with_unit_regex, source)
        if not match:
            return result

        source_unit = RegExpUtility.get_group(match, 'unit').strip().lower()
        if source_unit not in self.config.unit_map:
            return result

        num_str = RegExpUtility.get_group(match, 'num')
        before_str = source[:match.start()].strip().lower()

        return self.__parse_common_duration_with_unit(before_str, source_unit, num_str, reference)

    def _parse_duration(self, source: str, reference: datetime) -> DateTimeResolutionResult:
        result = DateTimeResolutionResult()

        # for case "前两年" "后三年"
        duration_result = next(iter(self.config.duration_extractor.extract(source, reference)), None)
        if not duration_result:
            return result

        match = regex.search(self.unit_regex, duration_result.text)
        if not match:
            return result

        source_unit = RegExpUtility.get_group(match, 'unit').strip().lower()
        if source_unit not in self.config.unit_map:
            return result

        before_str = source[:duration_result.start].strip().lower()
        number_str = duration_result.text[:match.start()].strip().lower()
        number_val = self.__convert_chinese_to_number(number_str)
        num_str = str(number_val)

        return self.__parse_common_duration_with_unit(before_str, source_unit, num_str, reference)

    def __parse_common_duration_with_unit(self, before: str, unit: str, num: str, reference: datetime) -> DateTimeResolutionResult:
        result = DateTimeResolutionResult()

        unit_str = self.config.unit_map[unit]

        past_match = regex.search(self.config.past_regex, before)
        has_past = past_match and len(past_match.group()) == len(before)

        future_match = regex.search(self.config.future_regex, before)
        has_future = future_match and len(future_match.group()) == len(before)

        if not has_future and not has_past:
            return result

        begin_date = reference
        end_date = reference
        difference = float(num)

        if unit_str == 'D':
            if has_past:
                begin_date += timedelta(days=-difference)
            if has_future:
                end_date += timedelta(days=difference)
        elif unit_str == 'W':
            if has_past:
                begin_date += timedelta(days=-7 * difference)
            if has_future:
                end_date += timedelta(days=7 * difference)
        elif unit_str == 'MON':
            if has_past:
                begin_date += datedelta(months=int(-difference))
            if has_future:
                end_date += datedelta(months=int(difference))
        elif unit_str == 'Y':
            if has_past:
                begin_date += datedelta(years=int(-difference))
            if has_future:
                end_date += datedelta(years=int(difference))
        else:
            return result

        if has_future:
            begin_date += timedelta(days=1)
            end_date += timedelta(days=1)

        begin_timex = FormatUtil.luis_date_from_datetime(begin_date)
        end_timex = FormatUtil.luis_date_from_datetime(end_date)

        result.timex = f'({begin_timex},{end_timex},P{num}{unit_str[0]})'

        result.future_value = [begin_date, end_date]
        result.past_value = [begin_date, end_date]

        result.success = True
        return result

    def __convert_chinese_to_number(self, source: str) -> int:
        num = -1
        er = next(iter(self.integer_extractor.extract(source)), None)

        if er and er.type == NumberConstants.SYS_NUM_INTEGER:
            num = int(self.number_parser.parse(er).value)

        return num

    def _parse_year_and_month(self, source: str, reference: datetime) -> DateTimeResolutionResult:
        result = DateTimeResolutionResult()

        match = regex.search(self.year_and_month_regex, source)

        if not match or len(match.group()) != len(source):
            match = regex.search(self.pure_number_year_and_month_regex, source)

        if not match or len(match.group()) != len(source):
            return result

        year = reference.year
        year_num = RegExpUtility.get_group(match, 'year')
        year_chinese = RegExpUtility.get_group(match, 'yearchs')
        year_relative = RegExpUtility.get_group(match, 'yearrel')

        if year_num.strip() != '':
            if self.config.is_year_only(year_num):
                year_num = year_num[:-1]
            year = self._convert_year(year_num, False)
        elif year_chinese.strip() != '':
            if self.config.is_year_only(year_chinese):
                year_chinese = year_chinese[:-1]
            year = self._convert_year(year_chinese, True)
        elif year_relative.strip() != '':
            year += self.config.get_swift_day_or_month(year_relative)

        if year < 100 and year >= 90:
            year += 1900
        elif year < 100 and year < 20:
            year += 2000

        month_str = RegExpUtility.get_group(match, 'month')
        month = self.config.month_of_year.get(month_str, 0) % 12

        begin_date = DateUtils.safe_create_from_min_value(year, month, 1)
        end_date = DateUtils.safe_create_from_min_value(year, month, 1) + datedelta(months=1)
        result.future_value = [begin_date, end_date]
        result.past_value = [begin_date, end_date]

        result.timex = f'{year:04d}-{month:02d}'

        result.success = True
        return result

    def _parse_year_to_year(self, source: str, reference: datetime) -> DateTimeResolutionResult:
        result = DateTimeResolutionResult()

        match = regex.search(self.year_to_year_regex, source)

        if not match:
            return result

        year_matches = list(regex.finditer(self.config.year_regex, source))
        chinese_year_matches = list(regex.finditer(self.chinese_year_regex, source))

        begin_year = 0
        end_year = 0

        if len(year_matches) == 2:
            begin_year = self.__convert_chinese_to_number(RegExpUtility.get_group(year_matches[0], 'year'))
            end_year = self.__convert_chinese_to_number(RegExpUtility.get_group(year_matches[1], 'year'))
        elif len(chinese_year_matches) == 2:
            begin_year = self.__convert_chinese_to_number(RegExpUtility.get_group(chinese_year_matches[0], 'yearchs'))
            end_year = self.__convert_chinese_to_number(RegExpUtility.get_group(chinese_year_matches[1], 'yearchs'))
        elif len(year_matches) == 1 and len(chinese_year_matches) == 1:
            if year_matches[0].start() < chinese_year_matches[0].start():
                begin_year = self.__convert_chinese_to_number(RegExpUtility.get_group(year_matches[0], 'year'))
                end_year = self.__convert_chinese_to_number(RegExpUtility.get_group(chinese_year_matches[0], 'yearchs'))
            else:
                begin_year = self.__convert_chinese_to_number(RegExpUtility.get_group(chinese_year_matches[0], 'yearchs'))
                end_year = self.__convert_chinese_to_number(RegExpUtility.get_group(year_matches[0], 'year'))

        begin_year = self.__sanitize_year(begin_year)
        end_year = self.__sanitize_year(end_year)

        begin_date = DateUtils.safe_create_from_min_value(begin_year, 1, 1)
        end_date = DateUtils.safe_create_from_min_value(end_year, 1, 1)
        result.future_value = [begin_date, end_date]
        result.past_value = [begin_date, end_date]

        begin_timex = FormatUtil.luis_date_from_datetime(begin_date)
        end_timex = FormatUtil.luis_date_from_datetime(end_date)
        result.timex = f'({begin_timex},{end_timex},P{end_year - begin_year}Y)'

        result.success = True
        return result

    def __sanitize_year(self, year: int) -> int:
        result = year
        if year < 100 and year >= 90:
            result += 1900
        elif year < 100 and year < 20:
            result += 2000
        return result

    def _parse_year(self, source: str, reference: datetime) -> DateTimeResolutionResult:
        source = source.strip().lower()
        result = DateTimeResolutionResult()
        is_chinese = False

        match = regex.search(self.config.year_regex, source)
        if not match or len(match.group()) != len(source):
            match = regex.search(self.year_in_chinese_regex, source)
            is_chinese = match and len(match.group()) == len(source)

        if not match or len(match.group()) != len(source):
            return result

        year_str = match.group()
        if self.config.is_year_only(year_str):
            year_str = year_str[:-1].strip()

        year = self._convert_year(year_str, is_chinese)
        if len(year_str) == 2:
            if year < 100 and year >= 30:
                year += 1900
            elif year < 30:
                year += 2000

        begin_day = DateUtils.safe_create_from_min_value(year, 1, 1)
        end_day = DateUtils.safe_create_from_min_value(year + 1, 1, 1)

        result.timex = f'{year:04d}'
        result.future_value = [begin_day, end_day]
        result.past_value = [begin_day, end_day]

        result.success = True
        return result

    def _convert_year(self, year_str: str, is_chinese: bool) -> int:
        year = -1
        if is_chinese:
            year_num = 0
            er = next(iter(self.integer_extractor.extract(year_str)), None)
            if er and er.type == NumberConstants.SYS_NUM_INTEGER:
                year_num = int(self.number_parser.parse(er).value)

            if year_num < 10:
                year_num = 0
                for char in year_str:
                    year_num *= 10
                    er = next(iter(self.integer_extractor.extract(char)), None)
                    if er and er.type == NumberConstants.SYS_NUM_INTEGER:
                        year_num += int(self.number_parser.parse(er).value)
            else:
                year = year_num
        else:
            year = int(year_str)

        return -1 if year == 0 else year

    def _get_week_of_month(self, cardinal, month, year, reference, no_year) -> DateTimeResolutionResult:
        result = DateTimeResolutionResult()
        seed_date = self._compute_date(cardinal, 1, month, year)

        future_date = seed_date
        past_date = seed_date

        if no_year and future_date < reference:
            future_date = self._compute_date(cardinal, 1, month, year + 1)
            if not future_date.month == month:
                future_date = future_date + timedelta(days=-7)

        if no_year and past_date >= reference:
            past_date = self._compute_date(cardinal, 1, month, year - 1)
            if not past_date.month == month:
                past_date = past_date + timedelta(days=-7)

        result.timex = ('XXXX' if no_year else f'{year:04d}') + f'-{month:02d}-W{cardinal:02d}'

        days_to_add = 6 if self._inclusive_end_period else 7
        result.future_value = [future_date, future_date + timedelta(days=days_to_add)]
        result.past_value = [past_date, past_date + timedelta(days=days_to_add)]

        result.success = True
        return result

    def _compute_date(self, cardinal: int, weekday: int, month: int, year: int):
        first_day = datetime(year, month, 1)
        first_week_day = DateUtils.this(first_day, weekday)

        if weekday == 0:
            weekday = 7

        first_day_of_week = first_day.isoweekday()

        if first_day_of_week == 7:
            first_day_of_week = 0

        if weekday < first_day_of_week:
            first_week_day = DateUtils.next(first_day, weekday)

        first_week_day = first_week_day + timedelta(days=7 * (cardinal - 1))

        return first_week_day

    def _parse_season(self, source: str, reference: datetime) -> DateTimeResolutionResult:
        result = DateTimeResolutionResult()

        match = regex.search(self.season_with_year_regex, source)

        if not match or len(match.group()) != len(source):
            return result

        year = reference.year
        year_num = RegExpUtility.get_group(match, 'year')
        year_chinese = RegExpUtility.get_group(match, 'yearchs')
        year_relative = RegExpUtility.get_group(match, 'yearrel')
        has_year = False

        if year_num.strip() != '':
            has_year = True
            if self.config.is_year_only(year_num):
                year_num = year_num[:-1]
            year = self._convert_year(year_num, False)
        elif year_chinese.strip() != '':
            has_year = True
            if self.config.is_year_only(year_chinese):
                year_chinese = year_chinese[:-1]
            year = self._convert_year(year_chinese, True)
        elif year_relative.strip() != '':
            has_year = True
            year += self.config.get_swift_day_or_month(year_relative)

        if year < 100 and year >= 90:
            year += 1900
        elif year < 100 and year < 20:
            year += 2000

        season_str = RegExpUtility.get_group(match, 'season')
        season = self.config.season_map.get(season_str, None)

        if has_year:
            result.timex = f'{year:02d}-{season}'

        result.success = True
        return result

    def _parse_quarter(self, source: str, reference: datetime) -> DateTimeResolutionResult:
        result = DateTimeResolutionResult()

        match = regex.search(self.config.quarter_regex, source)

        if not match or len(match.group()) != len(source):
            return result

        year = reference.year
        year_num = RegExpUtility.get_group(match, 'year')
        year_chinese = RegExpUtility.get_group(match, 'yearchs')
        year_relative = RegExpUtility.get_group(match, 'yearrel')
        has_year = False

        if year_num.strip() != '':
            has_year = True
            if self.config.is_year_only(year_num):
                year_num = year_num[:-1]
            year = self._convert_year(year_num, False)
        elif year_chinese.strip() != '':
            has_year = True
            if self.config.is_year_only(year_chinese):
                year_chinese = year_chinese[:-1]
            year = self._convert_year(year_chinese, True)
        elif year_relative.strip() != '':
            has_year = True
            year += self.config.get_swift_day_or_month(year_relative)

        if year < 100 and year >= 90:
            year += 1900
        elif year < 100 and year < 20:
            year += 2000

        cardinal_str = RegExpUtility.get_group(match, 'cardinal')
        quarter_num = self.config.cardinal_map.get(cardinal_str, None)

        begin_date = DateUtils.safe_create_from_min_value(year, quarter_num * 3 - 2, 1)
        end_date = DateUtils.safe_create_from_min_value(year, quarter_num * 3 + 1, 1)
        result.future_value = [begin_date, end_date]
        result.past_value = [begin_date, end_date]

        begin_luis = FormatUtil.luis_date_from_datetime(begin_date)
        end_luis = FormatUtil.luis_date_from_datetime(end_date)
        result.timex = f'({begin_luis},{end_luis},P3M)'

        result.success = True
        return result
