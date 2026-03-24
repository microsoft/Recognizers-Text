#  Copyright (c) Microsoft Corporation. All rights reserved.
#  Licensed under the MIT License.

import re
from abc import abstractmethod
from typing import List

from recognizers_text.model import Model, ModelResult
from recognizers_text.extractor import Extractor
from recognizers_text.parser import Parser
from recognizers_text.utilities import QueryProcessor
from recognizers_number_with_unit.number_with_unit.parsers import UnitValue, CurrencyUnitValue

# Matches an uppercase ISO currency prefix (1–3 letters + optional $) immediately
# followed by a digit — e.g. 'USD34', 'VND4,927', 'A$100', 'SG$40', 'CAD$1'.
# Used in CurrencyModel.parse() to insert a separating space before
# QueryProcessor lowercases the query, preventing the internal number extractor
# from misreading patterns like 'usd34.6 million' as '6 million'.
_CURRENCY_ISO_CONCAT_RE = re.compile(r'\b([A-Z]{1,3}\$|[A-Z]{3})(?=\d)')


def _to_original_pos(normalised_pos: int, insertions: List[int]) -> int:
    """Convert a position in the space-normalised string to the original string position.

    Each inserted space at original position insertions[k] shifts all subsequent
    normalised positions by +k+1.  To reverse: subtract the count of insertions
    whose normalised position falls strictly before normalised_pos.
    """
    count = sum(
        1 for k, p in enumerate(insertions) if p + k < normalised_pos
    )
    return normalised_pos - count


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

        query = QueryProcessor.preprocess(query, True)
        extraction_results = []
        parse_results = []

        try:
            for item in self.extractor_parser:
                extract_results = item.extractor.extract(query)
                for result in extract_results:
                    r = item.parser.parse(result)
                    if r.value is not None:
                        if isinstance(r.value, list):
                            for j in r.value:
                                parse_results.append(j)
                        else:
                            parse_results.append(r)

                for parse_result in parse_results:
                    model_result = ModelResult()
                    model_result.start = parse_result.start
                    model_result.end = parse_result.start + parse_result.length - 1
                    model_result.text = parse_result.text
                    model_result.type_name = self.model_type_name
                    model_result.resolution = self.get_resolution(
                        parse_result.value)

                    b_add = not [x for x in extraction_results if (model_result.start <= x.start and model_result.end >= x.end)]

                    if b_add:
                        extraction_results.append(model_result)
        except Exception:
            pass

        return extraction_results

    @staticmethod
    def get_resolution(data):
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
        elif isinstance(data, list):
            if hasattr(data[0].value, 'iso_currency'):
                return {
                    'value': data[0].value.number,
                    'unit': data[0].value.unit,
                    'isoCurrency': data[0].value.iso_currency
                }
            else:
                return {
                    'value': data[0].value.number,
                    'unit': data[0].value.unit
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

    def parse(self, query: str) -> List[ModelResult]:
        # Normalise uppercase ISO currency prefixes that are directly
        # concatenated to digits before the base class calls
        # QueryProcessor.preprocess() (which lowercases the query).
        #
        # Without this step the internal EnglishNumberExtractor (Unit mode)
        # misreads patterns such as:
        #   'USD34.6 million'  -> extracts '6 million'  (decimal boundary)
        #   'VND4,927 billion' -> extracts '927 billion' (comma boundary)
        #
        # After inserting the space:
        #   'USD34.6 million'  -> 'USD 34.6 million'  -> '34.6 million' ✓
        #   'VND4,927 billion' -> 'VND 4,927 billion' -> '4,927 billion' ✓
        #
        # Uppercase-only matching avoids false positives on common English
        # words ('can', 'try', 'nor', etc.) which are never all-caps.
        #
        # When spaces are inserted the base-class results carry positions and
        # text from the normalised string, not the original.  We record the
        # insertion points and map every result back to the original string so
        # that callers always receive offsets that are valid against their input.
        insertions = [m.end() for m in _CURRENCY_ISO_CONCAT_RE.finditer(query)]

        if not insertions:
            # No concatenation found — no position adjustment needed.
            return super().parse(query)

        normalised = _CURRENCY_ISO_CONCAT_RE.sub(r'\1 ', query)
        results = super().parse(normalised)

        for result in results:
            orig_start = _to_original_pos(result.start, insertions)
            orig_end = _to_original_pos(result.end, insertions)
            result.start = orig_start
            result.end = orig_end
            result.text = query[orig_start:orig_end + 1]

        return results


class DimensionModel(AbstractNumberWithUnitModel):
    @property
    def model_type_name(self) -> str:
        return 'dimension'


class TemperatureModel(AbstractNumberWithUnitModel):
    @property
    def model_type_name(self) -> str:
        return 'temperature'
