from datetime import datetime

from recognizers_text.utilities import RegExpUtility
from recognizers_date_time.resources.arabic_date_time import ArabicDateTime
from recognizers_date_time.date_time.base_time import BaseTimeParser
from recognizers_date_time.date_time.utilities import DateTimeResolutionResult, DateTimeFormatUtil, DateUtils


class ArabicTimeParser(BaseTimeParser):
    def internal_parser(self, source: str, reference: datetime) -> DateTimeResolutionResult:
        result = super().internal_parser(source, reference)
        if not result.success:
            result = self.parse_ish(source, reference)

        return result

    def parse_ish(self, source: str, reference: datetime) -> DateTimeResolutionResult:
        result = DateTimeResolutionResult()
        trimmed_source = source.strip().lower()

        match = RegExpUtility.get_safe_reg_exp(
            ArabicDateTime.IshRegex).match(source)
        if match and match.end() == len(trimmed_source):
            hour_str = RegExpUtility.get_group(match, 'hour')
            hour = 12
            if hour_str:
                hour = int(hour_str)

            result.timex = 'T' + DateTimeFormatUtil.to_str(hour, 2)
            result.future_value = result.past_value = DateUtils.safe_create_from_min_value(
                reference.year, reference.month, reference.day, hour, 0, 0)
            result.success = True

        return result
