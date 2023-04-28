#  Copyright (c) Microsoft Corporation. All rights reserved.
#  Licensed under the MIT License.

from abc import abstractmethod
from typing import List, Optional, Pattern, Dict, Match
from datetime import datetime, timedelta
from collections import namedtuple
import regex

from recognizers_text import MetaData
from recognizers_date_time.date_time.date_extractor import DateExtractor
from recognizers_text.extractor import ExtractResult
from recognizers_number.number.extractors import BaseNumberExtractor
from recognizers_number.number.parsers import BaseNumberParser
from ..constants import Constants, TimeTypeConstants
from recognizers_number.number.constants import Constants as NumConstants
from ..extractors import DateTimeExtractor
from ..parsers import DateTimeParser, DateTimeParseResult
from ..utilities import Token, merge_all_tokens, DateTimeResolutionResult, DateTimeUtilityConfiguration, AgoLaterUtil,\
    DateTimeFormatUtil, RegExpUtility, AgoLaterMode, DateTimeOptionsConfiguration, DateTimeOptions, filter_ambiguity, \
    TimexUtil, DurationParsingUtil


class CJKDateTimeExtractorConfiguration(DateTimeOptionsConfiguration):
    @property
    @abstractmethod
    def now_regex(self) -> Pattern:
        raise NotImplementedError

    @property
    @abstractmethod
    def preposition_regex(self) -> Pattern:
        raise NotImplementedError

    @property
    @abstractmethod
    def night_regex(self) -> Pattern:
        raise NotImplementedError

    @property
    @abstractmethod
    def time_of_special_day_regex(self) -> Pattern:
        raise NotImplementedError

    @property
    @abstractmethod
    def time_of_day_regex(self) -> Pattern:
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
    def connector_regex(self) -> Pattern:
        raise NotImplementedError

    @property
    @abstractmethod
    def duration_extractor(self) -> DateTimeExtractor:
        raise NotImplementedError

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
    def ambiguity_date_time_filters(self) -> Dict:
        raise NotImplementedError

    @abstractmethod
    def is_connector_token(self, middle):
        raise NotImplementedError


class BaseCJKDateTimeExtractor(DateTimeExtractor):
    @property
    def extractor_type_name(self) -> str:
        return Constants.SYS_DATETIME_DATETIME

    def __init__(self, config: CJKDateTimeExtractorConfiguration):
        self.config = config

    def extract(self, source: str, reference: datetime = None) -> List[ExtractResult]:

        if reference is None:
            reference = datetime.now()

        tokens: List[Token] = list()

        tokens.extend(self.merge_date_and_time(source, reference))
        tokens.extend(self.basic_regex_match(source))
        tokens.extend(self.time_of_today(source, reference))
        tokens.extend(self.duration_with_ago_and_later(source, reference))

        result = merge_all_tokens(tokens, source, self.extractor_type_name)

        result = filter_ambiguity(result, source, self.config.ambiguity_date_time_filters)

        return result

    # match "now"
    def basic_regex_match(self, source: str) -> List[Token]:
        tokens: List[Token] = list()
        # handle "now"
        matches: List[Match] = list(
            regex.finditer(self.config.now_regex, source))
        tokens.extend(map(lambda x: Token(x.start(), x.end()), matches))
        return tokens

    # Merge a Date entity and a Time entity, like "明天早上七点"
    def merge_date_and_time(self, source: str, reference: datetime) -> List[Token]:
        tokens: List[Token] = list()
        ers: List[ExtractResult] = self.config.date_point_extractor.extract(
            source, reference)

        if len(ers) < 1:
            return tokens

        ers.extend(self.config.time_point_extractor.extract(source, reference))

        if len(ers) < 2:
            return tokens

        ers = sorted(ers, key=lambda x: x.start)
        i = 0

        while i < len(ers) - 1:
            j = i + 1

            while j < len(ers) and ers[i].overlap(ers[j]):
                j += 1

            if j >= len(ers):
                break

            if ers[i].type is Constants.SYS_DATETIME_DATE and ers[j].type is Constants.SYS_DATETIME_TIME:
                middle_begin = ers[i].start + ers[i].length
                middle_end = ers[j].start

                if middle_begin > middle_end:
                    continue

                middle = source[middle_begin:middle_end].strip().lower()

                if self.config.is_connector_token(middle):
                    begin = ers[i].start
                    end = ers[j].start + ers[j].length
                    tokens.append(Token(begin, end))
                i = j + 1
                continue
            i = j

    # Parse a specific time of today, tonight, this afternoon, "今天下午七点"
    def time_of_today(self, source: str, reference: datetime) -> List[Token]:
        tokens: List[Token] = list()
        ers = self.config.time_point_extractor.extract(source, reference)

        for er in ers:
            before = source[:er.start]
            inner_match = regex.search(self.config.night_regex, er.text)

            if inner_match is not None and inner_match.start() == 0:
                before = source[:er.start + len(inner_match.group())]
            if not before:
                continue

            match = regex.search(self.config.time_of_special_day_regex, before)
            if match is not None and not before[match.end():].strip():
                begin = match.start()
                end = er.start + er.length
                tokens.append(Token(begin, end))

        # TimePeriodExtractor cases using TimeOfDayRegex are not processed here
        match_time_of_today = regex.search(self.config.time_of_special_day_regex, source)
        match_time_of_day = regex.search(self.config.time_of_day_regex, source)

        if match_time_of_today and not match_time_of_day:
            tokens.append(Token(match_time_of_today.start,
                                (match_time_of_today.start + match_time_of_today.length)
                                ))

        return tokens

    # Process case like "5分钟前" "二小时后"
    def duration_with_ago_and_later(self, source: str, reference: datetime) -> List[Token]:
        ret: List[Token] = list()
        duration_er = self.config.duration_extractor.extract(source, reference)

        for er in duration_er:
            pos = er.start + er.length
            if pos < len(source):
                suffix = source[pos]
                before_match = RegExpUtility.get_matches(self.config.before_regex, suffix)
                after_match = RegExpUtility.get_matches(self.config.after_regex, suffix)

                if (before_match and suffix.startswith(before_match[0])) \
                        or (after_match and suffix.startswith(after_match[0])):
                    meta_data = MetaData()
                    meta_data.is_duration_with_ago_and_later = True
                    ret.append(Token(er.start, pos + 1, meta_data))
        return ret


class CJKDateTimeParserConfiguration:
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
    def duration_parser(self) -> DateTimeParser:
        raise NotImplementedError

    @property
    @abstractmethod
    def time_parser(self) -> DateTimeParser:
        raise NotImplementedError

    @property
    @abstractmethod
    def integer_extractor(self) -> BaseNumberExtractor:
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
    def now_regex(self) -> Pattern:
        raise NotImplementedError

    @property
    @abstractmethod
    def lunar_regex(self) -> Pattern:
        raise NotImplementedError

    @property
    @abstractmethod
    def lunar_holiday_regex(self) -> Pattern:
        raise NotImplementedError

    @property
    @abstractmethod
    def simple_pm_regex(self) -> Pattern:
        raise NotImplementedError

    @property
    @abstractmethod
    def simple_am_regex(self) -> Pattern:
        raise NotImplementedError

    @property
    @abstractmethod
    def time_of_special_day_regex(self) -> Pattern:
        raise NotImplementedError

    @property
    @abstractmethod
    def datetime_period_unit_regex(self) -> Pattern:
        raise NotImplementedError

    @property
    @abstractmethod
    def duration_relative_duration_unit_regex(self) -> Pattern:
        raise NotImplementedError

    @property
    @abstractmethod
    def ago_later_regex(self) -> Pattern:
        raise NotImplementedError

    @property
    @abstractmethod
    def before_regex(self) -> Pattern:
        raise NotImplementedError

    @property
    @abstractmethod
    def after_regex(self) -> Pattern:
        raise NotImplementedError

    @abstractmethod
    def unit_map(self) -> Dict[str, str]:
        raise NotImplementedError

    @abstractmethod
    def get_matched_now_timex(self, source: str) -> bool:
        raise NotImplementedError

    @abstractmethod
    def get_swift_day(self, source: str) -> int:
        raise NotImplementedError

    @property
    @abstractmethod
    def utility_configuration(self) -> DateTimeUtilityConfiguration:
        raise NotImplementedError

    @abstractmethod
    def have_ambiguous_token(self, source: str, matched_text: str) -> bool:
        raise NotImplementedError

    @abstractmethod
    def adjust_by_time_of_day(self, source: str, hour: int, swift: int):
        raise NotImplementedError


class BaseCJKDateTimeParser(DateTimeParser):
    @property
    def parser_type_name(self) -> str:
        return Constants.SYS_DATETIME_DATETIME

    def __init__(self, config: CJKDateTimeParserConfiguration):
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
                inner_result = self.parse_time_of_speial_day_regex(source_text, reference)

            if not inner_result.success:
                inner_result = self.parser_duration_with_ago_and_later(source_text, reference)

            if inner_result.success:
                inner_result.future_resolution[TimeTypeConstants.DATETIME] = DateTimeFormatUtil.format_date_time(
                    inner_result.future_value)
                inner_result.past_resolution[TimeTypeConstants.DATETIME] = DateTimeFormatUtil.format_date_time(
                    inner_result.past_value)

                inner_result.is_lunar = self.is_lunar_calendar(source_text)

                result.value = inner_result
                result.timex_str = inner_result.timex if inner_result else ''
                result.resolution_str = ''

        return result

    def parse_basic_regex(self, source: str, reference: datetime) -> DateTimeResolutionResult:
        result = DateTimeResolutionResult()
        source = source.strip().lower()

        # handle "现在"
        match = regex.search(self.config.now_regex, source)
        if match and match.start() == 0 and match.group() == source:
            matched_now_timex = self.config.get_matched_now_timex(source)
            result.timex = matched_now_timex.timex
            result.future_value = reference
            result.past_value = reference
            result.success = matched_now_timex.matched
        return result

    # parse if lunar contains
    def is_lunar_calendar(self, source: str):
        source = source.strip().lower()
        match = regex.search(self.config.lunar_regex, source)
        if match:
            return True

    # merge a Date entity and a Time entity
    def merge_date_and_time(self, source: str, reference: datetime) -> DateTimeResolutionResult:
        ret = DateTimeResolutionResult()
        match_ago_later = regex.search(self.config.ago_later_regex, source)
        if match_ago_later:
            return ret

        er1 = self.config.date_extractor.extract(source, reference)
        if not er1:
            return ret

        er2 = self.config.time_extractor.extract(source, reference)
        if not er2:
            return ret

        pr1 = self.config.date_parser.parse(er1[0], reference)
        pr2 = self.config.time_parser.parse(er2[0], reference)
        if pr1.value is None or pr2.value is None:
            return ret

        future_date = pr1.value.future_value
        past_date = pr1.value.past_value
        time = pr2.value.future_value

        # handle cases with time like 25時 which resolve to the next day
        timex_hour = TimexUtil.parse_hour_from_time_timex(pr2.timex_str)
        if timex_hour > Constants.DAY_HOUR_COUNT:
            future_date.day = future_date.day + 1
            past_date.day = past_date.day + 1

        hour = time.hour
        minute = time.minute
        second = time.second

        #  handle morning, afternoon
        if self.config.simple_pm_regex.search(source) and hour < Constants.HALF_DAY_HOUR_COUNT:
            hour += Constants.HALF_DAY_HOUR_COUNT
        elif self.config.simple_am_regex.search(source) and hour >= Constants.HALF_DAY_HOUR_COUNT:
            hour += Constants.HALF_DAY_HOUR_COUNT

        time_str = pr2.timex_str
        if time_str.endswith(Constants.COMMENT_AMPM):
            time_str = time_str[0:len(time_str) - 4]

        ret.timex = pr1.timex_str + time_str

        val = pr2.value
        if hour <= Constants.COMMENT_AMPM and not self.config.simple_pm_regex.search(source) and not \
                self.config.simple_am_regex.search(source) and val.comment:
            ret.comment = Constants.COMMENT_AMPM

        ret.future_value = datetime(
            future_date.year, future_date.month, future_date.day, hour, minute, second)
        ret.past_value = datetime(
            past_date.year, past_date.month, past_date.day, hour, minute, second)
        ret.success = True

        return ret

    def parse_special_time_of_day(self, source: str, reference: datetime) -> DateTimeResolutionResult:
        result = DateTimeResolutionResult()
        ers = self.config.time_extractor.extract(source, reference)

        #  Handle 'eod', 'end of day'
        eod = regex.search(self.config.time_of_special_day_regex, source)
        match_ago_and_later = regex.search(self.config.ago_later_regex, source)
        if match_ago_and_later:
            duration_res = self.config.duration_extractor.extract(source, reference)
            pr1 = self.config.duration_parser.parse(duration_res[0], reference)
            is_future = match_ago_and_later.Groups[Constants.LATER_GROUP_NAME].value
            timex = pr1.timex_str

            # handle less and more mode
            if eod.Groups[Constants.LESS_GROUP_NAME].value:
                result.mod = Constants.LESS_THAN_MOD
            elif eod.Groups[Constants.MORE_GROUP_NAME].value:
                result.mod = Constants.MORE_THAN_MOD

            result_datetime = DurationParsingUtil.shift_date_time(timex, reference, future=is_future)




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
        result = self.parse_unspecific_time_of_date(source, reference)
        if result.success:
            return result

        extract_results = self.config.date_extractor.extract(source, reference)

        if len(extract_results) != 1:
            return result

        extract_result = next(iter(extract_results), None)
        before_str = source[0:extract_result.start]
        after_str = source[:extract_result.start + extract_result.end]
        if regex.search(self.config.specific_end_of_regex, before_str) or regex.search(
                self.config.specific_end_of_regex, after_str):
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
            self.config.utility_configuration
        )
