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
        matched: List[bool]=[False] *len(source)
        
        matches_list = map(lambda x : matches_val(matches=re.findall(x.re, source), val=x.val), self.regexes)
        matches_list = filter(lambda x : x.matches, matches_list)

        for ml in matches_list:
            for m in ml.matches:
                for j in range(len(source)):
                    matched[m.index+j]=True
                match_source[m]=ml.value

        # TODO: extractor
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