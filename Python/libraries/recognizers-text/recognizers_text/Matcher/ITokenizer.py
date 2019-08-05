from abc import ABC, abstractmethod


class Tokenizer(ABC):

    @abstractmethod
    def tokenize(self, _input: str) -> []:
        raise NotImplementedError
