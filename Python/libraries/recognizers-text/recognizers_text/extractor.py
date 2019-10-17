from abc import ABC, abstractmethod
from typing import List
from .meta_data import MetaData


class ExtractResult:
    def __init__(self):
        self.start: int = 0
        self.length: int = 0
        self.text: str = ''
        self.type: str = ''
        self.data: object = None
        self.meta_data: MetaData = None

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


class Metadata:

    def __init__(self):
        self._possibly_included_period_end = False
        self._is_duration_with_before_and_after = False
        self._is_holiday = False
        self._is_ordinal_relative = False
        self._offset = ''
        self._relative_to = ''

    @property
    def possibly_included_period_end(self):
        return self._possibly_included_period_end

    @possibly_included_period_end.setter
    def possibly_included_period_end(self, value):
        self._possibly_included_period_end = value

    @property
    def is_duration_with_before_and_after(self):
        return self._is_duration_with_before_and_after

    @is_duration_with_before_and_after.setter
    def is_duration_with_before_and_after(self, value):
        self._is_duration_with_before_and_after = value

    @property
    def is_holiday(self):
        return self._is_holiday

    @is_holiday.setter
    def is_holiday(self, value):
        self._is_holiday = value

    @property
    def is_ordinal_relative(self):
        return self._is_ordinal_relative

    @is_ordinal_relative.setter
    def is_ordinal_relative(self, value):
        self._is_ordinal_relative = value

    @property
    def offset(self):
        return self._offset

    @offset.setter
    def offset(self, value):
        self._offset = value

    @property
    def relative_to(self):
        return self._relative_to

    @relative_to.setter
    def relative_to(self, value):
        self._relative_to = value
