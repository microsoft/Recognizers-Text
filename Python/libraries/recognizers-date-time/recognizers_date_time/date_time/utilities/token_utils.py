import regex

from typing import List, Pattern
from recognizers_text.extractor import Metadata, ExtractResult


class Token:
    def __init__(self, start: int, end: int, metadata: Metadata = None):
        self._start: int = start
        self._end: int = end
        self._metadata = metadata

    @property
    def length(self) -> int:
        if self._start > self._end:
            return 0
        return self._end - self._start

    @property
    def start(self) -> int:
        return self._start

    @start.setter
    def start(self, value) -> int:
        self._start = value

    @property
    def end(self) -> int:
        return self._end

    @end.setter
    def end(self, value) -> int:
        self._end = value

    @property
    def metadata(self):
        return self._metadata

    @metadata.setter
    def metadata(self, value):
        self._metadata = value


def merge_all_tokens(tokens: List[Token], source: str, extractor_name: str) -> List[ExtractResult]:
    result = []

    merged_tokens: List[Token] = list()
    tokens_ = sorted(filter(None, tokens), key=lambda x: x.start)

    for token in tokens_:
        add = True

        for index, m_token in enumerate(merged_tokens):
            if not add:
                break

            if token.start >= m_token.start and token.end <= m_token.end:
                add = False

            if m_token.start < token.start < m_token.end:
                add = False

            if token.start <= m_token.start and token.end >= m_token.end:
                add = False
                merged_tokens[index] = token

        if add:
            merged_tokens.append(token)

    for token in merged_tokens:
        start = token.start
        length = token.length
        sub_str = source[start: start + length]

        extracted_result = ExtractResult()
        extracted_result.start = start
        extracted_result.length = length
        extracted_result.text = sub_str
        extracted_result.type = extractor_name
        extracted_result.data = None
        extracted_result.meta_data = token.metadata

        result.append(extracted_result)

    return result


def get_tokens_from_regex(pattern: Pattern, source: str) -> List[Token]:
    return list(map(lambda x: Token(x.start(), x.end()), regex.finditer(pattern, source)))
