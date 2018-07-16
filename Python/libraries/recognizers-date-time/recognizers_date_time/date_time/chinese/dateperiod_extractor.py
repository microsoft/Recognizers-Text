from typing import List
from datetime import datetime
import regex

from recognizers_text import ExtractResult

from ..base_dateperiod import BaseDatePeriodExtractor
from ..utilities import merge_all_tokens, Token
from .dateperiod_extractor_config import ChineseDatePeriodExtractorConfiguration

class ChineseDatePeriodExtractor(BaseDatePeriodExtractor):
    def __init__(self):
        super().__init__(ChineseDatePeriodExtractorConfiguration())

    def extract(self, source: str, reference: datetime = None) -> List[ExtractResult]:

        if not reference:
            reference = datetime.now()

        tokens = []
        tokens += self.match_simple_cases(source)
        tokens += self.merge_two_time_points(source, reference)
        tokens += self.match_number_with_unit(source)
        result = merge_all_tokens(tokens, source, self.extractor_type_name)

        return result

    def match_number_with_unit(self, source: str) -> List[Token]:
        tokens: List[Token] = list()
        durations: List[Token] = list()

        for er in self.config.integer_extractor.extract(source):
            after_str = source[er.start + er.length:]
            followed_unit_match = regex.search(self.config.followed_unit, after_str)

            if followed_unit_match and followed_unit_match.start() == 0:
                durations.append(Token(er.start, er.start + er.length + len(followed_unit_match.group())))

        for match in regex.finditer(self.config.number_combined_with_unit, source):
            durations.append(Token(match.start(), match.end()))

        for duration in durations:
            before_str = source[:duration.start].lower()

            if not before_str.strip():
                continue

            match = regex.search(self.config.past_regex, before_str)

            if match and not before_str[match.end():].strip():
                tokens.append(Token(match.start(), duration.end))
                continue

            match = regex.search(self.config.future_regex, before_str)

            if match and not before_str[match.end():].strip():
                tokens.append(Token(match.start(), duration.end))

        return tokens
