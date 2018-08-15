from datetime import datetime
import regex

from recognizers_text.utilities import RegExpUtility
from ..utilities import DateTimeResolutionResult
from ..base_time import BaseTimeParser
from .time_parser_config import EnglishTimeParserConfiguration

class EnglishTimeParser(BaseTimeParser):
    def __init__(self, config: EnglishTimeParserConfiguration):
        super().__init__(config)

    def internal_parser(self, source: str, reference: datetime) -> DateTimeResolutionResult:
        inner_result = self.parse_basic_regex_match(source, reference)
        if not inner_result.success:
            inner_result = self.parse_ish(source, reference)
        return inner_result

    def parse_ish(self, source: str, reference: datetime) -> DateTimeResolutionResult:
        result = DateTimeResolutionResult()
        source = source.strip().lower()
        match = regex.search(self.config.ish_regex, source)
        if match is None or match.group() != source:
            return result

        hour_str = RegExpUtility.get_group(match, 'hour')
        hour = 12
        if hour_str:
            hour = int(hour_str)

        result.timex = f'T{hour:02d}'
        result.future_value = datetime(reference.year, reference.month, reference.day, hour, 0, 0)
        result.past_value = result.future_value
        result.success = True
        return result
