from typing import Optional
from datetime import datetime, timedelta
from .date_extractor import ChineseDateExtractor
from .date_parser import ChineseDateParser
from .datetime_extractor import ChineseDateTimeExtractor
from .duration_extractor import ChineseDurationExtractor
from ..base_datetime import BaseDateTimeParser
from ..parsers import DateTimeParseResult
from ..constants import TimeTypeConstants, Constants
from recognizers_number import Constants as NumberConstants
from recognizers_text import RegExpUtility, ExtractResult
from ..utilities import DateTimeFormatUtil, DateTimeResolutionResult, DateUtils
from .datetime_parser_config import ChineseDateTimeParserConfiguration


class ChineseDateTimeParser(BaseDateTimeParser):
    def __init__(self):
        self.duration_extractor = ChineseDurationExtractor()
        config = ChineseDateTimeParserConfiguration()
        BaseDateTimeParser.__init__(self, config)

    def parse(self, source: ExtractResult, reference: datetime = None) -> Optional[DateTimeParseResult]:
        if reference is None:
            reference = datetime.now()

        value = None
        if source.type is self.parser_type_name:
            inner_result = self._merge_date_and_time(source.text, reference)
            if not inner_result.success:
                inner_result = self.parse_basic_regex(source.text, reference)

            if not inner_result.success:
                inner_result = self._parse_time_of_today(source.text, reference)

            if not inner_result.success:
                inner_result = self._parser_duration_with_ago_and_later(source.text, reference)

            if inner_result.success:
                inner_result.future_resolution = {
                    TimeTypeConstants.DATETIME: DateTimeFormatUtil.format_date_time(inner_result.future_value)}
                inner_result.past_resolution = {
                    TimeTypeConstants.DATETIME: DateTimeFormatUtil.format_date_time(inner_result.past_value)}
                value = inner_result

        ret = DateTimeParseResult(source)
        ret.value = value
        ret.timex_str = value.timex if value else ''
        ret.resolution_str = ''

        return ret

    def _merge_date_and_time(self, source: str, reference: datetime) -> DateTimeResolutionResult:
        ret = DateTimeResolutionResult()

        er1 = self.config.date_extractor.extract(source, reference)
        if not er1:
            return ret

        er2 = self.config.time_extractor.extract(source, reference)
        if not er2:
            return ret

        pr1 = self.config.date_parser.parse(er1[0], reference)
        pr2 = self.config.time_parser.parse(er2[0], reference)
        if pr1.value is None or pr2.value is None:
            return ret

        future_date = pr1.value.future_value
        past_date = pr1.value.past_value
        time = pr2.value.future_value

        hour = time.hour
        minute = time.minute
        second = time.second

        # handle morning, afternoon
        if self.config.pm_time_regex.search(source) and hour < 12:
            hour += 12
        elif self.config.am_time_regex.search(source) and hour >= 12:
            hour -= 12

        time_str = pr2.timex_str
        if time_str.endswith('ampm'):
            time_str = time_str[0:len(time_str)-4]

        time_str = 'T' + DateTimeFormatUtil.to_str(hour, 2) + time_str[3:]
        ret.timex = pr1.timex_str + time_str

        val = pr2.value
        if hour <= 12 and not self.config.pm_time_regex.search(source) and not self.config.am_time_regex.search(source) and val.comment:
            ret.comment = 'ampm'

        ret.future_value = datetime(
            future_date.year, future_date.month, future_date.day, hour, minute, second)
        ret.past_value = datetime(
            past_date.year, past_date.month, past_date.day, hour, minute, second)
        ret.success = True

        return ret

    def _parse_time_of_today(self, source: str, reference: datetime) -> DateTimeResolutionResult:
        ret = DateTimeResolutionResult()

        ers = self.config.time_extractor.extract(source, reference)
        if not len(ers) == 1:
            return ret

        pr = self.config.time_parser.parse(ers[0], reference)
        if pr.value is None:
            return ret

        time = pr.value.future_value

        hour = time.hour
        minute = time.minute
        second = time.second
        time_str = pr.timex_str

        matches = list(self.config.specific_time_of_day_regex.finditer(source))
        if matches:
            match = matches[-1]
            match_str = match.group().lower()

            swift = self.config.get_swift_day(match_str)
            date = reference + timedelta(days=swift)

            hour = self.config.get_hour(match_str, hour)

            # in this situation, luis_str cannot end up with 'ampm', because we always have 'morning' or 'night'
            if time_str.endswith('ampm'):
                time_str = time_str[0, len(time_str) - 4]

            time_str = 'T' + DateTimeFormatUtil.to_str(hour, 2) + time_str[3:]

            ret.timex = DateTimeFormatUtil.format_date(date) + time_str
            ret.future_value = datetime(
                date.year, date.month, date.day, hour, minute, second)
            ret.past_value = ret.future_value
            ret.success = True
            return ret

        return ret

    # Handle cases like "5分钟前" "一小时后"
    def _parser_duration_with_ago_and_later(self, source: str, reference: datetime) -> DateTimeResolutionResult:
        result = DateTimeResolutionResult()
        duration_res = self.duration_extractor.extract(source, reference).pop()

        if duration_res:
            match = ChineseDateTimeExtractor.date_time_period_unit_regex.search(source)
            if match:
                suffix = source[duration_res.start + duration_res.length:]
                src_unit = RegExpUtility.get_group(match, 'unit')

                number_str = source[duration_res.start:match.lastindex - duration_res.start + 1]
                number = ChineseDateParser.parse_chinese_written_number_to_value(ChineseDateParser(), number_str)

                if src_unit in self.config.unit_map:
                    unit_str = self.config.unit_map.get(src_unit)

                    before_match = RegExpUtility.get_matches(ChineseDateExtractor.before_regex, suffix)
                    if before_match and suffix.startswith(before_match[0]):
                        if unit_str == Constants.TIMEX_HOUR:
                            date = reference + timedelta(hours=-number)
                        elif unit_str == Constants.TIMEX_MINUTE:
                            date = reference + timedelta(minutes=-number)
                        elif unit_str == Constants.TIMEX_SECOND:
                            date = reference + timedelta(seconds=-number)
                        else:
                            return result

                        result.timex = DateTimeFormatUtil.luis_date_from_datetime(date)
                        result.future_value = result.past_value = date
                        result.success = True
                        return result

                    after_match = RegExpUtility.get_matches(ChineseDateExtractor.after_regex, suffix)
                    if after_match and suffix.startswith(after_match[0]):
                        if unit_str == Constants.TIMEX_HOUR:
                            date = reference + timedelta(hours=number)
                        elif unit_str == Constants.TIMEX_MINUTE:
                            date = reference + timedelta(minutes=number)
                        elif unit_str == Constants.TIMEX_SECOND:
                            date = reference + timedelta(seconds=number)
                        else:
                            return result

                        result.timex = DateTimeFormatUtil.luis_date_from_datetime(date)
                        result.future_value = result.past_value = date
                        result.success = True
                        return result

        return result

    # # convert Chinese Number to Integer
    # def parse_chinese_written_number_to_value(self, source: str) -> int:
    #     num = -1
    #     er: ExtractResult = next(
    #         iter(ChineseDateParser.integer_extractor.extract(source)), None)
    #     if er and er.type == NumberConstants.SYS_NUM_INTEGER:
    #         num = int(self.config.number_parser.parse(er).value)
    #
    #     return num
