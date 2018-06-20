from abc import ABC, abstractmethod
from typing import Dict, List, Optional
from collections import namedtuple
from decimal import Decimal

from .constants import *
from .utilities import DictionaryUtility
from recognizers_text.extractor import Extractor, ExtractResult
from recognizers_text.parser import Parser, ParseResult
from recognizers_number.culture import CultureInfo
from recognizers_number_with_unit.resources.base_currency import BaseCurrency 

UnitValue = namedtuple('UnitValue', ['number', 'unit'])
CurrencyUnitValue = namedtuple('UnitValue', ['number', 'unit', 'iso_currency'])

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
        self.currency_fraction_num_map = BaseCurrency.CurrencyFractionalRatios
        self.currency_fraction_mapping = BaseCurrency.CurrencyFractionMapping
        self.currency_name_to_iso_code_map = dict()
        self.currency_fraction_code_list = dict()


    def add_dict_to_unit_map(self, dictionary: Dict[str, str]):
        DictionaryUtility.bind_dictionary(dictionary, self.unit_map)

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

class BaseCurrencyParser(Parser):
    def __init__(self, config: NumberWithUnitParserConfiguration):
        self.config = config
        self.number_with_unit_parser = NumberWithUnitParser(config)

    def parse(self, source: ExtractResult) -> Optional[ParseResult]:
        if isinstance(source.data, List):
            ret = self.__merge_compound_unit(source)
        else:
            ret = self.number_with_unit_parser.parse(source)

            main_unit_iso_code = self.config.currency_name_to_iso_code_map.get(ret.value.unit, None) if ret.value else None
            if main_unit_iso_code and main_unit_iso_code.startswith(Constants.FAKE_ISO_CODE_PREFIX):
                ret.value = UnitValue(ret.value.number, ret.value.unit) if ret.value else None
            else:
                ret.value =  CurrencyUnitValue(ret.value.number, ret.value.unit, main_unit_iso_code)

        return ret

    def __merge_compound_unit(self, compound_result: ExtractResult) -> ParseResult:
        results = []
        compound_unit = compound_result.data

        count = 0
        result = None
        number_value = 0.0
        main_unit_value = ''
        main_unit_iso_code = ''
        fraction_unit_string = ''

        idx = 0
        while idx < len(compound_unit):
            extract_result = compound_unit[idx]
            parse_result = self.number_with_unit_parser.parse(extract_result)
            parse_result_value = parse_result.value
            unit_value = parse_result_value.unit if parse_result_value else None

            # Process a new group
            if count == 0:
                if not extract_result.type == Constants.SYS_UNIT_CURRENCY:
                    idx = idx + 1
                    continue

                # Initialize a new result
                result = ParseResult()
                result.start = extract_result.start
                result.length = extract_result.length
                result.text = extract_result.text
                result.type = extract_result.type
            
                main_unit_value = unit_value
                if parse_result_value and parse_result_value.number:
                    number_value = float(parse_result_value.number)
                result.resolution_str = parse_result.resolution_str

                main_unit_iso_code = self.config.currency_name_to_iso_code_map.get(unit_value, None)
                # If the main unit can't be recognized, finish process this group.
                if not main_unit_iso_code:
                    result.value = UnitValue(str(number_value), main_unit_value)
                    results.append(result)
                    result = None
                    idx = idx + 1
                    continue
                
                fraction_units_string = self.config.currency_fraction_mapping.get(main_unit_iso_code)
            else:
                if extract_result.type == Constants.SYS_NUM:
                    number_value = number_value + float(parse_result.value) * (1 / 100)
                    result.resolution_str = result.resolution_str + ' ' + parse_result.resolution_str
                    result.length = parse_result.start + parse_result.length - result.start
                    count = count + 1
                    idx = idx + 1
                    continue

                fraction_unit_code = self.config.currency_fraction_code_list.get(unit_value, None)
                fraction_num_value = self.config.currency_fraction_num_map.get(parse_result_value.unit, None) if parse_result_value else None

                if fraction_unit_code and fraction_num_value != 0 and self.__check_units_string_contains(fraction_unit_code, fraction_units_string):
                    number_value = number_value + (float(parse_result_value.number) * (1 / fraction_num_value) if parse_result_value else 0)
                    result.resolution_str = result.resolution_str + ' ' + parse_result.resolution_str
                    result.length = parse_result.start + parse_result.length - result.length
                else:
                    if result:
                        if not main_unit_iso_code or main_unit_iso_code.startswith(Constants.FAKE_ISO_CODE_PREFIX):
                            result.value = UnitValue(str(number_value), main_unit_value)
                        else:
                            result.value = CurrencyUnitValue(str(number_value), main_unit_value, main_unit_iso_code)
                        
                        results.append(result)
                        result = None
                    count = 0
                    idx = idx - 1
                    continue
            
            count = count + 1
            idx = idx + 1

        if result:
            if not main_unit_iso_code or main_unit_iso_code.startswith(Constants.FAKE_ISO_CODE_PREFIX):
                result.value = UnitValue(str(number_value), main_unit_value)
            else:
                result.value = CurrencyUnitValue(str(number_value), main_unit_value, main_unit_iso_code)

            results.append(result)
        
        self.__resolve_text(results, compound_result.text, compound_result.start)

        ret = ParseResult(compound_result)
        ret.value = results
        return ret
    
    def __check_units_string_contains(self, fraction_unit_code: str, fraction_units_string: str) -> bool:
        units_map = dict()
        DictionaryUtility.bind_units_string(units_map, '', fraction_units_string)
        if fraction_unit_code in units_map:
            return True
        
        return False

    def __resolve_text(self, prs: List[ParseResult], source: str, bias: int):
        for parse_result in prs:
            if parse_result.start and parse_result.length:
                parse_result.text = source[parse_result.start - bias:parse_result.length]

class BaseMergedUnitParser(Parser):
    def __init__(self, config: NumberWithUnitParserConfiguration):
        self.config = config
        self.number_with_unit_parser = NumberWithUnitParser(config)

    def parse(self, source: ExtractResult) -> Optional[ParseResult]:
        if source.type == Constants.SYS_UNIT_CURRENCY:
            pr = BaseCurrencyParser(self.config).parse(source)
        else:
            pr = self.number_with_unit_parser.parse(source)

        return pr
