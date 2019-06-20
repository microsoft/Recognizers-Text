from typing import Dict, List

from recognizers_choice.choice.extractors import ChoiceExtractDataResult
from recognizers_choice.choice.constants import Constants
from recognizers_text.extractor import ExtractResult
from recognizers_text.parser import Parser, ParseResult


class ChoiceParseDataResult:
    score: float
    other_matches: List[ExtractResult]

    def __init__(self, score=0.0, other_matches=[]):
        self.score = score
        self.other_matches = other_matches


class OtherMatchParseResult:
    score: float
    text: str
    value: object

    def __init__(self, score, text, value):
        self.score = score
        self.text = text
        self.value = value


class ChoiceParserConfiguration:
    resolutions: Dict[str, object]


class ChoiceParser(Parser):
    config: ChoiceParserConfiguration

    def __init__(self, config: ChoiceParserConfiguration):
        self.config = config

    def parse(self, ext_result):
        result = ParseResult(ext_result)
        data = ChoiceExtractDataResult(ext_result.data)
        result.value = self.config.resolutions.get(result.type)
        result.data = ChoiceParseDataResult(data.score, [self.__to_other_match_result(m) for m in data.other_matches])
        return result

    def __to_other_match_result(self, ext_result):
        parse_result = ParseResult(ext_result)
        ext_data = ChoiceExtractDataResult(ext_result.Data)

        result = OtherMatchParseResult(ext_data.score,
                                       parse_result.text,
                                       self.config.resolutions.get(parse_result.type))
        return result


class BooleanParser(ChoiceParser):
    def __init__(self):
        res: Dict[str, bool] = {
            Constants.SYS_BOOLEAN_TRUE: True,
            Constants.SYS_BOOLEAN_FALSE: False,
        }

        config = ChoiceParserConfiguration()
        config.resolutions = res

        ChoiceParser.__init__(self, config)
