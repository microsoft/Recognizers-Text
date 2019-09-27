from typing import List
from datetime import datetime

from recognizers_text import ExtractResult
from ..utilities import merge_all_tokens, Token, get_tokens_from_regex
from ..base_date import BaseDateExtractor
from .date_extractor_config import ChineseDateExtractorConfiguration
from .duration_extractor import ChineseDurationExtractor


class ChineseDateExtractor(BaseDateExtractor):
    def __init__(self):
        super().__init__(ChineseDateExtractorConfiguration())
        self.duration_extractor = ChineseDurationExtractor()

    def extract(self, source: str, reference: datetime = None) -> List[ExtractResult]:
        if reference is None:
            reference = datetime.now()

        tokens = self.basic_regex_match(source)
        tokens.extend(self.implicit_date(source))
        tokens.extend(self.duration_with_before_and_after(source, reference))

        result = merge_all_tokens(tokens, source, self.extractor_type_name)
        return result

    def duration_with_before_and_after(self, source: str, reference: datetime) -> List[Token]:
        ret: List[Token] = list()
        duration_results = self.duration_extractor.extract(source, reference)

        for er in duration_results:
            pos = er.start + er.length
            if pos < len(source) and source[pos] in ['前', '后']:
                ret.append(Token(er.start, pos + 1))

        return ret

    def basic_regex_match(self, source: str) -> List[Token]:
        ret: List[Token] = list()

        for regexp in self.config.date_regex_list:
            ret.extend(get_tokens_from_regex(regexp, source))

        return ret

    def implicit_date(self, source: str) -> List[Token]:
        ret: List[Token] = list()

        for regexp in self.config.implicit_date_list:
            ret.extend(get_tokens_from_regex(regexp, source))

        return ret
