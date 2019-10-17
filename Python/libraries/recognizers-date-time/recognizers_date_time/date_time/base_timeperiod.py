from abc import abstractmethod
from typing import List, Optional, Pattern, Dict, Match
from datetime import datetime, timedelta
from collections import namedtuple
import regex

from recognizers_text.utilities import RegExpUtility, QueryProcessor
from recognizers_text.extractor import Extractor, ExtractResult
from recognizers_date_time.date_time.base_time import BaseTimeExtractor, BaseTimeParser
from .constants import Constants, TimeTypeConstants
from .extractors import DateTimeExtractor
from .parsers import DateTimeParser, DateTimeParseResult
from .utilities import Token, merge_all_tokens, get_tokens_from_regex, DateTimeResolutionResult,\
    DateTimeUtilityConfiguration, DateTimeFormatUtil, ResolutionStartEnd, DateTimeOptionsConfiguration, DateTimeOptions

MatchedIndex = namedtuple('MatchedIndex', ['matched', 'index'])


class TimePeriodExtractorConfiguration(DateTimeOptionsConfiguration):
    @property
    @abstractmethod
    def simple_cases_regex(self) -> List[Pattern]:
        raise NotImplementedError

    @property
    @abstractmethod
    def till_regex(self) -> Pattern:
        raise NotImplementedError

    @property
    @abstractmethod
    def time_of_day_regex(self) -> Pattern:
        raise NotImplementedError

    @property
    @abstractmethod
    def general_ending_regex(self) -> Pattern:
        raise NotImplementedError

    @property
    @abstractmethod
    def single_time_extractor(self) -> DateTimeExtractor:
        raise NotImplementedError

    @property
    @abstractmethod
    def integer_extractor(self) -> Extractor:
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
    def token_before_date(self) -> str:
        raise NotImplementedError

    @property
    @abstractmethod
    def pure_number_regex(self) -> List[Pattern]:
        raise NotImplementedError


class BaseTimePeriodExtractor(DateTimeExtractor):
    @property
    def extractor_type_name(self) -> str:
        return Constants.SYS_DATETIME_TIMEPERIOD

    def __init__(self, config: TimePeriodExtractorConfiguration):
        self.config = config

    def extract(self, source: str, reference: datetime = None) -> List[ExtractResult]:
        if reference is None:
            reference = datetime.now()

        tokens = self.match_simple_cases(self.config.simple_cases_regex, source)
        tokens.extend(self.merge_two_time_points(source, reference))
        tokens.extend(self.match_night(source))

        if (self.config.options & DateTimeOptions.CALENDAR) != 0:
            tokens.extend(self.match_pure_number_cases(source))

        result = merge_all_tokens(tokens, source, self.extractor_type_name)

        if (self.config.options & DateTimeOptions.ENABLE_PREVIEW) != 0:
            # When TimeZone be migrated enable it
            pass

        if source == 'morgen':
            result = []

        return result

    def match_pure_number_cases(self, text):
        ret = []

        for regexp in self.config.pure_number_regex:

            matches = regexp.search(text)
            for match in matches:
                after_str = text[text.index(match.group()) + (match.end() - match.start()):]
                ending_match = self.config.general_ending_regex.search(after_str)
                if ending_match:
                    ret.append(Token(text.index(match.group()),
                                     text.index(match.group()) + (match.end() - match.start())))
        return ret

    def match_simple_cases(self, regexp: Pattern, source: str) -> List[Token]:
        result: List[Token] = list()

        for regexp in self.config.simple_cases_regex:
            matches: [Match] = regex.finditer(regexp, source)

            if matches:
                for match in matches:

                    if RegExpUtility.get_group(match, Constants.MINUTE_GROUP_NAME) or\
                            RegExpUtility.get_group(match, Constants.SECOND_GROUP_NAME):

                        end_with_valid_token = True
                        if (source.index(match.group()) + (match.end() - match.start())) == len(source):
                            end_with_valid_token = True

                        else:
                            after_str = source[source.index(match.group()) + (match.end() - match.start())]

                            end_with_general_endings = self.config.general_ending_regex.match(after_str)
                            end_with_am_pm = RegExpUtility.get_group(match, Constants.RIGHT_AM_PM_GROUP_NAME)

                            if end_with_general_endings or end_with_am_pm or\
                                    after_str.lstrip().startswith(self.config.token_before_date):
                                end_with_valid_token = True
                            elif (self.config.options & DateTimeOptions.ENABLE_PREVIEW) != 0:
                                # When TimeZone be migrated enable it
                                end_with_valid_token = False

                        if end_with_valid_token:
                            result.append(Token(source.index(match.group()), source.index(match.group()) +
                                                (match.end() - match.start())))
                    else:
                        match_pm_str = RegExpUtility.get_group(match, Constants.PM_GROUP_NAME)
                        match_am_str = RegExpUtility.get_group(match, Constants.AM_GROUP_NAME)
                        desc_str = RegExpUtility.get_group(match, Constants.DESC_GROUP_NAME)

                        if match_pm_str or match_am_str or desc_str:
                            result.append(Token(source.index(match.group()), source.index(match.group()) +
                                                (match.end() - match.start())))
                        else:
                            after_str = source[source.index(match.group()) + (match.end() - match.start()):]

                            # When TimeZone be migrated enable it
                            if (self.config.options & DateTimeOptions.ENABLE_PREVIEW) != 0:
                                result.append(Token(source.index(match.group()),
                                                    source.index(match.group()) + (match.end() - match.start())))

        return result

    def starts_with_time_zone(self, after_text: str):
        # it needs TimeZone
        starts_with_time_zone = False

        time_zone_extract_results = self.config.time

    def merge_two_time_points(self, source: str, reference: datetime) -> List[Token]:
        result: List[Token] = list()
        time_extract_results = self.config.single_time_extractor.extract(source, reference)
        num_extract_results = self.config.integer_extractor.extract(source)

        # Check if it is an ending number
        if num_extract_results:
            time_numbers: List[ExtractResult] = list()

            # check if it is a ending number
            ending_number = False
            num = num_extract_results[-1]
            if num.start + num.length == len(source):
                ending_number = True
            else:
                after = source[num.start + num.length:]
                if regex.search(self.config.general_ending_regex, after) is not None:
                    ending_number = True
            if ending_number:
                time_numbers.append(num)

            i = 0
            j = 0

            while i < len(num_extract_results):
                # find subsequent time point
                num_end = num_extract_results[i].start + num_extract_results[i].length

                while j < len(time_extract_results) and time_extract_results[j].start <= num_end:
                    j += 1

                if j >= len(time_extract_results):
                    break
                # check connector string
                middle = source[num_end:time_extract_results[j].start]
                match = regex.search(self.config.till_regex, middle)
                if match is not None and match.group() == middle.strip():
                    time_numbers.append(num_extract_results[i])
                i += 1

            # check overlap
            for time_num in time_numbers:
                overlap: bool = any(map(time_num.overlap, time_extract_results))
                if not overlap:
                    time_extract_results.append(time_num)

            time_extract_results = sorted(time_extract_results, key=lambda x: x.start)

        # merge "{TimePoint} to {TimePoint}", "between {TimePoint} and {TimePoint}"
        i = 0

        while i < len(time_extract_results)-1:
            middle_begin = time_extract_results[i].start + time_extract_results[i].length
            middle_end = time_extract_results[i + 1].start
            middle: str = source[middle_begin:middle_end].strip().lower()
            match = regex.search(self.config.till_regex, middle)

            # handle "{TimePoint} to {TimePoint}"
            if match is not None and match.start() == 0 and match.group() == middle:
                period_begin = time_extract_results[i].start
                period_end = time_extract_results[i + 1].start + time_extract_results[i + 1].length

                # handle "from"
                before = source[0:period_begin].strip().lower()
                from_index: MatchedIndex = self.config.get_from_token_index(
                    before)
                if from_index.matched:
                    period_begin = from_index.index

                # handle "between"
                between_index: MatchedIndex = self.config.get_between_token_index(
                    before)
                if between_index.matched:
                    period_begin = between_index.index

                result.append(Token(period_begin, period_end))
                i += 2
                continue

            # handle "between {TimePoint} and {TimePoint}"
            if self.config.has_connector_token(middle):
                period_begin = time_extract_results[i].start
                period_end = time_extract_results[i + 1].start + time_extract_results[i + 1].length

                # handle "between"
                before = source[0:period_begin].strip().lower()
                between_index: MatchedIndex = self.config.get_between_token_index(
                    before)
                if between_index.matched:
                    period_begin = between_index.index
                    result.append(Token(period_begin, period_end))
                    i += 2
                    continue

            i += 1

        return result

    def match_night(self, source: str) -> List[Token]:
        return get_tokens_from_regex(self.config.time_of_day_regex, source)


MatchedTimeRegex = namedtuple(
    'MatchedTimeRegex', ['matched', 'timex', 'begin_hour', 'end_hour', 'end_min'])


class TimePeriodParserConfiguration:
    @property
    @abstractmethod
    def time_extractor(self) -> BaseTimeExtractor:
        raise NotImplementedError

    @property
    @abstractmethod
    def time_parser(self) -> BaseTimeParser:
        raise NotImplementedError

    @property
    @abstractmethod
    def integer_extractor(self) -> Extractor:
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
    def time_of_day_regex(self) -> Pattern:
        raise NotImplementedError

    @property
    @abstractmethod
    def till_regex(self) -> Pattern:
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
    def get_matched_timex_range(self, source: str) -> MatchedTimeRegex:
        raise NotImplementedError


class BaseTimePeriodParser(DateTimeParser):
    @property
    def parser_type_name(self) -> str:
        return Constants.SYS_DATETIME_TIMEPERIOD

    def __init__(self, config: TimePeriodParserConfiguration):
        self.config = config

    def parse(self, source: ExtractResult, reference: datetime = None) -> Optional[DateTimeParseResult]:
        if reference is None:
            reference = datetime.now()

        value = None
        if source.type is self.parser_type_name:
            source_text = source.text.lower()

            inner_result = self.parse_simple_cases(source_text, reference)

            if not inner_result.success:
                inner_result = self.merge_two_time_points(
                    source_text, reference)

            if not inner_result.success:
                inner_result = self.parse_time_of_day(source_text, reference)

            if inner_result.success:
                inner_result.future_resolution[TimeTypeConstants.START_TIME] = DateTimeFormatUtil.format_time(
                    inner_result.future_value.start)
                inner_result.future_resolution[TimeTypeConstants.END_TIME] = DateTimeFormatUtil.format_time(
                    inner_result.future_value.end)
                inner_result.past_resolution[TimeTypeConstants.START_TIME] = DateTimeFormatUtil.format_time(
                    inner_result.past_value.start)
                inner_result.past_resolution[TimeTypeConstants.END_TIME] = DateTimeFormatUtil.format_time(
                    inner_result.past_value.end)
                value = inner_result

        result = DateTimeParseResult(source)
        result.value = value
        result.timex_str = value.timex if value is not None else ''
        result.resolution_str = ''

        return result

    def parse_simple_cases(self, source: str, reference: datetime) -> DateTimeResolutionResult:
        result = DateTimeResolutionResult()
        year = reference.year
        month = reference.month
        day = reference.day

        source = source.strip().lower()

        match = regex.search(self.config.pure_number_from_to_regex, source)
        if not match:
            match = regex.search(
                self.config.pure_number_between_and_regex, source)

        if not match or match.start() != 0:
            return result

        # this "from .. to .." pattern is valid if followed by a Date OR "pm"
        valid = False

        # get hours
        hour_group_list = RegExpUtility.get_group_list(match, Constants.HOUR_GROUP_NAME)

        hour_str = hour_group_list[0]
        begin_hour = self.config.numbers.get(hour_str, None)
        if not begin_hour:
            begin_hour = int(hour_str)

        hour_str = hour_group_list[1]
        end_hour = self.config.numbers.get(hour_str, None)
        if not end_hour:
            end_hour = int(hour_str)

        # parse PM
        left_desc: str = RegExpUtility.get_group(match, Constants.LEFT_DESC_GROUP_NAME)
        right_desc: str = RegExpUtility.get_group(match, Constants.RIGHT_DESC_GROUP_NAME)
        pm_str: str = RegExpUtility.get_group(match, Constants.PM_GROUP_NAME)
        am_str: str = RegExpUtility.get_group(match, Constants.AM_PM_GROUP_NAME)

        # The "am_pm" only occurs in time, don't have to consider it here

        if not left_desc:
            right_am_valid: bool = right_desc and regex.search(
                self.config.utility_configuration.am_desc_regex, right_desc.lower())
            right_pm_valid: bool = right_desc and regex.search(
                self.config.utility_configuration.pm_desc__regex, right_desc.lower())

            if am_str or right_am_valid:
                if end_hour >= 12:
                    end_hour -= 12

                if begin_hour >= 12 and begin_hour - 12 < end_hour:
                    begin_hour -= 12

                # Resolve case like "11 to 3am"
                if 12 > begin_hour > end_hour:
                    begin_hour += 12

                valid = True
            elif pm_str or right_pm_valid:
                if end_hour < 12:
                    end_hour += 12

                # Resolve case like "11 to 3pm"
                if begin_hour + 12 < end_hour:
                    begin_hour += 12

                valid = True
        if not valid:
            return result

        begin = f'T{begin_hour:02d}'
        end = f'T{end_hour:02d}'

        if begin_hour >= end_hour:
            end_hour += 24

        difference = end_hour - begin_hour
        result.timex = f'({begin},{end},PT{difference}H)'
        result.future_value = ResolutionStartEnd()
        result.past_value = ResolutionStartEnd()
        result.future_value.start = datetime(
            year, month, day) + timedelta(hours=begin_hour)
        result.future_value.end = datetime(
            year, month, day) + timedelta(hours=end_hour)
        result.past_value.start = result.future_value.start
        result.past_value.end = result.future_value.end
        result.success = True

        return result

    def merge_two_time_points(self, source: str, reference: datetime) -> DateTimeResolutionResult:
        result = DateTimeResolutionResult()
        extract_results = self.config.time_extractor.extract(source, reference)
        valid_time_number = True

        if len(extract_results) != 2:
            if len(extract_results) == 1:
                time_extract_results = extract_results[0]
                num_extract_results = self.config.integer_extractor.extract(source)

                for num in num_extract_results:
                    middle_begin = 0
                    middle_end = 0

                    # ending number
                    if num.start > time_extract_results.start + time_extract_results.length:
                        middle_begin = time_extract_results.start + time_extract_results.length
                        middle_end = num.start - middle_begin
                    elif num.start + num.length < time_extract_results.start:
                        middle_begin = num.start + num.length
                        middle_end = time_extract_results.start - middle_begin

                    # check if the middle string between the time point and the valid number is a connect string.
                    middle_str = source[middle_begin:middle_begin + middle_end]
                    if regex.search(self.config.till_regex, middle_str) is not None:
                        num.type = Constants.SYS_DATETIME_TIME
                        extract_results.append(num)
                        valid_time_number = True
                        break

                extract_results = sorted(extract_results, key=lambda x: x.start)

            if not valid_time_number:
                return result

        if len(extract_results) != 2:
            return result

        pr1 = self.config.time_parser.parse(extract_results[0], reference)
        pr2 = self.config.time_parser.parse(extract_results[1], reference)

        if pr1.value is None or pr2.value is None:
            return result

        am_pm_str1: str = pr1.value.comment
        am_pm_str2: str = pr2.value.comment
        begin_time: datetime = pr1.value.future_value
        end_time: datetime = pr2.value.future_value

        if am_pm_str2 and am_pm_str2.endswith(Constants.AM_PM_GROUP_NAME) and\
                end_time <= begin_time < end_time + timedelta(hours=12):
            end_time: datetime = end_time + timedelta(hours=12)
            pr2.value.future_value = end_time
            pr2.timex_str = f'T{end_time.hour}'
            if end_time.minute > 0:
                pr2.timex_str = f'{pr2.timex_str}:{end_time.minute}'

        if am_pm_str1 and am_pm_str1.endswith(Constants.AM_PM_GROUP_NAME) and\
                end_time > begin_time + timedelta(hours=12):
            begin_time: datetime = begin_time + timedelta(hours=12)
            pr1.value.future_value = begin_time
            pr1.timex_str = f'T{begin_time.hour}'
            if begin_time.minute > 0:
                pr1.timex_str = f'{pr1.timex_str}:{begin_time.minute}'

        if end_time < begin_time:
            end_time = end_time + timedelta(days=1)

        hours = QueryProcessor.float_or_int(
            (end_time - begin_time).total_seconds() // 3600)
        minutes = QueryProcessor.float_or_int(
            (end_time - begin_time).total_seconds() / 60 % 60)

        hours_str = f'{hours}H' if hours > 0 else ''
        minutes_str = f'{minutes}M' if 0 < minutes < 60 else ''
        result.timex = f'({pr1.timex_str},{pr2.timex_str},PT{hours_str}{minutes_str})'
        result.future_value = ResolutionStartEnd(begin_time, end_time)
        result.past_value = ResolutionStartEnd(begin_time, end_time)
        result.success = True
        if am_pm_str1 and am_pm_str1.endswith(Constants.AM_PM_GROUP_NAME) and am_pm_str2 and\
                am_pm_str2.endswith(Constants.AM_PM_GROUP_NAME):
            result.comment = Constants.AM_PM_GROUP_NAME

        result.sub_date_time_entities = [pr1, pr2]
        return result

    # parse "morning", "afternoon", "night"
    def parse_time_of_day(self, source: str, reference: datetime) -> DateTimeResolutionResult:
        result = DateTimeResolutionResult()
        year = reference.year
        month = reference.month
        day = reference.day

        # extract early/late prefix from text
        has_early = False
        has_late = False
        match = regex.search(self.config.time_of_day_regex, source)
        if match is not None:
            early = RegExpUtility.get_group(match, Constants.EARLY)
            if early:
                has_early = True
                source = source.replace(early, '')
                result.comment = Constants.EARLY
                result.mod = TimeTypeConstants.EARLY_MOD
            late = RegExpUtility.get_group(match, Constants.LATE)
            if late:
                has_late = True
                source = source.replace(late, '')
                result.comment = Constants.LATE
                result.mod = TimeTypeConstants.LATE_MOD

        timex_range = self.config.get_matched_timex_range(source)
        if not timex_range.matched:
            return result

        # modify time period if "early" or "late" is existed
        if has_early:
            timex_range = MatchedTimeRegex(
                matched=timex_range.matched,
                timex=timex_range.timex,
                begin_hour=timex_range.begin_hour,
                end_hour=timex_range.begin_hour + 2,
                end_min=0 if timex_range.end_min == 59 else timex_range.end_min
            )
        elif has_late:
            timex_range = MatchedTimeRegex(
                matched=timex_range.matched,
                timex=timex_range.timex,
                begin_hour=timex_range.begin_hour + 2,
                end_hour=timex_range.end_hour,
                end_min=timex_range.end_min
            )

        result.timex = timex_range.timex
        result.future_value = ResolutionStartEnd()
        result.past_value = ResolutionStartEnd()
        result.future_value.start = datetime(
            year, month, day, timex_range.begin_hour, 0, 0)
        result.future_value.end = datetime(
            year, month, day, timex_range.end_hour, timex_range.end_min, timex_range.end_min)
        result.past_value.start = result.future_value.start
        result.past_value.end = result.future_value.end

        result.success = True
        return result
