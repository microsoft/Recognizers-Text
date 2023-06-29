from abc import abstractmethod
from typing import Dict, List, Pattern, Optional
from datetime import datetime

from regex import regex

from recognizers_number_with_unit.number_with_unit.parsers import UnitValue
from recognizers_date_time.date_time import Constants, TimeTypeConstants
from recognizers_date_time.date_time.utilities import RegExpUtility, DateTimeOptionsConfiguration,\
    ExtractResultExtension, DurationParsingUtil, Token, get_tokens_from_regex, DateTimeOptions, Metadata, TimexUtil, \
    DateTimeResolutionResult
from recognizers_text.extractor import Extractor
from recognizers_text.parser import Parser
from recognizers_date_time.date_time.extractors import DateTimeExtractor
from recognizers_date_time.date_time.parsers import DateTimeParser, DateTimeParseResult
from recognizers_text.extractor import ExtractResult


class CJKDurationExtractorConfiguration(DateTimeOptionsConfiguration):
    @property
    @abstractmethod
    def duration_unit_regex(self) -> Pattern:
        return NotImplementedError

    @property
    @abstractmethod
    def duration_connector_regex(self) -> Pattern:
        return NotImplementedError

    @property
    @abstractmethod
    def year_regex(self) -> Pattern:
        return NotImplementedError

    @property
    @abstractmethod
    def all_regex(self) -> Pattern:
        return NotImplementedError

    @property
    @abstractmethod
    def half_regex(self) -> Pattern:
        return NotImplementedError

    @property
    @abstractmethod
    def relative_duration_unit_regex(self) -> Pattern:
        return NotImplementedError

    @property
    @abstractmethod
    def during_regex(self) -> Pattern:
        return NotImplementedError

    @property
    @abstractmethod
    def some_regex(self) -> Pattern:
        return NotImplementedError

    @property
    @abstractmethod
    def more_or_less_regex(self) -> Pattern:
        return NotImplementedError

    @property
    @abstractmethod
    def internal_extractor(self) -> Extractor:
        return NotImplementedError

    @property
    @abstractmethod
    def unit_map(self) -> Dict[str, str]:
        return NotImplementedError

    @property
    @abstractmethod
    def unit_value_map(self) -> Dict[str, float]:
        return NotImplementedError

    @property
    @abstractmethod
    def ambiguity_duration_filters_dict(self) -> Dict[Pattern, Pattern]:
        return NotImplementedError


class BaseCJKDurationExtractor(DateTimeExtractor):
    @property
    def extractor_type_name(self) -> str:
        return Constants.SYS_DATETIME_DURATION

    def __init__(self, config: CJKDurationExtractorConfiguration, merge: bool = True):
        self.config = config
        self.merge = merge

    def extract(self, source: str, reference: datetime = None) -> List[ExtractResult]:
        # Use unit to extract
        ret_list = self.config.internal_extractor.extract(source)
        result: List[ExtractResult] = list()

        for er_result in ret_list:
            # filter
            if regex.search(self.config.year_regex, er_result.text):
                continue

            result.append(er_result)

        # handle "all day", "more days", "few days"
        result.extend(self.implicit_duration(source))
        result = ExtractResultExtension.merge_all_tokens(result)

        if self.merge:
            result = self.merge_multiple_duration(source, result)
            result = ExtractResultExtension.filter_ambiguity(result, source,
                                                             self.config.ambiguity_duration_filters_dict)

        return result

    def merge_multiple_duration(self, text: str, er: List[ExtractResult]) -> List[ExtractResult]:
        if len(er) <= 1:
            return er

        unit_map = self.config.unit_map
        unit_value_map = self.config.unit_value_map
        unit_regex = self.config.duration_unit_regex
        result: List[ExtractResult] = list()

        first_extraction_index = 0
        time_unit = 0
        total_unit = 0

        while first_extraction_index < len(er):
            cur_unit = None
            unit_match = regex.match(unit_regex, er[first_extraction_index].text)

            if unit_match and RegExpUtility.get_group(unit_match, Constants.UNIT_GROUP_NAME) in unit_map:
                cur_unit = RegExpUtility.get_group(unit_match, Constants.UNIT_GROUP_NAME)
                total_unit += 1
                if DurationParsingUtil.is_time_duration_unit(unit_map[cur_unit]):
                    time_unit += 1

            if not cur_unit:
                first_extraction_index += 1
                continue

            second_extraction_index = first_extraction_index + 1
            while second_extraction_index < len(er):
                valid = False
                mid_str_begin = er[second_extraction_index - 1].start + er[second_extraction_index - 1].length
                mid_str_end = er[second_extraction_index].start
                if mid_str_begin > mid_str_end:
                    return er

                mid_str = text[mid_str_begin:mid_str_end-mid_str_begin]
                match = regex.match(self.config.duration_connector_regex, mid_str)
                if match:
                    # If the second element of a group is a modifier, it should not be merged with subsequent elements.
                    # For example "4 days or more and 1 week or less" should return 2 separate extractions.
                    if second_extraction_index > 1 and er[second_extraction_index - 1].meta_data and \
                            er[second_extraction_index - 1].meta_data.has_mod:
                        break

                    unit_match = regex.match(unit_regex, er[second_extraction_index].text)
                    if unit_match and RegExpUtility.get_group(unit_match, Constants.UNIT_GROUP_NAME) in unit_map:
                        next_unit_str = RegExpUtility.get_group(unit_match, Constants.UNIT_GROUP_NAME)
                        if unit_value_map[unit_map[next_unit_str]] != unit_value_map[unit_map[cur_unit]]:
                            valid = True
                            if unit_value_map[unit_map[next_unit_str]] < unit_value_map[unit_map[cur_unit]]:
                                cur_unit = next_unit_str

                        total_unit += 1
                        if DurationParsingUtil.is_time_duration_unit(unit_map[next_unit_str]):
                            time_unit += 1

                if not valid:
                    break

                second_extraction_index += 1

            if second_extraction_index - 1 > first_extraction_index:
                node = ExtractResult()
                node.start = er[first_extraction_index].start
                node.length = er[second_extraction_index - 1].start + er[second_extraction_index - 1].length - node.start
                node.text = text[node.start: node.length]
                node.type = er[first_extraction_index].type

                #  Add multiple duration type to extract result
                type = Constants.MULTIPLE_DURATION_TIME #  Default type
                if time_unit == total_unit:
                    type = Constants.MULTIPLE_DURATION_TIME
                elif time_unit == 0:
                    type = Constants.MULTIPLE_DURATION_DATE
                node.data = type

                result.append(node)
                time_unit = 0
                total_unit = 0
            else:
                result.append(er[first_extraction_index])

            first_extraction_index = second_extraction_index

        return result

    def implicit_duration(self, text: str) -> List[ExtractResult]:
        ret: List[Token] = list()
        #  handle "all day", "all year"
        ret.extend(get_tokens_from_regex(self.config.all_regex, text))
        #  handle "half day", "half year"
        ret.extend(get_tokens_from_regex(self.config.half_regex, text))
        #  handle "next day", "last year"
        ret.extend(get_tokens_from_regex(self.config.relative_duration_unit_regex, text))
        #  handle "more day", "more year"
        ret.extend(get_tokens_from_regex(self.config.more_or_less_regex, text))
        #  handle "few days", "few months"
        ret.extend(get_tokens_from_regex(self.config.some_regex, text))
        #  handle "during/for the day/week/month/year"
        if (self.config.options and DateTimeOptions.CALENDAR) != 0:
            ret.extend(get_tokens_from_regex(self.config.during_regex, text))

        result: List[ExtractResult] = list()
        for e in ret:
            node = ExtractResult()
            node.start = e.start
            node.length = e.length
            node.text = text[node.start:node.length]
            node.type = self.extractor_type_name
            node.meta_data = Metadata()
            node.meta_data.has_mod = True
            result.append(node)

        return result


class CJKDurationParserConfiguration(DateTimeOptionsConfiguration):
    @property
    @abstractmethod
    def duration_extractor(self) -> DateTimeExtractor:
        return NotImplementedError

    @property
    @abstractmethod
    def internal_parser(self) -> Parser:
        return NotImplementedError

    @property
    @abstractmethod
    def year_regex(self) -> Pattern:
        return NotImplementedError

    @property
    @abstractmethod
    def some_regex(self) -> Pattern:
        return NotImplementedError

    @property
    @abstractmethod
    def more_or_less_regex(self) -> Pattern:
        return NotImplementedError

    @property
    @abstractmethod
    def duration_unit_regex(self) -> Pattern:
        return NotImplementedError

    @property
    @abstractmethod
    def an_unit_regex(self) -> Pattern:
        return NotImplementedError

    @property
    @abstractmethod
    def duration_connector_regex(self) -> Pattern:
        return NotImplementedError

    @property
    @abstractmethod
    def unit_map(self) -> Dict[str, str]:
        return NotImplementedError

    @property
    @abstractmethod
    def unit_value_map(self) -> Dict[str, float]:
        return NotImplementedError


class BaseCJKDurationParser(DateTimeParser):
    @property
    def parser_type_name(self) -> str:
        return Constants.SYS_DATETIME_DURATION

    def __init__(self, config: CJKDurationParserConfiguration):
        self.config = config

    def parse(self, source: ExtractResult, reference: datetime = None) -> Optional[DateTimeParseResult]:
        datetime_parse_result = self.parse_merged_duration(source.text, reference)

        if not datetime_parse_result:
            datetime_parse_result = DurationParsingUtil.\
                parse_inexact_number_with_unit(source.text, self.config.some_regex, self.config.unit_map,
                                               self.config.unit_value_map, is_cjk=True)

        if not datetime_parse_result:
            datetime_parse_result = self.parse_an_unit(source.text)

        if not datetime_parse_result:
            parse_result = self.config.internal_parser.parse(source)
            unit_result = UnitValue(parse_result.value, parse_result.unit)
            if not unit_result:
                return None

            unit_str = unit_result.unit
            if unit_result.number:
                num = float(unit_result.number.__str__())
            else:
                num = 1

            datetime_parse_result.timex = TimexUtil.generate_duration_timex(num, unit_str,
                                                                            DurationParsingUtil.is_less_than_day(unit_str))
            datetime_parse_result.future_value = \
                datetime_parse_result.past_value = num * self.config.unit_value_map[unit_str]
            datetime_parse_result.success = True

            if datetime_parse_result.success:
                datetime_parse_result.future_resolution = {
                    TimeTypeConstants.DURATION: datetime_parse_result.future_value
                }
                datetime_parse_result.past_resolution = {
                    TimeTypeConstants.DURATION: datetime_parse_result.past_value
                }

                more_or_less_match = regex.match(self.config.more_or_less_regex, source.text)
                if more_or_less_match:
                    if RegExpUtility.get_group(more_or_less_match, Constants.LESS_GROUP_NAME):
                        datetime_parse_result.mod = Constants.LESS_THAN_MOD
                    elif RegExpUtility.get_group(more_or_less_match, Constants.MORE_GROUP_NAME):
                        datetime_parse_result.mod = Constants.MORE_THAN_MOD

            ret = DateTimeParseResult()
            ret.text = source.text
            ret.start = source.start
            ret.type = source.type
            ret.data = source.type
            ret.value = datetime_parse_result
            ret.timex_str = datetime_parse_result.timex
            ret.resolution_str = None

            return ret

    def filter_results(self, query: str, candidate_results: List[DateTimeParseResult]) -> List[DateTimeParseResult]:
        return candidate_results

    def parse_an_unit(self, text: str) -> DateTimeResolutionResult:
        ret = DateTimeResolutionResult()
        match = regex.match(self.config.an_unit_regex, text)
        if RegExpUtility.get_group(match, Constants.ANOTHER_GROUP_NAME):
            if RegExpUtility.get_group(match, Constants.HALF_GROUP_NAME):
                num_val = 0.5
            elif RegExpUtility.get_group(match, Constants.QUARTER_GROUP_NAME):
                num_val = 0.25
            elif RegExpUtility.get_group(match, Constants.THREE_QUARTER_GROUP_NAME):
                num_val = 0.75
            else:
                num_val = 1

            src_unit = RegExpUtility.get_group(match, Constants.UNIT_GROUP_NAME)
            if src_unit in self.config.unit_map:
                unit_str = self.config.unit_map[src_unit]
                ret.timex = TimexUtil.generate_duration_timex(num_val, unit_str,
                                                              DurationParsingUtil.is_less_than_day(unit_str))
                ret.future_value = ret.past_value = num_val * self.config.unit_value_map[unit_str]
                ret.success = True
        return ret

    def parse_merged_duration(self, text: str, reference: datetime) -> DateTimeResolutionResult:
        ret = DateTimeResolutionResult()
        duration_extractor = self.config.duration_extractor

        # DurationExtractor without parameter will not extract merged duration
        ers = duration_extractor.extract(text, reference)

        #  only handle merged duration cases like "1 month 21 days"
        if len(ers) <= 1:
            ret.success = False
            return ret

        start = ers[0].start
        if start != 0:
            before_str = text[0:start-1]
            if before_str:
                return ret

        end = ers[-1].start + ers[-1].length
        if end != len(text):
            after_str = text[end:]
            if after_str:
                return ret

        prs: List[DateTimeParseResult] = list()
        timex_dict: Dict[str, str] = dict()

        # insert timex into a dictionary
        for er in ers:
            unit_regex = self.config.duration_unit_regex
            unit_match = regex.match(unit_regex, text)
            if unit_match:
                pr = self.parse(er)
                if pr and pr.value:
                    timex_dict[self.config.unit_map[RegExpUtility.get_group(unit_match, Constants.UNIT_GROUP_NAME)]] \
                        = pr.timex_str
                    prs.append(pr)
        # sort the timex using the granularity of the duration, "P1M23D" for "1 month 23 days" and "23 days 1 month"
        if len(prs) > 0:
            ret.timex = TimexUtil.generate_compound_duration_timex(timex_dict, self.config.unit_value_map)

            value = 0
            for pr in prs:
                value += float(pr.value.future_value.__str__())
            ret.future_value = ret.past_value = value

        ret.success = True
        return ret
