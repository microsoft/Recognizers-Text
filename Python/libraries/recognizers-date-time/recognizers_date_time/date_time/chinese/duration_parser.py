from typing import Optional
from datetime import datetime

from recognizers_number_with_unit import NumberWithUnitParser

from ..constants import TimeTypeConstants
from ..utilities import DateTimeResolutionResult
from ..extractors import ExtractResult
from ..parsers import DateTimeParseResult
from ..base_duration import BaseDurationParser
from .duration_parser_config import ChineseDurationParserConfiguration, ChineseDurationNumberWithUnitParserConfiguration

class ChineseDurationParser(BaseDurationParser):
    def __init__(self):
        super().__init__(ChineseDurationParserConfiguration())
        self._internal_parser = NumberWithUnitParser(ChineseDurationNumberWithUnitParserConfiguration())

    def parse(self, source: ExtractResult, reference: datetime = None) -> Optional[DateTimeParseResult]:
        if reference is None:
            reference = datetime.now()

        result = DateTimeParseResult()

        if source.type is self.parser_type_name:
            inner_result = DateTimeResolutionResult()

            has_half_suffix = source.text.endswith('半')

            if has_half_suffix:
                source.length = source.length - 1
                source.text = source.text[:source.length]

            parse_result = self._internal_parser.parse(source)
            unit_result = parse_result.value

            if not unit_result:
                return result

            unit_str = unit_result.unit
            number_str = unit_result.number

            if has_half_suffix:
                number_str = str((float(number_str) + 0.5))

            unit_type = 'T' if self.is_less_than_day(unit_str) else ''
            time_value = int(float(number_str) * self.config.unit_value_map.get(unit_str, 1))

            inner_result.timex = f'P{unit_type}{number_str}{unit_str[0]}'
            inner_result.future_value = time_value
            inner_result.past_value = time_value
            inner_result.future_resolution[TimeTypeConstants.DURATION] = str(inner_result.future_value)
            inner_result.past_resolution[TimeTypeConstants.DURATION] = str(inner_result.past_value)

            result = DateTimeParseResult(source)
            result.value = inner_result
            result.timex_str = inner_result.timex if inner_result is not None else ''
            result.resolution_str = ''

        return result
