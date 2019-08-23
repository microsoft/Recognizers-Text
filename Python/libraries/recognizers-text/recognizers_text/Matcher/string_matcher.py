from .tokenizer import Tokenizer
from .match_strategy import MatchStrategy
from .simple_tokenizer import SimpleTokenizer
from .matcher import Matcher
from .trie_tree import TrieTree
from .ac_automaton import AcAutomaton
from multipledispatch import dispatch
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

    @dispatch(list)
    def init(self, values: []) -> None:
        self.init(values, list(map(lambda v: str(v), values)))

    @dispatch(dict)
    def init(self, values_dictionary: {}) -> None:
        values = []
        ids = []
        for item in values_dictionary:
            id = item
            for value in values_dictionary[item]:
                values.append(value)
                ids.append(id)

        self.init(values, ids)

    @dispatch(list, list)
    def init(self, values, ids: [] = []) -> None:
        tokenized_values = self.get_tokenized_text(values)
        self.matcher.init(tokenized_values, ids)

    @dispatch(list)
    def find(self, tokenized_query: []) -> []:
        return self.matcher.find(tokenized_query)

    @dispatch(str)
    def find(self, query_text: str = "") -> []:
        query_tokens = self.__tokenizer.tokenize(query_text)
        tokenized_query_text = list(map(lambda t: t.text, query_tokens))
        result = []
        for r in self.find(tokenized_query_text):
            start_token = query_tokens[r.start]
            end_token = query_tokens[r.start + r.length - 1]
            start = start_token.start
            length = end_token.end - start_token.start
            r_text = query_text[start: start + length]

            match_result = MatchResult()
            match_result.start = start
            match_result.length = length
            match_result.text = r_text
            match_result.canonical_values = r.canonical_values
            result.append(match_result)
        return result

    def get_tokenized_text(self, values: []) -> []:
        return list(map(lambda t: list(map(lambda i: i.text, self.tokenizer.tokenize(t))), values))
