from abc import ABC, abstractmethod
from typing import List, Dict, Set, Pattern, Match
from copy import deepcopy
from collections import namedtuple
from itertools import chain
import regex

from recognizers_text.utilities import RegExpUtility
from recognizers_text.extractor import Extractor, ExtractResult
from recognizers_number.culture import CultureInfo

PrefixUnitResult = namedtuple('PrefixUnitResult', ['offset', 'unit'])

class NumberWithUnitExtractorConfiguration(ABC):
    @property
    @abstractmethod
    def extract_type(self) -> str:
        raise NotImplementedError

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

    @property
    def culture_info(self) -> CultureInfo:
        return self._culture_info

    def __init__(self, culture_info: CultureInfo):
        self._culture_info = culture_info

class NumberWithUnitExtractor(Extractor):
    def __init__(self, config: NumberWithUnitExtractorConfiguration):
        self.config: NumberWithUnitExtractorConfiguration = config
        if self.config.suffix_list:
            self.suffix_regex: Set[Pattern] = self._build_regex_from_set(self.config.suffix_list.values())
        else:
            self.suffix_regex: Set[Pattern] = set()

        if self.config.prefix_list:
            max_length = max(map(len, ('|'.join(self.config.prefix_list.values()).split('|'))))

            self.max_prefix_match_len = max_length + 2
            self.prefix_regex: Set[Pattern] = self._build_regex_from_set(self.config.prefix_list.values())
        else:
            self.max_prefix_match_len = 0
            self.prefix_regex: Set[Pattern] = set()
        self.separate_regex = self._build_separate_regex_from_config()

    def extract(self, source: str) -> List[ExtractResult]:
        if not self._pre_check_str(source):
            return list()

        mapping_prefix: Dict[float, PrefixUnitResult] = dict()
        matched: List[bool] = [False] * len(source)
        numbers: List[ExtractResult] = self.config.unit_num_extractor.extract(source)
        result: List[ExtractResult] = list()
        source_len = len(source)

        if self.max_prefix_match_len != 0:
            for num in numbers:
                if num.start is None or num.length is None:
                    continue
                max_find_prefix = min(self.max_prefix_match_len, num.start)
                if max_find_prefix == 0:
                    continue

                left: str = source[num.start - max_find_prefix:num.start]
                last_index = len(left)
                best_match: Match = None
                for pattern in self.prefix_regex:
                    collection = list(filter(lambda x: len(x.group()), regex.finditer(pattern, left)))
                    for match in collection:
                        if left[match.start():last_index].strip() == match.group():
                            if best_match is None or best_match.start() >= match.start():
                                best_match = match
                if best_match:
                    mapping_prefix[num.start] = PrefixUnitResult(
                        offset=last_index - best_match.start(),
                        unit=left[best_match.start():last_index]
                    )
        for num in numbers:
            if num.start is None or num.length is None:
                continue
            start = num.start
            length = num.length
            max_find_len = source_len - start - length

            prefix_unit: PrefixUnitResult = mapping_prefix.get(start, None)

            if max_find_len > 0:
                right = source[start + length:start + length + max_find_len]
                unit_match_list = map(lambda x: list(regex.finditer(x, right)), self.suffix_regex)
                unit_match = chain.from_iterable(unit_match_list)
                unit_match = list(filter(lambda x: x.group(), unit_match))

                max_len = 0
                for match in unit_match:
                    if match.group():
                        end_pos = match.start() + len(match.group())
                        if match.start() >= 0:
                            middle: str = right[:min(match.start(), len(right))]
                            if max_len < end_pos and (not middle.strip() or middle.strip() == self.config.connector_token):
                                max_len = end_pos
                if max_len != 0:
                    for i in range(length + max_len):
                        matched[i+start] = True
                    ex_result = ExtractResult()
                    ex_result.start = start
                    ex_result.length = length + max_len
                    ex_result.text = source[start:start+length+max_len]
                    ex_result.type = self.config.extract_type

                    if prefix_unit:
                        ex_result.start -= prefix_unit.offset
                        ex_result.length += prefix_unit.offset
                        ex_result.text = prefix_unit.unit + ex_result.text

                    num.start = start - ex_result.start
                    ex_result.data = num
                    result.append(ex_result)
                    continue
            if prefix_unit:
                ex_result = ExtractResult()
                ex_result.start = num.start - prefix_unit.offset
                ex_result.length = num.length + prefix_unit.offset
                ex_result.text = prefix_unit.unit + num.text
                ex_result.type = self.config.extract_type

                num.start = start - ex_result.start
                ex_result.data = num
                result.append(ex_result)

        if self.separate_regex:
            result = self._extract_separate_units(source, result)

        return result

    def validate_unit(self, source: str) -> bool:
        return not source.startswith('-')

    def _pre_check_str(self, source: str) -> bool:
        return len(source) != 0

    def _extract_separate_units(self, source: str, num_depend_source: List[ExtractResult]) -> List[ExtractResult]:
        result = deepcopy(num_depend_source)
        match_result: List[bool] = [False] * len(source)
        for ex_result in num_depend_source:
            for i in range(ex_result.start, ex_result.end+1):
                match_result[i] = True
        match_collection = list(filter(lambda x: x.group(), regex.finditer(self.separate_regex, source)))
        for match in match_collection:
            i = 0
            while i < len(match.group()) and not match_result[match.start()+i]:
                i += 1
            if i == len(match.group()):
                for j in range(i):
                    match_result[j] = True
                to_add = ExtractResult()
                to_add.start = match.start()
                to_add.length = len(match.group())
                to_add.text = match.group()
                to_add.type = self.config.extract_type
                result.append(to_add)
        return result

    def _build_regex_from_set(self, definitions: List[str], ignore_case: bool = True) -> Set[Pattern]:
        return set(map(lambda x: self.__build_regex_from_str(x, ignore_case), definitions))

    def __build_regex_from_str(self, source: str, ignore_case: bool) -> Pattern:
        tokens = map(regex.escape, source.split('|'))
        definition = '|'.join(tokens)
        definition = f'{self.config.build_prefix}({definition}){self.config.build_suffix}'
        flags = regex.S + regex.I if ignore_case else regex.S
        return RegExpUtility.get_safe_reg_exp(definition, flags)

    def _build_separate_regex_from_config(self, ignore_case: bool = True) -> Pattern:
        separate_words: Set[str] = set()
        for add_word in self.config.prefix_list.values():
            separate_words |= set(filter(self.validate_unit, add_word.split('|')))
        for add_word in self.config.suffix_list.values():
            separate_words |= set(filter(self.validate_unit, add_word.split('|')))
        for to_delete in self.config.ambiguous_unit_list:
            separate_words.discard(to_delete)

        tokens = map(regex.escape, separate_words)
        if not tokens:
            return None

        tokens = sorted(tokens, key=len, reverse=True)
        definition = '|'.join(tokens)
        definition = f'{self.config.build_prefix}({definition}){self.config.build_suffix}'
        flags = regex.S + regex.I if ignore_case else regex.S
        return RegExpUtility.get_safe_reg_exp(definition, flags)

    def _dino_comparer(self, x: str, y: str) -> int:
        if not x:
            if not y:
                return 0
            else:
                return 1
        else:
            if not y:
                return -1
            else:
                if len(x) != len(y):
                    return len(y) - len(x)
                else:
                    if x.lower() < y.lower():
                        return -1
                    if y.lower() < x.lower():
                        return 1
                    return 0
