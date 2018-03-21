from typing import List, Optional
from datetime import datetime

from recognizers_text.extractor import ExtractResult
from .constants import Constants
from .extractors import DateTimeExtractor
from .parsers import DateTimeParser, DateTimeParseResult

class DatePeriodExtractorConfiguration:
    pass

class BaseDatePeriodExtractor(DateTimeExtractor):
    @property
    def extractor_type_name(self) -> str:
        return Constants.SYS_DATETIME_DATEPERIOD

    def __init__(self, config: DatePeriodExtractorConfiguration):
        self.config = config

    def extract(self, source: str, reference: datetime = None) -> List[ExtractResult]:
        #TODO: code
        pass

class DatePeriodParserConfiguration:
    pass

class BaseDatePeriodParser(DateTimeParser):
    @property
    def parser_type_name(self) -> str:
        return Constants.SYS_DATETIME_DATEPERIOD

    def __init__(self, config: DatePeriodParserConfiguration, include_end: bool = False):
        self.config = config
        self.include_end = include_end

    def parse(self, source: ExtractResult, reference: datetime = None) -> Optional[DateTimeParseResult]:
        #TODO: code
        pass
