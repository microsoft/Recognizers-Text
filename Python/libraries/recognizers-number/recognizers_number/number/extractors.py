from abc import ABC, abstractmethod, abstractproperty
from typing import List, Pattern, Dict, Match, NamedTuple
from collections import namedtuple
import re

from recognizers_text.extractor import Extractor, ExtractResult

re_val=namedtuple('re_val', ['re', 'val'])

class BaseNumberExtractor(Extractor):
    @abstractproperty
    def regexes(self) -> List[NamedTuple('re_val', [('re', Pattern), ('val', str)])]:
        raise NotImplementedError

    @abstractproperty
    def _extract_type(self) -> str:
        raise NotImplementedError

    @abstractproperty
    def _negative_number_terms(self) -> Pattern:
        raise NotImplementedError

    def extract(self, source: str) -> List[ExtractResult]:
        if source is None or len(source.strip()) is 0:
            return list()
        result: List[ExtractResult]=list()
        matchSource: Dict[[Match], str]=dict()
        matched: List[bool]=list()
        value_item=namedtuple('value_item', ['matches', 'value'])
        values=list()
        for p in self.regexes:
            matches=re.findall(p.re, source)
            values.append(value_item(matches=matches, value=p.val))

        values = filter(lambda v:v.matches, values)

        for v in values:
            for m in v:
                for j in range(len(m)):
                    matched[m.index+j] = True
        
        # TODO: extractor
        return result