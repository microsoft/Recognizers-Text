from abc import ABC, abstractmethod
from typing import List

class ExtractResult:
    def __init__(self):
        self.start: int = None
        self.length: int = None
        self.text: str = None
        self.type: str = None
        self.data: object = None

    @property
    def end(self):
        return self.start + self.length - 1

    def overlap(self, other: 'ExtractResult') -> bool:
        return (not self.start > other.end) and (not other.start > self.end)

    def cover(self, other: 'ExtractResult') -> bool:
        return (((other.start < self.start) and (other.end >= self.end))
                or ((other.start <= self.start) and (other.end > self.end)))

    @staticmethod
    def get_from_text(source: str):
        result = ExtractResult()
        result.start = 0
        result.length = len(source)
        result.text = source
        result.type = 'custom'
        return result

class Extractor(ABC):
    @abstractmethod
    def extract(self, source: str) -> List[ExtractResult]:
        raise NotImplementedError
