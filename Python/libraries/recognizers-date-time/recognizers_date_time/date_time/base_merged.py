from abc import abstractmethod
from typing import List, Optional, Pattern
from datetime import datetime
from collections import namedtuple
import regex

from recognizers_text.extractor import Extractor, ExtractResult
from .constants import Constants
from .extractors import DateTimeExtractor
from .parsers import DateTimeParser, DateTimeParseResult
from .date_time_recognizer import DateTimeOptions
from .utilities import Token, merge_all_tokens, RegExpUtility

MatchedIndex = namedtuple('MatchedIndex', ['matched', 'index'])

class MergedExtractorConfiguration:
    @property
    @abstractmethod
    def date_extractor(self) -> DateTimeExtractor:
        raise NotImplementedError

    @property
    @abstractmethod
    def time_extractor(self) -> DateTimeExtractor:
        raise NotImplementedError

    @property
    @abstractmethod
    def date_time_extractor(self) -> DateTimeExtractor:
        raise NotImplementedError

    @property
    @abstractmethod
    def date_period_extractor(self) -> DateTimeExtractor:
        raise NotImplementedError

    @property
    @abstractmethod
    def time_period_extractor(self) -> DateTimeExtractor:
        raise NotImplementedError

    @property
    @abstractmethod
    def date_time_period_extractor(self) -> DateTimeExtractor:
        raise NotImplementedError

    @property
    @abstractmethod
    def holiday_extractor(self) -> DateTimeExtractor:
        raise NotImplementedError

    @property
    @abstractmethod
    def duration_extractor(self) -> DateTimeExtractor:
        raise NotImplementedError

    @property
    @abstractmethod
    def set_extractor(self) -> DateTimeExtractor:
        raise NotImplementedError

    @property
    @abstractmethod
    def integer_extractor(self) -> Extractor:
        raise NotImplementedError

    @property
    @abstractmethod
    def after_regex(self) -> Pattern:
        raise NotImplementedError

    @property
    @abstractmethod
    def before_regex(self) -> Pattern:
        raise NotImplementedError

    @property
    @abstractmethod
    def since_regex(self) -> Pattern:
        raise NotImplementedError

    @property
    @abstractmethod
    def from_to_regex(self) -> Pattern:
        raise NotImplementedError

    @property
    @abstractmethod
    def single_ambiguous_month_regex(self) -> Pattern:
        raise NotImplementedError

    @property
    @abstractmethod
    def preposition_suffix_regex(self) -> Pattern:
        raise NotImplementedError

    @property
    @abstractmethod
    def number_ending_pattern(self) -> Pattern:
        raise NotImplementedError

    @property
    @abstractmethod
    def filter_word_regex_list(self) -> List[Pattern]:
        raise NotImplementedError

class BaseMergedExtractor(DateTimeExtractor):
    @property
    def extractor_type_name(self) -> str:
        return Constants.SYS_DATETIME_MERGED

    def __init__(self, config: MergedExtractorConfiguration, options: DateTimeOptions):
        self.config = config
        self.options = options

    def extract(self, source: str, reference: datetime = None) -> List[ExtractResult]:
        if reference is None:
            reference = datetime.now()

        result:List[ExtractResult] = list()
        result = self.add_to(result, self.config.date_extractor.extract(source, reference), source)
        result = self.add_to(result, self.config.time_extractor.extract(source, reference), source)
        result = self.add_to(result, self.config.duration_extractor.extract(source, reference), source)
        result = self.add_to(result, self.config.date_period_extractor.extract(source, reference), source)
        result = self.add_to(result, self.config.date_time_extractor.extract(source, reference), source)
        result = self.add_to(result, self.config.time_period_extractor.extract(source, reference), source)
        result = self.add_to(result, self.config.date_time_period_extractor.extract(source, reference), source)
        result = self.add_to(result, self.config.set_extractor.extract(source, reference), source)
        result = self.add_to(result, self.config.holiday_extractor.extract(source, reference), source)

        # this should be at the end since if need the extractor to determine the previous text contains time or not
        result = self.add_to(result, self.number_ending_regex_match(source, result), source)

        result = self.add_mod(result, source)

        # filtering
        if self.options & DateTimeOptions.CALENDAR:
            result = self.check_calendar_filter_list(result, source)

        result = sorted(result, key=lambda x: x.start)
        
        return result

    def add_to(self, destination: List[ExtractResult], source: List[ExtractResult], text: str) -> List[ExtractResult]:
        for value in source:
            if self.options & DateTimeOptions.SKIP_FROM_TO_MERGE and self.should_skip_from_merge(value):
                continue
            is_found = False
            overlap_indexes: List[int] = list()
            first_index = -1
            for index, dest in enumerate(destination):
                if dest.overlap(value):
                    is_found = True
                    if dest.cover(value):
                        if first_index == -1:
                            first_index = index
                        overlap_indexes.append(index)
                    else:
                        continue
            if not is_found:
                destination.append(value)
            else:
                temp_dst: List[ExtractResult] = list()
                for index, dest in enumerate(destination):
                    if index not in overlap_indexes:
                        temp_dst.append(dest)

                # insert at the first overlap occurence to keep the order
                temp_dst.insert(first_index, value)
                destination = temp_dst
        return destination

    def should_skip_from_merge(self, source: ExtractResult) -> bool:
        return regex.search(self.config.from_to_regex, source.text)

    def number_ending_regex_match(self, source: str, extract_results: List[ExtractResult]) -> List[ExtractResult]:
        tokens: List[Token] = list()

        for extract_result in extract_results:
            if extract_result.type in [Constants.SYS_DATETIME_TIME, Constants.SYS_DATETIME_DATETIME]:
                after_str = source[extract_result.start + extract_result.length:]
                match = regex.search(self.config.number_ending_pattern, after_str)
                if match:
                    new_time = RegExpUtility.get_group(match, 'newTime')
                    num_res = self.config.integer_extractor.extract(new_time)
                    if not num_res:
                        continue

                    start_position = extract_result.start + extract_result.length + match.group().index(new_time)
                    tokens.append(Token(start_position, start_position + len(new_time)))

        return merge_all_tokens(tokens, source, Constants.SYS_DATETIME_DATE)

    def add_mod(self, ers: List[ExtractResult], source: str) -> List[ExtractResult]:
        return map(lambda x: self.add_mod_item(x, source), ers)

    def add_mod_item(self, er: ExtractResult, source: str) -> ExtractResult:
        before_str = source[0:er.start]
        
        before = self.has_token_index(before_str.strip(), self.config.before_regex)
        if before.matched:
            mod_len = len(before_str) - before.index
            er.length =+ mod_len
            er.start -= mod_len
            er.text = source[er.start:er.start + er.length]
        
        after = self.has_token_index(before_str.strip(), self.config.after_regex)
        if after.matched:
            mod_len = len(before_str) - after.index
            er.length =+ mod_len
            er.start -= mod_len
            er.text = source[er.start:er.start + er.length]
        
        since = self.has_token_index(before_str.strip(), self.config.since_regex)
        if since.matched:
            mod_len = len(before_str) - since.index
            er.length =+ mod_len
            er.start -= mod_len
            er.text = source[er.start:er.start + er.length]
        
        return er

    def has_token_index(self, source: str, pattern: Pattern) -> MatchedIndex:
        match = regex.search(pattern, source)
        if match:
            return MatchedIndex(True, match.start())
        return MatchedIndex(False, -1)

    def check_calendar_filter_list(self, ers: List[ExtractResult], source: str) -> List[ExtractResult]:
        for er in reversed(ers):
            for pattern in self.config.filter_word_regex_list:
                if regex.search(pattern, er.text):
                    ers.remove(er)
                    break
        return ers


class MergedParserConfiguration:
    pass

class BaseMergedParser(DateTimeParser):
    @property
    def parser_type_name(self) -> str:
        return Constants.SYS_DATETIME_MERGED

    def __init__(self, config: MergedParserConfiguration, options: DateTimeOptions):
        self.config = config
        self.options = options

    def parse(self, source: ExtractResult, reference: datetime = None) -> Optional[DateTimeParseResult]:
        #TODO: code
        pass
