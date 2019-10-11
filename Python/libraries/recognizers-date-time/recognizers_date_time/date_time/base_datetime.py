from abc import abstractmethod
from typing import List, Optional, Pattern, Dict, Match
from datetime import datetime, timedelta
from collections import namedtuple
import regex

from recognizers_date_time.date_time.date_extractor import DateExtractor
from recognizers_text.extractor import ExtractResult
from recognizers_number.number.extractors import BaseNumberExtractor
from recognizers_number.number.parsers import BaseNumberParser
from .constants import Constants, TimeTypeConstants
from recognizers_number.number.constants import Constants as NumConstants
from .extractors import DateTimeExtractor
from .parsers import DateTimeParser, DateTimeParseResult
from .utilities import Token, merge_all_tokens, DateTimeResolutionResult, DateTimeUtilityConfiguration, AgoLaterUtil,\
    DateTimeFormatUtil, RegExpUtility, AgoLaterMode, DateTimeOptionsConfiguration, DateTimeOptions


class DateTimeExtractorConfiguration(DateTimeOptionsConfiguration):
    @property
    @abstractmethod
    def date_point_extractor(self) -> DateExtractor:
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
    def specific_end_of_regex(self) -> Pattern:
        raise NotImplementedError

    @property
    @abstractmethod
    def unspecific_end_of_regex(self) -> Pattern:
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

    @property
    @abstractmethod
    def number_as_time_regex(self) -> Pattern:
        raise NotImplementedError

    @property
    @abstractmethod
    def date_number_connector_regex(self) -> Pattern:
        raise NotImplementedError

    @property
    @abstractmethod
    def suffix_after_regex(self) -> Pattern:
        raise NotImplementedError

    @property
    @abstractmethod
    def year_suffix(self) -> Pattern:
        raise NotImplementedError

    @property
    @abstractmethod
    def year_regex(self) -> Pattern:
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
        tokens.extend(self.special_time_of_day(source, reference))

        result = merge_all_tokens(tokens, source, self.extractor_type_name)
        return result

    def special_time_of_day(self, text: str, reference: datetime):
        ret = []
        match = self.config.specific_end_of_regex.search(text)
        if match:
            ret.append(Token(text.index(match.group()), len(text)))

        return ret

    # merge a Date entity and a Time entity, like "at 7 tomorrow"
    def merge_date_and_time(self, source: str, reference: datetime) -> List[Token]:
        tokens: List[Token] = list()
        date_ers: List[ExtractResult] = self.config.date_point_extractor.extract(
            source, reference)

        if not date_ers:
            return tokens

        time_ers = self.config.time_point_extractor.extract(source, reference)
        time_num_matches = self.config.number_as_time_regex.match(source)

        if len(time_ers) == 0 and time_num_matches == 0:
            return tokens

        extract_results = date_ers
        extract_results.extend(time_ers)

        # handle cases which use numbers as time points
        # only enabled in CalendarMode
        if (self.config.options & DateTimeOptions.CALENDAR) != 0:
            num_ers = []

            idx = 0

            for idx in range(idx, len(time_num_matches), 1):
                match = time_num_matches[idx]
                node = ExtractResult()
                node.start = source.index(match.group())
                node.length = len(match.group())
                node.text = match.text
                node.type = NumConstants.SYS_NUM_INTEGER
                num_ers.append(node)

            extract_results.extend(num_ers)

        extract_results = sorted(extract_results, key=lambda x: x.start)

        i = 0

        while i < len(extract_results) - 1:

            j = i + 1

            while j < len(extract_results) and extract_results[i].overlap(extract_results[j]):
                j += 1

            if j >= len(extract_results):
                break

            if ((extract_results[i].type is Constants.SYS_DATETIME_DATE and extract_results[j].type is
                 Constants.SYS_DATETIME_TIME) or
                    (extract_results[i].type is Constants.SYS_DATETIME_TIME and extract_results[j].type is
                     Constants.SYS_DATETIME_DATE)or
                    (extract_results[i].type is Constants.SYS_DATETIME_DATE and extract_results[j] is
                     NumConstants.SYS_NUM_INTEGER)):
                middle_begin = extract_results[i].start + (extract_results[i].length or 0)
                middle_end = extract_results[j].start or 0

                if middle_begin > middle_end:
                    i = j + 1
                    continue

                middle_str = source[middle_begin: middle_end].strip()
                valid = False

                # for cases like "tomorrow 3", "tomorrow at 3"
                if extract_results[j].type is NumConstants.SYS_NUM_INTEGER:
                    match = self.config.date_number_connector_regex.search(middle_str)
                    if not middle_str or match:
                        valid = True
                else:
                    # for case like "3 pm or later on monday"
                    match = self.config.suffix_after_regex.search(middle_str)
                    if match:
                        middle_str = middle_str[
                            middle_str.index(match.group()) + len(match.group()): len(middle_end)].strip()

                    if not (match and len(middle_str) == 0):
                        if self.config.is_connector_token(middle_str):
                            valid = True

                if valid:
                    begin = extract_results[i].start or 0
                    end = (extract_results[j].start or 0) + (extract_results[j].length or 0)

                    end_index, start_index = self.extend_with_date_time_and_year(begin, end, source, reference)

                    tokens.append(Token(start_index, end_index))
                    i = j + 1
                    continue
            i = j

        # handle "in the afternoon" at the end of entity
        idx = 0
        for idx in range(idx, len(tokens), 1):
            after_str = source[tokens[idx].end:]
            match = self.config.suffix_regex.search(after_str)
            if match:
                tokens[idx] = Token(tokens[idx].start, tokens[idx].end + len(match.group()))

        # handle "day" prefixes
        idx = 0
        for idx in range(idx, len(tokens), 1):
            before_str = source[0: tokens[idx].start]
            match = self.config.utility_configuration.common_date_prefix_regex.search(before_str)
            if match:
                tokens[idx] = Token(tokens[idx].start - len(match.group()), tokens[idx].end)

        return tokens

    def extend_with_date_time_and_year(self, start_index: int, end_index: int, text: str, reference: datetime):
        # check whether there's a year behind.
        suffix = text[end_index:]
        match_year = self.config.year_suffix.search(suffix)
        if match_year and suffix.index(match_year.group()) == 0:

            check_year = self.config.date_point_extractor.get_year_from_text(self.config.year_regex.search(text))
            year = self.config.date_point_extractor.get_year_from_text(match_year)
            if Constants.MIN_YEAR_NUM <= year <= Constants.MAX_YEAR_NUM and check_year == year:
                end_index += (match_year.end() - match_year.start())

        return end_index, start_index

    def verify_end_token(self, source: str, token: Token) -> Token:
        after_str = source[token.end:]
        match = regex.search(self.config.suffix_regex, after_str)

        if match:
            token.end += len(match.group())

        return token

    # match "now"
    def basic_regex_match(self, source: str) -> List[Token]:
        tokens: List[Token] = list()
        # handle "now"
        matches: List[Match] = list(
            regex.finditer(self.config.now_regex, source))
        tokens.extend(map(lambda x: Token(x.start(), x.end()), matches))
        return tokens

    # parse a specific time of today, tonight, this afternoon, "this afternoon at 7"
    def time_of_today_before(self, source: str, reference: datetime) -> List[Token]:
        tokens: List[Token] = list()
        extract_results = self.config.time_point_extractor.extract(source, reference)

        for extract_result in extract_results:
            before = source[:extract_result.start]
            # handle "this morning at 7am"
            inner_match = regex.search(self.config.night_regex, extract_result.text)

            if inner_match and inner_match.start() == 0:
                before = source[:extract_result.start + len(inner_match.group())]

            if not before:
                continue

            match = regex.search(
                self.config.time_of_today_before_regex, before)
            if match:
                begin = match.start()
                end = extract_result.start + extract_result.length
                tokens.append(Token(begin, end))

        matches: List[Match] = list(regex.finditer(
            self.config.simple_time_of_today_before_regex, source))
        tokens.extend(map(lambda x: Token(x.start(), x.end()), matches))
        return tokens

    # parses a specific time of today, tonight, this afternoon, like "seven this afternoon"
    def time_of_today_after(self, source: str, reference: datetime) -> List[Token]:
        tokens: List[Token] = list()
        extract_results = self.config.time_point_extractor.extract(source, reference)

        for extract_result in extract_results:
            after = source[extract_result.start + extract_result.length:]
            if not after:
                continue

            match = regex.search(self.config.time_of_today_after_regex, after)
            if match:
                begin = extract_result.start
                end = extract_result.start + extract_result.length + len(match.group())
                tokens.append(Token(begin, end))

        matches: List[Match] = list(regex.finditer(
            self.config.simple_time_of_today_after_regex, source))
        tokens.extend(map(lambda x: Token(x.start(), x.end()), matches))
        return tokens

    def special_time_of_date(self, source: str, reference: datetime) -> List[Token]:
        tokens: List[Token] = list()
        extract_results = self.config.date_point_extractor.extract(source, reference)

        # handle "the end of the day"
        for extract_result in extract_results:
            before = source[:extract_result.start]
            before_match = regex.search(
                self.config.specific_end_of_regex, before)

            if before_match:
                tokens.append(
                    Token(before_match.start(), extract_result.start + extract_result.length))
            else:
                after = source[extract_result.start + extract_result.length:]
                after_match = regex.search(
                    self.config.specific_end_of_regex, after)
                if after_match:
                    tokens.append(Token(extract_result.start, extract_result.start +
                                        extract_result.length + after_match.end()))

        # handle "eod, end of day"
        eod = regex.finditer(self.config.unspecific_end_of_regex, source)
        for match in eod:
            tokens.append(Token(match.start(), match.end()))

        return tokens

    # process case like "two minutes ago" "three hours later"
    def duration_with_before_and_after(self, source: str, reference: datetime) -> List[Token]:
        tokens: List[Token] = list()
        extract_results = self.config.duration_extractor.extract(source, reference)

        for extract_result in extract_results:
            match = regex.search(self.config.unit_regex, extract_result.text)
            if match:
                tokens = AgoLaterUtil.extractor_duration_with_before_and_after(
                    source, extract_result, tokens, self.config.utility_configuration)

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
    def specific_end_of_regex(self) -> Pattern:
        raise NotImplementedError

    @property
    @abstractmethod
    def unspecific_end_of_regex(self) -> Pattern:
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
                inner_result = self.parse_special_time_of_date(
                    source_text, reference)

            if not inner_result.success:
                inner_result = self.parser_duration_with_ago_and_later(
                    source_text, reference)

            if inner_result.success:
                inner_result.future_resolution[TimeTypeConstants.DATETIME] = DateTimeFormatUtil.format_date_time(
                    inner_result.future_value)
                inner_result.past_resolution[TimeTypeConstants.DATETIME] = DateTimeFormatUtil.format_date_time(
                    inner_result.past_value)
                result.value = inner_result
                result.timex_str = inner_result.timex if inner_result else ''
                result.resolution_str = ''

        return result

    # merge a Date entity and a Time entity
    def merge_date_and_time(self, source: str, reference: datetime) -> DateTimeResolutionResult:
        result = DateTimeResolutionResult()
        extract_result1: ExtractResult = next(
            iter(self.config.date_extractor.extract(source, reference)), None)

        if extract_result1 is None:
            extract_results = self.config.date_extractor.extract(
                self.config.token_before_date + source, reference)

            if len(extract_results) == 1:
                extract_result1: ExtractResult = next(iter(extract_results), None)
                extract_result1.start -= len(self.config.token_before_date)
            else:
                return result
        else:
            # this is to understand if there is an ambiguous token in the text. For some languages (e.g. spanish)
            # the same word could mean different things (e.g a time in the day or an specific day).
            if self.config.have_ambiguous_token(source, extract_result1.text):
                return result

        extract_result2_list: List[ExtractResult] = self.config.time_extractor.extract(
            source, reference)
        extract_result2: ExtractResult = next(iter(extract_result2_list), None)

        if extract_result2 is None:
            # here we filter out "morning, afternoon, night..." time entities
            extract_result2_list = self.config.time_extractor.extract(
                self.config.token_before_time + source, reference)

            if len(extract_result2_list) == 1:
                extract_result2: ExtractResult = next(iter(extract_result2_list), None)
                extract_result2.start -= len(self.config.token_before_time)
            else:
                return result

        # handle case "Oct. 5 in the afternoon at 7:00"
        # in this case "5 in the afternoon" will be extract as a Time entity
        correct_time_idx = 0

        while correct_time_idx < len(extract_result2_list) and\
                extract_result2_list[correct_time_idx].overlap(extract_result1):
            correct_time_idx += 1

        if correct_time_idx >= len(extract_result2_list):
            return result

        extract_result2 = extract_result2_list[correct_time_idx]

        parse_result1 = self.config.date_parser.parse(extract_result1, reference)
        parse_result2 = self.config.time_parser.parse(extract_result2, reference)

        if not parse_result1.value or not parse_result2.value:
            return result

        future_date: datetime = parse_result1.value.future_value
        past_date: datetime = parse_result1.value.past_value
        time: datetime = parse_result2.value.future_value

        hour = time.hour
        minute = time.minute
        second = time.second

        # handle morning, afternoon
        if regex.search(self.config.pm_time_regex, source) and hour < 12:
            hour += 12
        elif regex.search(self.config.am_time_regex, source) and hour >= 12:
            hour -= 12

        time_str = parse_result2.timex_str
        if time_str.endswith(Constants.AM_PM_GROUP_NAME):
            time_str = time_str[:-4]

        time_str = f'T{hour:02d}{time_str[3:]}'
        result.timex = parse_result1.timex_str + time_str

        val = parse_result2.value

        has_am_pm = regex.search(self.config.pm_time_regex, source) and regex.search(
            self.config.am_time_regex, source)
        if hour <= 12 and not has_am_pm and val.comment:
            result.comment = Constants.AM_PM_GROUP_NAME

        result.future_value = datetime(
            future_date.year, future_date.month, future_date.day, hour, minute, second)
        result.past_value = datetime(
            past_date.year, past_date.month, past_date.day, hour, minute, second)
        result.success = True

        # change the value of time object
        parse_result2.timex_str = time_str
        if result.comment:
            parse_result2.value.comment = Constants.AM_PM_GROUP_NAME if result.comment == Constants.AM_PM_GROUP_NAME else ''

        # add the date and time object in case we want to split them
        result.sub_date_time_entities = [parse_result1, parse_result2]

        return result

    def parse_basic_regex(self, source: str, reference: datetime) -> DateTimeResolutionResult:
        result = DateTimeResolutionResult()
        source = source.strip().lower()

        # handle "now"
        match = regex.search(self.config.now_regex, source)
        if match and match.start() == 0 and match.group() == source:
            matched_now_timex = self.config.get_matched_now_timex(source)
            result.timex = matched_now_timex.timex
            result.future_value = reference
            result.past_value = reference
            result.success = matched_now_timex.matched
        return result

    def parse_time_of_today(self, source: str, reference: datetime) -> DateTimeResolutionResult:
        result = DateTimeResolutionResult()
        source = source.strip().lower()

        minute = 0
        second = 0

        whole_match = next(regex.finditer(
            self.config.simple_time_of_today_after_regex, source), None)
        if whole_match is None or whole_match.group() != source:
            whole_match = next(regex.finditer(
                self.config.simple_time_of_today_before_regex, source), None)

        if whole_match and whole_match.group() == source:
            hour_str = RegExpUtility.get_group(whole_match, Constants.HOUR_GROUP_NAME, None)
            if not hour_str:
                hour_str = RegExpUtility.get_group(
                    whole_match, Constants.HOUR_NUM_GROUP_NAME).lower()
                hour = self.config.numbers.get(hour_str)
            else:
                hour = int(hour_str)
            time_str = f'T{hour:02d}'
        else:
            ers = self.config.time_extractor.extract(source, reference)
            if len(ers) == 1:
                er = next(iter(ers), None)
            else:
                er = next(iter(self.config.time_extractor.extract(
                    self.config.token_before_time + source, reference)), None)
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

        match = next(regex.finditer(
            self.config.specific_time_of_day_regex, source), None)
        if match is None:
            return result

        match_str = match.group().lower()

        # handle "last", "next"
        swift = self.config.get_swift_day(match_str)

        date = reference + timedelta(days=swift)

        # handle "morning", "afternoon"
        hour = self.config.get_hour(match_str, hour)

        # in this situation, luisStr cannot end up with "ampm", because we always have a "morning" or "night"
        if time_str.endswith(Constants.AM_PM_GROUP_NAME):
            time_str = time_str[0:-4]

        time_str = f'T{hour:02d}{time_str[3:]}'

        result.timex = DateTimeFormatUtil.format_date(date) + time_str
        result.future_value = datetime(
            date.year, date.month, date.day, hour, minute, second)
        result.past_value = datetime(
            date.year, date.month, date.day, hour, minute, second)
        result.success = True

        return result

    def parse_special_time_of_date(self, source: str, reference: datetime) -> DateTimeResolutionResult:
        result = DateTimeResolutionResult()
        result = self.parse_unspecific_time_of_date(source, reference)
        if result.success is True:
            return result

        extract_results = self.config.date_extractor.extract(source, reference)

        if len(extract_results) != 1:
            return result

        extract_result = next(iter(extract_results), None)
        before_str = source[0:extract_result.start]

        if regex.search(self.config.specific_end_of_regex, before_str) is None:
            return result

        parse_result = self.config.date_parser.parse(extract_result, reference)
        result.timex = parse_result.timex_str + 'T23:59:59'
        future_date = parse_result.value.future_value
        past_date = parse_result.value.past_value
        result = self.resolve_end_of_day(parse_result.timex_str, future_date, past_date)

        return result

    def parse_unspecific_time_of_date(self, source: str, reference: datetime) -> DateTimeResolutionResult:
        # handle 'eod', 'end of day'
        result = DateTimeResolutionResult()
        eod = regex.search(self.config.unspecific_end_of_regex, source)
        if eod:
            result = self.resolve_end_of_day(
                DateTimeFormatUtil.format_date(reference), reference, reference)

        return result

    @staticmethod
    def resolve_end_of_day(timex_prefix: str, future_date: datetime, past_date: datetime) -> DateTimeResolutionResult:
        result = DateTimeResolutionResult()

        result.timex = timex_prefix + 'T23:59:59'
        result.future_value = datetime(
            future_date.year, future_date.month, future_date.day, 23, 59, 59, 0)
        result.past_value = datetime(
            past_date.year, past_date.month, past_date.day, 23, 59, 59, 0)
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
