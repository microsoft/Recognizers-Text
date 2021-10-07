#  Copyright (c) Microsoft Corporation. All rights reserved.
#  Licensed under the MIT License.

from typing import List
from datetime import datetime

from recognizers_text.model import Model, ModelResult
from recognizers_text.utilities import QueryProcessor
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

    def parse(self, query: str, reference: datetime = None) -> List[ModelResult]:  # pylint: disable=W0221
        query = QueryProcessor.preprocess(query)
        parser_dates = []

        try:
            extract_results = self.extractor.extract(query, reference)

            for result in extract_results:
                parse_result = self.parser.parse(result, reference)
                if isinstance(parse_result.value, list):
                    parser_dates += parse_result.value
                else:
                    parser_dates.append(parse_result)
        except Exception:
            pass

        return [self.__to_model_result(x) for x in parser_dates]

    @staticmethod
    def __to_model_result(parse_result_value) -> ModelResult:
        result = ModelResult()
        result.start = parse_result_value.start
        result.end = parse_result_value.start + parse_result_value.length - 1
        result.resolution = parse_result_value.value
        result.text = parse_result_value.text
        result.type_name = parse_result_value.type
        return result
