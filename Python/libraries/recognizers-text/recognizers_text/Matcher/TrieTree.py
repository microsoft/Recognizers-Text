from typing import List

from recognizers_text import ModelResult

from .Node import Node
from .abstract_matcher import AbstractMatcher
from .match_result import MatchResult


class TrieTree(AbstractMatcher):

    def __init__(self):
        self.__root = Node()

    @property
    def root(self) -> Node:
        return self.__root

    def insert(self, value: [], id: str) -> None:
        node = self.root
        for item in value:
            child = node[item]

            if child is None:
                node[item] = Node()
                child = node[item]

            node = child

        node.add_value(id)

    def init(self, values: [], ids: []) -> None:
        self.batch_insert(values, ids)
        self.convert_dict_to_list(self.root)

    def find(self, query_text: []) -> []:
        query_array = query_text

        for i in range(0, len(query_array)):
            node = self.root

            j = i

            for j in range(j, len(query_array)):

                if node.end:
                    yield MatchResult(i, j - i, node.values)

                if j == len(query_array):
                    break

                text = query_array[j]

                if node[text] is None:
                    break

                node = node[text]
