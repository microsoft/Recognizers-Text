from abc import abstractmethod
from typing import List
from recognizers_text.extractor import ExtractResult
from recognizers_text.extractor import Extractor


class DateTimeExtractor(Extractor):

    @property
    @abstractmethod
    def extract(self, text, reference) -> List[ExtractResult]:
        raise NotImplementedError
