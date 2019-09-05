from abc import ABC, abstractmethod
from typing import List, Optional, Pattern, Dict, Match
from datetime import datetime
import regex

from recognizers_text.utilities import QueryProcessor
from recognizers_text.extractor import ExtractResult
from recognizers_number.number.extractors import BaseNumberExtractor
from recognizers_number.number.parsers import BaseNumberParser
from .constants import Constants, TimeTypeConstants
from .extractors import DateTimeExtractor
from .parsers import DateTimeParser, DateTimeParseResult
from .utilities import Token, merge_all_tokens, DateTimeResolutionResult, RegExpUtility,\
    DateTimeOptionsConfiguration, DateTimeOptions, DurationParsingUtil, RegexExtension


class DurationExtractorConfiguration(DateTimeOptionsConfiguration):
    @property
    @abstractmethod
    def all_regex(self) -> Pattern:
        raise NotImplementedError

    @property
    @abstractmethod
    def half_regex(self) -> Pattern:
        raise NotImplementedError

    @property
    @abstractmethod
    def followed_unit(self) -> Pattern:
        raise NotImplementedError

    @property
    @abstractmethod
    def number_combined_with_unit(self) -> Pattern:
        raise NotImplementedError

    @property
    @abstractmethod
    def an_unit_regex(self) -> Pattern:
        raise NotImplementedError

    @property
    @abstractmethod
    def inexact_number_unit_regex(self) -> Pattern:
        raise NotImplementedError

    @property
    @abstractmethod
    def suffix_and_regex(self) -> Pattern:
        raise NotImplementedError

    @property
    @abstractmethod
    def relative_duration_unit_regex(self) -> Pattern:
        raise NotImplementedError

    @property
    @abstractmethod
    def cardinal_extractor(self) -> BaseNumberExtractor:
        raise NotImplementedError

    @property
    @abstractmethod
    def during_regex(self) -> BaseNumberExtractor:
        raise NotImplementedError

    @property
    @abstractmethod
    def unit_map(self) -> {}:
        raise NotImplementedError

    @property
    @abstractmethod
    def unit_value_map(self) -> {}:
        raise NotImplementedError

    @property
    @abstractmethod
    def duration_unit_regex(self) -> Pattern:
        raise NotImplementedError

    @property
    @abstractmethod
    def duration_connector_regex(self) -> Pattern:
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
    def options(self) -> DateTimeOptions:
        raise NotImplementedError


class BaseDurationExtractor(DateTimeExtractor):
    @property
    def extractor_type_name(self) -> str:
        return Constants.SYS_DATETIME_DURATION

    def __init__(self, config: DurationExtractorConfiguration, merge: bool = True):
        self.config = config
        self.merge = merge

    def extract(self, source: str, reference: datetime = None) -> List[ExtractResult]:
        if reference is None:
            reference = datetime.now()

        tokens = self.number_with_unit(source)
        tokens.extend(self.number_with_unit_and_suffix(source, tokens))
        tokens.extend(self.implicit_duration(source))

        rets = merge_all_tokens(tokens, source, self.extractor_type_name)

        if self.merge:
            rets = self.merge_multiple_duration(source, rets)

        rets = self.tag_inequality_prefix(source, rets)

        return rets

    def tag_inequality_prefix(self, text: str, ers: [ExtractResult]):

        for er in ers:
            before_string = text[0: er.start]
            is_inequality_prefix_matched = False

            match = RegexExtension.match_end(self.config.more_than_regex, before_string, True)

            if match.success:
                er.data = TimeTypeConstants.MORE_THAN_MOD
                is_inequality_prefix_matched = True

            if not is_inequality_prefix_matched:

                match = RegexExtension.match_end(self.config.less_than_regex, before_string, True)

                if match.success:
                    er.data = TimeTypeConstants.LESS_THAN_MOD
                    is_inequality_prefix_matched = True

            if is_inequality_prefix_matched:
                er.length += er.start - text.index(match.group())
                er.start = text.index(match.group())
                er.text = text[er.start: er.start + er.length]

        return ers

    def merge_multiple_duration(self, text: str, extractor_results: [ExtractResult]):

        if len(extractor_results) <= 1:
            return extractor_results

        unit_map = self.config.unit_map
        unit_value_map = self.config.unit_value_map
        unit_regex = self.config.duration_unit_regex

        ret = []

        first_extraction_index = 0
        time_unit = 0
        total_unit = 0

        while first_extraction_index < len(extractor_results):
            cur_unit = None
            unit_match = unit_regex.search(extractor_results[first_extraction_index].text)

            if unit_match and str(RegExpUtility.get_group(unit_match, 'unit')) in unit_map:

                cur_unit = str(RegExpUtility.get_group(unit_match, 'unit'))
                total_unit += 1
                if DurationParsingUtil.is_time_duration_unit(unit_map[cur_unit]):
                    time_unit += 1

            if not cur_unit:
                first_extraction_index += 1
                continue

            second_extraction_index = first_extraction_index + 1

            while second_extraction_index < len(extractor_results):
                valid = False
                mid_str_begin = extractor_results[second_extraction_index - 1].start +\
                    extractor_results[second_extraction_index - 1].length if \
                    extractor_results[second_extraction_index - 1].length else 0

                mid_str_end = extractor_results[second_extraction_index].start \
                    if extractor_results[second_extraction_index].start else 0
                mid_str = text[mid_str_begin, mid_str_begin + (mid_str_end - mid_str_begin)]

                match = self.config.duration_connector_regex.search(mid_str)
                if match:
                    unit_match = unit_regex.match(extractor_results[second_extraction_index].text)
                    if unit_match and str(RegExpUtility.get_group(unit_match, 'unit')) in unit_map:
                        next_unit_str = str(RegExpUtility.get_group(unit_match, 'unit'))
                        if unit_value_map[next_unit_str] != unit_value_map[cur_unit]:

                            valid = True
                            if unit_value_map[next_unit_str] < unit_value_map[cur_unit]:
                                cur_unit = next_unit_str

                        total_unit += 1
                        if DurationParsingUtil.is_time_duration_unit(unit_map[next_unit_str]):
                            time_unit += 1

                if not valid:
                    break

                second_extraction_index += 1

            if second_extraction_index - 1 > first_extraction_index:

                node: ExtractResult = ExtractResult()
                node.start = extractor_results[first_extraction_index].start
                node.length = extractor_results[second_extraction_index - 1].start +\
                    extractor_results[second_extraction_index - 1].length -\
                    node.start
                node.text = text[node.start or 0: node.length or 0]
                node.type = extractor_results[first_extraction_index].type

                if time_unit == total_unit:
                    type = Constants.MultipleDuration_Time
                elif time_unit == 0:
                    type = Constants.MultipleDuration_Date
                else:
                    type = Constants.MultipleDuration_DateTime

                node.data = type

                ret.append(node)

                time_unit = 0
                total_unit = 0
            else:
                ret.append(extractor_results[first_extraction_index])

            first_extraction_index = second_extraction_index

        return ret

    def number_with_unit(self, source: str) -> List[Token]:
        ers: List[ExtractResult] = self.config.cardinal_extractor.extract(
            source)
        result: List[Token] = list(
            filter(None, map(lambda x: self.__cardinal_to_token(x, source), ers)))
        result.extend(self.get_tokens_from_regex(
            self.config.number_combined_with_unit, source))
        result.extend(self.get_tokens_from_regex(
            self.config.an_unit_regex, source))
        result.extend(self.get_tokens_from_regex(
            self.config.inexact_number_unit_regex, source))
        return result

    def number_with_unit_and_suffix(self, source: str, tokens: List[Token]) -> List[Token]:
        result: List[Token] = list(
            filter(None, map(lambda x: self.__base_to_token(x, source), tokens)))
        return result

    def implicit_duration(self, source: str) -> List[Token]:
        result: List[Token] = self.get_tokens_from_regex(
            self.config.all_regex, source)
        result.extend(self.get_tokens_from_regex(
            self.config.half_regex, source))
        result.extend(self.get_tokens_from_regex(
            self.config.relative_duration_unit_regex, source))

        if (self.config.options & DateTimeOptions.CALENDAR) != 0:
            result.extend(self.get_tokens_from_regex(
                self.config.during_regex, source)
            )

        return result

    def __cardinal_to_token(self, cardinal: ExtractResult, source: str) -> Optional[Token]:
        after = source[cardinal.start + cardinal.length:]
        match = regex.match(self.config.followed_unit, after)

        if match is not None:
            return Token(cardinal.start, cardinal.start + cardinal.length + len(match.group()))

        return None

    def __base_to_token(self, token: Token, source: str) -> Optional[Token]:
        after = source[token.start + token.length:]
        match = regex.match(self.config.suffix_and_regex, after)

        if match is not None:
            return Token(token.start, token.start + token.length + len(match.group()))

        return None

    def get_tokens_from_regex(self, pattern: Pattern, source: str) -> List[Token]:
        return list(map(lambda x: Token(x.start(), x.end()), regex.finditer(pattern, source)))


class DurationParserConfiguration(ABC):
    @property
    @abstractmethod
    def cardinal_extractor(self) -> BaseNumberExtractor:
        raise NotImplementedError

    @property
    @abstractmethod
    def number_parser(self) -> BaseNumberParser:
        raise NotImplementedError

    @property
    @abstractmethod
    def followed_unit(self) -> Pattern:
        raise NotImplementedError

    @property
    @abstractmethod
    def suffix_and_regex(self) -> Pattern:
        raise NotImplementedError

    @property
    @abstractmethod
    def number_combined_with_unit(self) -> Pattern:
        raise NotImplementedError

    @property
    @abstractmethod
    def an_unit_regex(self) -> Pattern:
        raise NotImplementedError

    @property
    @abstractmethod
    def all_date_unit_regex(self) -> Pattern:
        raise NotImplementedError

    @property
    @abstractmethod
    def half_date_unit_regex(self) -> Pattern:
        raise NotImplementedError

    @property
    @abstractmethod
    def inexact_number_unit_regex(self) -> Pattern:
        raise NotImplementedError

    @property
    @abstractmethod
    def unit_map(self) -> Dict[str, str]:
        raise NotImplementedError

    @property
    @abstractmethod
    def unit_value_map(self) -> Dict[str, int]:
        raise NotImplementedError

    @property
    @abstractmethod
    def double_numbers(self) -> Dict[str, float]:
        raise NotImplementedError

    @property
    @abstractmethod
    def options(self) -> DateTimeOptions:
        return DateTimeOptions.NONE


class BaseDurationParser(DateTimeParser):
    @property
    def parser_type_name(self) -> str:
        return Constants.SYS_DATETIME_DURATION

    def __init__(self, config: DurationParserConfiguration):
        self.config = config

    def parse(self, source: ExtractResult, reference: datetime = None) -> Optional[DateTimeParseResult]:
        if reference is None:
            reference = datetime.now()

        result = DateTimeParseResult(source)

        if source.type is self.parser_type_name:
            source_text = source.text.lower()

            inner_result = self.parse_number_with_unit(source_text, reference)
            if not inner_result.success:
                inner_result = self.parse_implicit_duration(
                    source_text, reference)

            if inner_result.success:
                inner_result.future_resolution[TimeTypeConstants.DURATION] = str(
                    inner_result.future_value)
                inner_result.past_resolution[TimeTypeConstants.DURATION] = str(
                    inner_result.past_value)
                result.value = inner_result
                result.timex_str = inner_result.timex if inner_result is not None else ''
                result.resolution_str = ''

        return result

    def parse_number_with_unit(self, source: str, reference: datetime) -> DateTimeResolutionResult:
        source = source.strip()

        result = self.parse_number_space_unit(source)

        if not result.success:
            result = self.parse_number_combined_unit(source)

        if not result.success:
            result = self.parse_an_unit(source)

        if not result.success:
            result = self.parse_in_exact_number_unit(source)

        return result

    def parse_implicit_duration(self, source: str, reference: datetime) -> DateTimeResolutionResult:
        source = source.strip()

        result = self.get_result_from_regex(
            self.config.all_date_unit_regex, source, 1)

        if not result.success:
            result = self.get_result_from_regex(
                self.config.half_date_unit_regex, source, 0.5)

        if not result.success:
            result = self.get_result_from_regex(
                self.config.followed_unit, source, 1)

        return result

    def get_result_from_regex(self, pattern: Pattern, source: str, num: float) -> DateTimeResolutionResult:
        result = DateTimeResolutionResult()

        match: Match = regex.search(pattern, source)
        if match is None:
            return result

        source_unit: str = match.group('unit') or ''
        if source_unit not in self.config.unit_map:
            return result

        num = QueryProcessor.float_or_int(num)
        unit = self.config.unit_map[source_unit]
        is_time = 'T' if self.is_less_than_day(unit) else ''
        result.timex = f'P{is_time}{num}{unit[0]}'
        result.future_value = QueryProcessor.float_or_int(
            num * self.config.unit_value_map[source_unit])
        result.past_value = result.future_value
        result.success = True
        return result

    def parse_number_space_unit(self, source: str) -> DateTimeResolutionResult:
        result = DateTimeResolutionResult()

        ers = self.config.cardinal_extractor.extract(source)
        if len(ers) != 1:
            return result

        suffix = source
        source_unit = ''
        er = ers[0]
        pr = self.config.number_parser.parse(er)
        no_num = source[pr.start + pr.length:].strip().lower()
        match = regex.search(self.config.followed_unit, no_num)

        if match is not None:
            suffix = RegExpUtility.get_group(match, 'suffix')
            source_unit = RegExpUtility.get_group(match, 'unit')

        if source_unit not in self.config.unit_map:
            return result

        num = float(pr.value) + self.parse_number_with_unit_and_suffix(suffix)
        unit = self.config.unit_map[source_unit]

        num = QueryProcessor.float_or_int(num)
        is_time = 'T' if self.is_less_than_day(unit) else ''
        result.timex = f'P{is_time}{num}{unit[0]}'
        result.future_value = QueryProcessor.float_or_int(
            num * self.config.unit_value_map[source_unit])
        result.past_value = result.future_value
        result.success = True
        return result

    def parse_number_with_unit_and_suffix(self, source: str) -> float:
        match = regex.search(self.config.suffix_and_regex, source)

        if match is not None:
            num = match.group('suffix_num') or ''
            return self.config.double_numbers.get(num, 0)

        return 0

    def parse_number_combined_unit(self, source: str) -> DateTimeResolutionResult:
        result = DateTimeResolutionResult()

        match = regex.search(self.config.number_combined_with_unit, source)
        if match is None:
            return result

        num = float(match.group('num')) + \
            self.parse_number_with_unit_and_suffix(source)

        source_unit = match.group('unit') or ''
        if source_unit not in self.config.unit_map:
            return result

        unit = self.config.unit_map[source_unit]
        if num > 1000 and unit in ['Y', 'MON', 'W']:
            return result

        num = QueryProcessor.float_or_int(num)
        is_time = 'T' if self.is_less_than_day(unit) else ''
        result.timex = f'P{is_time}{num}{unit[0]}'
        result.future_value = QueryProcessor.float_or_int(
            num * self.config.unit_value_map[source_unit])
        result.past_value = result.future_value
        result.success = True
        return result

    def parse_an_unit(self, source: str) -> DateTimeResolutionResult:
        result = DateTimeResolutionResult()
        match = regex.search(self.config.an_unit_regex, source)

        if match is None:
            match = regex.search(self.config.half_date_unit_regex, source)

        if match is None:
            return result

        num = (0.5 if match.group('half') else 1) + \
            self.parse_number_with_unit_and_suffix(source)
        source_unit = match.group('unit') or ''

        if source_unit not in self.config.unit_map:
            return result

        num = QueryProcessor.float_or_int(num)
        unit = self.config.unit_map[source_unit]
        is_time = 'T' if self.is_less_than_day(unit) else ''
        result.timex = f'P{is_time}{num}{unit[0]}'
        result.future_value = QueryProcessor.float_or_int(
            num * self.config.unit_value_map[source_unit])
        result.past_value = result.future_value
        result.success = True
        return result

    def parse_in_exact_number_unit(self, source: str) -> DateTimeResolutionResult:
        result = DateTimeResolutionResult()
        match = regex.search(self.config.inexact_number_unit_regex, source)

        if match is None:
            return result

        # set the inexact number "few", "some" to 3 for now
        num = float(3)
        source_unit = match.group('unit') or ''
        if source_unit not in self.config.unit_map:
            return result

        unit = self.config.unit_map[source_unit]
        if num > 1000 and unit in ['Y', 'MON', 'W']:
            return result

        num = QueryProcessor.float_or_int(num)
        is_time = 'T' if self.is_less_than_day(unit) else ''
        result.timex = f'P{is_time}{num}{unit[0]}'
        result.future_value = QueryProcessor.float_or_int(
            num * self.config.unit_value_map[source_unit])
        result.past_value = result.future_value
        result.success = True
        return result

    def is_less_than_day(self, source: str) -> bool:
        return source in ['H', 'M', 'S']
