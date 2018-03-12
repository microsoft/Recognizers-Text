from abc import ABC, abstractmethod, abstractproperty
from typing import List, Pattern, Dict, Match, NamedTuple
from collections import namedtuple
import regex

from recognizers_number.number.models import long_format_type
from recognizers_text.extractor import Extractor, ExtractResult
from recognizers_number.resources.base_numbers import BaseNumbers
from recognizers_number.number.constants import Constants

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

        matches_list = list(map(lambda x : matches_val(matches=list(regex.finditer(x.re, source, flags=regex.I)), val=x.val), self.regexes))
        matches_list = list(filter(lambda x : len(x.matches) > 0, matches_list))

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
                        value.data = match_source.get(srcmatch, None)
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
        return re_definition

source_position_results = namedtuple('source_position_results', ['source', 'position', 'results'])

class BasePercentageExtractor(Extractor):
    @property
    def regexes(self) -> List[Pattern]:
        return self._regexes
        
    @property
    def _extract_type(self) -> str:
        return Constants.SYS_NUM_PERCENTAGE

    def __init__(self, number_extractor: BaseNumberExtractor):
        self.number_extractor = number_extractor
        self._regexes = self.generate_regexes()

    @abstractmethod
    def get_definitions(self) -> List[str]:
        raise NotImplementedError

    def generate_regexes(self, ignore_case: bool = True) -> List[Pattern]:
        definitions = self.get_definitions()
        options = regex.DOTALL | (regex.IGNORECASE if ignore_case else 0)
        return list(map(lambda d : regex.compile(d, options), definitions))

    def extract(self, source: str) -> List[ExtractResult]:
        origin = source

        # preprocess the source sentence via extracting and replacing the numbers in it
        preprocess = self.__preprocess_with_number_extracted(origin)
        source = preprocess.source
        positionmap = preprocess.position
        extractresults = preprocess.results

        allmatches = list(map(lambda p : list(regex.finditer(p, source)), self.regexes))
        matched: List[bool]=[False] * len(source)

        for matches in allmatches:
            for match in matches:
                for j in range(len(match.group())):
                    matched[match.start()+j]=True

        results = list()

        # get index of each matched results
        last = -1
        for i in range(len(source)):
            if not matched[i]:
                last = i
            else:
                if (i+1 == len(source) or not matched[i+1]):
                    start = last+1
                    length = i-last
                    substr = source[start:start+length].strip()
                    value = ExtractResult()
                    value.start = start
                    value.length = length
                    value.text = substr
                    value.type = self._extract_type
                    results.append(value)

        # post-processing, restoring the extracted numbers
        results = self.__post_processing(results, origin, positionmap, extractresults)

        return results

    def __preprocess_with_number_extracted(self, source: str) -> source_position_results:
        position_map = dict()
        extract_results = self.number_extractor.extract(source)

        dummy_token = BaseNumbers.NumberReplaceToken
        match: List[int]=[-1] * len(source)
        string_parts = list()

        for i in range(len(extract_results)):
            extract_result = extract_results[i]
            subtext = extract_result.text
            start = extract_result.start
            end = extract_result.end+1
            for j in range(start, end):
                if match[j] == -1:
                    match[j] = i

        start = 0
        for i in range(1, len(source)):
            if match[i] != match[i-1]:
                string_parts.append([start, i-1])
                start = i
        
        string_parts.append([start, len(source)-1])

        string_result = ''
        index = 0
        for part in string_parts:
            start = part[0]
            end = part[1]
            val_type = match[start]
            if val_type == -1:
                string_result += source[start:end+1]
                for i in range(start, end):
                    index += 1
                    position_map[index] = i
            else:
                original = source[start:end+1]
                string_result += dummy_token
                for i in range(0, len(dummy_token)):
                    index += 1
                    position_map[index] = start

        index += 1
        position_map[index] = len(source)

        return source_position_results(source=string_result, position=position_map, results=extract_results)

    def __post_processing(self, results: List[ExtractResult], source: str, positionmap: Dict[int, int], extractresults: List[ExtractResult]) -> List[ExtractResult]:
        dummy_token = BaseNumbers.NumberReplaceToken
        for i in range(len(results)):
            start = results[i].start
            end = results[i].start + results[i].length - 1
            text = results[i].text
            if start in positionmap and end in positionmap:
                original_start = positionmap[start]
                original_length = positionmap[end] - original_start
                results[i].start = original_start
                results[i].length = original_length
                results[i].text = source[original_start:original_start + original_length].strip()
                num_start = text.find(dummy_token)
                if num_start != -1:
                    num_original_start = start + num_start
                    if num_start in positionmap:
                        num_original_end = num_original_start + len(dummy_token)
                        data_key = source[positionmap[num_original_start]:positionmap[num_original_end]]
                        for j in range(i, len(extractresults)):
                            if results[i].start == extractresults[j].start and extractresults[j].text in results[i].text:
                                results[i].data = [data_key, extractresults[j]]
                                break
        return results