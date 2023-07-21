from abc import abstractmethod
from datetime import datetime, timedelta
from re import Pattern
from typing import Dict, List, Match

from datedelta import datedelta
from regex import regex

from recognizers_date_time.date_time import Constants
from recognizers_date_time.date_time.base_date import BaseDateParser
from recognizers_date_time.date_time.base_dateperiod import BaseDatePeriodParser
from recognizers_date_time.date_time.parsers import DateTimeParser, DateTimeParseResult
from recognizers_date_time.date_time.utilities import DateTimeOptionsConfiguration, DateTimeExtractor, \
    merge_all_tokens, ExtractResultExtension, Token, DateContext, DateTimeResolutionResult, TimeTypeConstants, \
    DateTimeFormatUtil, DateUtils, TimexUtil, DayOfWeek, DurationParsingUtil, DateTimeOptions
from recognizers_date_time.date_time.utilities.mod_and_date_result import ModAndDateResult
from recognizers_number import BaseNumberParser, BaseNumberExtractor
from recognizers_number import Constants as Num_Constants
from recognizers_text import Metadata, ExtractResult, Extractor, ConditionalMatch, RegExpUtility


class CJKDatePeriodExtractorConfiguration(DateTimeOptionsConfiguration):

    @property
    @abstractmethod
    def simple_cases_regexes(self) -> List[Pattern]:
        raise NotImplementedError

    @property
    @abstractmethod
    def till_regex(self) -> Pattern:
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
    def range_suffix_regex(self) -> Pattern:
        raise NotImplementedError

    @property
    @abstractmethod
    def future_regex(self) -> Pattern:
        raise NotImplementedError

    @property
    @abstractmethod
    def past_regex(self) -> Pattern:
        raise NotImplementedError

    @property
    @abstractmethod
    def first_last_of_year_regex(self) -> Pattern:
        raise NotImplementedError

    @property
    @abstractmethod
    def unit_regex(self) -> Pattern:
        raise NotImplementedError

    @property
    @abstractmethod
    def number_combined_with_unit(self) -> Pattern:
        raise NotImplementedError

    @property
    @abstractmethod
    def followed_unit(self) -> Pattern:
        raise NotImplementedError

    @property
    @abstractmethod
    def date_point_extractor(self) -> DateTimeExtractor:
        raise NotImplementedError

    @property
    @abstractmethod
    def duration_extractor(self) -> DateTimeExtractor:
        raise NotImplementedError

    @property
    @abstractmethod
    def integer_extractor(self) -> BaseNumberExtractor:
        raise NotImplementedError

    @property
    @abstractmethod
    def ambiguity_filters_dict(self) -> Dict[Pattern, Pattern]:
        raise NotImplementedError


class BaseCJKDatePeriodExtractor(DateTimeExtractor):

    @property
    def extractor_type_name(self) -> str:
        return Constants.SYS_DATETIME_DATEPERIOD

    def __init__(self, config: CJKDatePeriodExtractorConfiguration):
        self.config = config

    def extract(self, source: str, reference: datetime = None) -> List[ExtractResult]:
        if not reference:
            reference = datetime.now()
        tokens = []

        tokens += self.match_simple_cases(source)
        simple_cases_results = merge_all_tokens(tokens, source, self.extractor_type_name)
        tokens += self.match_complex_cases(source, simple_cases_results, reference)
        tokens += self.merge_two_time_points(source, reference)
        tokens += self.match_number_with_unit(source)
        tokens += self.match_duration(source, reference)

        rets = merge_all_tokens(tokens, source, self.extractor_type_name)

        # Remove common ambiguous cases
        rets = ExtractResultExtension.filter_ambiguity(rets, source, self.config.ambiguity_filters_dict)

        return rets

    def match_simple_cases(self, source: str) -> List[Token]:
        ret = []

        for regexp in self.config.simple_cases_regexes:
            matches = list(regex.finditer(regexp, source))
            for match in matches:
                ret.append(Token(match.start(), match.end()))

        return ret

    def match_duration(self, source: str, reference: datetime) -> List[Token]:
        ret: List[Token] = list()
        durations = []
        duration_extractions = self.config.duration_extractor.extract(source, reference)

        for duration_extraction in duration_extractions:
            date_unit_match = self.config.date_unit_regex.match(duration_extraction.text)

            if not date_unit_match:
                continue

            durations.append(
                Token(duration_extraction.start, duration_extraction.start + duration_extraction.length))

        for duration in durations:
            before_str = source[0:duration.start].lower()
            after_str = source[duration.start:duration.start + duration.length]

            if not before_str and not after_str:
                continue

            # handle cases with 'within' and 'next'
            match_within = RegExpUtility.match_begin(self.config.future_regex, after_str, trim=True)
            match_next = RegExpUtility.match_end(self.config.future_regex, after_str, trim=True)

            if match_within and match_next and not match_next.group() == Constants.WITHIN_GROUP_NAME:
                if match_next.value == match_within.value:
                    ret.append(Token(duration.start - match_next.length, duration.end))
                else:
                    ret.append(Token(duration.start - match_next.length, duration.end + match_within.length))
            elif match_within:
                ret.append(Token(duration.start, duration.end + match_within.length))
            elif match_next:
                ret.append(Token(duration.start - match_next.length, duration.end))

        return ret

    # merge two time points
    def merge_two_time_points(self, source: str, reference: datetime) -> List[Token]:
        ret: List[Token] = list()
        er = self.config.date_point_extractor.extract(source, reference)

        if not er:
            return ret

        # merge '{TimePoint} 到 {TimePoint}'
        idx = 0

        while idx < len(er) - 1:
            middle_begin = er[idx].start + er[idx].length
            middle_end = er[idx + 1].start

            if middle_begin >= middle_end:
                idx += 1
                continue

            middle_str = source[middle_begin:middle_end - middle_begin].strip()

            if RegExpUtility.exact_match(self.config.till_regex, middle_str, trim=True).success:
                period_begin = er[idx].start
                period_end = er[idx + 1].start + er[idx].length

                # handle suffix
                after_str = source[period_end:]
                match = RegExpUtility.match_begin(self.config.range_suffix_regex, after_str, trim=True)

                if match and match.success:
                    period_end = period_end + match.index + match.length

                # handle prefix
                before_str = source[:period_begin]
                match = RegExpUtility.match_end(self.config.range_prefix_regex, before_str, trim=True)

                if match and match.success:
                    period_begin = match.index

                ret.append(Token(period_begin, period_end))
                idx += 2
                continue
            idx += 1

        return ret

    # extract case like "前两年" "前三个月"
    def match_number_with_unit(self, source: str) -> List[Token]:
        ret: List[Token] = list()
        durations: List[Token] = list()

        ers = self.config.integer_extractor.extract(source)

        for er in ers:
            after_str = source[er.start + er.length:]
            match = RegExpUtility.match_begin(self.config.followed_unit, after_str, trim=True)

            if match and match.success:
                durations.append(Token(er.start, er.start + er.length + match.length))

        if self.config.number_combined_with_unit.match(source):

            for match in self.config.number_combined_with_unit.finditer(source):
                durations.append(Token(match.start(), match.end()))

        for duration in durations:
            before_str = source[:duration.start]

            if not before_str:
                continue

            # Cases like 'first 2 weeks of 2018' (2021年的前2周)
            match = RegExpUtility.match_end(self.config.first_last_of_year_regex, before_str, trim=True)

            if match and match.success:
                # Check if the unit is compatible (day, week,  month)
                duration_str = source[duration.start:duration.length]
                unit_match = self.config.unit_regex.match(duration_str)

                if unit_match.group(Constants.UNIT_OF_YEAR_GROUP_NAME):
                    ret.append(Token(match.start, duration.end))
                    continue

            match = RegExpUtility.match_end(self.config.past_regex, before_str, trim=True)

            if match and match.success:
                ret.append(Token(match.index, duration.end))
                continue

            match = RegExpUtility.match_end(self.config.future_regex, before_str, trim=True)

            if match and match.success:
                ret.append(Token(match.index, duration.end))

        return ret

    # Complex cases refer to the combination of daterange and datepoint
    # For Example: from|between {DateRange|DatePoint} to|till|and {DateRange|DatePoint}
    def match_complex_cases(self, source: str, simple_cases_results: List[ExtractResult], reference: datetime) \
            -> List[Token]:
        er = self.config.date_point_extractor.extract(source, reference)

        # Filter out DateRange results that are part of DatePoint results
        # For example, "Feb 1st 2018" => "Feb" and "2018" should be filtered out here

        er.extend(list(
            filter(
                lambda simple_date_range: not any(
                    list(
                        filter(
                            lambda date_point: date_point.start <= simple_date_range.start and date_point.start +
                                               date_point.length >= simple_date_range.start + simple_date_range.length,
                            er))),
                simple_cases_results))
        )

        er = list(sorted(er, key=lambda x: x.start))

        return self.merge_multiple_extractions(source, er)

    def merge_multiple_extractions(self, source: str, extraction_results: List[ExtractResult]) -> List[Token]:
        ret: List[Token] = list()

        metadata = Metadata()
        metadata.possibly_included_period_end = True

        if len(extraction_results) <= 1:
            return ret

        idx = 0

        while idx < len(extraction_results) - 1:
            middle_begin = extraction_results[idx].start + extraction_results[idx].length
            middle_end = extraction_results[idx + 1].start

            if middle_begin >= middle_end:
                idx += 1
                continue

            middle_str = source[middle_begin:middle_end - middle_begin].strip().lower()
            end_point_str = extraction_results[idx + 1].text
            start_point_str = extraction_results[idx].text

            if (RegExpUtility.exact_match(self.config.till_regex, middle_str, trim=True)
                    or (not middle_str and
                        (RegExpUtility.match_begin(self.config.till_regex, end_point_str, trim=True) or
                         RegExpUtility.match_end(self.config.till_regex, start_point_str, trim=True)))):

                period_begin = extraction_results[idx].start
                period_end = extraction_results[idx + 1].start + extraction_results[idx + 1].length

                # handle "from/between" together with till words (till/until/through...)
                before_str = source[:period_begin]

                before_match = RegExpUtility.match_end(self.config.range_prefix_regex, before_str, trim=True)

                if before_match:
                    period_begin = before_match.index
                else:
                    after_str = source[period_end:]

                    after_match = RegExpUtility.match_begin(self.config.range_suffix_regex, after_str, trim=True)

                    if after_match:
                        period_end += after_match.index + after_match.length

                ret.append(Token(period_begin, period_begin, metadata))

                idx += 2
                continue
            idx += 1

        return ret


class CJKDatePeriodParserConfiguration(DateTimeOptionsConfiguration):

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
    def date_extractor(self) -> DateTimeExtractor:
        raise NotImplementedError

    @property
    @abstractmethod
    def duration_extractor(self) -> DateTimeExtractor:
        raise NotImplementedError

    @property
    @abstractmethod
    def cardinal_extractor(self) -> Extractor:
        raise NotImplementedError

    @property
    @abstractmethod
    def duration_parser(self) -> DateTimeParser:
        raise NotImplementedError

    @property
    @abstractmethod
    def date_parser(self) -> BaseDateParser:
        raise NotImplementedError

    @property
    @abstractmethod
    def dynasty_year_map(self) -> Dict[str, int]:
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
        raise NotImplementedError

    @property
    @abstractmethod
    def month_of_year(self) -> Dict[str, int]:
        raise NotImplementedError

    @property
    @abstractmethod
    def season_map(self) -> Dict[str, str]:
        raise NotImplementedError

    @property
    @abstractmethod
    def dynasty_start_year(self) -> str:
        raise NotImplementedError

    @property
    @abstractmethod
    def token_before_date(self) -> str:
        raise NotImplementedError

    @property
    @abstractmethod
    def two_num_year(self) -> int:
        raise NotImplementedError

    @property
    @abstractmethod
    def simple_cases_regex(self) -> Pattern:
        raise NotImplementedError

    @property
    @abstractmethod
    def dynasty_year_regex(self) -> Pattern:
        raise NotImplementedError

    @property
    @abstractmethod
    def year_regex(self) -> Pattern:
        raise NotImplementedError

    @property
    @abstractmethod
    def relative_regex(self) -> Pattern:
        raise NotImplementedError

    @property
    @abstractmethod
    def relative_month_regex(self) -> Pattern:
        raise NotImplementedError

    @property
    @abstractmethod
    def duration_relative_duration_unit_regex(self) -> Pattern:
        raise NotImplementedError

    @property
    @abstractmethod
    def this_regex(self) -> Pattern:
        raise NotImplementedError

    @property
    @abstractmethod
    def last_regex(self) -> Pattern:
        raise NotImplementedError

    @property
    @abstractmethod
    def next_regex(self) -> Pattern:
        raise NotImplementedError

    @property
    @abstractmethod
    def year_to_year(self) -> Pattern:
        raise NotImplementedError

    @property
    @abstractmethod
    def year_to_year_suffix_required(self) -> Pattern:
        raise NotImplementedError

    @property
    @abstractmethod
    def year_in_cjk_regex(self) -> Pattern:
        raise NotImplementedError

    @property
    @abstractmethod
    def month_to_month(self) -> Pattern:
        raise NotImplementedError

    @property
    @abstractmethod
    def month_to_month_suffix_required(self) -> Pattern:
        raise NotImplementedError

    @property
    @abstractmethod
    def month_regex(self) -> Pattern:
        raise NotImplementedError

    @property
    @abstractmethod
    def year_and_month(self) -> Pattern:
        raise NotImplementedError

    @property
    @abstractmethod
    def pure_num_year_and_month(self) -> Pattern:
        raise NotImplementedError

    @property
    @abstractmethod
    def one_word_period_regex(self) -> Pattern:
        raise NotImplementedError

    @property
    @abstractmethod
    def number_combined_with_unit(self) -> Pattern:
        raise NotImplementedError

    @property
    @abstractmethod
    def past_regex(self) -> Pattern:
        raise NotImplementedError

    @property
    @abstractmethod
    def future_regex(self) -> Pattern:
        raise NotImplementedError

    @property
    @abstractmethod
    def week_with_week_day_range_regex(self) -> Pattern:
        raise NotImplementedError

    @property
    @abstractmethod
    def unit_regex(self) -> Pattern:
        raise NotImplementedError

    @property
    @abstractmethod
    def duration_unit_regex(self) -> Pattern:
        raise NotImplementedError

    @property
    @abstractmethod
    def week_of_month_regex(self) -> Pattern:
        raise NotImplementedError

    @property
    @abstractmethod
    def week_of_year_regex(self) -> Pattern:
        raise NotImplementedError

    @property
    @abstractmethod
    def week_of_date_regex(self) -> Pattern:
        raise NotImplementedError

    @property
    @abstractmethod
    def month_of_date_regex(self) -> Pattern:
        raise NotImplementedError

    @property
    @abstractmethod
    def which_week_regex(self) -> Pattern:
        raise NotImplementedError

    @property
    @abstractmethod
    def first_last_of_year_regex(self) -> Pattern:
        raise NotImplementedError

    @property
    @abstractmethod
    def season_with_year(self) -> Pattern:
        raise NotImplementedError

    @property
    @abstractmethod
    def quarter_regex(self) -> Pattern:
        raise NotImplementedError

    @property
    @abstractmethod
    def decade_regex(self) -> Pattern:
        raise NotImplementedError

    @property
    @abstractmethod
    def century_regex(self) -> Pattern:
        raise NotImplementedError

    @property
    @abstractmethod
    def day_to_day(self) -> Pattern:
        raise NotImplementedError

    @property
    @abstractmethod
    def month_day_range(self) -> Pattern:
        raise NotImplementedError

    @property
    @abstractmethod
    def day_regex_for_period(self) -> Pattern:
        raise NotImplementedError

    @property
    @abstractmethod
    def simple_year_and_month(self) -> Pattern:
        raise NotImplementedError

    @property
    @abstractmethod
    def special_month_regex(self) -> Pattern:
        raise NotImplementedError

    @property
    @abstractmethod
    def special_year_regex(self) -> Pattern:
        raise NotImplementedError

    @property
    @abstractmethod
    def later_early_period_regex(self) -> Pattern:
        raise NotImplementedError

    @property
    @abstractmethod
    def date_point_with_ago_and_later(self) -> Pattern:
        raise NotImplementedError

    @property
    @abstractmethod
    def reference_date_period_regex(self) -> Pattern:
        raise NotImplementedError

    @property
    @abstractmethod
    def wom_last_regex(self) -> Pattern:
        raise NotImplementedError

    @property
    @abstractmethod
    def wom_previous_regex(self) -> Pattern:
        raise NotImplementedError

    @property
    @abstractmethod
    def wom_next_regex(self) -> Pattern:
        raise NotImplementedError

    @property
    @abstractmethod
    def complex_date_period_regex(self) -> Pattern:
        raise NotImplementedError

    @abstractmethod
    def to_month_number(self, month_str: str) -> int:
        raise NotImplementedError

    @abstractmethod
    def is_month_only(self, source: str) -> bool:
        raise NotImplementedError

    @abstractmethod
    def is_weekend(self, source: str) -> bool:
        raise NotImplementedError

    @abstractmethod
    def is_week_only(self, source: str) -> bool:
        raise NotImplementedError

    @abstractmethod
    def is_year_only(self, source: str) -> bool:
        raise NotImplementedError

    @abstractmethod
    def is_this_year(self, source: str) -> bool:
        raise NotImplementedError

    @abstractmethod
    def is_year_to_date(self, source: str) -> bool:
        raise NotImplementedError

    @abstractmethod
    def is_last_year(self, source: str) -> bool:
        raise NotImplementedError

    @abstractmethod
    def is_next_year(self, source: str) -> bool:
        raise NotImplementedError

    @abstractmethod
    def is_year_after_next(self, source: str) -> bool:
        raise NotImplementedError

    @abstractmethod
    def is_year_before_last(self, source: str) -> bool:
        raise NotImplementedError

    @abstractmethod
    def get_swift_month(self, source: str):
        raise NotImplementedError

    @abstractmethod
    def get_swift_year(self, source: str):
        raise NotImplementedError


class BaseCJKDatePeriodParser(DateTimeParser):

    @property
    def parser_type_name(self) -> str:
        return Constants.SYS_DATETIME_DATEPERIOD

    def __init__(self, config: CJKDatePeriodParserConfiguration, inclusive_end_period: bool = False):
        self.config = config
        self._inclusive_end_period = inclusive_end_period

    def get_year_context(self, start_date_str: str, end_date_str: str, source: str) -> DateContext:
        is_end_date_pure_year = False
        context_year = Constants.INVALID_YEAR

        year_match_for_end_date = self.config.year_regex.match(end_date_str)

        if year_match_for_end_date and (year_match_for_end_date.end() - year_match_for_end_date.start()) == len(end_date_str):
            is_end_date_pure_year = True

        relative_match_for_start_date = self.config.relative_regex.match(start_date_str)
        relative_match_for_end_date = self.config.relative_regex.match(end_date_str)
        is_date_relative = relative_match_for_start_date or relative_match_for_end_date

        if not is_end_date_pure_year and not is_date_relative:

            for match in self.config.year_regex.finditer(source):
                year = self.get_year_from_text(match)
                if year != Constants.INVALID_YEAR:
                    context_year = year
                else:
                    # this indicates that the text has two different year value, no common context year
                    if context_year != year:
                        context_year = Constants.INVALID_YEAR
                        break

        result: DateContext = DateContext()
        result.year = context_year

        return result

    def parse(self, er: ExtractResult, reference: datetime) -> DateTimeParseResult:
        if not reference:
            reference = datetime.now()
        result_value = None

        if er.type == self.parser_type_name:
            source_text = er.text.strip().lower()
            inner_result = self.parse_base_date_period(source_text, reference)

            if not inner_result.success:
                inner_result = self.parse_complex_date_period(source_text, reference)

            if inner_result.success:

                if inner_result.mod == Constants.BEFORE_MOD:
                    inner_result.future_resolution = {
                        TimeTypeConstants.END_DATE: DateTimeFormatUtil.format_date(inner_result.future_value[0])
                    }
                    inner_result.past_resolution = {
                        TimeTypeConstants.END_DATE: DateTimeFormatUtil.format_date(inner_result.past_value[0])
                    }

                elif inner_result.mod == Constants.AFTER_MOD:
                    inner_result.future_resolution = {
                        TimeTypeConstants.START_DATE: DateTimeFormatUtil.format_date(inner_result.future_value[0])
                    }
                    inner_result.past_resolution = {
                        TimeTypeConstants.START_DATE: DateTimeFormatUtil.format_date(inner_result.past_value[0])
                    }

                elif inner_result.future_value and inner_result.past_value:
                    inner_result.future_resolution = {
                        TimeTypeConstants.START_DATE: DateTimeFormatUtil.format_date(inner_result.future_value[0]),
                        TimeTypeConstants.END_DATE: DateTimeFormatUtil.format_date(
                            inner_result.future_value[1])
                    }
                    inner_result.past_resolution = {
                        TimeTypeConstants.START_DATE: DateTimeFormatUtil.format_date(inner_result.past_value[0]),
                        TimeTypeConstants.END_DATE: DateTimeFormatUtil.format_date(
                            inner_result.past_value[1])
                    }

                else:
                    inner_result.future_resolution = inner_result.past_resolution = {}

                result_value = inner_result

        ret = DateTimeParseResult(er)
        ret.text = er.text
        ret.start = er.start
        ret.length = er.length
        ret.type = er.type
        ret.meta_data = er.meta_data
        ret.value = result_value
        ret.timex_str = result_value.timex if result_value else ''
        ret.resolution_str = ''

        return ret

    def parse_base_date_period(self, source: str, reference: datetime,
                               date_context: DateContext = None) -> DateTimeResolutionResult:

        inner_result = self.parse_simple_cases(source, reference)

        if not inner_result.success:
            inner_result = self.parse_duration(source, reference)
        if not inner_result.success:
            inner_result = self.parse_one_word_period(source, reference)
        if not inner_result.success:
            inner_result = self.parse_number_with_unit(source, reference)
        if not inner_result.success:
            inner_result = self.parse_day_to_day(source, reference)
        if not inner_result.success:
            inner_result = self.merge_two_time_points(source, reference)
        if not inner_result.success:
            inner_result = self.parse_year_and_month(source, reference)
        if not inner_result.success:
            inner_result = self.parse_year_to_year(source, reference)
        if not inner_result.success:
            inner_result = self.parse_month_to_month(source, reference)
        if not inner_result.success:
            inner_result = self.parse_year(source, reference)
        if not inner_result.success:
            inner_result = self.parse_week_of_month(source, reference)
        if not inner_result.success:
            inner_result = self.parse_week_of_year(source, reference)
        if not inner_result.success:
            inner_result = self.parse_week_of_date(source, reference)
        if not inner_result.success:
            inner_result = self.parse_month_of_date(source, reference)
        if not inner_result.success:
            inner_result = self.parse_which_week(source, reference)
        if not inner_result.success:
            inner_result = self.parse_season(source, reference)
        if not inner_result.success:
            inner_result = self.parse_quarter(source, reference)
        if not inner_result.success:
            inner_result = self.parse_decade(source, reference)
        if not inner_result.success:
            inner_result = self.parse_century(source, reference)
        # if not inner_result.success:
        #     inner_result = self.parse_date_point_with_ago_and_later(source, reference)
        if not inner_result.success and date_context:
            inner_result = date_context.process_date_period_entity_resolution(inner_result)

        return inner_result

    def get_year_from_text(self, match: Match) -> int:
        year = Constants.INVALID_YEAR

        year_str = match.group(Constants.YEAR_GROUP_NAME)
        written_year_str = match.groupdict().get(Constants.FULL_YEAR_GROUP_NAME)

        if year_str and year_str != written_year_str:
            year = self.convert_cjk_to_integer(year_str)

            if 100 > year >= Constants.MIN_TWO_DIGIT_YEAR_PAST_NUM:
                year += Constants.BASE_YEAR_PAST_CENTURY
            elif 0 <= year < Constants.MAX_TWO_DIGIT_YEAR_FUTURE_NUM:
                year += Constants.BASE_YEAR_CURRENT_CENTURY
        else:
            first_two_year_num_str = match.group(Constants.FIRST_TWO_YEAR_NUM)

            if first_two_year_num_str:
                er = ExtractResult()
                er.text = first_two_year_num_str
                er.start = match.get_group(Constants.FIRST_TWO_YEAR_NUM).start
                er.length = match.get_group(Constants.FIRST_TWO_YEAR_NUM).length

                first_two_year_num = int(self.config.number_parser.parse(er).value)

                last_two_year_num = 0
                last_two_year_num_str = match.get_group(Constants.LAST_TWO_YEAR_NUM)

                if last_two_year_num_str:
                    er.text = last_two_year_num_str
                    er.start = match.group(Constants.LAST_TWO_YEAR_NUM).start
                    er.length = match.group(Constants.LAST_TWO_YEAR_NUM).length

                    last_two_year_num = self.config.number_parser.parse(er)

                # Exclude pure number like "nineteen", "twenty four"
                if (first_two_year_num < 100 and last_two_year_num == 0) or \
                        (first_two_year_num < 100 and first_two_year_num % 10 == 0 and len(
                            last_two_year_num_str.strip().split(' ')) == 1):
                    year = Constants.INVALID_YEAR
                    return year
                if first_two_year_num >= 100:
                    year = first_two_year_num + last_two_year_num
                else:
                    year = (first_two_year_num * 100) + last_two_year_num
            else:
                if written_year_str:
                    er = ExtractResult
                    er.text = written_year_str
                    er.start = match.group(Constants.FULL_YEAR_GROUP_NAME).start
                    er.length = match.group(Constants.FULL_YEAR_GROUP_NAME).length

                    year = int(self.config.number_parser.parse(er).value)

                    if 100 > year >= Constants.MIN_TWO_DIGIT_YEAR_PAST_NUM:
                        year += Constants.BASE_YEAR_PAST_CENTURY
                    elif 0 <= year < Constants.MAX_TWO_DIGIT_YEAR_FUTURE_NUM:
                        year += Constants.BASE_YEAR_CURRENT_CENTURY

        return year

    def shift_resolution(self, date: [datetime, datetime], match: Match, start: bool) -> datetime:
        result = date[0]
        return result

    def convert_cjk_to_num(self, num_str: str) -> int:
        num = -1
        er = self.config.integer_extractor.extract(num_str)

        if er and er[0].type == Num_Constants.SYS_NUM_INTEGER:
            num = int(self.config.number_parser.parse(er[0]).value)

        return num

    # Convert CJK Year to Integer
    def convert_cjk_to_integer(self, year_cjk_str: str) -> int:
        num = 0

        dynasty_year = DateTimeFormatUtil.parse_dynasty_year(year_cjk_str,
                                                             self.config.dynasty_year_regex,
                                                             self.config.dynasty_start_year,
                                                             self.config.dynasty_year_map,
                                                             self.config.integer_extractor,
                                                             self.config.number_parser)

        if dynasty_year and dynasty_year > 0:
            return dynasty_year

        er = self.config.integer_extractor.extract(year_cjk_str)

        if er and er[0].type == Num_Constants.SYS_NUM_INTEGER:
            num = int(self.config.number_parser.parse(er[0]).value)

        if num < 10:
            num = 0

            for ch in year_cjk_str:

                num *= 10
                er = next(iter(self.config.integer_extractor.extract(ch)), None)
                if er and er.type == Num_Constants.SYS_NUM_INTEGER:
                    num += int(self.config.number_parser.parse(er).value)
            year = num
        else:
            year = num

        return -1 if year == 0 else year

    def parse_single_time_point(self, source: str, reference: datetime, date_context: DateContext = None) \
            -> DateTimeResolutionResult:
        ret = DateTimeResolutionResult()
        er = self.config.date_extractor.extract(source, reference)[0]

        if er:
            pr = self.config.date_parser.parse(er, reference)

            if pr:
                ret.timex = f'({pr.timex_str})'
                ret.future_value = pr.value.future_value
                ret.past_value = pr.value.past_value
                ret.success = True

        return ret

    def parse_simple_cases(self, source: str, reference: datetime) -> DateTimeResolutionResult:
        ret = DateTimeResolutionResult()
        year = reference.year
        month = reference.month

        no_year = False
        input_year = False

        match = self.config.simple_cases_regex.search(source)

        if not match:
            match = self.config.month_day_range.search(source)

        if match:
            days = match.captures(Constants.DAY_GROUP_NAME)
            begin_day = self.config.day_of_month[days[0]]
            end_day = self.config.day_of_month[days[1]]

            month_str = match.group(Constants.MONTH_GROUP_NAME)
            year_str = match.group(Constants.YEAR_GROUP_NAME)

            if year_str:
                year = self.parse_num_year(year_str)

                input_year = True
            else:
                no_year = True

            if month_str:
                month = self.config.to_month_number(month_str)
            else:
                month_str = match.group(Constants.REL_MONTH)
                this_match = self.config.this_regex.match(month_str)
                next_match = self.config.next_regex.match(month_str)

                if this_match:
                    pass
                elif next_match:
                    if month != 12:
                        month += 1
                    else:
                        month = 1
                        year += 1
                else:
                    if month != 1:
                        month -= 1
                    else:
                        month = 12
                        year -= 1

            begin_luis_str = DateTimeFormatUtil.luis_date(year if input_year or self.config.this_regex.match(month_str)
                                                                  or self.config.next_regex.match(month_str) else -1,
                                                          month, begin_day)
            end_luis_str = DateTimeFormatUtil.luis_date(year if input_year or self.config.this_regex.match(month_str)
                                                                or self.config.next_regex.match(month_str) else -1,
                                                        month, end_day)

        else:
            match = RegExpUtility.exact_match(self.config.special_year_regex, source, trim=True)

            if match and match.success:
                value = reference + datedelta(years=self.config.get_swift_year(match))
                ret.timex = DateTimeFormatUtil.luis_date_from_datetime(value)
                ret.future_value = ret.past_value = value
                ret.success = True

                return ret

            return ret

        future_begin_dates, past_begin_dates = DateUtils.generate_dates(no_year, reference, year, month, begin_day)
        future_end_dates, past_end_dates = DateUtils.generate_dates(no_year, reference, year, month, end_day)

        ret.timex = f'({begin_luis_str},{end_luis_str},P{end_day - begin_day}D)'
        ret.future_value = [future_begin_dates, future_end_dates]
        ret.past_value = [past_begin_dates, past_end_dates]
        ret.success = True

        return ret

    def parse_year_to_year(self, source: str, reference: datetime) -> DateTimeResolutionResult:
        ret = DateTimeResolutionResult()
        match = self.config.year_to_year.match(source)

        if not match:
            match = self.config.year_to_year_suffix_required.match(source)

        if match:
            year_match = list(self.config.year_regex.finditer(source))
            year_in_cjk_match = list(self.config.year_in_cjk_regex.finditer(source))

            begin_year = 0
            end_year = 0

            if len(year_match) == 2:
                begin_year = self.convert_cjk_to_integer(year_match[0].group(Constants.YEAR_GROUP_NAME))
                end_year = self.convert_cjk_to_integer(year_match[1].group(Constants.YEAR_GROUP_NAME))

            elif len(year_in_cjk_match) == 2:
                begin_year = self.convert_cjk_to_integer(year_in_cjk_match[0].group(Constants.YEAR_CJK_GROUP_NAME))
                end_year = self.convert_cjk_to_integer(year_in_cjk_match[1].group(Constants.YEAR_CJK_GROUP_NAME))

            elif len(year_in_cjk_match) == 1 and len(year_match) == 1:

                if year_match[0].start < year_in_cjk_match[0].start:
                    begin_year = int(year_match[0].get_group(Constants.YEAR_GROUP_NAME))
                    end_year = self.convert_cjk_to_integer(
                        year_in_cjk_match[0].get_group(Constants.YEAR_CJK_GROUP_NAME))

                else:
                    begin_year = self.convert_cjk_to_integer(
                        year_in_cjk_match[0].get_group(Constants.YEAR_CJK_GROUP_NAME))
                    end_year = int(year_match[0].get_group(Constants.YEAR_GROUP_NAME))

            if 100 > begin_year >= self.config.two_num_year:
                begin_year += Constants.BASE_YEAR_PAST_CENTURY
            elif begin_year < 100 and begin_year < self.config.two_num_year:
                begin_year += Constants.BASE_YEAR_CURRENT_CENTURY

            if 100 > end_year >= self.config.two_num_year:
                end_year += Constants.BASE_YEAR_PAST_CENTURY
            elif end_year < 100 and end_year < self.config.two_num_year:
                end_year += Constants.BASE_YEAR_CURRENT_CENTURY

            begin_day = DateUtils.safe_create_from_min_value(begin_year, 1, 1)
            end_day = DateUtils.safe_create_from_min_value(end_year, 1, 1)
            ret.timex = TimexUtil.generate_date_period_timex(begin_day, end_day, timex_type=3)
            ret.future_value = ret.past_value = [begin_day, end_day]
            ret.success = True

            return ret

        return ret

    # handle like "3月到5月", "3月和5月之间"
    def parse_month_to_month(self, source: str, reference: datetime) -> DateTimeResolutionResult:
        ret = DateTimeResolutionResult()
        match = self.config.month_to_month.match(source)

        if not match:
            match = self.config.month_to_month_suffix_required.match(source)

        if match:
            month_match = RegExpUtility.get_matches(self.config.month_regex, source)

            begin_month = 0
            end_month = 0

            if len(month_match) == 2:
                begin_month = self.config.to_month_number(month_match[0].get_group(Constants.MONTH_GROUP_NAME))
                end_month = self.config.to_month_number(month_match[1].get_group(Constants.MONTH_GROUP_NAME))

            elif match.group(Constants.MONTH_FROM_GROUP_NAME) and match.group(Constants.MONTH_TO_GROUP_NAME):
                begin_month = self.config.to_month_number(match.group(Constants.MONTH_FROM_GROUP_NAME))
                end_month = self.config.to_month_number(match.group(Constants.MONTH_TO_GROUP_NAME))

            year_match = RegExpUtility.get_matches(self.config.year_regex, source)
            has_year = False

            if len(year_match) > 0 and match.get_group(Constants.YEAR_GROUP_NAME):
                has_year = True

                if len(year_match) == 2:
                    begin_year = self.parse_num_year(year_match[0].get_group(Constants.YEAR_GROUP_NAME))
                    end_year = self.parse_num_year(year_match[1].get_group(Constants.YEAR_GROUP_NAME))

                else:
                    begin_year = end_year = self.parse_num_year(year_match[0].get_group(Constants.YEAR_GROUP_NAME))

            else:
                begin_year = end_year = reference.year

            curr_year = reference.year
            curr_month = reference.month
            begin_year_for_past_resolution = begin_year
            end_year_for_past_resolution = end_year
            begin_year_for_future_resolution = begin_year
            end_year_for_future_resolution = end_year
            duration_months = 0

            if has_year:
                diff_months = end_month - begin_month
                diff_year = end_year - begin_year

                duration_months = (diff_year * 12) + diff_months

            else:
                if begin_month < end_month:

                    # For this case, FutureValue and PastValue share the same resolution
                    if begin_month < curr_month <= end_month:
                        # Keep the beginYear and endYear equal to currentYear
                        pass

                    elif begin_month >= curr_month:
                        begin_year_for_past_resolution = end_year_for_past_resolution = curr_year - 1

                    elif end_month < curr_month:
                        begin_year_for_future_resolution = end_year_for_future_resolution = curr_year + 1

                    duration_months = end_month - begin_month

                elif begin_month > end_month:

                    # For this case, FutureValue and PastValue share the same resolution
                    if begin_month < curr_month:
                        end_year_for_past_resolution = end_year_for_future_resolution = curr_year + 1
                    else:
                        begin_year_for_past_resolution = curr_year - 1
                        end_year_for_future_resolution = curr_year + 1

                    duration_months = begin_month - end_month

            if duration_months != 0:
                begin_date_for_past_resolution = DateUtils.safe_create_from_min_value(begin_year_for_past_resolution,
                                                                                      begin_month, 1)
                end_date_for_past_resolution = DateUtils.safe_create_from_min_value(end_year_for_past_resolution,
                                                                                    end_month, 1)
                begin_date_for_future_resolution = DateUtils.safe_create_from_min_value(
                    begin_year_for_future_resolution,
                    begin_month, 1)
                end_date_for_future_resolution = DateUtils.safe_create_from_min_value(end_year_for_future_resolution,
                                                                                      end_month, 1)

                day_match = RegExpUtility.get_matches(self.config.day_regex_for_period, source)

                # handle cases like 2019年2月1日から3月まで
                if day_match and match.get_group(Constants.DAY_GROUP_NAME):
                    ret.timex = TimexUtil.generate_date_period_timex(begin_date_for_future_resolution,
                                                                     end_date_for_future_resolution, 0,
                                                                     begin_date_for_past_resolution,
                                                                     end_date_for_past_resolution, has_year)

                # If the year is not specified, the combined range timex will use fuzzy years.
                else:
                    ret.timex = TimexUtil.generate_date_period_timex(begin_date_for_future_resolution,
                                                                     end_date_for_future_resolution, 2,
                                                                     begin_date_for_past_resolution,
                                                                     end_date_for_past_resolution, has_year)

                ret.past_value = [begin_date_for_past_resolution, end_date_for_past_resolution]
                ret.future_value = [begin_date_for_future_resolution, end_date_for_future_resolution]
                ret.success = True

        return ret

    def parse_num_year(self, year_num: str) -> int:
        year = self.convert_cjk_to_integer(year_num)

        if 100 > year >= self.config.two_num_year:
            year += Constants.BASE_YEAR_PAST_CENTURY
        elif year < 100 and year < self.config.two_num_year:
            year += Constants.BASE_YEAR_CURRENT_CENTURY

        return year

    def parse_day_to_day(self, source: str, reference: datetime) -> DateTimeResolutionResult:
        ret = DateTimeResolutionResult()
        match = self.config.day_to_day.match(source)

        if match:
            day_match_match = list(regex.finditer(self.config.day_regex_for_period, source))

            begin_day = end_day = 0

            if len(day_match_match) == 2:
                day_from = RegExpUtility.get_group(day_match_match[0], Constants.DAY_GROUP_NAME)
                day_to = RegExpUtility.get_group(day_match_match[1], Constants.DAY_GROUP_NAME)

                begin_day = self.config.day_of_month[day_from]
                end_day = self.config.day_of_month[day_to]

            elif match.group(Constants.HALF_GROUP_NAME):
                er = self.config.duration_extractor.extract(match.group(Constants.HALF_GROUP_NAME), reference)
                pr = self.config.duration_parser.parse(er[0], reference)

                num = TimexUtil.parse_number_from_duration_timex(pr.timex_str)

                begin_day_2 = reference
                end_day_2 = reference + datedelta(days=num)

                ret.timex = TimexUtil.generate_date_period_timex(begin_day_2, end_day_2, timex_type=0)
                ret.past_value = ret.future_value = [begin_day_2, end_day_2]
                ret.success = True
                return ret

            begin_year_for_past_resolution = reference.year
            end_year_for_past_resolution = reference.year
            begin_year_for_future_resolution = reference.year
            end_year_for_future_resolution = reference.year

            curr_day = reference.day
            duration_months = duration_days = 0

            relative_month = self.config.relative_month_regex.match(source)
            curr_month = reference + datedelta(months=self.config.get_swift_month(relative_month.value)) \
                if relative_month else reference.month

            begin_month_for_past_resolution = curr_month
            end_month_for_past_resolution = curr_month
            begin_month_for_future_resolution = curr_month
            end_month_for_future_resolution = curr_month

            if begin_day < end_day:

                # For this case, FutureValue and PastValue share the same resolution
                if begin_day < curr_day <= end_day:
                    # Keep the beginMonth and endMonth equal to currentMonth
                    pass

                elif begin_day >= curr_day:
                    if curr_month == 1:
                        begin_month_for_past_resolution = end_month_for_past_resolution = Constants.MAX_MONTH
                        begin_year_for_past_resolution -= 1
                        end_year_for_past_resolution -= 1
                    else:
                        begin_month_for_past_resolution = end_month_for_past_resolution = curr_month - 1

                elif end_day < curr_day:
                    if curr_month == Constants.MAX_MONTH:
                        begin_month_for_future_resolution = end_month_for_future_resolution = 1
                        begin_year_for_future_resolution += 1
                        end_year_for_future_resolution += 1
                    else:
                        begin_month_for_future_resolution = end_month_for_future_resolution = curr_month + 1

                duration_days = end_day - begin_day

            elif begin_day > end_day:

                # For this case, FutureValue and PastValue share the same resolution
                if begin_day < curr_day:
                    if curr_month == Constants.MAX_MONTH:
                        end_month_for_past_resolution = end_month_for_future_resolution = 1
                        end_year_for_past_resolution += 1
                        end_year_for_future_resolution += 1
                    else:
                        end_month_for_past_resolution = end_month_for_future_resolution = curr_month + 1
                else:
                    if curr_month == Constants.MAX_MONTH:
                        begin_month_for_past_resolution = curr_month - 1
                        end_month_for_future_resolution = 1
                        end_year_for_future_resolution += 1
                    elif curr_month == 1:
                        begin_month_for_past_resolution = 12
                        begin_year_for_past_resolution -= 1
                        end_month_for_future_resolution = curr_month + 1
                    else:
                        begin_month_for_past_resolution = curr_month - 1
                        end_month_for_future_resolution = curr_month + 1
                duration_days = begin_day - end_day

            if duration_days != 0:
                begin_date_for_past_resolution = DateUtils.safe_create_from_min_value(begin_year_for_past_resolution,
                                                                                      begin_month_for_past_resolution,
                                                                                      begin_day)
                end_date_for_past_resolution = DateUtils.safe_create_from_min_value(end_year_for_past_resolution,
                                                                                    end_month_for_past_resolution,
                                                                                    end_day)
                begin_date_for_future_resolution = DateUtils.safe_create_from_min_value(
                    begin_year_for_future_resolution,
                    begin_month_for_future_resolution,
                    begin_day)
                end_date_for_future_resolution = DateUtils.safe_create_from_min_value(end_year_for_future_resolution,
                                                                                      end_month_for_future_resolution,
                                                                                      end_day)

                if relative_month:
                    ret.timex = TimexUtil.generate_date_period_timex(begin_date_for_future_resolution,
                                                                     end_date_for_future_resolution, 0)
                else:
                    ret.timex = TimexUtil.generate_date_period_timex(begin_date_for_future_resolution,
                                                                     end_date_for_future_resolution, 0,
                                                                     has_year=False, has_month=False)

                ret.past_value = [begin_date_for_past_resolution, end_date_for_past_resolution]
                ret.future_value = [begin_date_for_future_resolution, end_date_for_future_resolution]
                ret.success = True

        return ret

    # for case "2016年5月"
    def parse_year_and_month(self, source: str, reference: datetime) -> DateTimeResolutionResult:
        ret = DateTimeResolutionResult()
        match = RegExpUtility.exact_match(self.config.year_and_month, source, trim=True)

        if not match or not match.success:
            match = RegExpUtility.exact_match(self.config.pure_num_year_and_month, source, trim=True)

        if not match or not match.success:
            return ret

        year = reference.year
        year_num = match.get_group(Constants.YEAR_GROUP_NAME)
        year_cjk = match.get_group(Constants.YEAR_CJK_GROUP_NAME)
        year_rel = match.get_group(Constants.YEAR_RELATIVE)
        cardinal_str = match.get_group(Constants.CARDINAL)

        if year_num:
            if self.config.is_year_only(year_num):
                year_num = year_num[:len(year_num) - 1]

            year = self.convert_cjk_to_integer(year_num)

        elif year_cjk:
            if self.config.is_year_only(year_cjk):
                year_cjk = year_cjk[:len(year_num) - 1]

            year = self.convert_cjk_to_integer(year_cjk)

        elif year_rel:
            if self.config.is_last_year(year_rel):
                year -= 1
            elif self.config.is_next_year(year_rel):
                year += 1

        if 100 > year >= self.config.two_num_year:
            year += int(Constants.BASE_YEAR_PAST_CENTURY)
        elif year < self.config.two_num_year:
            year += int(Constants.BASE_YEAR_CURRENT_CENTURY)

        month_str = match.get_group(Constants.MONTH_GROUP_NAME)

        if month_str:
            month = self.config.to_month_number(month_str)
        elif RegExpUtility.exact_match(self.config.wom_last_regex, cardinal_str, trim=True).success:
            month = 12
        else:
            month = self.config.cardinal_map[cardinal_str]

        begin_day = DateUtils.safe_create_from_min_value(year, month, 1)

        if month == 12:
            end_day = DateUtils.safe_create_from_min_value(year + 1, 1, 1)
        else:
            end_day = DateUtils.safe_create_from_min_value(year, month + 1, 1)

        ret.timex = DateTimeFormatUtil.luis_date(year, month, 1)
        ret.future_value = ret.past_value = [begin_day, end_day]
        ret.success = True
        return ret

    # case like "今年三月" "这个周末" "五月"
    def parse_one_word_period(self, source: str, reference: datetime) -> DateTimeResolutionResult:
        ret = DateTimeResolutionResult()
        year = reference.year
        month = reference.month
        future_year = past_year = year

        is_reference_date_period = False

        trimmed_source = source.lower()
        match = RegExpUtility.exact_match(self.config.one_word_period_regex, trimmed_source, trim=True)

        # For cases "that week|month|year"
        if not match:
            match = RegExpUtility.exact_match(self.config.reference_date_period_regex, trimmed_source, trim=True)

            if match and match.success:
                is_reference_date_period = True
                ret.mod = TimeTypeConstants.REF_UNDEF_MOD

        if not match:
            match = RegExpUtility.exact_match(self.config.later_early_period_regex, trimmed_source, trim=True)

            if match and match.success:
                self.parse_later_early_period(source, reference)

        if match and match.success:
            month_str = match.get_group(Constants.MONTH_GROUP_NAME)

            if self.config.is_this_year(trimmed_source):
                ret.timex = TimexUtil.generate_year_timex(reference)
                ret.future_value = ret.past_value = [DateUtils.safe_create_from_min_value(year, 1, 1), reference]
                ret.success = True
                return ret

            if self.config.is_year_to_date(trimmed_source):
                ret.timex = TimexUtil.generate_year_timex(reference.year)
                ret.future_value = ret.past_value = [DateUtils.safe_create_from_min_value(year, 1, 1), reference]
                ret.success = True
                return ret

            next_match = self.config.next_regex.match(trimmed_source)
            last_match = self.config.last_regex.match(trimmed_source)

            if month_str:

                swift = -10
                year_rel = match.get_group(Constants.YEAR_RELATIVE)

                if year_rel:
                    if self.config.is_next_year(year_rel):
                        swift = 1
                    elif self.config.is_last_year(year_rel):
                        swift = -1
                    elif self.config.is_this_year(year_rel):
                        swift = 0

                month = self.config.to_month_number(month_str)

                if swift >= -1:
                    year += swift
                    ret.timex = DateTimeFormatUtil.luis_date(year, month, 1)
                    future_year = past_year = year
                else:
                    ret.timex = DateTimeFormatUtil.luis_date(Constants.INVALID_YEAR, month, 1)

                    if month < reference.month:
                        future_year += 1

                    if month >= reference.month:
                        past_year -= 1
            else:
                swift = 0
                if next_match:
                    if next_match.group(Constants.AFTER_MOD):
                        swift = 2
                    else:
                        swift = 1
                elif last_match:
                    swift = -1

                # Handle cases with "(上|下)半" like "上半月"、 "下半年"
                if match.get_group(Constants.HALF_TAG_GROUP_NAME):
                    return self.handle_with_half_tag(source, reference, ret, swift)

                if self.config.is_week_only(trimmed_source):
                    monday = DateUtils.this(reference, DayOfWeek.MONDAY) + datedelta(days=7 * swift)
                    ret.timex = TimexUtil.generate_week_timex() if is_reference_date_period else \
                        TimexUtil.generate_week_timex(monday)
                    ret.future_value = ret.past_value = [monday, DateUtils.this(reference, DayOfWeek.SUNDAY)
                                                         + datedelta(days=7 * swift) + datedelta(days=1)]
                    ret.success = True

                    return ret
                elif match.get_group(Constants.REST_OF_GROUP_NAME):
                    duration_str = match.get_group(Constants.SYS_DATETIME_DURATION)
                    duration_unit = self.config.unit_map[duration_str]
                    begin_date = end_date = reference

                    ret.timex = TimexUtil.generate_date_period_timex_with_diff(begin_date, end_date, duration_unit)
                    ret.future_value = ret.past_value = [begin_date, end_date]
                    ret.success = True
                    return ret

                if self.config.is_weekend(trimmed_source):
                    begin_date = DateUtils.this(reference, DayOfWeek.SATURDAY) + datedelta(days=7 * swift)
                    end_date = DateUtils.this(reference, DayOfWeek.SUNDAY) + datedelta(days=7 * swift)

                    match = RegExpUtility.exact_match(self.config.reference_date_period_regex, trimmed_source,
                                                      trim=True)

                    if match and match.success:
                        is_reference_date_period = True
                        ret.mod = TimeTypeConstants.REF_UNDEF_MOD

                    ret.timex = TimexUtil.generate_weekend_timex() if is_reference_date_period else \
                        TimexUtil.generate_weekend_timex(begin_date)
                    ret.future_value = ret.past_value = [begin_date, end_date + datedelta(days=1)]
                    ret.success = True
                    return ret

                if self.config.is_month_only(trimmed_source):
                    month = int(reference.month + swift)
                    year = int(reference.year + swift)
                    ret.timex = DateTimeFormatUtil.luis_date(year, month, reference.day)
                    ret.timex = TimexUtil.generate_month_timex() if is_reference_date_period else \
                        DateTimeFormatUtil.luis_date(year, month, 1)
                    future_year = past_year = year

                elif self.config.is_year_only(trimmed_source):

                    # Handle like "今年上半年"，"明年下半年"
                    swift = 0
                    trimmed_source, has_half, is_first_half = self.handle_with_half_year(match, trimmed_source)
                    swift = 0 if has_half else swift

                    year = int(reference.year) + swift

                    if self.config.is_last_year(trimmed_source):
                        year -= 1
                    elif self.config.is_next_year(trimmed_source):
                        year += 1
                    elif self.config.is_year_before_last(trimmed_source):
                        year -= 2
                    elif self.config.is_year_after_next(trimmed_source):
                        year += 2

                    return self.handle_year_result(ret, year, is_reference_date_period, has_half, is_first_half)

        else:
            return ret

        # only "month" will come to here

        ret.future_value = [DateUtils.safe_create_from_min_value(future_year, month, 1),
                            DateUtils.safe_create_from_min_value(future_year, month, 1) + datedelta(months=1)]
        ret.past_value = [DateUtils.safe_create_from_min_value(past_year, month, 1),
                          DateUtils.safe_create_from_min_value(past_year, month, 1) + datedelta(months=1)]

        ret.success = True

        return ret

    def parse_later_early_period(self, source: str, reference: datetime) -> DateTimeResolutionResult:
        ret = DateTimeResolutionResult()
        year = reference.year
        month = reference.month

        future_year = past_year = year

        early_prefix = False
        late_prefix = False
        mid_prefix = False
        earlier_prefix = False
        later_prefix = False
        is_reference_date_period = False

        trimmed_source = source.lower()

        match = RegExpUtility.exact_match(self.config.later_early_period_regex, trimmed_source, trim=True)

        if match and match.success:
            if match.get_group(Constants.EARLY_PREFIX):
                early_prefix = True
                trimmed_source = str(match.get_group(Constants.SUFFIX_GROUP_NAME))
                ret.mod = TimeTypeConstants.EARLY_MOD

            elif match.get_group(Constants.LATE_PREFIX):
                late_prefix = True
                trimmed_source = str(match.get_group(Constants.SUFFIX_GROUP_NAME))
                ret.mod = TimeTypeConstants.LATE_MOD

            elif match.get_group(Constants.MID_PREFIX):
                mid_prefix = True
                trimmed_source = str(match.get_group(Constants.SUFFIX_GROUP_NAME))
                ret.mod = TimeTypeConstants.MID_MOD

            swift = 0
            if match.get_group(Constants.MONTH_GROUP_NAME):
                swift = self.config.get_swift_year(trimmed_source)
            else:
                if match.get_group(Constants.NEXT_GROUP_NAME):
                    swift = 1
                elif match.get_group(Constants.LAST_GROUP_NAME):
                    swift = -1

            if match.get_group(Constants.REL_EARLY):
                earlier_prefix = True
                if BaseDatePeriodParser.is_present(swift):
                    ret.mod = None
            elif match.get_group(Constants.REL_LATE):
                later_prefix = True
                if BaseDatePeriodParser.is_present(swift):
                    ret.mod = None

            month_str = match.get_group(Constants.MONTH_GROUP_NAME)

            # Parse expressions "till date", "to date"
            if match.get_group(Constants.TO_DATE_GROUP_NAME):
                ret.timex = "PRESENT_REF"
                ret.future_value = ret.past_value = reference
                ret.mod = TimeTypeConstants.BEFORE_MOD
                ret.success = True
                return ret

            if month_str:

                swift = self.config.get_swift_year(trimmed_source)
                month = self.config.month_of_year[month_str]

                if swift >= 1:
                    ret.timex = f'{reference + datedelta(years=swift):04d}-{month:02d}'

                    year = year + swift
                    future_year = past_year = year
                else:
                    ret.timex = f'XXXX-{month:02d}'

                    if month < reference.month:
                        future_year += 1
                    if month >= reference.month:
                        past_year -= 1

            else:
                if match.get_group(Constants.NEXT_GROUP_NAME):
                    swift = 1
                elif match.get_group(Constants.LAST_GROUP_NAME):
                    swift = -1

                is_working_week = match.get_group(Constants.BUSINESS_DAY_GROUP_NAME).success

                is_week_only = self.config.is_week_only(trimmed_source)

                if is_working_week or is_week_only:
                    delta = Constants.WEEK_DAY_COUNT * swift
                    end_delta = delta

                    monday = DateUtils.this(reference, DayOfWeek.MONDAY) + datedelta(days=delta)
                    end_day = DayOfWeek.FRIDAY if is_working_week else DayOfWeek.SUNDAY

                    begin_date = monday
                    end_date = DateUtils.this(reference, end_day) + datedelta(days=end_delta) \
                        if self._inclusive_end_period else \
                        DateUtils.this(reference, end_day) + datedelta(days=end_delta + 1)

                    if early_prefix:
                        end_date = DateUtils.this(reference, DayOfWeek.WEDNESDAY) + datedelta(days=end_delta) \
                            if self._inclusive_end_period else \
                            DateUtils.this(reference, DayOfWeek.WEDNESDAY) + datedelta(days=end_delta + 1)

                    elif mid_prefix:
                        begin_date = DateUtils.this(reference, DayOfWeek.TUESDAY) + datedelta(days=delta)
                        end_date = DateUtils.this(reference, DayOfWeek.FRIDAY) + datedelta(days=end_delta) \
                            if self._inclusive_end_period else \
                            DateUtils.this(reference, DayOfWeek.FRIDAY) + datedelta(days=end_delta + 1)

                    elif late_prefix:
                        begin_date = DateUtils.this(reference, DayOfWeek.THURSDAY) + datedelta(days=delta)

                    if earlier_prefix and swift == 0:
                        if end_date > reference:
                            end_date = reference
                    elif later_prefix and swift == 0:
                        if begin_date < reference:
                            begin_date = reference

                    ret.timex = TimexUtil.generate_week_timex() if is_reference_date_period else \
                        TimexUtil.generate_week_timex(monday)

                    if late_prefix and swift != 0:
                        ret.mod = TimeTypeConstants.LATE_MOD

                    ret.future_value = ret.past_value = [begin_date, end_date]
                    ret.success = True
                    return ret

                if self.config.is_weekend(trimmed_source):
                    begin_date = DateUtils.this(reference, DayOfWeek.SATURDAY) + \
                                 datedelta(days=Constants.WEEK_DAY_COUNT * swift)
                    end_date = DateUtils.this(reference, DayOfWeek.SUNDAY) + \
                               datedelta(days=Constants.WEEK_DAY_COUNT * swift)
                    ret.timex = TimexUtil.generate_weekend_timex() if is_reference_date_period else \
                        TimexUtil.generate_weekend_timex(begin_date)
                    end_date = end_date if self._inclusive_end_period else end_date + datedelta(days=1)
                    ret.future_value = ret.past_value = [begin_date, end_date]
                    ret.success = True
                    return ret

                if self.config.is_month_only(trimmed_source):
                    date = reference + datedelta(months=swift)
                    month = date.month
                    year = date.year
                    ret.timex = TimexUtil.generate_month_timex() if is_reference_date_period else \
                        TimexUtil.generate_month_timex(date)
                    future_year = past_year = year

                elif self.config.is_year_only(trimmed_source):
                    date = reference + datedelta(years=swift)
                    year = date.year

                    if match.get_group(Constants.SPECIAL_GROUP_NAME):
                        swift = self.config.get_swift_year(trimmed_source)
                        date = Constants.INVALID_YEAR if swift < -1 else date
                        ret.timex = TimexUtil.generate_year_timex(date, None)
                        ret.success = True
                        return ret

                    begin_date = DateUtils.safe_create_from_min_value(year, 1, 1)
                    end_date = DateUtils.safe_create_from_min_value(year, 12, 31) if self._inclusive_end_period else \
                        DateUtils.safe_create_from_min_value(year, 12, 31) + datedelta(days=1)

                    if early_prefix:
                        end_date = DateUtils.safe_create_from_min_value(year, 6, 30) if self._inclusive_end_period else \
                            DateUtils.safe_create_from_min_value(year, 6, 30) + datedelta(days=1)

                    elif mid_prefix:
                        begin_date = DateUtils.safe_create_from_min_value(year, 4, 1)
                        end_date = DateUtils.safe_create_from_min_value(year, 9, 30) if self._inclusive_end_period else \
                            DateUtils.safe_create_from_min_value(year, 9, 30) + datedelta(days=1)

                    elif late_prefix:
                        begin_date = DateUtils.safe_create_from_min_value(year, Constants.WEEK_DAY_COUNT, 1)

                    if earlier_prefix and swift == 0:
                        if end_date > reference:
                            end_date = reference

                    elif later_prefix and swift == 0:
                        if begin_date < reference:
                            begin_date = reference

                    ret.timex = TimexUtil.generate_year_timex() if is_reference_date_period else \
                        TimexUtil.generate_year_timex(date)
                    ret.future_value = ret.past_value = [begin_date, end_date]
                    ret.success = True

                    return ret

                # Early/mid/late are resolved in this policy to 4 month ranges at the start/middle/end of the year.
                elif match.get_group(Constants.FOUR_DIGIT_YEAR_GROUP_NAME):
                    date = reference + datedelta(years=swift)
                    year = int(match.get_group(Constants.FOUR_DIGIT_YEAR_GROUP_NAME))

                    begin_date = DateUtils.safe_create_from_min_value(year, 1, 1)
                    end_date = DateUtils.safe_create_from_min_value(year, 12, 31) if self._inclusive_end_period else \
                        DateUtils.safe_create_from_min_value(year, 12, 31) + datedelta(days=1)

                    if early_prefix:
                        end_date = DateUtils.safe_create_from_min_value(year, 4, 30) if self._inclusive_end_period \
                            else DateUtils.safe_create_from_min_value(year, 4, 30) + datedelta(days=1)
                    elif mid_prefix:
                        begin_date = DateUtils.safe_create_from_min_value(year, 5, 1)
                        end_date = DateUtils.safe_create_from_min_value(year, 8, 31) if self._inclusive_end_period \
                            else DateUtils.safe_create_from_min_value(year, 8, 31) + datedelta(days=1)

                    elif late_prefix:
                        begin_date = DateUtils.safe_create_from_min_value(year, 9, 1)

                    ret.timex = TimexUtil.generate_year_timex() if is_reference_date_period else \
                        TimexUtil.generate_year_timex(begin_date)
                    ret.future_value = ret.past_value = [begin_date, end_date]
                    ret.success = True

                    return ret

        else:
            return ret

        # only "month" will come to here
        future_start = DateUtils.safe_create_from_min_value(future_year, month, 1)
        future_end = DateUtils.safe_create_from_min_value(future_year, month, 1) + datedelta(months=1) - datedelta(
            days=1) \
            if self._inclusive_end_period else DateUtils.safe_create_from_min_value(future_year, month, 1) + datedelta(
            months=1)

        past_start = DateUtils.safe_create_from_min_value(past_year, month, 1)
        past_end = DateUtils.safe_create_from_min_value(past_year, month, 1) + datedelta(months=1) - datedelta(days=1) \
            if self._inclusive_end_period else DateUtils.safe_create_from_min_value(past_year, month, 1) + datedelta(
            months=1)

        if early_prefix:
            future_end = DateUtils.safe_create_from_min_value(future_year, month, 15) if self._inclusive_end_period else \
                DateUtils.safe_create_from_min_value(future_year, month, 15) + datedelta(days=1)

            past_end = DateUtils.safe_create_from_min_value(past_year, month, 15) if self._inclusive_end_period else \
                DateUtils.safe_create_from_min_value(past_year, month, 15) + datedelta(days=1)

        elif mid_prefix:
            future_start = DateUtils.safe_create_from_min_value(future_year, month, 10)
            past_start = DateUtils.safe_create_from_min_value(past_year, month, 10)

            future_end = DateUtils.safe_create_from_min_value(future_year, month, 20) if self._inclusive_end_period \
                else DateUtils.safe_create_from_min_value(future_year, month, 20) + datedelta(days=1)

            past_end = DateUtils.safe_create_from_min_value(past_year, month, 20) if self._inclusive_end_period \
                else DateUtils.safe_create_from_min_value(past_year, month, 20) + datedelta(days=1)

        elif late_prefix:
            future_start = DateUtils.safe_create_from_min_value(future_year, month, 16)
            past_start = DateUtils.safe_create_from_min_value(past_year, month, 16)

        if earlier_prefix and future_end == past_end:
            if future_end > reference:
                future_end = past_end = reference

        elif later_prefix and future_start == past_start:
            if future_start < reference:
                future_start = past_start = reference

        ret.future_value = [future_start, future_end]
        ret.past_value = [past_start, past_end]
        ret.success = True

        return ret

    def handle_with_half_tag(self, source: str, reference: datetime, ret: DateTimeResolutionResult, swift: int) \
            -> DateTimeResolutionResult:
        year = reference.year
        month = reference.month

        if self.config.is_week_only(source):

            # Handle like "上半周"，"下半周"
            begin_day = DateUtils.this(reference, DayOfWeek.MONDAY) if swift == -1 else \
                DateUtils.this(reference, DayOfWeek.THURSDAY)
            end_day = DateUtils.this(reference, DayOfWeek.THURSDAY) if swift == -1 else \
                DateUtils.this(reference, DayOfWeek.SUNDAY) + datedelta(days=1)
            ret.timex = TimexUtil.generate_date_period_timex(begin_day, end_day, timex_type=0)

        elif self.config.is_month_only(source):

            # Handle like "上半月"，"下半月"
            month_start_day = DateUtils.safe_create_from_min_value(year, month, 1)
            month_end_day = DateUtils.safe_create_from_min_value(year, month + 1, 1)
            half_month_day = int((month_end_day.day - month_start_day.day) / 2)

            begin_day = month_start_day if swift == -1 else month_start_day + datedelta(days=half_month_day)
            end_day = month_start_day + datedelta(days=half_month_day) if swift == -1 else month_end_day
            ret.timex = TimexUtil.generate_date_period_timex(begin_day, end_day, timex_type=0)

        else:
            # Handle like "上(个)半年"，"下(个)半年"
            begin_day = DateUtils.safe_create_from_min_value(year, 1, 1) if swift == -1 \
                else DateUtils.safe_create_from_min_value(year, 7, 1)
            end_day = DateUtils.safe_create_from_min_value(year, 7, 1) if swift == -1 \
                else DateUtils.safe_create_from_min_value(year + 1, 1, 1)
            ret.timex = TimexUtil.generate_date_period_timex(begin_day, end_day, timex_type=2)

        ret.future_value = ret.past_value = [begin_day, end_day]
        ret.success = True
        return ret

    # only contains year like "2016年" or "2016年上半年"
    def parse_year(self, source: str, reference: datetime) -> DateTimeResolutionResult:
        ret = DateTimeResolutionResult()
        match = RegExpUtility.exact_match(self.config.year_regex, source, trim=True)

        if match and match.success:
            year_str = match.value

            # Handle like "2016年上半年"，"2017年下半年"
            year_str = self.handle_with_half_year(match, year_str)[0]

            # Trim() to handle extra whitespaces like '07 年'
            if self.config.is_year_only(year_str):
                year_str = year_str[:len(year_str) - 1]

                year = self.convert_cjk_to_integer(year_str)

                return self.handle_year_result(ret, year, False)

        match = RegExpUtility.exact_match(self.config.year_in_cjk_regex, source, trim=True)

        if match and match.success:
            year_str = match.value

            # Handle like "二零一七年上半年"，"二零一七年下半年"
            year_str = self.handle_with_half_year(match, year_str)[0]

            if self.config.is_year_only(year_str):
                year_str = year_str[:len(year_str) - 1]

            if len(year_str) == 1:
                return ret

            year = self.convert_cjk_to_integer(year_str)

            return self.handle_year_result(ret, year, False)

        return ret

    def handle_with_half_year(self, match: ConditionalMatch, source: str) -> [str, bool, bool]:
        first_half = match.get_group(Constants.FIRST_HALF_GROUP_NAME)
        second_half = match.get_group(Constants.SECOND_HALF_GROUP_NAME)

        has_half = False
        is_first_half = True if first_half else False

        if is_first_half or second_half:
            half_text = first_half if is_first_half else second_half
            source = source[:len(source) - len(half_text)]
            has_half = True

        return source.lower(), has_half, is_first_half

    def handle_year_result(self, ret: DateTimeResolutionResult, year: int, is_reference_date_period: bool,
                           has_half: bool = False, is_first_half: bool = False) -> DateTimeResolutionResult:
        if 100 > year >= self.config.two_num_year:
            year += Constants.BASE_YEAR_PAST_CENTURY
        elif year < 100 and year < self.config.two_num_year:
            year += Constants.BASE_YEAR_CURRENT_CENTURY

        begin_day = DateUtils.safe_create_from_min_value(year, 1, 1)
        end_day = DateUtils.safe_create_from_min_value(year + 1, 1, 1)

        ret.timex = TimexUtil.generate_year_timex() if is_reference_date_period else \
            DateTimeFormatUtil.luis_date(year, 1, 1)

        if has_half:
            if is_first_half:
                end_day = DateUtils.safe_create_from_min_value(year, 7, 1)
            else:
                begin_day = DateUtils.safe_create_from_min_value(year, 7, 1)

            ret.timex = TimexUtil.generate_date_period_timex(begin_day, end_day, timex_type=2)

        ret.future_value = ret.past_value = [begin_day, end_day]
        ret.success = True

        return ret

    # parse entities that made up by two time points
    def merge_two_time_points(self, source: str, reference: datetime) -> DateTimeResolutionResult:
        ret = DateTimeResolutionResult()
        er = self.config.date_extractor.extract(source, reference)

        if len(er) < 2:
            er = self.config.date_extractor.extract(self.config.token_before_date + source, reference)

            if len(er) < 2:
                return ret

            er[0].start -= len(self.config.token_before_date)
            er[1].start -= len(self.config.token_before_date)

        pr1 = self.config.date_parser.parse(er[0], reference)
        pr2 = self.config.date_parser.parse(er[1], reference)

        if len(er) >= 2:

            match = self.config.week_with_week_day_range_regex.match(source)

            if match:
                week_prefix = str(match.group(Constants.WEEK_GROUP_NAME).value)

                # Check if weekPrefix is already included in the extractions otherwise include it
                if week_prefix:

                    if week_prefix not in er[0].text:
                        er[0].text = week_prefix + er[0].text

                    if week_prefix not in er[1].text:
                        er[1].text = week_prefix + er[1].text

                pr1 = self.config.date_parser.parse(er[0], reference)
                pr2 = self.config.date_parser.parse(er[1], reference)

            else:
                date_context = self.get_year_context(er[0].text, er[1].text, source)

                if pr1.value is None or pr2.value is None:
                    return ret

                pr1 = date_context.process_date_entity_parsing_result(pr1)
                pr2 = date_context.process_date_entity_parsing_result(pr2)

                if date_context.is_empty() and (DateUtils.is_Feb_29th_datetime(pr1.value.future_value) or
                                                DateUtils.is_Feb_29th_datetime(pr2.value.future_value)):
                    [pr1, pr2] = date_context.sync_year(pr1, pr2)

        future_begin = pr1.value.future_value
        future_end = pr2.value.future_value
        past_begin = pr1.value.past_value
        past_end = pr2.value.past_value

        if future_begin > future_end:
            future_begin = past_begin

        if past_end < past_begin:
            past_end = future_end

        ret.timex = TimexUtil.generate_date_period_timex_str(future_begin, future_end, 0, pr1.timex_str, pr2.timex_str)

        if pr1.timex_str.startswith(Constants.TIMEX_FUZZY_YEAR) and \
                future_begin <= DateUtils.safe_create_from_value(DateUtils.min_value, future_begin.year, 2, 28) and \
                future_end >= DateUtils.safe_create_from_value(DateUtils.min_value, future_begin.year, 3, 1):
            # Handle cases like "2月28日到3月1日".
            # There may be different timexes for FutureValue and PastValue due to the different validity of Feb 29th.

            ret.comment = Constants.COMMENT_DOUBLETIMEX
            past_timex = TimexUtil.generate_date_period_timex_str(past_begin, past_end, 0, pr1.timex_str, pr2.timex_str)
            ret.timex = TimexUtil.merge_timex_alternatives(ret.timex, past_timex)

        ret.future_value = [future_begin, future_end]
        ret.past_value = [past_begin, past_end]
        ret.success = True

        return ret

    # handle like "前两年" "前三个月"
    def parse_number_with_unit(self, source: str, reference: datetime) -> DateTimeResolutionResult:
        ret = DateTimeResolutionResult()

        # if there are NO spaces between number and unit
        match = self.config.number_combined_with_unit.match(source)

        if match:
            src_unit = match.group(Constants.UNIT_GROUP_NAME)

            before_str = source[:match.start()]

            if src_unit in self.config.unit_map:
                unit_str = self.config.unit_map[src_unit]
                num_str = match.group(Constants.NUM)
                num_int = int(num_str)

                if RegExpUtility.exact_match(self.config.past_regex, before_str, trim=True).success:
                    if unit_str == Constants.TIMEX_DAY:
                        begin_date = reference - datedelta(days=num_int)
                        end_date = reference
                    elif unit_str == Constants.TIMEX_WEEK:
                        begin_date = reference - datedelta(days=7 * num_int)
                        end_date = reference
                    elif unit_str == Constants.TIMEX_MONTH_FULL:
                        begin_date = reference - datedelta(months=num_int)
                        end_date = reference
                    elif unit_str == Constants.TIMEX_YEAR:
                        begin_date = reference - datedelta(years=num_int)
                        end_date = reference
                    else:
                        return ret

                    ret.timex = f'({DateTimeFormatUtil.luis_date(begin_date.year, begin_date.month, begin_date.day)},' \
                                f'{DateTimeFormatUtil.luis_date(end_date.year, end_date.month, end_date.day)},' \
                                f'P{num_str}{unit_str[0]})'
                    ret.future_value = ret.past_value = [begin_date, end_date]
                    ret.success = True
                    return ret

                if RegExpUtility.exact_match(self.config.future_regex, before_str, trim=True).success:
                    if unit_str == Constants.TIMEX_DAY:
                        begin_date = reference
                        end_date = reference + datedelta(days=num_int)
                    elif unit_str == Constants.TIMEX_WEEK:
                        begin_date = reference
                        end_date = reference + datedelta(days=7 * num_int)
                    elif unit_str == Constants.TIMEX_MONTH_FULL:
                        begin_date = reference
                        end_date = reference + datedelta(months=num_int)
                    elif unit_str == Constants.TIMEX_YEAR:
                        begin_date = reference
                        end_date = reference + datedelta(years=num_int)
                    else:
                        return ret

                    ret.timex = f'({DateTimeFormatUtil.luis_date(begin_date.year, begin_date.month, begin_date.day) + datedelta(days=1)},' \
                                f'{DateTimeFormatUtil.luis_date(end_date.year, end_date.month, end_date.day) + datedelta(days=1)},' \
                                f'P{num_str}{unit_str[0]})'
                    ret.future_value = ret.past_value = [begin_date + datedelta(days=1), end_date + datedelta(days=1)]
                    ret.success = True
                    return ret
        return ret

    # Analogous to the same method in BaseDatePeriodParser, it deals with date periods that involve durations
    # e.g. "past 2 years", "within 2 days", "first 2 weeks of 2018".
    def parse_duration(self, source: str, reference: datetime) -> DateTimeResolutionResult:
        ret = DateTimeResolutionResult()

        # For cases like 'first 2 weeks of 2021' (2021年的前2周), 'past 2 years' (前两年), 'next 3 years' (后三年)
        duration_res = self.config.duration_extractor.extract(source, reference)

        match_half = RegExpUtility.exact_match(self.config.one_word_period_regex, source, trim=True)

        # halfTag cases are processed in ParseOneWordPeriod
        if match_half.success and match_half.get_group(Constants.HALF_TAG_GROUP_NAME):
            ret.success = False
            return ret

        if len(duration_res) > 0:
            before_str = source[:duration_res[0].start]
            after_str = source[duration_res[0].start:]

            matches = list(regex.finditer(self.config.unit_regex, duration_res[0].text))
            # matches = RegExpUtility.get_matches(self.config.unit_regex, duration_res[0].text)
            match_business_days = RegExpUtility.exact_match(self.config.duration_unit_regex, source, trim=True)

            # handle duration cases like "5 years 1 month 21 days" and "multiple business days"
            if (1 < len(matches) <= 3) or (
                    matches and match_business_days.get_group(Constants.BUSINESS_DAY_GROUP_NAME)
            ):
                ret = self.parse_multiple_dates_duration(source, reference)
                return ret

            elif len(matches) == 1:
                src_unit = RegExpUtility.get_group(matches[0], Constants.UNIT_GROUP_NAME)
                src_unit_index = duration_res[0].text.index(src_unit)

                number_str = duration_res[0].text[:src_unit_index]
                match_few = self.config.duration_relative_duration_unit_regex.match(source)

                if match_few and number_str == match_few.get_group(Constants.FEW_GROUP_NAME).Value:
                    number = 3
                else:
                    number = self.convert_cjk_to_num(number_str)

                if src_unit in self.config.unit_map:
                    begin_date = reference
                    end_date = reference

                    unit_str = self.config.unit_map[src_unit]

                    # Get prefix
                    prefix_match = None

                    if self.config.unit_regex.match(src_unit).groups(Constants.UNIT_OF_YEAR_GROUP_NAME):
                        #  Patterns like 'first 2 weeks of 2018' are considered only if the unit is compatible
                        prefix_match = RegExpUtility.exact_match(self.config.first_last_of_year_regex, before_str,
                                                                 trim=True)

                    if prefix_match.success:
                        is_of_year_match = prefix_match.success
                        is_past_match = prefix_match.groups(Constants.LAST_GROUP_NAME)
                    else:
                        is_of_year_match = False
                        is_past_match = None

                    is_future = False

                    if not prefix_match:
                        prefix_match = self.config.past_regex.match_exact(before_str)
                        is_past_match = prefix_match.success

                    if not prefix_match:
                        prefix_match = RegExpUtility.exact_match(self.config.future_regex, before_str, trim=True)
                        is_future = prefix_match.success

                    if not prefix_match:
                        prefix_match = RegExpUtility.exact_match(self.config.future_regex, after_str, trim=True)
                        is_future = prefix_match.success

                    if is_future and not RegExpUtility.exact_match(self.config.future_regex, after_str, trim=True). \
                            get_groups(Constants.WITHIN_GROUP_NAME):
                        # for the "within" case it should start from the current day
                        begin_date = begin_date + datedelta(days=1)
                        end_date = end_date + datedelta(days=1)

                    #  Shift by year (if present)
                    if is_of_year_match:

                        #  Get Year
                        year = self.get_year_from_text(prefix_match)

                        if year == Constants.INVALID_YEAR:
                            swift = 0
                            year_rel = prefix_match.get_group(Constants.YEAR_RELATIVE)
                            if self.config.is_last_year(year_rel):
                                swift = -1
                            elif self.config.is_next_year(year_rel):
                                swift = 1
                            year = reference.year + datedelta(years=swift)

                        #  Get begin/end dates for year
                        if unit_str == Constants.TIMEX_WEEK:
                            # First/last week of the year is calculated according to ISO definition
                            begin_date = DateUtils.this(DateUtils.get_first_thursday(year), DayOfWeek.MONDAY)
                            end_date = DateUtils.this(DateUtils.get_last_thursday(year), DayOfWeek.MONDAY) \
                                       + datedelta(days=7)
                        else:
                            begin_date = DateUtils.safe_create_from_min_value(year, 1, 1)
                            end_date = DateUtils.safe_create_from_min_value(year, 12, 31) + datedelta(days=1)

                    # Shift begin/end dates by duration span
                    if prefix_match:
                        if is_past_match:
                            begin_date = end_date

                            if unit_str == Constants.TIMEX_DAY:
                                begin_date = begin_date - datedelta(days=number)
                                pass
                            elif unit_str == Constants.TIMEX_WEEK:
                                begin_date = begin_date - datedelta(days=7 * number)
                                pass
                            elif unit_str == Constants.TIMEX_MONTH_FULL:
                                begin_date = begin_date - datedelta(months=number)
                                pass
                            elif unit_str == Constants.TIMEX_YEAR:
                                begin_date = begin_date - datedelta(years=number)
                                pass
                            else:
                                return ret
                        else:
                            end_date = begin_date

                            if unit_str == Constants.TIMEX_DAY:
                                end_date = end_date + datedelta(days=number)
                                pass
                            elif unit_str == Constants.TIMEX_WEEK:
                                end_date = end_date + datedelta(days=7 * number)
                                pass
                            elif unit_str == Constants.TIMEX_MONTH_FULL:
                                end_date = end_date + datedelta(months=number)
                                pass
                            elif unit_str == Constants.TIMEX_YEAR:
                                end_date = end_date + datedelta(years=number)
                                pass
                            else:
                                return ret

                        ret.timex = f'({DateTimeFormatUtil.luis_date(begin_date.year, begin_date.month, begin_date.day)},\
                        {DateTimeFormatUtil.luis_date(end_date.year, end_date.month, end_date.day)},' \
                                    f'P{number}{unit_str[0]})'
                        ret.future_value = ret.past_value = [begin_date, end_date]
                        ret.success = True
                        return ret

        return ret

    def parse_multiple_dates_duration(self, source: str, reference: datetime) -> DateTimeResolutionResult:
        ret = DateTimeResolutionResult()
        begin_date = end_date = reference
        duration_timex = ''
        rest_now_sunday = False

        duration_ers = self.config.duration_extractor.extract(source, reference)

        if len(duration_ers) > 0:
            duration_pr = self.config.duration_parser.parse(duration_ers[0])
            before_str = source[:duration_pr.start]
            after_str = source[duration_pr.start + duration_pr.length]

            mod_and_date = ModAndDateResult()
            mod_and_date_result = mod_and_date.mod_and_date_result(begin_date, end_date)

            if duration_pr.value:
                duration_result = duration_pr.value

                if not duration_result.timex:
                    return ret

                if self.config.past_regex.match(before_str) or self.config.past_regex.match(after_str):
                    mod_and_date_result = ModAndDateResult.get_mod_and_date(begin_date, end_date, reference,
                                                                            duration_result.timex, False)

                if (RegExpUtility.exact_match(self.config.future_regex, before_str, trim=True) or
                    RegExpUtility.exact_match(self.config.future_regex, after_str, trim=True)) \
                        and DurationParsingUtil.is_date_duration(duration_result.timex):

                    mod_and_date_result = ModAndDateResult.get_mod_and_date(begin_date, end_date, reference,
                                                                            duration_result.timex, True)
                    begin_date = mod_and_date_result.begin_date
                    end_date = mod_and_date_result.end_date

                    # In GetModAndDate, this "future" resolution will add one day to beginDate/endDate,
                    # but for the "within" case it should start from the current day.

                    if RegExpUtility.exact_match(self.config.future_regex, after_str, trim=True). \
                            get_group(Constants.WITHIN_GROUP_NAME):
                        begin_date = begin_date - datedelta(days=1)
                        end_date = end_date - datedelta(days=1)

                if mod_and_date_result.mod:
                    duration_pr.value.mod = mod_and_date_result.mod

                duration_timex = duration_result.timex
                ret.sub_date_time_entities = [duration_pr]

                if mod_and_date_result.date_list is not None:
                    ret.list = mod_and_date_result.date_list

            if (begin_date != end_date) or rest_now_sunday:
                end_date = end_date - datedelta(days=1) if self._inclusive_end_period else end_date

                ret.timex = f'({DateTimeFormatUtil.luis_date(begin_date.year, begin_date.month, begin_date.day)},' \
                            f'{DateTimeFormatUtil.luis_date(end_date.year, end_date.month, end_date.day)},' \
                            f'{duration_timex})'
                ret.future_value = ret.past_value = [begin_date, end_date]
                ret.success = True
                return ret

        ret.success = False
        return ret

    # case like "三月的第一周"
    def parse_week_of_month(self, source: str, reference: datetime) -> DateTimeResolutionResult:
        ret = DateTimeResolutionResult()
        trimmed_source = source.lower()

        match = self.config.week_of_month_regex.match(source)

        if not match:
            return ret

        cardinal_str = match.get_group(Constants.CARDINAL)
        month_str = match.get_group(Constants.MONTH_GROUP_NAME)
        no_year = False

        if month_str is None:
            swift = 0
            if RegExpUtility.match_begin(self.config.wom_next_regex, trimmed_source, trim=True):
                swift = 1
            elif RegExpUtility.match_begin(self.config.wom_previous_regex, trimmed_source, trim=True):
                swift = -1

            month = reference + datedelta(months=swift)
            year = reference + datedelta(years=swift)
            ret.timex = DateTimeFormatUtil.luis_date(reference.year, month, 1)
        else:
            month = self.config.to_month_number(month_str)
            year = self.get_year_from_text(match)

            if year == Constants.INVALID_YEAR:
                year = reference.year
                no_year = True

        ret = self.get_week_of_month(cardinal_str, month, year, reference, no_year)

        return ret

    def parse_week_of_date(self, source: str, reference: datetime) -> DateTimeResolutionResult:
        ret = DateTimeResolutionResult()
        match = self.config.week_of_date_regex.match(source)
        date_ers = self.config.date_extractor.extract(source, reference)

        # Cases like 'week of september 16th' (9月16日の週)

        if match and len(date_ers) == 1:
            pr = self.config.date_parser.parse(date_ers[0], reference).value

            if (self.config.options and DateTimeOptions.CALENDAR) != 0:
                monday = DateUtils.this(pr.future_value, DayOfWeek.MONDAY)
                ret.timex = DateTimeFormatUtil.to_iso_week_timex(monday)
            else:
                ret.timex = pr.timex

            ret.comment = Constants.COMMENT_WEEK_OF
            ret.future_value = BaseDatePeriodParser.get_week_range_from_date(pr.future_value)
            ret.past_value = BaseDatePeriodParser.get_week_range_from_date(pr.past_value)
            ret.success = True

        return ret

    def parse_month_of_date(self, source: str, reference: datetime) -> DateTimeResolutionResult:
        ret = DateTimeResolutionResult()
        match = self.config.month_of_date_regex.match(source)
        ex = self.config.date_extractor.extract(source, reference)

        # Cases like 'month of september 16th' (9月16日の月)

        if match and len(ex) == 1:
            pr = self.config.date_parser.parse(ex[0], reference).value
            ret.timex = pr.timex
            ret.comment = Constants.COMMENT_MONTH_OF
            ret.future_value = BaseDatePeriodParser.get_month_range_from_date(pr.future_value)
            ret.past_value = BaseDatePeriodParser.get_month_range_from_date(pr.past_value)
            ret.success = True

        return ret

    def parse_which_week(self, source: str, reference: datetime) -> DateTimeResolutionResult:
        ret = DateTimeResolutionResult()
        match = RegExpUtility.exact_match(self.config.which_week_regex, source, trim=True)

        if match and match.success:
            num = int(match.get_group(Constants.NUMBER))
            if num == 0:
                return ret

            year = reference.year
            ret.timex = f'{year:04d}-W{num:02d}'

            first_day = DateUtils.safe_create_from_value(DateUtils.min_value, year, 1, 1)
            first_thursday = DateUtils.this(first_day, DayOfWeek.THURSDAY)
            first_week = DateUtils.week_of_year(first_thursday)

            if first_week == 1:
                num -= 1

            value = first_thursday + datedelta(days=(7 * num) - 3)

            ret.future_value = [value, value + datedelta(days=7)]
            ret.past_value = [value, value + datedelta(days=7)]
            ret.success = True

        return ret

    def get_week_of_month(self, cardinal_str: str, month: int, year: int, reference: datetime, no_year: bool) \
            -> DateTimeResolutionResult:
        ret = DateTimeResolutionResult()
        target_monday = self.get_monday_of_target_week(cardinal_str, month, year)

        future_date = past_date = target_monday

        if no_year and future_date < reference:
            future_date = self.get_monday_of_target_week(cardinal_str, month, year + 1)

        if no_year and past_date >= reference:
            past_date = self.get_monday_of_target_week(cardinal_str, month, year - 1)

        if no_year:
            year = Constants.INVALID_YEAR

        # Note that if the cardinalStr equals to "last", the weekNumber would be fixed at "5"
        # This may lead to some inconsistency between Timex and Resolution
        # the StartDate and EndDate of the resolution would always be correct (following ISO week definition)
        #  But week number for "last week" might be inconsistent with the resolution as we only have one Timex,
        #  but we may have past and future resolutions which may have different week numbers

        week_num = self.get_week_number_for_month(cardinal_str)
        ret.timex = TimexUtil.generate_week_of_month_timex(year, month, week_num)

        ret.future_value = [future_date, future_date + datedelta(days=Constants.WEEK_DAY_COUNT - 1)] if \
            self._inclusive_end_period else [future_date, future_date + datedelta(days=Constants.WEEK_DAY_COUNT)]
        ret.past_value = [past_date, past_date + datedelta(days=Constants.WEEK_DAY_COUNT - 1)] if \
            self._inclusive_end_period else [past_date, past_date + datedelta(days=Constants.WEEK_DAY_COUNT)]
        ret.success = True

        return ret

    def get_monday_of_target_week(self, cardinal_str: str, month: int, year: int) -> datetime:

        if RegExpUtility.exact_match(self.config.wom_last_regex, cardinal_str, trim=True).success:
            last_thursday = DateUtils.get_last_thursday(year, month)
            result = DateUtils.this(last_thursday, DayOfWeek.MONDAY)
        else:
            cardinal = self.get_week_number_for_month(cardinal_str)
            first_thursday = DateUtils.get_first_thursday(year, month)

            result = DateUtils.this(first_thursday, DayOfWeek.MONDAY) + \
                     datedelta(days=Constants.WEEK_DAY_COUNT * (cardinal - 1))

        return result

    def get_week_number_for_month(self, cardinal_str: str) -> int:

        # "last week of month" might not be "5th week of month"
        #  Sometimes it can also be "4th week of month" depends on specific year and month
        #  But as we only have one Timex, we use "5" to indicate it's the "last week"

        if RegExpUtility.exact_match(self.config.wom_last_regex, cardinal_str, trim=True).success:
            cardinal = 5
        else:
            cardinal = self.config.cardinal_map[cardinal_str]

        return cardinal

    # Cases like 'second week of 2021' (2021年的第二周)
    def parse_week_of_year(self, source: str, reference: datetime) -> DateTimeResolutionResult:
        ret = DateTimeResolutionResult()

        match = self.config.week_of_year_regex.match(source)

        if not match:
            return ret

        cardinal_str = match.get_group(Constants.CARDINAL)
        order_str = match.get_group(Constants.ORDER)

        year = self.get_year_from_text(match)

        if year == Constants.INVALID_YEAR:
            swift = self.config.get_swift_year(order_str)

            if swift < -1:
                return ret

            year = reference.year + datedelta(years=swift)

        if RegExpUtility.exact_match(self.config.wom_last_regex, cardinal_str, trim=True).success:
            target_week_monday = DateUtils.this(DateUtils.get_last_thursday(year), DayOfWeek.MONDAY)
            ret.timex = TimexUtil.generate_week_timex(target_week_monday)
        else:
            week_num = self.config.cardinal_map[cardinal_str]
            target_week_monday = DateUtils.this(DateUtils.get_last_thursday(year), DayOfWeek.MONDAY) + \
                                 datedelta(days=Constants.WEEK_DAY_COUNT * (week_num - 1))
            ret.timex = TimexUtil.generate_week_of_year_timex(year, week_num)

        ret.future_value = [target_week_monday, target_week_monday + datedelta(days=Constants.WEEK_DAY_COUNT - 1)] \
            if self._inclusive_end_period else \
            [target_week_monday, target_week_monday + datedelta(days=Constants.WEEK_DAY_COUNT)]

        ret.past_value = [target_week_monday, target_week_monday + datedelta(days=Constants.WEEK_DAY_COUNT - 1)] \
            if self._inclusive_end_period else \
            [target_week_monday, target_week_monday + datedelta(days=Constants.WEEK_DAY_COUNT)]

        ret.success = True

        return ret

    # parse "今年夏天"
    def parse_season(self, source: str, reference: datetime) -> DateTimeResolutionResult:
        ret = DateTimeResolutionResult()
        match = RegExpUtility.exact_match(self.config.season_with_year, source, trim=True)

        if match and match.success:

            # parse year
            year = reference.year
            has_year = False
            year_num = match.get_group(Constants.YEAR_GROUP_NAME)
            year_cjk = match.get_group(Constants.YEAR_CJK_GROUP_NAME)
            year_rel = match.get_group(Constants.YEAR_RELATIVE)

            if year_num:
                has_year = True

                if self.config.is_year_only(year_num):
                    year_num = year_num[:len(year_num) - 1]
                year = int(year_num)

            elif year_cjk:
                has_year = True

                if self.config.is_year_only(year_cjk):
                    year_cjk = year_cjk[:len(year_cjk) - 1]
                year = self.convert_cjk_to_integer(year_cjk)

            elif year_rel:
                has_year = True

                if self.config.is_last_year(year_rel):
                    year -= 1
                elif self.config.is_next_year(year_rel):
                    year += 1

            RegExpUtility.match_begin(self.config.this_regex, source, trim=True)

            # handle cases like "this summer" 今夏
            if not has_year and RegExpUtility.match_begin(self.config.this_regex, source, trim=True):
                has_year = True
                year = reference.year
            elif not has_year and RegExpUtility.match_begin(self.config.next_regex, source, trim=True):
                has_year = True
                year = reference.year + datedelta(years=1)
            elif not has_year and RegExpUtility.match_begin(self.config.last_regex, source, trim=True):
                has_year = True
                year = reference.year - datedelta(years=1)

            if 100 > year >= self.config.two_num_year:
                year += Constants.BASE_YEAR_PAST_CENTURY
            elif year < 100 and year < self.config.two_num_year:
                year += Constants.BASE_YEAR_CURRENT_CENTURY

            # parse season
            season_str = match.get_group(Constants.SEASON)

            if match.get_group(Constants.EARLY_PREFIX):
                ret.mod = TimeTypeConstants.EARLY_MOD
            if match.get_group(Constants.MID_PREFIX):
                ret.mod = TimeTypeConstants.MID_MOD
            if match.get_group(Constants.LATE_PREFIX):
                ret.mod = TimeTypeConstants.LATE_MOD

            ret.timex = self.config.season_map[season_str]

            if has_year:
                ret.timex = f'{year:04d}-{ret.timex}'

            ret.success = True
            return ret

        return ret

    def parse_quarter(self, source: str, reference: datetime) -> DateTimeResolutionResult:
        ret = DateTimeResolutionResult()
        match = RegExpUtility.exact_match(self.config.quarter_regex, source, trim=True)

        if not match or not match.success:
            return ret

        # parse year
        year = reference.year
        year_num = match.get_group(Constants.YEAR_GROUP_NAME)
        year_cjk = match.get_group(Constants.YEAR_CJK_GROUP_NAME)
        year_rel = match.get_group(Constants.YEAR_RELATIVE)

        if year_num:
            if self.config.is_year_only(year_num):
                year_num = year_num[:len(year_num) - 1]

            year = int(year_num)

        elif year_cjk:
            if self.config.is_year_only(year_cjk):
                year_cjk = year_cjk[:len(year_cjk) - 1]

            year = self.convert_cjk_to_integer(year_cjk)

        elif year_rel:
            if self.config.is_last_year(year_rel):
                year -= 1
            elif self.config.is_next_year(year_rel):
                year += 1

        if 100 > year >= self.config.two_num_year:
            year += Constants.BASE_YEAR_PAST_CENTURY
        elif year < 100 and year < self.config.two_num_year:
            year += Constants.BASE_YEAR_CURRENT_CENTURY

        # parse quarter_num
        cardinal_str = match.get_group(Constants.CARDINAL)
        quarter_num = self.config.cardinal_map[cardinal_str]

        if year_num or year_rel:
            begin_date = DateUtils.safe_create_from_value(DateUtils.min_value, year,
                                                          ((quarter_num - 1) * Constants.TRIMESTER_MONTH_COUNT) + 1, 1)
            end_date = DateUtils.safe_create_from_value(DateUtils.min_value, year, quarter_num *
                                                        Constants.TRIMESTER_MONTH_COUNT, 1) + datedelta(months=1)
            ret.future_value = ret.past_value = [begin_date, end_date]
            ret.timex = TimexUtil.generate_date_period_timex(begin_date, end_date, 2)
            ret.success = True
        else:
            begin_date = DateUtils.safe_create_from_value(DateUtils.min_value, year,
                                                          ((quarter_num - 1) * Constants.TRIMESTER_MONTH_COUNT) + 1, 1)
            end_date = DateUtils.safe_create_from_value(DateUtils.min_value, year, quarter_num *
                                                        Constants.TRIMESTER_MONTH_COUNT, 1) + datedelta(months=1)
            ret.past_value = [begin_date, end_date]
            ret.timex = TimexUtil.generate_date_period_timex(begin_date, end_date, 2)
            begin_date = DateUtils.safe_create_from_value(DateUtils.min_value, year + 1,
                                                          ((quarter_num - 1) * Constants.TRIMESTER_MONTH_COUNT) + 1, 1)
            end_date = DateUtils.safe_create_from_value(DateUtils.min_value, year + 1, quarter_num *
                                                        Constants.TRIMESTER_MONTH_COUNT, 1) + datedelta(months=1)
            ret.future_value = [begin_date, end_date]

        ret.success = True
        return ret

    def parse_decade(self, source: str, reference: datetime) -> DateTimeResolutionResult:
        ret = DateTimeResolutionResult()

        century = int((reference.year / 100) + 1)
        decade_last_year = 10
        input_century = False

        match = RegExpUtility.exact_match(self.config.decade_regex, source, trim=True)

        if match and match.success:
            decade_str = match.get_group(Constants.DECADE)
            decade = int(decade_str)

            if not DateUtils.int_try_parse(decade_str)[1]:
                decade = self.convert_cjk_to_num(decade_str)

            century_str = match.get_group(Constants.CENTURY)

            if century_str:
                century = int(century_str)

                if not DateUtils.int_try_parse(century_str)[1]:
                    century = self.convert_cjk_to_num(century_str)

                else:
                    century_str = match.get_group(Constants.REL_CENTURY)

                    if century_str:

                        century_str = century_str.lower()
                        this_match = self.config.this_regex.match(century_str)
                        next_match = self.config.next_regex.match(century_str)

                        if this_match:
                            pass
                        elif next_match:
                            century += 1
                        else:
                            century -= 1

                        input_century = True
        else:
            return ret

        begin_year = ((century - 1) * 100) + decade
        first_two_num_of_year = match.get_group(Constants.FIRST_TWO_YEAR_NUM)

        # handle cases like "2000年代"
        if first_two_num_of_year:
            begin_year = (self.convert_cjk_to_integer(first_two_num_of_year) * 100) + decade

        ret.timex = TimexUtil.generate_decade_timex(begin_year, decade_last_year, decade, input_century)

        future_year = past_year = begin_year

        start_date = DateUtils.safe_create_from_value(DateUtils.min_value, begin_year, 1, 1)

        if not input_century and start_date < reference and not first_two_num_of_year:
            future_year += 100

        if not input_century and start_date >= reference and not first_two_num_of_year:
            past_year -= 100

        ret.future_value = [DateUtils.safe_create_from_value(DateUtils.min_value, future_year, 1, 1),
                            DateUtils.safe_create_from_value(DateUtils.min_value, future_year + decade_last_year, 1, 1)]
        ret.past_value = [DateUtils.safe_create_from_value(DateUtils.min_value, past_year, 1, 1),
                          DateUtils.safe_create_from_value(DateUtils.min_value, past_year + decade_last_year, 1, 1)]
        ret.success = True

        return ret

    def parse_century(self, source: str, reference: datetime) -> DateTimeResolutionResult:
        ret = DateTimeResolutionResult()
        century = (reference.year / 100) + 1

        match = RegExpUtility.exact_match(self.config.century_regex, source, trim=True)

        if match and match.success:
            century_str = match.get_group(Constants.CENTURY)

            if century_str:
                century = int(century_str)

                if not DateUtils.int_try_parse(century_str)[1]:
                    century = self.convert_cjk_to_num(century_str)

            begin_year = (century - 1) * Constants.CENTURY_YEARS_COUNT
            end_year = begin_year + Constants.CENTURY_YEARS_COUNT

            start_date = DateUtils.safe_create_from_value(begin_year, 1, 1, 1)
            end_date = DateUtils.safe_create_from_value(end_year, 1, 1, 1)

            ret.timex = TimexUtil.generate_date_period_timex(start_date, end_date, 3)
            ret.future_value = ret.past_value = [start_date, end_date]
            ret.success = True

        return ret

    # Only handle cases like "within/less than/more than x weeks from/before/after today"
    def parse_date_point_with_ago_and_later(self, source: str, reference: datetime) -> DateTimeResolutionResult:

        ret = DateTimeResolutionResult()
        er = self.config.date_extractor.extract(source, reference)[0]
        trimmed_source = source.lower()
        match = RegExpUtility.exact_match(self.config.date_point_with_ago_and_later, trimmed_source, trim=True)

        if er and match:
            is_ago = match.get_group(Constants.AGO_LABEL).success
            is_within = match.get_group(Constants.WITHIN_GROUP_NAME).success
            is_more_than = match.get_group(Constants.MORE_GROUP_NAME).success

            if match.get_group(Constants.YESTERDAY_GROUP_NAME):
                reference = reference - datedelta(days=1)

            er.text = source
            pr = self.config.date_parser.parse(er, reference)
            duration_extraction_result = self.config.duration_extractor.extract(er.text, reference)[0]

            if duration_extraction_result:

                duration = self.config.duration_parser.parse(duration_extraction_result, reference)
                duration_in_seconds = duration.value.past_value

                if is_within:

                    if is_ago:
                        start_date = pr.value.past_value
                        end_date = start_date + timedelta(seconds=duration_in_seconds)
                    else:
                        end_date = pr.value.future_value
                        start_date = end_date - timedelta(seconds=duration_in_seconds)

                    if start_date != DateUtils.min_value():
                        duration_timex = duration.value.timex

                        ret.timex = TimexUtil.generate_date_period_timex_with_duration(start_date, end_date,
                                                                                       duration_timex)
                        ret.future_value = ret.past_value = [start_date, end_date]
                        ret.success = True

                        return ret

                elif is_more_than:
                    ret.mod = Constants.BEFORE_MOD if is_ago else Constants.AFTER_MOD
                    ret.timex = pr.timex_str
                    ret.future_value = pr.value.future_value
                    ret.past_value = pr.value.past_value
                    ret.success = True

                    return ret

        ret.success = False
        return ret

    def parse_complex_date_period(self, source: str, reference: datetime) -> DateTimeResolutionResult:
        ret = DateTimeResolutionResult()
        match = self.config.complex_date_period_regex.match(source)

        if match and match.success:
            future_begin = DateUtils.min_value()
            future_end = DateUtils.min_value()
            past_begin = DateUtils.min_value()
            past_end = DateUtils.min_value()

            is_specific_date = False
            is_start_by_week = False
            is_end_by_week = False
            is_ambiguous_start = False
            is_ambiguous_end = False

            ambiguous_res = DateTimeResolutionResult()
            date_context = self.get_year_context(match.get_group(TimeTypeConstants.START).value,
                                                 match.get_group(TimeTypeConstants.END).value, source)

            start_resolution = self.parse_single_time_point(match.get_group(TimeTypeConstants.START).value,
                                                            reference, date_context)

            if start_resolution.success:
                future_begin = start_resolution.future_value
                past_begin = start_resolution.past_value
                is_specific_date = True

            if not start_resolution.success:
                start_resolution = self.parse_base_date_period(match.get_group(TimeTypeConstants.START).value,
                                                               reference, date_context)

                if start_resolution.success:
                    future_begin = self.shift_resolution(start_resolution.future_value, match, start=True)
                    past_begin = self.shift_resolution(start_resolution.past_value, match, start=True)

                    if "-W" in start_resolution.timex:
                        is_start_by_week = True

            if start_resolution.success:
                end_resolution = self.parse_single_time_point(match.get_group(TimeTypeConstants.END).value,
                                                              reference, date_context)

                if end_resolution.success:
                    future_end = end_resolution.future_value
                    past_end = end_resolution.past_value
                    is_specific_date = True

                if not end_resolution.success or is_ambiguous_end:
                    end_resolution = self.parse_base_date_period(match.get_group(TimeTypeConstants.END).value,
                                                                 reference, date_context)

                    if end_resolution.success:
                        # When the end group contains modifiers such as 'end of', 'middle of', the end resolution
                        # must be updated accordingly.

                        future_end = self.shift_resolution(end_resolution.future_value, match, start=False)
                        past_end = self.shift_resolution(end_resolution.past_value, match, start=False)

                        if "-W" in end_resolution.timex:
                            is_end_by_week = True

                if end_resolution.success:
                    # When start or end is ambiguous it is better to resolve it to the type of the unambiguous
                    # extraction. In Spanish, for example, 'de lunes a mar' (from Monday to Tuesday) or
                    # 'de enero a mar' (from January to March). In the first case 'mar' is resolved as Date (weekday),
                    # in the second case it is resolved as DatePeriod (month). if (isAmbiguousStart && isSpecificDate)

                    if is_ambiguous_start and is_specific_date:
                        start_resolution = ambiguous_res
                        future_begin = start_resolution.future_value
                        past_begin = start_resolution.past_value

                    elif is_ambiguous_end and is_specific_date:
                        end_resolution = ambiguous_res
                        future_end = end_resolution.future_value
                        past_end = end_resolution.past_value

                    if future_begin > future_end:
                        if not date_context:
                            future_begin = past_begin
                        else:
                            future_begin = DateContext.swift_date_object(future_begin, future_end)

                    if past_end < past_begin:
                        if not date_context:
                            past_end = future_end
                        else:
                            past_begin = DateContext.swift_date_object(past_begin, past_end)

                    # If both begin/end are date ranges in "Month", the Timex should be ByMonth
                    # The year period case should already be handled in Basic Cases
                    date_period_timex_type = 2

                    if is_specific_date:
                        # If at least one of the begin/end is specific date, the Timex should be ByDay
                        date_period_timex_type = 0

                    elif is_start_by_week and is_end_by_week:
                        # If both begin/end are date ranges in "Week", the Timex should be ByWeek
                        date_period_timex_type = 1

                    has_year = not start_resolution.timex.startswith(Constants.TIMEX_FUZZY_YEAR) or \
                               not end_resolution.timex.startswith(Constants.TIMEX_FUZZY_YEAR)

                    # If the year is not specified, the combined range timex will use fuzzy years.
                    ret.timex = TimexUtil.generate_date_period_timex(future_begin, future_end, date_period_timex_type,
                                                                     past_begin, past_end, has_year)
                    ret.future_value = [future_begin, future_end]
                    ret.past_value = [past_begin, past_end]
                    ret.success = True

        return ret
