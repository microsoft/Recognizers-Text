from typing import List, Optional
from datetime import datetime

from recognizers_text.extractor import ExtractResult
from .constants import Constants
from .extractors import DateTimeExtractor
from .parsers import DateTimeParser, DateTimeParseResult
from .date_time_recognizer import DateTimeOptions

class MergedExtractorConfiguration:
    pass

class BaseMergedExtractor(DateTimeExtractor):
    @property
    def extractor_type_name(self) -> str:
        return Constants.SYS_DATETIME_MERGED

    def __init__(self, config: MergedExtractorConfiguration, options: DateTimeOptions):
        self.config = config
        self.options = options

    def extract(self, source: str, reference: datetime = None) -> List[ExtractResult]:
        #TODO: code
        pass

class MergedParserConfiguration:
    pass

class BaseMergedParser(DateTimeParser):
    @property
    def parser_type_name(self) -> str:
        return Constants.SYS_DATETIME_MERGED

    def __init__(self, config: MergedParserConfiguration, options: DateTimeOptions):
        self.config = config
        self.options = options

    def parse(self, source: ExtractResult, reference: datetime = None) -> Optional[DateTimeParseResult]:
        #TODO: code
        pass
