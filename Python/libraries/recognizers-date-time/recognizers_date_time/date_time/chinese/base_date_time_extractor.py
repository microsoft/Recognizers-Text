from typing import List, Dict, Pattern, Match, Optional
from abc import ABC, abstractmethod
from datetime import datetime
import regex

from recognizers_text import ExtractResult
from ..extractors import DateTimeExtractor

class DateTimeExtra:
    def __init__(self):
        self.data_type: any = None
        self.named_entity: Dict[str, List[str]] = dict()
        self.match: Match = None

class TimeResult:
    def __init__(self, hour: int, minute: int, second: int, low_bound: int = -1):
        self.hour = hour
        self.minute = minute
        self.second = second
        self.low_bound = low_bound

class TimeResolutionUtils:
    @staticmethod
    def add_description(time_result: TimeResult, low_bound_map: Dict[str, int], description: str):
        if description in low_bound_map and time_result.hour < low_bound_map[description]:
            time_result.hour = time_result.hour + 12
            time_result.low_bound = low_bound_map[description]
        else:
            time_result.low_bound = 0

    @staticmethod
    def match_to_value(only_digit_match: Pattern, numbers_map: Dict[str, int], source: str) -> int:
        if not source.strip():
            return -1

        if regex.match(only_digit_match, source):
            return int(source)

        if len(source) == 1:
            return numbers_map[source]

        value = 1
        for index, char in enumerate(source):
            if char == '十':
                value = value * 10
            elif index == 0:
                value = value * numbers_map[char]
            else:
                value = value + numbers_map[char]

        return value

class ChineseBaseDateTimeExtractor(DateTimeExtractor):
    def __init__(self, regex_dict: Dict[Pattern, any]):
        self._regex_dict = regex_dict

    def extract(self, source: str, reference: datetime = None) -> List[ExtractResult]:

        if reference is None:
            reference = datetime.now()

        result: List[ExtractResult] = list()
        if not source:
            return result

        match_source: Dict[Match, any] = dict()
        matched: List[bool] = [False] * len(source)

        collections = list(map(lambda x: (list(regex.finditer(x[0], source)), x[1]), self._regex_dict.items()))
        collections = list(filter(lambda x: len(x[0]) > 0, collections))

        for collection in collections:
            for match in collection[0]:
                for j in range(len(match.group())):
                    matched[match.start() + j] = True
                match_source[match] = collection[1]

        last = -1
        for i in range(len(source)):
            if matched[i]:
                if (i + 1 == len(source) or not matched[i + 1]):
                    start = last + 1
                    length = i - last
                    text = source[start:start+length].strip()
                    src_match = next((x for x in iter(match_source) if (x.start() == start and (x.end() - x.start()) == length)), None)
                    if src_match:
                        value = ExtractResult()
                        value.start = start
                        value.length = length
                        value.text = text
                        value.type = self.extractor_type_name
                        value.data = self.__get_data(match_source, src_match)
                        result.append(value)
            else:
                last = i

        return result

    def __get_data(self, source: Dict[Match, any], key: Match) -> any:
        if key not in source:
            return None

        result = DateTimeExtra()
        result.data_type = source[key]
        result.named_entity = key.capturesdict()
        result.match = key
        return result