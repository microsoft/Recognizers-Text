from typing import List, Optional
from datetime import datetime

from recognizers_text.extractor import ExtractResult
from .constants import Constants
from .extractors import DateTimeExtractor
from .parsers import DateTimeParser, DateTimeParseResult

class HolidayExtractorConfiguration:
    pass

class BaseHolidayExtractor(DateTimeExtractor):
    @property
    def extractor_type_name(self) -> str:
        return Constants.SYS_DATETIME_DATE

    def __init__(self, config: HolidayExtractorConfiguration):
        self.config = config

    def extract(self, source: str, reference: datetime = None) -> List[ExtractResult]:
        #TODO: code
        pass

class HolidayParserConfiguration:
    pass

class BaseHolidayParser(DateTimeParser):
    @property
    def parser_type_name(self) -> str:
        return Constants.SYS_DATETIME_DATE

    def __init__(self, config: HolidayParserConfiguration):
        self.config = config

    def parse(self, source: ExtractResult, reference: datetime = None) -> Optional[DateTimeParseResult]:
        #TODO: code
        pass
