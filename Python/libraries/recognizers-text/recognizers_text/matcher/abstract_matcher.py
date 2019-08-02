from recognizers_text.matcher import matcher
from abc import abstractmethod


class AbstractMatcher(matcher):
    @property
    @abstractmethod
    def __init__(self, values: [str], ids: [str]):
        pass

    @property
    @abstractmethod
    def find(self, query_text: [str]) -> [str]:
        pass

    @property
    @abstractmethod
    def insert(self, value: [str], id: str):
        pass

    @property
    @abstractmethod
    def insert(self, value: [str], id: str):
        pass

    def is_match(self, query_text: [str]):
        pass

    def batch_insert(self, query_text: [str], ids: str):
        pass

    def convert_dict_to_list(self, node):
        pass
