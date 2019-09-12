from abc import ABC, abstractmethod
from typing import List
from recognizers_text.model import ModelResult


class Matcher(ABC):

    @abstractmethod
    def init(self, values: [], ids: []) -> None:
        raise NotImplementedError

    @abstractmethod
    def find(self, query_text: []) -> []:
        raise NotImplementedError
