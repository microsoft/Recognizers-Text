from abc import ABC, abstractmethod
from typing import List, Optional, Pattern, Dict
from datetime import datetime, timedelta
from collections import namedtuple
import regex

from recognizers_text.utilities import RegExpUtility, FormatUtility
from recognizers_text.extractor import Extractor, ExtractResult
from recognizers_date_time.date_time.extractors import DateTimeExtractor
from recognizers_date_time.date_time.base_time import BaseTimeExtractor, BaseTimeParser
from .constants import Constants, TimeTypeConstants
from .extractors import DateTimeExtractor
from .parsers import DateTimeParser, DateTimeParseResult
from .utilities import Token, merge_all_tokens, get_tokens_from_regex, DateTimeResolutionResult, DateTimeUtilityConfiguration, FormatUtil, ResolutionStartEnd

MatchedIndex = namedtuple('MatchedIndex', ['matched', 'index'])

class TimePeriodExtractorConfiguration(ABC):
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

class BaseTimePeriodExtractor(DateTimeExtractor):
    @property
    def extractor_type_name(self) -> str:
        return Constants.SYS_DATETIME_TIMEPERIOD

    def __init__(self, config: TimePeriodExtractorConfiguration):
        self.config = config

    def extract(self, source: str, reference: datetime = None) -> List[ExtractResult]:
        if reference is None:
            reference = datetime.now()

        tokens = self.match_simple_cases(source)
        tokens.extend(self.merge_two_time_points(source, reference))
        tokens.extend(self.match_night(source))

        result = merge_all_tokens(tokens, source, self.extractor_type_name)
        return result

    def match_simple_cases(self, source: str) -> List[Token]:
        result: List[Token] = list()

        for pattern in self.config.simple_cases_regex:
            for match in regex.finditer(pattern, source):
                pm = RegExpUtility.get_group(match, 'pm')
                am = RegExpUtility.get_group(match, 'am')
                desc = RegExpUtility.get_group(match, 'desc')

                if pm or am or desc:
                    result.append(Token(match.start(), match.end()))

        return result

    def merge_two_time_points(self, source: str, reference: datetime) -> List[Token]:
        result: List[Token] = list()
        time_ers = self.config.single_time_extractor.extract(source, reference)
        num_ers = self.config.integer_extractor.extract(source)

        # Check if it is an ending number
        if num_ers:
            time_numbers: List[ExtractResult] = list()

            # check if it is a ending number
            ending_number = False
            num = num_ers[-1]
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

            while i < len(num_ers):
                # find subsequent time point
                num_end = num_ers[i].start + num_ers[i].length

                while j < len(time_ers) and time_ers[j].start <= num_end:
                    j += 1

                if j >= len(time_ers):
                    break
                # check connector string
                middle = source[num_end:time_ers[j].start]
                match = regex.search(self.config.till_regex, middle)
                if match is not None and match.group() == middle.strip():
                    time_numbers.append(num_ers[i])
                i += 1

            # check overlap
            for time_num in time_numbers:
                overlap: bool = any(map(time_num.overlap, time_ers))
                if not overlap:
                    time_ers.append(time_num)

            time_ers = sorted(time_ers, key=lambda x: x.start)

        # merge "{TimePoint} to {TimePoint}", "between {TimePoint} and {TimePoint}"
        i = 0

        while i < len(time_ers)-1:
            middle_begin = time_ers[i].start + time_ers[i].length
            middle_end = time_ers[i+1].start
            middle: str = source[middle_begin:middle_end].strip().lower()
            match = regex.search(self.config.till_regex, middle)

            # handle "{TimePoint} to {TimePoint}"
            if match is not None and match.start() == 0 and match.group() == middle:
                period_begin = time_ers[i].start
                period_end = time_ers[i+1].start + time_ers[i+1].length

                # handle "from"
                before = source[0:period_begin].strip().lower()
                from_index: MatchedIndex = self.config.get_from_token_index(before)
                if from_index.matched:
                    period_begin = from_index.index

                # handle "between"
                between_index: MatchedIndex = self.config.get_between_token_index(before)
                if between_index.matched:
                    period_begin = between_index.index

                result.append(Token(period_begin, period_end))
                i += 2
                continue

            # handle "between {TimePoint} and {TimePoint}"
            if self.config.has_connector_token(middle):
                period_begin = time_ers[i].start
                period_end = time_ers[i+1].start + time_ers[i+1].length

                # handle "between"
                before = source[0:period_begin].strip().lower()
                between_index: MatchedIndex = self.config.get_between_token_index(before)
                if between_index.matched:
                    period_begin = between_index.index
                    result.append(Token(period_begin, period_end))
                    i += 2
                    continue

            i += 1

        return result

    def match_night(self, source: str) -> List[Token]:
        return get_tokens_from_regex(self.config.time_of_day_regex, source)

MatchedTimeRegex = namedtuple('MatchedTimeRegex', ['matched', 'timex', 'begin_hour', 'end_hour', 'end_min'])

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
                inner_result = self.merge_two_time_points(source_text, reference)

            if not inner_result.success:
                inner_result = self.parse_night(source_text, reference)

            if inner_result.success:
                inner_result.future_resolution[TimeTypeConstants.START_TIME] = FormatUtil.format_time(inner_result.future_value.start)
                inner_result.future_resolution[TimeTypeConstants.END_TIME] = FormatUtil.format_time(inner_result.future_value.end)
                inner_result.past_resolution[TimeTypeConstants.START_TIME] = FormatUtil.format_time(inner_result.past_value.start)
                inner_result.past_resolution[TimeTypeConstants.END_TIME] = FormatUtil.format_time(inner_result.past_value.end)
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
            match = regex.search(self.config.pure_number_between_and_regex, source)

        if not match or match.start() != 0:
            return result

        # this "from .. to .." pattern is valid if followed by a Date OR "pm"
        valid = False

        # get hours
        hour_group_list = RegExpUtility.get_group_list(match, 'hour')

        hour_str = hour_group_list[0]
        begin_hour = self.config.numbers.get(hour_str, None)
        if not begin_hour:
            begin_hour = int(hour_str)

        hour_str = hour_group_list[1]
        end_hour = self.config.numbers.get(hour_str, None)
        if not end_hour:
            end_hour = int(hour_str)

        # parse PM
        left_desc: str = RegExpUtility.get_group(match, 'leftDesc')
        right_desc: str = RegExpUtility.get_group(match, 'rightDesc')
        pm_str: str = RegExpUtility.get_group(match, 'pm')
        am_str: str = RegExpUtility.get_group(match, 'am')

        # The "ampm" only occurs in time, don't have to consider it here

        if not left_desc:
            rigth_am_valid: bool = right_desc and regex.search(self.config.utility_configuration.am_desc_regex, right_desc.lower())
            rigth_pm_valid: bool = right_desc and regex.search(self.config.utility_configuration.pm_desc__regex, right_desc.lower())

            if am_str or rigth_am_valid:
                if end_hour >= 12:
                    end_hour -= 12

                if begin_hour >= 12 and begin_hour - 12 < end_hour:
                    begin_hour -= 12

                # Resolve case like "11 to 3am"
                if begin_hour < 12 and begin_hour > end_hour:
                    begin_hour += 12

                valid = True
            elif pm_str or rigth_pm_valid:
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
        result.future_value.start = datetime(year, month, day) + timedelta(hours=begin_hour)
        result.future_value.end = datetime(year, month, day) + timedelta(hours=end_hour)
        result.past_value.start = result.future_value.start
        result.past_value.end = result.future_value.end
        result.success = True

        return result

    def merge_two_time_points(self, source: str, reference: datetime) -> DateTimeResolutionResult:
        result = DateTimeResolutionResult()
        ers = self.config.time_extractor.extract(source, reference)
        valid_time_number = True

        if len(ers) != 2:
            if len(ers) == 1:
                time_er = ers[0]
                num_ers = self.config.integer_extractor.extract(source)

                for num in num_ers:
                    middle_begin = 0
                    middle_end = 0

                    # ending number
                    if num.start > time_er.start + time_er.length:
                        middle_begin = time_er.start + time_er.length
                        middle_end = num.start - middle_begin
                    elif num.start + num.length < time_er.start:
                        middle_begin = num.start + num.length
                        middle_end = time_er.start - middle_begin

                    # check if the middle string between the time point and the valid number is a connect string.
                    middle_str = source[middle_begin:middle_begin + middle_end]
                    if regex.search(self.config.till_regex, middle_str) is not None:
                        num.type = Constants.SYS_DATETIME_TIME
                        ers.append(num)
                        valid_time_number = True
                        break

                ers = sorted(ers, key=lambda x: x.start)

            if not valid_time_number:
                return result

        if len(ers) != 2:
            return result

        pr1 = self.config.time_parser.parse(ers[0], reference)
        pr2 = self.config.time_parser.parse(ers[1], reference)

        if pr1.value is None or pr2.value is None:
            return result

        ampm_str1: str = pr1.value.comment
        ampm_str2: str = pr2.value.comment
        begin_time: datetime = pr1.value.future_value
        end_time: datetime = pr2.value.future_value

        if ampm_str2 and ampm_str2.endswith('ampm') and end_time <= begin_time and end_time + timedelta(hours=12) > begin_time:
            end_time: datetime = end_time + timedelta(hours=12)
            pr2.value.future_value = end_time
            pr2.timex_str = f'T{end_time.hour}'
            if end_time.minute > 0:
                pr2.timex_str = f'{pr2.timex_str}:{end_time.minute}'

        if ampm_str1 and ampm_str1.endswith('ampm') and end_time > begin_time + timedelta(hours=12):
            begin_time: datetime = begin_time + timedelta(hours=12)
            pr1.value.future_value = begin_time
            pr1.timex_str = f'T{begin_time.hour}'
            if begin_time.minute > 0:
                pr1.timex_str = f'{pr1.timex_str}:{begin_time.minute}'

        if end_time < begin_time:
            end_time = end_time + timedelta(days=1)

        hours = FormatUtility.float_or_int((end_time - begin_time).total_seconds() // 3600)
        minutes = FormatUtility.float_or_int((end_time - begin_time).total_seconds() / 60 % 60)

        hours_str = f'{hours}H' if hours > 0 else ''
        minutes_str = f'{minutes}M' if minutes > 0 and minutes < 60 else ''
        result.timex = f'({pr1.timex_str},{pr2.timex_str},PT{hours_str}{minutes_str})'
        result.future_value = ResolutionStartEnd(begin_time, end_time)
        result.past_value = ResolutionStartEnd(begin_time, end_time)
        result.success = True
        if ampm_str1 and ampm_str1.endswith('ampm') and ampm_str2 and ampm_str2.endswith('ampm'):
            result.comment = 'ampm'

        result.sub_date_time_entities = [pr1, pr2]
        return result

    # parse "morning", "afternoon", "night"
    def parse_night(self, source: str, reference: datetime) -> DateTimeResolutionResult:
        result = DateTimeResolutionResult()
        year = reference.year
        month = reference.month
        day = reference.day

        # extract early/late prefix from text
        has_early = False
        has_late = False
        match = regex.search(self.config.time_of_day_regex, source)
        if match is not None:
            early = RegExpUtility.get_group(match, 'early')
            if early:
                has_early = True
                source = source.replace(early, '')
                result.comment = 'early'
            late = RegExpUtility.get_group(match, 'late')
            if late:
                has_late = True
                source = source.replace(late, '')
                result.comment = 'late'

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
        result.future_value.start = datetime(year, month, day, timex_range.begin_hour, 0, 0)
        result.future_value.end = datetime(year, month, day, timex_range.end_hour, timex_range.end_min, timex_range.end_min)
        result.past_value.start = result.future_value.start
        result.past_value.end = result.future_value.end

        result.success = True
        return result
