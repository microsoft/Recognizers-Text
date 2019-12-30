import time
import regex
from typing import List, Pattern, Match
from abc import ABC, abstractmethod
from datetime import datetime
from .utilities import DateTimeOptionsConfiguration, DateTimeResolutionResult, TimeZoneResolutionResult, Token,\
    MatchingUtil, DateUtils
from .parsers import DateTimeParser, DateTimeParseResult
from .datetime_zone_extractor import DateTimeZoneExtractor
from .constants import Constants
from ..resources import TimeZoneDefinitions
from recognizers_text import ExtractResult, ParseResult, RegExpUtility, QueryProcessor
from recognizers_text.matcher.string_matcher import StringMatcher


class TimeZoneExtractorConfiguration(DateTimeOptionsConfiguration):
    @property
    @abstractmethod
    def direct_utc_regex(self) -> Pattern:
        raise NotImplementedError

    @property
    @abstractmethod
    def timezone_matcher(self):
        raise NotImplementedError

    @property
    @abstractmethod
    def location_time_suffix_regex(self) -> Pattern:
        raise NotImplementedError

    @property
    @abstractmethod
    def location_matcher(self) -> StringMatcher:
        raise NotImplementedError

    @property
    @abstractmethod
    def ambiguous_timezone_list(self) -> List[str]:
        raise NotImplementedError


class BaseTimeZoneParser(DateTimeParser):
    @property
    def parser_type_name(self) -> str:
        return Constants.SYS_DATETIME_TIMEZONE

    @property
    def time_zone_end_regex(self) -> Pattern:
        return self._time_zone_end_regex

    def __init__(self):
        self._time_zone_end_regex = RegExpUtility.get_safe_reg_exp(
            "time$|timezone$")

    @staticmethod
    def compute_minutes(utc_offset: str) -> int:
        if len(utc_offset) == 0:
            return Constants.INVALID_OFFSET_VALUE

        utc_offset = utc_offset.strip()

        sign = Constants.POSITIVE_SIGN  # later than utc, default value
        if utc_offset.startswith("+") or utc_offset.startswith("-") or utc_offset.startswith("±"):
            if utc_offset.startswith("-"):
                sign = Constants.NEGATIVE_SIGN  # Earlier than utc 0
            utc_offset = utc_offset[1:].strip()

        hour = minutes = 0
        if ':' in utc_offset:
            tokens = list(utc_offset.split(":"))
            hour = int(tokens[0])
            minutes = int(tokens[1])
        elif DateUtils.int_try_parse(utc_offset) != 0:
            hour, is_parsed = DateUtils.int_try_parse(utc_offset)
            minutes = 0

        if hour > Constants.HALF_DAY_HOUR_COUNT:
            return Constants.INVALID_OFFSET_VALUE

        if minutes not in [0, 15, 30, 45, 60]:
            return Constants.INVALID_OFFSET_VALUE

        offset_in_minutes = ((hour * 60) + minutes) * sign

        return offset_in_minutes

    def convert_offset_in_mins_to_offset_string(self, offset_mins: int) -> str:
        return f'UTC{"+" if offset_mins >= 0 else "-"}{self.convert_mins_to_regular_format(abs(offset_mins))}'

    @staticmethod
    def convert_mins_to_regular_format(offset_mins: int) -> str:
        tokens = list((str(offset_mins/60)).split("."))
        hour = int(tokens[0])
        min = int(tokens[1])
        return f'{hour}:{min}'

    def normalize_text(self, text: str) -> str:
        text = regex.sub(r'\s+', ' ', text)
        text = self._time_zone_end_regex.sub('', text)
        return text.strip().lower()

    def parse(self, extract_result: ExtractResult, ref_date: datetime = None) -> DateTimeParseResult:
        datetime_result = DateTimeParseResult()
        datetime_result.start = extract_result.start
        datetime_result.length = extract_result.length
        datetime_result.text = extract_result.text
        datetime_result.type = extract_result.type

        text = extract_result.text
        normalized_text = self.normalize_text(text)
        match = regex.match(TimeZoneDefinitions.DirectUtcRegex, text)
        matched = match.group(2) if match else ''
        offset_minutes = self.compute_minutes(matched)

        if offset_minutes != Constants.INVALID_OFFSET_VALUE:
            datetime_result.value = self.get_datetime_resolution_result(offset_minutes, text)
            datetime_result.resolution_str = f'{Constants.UTC_OFFSET_MINS_KEY}: {offset_minutes}'
        elif normalized_text in TimeZoneDefinitions.AbbrToMinMapping and TimeZoneDefinitions.AbbrToMinMapping[normalized_text] != Constants.INVALID_OFFSET_VALUE:
            utc_minute_shift = TimeZoneDefinitions.AbbrToMinMapping[normalized_text]

            datetime_result.value = self.get_datetime_resolution_result(utc_minute_shift, text)
            datetime_result.resolution_str = f'{Constants.UTC_OFFSET_MINS_KEY}: {utc_minute_shift}'
        elif normalized_text in TimeZoneDefinitions.FullToMinMapping and TimeZoneDefinitions.FullToMinMapping[normalized_text] != Constants.INVALID_OFFSET_VALUE:
            utc_minute_shift = TimeZoneDefinitions.FullToMinMapping[normalized_text]

            datetime_result.value = self.get_datetime_resolution_result(utc_minute_shift, text)
            datetime_result.resolution_str = f'{Constants.UTC_OFFSET_MINS_KEY}: {utc_minute_shift}'
        else:
            datetime_result.value = DateTimeResolutionResult()
            datetime_result.value.success = True

            timezone_resolution = TimeZoneResolutionResult()
            timezone_resolution.value = "UTC+XX:XX"
            timezone_resolution.utc_offset_mins = Constants.INVALID_OFFSET_VALUE
            timezone_resolution.time_zone_text = text

            datetime_result.value.timezone_resolution = timezone_resolution
            datetime_result.resolution_str = Constants.UTC_OFFSET_MINS_KEY + ": XX:XX"

        return datetime_result

    def get_datetime_resolution_result(self, offset_mins: int, text: str) -> DateTimeResolutionResult:
        datetime_resolution = DateTimeResolutionResult()
        datetime_resolution.success = True

        timezone_resolution = TimeZoneResolutionResult()
        timezone_resolution.value = self.convert_offset_in_mins_to_offset_string(offset_mins)
        timezone_resolution.utc_offset_mins = offset_mins
        timezone_resolution.time_zone_text = text

        datetime_resolution.timezone_resolution = timezone_resolution

        return datetime_resolution


class BaseTimeZoneExtractor(DateTimeZoneExtractor):
    @property
    def extractor_type_name(self) -> str:
        return Constants.SYS_DATETIME_TIMEZONE

    def __init__(self, config: TimeZoneExtractorConfiguration):
        self.config = config

    def extract(self, source: str, reference: datetime = None) -> List[ExtractResult]:
        from .utilities import merge_all_tokens
        tokens = []

        normalized_text = QueryProcessor.remove_diacritics(source)

        tokens.extend(self.match_timezones(normalized_text))
        tokens.extend(self.match_location_times(normalized_text, tokens))

        return merge_all_tokens(tokens, source, self.extractor_type_name)

    def remove_ambiguous_time_zone(self, extract_result: List[ExtractResult]) -> List[ExtractResult]:
        return [item for item in extract_result if self.config.ambiguous_time_zone_list in item.text]

    def match_location_times(self, text: str, tokens: List[Token]) -> List[Token]:
        result: List[Token] = list()

        if not self.config.location_time_suffix_regex:
            return result

        time_match = list(regex.finditer(self.config.location_time_suffix_regex, text))

        # Before calling a Find() in location matcher, check if all the matched suffixes by
        # LocationTimeSuffixRegex are already inside tokens extracted by TimeZone matcher.
        # If so, don't call the Find() as they have been extracted by TimeZone matcher, otherwise, call it.
        is_all_suffix_inside_tokens = True
        for match in time_match:
            is_inside = False
            for token in tokens:
                if token.start <= match.start() and token.end >= match.start() + len(match.group()):
                    is_inside = True
                    break

            if not is_inside:
                is_all_suffix_inside_tokens = False

            if not is_all_suffix_inside_tokens:
                break

        if len(time_match) != 0 and not is_all_suffix_inside_tokens:
            last_match_index = time_match[len(time_match) - 1].start()
            sub_str = text[0:last_match_index]
            matches = self.config.location_matcher.find(sub_str)
            location_matches = MatchingUtil.remove_sub_matches(matches)

            i = 0
            for match in time_match:
                has_city_before = False
                while i < len(location_matches) and location_matches[i].end <= match.start():
                    has_city_before = True
                    i += 1

                    if i == len(location_matches):
                        break

                if has_city_before and location_matches[i - 1].end == match.start():
                    result.append(Token(location_matches[i - 1].start, match.start() + len(match.group())))
                if i == len(location_matches):
                    break

        return result

    def match_timezones(self, text: str) -> List[Token]:
        result = []

        # Direct UTC matches
        if self.config.direct_utc_regex:
            direct_utc = list(regex.finditer(self.config.direct_utc_regex, text))
            for match in direct_utc:
                result.append(Token(match.start(), match.start() + len(match.group())))

            matches = self.config.timezone_matcher.find(text)
            for match in matches:
                result.append(Token(match.start, match.start + match.length))

        return result
