#  Copyright (c) Microsoft Corporation. All rights reserved.
#  Licensed under the MIT License.

from typing import Dict


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
