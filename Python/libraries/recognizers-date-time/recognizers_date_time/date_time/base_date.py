#  Copyright (c) Microsoft Corporation. All rights reserved.
#  Licensed under the MIT License.

from abc import ABC, abstractmethod
from typing import List, Optional, Pattern, Dict, Match
from datetime import datetime, timedelta

from recognizers_date_time.date_time.abstract_year_extractor import AbstractYearExtractor
from datedelta import datedelta
from recognizers_text.extractor import ExtractResult
from recognizers_text.utilities import RegExpUtility, flatten
from recognizers_number.number import Constants as NumberConstants
from .constants import Constants, TimeTypeConstants
from .extractors import DateTimeExtractor
from .parsers import DateTimeParser, DateTimeParseResult
from .utilities import Token
import regex
import calendar


class DateTimeUtilityConfiguration(ABC):
    @property
    @abstractmethod
    def date_unit_regex(self) -> Pattern:
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
    def check_both_before_after(self) -> Pattern:
        raise NotImplementedError

    @property
    @abstractmethod
    def range_prefix_regex(self) -> Pattern:
        raise NotImplementedError


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
    def week_day_end(self) -> Pattern:
        raise NotImplementedError

    @property
    @abstractmethod
    def week_day_start(self) -> Pattern:
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
    def week_day_and_day_regex(self) -> Pattern:
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
    def prefix_article_regex(self) -> Pattern:
        raise NotImplementedError

    @property
    @abstractmethod
    def day_of_week(self) -> Dict[str, int]:
        raise NotImplementedError

    @property
    @abstractmethod
    def month_of_year(self) -> Dict[str, int]:
        raise NotImplementedError

    @property
    @abstractmethod
    def ordinal_extractor(self):
        raise NotImplementedError

    @property
    @abstractmethod
    def integer_extractor(self):
        raise NotImplementedError

    @property
    @abstractmethod
    def number_parser(self):
        raise NotImplementedError

    @property
    @abstractmethod
    def duration_extractor(self) -> DateTimeExtractor:
        raise NotImplementedError

    @property
    @abstractmethod
    def utility_configuration(self):
        raise NotImplementedError

    @property
    @abstractmethod
    def strict_relative_regex(self) -> Pattern:
        raise NotImplementedError

    @property
    @abstractmethod
    def range_connector_symbol_regex(self) -> Pattern:
        raise NotImplementedError

    @property
    @abstractmethod
    def year_suffix(self) -> Pattern:
        raise NotImplementedError

    @property
    @abstractmethod
    def more_than_regex(self) -> Pattern:
        raise NotImplementedError

    @property
    @abstractmethod
    def less_than_regex(self) -> Pattern:
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
    def since_year_suffix_regex(self) -> Pattern:
        raise NotImplementedError

    @property
    @abstractmethod
    def check_both_before_after(self) -> Pattern:
        raise NotImplementedError


class BaseDateExtractor(DateTimeExtractor, AbstractYearExtractor):
    @property
    def extractor_type_name(self) -> str:
        return Constants.SYS_DATETIME_DATE

    def __init__(self, config: DateExtractorConfiguration):
        super().__init__(config)

    def extract(self, source: str, reference: datetime = None) -> List[ExtractResult]:
        from .utilities import merge_all_tokens
        if reference is None:
            reference = datetime.now()

        tokens = []
        tokens.extend(self.basic_regex_match(source))
        tokens.extend(self.implicit_date(source))
        tokens.extend(self.number_with_month(source, reference))
        tokens.extend(self.relative_duration_date(source, reference))

        result = merge_all_tokens(tokens, source, self.extractor_type_name)
        return result

    def basic_regex_match(self, source: str) -> []:
        from .utilities import Token
        from .utilities import RegExpUtility
        ret: List[Token] = list()

        for regexp in self.config.date_regex_list:

            matches = list(regexp.finditer(source))
            if matches:
                for match in matches:

                    # some match might be part of the date range entity, and might be split in a wrong way
                    if self.validate_match(match, source):

                        # Cases that the relative term is before
                        # the detected date entity, like "this 5/12", "next friday 5/12"
                        pre_text = source[0:source.index(match.group())]
                        relative_regex = RegExpUtility.match_end(self.config.strict_relative_regex, pre_text, True)

                        if relative_regex:
                            if relative_regex.success:
                                ret.append(Token(relative_regex.index, source.index(match.group())
                                                 + match.end() - match.start()))
                            else:
                                ret.append(Token(source.index(match.group()),
                                                 source.index(match.group()) + match.end() - match.start()))
                        else:
                            ret.append(Token(source.index(match.group()),
                                             source.index(match.group()) + match.end() - match.start()))

        return ret

    # this method is to validate whether the match is part of date range and is a correct split
    # For example: in case "10-1 - 11-7", "10-1 - 11" can be matched by some of the Regexes,
    # but the full text is a date range, so "10-1 - 11" is not a correct split
    def validate_match(self, match: Match, text: str):

        # If the match doesn't contains "year" part, it will not be ambiguous and it's a valid match
        is_valid_match = not RegExpUtility.get_group(
            match, Constants.YEAR_GROUP_NAME)

        if not is_valid_match:
            year_group = RegExpUtility.get_group(
                match, Constants.YEAR_GROUP_NAME)
            # If the "year" part is not at the end of the match, it's a valid match
            if not text.index(year_group) + len(year_group) == text.index(match.group())\
                    + (match.end() - match.start()):
                is_valid_match = True
            else:
                sub_text = text[text.index(year_group):]

                # If the following text (include the "year" part) doesn't start with a Date entity, it's a valid match
                if not self.starts_with_basic_date(sub_text):
                    is_valid_match = True
                else:

                    # If the following text (include the "year" part) starts with a Date entity,
                    # but the following text (doesn't include the "year" part) also starts with a
                    # valid Date entity, the current match is still valid
                    # For example, "10-1-2018-10-2-2018". Match "10-1-2018" is valid because
                    # though "2018-10-2" a valid match (indicates the first year "2018" might
                    # belongs to the second Date entity), but "10-2-2018" is also

                    sub_text = text[text.index(year_group) + len(year_group):].strip()
                    sub_text = self.trim_start_range_connector_symbols(sub_text)
                    is_valid_match = self.starts_with_basic_date(sub_text)

            # Expressions with mixed separators are not considered valid dates e.g. "30/4.85" (unless one is a comma "30/4, 2016")
            day_group = RegExpUtility.get_group(match, Constants.DAY_GROUP_NAME)
            month_group = RegExpUtility.get_group(match, Constants.MONTH_GROUP_NAME)
            if day_group and month_group:
                no_date_text = match.group().replace(year_group, "").replace(month_group, "").replace(day_group, "")
                separators = ["/", "\\", "-", "."]
                separator_count = 0
                for separator in separators:
                    if separator in no_date_text:
                        separator_count += 1
                    if separator_count > 1:
                        is_valid_match = False
                        break

        return is_valid_match

    # TODO: Simplify this method to improve the performance
    def trim_start_range_connector_symbols(self, text: str):

        range_connector_symbol_matches = [self.config.range_connector_symbol_regex.match(text)]

        for symbol_match in range_connector_symbol_matches:
            start_symbol_length = -1

            if symbol_match and text.index(symbol_match.group()) == 0 and len(symbol_match.group()) >\
                    start_symbol_length:
                start_symbol_length = len(symbol_match.group())

            if start_symbol_length > 0:
                text = text[start_symbol_length:]

        return text.strip()

    # TODO: Simplify this method to improve the performance
    def starts_with_basic_date(self, text: str):
        from .utilities import RegExpUtility
        for regexp in self.config.date_regex_list:
            match = RegExpUtility.match_begin(regexp, text, True)

            if match:
                return True

        return False

    def implicit_date(self, source: str) -> []:
        from .utilities import get_tokens_from_regex
        ret: List[Token] = list()

        for regexp in self.config.implicit_date_list:
            ret.extend(get_tokens_from_regex(regexp, source))

        return ret

    def number_with_month(self, source: str, reference: datetime) -> []:
        from .utilities import DateUtils
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
                    start_index = match.start()
                    result_length = result.length if result.length else 0
                    end_index = match.start() + len(match.group()) + result_length

                    start_index, end_index = self.extend_with_week_day_and_year(
                        start_index, end_index, self.config.month_of_year[str(RegExpUtility.get_group(
                            match, Constants.MONTH_GROUP_NAME)).lower()], num, source, reference)

                    ret.append(
                        Token(match.start(), end_index))
                    continue

                # handling cases like 'for the 25th'
                matches = regex.finditer(self.config.for_the_regex, source)
                is_found = False

                for match_case in matches:
                    if match_case is not None:
                        ordinal_num = RegExpUtility.get_group(
                            match_case, Constants.DAY_OF_MONTH)

                        if ordinal_num == result.text:
                            length = len(
                                RegExpUtility.get_group(match_case, TimeTypeConstants.END))
                            ret.append(Token(match_case.start(),
                                             match_case.end() - length))
                            is_found = True

                if is_found:
                    continue

                # handling cases like 'Thursday the 21st', which both 'Thursday' and '21st' refer to a same date
                matches = regex.finditer(
                    self.config.week_day_and_day_of_month_regex, source)

                for match_case in matches:
                    if match_case is not None:
                        ordinal_num = RegExpUtility.get_group(
                            match_case, Constants.DAY_OF_MONTH)

                        if ordinal_num == result.text:
                            month = reference.month
                            year = reference.year

                            # get week of day for the ordinal number which is regarded as a date of reference month
                            date = DateUtils.safe_create_from_min_value(
                                year, month, num)
                            num_week_day_str: str = calendar.day_name[date.weekday()].lower(
                            )

                            # get week day from text directly, compare it with the weekday generated above
                            # to see whether they refer to a same week day
                            extracted_week_day_str = RegExpUtility.get_group(
                                match_case, 'weekday').lower()
                            if (date != DateUtils.min_value and
                                    self.config.day_of_week[num_week_day_str] ==
                                    self.config.day_of_week[extracted_week_day_str]):
                                ret.append(
                                    Token(match_case.start(), match_case.end()))
                                is_found = True

                if is_found:
                    continue

                # Handling cases like 'Monday 21', which both 'Monday' and '21' refer to the same date
                # The year of expected date can be different to the year of referenceDate.

                matches = regex.finditer(self.config.week_day_and_day_regex, source)
                for match_case in matches:

                    if match_case:
                        match_length = result.start + result.length - match_case.start()

                        if match_length == match_case.start():
                            ret.append(Token(match_case.start(), match_case.end()))
                            is_found = True

                if is_found:
                    continue

                # handling cases like '20th of next month'
                suffix_str: str = source[result.start + result.length:].lower()
                match = regex.match(
                    self.config.relative_month_regex, suffix_str.strip())
                space_len = len(suffix_str) - len(suffix_str.strip())

                if match is not None and match.start() == 0:

                    space_len = len(suffix_str) - len(suffix_str.strip())
                    res_start = result.start
                    res_end = res_start + result.length + space_len + len(match.group())

                    # Check if prefix contains 'the', include it if any
                    prefix = source[: res_start or 0]
                    prefix_match = self.config.prefix_article_regex.match(prefix)
                    if prefix_match:
                        res_start = prefix_match.start()

                    ret.append(
                        Token(res_start, res_end))

                # handling cases like 'second Sunday'

                suffix_str = source[result.start + result.length:]
                match = regex.match(
                    self.config.week_day_regex, suffix_str.strip())
                if (match is not None and match.start() == 0 and 1 <= num <= 5 and
                        result.type == NumberConstants.SYS_NUM_ORDINAL):
                    week_day_str = RegExpUtility.get_group(match, Constants.WEEKDAY_GROUP_NAME).lower()

                    if week_day_str in self.config.day_of_week:
                        ret.append(
                            Token(result.start, result.start + result.length + space_len + len(match.group())))
            # For cases like "I'll go back twenty second of June"
            if result.start + result.length < len(source):
                after_string = source[result.start + result.length:]
                match = regex.match(self.config.of_month, after_string)

                if match is not None:
                    start_index = result.start if result.start else 0
                    result_length = result.length if result.length else 0
                    end_index = (start_index + result_length) + len(match.group())

                    self.extend_with_week_day_and_year(start_index, end_index,
                                                       self.config.month_of_year[RegExpUtility.get_group(
                                                           match, Constants.MONTH_GROUP_NAME).lower() or str(
                                                           reference.month)], num, source, reference)

                    ret.append(Token(start_index, start_index +
                                     result.length + len(match.group())))

        return ret

    def get_year_index(self, affix, year, in_prefix):
        index = 0
        match_year = self.config.year_suffix.match(affix)
        success = match_year and match_year.start() if not in_prefix else match_year and match_year.start() \
            + match_year.end() == len(affix.strip())

        if success:
            year = self.get_year_from_text(match_year)

            if Constants.MIN_YEAR_NUM <= year <= Constants.MAX_YEAR_NUM:
                index = match_year.length if not in_prefix else match_year.end() + (len(affix) - len(affix.strip()))

        return index, success

    def extend_with_week_day_and_year(self, start_index: int, end_index: int, month: int,
                                      day: int, text: str, reference: datetime):
        from .utilities import DateUtils
        import calendar

        year = reference.year

        # Check whether there's a year
        suffix = text[end_index:]
        prefix = text[0: start_index]
        year_index, success = self.get_year_index(suffix, year, False)
        end_index += year_index

        # Check also in prefix
        if not success and self.config.check_both_before_after:
            year_index, success = self.get_year_index(suffix, year, False)
            start_index -= year_index

        # Check also in prefix
        date = DateUtils.safe_create_from_value(DateUtils.min_value, year, month, day)
        is_match_in_suffix = False
        match_week_day = self.config.week_day_end.match(prefix)

        if not match_week_day:
            match_week_day = self.config.week_day_start.match(suffix)
            is_match_in_suffix = True if match_week_day else False

        if match_week_day:
            # Get weekday from context directly, compare it with the weekday extraction above
            # to see whether they reference the same weekday
            extracted_week_day_str = RegExpUtility.get_group(
                match_week_day, Constants.WEEKDAY_GROUP_NAME)
            num_week_day_str = calendar.day_name[date.weekday()].lower()
            week_day_1 = self.config.day_of_week.get(num_week_day_str)
            week_day_2 = self.config.day_of_week.get(extracted_week_day_str)

            if self.config.day_of_week.get(num_week_day_str, week_day_1) and \
                    self.config.day_of_week.get(extracted_week_day_str, week_day_2):

                if not date == DateUtils.min_value and week_day_1 == week_day_2:
                    if not is_match_in_suffix:
                        start_index = match_week_day.start()
                    else:
                        end_index += match_week_day.end()

        return start_index, end_index

    # "In 3 days/weeks/months/years" = "3 days/weeks/months/years from now"
    def extract_relative_duration_date_with_in_prefix(self, source: str, duration_er: [ExtractResult],
                                                      reference: datetime):
        from .utilities import Token
        result: [Token] = []

        durations: [Token] = []

        for duration_extraction in duration_er:

            match = self.config.date_unit_regex.search(duration_extraction.text)
            if match:
                durations.append(Token(duration_extraction.start or 0, (duration_extraction.start or 0)
                                       + duration_extraction.length or 0))

        for duration in durations:
            before_str = source[0:duration.start]
            after_str = source[duration.start + duration.length:]

            if (str.isspace(before_str) or before_str is None) and (str.isspace(after_str) or after_str is None):
                continue

            ers, success = self.extract_in_connector(source, after_str, before_str, duration, True)
            result.append(ers)
            if not success and self.config.check_both_before_after:
                ers, success = self.extract_in_connector(source, after_str, before_str, duration, True)
                result.append(ers)

        return flatten(result)

    def relative_duration_date(self, source: str, reference: datetime) -> []:
        from .utilities import AgoLaterUtil
        tokens = []
        duration_extracted_results = self.config.duration_extractor.extract(source, reference)

        for extracted_result in duration_extracted_results:

            # if it is a multiple duration but its type is not equal to Date, skip it here
            if self.is_multiple_duration(extracted_result) and not self.is_multiple_duration_date(extracted_result):
                break

            # Some types of duration can be compounded with "before", "after" or "from" suffix to create a "date"
            # While some other types of durations, when compounded with such suffix, it will not create a "date",
            # but create a "dateperiod"
            # For example, durations like "3 days", "2 weeks", "1 week and 2 days",
            # can be compounded with such suffix to create a "date"
            # But "more than 3 days", "less than 2 weeks", when compounded with such
            # suffix, it will become cases like "more than 3 days from today" which is a "dateperiod", not a "date"
            # As this parent method is aimed to extract RelativeDurationDate, so for
            # cases with "more than" or "less than", we remove the prefix so as
            # to extract the expected RelativeDurationDate
            if self.is_inequality_duration(extracted_result):
                self.strip_inequality_duration(extracted_result)

            match = self.config.date_unit_regex.search(extracted_result.text)

            if match:
                tokens.extend(
                    AgoLaterUtil.extractor_duration_with_before_and_after(source, extracted_result, tokens,
                                                                          self.config.utility_configuration))

        # Extract cases like "in 3 weeks", which equals to "3 weeks from today"
        relative_duration_date_with_in_prefix =\
            self.extract_relative_duration_date_with_in_prefix(source, duration_extracted_results, reference)

        for extract_result_with_in_prefix in relative_duration_date_with_in_prefix:
            if not self.is_overlap_with_exist_extractions(extract_result_with_in_prefix, tokens):
                tokens.append(extract_result_with_in_prefix)

        return tokens

    @staticmethod
    def is_overlap_with_exist_extractions(extract_result, exist_extract_results):

        for exist_extract_result in exist_extract_results:
            if extract_result.start < exist_extract_result.end and extract_result.end > exist_extract_result.start:
                return True

        return False

    def strip_inequality_duration(self, extract_result: ExtractResult):
        if self.config.check_both_before_after:
            self.strip_inequality(extract_result, self.config.more_than_regex, False)
            self.strip_inequality(extract_result, self.config.less_than_regex, False)
        else:
            self.strip_inequality(extract_result, self.config.more_than_regex, True)
            self.strip_inequality(extract_result, self.config.less_than_regex, True)

    @staticmethod
    def strip_inequality(extract_result: ExtractResult, regexp: Pattern, in_prefix: bool):
        if regex.search(regexp, extract_result.text):
            original_length = len(extract_result.text)
            extract_result.text = str(regexp).replace(extract_result.text, '').strip()
            if in_prefix:
                extract_result.start += original_length - len(extract_result.text)

            extract_result.length = len(extract_result.text)
            extract_result.data = ''

    @staticmethod
    def is_multiple_duration_date(er: ExtractResult):
        return er.data is not None and er.data == Constants.MULTIPLE_DURATION_DATE

    @staticmethod
    def is_multiple_duration(er: ExtractResult):
        return er.data is not None and str(er.data).startswith(Constants.MULTIPLE_DURATION_PREFIX)

    # Cases like "more than 3 days", "less than 4 weeks"
    @staticmethod
    def is_inequality_duration(er: ExtractResult):
        return er.data is not None and er.data == TimeTypeConstants.MORE_THAN_MOD \
            or er.data == TimeTypeConstants.LESS_THAN_MOD

    def extract_in_connector(self, text, first_str, second_str, duration, in_prefix):
        from recognizers_date_time import Token
        result = []
        match = RegExpUtility.match_end(self.config.in_connector_regex, first_str, True) if in_prefix else RegExpUtility.match_begin(self.config.in_connector_regex, first_str, True)
        success = False if not match else match.success
        if match and match.success:

            start_token = match.index
            range_unit_math = self.config.range_unit_regex.match(text[duration.start: duration.start
                                                                      + duration.length])

            if range_unit_math:
                since_year_match = self.config.since_year_suffix_regex.match(second_str)

                if since_year_match:
                    result.append(Token(start_token, duration.end + len(since_year_match)))

                else:
                    result.append(Token(start_token, duration.end))
        return result, success


class DateParserConfiguration(ABC):

    @property
    @abstractmethod
    def ordinal_extractor(self):
        raise NotImplementedError

    @property
    @abstractmethod
    def integer_extractor(self):
        raise NotImplementedError

    @property
    @abstractmethod
    def cardinal_extractor(self):
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
    def duration_parser(self) -> DateTimeParser:
        raise NotImplementedError

    @property
    @abstractmethod
    def number_parser(self):
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
    def utility_configuration(self):
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

    @property
    @abstractmethod
    def check_both_before_after(self) -> bool:
        raise NotImplementedError


class BaseDateParser(DateTimeParser):

    @property
    def parser_type_name(self) -> str:
        return Constants.SYS_DATETIME_DATE

    def __init__(self, config: DateParserConfiguration):
        self.config = config

    def parse(self, source: ExtractResult, reference: datetime = None) -> Optional[DateTimeParseResult]:
        from .utilities import DateTimeFormatUtil
        if reference is None:
            reference = datetime.now()

        result_value: DateTimeParseResult = None

        if source.type is self.parser_type_name:
            source_text = source.text.lower()
            inner_result = self.parse_basic_regex_match(source_text, reference)

            if not inner_result.success:
                inner_result = self.parse_implicit_date(source_text, reference)

            if not inner_result.success:
                inner_result = self.parse_weekday_of_month(
                    source_text, reference)

            if not inner_result.success:
                inner_result = self.parser_duration_with_ago_and_later(
                    source_text, reference)

            if not inner_result.success:
                inner_result = self.parse_number_with_month(
                    source_text, reference)

            if not inner_result.success:
                inner_result = self.parse_single_number(source_text, reference)

            if inner_result.success:
                inner_result.future_resolution: Dict[str, str] = dict()
                inner_result.future_resolution[TimeTypeConstants.DATE] = DateTimeFormatUtil.format_date(
                    inner_result.future_value)
                inner_result.past_resolution: Dict[str, str] = dict()
                inner_result.past_resolution[TimeTypeConstants.DATE] = DateTimeFormatUtil.format_date(
                    inner_result.past_value)
                result_value = inner_result

        result = DateTimeParseResult(source)
        result.value = result_value
        result.timex_str = result_value.timex if result_value is not None else ''
        result.resolution_str = ''

        return result

    def parse_basic_regex_match(self, source: str, reference: datetime) -> DateTimeParseResult:
        from .utilities import DateTimeResolutionResult
        trimmed_source = source.strip()
        result = DateTimeResolutionResult()

        for regexp in self.config.date_regex:
            offset = 0
            match = regex.search(regexp, trimmed_source)

            if match is None:
                match = regex.search(
                    regexp, self.config.date_token_prefix + trimmed_source)
                offset = len(self.config.date_token_prefix)

            if match and match.start() == offset and len(match.group()) == len(trimmed_source):
                result = self.match_to_date(match, reference)
                break

        return result

    def match_to_date(self, match, reference: datetime):
        from .utilities import DateTimeResolutionResult
        from .utilities import DateUtils
        from .utilities import DateTimeFormatUtil

        result = DateTimeResolutionResult()
        month = 0
        day = 0
        year = 0

        year_str = RegExpUtility.get_group(match, Constants.YEAR_GROUP_NAME)
        written_year_str = RegExpUtility.get_group(match, Constants.FULL_YEAR_GROUP_NAME)
        month_str = RegExpUtility.get_group(match, Constants.MONTH_GROUP_NAME)
        day_str = RegExpUtility.get_group(match, Constants.DAY_GROUP_NAME)

        if month_str in self.config.month_of_year and day_str in self.config.day_of_month:

            month = self.config.month_of_year.get(month_str)
            day = self.config.day_of_month.get(day_str)

            if written_year_str:
                year = self.config.date_extractor.get_year_from_text(match)
            elif year_str:
                year = int(year_str) if year_str.isnumeric() else 0

                if 100 > year >= Constants.MIN_TWO_DIGIT_YEAR_PAST_NUM:
                    year += 1900
                elif 0 <= year < Constants.MAX_TWO_DIGIT_YEAR_FUTURE_NUM:
                    year += 2000

        no_year = False

        if year == 0:
            year = reference.year
            result.timex = DateTimeFormatUtil.luis_date(-1, month, day)
            no_year = True
        else:
            result.timex = DateTimeFormatUtil.luis_date(year, month, day)

        future_date, past_date = DateUtils.generate_dates(no_year, reference, year, month, day)
        #future_date = DateUtils.safe_create_from_min_value(no_year, reference, year, month, day)
        #past_date = DateUtils.safe_create_from_min_value(no_year, reference, year, month, day)

        result.future_value = future_date
        result.past_value = past_date
        result.success = True
        return result

    def parse_implicit_date(self, source: str, reference: datetime) -> DateTimeParseResult:
        from .utilities import DateUtils
        from .utilities import DateTimeFormatUtil
        from .utilities import DateTimeResolutionResult
        from .utilities import DayOfWeek
        trimmed_source = source.strip()
        result = DateTimeResolutionResult()

        # handle "on 12"
        match = regex.search(self.config.on_regex,
                             self.config.date_token_prefix + trimmed_source)
        if match and match.start() == len(self.config.date_token_prefix) and len(match.group()) == len(trimmed_source):
            day = 0
            month = reference.month
            year = reference.year
            day_str = match.group(Constants.DAY_GROUP_NAME)
            day = self.config.day_of_month.get(day_str)

            result.timex = DateTimeFormatUtil.luis_date(-1, -1, day)

            try_str = DateTimeFormatUtil.luis_date(year, month, day)
            try_date = datetime.strptime(try_str, '%Y-%m-%d')
            future_date: datetime
            past_date: datetime

            if try_date:
                future_date = DateUtils.safe_create_from_min_value(
                    year, month, day)
                past_date = DateUtils.safe_create_from_min_value(
                    year, month, day)

                if future_date < reference:
                    future_date += datedelta(months=1)

                if past_date >= reference:
                    past_date += datedelta(months=-1)
            else:
                future_date = DateUtils.safe_create_from_min_value(
                    year, month + 1, day)
                past_date = DateUtils.safe_create_from_min_value(
                    year, month - 1, day)

            result.future_value = future_date
            result.past_value = past_date
            result.success = True
            return result

        # handle "today", "the day before yesterday"
        match = regex.match(self.config.special_day_regex, trimmed_source)
        if match and match.start() == 0 and len(match.group()) == len(trimmed_source):
            swift = self.config.get_swift_day(match.group())
            today = DateUtils.safe_create_from_min_value(
                reference.year, reference.month, reference.day)
            value = today + timedelta(days=swift)
            result.timex = DateTimeFormatUtil.luis_date_from_datetime(value)
            result.future_value = value
            result.past_value = value
            result.success = True
            return result

        # handle "next Sunday"
        match = regex.match(self.config.next_regex, trimmed_source)
        if match and match.start() == 0 and len(match.group()) == len(trimmed_source):
            weekday_str = match.group(Constants.WEEKDAY_GROUP_NAME)
            value = DateUtils.next(
                reference, self.config.day_of_week.get(weekday_str))

            result.timex = DateTimeFormatUtil.luis_date_from_datetime(value)
            result.future_value = value
            result.past_value = value
            result.success = True
            return result

        # handle "this Friday"
        match = regex.match(self.config.this_regex, trimmed_source)
        if match and match.start() == 0 and len(match.group()) == len(trimmed_source):
            weekday_str = match.group(Constants.WEEKDAY_GROUP_NAME)
            value = DateUtils.this(
                reference, self.config.day_of_week.get(weekday_str))

            result.timex = DateTimeFormatUtil.luis_date_from_datetime(value)
            result.future_value = value
            result.past_value = value
            result.success = True
            return result

        # handle "last Friday", "last mon"
        match = regex.match(self.config.last_regex, trimmed_source)
        if match and match.start() == 0 and len(match.group()) == len(trimmed_source):
            weekday_str = match.group(Constants.WEEKDAY_GROUP_NAME)
            value = DateUtils.last(
                reference, self.config.day_of_week.get(weekday_str))

            result.timex = DateTimeFormatUtil.luis_date_from_datetime(value)
            result.future_value = value
            result.past_value = value
            result.success = True
            return result

        # handle "Friday"
        match = regex.match(self.config.week_day_regex, trimmed_source)
        if match and match.start() == 0 and len(match.group()) == len(trimmed_source):
            weekday_str = match.group(Constants.WEEKDAY_GROUP_NAME)
            weekday = self.config.day_of_week.get(weekday_str)
            value = DateUtils.this(reference, weekday)

            if weekday < int(DayOfWeek.MONDAY):
                weekday = int(DayOfWeek.SUNDAY)

            if weekday < reference.isoweekday():
                value = DateUtils.next(reference, weekday)

            result.timex = 'XXXX-WXX-' + str(weekday)
            future_date = value
            past_date = value

            if future_date < reference:
                future_date += timedelta(weeks=1)

            if past_date >= reference:
                past_date -= timedelta(weeks=1)

            result.future_value = DateUtils.safe_create_from_min_value(future_date.year, future_date.month, future_date.day)
            result.past_value = DateUtils.safe_create_from_min_value(past_date.year, past_date.month, past_date.day)
            result.success = True
            return result

        # handle "for the 27th."
        match = regex.match(self.config.for_the_regex, trimmed_source)
        if match:
            day_str = match.group(Constants.DAY_OF_MONTH)
            er = ExtractResult.get_from_text(day_str)
            day = int(self.config.number_parser.parse(er).value)

            month = reference.month
            year = reference.year

            result.timex = DateTimeFormatUtil.luis_date(-1, -1, day)
            date = datetime(year, month, day)
            result.future_value = date
            result.past_value = date
            result.success = True

            return result

        # handling cases like 'Thursday the 21st', which both 'Thursday' and '21st' refer to a same date
        match = regex.match(
            self.config.week_day_and_day_of_month_regex, trimmed_source)
        if match:
            day_str = match.group(Constants.DAY_OF_MONTH)
            er = ExtractResult.get_from_text(day_str)
            day = int(self.config.number_parser.parse(er).value)
            month = reference.month
            year = reference.year

            # the validity of the phrase is guaranteed in the Date Extractor
            result.timex = DateTimeFormatUtil.luis_date(year, month, day)
            date = datetime(year, month, day)
            result.future_value = date
            result.past_value = date
            result.success = True

            return result

        return result

    def parse_weekday_of_month(self, source: str, reference: datetime) -> DateTimeParseResult:
        from .utilities import DateTimeFormatUtil
        from .utilities import DateTimeResolutionResult
        trimmed_source = source.strip()
        result = DateTimeResolutionResult()
        match = regex.match(
            self.config.week_day_of_month_regex, trimmed_source)

        if not match:
            return result

        cardinal_str = RegExpUtility.get_group(match, Constants.CARDINAL)
        weekday_str = RegExpUtility.get_group(match, Constants.WEEKDAY_GROUP_NAME)
        month_str = RegExpUtility.get_group(match, Constants.MONTH_GROUP_NAME)
        no_year = False
        cardinal = 5 if self.config.is_cardinal_last(
            cardinal_str) else self.config.cardinal_map.get(cardinal_str)
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
            future_date = self._compute_date(
                cardinal, weekday, month, year + 1)
            if future_date.month != month:
                future_date = future_date.replace(day=future_date.day - 7)

        if no_year and past_date >= reference:
            past_date = self._compute_date(cardinal, weekday, month, year - 1)
            if past_date.month != month:
                past_date = past_date.replace(day=past_date.date - 7)

        result.timex = '-'.join(['XXXX', DateTimeFormatUtil.to_str(month, 2),
                                 'WXX', str(weekday), '#' + str(cardinal)])
        result.future_value = future_date
        result.past_value = past_date
        result.success = True
        return result

    def _compute_date(self, cardinal: int, weekday, month: int, year: int):
        from .utilities import DateUtils
        from .utilities import DayOfWeek
        first_day = datetime(year, month, 1)
        first_weekday = DateUtils.this(first_day, weekday)

        if weekday == 0:
            weekday = int(DayOfWeek.SUNDAY)

        if weekday < first_day.isoweekday():
            first_weekday = DateUtils.next(first_day, weekday)

        first_weekday = first_weekday.replace(
            day=first_weekday.day + (7 * (cardinal - 1)))
        return first_weekday

    def parser_duration_with_ago_and_later(self, source: str, reference: datetime) -> DateTimeParseResult:
        from .utilities import AgoLaterUtil
        from .utilities import AgoLaterMode
        return AgoLaterUtil.parse_duration_with_ago_and_later(
            source,
            reference,
            self.config.duration_extractor,
            self.config.duration_parser,
            self.config.unit_map,
            self.config.unit_regex,
            self.config.utility_configuration)

    def parse_number_with_month(self, source: str, reference: datetime) -> DateTimeParseResult:
        from .utilities import DateUtils
        from .utilities import DateTimeFormatUtil
        from .utilities import DateTimeResolutionResult
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
            match = regex.search(
                self.config.relative_month_regex, trimmed_source)
            if match:
                month_str = match.group(Constants.ORDER)
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
                wanted_week_day = self.config.day_of_week.get(
                    match.group(Constants.WEEKDAY_GROUP_NAME))
                first_date = DateUtils.safe_create_from_min_value(
                    reference.year, reference.month, 1)
                first_weekday = first_date.isoweekday()
                delta_days = wanted_week_day - \
                    first_weekday if wanted_week_day > first_weekday else wanted_week_day - first_weekday + 7
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
            result.timex = DateTimeFormatUtil.luis_date(-1, month, day)

            if future_date < reference:
                future_date = future_date.replace(year=future_date.year+1)

            if past_date >= reference:
                past_date = past_date.replace(year=past_date.year+1)
        else:
            result.timex = DateTimeFormatUtil.luis_date(year, month, day)

        result.future_value = future_date
        result.past_value = past_date
        result.success = True
        return result

    def parse_single_number(self, source: str, reference: datetime) -> DateTimeParseResult:
        from .utilities import DateUtils
        from .utilities import DateTimeFormatUtil
        from .utilities import DateTimeResolutionResult
        trimmed_source = source.strip()
        result = DateTimeResolutionResult()

        ers = self.config.ordinal_extractor.extract(trimmed_source)

        if not ers or not ers[0].text:
            ers = self.config.integer_extractor.extract(trimmed_source)

        if not ers or not ers[0].text:
            return result

        day = int(self.config.number_parser.parse(ers[0]).value)
        month = reference.month
        year = reference.year

        result.timex = DateTimeFormatUtil.luis_date(-1, -1, day)
        past_date = DateUtils.safe_create_from_min_value(year, month, day)
        future_date = DateUtils.safe_create_from_min_value(year, month, day)

        if future_date != DateUtils.min_value and future_date < reference:
            future_date = future_date.replace(month=future_date.month + 1)

        if past_date != DateUtils.min_value and past_date >= reference:
            if past_date.month - 1 == 0:
                past_date = past_date.replace(month=12, year=past_date.year - 1)
            else:
                past_date = past_date.replace(month=past_date.month - 1)

        result.future_value = future_date
        result.past_value = past_date
        result.success = True
        return result
