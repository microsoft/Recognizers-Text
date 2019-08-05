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
