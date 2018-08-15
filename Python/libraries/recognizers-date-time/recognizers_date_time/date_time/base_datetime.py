from abc import ABC, abstractmethod
from typing import List, Optional, Pattern, Dict, Match
from datetime import datetime, timedelta
from collections import namedtuple
import regex

from recognizers_text.extractor import ExtractResult
from recognizers_number.number.extractors import BaseNumberExtractor
from recognizers_number.number.parsers import BaseNumberParser
from .constants import Constants, TimeTypeConstants
from .extractors import DateTimeExtractor
from .parsers import DateTimeParser, DateTimeParseResult
from .utilities import Token, merge_all_tokens, DateTimeResolutionResult, DateTimeUtilityConfiguration, AgoLaterUtil, FormatUtil, RegExpUtility, AgoLaterMode

class DateTimeExtractorConfiguration(ABC):
    @property
    @abstractmethod
    def date_point_extractor(self) -> DateTimeExtractor:
        raise NotImplementedError

    @property
    @abstractmethod
    def time_point_extractor(self) -> DateTimeExtractor:
        raise NotImplementedError

    @property
    @abstractmethod
    def duration_extractor(self) -> DateTimeExtractor:
        raise NotImplementedError

    @property
    @abstractmethod
    def suffix_regex(self) -> Pattern:
        raise NotImplementedError

    @property
    @abstractmethod
    def now_regex(self) -> Pattern:
        raise NotImplementedError

    @property
    @abstractmethod
    def time_of_today_after_regex(self) -> Pattern:
        raise NotImplementedError

    @property
    @abstractmethod
    def simple_time_of_today_after_regex(self) -> Pattern:
        raise NotImplementedError

    @property
    @abstractmethod
    def night_regex(self) -> Pattern:
        raise NotImplementedError

    @property
    @abstractmethod
    def time_of_today_before_regex(self) -> Pattern:
        raise NotImplementedError

    @property
    @abstractmethod
    def simple_time_of_today_before_regex(self) -> Pattern:
        raise NotImplementedError

    @property
    @abstractmethod
    def the_end_of_regex(self) -> Pattern:
        raise NotImplementedError

    @property
    @abstractmethod
    def unit_regex(self) -> Pattern:
        raise NotImplementedError

    @property
    @abstractmethod
    def utility_configuration(self) -> DateTimeUtilityConfiguration:
        raise NotImplementedError

    @abstractmethod
    def is_connector_token(self, source: str) -> bool:
        raise NotImplementedError

class BaseDateTimeExtractor(DateTimeExtractor):
    @property
    def extractor_type_name(self) -> str:
        return Constants.SYS_DATETIME_DATETIME

    def __init__(self, config: DateTimeExtractorConfiguration):
        self.config = config

    def extract(self, source: str, reference: datetime = None) -> List[ExtractResult]:

        if reference is None:
            reference = datetime.now()

        tokens: List[Token] = list()
        tokens.extend(self.merge_date_and_time(source, reference))
        tokens.extend(self.basic_regex_match(source))
        tokens.extend(self.time_of_today_before(source, reference))
        tokens.extend(self.time_of_today_after(source, reference))
        tokens.extend(self.special_time_of_date(source, reference))
        tokens.extend(self.duration_with_before_and_after(source, reference))

        result = merge_all_tokens(tokens, source, self.extractor_type_name)
        return result

    def merge_date_and_time(self, source: str, reference: datetime) -> List[Token]:
        tokens: List[Token] = list()
        ers: List[ExtractResult] = self.config.date_point_extractor.extract(source, reference)

        if not ers:
            return tokens

        ers.extend(self.config.time_point_extractor.extract(source, reference))

        if len(ers) < 2:
            return tokens

        ers = sorted(ers, key=lambda x: x.start)
        i = 0

        while i < len(ers)-1:
            j = i+1

            while j < len(ers) and ers[i].overlap(ers[j]):
                j += 1

            if j >= len(ers):
                break

            if ((ers[i].type is Constants.SYS_DATETIME_DATE and ers[j].type is Constants.SYS_DATETIME_TIME) or
                    (ers[i].type is Constants.SYS_DATETIME_TIME and ers[j].type is Constants.SYS_DATETIME_DATE)):
                middle_begin = ers[i].start + ers[i].length
                middle_end = ers[j].start

                if middle_begin > middle_end:
                    i = j + 1
                    continue

                middle = source[middle_begin:middle_end].strip().lower()

                if self.config.is_connector_token(middle):
                    begin = ers[i].start
                    end = ers[j].start + ers[j].length
                    tokens.append(Token(begin, end))
                i = j + 1
                continue
            i = j

        tokens = list(map(lambda x: self.verify_end_token(source, x), tokens))
        return tokens

    def verify_end_token(self, source: str, token: Token) -> Token:
        after_str = source[token.end:]
        match = regex.search(self.config.suffix_regex, after_str)

        if match is not None:
            token.end += len(match.group())

        return token

    def basic_regex_match(self, source: str) -> List[Token]:
        tokens: List[Token] = list()
        matches: List[Match] = list(regex.finditer(self.config.now_regex, source))
        tokens.extend(map(lambda x: Token(x.start(), x.end()), matches))
        return tokens

    def time_of_today_before(self, source: str, reference: datetime) -> List[Token]:
        tokens: List[Token] = list()
        ers = self.config.time_point_extractor.extract(source, reference)

        for er in ers:
            before = source[:er.start]
            inner_match = regex.search(self.config.night_regex, er.text)

            if inner_match is not None and inner_match.start() == 0:
                before = source[:er.start + len(inner_match.group())]

            if not before:
                continue

            match = regex.search(self.config.time_of_today_before_regex, before)
            if match is not None:
                begin = match.start()
                end = er.start + er.length
                tokens.append(Token(begin, end))

        matches: List[Match] = list(regex.finditer(self.config.simple_time_of_today_before_regex, source))
        tokens.extend(map(lambda x: Token(x.start(), x.end()), matches))
        return tokens

    def time_of_today_after(self, source: str, reference: datetime) -> List[Token]:
        tokens: List[Token] = list()
        ers = self.config.time_point_extractor.extract(source, reference)

        for er in ers:
            after = source[er.start + er.length:]
            if not after:
                continue

            match = regex.search(self.config.time_of_today_after_regex, after)
            if match is not None:
                begin = er.start
                end = er.start + er.length + len(match.group())
                tokens.append(Token(begin, end))

        matches: List[Match] = list(regex.finditer(self.config.simple_time_of_today_after_regex, source))
        tokens.extend(map(lambda x: Token(x.start(), x.end()), matches))
        return tokens

    def special_time_of_date(self, source: str, reference: datetime) -> List[Token]:
        tokens: List[Token] = list()
        ers = self.config.date_point_extractor.extract(source, reference)

        for er in ers:
            before = source[:er.start]
            before_match = regex.search(self.config.the_end_of_regex, before)

            if before_match is not None:
                tokens.append(Token(before_match.start(), er.start + er.length))
            else:
                after = source[er.start + er.length:]
                after_match = regex.search(self.config.the_end_of_regex, after)
                if after_match is not None:
                    tokens.append(Token(er.start, er.start + er.length + after_match.end()))

        return tokens

    def duration_with_before_and_after(self, source: str, reference: datetime) -> List[Token]:
        tokens: List[Token] = list()
        ers = self.config.duration_extractor.extract(source, reference)

        for er in ers:
            match = regex.search(self.config.unit_regex, er.text)
            if match is not None:
                tokens = AgoLaterUtil.extractor_duration_with_before_and_after(source, er, tokens, self.config.utility_configuration)

        return tokens

MatchedTimex = namedtuple('MatchedTimex', ['matched', 'timex'])

class DateTimeParserConfiguration:
    @property
    @abstractmethod
    def token_before_date(self) -> str:
        raise NotImplementedError

    @property
    @abstractmethod
    def token_before_time(self) -> str:
        raise NotImplementedError

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
    def date_parser(self) -> DateTimeParser:
        raise NotImplementedError

    @property
    @abstractmethod
    def time_parser(self) -> DateTimeParser:
        raise NotImplementedError

    @property
    @abstractmethod
    def cardinal_extractor(self) -> BaseNumberExtractor:
        raise NotImplementedError

    @property
    @abstractmethod
    def number_parser(self) -> BaseNumberParser:
        raise NotImplementedError

    @property
    @abstractmethod
    def duration_extractor(self) -> DateTimeExtractor:
        raise NotImplementedError

    @property
    @abstractmethod
    def duration_parser(self) -> DateTimeParser:
        raise NotImplementedError

    @property
    @abstractmethod
    def now_regex(self) -> Pattern:
        raise NotImplementedError

    @property
    @abstractmethod
    def am_time_regex(self) -> Pattern:
        raise NotImplementedError

    @property
    @abstractmethod
    def pm_time_regex(self) -> Pattern:
        raise NotImplementedError

    @property
    @abstractmethod
    def simple_time_of_today_after_regex(self) -> Pattern:
        raise NotImplementedError

    @property
    @abstractmethod
    def simple_time_of_today_before_regex(self) -> Pattern:
        raise NotImplementedError

    @property
    @abstractmethod
    def specific_time_of_day_regex(self) -> Pattern:
        raise NotImplementedError

    @property
    @abstractmethod
    def the_end_of_regex(self) -> Pattern:
        raise NotImplementedError

    @property
    @abstractmethod
    def unit_regex(self) -> Pattern:
        raise NotImplementedError

    @property
    @abstractmethod
    def unit_map(self) -> Dict[str, str]:
        raise NotImplementedError

    @property
    @abstractmethod
    def numbers(self) -> Dict[str, int]:
        raise NotImplementedError

    @property
    @abstractmethod
    def utility_configuration(self) -> DateTimeUtilityConfiguration:
        raise NotImplementedError

    @abstractmethod
    def have_ambiguous_token(self, source: str, matched_text: str) -> bool:
        raise NotImplementedError

    @abstractmethod
    def get_matched_now_timex(self, source: str) -> MatchedTimex:
        raise NotImplementedError

    @abstractmethod
    def get_swift_day(self, source: str) -> int:
        raise NotImplementedError

    @abstractmethod
    def get_hour(self, source: str, hour: int) -> int:
        raise NotImplementedError

class BaseDateTimeParser(DateTimeParser):
    @property
    def parser_type_name(self) -> str:
        return Constants.SYS_DATETIME_DATETIME

    def __init__(self, config: DateTimeParserConfiguration):
        self.config = config

    def parse(self, source: ExtractResult, reference: datetime = None) -> Optional[DateTimeParseResult]:
        if reference is None:
            reference = datetime.now()

        result = DateTimeParseResult(source)

        if source.type is self.parser_type_name:
            source_text = source.text.lower()
            inner_result = self.merge_date_and_time(source_text, reference)

            if not inner_result.success:
                inner_result = self.parse_basic_regex(source_text, reference)

            if not inner_result.success:
                inner_result = self.parse_time_of_today(source_text, reference)

            if not inner_result.success:
                inner_result = self.parse_special_time_of_date(source_text, reference)

            if not inner_result.success:
                inner_result = self.parser_duration_with_ago_and_later(source_text, reference)

            if inner_result.success:
                inner_result.future_resolution[TimeTypeConstants.DATETIME] = FormatUtil.format_date_time(inner_result.future_value)
                inner_result.past_resolution[TimeTypeConstants.DATETIME] = FormatUtil.format_date_time(inner_result.past_value)
                result.value = inner_result
                result.timex_str = inner_result.timex if inner_result is not None else ''
                result.resolution_str = ''

        return result

    # merge a Date entity and a Time entity
    def merge_date_and_time(self, source: str, reference: datetime) -> DateTimeResolutionResult:
        result = DateTimeResolutionResult()
        er1: ExtractResult = next(iter(self.config.date_extractor.extract(source, reference)), None)

        if er1 is None:
            ers = self.config.date_extractor.extract(self.config.token_before_date + source, reference)

            if len(ers) == 1:
                er1: ExtractResult = next(iter(ers), None)
                er1.start -= len(self.config.token_before_date)
            else:
                return result
        else:
            # this is to understand if there is an ambiguous token in the text. For some languages (e.g. spanish)
            # the same word could mean different things (e.g a time in the day or an specific day).
            if self.config.have_ambiguous_token(source, er1.text):
                return result

        er2_list: List[ExtractResult] = self.config.time_extractor.extract(source, reference)
        er2: ExtractResult = next(iter(er2_list), None)

        if er2 is None:
            # here we filter out "morning, afternoon, night..." time entities
            er2_list = self.config.time_extractor.extract(self.config.token_before_time + source, reference)

            if len(er2_list) == 1:
                er2: ExtractResult = next(iter(er2_list), None)
                er2.start -= len(self.config.token_before_time)
            else:
                return result

        # handle case "Oct. 5 in the afternoon at 7:00"
        # in this case "5 in the afternoon" will be extract as a Time entity
        correct_time_idx = 0

        while correct_time_idx < len(er2_list) and er2_list[correct_time_idx].overlap(er1):
            correct_time_idx += 1

        if correct_time_idx >= len(er2_list):
            return result

        er2 = er2_list[correct_time_idx]

        pr1 = self.config.date_parser.parse(er1, reference)
        pr2 = self.config.time_parser.parse(er2, reference)

        if not pr1.value or not pr2.value:
            return result

        future_date: datetime = pr1.value.future_value
        past_date: datetime = pr1.value.past_value
        time: datetime = pr2.value.future_value

        hour = time.hour
        minute = time.minute
        second = time.second

        # handle morning, afternoon
        if regex.search(self.config.pm_time_regex, source) is not None and hour < 12:
            hour += 12
        elif regex.search(self.config.am_time_regex, source) is not None and hour >= 12:
            hour -= 12

        time_str = pr2.timex_str
        if time_str.endswith('ampm'):
            time_str = time_str[:-4]

        time_str = f'T{hour:02d}{time_str[3:]}'
        result.timex = pr1.timex_str + time_str

        val = pr2.value

        has_am_pm = regex.search(self.config.pm_time_regex, source) and regex.search(self.config.am_time_regex, source)
        if hour <= 12 and not has_am_pm and val.comment:
            result.comment = 'ampm'

        result.future_value = datetime(future_date.year, future_date.month, future_date.day, hour, minute, second)
        result.past_value = datetime(past_date.year, past_date.month, past_date.day, hour, minute, second)
        result.success = True

        # change the value of time object
        pr2.timex_str = time_str
        if result.comment is not None:
            pr2.value.comment = 'ampm' if result.comment == 'ampm' else ''

        # add the date and time object in case we want to split them
        result.sub_date_time_entities = [pr1, pr2]

        return result

    def parse_basic_regex(self, source: str, reference: datetime) -> DateTimeResolutionResult:
        result = DateTimeResolutionResult()
        source = source.strip().lower()

        # handle "now"
        match = regex.search(self.config.now_regex, source)
        if match is not None and match.start() == 0 and match.group() == source:
            matched_now_timex = self.config.get_matched_now_timex(source)
            result.timex = matched_now_timex.timex
            result.future_value = reference
            result.past_value = reference
            result.success = matched_now_timex.matched
        return result

    def parse_time_of_today(self, source: str, reference: datetime) -> DateTimeResolutionResult:
        result = DateTimeResolutionResult()
        source = source.strip().lower()

        hour = 0
        minute = 0
        second = 0
        time_str: str = None

        whole_match = next(regex.finditer(self.config.simple_time_of_today_after_regex, source), None)
        if whole_match is None or whole_match.group() != source:
            whole_match = next(regex.finditer(self.config.simple_time_of_today_before_regex, source), None)

        if whole_match is not None and whole_match.group() == source:
            hour_str = RegExpUtility.get_group(whole_match, 'hour', None)
            if not hour_str:
                hour_str = RegExpUtility.get_group(whole_match, 'hournum').lower()
                hour = self.config.numbers.get(hour_str)
            else:
                hour = int(hour_str)
            time_str = f'T{hour:02d}'
        else:
            ers = self.config.time_extractor.extract(source, reference)
            if len(ers) == 1:
                er = next(iter(ers), None)
            else:
                er = next(iter(self.config.time_extractor.extract(self.config.token_before_time + source, reference)), None)
                if er is None:
                    return result
                er.start -= len(self.config.token_before_time)

            pr = self.config.time_parser.parse(er, reference)
            if pr.value is None:
                return result

            time: datetime = pr.value.future_value

            hour = time.hour
            minute = time.minute
            second = time.second
            time_str = pr.timex_str

        match = next(regex.finditer(self.config.specific_time_of_day_regex, source), None)
        if match is None:
            return result

        match_str = match.group().lower()

        # handle "last", "next"
        swift = self.config.get_swift_day(match_str)

        date = reference + timedelta(days=swift)

        # handle "morning", "afternoon"
        hour = self.config.get_hour(match_str, hour)

        # in this situation, luisStr cannot end up with "ampm", because we always have a "morning" or "night"
        if time_str.endswith('ampm'):
            time_str = time_str[0:-4]

        time_str = f'T{hour:02d}{time_str[3:]}'

        result.timex = FormatUtil.format_date(date) + time_str
        result.future_value = datetime(date.year, date.month, date.day, hour, minute, second)
        result.past_value = datetime(date.year, date.month, date.day, hour, minute, second)
        result.success = True

        return result

    def parse_special_time_of_date(self, source: str, reference: datetime) -> DateTimeResolutionResult:
        result = DateTimeResolutionResult()
        ers = self.config.date_extractor.extract(source, reference)

        if len(ers) != 1:
            return result

        er = next(iter(ers), None)
        before_str = source[0:er.start]

        if regex.search(self.config.the_end_of_regex, before_str) is None:
            return result

        pr = self.config.date_parser.parse(er, reference)
        result.timex = pr.timex_str + 'T23:59'
        future_date = pr.value.future_value + timedelta(days=1) + timedelta(minutes=-1)
        result.future_value = future_date
        past_date = pr.value.past_value + timedelta(days=1) + timedelta(minutes=-1)
        result.past_value = past_date
        result.success = True

        return result

    def parser_duration_with_ago_and_later(self, source: str, reference: datetime) -> DateTimeResolutionResult:
        return AgoLaterUtil.parse_duration_with_ago_and_later(
            source,
            reference,
            self.config.duration_extractor,
            self.config.duration_parser,
            self.config.unit_map,
            self.config.unit_regex,
            self.config.utility_configuration,
            AgoLaterMode.DATETIME
        )
