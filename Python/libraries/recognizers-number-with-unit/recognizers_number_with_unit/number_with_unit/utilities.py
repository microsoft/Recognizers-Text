#  Copyright (c) Microsoft Corporation. All rights reserved.
#  Licensed under the MIT License.

from typing import Dict, List, Pattern
from recognizers_text import ExtractResult, RegExpUtility


class DictionaryUtility():
    # Safely bind dictionary which contains several key-value pairs to the destination dictionary.
    # This function is used to bind all the prefix and suffix for units.
    @staticmethod
    def bind_dictionary(dictionary: Dict[str, str], source_dictionary: Dict[str, str]):
        if not dictionary:
            return
        for key, value in dictionary.items():
            if not key:
                continue

            DictionaryUtility.bind_units_string(source_dictionary, key, value)

    # Bind keys in a string which contains words separated by '|'.
    @staticmethod
    def bind_units_string(source_dictionary: Dict[str, str], key: str, source: str):
        values = source.strip().split('|')
        for token in values:
            if not token or token in source_dictionary:
                continue
            source_dictionary[token] = key


class Token:
    def __init__(self, start: int, end: int):
        self._start: int = start
        self._end: int = end

    @property
    def length(self) -> int:
        if self._start > self._end:
            return 0
        return self._end - self._start

    @property
    def start(self) -> int:
        return self._start

    @start.setter
    def start(self, value) -> int:
        self._start = value

    @property
    def end(self) -> int:
        return self._end

    @end.setter
    def end(self, value) -> int:
        self._end = value


class CommonUtils:
    #  Expand patterns with 'half' suffix in CJK implementation.
    @staticmethod
    def expand_half_suffix(source: str, result: List[ExtractResult], numbers: List[ExtractResult],
                           half_unit_regex: Pattern) -> List[ExtractResult]:
        if half_unit_regex and numbers:
            match: List[ExtractResult] = []

            for number in numbers:
                if len(RegExpUtility.get_matches(half_unit_regex, number.text)) == 1:
                    match.append(number)
            if len(match) > 0:
                res: List[ExtractResult] = []
                for er in result:
                    start = int(er.start)
                    length = int(er.length)
                    match_suffix: List[ExtractResult] = []
                    for mr in match:
                        #  Take into account possible whitespaces between result and half unit.
                        if int(mr.start) - (start + length) >= 0:
                            sub_length = int(mr.start) - (start + length)
                        else:
                            sub_length = 0
                        mid_str = source[start+length:sub_length]
                        if not mid_str and int(mr.start) - (start + length) >= 0:
                            match_suffix.append(mr)

                    if len(match_suffix) == 1:
                        mr = match_suffix[0]
                        suffix_length = int(mr.start) + int(mr.length) - (start + length)
                        er.length += suffix_length
                        er.text += source[start+length:suffix_length]
                        tmp = ExtractResult()
                        tmp.data = er.data
                        er.data = [tmp, mr]
                    res.append(er)
                result = res
        return result


