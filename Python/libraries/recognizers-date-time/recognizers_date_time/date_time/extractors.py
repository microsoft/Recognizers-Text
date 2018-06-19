from abc import abstractmethod
from typing import List
from datetime import datetime

from recognizers_text.extractor import Extractor, ExtractResult

class DateTimeExtractor(Extractor):
    @property
    @abstractmethod
    def extractor_type_name(self) -> str:
        raise NotImplementedError

    @abstractmethod
    def extract(self, source: str, reference: datetime = None) -> List[ExtractResult]:#pylint: disable=W0221
        raise NotImplementedError
