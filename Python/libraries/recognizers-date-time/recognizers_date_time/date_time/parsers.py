from abc import abstractmethod
from typing import Optional
from datetime import datetime

from recognizers_text.extractor import ExtractResult
from recognizers_text.parser import Parser, ParseResult

class DateTimeParseResult(ParseResult):
    def __init__(self):
        super().__init__()
        self.timex_str: str

class DateTimeParser(Parser):
    @abstractmethod
    def parse(self, source: ExtractResult, reference: datetime = None) -> Optional[DateTimeParseResult]:#pylint: disable=W0221
        raise NotImplementedError
