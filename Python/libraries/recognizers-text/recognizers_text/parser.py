from abc import ABC, abstractmethod
from typing import Optional
from .extractor import ExtractResult


class ParseResult(ExtractResult):
    def __init__(self, source: ExtractResult = None):
        super().__init__()
        self.value: object = None
        self.resolution_str: str = None
        if source is not None:
            self.start = source.start
            self.length = source.length
            self.text = source.text
            self.type = source.type
            self.data = source.data


class Parser(ABC):
    @abstractmethod
    def parse(self, source: ExtractResult) -> Optional[ParseResult]:
        raise NotImplementedError
