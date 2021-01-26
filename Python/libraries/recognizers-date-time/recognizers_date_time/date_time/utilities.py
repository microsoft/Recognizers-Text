from enum import Enum, IntEnum, IntFlag
from abc import ABC, abstractmethod
from typing import List, Dict, Pattern, Union, Match
from datetime import datetime, timedelta
import calendar

from datedelta import datedelta
import regex

from recognizers_text.matcher.number_with_unit_tokenizer import NumberWithUnitTokenizer
from recognizers_text.matcher.match_strategy import MatchStrategy
from recognizers_text.extractor import ExtractResult, Metadata
from recognizers_text.utilities import RegExpUtility
from recognizers_date_time.date_time.constants import TimeTypeConstants, Constants
from recognizers_date_time.date_time.extractors import DateTimeExtractor
from recognizers_date_time.date_time.parsers import DateTimeParser, DateTimeParseResult
from recognizers_text.matcher.string_matcher import StringMatcher, MatchResult


class TimeZoneUtility:

    @staticmethod
    def merge_time_zones(original_extract_results: [ExtractResult], time_zone_ers: [ExtractResult], text: str):

        for extract_result in original_extract_results:
            for time_zone_er in time_zone_ers:

                begin = extract_result.start + extract_result.length
                end = time_zone_er.start

                if begin < end:
                    gap_text = text[begin: begin + (end - begin)]

                    if gap_text.isspace() or gap_text is None:
                        new_length = time_zone_er.start + time_zone_er.length - extract_result.start

                        extract_result.text = text[extract_result.start:new_length]
                        extract_result.length = new_length
                        extract_result.data = {Constants.SYS_DATETIME_TIMEZONE, time_zone_er}

                if extract_result.overlap(time_zone_er):
                    extract_result.data = {Constants.SYS_DATETIME_TIMEZONE, time_zone_er}

        return original_extract_results

    @staticmethod
    def should_resolve_time_zone(extract_result: ExtractResult, options):
        enable_preview = (options & DateTimeOptions.ENABLE_PREVIEW) != 0

        if not enable_preview:
            return False

        has_time_zone_data = False

        if isinstance(extract_result.data, {}):
            meta_data = extract_result.data
            if meta_data is not None and Constants.SYS_DATETIME_TIMEZONE in meta_data.keys():
                has_time_zone_data = True

        return has_time_zone_data

    @staticmethod
    def build_matcher_from_lists(*collections: List[str]) -> StringMatcher:
        matcher = StringMatcher(MatchStrategy.TrieTree, NumberWithUnitTokenizer())

        matcher_list = []
        for collection in collections:
            list(map(lambda x: matcher_list.append(x.strip().lower()), collection))

        matcher_list = TimeZoneUtility.distinct(matcher_list)

        matcher.init(matcher_list)

        return matcher

    @staticmethod
    def distinct(list1):

        unique_list = []
        for x in list1:

            if x not in unique_list:
                unique_list.append(x)

        return unique_list


class DateTimeOptions(IntFlag):
    NONE = 0
    SKIP_FROM_TO_MERGE = 1
    SPLIT_DATE_AND_TIME = 2
    CALENDAR = 4
    EXTENDED_TYPES = 8
    FAIL_FAST = 2097152
    EXPERIMENTAL_MODE = 4194304
    ENABLE_PREVIEW = 8388608


class DateTimeOptionsConfiguration:
    @property
    def options(self):
        return self._options

    @property
    def dmy_date_format(self) -> bool:
        return self._dmy_date_format

    def __init__(self, options=DateTimeOptions.NONE, dmy_date_format=False):
        self._options = options
        self._dmy_date_format = dmy_date_format


class DurationParsingUtil:

    @staticmethod
    def is_time_duration_unit(uni_str: str):

        if uni_str == Constants.UNIT_H:
            result = True
        elif uni_str == Constants.UNIT_M:
            result = True
        elif uni_str == Constants.UNIT_S:
            result = True
        else:
            result = False

        return result


class Token:
    def __init__(self, start: int, end: int, metadata: Metadata = None):
        self._start: int = start
        self._end: int = end
        self._metadata = metadata

    @property
    def length(self) -> int:
        if self._start > self._end:
            return 0
        return self._end - self._start

    @property
    def start(self) -> int:
        return self._start

    @start.setter
    def start(self, value) -> int:
        self._start = value

    @property
    def end(self) -> int:
        return self._end

    @end.setter
    def end(self, value) -> int:
        self._end = value

    @property
    def metadata(self):
        return self._metadata

    @metadata.setter
    def metadata(self, value):
        self._metadata = value


def merge_all_tokens(tokens: List[Token], source: str, extractor_name: str) -> List[ExtractResult]:
    result = []

    merged_tokens: List[Token] = list()
    tokens_ = sorted(filter(None, tokens), key=lambda x: x.start)

    for token in tokens_:
        add = True

        for index, m_token in enumerate(merged_tokens):
            if not add:
                break

            if token.start >= m_token.start and token.end <= m_token.end:
                add = False

            if m_token.start < token.start < m_token.end:
                add = False

            if token.start <= m_token.start and token.end >= m_token.end:
                add = False
                merged_tokens[index] = token

        if add:
            merged_tokens.append(token)

    for token in merged_tokens:
        start = token.start
        length = token.length
        sub_str = source[start: start + length]

        extracted_result = ExtractResult()
        extracted_result.start = start
        extracted_result.length = length
        extracted_result.text = sub_str
        extracted_result.type = extractor_name
        extracted_result.data = None
        extracted_result.meta_data = token.metadata

        result.append(extracted_result)

    return result


def __token_to_result(token: Token, source: str, name: str) -> ExtractResult:
    result: ExtractResult = ExtractResult()
    result.start = token.start
    result.length = token.length
    result.text = source[token.start:token.end]
    result.type = name
    return result


def get_tokens_from_regex(pattern: Pattern, source: str) -> List[Token]:
    return list(map(lambda x: Token(x.start(), x.end()), regex.finditer(pattern, source)))


class ResolutionStartEnd:
    def __init__(self, _start=None, _end=None):
        self.start = _start
        self.end = _end

    @property
    def _start(self):
        return self.start

    @property
    def _end(self):
        return self.end


class DateTimeResolutionResult:
    def __init__(self):
        self.success: bool = False
        self.timex: str = ''
        self.is_lunar: bool = False
        self.mod: str = ''
        self.has_range_changing_mod: bool = False
        self.comment: str = ''
        self.future_resolution: Dict[str, str] = dict()
        self.past_resolution: Dict[str, str] = dict()
        self.future_value: object = None
        self.past_value: object = None
        self.sub_date_time_entities: List[object] = list()
        self.timezone_resolution: TimeZoneResolutionResult()
        self.list: List[object] = list()


class TimeOfDayResolution:
    def __init__(self):
        self.timex: str = None
        self.begin_hour: int = 0
        self.end_hour: int = 0
        self.end_min: int = 0


class DateTimeFormatUtil:
    HourTimeRegex = RegExpUtility.get_safe_reg_exp(r'(?<!P)T\d{2}')

    @staticmethod
    def to_str(num: Union[int, float], size: int) -> str:
        format_ = f'{{0:0{size}d}}'
        return str.format(format_, num)

    @staticmethod
    def luis_date(year: int, month: int, day: int) -> str:
        if year == -1:
            if month == -1:
                return f'XXXX-XX-{day:02d}'
            return f'XXXX-{month:02d}-{day:02d}'
        return f'{year:04d}-{month:02d}-{day:02d}'

    @staticmethod
    def luis_date_from_datetime(date: datetime) -> str:
        return DateTimeFormatUtil.luis_date(date.year, date.month, date.day)

    @staticmethod
    def luis_time(hour: int, minute: int, second: int) -> str:
        return f'{hour:02d}:{minute:02d}:{second:02d}'

    @staticmethod
    def luis_time_from_datetime(time: datetime) -> str:
        return DateTimeFormatUtil.luis_time(time.hour, time.minute, time.second)

    @staticmethod
    def luis_date_time(time: datetime) -> str:
        return DateTimeFormatUtil.luis_date_from_datetime(time) + 'T' + DateTimeFormatUtil.luis_time_from_datetime(time)

    @staticmethod
    def luis_time_span(begin_time: datetime, end_time: datetime) -> str:
        timex_builder = f'{Constants.GENERAL_PERIOD_PREFIX}{Constants.TIME_TIMEX_PREFIX}'

        total_hours = end_time.hour - begin_time.hour
        total_minutes = end_time.minute - begin_time.minute
        total_seconds = end_time.second - begin_time.second

        if total_hours > 0:
            timex_builder += f'{total_hours}H'
        if total_minutes > 0:
            timex_builder += f'{total_minutes}M'
        if total_seconds > 0:
            timex_builder += f'{total_seconds}S'

        return str(timex_builder)

    @staticmethod
    def format_date(date: datetime) -> str:
        return f'{date.year:04d}-{date.month:02d}-{date.day:02d}'

    @staticmethod
    def format_time(time: datetime) -> str:
        return f'{time.hour:02d}:{time.minute:02d}:{time.second:02d}'

    @staticmethod
    def format_date_time(date_time: datetime) -> str:
        return DateTimeFormatUtil.format_date(date_time) + ' ' + DateTimeFormatUtil.format_time(date_time)

    @staticmethod
    def all_str_to_pm(source: str) -> str:
        matches = list(regex.finditer(
            DateTimeFormatUtil.HourTimeRegex, source))
        split: List[str] = list()
        last_position = 0

        for match in matches:
            if last_position != match.start():
                split.append(source[last_position:match.start()])

            split.append(source[match.start():match.end()])
            last_position = match.end()

        if source[:last_position]:
            split.append(source[last_position:])

        for index, value in enumerate(split):
            if regex.search(DateTimeFormatUtil.HourTimeRegex, value):
                split[index] = DateTimeFormatUtil.to_pm(value)

        return ''.join(split)

    @staticmethod
    def to_pm(source: str) -> str:
        result = ''

        if source.startswith(Constants.UNIT_T):
            result = Constants.UNIT_T
            source = source[1:]

        split = source.split(':')
        hour = int(split[0])
        hour = 0 if hour == 12 else hour + 12
        split[0] = f'{hour:02d}'
        return result + ':'.join(split)

# ISO weekday


class DayOfWeek(IntEnum):
    MONDAY = 1
    TUESDAY = 2
    WEDNESDAY = 3
    THURSDAY = 4
    FRIDAY = 5
    SATURDAY = 6
    SUNDAY = 7


class DateUtils:
    min_value = datetime(1, 1, 1, 0, 0, 0, 0)

    # Generate future/past date for cases without specific year like "Feb 29th"
    @staticmethod
    def generate_dates(no_year: bool, reference: datetime, year: int, month: int, day: int) -> list:
        future_date = DateUtils.safe_create_from_min_value(year, month, day)
        past_date = DateUtils.safe_create_from_min_value(year, month, day)
        future_year = year
        past_year = year
        if no_year:
            if DateUtils.is_Feb_29th(year, month, day):
                if DateUtils.is_leap_year(year):
                    if future_date < reference:
                        future_date = DateUtils.safe_create_from_min_value(future_year + 4, month, day)
                    else:
                        past_date = DateUtils.safe_create_from_min_value(past_year - 4, month, day)
                else:
                    past_year = past_year >> 2 << 2
                    if not DateUtils.is_leap_year(past_year):
                        past_year -= 4

                    future_year = past_year + 4
                    if not DateUtils.is_leap_year(future_year):
                        future_year += 4

                    future_date = DateUtils.safe_create_from_min_value(future_year, month, day)
                    past_date = DateUtils.safe_create_from_min_value(past_year, month, day)
            else:
                if future_date < reference and DateUtils.is_valid_date(year, month, day):
                    future_date = DateUtils.safe_create_from_min_value(year + 1, month, day)

                if past_date >= reference and DateUtils.is_valid_date(year, month, day):
                    past_date = DateUtils.safe_create_from_min_value(year - 1, month, day)
        return future_date, past_date

    @staticmethod
    def int_try_parse(value):
        try:
            return int(value), True
        except ValueError:
            return value, False

    @staticmethod
    def safe_create_from_value(seed: datetime, year: int, month: int, day: int,
                               hour: int = 0, minute: int = 0, second: int = 0) -> datetime:
        if DateUtils.is_valid_date(year, month, day) and DateUtils.is_valid_time(hour, minute, second):
            return datetime(year, month, day, hour, minute, second)

        return seed

    @staticmethod
    def safe_create_from_min_value(year: int, month: int, day: int,
                                   hour: int = 0, minute: int = 0, second: int = 0) -> datetime:
        return DateUtils.safe_create_from_value(DateUtils.min_value, year, month, day, hour, minute, second)

    @staticmethod
    def safe_create_from_min_value_date_time(date: datetime, time: datetime = None) -> datetime:
        return DateUtils.safe_create_from_value(DateUtils.min_value, date.year, date.month, date.day,
                                                time.hour if time else 0, time.minute if time else 0,
                                                time.second if time else 0)

    @staticmethod
    def is_valid_date(year: int, month: int, day: int) -> bool:
        try:
            datetime(year, month, day)
            return True
        except ValueError:
            return False

    @staticmethod
    def is_valid_datetime(date: datetime) -> bool:
        return date != DateUtils.min_value

    @staticmethod
    def is_valid_time(hour: int, minute: int, second: int) -> bool:
        return 0 <= hour < 24 and 0 <= minute < 60 and second >= 0 and minute < 60

    @staticmethod
    def this(from_date: datetime, day_of_week: DayOfWeek) -> datetime:
        start = from_date.isoweekday()
        target = day_of_week if day_of_week >= int(
            DayOfWeek.MONDAY) else int(DayOfWeek.SUNDAY)
        result = from_date + timedelta(days=target-start)
        return result

    @staticmethod
    def next(from_date: datetime, day_of_week: DayOfWeek) -> datetime:
        return DateUtils.this(from_date, day_of_week) + timedelta(weeks=1)

    @staticmethod
    def last(from_date: datetime, day_of_week: DayOfWeek) -> datetime:
        return DateUtils.this(from_date, day_of_week) - timedelta(weeks=1)

    @staticmethod
    def safe_create_date_resolve_overflow(year: int, month: int, day: int) -> datetime:
        if month > 12:
            year = year + month // 12
            month = month % 12
        return DateUtils.safe_create_from_min_value(year, month, day)

    @staticmethod
    def total_hours(from_date: datetime, to_date: datetime) -> int:
        return round((to_date - from_date).total_seconds() / 3600)

    @staticmethod
    def day_of_year(seed: datetime) -> int:
        return seed.timetuple().tm_yday

    @staticmethod
    def last_day_of_month(year: int, month: int) -> int:
        return calendar.monthrange(year, month)[1]

    @staticmethod
    def week_of_year(date: datetime) -> int:
        return date.isocalendar()[1]

    @staticmethod
    def is_leap_year(year) -> bool:
        return (year % 4 == 0) and (year % 100 != 0) or (year % 400 == 0)

    @staticmethod
    def is_Feb_29th(year, month, day):
        return month == 2 and day == 29

    @staticmethod
    def is_Feb_29th_datetime(date: datetime):
        return date.month == 2 and date.day == 29


class HolidayFunctions:

    @staticmethod
    def calculate_holiday_by_easter(year: int, days: int = 0) -> datetime:

        day = 0
        month = 3

        g = year % 19
        c = year / 100

        h = (c - int(c / 4) - int(((8 * c) + 13) / 25) + (19 * g) + 15) % 30
        i = h - (int(h / 28) * (1 - (int(h / 28) * int(29 / (h + 1)) * int((21 - g) / 11))))
        day = i - ((year + int(year / 4) + i + 2 - c + int(c / 4)) % 7) + 28

        if day > 31:
            month += 1
            day -= 31

        return DateUtils.safe_create_from_min_value(year, month, int(day)) + timedelta(days=days)


class DateTimeUtilityConfiguration(ABC):
    @property
    @abstractmethod
    def since_year_suffix_regex(self) -> Pattern:
        raise NotImplementedError

    @property
    @abstractmethod
    def date_unit_regex(self) -> Pattern:
        raise NotImplementedError

    @property
    @abstractmethod
    def range_prefix_regex(self) -> Pattern:
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
    def in_connector_regex(self) -> Pattern:
        raise NotImplementedError

    @property
    @abstractmethod
    def range_unit_regex(self) -> Pattern:
        raise NotImplementedError

    @property
    @abstractmethod
    def am_desc_regex(self) -> Pattern:
        raise NotImplementedError

    @property
    @abstractmethod
    def pm_desc__regex(self) -> Pattern:
        raise NotImplementedError

    @property
    @abstractmethod
    def am_pm_desc_regex(self) -> Pattern:
        raise NotImplementedError

    @property
    @abstractmethod
    def time_unit_regex(self) -> Pattern:
        raise NotImplementedError

    @property
    @abstractmethod
    def within_next_prefix_regex(self) -> Pattern:
        raise NotImplementedError

    @property
    @abstractmethod
    def common_date_prefix_regex(self) -> Pattern:
        raise NotImplementedError

    @property
    @abstractmethod
    def check_both_before_after(self) -> Pattern:
        raise NotImplementedError


class MatchedIndex:
    def __init__(self, matched: bool, index: int):
        self.matched = matched
        self.index = index


class MatchingUtil:

    @staticmethod
    def pre_process_text_remove_superfluous_words(text: str, matcher: Pattern):
        superfluous_word_matches = MatchingUtil.remove_sub_matches(matcher.find(text))

        bias = 0[0]

        for match in superfluous_word_matches:
            text = text[match.start - bias: match.length]

            bias += match.length

        return text, superfluous_word_matches

    @staticmethod
    def post_process_recover_superfluous_words(extract_results: List[ExtractResult], superfluous_word_matches,
                                               origin_text: str):
        for match in superfluous_word_matches:
            for extract_result in extract_results:

                extract_result_end = extract_result.start + extract_result.length
                if extract_result.start < match.start <= extract_result_end:
                    extract_result.length += len(match)

                if match.start <= extract_result.start:
                    extract_result.start += len(match)

        for extract_result in extract_results:
            extract_result.text = origin_text[extract_result.start: extract_result.start + extract_result.length]

        return extract_results

    @staticmethod
    def remove_sub_matches(match_results: List[MatchResult]):
        match_list = list(match_results)

        match_list = (list(filter(lambda item: not any(list(filter(
            lambda ritem: (ritem.start < item.start and ritem.end >= item.end) or (
                ritem.start <= item.start and ritem.end > item.end), match_list))), match_list)))

        return match_list

    @staticmethod
    def get_ago_later_index(source: str, regexp: Pattern, in_suffix) -> MatchedIndex:
        result = MatchedIndex(matched=False, index=-1)
        trimmed_source = source.strip().lower()
        match = RegExpUtility.match_begin(regexp, trimmed_source, True) if in_suffix else\
            RegExpUtility.match_end(regexp, trimmed_source, True)

        if match and match.success:
            result.index = source.lower().find(match.group()) + (match.length if in_suffix else 0)
            result.matched = True

        return result

    @staticmethod
    def contains_ago_later_index(source: str, regexp: Pattern, in_suffix) -> bool:
        return MatchingUtil.get_ago_later_index(source, regexp, in_suffix).matched

    @staticmethod
    def get_term_index(source: str, regexp: Pattern) -> MatchedIndex:
        result = MatchedIndex(matched=False, index=-1)
        referenced_match = regex.search(
            regexp, source.strip().lower().split(' ').pop())

        if referenced_match:
            result = MatchedIndex(matched=True, index=len(
                source) - source.lower().rfind(referenced_match.group()))

        return result

    @staticmethod
    def contains_term_index(source: str, regexp: Pattern) -> bool:
        return MatchingUtil.get_term_index(source, regexp).matched


class AgoLaterMode(Enum):
    DATE = 0
    DATETIME = 1


class AgoLaterUtil:
    @staticmethod
    def extractor_duration_with_before_and_after(source: str, extract_result: ExtractResult,
                                                 ret: List[Token], config: DateTimeUtilityConfiguration) -> List[Token]:
        pos = extract_result.start + extract_result.length
        index = 0
        if pos <= len(source):
            after_string = source[pos:]
            before_string = source[0: extract_result.start]
            is_time_duration = config.time_unit_regex.search(extract_result.text)
            ago_later_regexes = [config.ago_regex, config.later_regex]
            is_match = False

            for regexp in ago_later_regexes:
                token_after = token_before = None
                is_day_match = False
                # Check after_string
                if MatchingUtil.get_ago_later_index(after_string, regexp, True).matched:
                    # We don't support cases like "5 minutes from today" for now
                    # Cases like "5 minutes ago" or "5 minutes from now" are supported
                    # Cases like "2 days before today" or "2 weeks from today" are also supported

                    is_day_match = RegExpUtility.get_group(
                        regexp.match(after_string), Constants.DAY_GROUP_NAME)

                    index = MatchingUtil.get_ago_later_index(
                        after_string, regexp, True).index

                    if not(is_time_duration and is_day_match):
                        token_after = Token(extract_result.start, extract_result.start +
                                            extract_result.length + index)
                        is_match = True

                if config.check_both_before_after:
                    before_after_str = before_string + after_string
                    is_range_match = RegExpUtility.match_begin(config.range_prefix_regex, after_string[:index], True)
                    index_start = MatchingUtil.get_ago_later_index(before_after_str, regexp, False)
                    if not is_range_match and index_start.matched:
                        is_day_match = regexp.match(before_after_str)

                        if is_day_match and not (is_time_duration and is_day_match):
                            ret.append(Token(index_start.index, (extract_result.start + extract_result.length or 0) + index))
                            is_match = True
                    index = MatchingUtil.get_ago_later_index(before_string, regexp, False).index
                    if MatchingUtil.get_ago_later_index(before_string, regexp, False).matched:
                        is_day_match = RegExpUtility.get_group(regexp.match(before_string), 'day')

                        if not (is_day_match and is_time_duration):
                            token_before = Token(index, extract_result.start + extract_result.length or 0)
                            is_match = True

                if token_after is not None and token_before is not None and token_before.start + token_before.length > token_after.start:
                    ret.append(Token(token_before.start, token_after.start + token_after.length - token_before.start))
                elif token_after is not None:
                    ret.append(token_after)
                elif token_before is not None:
                    ret.append(token_before)
                if is_match:
                    break

            if not is_match:
                in_within_regex_tuples = [
                    (config.in_connector_regex, [config.range_unit_regex]),
                    (config.within_next_prefix_regex, [config.date_unit_regex, config.time_unit_regex])
                ]

                for regexp in in_within_regex_tuples:
                    is_match_after = False
                    index = MatchingUtil.get_term_index(before_string, regexp[0]).index
                    if index > 0:
                        is_match = True
                    elif config.check_both_before_after and\
                            MatchingUtil.get_ago_later_index(after_string, regexp[0], True).matched:
                        is_match = is_match_after = True

                    if is_match:
                        is_unit_match = False
                        for unit_regex in regexp[1]:
                            is_unit_match = is_unit_match or unit_regex.match(extract_result.text)

                        if not is_unit_match:
                            if extract_result.start is not None and extract_result.length is not None and\
                                    extract_result.start >= index or is_match_after:
                                start = extract_result.start - (index if not is_match_after else 0)
                                end = extract_result.start + extract_result.length + (index if is_match_after else 0)
                                ret.append(Token(start, end))
                        break
        return ret

    @staticmethod
    def parse_duration_with_ago_and_later(source: str, reference: datetime,
                                          duration_extractor: DateTimeExtractor,
                                          duration_parser: DateTimeParser,
                                          unit_map: Dict[str, str],
                                          unit_regex: Pattern,
                                          utility_configuration: DateTimeUtilityConfiguration)\
            -> DateTimeResolutionResult:
        result = DateTimeResolutionResult()

        if duration_extractor:
            duration_extract = duration_extractor.extract(source, reference)
        else:
            return result

        if not duration_extract:
            return result

        duration = next(iter(duration_extract))
        pr = duration_parser.parse(duration, reference)

        if not pr:
            return result

        match = regex.search(unit_regex, source)
        if not match:
            return result

        after_str = source[duration.start + duration.length:]
        before_str = source[0:duration.start]
        src_unit = match.group(Constants.UNIT)
        duration_result: DateTimeResolutionResult = pr.value
        num_str = duration_result.timex[0:len(
            duration_result.timex) - 1].replace(Constants.UNIT_P, '').replace(Constants.UNIT_T, '')
        num = int(num_str)

        mode = AgoLaterMode.DATE
        if pr.timex_str.__contains__("T"):
            mode = AgoLaterMode.DATETIME

        if pr.value:
            return AgoLaterUtil.get_ago_later_result(
                pr, num, unit_map, src_unit, after_str, before_str, reference,
                utility_configuration, mode)

        return result

    @staticmethod
    def __matched_string(regexp, string):
        is_match = True
        match = regexp.match(string)
        day_str = match.group('day')

        return is_match, match, day_str

    @staticmethod
    def get_ago_later_result(
            duration_parse_result: DateTimeParseResult, num: int,
            unit_map: Dict[str, str], src_unit: str, after_str: str,
            before_str: str, reference: datetime,
            utility_configuration: DateTimeUtilityConfiguration, mode: AgoLaterMode):

        result = DateTimeResolutionResult()
        unit_str = unit_map.get(src_unit)

        if not unit_str:
            return result

        contains_ago = MatchingUtil.contains_ago_later_index(
            after_str, utility_configuration.ago_regex, True)
        contains_later_or_in = MatchingUtil.contains_ago_later_index(
            after_str, utility_configuration.later_regex, False) or\
            MatchingUtil.contains_term_index(before_str, utility_configuration.in_connector_regex)

        if contains_ago:
            result = AgoLaterUtil.get_date_result(
                unit_str, num, reference, False, mode)
            duration_parse_result.value.mod = TimeTypeConstants.BEFORE_MOD
            result.sub_date_time_entities = [duration_parse_result]
            return result

        if contains_later_or_in:
            result = AgoLaterUtil.get_date_result(
                unit_str, num, reference, True, mode)
            duration_parse_result.value.mod = TimeTypeConstants.AFTER_MOD
            result.sub_date_time_entities = [duration_parse_result]
            return result

        return result

    @staticmethod
    def get_date_result(
            unit_str: str, num: int, reference: datetime, is_future: bool,
            mode: AgoLaterMode) -> DateTimeResolutionResult:
        value = reference
        result = DateTimeResolutionResult()
        swift = 1 if is_future else -1

        if unit_str == Constants.UNIT_D:
            value += timedelta(days=num * swift)
        elif unit_str == Constants.UNIT_W:
            value += timedelta(days=num * swift * 7)
        elif unit_str == Constants.UNIT_MON:
            value += datedelta(months=num * swift)
        elif unit_str == Constants.UNIT_Y:
            value += datedelta(years=num * swift)
        elif unit_str == Constants.UNIT_H:
            value += timedelta(hours=num * swift)
        elif unit_str == Constants.UNIT_M:
            value += timedelta(minutes=num * swift)
        elif unit_str == Constants.UNIT_S:
            value += timedelta(seconds=num * swift)
        else:
            return result

        result.timex = DateTimeFormatUtil.luis_date_from_datetime(
            value) if mode == AgoLaterMode.DATE else DateTimeFormatUtil.luis_date_time(value)
        result.future_value = value
        result.past_value = value
        result.success = True
        return result


class DateContext:
    year: int = Constants.INVALID_YEAR

    # This method is to ensure the begin date is less than the end date. As DateContext only supports common Year as
    # context, so it subtracts one year from beginDate.
    # @TODO problematic in other usages.
    @staticmethod
    def swift_date_object(begin_date: datetime, end_date: datetime) -> datetime:
        if begin_date > end_date:
            begin_date = begin_date - datedelta(years=1)
        return begin_date

    def process_date_entity_parsing_result(self, original_result: DateTimeParseResult) -> DateTimeParseResult:
        if not self.is_empty():
            original_result.timex_str = TimexUtil.set_timex_with_context(original_result.timex_str, self)
            original_result.value = self.process_date_entity_resolution(original_result.value)

        return original_result

    def process_date_entity_resolution(self, resolution_result: DateTimeResolutionResult) -> DateTimeResolutionResult:
        if not self.is_empty():
            resolution_result.timex = TimexUtil.set_timex_with_context(resolution_result.timex, self)
            resolution_result.future_value = self.__set_date_with_context(resolution_result.future_value)
            resolution_result.past_value = self.__set_date_with_context(resolution_result.past_value)
        return resolution_result

    def process_date_period_entity_resolution(self, resolution_result: DateTimeResolutionResult) -> DateTimeResolutionResult:
        if not self.is_empty():
            resolution_result.timex = TimexUtil.set_timex_with_context(resolution_result.timex, self)
            resolution_result.future_value = self.__set_date_range_with_context(resolution_result.future_value)
            resolution_result.past_value = self.__set_date_range_with_context(resolution_result.past_value)
        return resolution_result

    def is_empty(self) -> bool:
        return self.year == Constants.INVALID_YEAR

    def __set_date_with_context(self, original_date, year=-1) -> datetime:
        if not DateUtils.is_valid_datetime(original_date):
            return original_date
        value = DateUtils.safe_create_from_min_value(year=self.year if year == -1 else year, month=original_date.month, day=original_date.day)
        return value

    def __set_date_range_with_context(self, original_date_range):
        start_date = self.__set_date_with_context(original_date_range[0])
        end_date = self.__set_date_with_context(original_date_range[1])
        result = {
            TimeTypeConstants.START_DATE: start_date,
            TimeTypeConstants.END_DATE: end_date
        }

        return result

    # This method is to ensure the year of begin date is same with the end date in no year situation.
    def sync_year(self, pr1, pr2):
        if self.is_empty():
            if DateUtils.is_Feb_29th_datetime(pr1.value.future_value):
                future_year = pr1.value.future_value.year
                past_year = pr1.value.past_value.year
                pr2.value = self.sync_year_resolution(pr2.value, future_year, past_year)
            elif DateUtils.is_Feb_29th_datetime(pr2.value.future_value):
                future_year = pr2.value.future_value.year
                past_year = pr2.value.past_value.year
                pr1.value = self.sync_year_resolution(pr1.value, future_year, past_year)
        return pr1, pr2

    def sync_year_resolution(self, resolution_result, future_year, past_year):
        resolution_result.future_value = self.__set_date_with_context(resolution_result.future_value, future_year)
        resolution_result.past_value = self.__set_date_with_context(resolution_result.past_value, past_year)
        return resolution_result


date_period_timex_type_to_suffix = {
    0: Constants.TIMEX_DAY,
    1: Constants.TIMEX_WEEK,
    2: Constants.TIMEX_MONTH,
    3: Constants.TIMEX_YEAR,
}


class RangeTimexComponents:
    def __init__(self):
        self.begin_timex = ''
        self.end_timex = ''
        self.duration_timex = ''
        self.is_valid = False


class TimexUtil:

    @staticmethod
    def merge_timex_alternatives(timex1: str, timex2: str) -> str:
        if timex1 == timex2:
            return timex1
        return f"{timex1}{Constants.COMPOSTIE_TIMEX_DELIMITER}{timex2}"

    @staticmethod
    def parse_time_of_day(tod: str) -> TimeOfDayResolution:
        result = TimeOfDayResolution()

        if tod == Constants.EARLY_MORNING:
            result.timex = Constants.EARLY_MORNING
            result.begin_hour = 4
            result.end_hour = 8
        elif tod == Constants.MORNING:
            result.timex = Constants.MORNING
            result.begin_hour = 8
            result.end_hour = 12
        elif tod == Constants.MID_DAY:
            result.timex = Constants.MID_DAY
            result.begin_hour = 11
            result.end_hour = 13
        elif tod == Constants.AFTERNOON:
            result.timex = Constants.AFTERNOON
            result.begin_hour = 12
            result.end_hour = 16
        elif tod == Constants.EVENING:
            result.timex = Constants.EVENING
            result.begin_hour = 16
            result.end_hour = 20
        elif tod == Constants.DAYTIME:
            result.timex = Constants.DAYTIME
            result.begin_hour = 8
            result.end_hour = 18
        elif tod == Constants.BUSINESS_HOUR:
            result.timex = Constants.BUSINESS_HOUR
            result.begin_hour = 8
            result.end_hour = 18
        elif tod == Constants.NIGHT:
            result.timex = Constants.NIGHT
            result.begin_hour = 20
            result.end_hour = 23
            result.end_min = 59

        return result

    @staticmethod
    def set_timex_with_context(timex: str, context: DateContext) -> str:
        result = timex.replace(Constants.TIMEX_FUZZY_YEAR, str(context.year))
        return result

    @staticmethod
    def generate_date_period_timex_unit_count(begin, end, timex_type, equal_duration_length=True):
        unit_count = 'XX'

        if equal_duration_length:
            if timex_type == 0:
                unit_count = (end - begin).days

            if timex_type == 1:
                unit_count = (end - begin).days/7
            if timex_type == 2:
                unit_count = ((end.year - begin.year) * 12) + (end.month - begin.month)
            if timex_type == 3:
                unit_count = (end.year - begin.year) + ((end.mont - begin.month) / 12.0)
        return unit_count

    @staticmethod
    def generate_date_period_timex_str(begin, end, timex_type, timex1, timex2):
        boundary_valid = DateUtils.is_valid_datetime(begin) and DateUtils.is_valid_datetime(end)
        unit_count = TimexUtil.generate_date_period_timex_unit_count(begin, end, timex_type) if boundary_valid else "X"
        return f"({timex1},{timex2},P{unit_count}{date_period_timex_type_to_suffix[timex_type]})"

    @staticmethod
    def generate_date_period_timex(begin, end, timex_type, alternative_begin=datetime.now(), alternative_end=datetime.now()):
        equal_duration_length = (end - begin).days == (alternative_end - alternative_begin).days or datetime.now() == alternative_end == alternative_begin
        unit_count = TimexUtil.generate_date_period_timex_unit_count(begin, end, timex_type, equal_duration_length)
        date_period_timex = f'P{unit_count}{date_period_timex_type_to_suffix[timex_type]}'

        return f'({DateTimeFormatUtil.luis_date(begin.year, begin.month, begin.day)},' \
               f'{DateTimeFormatUtil.luis_date(end.year, end.month, end.day)},{date_period_timex})'

    @staticmethod
    def _process_double_timex(resolution_dic: Dict[str, object], future_key: str, past_key: str, origin_timex: str):
        timexes = origin_timex.split(Constants.COMPOSTIE_TIMEX_DELIMITER)
        if not future_key in resolution_dic or not past_key in resolution_dic or len(timexes) != 2:
            return
        future_resolution = resolution_dic[future_key]
        past_resolution = resolution_dic[past_key]
        future_resolution[Constants.TIMEX_KEY] = timexes[0]
        past_resolution[Constants.TIMEX_KEY] = timexes[1]

    @staticmethod
    def _has_double_timex(comment: str):
        return comment == Constants.COMMENT_DOUBLETIMEX

    @staticmethod
    def is_range_timex(timex: str) -> bool:
        return timex and timex.startswith("(")

    @staticmethod
    def get_range_timex_components(range_timex: str) -> RangeTimexComponents:
        range_timex = range_timex.replace('(', '').replace(')', '')
        components = range_timex.split(',')
        result = RangeTimexComponents()
        if len(components) == 3:
            result.begin_timex = components[0]
            result.end_timex = components[1]
            result.duration_timex = components[2]
            result.is_valid = True

        return result

    @staticmethod
    def combine_date_and_time_timex(date_timex: str, time_timex: str):
        return f'{date_timex}{time_timex}'

    @staticmethod
    def generate_date_time_period_timex(begin_timex: str, end_timex: str, duration_timex: str):
        return f'({begin_timex},{end_timex},{duration_timex})'


class TimeZoneResolutionResult:
    def __init__(self):
        self.value: str = ''
        self.utc_offset_mins: int = 0
        self.time_zone_text: str = ''


def parse_chinese_dynasty_year(year_str: str, dynasty_year_regex: Pattern, dynasty_start_year: str, dynasty_year_map: dict, integer_extractor, number_parser):
    dynasty_year_match = regex.search(dynasty_year_regex, year_str)
    if dynasty_year_match and dynasty_year_match.start() == 0 and len(dynasty_year_match.group()) == len(year_str):
        # handle "康熙元年" refer to https://zh.wikipedia.org/wiki/%E5%B9%B4%E5%8F%B7
        dynasty_str = RegExpUtility.get_group(dynasty_year_match, "dynasty")
        bias_year_str = RegExpUtility.get_group(dynasty_year_match, "biasYear")
        basic_year = dynasty_year_map[dynasty_str]
        if bias_year_str == dynasty_start_year:
            bias_year = 1
        else:
            er = next(iter(integer_extractor.extract(bias_year_str)), None)
            bias_year = int(number_parser.parse(er).value)
        year = int(basic_year + bias_year - 1)
        return year
    return None
