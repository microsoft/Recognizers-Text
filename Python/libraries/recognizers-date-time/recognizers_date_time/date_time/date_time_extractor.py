from abc import abstractmethod
from typing import List
from recognizers_text.extractor import ExtractResult
from recognizers_text.extractor import Extractor


class DateTimeExtractor(Extractor):

    @abstractmethod
    def extract(self, text) -> List[ExtractResult]:
        raise NotImplementedError
