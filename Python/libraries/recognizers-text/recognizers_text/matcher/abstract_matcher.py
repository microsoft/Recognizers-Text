from recognizers_text.matcher import matcher
from abc import abstractmethod


class AbstractMatcher(matcher):
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
        pass

    def batch_insert(self, query_text: [str], ids: str):
        pass

    def convert_dict_to_list(self, node):
        pass
