from abc import ABC, abstractproperty
from typing import List, Dict, Generic, TypeVar, Callable, Tuple, NamedTuple, Optional
from enum import Enum
from collections import namedtuple

from recognizers_text import Model, ModelResult
from recognizers_text.extractor import Extractor, ExtractResult
from recognizers_text.parser import ParseResult, Parser

class NumberMode(Enum):
    DEFAULT=0,
    CURRENCY=1,
    PURE_NUMBER=2

long_format_type = namedtuple('long_format_type', ['thousands_mark', 'decimals_mark'])

class LongFormatMode:
    INTEGER_COMMA=long_format_type(thousands_mark=',', decimals_mark=None)
    INTEGER_DOT=long_format_type(thousands_mark='.', decimals_mark=None)
    DOUBLE_COMMA_DOT=long_format_type(thousands_mark=',', decimals_mark='.')
    DOUBLE_DOT_COMMA=long_format_type(thousands_mark='.', decimals_mark=',')

class AbstractNumberModel(Model):
    @abstractproperty
    def model_type_name(self) -> str:
        raise NotImplementedError

    def __init__(self, parser: Parser, extractor: Extractor):
        self.parser: Parser=parser
        self.extractor: Extractor=extractor

    def parse(self, query: str) -> List[ModelResult]:
        extract_results=self.extractor.extract(query)
        results = list()
        for r in extract_results:
            results.append(self.__single_parse(r))
        return results

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
    @property
    def model_type_name(self) -> str:
        return 'number'

class OrdinalModel(AbstractNumberModel):
    @property
    def model_type_name(self) -> str:
        return 'ordinal'

class PercentModel(AbstractNumberModel):
    @property
    def model_type_name(self) -> str:
        return 'percentage'