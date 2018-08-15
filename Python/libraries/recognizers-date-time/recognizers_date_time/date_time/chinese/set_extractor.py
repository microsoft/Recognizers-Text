from typing import List, Pattern
from datetime import datetime
import regex

from recognizers_text import ExtractResult

from ..extractors import DateTimeExtractor
from ..utilities import Token, merge_all_tokens
from ..base_set import BaseSetExtractor
from .set_extractor_config import ChineseSetExtractorConfiguration

class ChineseSetExtractor(BaseSetExtractor):
    def __init__(self):
        super().__init__(ChineseSetExtractorConfiguration())

    def extract(self, source: str, reference: datetime = None) -> List[ExtractResult]:
        if reference is None:
            reference = datetime.now()

        tokens: List[Token] = list()
        tokens.extend(self.match_each_unit(source))
        tokens.extend(self.match_each_duration(source, reference))
        tokens.extend(self.match_each_specific(self.config.time_extractor, self.config.each_day_regex, source, reference))
        tokens.extend(self.match_each_specific(self.config.date_extractor, self.config.each_prefix_regex, source, reference))
        tokens.extend(self.match_each_specific(self.config.date_time_extractor, self.config.each_prefix_regex, source, reference))
        result = merge_all_tokens(tokens, source, self.extractor_type_name)
        return result

    def match_each_specific(self, extractor: DateTimeExtractor, pattern: Pattern, source: str, reference: datetime) -> List[Token]:
        for er in extractor.extract(source, reference):
            before_str = source[0:er.start]
            match = regex.search(pattern, before_str)
            if match:
                yield Token(match.start(), er.start + er.length)
