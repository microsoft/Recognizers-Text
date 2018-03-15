from abc import ABC, abstractmethod
from typing import List, Dict, Set, Pattern

from recognizers_text.extractor import Extractor, ExtractResult
from recognizers_number.culture import CultureInfo

class NumberWithUnitExtractorConfiguration(ABC):
    @property
    @abstractmethod
    def suffix_list(self) -> Dict[str, str]:
        raise NotImplementedError

    @property
    @abstractmethod
    def prefix_list(self) -> Dict[str, str]:
        raise NotImplementedError

    @property
    @abstractmethod
    def ambiguous_unit_list(self) -> List[str]:
        raise NotImplementedError

    @property
    @abstractmethod
    def extract_type(self) -> str:
        raise NotImplementedError

    @property
    def culture_info(self) -> CultureInfo:
        return self._culture_info

    @property
    @abstractmethod
    def unit_num_extractor(self) -> Extractor:
        raise NotImplementedError

    @property
    @abstractmethod
    def build_prefix(self) -> str:
        raise NotImplementedError

    @property
    @abstractmethod
    def build_suffix(self) -> str:
        raise NotImplementedError

    @property
    @abstractmethod
    def connector_token(self) -> str:
        raise NotImplementedError

    def __init__(self, culture_info: CultureInfo):
        self._culture_info = culture_info

class NumberWithUnitExtractor(Extractor):
    def __init__(self, config: NumberWithUnitExtractorConfiguration):
        self.config: NumberWithUnitExtractorConfiguration = config
        #TODO generate regexes

    def extract(self, source: str) -> List[ExtractResult]:
        pass

    def validate_unit(self, source: str) -> bool:
        pass

    def _pre_check_str(self, source: str) -> str:
        pass

    def _extract_separate_units(self, num_depend_source: List[ExtractResult]) -> List[ExtractResult]:
        #TODO use numDependSource and return numDependResult with all changes
        pass

    def _build_regex_from_set(self, definitions: List[str], ignore_case: bool = True) -> Set[Pattern]:
        pass

    def _build_separate_regex_from_config(self, ignore_case: bool = True) -> Pattern:
        pass

    def _dino_comparer(self, x: str, y: str) -> int:
        pass
