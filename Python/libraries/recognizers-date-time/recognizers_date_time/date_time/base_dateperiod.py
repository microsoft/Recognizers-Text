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
    RegExpUtility, RegexExtension

MatchedIndex = namedtuple('MatchedIndex', ['matched', 'index'])


class DatePeriodExtractorConfiguration(ABC):
    @property
    @abstractmethod
    def simple_cases_regexes(self) -> List[Pattern]:
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

        matches = regex.finditer(self.config.year_period_regex, text)

        for match in matches:
            match_year = regex.match(self.config.year_regex, match.string)

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
                        break
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

                if RegexExtension.is_exact_match(self.config.month_num_regex, digit_number_str, True):
                    has_invalid_dash_context = True

        has_dash_suffix, dash_suffix_index = self.has_dash_suffix(match, text)

        # Filter out cases like "2100-82" where "2100" should not be extracted as a DateRange
        # Filter out cases like "2010-2015-82" where "2010-2015" should not be extracted as a DateRange
        if has_dash_suffix:

            has_digit_number_after_dash, number_end_index = self.has_digit_number_after_dash(text, dash_suffix_index)

            number_start_index = text.index(match.group()) + (match.end() - match.start()) + 1
            digit_number_str = text[number_start_index: number_start_index + (number_end_index - number_start_index)]

            if RegexExtension.is_exact_match(self.config.month_num_regex, digit_number_str, True):
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
                            date_point.length >= simple_date_range.start + simple_date_range.length,
                            er))),
                simple_date_range_results))
        )

        er = list(sorted(er, key=lambda x: x.start))

        return self.merge_multiple_extractions(text, er)

    def match_simple_cases(self, source: str) -> List[ExtractResult]:
        tokens = []

        for regexp in self.config.simple_cases_regexes:
            matches = list(regex.finditer(regexp, source))

            for match in matches:
                add_token = True
                match_year = self.config.year_regex.search(match.group())

                if match_year and len(match_year.group()) == len(match.group()):
                    year_str = match_year.group(Constants.YEAR_GROUP_NAME)

                    if not year_str:
                        year = self.__get_year_from_text(match_year)

                        if not (Constants.MIN_YEAR_NUM <= year <= Constants.MAX_YEAR_NUM):
                            add_token = False

                if (match.end() - match.start() == Constants.FOUR_DIGITS_YEAR_LENGTH) and self.__infix_boundary_check(
                        match, source):
                    sub_str = source[match.start() - 1: match.end() + 1]

                    # Handle single year which is surrounded by '-' at both sides, e.g., a single year falls in a GUID
                    if self.config.illegal_year_regex.match(sub_str):
                        add_token = False

                if add_token:
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

    def merge_two_time_points(self, source: str, reference: datetime) -> List[ExtractResult]:
        tokens = []
        er = self.config.date_point_extractor.extract(source, reference)

        # Handle "now"
        if len(er) <= 1:
            match = self.config.now_regex.search(source)
            if match is None:
                return tokens
            now_er = ExtractResult()
            now_er.start = match.start()
            now_er.length = match.end() - match.start()
            er.append(now_er)
            er = sorted(er, key=lambda x: x.start)

        idx = 0

        while idx < len(er) - 1:
            middle_begin = er[idx].start + (er[idx].length or 0)
            middle_end = er[idx + 1].start or 0

            if middle_begin >= middle_end:
                idx += 1
                continue

            middle_str = source[middle_begin:middle_end].strip().lower()
            match = self.config.till_regex.search(middle_str)

            if match and match.group() and match.start() == 0 and match.end() - match.start() == len(middle_str):
                period_begin = er[idx].start
                period_end = (er[idx + 1].start or 0) + \
                             (er[idx + 1].length or 0)
                before_str = source[0:period_begin].strip().lower()
                from_token_index = self.config.get_from_token_index(before_str)
                between_token_index = self.config.get_between_token_index(
                    before_str)

                if from_token_index.matched or between_token_index.matched:
                    period_begin = from_token_index.index if from_token_index.matched else between_token_index.index
                tokens.append(Token(period_begin, period_end))
                idx += 2
                continue

            if self.config.has_connector_token(middle_str):
                period_begin = er[idx].start or 0
                period_end = (er[idx + 1].start or 0) + \
                             (er[idx + 1].length or 0)
                before_str = source[0:period_begin].strip().lower()
                between_token_index = self.config.get_between_token_index(
                    before_str)

                if between_token_index.matched:
                    period_begin = between_token_index.index
                    tokens.append(Token(period_begin, period_end))
                    idx += 2
                    continue

            idx += 1
        return tokens

    def merge_multiple_extractions(self, source: str, erd: [ExtractResult]):
        tokens = []

        metadata = Metadata()
        metadata.possibly_included_period_end = True

        if len(erd) < 1:
            return tokens
        er = []
        for x in erd:
            if x.data is not None:
                er.append(x)
        idx = 0
        if len(er) < 1:
            return tokens

        while idx < len(er) - 1:
            middle_begin = er[idx].start + (er[idx].length or 0)
            middle_end = er[idx + 1].start or 0

            if middle_begin >= middle_end:
                idx += 1
                continue

            middle_str = source[middle_begin:middle_end].strip().lower()
            match = self.config.till_regex.search(middle_str)

            if match and match.group() and match.start() == 0 and match.end() - match.start() == len(middle_str):
                period_begin = er[idx].start
                period_end = (er[idx + 1].start or 0) + \
                             (er[idx + 1].length or 0)

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
                period_begin = er[idx].start or 0
                period_end = (er[idx + 1].start or 0) + \
                             (er[idx + 1].length or 0)

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

            idx += 1

    def match_duration(self, source: str, reference: datetime) -> List[ExtractResult]:
        tokens = []
        durations = []

        for duration_ex in self.config.duration_extractor.extract(source, reference):
            match = self.config.date_unit_regex.search(duration_ex.text)
            if match:
                durations.append(
                    Token(duration_ex.start, duration_ex.start + duration_ex.length))

        for duration in durations:
            before_str = source[0:duration.start].lower()

            if not before_str:
                break

            match = self.config.past_regex.search(before_str)
            if self.__match_regex_in_prefix(before_str, match):
                tokens.append(Token(match.start(), duration.end))
                break

            match = self.config.future_regex.search(before_str)
            if self.__match_regex_in_prefix(before_str, match):
                tokens.append(Token(match.start(), duration.end))
                break

            match = self.config.in_connector_regex.search(before_str)
            if self.__match_regex_in_prefix(before_str, match):
                range_str = source[duration.start:duration.start + duration.length]
                range_match = self.config.range_unit_regex.search(range_str)

                if range_match:
                    tokens.append(Token(match.start(), duration.end))
                break
        return tokens

    # 1. Extract the month of date, week of date to a date range
    # 2. Extract cases like within two weeks from/before today/tomorrow/yesterday

    def single_time_point_with_patterns(self, source: str, ordinal_extractions:
                                        [ExtractResult], reference: datetime) -> List[ExtractResult]:
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
                result.extend(self.__get_token_for_regex_matching(before_string,
                                                                  self.config.week_of_regex, extraction_result))
                result.extend(self.__get_token_for_regex_matching(before_string,
                                                                  self.config.month_of_regex, extraction_result))

                # Cases like "3 days from today", "2 weeks before yesterday", "3 months after tomorrow"
                if self.is_relative_duration_date(extraction_result):
                    result.extend(self.__get_token_for_regex_matching(before_string,
                                                                      self.config.less_than_regex, extraction_result))
                    result.extend(self.__get_token_for_regex_matching(before_string,
                                                                      self.config.more_than_regex, extraction_result))

                    # For "within" case, only duration with relative to "today" or "now" makes sense
                    # Cases like "within 3 days from yesterday/tomorrow" does not make any sense
                    if self.is_date_relative_to_now_or_today(extraction_result):

                        match = self.config.within_next_prefix_regex.search(before_string)
                        if match:

                            is_next = not RegExpUtility.get_group(match, Constants.NEXT_GROUP_NAME)

                            # For "within" case
                            # Cases like "within the next 5 days before today" is not acceptable
                            if not is_next and self.is_ago_relative_duration_date(extraction_result) is not None:
                                result.extend(self.__get_token_for_regex_matching(
                                    before_string,
                                    self.config.within_next_prefix_regex,
                                    extraction_result)
                                )

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
    def __get_token_for_regex_matching(source: str, regexp: Pattern, er: ExtractResult) -> List[Token]:
        tokens = []
        match = regex.search(regexp, source)

        if match and source.strip().endswith(match.group().strip()):
            start_index = source.rfind(match.group())
            tokens.append(Token(start_index, er.start + er.length))

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
    def date_extractor(self) -> DateTimeExtractor:
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


class BaseDatePeriodParser(DateTimeParser):
    @property
    def parser_type_name(self) -> str:
        return Constants.SYS_DATETIME_DATEPERIOD

    week_of_comment = Constants.WEEK_OF
    month_of_comment = Constants.MONTH_OF

    def __init__(self, config: DatePeriodParserConfiguration, inclusive_end_period: bool = False):
        self.config = config
        self._inclusive_end_period = inclusive_end_period

    def parse(self, source: ExtractResult, reference: datetime = None) -> Optional[DateTimeParseResult]:
        if not reference:
            reference = datetime.now()

        if source.type == self.parser_type_name:
            source_text = source.text.strip().lower()
            inner_result = self.__parse_month_with_year(source_text, reference)

            if not inner_result.success:
                inner_result = self._parse_simple_case(source_text, reference)

            if not inner_result.success:
                inner_result = self._parse_one_word_period(
                    source_text, reference)

            if not inner_result.success:
                inner_result = self._merge_two_times_points(
                    source_text, reference)

            if not inner_result.success:
                inner_result = self._parse_year(source_text, reference)

            if not inner_result.success:
                inner_result = self._parse_week_of_month(
                    source_text, reference)

            if not inner_result.success:
                inner_result = self._parse_week_of_year(source_text, reference)

            if not inner_result.success:
                inner_result = self._parse_half_year(source_text, reference)

            if not inner_result.success:
                inner_result = self.__parse_quarter(source_text, reference)

            if not inner_result.success:
                inner_result = self.__parse_season(source_text, reference)

            if not inner_result.success:
                inner_result = self.__parse_which_week(source_text, reference)

            if not inner_result.success:
                inner_result = self.__parse_week_of_date(
                    source_text, reference)

            if not inner_result.success:
                inner_result = self.__parse_month_of_date(
                    source_text, reference)

            if not inner_result.success:
                inner_result = self._parse_duration(source_text, reference)

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
        result.value = result_value
        result.timex_str = result_value.timex if result_value else ''
        result.resolution_str = ''

        return result

    def __parse_month_with_year(self, source: str, reference: datetime) -> DateTimeResolutionResult:
        trimmed_source = source.strip().lower()
        result = DateTimeResolutionResult()
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
        begin_date = DateUtils.safe_create_from_value(
            DateUtils.min_value, year, month, 1)
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
        year = reference.year
        month = reference.month
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

        begin_date_luis = DateTimeFormatUtil.luis_date(
            year if not no_year else -1, month, begin_day)
        end_date_luis = DateTimeFormatUtil.luis_date(
            year if not no_year else -1, month, end_day)
        future_year = year
        past_year = year
        start_date = DateUtils.safe_create_from_value(
            DateUtils.min_value, year, month, begin_day)

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
        year = reference.year
        month = reference.month

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

        future_year = year
        past_year = year
        trimmed_source = source.strip().lower()
        match = self.config.one_word_period_regex.search(trimmed_source)

        if not (match and match.start() == 0 and match.end() == len(trimmed_source)):
            match = self.config.later_early_period_regex.match(trimmed_source)

        if not (match and match.start() == 0 and match.end() == len(trimmed_source)):
            return result

        early_prefix = False
        late_prefix = False
        mid_prefix = False

        if RegExpUtility.get_group(match, Constants.EARLY_PREFIX):
            early_prefix = True
            trimmed_source = match.group(Constants.SUFFIX_GROUP_NAME)
            result.mod = TimeTypeConstants.EARLY_MOD
        elif RegExpUtility.get_group(match, Constants.LATE_PREFIX):
            late_prefix = True
            trimmed_source = match.group(Constants.SUFFIX_GROUP_NAME)
            result.mod = TimeTypeConstants.LATE_MOD
        elif RegExpUtility.get_group(match, Constants.MID_PREFIX):
            mid_prefix = True
            trimmed_source = match.group(Constants.SUFFIX_GROUP_NAME)
            result.mod = TimeTypeConstants.MID_MOD

        swift = 0
        month_str = RegExpUtility.get_group(match, Constants.MONTH_GROUP_NAME)
        if month_str:
            swift = self.config.get_swift_year(trimmed_source)
        else:
            swift = self.config.get_swift_day_or_month(trimmed_source)

        if self.config.unspecific_end_of_range_regex is not None and self.config.unspecific_end_of_range_regex.match(
                match.string):
            late_prefix = True
            trimmed_source = match.string
            result.mod = TimeTypeConstants.LATE_MOD

        if RegExpUtility.get_group(match, Constants.REL_EARLY):
            early_prefix = True
            if self.is_present(swift):
                result.mod = None
        elif RegExpUtility.get_group(match, Constants.REL_LATE):
            late_prefix = True
            if self.is_present(swift):
                result.mod = None

        month_str = RegExpUtility.get_group(match, Constants.MONTH_GROUP_NAME)

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
                thursday = DateUtils.this(
                    reference, DayOfWeek.THURSDAY) + datedelta(days=7 * swift)
                result.timex = f'{thursday.year:04d}-W{DateUtils.week_of_year(thursday):02d}'
                begin_date = DateUtils.this(
                    reference, DayOfWeek.MONDAY) + datedelta(days=7 * swift)
                end_date = DateUtils.this(
                    reference, DayOfWeek.SUNDAY) + datedelta(days=7 * swift)

                if early_prefix:
                    end_date = DateUtils.this(
                        reference, DayOfWeek.WEDNESDAY) + datedelta(days=7 * swift)
                elif mid_prefix:
                    begin_date = DateUtils.this(
                        reference, DayOfWeek.TUESDAY) + datedelta(days=7 * swift)
                    end_date = DateUtils.this(
                        reference, DayOfWeek.FRIDAY) + datedelta(days=7 * swift)
                elif late_prefix:
                    begin_date = DateUtils.this(
                        reference, DayOfWeek.THURSDAY) + datedelta(days=7 * swift)

                if not self._inclusive_end_period:
                    end_date = end_date + datedelta(days=1)

                if early_prefix and swift == 0:
                    if end_date > reference:
                        end_date = reference
                elif late_prefix and swift == 0:
                    if begin_date < reference:
                        begin_date = reference

                result.future_value = [begin_date, end_date]
                result.past_value = [begin_date, end_date]
                result.success = True

                return result

            if self.config.is_weekend(trimmed_source):
                begin_date = DateUtils.this(
                    reference, DayOfWeek.SATURDAY) + datedelta(days=7 * swift)
                end_date = DateUtils.this(
                    reference, DayOfWeek.SUNDAY) + datedelta(days=7 * swift)

                if not self._inclusive_end_period:
                    end_date = end_date + datedelta(days=1)

                result.timex = f'{year:04d}-W{begin_date.isocalendar()[1]:02d}-WE'
                result.future_value = [begin_date, end_date]
                result.past_value = [begin_date, end_date]
                result.success = True

                return result

            if self.config.is_month_only(trimmed_source):
                temp_date = reference + datedelta(months=swift)
                month = temp_date.month
                year = temp_date.year
                result.timex = f'{year:04d}-{month:02d}'
                future_year = year
                past_year = year
            elif self.config.is_year_only(trimmed_source):
                temp_date = reference + datedelta(years=swift)
                year = temp_date.year

                if late_prefix:
                    begin_date = DateUtils.safe_create_from_min_value(
                        year, 7, 1)
                else:
                    begin_date = DateUtils.safe_create_from_min_value(
                        year, 1, 1)

                if early_prefix:
                    end_date = DateUtils.safe_create_from_min_value(
                        year, 6, 30)
                else:
                    end_date = DateUtils.safe_create_from_min_value(
                        year, 12, 31)

                if not self._inclusive_end_period:
                    end_date = end_date + datedelta(days=1)

                result.timex = f'{year:04d}'
                result.future_value = [begin_date, end_date]
                result.past_value = [begin_date, end_date]
                result.success = True

                return result

        future_start = DateUtils.safe_create_from_min_value(
            future_year, month, 1)
        future_end = DateUtils.safe_create_from_min_value(
            future_year, month, 1) + datedelta(months=1)
        past_start = DateUtils.safe_create_from_min_value(past_year, month, 1)
        past_end = DateUtils.safe_create_from_min_value(
            past_year, month, 1) + datedelta(months=1)

        if self._inclusive_end_period:
            future_end = future_end + datedelta(days=-1)
            past_end = past_end + datedelta(days=-1)

        if early_prefix:
            future_end = DateUtils.safe_create_from_min_value(
                future_year, month, 15)
            past_end = DateUtils.safe_create_from_min_value(
                past_year, month, 15)

            if not self._inclusive_end_period:
                future_end = future_end + datedelta(days=1)
                past_end = past_end + datedelta(days=1)
        elif late_prefix:
            future_start = DateUtils.safe_create_from_min_value(
                future_year, month, 16)
            past_start = DateUtils.safe_create_from_min_value(
                past_year, month, 16)

        result.future_value = [future_start, future_end]
        result.past_value = [past_start, past_end]
        result.success = True

        return result

    # Parse entities that are made up by two time points
    def _merge_two_times_points(self, source: str, reference: datetime) -> DateTimeResolutionResult:
        trimmed_source = source.strip()
        result = DateTimeResolutionResult()
        ers = self.config.date_extractor.extract(trimmed_source, reference)
        prs = []

        if not ers or len(ers) < 2:
            ers = self.config.date_extractor.extract(
                self.config.token_before_date + trimmed_source, reference)

            if len(ers) >= 2:
                for er in ers:
                    er.start -= len(self.config.token_before_date)
            else:
                now_pr = self._parse_now_as_date(source, reference)
                if now_pr.value is None or now_pr.start is None or len(ers) < 1:
                    return result

                date_pr = self.config.date_parser.parse(ers[0], reference)
                prs.append(now_pr)
                prs.append(date_pr)
                prs = sorted(prs, key=lambda x: x.start)

        if len(ers) >= 2:

            # Propagate the possible future relative context from the first entity to the second one in the range.
            # Handles cases like "next monday to friday"
            match = self.config.week_with_week_day_range_regex.search(source)
            if match:
                week_prefix = RegExpUtility.get_group(match, Constants.WEEK_GROUP_NAME)

                if week_prefix:
                    ers[0].text = f'{week_prefix} {ers[0].text}'
                    ers[1].text = f'{week_prefix} {ers[1].text}'

            for er in ers:
                pr = self.config.date_parser.parse(er, reference)
                if pr:
                    prs.append(pr)

        if len(prs) < 2:
            return result

        pr_begin = prs[0]
        pr_end = prs[1]
        future_begin = pr_begin.value.future_value
        future_end = pr_end.value.future_value
        past_begin = pr_begin.value.past_value
        past_end = pr_end.value.past_value

        if future_begin > future_end:
            future_begin = past_begin

        if past_end < past_begin:
            past_end = future_end

        result.sub_date_time_entities = prs
        result.timex = f'({pr_begin.timex_str},{pr_end.timex_str},P{(future_end - future_begin).days}D)'
        result.future_value = [future_begin, future_end]
        result.past_value = [past_begin, past_end]
        result.success = True

        return result

    # Handle "between...and..." when contains with "now"
    def _parse_now_as_date(self, source: str, reference: datetime) -> DateTimeParseResult:
        pr = DateTimeParseResult()
        match = self.config.now_regex.search(source)
        if match is not None:
            value = DateUtils.safe_create_from_min_value(
                reference.year, reference.month, reference.day)
            ret_now = DateTimeResolutionResult()
            ret_now.timex = DateTimeFormatUtil.luis_date_from_datetime(
                reference)
            ret_now.future_value = value
            ret_now.past_value = value
            pr.text = match.string
            pr.start = match.start()
            pr.length = match.end() - match.start()
            pr.value = ret_now
            pr.type = Constants.SYS_DATETIME_DATE
            pr.timex_str = ret_now.timex
        return pr

    def _parse_year(self, source: str, reference: datetime) -> DateTimeResolutionResult:
        trimmed_source = source.strip()
        result = DateTimeResolutionResult()
        match = self.config.year_regex.search(trimmed_source)

        if not (match and len(match.group()) == len(trimmed_source)):
            return result

        year = int(match.group())
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

    # To be consistency, we follow the definition of "week of year":
    # "first week of the month" - it has the month's first Thursday in it
    # "last week of the month" - it has the month's last Thursday in it
    def _parse_week_of_month(self, source: str, reference: datetime) -> DateTimeResolutionResult:
        result = DateTimeResolutionResult()
        match = self.config.week_of_month_regex.search(source)

        if not (match and len(match.group()) == len(source)):
            return result

        cardinal_str = RegExpUtility.get_group(match, Constants.CARDINAL)
        month_str = RegExpUtility.get_group(match, Constants.MONTH_GROUP_NAME)
        month = reference.month
        year = reference.year
        no_year = False
        cardinal = 5 if self.config.is_last_cardinal(
            cardinal_str) else self.config.cardinal_map.get(cardinal_str)

        if not month_str:
            swift = self.config.get_swift_day_or_month(source)
            temp_data = reference + datedelta(months=swift)
            month = temp_data.month
            year = temp_data.year
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

        future_date = seed_date
        past_date = seed_date

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
        result.timex = (
            'XXXX' if no_year else f'{year:04d}') + f'-{month:02d}-W{cardinal:02d}'
        days_to_add = 6 if self._inclusive_end_period else 7
        result.future_value = [future_date,
                               future_date + datedelta(days=days_to_add)]
        result.past_value = [past_date,
                             past_date + datedelta(days=days_to_add)]
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
                first_day_week_monday = DateUtils.this(
                    first_day + datedelta(days=7), DayOfWeek.MONDAY)

            target_week_monday = first_day_week_monday + datedelta(days=7 * (cardinal - 1))
            result.timex = f'{year:04d}-W{week_num:02d}'

        days_to_add = 6 if self._inclusive_end_period else 7
        result.future_value = [target_week_monday,
                               target_week_monday + datedelta(days=days_to_add)]
        result.past_value = [target_week_monday,
                             target_week_monday + datedelta(days=days_to_add)]
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

        if number_str:
            quarter_num = int(number_str)
        else:
            quarter_num = self.config.cardinal_map[cardinal_str]

        begin_date = DateUtils.safe_create_date_resolve_overflow(
            year, ((quarter_num - 1) * Constants.SEMESTER_MONTH_COUNT) + 1, 1)
        end_date = DateUtils.safe_create_date_resolve_overflow(
            year, (quarter_num * Constants.SEMESTER_MONTH_COUNT) + 1, 1)

        result.future_value = [begin_date, end_date]
        result.past_value = [begin_date, end_date]
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
        ers = self.config.duration_extractor.extract(source, reference)
        begin_date = reference
        end_date = reference
        rest_now_sunday = False
        duration_timex = ''
        mod = ''

        if len(ers) == 1:
            pr = self.config.duration_parser.parse(ers[0])

            if pr is None:
                return result

            before_str = source[0:pr.start].strip()
            duration_result = pr.value

            if not duration_result.timex:
                return result

            prefix_match = self.config.past_regex.search(before_str)

            if prefix_match:
                mod = TimeTypeConstants.BEFORE_MOD
                begin_date = self.__get_swift_date(
                    end_date, duration_result.timex, False)

            prefix_match = self.config.future_regex.search(before_str)

            if prefix_match and len(prefix_match.string) == len(before_str):
                mod = TimeTypeConstants.AFTER_MOD
                begin_date = reference + timedelta(days=1)
                end_date = self.__get_swift_date(
                    begin_date, duration_result.timex, True)

            prefix_match = self.config.in_connector_regex.search(before_str)

            if prefix_match and len(prefix_match.string) == len(before_str):
                mod = TimeTypeConstants.AFTER_MOD
                begin_date = reference + timedelta(days=1)
                end_date = self.__get_swift_date(
                    begin_date, duration_result.timex, True)
                unit = duration_result.timex[len(duration_result.timex) - 1]
                duration_result.timex = f'P1{unit}'
                begin_date = self.__get_swift_date(
                    end_date, duration_result.timex, False)

            if mod:
                pr.value.mod = mod

            duration_timex = duration_result.timex
            result.sub_date_time_entities = [pr]

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
                end_date = DateUtils.safe_create_from_min_value(
                    begin_date.year, 12, 31)
                diff_days = DateUtils.day_of_year(
                    end_date) - DateUtils.day_of_year(begin_date) + 1
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

        first_day = DateUtils.safe_create_from_value(
            DateUtils.min_value, year, 1, 1)
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

        date_resolution = self.config.date_parser.parse(
            ers[0], reference).value
        result.timex = date_resolution.timex
        result.comment = BaseDatePeriodParser.week_of_comment
        result.future_value = self.__get_week_range_from_date(
            date_resolution.future_value)
        result.past_value = self.__get_week_range_from_date(
            date_resolution.past_value)
        result.success = True
        return result

    def __parse_month_of_date(self, source: str, reference: datetime) -> DateTimeResolutionResult:
        result = DateTimeResolutionResult()
        match = self.config.month_of_regex.search(source)
        ers = self.config.date_extractor.extract(source, reference)

        if not (match and len(ers) == 1):
            return result

        date_resolution = self.config.date_parser.parse(
            ers[0], reference).value
        result.timex = date_resolution.timex
        result.comment = BaseDatePeriodParser.week_of_comment
        result.future_value = self.__get_month_range_from_date(
            date_resolution.future_value)
        result.past_value = self.__get_month_range_from_date(
            date_resolution.past_value)
        result.success = True
        return result

    def __get_week_range_from_date(self, seed_date: datetime) -> List[datetime]:
        begin_date = DateUtils.this(seed_date, DayOfWeek.MONDAY)
        end_date = begin_date + timedelta(days=6 if self._inclusive_end_period else 7)
        return [begin_date, end_date]

    @staticmethod
    def __get_month_range_from_date(seed_date: datetime) -> List[datetime]:
        begin_date = DateUtils.safe_create_from_value(
            DateUtils.min_value, seed_date.year, seed_date.month, 1)
        end_date = DateUtils.safe_create_from_value(
            DateUtils.min_value, seed_date.year, seed_date.month + 1, 1)
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
