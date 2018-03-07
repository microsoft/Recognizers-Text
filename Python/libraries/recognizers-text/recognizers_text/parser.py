from abc import ABC, abstractmethod
from typing import Optional
from .extractor import ExtractResult

class ParseResult(ExtractResult):
    def __init__(self, source: ExtractResult=None):
        self.value: object=None
        self.resolution_str: str
        if source is not None:
            self = source

class Parser(ABC):
    @abstractmethod
    def parse(self, source: ExtractResult) -> Optional[ParseResult]:
        raise NotImplementedError