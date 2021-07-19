#  Copyright (c) Microsoft Corporation. All rights reserved.
#  Licensed under the MIT License.

from abc import abstractmethod
from typing import List, Optional, Pattern, Dict, Match
from datetime import datetime
import regex

from recognizers_text.utilities import RegExpUtility
from recognizers_text.extractor import ExtractResult
from .constants import Constants, TimeTypeConstants
from .extractors import DateTimeExtractor
from .parsers import DateTimeParser, DateTimeParseResult
from .utilities import DateTimeOptionsConfiguration, DateTimeOptions, merge_all_tokens, TimeZoneUtility, RegExpUtility


class TimeExtractorConfiguration(DateTimeOptionsConfiguration):

    @property
    @abstractmethod
    def time_zone_extractor(self) -> DateTimeExtractor:
        raise NotImplementedError

    @property
    @abstractmethod
    def time_regex_list(self) -> List[Pattern]:
        raise NotImplementedError

    @property
    @abstractmethod
    def at_regex(self) -> Pattern:
        raise NotImplementedError

    @property
    @abstractmethod
    def ish_regex(self) -> Pattern:
        raise NotImplementedError

    @property
    @abstractmethod
    def time_before_after_regex(self) -> Pattern:
        raise NotImplementedError


class BaseTimeExtractor(DateTimeExtractor):
    @property
    def extractor_type_name(self) -> str:
        return Constants.SYS_DATETIME_TIME

    def __init__(self, config: TimeExtractorConfiguration):
        self.config = config

    def extract(self, source: str, reference: datetime = None) -> List[ExtractResult]:

        if reference is None:
            reference = datetime.now()

        tokens = self.basic_regex_match(source)
        tokens.extend(self.at_regex_match(source))
        tokens.extend(self.before_after_regex_match(source))
        tokens.extend(self.specials_regex_match(source))

        result = merge_all_tokens(tokens, source, self.extractor_type_name)

        if (self.config.options & DateTimeOptions.ENABLE_PREVIEW) != 0:
            result = TimeZoneUtility().merge_time_zones(
                result,
                self.config.time_zone_extractor.extract(source, reference),
                source
            )

        return result

    def before_after_regex_match(self, source: str) -> []:
        from recognizers_date_time import DateTimeOptions
        from .utilities import Token
        result: List[Token] = list()

        if (self.config.options & DateTimeOptions.CALENDAR) != 0:

            before_after_regex = self.config.time_before_after_regex
            if before_after_regex.search(source):
                matches: Match = before_after_regex.match(source)
                for match in matches:
                    result.append(Token(source.index(match.group()), source.index(match.group()) +
                                        (match.end() - match.start())))

        return result

    @staticmethod
    def lth_check(match: Match) -> bool:

        match_val = match.group()

        lth = None
        if match.re.groupindex.keys().__contains__('lth'):
            lth = match.group('lth')

        return (lth is None) or (len(lth) != len(match_val) and not (len(match_val) == len(lth) + 1 and match_val.endswith(' ')))

    def basic_regex_match(self, source: str) -> []:
        from .utilities import Token
        result: List[Token] = list()

        for pattern in self.config.time_regex_list:
            matches = list(regex.finditer(pattern, source))

            # @TODO Workaround to avoid incorrect partial-only matches. Remove after time regex reviews across languages.
            matches = list(filter(lambda match: self.lth_check(match), matches))

            result.extend(map(lambda x: Token(x.start(), x.end()), matches))

        return result

    def at_regex_match(self, source: str) -> []:
        from .utilities import Token
        result: List[Token] = list()
        matches = list(filter(lambda x: x.group(),
                              regex.finditer(self.config.at_regex, source)))

        for match in matches:
            if match.end() < len(source) and source[match.end()] == '%':
                continue
            result.append(Token(match.start(), match.end()))

        return result

    def specials_regex_match(self, source: str) -> []:
        from .utilities import Token
        result: List[Token] = list()
        if self.config.ish_regex:
            matches = list(filter(lambda x: x.group(),
                                  regex.finditer(self.config.ish_regex, source)))
            result.extend(map(lambda x: Token(x.start(), x.end()), matches))
        return result


class AdjustParams:
    def __init__(self, hour: int = 0, minute: int = 0, has_minute: bool = False,
                 has_am: bool = False, has_pm: bool = False):
        self.hour = hour
        self.minute = minute
        self.has_minute = has_minute
        self.has_am = has_am
        self.has_pm = has_pm


class TimeParserConfiguration:
    @property
    @abstractmethod
    def time_token_prefix(self) -> str:
        raise NotImplementedError

    @property
    @abstractmethod
    def at_regex(self) -> Pattern:
        raise NotImplementedError

    @property
    @abstractmethod
    def time_regexes(self) -> List[Pattern]:
        raise NotImplementedError

    @property
    @abstractmethod
    def numbers(self) -> Dict[str, int]:
        raise NotImplementedError

    @property
    @abstractmethod
    def utility_configuration(self):
        raise NotImplementedError

    @property
    @abstractmethod
    def time_zone_parser(self) -> DateTimeParser:
        raise NotImplementedError

    @abstractmethod
    def adjust_by_prefix(self, prefix: str, adjust: AdjustParams):
        raise NotImplementedError

    @abstractmethod
    def adjust_by_suffix(self, suffix: str, adjust: AdjustParams):
        raise NotImplementedError


class BaseTimeParser(DateTimeParser):
    @property
    def parser_type_name(self) -> str:
        return Constants.SYS_DATETIME_TIME

    def __init__(self, config: TimeParserConfiguration):
        self.config = config

    def parse(self, source: ExtractResult, reference: datetime = None) -> Optional[DateTimeParseResult]:
        from .utilities import DateTimeFormatUtil
        if reference is None:
            reference = datetime.now()

        result = DateTimeParseResult(source)

        if source.type is self.parser_type_name:
            source_text = source.text.lower()

            inner_result = self.internal_parser(source_text, reference)
            if inner_result.success:
                inner_result.future_resolution[TimeTypeConstants.TIME] = DateTimeFormatUtil.format_time(
                    inner_result.future_value)
                inner_result.past_resolution[TimeTypeConstants.TIME] = DateTimeFormatUtil.format_time(
                    inner_result.past_value)
                result.value = inner_result
                result.timex_str = inner_result.timex if inner_result is not None else ''
                result.resolution_str = ''

        return result

    def internal_parser(self, source: str, reference: datetime):
        inner_result = self.parse_basic_regex_match(source, reference)
        return inner_result

    def parse_basic_regex_match(self, source: str, reference: datetime):
        from .utilities import DateTimeResolutionResult, DateUtils
        source = source.strip().lower()
        offset = 0
        match = regex.search(self.config.at_regex, source)
        if not match:
            match = regex.search(self.config.at_regex,
                                 self.config.time_token_prefix + source)
            offset = len(self.config.time_token_prefix)

        if match is not None and match.start() == offset and match.group() == source:
            return self.match_to_time(match, reference)

        hour = self.config.numbers.get(source, -1)
        if 0 <= hour <= 24:
            result = DateTimeResolutionResult()
            if hour == 24:
                hour = 0
            if hour <= 12 and hour != 0:
                result.comment = Constants.AM_PM_GROUP_NAME

            result.timex = f'T{hour:02d}'
            result.future_value = DateUtils.safe_create_from_min_value(
                reference.year, reference.month, reference.day, hour, 0, 0)
            result.past_value = result.future_value
            result.success = True
            return result

        for pattern in self.config.time_regexes:
            offset = 0
            match = RegExpUtility.exact_match(pattern, source, True)
            if match and match.success:
                return self.match_to_time(match, reference)

        return DateTimeResolutionResult()

    def match_to_time(self, match: Match, reference: datetime):
        from .utilities import DateTimeResolutionResult

        result = DateTimeResolutionResult()
        hour = 0
        minute = 0
        second = 0
        day = reference.day
        month = reference.month
        year = reference.year
        has_minute = False
        has_seconds = False
        has_am = False
        has_pm = False
        has_mid = False

        written_time_str = RegExpUtility.get_group(match, Constants.WRITTEN_TIME)
        if written_time_str.strip():
            # get hour
            hour_str = RegExpUtility.get_group(match, Constants.HOUR_NUM_GROUP_NAME)
            hour_str = hour_str.lower()
            hour = self.config.numbers[hour_str]

            # get minute
            min_str = RegExpUtility.get_group(match, Constants.MIN_NUM)
            tens_str = RegExpUtility.get_group(match, Constants.TENS_GROUP_NAME)

            if min_str.strip():
                minute = self.config.numbers[min_str]
                if tens_str.strip():
                    minute += self.config.numbers[tens_str]
                has_minute = True
        elif str.strip(RegExpUtility.get_group(match, Constants.MID)):
            has_mid = True
            if str.strip(RegExpUtility.get_group(match, Constants.MID_NIGHT)):
                hour = 0
                minute = 0
                second = 0
            elif str.strip(RegExpUtility.get_group(match, Constants.MID_MORNING)):
                hour = 10
                minute = 0
                second = 0
            elif str.strip(RegExpUtility.get_group(match, Constants.MID_AFTERNOON)):
                hour = 14
                minute = 0
                second = 0
            elif str.strip(RegExpUtility.get_group(match, Constants.MIDDAY)):
                hour = 12
                minute = 0
                second = 0
        else:
            # get hour
            hour_str = RegExpUtility.get_group(match, Constants.HOUR_GROUP_NAME)
            if hour_str.strip() == '':
                hour_str = RegExpUtility.get_group(match, Constants.HOUR_NUM_GROUP_NAME)
                hour_str = hour_str.lower()
                hour = self.config.numbers.get(hour_str, None)
                if hour is None:
                    return result
            else:
                hour = int(hour_str) if hour_str.isnumeric(
                ) else self.config.numbers.get(hour_str, None)
                if not hour:
                    return result
            # get minute
            min_str = RegExpUtility.get_group(match, Constants.MINUTE_GROUP_NAME)
            min_str = min_str.lower()
            if min_str.strip() == '':
                min_str = RegExpUtility.get_group(match, Constants.MIN_NUM)
                if min_str.strip():
                    minute = self.config.numbers[min_str]
                    has_minute = True
                tens_str = RegExpUtility.get_group(match, Constants.TENS_GROUP_NAME)
                if tens_str.strip():
                    minute += self.config.numbers[tens_str]
                    has_minute = True
            else:
                minute = int(min_str)
                has_minute = True
            # get second
            sec_str = RegExpUtility.get_group(match, Constants.SECOND_GROUP_NAME)
            sec_str = sec_str.lower()
            if sec_str.strip():
                second = int(sec_str)
                has_seconds = True

        # adjust by desc string
        desc_str = RegExpUtility.get_group(match, Constants.DESC_GROUP_NAME).lower()

        if any([regex.search(self.config.utility_configuration.am_desc_regex, desc_str),
                regex.search(self.config.utility_configuration.am_pm_desc_regex, desc_str)]) or RegExpUtility.get_group(
                match, Constants.IMPLICIT_AM_GROUP_NAME):
            if hour >= 12:
                hour -= 12
            if regex.search(self.config.utility_configuration.am_pm_desc_regex, desc_str) is None:
                has_am = True
        elif regex.search(self.config.utility_configuration.pm_desc__regex,
                          desc_str) is not None or RegExpUtility.get_group(match, Constants.IMPLICIT_PM_GROUP_NAME):
            if hour < 12:
                hour += 12
            has_pm = True

        # adjust min by prefix
        time_prefix = RegExpUtility.get_group(match, Constants.PREFIX_GROUP_NAME)
        time_prefix = time_prefix.lower()
        if time_prefix.strip():
            adjust = AdjustParams(hour, minute, has_minute)
            self.config.adjust_by_prefix(time_prefix, adjust)
            hour = adjust.hour
            minute = adjust.minute
            has_minute = adjust.has_minute

        # adjust min by suffix
        time_suffix = RegExpUtility.get_group(match, Constants.SUFFIX_GROUP_NAME)
        time_suffix = time_suffix.lower()
        if time_suffix.strip():
            adjust = AdjustParams(hour, minute, has_minute, has_am, has_pm)
            self.config.adjust_by_suffix(time_suffix, adjust)
            hour = adjust.hour
            minute = adjust.minute
            has_minute = adjust.has_minute
            has_am = adjust.has_am
            has_pm = adjust.has_pm

        if hour == 24:
            hour = 0

        result.timex = f'T{hour:02d}'
        if has_minute:
            result.timex += f':{minute:02d}'

        if has_seconds:
            result.timex += f':{second:02d}'

        if hour <= 12 and not has_pm and not has_am and not has_mid:
            result.comment = Constants.AM_PM_GROUP_NAME

        result.future_value = datetime(year, month, day, hour, minute, second)
        result.past_value = result.future_value
        result.success = True

        return result
