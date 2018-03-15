from abc import abstractmethod
from typing import List, Optional, Dict
from collections import namedtuple

from recognizers_text.model import Model, ModelResult
from recognizers_text.extractor import Extractor
from recognizers_text.parser import Parser

class AbstractNumberWithUnitModel(Model):
    @property
    @abstractmethod
    def model_type_name(self) -> str:
        raise NotImplementedError

    def __init__(self, extractor_parser_dict: Dict[Extractor, Parser]):
        self.extractor_parser_dict = extractor_parser_dict

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
