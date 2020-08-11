from abc import ABC
from typing import List, Dict, Pattern

from grapheme.api import slice
import regex
from recognizers_text import StringUtility, RegExpUtility
from recognizers_text.extractor import Extractor, ExtractResult

from .constants import *


class ChoiceExtractDataResult:
    source: str
    score: float
    other_matches: List[ExtractResult]

    def __init__(self, source='', score=0.0, other_matches=[]):
        self.source = source
        self.score = score
        self.other_matches = other_matches


class ChoiceExtractorConfiguration(ABC):
    regexes_map = Dict[Pattern, str]
    token_regex: Pattern
    allow_partial_match: bool
    max_distance: int
    only_top_match: bool


class ChoiceExtractor(Extractor):
    extract_type: str

    def __init__(self, config: ChoiceExtractorConfiguration):
        self.config = config

    def extract(self, source: str):
        results: List[ExtractResult] = list()
        partial_results: List[ExtractResult] = list()
        trimmed_source = source.lower()

        if source is None or source.strip() == '':
            return results
        source_tokens = self.__tokenize(trimmed_source)

        for (regexp, type_extracted) in self.config.regexes_map.items():
            for match in RegExpUtility.get_matches(regexp, trimmed_source):
                match_tokens = self.__tokenize(match)
                top_score = 0.0

                for i in range(len(source_tokens)):
                    score = self.match_value(source_tokens, match_tokens, i)
                    top_score = max(top_score, score)

                if top_score > 0.0:
                    value = ExtractResult()
                    start = trimmed_source.index(match)
                    length = len(match)
                    text = source[start: start + length].strip()
                    value.start = start
                    value.length = length
                    value.text = text
                    value.type = type_extracted
                    value.data = ChoiceExtractDataResult(source, top_score)

                    partial_results.append(value)

        if len(partial_results) == 0:
            return results

        partial_results = sorted(partial_results, key=lambda res: res.start)

        if self.config.only_top_match:
            top_score = 0.0
            top_result_index = 0
            for i in range(len(partial_results)):
                data = ChoiceExtractDataResult(
                    source, partial_results[i].data.score)
                if data.score > top_score:
                    top_score = data.score
                    top_result_index = i

            top_result = ChoiceExtractDataResult(partial_results[top_result_index].data.source,
                                                 partial_results[top_result_index].data.score)
            top_result.other_matches = partial_results
            results.append(partial_results[top_result_index])
        else:
            results = partial_results

        return results

    def match_value(self, source: List[str], match: List[str], start_pos: int) -> float:
        matched = 0
        total_deviation = 0
        for match_token in match:
            pos = StringUtility.index_of(source, match_token, start_pos)

            if pos >= 0:
                distance = pos - start_pos if matched > 0 else 0

                if distance <= self.config.max_distance:
                    matched = matched + 1
                    total_deviation = total_deviation + distance
                    start_pos = pos + 1

        score = 0.0

        if matched > 0 and (matched == len(match) or self.config.allow_partial_match):
            completeness = matched / len(match)
            accuracy = completeness * (matched / (matched + total_deviation))
            initial_score = accuracy * (matched / len(source))
            score = 0.4 + (0.6 * initial_score)
        return score

    def __tokenize(self, source: str) -> List[str]:
        tokens = []
        chars = slice(source)

        token: str = ''
        pattern = regex.compile(self.config.token_regex)
        for char in chars:
            if StringUtility.is_emoji(char):
                tokens.append(char)
                if not (token is None or token.strip() == ''):
                    tokens.append(token)
                    token = ''
            elif not (pattern.search(char) is not None or chars.strip() == ''):
                token = token + char
            elif token != '' or token.strip() != '':
                tokens.append(token)
                token = ''

        if token != '' or token.strip() != '':
            tokens.append(token)
            token = ''

        return tokens


class BooleanExtractorConfiguration(ABC):
    regex_true: Pattern
    regex_false: Pattern
    token_regex: Pattern
    only_top_match: bool

    def __init__(self, regex_true, regex_false, token_regex, only_top_match):
        self.regex_true = RegExpUtility.get_safe_reg_exp(regex_true)
        self.regex_false = RegExpUtility.get_safe_reg_exp(regex_false)
        self.token_regex = RegExpUtility.get_safe_reg_exp(token_regex)
        self.only_top_match = only_top_match


class BooleanExtractor(ChoiceExtractor):
    booleanTrue = Constants.SYS_BOOLEAN_TRUE
    booleanFalse = Constants.SYS_BOOLEAN_FALSE

    def __init__(self, config: BooleanExtractorConfiguration):
        regexes_map = {}
        update_values = {
            config.regex_true: Constants.SYS_BOOLEAN_TRUE,
            config.regex_false: Constants.SYS_BOOLEAN_FALSE
        }
        regexes_map.update(update_values)
        options_config = ChoiceExtractorConfiguration()
        options_config.regexes_map = regexes_map
        options_config.token_regex = config.token_regex
        options_config.allow_partial_match = False
        options_config.max_distance = 2
        options_config.only_top_match = config.only_top_match

        ChoiceExtractor.__init__(self, options_config)
        self.extract_type = Constants.SYS_BOOLEAN
