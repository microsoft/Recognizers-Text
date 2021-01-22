import math
from abc import ABC, abstractmethod
from typing import List, Optional, Pattern, Match, Dict
from datetime import datetime, timedelta
from collections import namedtuple

import regex
from datedelta import datedelta

from recognizers_text.extractor import ExtractResult, Extractor, Metadata
from recognizers_date_time.date_time.date_extractor import DateExtractor
from recognizers_number.number import BaseNumberParser, BaseNumberExtractor
from .constants import Constants, TimeTypeConstants
from .extractors import DateTimeExtractor
from .parsers import DateTimeParser, DateTimeParseResult
from .base_date import BaseDateParser
from .base_duration import BaseDurationParser
from .utilities import Token, merge_all_tokens, DateTimeFormatUtil, DateTimeResolutionResult, DateUtils, DayOfWeek, \
    RegExpUtility, DateContext, TimexUtil

MatchedIndex = namedtuple('MatchedIndex', ['matched', 'index'])


class DatePeriodExtractorConfiguration(ABC):

    @property
    @abstractmethod
    def previous_prefix_regex(self) -> Pattern:
        raise NotImplementedError

    @property
    @abstractmethod
    def simple_cases_regexes(self) -> List[Pattern]:
        raise NotImplementedError

    @property
    @abstractmethod
    def check_both_before_after(self) -> Pattern:
        raise NotImplementedError

    @property
    @abstractmethod
    def illegal_year_regex(self) -> Pattern:
        raise NotImplementedError

    @property
    @abstractmethod
    def year_regex(self) -> Pattern:
        raise NotImplementedError

    @property
    @abstractmethod
    def till_regex(self) -> Pattern:
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
    def past_regex(self) -> Pattern:
        raise NotImplementedError

    @property
    @abstractmethod
    def future_regex(self) -> Pattern:
        raise NotImplementedError

    @property
    @abstractmethod
    def week_of_regex(self) -> Pattern:
        raise NotImplementedError

    @property
    @abstractmethod
    def month_of_regex(self) -> Pattern:
        raise NotImplementedError

    @property
    @abstractmethod
    def date_unit_regex(self) -> Pattern:
        raise NotImplementedError

    @property
    @abstractmethod
    def time_unit_regex(self) -> Pattern:
        raise NotImplementedError

    @property
    @abstractmethod
    def in_connector_regex(self) -> Pattern:
        raise NotImplementedError

    @property
    @abstractmethod
    def range_unit_regex(self) -> Pattern:
        raise NotImplementedError

    @property
    @abstractmethod
    def now_regex(self) -> Pattern:
        raise NotImplementedError

    @property
    @abstractmethod
    def date_point_extractor(self) -> DateExtractor:
        raise NotImplementedError

    @property
    @abstractmethod
    def integer_extractor(self) -> BaseNumberExtractor:
        raise NotImplementedError

    @property
    @abstractmethod
    def ordinal_extractor(self) -> Extractor:
        raise NotImplementedError

    @property
    @abstractmethod
    def cardinal_extractor(self) -> Extractor:
        raise NotImplementedError

    @property
    @abstractmethod
    def number_parser(self) -> BaseNumberParser:
        raise NotImplementedError

    @property
    @abstractmethod
    def duration_extractor(self) -> DateTimeExtractor:
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
    def ago_regex(self) -> Pattern:
        raise NotImplementedError

    @property
    @abstractmethod
    def later_regex(self) -> Pattern:
        raise NotImplementedError

    @property
    @abstractmethod
    def less_than_regex(self) -> Pattern:
        raise NotImplementedError

    @property
    @abstractmethod
    def more_than_regex(self) -> Pattern:
        raise NotImplementedError

    @property
    @abstractmethod
    def duration_date_restrictions(self) -> [str]:
        raise NotImplementedError

    @property
    @abstractmethod
    def year_period_regex(self) -> Pattern:
        raise NotImplementedError

    @property
    @abstractmethod
    def century_suffix_regex(self) -> Pattern:
        raise NotImplementedError

    @property
    @abstractmethod
    def month_num_regex(self) -> Pattern:
        raise NotImplementedError

    @property
    @abstractmethod
    def check_both_before_after(self) -> bool:
        raise NotImplementedError

    @property
    @abstractmethod
    def previous_prefix_regex(self) -> Pattern:
        raise NotImplementedError


class BaseDatePeriodExtractor(DateTimeExtractor):
    @property
    def extractor_type_name(self) -> str:
        return Constants.SYS_DATETIME_DATEPERIOD

    def __init__(self, config: DatePeriodExtractorConfiguration):
        self.config = config

    def extract(self, source: str, reference: datetime = None) -> List[ExtractResult]:
        if not reference:
            reference = datetime.now()
        tokens = []
        tokens += self.match_simple_cases(source)

        simple_cases_results = merge_all_tokens(tokens, source, self.extractor_type_name)
        ordinal_extractions = self.config.ordinal_extractor.extract(source)

        tokens += self.merge_two_time_points(source, reference)
        tokens += self.match_duration(source, reference)
        tokens += self.single_time_point_with_patterns(source, ordinal_extractions, reference)
        tokens += self.match_complex_cases(source, simple_cases_results, reference)
        tokens += self.match_year_period(source)
        tokens += self.match_ordinal_number_with_century_suffix(source, ordinal_extractions)

        result = merge_all_tokens(tokens, source, self.extractor_type_name)

        return result

    # Cases like "21st century"
    def match_ordinal_number_with_century_suffix(self, text: str, ordinal_extractions: [ExtractResult]):
        result = []

        for er in ordinal_extractions:
            if er.start + er.length >= len(text):
                continue

            after_string = text[er.start + er.length:]
            trimmed_after_string = after_string.rstrip()
            white_spaces_count = len(after_string) - len(trimmed_after_string)
            after_string_offset = (er.start + er.length) + white_spaces_count

            match = regex.match(self.config.century_suffix_regex, trimmed_after_string)

            if match:
                result.append(Token(er.start, after_string_offset + text.index(match.group()) +
                                    (match.end() - match.start())))

        return result

    def match_year_period(self, text: str):
        result = []

        metadata = Metadata()
        metadata.possibly_included_period_end = True

        matches = list(regex.finditer(self.config.year_period_regex, text))

        for match in matches:
            match_year = regex.search(self.config.year_regex, match.group())

            # Single year cases like "1998"
            if match_year is not None and (match_year.end() - match_year.start()) == len(match.group()):
                year = self.config.date_point_extractor.get_year_from_text(match_year)
                if not (Constants.MIN_YEAR_NUM <= year <= Constants.MAX_YEAR_NUM):
                    continue

                # Possibly include period end only apply for cases like "2014-2018", which are not single year cases
                metadata.possibly_included_period_end = False
            else:
                year_matches = list(regex.finditer(self.config.year_regex, match.group()))
                all_digit_year = True
                is_valid_year = True

                for year_match in year_matches:
                    year = self.config.date_point_extractor.get_year_from_text(year_match)
                    if not (Constants.MIN_YEAR_NUM <= year <= Constants.MAX_YEAR_NUM):
                        is_valid_year = False
                        continue
                    elif len(year_match) != Constants.FOUR_DIGITS_YEAR_LENGTH:
                        all_digit_year = False

                if not is_valid_year:
                    continue

                # Cases like "2010-2015"
                if all_digit_year:

                    # Filter out cases like "82-2010-2015" or "2010-2015-82"
                    # where "2010-2015" should not be extracted as a DateRange
                    if self.has_invalid_dash_context(match, text):
                        continue

            result.append(Token(match.start(), match.start() - (match.end() - match.start()), metadata))

        return result

    # This method is to detect the invalid dash context
    # Some match with invalid dash context might be false positives
    # For example, it can be part of the pone number like "Tel: 138-2010-2015"
    def has_invalid_dash_context(self, match: Match, text: str):

        has_invalid_dash_context = False

        has_dash_prefix, dash_prefix_index = self.has_dash_prefix(match, text)

        # Filter out cases like "82-2100" where "2100" should not be extracted as a DateRange
        # Filter out cases like "82-2010-2015" where "2010-2015" should not be extracted as a DateRange
        if has_dash_prefix:
            has_digit_number_before_dash, number_start_index = self.has_digit_number_before_dash(text,
                                                                                                 dash_prefix_index)

            if has_digit_number_before_dash:
                digit_number_str = text[number_start_index: number_start_index + (text.index(match.group()) -
                                                                                  1 - number_start_index)]

                if RegExpUtility.is_exact_match(self.config.month_num_regex, digit_number_str, True):
                    has_invalid_dash_context = True

        has_dash_suffix, dash_suffix_index = self.has_dash_suffix(match, text)

        # Filter out cases like "2100-82" where "2100" should not be extracted as a DateRange
        # Filter out cases like "2010-2015-82" where "2010-2015" should not be extracted as a DateRange
        if has_dash_suffix:

            has_digit_number_after_dash, number_end_index = self.has_digit_number_after_dash(text, dash_suffix_index)

            number_start_index = text.index(match.group()) + (match.end() - match.start()) + 1
            digit_number_str = text[number_start_index: number_start_index + (number_end_index - number_start_index)]

            if RegExpUtility.is_exact_match(self.config.month_num_regex, digit_number_str, True):
                has_invalid_dash_context = True

        return has_invalid_dash_context

    def has_digit_number_after_dash(self, source: str, dash_suffix_index: int):

        has_digit_number_after_dash = False
        number_end_index = -1

        i = dash_suffix_index + 1

        for i in range(i, len(source) + 1, 1):

            if source[i] == ' ':
                continue
            if self.is_digit_char(source[i]):
                has_digit_number_after_dash = True

            if self.is_digit_char(source[i]):
                if has_digit_number_after_dash:
                    number_end_index = i + 1
                break

        if has_digit_number_after_dash and number_end_index == -1:
            number_end_index = 0

        return has_digit_number_after_dash, number_end_index

    @staticmethod
    def has_dash_suffix(match: Match, source: str):

        has_dash_suffix = False

        dash_suffix_index = -1

        i = source.index(match.group()) + (match.end() - match.start())
        for i in range(i, len(source) + 1, 1):
            if source[i] != ' ' and source[i] != '-':
                break
            elif source[i] == '-':
                has_dash_suffix = True
                dash_suffix_index = i
                break
        return has_dash_suffix, dash_suffix_index

    @staticmethod
    def has_dash_prefix(match: Match, source: str):

        has_dash_prefix = False

        dash_prefix_index = -1

        i = source.index(match.group()) - 1
        for i in range(i, 0, -1):
            if source[i] != ' ' and source[i] != '-':
                break

            elif source[i] == '-':
                has_dash_prefix = True
                dash_prefix_index = i
                break
        return has_dash_prefix, dash_prefix_index

    def has_digit_number_before_dash(self, source: str, dash_prefix_index: int):

        has_digit_number_before_dash = False
        number_start_index = -1

        i = dash_prefix_index - 1

        for i in range(i, 0, -1):

            if source[i] == ' ':
                continue
            if self.is_digit_char(source[i]):
                has_digit_number_before_dash = True

            if self.is_digit_char(source[i]):
                if has_digit_number_before_dash:
                    number_start_index = i + 1
                break

        if has_digit_number_before_dash and number_start_index == -1:
            number_start_index = 0

        return has_digit_number_before_dash, number_start_index

    @staticmethod
    def is_digit_char(ch: chr):
        return '0' <= ch <= '9'

    # Complex cases refer to the combination of daterange and datepoint
    # For Example: from|between {DateRange|DatePoint} to|till|and {DateRange|DatePoint}
    def match_complex_cases(self, text: str, simple_date_range_results: [ExtractResult], reference: datetime):

        er = self.config.date_point_extractor.extract(text, reference)

        # Filter out DateRange results that are part of DatePoint results
        # For example, "Feb 1st 2018" => "Feb" and "2018" should be filtered out here
        er.extend(list(
            filter(
                lambda simple_date_range: not any(
                    list(
                        filter(
                            lambda date_point: date_point.start <= simple_date_range.start and date_point.start +
                            date_point.length >= simple_date_range.start + simple_date_range.length, er))),
                simple_date_range_results))
        )

        er = list(sorted(er, key=lambda x: x.start))

        return self.merge_multiple_extractions(text, er)

    def match_simple_cases(self, source: str) -> List[ExtractResult]:
        tokens = []

        for regexp in self.config.simple_cases_regexes:
            matches = list(regex.finditer(regexp, source))

            for match in matches:
                match_year = self.config.year_regex.search(match.group())

                if match_year and len(match_year.group()) == len(match.group()):
                    year_str = self.config.date_point_extractor.get_year_from_text(match_year)

                    if not (Constants.MIN_YEAR_NUM <= year_str <= Constants.MAX_YEAR_NUM):
                        continue

                if (match.end() - match.start() == Constants.FOUR_DIGITS_YEAR_LENGTH) and self.__infix_boundary_check(
                        match, source):
                    sub_str = source[match.start() - 1: match.end() + 1]

                    # Handle single year which is surrounded by '-' at both sides, e.g., a single year falls in a GUID
                    if self.config.illegal_year_regex.match(sub_str):
                        continue

                tokens.append(Token(match.start(), match.end()))

        return tokens

    def __get_year_from_text(self, match) -> int:
        first_two_year_num_str = match.group(Constants.FIRST_TWO_YEAR_NUM)

        if first_two_year_num_str:
            er = ExtractResult()
            er.text = first_two_year_num_str
            er.start = match.start(Constants.FIRST_TWO_YEAR_NUM)
            er.length = match.end(Constants.FIRST_TWO_YEAR_NUM) - er.start
            first_two_year_num = self.config.number_parser.parse(er).value

            last_two_year_num = 0
            last_two_year_num_str = match.group(Constants.LAST_TWO_YEAR_NUM)

            if last_two_year_num_str:
                er.text = last_two_year_num_str
                er.start = match.start(Constants.LAST_TWO_YEAR_NUM)
                er.length = match.end(Constants.LAST_TWO_YEAR_NUM) - er.start
                last_two_year_num = self.config.number_parser.parse(er).value

            if first_two_year_num < 100 and last_two_year_num == 0 or first_two_year_num < 100 and\
                    first_two_year_num % 10 == 0 and len(
                    last_two_year_num_str.strip().split(' ')) == 1:
                return -1

            if first_two_year_num >= 100:
                return first_two_year_num + last_two_year_num

            return first_two_year_num * 100 + last_two_year_num
        else:
            return -1

    def merge_two_time_points(self, source: str, reference: datetime) -> List[Token]:
        extract_result = self.config.date_point_extractor.extract(source)

        # Handle "now"
        matches = list(self.config.now_regex.finditer(source))
        if len(matches) != 0:
            for match in matches:
                now_extract_result = ExtractResult()
                now_extract_result.start = match.start()
                now_extract_result.length = match.end() - match.start()
                extract_result.append(now_extract_result)

            extract_result = sorted(extract_result, key=lambda x: x.start)

        return self.merge_multiple_extractions(source, extract_result)

    def merge_multiple_extractions(self, source: str, extract_result: [ExtractResult]) -> List[Token]:
        tokens = []

        metadata = Metadata()
        metadata.possibly_included_period_end = True

        if len(extract_result) <= 1:
            return tokens

        idx = 0

        while idx < len(extract_result) - 1:
            middle_begin = extract_result[idx].start + (extract_result[idx].length or 0)
            middle_end = extract_result[idx + 1].start or 0

            if middle_begin >= middle_end:
                idx += 1
                continue

            middle_str = source[middle_begin:middle_end].strip().lower()

            if RegExpUtility.is_exact_match(self.config.till_regex, middle_str, True):
                period_begin = extract_result[idx].start
                period_end = (extract_result[idx + 1].start or 0) + \
                             (extract_result[idx + 1].length or 0)

                # Handle "from/between" together with till words (till/until/through...)
                before_str = source[0:period_begin].strip().lower()
                from_token_index = self.config.get_from_token_index(before_str)
                between_token_index = self.config.get_between_token_index(
                    before_str)

                if from_token_index.matched or between_token_index.matched:
                    period_begin = from_token_index.index if from_token_index.matched else between_token_index.index
                tokens.append(Token(period_begin, period_end, metadata))

                # Merge two tokens here, increase the index by two
                idx += 2
                continue

            if self.config.has_connector_token(middle_str):
                period_begin = extract_result[idx].start or 0
                period_end = (extract_result[idx + 1].start or 0) + \
                             (extract_result[idx + 1].length or 0)

                # handle "between...and..." case
                before_str = source[0:period_begin].strip().lower()
                between_token_index = self.config.get_between_token_index(
                    before_str)

                if between_token_index.matched:
                    period_begin = between_token_index.index
                    tokens.append(Token(period_begin, period_end, metadata))

                    # Merge two tokens here, increase the index by two
                    idx += 2
                    continue

                if self.config.check_both_before_after:
                    after_str = source[period_end: len(source) - period_end]
                    between_token_index = self.config.get_between_token_index(after_str)
                    if between_token_index.matched:
                        period_end += after_str
                        tokens.append(Token(period_begin, period_end, metadata))

                        # merge two tokens here, increase the index by two
                        idx += 2
                        continue

            idx += 1

        return tokens

    def match_duration(self, source: str, reference: datetime) -> List[Token]:
        tokens = []
        durations = []
        duration_extractions = self.config.duration_extractor.extract(source, reference)

        for duration_extraction in self.config.duration_extractor.extract(source, reference):
            match = self.config.date_unit_regex.search(duration_extraction.text)
            if match:
                durations.append(
                    Token(duration_extraction.start, duration_extraction.start + duration_extraction.length))

        for duration in durations:
            before_str = source[0:duration.start].lower()
            after_str = source[duration.start:duration.start + duration.length]

            if not before_str or not after_str:
                continue

            # within "Days/Weeks/Months/Years" should be handled as dateRange here
            # if duration contains "Seconds/Minutes/Hours", it should be treated as datetimeRange
            match_token = self._match_within_next_affix_regex(source, duration, True)
            if match_token.start >= 0:
                tokens.append(match_token)
                continue

            if self.config.check_both_before_after:
                match_token = self._match_within_next_affix_regex(source, duration, False)
                if match_token.start >= 0:
                    tokens.append(match_token)
                    continue

            # Match prefix
            match = RegExpUtility.match_end(self.config.past_regex, before_str, True)
            index = -1

            if match and match.success:
                index = match.index
            if index < 0:
                # For cases like 'next five days'
                match = RegExpUtility.match_end(self.config.future_regex, before_str, True)

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

            # Match suffix
            match = RegExpUtility.match_begin(self.config.past_regex, after_str, True)

            if match and match.success:
                tokens.append(Token(duration.start, duration.end + match.index + match.length))
                continue

            match = RegExpUtility.match_begin(self.config.future_suffix_regex, after_str, True)

            if match and match.success:
                tokens.append(Token(duration.start, duration.end + match.index + match.length))
                continue

        return tokens

    def _match_within_next_affix_regex(self, source, duration, in_prefix):
        before_str = source[0: duration.start]
        after_str = source[duration.start: duration.start + duration.length]
        end_token = start_token = -1

        match = RegExpUtility.match_end(self.config.within_next_prefix_regex, before_str, True) if in_prefix else \
            RegExpUtility.match_begin(self.config.within_next_prefix_regex, after_str, True)

        if match and match.success:
            duration_str = source[duration.start: duration.length]
            match_date = self.config.date_unit_regex.match(duration_str)
            match_time = self.config.time_unit_regex.match(duration_str)

            if match_date and not match_time:
                start_token = match.index if in_prefix else duration.start
                end_token = duration.end if in_prefix else duration.end + match.index + match.length

                if not in_prefix:
                    # check prefix for next
                    match = RegExpUtility.match_end(self.config.future_regex, before_str, True)

        return Token(start_token, end_token)

    def __extract_within_next_prefix(self, substr, extract_result, in_prefix):
        result = []
        match = self.config.within_next_prefix_regex.search(substr)

        if match:
            is_next = not RegExpUtility.get_group(match, Constants.NEXT_GROUP_NAME)

            # For "within" case
            # Cases like "within the next 5 days before today" is not acceptable
            if not (is_next and self.is_ago_relative_duration_date(extract_result)):
                result.extend(self.__get_token_for_regex_matching(
                    substr,
                    self.config.within_next_prefix_regex,
                    extract_result, in_prefix)
                )
        return result
    # 1. Extract the month of date, week of date to a date range
    # 2. Extract cases like within two weeks from/before today/tomorrow/yesterday

    def single_time_point_with_patterns(self, source: str, ordinal_extractions:
                                        [ExtractResult], reference: datetime) -> List[Token]:
        result = []
        date_points = self.config.date_point_extractor.extract(source, reference)

        # For cases like "week of the 18th"
        date_points.extend(list(filter(lambda o: not any(list(filter(lambda x: x.overlap(o), date_points))),
                                       ordinal_extractions)))

        if len(date_points) < 1:
            return result

        for extraction_result in date_points:

            if extraction_result.start is not None and extraction_result.length is not None:
                before_string = source[0: extraction_result.start]
                after_string = source[extraction_result.start + extraction_result.length: len(source)
                                      - extraction_result.start - extraction_result.length]
                result.extend(self.__get_token_for_regex_matching(before_string,
                                                                  self.config.week_of_regex, extraction_result, True))
                result.extend(self.__get_token_for_regex_matching(before_string,
                                                                  self.config.month_of_regex, extraction_result, True))

                # check also after_string
                if self.config.check_both_before_after:
                    result.extend(self.__get_token_for_regex_matching(after_string,
                                                                      self.config.week_of_regex, extraction_result,
                                                                      True))
                    result.extend(self.__get_token_for_regex_matching(after_string,
                                                                      self.config.month_of_regex, extraction_result,
                                                                      True))
                # Cases like "3 days from today", "2 weeks before yesterday", "3 months after tomorrow"
                if self.is_relative_duration_date(extraction_result):
                    result.extend(self.__get_token_for_regex_matching(before_string,
                                                                      self.config.less_than_regex, extraction_result,
                                                                      False))
                    result.extend(self.__get_token_for_regex_matching(before_string,
                                                                      self.config.more_than_regex, extraction_result,
                                                                      False))
                    if self.config.check_both_before_after:
                        result.extend(self.__get_token_for_regex_matching(after_string,
                                                                          self.config.less_than_regex,
                                                                          extraction_result,
                                                                          False))
                        result.extend(self.__get_token_for_regex_matching(after_string,
                                                                          self.config.more_than_regex,
                                                                          extraction_result,
                                                                          False))
                    # For "within" case, only duration with relative to "today" or "now" makes sense
                    # Cases like "within 3 days from yesterday/tomorrow" does not make any sense
                    if self.is_date_relative_to_now_or_today(extraction_result):

                        tokens = self.__extract_within_next_prefix(before_string, extraction_result, True)
                        result.extend(tokens)

                        # check also after_string
                        if self.config.check_both_before_after and len(tokens) == 0:
                            tokens = self.__extract_within_next_prefix(after_string, extraction_result, False)
                            result.extend(tokens)
        return result

    def is_ago_relative_duration_date(self, er: ExtractResult):
        return self.config.ago_regex.search(er.text)

    # Cases like "2 days from today", "2 weeks before yesterday", "3 months after tomorrow"
    def is_relative_duration_date(self, er: ExtractResult):
        is_ago = regex.search(self.config.ago_regex, er.text)
        is_later = regex.search(self.config.later_regex, er.text)
        return is_ago or is_later

    def is_date_relative_to_now_or_today(self, er: ExtractResult):
        for flag_word in self.config.duration_date_restrictions:
            if flag_word in er.text:
                return True
        return False

    @staticmethod
    def __match_regex_in_prefix(source: str, match: Match) -> bool:
        return match and not source[match.end():].strip()

    @staticmethod
    def __get_token_for_regex_matching(source: str, regexp: Pattern, er: ExtractResult, in_prefix: bool) -> List[Token]:
        tokens = []
        match = regex.search(regexp, source)
        is_match_at_edge = False if not match else source.strip().endswith(match.group().strip()) if in_prefix else\
            source.strip().startswith(match.group().strip())

        if match and is_match_at_edge:
            start_index = source.rfind(match.group())
            end_index = er.start + er.length
            end_index += 0 if in_prefix else match.index + match.length
            tokens.append(Token(start_index, end_index))

        return tokens

    # Check whether the match is an infix of source
    @staticmethod
    def __infix_boundary_check(match: Match, source: str) -> bool:
        is_match_infix_of_source = False
        if match.start() > 0 and match.end() < len(source):
            if source[match.start():match.end()] == match.group():
                is_match_infix_of_source = True

        return is_match_infix_of_source


class DatePeriodParserConfiguration(ABC):
    @property
    @abstractmethod
    def less_than_regex(self) -> Pattern:
        raise NotImplementedError

    @property
    @abstractmethod
    def check_both_before_after(self) -> Pattern:
        raise NotImplementedError

    @property
    @abstractmethod
    def reference_date_period_regex(self) -> Pattern:
        raise NotImplementedError

    @property
    @abstractmethod
    def date_extractor(self) -> DateTimeExtractor:
        raise NotImplementedError

    @property
    @abstractmethod
    def complex_dateperiod_regex(self) -> Pattern:
        raise NotImplementedError

    @property
    @abstractmethod
    def date_parser(self) -> BaseDateParser:
        raise NotImplementedError

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
    def month_front_between_regex(self) -> Pattern:
        raise NotImplementedError

    @property
    @abstractmethod
    def between_regex(self) -> Pattern:
        raise NotImplementedError

    @property
    @abstractmethod
    def month_front_simple_cases_regex(self) -> Pattern:
        raise NotImplementedError

    @property
    @abstractmethod
    def simple_cases_regex(self) -> Pattern:
        raise NotImplementedError

    @property
    @abstractmethod
    def one_word_period_regex(self) -> Pattern:
        raise NotImplementedError

    @property
    @abstractmethod
    def month_with_year(self) -> Pattern:
        raise NotImplementedError

    @property
    @abstractmethod
    def month_num_with_year(self) -> Pattern:
        raise NotImplementedError

    @property
    @abstractmethod
    def year_regex(self) -> Pattern:
        raise NotImplementedError

    @property
    @abstractmethod
    def decade_with_century_regex(self) -> Pattern:
        raise NotImplementedError

    @property
    @abstractmethod
    def ago_regex(self) -> Pattern:
        raise NotImplementedError

    @property
    @abstractmethod
    def later_regex(self) -> Pattern:
        raise NotImplementedError

    @property
    @abstractmethod
    def relative_regex(self) -> Pattern:
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
    def in_connector_regex(self) -> Pattern:
        raise NotImplementedError

    @property
    @abstractmethod
    def week_of_month_regex(self) -> Pattern:
        raise NotImplementedError

    @property
    @abstractmethod
    def week_of_year_regex(self) -> Pattern:
        raise NotImplementedError

    @property
    @abstractmethod
    def quarter_regex(self) -> Pattern:
        raise NotImplementedError

    @property
    @abstractmethod
    def quarter_regex_year_front(self) -> Pattern:
        raise NotImplementedError

    @property
    @abstractmethod
    def all_half_year_regex(self) -> Pattern:
        raise NotImplementedError

    @property
    @abstractmethod
    def season_regex(self) -> Pattern:
        raise NotImplementedError

    @property
    @abstractmethod
    def week_of_regex(self) -> Pattern:
        raise NotImplementedError

    @property
    @abstractmethod
    def month_of_regex(self) -> Pattern:
        raise NotImplementedError

    @property
    @abstractmethod
    def which_week_regex(self) -> Pattern:
        raise NotImplementedError

    @property
    @abstractmethod
    def rest_of_date_regex(self) -> Pattern:
        raise NotImplementedError

    @property
    @abstractmethod
    def later_early_period_regex(self) -> Pattern:
        raise NotImplementedError

    @property
    @abstractmethod
    def week_with_week_day_range_regex(self) -> Pattern:
        raise NotImplementedError

    @property
    @abstractmethod
    def unspecific_end_of_range_regex(self) -> Pattern:
        raise NotImplementedError

    @property
    @abstractmethod
    def token_before_date(self) -> str:
        raise NotImplementedError

    @property
    @abstractmethod
    def day_of_month(self) -> Dict[str, int]:
        raise NotImplementedError

    @property
    @abstractmethod
    def month_of_year(self) -> Dict[str, int]:
        raise NotImplementedError

    @property
    @abstractmethod
    def cardinal_map(self) -> Dict[str, int]:
        raise NotImplementedError

    @property
    @abstractmethod
    def season_map(self) -> Dict[str, str]:
        raise NotImplementedError

    @property
    @abstractmethod
    def unit_map(self) -> Dict[str, str]:
        raise NotImplementedError

    @property
    @abstractmethod
    def now_regex(self) -> Pattern:
        raise NotImplementedError

    @abstractmethod
    def get_swift_day_or_month(self, source: str) -> int:
        raise NotImplementedError

    @abstractmethod
    def get_swift_year(self, source: str) -> int:
        raise NotImplementedError

    @abstractmethod
    def is_future(self, source: str) -> bool:
        raise NotImplementedError

    @abstractmethod
    def is_year_to_date(self, source: str) -> bool:
        raise NotImplementedError

    @abstractmethod
    def is_month_to_date(self, source: str) -> bool:
        raise NotImplementedError

    @abstractmethod
    def is_week_only(self, source: str) -> bool:
        raise NotImplementedError

    @abstractmethod
    def is_weekend(self, source: str) -> bool:
        raise NotImplementedError

    @abstractmethod
    def is_month_only(self, source: str) -> bool:
        raise NotImplementedError

    @abstractmethod
    def is_last_cardinal(self, source: str) -> bool:
        raise NotImplementedError

    @abstractmethod
    def is_year_only(self, source: str) -> bool:
        raise NotImplementedError


def match_with_next_prefix(self, sub_str, is_ago, is_less_than_or_with_in, is_more_than):
    match = self.config.within_next_prefix_regex.match(sub_str)
    if match and match.success:
        is_next = match.Groups["next"].value

        # cases like "within the next 5 days before today" is not acceptable
        if not (is_next and is_ago):
            is_less_than_or_with_in = True
    is_less_than_or_with_in = is_less_than_or_with_in or self.config.less_than_regex.match(sub_str).success
    is_more_than = self.config.more_than_regex.match(sub_str).success


class BaseDatePeriodParser(DateTimeParser):
    @property
    def parser_type_name(self) -> str:
        return Constants.SYS_DATETIME_DATEPERIOD

    week_of_comment = Constants.WEEK_OF
    month_of_comment = Constants.MONTH_OF

    def __init__(self, config: DatePeriodParserConfiguration, inclusive_end_period: bool = False):
        self.config = config
        self._inclusive_end_period = inclusive_end_period

    def get_year_context(self, config: DatePeriodParserConfiguration, start_date_str: str, end_date_str: str,
                         text: str) -> DateContext:
        is_end_date_pure_year = False
        is_date_relative = False
        context_year = Constants.INVALID_YEAR
        year_match_for_end_date = self.config.year_regex.match(end_date_str)
        if year_match_for_end_date and hasattr(year_match_for_end_date, 'success') and \
                year_match_for_end_date.success and \
                len(year_match_for_end_date) == len(end_date_str):
            is_end_date_pure_year = True
        else:
            is_end_date_pure_year = False
        relative_match_for_start_date = config.relative_regex.search(start_date_str)
        relative_match_for_end_date = config.relative_regex.search(end_date_str)
        if relative_match_for_start_date and relative_match_for_end_date:
            if hasattr(relative_match_for_start_date, 'success') and \
                    hasattr(relative_match_for_end_date, 'success'):
                is_date_relative = relative_match_for_start_date.success or \
                    relative_match_for_end_date.success
        else:
            is_date_relative = None
        if not is_end_date_pure_year and not is_date_relative:
            for match in list(config.year_regex.finditer(text)):
                year = config.date_extractor.get_year_from_text(match)

                if year != Constants.INVALID_YEAR:
                    if context_year == Constants.INVALID_YEAR:
                        context_year = year
                    else:
                        # This indicates that the text has two different year value, no common context year
                        if context_year != year:
                            context_year = Constants.INVALID_YEAR

        result: DateContext = DateContext()
        result.year = context_year

        return result

    def parse(self, source: ExtractResult, reference: datetime = None) -> Optional[DateTimeParseResult]:
        if not reference:
            reference = datetime.now()
        result_value = None

        if source.type == self.parser_type_name:
            source_text = source.text.strip().lower()
            inner_result = self._parse_base_date_period(source_text, reference)

            if not inner_result.success:
                inner_result = self._parse_complex_date_period(source_text, reference)

            if inner_result.success:
                if inner_result.future_value and inner_result.past_value:
                    inner_result.future_resolution = {
                        TimeTypeConstants.START_DATE: DateTimeFormatUtil.format_date(inner_result.future_value[0]),
                        TimeTypeConstants.END_DATE: DateTimeFormatUtil.format_date(
                            inner_result.future_value[1])
                    }
                    inner_result.past_resolution = {
                        TimeTypeConstants.START_DATE: DateTimeFormatUtil.format_date(inner_result.past_value[0]),
                        TimeTypeConstants.END_DATE: DateTimeFormatUtil.format_date(
                            inner_result.past_value[1])
                    }

                else:
                    inner_result.future_resolution = {}
                    inner_result.past_resolution = {}

                result_value = inner_result

        result = DateTimeParseResult(source)
        result.text = source.text
        result.start = source.start
        result.length = source.length
        result.type = source.type
        result.meta_data = source.meta_data
        result.value = result_value
        result.timex_str = result_value.timex if result_value else ''
        result.resolution_str = ''

        return result

    def _parse_base_date_period(self, text: str, reference_date: datetime, date_context: DateContext = None) \
            -> DateTimeResolutionResult:
        inner_result = self.__parse_month_with_year(text, reference_date)
        if not inner_result.success:
            inner_result = self._parse_simple_case(text, reference_date)

        if not inner_result.success:
            inner_result = self._parse_one_word_period(text, reference_date)

        if not inner_result.success:
            inner_result = self._merge_two_times_points(text, reference_date)

        if not inner_result.success:
            inner_result = self._parse_year(text, reference_date)

        if not inner_result.success:
            inner_result = self._parse_week_of_month(text, reference_date)

        if not inner_result.success:
            inner_result = self._parse_week_of_year(text, reference_date)

        if not inner_result.success:
            inner_result = self._parse_half_year(text, reference_date)

        if not inner_result.success:
            inner_result = self.__parse_quarter(text, reference_date)

        if not inner_result.success:
            inner_result = self.__parse_season(text, reference_date)

        if not inner_result.success:
            inner_result = self.__parse_which_week(text, reference_date)

        if not inner_result.success:
            inner_result = self.__parse_week_of_date(text, reference_date)

        if not inner_result.success:
            inner_result = self.__parse_month_of_date(text, reference_date)

        if not inner_result.success:
            inner_result = self.__parse_decade(text, reference_date)

        # Cases like "within/less than/more than x weeks from/before/after today"
        if not inner_result.success:
            inner_result = self.__parse_date_point_with_ago_and_later(text, reference_date)

        if not inner_result.success:
            inner_result = self._parse_duration(text, reference_date)

        if inner_result.success and date_context is not None:
            inner_result = date_context.process_date_period_entity_resolution(inner_result)

        return inner_result

    def _parse_complex_date_period(self, source, reference):
        result = DateTimeResolutionResult()
        match = self.config.complex_dateperiod_regex.match(source)

        if match:
            future_begin = future_end = past_begin = past_end = datetime.min
            is_specific_date = is_start_by_week = is_end_by_week = False
            date_context = self.get_year_context(self.config, match.group('start').strip(), match.group('end').strip(),
                                                 source)
            start_resolution = self.__parse_single_time_point(match.group('start').strip(), reference, date_context)

            if start_resolution and start_resolution.success:
                future_begin, past_begin = start_resolution.future_value, start_resolution.past_value
                is_specific_date = True

            else:
                start_resolution = self._parse_base_date_period(match.group('start').strip(), reference, date_context)

                if start_resolution and start_resolution.success:
                    future_begin = start_resolution.future_value.get(TimeTypeConstants.START_DATE)
                    past_begin = start_resolution.past_value.get(TimeTypeConstants.START_DATE)

                    if '-W' in start_resolution.timex:
                        is_start_by_week = True

            if start_resolution:
                end_resolution = self.__parse_single_time_point(match.group('end').strip(), reference, date_context)

                if end_resolution and end_resolution.success:
                    future_end, past_end = end_resolution.future_value, end_resolution.past_value
                    is_specific_date = True

                else:
                    end_resolution = self._parse_base_date_period(match.group('end').strip(), reference, date_context)

                    if end_resolution and end_resolution.success:
                        future_end = end_resolution.future_value.get(TimeTypeConstants.START_DATE)
                        past_end = end_resolution.past_value.get(TimeTypeConstants.START_DATE)

                        if '-W' in end_resolution.timex:
                            is_end_by_week = True

                if end_resolution:
                    if future_begin > future_end:
                        future_begin = past_begin if not date_context or date_context.is_empty() else \
                            DateContext.swift_date_object(future_begin, future_end)

                    if past_end < past_begin:
                        if not date_context:
                            past_end = future_end
                        else:
                            past_begin = DateContext.swift_date_object(past_begin, past_end)

                    date_period_timex_type = 0 if is_specific_date else 1 if is_start_by_week or is_end_by_week else 2

                    result.timex = TimexUtil.generate_date_period_timex(
                        future_begin, future_end, date_period_timex_type, past_begin, past_end)
                    result.future_value = [future_begin, future_end]
                    result.past_value = [past_begin, past_end]
                    result.success = True

        return result

    def __parse_single_time_point(self, source, reference, date_context=None):
        result = DateTimeResolutionResult()
        extract_result = next(iter(self.config.date_extractor.extract(source, reference)), None)

        if extract_result:
            match = self.config.week_with_week_day_range_regex.match(source)
            week_prefix = None

            if match and match.success:
                week_prefix = match.group('week')

            if week_prefix:
                extract_result.text = f'{week_prefix} {extract_result.text}'

            parse_result = self.config.date_parser.parse(extract_result, reference)

            if parse_result:
                result.timex = f'({parse_result.timex_str}'
                result.future_value = parse_result.value.future_value
                result.past_value = parse_result.value.past_value
                result.success = True
            if date_context:
                result = date_context.process_date_entity_resolution(result)

        return result

    def __parse_month_with_year(self, source: str, reference: datetime) -> DateTimeResolutionResult:
        trimmed_source, result = source.strip().lower(), DateTimeResolutionResult()
        match = self.config.month_with_year.search(trimmed_source)
        if not match:
            match = self.config.month_num_with_year.search(trimmed_source)

        if not (match and match.end() - match.start() == len(trimmed_source)):
            return result

        month_str = RegExpUtility.get_group(match, Constants.MONTH_GROUP_NAME)
        year_str = RegExpUtility.get_group(match, Constants.YEAR_GROUP_NAME)
        order_str = RegExpUtility.get_group(match, Constants.ORDER)
        month = self.config.month_of_year.get(month_str)

        try:
            year = int(year_str)
        except ValueError:
            swift = self.config.get_swift_year(order_str)
            if swift < 1:
                return result
            year = reference.year + swift

        begin_date = DateUtils.safe_create_from_value(DateUtils.min_value, year, month, 1)
        add_days = -1 if self._inclusive_end_period else 0
        end_date = begin_date + datedelta(months=1) + datedelta(days=add_days)

        result.future_value = [begin_date, end_date]
        result.past_value = [begin_date, end_date]
        result.timex = f'{year:04d}-{month:02d}'
        result.success = True

        return result

    def _get_match_simple_case(self, source: str) -> Match:
        match = self.config.month_front_between_regex.search(source)

        if not match:
            match = self.config.between_regex.search(source)

        if not match:
            match = self.config.month_front_simple_cases_regex.search(source)

        if not match:
            match = self.config.simple_cases_regex.search(source)

        return match

    def _parse_simple_case(self, source: str, reference: datetime) -> DateTimeResolutionResult:
        result = DateTimeResolutionResult()
        year, month = reference.year, reference.month
        no_year = True

        match = self._get_match_simple_case(source)

        if not match or match.start() != 0 or match.group() != source:
            return result

        days = match.captures(Constants.DAY_GROUP_NAME)
        begin_day = self.config.day_of_month.get(days[0])
        end_day = self.config.day_of_month.get(days[1])
        year_str = match.group(Constants.YEAR_GROUP_NAME)

        if year_str:
            year = int(year_str)
            no_year = False

        month_str = match.group(Constants.MONTH_GROUP_NAME)

        if month_str:
            month = self.config.month_of_year.get(month_str)
        else:
            month_str = match.group(Constants.REL_MONTH)
            month += self.config.get_swift_day_or_month(month_str)

            if month < 1:
                month = 1
                year -= 1
            elif month > 12:
                month = 12
                year += 1

            if self.config.is_future(month_str):
                no_year = False

        begin_date_luis = DateTimeFormatUtil.luis_date(year if not no_year else -1, month, begin_day)
        end_date_luis = DateTimeFormatUtil.luis_date(year if not no_year else -1, month, end_day)
        future_year = past_year = year
        start_date = DateUtils.safe_create_from_value(DateUtils.min_value, year, month, begin_day)

        if no_year and start_date < reference:
            future_year += 1

        if no_year and start_date >= reference:
            past_year -= 1

        result.timex = f'({begin_date_luis},{end_date_luis},P{end_day - begin_day}D)'
        result.future_value = [
            DateUtils.safe_create_from_value(
                DateUtils.min_value, future_year, month, begin_day),
            DateUtils.safe_create_from_value(
                DateUtils.min_value, future_year, month, end_day)
        ]
        result.past_value = [
            DateUtils.safe_create_from_value(
                DateUtils.min_value, past_year, month, begin_day),
            DateUtils.safe_create_from_value(
                DateUtils.min_value, past_year, month, end_day)
        ]
        result.success = True

        return result

    @staticmethod
    def is_present(swift):
        return swift == 0

    def _parse_one_word_period(self, source: str, reference: datetime) -> DateTimeResolutionResult:
        result = DateTimeResolutionResult()
        year, month = reference.year, reference.month

        if self.config.is_year_to_date(source):
            result.timex = f'{year:04d}'
            result.future_value = [DateUtils.safe_create_from_value(
                DateUtils.min_value, year, 1, 1), reference]
            result.past_value = [DateUtils.safe_create_from_value(
                DateUtils.min_value, year, 1, 1), reference]
            result.success = True

            return result

        if self.config.is_month_to_date(source):
            result.timex = f'{year:04d}-{month:02d}'
            result.future_value = [DateUtils.safe_create_from_value(
                DateUtils.min_value, year, month, 1), reference]
            result.past_value = [DateUtils.safe_create_from_value(
                DateUtils.min_value, year, month, month, 1), reference]
            result.success = True

            return result

        future_year = past_year = year
        trimmed_source = source.strip().lower()
        match = RegExpUtility.exact_match(self.config.one_word_period_regex, trimmed_source, True)

        if not (match and match.success):
            match = RegExpUtility.exact_match(self.config.later_early_period_regex, trimmed_source, True)

        # For cases "that week|month|year"
        if not (match and match.success):
            match = RegExpUtility.exact_match(self.config.reference_date_period_regex, trimmed_source, True)

        if not (match and match.success):
            return result

        early_prefix = late_prefix = mid_prefix = False

        if match.success:
            if match.get_group(Constants.EARLY_PREFIX):
                early_prefix = True
                trimmed_source = match.group(Constants.SUFFIX_GROUP_NAME)
                result.mod = TimeTypeConstants.EARLY_MOD

            elif match.get_group(Constants.LATE_PREFIX):
                late_prefix = True
                trimmed_source = match.group(Constants.SUFFIX_GROUP_NAME)
                result.mod = TimeTypeConstants.LATE_MOD

            elif match.get_group(Constants.MID_PREFIX):
                mid_prefix = True
                trimmed_source = match.group(Constants.SUFFIX_GROUP_NAME)
                result.mod = TimeTypeConstants.MID_MOD

            swift = 0
            month_str = match.get_group(Constants.MONTH_GROUP_NAME)
            if month_str:
                swift = self.config.get_swift_year(trimmed_source)
            else:
                swift = self.config.get_swift_day_or_month(trimmed_source)

            if self.config.unspecific_end_of_range_regex is not None and self.config.unspecific_end_of_range_regex.match(
                    match.value):
                late_prefix = True
                trimmed_source = match.value
                result.mod = TimeTypeConstants.LATE_MOD

            if match.get_group(Constants.REL_EARLY):
                early_prefix = True
                if self.is_present(swift):
                    result.mod = None

            elif match.get_group(Constants.REL_LATE):
                late_prefix = True
                if self.is_present(swift):
                    result.mod = None

            month_str = match.get_group(Constants.MONTH_GROUP_NAME)

            if month_str:
                swift = self.config.get_swift_year(trimmed_source)
                month = self.config.month_of_year.get(month_str)

                if swift >= -1:
                    result.timex = f'{year + swift:04d}-{month:02d}'
                    year += swift
                    future_year = year
                    past_year = year
                else:
                    result.timex = f'XXXX-{month:02d}'

                    if month < reference.month:
                        future_year += 1

                    if month >= reference.month:
                        past_year -= 1

            else:
                swift = self.config.get_swift_day_or_month(trimmed_source)

                if self.config.is_week_only(trimmed_source):
                    thursday = DateUtils.this(reference, DayOfWeek.THURSDAY) + datedelta(days=7 * swift)
                    result.timex = f'{thursday.year:04d}-W{DateUtils.week_of_year(thursday):02d}'
                    begin_date = DateUtils.this(reference, DayOfWeek.MONDAY) + datedelta(days=7 * swift)
                    end_date = DateUtils.this(reference, DayOfWeek.SUNDAY) + datedelta(days=7 * swift)

                    if early_prefix:
                        end_date = DateUtils.this(
                            reference, DayOfWeek.WEDNESDAY) + datedelta(days=7 * swift)

                    elif mid_prefix:
                        begin_date = DateUtils.this(reference, DayOfWeek.TUESDAY) + datedelta(days=7 * swift)
                        end_date = DateUtils.this(reference, DayOfWeek.FRIDAY) + datedelta(days=7 * swift)

                    elif late_prefix:
                        begin_date = DateUtils.this(reference, DayOfWeek.THURSDAY) + datedelta(days=7 * swift)

                    if not self._inclusive_end_period:
                        end_date = end_date + datedelta(days=1)

                    if early_prefix and swift == 0:
                        if end_date > reference:
                            end_date = reference

                    elif late_prefix and swift == 0:
                        if begin_date < reference:
                            begin_date = reference

                    result.future_value = result.past_value = [begin_date, end_date]
                    result.success = True

                    return result

                if self.config.is_weekend(trimmed_source):
                    begin_date = DateUtils.this(reference, DayOfWeek.SATURDAY) + datedelta(days=7 * swift)
                    end_date = DateUtils.this(reference, DayOfWeek.SUNDAY) + datedelta(days=7 * swift)

                    if not self._inclusive_end_period:
                        end_date = end_date + datedelta(days=1)

                    result.timex = f'{year:04d}-W{begin_date.isocalendar()[1]:02d}-WE'
                    result.future_value = result.past_value = [begin_date, end_date]
                    result.success = True

                    return result

                if self.config.is_month_only(trimmed_source):
                    temp_date = reference + datedelta(months=swift)
                    month, year = temp_date.month, temp_date.year
                    result.timex = f'{year:04d}-{month:02d}'
                    future_year = past_year = year

                elif self.config.is_year_only(trimmed_source):
                    temp_date = reference + datedelta(years=swift)
                    year = temp_date.year

                    if late_prefix:
                        begin_date = DateUtils.safe_create_from_min_value(year, 7, 1)
                    else:
                        begin_date = DateUtils.safe_create_from_min_value(year, 1, 1)

                    if early_prefix:
                        end_date = DateUtils.safe_create_from_min_value(year, 6, 30)
                    else:
                        end_date = DateUtils.safe_create_from_min_value(year, 12, 31)

                    if not self._inclusive_end_period:
                        end_date = end_date + datedelta(days=1)

                    result.timex = f'{year:04d}'
                    result.future_value = result.past_value = [begin_date, end_date]
                    result.success = True

                    return result
        else:
            return result

        future_start = DateUtils.safe_create_from_min_value(future_year, month, 1)
        future_end = DateUtils.safe_create_from_min_value(future_year, month, 1) + datedelta(months=1)
        past_start = DateUtils.safe_create_from_min_value(past_year, month, 1)
        past_end = DateUtils.safe_create_from_min_value(past_year, month, 1) + datedelta(months=1)

        if self._inclusive_end_period:
            future_end = future_end + datedelta(days=-1)
            past_end = past_end + datedelta(days=-1)

        if early_prefix:
            future_end = DateUtils.safe_create_from_min_value(future_year, month, 15)
            past_end = DateUtils.safe_create_from_min_value(past_year, month, 15)

            if not self._inclusive_end_period:
                future_end = future_end + datedelta(days=1)
                past_end = past_end + datedelta(days=1)
        elif late_prefix:
            future_start = DateUtils.safe_create_from_min_value(future_year, month, 16)
            past_start = DateUtils.safe_create_from_min_value(past_year, month, 16)

        result.future_value = [future_start, future_end]
        result.past_value = [past_start, past_end]
        result.success = True

        return result

    # Parse entities that are made up by two time points

    def _merge_two_times_points(self, source: str, reference: datetime) -> DateTimeResolutionResult:
        trimmed_source = source.strip()
        result = DateTimeResolutionResult()
        parse_results = []
        parse_result1 = parse_result2 = None
        extract_results = self.config.date_extractor.extract(trimmed_source, reference)

        if not extract_results or len(extract_results) < 2:
            extract_results = self.config.date_extractor.extract(
                self.config.token_before_date + trimmed_source, reference)

            if len(extract_results) >= 2:
                for extract_result in extract_results:
                    extract_result.start -= len(self.config.token_before_date)
            else:
                now_pr = self._parse_now_as_date(source, reference)
                if now_pr.value is None or len(extract_results) < 1:
                    return result

                date_pr = self.config.date_parser.parse(extract_results[0], reference)

                parse_result1 = date_pr if date_pr.start < now_pr.start else now_pr
                parse_result2 = now_pr if date_pr.start < now_pr.start else date_pr

                parse_results.append(now_pr)
                parse_results.append(date_pr)
                parse_results = sorted(parse_results, key=lambda x: x.start)

        if extract_results and len(extract_results) >= 2:
            # Propagate the possible future relative context from the first entity to the second one in the range.
            # Handles cases like "next monday to friday"
            future_match_for_start_date = self.config.future_regex.match(extract_results[0].text)
            future_match_for_end_date = self.config.future_regex.match(extract_results[1].text)

            if future_match_for_start_date and future_match_for_start_date.success and not future_match_for_end_date.success:
                extract_results[1].text = future_match_for_start_date.value + ' ' + extract_results[1].text

            match = self.config.week_with_week_day_range_regex.search(source)
            week_prefix = None

            # Check if weekPrefix is already included in the extractions otherwise include it
            if match:
                week_prefix = RegExpUtility.get_group(match, Constants.WEEK_GROUP_NAME)

            if week_prefix:
                extract_results[0].text = f'{week_prefix} {extract_results[0].text}'
                extract_results[1].text = f'{week_prefix} {extract_results[1].text}'

            date_context = self.get_year_context(self.config, extract_results[0].text, extract_results[1].text, source)

            parse_result1 = self.config.date_parser.parse(extract_results[0], reference)
            parse_result2 = self.config.date_parser.parse(extract_results[1], reference)

            if not parse_result1.value or not parse_result2:
                return result

            parse_result1 = date_context.process_date_entity_parsing_result(parse_result1)
            parse_result2 = date_context.process_date_entity_parsing_result(parse_result2)

            # When the case has no specified year, we should sync the future/past year due to invalid date Feb 29th.
            if date_context.is_empty() and (DateUtils.is_Feb_29th_datetime(parse_result1.value.future_value) or DateUtils.is_Feb_29th_datetime(parse_result2.value.future_value)):
                parse_result1, parse_result2 = date_context.sync_year(parse_result1, parse_result2)

        result.sub_date_time_entities = [parse_result1, parse_result2]

        future_begin = parse_result1.value.future_value
        future_end = parse_result2.value.future_value
        past_begin = parse_result1.value.past_value
        past_end = parse_result2.value.past_value

        if future_begin > future_end:
            future_begin = past_begin

        if past_end < past_begin:
            past_end = future_end

        result.timex = TimexUtil.generate_date_period_timex_str(future_begin, future_end, 0, parse_result1.timex_str, parse_result2.timex_str)

        if parse_result1.timex_str.startswith(Constants.TIMEX_FUZZY_YEAR) and future_begin <= DateUtils.safe_create_from_value(DateUtils.min_value, future_begin.year, 2, 28) and future_end >= DateUtils.safe_create_from_value(DateUtils.min_value, future_begin.year, 3, 1):
            # Handle cases like "2/28 - 3/1".
            # There may be different timexes for FutureValue and PastValue due to the different validity of Feb 29th.
            result.comment = Constants.COMMENT_DOUBLETIMEX
            past_timex = TimexUtil.generate_date_period_timex_str(past_begin, past_end, 0, parse_result1.timex_str, parse_result2.timex_str)
            result.timex = TimexUtil.merge_timex_alternatives(result.timex, past_timex)
        result.future_value = [future_begin, future_end]
        result.past_value = [past_begin, past_end]
        result.success = True

        return result

    # Handle "between...and..." when contains with "now"
    def _parse_now_as_date(self, source: str, reference: datetime) -> DateTimeParseResult:
        parse_result = DateTimeParseResult()
        match = self.config.now_regex.search(source)
        if match is not None:
            value = DateUtils.safe_create_from_min_value(
                reference.year, reference.month, reference.day)
            ret_now = DateTimeResolutionResult()
            ret_now.timex = DateTimeFormatUtil.luis_date_from_datetime(
                reference)
            ret_now.future_value = value
            ret_now.past_value = value
            parse_result.text = match.string
            parse_result.start = match.start()
            parse_result.length = match.end() - match.start()
            parse_result.value = ret_now
            parse_result.type = Constants.SYS_DATETIME_DATE
            parse_result.timex_str = ret_now.timex
        return parse_result

    def _parse_year(self, source: str, reference: datetime) -> DateTimeResolutionResult:
        trimmed_source = source.strip()
        result = DateTimeResolutionResult()
        match = self.config.year_regex.search(trimmed_source)

        if not (match and len(match.group()) == len(trimmed_source)):
            return result

        year = int(self.config.date_extractor.get_year_from_text(match))
        begin_date = DateUtils.safe_create_from_value(
            DateUtils.min_value, year, 1, 1)
        end_date = DateUtils.safe_create_from_value(
            DateUtils.min_value, year + 1, 1, 1)

        if self._inclusive_end_period:
            end_date = end_date + datedelta(days=-1)

        result.timex = f'{year:04d}'
        result.future_value = [begin_date, end_date]
        result.past_value = [begin_date, end_date]
        result.success = True

        return result

    # To be consistent, we follow the definition of "week of year":
    # "first week of the month" - it has the month's first Thursday in it
    # "last week of the month" - it has the month's last Thursday in it
    def _parse_week_of_month(self, source: str, reference: datetime) -> DateTimeResolutionResult:
        result = DateTimeResolutionResult()
        match = self.config.week_of_month_regex.search(source)

        if not (match and len(match.group()) == len(source)):
            return result

        cardinal_str = RegExpUtility.get_group(match, Constants.CARDINAL)
        month_str = RegExpUtility.get_group(match, Constants.MONTH_GROUP_NAME)
        year, month = reference.year, reference.month
        no_year = False
        cardinal = 5 if self.config.is_last_cardinal(cardinal_str) else self.config.cardinal_map.get(cardinal_str)

        if not month_str:
            swift = self.config.get_swift_day_or_month(source)
            temp_data = reference + datedelta(months=swift)
            year, month = temp_data.year, temp_data.month

        else:
            month = self.config.month_of_year.get(month_str)
            no_year = True

        return self._get_week_of_month(cardinal, month, year, reference, no_year)

    def _get_week_of_month(self, cardinal, month, year, reference, no_year) -> DateTimeResolutionResult:
        result = DateTimeResolutionResult()
        seed_date = self._compute_date(cardinal, 1, month, year)

        if not seed_date.month == month:
            cardinal -= 1
            seed_date = seed_date + datedelta(days=-7)

        future_date = past_date = seed_date

        if no_year and future_date < reference:
            future_date = self._compute_date(cardinal, 1, month, year + 1)
            if not future_date.month == month:
                future_date = future_date + datedelta(days=-7)

        if no_year and past_date >= reference:
            past_date = self._compute_date(cardinal, 1, month, year - 1)
            if not past_date.month == month:
                past_date = past_date + datedelta(days=-7)

        # Note that if the cardinalStr equals to "last", the weekNumber would be fixed at "5"
        # This may lead to some inconsistency between Timex and Resolution
        # the StartDate and EndDate of the resolution would always be correct (following ISO week definition).
        # But week number for "last week" might be inconsistent with the resolution as we only have one Timex,
        # but we may have past and future resolutions which may have different week numbers
        result.timex = ('XXXX' if no_year else f'{year:04d}') + f'-{month:02d}-W{cardinal:02d}'
        days_to_add = 6 if self._inclusive_end_period else 7
        result.future_value = [future_date, future_date + datedelta(days=days_to_add)]
        result.past_value = [past_date, past_date + datedelta(days=days_to_add)]
        result.success = True

        return result

    # We follow the ISO week definition:
    # "first week of the year" - it has the year's first Thursday in it
    # "last week of the year" - it has the year's last Thursday in it
    def _parse_week_of_year(self, source: str, reference: datetime) -> DateTimeResolutionResult:
        result = DateTimeResolutionResult()
        match = self.config.week_of_year_regex.search(source)

        if not (match and len(match.group()) == len(source)):
            return result

        cardinal_str = match.group(Constants.CARDINAL)
        year_str = match.group(Constants.YEAR_GROUP_NAME)
        order_str = match.group(Constants.ORDER)

        if not year_str:
            swift = self.config.get_swift_year(order_str)
            if swift < -1:
                return result
            year = reference.year + swift
        else:
            try:
                year = int(year_str)
            except ValueError:
                swift = self.config.get_swift_year(order_str)
                if swift < -1:
                    return result
                year = reference.year + swift

        if self.config.is_last_cardinal(cardinal_str):
            last_day = DateUtils.safe_create_from_min_value(year, 12, 31)
            last_day_week_monday = DateUtils.this(last_day, DayOfWeek.MONDAY)
            week_num = DateUtils.week_of_year(last_day)

            if week_num == 1:
                last_day_week_monday = DateUtils.this(
                    last_day + datedelta(days=-7), DayOfWeek.MONDAY)

            target_week_monday = last_day_week_monday
            week_num = DateUtils.week_of_year(target_week_monday)
            result.timex = f'{year:04d}-W{week_num:02d}'

        else:
            cardinal = self.config.cardinal_map.get(cardinal_str)
            first_day = DateUtils.safe_create_from_min_value(year, 1, 1)
            first_day_week_monday = DateUtils.this(first_day, DayOfWeek.MONDAY)
            week_num = DateUtils.week_of_year(first_day)

            if not week_num == 1:
                first_day_week_monday = DateUtils.this(first_day + datedelta(days=7), DayOfWeek.MONDAY)

            target_week_monday = first_day_week_monday + datedelta(days=7 * (cardinal - 1))
            result.timex = f'{year:04d}-W{week_num:02d}'

        days_to_add = 6 if self._inclusive_end_period else 7
        result.future_value = [target_week_monday, target_week_monday + datedelta(days=days_to_add)]
        result.past_value = [target_week_monday, target_week_monday + datedelta(days=days_to_add)]
        result.success = True

        return result

    def _parse_half_year(self, source: str, reference: datetime) -> DateTimeResolutionResult:
        result = DateTimeResolutionResult()
        match = self.config.all_half_year_regex.search(source)

        if not (match and len(match.group()) == len(source)):
            return result

        cardinal_str = match.group(Constants.CARDINAL)
        year_str = match.group(Constants.YEAR_GROUP_NAME)
        order_str = match.group(Constants.ORDER)
        number_str = match.group(Constants.NUMBER)

        try:
            year = int(year_str)

        except (ValueError, TypeError):
            order_str = '' if order_str is None else order_str
            swift = self.config.get_swift_year(order_str)
            if swift < -1:
                return result
            year = reference.year + swift

        quarter_num = self.config.cardinal_map[cardinal_str] if not number_str else int(number_str)
        begin_date = DateUtils.safe_create_date_resolve_overflow(
            year, ((quarter_num - 1) * Constants.SEMESTER_MONTH_COUNT) + 1, 1)
        end_date = DateUtils.safe_create_date_resolve_overflow(
            year, (quarter_num * Constants.SEMESTER_MONTH_COUNT) + 1, 1)

        result.future_value = result.past_value = [begin_date, end_date]
        result.timex = f'({DateTimeFormatUtil.luis_date_from_datetime(begin_date)},' \
            f'{DateTimeFormatUtil.luis_date_from_datetime(end_date)},P6M)'
        result.success = True

        return result

    def _compute_date(self, cardinal: int, weekday: int, month: int, year: int):
        first_day = datetime(year, month, 1)
        first_week_day = DateUtils.this(first_day, weekday)

        if weekday == 0:
            weekday = 7

        first_day_of_week = first_day.isoweekday()

        if weekday < first_day_of_week:
            first_week_day = DateUtils.next(first_day, weekday)

        first_week_day = first_week_day + datedelta(days=7 * (cardinal - 1))

        return first_week_day

    def _parse_duration(self, source: str, reference: datetime) -> DateTimeResolutionResult:
        result = DateTimeResolutionResult()
        extract_results = self.config.duration_extractor.extract(source, reference)
        begin_date = end_date = reference
        rest_now_sunday = False
        duration_timex = mod = ''

        if len(extract_results) == 1:
            parse_result = self.config.duration_parser.parse(extract_results[0])

            if parse_result is None:
                return result

            before_str = source[0:parse_result.start].strip()
            duration_result = parse_result.value

            if not duration_result.timex:
                return result

            prefix_match = self.config.past_regex.search(before_str)

            if prefix_match:
                mod = TimeTypeConstants.BEFORE_MOD
                begin_date = self.__get_swift_date(end_date, duration_result.timex, False)

            is_match = False
            prefix_match = self.config.future_regex.search(before_str)

            if prefix_match and len(prefix_match.string) == len(before_str):
                mod = TimeTypeConstants.AFTER_MOD
                begin_date = reference + timedelta(days=1)
                end_date = self.__get_swift_date(begin_date, duration_result.timex, True)
                is_match = True

            prefix_match = self.config.in_connector_regex.search(before_str)

            if prefix_match and len(prefix_match.string) == len(before_str) and not is_match:
                mod = TimeTypeConstants.AFTER_MOD
                begin_date = reference + timedelta(days=1)
                end_date = self.__get_swift_date(begin_date, duration_result.timex, True)
                unit = duration_result.timex[len(duration_result.timex) - 1]
                duration_result.timex = f'P1{unit}'
                begin_date = self.__get_swift_date(end_date, duration_result.timex, False)

            if mod:
                parse_result.value.mod = mod

            duration_timex = duration_result.timex
            result.sub_date_time_entities = [parse_result]

        match = self.config.rest_of_date_regex.search(source)

        if match:
            diff_days = 0
            duration_str = match.group(TimeTypeConstants.DURATION)
            duration_unit = self.config.unit_map.get(duration_str)

            if duration_unit == Constants.UNIT_W:
                diff_days = 7 - begin_date.isoweekday()
                end_date = reference + timedelta(days=diff_days)
                rest_now_sunday = diff_days == 0

            elif duration_unit == Constants.UNIT_MON:
                end_date = DateUtils.safe_create_from_min_value(
                    begin_date.year, begin_date.month, DateUtils.last_day_of_month(begin_date.year, begin_date.month))
                diff_days = end_date.day - begin_date.day + 1

            elif duration_unit == Constants.UNIT_Y:
                end_date = DateUtils.safe_create_from_min_value(begin_date.year, 12, 31)
                diff_days = DateUtils.day_of_year(end_date) - DateUtils.day_of_year(begin_date) + 1

            duration_timex = f'P{diff_days}D'

        if not begin_date == end_date or rest_now_sunday:
            if self._inclusive_end_period:
                end_date = end_date + timedelta(days=-1)

            result.timex = f'({DateTimeFormatUtil.luis_date_from_datetime(begin_date)},' \
                f'{DateTimeFormatUtil.luis_date_from_datetime(end_date)},{duration_timex})'
            result.future_value = [begin_date, end_date]
            result.past_value = [begin_date, end_date]
            result.success = True

        return result

    # Only handle cases like "within/less than/more than x weeks from/before/after today"
    def __parse_date_point_with_ago_and_later(self, source: str, reference: datetime) -> DateTimeResolutionResult:
        result = DateTimeResolutionResult()
        extract_result = next(iter(self.config.date_extractor.extract(source, reference)), None)

        if extract_result:
            before_str = source[0:extract_result.start].strip()
            after_str = source[extract_result.start + extract_result.length:]
            is_ago = self.config.ago_regex.match(extract_result.text)
            is_later = self.config.later_regex.match(extract_result.text)

            if (before_str or (self.config.check_both_before_after and after_str)) and (is_ago or is_later):
                is_less_than_or_with_in = False
                is_more_than = False

                # TODO: move hardcoded English strings to definition
                # cases like "within 3 days from yesterday/tomorrow" does not make any sense
                if "today" in extract_result or "now" in extract_result:
                    match_with_next_prefix(before_str, is_ago, is_less_than_or_with_in, is_more_than)
                else:
                    is_less_than_or_with_in = is_less_than_or_with_in or self.config.less_than_regex.match(before_str).success
                    is_more_than = self.config.less_than_regex.match(before_str).success

                # Check also after_str
                if self.config.check_both_before_after and is_less_than_or_with_in and is_more_than:
                    match_with_next_prefix(after_str, is_ago, is_less_than_or_with_in, is_more_than)

                parsing_result = datetime(self.config.date_parser.parse(extract_result, reference))
                duration_extraction_result = next(self.config.duration_extractor.extract(extract_result.text), None)

                if duration_extraction_result:
                    duration = self.config.duration_parser.parse(duration_extraction_result)
                    duration_in_seconds = DateTimeResolutionResult(duration.value).past_value

                if is_less_than_or_with_in:
                    start_date: datetime
                    end_date: datetime

                    if is_ago:
                        start_date = DateTimeResolutionResult(parsing_result.value).past_value
                        end_date = start_date.AddSeconds(duration_in_seconds)
                    else:
                        end_date = DateTimeResolutionResult(parsing_result.value).future_value
                        start_date = end_date.AddSeconds(-duration_in_seconds)

                    if start_date != datetime.min:
                        start_luis_str = DateTimeFormatUtil.luis_date(start_date, 1, 1)
                        end_luis_str = DateTimeFormatUtil.luis_date(end_date, 1, 1)
                        duration_timex = DateTimeResolutionResult(duration.value).timex

                        result.timex = "({startLuisStr},{endLuisStr},{durationTimex})"
                        result.future_value = Dict[start_date, end_date]
                        result.past_value = Dict[start_date, end_date]

                        result.success = True
                    else:
                        if is_more_than:
                            result.mod = Constants.BEFORE_MOD if is_ago else Constants.AFTER_MOD
                            result.timex = parsing_result.TimexStr
                            result.future_value = parsing_result.future_value
                            result.past_value = parsing_result.past_value
                            result.success = True
        return result

    def __parse_decade(self, source: str, reference_date: datetime) -> DateTimeResolutionResult:
        result = DateTimeResolutionResult()
        first_two_number_of_year = reference_date.year / 100

        decade_last_year = 10
        swift = 1
        input_century = False

        trimmed_source = source.strip().lower()
        match = self.config.decade_with_century_regex.match(trimmed_source, True)
        begin_luis_str: str
        end_luis_str: str

        if match and match.success:
            decade_str = match.group("decade").value
            decade = int(decade_str)

            if not DateUtils.int_try_parse(decade_str)[1]:

                if decade_str in self.config.written_decades:
                    decade = self.config.written_decades[decade_str]
                else:
                    if decade_str in self.config.special_decade_cases:
                        first_two_number_of_year = self.config.special_decade_cases[decade_str] / 100
                        decade = self.config.special_decade_cases[decade_str] % 100
                        inputCentury = True

            century_str = match.group("century").value
            if century_str:
                if not DateUtils.int_try_parse(century_str)[1]:
                    if century_str in self.config.numbers:
                        first_two_number_of_year = self.config.numbers[century_str]
                    else:
                        # handle the case like "one/two thousand", "one/two hundred", etc.
                        extract_result = self.config.integer_extractor.extract(century_str)
                        if len(extract_result) == 0:
                            return result

                        first_two_number_of_year = int(self.config.number_parser.parse(extract_result[0]))
                        if first_two_number_of_year >= 100:
                            first_two_number_of_year = first_two_number_of_year / 100
                input_century = True
        else:
            # handle cases like "the last 2 decades", "the next decade"
            match = RegExpUtility.is_exact_match(self.config.relative_decade_regex, trimmed_source, True)

            if match and match.success:
                input_century = True
                swift = self.config.get_swift_day_or_month(trimmed_source)

                num_str = match.groups["number"].value
                extract_result = self.config.integer_extractor.extract(num_str)
                if len(extract_result) == 1:
                    swift_num = int(self.config.number_parser.parse(extract_result[0]))
                    swift = swift * swift_num

                begin_decade = (reference_date.year % 100) / 10
                if swift < 0:
                    begin_decade += swift
                else:
                    if swift > 0:
                        begin_decade += 1
                decade = begin_decade * 10
            else:
                return result

        begin_year = (first_two_number_of_year * 100) + decade

        # swift + 0 corresponding to the/this decade
        total_last_year = decade_last_year * abs(1 if swift == 0 else swift)

        if input_century:
            begin_luis_str = DateTimeFormatUtil.luis_date(begin_year, 1, 1)
            end_luis_str = DateTimeFormatUtil.luis_date(begin_year + total_last_year, 1, 1)
        else:
            begin_year_str = "XX" + decade
            begin_luis_str = DateTimeFormatUtil.luis_date(-1, 1, 1)
            begin_luis_str = begin_luis_str.replace("XXXX", begin_year_str)

            end_year_str = "XX" + (decade + total_last_year)
            end_luis_str = DateTimeFormatUtil.luis_date(-1, 1, 1)
            end_luis_str = end_luis_str.replace("XXXX", end_year_str)

        result.timex = "({beginLuisStr},{endLuisStr},P{totalLastYear}Y)"

        future_year = begin_year
        past_year = begin_year
        start_date = DateUtils.safe_create_from_value(begin_year, 1, 1, 1)
        if not input_century and start_date < reference_date:
            future_year += 100

        if not input_century and start_date >= reference_date:
            past_year -= 100

        result.future_value = Dict[datetime, datetime]
        result.future_value = [DateUtils.safe_create_from_value(future_year, 1, 1, 1),
                               DateUtils.safe_create_from_value(future_year + total_last_year, 1, 1, 1)]

        result.past_value = Dict[datetime, datetime]
        result.past_value = [DateUtils.safe_create_from_value(past_year, 1, 1, 1),
                             DateUtils.safe_create_from_value(past_year + total_last_year, 1, 1, 1)]

        result.success = True

        return result

    def __parse_quarter(self, source: str, reference: datetime) -> DateTimeResolutionResult:
        result = DateTimeResolutionResult()
        match = self.config.quarter_regex.search(source)
        if not (match and len(match.group()) == len(source)):
            match = self.config.quarter_regex_year_front.search(source)
        if not (match and len(match.group()) == len(source)):
            return result

        cardinal_str = RegExpUtility.get_group(match, Constants.CARDINAL)
        year_str = RegExpUtility.get_group(match, Constants.YEAR_GROUP_NAME)
        order_quarter_str = RegExpUtility.get_group(match, Constants.ORDER_QUARTER)
        order_str = None if order_quarter_str else RegExpUtility.get_group(match, Constants.ORDER)
        quarter_str = RegExpUtility.get_group(match, Constants.NUMBER)

        no_specific_value = False
        try:
            year = int(year_str)
        except (ValueError, TypeError):
            order_str = '' if order_str is None else order_str
            swift = 0 if order_quarter_str else self.config.get_swift_year(order_str)
            if swift < -1:
                swift = 0
                no_specific_value = True
            year = reference.year + swift

        if quarter_str:
            quarter_num = int(quarter_str)
        elif order_quarter_str:
            month = reference.month
            quarter_num = math.ceil(month / Constants.TRIMESTER_MONTH_COUNT)
            swift = self.config.get_swift_year(order_quarter_str)
            quarter_num += swift
            if quarter_num <= 0:
                quarter_num += Constants.QUARTER_COUNT
                year -= 1
            elif quarter_num > Constants.QUARTER_COUNT:
                quarter_num -= Constants.QUARTER_COUNT
                year += 1
        else:
            quarter_num = self.config.cardinal_map[cardinal_str]

        begin_date = DateUtils.safe_create_date_resolve_overflow(
            year, ((quarter_num - 1) * Constants.TRIMESTER_MONTH_COUNT) + 1, 1)
        end_date = DateUtils.safe_create_date_resolve_overflow(
            year, (quarter_num * Constants.TRIMESTER_MONTH_COUNT) + 1, 1)

        if no_specific_value:
            if end_date < reference:
                result.past_value = [begin_date, end_date]

                future_begin_date = DateUtils.safe_create_date_resolve_overflow(
                    year + 1, ((quarter_num - 1) * Constants.TRIMESTER_MONTH_COUNT) + 1, 1)
                future_end_date = DateUtils.safe_create_date_resolve_overflow(
                    year + 1, (quarter_num * Constants.TRIMESTER_MONTH_COUNT) + 1, 1)
                result.future_value = [future_begin_date, future_end_date]
            elif end_date > reference:
                result.future_value = [begin_date, end_date]

                past_begin_date = DateUtils.safe_create_date_resolve_overflow(
                    year - 1, ((quarter_num - 1) * Constants.TRIMESTER_MONTH_COUNT) + 1, 1)
                past_end_date = DateUtils.safe_create_date_resolve_overflow(
                    year - 1, (quarter_num * Constants.TRIMESTER_MONTH_COUNT) + 1, 1)
                result.past_value = [past_begin_date, past_end_date]
            else:
                result.future_value = [begin_date, end_date]
                result.past_value = [begin_date, end_date]

            result.timex = f'({DateTimeFormatUtil.luis_date(-1, begin_date.month, 1)},' \
                f'{DateTimeFormatUtil.luis_date(-1, end_date.month, 1)},P3M)'
        else:
            result.future_value = [begin_date, end_date]
            result.past_value = [begin_date, end_date]
            result.timex = f'({DateTimeFormatUtil.luis_date_from_datetime(begin_date)},' \
                f'{DateTimeFormatUtil.luis_date_from_datetime(end_date)},P3M)'

        result.success = True
        return result

    def __parse_season(self, source: str, reference: datetime) -> DateTimeResolutionResult:
        result = DateTimeResolutionResult()
        match = self.config.season_regex.search(source)
        if not (match and len(match.group()) == len(source)):
            return result

        swift = self.config.get_swift_year(source)
        year_str = match.group(Constants.YEAR_GROUP_NAME)
        year = reference.year
        season_str = match.group(Constants.SEAS)
        season = self.config.season_map[season_str]

        if swift >= -1 or year_str:
            if not year_str:
                year_str = year + swift
            result.timex = f'{year_str}-{season}'
        else:
            result.timex = season

        result.success = True
        return result

    def __parse_which_week(self, source: str, reference: datetime) -> DateTimeResolutionResult:
        result = DateTimeResolutionResult()
        match = self.config.which_week_regex.search(source)
        if not match:
            return result

        num = int(match.group(Constants.NUMBER))
        year = reference.year
        result.timex = f'{year:04d}-W{num:02d}'
        first_day = DateUtils.safe_create_from_value(DateUtils.min_value, year, 1, 1)
        first_thursday = DateUtils.this(first_day, DayOfWeek.THURSDAY)
        first_week = DateUtils.week_of_year(first_thursday)

        if first_week == 1:
            num -= 1

        result_date = first_thursday + timedelta(days=7 * num - 3)

        result.future_value = [result_date, result_date + timedelta(days=7)]
        result.past_value = [result_date, result_date + timedelta(days=7)]
        result.success = True
        return result

    def __parse_week_of_date(self, source: str, reference: datetime) -> DateTimeResolutionResult:
        result = DateTimeResolutionResult()
        match = self.config.week_of_regex.search(source)
        ers = self.config.date_extractor.extract(source, reference)

        if not (match and len(ers) == 1):
            return result

        date_resolution = self.config.date_parser.parse(ers[0], reference).value
        result.timex = date_resolution.timex
        result.comment = BaseDatePeriodParser.week_of_comment
        result.future_value = self.__get_week_range_from_date(date_resolution.future_value)
        result.past_value = self.__get_week_range_from_date(date_resolution.past_value)
        result.success = True
        return result

    def __parse_month_of_date(self, source: str, reference: datetime) -> DateTimeResolutionResult:
        result = DateTimeResolutionResult()
        match = self.config.month_of_regex.search(source)
        ers = self.config.date_extractor.extract(source, reference)

        if not (match and len(ers) == 1):
            return result

        date_resolution = self.config.date_parser.parse(ers[0], reference).value
        result.timex = date_resolution.timex
        result.comment = BaseDatePeriodParser.week_of_comment
        result.future_value = self.__get_month_range_from_date(date_resolution.future_value)
        result.past_value = self.__get_month_range_from_date(date_resolution.past_value)
        result.success = True
        return result

    def __get_week_range_from_date(self, seed_date: datetime) -> List[datetime]:
        begin_date = DateUtils.this(seed_date, DayOfWeek.MONDAY)
        end_date = begin_date + timedelta(days=6 if self._inclusive_end_period else 7)
        return [begin_date, end_date]

    @staticmethod
    def __get_month_range_from_date(seed_date: datetime) -> List[datetime]:
        begin_date = DateUtils.safe_create_from_value(DateUtils.min_value, seed_date.year, seed_date.month, 1)
        end_date = DateUtils.safe_create_from_value(DateUtils.min_value, seed_date.year, seed_date.month + 1, 1)
        return [begin_date, end_date]

    @staticmethod
    def __get_swift_date(date: datetime, timex: str, is_positive_swift: bool) -> datetime:
        result = date
        num_str = timex.replace('P', '')[0:len(timex) - 2]
        unit_str = timex[len(timex) - 1]
        swift = int(num_str)

        if swift == 0:
            return result

        if not is_positive_swift:
            swift = swift * -1
        if unit_str == Constants.UNIT_D:
            result = result + timedelta(days=swift)
        elif unit_str == Constants.UNIT_W:
            result = result + timedelta(weeks=swift)
        elif unit_str == Constants.UNIT_M:
            result = result + datedelta(months=swift)
        elif unit_str == Constants.UNIT_Y:
            result = result + datedelta(years=swift)

        return result
