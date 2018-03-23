from abc import ABC, abstractmethod
from typing import List, Optional, Pattern, Dict
from datetime import datetime
import calendar
import regex

from recognizers_text.extractor import ExtractResult
from recognizers_text.utilities import RegExpUtility
from recognizers_number import BaseNumberExtractor, BaseNumberParser
from recognizers_number.number import Constants as NumberConstants
from .constants import Constants
from .extractors import DateTimeExtractor
from .parsers import DateTimeParser, DateTimeParseResult
from .utilities import DateTimeUtilityConfiguration, Token, merge_all_tokens, get_tokens_from_regex, DateUtils, AgoLaterUtil

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
                            length = len(RegExpUtility.get_group(match_case,'end'))
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
    def week_day_of_monthRegex(self) -> Pattern:
        raise NotImplementedError

    @property
    @abstractmethod
    def forTheRegex(self) -> Pattern:
        raise NotImplementedError

    @property
    @abstractmethod
    def weekDayAndDayOfMonthRegex(self) -> Pattern:
        raise NotImplementedError

    @property
    @abstractmethod
    def relativeMonthRegex(self) -> Pattern:
        raise NotImplementedError

    @property
    @abstractmethod
    def utilityConfiguration(self) -> DateTimeUtilityConfiguration:
        raise NotImplementedError

    @property
    @abstractmethod
    def dateTokenPrefix(self) -> str:
        raise NotImplementedError

    @abstractmethod
    def getSwiftDay(self, source: str) -> int:
        raise NotImplementedError
        
    @abstractmethod
    def getSwiftMonth(self, source: str) -> int:
        raise NotImplementedError

    @abstractmethod
    def isCardinalLast(self, source: str) -> bool:
        raise NotImplementedError

class BaseDateParser(DateTimeParser):
    @property
    def parser_type_name(self) -> str:
        return Constants.SYS_DATETIME_DATE

    def __init__(self, config: DateParserConfiguration):
        self.config = config

    def parse(self, source: ExtractResult, reference: datetime = None) -> Optional[DateTimeParseResult]:
        #TODO: code
        pass
