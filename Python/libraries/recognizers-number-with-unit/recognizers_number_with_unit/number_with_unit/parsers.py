from abc import ABC, abstractmethod
from typing import Dict, List, Optional
from collections import namedtuple

from recognizers_text.extractor import Extractor, ExtractResult
from recognizers_text.parser import Parser, ParseResult
from recognizers_number.culture import CultureInfo

UnitValue = namedtuple('UnitValue', ['number', 'unit'])

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
        if not dictionary:
            return
        for key, value in dictionary.items():
            if not key:
                continue

            values = value.strip().split('|')
            for token in values:
                if token and token not in self.unit_map:
                    self.unit_map[token] = key

class NumberWithUnitParser(Parser):
    def __init__(self, config: NumberWithUnitParserConfiguration):
        self.config: NumberWithUnitParserConfiguration = config

    def parse(self, source: ExtractResult) -> Optional[ParseResult]:
        ret = ParseResult(source)
        number_result = None
        if source.data and isinstance(source.data, ExtractResult):
            number_result = source.data
        else: # if there is no unitResult, means there is just unit
            number_result = ExtractResult()
            number_result.start = -1
            number_result.length = 0
            number_result.text = None
            number_result.type = None
        #key contains units
        key = source.text
        unit_key_build = ''
        unit_keys = []
        i = 0
        while i <= len(key):
            if i == len(key):
                if unit_key_build:
                    self.__add_if_not_contained(unit_keys, unit_key_build.strip())
            # number_result.start is a relative position
            elif i == number_result.start:
                if unit_key_build:
                    self.__add_if_not_contained(unit_keys, unit_key_build.strip())
                    unit_key_build = ''
                if number_result.length:
                    i = number_result.start + number_result.length - 1
            else:
                unit_key_build += key[i]
            i += 1

        #Unit type depends on last unit in suffix.
        last_unit = unit_keys[-1].lower()
        if self.config.connector_token and last_unit.startswith(self.config.connector_token):
            last_unit = last_unit[len(self.config.connector_token):].strip()
        if key and self.config.unit_map and last_unit in self.config.unit_map:
            unit_value = self.config.unit_map[last_unit]
            num_value = self.config.internal_number_parser.parse(number_result) if number_result.text else None
            resolution_str = num_value.resolution_str if num_value else None

            ret.value = UnitValue(
                number=resolution_str,
                unit=unit_value)
            ret.resolution_str = (f'{resolution_str} {unit_value}').strip()

        return ret

    def __add_if_not_contained(self, keys: List[str], new_key: str):
        if not [x for x in keys if new_key in x]:
            keys.append(new_key)
