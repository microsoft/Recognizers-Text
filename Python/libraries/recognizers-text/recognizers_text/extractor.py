from abc import ABC, abstractmethod
from typing import List

class ExtractResult:
    def __init__(self):
        self.start: int
        self.length: int
        self.text: str
        self.type: str
        self.data: object=None

    @property
    def end(self):
        return self.start + self.length - 1

    def overlap(self, other: 'ExtractResult') -> bool:
        return (not self.start > other.end) and (not other.start > self.end)

    def cover(self, other: 'ExtractResult') -> bool:
        return (((other.start < self.start) and (other.end >= self.end))
            or ((other.start <= self.start) and (other.end > self.end)))

class Extractor(ABC):
    @abstractmethod
    def extract(self, input: str) -> List[ExtractResult]:
        raise NotImplementedError