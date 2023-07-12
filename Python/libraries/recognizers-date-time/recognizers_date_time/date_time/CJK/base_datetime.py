from abc import abstractmethod
from typing import List, Optional, Pattern, Dict, Match
from datetime import datetime, timedelta
from collections import namedtuple
import regex

from recognizers_text import MetaData
from recognizers_date_time.date_time.date_extractor import DateExtractor
from recognizers_text.extractor import ExtractResult, Extractor
from recognizers_number.number.extractors import BaseNumberExtractor
from recognizers_number.number.parsers import BaseNumberParser
from recognizers_date_time.date_time.constants import Constants, TimeTypeConstants
from recognizers_number.number.constants import Constants as NumConstants
from recognizers_date_time.date_time.extractors import DateTimeExtractor
from recognizers_date_time.date_time.parsers import DateTimeParser, DateTimeParseResult
from recognizers_date_time.date_time.utilities import Token, merge_all_tokens, DateTimeResolutionResult, \
    DateTimeFormatUtil, RegExpUtility, DateTimeOptionsConfiguration, TimexUtil, DurationParsingUtil, \
    ExtractResultExtension


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
    def ambiguity_date_time_filters(self) -> Dict[Pattern, Pattern]:
        raise NotImplementedError

    @abstractmethod
    def is_connector_token(self, middle):
        raise NotImplementedError


class BaseCJKDateTimeExtractor(Extractor):
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
        # tokens.extend(self.duration_with_ago_and_later(source, reference))

        result = merge_all_tokens(tokens, source, self.extractor_type_name)

        result = ExtractResultExtension.filter_ambiguity(result, source, self.config.ambiguity_date_time_filters)

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
                    break

                middle = source[middle_begin:middle_end].strip().lower()

                if not middle or self.config.is_connector_token(middle) or \
                        RegExpUtility.is_exact_match(self.config.preposition_regex, middle, False):
                    begin = ers[i].start
                    end = ers[j].start + ers[j].length
                    tokens.append(Token(begin, end))
                i = j + 1
                continue
            i = j
        return tokens

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
        match_time_of_today = RegExpUtility.get_matches(self.config.time_of_special_day_regex, source)
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


MatchedTimex = namedtuple('MatchedTimex', ['matched', 'timex'])


class CJKDateTimeParserConfiguration(DateTimeOptionsConfiguration):
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

    @property
    @abstractmethod
    def unit_map(self) -> Dict[str, str]:
        raise NotImplementedError

    @abstractmethod
    def get_matched_now_timex(self, source: str) -> MatchedTimex:
        raise NotImplementedError

    @abstractmethod
    def get_swift_day(self, source: str) -> int:
        raise NotImplementedError

    @abstractmethod
    def adjust_by_time_of_day(self, source: str, hour: int, swift: int) -> None:
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
                inner_result = self.parse_time_of_special_day_regex(source_text, reference)

            # if not inner_result.success:
            #     inner_result = self.parser_duration_with_ago_and_later(source_text, reference)

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

    def filter_results(self, query: str, candidate_results: List[DateTimeParseResult]):
        return candidate_results

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
        match = regex.match(self.config.lunar_regex, source)
        if match:
            return True
        match = regex.match(self.config.lunar_holiday_regex, source)
        if match:
            return True
        return False

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
            future_date.day += timedelta(days=1)
            past_date.day += timedelta(days=1)

        hour = time.hour
        minute = time.minute
        second = time.second

        #  handle morning, afternoon
        if self.config.simple_pm_regex.search(source) and hour < Constants.HALF_DAY_HOUR_COUNT:
            hour += Constants.HALF_DAY_HOUR_COUNT
        elif self.config.simple_am_regex.search(source) and hour >= Constants.HALF_DAY_HOUR_COUNT:
            hour -= Constants.HALF_DAY_HOUR_COUNT

        time_str = pr2.timex_str
        if time_str.endswith(Constants.COMMENT_AMPM):
            time_str = time_str[0:len(time_str) - 4]

        ret.timex = pr1.timex_str + time_str

        val = pr2.value
        if hour <= Constants.HALF_DAY_HOUR_COUNT and not self.config.simple_pm_regex.search(source) and not \
                self.config.simple_am_regex.search(source) and val.comment:
            ret.comment = Constants.COMMENT_AMPM

        ret.future_value = datetime(
            future_date.year, future_date.month, future_date.day, hour, minute, second)
        ret.past_value = datetime(
            past_date.year, past_date.month, past_date.day, hour, minute, second)
        ret.success = True

        return ret

    def parse_time_of_special_day_regex(self, source: str, reference: datetime) -> DateTimeResolutionResult:
        result = DateTimeResolutionResult()
        ers = self.config.time_extractor.extract(source, reference)

        #  Handle 'eod', 'end of day'
        eod = regex.search(self.config.time_of_special_day_regex, source)
        match_ago_and_later = regex.search(self.config.ago_later_regex, source)
        if match_ago_and_later:
            duration_res = self.config.duration_extractor.extract(source, reference)
            pr1 = self.config.duration_parser.parse(duration_res[0], reference)
            is_future = False
            if RegExpUtility.get_group(match_ago_and_later, Constants.LATER_GROUP_NAME):
                is_future = True
            timex = pr1.timex_str

            # handle less and more mode
            if RegExpUtility.get_group(eod, Constants.LESS_GROUP_NAME):
                result.mod = Constants.LESS_THAN_MOD
            elif RegExpUtility.get_group(eod, Constants.MORE_GROUP_NAME):
                result.mod = Constants.MORE_THAN_MOD

            result_datetime = DurationParsingUtil.shift_date_time(timex, reference, future=is_future)
            result.timex = TimexUtil.generate_date_time_timex(result_datetime)
            result.future_value = result.past_value = result_datetime
            result.sub_date_time_entities = [pr1]

            result.success = True
            return result

        if RegExpUtility.get_group(eod, Constants.SPECIFIC_END_OF_GROUP_NAME) and len(ers) == 0:
            result = self.parse_special_time_of_date(source, reference)
            return result

        if eod and len(ers) != 1:
            if RegExpUtility.get_group(eod, Constants.TOMORROW_GROUP_NAME):
                tomorrow_date = reference + timedelta(days=1)
                result = DateTimeFormatUtil.resolve_end_of_day(DateTimeFormatUtil.format_date(tomorrow_date),
                                                               tomorrow_date, tomorrow_date)
            else:
                result = DateTimeFormatUtil.resolve_end_of_day(DateTimeFormatUtil.format_date(reference),
                                                               reference, reference)
            return result

        if len(ers) != 1:
            return result

        pr = self.config.time_parser.parse(ers[0], reference)
        if not pr.value:
            return result

        time = pr.value.future_value

        hour = time.hour
        min = time.minute
        sec = time.second

        match = regex.search(self.config.time_of_special_day_regex, source)
        if match:
            swift = 0
            self.config.adjust_by_time_of_day(match.group(0), hour, swift)
            date = reference + timedelta(days=swift)

            # in this situation, luisStr cannot end up with "ampm", because we always have a "morning" or "night"
            time_str = pr.timex_str
            if time_str.endswith(Constants.COMMENT_AMPM):
                time_str = time_str[0:-4]

            #  handle less and more mode
            if RegExpUtility.get_group(match, Constants.LESS_GROUP_NAME):
                result.mod = Constants.LESS_THAN_MOD
            elif RegExpUtility.get_group(match, Constants.MORE_GROUP_NAME):
                result.mod = Constants.MORE_THAN_MOD

            time_str = Constants.TIME_TIMEX_PREFIX + DateTimeFormatUtil.to_str(hour, 2) + time_str[3:]
            result.timex = DateTimeFormatUtil.format_date(date) + time_str
            result.future_value = datetime(
                date.year, date.month, date.day, hour, min, sec)
            result.past_value = result.future_value
            result.success = True
            return result
        return result

    def parse_special_time_of_date(self, source: str, reference: datetime) -> DateTimeResolutionResult:
        result = DateTimeResolutionResult()
        ers = self.config.date_extractor.extract(source, reference)

        if len(ers) != 1:
            return result

        parse_result = self.config.date_parser.parse(ers[0], reference)
        future_date = parse_result.value.future_value
        past_date = parse_result.value.past_value

        result = DateTimeFormatUtil.resolve_end_of_day(parse_result.timex_str, future_date, past_date)
        return result

    # handle cases like "5分钟前", "1小时以后"
    def parser_duration_with_ago_and_later(self, source: str, reference: datetime) -> DateTimeResolutionResult:
        result = DateTimeResolutionResult()
        duration_result = self.config.duration_extractor.extract(source, reference)

        if len(duration_result) > 0:
            match_ago_later = regex.search(self.config.ago_later_regex, source)

            if match_ago_later:
                parse_result = self.config.duration_parser.parse(duration_result[0], reference)
                is_future = False
                if RegExpUtility.get_group(match_ago_later, Constants.LATER_GROUP_NAME):
                    is_future = True
                timex = parse_result.timex_str

                result_date_time = DurationParsingUtil.shift_date_time(timex, reference, future=is_future)
                result.timex = TimexUtil.generate_date_time_timex(result_date_time)
                result.future_value = result_date_time
                result.past_value = result_date_time
                result.sub_date_time_entities = [parse_result]

                result.success = True

                return result

            match = regex.match(self.config.datetime_period_unit_regex, source)
            if match:
                suffix = source[duration_result[0].start:match.start() + duration_result[0].length].strip()
                src_unit = RegExpUtility.get_group(match, Constants.UNIT_GROUP_NAME)

                number_str = source[duration_result[0].start:match.start() - duration_result[0].start].strip()
                number = self.convert_CJK_to_num(number_str)

                if src_unit in self.config.unit_map:
                    unit_str = self.config.unit_map[src_unit]
                    before_match = regex.search(self.config.before_regex, suffix)
                    if before_match and suffix.startswith(before_match.group()):
                        if unit_str == Constants.TIMEX_HOUR:
                            date = reference + timedelta(hours=-number)
                        elif unit_str == Constants.TIMEX_MINUTE:
                            date = reference + timedelta(minutes=-number)
                        elif unit_str == Constants.TIMEX_SECOND:
                            date = reference + timedelta(seconds=-number)
                        else:
                            return result

                        result.timex = DateTimeFormatUtil.luis_date_from_datetime(date)
                        result.future_value = result.past_value = date
                        result.success = True
                        return result

                    after_match = regex.search(self.config.after_regex, source)
                    if after_match and suffix.startswith(after_match.group()):
                        if unit_str == Constants.TIMEX_HOUR:
                            date = reference + timedelta(hours=number)
                        elif unit_str == Constants.TIMEX_MINUTE:
                            date = reference + timedelta(minutes=number)
                        elif unit_str == Constants.TIMEX_SECOND:
                            date = reference + timedelta(seconds=number)
                        else:
                            return result

                        result.timex = DateTimeFormatUtil.luis_date_from_datetime(date)
                        result.future_value = result.past_value = date
                        result.success = True
                        return result
        return result

    def convert_CJK_to_num(self, num_str: str) -> int:
        num = -1
        er: ExtractResult = next(
            iter(self.config.integer_extractor.extract(num_str)), None)
        if er and er.type == NumConstants.SYS_NUM_INTEGER:
            try:
                num = int(self.config.number_parser.parse(er).value)
            except ValueError:
                num = 0
        return num
