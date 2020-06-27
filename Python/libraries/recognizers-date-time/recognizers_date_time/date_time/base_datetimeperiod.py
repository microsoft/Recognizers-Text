from abc import abstractmethod
from typing import List, Optional, Pattern, Dict, Match
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
from .utilities import Token, merge_all_tokens, RegExpUtility, DateTimeFormatUtil, DateTimeResolutionResult, \
    DateUtils, RegExpUtility, DateTimeOptionsConfiguration, DateTimeOptions, TimexUtil


class MatchedTimeRange:
    def __init__(self, time_str: str = '', begin_hour: int = 0, end_hour: int = 0, end_min: int = 0,
                 success: bool = False, swift: int = 0):
        self.time_str = time_str
        self.begin_hour = begin_hour
        self.end_hour = end_hour
        self.end_min = end_min
        self.swift = swift
        self.success = success


MatchedIndex = namedtuple('MatchedIndex', ['matched', 'index'])
BeginEnd = namedtuple('BeginEnd', ['begin', 'end'])


class DateTimePeriodExtractorConfiguration(DateTimeOptionsConfiguration):

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
    def previous_prefix_regex(self) -> Pattern:
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

    @abstractmethod
    def token_before_date(self) -> str:
        raise NotImplementedError

    @property
    @abstractmethod
    def within_next_prefix_regex(self) -> Pattern:
        raise NotImplementedError

    @property
    @abstractmethod
    def future_suffix_regex(self) -> Pattern:
        raise NotImplementedError

    @property
    @abstractmethod
    def date_unit_regex(self) -> Pattern:
        raise NotImplementedError

    @property
    @abstractmethod
    def am_desc_regex(self) -> Pattern:
        raise NotImplementedError

    @property
    @abstractmethod
    def pm_desc_regex(self) -> Pattern:
        raise NotImplementedError

    @property
    @abstractmethod
    def prefix_day_regex(self) -> Pattern:
        raise NotImplementedError

    @property
    @abstractmethod
    def before_regex(self) -> Pattern:
        raise NotImplementedError

    @property
    @abstractmethod
    def after_regex(self) -> Pattern:
        raise NotImplementedError

    @property
    @abstractmethod
    def suffix_regex(self) -> Pattern:
        raise NotImplementedError

    @property
    @abstractmethod
    def check_both_before_after(self) -> Pattern:
        raise NotImplementedError

    @property
    @abstractmethod
    def time_zone_extractor(self) -> DateTimeExtractor:
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

        # Date and time Extractions should be extracted from the text only once,
        # and shared in the methods below, passed by value

        date_ers = self.config.single_date_extractor.extract(source, reference)
        time_ers = self.config.single_time_extractor.extract(source, reference)

        tokens = []
        tokens.extend(self.match_simple_cases(source, reference))
        tokens.extend(self.merge_two_time_points(source, reference, list(date_ers), list(time_ers)))
        tokens.extend(self.match_duration(source, reference))
        tokens.extend(self.match_time_of_day(source, reference, date_ers))
        tokens.extend(self.match_relative_unit(source))
        tokens.extend(self.match_date_with_period_prefix(source, reference, date_ers))
        tokens.extend(self.merge_date_with_time_period_suffix(source, date_ers, time_ers))

        result = merge_all_tokens(tokens, source, self.extractor_type_name)

        if (self.config.options & DateTimeOptions.ENABLE_PREVIEW) != 0:
            # When TimeZone is migrated enable it
            pass

        return result

    def match_date_with_period_prefix(self, source, reference, date_ers):
        result = []

        for date_er in date_ers:
            date_str_end = date_er.start + date_er.length
            before_str = source[0:date_er.start].strip()
            match = regex.search(self.config.prefix_day_regex, before_str)
            if match:
                result.append(Token(match.start(), date_str_end))
            elif self.config.check_both_before_after:
                # check also after_str
                after_str = source[date_str_end: len(source) - date_str_end]
                match_after = RegExpUtility.match_begin(self.config.prefix_day_regex, after_str, True)
                if match_after and match_after.success:
                    result.append(Token(date_er.start, date_str_end + match_after.index + match_after.length))
        return result

    # Cases like "today after 2:00pm", "1/1/2015 before 2:00 in the afternoon"
    def merge_date_with_time_period_suffix(self, text: str, date_ers: [ExtractResult], time_ers: [ExtractResult]):
        result = []

        if not any(date_ers):
            return result

        if not any(time_ers):
            return result

        extracted_results: [Match] = date_ers
        extracted_results.extend(time_ers)

        ers = sorted(extracted_results, key=lambda x: x.start)

        i = 0

        while i < len(extracted_results) - 1:

            j = i + 1
            while j < len(extracted_results) and extracted_results[i].overlap(extracted_results[j]):
                j += 1

            if j >= len(extracted_results):
                break

            if extracted_results[i].type == Constants.SYS_DATETIME_DATE and\
                    extracted_results[j].type == Constants.SYS_DATETIME_TIME:
                middle_begin = extracted_results[i].start + (extracted_results[i].length or 0)
                middle_end = extracted_results[j].start or 0
                if middle_begin > middle_end:
                    i = j + 1
                    continue

                middle_str = text[middle_begin: middle_end].strip()

                if self.is_valid_connector_for_date_and_time_period(middle_str):
                    begin = ers[i].start or 0
                    end = (ers[j].start or 0) + (ers[j].length or 0)
                    result.append(Token(begin, end))
                elif self.config.check_both_before_after:
                    # Check also after_str
                    after_start = ers[j].start + ers[j].length or 0
                    after_str = text[:after_start]
                    if self.is_valid_connector_for_date_and_time_period(after_str):
                        begin = ers[i].start or 0
                        end = (ers[j].start or 0) + (ers[j].length or 0)
                        result.append(Token(begin, end))

                i = j + 1
                continue

            i = j

        idx = 0
        for idx in range(idx, len(result)-1, 1):
            after_str = text[result[idx].end]
            match = self.config.suffix_regex.search(after_str)
            if match:
                result[idx] = Token(result[idx].start, result[idx].end + (match.end() - match.start()))

        return result

    def is_valid_connector_for_date_and_time_period(self, text: str):
        before_after_regexes = [self.config.before_regex, self.config.after_regex]
        for regexp in before_after_regexes:

            if RegExpUtility.is_exact_match(regexp, text, True):
                return True

        return False

    def match_time_of_day(self, source: str, reference: datetime, date_extract_results: [ExtractResult] = None):
        tokens = []
        result = []

        matches = list(regex.finditer(self.config.specific_time_of_day_regex, source))
        for match in matches:
            tokens.append(Token(match.start(), match.end()))

        # Date followed by morning, afternoon or morning, afternoon followed by Date
        if len(date_extract_results) == 0:
            return tokens

        for extracted_result in date_extract_results:
            after_str = source[extracted_result.start + extracted_result.length:]

            match = regex.search(self.config.period_time_of_day_with_date_regex, after_str)

            if match:
                # For cases like "Friday afternoon between 1PM and 4PM" which "Friday afternoon" need to be
                # extracted first
                match_start = match.start()
                if not after_str[0:match.start()] or after_str[0:match.start()].isspace():
                    start = extracted_result.start
                    end = extracted_result.start + extracted_result.length + len(
                        RegExpUtility.get_group(match, Constants.TIME_OF_DAY_GROUP_NAME)) + \
                        match.start(Constants.TIME_OF_DAY_GROUP_NAME)

                    tokens.append(Token(start, end))
                    break

                connector_str = after_str[0:match.start()]

                # Trim here is set to false as the Regex might catch white spaces before or after the text
                if RegExpUtility.is_exact_match(self.config.middle_pause_regex, connector_str, False):
                    suffix = after_str[match.end():].strip()

                    ending_match = regex.search(self.config.general_ending_regex, suffix)
                    if ending_match:
                        tokens.append(Token(extracted_result.start, extracted_result.start +
                                            extracted_result.length + match.end()))

            if not match:
                match = regex.search(self.config.am_desc_regex, after_str)

            if not match or after_str[0:match.start()]:
                match = regex.search(self.config.pm_desc_regex, after_str)

            if match:
                if not after_str[0:match.start()]:
                    tokens.append(Token(extracted_result.start, extracted_result.end + match.end()))

            prefix_str = source[0: extracted_result.start]

            match = regex.search(self.config.period_time_of_day_with_date_regex, prefix_str)
            if match:
                if not prefix_str[match.end():] or prefix_str[match.end():].isspace():
                    mid_str = source[match.end(): extracted_result.start]
                    if mid_str and mid_str.isspace():
                        tokens.append(Token(match.start(), extracted_result.start + extracted_result.length))
                        m_start = match.start()
                else:
                    connector_str = prefix_str[match.end():]

                    # Trim here is set to false as the Regex might catch white spaces before or after the text
                    if RegExpUtility.is_exact_match(self.config.middle_pause_regex, connector_str, False):
                        suffix = source[extracted_result.start + extracted_result.length:].strip(' ')
                        ending_match = self.config.general_ending_regex.match(suffix)
                        if ending_match:
                            tokens.append(Token(match.start(), extracted_result.start + extracted_result.length))

        result = list(tokens)

        # Check whether there are adjacent time period strings, before or after
        for token in result:
            # Try to extract a time period in before-string
            if token.start > 0:
                before_str = source[0:token.start]
                if before_str:
                    time_extract_results = self.config.time_period_extractor.extract(before_str)
                    if len(time_extract_results) > 0:
                        for time_period in time_extract_results:
                            mid_str = before_str[time_period.start + time_period.length:]
                            if (not mid_str or mid_str.isspace()) and not time_period.meta_data:
                                tokens.append(Token(time_period.start, time_period.start + time_period.length +
                                                    len(mid_str) + token.length))

            # Try to extract a time period in after-string
            if token.end <= len(source):
                after_str = source[token.start + token.length:]
                if after_str:
                    time_extract_results = self.config.time_period_extractor.extract(after_str)
                    if len(time_extract_results) > 0:
                        for time_period in time_extract_results:
                            mid_str = after_str[0:time_period.start]
                            if (not mid_str or mid_str.isspace()) and not time_period.meta_data:
                                tokens.append(Token(token.start, token.end + len(mid_str) + time_period.length))

        return tokens

    def match_simple_cases(self, source: str, reference: datetime) -> List[Token]:
        tokens: List[Token] = list()
        source = source.strip().lower()
        simple_cases_matches = list(filter(lambda t: t != [], map(lambda x: list(
            regex.finditer(x, source)), self.config.simple_cases_regexes)))

        for matches in simple_cases_matches:
            for match in matches:
                # has a date before?
                has_before_date = False

                before_str = source[0:match.start()].strip()
                if before_str:
                    er = next(reversed(self.config.single_date_extractor.extract(
                        before_str, reference)), None)
                    if er:
                        begin = er.start
                        end = er.start + er.length
                        middle_str = before_str[end:].strip()
                        if not middle_str or RegExpUtility.is_exact_match(self.config.preposition_regex, middle_str, True):
                            tokens.append(Token(begin, match.end()))
                            has_before_date = True

                followed_str = source[match.end():]
                if followed_str and not has_before_date:
                    extracted_result = next(iter(self.config.single_date_extractor.extract(
                        followed_str, reference)), None)
                    if extracted_result:
                        begin = extracted_result.start
                        end = extracted_result.start + extracted_result.length
                        middle_str = followed_str[0:begin].strip()
                        if middle_str == '' or regex.search(self.config.preposition_regex, middle_str):
                            tokens.append(
                                Token(match.start(), match.end() + end))
        return tokens

    def merge_two_time_points(self, source: str, reference: datetime, date_ers: [ExtractResult],
                              time_ers: [ExtractResult]) -> List[Token]:
        tokens = []
        ers_datetime = self.config.single_date_time_extractor.extract(source, reference)
        time_points = []

        # Handle the overlap problem
        j = 0
        for er_datetime in ers_datetime:
            time_points.append(er_datetime)

            while j < len(time_ers) and time_ers[j].start + time_ers[j].length < er_datetime.start:
                time_points.append(time_ers[j])
                j += 1

            while j < len(time_ers) and time_ers[j].overlap(er_datetime):
                j += 1

        while j < len(time_ers):
            time_points.append(time_ers[j])
            j += 1
        time_points = sorted(time_points, key=lambda x: x.start)

        # Merge "{TimePoint} to {TimePoint}", "between {TimePoint} and {TimePoint}"
        index = 0
        while index < len(time_points) - 1:
            if time_points[index].type == Constants.SYS_DATETIME_TIME and time_points[index + 1].type == \
                    Constants.SYS_DATETIME_TIME:
                index += 1
                break

            middle_begin = time_points[index].start + time_points[index].length
            middle_end = time_points[index + 1].start

            middle_str = source[middle_begin:middle_end].strip().lower()

            # Handle "{TimePoint} to {TimePoint}"
            if RegExpUtility.is_exact_match(self.config.till_regex, middle_str, True):
                period_begin = time_points[index].start
                period_end = time_points[index + 1].start + time_points[index + 1].length

                # Handle "from"
                before_str = source[0:period_begin].strip()

                match_from = self.config.get_from_token_index(before_str)
                from_token_index = match_from if match_from.matched else self.config.get_between_token_index(
                    before_str)
                if from_token_index.matched:
                    period_begin = from_token_index.index
                elif self.config.check_both_before_after:
                    after_str = source[period_end:len(source) - period_end]
                    after_token_index = self.config.get_between_token_index(after_str)
                    if after_token_index.matched:
                        # Handle "between" in after_str
                        period_end += after_token_index.index

                tokens.append(Token(period_begin, period_end))
                index += 2
                break

            # Handle "between {TimePoint} and {TimePoint}"
            if self.config.has_connector_token(middle_str):
                period_begin = time_points[index].start
                period_end = time_points[index + 1].start + time_points[index + 1].length

                before_str = source[0:period_begin].strip()
                before_token_index = self.config.get_between_token_index(before_str)
                if before_token_index.matched:
                    period_begin = before_token_index.index
                    tokens.append(Token(period_begin, period_end))
                    index += 2
                    break

            index += 1

        # Regarding the phrase as-- {Date} {TimePeriod}, like "2015-9-23 1pm to 4"
        # Or {TimePeriod} ond {Date}, like "1:30 to 4 2015-9-23"
        ers_time_period = self.config.time_period_extractor.extract(source, reference)

        for er_time_period in ers_time_period:
            if not er_time_period.meta_data:
                date_ers.append(er_time_period)

        points: List[ExtractResult] = sorted(date_ers, key=lambda x: x.start)

        index = 0
        while index < len(points) - 1:
            if points[index].type == points[index + 1].type:
                break

            mid_begin = points[index].start + points[index].length
            mid_end = points[index + 1].start

            if mid_end - mid_begin > 0:
                mid_str = source[mid_begin:mid_end]
                if not mid_str.strip() or mid_str.strip().startswith(self.config.token_before_date):
                    # Extend date extraction for cases like "Monday evening next week"
                    extended_str = points[index].text + source[int(points[index + 1].start + points[index + 1].length):]
                    extended_date_str = self.config.single_date_extractor.extract(extended_str)
                    offset = 0
                    if extended_date_str is not None and extended_date_str.index == 0:
                        offset = int(len(extended_date_str) - points[index].length)

                    tokens.append(Token(points[index].start,
                                        offset + points[index + 1].start + points[index + 1].length))
                    index += 2

            index += 1

        return tokens

    def match_duration(self, source: str, reference: datetime) -> List[Token]:
        tokens: List[Token] = list()
        source = source.strip().lower()

        ers_duration: List[ExtractResult] = self.config.duration_extractor.extract(
            source, reference)
        durations: List[Token] = list()

        for extracted_result in ers_duration:
            if regex.search(self.config.time_unit_regex, extracted_result.text):
                durations.append(Token(extracted_result.start, extracted_result.start + extracted_result.length))

        for duration in durations:
            before_str = source[0:duration.start].strip()
            after_str = source[duration.start + duration.length:].strip()
            if not before_str and not after_str:
                break

            # within (the) (next) "Seconds/Minutes/Hours" should be handled as datetimeRange here
            # within (the) (next) XX days/months/years + "Seconds/Minutes/Hours" should
            # also be handled as datetimeRange here
            token = self.match_within_next_prefix(before_str, source, duration, True)
            if token.start >= 0:
                tokens.append(token)
                break

            # check also afterStr
            if self.config.check_both_before_after:
                token = self.match_within_next_prefix(after_str, source, duration, False)
                if token.start >= 0:
                    tokens.append(token)
                    break

            match = RegExpUtility.match_end(self.config.previous_prefix_regex, before_str, True)
            index = -1
            if match and match.success:
                index = match.index
            if index < 0:
                # For cases like 'next five days'
                match = RegExpUtility.match_end(self.config.next_prefix_regex, before_str, True)
                if match and match.success:
                    index = match.index
            if index >= 0:
                prefix = before_str[0: index].strip()
                duration_text = source[duration.start: duration.length]
                numbers_in_prefix = self.config.cardinal_extractor.extract(prefix)
                numbers_in_duration = self.config.cardinal_extractor.extract(duration_text)

                # Cases like "2 upcoming days", should be supported here
                # Cases like "2 upcoming 3 days" is invalid, only extract "upcoming 3 days" by default
                if any(numbers_in_prefix) and not any(numbers_in_duration):
                    last_number = sorted(numbers_in_prefix, key=lambda t: t.start + t.length).pop()

                    # Prefix should end with the last number
                    if last_number.start + last_number.length == len(prefix):
                        tokens.append(Token(last_number.start, duration.end))
                else:
                    tokens.append(Token(index, duration.end))
                continue

            match_date_unit = regex.search(self.config.date_unit_regex, after_str)
            if not match_date_unit:
                # Match suffix
                match = RegExpUtility.match_begin(self.config.previous_prefix_regex, after_str, True)

                if match and match.success:
                    tokens.append(Token(duration.start, duration.end + match.index + match.length + 1))
                    continue

                match = RegExpUtility.match_begin(self.config.next_prefix_regex, after_str, True)

                if match and match.success:
                    tokens.append(Token(duration.start, duration.end + match.index + match.length))
                    continue

                match = RegExpUtility.match_begin(self.config.future_suffix_regex, after_str, True)

                if match and match.success:
                    tokens.append(Token(duration.start, duration.end + match.index + match.length))
                    continue
        return tokens

    def match_within_next_prefix(self, sub_str: str, source: str, duration: Token, in_prefix: bool) -> Token:
        start_out = end_out = -1
        success = False
        match = self.config.within_next_prefix_regex.match(sub_str)

        if self.match_prefix_regex_in_segment(sub_str, match, in_prefix):
            if in_prefix:
                start_token = source.index(match.group())
                end_token = duration.end + 0
            else:
                start_token = duration.start
                end_token = duration.end + (source.index(match.group()) + duration.length)
            match = self.config.time_unit_regex.match(source[duration.start: duration.length])
            success = match

            if not in_prefix:
                # Match prefix for "next"
                before_str = source[0:duration.start]
                match_next = self.config.next_prefix_regex.match(before_str)
                success = match or match_next
                if self.match_prefix_regex_in_segment(before_str, match_next, True):
                    start_token = match_next.start

            if success:
                start_out, end_out = start_token, end_token

        return Token(start_out, end_out)

    def match_night(self, source: str, reference: datetime) -> List[Token]:
        tokens: List[Token] = list()
        source = source.strip().lower()

        matches = regex.finditer(
            self.config.specific_time_of_day_regex, source)
        tokens.extend(map(lambda x: Token(x.start(), x.end()), matches))

        ers_date: List[ExtractResult] = self.config.single_date_extractor.extract(
            source, reference)

        for extracted_result in ers_date:
            after_str = source[extracted_result.start + extracted_result.length:]
            match = regex.search(
                self.config.period_time_of_day_with_date_regex, after_str)

            if match:
                if not after_str[0:match.start()].strip():
                    tokens.append(
                        Token(extracted_result.start, extracted_result.start + extracted_result.length + match.end()))
                else:
                    pause_match = regex.search(
                        self.config.middle_pause_regex, after_str[0:match.start()].strip())
                    if pause_match:
                        suffix = after_str[match.end():].strip()

                        ending_match = regex.search(
                            self.config.general_ending_regex, suffix)
                        if ending_match:
                            tokens.append(
                                Token(extracted_result.start, extracted_result.start +
                                      extracted_result.length + match.end()))

            before_str = source[0:extracted_result.start]
            match = regex.search(
                self.config.period_time_of_day_with_date_regex, before_str)

            if match:
                if not before_str[match.end():].strip():
                    middle_str = source[match.end():extracted_result.start]
                    if middle_str == ' ':
                        tokens.append(
                            Token(match.start(), extracted_result.start + extracted_result.length))
                else:
                    pause_match = regex.search(
                        self.config.middle_pause_regex, before_str[match.end():])
                    if pause_match:
                        suffix = source[extracted_result.start + extracted_result.length:].strip()

                        ending_match = regex.search(
                            self.config.general_ending_regex, suffix)
                        if ending_match:
                            tokens.append(
                                Token(match.start(), extracted_result.start + extracted_result.length))

            # check whether there are adjacent time period strings, before or after
            for token in tokens:
                # try to extract a time period in before-string
                if token.start > 0:
                    before_str = source[0:token.start].strip()
                    if before_str:
                        ers_time = self.config.time_period_extractor.extract(
                            before_str, reference)

                        for er_time in ers_time:
                            middle_str = before_str[er_time.start +
                                                    er_time.length:].strip()
                            if not middle_str:
                                tokens.append(Token(er_time.start,
                                                    er_time.start + er_time.length + len(middle_str) + token.length))

                if token.start + token.length <= len(source):
                    after_str = source[token.start + token.length:]
                    if after_str:
                        ers_time = self.config.time_period_extractor.extract(
                            after_str, reference)
                        for er_time in ers_time:
                            middle_str = after_str[0:er_time.start]
                            if not middle_str:
                                token_end = token.start + token.length + \
                                    len(middle_str) + er_time.length
                                tokens.append(Token(token.start, token_end))

        return tokens

    @staticmethod
    def match_prefix_regex_in_segment(string: str, match: Match, in_prefix: bool):
        substring = ''
        if match:
            substring = string[match.start(): match.end()] if in_prefix else string[0: match.start()]
        return match and substring.strip() is not ''

    def match_relative_unit(self, source: str) -> List[Token]:
        tokens: List[Token] = list()
        matches = list(regex.finditer(
            self.config.relative_time_unit_regex, source))

        if not matches:
            matches = list(regex.finditer(
                self.config.rest_of_date_time_regex, source))

        tokens.extend(map(lambda x: Token(x.start(), x.end()), matches))
        return tokens


class DateTimePeriodParserConfiguration:
    @property
    @abstractmethod
    def future_suffix_regex(self):
        raise NotImplementedError

    @property
    @abstractmethod
    def within_next_prefix_regex(self):
        raise NotImplementedError

    @property
    @abstractmethod
    def previous_prefix_regex(self):
        raise NotImplementedError

    @property
    @abstractmethod
    def cardinal_extractor(self):
        raise NotImplementedError

    @property
    @abstractmethod
    def am_desc_regex(self):
        raise NotImplementedError

    @property
    @abstractmethod
    def pm_desc_regex(self):
        raise NotImplementedError

    @property
    @abstractmethod
    def before_regex(self):
        raise NotImplementedError

    @property
    @abstractmethod
    def after_regex(self):
        raise NotImplementedError

    @property
    @abstractmethod
    def prefix_day_regex(self):
        raise NotImplementedError

    @property
    @abstractmethod
    def token_before_date(self):
        raise NotImplementedError

    @property
    @abstractmethod
    def check_both_before_after(self) -> bool:
        raise NotImplementedError

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
    def time_of_day_regex(self) -> Pattern:
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
    def time_zone_parser(self) -> DateTimeParser:
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
    def get_matched_time_range(self, source: str):
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

            inner_result = self.merge_date_and_time_periods(
                source_text, reference)

            if not inner_result.success:
                inner_result = self.merge_two_time_points(
                    source_text, reference)

            if not inner_result.success:
                inner_result = self.parse_specific_time_of_day(
                    source_text, reference)

            if not inner_result.success:
                inner_result = self.parse_duration(source_text, reference)

            if not inner_result.success:
                inner_result = self.parse_relative_unit(source_text, reference)

            if not inner_result.success:
                inner_result = self.parse_date_with_period_prefix(source_text, reference)

            if not inner_result.success:
                inner_result = self.parse_date_with_time_period_suffix(source_text, reference)

            if inner_result.success:
                inner_result.future_resolution[TimeTypeConstants.START_DATETIME] = DateTimeFormatUtil.format_date_time(
                    inner_result.future_value[0])
                inner_result.future_resolution[TimeTypeConstants.END_DATETIME] = DateTimeFormatUtil.format_date_time(
                    inner_result.future_value[1])
                inner_result.past_resolution[TimeTypeConstants.START_DATETIME] = DateTimeFormatUtil.format_date_time(
                    inner_result.past_value[0])
                inner_result.past_resolution[TimeTypeConstants.END_DATETIME] = DateTimeFormatUtil.format_date_time(
                    inner_result.past_value[1])
                result.value = inner_result
                result.timex_str = inner_result.timex if inner_result is not None else ''
                result.resolution_str = ''

        return result

    def is_before_or_after_mod(self, mod: str) -> bool:
        if not self.config.check_both_before_after:
            return mod and (mod == TimeTypeConstants.BEFORE_MOD or mod == TimeTypeConstants.AFTER_MOD)
        else:
            # matches with inclusive_mod_prepositions are also parsed here
            return mod and (mod == TimeTypeConstants.BEFORE_MOD or mod == TimeTypeConstants.AFTER_MOD or
                            mod == TimeTypeConstants.UNTIL_MOD or mod == TimeTypeConstants.SINCE_MOD)

    def merge_date_and_time_periods(self, source: str, reference: datetime) -> DateTimeResolutionResult:
        result = DateTimeResolutionResult()
        trimmed_source = source.strip().lower()

        extracted_results = self.config.time_period_extractor.extract(trimmed_source, reference)
        if len(extracted_results) == 0:
            return self.parse_simple_cases(trimmed_source, reference)
        elif len(extracted_results) == 1:
            time_period_parse_result = self.config.time_period_parser.parse(extracted_results[0])
            time_period_resolution_result = time_period_parse_result.value

            if time_period_resolution_result is None:
                return self.parse_simple_cases(trimmed_source, reference)

            time_period_timex = time_period_resolution_result.timex

            # if it is a range type timex
            if TimexUtil.is_range_timex(time_period_timex):
                date_result = self.config.date_extractor.extract(trimmed_source.replace(extracted_results[0].text, ''), reference)

                # Check if token_before_date is null
                date_text = trimmed_source.replace(extracted_results[0].text, '').replace(self.config.token_before_date, ''). \
                    strip() if self.config.token_before_date else trimmed_source.replace(extracted_results[0].text, '').strip()
                if self.config.check_both_before_after:
                    token_list_before_date = list(self.config.token_before_date.split('|'))
                    for token in filter(lambda n: n, token_list_before_date):
                        date_text = date_text.replace(token, '').strip()

                # If only one Date is extracted and the Date text equals to the rest part of source text
                if len(date_result) == 1 and date_text == date_result[0].text:
                    parse_result = self.config.date_parser.parse(date_result[0], reference)

                    if parse_result.value:
                        future_time = parse_result.value.future_value
                        past_time = parse_result.value.past_value

                        date_timex = parse_result.timex_str
                    else:
                        return self.parse_simple_cases(trimmed_source, source)

                    range_timex_components = TimexUtil.get_range_timex_components(time_period_timex)

                    if range_timex_components.is_valid:
                        begin_timex = TimexUtil.combine_date_and_time_timex(date_timex, range_timex_components.begin_timex)
                        end_timex = TimexUtil.combine_date_and_time_timex(date_timex, range_timex_components.end_timex)
                        result.timex = TimexUtil.generate_date_time_period_timex(begin_timex, end_timex, range_timex_components.duration_timex)

                        time_period_future_value = (time_period_resolution_result.future_value.start, time_period_resolution_result.future_value.end)
                        begin_time = time_period_future_value[0]
                        end_time = time_period_future_value[1]

                        result.future_value = [
                            DateUtils.safe_create_from_min_value(future_time.year, future_time.month, future_time.day,
                                                                 begin_time.hour, begin_time.minute,
                                                                 begin_time.second),
                            DateUtils.safe_create_from_min_value(future_time.year, future_time.month, future_time.day,
                                                                 end_time.hour, end_time.minute,
                                                                 end_time.second)
                        ]

                        result.past_value = [
                            DateUtils.safe_create_from_min_value(past_time.year, past_time.month, past_time.day,
                                                                 begin_time.hour, begin_time.minute,
                                                                 begin_time.second),
                            DateUtils.safe_create_from_min_value(past_time.year, past_time.month, past_time.day,
                                                                 end_time.hour, end_time.minute,
                                                                 end_time.second)
                        ]

                        if time_period_resolution_result.comment and time_period_resolution_result.comment == Constants.COMMENT_AMPM:
                            # ampm comment is used for later set_parser_result to judge whether this parse result should
                            # have two parsing results.
                            # Cases like "from 10:30 to 11 on 1/1/2015" should have ampm comment, as it can be parsed
                            # to "10:30am to 11am" and also be parsed to "10:30pm to 11pm".
                            # Cases like "from 10:30 to 3 on 1/1/2015" should not have ampm comment.
                            if begin_time.hour < Constants.HALF_DAY_HOUR_COUNT and end_time.hour < Constants.HALF_DAY_HOUR_COUNT:
                                result.comment = Constants.COMMENT_AMPM

                        result.success = True
                        result.sub_date_time_entities = [parse_result, time_period_parse_result]

                        return result

                return self.parse_simple_cases(source, reference)

        return result

    def parse_simple_cases(self, source: str, reference: datetime) -> DateTimeResolutionResult:
        result = DateTimeResolutionResult()
        match = regex.search(self.config.pure_number_from_to_regex, source)

        if not match:
            match = regex.search(
                self.config.pure_number_between_and_regex, source)

        if not match or match.start() != 0:
            return result

        hour_group = RegExpUtility.get_group_list(match, Constants.HOUR_GROUP_NAME)
        begin_hour = self.config.numbers.get(hour_group[0])

        if not begin_hour:
            begin_hour = int(hour_group[0])
        end_hour = self.config.numbers.get(hour_group[1])

        if not end_hour:
            end_hour = int(hour_group[1])

        extracted_result = next(iter(self.config.date_extractor.extract(
            source.replace(match.group(), ''), reference)), None)

        if not extracted_result:
            return result

        pr = self.config.date_parser.parse(extracted_result, reference)

        if not pr:
            return result

        date_result: DateTimeResolutionResult = pr.value
        future_date: datetime = date_result.future_value
        past_date: datetime = date_result.past_value
        date_str = pr.timex_str

        has_am = False
        has_pm = False
        am_str = RegExpUtility.get_group(match, Constants.AM_GROUP_NAME)
        pm_str = RegExpUtility.get_group(match, Constants.PM_GROUP_NAME)
        desc_str = RegExpUtility.get_group(match, Constants.DESC_GROUP_NAME)

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
            result.comment = Constants.AM_PM_GROUP_NAME

        begin_str = f'{date_str}T{begin_hour:02d}'
        end_str = f'{date_str}T{end_hour:02d}'

        result.timex = f'({begin_str},{end_str},PT{end_hour - begin_hour}H)'
        result.future_value = [
            DateUtils.safe_create_from_min_value(future_date.year, future_date.month, future_date.day, begin_hour, 0,
                                                 0),
            DateUtils.safe_create_from_min_value(
                future_date.year, future_date.month, future_date.day, end_hour, 0, 0)
        ]
        result.past_value = [
            DateUtils.safe_create_from_min_value(
                past_date.year, past_date.month, past_date.day, begin_hour, 0, 0),
            DateUtils.safe_create_from_min_value(
                past_date.year, past_date.month, past_date.day, end_hour, 0, 0)
        ]

        result.success = True
        return result

    @staticmethod
    def get_two_points(begin_er: ExtractResult, end_er: ExtractResult, begin_parser: DateTimeParser,
                       end_parser: DateTimeParser, reference: datetime) -> BeginEnd:
        return BeginEnd(begin=begin_parser.parse(begin_er, reference), end=end_parser.parse(end_er, reference))

    def merge_two_time_points(self, source: str, reference: datetime) -> DateTimeResolutionResult:
        result = DateTimeResolutionResult()

        parse_result1 = parse_result2 = None
        both_have_dates = False
        begin_has_date = False
        end_has_date = False

        time_ers = self.config.time_extractor.extract(source, reference)
        datetime_ers = self.config.date_time_extractor.extract(source, reference)

        if len(datetime_ers) == 2:
            parse_result1 = self.config.date_time_parser.parse(datetime_ers[0], reference)
            parse_result2 = self.config.date_time_parser.parse(datetime_ers[1], reference)
            both_have_dates = True
        elif len(datetime_ers) == 1 and len(time_ers) == 2:
            if not datetime_ers[0].overlap(time_ers[0]):
                parse_result1 = self.config.time_parser.parse(time_ers[0], reference)
                parse_result2 = self.config.date_time_parser.parse(datetime_ers[0], reference)
                end_has_date = True
            else:
                parse_result1 = self.config.date_time_parser.parse(datetime_ers[0], reference)
                parse_result2 = self.config.time_parser.parse(time_ers[1], reference)
                begin_has_date = True
        elif len(datetime_ers) == 1 and len(time_ers) == 1:
            if time_ers[0].start < datetime_ers[0].start:
                parse_result1 = self.config.time_parser.parse(time_ers[0], reference)
                parse_result2 = self.config.date_time_parser.parse(datetime_ers[0], reference)
                end_has_date = True
            elif time_ers[0].start >= datetime_ers[0].start + datetime_ers[0].length:
                parse_result1 = self.config.date_time_parser.parse(datetime_ers[0], reference)
                parse_result2 = self.config.time_parser.parse(time_ers[0], reference)
                begin_has_date = True
            else:
                # If the only TimeExtractResult is part of DateTimeExtractResult, then it should not be handled
                # in this method
                return result
        elif len(time_ers) == 2:
            # If both ends are Time. Then this is a TimePeriod, not a DateTimePeriod
            return result
        else:
            return result

        if not parse_result1.value or not parse_result2:
            return result

        future_begin = parse_result1.value.future_value
        future_end = parse_result2.value.future_value

        past_begin = parse_result1.value.past_value
        past_end = parse_result2.value.past_value

        if both_have_dates:
            if future_begin > future_end:
                future_begin = past_begin
            if past_end < past_begin:
                past_end = future_end

            result.timex = f'({parse_result1.timex_str},{parse_result2.timex_str},PT{DateUtils.total_hours(future_begin, future_end)}H)'

            # Do nothing
        elif begin_has_date:
            future_end = DateUtils.safe_create_from_min_value(future_begin.year, future_begin.month, future_begin.day,
                                                              future_end.hour, future_end.minute, future_end.second)
            past_end = DateUtils.safe_create_from_min_value(past_begin.year, past_begin.month, past_begin.day,
                                                            past_end.hour, past_end.minute, past_end.second)

            date_str = parse_result1.timex_str.split('T')[0]
            duration_str = DateTimeFormatUtil.luis_time_span(future_begin, future_end)
            result.timex = f'({parse_result1.timex_str},{date_str + parse_result2.timex_str},{duration_str})'
        elif end_has_date:
            future_begin = DateUtils.safe_create_from_min_value(future_end.year, future_end.month, future_end.day,
                                                                future_begin.hour, future_begin.minute, future_begin.second)
            past_begin = DateUtils.safe_create_from_min_value(past_end.year, past_end.month, past_end.day,
                                                              past_begin.hour, past_begin.minute, past_begin.second)

            date_str = parse_result2.timex_str.split('T')[0]
            duration_str = DateTimeFormatUtil.luis_time_span(past_begin, past_end)
            result.timex = f'({date_str + parse_result1.timex_str},{parse_result2.timex_str},{duration_str})'

        am_pm_str_1 = parse_result1.value.comment
        am_pm_str_2 = parse_result2.value.comment
        if am_pm_str_1 and am_pm_str_1.endswith(Constants.COMMENT_AMPM) and \
           am_pm_str_2 and am_pm_str_2.endswith(Constants.COMMENT_AMPM):
            result.comment = Constants.COMMENT_AMPM

        result.future_value = (future_begin, future_end)
        result.past_value = (past_begin, past_end)
        result.success = True
        result.sub_date_time_entities = [parse_result1, parse_result2]

        return result

    @staticmethod
    def get_datetime(date: datetime, time: datetime) -> datetime:
        return DateUtils.safe_create_from_min_value(date.year, date.month, date.day, time.hour, time.minute,
                                                    time.second)

    # Parse specific time of day like 'this nigth', 'early morning', 'last evening'
    def parse_specific_time_of_day(self, source: str, reference: datetime) -> DateTimeResolutionResult:
        result = DateTimeResolutionResult()
        trimmed_source = source.strip()
        time_text = trimmed_source

        match = regex.search(self.config.period_time_of_day_with_date_regex, trimmed_source)
        # Extract early/late prefix from text if any
        has_early = False
        has_late = False
        if match:
            time_text = RegExpUtility.get_group(match, Constants.TIME_OF_DAY_GROUP_NAME)
            if RegExpUtility.get_group(match, Constants.COMMENT_EARLY):
                has_early = True
                result.comment = Constants.COMMENT_EARLY
                result.mod = TimeTypeConstants.EARLY_MOD
            if RegExpUtility.get_group(match, Constants.COMMENT_LATE):
                has_late = True
                result.comment = Constants.COMMENT_LATE
                result.mod = TimeTypeConstants.LATE_MOD
        else:
            match = self.config.am_desc_regex.match(trimmed_source)
            if not match:
                match = self.config.pm_desc_regex.match(trimmed_source)
            else:
                time_text = match.group()

        # handle time of day

        # Late/early only works with time of day
        # Only standard time of day (morning, afternoon, evening and night) will not directly return
        values = self.config.get_matched_time_range(time_text)
        if not values.success:
            return result

        # Modify time period if 'early' or 'late' exists
        # Since 'time of day' is defined as four hour periods
        # the first 2 hours represent early, the later 2 hours represent late
        if has_early:
            values.end_hour = values.begin_hour + 2
            if values.end_min == 59:
                values.end_min = 0
        elif has_late:
            values.begin_hour = values.begin_hour + 2

        if RegExpUtility.is_exact_match(self.config.specific_time_of_day_regex, trimmed_source, True):
            swift = self.config.get_swift_prefix(trimmed_source)
            date = (reference + timedelta(days=swift)).date()
            day = date.day
            month = date.month
            year = date.year

            result.timex = DateTimeFormatUtil.format_date(date) + values.time_str

            result.future_value = result.past_value = (DateUtils.safe_create_from_min_value(year, month, day,
                                                                                            values.begin_hour, 0, 0),
                                                       DateUtils.safe_create_from_min_value(year, month, day,
                                                                                            values.end_hour, values.end_min, values.end_min))

            result.success = True
            return result

        # Handle Date followed by morning, afternoon and morning, afternoon followed by Date
        match = self.config.period_time_of_day_with_date_regex.search(trimmed_source)

        if not match:
            match = self.config.am_desc_regex.sarch(trimmed_source)
            if not match:
                match = self.config.pm_desc_regex.search(trimmed_source)
        else:
            before_str = trimmed_source[0:match.start()].strip()
            _before_str = before_str
            trimmed_before_str = ''

            after_str = trimmed_source[match.end():].strip()
            _after_str = after_str
            trimmed_after_str = ''

            # Eliminate time period, if any
            time_period_extract_results = self.config.time_period_extractor.extract(before_str)
            if len(time_period_extract_results) > 0:
                start = time_period_extract_results[0].start
                length = time_period_extract_results[0].length
                for i in range(start, length):
                    trimmed_before_str = _before_str.replace(_before_str[start], '', 1)
                    _before_str = trimmed_before_str
                trimmed_before_str = trimmed_before_str.strip()
            else:
                time_period_extract_results = self.config.time_period_extractor.extract(after_str)
                if len(time_period_extract_results) > 0:
                    start = time_period_extract_results[0].start
                    length = time_period_extract_results[0].length
                    for i in range(start, length):
                        trimmed_after_str = _after_str.replace(_after_str[start], '', 1)
                        _before_str = trimmed_after_str
                    trimmed_after_str = trimmed_after_str.strip()

            extracted_results = self.config.date_extractor.extract((trimmed_before_str if trimmed_before_str is not '' else before_str) + ' ' + (trimmed_after_str if trimmed_after_str is not '' else after_str), reference)
            if len(extracted_results) == 0 or extracted_results[0].length < len(trimmed_before_str):
                valid = False
                if len(extracted_results) > 0 and extracted_results[0].start == 0:
                    mid_str = before_str[extracted_results[0].start + extracted_results[0].length:]
                    if mid_str.replace(',', ' '):
                        valid = True

                if not valid:
                    extracted_results = self.config.date_extractor.extract(after_str, reference)
                    if len(extracted_results) == 0 or extracted_results[0].length != len(after_str):
                        if len(extracted_results) > 0 and extracted_results[0].start + extracted_results[0].length ==\
                                len(after_str):
                            mid_str = after_str[0:extracted_results[0].start]
                            if not mid_str.replace(',', ' '):
                                valid = True
                    else:
                        valid = True
                if not valid:
                    return result

            has_specific_time_period = False
            if len(time_period_extract_results) > 0:
                time_parse_result = self.config.time_period_parser.parse(time_period_extract_results[0], reference)
                if time_parse_result:
                    period_future = (time_parse_result.value.future_value.start, time_parse_result.value.future_value.end)
                    period_past = (time_parse_result.value.past_value.start, time_parse_result.value.past_value.end)

                    if period_future == period_past:
                        values.begin_hour = period_future[0].hour
                        values.end_hour = period_future[1].hour
                    else:
                        if period_future[0].hour >= values.begin_hour or period_future[1].hour <= values.end_hour:
                            values.begin_hour = period_future[0].hour
                            values.end_hour = period_future[1].hour
                        else:
                            values.begin_hour = period_past[0].hour
                            values.end_hour = period_past[1].hour

                    has_specific_time_period = True

            parse_result = self.config.date_parser.parse(extracted_results[0], reference)
            future_date = parse_result.value.future_value
            past_date = parse_result.value.past_value

            if not has_specific_time_period:
                result.timex = parse_result.timex_str + values.time_str
            else:
                format_str = '({}T{},{}T{},PT{}H)'
                result.timex = format_str.format(parse_result.timex_str, values.begin_hour, parse_result.timex_str, values.end_hour,
                                                 values.end_hour - values.begin_hour)

            result.future_value = (DateUtils.safe_create_from_min_value(future_date.year, future_date.month,
                                                                        future_date.day, values.begin_hour, 0, 0),
                                   DateUtils.safe_create_from_min_value(future_date.year, future_date.month,
                                                                        future_date.day, values.end_hour, values.end_min, values.end_min))
            result.past_value = (DateUtils.safe_create_from_min_value(past_date.year, past_date.month,
                                                                      past_date.day, values.begin_hour, 0, 0),
                                 DateUtils.safe_create_from_min_value(past_date.year, past_date.month,
                                                                      past_date.day, values.end_hour, values.end_min, values.end_min))

            result.success = True
            return result

        return result

    def parse_duration(self, source: str, reference: datetime) -> DateTimeResolutionResult:
        result = DateTimeResolutionResult()

        # for rest of datetime, it will be handled in next function
        if regex.search(self.config.rest_of_date_time_regex, source):
            return result

        extracted_result = self.config.duration_extractor.extract(source, reference)
        if len(extracted_result) == 1:
            parse_result = self.config.duration_parser.parse(extracted_result[0])

            before_str = source[0:parse_result.start].strip()
            after_str = source[parse_result.start + parse_result.length:].strip()

            numbers_in_suffix = self.config.cardinal_extractor.extract(before_str)
            numbers_in_duration = self.config.cardinal_extractor.extract(extracted_result[0].text)

            # Handle cases like "2 upcoming days", "5 previous years"
            if any(numbers_in_suffix) and not any(numbers_in_duration):
                number_extracted_result = next(numbers_in_suffix)
                number_text = number_extracted_result.text
                duration_text = extracted_result[0].text
                combined_text = f'{number_text} {duration_text}'
                combined_duration_extracted_result = self.config.duration_extractor.extract(combined_text, reference)

                if any(combined_duration_extracted_result):
                    parse_result = self.config.duration_parser.parse(next(combined_duration_extracted_result))
                    start_index = number_extracted_result.start + number_extracted_result.length
                    before_str = before_str[start_index:].strip()

            if parse_result.value:
                swift_seconds = 0
                mod = ''
                duration_result = parse_result.value

                if duration_result.past_value and duration_result.future_value:
                    swift_seconds = int(float(duration_result.future_value))

                end_time = begin_time = reference

                if RegExpUtility.is_exact_match(self.config.previous_prefix_regex, before_str, True):
                    mod = TimeTypeConstants.BEFORE_MOD
                    begin_time = reference + timedelta(seconds=-swift_seconds)

                # Handle the "within (the) (next) xx seconds/minutes/hours" case
                # Should also habdle the multiple duration case like P1DT8H
                # Set the begin_time equal to reference time for now
                if RegExpUtility.is_exact_match(self.config.within_next_prefix_regex, before_str, True):
                    end_time = begin_time + timedelta(seconds=swift_seconds)
                if self.config.check_both_before_after and RegExpUtility.is_exact_match(self.config.within_next_prefix_regex, after_str, True):
                    end_time = begin_time + timedelta(seconds=swift_seconds)
                if RegExpUtility.is_exact_match(self.config.future_regex, before_str, True):
                    mod = TimeTypeConstants.AFTER_MOD
                    end_time = begin_time + timedelta(seconds=swift_seconds)
                if RegExpUtility.is_exact_match(self.config.previous_prefix_regex, after_str, True):
                    mod = TimeTypeConstants.BEFORE_MOD
                    begin_time = reference + timedelta(seconds=-swift_seconds)
                if RegExpUtility.is_exact_match(self.config.future_regex, after_str, True):
                    mod = TimeTypeConstants.AFTER_MOD
                    end_time = begin_time + timedelta(seconds=swift_seconds)
                if RegExpUtility.is_exact_match(self.config.future_suffix_regex, after_str, True):
                    mod = TimeTypeConstants.AFTER_MOD
                    end_time = begin_time + timedelta(seconds=swift_seconds)

                result.timex = f'({DateTimeFormatUtil.luis_date_from_datetime(begin_time)}T' \
                               f'{DateTimeFormatUtil.luis_time_from_datetime(begin_time)},' + \
                               f'{DateTimeFormatUtil.luis_date_from_datetime(end_time)}T' \
                               f'{DateTimeFormatUtil.luis_time_from_datetime(end_time)},' + \
                               f'{duration_result.timex})'

                result.future_value = result.past_value = (begin_time, end_time)
                result.success = True

                if mod:
                    parse_result.value.mod = mod

                result.sub_date_time_entities = [parse_result]

                return result

        return result

    def get_valid_connector_mod_for_date_and_time_period(self, source: str, in_prefix: bool):
        mod = None

        # Item1 is the regexp to be tested
        # Item2 is the mod corresponding to an inclusive match (i.e. containing an inclusive_mod_prepositions,
        # e.g. "at or before3")
        # Item3 is the mod corresponding to a non-inclusive match (e.g "before 3")
        before_after_regex_tuple = [(self.config.before_regex, TimeTypeConstants.UNTIL_MOD,
                                     TimeTypeConstants.BEFORE_MOD),
                                    (self.config.after_regex, TimeTypeConstants.SINCE_MOD,
                                     TimeTypeConstants.AFTER_MOD)]

        for regexp in before_after_regex_tuple:
            match = regexp[0].match(source) if in_prefix else RegExpUtility.match_begin(regexp[0], source, True)
            if match and match.success:
                mod = regexp[2] if in_prefix else (regexp[1] if RegExpUtility.get_group(match, 'include') else
                                                   regexp[2])
                return mod

        return mod

    def parse_relative_unit(self, source: str, reference: datetime) -> DateTimeResolutionResult:
        result = DateTimeResolutionResult()
        match = regex.search(self.config.relative_time_unit_regex, source)

        if not match:
            match = regex.search(self.config.rest_of_date_time_regex, source)

        if not match:
            return result

        src_unit = RegExpUtility.get_group(match, Constants.UNIT)
        unit_str = self.config.unit_map.get(src_unit, None)

        if not unit_str:
            return result

        swift = 1
        prefix_match = regex.search(self.config.past_regex, source)
        if prefix_match:
            swift = -1

        begin_time: datetime = reference
        end_time: datetime = reference

        if unit_str == 'D':
            end_time = DateUtils.safe_create_from_min_value(begin_time.year, begin_time.month,
                                                            begin_time.day) + timedelta(days=1, seconds=-1)
            difference = int((end_time - begin_time).total_seconds())
            pt_timex = f'PT{difference}S'
        elif unit_str == 'H':
            begin_time = begin_time + \
                timedelta(hours=0 if swift > 0 else swift)
            end_time = end_time + timedelta(hours=swift if swift > 0 else 0)
            pt_timex = 'PT1H'
        elif unit_str == 'M':
            begin_time = begin_time + \
                timedelta(minutes=0 if swift > 0 else swift)
            end_time = end_time + timedelta(minutes=swift if swift > 0 else 0)
            pt_timex = 'PT1M'
        elif unit_str == 'S':
            begin_time = begin_time + \
                timedelta(seconds=0 if swift > 0 else swift)
            end_time = end_time + timedelta(seconds=swift if swift > 0 else 0)
            pt_timex = 'PT1S'
        else:
            return result

        luis_date_begin = DateTimeFormatUtil.luis_date_from_datetime(
            begin_time)
        luis_time_begin = DateTimeFormatUtil.luis_time_from_datetime(
            begin_time)
        luis_date_end = DateTimeFormatUtil.luis_date_from_datetime(end_time)
        luis_time_end = DateTimeFormatUtil.luis_time_from_datetime(end_time)

        result.timex = f'({luis_date_begin}T{luis_time_begin},{luis_date_end}T{luis_time_end},{pt_timex})'
        result.future_value = [begin_time, end_time]
        result.past_value = [begin_time, end_time]
        result.success = True

        return result

    def parse_date_with_period_prefix(self, source: str, reference: datetime) -> DateTimeResolutionResult:
        result = DateTimeResolutionResult()

        date_result = self.config.date_extractor.extract(source)
        if len(date_result) > 0:
            before_str = source[0: date_result[-1].start]
            match = self.config.prefix_day_regex.match(before_str)

            # Check also after_str
            if not match and self.config.check_both_before_after:
                after_str = source[date_result[-1].start + date_result[-1].length: len(source) - date_result[-1].start + date_result[-1].length]
                match = self.config.prefix_day_regex.match(after_str)

            if match:
                parse_result = self.config.date_parser.parse(date_result[-1], reference)
                if parse_result.value:
                    start_time = parse_result.value.future_value
                    start_time = datetime(start_time.year, start_time.month, start_time.day)
                    end_time = start_time

                    if RegExpUtility.get_group(match, Constants.EARLY_PREFIX):
                        end_time = start_time + timedelta(hours=Constants.HALF_DAY_HOUR_COUNT)
                        result.mod = TimeTypeConstants.EARLY_MOD
                    elif RegExpUtility.get_group(match, Constants.MID_PREFIX):
                        start_time = start_time + timedelta(hours=Constants.HALF_DAY_HOUR_COUNT - Constants.HALF_MID_DAY_DURATION_HOUR_COUNT)
                        end_time = end_time + timedelta(hours=Constants.HALF_DAY_HOUR_COUNT + Constants.HALF_MID_DAY_DURATION_HOUR_COUNT)
                        result.mod = TimeTypeConstants.MID_MOD
                    elif RegExpUtility.get_group(match, Constants.LATE_PREFIX):
                        start_time = start_time + timedelta(hours=Constants.HALF_DAY_HOUR_COUNT)
                        end_time = end_time + timedelta(hours=Constants.HALF_DAY_HOUR_COUNT)
                        result.mod = TimeTypeConstants.LATE_MOD
                    else:
                        return result

                    result.timex = parse_result.timex_str
                    result.past_value = result.future_value = (start_time, end_time)
                    result.success = True

        return result

    def parse_date_with_time_period_suffix(self, source: str, reference: datetime) -> DateTimeResolutionResult:
        result = DateTimeResolutionResult()

        date_extract_result = next(iter(self.config.date_extractor.extract(source)), None)
        time_extract_result = next(iter(self.config.time_extractor.extract(source)), None)

        if date_extract_result and time_extract_result:
            date_str_end = int(date_extract_result.start + date_extract_result.length)
            time_str_end = int(time_extract_result.start + time_extract_result.length)

            if date_str_end < time_extract_result.start:
                mid_str = source[date_str_end:time_extract_result.start - date_str_end].strip()
                after_str = source[time_str_end:]

                mod_str = self.get_valid_connector_mod_for_date_and_time_period(mid_str, True)

                # Check also after_str
                if mod_str and self.config.check_both_before_after:
                    mod_str = self.get_valid_connector_mod_for_date_and_time_period(after_str, False) if\
                        len(mid_str) <= 4 else None

                if mod_str:
                    date_parse_result = self.config.date_parser.parse(date_extract_result, reference)
                    time_parse_result = self.config.time_parser.parse(time_extract_result, reference)

                    if date_parse_result and time_parse_result:
                        time_resolution_result = time_parse_result.value
                        date_resolution_result = date_parse_result.value
                        future_date_value = date_resolution_result.future_value
                        past_date_value = date_resolution_result.past_value
                        future_time_value = time_resolution_result.future_value
                        past_time_value = time_resolution_result.past_value

                        result.comment = time_resolution_result.comment
                        result.timex = f'{date_parse_result.timex_str}{time_parse_result.timex_str}'

                        result.future_value = DateUtils.safe_create_from_min_value(future_date_value.year,
                                                                                   future_date_value.month,
                                                                                   future_date_value.day,
                                                                                   future_time_value.hour,
                                                                                   future_time_value.minute,
                                                                                   future_time_value.second)
                        result.past_value = DateUtils.safe_create_from_min_value(past_date_value.year,
                                                                                 past_date_value.month,
                                                                                 past_date_value.day,
                                                                                 past_time_value.hour,
                                                                                 past_time_value.minute,
                                                                                 past_time_value.second)

                        result.mod = mod_str
                        result.sub_date_time_entities = [date_parse_result, time_parse_result]
                        result.success = True

        return result
