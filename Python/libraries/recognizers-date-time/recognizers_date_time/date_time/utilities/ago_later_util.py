from enum import Enum
from typing import List, Dict, Pattern
from datetime import datetime, timedelta

from datedelta import datedelta
import regex

from recognizers_text.extractor import ExtractResult
from recognizers_text.utilities import RegExpUtility
from recognizers_date_time.date_time.constants import TimeTypeConstants, Constants
from recognizers_date_time.date_time.extractors import DateTimeExtractor
from recognizers_date_time.date_time.parsers import DateTimeParser, DateTimeParseResult
from recognizers_date_time.date_time.utilities import Token, DateTimeUtilityConfiguration, MatchingUtil, \
    DateTimeResolutionResult, DateTimeFormatUtil


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