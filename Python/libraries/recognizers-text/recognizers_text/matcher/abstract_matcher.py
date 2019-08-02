from abc import ABC, abstractmethod
from typing import List
from recognizers_text.model import ModelResult


class Matcher(ABC):
    @property
    @abstractmethod
    def model_type_name(self) -> str:
        pass

    @abstractmethod
    def parse(self, source: str) -> List[ModelResult]:
        pass


class AbstractMatcher(Matcher):
    @property
    @abstractmethod
    def __init__(self, values: List[str], ids: List[str]):
        pass

    @property
    @abstractmethod
    def find(self, query_text: List[str]) -> List[str]:
        pass

    @property
    @abstractmethod
    def insert(self, value: List[str], id: str):
        pass

    @property
    @abstractmethod
    def insert(self, value: List[str], id: str):
        pass

    def is_match(self, query_text: List[str]):
        pass

    def batch_insert(self, query_text: List[str], ids: str):
        pass

    def convert_dict_to_list(self, node):
        pass
