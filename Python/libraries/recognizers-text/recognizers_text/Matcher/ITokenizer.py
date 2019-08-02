from abc import ABC, abstractmethod
from typing import TypeVar


class ITokenizer(ABC):

    T = TypeVar('T')

    @abstractmethod
    def tokenize(self, _input: str) -> list():
        raise NotImplementedError
