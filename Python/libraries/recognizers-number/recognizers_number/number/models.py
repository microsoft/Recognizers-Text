from abc import abstractmethod
from typing import List, Optional
from enum import Enum
from collections import namedtuple

from recognizers_text import Model, ModelResult
from recognizers_text.extractor import Extractor, ExtractResult
from recognizers_text.parser import Parser
from recognizers_text.utilities import QueryProcessor
from recognizers_number.number.constants import Constants


class NumberMode(Enum):
    DEFAULT = 0
    CURRENCY = 1
    PURE_NUMBER = 2
    Unit = 3


LongFormatType = namedtuple(
    'LongFormatType', ['thousands_mark', 'decimals_mark'])


class LongFormatMode:
    INTEGER_COMMA = LongFormatType(thousands_mark=',', decimals_mark=None)
    INTEGER_DOT = LongFormatType(thousands_mark='.', decimals_mark=None)
    INTEGER_BLANK = LongFormatType(thousands_mark=' ', decimals_mark=None)
    INTEGER_NO_BREAK_SPACE = LongFormatType(
        thousands_mark=Constants.NO_BREAK_SPACE, decimals_mark=None)
    DOUBLE_COMMA_DOT = LongFormatType(thousands_mark=',', decimals_mark='.')
    DOUBLE_NO_BREAK_SPACE_DOT = LongFormatType(
        thousands_mark=Constants.NO_BREAK_SPACE, decimals_mark='.')
    DOUBLE_DOT_COMMA = LongFormatType(thousands_mark='.', decimals_mark=',')
    DOUBLE_NO_BREAK_SPACE_COMMA = LongFormatType(
        thousands_mark=Constants.NO_BREAK_SPACE, decimals_mark=',')


class AbstractNumberModel(Model):
    @property
    @abstractmethod
    def model_type_name(self) -> str:
        raise NotImplementedError

    def __init__(self, parser: Parser, extractor: Extractor):
        self.parser: Parser = parser
        self.extractor: Extractor = extractor

    def parse(self, query: str) -> List[ModelResult]:
        query = QueryProcessor.preprocess(query, True)
        results = []

        try:
            extract_results = self.extractor.extract(query)
            results = list(map(self.__single_parse, extract_results))
        except Exception:
            pass

        return results

    def __single_parse(self, source: ExtractResult) -> Optional[ModelResult]:
        parse_result = self.parser.parse(source)
        if parse_result is None:
            return None
        result = ModelResult()
        result.start = parse_result.start
        result.end = parse_result.end
        result.resolution = dict([('value', parse_result.resolution_str)])
        result.text = parse_result.text
        result.type_name = self.model_type_name
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
