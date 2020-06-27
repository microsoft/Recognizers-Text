from abc import abstractmethod
from typing import List
from recognizers_text.extractor import ExtractResult


class DateTimeListExtractor:

    @abstractmethod
    def extract(self, extract_result, text, reference) -> List[ExtractResult]:
        raise NotImplementedError
