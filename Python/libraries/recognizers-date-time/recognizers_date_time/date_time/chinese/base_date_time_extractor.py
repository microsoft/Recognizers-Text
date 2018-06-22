from typing import List, Dict, Pattern, Match
from abc import ABC, abstractmethod
from datetime import datetime
import regex

from recognizers_text import ExtractResult
from ..extractors import DateTimeExtractor

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

    def __get_data(self, source: Dict[Match, any], key: Match) -> any:
        if key not in source:
            return None
        
        result: any
        result.data_type = source[key]
        result.named_entity = key.groupdict()
        return result