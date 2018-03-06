from typing import List, Dict, Generic, TypeVar, Callable, Tuple, NamedTuple
from recognizers_text import Model, ModelResult

class NumberModel(Model):
    def __init__(self):
        pass
    
    def parse(self, query: str) -> List[ModelResult]:
        pass
