from .tokenizer import Tokenizer
from .match_strategy import MatchStrategy
from .simple_tokenizer import SimpleTokenizer
from .matcher import Matcher
from .trie_tree import TrieTree
from .ac_automaton import AcAutomaton
from .match_result import MatchResult


class StringMatcher:

    def __init__(self, match_strategy: MatchStrategy = MatchStrategy.TrieTree, tokenizer: Tokenizer = None):
        self.tokenizer = tokenizer or SimpleTokenizer()
        self.__matcher = self.switch_match(match_strategy)

    @staticmethod
    def switch_match(match_strategy: MatchStrategy):
        if match_strategy == MatchStrategy.AcAutomaton:
            return AcAutomaton()

        elif match_strategy == MatchStrategy.TrieTree:
            return TrieTree()

        else:
            raise ValueError('Unsupported match strategy: {}'.format(match_strategy))

    @property
    def tokenizer(self) -> Tokenizer:
        return self.__tokenizer

    @tokenizer.setter
    def tokenizer(self, tokenizer) -> None:
        self.__tokenizer = tokenizer

    @property
    def matcher(self) -> Matcher:
        return self.__matcher

    @matcher.setter
    def matcher(self, matcher) -> None:
        self.__matcher = matcher

    def init(self, values, ids: [] = None):
        if values and type(values) is list:
            _ids = list(map(lambda v: str(v), values)) if not ids else ids
            tokenized_values = self.get_tokenized_text(values)
            self.matcher.init(tokenized_values, _ids)
        elif values and type(values) is dict:
            values_dictionary = []
            for item in values_dictionary:
                id = item
                for value in values[item]:
                    values_dictionary.append(value)
                    ids.append(id)

            self.init(values, ids)

    def find(self, tokenized_query) -> []:
        if type(tokenized_query) is list:
            return self.matcher.find(tokenized_query)
        query_tokens = self.__tokenizer.tokenize(tokenized_query)
        tokenized_query_text = list(map(lambda t: t.text, query_tokens))
        result = []
        for r in self.find(tokenized_query_text):
            start_token = query_tokens[r.start]
            end_token = query_tokens[r.start + r.length - 1]
            start = start_token.start
            length = end_token.end - start_token.start
            r_text = tokenized_query[start: start + length]

            match_result = MatchResult()
            match_result.start = start
            match_result.length = length
            match_result.text = r_text
            match_result.canonical_values = r.canonical_values
            result.append(match_result)
        return result

    def get_tokenized_text(self, values: []) -> []:
        return list(map(lambda t: list(map(lambda i: i.text, self.tokenizer.tokenize(t))), values))
