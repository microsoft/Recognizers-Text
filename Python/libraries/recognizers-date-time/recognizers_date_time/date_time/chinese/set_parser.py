from typing import Optional
from datetime import datetime

from recognizers_text.utilities import RegExpUtility
from recognizers_text.extractor import ExtractResult
from ..constants import TimeTypeConstants
from ..utilities import DateTimeResolutionResult
from ..base_set import BaseSetParser
from ..parsers import DateTimeParser, DateTimeParseResult
from ..extractors import DateTimeExtractor
from .set_parser_config import ChineseSetParserConfiguration

class ChineseSetParser(BaseSetParser):
    def __init__(self):
        config = ChineseSetParserConfiguration()
        BaseSetParser.__init__(self, config)

    def parse(self, source: ExtractResult, reference: datetime = None) -> Optional[DateTimeParseResult]:
        if reference is None:
            reference = datetime.now()

        value = None

        if source.type is self.parser_type_name:
            inner_result = self.parse_each_unit(source.text)

        if not inner_result.success:
            inner_result = self.parse_each_duration(source.text, reference)

        if not inner_result.success:
            inner_result = self.parser_time_everyday(source.text, reference)

        if not inner_result.success:
            inner_result = self.parse_each(self.config.date_time_extractor, self.config.date_time_parser, source.text, reference)

        if not inner_result.success:
            inner_result = self.parse_each(self.config.date_extractor, self.config.date_parser, source.text, reference)

        if inner_result.success:
            inner_result.future_resolution[TimeTypeConstants.SET] = inner_result.future_value
            inner_result.past_resolution[TimeTypeConstants.SET] = inner_result.past_value
            value = inner_result

        result = DateTimeParseResult(source)
        result.value = value
        result.timex_str = value.timex if value else ''
        result.resolution_str = ''

        return result

    def parse_each_unit(self, source: str) -> DateTimeResolutionResult:
        result = DateTimeResolutionResult()

        match = self.config.each_unit_regex.search(source)
        if not (match and (match.end() - match.start()) == len(source)):
            return result

        source_unit = RegExpUtility.get_group(match, 'unit')
        if not (source_unit and source_unit in self.config.unit_map):
            return result

        get_matched_unit_timex = self.config.get_matched_unit_timex(source_unit)
        if not get_matched_unit_timex.matched:
            return result

        result.timex = get_matched_unit_timex.timex
        result.future_value = result.past_value = 'Set: ' + result.timex
        result.success = True
        return result

    def parser_time_everyday(self, source: str, reference: datetime) -> DateTimeResolutionResult:
        result = DateTimeResolutionResult()
        ers = self.config.time_extractor.extract(source, reference)
        if len(ers) != 1:
            return result

        er = ers[0]
        before_str = source[0: er.start]
        match = self.config.each_day_regex.search(before_str)
        if not match:
            return result

        pr = self.config.time_parser.parse(er)
        result.timex = pr.timex_str
        result.future_value = result.past_value = 'Set: ' + result.timex
        result.success = True

        return result

    def parse_each(self, extractor: DateTimeExtractor, parser: DateTimeParser,
                   source: str, reference: datetime) -> DateTimeResolutionResult:
        result = DateTimeResolutionResult()
        ers = extractor.extract(source, reference)
        if len(ers) != 1:
            return result

        er = ers[0]
        before_str = source[0:er.start]
        match = self.config.each_prefix_regex.search(before_str)
        if not match:
            return result

        timex = parser.parse(er).timex_str
        result.timex = timex
        result.future_value = 'Set: ' + timex
        result.past_value = 'Set: ' + timex
        result.success = True

        return result
