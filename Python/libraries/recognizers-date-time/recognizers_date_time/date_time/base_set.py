from abc import ABC, abstractmethod
from typing import List, Optional, Pattern, Dict
from datetime import datetime
import regex

from recognizers_text.utilities import RegExpUtility
from recognizers_text.extractor import ExtractResult
from .constants import Constants, TimeTypeConstants
from .extractors import DateTimeExtractor
from .parsers import DateTimeParser, DateTimeParseResult
from .utilities import Token, merge_all_tokens, DateTimeResolutionResult
from .base_duration import BaseDurationParser
from .base_timeperiod import BaseTimePeriodParser
from .base_time import BaseTimeParser
from .base_date import BaseDateParser
from .base_datetime import BaseDateTimeParser, MatchedTimex
from .base_dateperiod import BaseDatePeriodParser
from .base_datetimeperiod import BaseDateTimePeriodParser

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
        tokens.extend(self.match_each(self.config.date_time_period_extractor, source, reference))
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
            if regex.search(self.config.last_regex, extract_result.text):
                continue

            before_str = source[0:extract_result.start]
            match = regex.search(self.config.each_prefix_regex, before_str)
            if match:
                yield Token(match.start(), extract_result.start + extract_result.length)

    def time_everyday(self, source: str, reference: datetime) -> List[Token]:
        for extract_result in self.config.time_extractor.extract(source, reference):
            after_str = source[extract_result.start + extract_result.length:]
            if not after_str and self.config.before_each_day_regex is not None:
                before_str = source[0:extract_result.start]
                before_match = regex.match(self.config.before_each_day_regex, before_str)
                if before_match:
                    yield Token(before_match.start(), extract_result.start + extract_result.length)
            else:
                after_match = regex.match(self.config.each_day_regex, after_str)
                if after_match:
                    yield Token(
                        extract_result.start,
                        extract_result.start + extract_result.length + len(after_match.group()))

    def match_each(self, extractor: DateTimeExtractor, source: str, reference: datetime) -> List[Token]:
        for match in regex.finditer(self.config.set_each_regex, source):
            trimmed_source = source[0:match.start()] + source[match.end():]

            for extract_result in extractor.extract(trimmed_source, reference):
                if (extract_result.start <= match.start()
                        and extract_result.start + extract_result.length > match.start()):
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
    @property
    @abstractmethod
    def duration_extractor(self) -> DateTimeExtractor:
        raise NotImplementedError

    @property
    @abstractmethod
    def duration_parser(self) -> BaseDurationParser:
        raise NotImplementedError

    @property
    @abstractmethod
    def time_extractor(self) -> DateTimeExtractor:
        raise NotImplementedError

    @property
    @abstractmethod
    def time_parser(self) -> BaseTimeParser:
        raise NotImplementedError

    @property
    @abstractmethod
    def date_extractor(self) -> DateTimeExtractor:
        raise NotImplementedError

    @property
    @abstractmethod
    def date_parser(self) -> BaseDateParser:
        raise NotImplementedError

    @property
    @abstractmethod
    def date_time_extractor(self) -> DateTimeExtractor:
        raise NotImplementedError

    @property
    @abstractmethod
    def date_time_parser(self) -> BaseDateTimeParser:
        raise NotImplementedError

    @property
    @abstractmethod
    def date_period_extractor(self) -> DateTimeExtractor:
        raise NotImplementedError

    @property
    @abstractmethod
    def date_period_parser(self) -> BaseDatePeriodParser:
        raise NotImplementedError

    @property
    @abstractmethod
    def time_period_extractor(self) -> DateTimeExtractor:
        raise NotImplementedError

    @property
    @abstractmethod
    def time_period_parser(self) -> BaseTimePeriodParser:
        raise NotImplementedError

    @property
    @abstractmethod
    def date_time_period_extractor(self) -> DateTimeExtractor:
        raise NotImplementedError

    @property
    @abstractmethod
    def date_time_period_parser(self) -> BaseDateTimePeriodParser:
        raise NotImplementedError

    @property
    @abstractmethod
    def unit_map(self) -> Dict[str, str]:
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
    def set_week_day_regex(self) -> Pattern:
        raise NotImplementedError

    @property
    @abstractmethod
    def set_each_regex(self) -> Pattern:
        raise NotImplementedError

    @abstractmethod
    def get_matched_daily_timex(self, text: str) -> MatchedTimex:
        raise NotImplementedError

    @abstractmethod
    def get_matched_unit_timex(self, text: str) -> MatchedTimex:
        raise NotImplementedError

class BaseSetParser(DateTimeParser):
    @property
    def parser_type_name(self) -> str:
        return Constants.SYS_DATETIME_SET

    def __init__(self, config: SetParserConfiguration):
        self.config = config

    def parse(self, source: ExtractResult, reference: datetime = None) -> Optional[DateTimeParseResult]:
        if reference is None:
            reference = datetime.now()

        result = DateTimeParseResult(source)

        if source.type is self.parser_type_name:
            inner_result = self.parse_each_unit(source.text)

            if not inner_result.success:
                inner_result = self.parse_each_duration(source.text, reference)

            if not inner_result.success:
                inner_result = self.parser_time_everyday(source.text, reference)

            # NOTE: Please do not change the order of following function
            # datetimeperiod>dateperiod>timeperiod>datetime>date>time
            if not inner_result.success:
                inner_result = self.parse_each(self.config.date_time_period_extractor,
                                               self.config.date_time_period_parser, source.text, reference)
            if not inner_result.success:
                inner_result = self.parse_each(self.config.date_period_extractor,
                                               self.config.date_period_parser, source.text, reference)
            if not inner_result.success:
                inner_result = self.parse_each(self.config.time_period_extractor,
                                               self.config.time_period_parser, source.text, reference)
            if not inner_result.success:
                inner_result = self.parse_each(self.config.date_time_extractor,
                                               self.config.date_time_parser, source.text, reference)
            if not inner_result.success:
                inner_result = self.parse_each(self.config.date_extractor,
                                               self.config.date_parser, source.text, reference)
            if not inner_result.success:
                inner_result = self.parse_each(self.config.time_extractor,
                                               self.config.time_parser, source.text, reference)

            if inner_result.success:
                inner_result.future_resolution[TimeTypeConstants.SET] = inner_result.future_value
                inner_result.past_resolution[TimeTypeConstants.SET] = inner_result.past_value
                result.value = inner_result
                result.timex_str = inner_result.timex if inner_result is not None else ''
                result.resolution_str = ''

        return result

    def parse_each_unit(self, source: str) -> DateTimeResolutionResult:
        result = DateTimeResolutionResult()
        # handle "daily", "weekly"
        match = regex.match(self.config.periodic_regex, source)
        if match:
            get_matched_daily_timex = self.config.get_matched_daily_timex(source)
            if not get_matched_daily_timex.matched:
                return result

            result.timex = get_matched_daily_timex.timex
            result.future_value = result.past_value = 'Set: ' + result.timex
            result.success = True

        # handle "each month"
        match = regex.match(self.config.each_unit_regex, source)
        if match and len(match.group()) == len(source):
            source_unit = RegExpUtility.get_group(match, 'unit')
            if source_unit and source_unit in self.config.unit_map:
                get_matched_unit_timex = self.config.get_matched_unit_timex(source_unit)
                if not get_matched_unit_timex.matched:
                    return result

                if RegExpUtility.get_group(match, 'other'):
                    get_matched_unit_timex = MatchedTimex(matched=get_matched_unit_timex.matched, timex=get_matched_unit_timex.timex.replace('1', '2'))

                result.timex = get_matched_unit_timex.timex
                result.future_value = result.past_value = 'Set: ' + result.timex
                result.success = True

        return result

    def parse_each_duration(self, source: str, reference: datetime) -> DateTimeResolutionResult:
        result = DateTimeResolutionResult()
        ers = self.config.duration_extractor.extract(source, reference)
        if len(ers) != 1 or source[ers[0].start+ers[0].length:]:
            return result

        before_str = source[0:ers[0].start]
        matches = regex.match(self.config.each_prefix_regex, before_str)
        if matches:
            pr = self.config.duration_parser.parse(ers[0], datetime.now())
            result.timex = pr.timex_str
            result.future_value = result.past_value = 'Set: ' + pr.timex_str
            result.success = True

        return result

    def parser_time_everyday(self, source: str, reference: datetime) -> DateTimeResolutionResult:
        result = DateTimeResolutionResult()
        ers = self.config.time_extractor.extract(source, reference)
        if len(ers) != 1:
            return result

        after_str = source.replace(ers[0].text, '')
        matches = regex.match(self.config.each_day_regex, after_str)
        if matches:
            pr = self.config.time_parser.parse(ers[0], datetime.now())
            result.timex = pr.timex_str
            result.future_value = result.past_value = 'Set: ' + result.timex
            result.success = True

        return result

    def parse_each(self, extractor: DateTimeExtractor, parser: DateTimeParser,
                   source: str, reference: datetime) -> DateTimeResolutionResult:
        result = DateTimeResolutionResult()
        success = False
        er: List[ExtractResult] = list()

        match = regex.search(self.config.set_each_regex, source)
        if match:
            trimmed_text = source[0:match.start()] + source[match.end():]
            er = extractor.extract(trimmed_text, reference)
            if(len(er) == 1 and er[0].length == len(trimmed_text)):
                success = True

        match = regex.search(self.config.set_week_day_regex, source)
        if match:
            trimmed_text = source[0:match.start()] + RegExpUtility.get_group(match, 'weekday') + source[match.end():]
            er = extractor.extract(trimmed_text, reference)
            if len(er) == 1 and er[0].length == len(trimmed_text):
                success = True

        if success:
            pr = parser.parse(er[0])
            result.timex = pr.timex_str
            result.future_value = 'Set: ' + pr.timex_str
            result.past_value = 'Set: ' + pr.timex_str
            result.success = True

        return result
