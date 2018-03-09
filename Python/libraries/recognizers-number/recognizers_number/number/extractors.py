from abc import ABC, abstractmethod, abstractproperty
from typing import List, Pattern, Dict, Match, NamedTuple
from collections import namedtuple
import regex

from recognizers_number.number.models import long_format_type
from recognizers_text.extractor import Extractor, ExtractResult
from recognizers_number.resources.base_numbers import BaseNumbers

re_val=namedtuple('re_val', ['re', 'val'])
matches_val=namedtuple('matches_val', ['matches', 'val'])

class BaseNumberExtractor(Extractor):
    @abstractproperty
    def regexes(self) -> List[re_val]:
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
        match_source: Dict[[Match], str]=dict()
        matched: List[bool]=[False] * len(source)

        matches_list = map(lambda x : matches_val(matches=list(regex.finditer(x.re, source)), val=x.val), self.regexes)
        matches_list = filter(lambda x : x.matches is not None, matches_list)

        for ml in matches_list:
            for m in ml.matches:
                for j in range(len(m.group())):
                    matched[m.start()+j]=True
                # Keep Source Data for extra information
                match_source[m]=ml.val

        last = -1
        for i in range(len(source)):
            if not matched[i]:
                last = i
            else:
                if (i+1 == len(source) or not matched[i+1]):
                    start = last+1
                    length = i-last
                    substr = source[start:start+length].strip()
                    srcmatch = next(x for x in iter(match_source) if (x.start() == start and (x.end() - x.start()) == length))

                    # extract negative numbers
                    if self._negative_number_terms is not None:
                        match = regex.search(self._negative_number_terms, source[0:start])
                        if match is not None:
                            start = match.start()
                            length = length + match.end() - match.start()
                            substr = source[start:start+length].strip()

                    if srcmatch is not None:
                        value = ExtractResult()
                        value.start = start
                        value.length = length
                        value.text = substr
                        value.type = self._extract_type
                        if srcmatch in match_source:
                            value.data = srcmatch
                        result.append(value)
        return result

    def _generate_format_regex(self, format_type: long_format_type, placeholder: str=None) -> Pattern:
        if placeholder is None:
            placeholder = BaseNumbers.PlaceHolderDefault
        
        re_definition = None
        if format_type.decimals_mark is None:
            re_definition = BaseNumbers.IntegerRegexDefinition(placeholder, format_type.thousands_mark)
        else:
            re_definition = BaseNumbers.DoubleRegexDefinition(placeholder, format_type.thousands_mark, format_type.decimals_mark)
        return regex.compile(re_definition)