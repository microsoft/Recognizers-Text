#  Copyright (c) Microsoft Corporation. All rights reserved.
#  Licensed under the MIT License.

from typing import Optional
from datetime import datetime, timedelta
import regex

from recognizers_text import RegExpUtility
from recognizers_number import ExtractResult, ChineseCardinalExtractor,\
    CJKNumberParser, ChineseNumberParserConfiguration

from ...resources.chinese_date_time import ChineseDateTime
from ..constants import TimeTypeConstants, Constants
from ..utilities import DateTimeFormatUtil, DateTimeResolutionResult, DateUtils
from ..parsers import DateTimeParseResult
from ..base_datetimeperiod import BaseDateTimePeriodParser, BeginEnd
from .datetimeperiod_parser_config import ChineseDateTimePeriodParserConfiguration
from .date_extractor import ChineseDateExtractor
from .timeperiod_extractor import ChineseTimePeriodExtractor


class ChineseDateTimePeriodParser(BaseDateTimePeriodParser):
    def __init__(self):
        super().__init__(ChineseDateTimePeriodParserConfiguration())
        self.tmo_regex = RegExpUtility.get_safe_reg_exp(
            ChineseDateTime.DateTimePeriodMORegex)
        self.tmi_regex = RegExpUtility.get_safe_reg_exp(
            ChineseDateTime.DateTimePeriodMIRegex)
        self.taf_regex = RegExpUtility.get_safe_reg_exp(
            ChineseDateTime.DateTimePeriodAFRegex)
        self.tev_regex = RegExpUtility.get_safe_reg_exp(
            ChineseDateTime.DateTimePeriodEVRegex)
        self.tni_regex = RegExpUtility.get_safe_reg_exp(
            ChineseDateTime.DateTimePeriodNIRegex)
        self.unit_regex = RegExpUtility.get_safe_reg_exp(
            ChineseDateTime.DateTimePeriodUnitRegex)
        self.time_of_day_regex = RegExpUtility.get_safe_reg_exp(
            ChineseDateTime.TimeOfDayRegex)
        self.single_date_extractor = ChineseDateExtractor()
        self.time_period_extractor = ChineseTimePeriodExtractor()
        self.cardinal_extractor = ChineseCardinalExtractor()
        self.cardinal_parser = CJKNumberParser(
            ChineseNumberParserConfiguration())

    def parse(self, source: ExtractResult, reference: datetime = None) -> Optional[DateTimeParseResult]:
        if reference is None:
            reference = datetime.now()

        result = DateTimeParseResult(source)

        if source.type is self.parser_type_name:
            source_text = source.text.strip().lower()

            inner_result = self.merge_date_and_time_periods(
                source_text, reference)

            if not inner_result.success:
                inner_result = self.merge_two_time_points(
                    source_text, reference)

            if not inner_result.success:
                inner_result = self.parse_specific_time_of_day(
                    source_text, reference)

            if not inner_result.success:
                inner_result = self._parse_number_with_unit(
                    source_text, reference)

            if inner_result.success:
                inner_result.future_resolution[TimeTypeConstants.START_DATETIME] = DateTimeFormatUtil.format_date_time(
                    inner_result.future_value[0])
                inner_result.future_resolution[TimeTypeConstants.END_DATETIME] = DateTimeFormatUtil.format_date_time(
                    inner_result.future_value[1])
                inner_result.past_resolution[TimeTypeConstants.START_DATETIME] = DateTimeFormatUtil.format_date_time(
                    inner_result.past_value[0])
                inner_result.past_resolution[TimeTypeConstants.END_DATETIME] = DateTimeFormatUtil.format_date_time(
                    inner_result.past_value[1])
                result.value = inner_result
                result.timex_str = inner_result.timex if inner_result is not None else ''
                result.resolution_str = ''

        return result

    def merge_date_and_time_periods(self, trimmed_source: str, reference: datetime) -> DateTimeResolutionResult:
        result = DateTimeResolutionResult()

        extracted_result1 = self.single_date_extractor.extract(trimmed_source, reference)
        extracted_result2 = self.time_period_extractor.extract(trimmed_source, reference)
        if len(extracted_result1) != 1 or len(extracted_result2) != 1:
            return result

        parsed_result1 = self.config.date_parser.parse(extracted_result1[0], reference)
        parsed_result2 = self.config.time_period_parser.parse(extracted_result2[0], reference)
        time_range = tuple(parsed_result2.value.future_value)
        begin_time = time_range[0]
        end_time = time_range[1]
        future_date = parsed_result1.value.future_value
        past_date = parsed_result1.value.past_value

        result.future_value = (
            DateUtils.safe_create_from_min_value(
                future_date.year, future_date.month, future_date.day,
                begin_time.hour, begin_time.minute, begin_time.second),
            DateUtils.safe_create_from_min_value(
                future_date.year, future_date.month, future_date.day,
                end_time.hour, end_time.minute, end_time.second)
        )

        result.past_value = (
            DateUtils.safe_create_from_min_value(
                past_date.year, past_date.month, past_date.day,
                begin_time.hour, begin_time.minute, begin_time.second),
            DateUtils.safe_create_from_min_value(
                past_date.year, past_date.month, past_date.day,
                end_time.hour, end_time.minute, end_time.second)
        )

        split = parsed_result2.timex_str.split('T')
        if len(split) != 4:
            return result

        date_str = parsed_result1.timex_str

        result.timex = split[0] + date_str + 'T' + split[1] + date_str + 'T' + split[2] + 'T' + split[3]
        result.success = True
        return result

    def merge_two_time_points(self, source: str, reference: datetime) -> DateTimeResolutionResult:
        result = DateTimeResolutionResult()

        prs: BeginEnd = None
        time_ers = self.config.time_extractor.extract(source, reference)
        datetime_ers = self.config.date_time_extractor.extract(
            source, reference)

        both_has_date = False
        begin_has_date = False
        end_has_date = False

        if len(datetime_ers) == 2:
            prs = self.get_two_points(
                datetime_ers[0], datetime_ers[1], self.config.date_time_parser, self.config.date_time_parser, reference)
            both_has_date = True
        elif len(datetime_ers) == 1 and len(time_ers) == 2:
            if datetime_ers[0].overlap(time_ers[0]):
                prs = self.get_two_points(
                    datetime_ers[0], time_ers[1], self.config.date_time_parser, self.config.time_parser, reference)
                begin_has_date = True
            else:
                prs = self.get_two_points(
                    time_ers[0], datetime_ers[0], self.config.time_parser, self.config.date_time_parser, reference)
                end_has_date = True
        elif len(datetime_ers) == 1 and len(time_ers) == 1:
            if time_ers[0].start < datetime_ers[0].start:
                prs = self.get_two_points(
                    time_ers[0], datetime_ers[0], self.config.time_parser, self.config.date_time_parser, reference)
                end_has_date = True
            else:
                prs = self.get_two_points(
                    datetime_ers[0], time_ers[0], self.config.date_time_parser, self.config.time_parser, reference)
                begin_has_date = True

        if prs is None or not prs.begin.value or not prs.end.value:
            return result

        begin: DateTimeResolutionResult = prs.begin.value
        end: DateTimeResolutionResult = prs.end.value

        future_begin: datetime = begin.future_value
        future_end: datetime = end.future_value
        past_begin: datetime = begin.past_value
        past_end: datetime = end.past_value

        if future_begin > future_end:
            future_begin = past_begin

        if past_end < past_begin:
            past_end = future_end

        right_time = DateUtils.safe_create_from_min_value_date_time(reference)
        left_time = DateUtils.safe_create_from_min_value_date_time(reference)

        if both_has_date:
            right_time = DateUtils.safe_create_from_min_value_date_time(
                future_end)
            left_time = DateUtils.safe_create_from_min_value_date_time(
                future_begin)
        elif begin_has_date:
            # TODO: Handle "明天下午两点到五点"
            future_end = self.get_datetime(future_begin, future_end)
            past_end = self.get_datetime(past_begin, past_end)
            left_time = DateUtils.safe_create_from_min_value_date_time(
                future_begin)
        elif end_has_date:
            # TODO: Handle "明天下午两点到五点"
            future_begin = self.get_datetime(future_end, future_begin)
            past_begin = self.get_datetime(past_end, past_begin)
            right_time = DateUtils.safe_create_from_min_value_date_time(
                future_end)

        left: DateTimeResolutionResult = prs.begin.value
        right: DateTimeResolutionResult = prs.end.value
        left_result_time: datetime = left.future_value
        right_result_time: datetime = right.future_value

        left_time += timedelta(hours=left_result_time.hour,
                               minutes=left_result_time.minute, seconds=left_result_time.second)
        right_time += timedelta(hours=right_result_time.hour,
                                minutes=right_result_time.minute, seconds=right_result_time.second)

        # the right side time contains "ampm", while the left side doesn't
        if right.comment == 'ampm' and not left.comment and right_time < left_time:
            right_time += timedelta(hours=12)

        if right_time < left_time:
            right_time += timedelta(days=1)

        result.future_value = [left_time, right_time]
        result.past_value = [left_time, right_time]

        fuzzy_timex = 'X' in prs.begin.timex_str or 'X' in prs.end.timex_str
        left_timex = prs.begin.timex_str if fuzzy_timex else DateTimeFormatUtil.luis_date_time(
            left_time)
        right_timex = prs.end.timex_str if fuzzy_timex else DateTimeFormatUtil.luis_date_time(
            right_time)
        total_hours = DateUtils.total_hours(left_time, right_time)
        result.timex = f'({left_timex},{right_timex},PT{total_hours}H)'

        result.success = True
        return result

    def parse_specific_time_of_day(self, source: str, reference: datetime) -> DateTimeResolutionResult:
        result = DateTimeResolutionResult()
        trimmed_source = source.strip()
        begin_hour = end_hour = end_min = 0

        # Handle 昨晚，今晨
        if RegExpUtility.is_exact_match(self.config.specific_time_of_day_regex, trimmed_source, True):
            values = self.config.get_matched_time_range(source)
            if not values:
                return result

            swift = values.swift
            date = reference.date() + timedelta(days=swift)
            day = date.day
            month = date.month
            year = date.year

            result.timex = DateTimeFormatUtil.format_date(date) + values.time_str
            result.future_value = result.past_value = [
                DateUtils.safe_create_from_min_value(
                    year, month, day, values.begin_hour, 0, 0),
                DateUtils.safe_create_from_min_value(
                    year, month, day, values.end_hour, values.end_min, values.end_min)
            ]

            result.success = True
            return result

        # handle morning, afternoon..
        if regex.search(self.tmo_regex, source):
            time_str = 'TMO'
            begin_hour = 8
            end_hour = 12
        elif regex.search(self.tmi_regex, source):
            time_str = 'TMI'
            begin_hour = 11
            end_hour = 13
        elif regex.search(self.taf_regex, source):
            time_str = 'TAF'
            begin_hour = Constants.HALF_DAY_HOUR_COUNT
            end_hour = 16
        elif regex.search(self.tev_regex, source):
            time_str = 'TEV'
            begin_hour = 16
            end_hour = 20
        elif regex.search(self.tni_regex, source):
            time_str = 'TNI'
            begin_hour = 20
            end_hour = 23
            end_min = 59
        else:
            return result

        if RegExpUtility.is_exact_match(self.config.specific_time_of_day_regex, trimmed_source, True):
            swift = 0
            if regex.search(self.config.next_regex, trimmed_source):
                swift = 1
            elif regex.search(self.config.last_regex, trimmed_source):
                swift = -1

            date = reference.date() + timedelta(days=swift)
            day = date.day
            month = date.month
            year = date.year

            result.timex = DateTimeFormatUtil.format_date(date) + time_str
            result.future_value = result.past_value = [
                DateUtils.safe_create_from_min_value(
                    year, month, day, begin_hour, 0, 0),
                DateUtils.safe_create_from_min_value(
                    year, month, day, end_hour, end_min, end_min)
            ]

            result.success = True
            return result

        # handle Date followed by morning, afternoon
        match = regex.search(self.config.time_of_day_regex, trimmed_source)
        if match:
            before_str = trimmed_source[0:match.start()].strip()
            extracted_results = self.single_date_extractor.extract(before_str, reference)

            if len(extracted_results) == 0 or extracted_results[0].length != len(before_str):
                return result

            parse_result = self.config.date_parser.parse(extracted_results[0], reference)
            future_date = parse_result.value.future_value
            past_date = parse_result.value.past_value

            result.timex = parse_result.timex_str + time_str

            result.future_value = (
                DateUtils.safe_create_from_min_value(future_date.year, future_date.month, future_date.day,
                                                     begin_hour, 0, 0),
                DateUtils.safe_create_from_min_value(future_date.year, future_date.month, future_date.day,
                                                     end_hour, end_min, end_min)
            )

            result.past_value = (
                DateUtils.safe_create_from_min_value(past_date.year, past_date.month, past_date.day,
                                                     begin_hour, 0, 0),
                DateUtils.safe_create_from_min_value(past_date.year, past_date.month, past_date.day,
                                                     end_hour, end_min, end_min)
            )

            result.success = True
            return result

        return result

    def _parse_number_with_unit(self, source: str, reference: datetime) -> DateTimeResolutionResult:
        result = DateTimeResolutionResult()
        ers = self.cardinal_extractor.extract(source)

        if len(ers) == 1:
            er = ers[0]
            pr = self.cardinal_parser.parse(er)
            source_unit: str = source[er.start + er.length:].strip().lower()

            if source_unit.startswith('个'):
                source_unit = source_unit[1:]

            before_str = source[:er.start].strip().lower()

            return self.__parse_common_duration_with_unit(before_str, source_unit,
                                                          pr.resolution_str, float(pr.value), reference)

        # handle "last hour"
        match = regex.search(self.unit_regex, source)

        if match:
            source_unit = RegExpUtility.get_group(match, 'unit')
            before_str = source[:match.start()].strip().lower()

            return self.__parse_common_duration_with_unit(before_str, source_unit, '1', 1, reference)

        return result

    def __parse_common_duration_with_unit(self, before: str, unit: str, num: str, swift: float, reference: datetime)\
            -> DateTimeResolutionResult:
        result = DateTimeResolutionResult()

        if unit not in self.config.unit_map:
            return result

        unit_str = self.config.unit_map[unit]

        past_match = regex.search(self.config.past_regex, before)
        has_past = past_match and len(past_match.group()) == len(before)

        future_match = regex.search(self.config.future_regex, before)
        has_future = future_match and len(future_match.group()) == len(before)

        if not has_future and not has_past:
            return result

        begin_date = reference
        end_date = reference

        if unit_str == 'H':
            if has_past:
                begin_date += timedelta(hours=-swift)
            if has_future:
                end_date += timedelta(hours=swift)
        elif unit_str == 'M':
            if has_past:
                begin_date += timedelta(minutes=-swift)
            if has_future:
                end_date += timedelta(minutes=swift)
        elif unit_str == 'S':
            if has_past:
                begin_date += timedelta(seconds=-swift)
            if has_future:
                end_date += timedelta(seconds=swift)
        else:
            return result

        begin_timex = DateTimeFormatUtil.luis_date_from_datetime(
            begin_date) + 'T' + DateTimeFormatUtil.luis_time_from_datetime(begin_date)
        end_timex = DateTimeFormatUtil.luis_date_from_datetime(
            end_date) + 'T' + DateTimeFormatUtil.luis_time_from_datetime(end_date)

        result.timex = f'({begin_timex},{end_timex},PT{num}{unit_str[0]})'

        result.future_value = [begin_date, end_date]
        result.past_value = [begin_date, end_date]

        result.success = True
        return result
