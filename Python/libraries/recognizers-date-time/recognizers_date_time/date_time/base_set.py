from abc import ABC, abstractmethod
from typing import List, Optional, Pattern
from datetime import datetime
import regex

from recognizers_text.utilities import RegExpUtility
from recognizers_text.extractor import ExtractResult
from .constants import Constants
from .extractors import DateTimeExtractor
from .parsers import DateTimeParser, DateTimeParseResult
from .utilities import Token, merge_all_tokens

class SetExtractorConfiguration(ABC):
    @property
    @abstractmethod
    def last_regex(self) -> Pattern:
        raise NotImplementedError

    @property
    @abstractmethod
    def each_prefix_regex(self) -> Pattern:
        raise NotImplementedError

    @property
    @abstractmethod
    def periodic_regex(self) -> Pattern:
        raise NotImplementedError

    @property
    @abstractmethod
    def each_unit_regex(self) -> Pattern:
        raise NotImplementedError

    @property
    @abstractmethod
    def each_day_regex(self) -> Pattern:
        raise NotImplementedError

    @property
    @abstractmethod
    def before_each_day_regex(self) -> Pattern:
        raise NotImplementedError

    @property
    @abstractmethod
    def set_week_day_regex(self) -> Pattern:
        raise NotImplementedError

    @property
    @abstractmethod
    def set_each_regex(self) -> Pattern:
        raise NotImplementedError

    @property
    @abstractmethod
    def duration_extractor(self) -> DateTimeExtractor:
        raise NotImplementedError

    @property
    @abstractmethod
    def time_extractor(self) -> DateTimeExtractor:
        raise NotImplementedError

    @property
    @abstractmethod
    def date_extractor(self) -> DateTimeExtractor:
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

class BaseSetExtractor(DateTimeExtractor):
    @property
    def extractor_type_name(self) -> str:
        return Constants.SYS_DATETIME_SET

    def __init__(self, config: SetExtractorConfiguration):
        self.config = config

    def extract(self, source: str, reference: datetime = None) -> List[ExtractResult]:
        if reference is None:
            reference = datetime.now()

        tokens: List[Token] = list()
        tokens.extend(self.match_each_unit(source))
        tokens.extend(self.match_periodic(source))
        tokens.extend(self.match_each_duration(source, reference))
        tokens.extend(self.time_everyday(source, reference))
        tokens.extend(self.match_each(self.config.date_extractor, source, reference))
        tokens.extend(self.match_each(self.config.time_extractor, source, reference))
        tokens.extend(self.match_each(self.config.date_time_extractor, source, reference))
        tokens.extend(self.match_each(self.config.date_period_extractor, source, reference))
        tokens.extend(self.match_each(self.config.time_period_extractor, source, reference))
        # tokens.extend(self.match_each(self.config.date_time_period_extractor, source, reference))
        result = merge_all_tokens(tokens, source, self.extractor_type_name)
        return result

    def match_each_unit(self, source: str) -> List[Token]:
        for match in regex.finditer(self.config.each_unit_regex, source):
            yield Token(match.start(), match.end())

    def match_periodic(self, source: str) -> List[Token]:
        for match in regex.finditer(self.config.periodic_regex, source):
            yield Token(match.start(), match.end())

    def match_each_duration(self, source: str, reference: datetime) -> List[Token]:
        for extract_result in self.config.duration_extractor.extract(source, reference):
            if regex.match(self.config.last_regex, extract_result.text):
                continue
            before_str = source[0:extract_result.start]
            match = regex.match(self.config.each_prefix_regex, before_str)
            if match:
                yield Token(match.start(), extract_result.start + extract_result.length)

    def time_everyday(self, source: str, reference: datetime) -> List[Token]:
        for extract_result in self.config.time_extractor.extract(source, reference):
            after_str = source[extract_result.start + extract_result.length]
            if not after_str and self.config.before_each_day_regex is not None:
                before_str = source[0:extract_result.start]
                before_match = regex.match(self.config.before_each_day_regex, before_str)
                if before_match:
                    yield Token(before_match.start(), extract_result.start + extract_result.length)
            else:
                after_match = regex.match(self.config.each_day_regex, after_str)
                if after_match:
                    yield Token(extract_result.start, extract_result.start + extract_result.length + len(after_match.group()))

    def match_each(self, extractor: DateTimeExtractor, source: str, reference: datetime) -> List[Token]:
        for match in regex.finditer(self.config.set_each_regex, source):
            trimmed_source = source[0:match.start()] + source[match.end()]
            for extract_result in extractor.extract(trimmed_source, reference):
                if extract_result.start <= match.start() and extract_result.start + extract_result.length > match.start():
                    yield Token(extract_result.start, extract_result.start + extract_result.length + len(match.group()))

        for match in regex.finditer(self.config.set_week_day_regex, source):
            trimmed_source = source[0:match.start()] + RegExpUtility.get_group(match, 'weekday') + source[match.end():]
            for extract_result in extractor.extract(trimmed_source, reference):
                if extract_result.start <= match.start():
                    length = extract_result.length + 1
                    prefix = RegExpUtility.get_group(match, 'prefix')
                    if prefix:
                        length += len(prefix)

                    yield Token(extract_result.start, extract_result.start + length)

class SetParserConfiguration:
    pass

class BaseSetParser(DateTimeParser):
    @property
    def parser_type_name(self) -> str:
        return Constants.SYS_DATETIME_SET

    def __init__(self, config: SetParserConfiguration):
        self.config = config

    def parse(self, source: ExtractResult, reference: datetime = None) -> Optional[DateTimeParseResult]:
        #TODO: code
        pass
