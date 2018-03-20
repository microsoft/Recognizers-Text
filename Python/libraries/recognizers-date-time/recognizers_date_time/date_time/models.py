from typing import List
from datetime import datetime

from recognizers_text.model import Model, ModelResult
from .extractors import DateTimeExtractor
from .parsers import DateTimeParser

class DateTimeModelResult(ModelResult):
    def __init__(self):
        super().__init__()
        self.timex_str: str

class DateTimeModel(Model):
    def model_type_name(self) -> str:
        return 'datetime'

    def __init__(self, parser: DateTimeParser, extractor: DateTimeExtractor):
        self.parser = parser
        self.extractor = extractor

    def parse(self, query: str, reference: datetime = None) -> List[ModelResult]:#pylint: disable=W0221
        #TODO implement code
        pass
