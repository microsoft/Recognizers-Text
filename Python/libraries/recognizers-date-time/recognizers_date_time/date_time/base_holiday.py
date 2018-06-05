from abc import ABC, abstractmethod
from typing import List, Optional, Pattern
from datetime import datetime

from recognizers_text.extractor import ExtractResult
from .constants import Constants
from .extractors import DateTimeExtractor
from .parsers import DateTimeParser, DateTimeParseResult
from .utilities import Token, merge_all_tokens

class HolidayExtractorConfiguration(ABC):
    @property
    @abstractmethod
    def holiday_regexes(self) -> List[Pattern]:
        raise NotImplementedError

class BaseHolidayExtractor(DateTimeExtractor):
    @property
    def extractor_type_name(self) -> str:
        return Constants.SYS_DATETIME_DATE

    def __init__(self, config: HolidayExtractorConfiguration):
        self.config = config

    def extract(self, source: str, reference: datetime = None) -> List[ExtractResult]:
        if not reference:
            reference = datetime.now()
        tokens = []
        tokens += self.__holiday_match(source)
        result = merge_all_tokens(tokens, source, self.extractor_type_name)
        return result

    def __holiday_match(self, source: str) -> List[Token]:
        tokens = []
        for regexp in self.config.holiday_regexes:
            matches = regexp.finditer(source)
            for match in matches:
                tokens.append(Token(match.start(), match.end()))
        return tokens

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
