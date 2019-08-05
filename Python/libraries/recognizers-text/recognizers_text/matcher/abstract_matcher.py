from .matcher import Matcher
from abc import abstractmethod


class AbstractMatcher(Matcher):
    @abstractmethod
    def init(self, values: [str], ids: [str]):
        pass

    @abstractmethod
    def find(self, query_text: [str]) -> [str]:
        pass

    @abstractmethod
    def insert(self, value: [str], id: str):
        pass

    def is_match(self, query_text: [str]):
        result = next((e for e in self.find(query_text) if e is None), None)
        return result

    def batch_insert(self, values: [], ids: []):
        if len(values) != len(ids):
            raise Exception('Lengths of Values and Ids are different.')

        for i in len(values):
            self.insert(values[i], ids[i])
            i += 1

    def convert_dict_to_list(self, node):
        if node.children is None:
            return

        for kvp in node.children:
            self.convert_dict_to_list(kvp.value)

        node.children = {node.children}
