from abc import ABC, abstractproperty
from typing import List, Dict, Generic, TypeVar, Callable, Tuple, NamedTuple, Optional
from recognizers_text import Model, ModelResult
from recognizers_text.extractor import Extractor, ExtractResult
from recognizers_text.parser import ParseResult, Parser

class AbstractNumberModel(Model):
    @abstractproperty
    def model_type_name(self) -> str:
        raise NotImplementedError

    def __init__(self, parser: Parser, extractor: Extractor):
        self.parser: Parser=parser
        self.extractor: Extractor=extractor

    def parse(self, query: str) -> List[ModelResult]:
        extract_results=self.extractor.extract(query)
        return map(self.__single_parse, extract_results)

    def __single_parse(self, source: ExtractResult) -> Optional[ModelResult]:
        parse_result=self.parser.parse(source)
        if parse_result is None:
            return None
        result = ModelResult()
        result.start=parse_result.start
        result.end=parse_result.end
        result.resolution=dict([('value', parse_result.resolution_str)])
        result.text=parse_result.text
        result.type_name=self.model_type_name
        return result

class NumberModel(AbstractNumberModel):
    def model_type_name(self) -> str:
        return 'number'

class OrdinalModel(AbstractNumberModel):
    def model_type_name(self) -> str:
        return 'ordinal'

class PercentModel(AbstractNumberModel):
    def model_type_name(self) -> str:
        return 'percentage'