from typing import List

from recognizers_text.extractor import ExtractResult

class Token:
    def __init__(self, start: int, end: int):
        self.start: int = start
        self.end: int = end

    @property
    def length(self) -> int:
        return self.end - self.start

def merge_all_tokens(tokens: List[Token], source: str, extractor_name: str) -> List[ExtractResult]:
    merged_tokens: List[Token] = list()
    tokens_ = sorted(filter(None, tokens), key=lambda x: x.start, reverse=True)
    for token in tokens_:
        add = True
        for index, m_token in enumerate(merged_tokens):
            if not add:
                break
            if token.start >= m_token.start and token.end <= m_token.end:
                add = False
            if token.start > m_token.start and token.start < m_token.end:
                add = False
            if token.start <= m_token.start and token.end >= m_token.end:
                add = False
                merged_tokens[index] = token
        if add:
            merged_tokens.append(token)
    result: List[ExtractResult] = list(map(lambda x: __token_to_result(x, source, extractor_name), merged_tokens))
    return result

def __token_to_result(token: Token, source: str, name: str) -> ExtractResult:
    result: ExtractResult = ExtractResult()
    result.start = token.start
    result.length = token.length
    result.text = source[token.start, token.end]
    result.type = name
    return result
