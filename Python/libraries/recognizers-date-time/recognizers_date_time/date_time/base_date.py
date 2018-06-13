from abc import ABC, abstractmethod
from typing import List, Optional, Pattern, Dict
from datetime import datetime, timedelta
import calendar
from datedelta import datedelta
import regex

from recognizers_text.extractor import ExtractResult
from recognizers_text.utilities import RegExpUtility
from recognizers_number import BaseNumberExtractor, BaseNumberParser
from recognizers_number.number import Constants as NumberConstants
from .constants import Constants, TimeTypeConstants
from .extractors import DateTimeExtractor
from .parsers import DateTimeParser, DateTimeParseResult
from .utilities import DateTimeUtilityConfiguration, Token, merge_all_tokens, get_tokens_from_regex, DateUtils, AgoLaterUtil, FormatUtil, DateTimeResolutionResult, DayOfWeek, AgoLaterMode

class DateExtractorConfiguration(ABC):
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
    def month_end(self) -> Pattern:
        raise NotImplementedError

    @property
    @abstractmethod
    def of_month(self) -> Pattern:
        raise NotImplementedError

    @property
    @abstractmethod
    def date_unit_regex(self) -> Pattern:
        raise NotImplementedError

    @property
    @abstractmethod
    def for_the_regex(self) -> Pattern:
        raise NotImplementedError

    @property
    @abstractmethod
    def week_day_and_day_of_month_regex(self) -> Pattern:
        raise NotImplementedError

    @property
    @abstractmethod
    def relative_month_regex(self) -> Pattern:
        raise NotImplementedError

    @property
    @abstractmethod
    def week_day_regex(self) -> Pattern:
        raise NotImplementedError

    @property
    @abstractmethod
    def day_of_week(self) -> Dict[str, int]:
        raise NotImplementedError

    @property
    @abstractmethod
    def ordinal_extractor(self) -> BaseNumberExtractor:
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
    def utility_configuration(self) -> DateTimeUtilityConfiguration:
        raise NotImplementedError

class BaseDateExtractor(DateTimeExtractor):
    @property
    def extractor_type_name(self) -> str:
        return Constants.SYS_DATETIME_DATE

    def __init__(self, config: DateExtractorConfiguration):
        self.config = config

    def extract(self, source: str, reference: datetime = None) -> List[ExtractResult]:
        if reference is None:
            reference = datetime.now()

        tokens = self.basic_regex_match(source)
        tokens.extend(self.implicit_date(source))
        tokens.extend(self.number_with_month(source, reference))
        tokens.extend(self.duration_with_before_and_after(source, reference))

        result = merge_all_tokens(tokens, source, self.extractor_type_name)
        return result

    def basic_regex_match(self, source: str) -> List[Token]:
        ret: List[Token] = list()

        for regexp in self.config.date_regex_list:
            ret.extend(get_tokens_from_regex(regexp, source))

        return ret

    def implicit_date(self, source: str) -> List[Token]:
        ret: List[Token] = list()

        for regexp in self.config.implicit_date_list:
            ret.extend(get_tokens_from_regex(regexp, source))

        return ret

    def number_with_month(self, source: str, reference: datetime) -> List[Token]:
        ret: List[Token] = list()
        extract_results = self.config.ordinal_extractor.extract(source)
        extract_results.extend(self.config.integer_extractor.extract(source))

        for result in extract_results:
            num = int(self.config.number_parser.parse(result).value)

            if num < 1 or num > 31:
                continue

            if result.start >= 0:
                front_string = source[0:result.start or 0]
                match = regex.search(self.config.month_end, front_string)

                if match is not None:
                    ret.append(Token(match.start(), match.end() + result.length))
                    continue

                # handling cases like 'for the 25th'
                matches = regex.finditer(self.config.for_the_regex, source)
                is_found = False

                for match_case in matches:
                    if match_case is not None:
                        ordinal_num = RegExpUtility.get_group(match_case, 'DayOfMonth')

                        if ordinal_num == result.text:
                            length = len(RegExpUtility.get_group(match_case, 'end'))
                            ret.append(Token(match_case.start(), match_case.end() - length))
                            is_found = True

                if is_found:
                    continue

                # handling cases like 'Thursday the 21st', which both 'Thursday' and '21st' refer to a same date
                matches = regex.finditer(self.config.week_day_and_day_of_month_regex, source)

                for match_case in matches:
                    if match_case is not None:
                        ordinal_num = RegExpUtility.get_group(match_case, 'DayOfMonth')

                        if ordinal_num == result.text:
                            month = reference.month
                            year = reference.year

                            # get week of day for the ordinal number which is regarded as a date of reference month
                            date = DateUtils.safe_create_from_min_value(year, month, num)
                            num_week_day_str: str = calendar.day_name[date.weekday()].lower()

                            # get week day from text directly, compare it with the weekday generated above
                            # to see whether they refer to a same week day
                            extracted_week_day_str = RegExpUtility.get_group(match_case, 'weekday').lower()
                            if (date != DateUtils.min_value and
                                    self.config.day_of_week[num_week_day_str] ==
                                    self.config.day_of_week[extracted_week_day_str]):
                                ret.append(
                                    Token(match_case.start(), match_case.end()))
                                is_found = True

                if is_found:
                    continue

                # handling cases like '20th of next month'
                suffix_str: str = source[result.start + result.length:].lower()
                match = regex.match(self.config.relative_month_regex, suffix_str.strip())
                space_len = len(suffix_str) - len(suffix_str.strip())

                if match is not None and match.start() == 0:
                    ret.append(
                        Token(result.start, result.start + result.length + space_len + len(match.group())))

                # handling cases like 'second Sunday'
                match = regex.match(
                    self.config.week_day_regex, suffix_str.strip())
                if (match is not None and match.start() == 0 and
                        num >= 1 and num <= 5 and
                        result.type == NumberConstants.SYS_NUM_ORDINAL):
                    week_day_str = RegExpUtility.get_group(match, 'weekday')

                    if week_day_str in self.config.day_of_week:
                        ret.append(
                            Token(result.start, result.start + result.length + space_len + len(match.group())))

            if result.start + result.length < len(source):
                after_string = source[result.start + result.length:]
                match = regex.match(self.config.of_month, after_string)

                if match is not None:
                    ret.append(Token(result.start, result.start + result.length + len(match.group())))

        return ret

    def duration_with_before_and_after(self, source: str, reference: datetime) -> List[Token]:
        ret: List[Token] = list()
        duration_results = self.config.duration_extractor.extract(source, reference)

        for result in duration_results:
            match = regex.search(self.config.date_unit_regex, result.text)

            if match is None:
                continue

            ret = AgoLaterUtil.extractor_duration_with_before_and_after(source, result, ret, self.config.utility_configuration)

        return ret

class DateParserConfiguration(ABC):
    @property
    @abstractmethod
    def ordinal_extractor(self) -> BaseNumberExtractor:
        raise NotImplementedError

    @property
    @abstractmethod
    def integer_extractor(self) -> BaseNumberExtractor:
        raise NotImplementedError

    @property
    @abstractmethod
    def cardinal_extractor(self) -> BaseNumberExtractor:
        raise NotImplementedError

    @property
    @abstractmethod
    def duration_extractor(self) -> DateTimeExtractor:
        raise NotImplementedError

    @property
    @abstractmethod
    def duration_parser(self) -> DateTimeParser:
        raise NotImplementedError

    @property
    @abstractmethod
    def number_parser(self) -> BaseNumberParser:
        raise NotImplementedError

    @property
    @abstractmethod
    def month_of_year(self) -> Dict[str, int]:
        raise NotImplementedError

    @property
    @abstractmethod
    def day_of_month(self) -> Dict[str, int]:
        raise NotImplementedError

    @property
    @abstractmethod
    def day_of_week(self) -> Dict[str, int]:
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
    def date_regex(self) -> List[Pattern]:
        raise NotImplementedError

    @property
    @abstractmethod
    def on_regex(self) -> Pattern:
        raise NotImplementedError

    @property
    @abstractmethod
    def special_day_regex(self) -> Pattern:
        raise NotImplementedError

    @property
    @abstractmethod
    def next_regex(self) -> Pattern:
        raise NotImplementedError

    @property
    @abstractmethod
    def unit_regex(self) -> Pattern:
        raise NotImplementedError

    @property
    @abstractmethod
    def month_regex(self) -> Pattern:
        raise NotImplementedError

    @property
    @abstractmethod
    def week_day_regex(self) -> Pattern:
        raise NotImplementedError

    @property
    @abstractmethod
    def last_regex(self) -> Pattern:
        raise NotImplementedError

    @property
    @abstractmethod
    def this_regex(self) -> Pattern:
        raise NotImplementedError

    @property
    @abstractmethod
    def week_day_of_month_regex(self) -> Pattern:
        raise NotImplementedError

    @property
    @abstractmethod
    def for_the_regex(self) -> Pattern:
        raise NotImplementedError

    @property
    @abstractmethod
    def week_day_and_day_of_month_regex(self) -> Pattern:
        raise NotImplementedError

    @property
    @abstractmethod
    def relative_month_regex(self) -> Pattern:
        raise NotImplementedError

    @property
    @abstractmethod
    def utility_configuration(self) -> DateTimeUtilityConfiguration:
        raise NotImplementedError

    @property
    @abstractmethod
    def date_token_prefix(self) -> str:
        raise NotImplementedError

    @abstractmethod
    def get_swift_day(self, source: str) -> int:
        raise NotImplementedError

    @abstractmethod
    def get_swift_month(self, source: str) -> int:
        raise NotImplementedError

    @abstractmethod
    def is_cardinal_last(self, source: str) -> bool:
        raise NotImplementedError

class BaseDateParser(DateTimeParser):
    @property
    def parser_type_name(self) -> str:
        return Constants.SYS_DATETIME_DATE

    def __init__(self, config: DateParserConfiguration):
        self.config = config

    def parse(self, source: ExtractResult, reference: datetime = None) -> Optional[DateTimeParseResult]:
        if reference is None:
            reference = datetime.now()

        result_value: DateTimeParseResult = None

        if source.type is self.parser_type_name:
            source_text = source.text.lower()
            inner_result = self.parse_basic_regex_match(source_text, reference)

            if not inner_result.success:
                inner_result = self.parse_implicit_date(source_text, reference)

            if not inner_result.success:
                inner_result = self.parse_weekday_of_month(source_text, reference)

            if not inner_result.success:
                inner_result = self.parser_duration_with_ago_and_later(source_text, reference)

            if not inner_result.success:
                inner_result = self.parse_number_with_month(source_text, reference)

            if not inner_result.success:
                inner_result = self.parse_single_number(source_text, reference)

            if inner_result.success:
                inner_result.future_resolution: Dict[str, str] = dict()
                inner_result.future_resolution[TimeTypeConstants.DATE] = FormatUtil.format_date(inner_result.future_value)
                inner_result.past_resolution: Dict[str, str] = dict()
                inner_result.past_resolution[TimeTypeConstants.DATE] = FormatUtil.format_date(inner_result.past_value)
                result_value = inner_result

        result = DateTimeParseResult(source)
        result.value = result_value
        result.timex_str = result_value.timex if result_value is not None else ''
        result.resolution_str = ''

        return result

    def parse_basic_regex_match(self, source: str, reference: datetime) -> DateTimeParseResult:
        trimmed_source = source.strip()
        result = DateTimeResolutionResult()

        for regexp in self.config.date_regex:
            offset = 0
            match = regex.search(regexp, trimmed_source)

            if match is None:
                match = regex.search(regexp, self.config.date_token_prefix + trimmed_source)
                offset = len(self.config.date_token_prefix)

            if match and match.start() == offset and len(match.group()) == len(trimmed_source):
                result = self.match_to_date(match, reference)
                break

        return result

    def match_to_date(self, match, reference: datetime)-> DateTimeResolutionResult:
        result = DateTimeResolutionResult()
        year_str = RegExpUtility.get_group(match, 'year')
        month_str = RegExpUtility.get_group(match, 'month')
        day_str = RegExpUtility.get_group(match, 'day')
        month = 0
        day = 0
        year = 0

        if month_str in self.config.month_of_year and day_str in self.config.day_of_month:
            month = self.config.month_of_year.get(month_str)
            day = self.config.day_of_month.get(day_str)

            if year_str:
                year = int(year_str) if year_str.isnumeric() else 0

                if year < 100 and year >= 90:
                    year += 1900
                elif year < 100 and year < 20:
                    year += 2000

        no_year = False

        if year == 0:
            year = reference.year
            result.timex = FormatUtil.luis_date(-1, month, day)
            no_year = True
        else:
            result.timex = FormatUtil.luis_date(year, month, day)

        future_date = DateUtils.safe_create_from_min_value(year, month, day)
        past_date = DateUtils.safe_create_from_min_value(year, month, day)

        if no_year and future_date < reference:
            future_date = DateUtils.safe_create_from_min_value(year + 1, month, day)

        if no_year and past_date >= reference:
            past_date = DateUtils.safe_create_from_min_value(year - 1, month, day)

        result.future_value = future_date
        result.past_value = past_date
        result.success = True
        return result

    def parse_implicit_date(self, source: str, reference: datetime) -> DateTimeParseResult:
        trimmed_source = source.strip()
        result = DateTimeResolutionResult()

        # handle "on 12"
        match = regex.search(self.config.on_regex, self.config.date_token_prefix + trimmed_source)
        if match and match.start() == len(self.config.date_token_prefix) and len(match.group()) == len(trimmed_source):
            day = 0
            month = reference.month
            year = reference.year
            day_str = match.group('day')
            day = self.config.day_of_month.get(day_str)

            result.timex = FormatUtil.luis_date(-1, -1, day)

            try_str = FormatUtil.luis_date(year, month, day)
            try_date = datetime.strptime(try_str, '%Y-%m-%d')
            future_date: datetime
            past_date: datetime

            if try_date:
                future_date = DateUtils.safe_create_from_min_value(year, month, day)
                past_date = DateUtils.safe_create_from_min_value(year, month, day)

                if future_date < reference:
                    future_date += datedelta(months=1)

                if past_date >= reference:
                    past_date += datedelta(months=-1)
            else:
                future_date = DateUtils.safe_create_from_min_value(year, month + 1, day)
                past_date = DateUtils.safe_create_from_min_value(year, month - 1, day)

            result.future_value = future_date
            result.past_value = past_date
            result.success = True
            return result

        # handle "today", "the day before yesterday"
        match = regex.match(self.config.special_day_regex, trimmed_source)
        if match and match.start() == 0 and len(match.group()) == len(trimmed_source):
            swift = self.config.get_swift_day(match.group())
            value = reference + timedelta(days=swift)
            result.timex = FormatUtil.luis_date_from_datetime(value)
            result.future_value = value
            result.past_value = value
            result.success = True
            return result

        # handle "next Sunday"
        match = regex.match(self.config.next_regex, trimmed_source)
        if match and match.start() == 0 and len(match.group()) == len(trimmed_source):
            weekday_str = match.group('weekday')
            value = DateUtils.next(reference, self.config.day_of_week.get(weekday_str))

            result.timex = FormatUtil.luis_date_from_datetime(value)
            result.future_value = value
            result.past_value = value
            result.success = True
            return result

        # handle "this Friday"
        match = regex.match(self.config.this_regex, trimmed_source)
        if match and match.start() == 0 and len(match.group()) == len(trimmed_source):
            weekday_str = match.group('weekday')
            value = DateUtils.this(reference, self.config.day_of_week.get(weekday_str))

            result.timex = FormatUtil.luis_date_from_datetime(value)
            result.future_value = value
            result.past_value = value
            result.success = True
            return result

        # handle "last Friday", "last mon"
        match = regex.match(self.config.last_regex, trimmed_source)
        if match and match.start() == 0 and len(match.group()) == len(trimmed_source):
            weekday_str = match.group('weekday')
            value = DateUtils.last(reference, self.config.day_of_week.get(weekday_str))

            result.timex = FormatUtil.luis_date_from_datetime(value)
            result.future_value = value
            result.past_value = value
            result.success = True
            return result

        # handle "Friday"
        match = regex.match(self.config.week_day_regex, trimmed_source)
        if match and match.start() == 0 and len(match.group()) == len(trimmed_source):
            weekday_str = match.group('weekday')
            weekday = self.config.day_of_week.get(weekday_str)
            value = DateUtils.this(reference, weekday)

            if weekday < int(DayOfWeek.Monday):
                weekday = int(DayOfWeek.Sunday)

            if weekday < reference.isoweekday():
                value = DateUtils.next(reference, weekday)

            result.timex = 'XXXX-WXX-' + str(weekday)
            future_date = value
            past_date = value

            if future_date < reference:
                future_date += timedelta(weeks=1)

            if past_date >= reference:
                past_date -= timedelta(weeks=1)

            result.future_value = future_date
            result.past_value = past_date
            result.success = True
            return result

        # handle "for the 27th."
        match = regex.match(self.config.for_the_regex, trimmed_source)
        if match:
            day_str = match.group('DayOfMonth')
            er = ExtractResult.get_from_text(day_str)
            day = int(self.config.number_parser.parse(er).value)

            month = reference.month
            year = reference.year

            result.timex = FormatUtil.luis_date(-1, -1, day)
            date = datetime(year, month, day)
            result.future_value = date
            result.past_value = date
            result.success = True

            return result

        # handling cases like 'Thursday the 21st', which both 'Thursday' and '21st' refer to a same date
        match = regex.match(self.config.week_day_and_day_of_month_regex, trimmed_source)
        if match:
            day_str = match.group('DayOfMonth')
            er = ExtractResult.get_from_text(day_str)
            day = int(self.config.number_parser.parse(er).value)
            month = reference.month
            year = reference.year

            # the validity of the phrase is guaranteed in the Date Extractor
            result.timex = FormatUtil.luis_date(year, month, day)
            date = datetime(year, month, day)
            result.future_value = date
            result.past_value = date
            result.success = True

            return result

        return result

    def parse_weekday_of_month(self, source: str, reference: datetime) -> DateTimeParseResult:
        trimmed_source = source.strip()
        result = DateTimeResolutionResult()
        match = regex.match(self.config.week_day_of_month_regex, trimmed_source)

        if not match:
            return result

        cardinal_str = RegExpUtility.get_group(match, 'cardinal')
        weekday_str = RegExpUtility.get_group(match, 'weekday')
        month_str = RegExpUtility.get_group(match, 'month')
        no_year = False
        cardinal = 5 if self.config.is_cardinal_last(cardinal_str) else self.config.cardinal_map.get(cardinal_str)
        weekday = self.config.day_of_week.get(weekday_str)
        month = reference.month
        year = reference.year

        if not month_str:
            swift = self.config.get_swift_month(trimmed_source)
            temp = reference.replace(month=reference.month + swift)
            month = temp.month
            year = temp.year
        else:
            month = self.config.month_of_year.get(month_str)
            no_year = True

        value = self._compute_date(cardinal, weekday, month, year)

        if value.month != month:
            cardinal -= 1
            value = value.replace(day=value.day - 7)

        future_date = value
        past_date = value

        if no_year and future_date < reference:
            future_date = self._compute_date(cardinal, weekday, month, year + 1)
            if future_date.month != month:
                future_date = future_date.replace(day=future_date.day - 7)

        if no_year and past_date >= reference:
            past_date = self._compute_date(cardinal, weekday, month, year - 1)
            if past_date.month != month:
                past_date = past_date.replace(day=past_date.date - 7)

        result.timex = '-'.join(['XXXX', FormatUtil.to_str(month, 2), 'WXX', str(weekday), '#' + str(cardinal)])
        result.future_value = future_date
        result.past_value = past_date
        result.success = True
        return result

    def _compute_date(self, cardinal: int, weekday: DayOfWeek, month: int, year: int):
        first_day = datetime(year, month, 1)
        first_weekday = DateUtils.this(first_day, weekday)

        if weekday == 0:
            weekday = int(DayOfWeek.Sunday)

        if weekday < first_day.isoweekday():
            first_weekday = DateUtils.next(first_day, weekday)

        first_weekday = first_weekday.replace(day=first_weekday.day + (7 * (cardinal - 1)))
        return first_weekday

    def parser_duration_with_ago_and_later(self, source: str, reference: datetime) -> DateTimeParseResult:
        return AgoLaterUtil.parse_duration_with_ago_and_later(
            source,
            reference,
            self.config.duration_extractor,
            self.config.duration_parser,
            self.config.unit_map,
            self.config.unit_regex,
            self.config.utility_configuration,
            AgoLaterMode.DATE)

    def parse_number_with_month(self, source: str, reference: datetime) -> DateTimeParseResult:
        trimmed_source = source.strip()
        ambiguous = True
        result = DateTimeResolutionResult()

        ers = self.config.ordinal_extractor.extract(trimmed_source)

        if not ers:
            ers = self.config.integer_extractor.extract(trimmed_source)

        if not ers:
            return result

        num = int(self.config.number_parser.parse(ers[0]).value)
        day = 1
        month = 0

        match = regex.search(self.config.month_regex, trimmed_source)

        if match:
            month = self.config.month_of_year.get(match.group())
            day = num
        else:
            # handling relative month
            match = regex.search(self.config.relative_month_regex, trimmed_source)
            if match:
                month_str = match.group('order')
                swift = self.config.get_swift_month(month_str)
                date = reference.replace(month=reference.month+swift)
                month = date.month
                day = num
                ambiguous = False

        # handling casesd like 'second Sunday'
        if not match:
            match = regex.search(self.config.week_day_regex, trimmed_source)
            if match:
                month = reference.month
                # resolve the date of wanted week day
                wanted_week_day = self.config.day_of_week.get(match.group('weekday'))
                first_date = DateUtils.safe_create_from_min_value(reference.year, reference.month, 1)
                first_weekday = first_date.isoweekday()
                delta_days = wanted_week_day - first_weekday if wanted_week_day > first_weekday else wanted_week_day - first_weekday + 7
                first_wanted_week_day = first_date + timedelta(days=delta_days)
                day = first_wanted_week_day.day + ((num - 1) * 7)
                ambiguous = False

        if not match:
            return result

        year = reference.year

        # for LUIS format value string
        date = DateUtils.safe_create_from_min_value(year, month, day)
        future_date = date
        past_date = date

        if ambiguous:
            result.timex = FormatUtil.luis_date(-1, month, day)

            if future_date < reference:
                future_date = future_date.replace(year=future_date.year+1)

            if past_date >= reference:
                past_date = past_date.replace(year=past_date.year+1)
        else:
            result.timex = FormatUtil.luis_date(year, month, day)

        result.future_value = future_date
        result.past_value = past_date
        result.success = True
        return result

    def parse_single_number(self, source: str, reference: datetime) -> DateTimeParseResult:
        trimmed_source = source.strip()
        result = DateTimeResolutionResult()

        ers = self.config.ordinal_extractor.extract(trimmed_source)

        if not ers or not ers[0].text:
            ers = self.config.integer_extractor.extract(trimmed_source)

        if not ers or ers[0].text:
            return result

        day = int(self.config.number_parser.parse(ers[0]).value)
        month = reference.month
        year = reference.year

        result.timex = FormatUtil.luis_date(-1, -1, day)
        past_date = DateUtils.safe_create_from_min_value(year, month, day)
        future_date = DateUtils.safe_create_from_min_value(year, month, day)

        if future_date != DateUtils.min_value and future_date < reference:
            future_date = future_date.replace(month=future_date.month + 1)

        if past_date != DateUtils.min_value and past_date >= reference:
            past_date = past_date.replace(month=past_date.month - 1)

        result.future_value = future_date
        result.past_value = past_date
        result.success = True
        return result
