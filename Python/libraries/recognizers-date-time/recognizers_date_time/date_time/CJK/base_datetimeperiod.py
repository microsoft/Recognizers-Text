from abc import abstractmethod
from datetime import datetime, timedelta
from collections import namedtuple
from typing import List, Pattern, Dict, Match

import regex

from recognizers_text.extractor import Extractor, ExtractResult
from recognizers_text.parser import Parser
from recognizers_date_time.date_time.constants import Constants
from recognizers_date_time.date_time.extractors import DateTimeExtractor
from recognizers_date_time.date_time.parsers import DateTimeParser
from recognizers_date_time.date_time.utilities import DateTimeOptionsConfiguration, Token, merge_all_tokens, \
    ExtractResultExtension, RegExpUtility, DateTimeParseResult, DateTimeResolutionResult, \
    TimexUtil, DateUtils, DateTimeFormatUtil, TimeTypeConstants

MatchedIndex = namedtuple('MatchedIndex', ['matched', 'index'])
MatchedTimeRegex = namedtuple(
    'MatchedTimeRegex', ['matched', 'timex', 'begin_hour', 'end_hour', 'end_min'])
MatchedTimeRegexAndSwift = namedtuple(
    'MatchedTimeRegex', ['matched', 'timex', 'begin_hour', 'end_hour', 'end_min', 'swift'])
BeginEnd = namedtuple('BeginEnd', ['begin', 'end'])


class CJKDateTimePeriodExtractorConfiguration(DateTimeOptionsConfiguration):
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
    def followed_unit(self) -> Pattern:
        raise NotImplementedError

    @property
    @abstractmethod
    def unit_regex(self) -> Pattern:
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
    def time_period_left_regex(self) -> Pattern:
        raise NotImplementedError

    @property
    @abstractmethod
    def relative_regex(self) -> Pattern:
        raise NotImplementedError

    @property
    @abstractmethod
    def rest_of_date_regex(self) -> Pattern:
        raise NotImplementedError

    @property
    @abstractmethod
    def am_pm_desc_regex(self) -> Pattern:
        raise NotImplementedError

    @property
    @abstractmethod
    def this_regex(self) -> Pattern:
        raise NotImplementedError

    @property
    @abstractmethod
    def before_after_regex(self) -> Pattern:
        raise NotImplementedError

    @property
    @abstractmethod
    def cardinal_extractor(self) -> Extractor:
        raise NotImplementedError

    @property
    @abstractmethod
    def single_date_extractor(self) -> DateTimeExtractor:
        raise NotImplementedError

    @property
    @abstractmethod
    def single_time_extractor(self) -> DateTimeExtractor:
        raise NotImplementedError

    @property
    @abstractmethod
    def single_date_time_extractor(self) -> DateTimeExtractor:
        raise NotImplementedError

    @property
    @abstractmethod
    def duration_extractor(self) -> DateTimeExtractor:
        raise NotImplementedError

    @property
    @abstractmethod
    def time_period_extractor(self) -> DateTimeExtractor:
        raise NotImplementedError

    @abstractmethod
    def get_from_token_index(self, text: str) -> MatchedIndex:
        raise NotImplementedError

    @abstractmethod
    def has_connector_token(self, text: str) -> bool:
        raise NotImplementedError

    @abstractmethod
    def get_between_token_index(self, text: str) -> MatchedIndex:
        raise NotImplementedError


class BaseCJKDateTimePeriodExtractor(DateTimeExtractor):
    @property
    def extractor_type_name(self) -> str:
        return Constants.SYS_DATETIME_DATETIME

    def __init__(self, config: CJKDateTimePeriodExtractorConfiguration):
        self.config = config

    def extract(self, text: str, reference_time: datetime = None) -> List[ExtractResult]:
        if reference_time is None:
            reference_time = datetime.now()
        # Date and time Extractions should be extracted from the text only once,
        # and shared in the methods below, passed by value

        date_ers = self.config.single_date_extractor.extract(text, reference_time)
        time_ers = self.config.single_time_extractor.extract(text, reference_time)
        time_range_ers = self.config.time_period_extractor.extract(text, reference_time)
        date_time_ers = self.config.single_date_time_extractor.extract(text, reference_time)

        tokens: List[Token] = list()

        tokens.extend(self.merge_date_and_time_period(text, date_ers, time_range_ers))
        tokens.extend(self.merge_two_time_points(text, date_time_ers, time_ers))
        # tokens.extend(self.match_duration(text, reference_time))
        tokens.extend(self.match_relative_unit(text))
        tokens.extend(self.match_date_with_period_suffix(text, date_ers))
        tokens.extend(self.match_number_with_unit(text))
        tokens.extend(self.match_night(text, reference_time))
        tokens.extend(self.merge_date_with_time_period_suffix(text, date_ers, time_ers))

        return merge_all_tokens(tokens, text, self.extractor_type_name)

    # merge Date and Time period
    def merge_date_and_time_period(self, text: str, data_ers: List[ExtractResult], time_range_ers: List[ExtractResult]) \
            -> List[Token]:
        ret: List[Token] = list()
        time_points: List[ExtractResult] = list()

        # handle the overlap problem
        j = 0
        for data_er in data_ers:
            time_points.append(data_er)
            while j < len(time_range_ers) and time_range_ers[j].start + time_range_ers[j].length <= data_er.start:
                time_points.append(time_range_ers[j])
                j += 1

            while j < len(time_range_ers) and ExtractResultExtension.is_overlap(time_range_ers[j], data_er):
                j += 1

        while j < len(time_range_ers):
            time_points.append(time_range_ers[j])
            j += 1

        time_points = list(sorted(time_points, key=lambda x: x.start))

        # merge {Date} {TimePeriod}
        idx = 0
        while idx < len(time_points) - 1:
            if time_points[idx].type == Constants.SYS_DATETIME_DATE and \
                    time_points[idx + 1].type == Constants.SYS_DATETIME_TIMEPERIOD:
                middle_begin = time_points[idx].start + time_points[idx].length
                middle_end = time_points[idx + 1].start

                middle_str = text[middle_begin: middle_end - middle_begin].strip()
                preposition_regex_match = regex.match(self.config.preposition_regex, middle_str)
                if middle_str or preposition_regex_match:
                    period_begin = time_points[idx].start
                    period_end = time_points[idx + 1].start + time_points[idx + 1].length
                    ret.append(Token(period_begin, period_end))
                    idx += 2
                    continue
                idx += 1
            idx += 1
        return ret

    def merge_two_time_points(self, text: str, date_time_ers: List[ExtractResult], time_ers: List[ExtractResult]) \
            -> List[Token]:

        ret: List[Token] = list()
        time_points: List[ExtractResult] = list()

        # handle the overlap problem
        j = 0
        for date_time_er in date_time_ers:
            time_points.append(date_time_er)
            while j < len(time_ers) and time_ers[j].start + time_ers[j].length <= date_time_er.start:
                time_points.append(time_ers[j])
                j += 1
            while j < len(time_ers) and ExtractResultExtension.is_overlap(time_ers[j], date_time_er):
                j += 1

        while j < len(time_ers):
            time_points.append(time_ers[j])

        time_points = list(sorted(time_points, key=lambda x: x.start))

        # merge "{TimePoint} to {TimePoint}", "between {TimePoint} and {TimePoint}"
        idx = 0
        while idx < len(time_points) - 1:
            # if both ends are Time. then this is a TimePeriod, not a DateTimePeriod
            if time_points[idx].type == Constants.SYS_DATETIME_TIME \
                    and time_points[idx + 1].type == Constants.SYS_DATETIME_TIME:
                idx += 1
                continue

            middle_begin = time_points[idx].start + time_points[idx].length
            middle_end = time_points[idx + 1].start

            middle_str = text[middle_begin: middle_end - middle_begin].strip()

            # handle "{TimePoint} to {TimePoint}"
            if RegExpUtility.is_exact_match(self.config.till_regex, middle_str, True):
                period_begin = time_points[idx].start
                period_end = time_points[idx + 1].start + time_points[idx + 1].length

                # handle "from"
                before_str = time_points[0:period_begin]
                match_from = self.config.get_from_token_index(before_str)
                from_token_index = match_from if match_from.matched else self.config.get_between_token_index(before_str)

                if from_token_index.matched:
                    period_begin = from_token_index.index
                else:
                    after_str = text[period_end:]
                    after_token_index = self.config.get_from_token_index(after_str)
                    if after_token_index.matched:
                        period_end += after_token_index.index

                ret.append(Token(period_begin, period_end))
                idx += 2
                continue

            # handle "between {TimePoint} and {TimePoint}"
            if self.config.has_connector_token(middle_str):
                period_begin = time_points[idx].start
                period_end = time_points[idx + 1].start + time_points[idx + 1].length

                # handle "between"
                after_str = text[period_end:]
                after_token_between_index = self.config.get_between_token_index(after_str)
                if after_token_between_index.matched:
                    ret.append(Token(period_begin, period_end + after_token_between_index.index))
                    idx += 2
                    continue

            idx += 1

        return ret

    def match_night(self, text: str, reference_time: datetime) -> List[Token]:
        ret: List[Token] = list()

        matches = regex.finditer(self.config.specific_time_of_day_regex, text)
        ret.extend(map(lambda x: Token(x.start(), x.end()), matches))

        # Date followed by morning, afternoon
        ers = self.config.single_date_extractor.extract(text, reference_time)
        if len(ers) == 0:
            return ret

        for er in ers:
            after_str = text[er.start + er.length]
            match = regex.match(self.config.time_of_day_regex, after_str)
            if match:
                middle_str = after_str[0:match.start()]
                if not middle_str.strip() or regex.search(self.config.preposition_regex, middle_str):
                    ret.append(Token(er.start, er.start + er.length + match.start() + match.end()))
        return ret

    # Cases like "2015年1月1日の2時以降", "On January 1, 2015 after 2:00"
    def merge_date_with_time_period_suffix(self, text: str, date_ers: List[ExtractResult],
                                           time_ers: List[ExtractResult]) -> List[Token]:
        ret: List[Token] = list()

        if not any(date_ers):
            return ret
        if not any(time_ers):
            return ret

        ers: [Match] = date_ers
        ers.extend(time_ers)

        ers = sorted(ers, key=lambda x: x.start)

        i = 0
        while i < len(ers) - 1:
            j = i + 1
            while j < len(ers) and ExtractResultExtension.is_overlap(ers[i], ers[j]):
                j += 1
            if j >= len(ers):
                break

            if ers[i].type == Constants.SYS_DATETIME_DATE and ers[j].type == Constants.SYS_DATETIME_TIME:
                middle_begin = ers[i].start + (ers[i].length or 0)
                middle_end = ers[j].start or 0

                if middle_begin > middle_end:
                    i = j + 1
                    continue

                middle_str = text[middle_begin: middle_end - middle_begin].strip()

                match = regex.match(self.config.before_after_regex, middle_str)
                if match:
                    begin = ers[i].start or 0
                    end = (ers[j].start or 0) + (ers[j].length or 0)
                    ret.append(Token(begin, end))

                i = j + 1
                continue
            i = j
        return ret

    # Extract patterns that involve durations e.g. "Within 5 hours from now"
    def match_duration(self, text: str, reference: datetime) -> List[Token]:
        ret: List[Token] = list()
        duration_extractions: List[ExtractResult] = self.config.duration_extractor.extract(text, reference)

        for duration_extraction in duration_extractions:
            if not regex.search(self.config.unit_regex, duration_extraction.text):
                continue
            duration = Token(duration_extraction.start or 0,
                             (duration_extraction.start + duration_extraction.length or 0))
            before_str = text[0:duration.start]
            after_str = text[duration.start + duration.length:].strip()

            if not before_str and not after_str:
                continue

            start_out = -1
            end_out = -1
            match = regex.match(self.config.future_regex, after_str)

            in_prefix_match = regex.match(self.config.this_regex, before_str)
            in_prefix = True if in_prefix_match else False

            if RegExpUtility.get_group(match, Constants.WITHIN_GROUP_NAME):
                start_token = in_prefix_match.start() if in_prefix else duration.start
                within_length = len(RegExpUtility.get_group(match, Constants.WITHIN_GROUP_NAME))
                end_token = duration.end + (0 if in_prefix else match.start() + match.end())

                match = regex.match(self.config.unit_regex, text[duration.start:duration.length])

                if match:
                    start_out = start_token
                    end_out = end_token + within_length if in_prefix else end_token

                ret.append(Token(start_out, end_out))

        return ret

    def match_relative_unit(self, text: str) -> List[Token]:
        ret: List[Token] = list()
        matches = list(regex.finditer(self.config.rest_of_date_regex, text))
        ret.extend(map(lambda x: Token(x.start(), x.start() + x.end()), matches))
        return ret

    # For cases like "Early in the day Wednesday"
    def match_date_with_period_suffix(self, text: str, date_ers: List[ExtractResult]) -> List[Token]:
        ret: List[Token] = list()

        for date_er in date_ers:
            date_str_end = int(date_er.start + date_er.length)
            after_str = text[date_str_end: len(text) - date_str_end]
            match_after = RegExpUtility.match_begin(self.config.time_period_left_regex, after_str, True)
            if match_after.success:
                ret.append(Token(int(date_er.start), date_str_end + match_after.index + match_after.length))

        return ret

    def match_number_with_unit(self, text: str) -> List[Token]:
        ret: List[Token] = list()
        durations: List[Token] = list()

        ers = self.config.cardinal_extractor.extract(text)

        for er in ers:
            after_str = text[(er.start + er.length) or 0:]
            followed_unit_match = RegExpUtility.match_begin(self.config.followed_unit, after_str, True)

            if followed_unit_match.success:
                durations.append(Token(er.start or 0, (er.start + er.length) or 0 +
                                       len(followed_unit_match.group())))

            past_regex_match = RegExpUtility.match_begin(self.config.past_regex, after_str, True)
            if past_regex_match.success:
                durations.append(Token(er.start or 0, (er.start + er.length) or 0 + len(past_regex_match.group())))

        for match in RegExpUtility.get_matches(self.config.unit_regex, text):
            durations.append(Token(match.start(), match.start() + match.end()))

        for duration in durations:
            before_str = text[0:duration.start]
            if not before_str.strip():
                continue

            past_regex_match = RegExpUtility.match_end(self.config.past_regex, before_str, True)
            if past_regex_match.success:
                ret.append(Token(match.start(), duration.end))
                continue

            future_regex_match = RegExpUtility.match_end(self.config.future_regex, before_str, True)
            if future_regex_match.success:
                ret.append(Token(match.start(), duration.end))

            time_period_left_regex_match = RegExpUtility.match_end(self.config.time_period_left_regex, before_str, True)
            if time_period_left_regex_match.success:
                ret.append(Token(match.start(), duration.end))

        return ret


class CJKDateTimePeriodParserConfiguration(DateTimeOptionsConfiguration):
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
    def duration_parser(self) -> DateTimeExtractor:
        raise NotImplementedError

    @property
    @abstractmethod
    def cardinal_extractor(self) -> Extractor:
        raise NotImplementedError

    @property
    @abstractmethod
    def cardinal_parser(self) -> Parser:
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
    def specific_time_of_day_regex(self) -> Pattern:
        raise NotImplementedError

    @property
    @abstractmethod
    def time_of_day_regex(self) -> Pattern:
        raise NotImplementedError

    @property
    @abstractmethod
    def next_regex(self) -> Pattern:
        raise NotImplementedError

    @property
    @abstractmethod
    def last_regex(self) -> Pattern:
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
    def weekday_regex(self) -> Pattern:
        raise NotImplementedError

    @property
    @abstractmethod
    def time_period_left_regex(self) -> Pattern:
        raise NotImplementedError

    @property
    @abstractmethod
    def unit_regex(self) -> Pattern:
        raise NotImplementedError

    @property
    @abstractmethod
    def rest_of_date_regex(self) -> Pattern:
        raise NotImplementedError

    @property
    @abstractmethod
    def am_pm_desc_regex(self) -> Pattern:
        raise NotImplementedError

    @property
    @abstractmethod
    def unit_map(self) -> Dict[str, str]:
        raise NotImplementedError

    @abstractmethod
    def get_matched_time_range(self, text: str) -> MatchedTimeRegex:
        raise NotImplementedError

    @abstractmethod
    def get_matched_time_range_and_swift(self, text: str) -> MatchedTimeRegexAndSwift:
        raise NotImplementedError


class BaseCJKDateTimePeriodParser(DateTimeParser):
    @property
    def parser_type_name(self) -> str:
        return Constants.SYS_DATETIME_DATETIME

    def __init__(self, config: CJKDateTimePeriodParserConfiguration):
        self.config = config

    def parse(self, source: ExtractResult, reference: datetime = None) -> DateTimeParseResult:
        reference_time = reference if reference is not None else datetime.now()
        value = None

        if source.type == self.parser_type_name:
            inner_result = self.merge_date_and_time_period(source.text, reference_time)

            if not inner_result.success:
                inner_result = self.merge_two_time_points(source.text, reference_time)

            # if not inner_result.success:
            #     inner_result = self.parse_duration(source.text, reference_time)

            if not inner_result.success:
                inner_result = self.parse_specific_night(source.text, reference_time)

            if not inner_result.success:
                inner_result = self.parse_number_with_unit(source.text, reference_time)

            if not inner_result.success:
                inner_result = self.parse_relative_unit(source.text, reference_time)

            if not inner_result.success:
                inner_result = self.parse_date_with_period_suffix(source.text, reference_time)

            if not inner_result.success:
                inner_result = self.parse_date_with_time_period_suffix(source.text, reference_time)

            if inner_result.success:
                if inner_result.mod == Constants.BEFORE_MOD:
                    # Cases like "last tuesday by 2:00 pm" there is no StartTime
                    inner_result.future_resolution = {
                        TimeTypeConstants.END_DATETIME: DateTimeFormatUtil
                        .format_date_time(datetime(inner_result.future_value))
                    }
                    inner_result.past_resolution = {
                        TimeTypeConstants.END_DATETIME: DateTimeFormatUtil
                        .format_date_time(datetime(inner_result.past_value))
                    }
                else:
                    inner_result.future_resolution = {
                        TimeTypeConstants.START_DATETIME: DateTimeFormatUtil.
                        format_date_time(datetime(inner_result.future_value))[0],
                        TimeTypeConstants.END_DATETIME: DateTimeFormatUtil.
                        format_date_time(datetime(inner_result.future_value))[1]
                    }

                    inner_result.past_resolution = {
                        TimeTypeConstants.START_DATETIME: DateTimeFormatUtil.
                        format_date_time(datetime(inner_result.past_value))[0],
                        TimeTypeConstants.END_DATETIME: DateTimeFormatUtil.
                        format_date_time(datetime(inner_result.past_value))[1]
                    }

                value = inner_result

        ret = DateTimeResolutionResult(
            text=source.text,
            start=source.start,
            length=source.length,
            type=source.type,
            date=source.data,
            value=value,
            timex_str='' if not value else value.timex,
            resolution_str=''
        )
        return ret

    def filter_result(self, query: str, candidare_results: List[DateTimeParseResult]) -> List[DateTimeParseResult]:
        return candidare_results

    def merge_date_and_time_period(self, text: str, reference_time: datetime) -> DateTimeResolutionResult:
        ret = DateTimeResolutionResult()

        er1 = self.config.date_extractor.extract(text, reference_time)
        er2 = self.config.time_period_extractor.extract(text, reference_time)

        if len(er1) != 1 or len(er2) != 1:
            return ret

        pr1 = self.config.date_parser.parse(er1[0], reference_time)
        pr2 = self.config.time_period_parser.parse(er2[0], reference_time)
        time_range = tuple(pr2.value.future_value)
        begin_time = time_range[0]
        end_time = time_range[1]
        future_date = pr1.value.future_value
        past_date = pr1.value.past_value

        # handle cases with time like 25時 which resolve to the next day
        swift_day = 0
        timex_hours = TimexUtil.parse_hours_from_time_period_timex(pr2.timex_str)
        ampm_desc_regex_match = regex.match(self.config.am_pm_desc_regex, text)

        if ampm_desc_regex_match and timex_hours[0] < Constants.HALF_DAY_HOUR_COUNT \
                and timex_hours[1] < Constants.HALF_DAY_HOUR_COUNT:
            ret.comment = Constants.COMMENT_AMPM

        if timex_hours[0] > Constants.DAY_HOUR_COUNT:
            past_date = past_date + timedelta(days=1)
            future_date = future_date + timedelta(days=1)
        elif timex_hours[1] > Constants.DAY_HOUR_COUNT:
            swift_day += 1

        past_date_alt = past_date + timedelta(days=swift_day)
        future_date_alt = future_date + timedelta(days=swift_day)

        ret.future_value = (
            DateUtils.safe_create_from_min_value(
                future_date.year, future_date.month, future_date.day,
                begin_time.hour, begin_time.minute, begin_time.second),
            DateUtils.safe_create_from_min_value(
                future_date_alt.year, future_date_alt.month, future_date_alt.day,
                end_time.hour, end_time.minute, end_time.second)
        )

        ret.past_value = (
            DateUtils.safe_create_from_min_value(
                past_date.year, past_date.month, past_date.day,
                begin_time.hour, begin_time.minute, begin_time.second),
            DateUtils.safe_create_from_min_value(
                past_date_alt.year, past_date_alt.month, past_date_alt.day,
                end_time.hour, end_time.minute, end_time.second)
        )

        ret.timex = TimexUtil.generate_split_date_time_period_timex(pr1.timex_str, pr2.timex_str)
        ret.success = True if ret.timex else False

        return ret

    # Cases like "last tuesday by 2:00pm"
    def parse_date_with_time_period_suffix(self, text: str, reference_time: datetime):
        ret = DateTimeResolutionResult()

        date_er = self.config.date_extractor.extract(text, reference_time)
        time_er = self.config.time_extractor.extract(text, reference_time)

        if len(date_er) > 0 and len(time_er) > 0:
            match = RegExpUtility.match_end(self.config.past_regex, text, True)

            if RegExpUtility.get_group(match, Constants.BEFORE_MOD):
                ret.mod = Constants.BEFORE_MOD

            date_pr = self.config.date_parser.parse(date_er[0], reference_time)
            time_pr = self.config.time_parser.parse(time_er[0], reference_time)

            if date_pr and time_pr:
                time_resolution_result = time_pr.value
                date_resolution_result = date_pr.value
                future_date_value = date_resolution_result.future_value
                past_date_value = date_resolution_result.past_value
                future_time_value = time_resolution_result.future_value
                past_time_value: datetime = time_resolution_result.past_value

                ret.comment = time_resolution_result.comment
                ret.timex = TimexUtil.combine_date_and_time_timex(date_pr.timex_str, time_pr.timex_str)

                ret.future_value = DateUtils.safe_create_from_value(future_date_value.year,
                                                                    future_date_value.month,
                                                                    future_date_value.day,
                                                                    future_date_value.hour,
                                                                    future_date_value.minute,
                                                                    future_date_value.second)
                ret.past_value = DateUtils.safe_create_from_value(past_date_value.year,
                                                                  past_date_value.month,
                                                                  past_date_value.day,
                                                                  past_date_value.hour,
                                                                  past_date_value.minute,
                                                                  past_date_value.second)

                ret.sub_date_time_entities = [date_pr, time_pr]
                ret.success = True

        return ret

    def merge_two_time_points(self, text: str, reference_time: datetime) -> DateTimeResolutionResult:
        ret = DateTimeResolutionResult()
        pr1: DateTimeParseResult = None
        pr2: DateTimeParseResult = None

        both_have_dates: bool = False
        begin_has_date: bool = False
        end_has_date: bool = False

        er1 = self.config.time_extractor.extract(text, reference_time)
        er2 = self.config.date_time_extractor.extract(text, reference_time)

        right_time = DateUtils.safe_create_from_value(reference_time.year,
                                                      reference_time.month,
                                                      reference_time.day)
        left_time = DateUtils.safe_create_from_value(reference_time.year,
                                                     reference_time.month,
                                                     reference_time.day)

        match = regex.match(self.config.future_regex, text)

        # cases including 'within' are processed in ParseDuration
        # if RegExpUtility.get_group(match, Constants.WITHIN_GROUP_NAME):
        #     return self.parse_duration(text, reference_time)

        match_weekday = regex.match(self.config.weekday_regex, text)

        if match_weekday:
            return ret

        if len(er2) == 2:
            pr1 = self.config.date_time_parser.parse(er2[0], reference_time)
            pr2 = self.config.date_time_parser.parse(er2[1], reference_time)
            both_have_dates = True

        elif len(er2) == 1 and len(er1) == 2:
            if not ExtractResultExtension.is_overlap(er2[0], er1[1]):
                pr1 = self.config.time_parser.parse(er1[0], reference_time)
                pr2 = self.config.date_time_parser.parse(er2[0], reference_time)
                end_has_date = True
            else:
                pr1 = self.config.date_time_parser.parse(er2[0], reference_time)
                pr2 = self.config.time_parser.parse(er1[0], reference_time)
                begin_has_date = True

        elif len(er2) == 1 and len(er1) == 1:
            if er1[0].start < er2[0].start:
                pr1 = self.config.time_parser.parse(er1[0], reference_time)
                pr2 = self.config.date_time_parser.parse(er2[0], reference_time)
                end_has_date = True
            else:
                pr1 = self.config.date_time_parser.parse(er2[0], reference_time)
                pr2 = self.config.time_parser.parse(er1[0], reference_time)
                begin_has_date = True

        elif len(er1) == 2:
            # if both ends are Time. then this is a TimePeriod, not a DateTimePeriod
            return ret

        else:
            return ret

        if not pr1.value or not pr2.value:
            return ret

        future_begin: datetime = pr1.value.future_value
        future_end: datetime = pr2.value.future_value

        past_begin = pr1.value.past_value

        if future_begin > future_end:
            future_begin = past_begin

        if both_have_dates:
            right_time = DateUtils.safe_create_from_value(future_end.year,
                                                          future_end.month,
                                                          future_end.day)
            left_time = DateUtils.safe_create_from_value(future_begin.year,
                                                         future_begin.month,
                                                         future_begin.day)
        elif begin_has_date:
            left_time = DateUtils.safe_create_from_value(future_begin.year,
                                                         future_begin.month,
                                                         future_begin.day)
        elif end_has_date:
            right_time = DateUtils.safe_create_from_value(future_end.year,
                                                          future_end.month,
                                                          future_end.day)
        left_result: DateTimeResolutionResult = pr1.value
        right_result: DateTimeResolutionResult = pr2.value
        left_result_time: datetime = left_result.future_value
        right_result_time: datetime = right_result.future_value

        # check if the right time is smaller than the left time, if yes, add one day
        hour = left_result_time.hour if left_result_time.hour > 0 else 0
        minute = left_result_time.minute if left_result_time.minute > 0 else 0
        second = left_result_time.second if left_result_time.second > 0 else 0

        left_time = left_time + timedelta(hours=hour, minutes=minute, seconds=second)

        hour = right_result_time.hour if right_result_time.hour > 0 else 0
        minute = right_result_time.minute if right_result_time.minute > 0 else 0
        second = right_result_time.second if right_result_time.second > 0 else 0

        right_time = right_time + timedelta(hours=hour, minutes=minute, seconds=second)

        # the right side time contains "ampm", while the left side doesn't
        if right_result.comment == Constants.COMMENT_AMPM and not left_result.comment and right_time < left_time:
            right_time = right_time + timedelta(days=Constants.HALF_DAY_HOUR_COUNT)

        if right_time < left_time:
            right_time = right_time + timedelta(days=1)

        ret.future_value = ret.past_value = (left_time, right_time)

        left_timex = pr1.timex_str
        right_timex = pr2.timex_str

        if begin_has_date:
            right_timex = DateTimeFormatUtil.luis_date_short_time(right_time, pr2.timex_str)
        elif end_has_date:
            left_timex = DateTimeFormatUtil.luis_date_short_time(left_time, pr1.timex_str)

        ret.timex = TimexUtil.generate_date_time_period_timex(left_timex, right_timex, right_time - left_time)
        ret.success = True
        return ret

    def parse_duration(self, text: str, reference_time: datetime) -> DateTimeResolutionResult:
        ret = DateTimeResolutionResult()
        ers = self.config.duration_extractor.extract(text, reference_time)

        if len(ers) == 1:
            pr = self.config.duration_parser.parse(ers[0])
            after_str = text[pr.start + pr.legth:].strip()

            if pr.value:
                swift_seconds = 0
                mod = ''
                duration_result: DateTimeResolutionResult = pr.value

                if type(duration_result.past_value) == float and type(duration_result.future_value) == float:
                    swift_seconds += int(duration_result.future_value)

                begin_time: datetime = None
                end_time = begin_time = reference_time
                match = regex.match(self.config.future_regex, after_str)

                if RegExpUtility.get_group(match, Constants.WITHIN_GROUP_NAME):
                    end_time = begin_time + timedelta(seconds=swift_seconds)
                    ret.timex = TimexUtil.generate_date_time_period_timex(begin_time, end_time, duration_result.timex)

                    ret.future_value = ret.past_value = (begin_time, end_time)
                    ret.success = True

                    if mod:
                        pr.value.Mod = mod

                    ret.sub_date_time_entities = [pr]
                    return ret

        return ret

    # Parse cases like "this night"
    def parse_specific_night(self, text: str, reference_time: datetime) -> DateTimeResolutionResult:
        ret = DateTimeResolutionResult()
        trimmed_text = text.strip()
        begin_hour = end_hour = end_min = 0
        time_str: str = None

        # Handle 昨晚 (last night)，今晨 (this morning)
        if RegExpUtility.is_exact_match(self.config.specific_time_of_day_regex, trimmed_text, True):
            # handle the ambiguous case "ぎりぎり" [the latest possible time]
            latest = regex.match(self.config.specific_time_of_day_regex, text)

            if RegExpUtility.get_group(latest, Constants.LATER_GROUP_NAME):
                begin_date: datetime = reference_time + timedelta(minutes=-1)
                end_date: datetime = reference_time

                diff = end_date - begin_date

                ret.timex = TimexUtil.generate_date_time_period_timex(begin_date, end_date)
                ret.future_value = ret.past_value = (begin_date, end_date)
                ret.success = True
                return ret

            if not self.config.get_matched_time_range_and_swift(trimmed_text):
                return ret

            night_regex_match = regex.match(self.config.next_regex, trimmed_text)
            last_regex_match = regex.match(self.config.last_regex, trimmed_text)
            if night_regex_match:
                swift = 1
            elif last_regex_match:
                swift = -1

            date = datetime.date(reference_time + timedelta(days=swift))
            day = date.day
            month = date.month
            year = date.year

            ret.timex = DateTimeFormatUtil.format_date(date) + time_str
            ret.future_value = ret.past_value = (DateUtils.safe_create_from_value(year, month, day, begin_hour, 0, 0),
                                                 DateUtils.safe_create_from_value(year, month, day, end_hour, end_min,
                                                                                  end_min))
            ret.success = True
            return ret

        # Handle cases like morning, afternoon
        if not self.config.get_matched_time_range(trimmed_text):
            return ret

        if RegExpUtility.is_exact_match(self.config.specific_time_of_day_regex, trimmed_text, True):
            swift = 0
            night_regex_match = regex.match(self.config.next_regex, trimmed_text)
            last_regex_match = regex.match(self.config.next_regex, trimmed_text)
            if night_regex_match:
                swift = 1
            elif last_regex_match:
                swift = -1
            date = datetime.date(reference_time + timedelta(days=swift))
            day = date.day
            month = date.month
            year = date.year

            ret.timex = DateTimeFormatUtil.format_date(date) + time_str
            ret.future_value = ret.past_value = (DateUtils.safe_create_from_value(year, month, day, begin_hour, 0, 0),
                                                 DateUtils.safe_create_from_value(year, month, day, end_hour, end_min,
                                                                                  end_min))
            ret.success = True
            return ret

        # handle Date followed by morning, afternoon
        match = regex.match(self.config.time_of_day_regex, trimmed_text)

        if match:
            before_str = trimmed_text[0:match.start()].strip()
            ers = self.config.date_extractor.extract(before_str, reference_time)

            if len(ers) == 0 or ers[0].length != len(before_str):
                return ret

            pr = self.config.date_parser.parse(ers[0], reference_time)
            future_date = pr.value.future_value
            past_date = pr.value.past_value

            ret.timex = pr.timex_str + time_str

            ret.future_value = (DateUtils.safe_create_from_value(future_date.year,
                                                                 future_date.month,
                                                                 future_date.day,
                                                                 begin_hour, 0, 0),
                                DateUtils.safe_create_from_value(future_date.year,
                                                                 future_date.month,
                                                                 future_date.day,
                                                                 end_hour, end_min, end_min))
            ret.past_value = (DateUtils.safe_create_from_value(past_date.year,
                                                               past_date.month,
                                                               past_date.day,
                                                               past_date, 0, 0),
                              DateUtils.safe_create_from_value(past_date.year,
                                                               past_date.month,
                                                               past_date.day,
                                                               end_hour, end_min, end_min))
            ret.success = True
            return ret

        return ret

    # parse "in 20 minutes"
    def parse_number_with_unit(self, text: str, reference_time: datetime) -> DateTimeResolutionResult:
        ret = DateTimeResolutionResult()
        unit_str: str = None

        # if there are spaces between number and unit
        ers = self.config.cardinal_extractor.extract(text)
        if len(ers) == 1:
            pr = self.config.cardinal_parser.parse(ers[0])
            src_unit = text[ers[0].start + ers[0].length].strip()

            if src_unit.startswith("个"):
                src_unit = src_unit[1:]

            before_str = text[0: ers[0].start]
            if src_unit in self.config.unit_map:
                num_str = pr.resolution_str
                unit_str = self.config.unit_map[src_unit]
                prefix_match = RegExpUtility.exact_match(self.config.past_regex, before_str, True)
                if prefix_match.success:
                    begin_date: datetime = None
                    end_date: datetime = None

                    if unit_str == "H":
                        begin_date = reference_time + timedelta(hours=-float(pr.value))
                        end_date = reference_time
                    elif unit_str == "M":
                        begin_date = reference_time + timedelta(minutes=-float(pr.value))
                        end_date = reference_time
                    elif unit_str == "S":
                        begin_date = reference_time + timedelta(seconds=-float(pr.value))
                        end_date = reference_time
                    else:
                        return ret

                    ret.timex = f"{DateTimeFormatUtil.luis_date(begin_date)}T" \
                                f"{DateTimeFormatUtil.luis_time(begin_date)}," \
                                f"{DateTimeFormatUtil.luis_date(end_date)}T" \
                                f"{DateTimeFormatUtil.luis_time(end_date)},PT" \
                                f"{num_str}{unit_str[0]}"
                    ret.future_value = ret.past_value = (begin_date, end_date)
                    ret.success = True
                    return ret

                if not prefix_match.success:
                    prefix_match = RegExpUtility.match_end(self.config.time_period_left_regex, before_str, True)

                if prefix_match.success:
                    begin_date: datetime = None
                    end_date: datetime = None

                    if unit_str == "H":
                        begin_date = reference_time
                        end_date = reference_time + timedelta(days=float(pr.value))
                    elif unit_str == "M":
                        begin_date = reference_time
                        end_date = reference_time + timedelta(minutes=float(pr.value))
                    elif unit_str == "S":
                        begin_date = reference_time
                        end_date = reference_time + timedelta(seconds=float(pr.value))
                    else:
                        return ret

                    ret.timex = f"{DateTimeFormatUtil.luis_date(begin_date)}T" \
                                f"{DateTimeFormatUtil.luis_time(begin_date)}," \
                                f"{DateTimeFormatUtil.luis_date(end_date)}T" \
                                f"{DateTimeFormatUtil.luis_time(end_date)},PT" \
                                f"{num_str}{unit_str[0]}"
                    ret.future_value = ret.past_value = (begin_date, end_date)
                    ret.success = True
                    return ret

        # handle "last hour"
        match = regex.match(self.config.unit_regex, text)
        if match:
            src_unit = RegExpUtility.get_group(match, Constants.UNIT_GROUP_NAME)
            before_str = text[0:match.start()].strip()

            if src_unit in self.config.unit_map:
                unit_str = self.config.unit_regex[src_unit]

                if RegExpUtility.is_exact_match(self.config.past_regex, before_str, True):
                    begin_date: datetime = None
                    end_date: datetime = None

                    if unit_str == "H":
                        begin_date = reference_time + timedelta(days=-1)
                        end_date = reference_time
                    elif unit_str == "M":
                        begin_date = reference_time + timedelta(minutes=-1)
                        end_date = reference_time
                    elif unit_str == "S":
                        begin_date = reference_time + timedelta(seconds=-1)
                        end_date = reference_time
                    else:
                        return ret

                    ret.timex = f"{DateTimeFormatUtil.luis_date(begin_date)}T" \
                                f"{DateTimeFormatUtil.luis_time(begin_date)}," \
                                f"{DateTimeFormatUtil.luis_date(end_date)}T" \
                                f"{DateTimeFormatUtil.luis_time(end_date)},PT" \
                                f"{unit_str[0]}"
                    ret.future_value = ret.past_value = (begin_date, end_date)
                    ret.success = True
                    return ret

                if RegExpUtility.is_exact_match(self.config.future_regex, before_str, True):
                    begin_date: datetime = None
                    end_date: datetime = None

                    if unit_str == "H":
                        beginDate = reference_time
                        endDate = reference_time + timedelta(hours=1)
                    elif unit_str == "M":
                        beginDate = reference_time
                        endDate = reference_time + timedelta(minutes=1)
                    elif unit_str == "S":
                        beginDate = reference_time
                        endDate = reference_time + timedelta(seconds=1)
                    else:
                        return ret

                    ret.timex = f"{DateTimeFormatUtil.luis_date(begin_date)}T" \
                                f"{DateTimeFormatUtil.luis_time(begin_date)}," \
                                f"{DateTimeFormatUtil.luis_date(end_date)}T" \
                                f"{DateTimeFormatUtil.luis_time(end_date)},PT" \
                                f"{unit_str[0]}"
                    ret.future_value = ret.past_value = (begin_date, end_date)
                    ret.success = True
                    return ret

        return ret

    def parse_relative_unit(self, text: str, reference_time: datetime):
        ret = DateTimeResolutionResult()

        match = regex.match(self.config.rest_of_date_regex, text)

        if match:
            src_unit = RegExpUtility.get_group(match, Constants.UNIT_GROUP_NAME)
            unit_str = self.config.unit_map[src_unit]
            swift_value = 1
            begin_time: datetime = reference_time
            end_time: datetime = reference_time

            if src_unit in self.config.unit_map:
                ret.timex = TimexUtil.generate_relative_unit_date_time_period_timex(begin_time,
                                                                                    end_time,
                                                                                    reference_time,
                                                                                    unit_str,
                                                                                    swift_value)
                ret.future_value = ret.past_value = (begin_time, end_time)
                ret.success = True if ret.timex else False
                return ret

        return ret

    # cases like "Early in the day Wednesday"
    def parse_date_with_period_suffix(self, text: str, reference_time: datetime) -> DateTimeResolutionResult:
        ret = DateTimeResolutionResult()
        date_result = self.config.date_extractor.extract(text, reference_time)

        if len(date_result) > 0:
            pr = DateTimeParseResult()
            after_str = text[date_result[0].start + date_result[0].length:
                             len(text) - date_result[0].start + date_result[0].length].lstrip()
            match = regex.match(self.config.time_period_left_regex, after_str)
            if match:
                pr = self.config.date_parser.parse(date_result[0], reference_time)

            if match:
                if pr.value:
                    start_time: datetime = pr.value.future_value
                    start_time = datetime(start_time.year, start_time.month, start_time.day)
                    end_time: datetime = start_time

                    if RegExpUtility.get_group(match, Constants.EARLY_PREFIX_GROUP_NAME):
                        end_time = end_time + timedelta(hours=Constants.HALF_DAY_HOUR_COUNT)
                        ret.mod = Constants.EARLY_MOD

                    elif RegExpUtility.get_group(match, Constants.MID_PREFIX_GROUP_NAME):
                        start_time = start_time + timedelta(hours=Constants.HALF_DAY_HOUR_COUNT -
                                                            Constants.HALF_MID_DAY_DURATION_HOUR_COUNT)
                        end_time = end_time + timedelta(
                            hours=Constants.HALF_DAY_HOUR_COUNT + Constants.HALF_MID_DAY_DURATION_HOUR_COUNT)
                        ret.mod = Constants.MID_MOD

                    elif RegExpUtility.get_group(match, Constants.LATE_PREFIX_GROUP_NAME):
                        start_time = start_time + timedelta(hours=Constants.HALF_DAY_HOUR_COUNT)
                        end_time = start_time + timedelta(hours=Constants.HALF_DAY_HOUR_COUNT)
                        ret.mod = Constants.LATE_MOD

                    else:
                        return ret

                    ret.timex = pr.timex_str
                    ret.past_value = ret.future_value = (start_time, end_time)
                    ret.success = True
        return ret
