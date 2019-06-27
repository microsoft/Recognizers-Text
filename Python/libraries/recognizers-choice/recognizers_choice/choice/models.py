from abc import abstractmethod

from recognizers_text.extractor import Extractor
from recognizers_text.model import Model, ModelResult
from recognizers_text.parser import Parser, ParseResult


class ChoiceModel(Model):
    @property
    @abstractmethod
    def model_type_name(self) -> str:
        raise NotImplementedError

    def __init__(self, parser: Parser, extractor: Extractor):
        self.extractor = extractor
        self.parser = parser

    def parse(self, source: str):
        extract_results = self.extractor.extract(source)
        parse_results = [self.parser.parse(e) for e in extract_results]
        result = []
        for o in parse_results:
            model_result = ModelResult()
            model_result.start = o.start
            model_result.end = o.start + len(o.text) - 1
            model_result.resolution = self.get_resolution(o)
            model_result.text = o.text
            model_result.type_name = self.model_type_name

            result.append(model_result)

        return result

    @abstractmethod
    def get_resolution(self, data: ParseResult):
        raise NotImplementedError


class BooleanModel(ChoiceModel):
    @property
    def model_type_name(self) -> str:
        return 'boolean'

    def get_resolution(self, sources: ParseResult):
        results = {
            'value': sources.value,
            'score': sources.data.score
        }

        if sources.data.other_matches:
            results.other_results = [{'text': o.text,
                                      'value': o.value,
                                      'score': o.data.score}
                                     for o in sources.data.other_matches]
        return results
