from abc import abstractmethod
from datetime import datetime
from typing import Pattern, List, Dict

import regex

from recognizers_date_time.date_time.constants import Constants, TimeTypeConstants
from recognizers_date_time.date_time.extractors import DateTimeExtractor
from recognizers_date_time.date_time.parsers import DateTimeParser, DateTimeParseResult
from recognizers_date_time.date_time.base_datetime import MatchedTimex
from recognizers_date_time.date_time.utilities import DateTimeOptionsConfiguration, Token, RegExpUtility, \
    merge_all_tokens, DateTimeResolutionResult, SetHandler
from recognizers_text.extractor import ExtractResult


class CJKSetExtractorConfiguration(DateTimeOptionsConfiguration):
    @property
    @abstractmethod
    def last_regex(self) -> Pattern:
        raise NotImplementedError

    @property
    @abstractmethod
    def each_prefix_regex(self) -> Pattern:
        raise NotImplementedError

    @property
    @abstractmethod
    def each_suffix_regex(self) -> Pattern:
        raise NotImplementedError

    @property
    @abstractmethod
    def each_unit_regex(self) -> Pattern:
        raise NotImplementedError

    @property
    @abstractmethod
    def unit_regex(self) -> Pattern:
        raise NotImplementedError

    @property
    @abstractmethod
    def each_day_regex(self) -> Pattern:
        raise NotImplementedError

    @property
    @abstractmethod
    def each_date_unit_regex(self) -> Pattern:
        raise NotImplementedError

    @property
    @abstractmethod
    def duration_extractor(self) -> DateTimeExtractor:
        raise NotImplementedError

    @property
    @abstractmethod
    def time_extractor(self) -> DateTimeExtractor:
        raise NotImplementedError

    @property
    @abstractmethod
    def date_extractor(self) -> DateTimeExtractor:
        raise NotImplementedError

    @property
    @abstractmethod
    def date_time_extractor(self) -> DateTimeExtractor:
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


class BaseCJKSetExtractor(DateTimeExtractor):

    @property
    def extractor_type_name(self) -> str:
        return Constants.SYS_DATETIME_SET

    def __init__(self, config: CJKSetExtractorConfiguration):
        self.config = config

    def extract(self, text: str, reference: datetime = None) -> List[ExtractResult]:
        if reference is None:
            reference = datetime.now()
        tokens: List[Token] = list()

        tokens.extend(self.match_each_unit(text))
        tokens.extend(self.match_each_duration(text, reference))
        tokens.extend(self.match_each(self.config.date_extractor, text, reference))
        tokens.extend(self.match_each(self.config.date_time_extractor, text, reference))
        tokens.extend(self.match_each(self.config.time_period_extractor, text, reference))
        tokens.extend(self.match_each(self.config.time_extractor, text, reference))

        result = merge_all_tokens(tokens, text, self.extractor_type_name)
        return result

    def match_each_duration(self, text: str, reference_time: datetime):
        ret: List[Token] = list()
        ers = self.config.duration_extractor.extract(text, reference_time)

        for er in ers:
            # "each last summer" doesn't make sense
            if regex.search(self.config.last_regex, er.text):
                continue

            before_str = text[0:er.start]
            before_match = regex.search(self.config.each_prefix_regex, before_str)
            if before_match:
                ret.append(Token(before_match.start(), er.start + er.length))
            else:
                after_str = text[er.start + er.length:]
                after_match = regex.search(self.config.each_suffix_regex, after_str)
                if after_match:
                    ret.append(Token(er.start, er.start + er.length + len(after_match.group())))
        return ret

    def match_each_unit(self, text: str) -> List[Token]:
        # Handle "each month"
        for regexp in regex.finditer(self.config.each_unit_regex, text):
            yield Token(regexp.start(), regexp.start() + regexp.end())

    def match_each(self, extractor: DateTimeExtractor, text: str, reference_time: datetime):
        ret: List[Token] = list()
        ers = extractor.extract(text, reference_time)

        for er in ers:
            before_str = text[0:er.start].strip()
            match = regex.search(self.config.each_prefix_regex, before_str)

            if match:
                ret.append(Token(match.start(), match.start() + match.end() + er.length))
            elif er.type == Constants.SYS_DATETIME_TIME or er.type == Constants.SYS_DATETIME_DATE:
                # Cases like "every day at 2pm" or "every year on April 15th"
                each_regex = self.config.each_day_regex if er.type == Constants.SYS_DATETIME_TIME \
                    else self.config.each_date_unit_regex

                match = regex.search(each_regex, before_str)
                if match:
                    ret.append(Token(match.start(), match.start() + match.end() + er.length))
        return ret


class CJKSetParserConfiguration(DateTimeOptionsConfiguration):

    @property
    @abstractmethod
    def duration_extractor(self) -> DateTimeExtractor:
        return NotImplementedError

    @property
    @abstractmethod
    def duration_parser(self) -> DateTimeParser:
        return NotImplementedError

    @property
    @abstractmethod
    def time_extractor(self) -> DateTimeExtractor:
        return NotImplementedError

    @property
    @abstractmethod
    def time_parser(self) -> DateTimeParser:
        return NotImplementedError

    @property
    @abstractmethod
    def time_period_extractor(self) -> DateTimeExtractor:
        return NotImplementedError

    @property
    @abstractmethod
    def time_period_parser(self) -> DateTimeParser:
        return NotImplementedError

    @property
    @abstractmethod
    def date_extractor(self) -> DateTimeExtractor:
        return NotImplementedError

    @property
    @abstractmethod
    def date_parser(self) -> DateTimeParser:
        return NotImplementedError

    @property
    @abstractmethod
    def date_time_extractor(self) -> DateTimeExtractor:
        return NotImplementedError

    @property
    @abstractmethod
    def date_time_parser(self) -> DateTimeParser:
        return NotImplementedError

    @property
    @abstractmethod
    def unit_map(self) -> Dict[str, str]:
        raise NotImplementedError

    @property
    @abstractmethod
    def each_prefix_regex(self) -> Pattern:
        raise NotImplementedError

    @property
    @abstractmethod
    def each_unit_regex(self) -> Pattern:
        raise NotImplementedError

    @property
    @abstractmethod
    def each_day_regex(self) -> Pattern:
        raise NotImplementedError

    @property
    @abstractmethod
    def each_date_unit_regex(self) -> Pattern:
        raise NotImplementedError

    @abstractmethod
    def get_matched_unit_timex(self, text: str) -> MatchedTimex:
        raise NotImplementedError


class BaseCJKSetParser(DateTimeParser):

    @property
    def parser_type_name(self) -> str:
        return Constants.SYS_DATETIME_SET

    def __init__(self, config: CJKSetParserConfiguration):
        self.config = config

    def parse(self, extract_result: ExtractResult, reference_date: datetime = None):
        if reference_date is None:
            reference_date = datetime.now()

        value = None
        if extract_result.type == self.parser_type_name:
            inner_result = self.parse_each_unit(extract_result.text)

            if not inner_result.success:
                inner_result = self.parse_each_duration(extract_result.text, reference_date)

            # NOTE: Please do not change the order of following function
            # we must consider datetime before date
            if not inner_result.success:
                inner_result = self.parse_each(self.config.date_time_extractor,
                                               self.config.date_time_parser,
                                               extract_result.text,
                                               reference_date)
            if not inner_result.success:
                inner_result = self.parse_each(self.config.date_extractor,
                                               self.config.date_parser,
                                               extract_result.text, reference_date)
            if not inner_result.success:
                inner_result = self.parse_each(self.config.time_period_extractor,
                                               self.config.time_period_parser,
                                               extract_result.text, reference_date)
            if not inner_result.success:
                inner_result = self.parse_each(self.config.time_extractor,
                                               self.config.time_parser,
                                               extract_result.text, reference_date)

            if inner_result.success:
                inner_result.future_resolution = {
                    TimeTypeConstants.SET: str(inner_result.future_value)
                }
                inner_result.past_resolution = {
                    TimeTypeConstants.SET: str(inner_result.past_value)
                }

                value = inner_result

        ret = DateTimeParseResult(
            text=extract_result.text,
            start=extract_result.start,
            length=extract_result.length,
            type=extract_result.type,
            data=extract_result.data,
            value=value,
            timex_str=value.timex if value else '',
            resolution_str=''
        )
        return ret

    def filter_results(self, query: str, candidate_results: List[DateTimeParseResult]) -> List[DateTimeParseResult]:
        return candidate_results

    def parse_each_duration(self, text: str, ref_date: datetime):
        ret = DateTimeResolutionResult()
        ers = self.config.duration_extractor.extract(text, ref_date)

        if len(ers) != 1 or not text[ers[0].start + ers[0].length:].strip():
            return ret

        after_str = text[ers[0].start + ers[0].length:]
        matches = regex.match(self.config.each_prefix_regex, after_str)
        if matches:
            pr = self.config.duration_parser.parse(ers[0], datetime.now())
            ret = SetHandler.resolve_set(pr.timex_str)
            return ret

        return ret

    def parse_each_unit(self, text: str) -> DateTimeResolutionResult:
        ret = DateTimeResolutionResult()
        # handle "each month"
        match = regex.match(self.config.each_unit_regex, text)
        if match:
            source_unit = RegExpUtility.get_group(match, Constants.UNIT)
            if source_unit and source_unit in self.config.unit_map:
                get_matched_unit_timex = self.config.get_matched_unit_timex(source_unit)
                if get_matched_unit_timex.matched:
                    ret = SetHandler.resolve_set(ret, get_matched_unit_timex.timex)

        return ret

    def parse_each(self, extractor: DateTimeExtractor, parser: DateTimeParser, text: str, ref_date: datetime):
        ret = DateTimeResolutionResult()
        ers = extractor.extract(text, ref_date)
        success = False

        for er in ers:
            before_str = text[0:er.start].strip()
            match = regex.search(self.config.each_prefix_regex, before_str)

            if match and (match.end() - match.start() + er.length == len(text)):
                success = True
            elif er.type == Constants.SYS_DATETIME_TIME or er.type == Constants.SYS_DATETIME_DATE:
                # Cases like "every day at 2pm" or "every year on April 15th"
                each_regex = self.config.each_day_regex if er.type == Constants.SYS_DATETIME_TIME \
                    else self.config.each_date_unit_regex

                match = regex.match(each_regex, before_str)
                if match and (match.end() - match.start() + er.length == len(text)):
                    success = True

            if success:
                pr = parser.parse(er, ref_date)
                ret = SetHandler.resolve_set(ret, pr.timex_str)
                break

        return ret




