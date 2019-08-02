from abc import ABC, abstractmethod
from typing import TypeVar


class IMatcher(ABC):

    T = TypeVar('T')

    @abstractmethod
    def init(self, values: list()[T], ids: []) -> None:
        raise NotImplementedError

    @abstractmethod
    def find(self, query_text: list()[T]) -> list()[T]:
        raise NotImplementedError
