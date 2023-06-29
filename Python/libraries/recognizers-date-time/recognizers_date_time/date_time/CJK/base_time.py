from abc import abstractmethod
from typing import List, Optional, Pattern, Dict, Match
from datetime import datetime
import regex

from recognizers_text.extractor import ExtractResult
from recognizers_date_time.date_time.constants import Constants, TimeTypeConstants
from recognizers_date_time.date_time.extractors import DateTimeExtractor
from recognizers_date_time.date_time.parsers import DateTimeParser, DateTimeParseResult
from recognizers_date_time.date_time.utilities import DateTimeOptionsConfiguration, RegExpUtility, DateTimeFormatUtil, \
    ExtractResultExtension, DateTimeExtra, TimeFunctions
from recognizers_date_time.resources import BaseDateTime
from recognizers_date_time.date_time.data_structures import TimeType


class CJKTimeExtractorConfiguration(DateTimeOptionsConfiguration):

    @property
    @abstractmethod
    def regexes(self) -> Dict[Pattern, TimeType]:
        raise NotImplementedError

    @property
    @abstractmethod
    def ambiguity_time_filters_dict(self) -> Dict[Pattern, Pattern]:
        raise NotImplementedError


class BaseCJKTimeExtractor(DateTimeExtractor):
    @property
    def hour_regex(self) -> Pattern:
        return self._hour_regex

    @property
    def minute_regex(self) -> Pattern:
        return self._minute_regex

    @property
    def second_regex(self) -> Pattern:
        return self._second_regex

    @property
    def extractor_type_name(self) -> str:
        return Constants.SYS_DATETIME_TIME

    def __init__(self, config: CJKTimeExtractorConfiguration):
        self.config = config
        self._hour_regex = RegExpUtility.get_safe_reg_exp(BaseDateTime.HourRegex)
        self._minute_regex = RegExpUtility.get_safe_reg_exp(BaseDateTime.MinuteRegex)
        self._second_regex = RegExpUtility.get_safe_reg_exp(BaseDateTime.SecondRegex)

    def extract(self, source: str, reference: datetime = None) -> List[ExtractResult]:

        result: List[ExtractResult] = list()
        if not source:
            return result

        match_source: Dict[Match, any] = dict()
        matched: List[bool] = [False] * len(source)

        collections = list(map(lambda x: (
            list(regex.finditer(x[0], source)), x[1]), self.config.regexes.items()))
        collections = list(filter(lambda x: len(x[0]) > 0, collections))

        for collection in collections:
            for match in collection[0]:
                for j in range(len(match.group())):
                    matched[match.start() + j] = True
                match_source[match] = collection[1]

        last = -1
        for i in range(len(source)):
            if matched[i]:
                if i + 1 == len(source) or not matched[i + 1]:
                    start = last + 1
                    length = i - last
                    text = source[start:start + length].strip()
                    src_match = next((x for x in iter(match_source) if (
                            x.start() == start and (x.end() - x.start()) == length)), None)
                    if src_match:
                        value = ExtractResult()
                        value.start = start
                        value.length = length
                        value.text = text
                        value.type = self.extractor_type_name
                        value.data = self.__get_data(match_source, src_match)
                        result.append(value)
            else:
                last = i

        result = ExtractResultExtension.filter_ambiguity(result, source, self.config.ambiguity_time_filters_dict)

        return result

    @staticmethod
    def __get_data(source: Dict[Match, any], key: Match) -> any:
        if key not in source:
            return None

        result = DateTimeExtra()
        result.data_type = source[key]
        result.named_entity = key.capturesdict()
        result.match = key
        return result


class CJKTimeParserConfiguration(DateTimeOptionsConfiguration):
    @property
    @abstractmethod
    def time_extractor(self) -> DateTimeExtractor:
        raise NotImplementedError

    @property
    @abstractmethod
    def time_func(self) -> TimeFunctions:
        raise NotImplementedError

    @property
    @abstractmethod
    def function_map(self):
        raise NotImplementedError


class BaseCJKTimeParser(DateTimeParser):
    @property
    def parser_type_name(self) -> str:
        return Constants.SYS_DATETIME_TIME

    def __init__(self, config: CJKTimeParserConfiguration):
        self.config = config

    def parse(self, source: ExtractResult, reference: datetime = None) -> Optional[DateTimeParseResult]:
        if reference is None:
            reference = datetime.now()

        extra: DateTimeExtra = source.data

        if not extra:
            result = self.config.time_extractor.extract(source.text, reference)
            extra = result[0].data

        if extra:
            time_result = self.config.function_map[extra.data_type](extra)
            parse_result = self.config.time_func.pack_time_result(extra, time_result, reference)

            if parse_result.success:
                parse_result.future_resolution[TimeTypeConstants.TIME] = DateTimeFormatUtil.format_time(
                    parse_result.future_value)
                parse_result.past_resolution[TimeTypeConstants.TIME] = DateTimeFormatUtil.format_time(
                    parse_result.past_value)

            result = DateTimeParseResult(source)
            result.value = parse_result
            result.data = time_result
            result.timex_str = parse_result.timex if parse_result is not None else ''
            result.resolution_str = ''

            return result

        return None

    def filter_results(self, query: str, candidate_results: List[DateTimeParseResult]):
        return candidate_results
