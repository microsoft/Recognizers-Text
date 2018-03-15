from abc import ABC, abstractmethod
from typing import Dict, List, Optional

from recognizers_text.extractor import Extractor, ExtractResult
from recognizers_text.parser import Parser, ParseResult
from recognizers_number.culture import CultureInfo

class NumberWithUnitParserConfiguration(ABC):
    @property
    @abstractmethod
    def internal_number_parser(self) -> Parser:
        raise NotImplementedError

    @property
    @abstractmethod
    def internal_number_extractor(self) -> Extractor:
        raise NotImplementedError

    @property
    @abstractmethod
    def connector_token(self) -> str:
        raise NotImplementedError

    def __init__(self, culture_info: CultureInfo):
        self.culture_info: CultureInfo = culture_info
        self.unit_map: Dict[str, str] = dict()

    def add_dict_to_unit_map(self, dictionary: Dict[str, str]):
        pass

class NumberWithUnitParser(Parser):
    def __init__(self, config: NumberWithUnitParserConfiguration):
        self.config: NumberWithUnitParserConfiguration = config

    def parse(self, source: ExtractResult) -> Optional[ParseResult]:
        pass

    def __add_if_not_contained(self, keys: List[str], new_key: str):
        pass
