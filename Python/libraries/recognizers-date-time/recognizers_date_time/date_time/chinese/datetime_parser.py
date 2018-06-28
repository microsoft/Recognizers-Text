from typing import Optional
from datetime import datetime, timedelta

from recognizers_text.extractor import ExtractResult
from ..base_datetime import BaseDateTimeParser
from ..parsers import DateTimeParseResult
from ..constants import TimeTypeConstants
from ..utilities import FormatUtil, DateTimeResolutionResult, DateUtils
from .datetime_parser_config import ChineseDateTimeParserConfiguration

class ChineseDateTimeParser(BaseDateTimeParser):
    def __init__(self):
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

            if inner_result.success:
                inner_result.future_resolution = {TimeTypeConstants.DATETIME: FormatUtil.format_date_time(inner_result.future_value)}
                inner_result.past_resolution = {TimeTypeConstants.DATETIME: FormatUtil.format_date_time(inner_result.past_value)}
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

        time_str = 'T' + FormatUtil.to_str(hour, 2) + time_str[3:]
        ret.timex = pr1.timex_str + time_str

        val = pr2.value
        if hour <= 12 and not self.config.pm_time_regex.search(source) and not self.config.am_time_regex.search(source) and val.comment:
            ret.comment = 'ampm'

        ret.future_value = datetime(future_date.year, future_date.month, future_date.day, hour, minute, second)
        ret.past_value = datetime(past_date.year, past_date.month, past_date.day, hour, minute, second)
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

            time_str = 'T' + FormatUtil.to_str(hour, 2) + time_str[3:]

            ret.timex = FormatUtil.format_date(date) + time_str
            ret.future_value = datetime(date.year, date.month, date.day, hour, minute, second)
            ret.past_value = ret.future_value
            ret.success = True
            return ret

        return ret
