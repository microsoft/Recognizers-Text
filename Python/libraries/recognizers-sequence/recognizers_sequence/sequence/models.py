from abc import abstractmethod
from typing import List

from recognizers_sequence.sequence.constants import Constants
from recognizers_text import QueryProcessor
from recognizers_text.model import Model, ModelResult
from recognizers_text.extractor import Extractor
from recognizers_text.parser import Parser, ParseResult


class AbstractSequenceModel(Model):
    @property
    @abstractmethod
    def model_type_name(self) -> str:
        raise NotImplementedError

    def __init__(self, parser: Parser, extractor: Extractor):
        self.extractor = extractor
        self.parser = parser

    def parse(self, query: str) -> List[ModelResult]:
        model_results: List[ModelResult] = list()
        parse_results = []

        try:
            extract_results = self.extractor.extract(query)
            parse_results = [self.parser.parse(e) for e in extract_results]
        except Exception:
            pass

        for parse_result in parse_results:
            model_result = ModelResult()
            model_result.start = parse_result.start
            model_result.end = parse_result.start + parse_result.length - 1
            model_result.text = parse_result.text
            model_result.type_name = self.model_type_name
            model_result.resolution = self.get_resolution(parse_result)

            model_results.append(model_result)

        return model_results

    @staticmethod
    def get_resolution(data: ParseResult):
        return {
            'value': data.resolution_str
        }


class PhoneNumberModel(AbstractSequenceModel):
    @property
    def model_type_name(self) -> str:
        return Constants.MODEL_PHONE_NUMBER

    def parse(self, query: str) -> List[ModelResult]:

        model_results: List[ModelResult] = list()
        parse_results = []
        query = QueryProcessor.preprocess(query)

        try:
            extract_results = self.extractor.extract(query)
            parse_results = [self.parser.parse(e) for e in extract_results]
        except Exception:
            pass

        filtered_parse_results = list(filter(lambda o: o.data is not None, parse_results))
        for parse_result in filtered_parse_results:
            model_result = ModelResult()
            model_result.start = parse_result.start
            model_result.end = parse_result.start + parse_result.length - 1
            model_result.text = parse_result.text
            model_result.type_name = self.model_type_name
            model_result.resolution = self.get_resolution(parse_result)

            model_results.append(model_result)

        return model_results

    def get_resolution(self, data: ParseResult):
        return {
            'value': data.resolution_str,
            'score': '%g' % data.value
        }


class EmailModel(AbstractSequenceModel):
    @property
    def model_type_name(self) -> str:
        return Constants.MODEL_EMAIL


class GUIDModel(AbstractSequenceModel):
    @property
    def model_type_name(self) -> str:
        return Constants.MODEL_GUID

    def parse(self, query: str) -> List[ModelResult]:
        model_results: List[ModelResult] = list()
        parse_results = []
        query = QueryProcessor.preprocess(query)

        try:
            extract_results = self.extractor.extract(query)
            parse_results = [self.parser.parse(e) for e in extract_results]
        except Exception:
            pass

        for parse_result in parse_results:
            model_result = ModelResult()
            model_result.start = parse_result.start
            model_result.end = parse_result.start + parse_result.length - 1
            model_result.text = parse_result.text
            model_result.type_name = self.model_type_name
            model_result.resolution = self.get_resolution(parse_result)

            model_results.append(model_result)

        return model_results

    def get_resolution(self, data: ParseResult):
        return {
            'value': data.resolution_str,
            'score': '%g' % data.value
        }


class HashtagModel(AbstractSequenceModel):

    @property
    def model_type_name(self) -> str:
        return Constants.MODEL_HASHTAG


class IpAddressModel(AbstractSequenceModel):
    @property
    def model_type_name(self) -> str:
        return Constants.MODEL_IP

    def parse(self, query: str) -> List[ModelResult]:
        model_results: List[ModelResult] = list()
        parse_results = []

        try:
            extract_results = self.extractor.extract(query)
            parse_results = [self.parser.parse(e) for e in extract_results]
        except Exception:
            pass

        filtered_parse_results = list(filter(lambda o: o.data is not None, parse_results))
        for parse_result in filtered_parse_results:
            model_result = ModelResult()
            model_result.start = parse_result.start
            model_result.end = parse_result.start + parse_result.length - 1
            model_result.text = parse_result.text
            model_result.type_name = self.model_type_name
            model_result.resolution = self.get_resolution(parse_result)

            model_results.append(model_result)

        return model_results

    def get_resolution(self, data: ParseResult):
        return {
            'value': data.resolution_str,
            'score': str(data.value)
        }


class MentionModel(AbstractSequenceModel):
    @property
    def model_type_name(self) -> str:
        return Constants.MODEL_MENTION


class URLModel(AbstractSequenceModel):
    @property
    def model_type_name(self) -> str:
        return Constants.MODEL_URL
