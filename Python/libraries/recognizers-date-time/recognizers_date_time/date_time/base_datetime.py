from typing import List, Optional
from datetime import datetime

from recognizers_text.extractor import ExtractResult
from .constants import Constants
from .extractors import DateTimeExtractor
from .parsers import DateTimeParser, DateTimeParseResult

class DateTimeExtractorConfiguration:
    pass

class BaseDateTimeExtractor(DateTimeExtractor):
    @property
    def extractor_type_name(self) -> str:
        return Constants.SYS_DATETIME_DATETIME

    def __init__(self, config: DateTimeExtractorConfiguration):
        self.config = config

    def extract(self, source: str, reference: datetime = None) -> List[ExtractResult]:
        #TODO: code
        pass

class DateTimeParserConfiguration:
    pass

class BaseDateTimeParser(DateTimeParser):
    @property
    def parser_type_name(self) -> str:
        return Constants.SYS_DATETIME_DATETIME

    def __init__(self, config: DateTimeParserConfiguration):
        self.config = config

    def parse(self, source: ExtractResult, reference: datetime = None) -> Optional[DateTimeParseResult]:
        #TODO: code
        pass
