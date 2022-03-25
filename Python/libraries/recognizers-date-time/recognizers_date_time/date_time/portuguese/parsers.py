#  Copyright (c) Microsoft Corporation. All rights reserved.
#  Licensed under the MIT License.

from datetime import datetime
import regex

from recognizers_text.utilities import RegExpUtility
from ..utilities import DateTimeResolutionResult
from ..base_datetimeperiod import BaseDateTimePeriodParser
from ..base_time import BaseTimeParser
from ...resources import PortugueseDateTime
from .datetimeperiod_parser_config import PortugueseDateTimePeriodParserConfiguration


class PortugueseTimeParser(BaseTimeParser):
    def __init__(self, config: PortugueseDateTimePeriodParserConfiguration):
        BaseDateTimePeriodParser.__init__(self, config)

    def parse_specific_time_of_day(self, source: str, reference: datetime) -> DateTimeResolutionResult:
        result = DateTimeResolutionResult()
        trimmed_text = source.strip().lower()

        # Handle morning, afternoon..
        values = self.config.get_matched_time_range(trimmed_text)
        if not values.success:
            return result

        match = self.config.specific_time_of_day_regex.match(trimmed_text)

        if match and len(match.group()) == len(trimmed_text):
            swift = self.config.get_swift_prefix(trimmed_text)

            date = reference + timedelta(days=swift)
            date = date.replace(hour=0, minute=0, second=0)
            day = date.day
            month = date.month
            year = date.year

            result.timex = DateTimeFormatUtil.format_date(date) + values.time_str

            result.past_value = [
                DateUtils.safe_create_from_value(
                    DateUtils.min_value, year, month, day, values.begin_hour, 0, 0),
                DateUtils.safe_create_from_value(
                    DateUtils.min_value, year, month, day, values.end_hour, values.end_min, values.end_min)
            ]
            result.future_value = result.past_value

            result.success = True
            return result

        start_index = len(PortugueseDateTime.Tomorrow) if trimmed_text.startswith(
            PortugueseDateTime.Tomorrow) else 0

        # handle Date followed by morning, afternoon
        # Add handling code to handle morning, afternoon followed by Date
        # Add handling code to handle early/late morning, afternoon.
        # TODO: use regex from config: match = this.config.TimeOfDayRegex.Match(trimedText.Substring(startIndex));
        matches = list(RegExpUtility.get_safe_reg_exp(
            PortugueseDateTime.TimeOfDayRegex).finditer(trimmed_text[start_index:]))
        if matches:
            match = matches[0]
            before_str = trimmed_text[0:match.start() + match.end()].strip()
            ers = self.config.date_extractor.extract(before_str, reference)

            if not ers:
                return result

            pr = self.config.date_parser.parse(ers[0], reference)

            future_date = pr.value.future_value
            past_date = pr.value.past_value

            result.timex = pr.timex_str + values.time_str

            result.future_value = [
                DateUtils.safe_create_from_value(
                    DateUtils.min_value, future_date.year, future_date.month, future_date.day, values.begin_hour, 0, 0),
                DateUtils.safe_create_from_value(
                    DateUtils.min_value, future_date.year, future_date.month, future_date.day, values.end_hour,
                    values.end_min, values.end_min)
            ]
            result.past_value = [
                DateUtils.safe_create_from_value(
                    DateUtils.min_value, past_date.year, past_date.month, past_date.day, values.begin_hour, 0, 0),
                DateUtils.safe_create_from_value(
                    DateUtils.min_value, past_date.year, past_date.month, past_date.day, values.end_hour,
                    values.end_min, values.end_min)
            ]

            result.success = True
            return result

        return result
