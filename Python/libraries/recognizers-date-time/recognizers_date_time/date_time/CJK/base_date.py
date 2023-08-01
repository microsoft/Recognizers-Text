import calendar
from datetime import datetime, timedelta
from abc import abstractmethod, ABC
from datedelta import datedelta
from dateutil.relativedelta import relativedelta

from typing import List, Pattern, Dict, Match

from recognizers_number.number import CJKNumberParser, Constants as Num_Constants
from recognizers_date_time.date_time import Constants as Date_Constants
from recognizers_date_time.date_time.base_date import BaseDateParser
from recognizers_date_time.date_time.utilities import DateTimeExtractor, DateTimeParser, \
    ExtractResultExtension, merge_all_tokens, DateTimeUtilityConfiguration, DateUtils, DateTimeParseResult, \
    TimeTypeConstants, DateTimeFormatUtil, DateTimeResolutionResult, DurationParsingUtil, DayOfWeek, TimexUtil, Token
from recognizers_date_time.date_time.CJK import CJKCommonDateTimeParserConfiguration
from recognizers_text import ExtractResult, RegExpUtility, MetaData
from recognizers_date_time.date_time.abstract_year_extractor import AbstractYearExtractor


class CJKDateExtractorConfiguration(ABC):

    @property
    @abstractmethod
    def date_regex_list(self) -> List[Pattern]:
        raise NotImplementedError

    @property
    @abstractmethod
    def implicit_date_list(self) -> List[Pattern]:
        raise NotImplementedError

    @property
    @abstractmethod
    def datetime_period_unit_regex(self) -> Pattern:
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
    def week_day_start_end(self) -> Pattern:
        raise NotImplementedError

    @property
    @abstractmethod
    def range_connector_symbol_regex(self) -> Pattern:
        raise NotImplementedError

    @property
    @abstractmethod
    def duration_extractor(self) -> DateTimeExtractor:
        raise NotImplementedError

    @property
    @abstractmethod
    def ambiguity_date_filters_dict(self) -> Dict[Pattern, Pattern]:
        raise NotImplementedError

    @property
    @abstractmethod
    def number_parser(self) -> CJKNumberParser:
        raise NotImplementedError


class BaseCJKDateExtractor(DateTimeExtractor, AbstractYearExtractor):
    @property
    def extractor_type_name(self) -> str:
        return Date_Constants.SYS_DATETIME_DATE

    def __init__(self, config: CJKDateExtractorConfiguration):
        super().__init__(config)
        self.config = config

    def extract(self, source: str, reference: datetime = None) -> List[ExtractResult]:
        if reference is None:
            reference = datetime.now()

        tokens: List[Token] = list()
        tokens.extend(self.basic_regex_match(source))
        tokens.extend(self.implicit_date(source))
        tokens.extend(self.duration_with_ago_and_later(source, reference))
        result = merge_all_tokens(tokens, source, self.extractor_type_name)

        result = ExtractResultExtension.filter_ambiguity(result, source, self.config.ambiguity_date_filters_dict)

        return result

    # Match basic patterns in DateRegexList
    def basic_regex_match(self, source: str) -> List[Token]:
        ret: List[Token] = list()

        for regexp in self.config.date_regex_list:

            matches = list(regexp.finditer(source))
            if matches:
                for match in matches:
                    ret.append(Token(match.start(), source.index(match.group()) + match.end() - match.start()))

        return ret

    # Match several other implicit cases
    def implicit_date(self, source: str) -> List[Token]:
        ret: List[Token] = list()

        for regexp in self.config.implicit_date_list:

            for match in regexp.finditer(source):
                ret.append(Token(match.start(), source.index(match.group()) + match.end() - match.start()))

        return ret

    # process case like "三天前" "两个月前"
    def duration_with_ago_and_later(self, source: str, reference: datetime) -> List[Token]:
        ret: List[Token] = list()
        duration_extracted_results = self.config.duration_extractor.extract(source, reference)

        for extracted_result in duration_extracted_results:

            # Only handles date durations here
            # Cases with dateTime durations will be handled in DateTime Extractor
            if self.config.datetime_period_unit_regex.match(extracted_result.text):
                continue

            pos = extracted_result.start + extracted_result.length

            if pos < len(source):
                suffix = source[pos:]
                match = RegExpUtility.get_matches(self.config.before_regex, suffix)

                if not match:
                    match = RegExpUtility.get_matches(self.config.after_regex, suffix)

                if match and suffix.startswith(match[0]):
                    meta_data = MetaData()
                    meta_data.is_duration_date_with_weekday = True
                    # "Extend extraction with weekdays like in "Friday two weeks from now", "in 3 weeks on Monday""
                    ret.append(Token(extracted_result.start, pos + len(match[0]), meta_data))

        ret.extend(self.extend_with_week_day(ret, source))

        return ret

    def extend_with_week_day(self, ret: List[Token], source: str):
        new_ret: List[Token] = list()

        for er in ret:
            before_str = source[0: er.start]
            after_str = source[er.end:]

            before_match = self.config.week_day_start_end.match(before_str)
            after_match = self.config.week_day_start_end.match(after_str)

            if before_match or after_match:
                start = before_match.start() if before_match else er.start
                end = er.end if before_match else er.end + after_match.start() + len(after_match.group())

                meta_data = MetaData()
                meta_data.is_duration_date_with_weekday = True
                ret.append(Token(start, end, meta_data))

        return new_ret


class CJKDateParserConfiguration(CJKCommonDateTimeParserConfiguration):

    @property
    @abstractmethod
    def cardinal_extractor(self):
        raise NotImplementedError

    @property
    @abstractmethod
    def integer_extractor(self):
        raise NotImplementedError

    @property
    @abstractmethod
    def ordinal_extractor(self):
        raise NotImplementedError

    @property
    @abstractmethod
    def number_parser(self):
        raise NotImplementedError

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
    def duration_extractor(self) -> DateTimeExtractor:
        raise NotImplementedError

    @property
    @abstractmethod
    def date_period_extractor(self) -> DateTimeExtractor:
        raise NotImplementedError

    @property
    @abstractmethod
    def time_period_extractor(self) -> DateTimeExtractor:
        raise NotImplementedError

    @property
    @abstractmethod
    def date_time_period_extractor(self) -> DateTimeExtractor:
        raise NotImplementedError

    @property
    @abstractmethod
    def set_extractor(self) -> DateTimeExtractor:
        raise NotImplementedError

    @property
    @abstractmethod
    def holiday_extractor(self) -> DateTimeExtractor:
        raise NotImplementedError

    @property
    @abstractmethod
    def date_parser(self) -> BaseDateParser:
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
    def duration_parser(self) -> DateTimeParser:
        raise NotImplementedError

    @property
    @abstractmethod
    def date_period_parser(self) -> DateTimeParser:
        raise NotImplementedError

    @property
    @abstractmethod
    def time_period_parser(self) -> DateTimeParser:
        raise NotImplementedError

    @property
    @abstractmethod
    def date_time_period_parser(self) -> DateTimeParser:
        raise NotImplementedError

    @property
    @abstractmethod
    def set_parser(self) -> DateTimeParser:
        raise NotImplementedError

    @property
    @abstractmethod
    def holiday_parser(self) -> DateTimeParser:
        raise NotImplementedError

    @property
    @abstractmethod
    def date_time_alt_parser(self) -> DateTimeParser:
        raise NotImplementedError

    @property
    @abstractmethod
    def time_zone_parser(self) -> DateTimeParser:
        raise NotImplementedError

    @property
    @abstractmethod
    def month_of_year(self) -> Dict[str, int]:
        raise NotImplementedError

    @property
    @abstractmethod
    def numbers(self) -> Dict[str, int]:
        raise NotImplementedError

    @property
    @abstractmethod
    def double_numbers(self) -> Dict[str, int]:
        raise NotImplementedError

    @property
    @abstractmethod
    def unit_value_map(self) -> Dict[str, int]:
        raise NotImplementedError

    @property
    @abstractmethod
    def season_map(self) -> Dict[str, str]:
        raise NotImplementedError

    @property
    @abstractmethod
    def special_year_prefixes_map(self) -> Dict[str, str]:
        raise NotImplementedError

    @property
    @abstractmethod
    def unit_map(self) -> Dict[str, str]:
        raise NotImplementedError

    @property
    @abstractmethod
    def cardinal_map(self) -> Dict[str, int]:
        raise NotImplementedError

    @property
    @abstractmethod
    def day_of_month(self) -> Dict[str, int]:
        return NotImplementedError

    @property
    @abstractmethod
    def day_of_week(self) -> Dict[str, int]:
        raise NotImplementedError

    @property
    @abstractmethod
    def written_decades(self) -> Dict[str, int]:
        raise NotImplementedError

    @property
    @abstractmethod
    def special_decade_cases(self) -> Dict[str, int]:
        raise NotImplementedError

    @property
    @abstractmethod
    def special_date(self) -> Pattern:
        raise NotImplementedError

    @property
    @abstractmethod
    def utility_configuration(self) -> DateTimeUtilityConfiguration:
        raise NotImplementedError

    @property
    @abstractmethod
    def date_regex_list(self):
        raise NotImplementedError

    @property
    @abstractmethod
    def next_re(self):
        raise NotImplementedError

    @property
    @abstractmethod
    def last_re(self):
        raise NotImplementedError

    @property
    @abstractmethod
    def special_day_regex(self):
        raise NotImplementedError

    @abstractmethod
    def get_swift_day(self, day_str: str):
        raise NotImplementedError

    @property
    @abstractmethod
    def duration_relative_duration_unit_regex(self):
        raise NotImplementedError

    @property
    @abstractmethod
    def special_day_with_num_regex(self):
        raise NotImplementedError

    @property
    @abstractmethod
    def week_day_and_day_regex(self):
        raise NotImplementedError

    @property
    @abstractmethod
    def next_regex(self):
        raise NotImplementedError

    @property
    @abstractmethod
    def this_regex(self):
        raise NotImplementedError

    @property
    @abstractmethod
    def last_regex(self):
        raise NotImplementedError

    @property
    @abstractmethod
    def week_day_regex(self):
        raise NotImplementedError

    @property
    @abstractmethod
    def strict_week_day_regex(self):
        raise NotImplementedError

    @property
    @abstractmethod
    def week_day_of_month_regex(self):
        raise NotImplementedError

    @abstractmethod
    def is_cardinal_last(self, source: str) -> bool:
        raise NotImplementedError

    @property
    @abstractmethod
    def next_month_regex(self) -> Pattern:
        raise NotImplementedError

    @property
    @abstractmethod
    def last_month_regex(self) -> Pattern:
        raise NotImplementedError

    @property
    @abstractmethod
    def lunar_regex(self):
        raise NotImplementedError

    @property
    @abstractmethod
    def last_week_day_regex(self):
        raise NotImplementedError

    @property
    @abstractmethod
    def unit_regex(self):
        raise NotImplementedError

    @property
    @abstractmethod
    def before_regex(self):
        raise NotImplementedError

    @property
    @abstractmethod
    def after_regex(self):
        raise NotImplementedError

    @property
    @abstractmethod
    def dynasty_year_regex(self):
        raise NotImplementedError

    @property
    @abstractmethod
    def dynasty_start_year(self):
        raise NotImplementedError

    @property
    @abstractmethod
    def dynasty_year_map(self):
        raise NotImplementedError


class BaseCJKDateParser(DateTimeParser):

    def __init__(self, config: CJKDateParserConfiguration):
        self.config = config
        self.month_max_days: List[int] = [
            31, 29, 31, 30, 31, 30, 31, 31, 30, 31, 30, 31]

    @property
    def no_date(self):
        return DateUtils.safe_create_from_value(DateUtils.min_value, 0, 0, 0)

    @property
    def parser_type_name(self) -> str:
        return Date_Constants.SYS_DATETIME_DATE

    def parse(self, source: ExtractResult, reference: datetime = None) -> DateTimeParseResult:
        if reference is None:
            reference = datetime.now()

        if source.type is self.parser_type_name:
            value = self.inner_parser(source.text, reference)

        result = DateTimeParseResult()
        result.text = source.text
        result.start = source.start
        result.length = source.length
        result.type = source.type
        result.data = source.data
        result.value = value
        result.timex_str = '' if not value else value.timex
        result.resolution_str = ''

        return result

    def filter_results(self, query: str, candidate_results: List[DateTimeParseResult]):
        return candidate_results

    def inner_parser(self, source_text: str, reference: datetime) -> DateTimeResolutionResult:
        inner_result = self.parse_basic_regex_match(source_text, reference)

        if not inner_result.success:
            inner_result = self.parse_weekday_of_month(
                source_text, reference)

        if not inner_result.success:
            inner_result = self.parse_implicit_date(source_text, reference)

        if not inner_result.success:
            inner_result = self.parser_duration_with_ago_and_later(
                source_text, reference)

        if inner_result.success:
            inner_result.future_resolution: Dict[str, str] = dict()
            inner_result.future_resolution[TimeTypeConstants.DATE] = DateTimeFormatUtil.format_date(
                inner_result.future_value)
            inner_result.past_resolution: Dict[str, str] = dict()
            inner_result.past_resolution[TimeTypeConstants.DATE] = DateTimeFormatUtil.format_date(
                inner_result.past_value)
            inner_result.is_lunar = self.is_lunar_calendar(source_text)

            result_value = inner_result

            return result_value

        return None

    # parse basic patterns in DateRegexList
    def parse_basic_regex_match(self, source_text: str, reference: datetime) -> DateTimeResolutionResult:
        ret = DateTimeResolutionResult()

        for regexp in self.config.date_regex_list:
            match = RegExpUtility.exact_match(regexp, source_text, trim=True)

            if match and match.success:
                # Value string will be set in Match2Date method
                ret = self.match_to_date(match, reference)
                return ret

        return ret

    # match several other cases
    # including '今天', '后天', '十三日'
    def parse_implicit_date(self, source_text: str, reference: datetime) -> DateTimeResolutionResult:
        ret = DateTimeResolutionResult()

        # handle "十二日" "明年这个月三日" "本月十一日"
        match = RegExpUtility.exact_match(self.config.special_date, source_text, trim=True)

        if match and match.success:
            year_str = match.get_group(Date_Constants.THIS_YEAR_GROUP_NAME)
            month_str = match.get_group(Date_Constants.THIS_MONTH_GROUP_NAME)
            day_str = match.get_group(Date_Constants.DAY_GROUP_NAME)

            month = reference.month
            year = reference.year
            day = self.config.day_of_month[day_str]

            has_year = False
            has_month = False

            if month_str:
                has_month = True
                has_year = True

                if self.config.next_re.match(month_str):
                    month += 1
                    if month == (Date_Constants.MAX_MONTH + 1):
                        month = Date_Constants.MIN_MONTH
                        year += 1
                elif self.config.last_re.match(month_str):
                    month -= 1
                    if month == (Date_Constants.MIN_MONTH + 1):
                        month = Date_Constants.MAX_MONTH
                        year -= 1

                if year_str:
                    has_year = True
                    if self.config.next_re.match(year_str):
                        year += 1
                    elif self.config.last_re.match(year_str):
                        year -= 1

            ret.timex = DateTimeFormatUtil.luis_date(year if has_year else -1, month if has_month else -1, day)

            future_date: datetime
            past_date: datetime

            if day > self.get_month_max_day(year, month):
                future_month = month + 1
                past_month = month - 1
                future_year = year
                past_year = year

                if future_month == (Date_Constants.MAX_MONTH + 1):
                    future_month = Date_Constants.MIN_MONTH
                    future_year = year + 1

                if past_month == (Date_Constants.MIN_MONTH - 1):
                    past_month = Date_Constants.MAX_MONTH
                    past_year = year - 1

                is_future_valid = DateUtils.is_valid_date(future_year, future_month, day)
                is_past_valid = DateUtils.is_valid_date(past_year, past_month, day)

                if is_future_valid and is_past_valid:
                    future_date = DateUtils.safe_create_from_value(DateUtils.min_value, future_year, future_month, day)
                    past_date = DateUtils.safe_create_from_value(DateUtils.min_value, past_year, past_month, day)
                elif is_future_valid and not is_past_valid:
                    future_date = past_date = DateUtils.safe_create_from_value(DateUtils.min_value, future_year,
                                                                               future_month, day)
                elif not is_future_valid and not is_past_valid:
                    future_date = past_date = DateUtils.safe_create_from_value(DateUtils.min_value, past_year,
                                                                               past_month, day)
                else:
                    # Fall back to normal cases, might lead to resolution failure
                    future_date = past_date = DateUtils.safe_create_from_value(DateUtils.min_value, year, month, day)
            else:
                future_date = past_date = DateUtils.safe_create_from_value(DateUtils.min_value, year, month, day)

                if not has_month:
                    if future_date < reference:
                        if DateUtils.is_valid_date(year, month + 1, day):
                            future_date += datedelta(months=1)
                    if past_date >= reference:
                        if DateUtils.is_valid_date(year, month - 1, day):
                            past_date -= datedelta(months=1)
                        elif DateUtils.is_Feb_29th(year, month, day):
                            past_date -= datedelta(months=2)
                elif not has_year:
                    if future_date < reference:
                        if DateUtils.is_valid_date(year + 1, month, day):
                            future_date += datedelta(years=1)
                    if past_date >= reference:
                        if DateUtils.is_valid_date(year - 1, month, day):
                            past_date -= datedelta(years=1)

            ret.future_value = future_date
            ret.past_value = past_date
            ret.success = True

            return ret

        # handle cases like "昨日", "明日", "大后天"
        match = RegExpUtility.exact_match(self.config.special_day_regex, source_text, trim=True)

        if match and match.success:
            value = reference + datedelta(days=self.config.get_swift_day(match.value))
            ret.timex = DateTimeFormatUtil.luis_date_from_datetime(value)
            ret.future_value = ret.past_value = DateUtils.safe_create_from_value(DateUtils.min_value, value.year, value.month, value.day)
            ret.success = True

            return ret

        # Handle "今から2日曜日" (2 Sundays from now)
        exact_match = RegExpUtility.exact_match(self.config.special_day_with_num_regex, source_text, trim=True)

        if exact_match and exact_match.success:
            num_ers = self.config.integer_extractor.extract(source_text)
            weekday_str = exact_match.get_group(Date_Constants.WEEKDAY_GROUP_NAME)

            if weekday_str and len(num_ers) > 0:
                num = int(self.config.number_parser.parse(num_ers[0]).value)
                value = reference

                # Check whether the determined day of this week has passed.
                if value.isoweekday() > DayOfWeek(self.config.day_of_week[weekday_str]):
                    num -= 1

                while num > 0:
                    value = DateUtils.next(value, self.get_day_of_week(weekday_str))
                    num -= 1

                ret.timex = DateTimeFormatUtil.luis_date_from_datetime(value)
                ret.future_value = DateUtils.safe_create_from_min_value(value.year, value.month, value.day)
                ret.past_value = ret.future_value
                ret.success = True

            return ret

        # handle "明日から3週間" (3 weeks from tomorrow)
        duration_extracted_results = self.config.duration_extractor.extract(source_text, reference)
        unit_match = self.config.duration_relative_duration_unit_regex.match(source_text)
        is_within = False
        within_regex = RegExpUtility.match_end(
            self.config.duration_relative_duration_unit_regex, source_text, trim=True)
        if within_regex:
            is_within = True if within_regex.get_group(Date_Constants.WITHIN_GROUP_NAME) else False

        if (exact_match or is_within) and unit_match and len(duration_extracted_results) > 0 \
                and not unit_match.get_group(Date_Constants.FEW_GROUP_NAME):
            pr = self.config.duration_parser.parse(duration_extracted_results[0], reference)
            day_str = unit_match.get_group(Date_Constants.LATER_GROUP_NAME)
            future = True
            swift = 0

            if pr:
                if day_str:
                    swift = self.config.get_swift_day(day_str)

                result_date_time = DurationParsingUtil.shift_date_time(pr.timex_str,
                                                                       (reference + datedelta(days=swift)),
                                                                       future)
                ret.timex = f'{DateTimeFormatUtil.luis_date_from_datetime(result_date_time)}'
                ret.future_value = past_value = result_date_time
                ret.success = True
                return ret

        if not ret.success:
            ret = self.match_weekday_and_day(source_text, reference)

        if not ret.success:
            ret = self.match_this_weekday(source_text, reference)

        if not ret.success:
            ret = self.match_next_weekday(source_text, reference)

        if not ret.success:
            ret = self.match_last_weekday(source_text, reference)

        if not ret.success:
            ret = self.match_weekday_alone(source_text, reference)

        return ret

    def match_weekday_and_day(self, source_text: str, reference: datetime) -> DateTimeResolutionResult:
        result_value = DateTimeResolutionResult()

        # Handling cases like 'Monday 21', which both 'Monday' and '21' refer to the same date.
        # The year of expected date can be different to the year of referenceDate.
        match = self.config.week_day_and_day_regex.match(source_text)

        if match:
            month = reference.month
            year = reference.year

            # Create a extract result which content ordinal string of text
            er = ExtractResult()
            er.text = match.get_group(Date_Constants.DAY_GROUP_NAME).text
            er.start = match.get_group(Date_Constants.DAY_GROUP_NAME).start
            er.length = match.get_group(Date_Constants.DAY_GROUP_NAME).length

            # "Parse the day in text into number"
            day = self.convert_cjk_to_num(er.text)

            # Firstly, find a latest date with the "day" as pivotDate. Secondly, if the pivotDate equals the
            # referenced date, in other word, the day of the referenced date is exactly the "day". In this way,
            # check if the pivotDate is the weekday. If so, then the futureDate and the previousDate are the same
            # date (referenced date). Otherwise, increase the pivotDate month by month to find the latest futureDate
            # and decrease the pivotDate month by month to the latest previousDate. Notice: if the "day" is larger
            # than 28, some months should be ignored in the increase or decrease procedure.

            days_in_month = calendar.monthrange(year, month)[1]
            if days_in_month >= day:
                pivot_date = DateUtils.safe_create_from_min_value(year, month, day)
            else:
                # Add 1 month is enough, since 1, 3, 5, 7, 8, 10, 12 months has 31 days
                pivot_date = DateUtils.safe_create_from_min_value(year, month + 1, day)

            num_week_day_int = pivot_date.isoweekday() % 7
            extracted_week_day_str = match.get_group(Date_Constants.WEEKDAY_GROUP_NAME)
            week_day = self.config.day_of_week[extracted_week_day_str]

            if pivot_date != DateUtils.min_value:

                if num_week_day_int == week_day:
                    # The referenceDate is the weekday and with the "day".
                    result_value.future_value = datetime(year, month, day)
                    result_value.past_value = datetime(year, month, day)
                    result_value.timex = DateTimeFormatUtil.luis_date(year, month, day)
                else:
                    future_date = pivot_date
                    past_date = pivot_date

                    while future_date.isoweekday() % 7 != week_day or future_date.day != day or future_date < reference:
                        # Increase the futureDate month by month to find the expected date (the "day" is the weekday)
                        # and make sure the futureDate not less than the referenceDate.
                        future_date += relativedelta(months=1)
                        tmp_days_in_month = calendar.monthrange(future_date.year, future_date.month)[1]
                        if tmp_days_in_month >= day:
                            # For months like January 31, after add 1 month, February 31 won't be returned,
                            # so the day should be revised ASAP.
                            future_date = DateUtils.safe_create_from_value(future_date, future_date.year,
                                                                           future_date.month, day)

                    result_value.future_value = future_date

                    while past_date.isoweekday() % 7 != week_day or past_date.day != day or past_date > reference:
                        # Decrease the pastDate month by month to find the expected date (the "day" is the weekday) and
                        # make sure the pastDate not larger than the referenceDate.
                        past_date -= relativedelta(months=1)
                        tmp_days_in_month = calendar.monthrange(past_date.year, past_date.month)[1]
                        if tmp_days_in_month >= day:
                            # For months like March 31, after minus 1 month, February 31
                            # won't be returned, so the day should be revised ASAP.
                            past_date = DateUtils.safe_create_from_value(DateUtils.min_value, past_date.year,
                                                                         past_date.month, day)

                    result_value.past_value = past_date

                    if week_day == 0:
                        week_day = 7

                    result_value.timex = TimexUtil.generate_weekday_timex(week_day)

            result_value.success = True
            return result_value

        return result_value

    def match_next_weekday(self, source_text: str, reference: datetime) -> DateTimeResolutionResult:
        result = DateTimeResolutionResult()

        match = RegExpUtility.exact_match(self.config.next_regex, source_text, trim=True)

        if match and match.success:
            weekday_key = match.get_group(Date_Constants.WEEKDAY_GROUP_NAME)
            value = DateUtils.next(reference, self.get_day_of_week(weekday_key))

            result.timex = DateTimeFormatUtil.luis_date_from_datetime(value)
            result.future_value = result.past_value = DateUtils.safe_create_from_min_value(value.year, value.month,
                                                                                           value.day)
            result.success = True
        return result

    def match_this_weekday(self, source_text: str, reference: datetime) -> DateTimeResolutionResult:
        result = DateTimeResolutionResult()

        match = RegExpUtility.exact_match(self.config.this_regex, source_text, trim=True)

        if match and match.success:
            weekday_key = match.get_group(Date_Constants.WEEKDAY_GROUP_NAME)
            value = DateUtils.this(reference, self.get_day_of_week(weekday_key))

            result.timex = DateTimeFormatUtil.luis_date_from_datetime(value)
            result.future_value = result.past_value = DateUtils.safe_create_from_min_value(value.year, value.month,
                                                                                           value.day)
            result.success = True

        return result

    def match_last_weekday(self, source_text: str, reference: datetime) -> DateTimeResolutionResult:
        result = DateTimeResolutionResult()

        match = RegExpUtility.exact_match(self.config.last_regex, source_text, trim=True)

        if match and match.success:
            weekday_key = match.get_group(Date_Constants.WEEKDAY_GROUP_NAME)
            value = DateUtils.last(reference, self.get_day_of_week(weekday_key))

            result.timex = DateTimeFormatUtil.luis_date_from_datetime(value)
            result.future_value = result.past_value = DateUtils.safe_create_from_min_value(value.year, value.month,
                                                                                           value.day)
            result.success = True

        return result

    def match_weekday_alone(self, source_text: str, reference: datetime) -> DateTimeResolutionResult:
        result = DateTimeResolutionResult()

        match = RegExpUtility.exact_match(self.config.strict_week_day_regex, source_text, trim=True)

        if match and match.success:
            weekday_str = match.get_group(Date_Constants.WEEKDAY_GROUP_NAME)
            weekday = self.get_day_of_week(weekday_str)
            value = DateUtils.this(reference, weekday)

            if weekday < int(DayOfWeek.MONDAY):
                weekday = int(DayOfWeek.SUNDAY)

            if weekday < reference.isoweekday():
                value = DateUtils.next(reference, weekday)

            result.timex = TimexUtil.generate_weekday_timex(weekday)
            future_date = past_date = value

            if future_date < reference:
                future_date += timedelta(weeks=1)

            if past_date >= reference:
                past_date -= timedelta(weeks=1)

            result.future_value = future_date
            result.past_value = past_date
            result.success = True

        return result

    def parse_weekday_of_month(self, source_text: str, reference: datetime) -> DateTimeResolutionResult:
        ret = DateTimeResolutionResult()
        trimmed_source = source_text.strip()

        match = self.config.week_day_of_month_regex.match(trimmed_source)

        if not match:
            return ret

        cardinal_str = match.group(Date_Constants.CARDINAL)
        weekday_str = match.group(Date_Constants.WEEKDAY_GROUP_NAME)
        month_str = match.group(Date_Constants.MONTH_GROUP_NAME)
        no_year = False

        if RegExpUtility.exact_match(self.config.last_week_day_regex, cardinal_str, trim=True).success:
            cardinal = 5
        else:
            cardinal = self.config.cardinal_map.get(cardinal_str)

        weekday = self.config.day_of_week[weekday_str]

        if not month_str:
            swift = 0

            next_month_match = RegExpUtility.match_begin(self.config.next_month_regex, trimmed_source, trim=True)
            last_month_match = RegExpUtility.match_begin(self.config.last_month_regex, trimmed_source, trim=True)

            if next_month_match and next_month_match.success:
                swift = 1
            elif last_month_match and last_month_match.success:
                swift = -1

            temp = reference + datedelta(months=swift)
            month = temp.month
            year = temp.year
        else:
            month = self.config.month_of_year[month_str]
            year = reference.year
            no_year = True

        value = self.compute_date(cardinal, weekday, month, year)

        if value.month != month:
            cardinal -= 1
            value = value - datedelta(days=7)

        future_date = value
        past_date = value

        if no_year and future_date < reference:
            future_date = self.compute_date(cardinal, weekday, month, year + 1)

            if future_date.month != month:
                future_date = future_date - datedelta(days=7)

        if no_year and past_date >= reference:
            past_date = self.compute_date(cardinal, weekday, month, year - 1)

            if past_date.month != month:
                past_date = past_date - datedelta(days=7)

        # here is a very special case, timeX follows future date
        ret.timex = f'XXXX-{month:02d}-WXX-{weekday}-#{cardinal}'
        ret.future_value = future_date
        ret.past_value = past_date
        ret.success = True

        return ret

    # parse a regex match which includes 'day', 'month' and 'year' (optional) group
    def match_to_date(self, match: Match, reference: datetime) -> DateTimeResolutionResult:
        ret = DateTimeResolutionResult()

        month_str = RegExpUtility.get_group(match, Date_Constants.MONTH_GROUP_NAME)
        day_str = RegExpUtility.get_group(match, Date_Constants.DAY_GROUP_NAME)
        year_str = RegExpUtility.get_group(match, Date_Constants.YEAR_GROUP_NAME)
        year_cjk_str = RegExpUtility.get_group(match, Date_Constants.YEAR_CJK_GROUP_NAME)

        month = day = 0

        tmp = self.convert_cjk_year_to_integer(year_cjk_str)

        year = 0 if tmp == -1 else tmp

        if month_str in self.config.month_of_year and day_str in self.config.day_of_month:
            month = self.config.month_of_year[month_str] % 12 if self.config.month_of_year[month_str] > 12 else \
                self.config.month_of_year[month_str]
            day = self.config.day_of_month[day_str] % 31 if self.config.day_of_month[day_str] > 31 else \
                self.config.day_of_month[day_str]

            if year_str:
                year = int(year_str)

                if 100 > year >= Date_Constants.MIN_TWO_DIGIT_YEAR_PAST_NUM:
                    year += Date_Constants.BASE_YEAR_PAST_CENTURY
                elif 0 <= year < Date_Constants.MAX_TWO_DIGIT_YEAR_FUTURE_NUM:
                    year += Date_Constants.BASE_YEAR_CURRENT_CENTURY

        no_year = False

        if year == 0:
            year = reference.year
            ret.timex = DateTimeFormatUtil.luis_date(-1, month, day)
            no_year = True
        else:
            ret.timex = DateTimeFormatUtil.luis_date(year, month, day)

        future_date, past_date = DateUtils.generate_dates(no_year, reference, year, month, day)

        ret.future_value = future_date
        ret.past_value = past_date
        ret.success = True
        return ret

    def compute_date(self, cardinal: int, weekday: int, month: int, year: int) -> datetime:
        first_day = DateUtils.safe_create_from_value(DateUtils.min_value, year, month, 1)
        first_weekday = DateUtils.this(first_day, self.get_day_of_week(weekday))
        day_of_week_of_first_day = first_day.isoweekday()

        if weekday == 0:
            weekday = int(DayOfWeek.SUNDAY)

        if day_of_week_of_first_day == 0:
            day_of_week_of_first_day = 7

        if weekday < day_of_week_of_first_day:
            first_weekday = DateUtils.next(first_day, DayOfWeek(weekday))

        first_weekday = first_weekday + datedelta(days=7 * (cardinal - 1))

        return first_weekday

    # parse if lunar contains
    def is_lunar_calendar(self, text: str) -> bool:
        trimmed_source = text.strip()
        is_lunar_match = self.config.lunar_regex.match(trimmed_source)
        return is_lunar_match

    # Judge if a date is valid
    @staticmethod
    def is_valid_date(year: int, month: int, day: int) -> bool:

        if month < Date_Constants.MIN_MONTH:
            year -= 1
            month = Date_Constants.MAX_MONTH

        if month > Date_Constants.MAX_MONTH:
            year += 1
            month = Date_Constants.MIN_MONTH

        return DateUtils.is_valid_date(year, month, day)

    # Handle cases like "三天前" "Three days ago"
    def parser_duration_with_ago_and_later(self, source_text: str, reference: datetime) -> DateTimeResolutionResult:
        ret = DateTimeResolutionResult()
        num_str = ''
        unit_str = ''

        duration_extracted_results = self.config.duration_extractor.extract(source_text, reference)

        if duration_extracted_results:
            match = self.config.unit_regex.match(source_text)

            if match:
                suffix = source_text[
                         duration_extracted_results[0].start + duration_extracted_results[0].length:].strip()
                src_unit = match.get_group(Date_Constants.UNIT)

                number_str = source_text[duration_extracted_results[0].start:
                                         match.lastindex - duration_extracted_results.start + 1]
                unit_match = self.config.duration_relative_duration_unit_regex.match(source_text)

                few_in_unit_match = unit_match.get_group(Date_Constants.FEW_GROUP_NAME)

                # set the inexact number "数" (few) to 3 for now
                number = 3 if number_str == few_in_unit_match else self.convert_cjk_to_num(num_str)

                if not number_str == few_in_unit_match:
                    if suffix == unit_match.value:
                        pr = self.config.duration_parser.parse(duration_extracted_results[0], reference)
                        is_future = suffix == unit_match.get_group(Date_Constants.LATER_GROUP_NAME)
                        swift = 0

                        if pr:
                            result_date_time = DurationParsingUtil.shift_date_time(pr.timex_str,
                                                                                   (reference + datedelta(days=swift)),
                                                                                   is_future)
                            ret.timex = DateTimeFormatUtil.luis_date_from_datetime(result_date_time)
                            ret.future_value = past_value = result_date_time
                            ret.success = True
                            return ret

                if src_unit in self.config.unit_map:
                    unit_str = self.config.unit_map[src_unit]
                    unit_type = 'T' if self.config.duration_parser.is_less_than_day(unit_str) else ''
                    ret.timex = f'P{unit_type}{number_str}{unit_str[0]}'

                    date = Date_Constants.INVALID_DATE_STRING

                    before_match = self.config.before_regex.match(suffix)

                    if before_match and suffix.startswith(before_match):
                        date = DurationParsingUtil.shift_date_time(ret.timex, reference, False)

                    after_match = self.config.after_regex.match(suffix)
                    if after_match and suffix.startswith(after_match[0]):
                        date = DurationParsingUtil.shift_date_time(ret.timex, reference, True)

                    if date != Date_Constants.INVALID_DATE_STRING:
                        ret.timex = DateTimeFormatUtil.luis_date_from_datetime(date)
                        ret.future_value = ret.past_value = date
                        ret.success = True
                        return ret

        return ret

    # Convert CJK Number to Integer
    def convert_cjk_to_num(self, num_string: str) -> int:
        num = -1
        er = self.config.integer_extractor.extract(num_string)

        if er:
            if er[0].type == Num_Constants.SYS_NUM_INTEGER:
                num = int(self.config.number_parser.parse(er[0]).value)

        return num

    # convert CJK Year to Integer
    def convert_cjk_year_to_integer(self, year_cjk_string: str) -> int:
        year = num = 0

        dynasty_year = DateTimeFormatUtil.parse_dynasty_year(year_cjk_string,
                                                             self.config.dynasty_year_regex,
                                                             self.config.dynasty_start_year,
                                                             self.config.dynasty_year_map,
                                                             self.config.integer_extractor,
                                                             self.config.number_parser)
        if dynasty_year and dynasty_year > 0:
            return dynasty_year

        er = self.config.integer_extractor.extract(year_cjk_string)

        if er:
            if er[0].type == Num_Constants.SYS_NUM_INTEGER:
                num = self.config.number_parser.parse(er[0]).value

        if num < 10:
            num = 0
            for year in year_cjk_string:
                num *= 10

                er = self.config.integer_extractor.extract(str(year))

                if er:
                    if er[0].type == Num_Constants.SYS_NUM_INTEGER:
                        num += self.config.number_parser.parse(er[0])

        year = -1 if num < 10 else num

        return year

    def get_month_max_day(self, year, month) -> int:
        max_day = self.month_max_days[month - 1]

        if not DateUtils.is_leap_year(year) and month == 2:
            max_day -= 1
        return max_day

    def get_day_of_week(self, weekday_key: any) -> DayOfWeek:
        if type(weekday_key) == str:
            return DayOfWeek.SUNDAY if self.config.day_of_week[weekday_key] == 0 \
                else DayOfWeek(self.config.day_of_week[weekday_key])
        elif type(weekday_key) == int:
            return DayOfWeek.SUNDAY if weekday_key == 0 \
                else DayOfWeek(weekday_key)
