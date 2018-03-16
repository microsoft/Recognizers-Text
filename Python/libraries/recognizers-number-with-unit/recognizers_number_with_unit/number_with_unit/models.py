from abc import abstractmethod
from typing import List, Optional, Dict
from collections import namedtuple

from recognizers_text.model import Model, ModelResult
from recognizers_text.extractor import Extractor
from recognizers_text.parser import Parser

ExtractorParserItem = namedtuple('ExtractorParserItem', ['extractor', 'parser'])
class AbstractNumberWithUnitModel(Model):
    @property
    @abstractmethod
    def model_type_name(self) -> str:
        raise NotImplementedError

    def __init__(self, extractor_parser_dict: Dict[Extractor, Parser]):
        self.extractor_parser_dict: Dict[str, ExtractorParserItem] = dict()
        for extractor in extractor_parser_dict:
            self.extractor_parser_dict[type(extractor).__name__] = ExtractorParserItem(extractor, extractor_parser_dict[extractor])

    def parse(self, query: str) -> List[ModelResult]:
        #TODO parse code
        pass

class AgeModel(AbstractNumberWithUnitModel):
    @property
    def model_type_name(self) -> str:
        return 'age'

class CurrencyModel(AbstractNumberWithUnitModel):
    @property
    def model_type_name(self) -> str:
        return 'currency'

class DimensionModel(AbstractNumberWithUnitModel):
    @property
    def model_type_name(self) -> str:
        return 'dimension'

class TemperatureModel(AbstractNumberWithUnitModel):
    @property
    def model_type_name(self) -> str:
        return 'temperature'
