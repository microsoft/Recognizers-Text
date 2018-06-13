from abc import ABC, abstractmethod
from typing import List, Optional, Pattern, Dict
from datetime import datetime, timedelta
from collections import namedtuple
import regex

from recognizers_text.extractor import ExtractResult
from recognizers_number import BaseNumberExtractor
from .base_date import BaseDateExtractor
from .base_time import BaseTimeExtractor
from .base_datetime import BaseDateTimeExtractor
from .base_duration import BaseDurationExtractor
from .base_timeperiod import BaseTimePeriodExtractor
from .constants import Constants, TimeTypeConstants
from .extractors import DateTimeExtractor
from .parsers import DateTimeParser, DateTimeParseResult
from .utilities import Token, merge_all_tokens, RegExpUtility, FormatUtil, DateTimeResolutionResult, DateUtils

MatchedIndex = namedtuple('MatchedIndex', ['matched', 'index'])
MatchedTimeRange = namedtuple('MatchedTimeRange', ['time_str', 'begin_hour', 'end_hour', 'end_min', 'success'])
BeginEnd = namedtuple('BeginEnd', ['begin', 'end'])

class DateTimePeriodExtractorConfiguration(ABC):
    @property
    @abstractmethod
    def cardinal_extractor(self) -> BaseNumberExtractor:
        raise NotImplementedError

    @property
    @abstractmethod
    def single_date_extractor(self) -> BaseDateExtractor:
        raise NotImplementedError

    @property
    @abstractmethod
    def single_time_extractor(self) -> BaseTimeExtractor:
        raise NotImplementedError

    @property
    @abstractmethod
    def single_date_time_extractor(self) -> BaseDateTimeExtractor:
        raise NotImplementedError

    @property
    @abstractmethod
    def duration_extractor(self) -> BaseDurationExtractor:
        raise NotImplementedError

    @property
    @abstractmethod
    def time_period_extractor(self) -> BaseTimePeriodExtractor:
        raise NotImplementedError

    @property
    @abstractmethod
    def simple_cases_regexes(self) -> List[Pattern]:
        raise NotImplementedError

    @property
    @abstractmethod
    def preposition_regex(self) -> Pattern:
        raise NotImplementedError

    @property
    @abstractmethod
    def till_regex(self) -> Pattern:
        raise NotImplementedError

    @property
    @abstractmethod
    def specific_time_of_day_regex(self) -> Pattern:
        raise NotImplementedError

    @property
    @abstractmethod
    def time_of_day_regex(self) -> Pattern:
        raise NotImplementedError

    @property
    @abstractmethod
    def period_time_of_day_with_date_regex(self) -> Pattern:
        raise NotImplementedError

    @property
    @abstractmethod
    def followed_unit(self) -> Pattern:
        raise NotImplementedError

    @property
    @abstractmethod
    def number_combined_with_unit(self) -> Pattern:
        raise NotImplementedError

    @property
    @abstractmethod
    def time_unit_regex(self) -> Pattern:
        raise NotImplementedError

    @property
    @abstractmethod
    def past_prefix_regex(self) -> Pattern:
        raise NotImplementedError

    @property
    @abstractmethod
    def next_prefix_regex(self) -> Pattern:
        raise NotImplementedError

    @property
    @abstractmethod
    def relative_time_unit_regex(self) -> Pattern:
        raise NotImplementedError

    @property
    @abstractmethod
    def rest_of_date_time_regex(self) -> Pattern:
        raise NotImplementedError

    @property
    @abstractmethod
    def general_ending_regex(self) -> Pattern:
        raise NotImplementedError

    @property
    @abstractmethod
    def middle_pause_regex(self) -> Pattern:
        raise NotImplementedError

    @abstractmethod
    def get_from_token_index(self, source: str) -> MatchedIndex:
        raise NotImplementedError

    @abstractmethod
    def get_between_token_index(self, source: str) -> MatchedIndex:
        raise NotImplementedError

    @abstractmethod
    def has_connector_token(self, source: str) -> bool:
        raise NotImplementedError

class BaseDateTimePeriodExtractor(DateTimeExtractor):
    @property
    def extractor_type_name(self) -> str:
        return Constants.SYS_DATETIME_DATETIMEPERIOD

    def __init__(self, config: DateTimePeriodExtractorConfiguration):
        self.config = config

    def extract(self, source: str, reference: datetime = None) -> List[ExtractResult]:
        if reference is None:
            reference = datetime.now()

        tokens: List[Token] = list()
        tokens.extend(self.match_simple_cases(source, reference))
        tokens.extend(self.merge_two_time_points(source, reference))
        tokens.extend(self.match_duration(source, reference))
        tokens.extend(self.match_night(source, reference))
        tokens.extend(self.match_relative_unit(source))

        result = merge_all_tokens(tokens, source, self.extractor_type_name)
        return result

    def match_simple_cases(self, source: str, reference: datetime) -> List[Token]:
        tokens: List[Token] = list()
        source = source.strip().lower()
        simple_cases_matches = list(map(lambda x: list(regex.finditer(x, source)), self.config.simple_cases_regexes))

        for matches in simple_cases_matches:
            for match in matches:
                # has a date before?
                has_before_date = False

                before_str = source[0:match.start()].strip()
                if before_str:
                    er = next(iter(self.config.single_date_extractor.extract(before_str, reference)), None)
                    if er:
                        begin = er.start
                        end = er.start + er.length
                        middle_str = before_str[end:].strip()
                        if middle_str == '' or regex.search(self.config.preposition_regex, middle_str):
                            tokens.append(Token(begin, match.end()))
                            has_before_date = True

                followed_str = source[match.end():]
                if followed_str and not has_before_date:
                    er = next(iter(self.config.single_date_extractor.extract(followed_str, reference)), None)
                    if er:
                        begin = er.start
                        end = er.start + er.length
                        middle_str = followed_str[0:er.start].strip()
                        if middle_str == '' or regex.search(self.config.preposition_regex, middle_str):
                            tokens.append(Token(match.start(), match.end() + end))

        return tokens

    def merge_two_time_points(self, source: str, reference: datetime) -> List[Token]:
        tokens: List[Token] = list()
        source = source.strip().lower()
        ers_datetime: List[ExtractResult] = self.config.single_date_time_extractor.extract(source, reference)
        ers_time: List[ExtractResult] = self.config.single_time_extractor.extract(source, reference)
        inner_marks: List[ExtractResult] = list()

        j = 0

        for er_datetime in ers_datetime:
            inner_marks.append(er_datetime)

            while j < len(ers_time) and ers_time[j].start + ers_time[j].length < er_datetime.start:
                inner_marks.append(ers_time[j])
                j += 1

            while j < len(ers_time) and ers_time[j].overlap(er_datetime):
                j += 1

        while j < len(ers_time):
            inner_marks.append(ers_time[j])
            j += 1
        inner_marks = sorted(inner_marks, key=lambda x: x.start)

        idx = 0
        ceil = len(inner_marks) - 1

        while idx < ceil:
            current_mark = inner_marks[idx]
            next_mark = inner_marks[idx + 1]

            if current_mark.type == Constants.SYS_DATETIME_TIME and next_mark.type == Constants.SYS_DATETIME_TIME:
                idx += 1
                continue

            middle_begin = current_mark.start + current_mark.length
            middle_end = next_mark.start

            middle_str = source[middle_begin:middle_end].strip()
            match = regex.search(self.config.till_regex, middle_str)

            if match and match.group() == middle_str:
                period_begin = current_mark.start
                period_end = next_mark.start + next_mark.length
                before_str = source[0:period_begin].strip()
                match_from = self.config.get_from_token_index(before_str)
                from_token_index = match_from if match_from.matched else self.config.get_between_token_index(before_str)
                if from_token_index.matched:
                    period_begin = from_token_index.index
                tokens.append(Token(period_begin, period_end))
                idx += 2
                continue

            if self.config.has_connector_token(middle_str):
                period_begin = current_mark.start
                period_end = next_mark.start + next_mark.length
                before_str = source[0:period_begin].strip()
                between_token_index = self.config.get_between_token_index(before_str)
                if between_token_index.matched:
                    period_begin = between_token_index.index
                    tokens.append(Token(period_begin, period_end))
                    idx += 2
                    continue

            idx += 1

        return tokens

    def match_duration(self, source: str, reference: datetime) -> List[Token]:
        tokens: List[Token] = list()
        source = source.strip().lower()

        ers_duration: List[ExtractResult] = self.config.duration_extractor.extract(source, reference)
        durations: List[Token] = list()

        for er in ers_duration:
            if regex.search(self.config.time_unit_regex, er.text):
                durations.append(Token(er.start, er.start + er.length))

        for duration in durations:
            before_str = source[0:duration.start].strip()
            if before_str:
                match = regex.search(self.config.past_prefix_regex, before_str)
                if match and not before_str[match.end():]:
                    tokens.append(Token(match.start(), duration.end))
                    continue

                match = regex.search(self.config.next_prefix_regex, before_str)
                if match and not before_str[match.end():]:
                    tokens.append(Token(match.start(), duration.end))

        return tokens

    def match_night(self, source: str, reference: datetime) -> List[Token]:
        tokens: List[Token] = list()
        source = source.strip().lower()

        matches = regex.finditer(self.config.specific_time_of_day_regex, source)
        tokens.extend(map(lambda x: Token(x.start(), x.end()), matches))

        ers_date: List[ExtractResult] = self.config.single_date_extractor.extract(source, reference)

        for er in ers_date:
            after_str = source[er.start + er.length:]
            match = regex.search(self.config.period_time_of_day_with_date_regex, after_str)

            if match:
                if not after_str[0:match.start()].strip():
                    tokens.append(Token(er.start, er.start + er.length + match.end()))
                else:
                    pause_match = regex.search(self.config.middle_pause_regex, after_str[0:match.start()].strip())
                    if pause_match:
                        suffix = after_str[match.end():].strip()

                        ending_match = regex.search(self.config.general_ending_regex, suffix)
                        if ending_match:
                            tokens.append(Token(er.start, er.start + er.length + match.end()))

            before_str = source[0:er.start]
            match = regex.search(self.config.period_time_of_day_with_date_regex, before_str)

            if match:
                if not before_str[match.end():].strip():
                    middle_str = source[match.end():er.start]
                    if middle_str == ' ':
                        tokens.append(Token(match.start(), er.start + er.length))
                else:
                    pause_match = regex.search(self.config.middle_pause_regex, before_str[match.end():])
                    if pause_match:
                        suffix = source[er.start + er.length:].strip()

                        ending_match = regex.search(self.config.general_ending_regex, suffix)
                        if ending_match:
                            tokens.append(Token(match.start(), er.start + er.length))

            # check whether there are adjacent time period strings, before or after
            for token in tokens:
                # try to extract a time period in before-string
                if token.start > 0:
                    before_str = source[0:token.start].strip()
                    if before_str:
                        ers_time = self.config.time_period_extractor.extract(before_str, reference)

                        for er_time in ers_time:
                            middle_str = before_str[er_time.start + er_time.length:].strip()
                            if not middle_str:
                                tokens.append(Token(er_time.start, er_time.start + er_time.length + len(middle_str) + token.length))

                if token.start + token.length <= len(source):
                    after_str = source[token.start + token.length:]
                    if after_str:
                        ers_time = self.config.time_period_extractor.extract(after_str, reference)
                        for er_time in ers_time:
                            middle_str = after_str[0:er_time.start]
                            if not middle_str:
                                token_end = token.start + token.length + len(middle_str) + er_time.length
                                tokens.append(Token(token.start, token_end))

        return tokens

    def match_relative_unit(self, source: str) -> List[Token]:
        tokens: List[Token] = list()
        matches = list(regex.finditer(self.config.relative_time_unit_regex, source))

        if not matches:
            matches = list(regex.finditer(self.config.rest_of_date_time_regex, source))

        tokens.extend(map(lambda x: Token(x.start(), x.end()), matches))
        return tokens

class DateTimePeriodParserConfiguration:
    @property
    @abstractmethod
    def pure_number_from_to_regex(self) -> Pattern:
        raise NotImplementedError

    @property
    @abstractmethod
    def pure_number_between_and_regex(self) -> Pattern:
        raise NotImplementedError

    @property
    @abstractmethod
    def period_time_of_day_with_date_regex(self) -> Pattern:
        raise NotImplementedError

    @property
    @abstractmethod
    def specific_time_of_day_regex(self) -> Pattern:
        raise NotImplementedError

    @property
    @abstractmethod
    def past_regex(self) -> Pattern:
        raise NotImplementedError

    @property
    @abstractmethod
    def future_regex(self) -> Pattern:
        raise NotImplementedError

    @property
    @abstractmethod
    def relative_time_unit_regex(self) -> Pattern:
        raise NotImplementedError

    @property
    @abstractmethod
    def rest_of_date_time_regex(self) -> Pattern:
        raise NotImplementedError

    @property
    @abstractmethod
    def numbers(self) -> Dict[str, int]:
        raise NotImplementedError

    @property
    @abstractmethod
    def unit_map(self) -> Dict[str, str]:
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
    def date_time_extractor(self) -> DateTimeExtractor:
        raise NotImplementedError

    @property
    @abstractmethod
    def time_period_extractor(self) -> DateTimeExtractor:
        raise NotImplementedError

    @property
    @abstractmethod
    def duration_extractor(self) -> DateTimeExtractor:
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
    def date_time_parser(self) -> DateTimeParser:
        raise NotImplementedError

    @property
    @abstractmethod
    def time_period_parser(self) -> DateTimeParser:
        raise NotImplementedError

    @property
    @abstractmethod
    def duration_parser(self) -> DateTimeParser:
        raise NotImplementedError

    @abstractmethod
    def get_matched_time_range(self, source: str) -> MatchedTimeRange:
        raise NotImplementedError

    @abstractmethod
    def get_swift_prefix(self, source: str) -> int:
        raise NotImplementedError

class BaseDateTimePeriodParser(DateTimeParser):
    @property
    def parser_type_name(self) -> str:
        return Constants.SYS_DATETIME_DATETIMEPERIOD

    def __init__(self, config: DateTimePeriodParserConfiguration):
        self.config = config

    def parse(self, source: ExtractResult, reference: datetime = None) -> Optional[DateTimeParseResult]:
        if reference is None:
            reference = datetime.now()

        result = DateTimeParseResult(source)

        if source.type is self.parser_type_name:
            source_text = source.text.lower()

            inner_result = self.merge_date_and_time_periods(source_text, reference)

            if not inner_result.success:
                inner_result = self.merge_two_time_points(source_text, reference)

            if not inner_result.success:
                inner_result = self.parse_specific_time_of_day(source_text, reference)

            if not inner_result.success:
                inner_result = self.parse_duration(source_text, reference)

            if not inner_result.success:
                inner_result = self.parse_relative_unit(source_text, reference)

            if inner_result.success:
                inner_result.future_resolution[TimeTypeConstants.START_DATETIME] = FormatUtil.format_date_time(inner_result.future_value[0])
                inner_result.future_resolution[TimeTypeConstants.END_DATETIME] = FormatUtil.format_date_time(inner_result.future_value[1])
                inner_result.past_resolution[TimeTypeConstants.START_DATETIME] = FormatUtil.format_date_time(inner_result.past_value[0])
                inner_result.past_resolution[TimeTypeConstants.END_DATETIME] = FormatUtil.format_date_time(inner_result.past_value[1])
                result.value = inner_result
                result.timex_str = inner_result.timex if inner_result is not None else ''
                result.resolution_str = ''

        return result

    def merge_date_and_time_periods(self, source: str, reference: datetime) -> DateTimeResolutionResult:
        result = DateTimeResolutionResult()
        source = source.strip().lower()

        er = next(iter(self.config.time_period_extractor.extract(source, reference)), None)
        if er is None:
            return self.parse_simple_cases(source, reference)

        time_period_parse_result = self.config.time_period_parser.parse(er)
        time_period_resolution_result = time_period_parse_result.value

        if time_period_resolution_result is None:
            return self.parse_simple_cases(source, reference)

        time_period_timex: str = time_period_resolution_result.timex

        # if it is a range type timex
        if time_period_timex and time_period_timex.startswith('('):
            date_result = next(iter(self.config.date_extractor.extract(source.replace(er.text, ''), reference)), None)
            date_str = ''
            future_time: datetime = None
            past_time: datetime = None

            if date_result and source.replace(er.text, '').strip() == date_result.text:
                pr = self.config.date_parser.parse(date_result, reference)
                if pr.value:
                    future_time = pr.value.future_value
                    past_time = pr.value.past_value
                    date_str = pr.timex_str
                else:
                    return self.parse_simple_cases(source, reference)

                time_period_timex = time_period_timex.replace('(', '').replace(')', '')
                time_period_timex_array = time_period_timex.split(',')
                time_period_future_value = time_period_resolution_result.future_value
                begin_time: datetime = time_period_future_value.start
                end_time: datetime = time_period_future_value.end

                if len(time_period_timex_array) == 3:
                    begin_str = date_str + time_period_timex_array[0]
                    end_str = date_str + time_period_timex_array[1]

                    result.timex = f'({begin_str},{end_str},{time_period_timex_array[2]})'
                    result.future_value = [
                        DateUtils.safe_create_from_min_value(future_time.year, future_time.month, future_time.day, begin_time.hour, begin_time.minute, begin_time.second),
                        DateUtils.safe_create_from_min_value(future_time.year, future_time.month, future_time.day, end_time.hour, end_time.minute, end_time.second)
                    ]
                    result.past_value = [
                        DateUtils.safe_create_from_min_value(past_time.year, past_time.month, past_time.day, begin_time.hour, begin_time.minute, begin_time.second),
                        DateUtils.safe_create_from_min_value(past_time.year, past_time.month, past_time.day, end_time.hour, end_time.minute, end_time.second)
                    ]

                    if time_period_resolution_result.comment == 'ampm':
                        result.comment = 'ampm'

                    result.success = True
                    result.sub_date_time_entities = [pr, time_period_parse_result]

                    return result
            else:
                return self.parse_simple_cases(source, reference)

        return self.parse_simple_cases(source, reference)

    def parse_simple_cases(self, source: str, reference: datetime) -> DateTimeResolutionResult:
        result = DateTimeResolutionResult()
        match = regex.search(self.config.pure_number_from_to_regex, source)

        if not match:
            match = regex.search(self.config.pure_number_between_and_regex, source)

        if not match or match.start() != 0:
            return result

        hour_group = RegExpUtility.get_group_list(match, 'hour')
        begin_hour = self.config.numbers.get(hour_group[0])

        if not begin_hour:
            begin_hour = int(hour_group[0])
        end_hour = self.config.numbers.get(hour_group[1])

        if not end_hour:
            end_hour = int(hour_group[1])

        er = next(iter(self.config.date_extractor.extract(source.replace(match.group(), ''), reference)), None)

        if not er:
            return result

        pr = self.config.date_parser.parse(er, reference)

        if not pr:
            return result

        date_result: DateTimeResolutionResult = pr.value
        future_date: datetime = date_result.future_value
        past_date: datetime = date_result.past_value
        date_str = pr.timex_str

        has_am = False
        has_pm = False
        am_str = RegExpUtility.get_group(match, 'am')
        pm_str = RegExpUtility.get_group(match, 'pm')
        desc_str = RegExpUtility.get_group(match, 'desc')

        if am_str or desc_str.startswith('a'):
            if begin_hour >= 12:
                begin_hour = begin_hour - 12
            if end_hour >= 12:
                end_hour = end_hour - 12
            has_am = True

        if pm_str or desc_str.startswith('p'):
            if begin_hour < 12:
                begin_hour = begin_hour + 12
            if end_hour < 12:
                end_hour = end_hour + 12
            has_pm = True

        if not has_am and not has_pm and begin_hour <= 12 and end_hour <= 12:
            result.comment = 'ampm'

        begin_str = f'{date_str}T{begin_hour:02d}'
        end_str = f'{date_str}T{end_hour:02d}'

        result.timex = f'({begin_str},{end_str},PT{end_hour - begin_hour}H)'
        result.future_value = [
            DateUtils.safe_create_from_min_value(future_date.year, future_date.month, future_date.day, begin_hour, 0, 0),
            DateUtils.safe_create_from_min_value(future_date.year, future_date.month, future_date.day, end_hour, 0, 0)
        ]
        result.past_value = [
            DateUtils.safe_create_from_min_value(past_date.year, past_date.month, past_date.day, begin_hour, 0, 0),
            DateUtils.safe_create_from_min_value(past_date.year, past_date.month, past_date.day, end_hour, 0, 0)
        ]

        result.success = True
        return result

    def get_two_points(self, begin_er: ExtractResult, end_er: ExtractResult, begin_parser: DateTimeParser, end_parser: DateTimeParser, reference: datetime) -> BeginEnd:
        return BeginEnd(begin=begin_parser.parse(begin_er, reference), end=end_parser.parse(end_er, reference))

    def merge_two_time_points(self, source: str, reference: datetime) -> DateTimeResolutionResult:
        result = DateTimeResolutionResult()

        prs: BeginEnd = None
        time_ers = self.config.time_extractor.extract(source, reference)
        datetime_ers = self.config.date_time_extractor.extract(source, reference)

        both_has_date = False
        begin_has_date = False
        end_has_date = False

        if len(datetime_ers) == 2:
            prs = self.get_two_points(datetime_ers[0], datetime_ers[1], self.config.date_time_parser, self.config.date_time_parser, reference)
            both_has_date = True
        elif len(datetime_ers) == 1 and len(time_ers) == 2:
            if datetime_ers[0].overlap(time_ers[0]):
                prs = self.get_two_points(datetime_ers[0], time_ers[1], self.config.date_time_parser, self.config.time_parser, reference)
                begin_has_date = True
            else:
                prs = self.get_two_points(time_ers[0], datetime_ers[0], self.config.time_parser, self.config.date_time_parser, reference)
                end_has_date = True
        elif len(datetime_ers) == 1 and len(time_ers) == 1:
            if time_ers[0].start < datetime_ers[0].start:
                prs = self.get_two_points(time_ers[0], datetime_ers[0], self.config.time_parser, self.config.date_time_parser, reference)
                end_has_date = True
            else:
                prs = self.get_two_points(datetime_ers[0], time_ers[0], self.config.date_time_parser, self.config.time_parser, reference)
                begin_has_date = True

        if prs is None or not prs.begin.value or not prs.end.value:
            return result

        begin: DateTimeResolutionResult = prs.begin.value
        end: DateTimeResolutionResult = prs.end.value

        future_begin: datetime = begin.future_value
        future_end: datetime = end.future_value
        past_begin: datetime = begin.past_value
        past_end: datetime = end.past_value

        if both_has_date:
            if future_begin > future_end:
                future_begin = past_begin

            if past_end < past_begin:
                past_end = future_end

            total_hours = DateUtils.total_hours(future_begin, future_end)
            result.timex = f'({prs.begin.timex_str},{prs.end.timex_str},PT{total_hours}H)'
        elif begin_has_date:
            future_end = self.get_datetime(future_begin, future_end)
            past_end = self.get_datetime(past_begin, past_end)
            total_hours = DateUtils.total_hours(future_begin, future_end)
            date_str = prs.begin.timex_str.split('T').pop()
            result.timex = f'({prs.begin.timex_str},{date_str}{prs.end.timex_str},PT{total_hours}H)'
        elif end_has_date:
            future_begin = self.get_datetime(future_end, future_begin)
            past_begin = self.get_datetime(past_end, past_begin)
            total_hours = DateUtils.total_hours(future_begin, future_end)
            date_str = prs.end.timex_str.split('T')[0]
            result.timex = f'({date_str}{prs.begin.timex_str},{prs.end.timex_str},PT{total_hours}H)'

        if begin.comment and begin.comment.endswith('ampm') and end.comment and end.comment.endswith('ampm'):
            result.comment = 'ampm'

        result.future_value = [future_begin, future_end]
        result.past_value = [past_begin, past_end]
        result.success = True
        result.sub_date_time_entities = [prs.begin, prs.end]

        return result

    def get_datetime(self, date: datetime, time: datetime) -> datetime:
        return DateUtils.safe_create_from_min_value(date.year, date.month, date.day, time.hour, time.minute, time.second)

    def parse_specific_time_of_day(self, source: str, reference: datetime) -> DateTimeResolutionResult:
        result = DateTimeResolutionResult()
        time_str = source
        has_early = False
        has_late = False

        match = regex.search(self.config.period_time_of_day_with_date_regex, source)
        if match:
            time_str = RegExpUtility.get_group(match, 'timeOfDay')
            if RegExpUtility.get_group(match, 'early'):
                has_early = True
                result.comment = 'early'
            elif RegExpUtility.get_group(match, 'late'):
                has_late = True
                result.comment = 'late'

        matched = self.config.get_matched_time_range(time_str)
        if not matched.success:
            return result

        if has_early:
            matched = MatchedTimeRange(matched.time_str, matched.begin_hour, matched.begin_hour + 2, 0 if matched.end_min == 59 else matched.end_min, matched.success)
        elif has_late:
            matched = MatchedTimeRange(matched.time_str, matched.begin_hour + 2, matched.end_hour, matched.end_min, matched.success)

        match = regex.search(self.config.specific_time_of_day_regex, source)
        if match and match.group() == source:
            swift = self.config.get_swift_prefix(source)
            date = reference + timedelta(days=swift)
            result.timex = FormatUtil.format_date(date) + matched.time_str
            result.future_value = [
                DateUtils.safe_create_from_min_value(date.year, date.month, date.day, matched.begin_hour, 0, 0),
                DateUtils.safe_create_from_min_value(date.year, date.month, date.day, matched.end_hour, matched.end_min, matched.end_min)
            ]
            result.past_value = [
                DateUtils.safe_create_from_min_value(date.year, date.month, date.day, matched.begin_hour, 0, 0),
                DateUtils.safe_create_from_min_value(date.year, date.month, date.day, matched.end_hour, matched.end_min, matched.end_min)
            ]
            result.success = True

            return result

        match = regex.search(self.config.period_time_of_day_with_date_regex, source)
        if not match:
            return result

        before_str = source[0:match.start()].strip()
        after_str = source[match.end():].strip()
        er = next(iter(self.config.date_extractor.extract(before_str, reference)), None)

        # eliminate time period, if any
        time_period_er = next(iter(self.config.time_period_extractor.extract(before_str)), None)
        if time_period_er:
            before_str = before_str[time_period_er.start:time_period_er.start + time_period_er.length].strip()
        else:
            time_period_er = next(iter(self.config.time_period_extractor.extract(after_str)), None)
            if time_period_er:
                after_str = after_str[time_period_er.start:time_period_er.start + time_period_er.length].strip()

        if not er or er.length != len(before_str):
            valid = False
            if er and er.start == 0:
                middle_str = before_str[er.start + er.length:]
                if not middle_str.replace(',', ''):
                    valid = True

            if not valid:
                er = next(iter(self.config.date_extractor.extract(after_str)), None)
                if not er or er.length != len(after_str):
                    if er and er.start + er.length == len(after_str):
                        valid = True
                else:
                    valid = True

                if not valid:
                    return result

        has_specific_time_period = False
        if time_period_er:
            time_pr = self.config.time_period_parser.parse(time_period_er)
            if time_pr:
                period_future = time_pr.value.future_value
                period_past = time_pr.value.past_value

                if period_future == period_past:
                    begin_hour: datetime = period_future.start
                    end_hour: datetime = period_future.end
                    matched = MatchedTimeRange(matched.time_str, begin_hour.hour, end_hour.hour, matched.end_min, matched.success)
                else:
                    if period_future.start.hour >= matched.begin_hour or period_future.end.hour <= matched.end_hour:
                        begin_hour: datetime = period_future.start
                        end_hour: datetime = period_future.end
                        matched = MatchedTimeRange(matched.time_str, begin_hour.hour, end_hour.hour, matched.end_min, matched.success)
                    else:
                        begin_hour: datetime = period_past.start
                        end_hour: datetime = period_past.end
                        matched = MatchedTimeRange(matched.time_str, begin_hour.hour, end_hour.hour, matched.end_min, matched.success)

                has_specific_time_period = True

        pr = self.config.date_parser.parse(er, reference)
        if not pr:
            return result

        future_date: datetime = pr.value.future_value
        past_date: datetime = pr.value.past_value

        if not has_specific_time_period:
            result.timex = pr.timex_str + matched.time_str
        else:
            result.timex = f'({pr.timex_str}T{matched.begin_hour},{pr.timex_str}T{matched.end_hour},PT{matched.end_hour - matched.begin_hour}H)'

        result.future_value = [
            DateUtils.safe_create_from_min_value(future_date.year, future_date.month, future_date.day, matched.begin_hour, 0, 0),
            DateUtils.safe_create_from_min_value(future_date.year, future_date.month, future_date.day, matched.end_hour, matched.end_min, matched.end_min)
        ]
        result.past_value = [
            DateUtils.safe_create_from_min_value(past_date.year, past_date.month, past_date.day, matched.begin_hour, 0, 0),
            DateUtils.safe_create_from_min_value(past_date.year, past_date.month, past_date.day, matched.end_hour, matched.end_min, matched.end_min)
        ]
        result.success = True

        return result

    def parse_duration(self, source: str, reference: datetime) -> DateTimeResolutionResult:
        result = DateTimeResolutionResult()

        # for rest of datetime, it will be handled in next function
        if regex.search(self.config.rest_of_date_time_regex, source):
            return result

        ers = self.config.duration_extractor.extract(source, reference)
        if len(ers) != 1:
            return result

        pr = self.config.duration_parser.parse(ers[0], source)
        if not pr:
            return result

        before_str = source[0:pr.start].strip()
        duration_result: DateTimeResolutionResult = pr.value
        swift_second = 0
        mod = ''

        if isinstance(duration_result.past_value, int) and isinstance(duration_result.future_value, int):
            swift_second = int(duration_result.future_value)

        begin_time: datetime = reference
        end_time: datetime = reference

        prefix_match = regex.search(self.config.past_regex, before_str)
        if prefix_match and prefix_match.group() == before_str:
            mod = TimeTypeConstants.BEFORE_MOD
            begin_time = begin_time - timedelta(seconds=swift_second)

        prefix_match = regex.search(self.config.future_regex, before_str)
        if prefix_match and prefix_match.group() == before_str:
            mod = TimeTypeConstants.AFTER_MOD
            end_time = begin_time + timedelta(seconds=swift_second)

        luis_date_begin = FormatUtil.luis_date_from_datetime(begin_time)
        luis_time_begin = FormatUtil.luis_time_from_datetime(begin_time)
        luis_date_end = FormatUtil.luis_date_from_datetime(end_time)
        luis_time_end = FormatUtil.luis_time_from_datetime(end_time)

        result.timex = f'({luis_date_begin}T{luis_time_begin},{luis_date_end}T{luis_time_end},{duration_result.timex})'
        result.future_value = [begin_time, end_time]
        result.past_value = [begin_time, end_time]
        result.success = True

        if mod:
            pr.value.mod = mod

        result.sub_date_time_entities = [pr]

        return result

    def parse_relative_unit(self, source: str, reference: datetime) -> DateTimeResolutionResult:
        result = DateTimeResolutionResult()
        match = regex.search(self.config.relative_time_unit_regex, source)

        if not match:
            match = regex.search(self.config.rest_of_date_time_regex, source)

        if not match:
            return result

        src_unit = RegExpUtility.get_group(match, 'unit')
        unit_str = self.config.unit_map.get(src_unit, None)

        if not unit_str:
            return result

        swift = 1
        prefix_match = regex.search(self.config.past_regex, source)
        if prefix_match:
            swift = -1

        begin_time: datetime = reference
        end_time: datetime = reference

        pt_timex = ''

        if unit_str == 'D':
            end_time = DateUtils.safe_create_from_min_value(begin_time.year, begin_time.month, begin_time.day) + timedelta(days=1, seconds=-1)
            difference = int((end_time - begin_time).total_seconds())
            pt_timex = f'PT{difference}S'
        elif unit_str == 'H':
            begin_time = begin_time + timedelta(hours=0 if swift > 0 else swift)
            end_time = end_time + timedelta(hours=swift if swift > 0 else 0)
            pt_timex = 'PT1H'
        elif unit_str == 'M':
            begin_time = begin_time + timedelta(minutes=0 if swift > 0 else swift)
            end_time = end_time + timedelta(minutes=swift if swift > 0 else 0)
            pt_timex = 'PT1M'
        elif unit_str == 'S':
            begin_time = begin_time + timedelta(seconds=0 if swift > 0 else swift)
            end_time = end_time + timedelta(seconds=swift if swift > 0 else 0)
            pt_timex = 'PT1S'
        else:
            return result

        luis_date_begin = FormatUtil.luis_date_from_datetime(begin_time)
        luis_time_begin = FormatUtil.luis_time_from_datetime(begin_time)
        luis_date_end = FormatUtil.luis_date_from_datetime(end_time)
        luis_time_end = FormatUtil.luis_time_from_datetime(end_time)

        result.timex = f'({luis_date_begin}T{luis_time_begin},{luis_date_end}T{luis_time_end},{pt_timex})'
        result.future_value = [begin_time, end_time]
        result.past_value = [begin_time, end_time]
        result.success = True

        return result
