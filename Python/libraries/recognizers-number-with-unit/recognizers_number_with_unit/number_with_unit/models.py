from abc import abstractmethod
from typing import List, Optional, Dict

from recognizers_text.model import Model, ModelResult
from recognizers_text.extractor import Extractor
from recognizers_text.parser import Parser

class ExtractorParserModel:
    def __init__(self, extractor:Extractor, parser: Parser):
        self.extractor = extractor
        self.parser = parser

class AbstractNumberWithUnitModel(Model):
    @property
    @abstractmethod
    def model_type_name(self) -> str:
        raise NotImplementedError

    def __init__(self, extractor_parser: List[ExtractorParserModel]):
        self.extractor_parser: List[ExtractorParserModel] = extractor_parser

    def parse(self, query: str) -> List[ModelResult]:
        #query = FormatUtility.preProcess(query, false) TODO: for chinese characters
        extraction_results = []
        for item in self.extractor_parser:
            extract_results = item.extractor.extract(query)
            parse_results = [r for r in [item.parser.parse(r) for r in extract_results] if not r.value is None]

            for parse_result in parse_results:
                model_result = ModelResult()
                model_result.start = parse_result.start
                model_result.end = parse_result.start + result.end - 1
                model_result.text = parse_result.text
                model_result.type_name = self.model_type_name
                
                b_add = not [x for x in extraction_results if x.start == model_result.start and x.end == model_result.end]

                if b_add:
                    extraction_results.push(result)

        return extraction_results

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
