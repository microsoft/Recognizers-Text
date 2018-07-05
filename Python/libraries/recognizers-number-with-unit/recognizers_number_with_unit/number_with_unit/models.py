from abc import abstractmethod
from typing import List
from collections import namedtuple

from recognizers_text.model import Model, ModelResult
from recognizers_text.extractor import Extractor
from recognizers_text.parser import Parser
from recognizers_text.utilities import FormatUtility
from recognizers_number_with_unit.number_with_unit.parsers import UnitValue, CurrencyUnitValue

class ExtractorParserModel:
    def __init__(self, extractor: Extractor, parser: Parser):
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
        query = FormatUtility.preprocess(query, False)
        #query = FormatUtility.preProcess(query, false) TODO: for chinese characters
        extraction_results = []
        for item in self.extractor_parser:
            extract_results = item.extractor.extract(query)
            parse_results = [r for r in [item.parser.parse(r) for r in extract_results] if not r.value is None]

            for parse_result in parse_results:
                model_result = ModelResult()
                model_result.start = parse_result.start
                model_result.end = parse_result.start + parse_result.end - 1
                model_result.text = parse_result.text
                model_result.type_name = self.model_type_name
                model_result.resolution = self.get_resolution(parse_result.value)

                b_add = not [x for x in extraction_results if x.start == model_result.start and x.end == model_result.end]

                if b_add:
                    extraction_results.append(model_result)

        return extraction_results

    def get_resolution(self, data):
        if isinstance(data, str):
            return {
                'value': data
            }
        elif isinstance(data, UnitValue):
            return {
                'value': data.number,
                'unit': data.unit
            }
        elif isinstance(data, CurrencyUnitValue):
            return {
                'value': data.number,
                'unit': data.unit,
                'isoCurrency': data.iso_currency
            }
        return None

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
