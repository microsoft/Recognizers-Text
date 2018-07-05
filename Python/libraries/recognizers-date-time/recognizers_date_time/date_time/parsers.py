from abc import abstractmethod
from typing import Optional
from datetime import datetime

from recognizers_text.extractor import ExtractResult
from recognizers_text.parser import Parser, ParseResult

class DateTimeParseResult(ParseResult):
    def __init__(self, source: ExtractResult = None):
        super().__init__(source)
        self.timex_str: str = ''

class DateTimeParser(Parser):
    @property
    @abstractmethod
    def parser_type_name(self) -> str:
        raise NotImplementedError

    @abstractmethod
    def parse(self, source: ExtractResult, reference: datetime = None) -> Optional[DateTimeParseResult]:#pylint: disable=W0221
        raise NotImplementedError
