from .Node import Node
from .abstract_matcher import AbstractMatcher


class TrieTree(AbstractMatcher):

    def __init__(self):
        self.__root = Node()

    @property
    def root(self) -> Node():
        return self.__root

    def insert(self, value: [], _id: str) -> None:
        node = self.root

        for item in value:
            child = node[item]

            if child is None:
                child = node[item] = Node()

            node = child

        node.add_value(id)

    def init(self, values: [], ids: []) -> None:
        self.batch_insert(values, ids)
        self.convert_dict_to_list(self.root)

    def find(self, query_text: []) -> []:
        query_array = query_text

        i = 0
        while i < len(query_array):

            node = self.root
            j = i
            while j <= len(query_array):
                j += 1
