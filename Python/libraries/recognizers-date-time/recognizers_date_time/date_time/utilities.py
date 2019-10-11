from enum import Enum, IntEnum, IntFlag
from abc import ABC, abstractmethod
from typing import List, Dict, Pattern, Union
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

    def build_matcher_from_lists(self, collections: []):

        matcher: StringMatcher = StringMatcher(MatchStrategy.TrieTree, NumberWithUnitTokenizer())

        matcher_list = []

        for collection in collections:
            list(map(lambda x: matcher_list.append(x.strip().lower()), collection))

        matcher_list = self.distinct(matcher_list)

        matcher.init(matcher_list)

        return matcher

    @staticmethod
    def distinct(list1):

        unique_list = []
        for x in list1:

            if x not in unique_list:
                unique_list.append(x)

        return unique_list


class RegexExtension:

    @staticmethod
    def match_begin(regex: Pattern, text: str, trim: bool):
        match = regex.match(text)

        if match is None:
            return None

        str_before = text[0: text.index(match.group())]

        if trim:
            str_before = str_before.strip()

        return ConditionalMatch(match, match and (str.isspace(str_before) or str_before is None))

    @staticmethod
    def match_end(regexp: Pattern, text: str, trim: bool):
        match = regex.match(regexp, text)

        if match is None:
            return ConditionalMatch(regexp, False)

        srt_after = text[text.index(match.group()) + (match.end() - match.start()):]

        if trim:
            srt_after = srt_after.strip()

        success = match and (str.isspace(srt_after) or srt_after is None)

        conditional = ConditionalMatch(match, success)

        return conditional

    @staticmethod
    def is_exact_match(regex: Pattern, text: str, trim: bool):
        match = regex.match(text)

        length = len(text.strip()) if trim else len(text)

        return match and len(match.group()) == length


class ConditionalMatch:

    def __init__(self, match: Pattern, success: bool):
        self._match = match,
        self._success = success

    @property
    def match(self) -> Pattern:
        return self._match

    @match.setter
    def match(self, value):
        self._match = value

    @property
    def success(self) -> bool:
        return self._success

    @success.setter
    def success(self, value):
        self._success = value

    @property
    def index(self) -> int:
        return self.match[0].string.index(self.match[0].group())

    @property
    def length(self) -> int:
        return len(self.match[0].group())

    @property
    def value(self) -> str:
        return self.match[0].string

    @property
    def groups(self):
        return self.match[0].groups()


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

    result: List[ExtractResult] = list(
        map(lambda x: __token_to_result(x, source, extractor_name), merged_tokens))
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
    def __init__(self, start=None, end=None):
        self.start = start
        self.end = end


class DateTimeResolutionResult:
    def __init__(self):
        self.success: bool = False
        self.timex: str = ''
        self.is_lunar: bool = False
        self.mod: str = ''
        self.comment: str = ''
        self.future_resolution: Dict[str, str] = dict()
        self.past_resolution: Dict[str, str] = dict()
        self.future_value: object = None
        self.past_value: object = None
        self.sub_date_time_entities: List[object] = list()


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


class DateTimeUtilityConfiguration(ABC):
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


class MatchedIndex:
    def __init__(self, matched: bool, index: int):
        self.matched = matched
        self.index = index


class MatchingUtil:

    @staticmethod
    def pre_process_text_remove_superfluous_words(text: str, matcher: Pattern) -> str:
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
        match_list = list(filter(lambda x: list(
            filter(lambda m: m.start() < x.start + x.length and m.start() +
                   len(m.group()) > x.start, match_results)), match_results))

        if len(match_list) > 0:
            for item in match_results:
                for i in match_list:
                    if item is i:
                        match_results.remove(item)

        return match_results

    @staticmethod
    def get_ago_later_index(source: str, regexp: Pattern) -> MatchedIndex:
        result = MatchedIndex(matched=False, index=-1)
        referenced_matches = regex.match(regexp, source.strip().lower())

        if referenced_matches and referenced_matches.start() == 0:
            result.index = source.lower().find(referenced_matches.group()) + \
                len(referenced_matches.group())
            result.matched = True

        return result

    @staticmethod
    def contains_ago_later_index(source: str, regexp: Pattern) -> bool:
        return MatchingUtil.get_ago_later_index(source, regexp).matched

    @staticmethod
    def get_in_index(source: str, regexp: Pattern) -> MatchedIndex:
        result = MatchedIndex(matched=False, index=-1)
        referenced_match = regex.search(
            regexp, source.strip().lower().split(' ').pop())

        if referenced_match:
            result = MatchedIndex(matched=True, index=len(
                source) - source.lower().rfind(referenced_match.group()))

        return result

    @staticmethod
    def contains_in_index(source: str, regexp: Pattern) -> bool:
        return MatchingUtil.get_in_index(source, regexp).matched


class AgoLaterMode(Enum):
    DATE = 0
    DATETIME = 1


class AgoLaterUtil:
    @staticmethod
    def extractor_duration_with_before_and_after(source: str, extract_result: ExtractResult,
                                                 ret: List[Token], config: DateTimeUtilityConfiguration) -> List[Token]:
        pos = extract_result.start + extract_result.length

        if pos <= len(source):
            after_string = source[pos:]
            before_string = source[0: extract_result.start]
            is_time_duration = config.time_unit_regex.search(extract_result.text)

            if MatchingUtil.get_ago_later_index(after_string, config.ago_regex).matched:
                # We don't support cases like "5 minutes from today" for now
                # Cases like "5 minutes ago" or "5 minutes from now" are supported
                # Cases like "2 days before today" or "2 weeks from today" are also supported
                is_day_match_in_after_string = RegExpUtility.get_group(
                    config.ago_regex.match(after_string), Constants.DAY_GROUP_NAME)

                value = MatchingUtil.get_ago_later_index(
                    after_string, config.ago_regex)

                if not(is_time_duration and is_day_match_in_after_string):
                    ret.append(Token(extract_result.start, extract_result.start +
                                     extract_result.length + value.index))

            elif MatchingUtil.get_ago_later_index(after_string, config.later_regex).matched:

                is_day_match_in_after_string = RegExpUtility.get_group(
                    config.later_regex.search(after_string), Constants.DAY_GROUP_NAME)

                value = MatchingUtil.get_ago_later_index(
                    after_string, config.later_regex)

                if not(is_time_duration and is_day_match_in_after_string):
                    ret.append(Token(extract_result.start, extract_result.start +
                                     extract_result.length + value.index))

            elif MatchingUtil.get_in_index(before_string, config.in_connector_regex).matched:
                # For range unit like "week, month, year", it should output dateRange or datetimeRange
                if not (config.range_unit_regex.search(extract_result.text)):
                    value = MatchingUtil.get_in_index(
                        before_string, config.in_connector_regex)

                    if extract_result.start is not None\
                            and extract_result.length is not None\
                            and extract_result.start >= value.index:
                        ret.append(Token(extract_result.start - value.index,
                                         extract_result.start + extract_result.length))

            elif MatchingUtil.get_in_index(before_string, config.within_next_prefix_regex).matched:

                # For range unit like "week, month, year, day, second, minute, hour",
                # it should output dateRange or datetimeRange
                if not (config.range_unit_regex.search(extract_result.text)) and not config.time_unit_regex.search(
                        extract_result.text):
                    value = MatchingUtil.get_in_index(
                        before_string, config.within_next_prefix_regex)
                    if extract_result.start is not None and extract_result.length is not None and\
                            extract_result.start >= value.index:
                        ret.append(
                            Token(extract_result.start - value.index, extract_result.start + extract_result.length))

        return ret

    @staticmethod
    def parse_duration_with_ago_and_later(source: str, reference: datetime,
                                          duration_extractor: DateTimeExtractor,
                                          duration_parser: DateTimeParser,
                                          unit_map: Dict[str, str],
                                          unit_regex: Pattern,
                                          utility_configuration: DateTimeUtilityConfiguration,
                                          mode: AgoLaterMode) -> DateTimeResolutionResult:
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

        if not num:
            return result

        return AgoLaterUtil.get_ago_later_result(
            pr, num, unit_map, src_unit, after_str, before_str, reference,
            utility_configuration, mode)

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
            after_str, utility_configuration.ago_regex)
        contains_later_or_in = MatchingUtil.contains_ago_later_index(
            after_str, utility_configuration.later_regex) or\
            MatchingUtil.contains_in_index(before_str, utility_configuration.in_connector_regex)

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


class TimexUtil:
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
