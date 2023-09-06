from abc import abstractmethod, ABC
from typing import List, Optional, Pattern, Dict
from datetime import datetime
from collections import namedtuple
import regex

from recognizers_text.extractor import Extractor, ExtractResult
from .constants import Constants, TimeTypeConstants
from .extractors import DateTimeExtractor
from .parsers import DateTimeParser, DateTimeParseResult
from .base_date import BaseDateParser
from .base_time import BaseTimeParser
from .utilities import Token, merge_all_tokens, DateTimeOptions, DateTimeFormatUtil, DateUtils, RegExpUtility, TimexUtil

MatchedIndex = namedtuple('MatchedIndex', ['matched', 'index'])


class MinimalMergedExtractorConfiguration:

    @property
    @abstractmethod
    def ambiguous_range_modifier_prefix(self) -> Pattern:
        raise NotImplementedError

    @property
    @abstractmethod
    def number_ending_pattern(self) -> Pattern:
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
    def integer_extractor(self) -> Extractor:
        raise NotImplementedError

    @property
    @abstractmethod
    def equal_regex(self) -> Pattern:
        raise NotImplementedError

    @property
    @abstractmethod
    def check_both_before_after(self):
        raise NotImplementedError


class MinimalMergedExtractor(DateTimeExtractor):
    @property
    def extractor_type_name(self) -> str:
        return Constants.SYS_DATETIME_MERGED

    def __init__(self, config: MinimalMergedExtractorConfiguration, options: DateTimeOptions):
        self.config = config
        self.options = options

    def extract(self, source: str, reference: datetime = None) -> List[ExtractResult]:
        if reference is None:
            reference = datetime.now()

        result: List[ExtractResult] = list()

        # The order is important, since there can be conflicts in merging
        result = self.add_to(
            result, self.config.date_extractor.extract(source, reference), source)
        result = self.add_to(
            result, self.config.time_extractor.extract(source, reference), source)

        # this should be at the end since if need the extractor to determine the previous text contains time or not
        result = self.add_to(
            result, self.number_ending_regex_match(source, result), source)

        result = sorted(result, key=lambda x: x.start)

        return result

    def add_to(self, destinations: List[ExtractResult], source: List[ExtractResult], text: str) -> List[ExtractResult]:
        for value in source:
            is_found = False
            overlap_indexes: List[int] = list()
            first_index = -1

            for index, destination in enumerate(destinations):
                if destination.overlap(value):
                    is_found = True
                    if destination.cover(value):
                        if first_index == -1:
                            first_index = index
                        overlap_indexes.append(index)
                    else:
                        continue

            if not is_found:
                destinations.append(value)
            elif overlap_indexes:
                temp_dst: List[ExtractResult] = list()

                for index, destination in enumerate(destinations):
                    if index not in overlap_indexes:
                        temp_dst.append(destination)

                # insert at the first overlap occurence to keep the order
                temp_dst.insert(first_index, value)
                destinations = temp_dst
        return destinations

    def number_ending_regex_match(self, source: str, extract_results: List[ExtractResult]) -> List[ExtractResult]:
        tokens: List[Token] = list()

        for extract_result in extract_results:
            if extract_result.type in [Constants.SYS_DATETIME_TIME, Constants.SYS_DATETIME_DATETIME]:
                after_str = source[extract_result.start +
                                   extract_result.length:]
                match = regex.search(
                    self.config.number_ending_pattern, after_str)
                if match:
                    new_time = RegExpUtility.get_group(match, Constants.NEW_TIME)
                    num_res = self.config.integer_extractor.extract(new_time)
                    if not num_res:
                        continue

                    start_position = extract_result.start + \
                        extract_result.length + match.group().index(new_time)
                    tokens.append(
                        Token(start_position, start_position + len(new_time)))

        return merge_all_tokens(tokens, source, Constants.SYS_DATETIME_TIME)


class MinimalMergedParserConfiguration(ABC):
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
    def since_regex(self) -> Pattern:
        raise NotImplementedError

    @property
    @abstractmethod
    def around_regex(self) -> Pattern:
        raise NotImplementedError

    @property
    @abstractmethod
    def equal_regex(self) -> Pattern:
        raise NotImplementedError

    @property
    @abstractmethod
    def year_regex(self) -> Pattern:
        raise NotImplementedError

    @property
    @abstractmethod
    def suffix_after(self) -> Pattern:
        raise NotImplementedError

    @property
    @abstractmethod
    def date_parser(self) -> BaseDateParser:
        raise NotImplementedError

    @property
    @abstractmethod
    def time_parser(self) -> BaseTimeParser:
        raise NotImplementedError


class MinimalMergedParser(DateTimeParser):
    @property
    def parser_type_name(self) -> str:
        return Constants.SYS_DATETIME_MERGED

    def __init__(self, config: MinimalMergedParserConfiguration, options: DateTimeOptions):
        self.__date_min_value = DateTimeFormatUtil.format_date(
            DateUtils.min_value)
        self.__date_time_min_value = DateTimeFormatUtil.format_date_time(
            DateUtils.min_value)
        self.config = config
        self.options = options

    def parse(self, source: ExtractResult, reference: datetime = None) -> Optional[DateTimeParseResult]:
        if not reference:
            reference = datetime.now()

        # Push, save the MOD string
        has_before = False
        has_after = False
        has_since = False
        has_around = False
        has_equal = False
        has_date_after = False
        match_is_after = False

        # "inclusive_mod" means MOD should include the start/end time
        # For example, cases like "on or later than", "earlier than or in" have inclusive modifier
        has_inclusive_mod = False
        mod_str = ''

        result = self.parse_result(source, reference)
        if not result:
            return None

        # Pop, restore the MOD string
        if has_before and result.value:
            result.length += len(mod_str)
            result.start -= 0 if match_is_after else len(mod_str)
            result.text = result.text + mod_str if match_is_after else mod_str + result.text
            val = result.value

            val.mod = self.combine_mod(val.mod, TimeTypeConstants.BEFORE_MOD if not has_inclusive_mod else
                                       TimeTypeConstants.UNTIL_MOD)
            if has_around:
                val.mod = self.combine_mod(TimeTypeConstants.APPROX_MOD, val.mod)
                has_around = False
            result.value = val

        if has_after and result.value:
            result.length += len(mod_str)
            result.start -= len(mod_str)
            result.text = mod_str + result.text
            val = result.value

            val.mod = self.combine_mod(val.mod, TimeTypeConstants.AFTER_MOD if not has_inclusive_mod else
                                       TimeTypeConstants.SINCE_MOD)
            if has_around:
                val.mod = self.combine_mod(TimeTypeConstants.APPROX_MOD, val.mod)
                has_around = False
            result.value = val

        if has_since and result.value:
            result.length += len(mod_str)
            result.start -= len(mod_str)
            result.text = mod_str + result.text
            val = result.value
            val.mod = TimeTypeConstants.SINCE_MOD
            if has_around:
                val.mod = self.combine_mod(TimeTypeConstants.APPROX_MOD, val.mod)
                has_around = False
            result.value = val

        if has_around and result.value:
            result.length += len(mod_str)
            result.start -= len(mod_str)
            result.text = mod_str + result.text
            val = result.value
            val.mod = TimeTypeConstants.APPROX_MOD
            result.value = val

        if has_equal and result.value:
            result.length += len(mod_str)
            result.start -= len(mod_str)
            result.text = mod_str + result.text

        if has_date_after and result.value:
            result.length += len(mod_str)
            result.text = result.text + mod_str
            val = result.value
            val.mod = self.combine_mod(val.mod, TimeTypeConstants.SINCE_MOD)
            result.value = val
            has_since = True

        # For cases like "3 pm or later on monday"
        match = self.config.suffix_after.match(result.text)
        if result.value and (match.start() != 0 if match else match) and \
                result.type == Constants.SYS_DATETIME_DATETIME:
            val = result.value
            val.mod = self.combine_mod(val.mod, TimeTypeConstants.SINCE_MOD)
            result.value = val
            has_since = True

        if self.options & DateTimeOptions.SPLIT_DATE_AND_TIME and result.value and result.value.sub_date_time_entities:
            result.value = self._date_time_resolution_for_split(result)
        else:
            result = self.set_parse_result(
                result, has_before, has_after, has_since)

        return result

    def parse_result(self, source: ExtractResult, reference: datetime):
        if source.type == Constants.SYS_DATETIME_DATE:
            result = self.config.date_parser.parse(source, reference)
        elif source.type == Constants.SYS_DATETIME_TIME:
            result = self.config.time_parser.parse(source, reference)
        else:
            return None

        return result

    @staticmethod
    def combine_mod(original_mod: str, new_mod: str):
        combined_mod = new_mod

        if original_mod:
            combined_mod = f"{new_mod}-{original_mod}"

        return combined_mod

    def set_parse_result(self, slot: DateTimeParseResult, has_before: bool, has_after: bool, has_since: bool)\
            -> DateTimeParseResult:
        slot.value = self._date_time_resolution(
            slot, has_before, has_after, has_since)
        slot.type = f'{self.parser_type_name}.' \
                    f'{self._determine_date_time_types(slot.type, has_before, has_after, has_since)}'
        return slot

    def _get_parse_result(self, extractor_result: Extractor, reference: datetime) -> DateTimeParseResult:
        extractor_type = extractor_result.type
        if extractor_type == Constants.SYS_DATETIME_DATE:
            result = self.config.date_parser.parse(extractor_result, reference)
            return result
        elif extractor_type == Constants.SYS_DATETIME_TIME:
            return self.config.time_parser.parse(extractor_result, reference)
        else:
            return None

    def _determine_date_time_types(self, dtype: str, has_before: bool, has_after: bool, has_since: bool) -> str:
        if self.options & DateTimeOptions.SPLIT_DATE_AND_TIME:
            if dtype == Constants.SYS_DATETIME_DATETIME:
                return Constants.SYS_DATETIME_TIME
        else:
            if has_before or has_after or has_since:
                if dtype == Constants.SYS_DATETIME_DATE:
                    return Constants.SYS_DATETIME_DATEPERIOD

                if dtype == Constants.SYS_DATETIME_TIME:
                    return Constants.SYS_DATETIME_TIMEPERIOD

                if dtype == Constants.SYS_DATETIME_DATETIME:
                    return Constants.SYS_DATETIME_DATETIMEPERIOD
        return dtype

    def _determine_source_entity_type(self, source_type: str, new_type: str, has_mod: bool) -> Optional[str]:
        if not has_mod:
            return None

        if new_type != source_type:
            return Constants.SYS_DATETIME_DATETIMEPOINT

        if new_type == Constants.SYS_DATETIME_DATEPERIOD:
            return Constants.SYS_DATETIME_DATETIMEPERIOD

    def _date_time_resolution_for_split(self, slot: DateTimeParseResult) -> List[DateTimeParseResult]:
        results = []
        if slot.value.sub_date_time_entities:
            sub_entities = slot.value.sub_date_time_entities

            for sub_entity in sub_entities:
                result = sub_entity
                result.start += slot.start
                results += self._date_time_resolution_for_split(result)
        else:
            slot.value = self._date_time_resolution(slot, False, False, False)
            slot.type = f'{self.parser_type_name}.{self._determine_date_time_types(slot.type, False, False, False)}'
            results.append(slot)

        return results

    def _date_time_resolution(self, slot: DateTimeParseResult, has_before, has_after, has_since) ->\
            List[Dict[str, str]]:
        if not slot:
            return None

        result = dict()
        resolutions = []

        dtype = slot.type
        output_type = self._determine_date_time_types(dtype, has_before, has_after, has_since)
        source_entity = self._determine_source_entity_type(dtype, output_type, has_before or has_after or has_since)

        timex = slot.timex_str

        value = slot.value
        if not value:
            return None

        is_lunar = value.is_lunar
        mod = value.mod
        comment = value.comment

        self._add_resolution_fields_any(result, Constants.TIMEX_KEY, timex)
        self._add_resolution_fields_any(result, Constants.COMMENT_KEY, comment)
        self._add_resolution_fields_any(result, Constants.MOD_KEY, mod)
        self._add_resolution_fields_any(result, Constants.TYPE_KEY, output_type)
        self._add_resolution_fields_any(
            result, Constants.IS_LUNAR_KEY, str(is_lunar).lower() if is_lunar else '')

        future_resolution = value.future_resolution
        past_resolution = value.past_resolution

        future = self._generate_from_resolution(dtype, future_resolution, mod)
        past = self._generate_from_resolution(dtype, past_resolution, mod)

        future_values = sorted(future.values())
        past_values = sorted(past.values())
        intersect_values = [i for i, j in zip(
            future_values, past_values) if i == j]

        if len(intersect_values) == len(past_values) and len(intersect_values) == len(future_values):
            if past_values:
                self._add_resolution_fields_any(
                    result, Constants.RESOLVE_KEY, past)
        else:
            if past_values:
                self._add_resolution_fields_any(
                    result, Constants.RESOLVE_TO_PAST_KEY, past)
            if future_values:
                self._add_resolution_fields_any(
                    result, Constants.RESOLVE_TO_FUTURE_KEY, future)

        if comment == Constants.AM_PM_GROUP_NAME:
            if Constants.RESOLVE_KEY in result:
                self._resolve_ampm(result, Constants.RESOLVE_KEY)
            else:
                self._resolve_ampm(result, Constants.RESOLVE_TO_PAST_KEY)
                self._resolve_ampm(result, Constants.RESOLVE_TO_FUTURE_KEY)

        if TimexUtil._has_double_timex(comment):
            TimexUtil._process_double_timex(result, Constants.RESOLVE_TO_FUTURE_KEY, Constants.RESOLVE_TO_PAST_KEY, timex)

        for value in result.values():
            if isinstance(value, dict):
                new_values = {}
                self._add_resolution_fields(
                    new_values, Constants.TIMEX_KEY, timex)
                self._add_resolution_fields(new_values, Constants.MOD_KEY, mod)

                self._add_resolution_fields(new_values, Constants.TYPE_KEY, output_type)
                self._add_resolution_fields(new_values, Constants.IS_LUNAR_KEY,
                                            str(is_lunar).lower() if is_lunar else '')
                self._add_resolution_fields(new_values, Constants.SOURCE_TYPE, source_entity)

                for inner_key in value:
                    new_values[inner_key] = value[inner_key]

                resolutions.append(new_values)

        if not past and not future:
            o = {}
            o['timex'] = timex
            o['type'] = output_type
            o['value'] = 'not resolved'
            resolutions.append(o)

        return {'values': resolutions}

    def _add_resolution_fields_any(self, dic: Dict[str, str], key: str, value: object):
        if isinstance(value, str):
            if value:
                dic[key] = value
        else:
            dic[key] = value

    def _add_resolution_fields(self, dic: [str, str], key: str, value: str):
        if value:
            dic[key] = value

    def _generate_from_resolution(self, dtype: str, resolution: Dict[str, str], mod: str) -> Dict[str, str]:
        result = {}

        if dtype == Constants.SYS_DATETIME_TIME:
            self.__add_single_date_time_to_resolution(
                resolution, TimeTypeConstants.TIME, mod, result)
        elif dtype == Constants.SYS_DATETIME_DATE:
            self.__add_single_date_time_to_resolution(
                resolution, TimeTypeConstants.DATE, mod, result)

        return result

    def __add_single_date_time_to_resolution(self, resolutions: Dict[str, str], dtype: str,
                                             mod: str, result: Dict[str, str]):
        key = TimeTypeConstants.VALUE
        value = resolutions[dtype]
        if not value or value.startswith(self.__date_min_value):
            return

        if mod:
            if mod.startswith(TimeTypeConstants.BEFORE_MOD):
                key = TimeTypeConstants.END
            elif mod.startswith(TimeTypeConstants.AFTER_MOD):
                key = TimeTypeConstants.START
            elif mod.startswith(TimeTypeConstants.SINCE_MOD):
                key = TimeTypeConstants.START
            elif mod.startswith(TimeTypeConstants.UNTIL_MOD):
                key = TimeTypeConstants.END

        result[key] = value

    def __add_period_to_resolution(self, resolutions: Dict[str, str], start_type: str,
                                   end_type: str, mod: str, result: Dict[str, str]):
        start = resolutions.get(start_type, None)
        end = resolutions.get(end_type, None)
        if mod:
            if mod.startswith(TimeTypeConstants.BEFORE_MOD):
                if mod.endswith(TimeTypeConstants.LATE_MOD):
                    result[TimeTypeConstants.END] = end
                else:
                    result[TimeTypeConstants.END] = start
                return
            if mod.startswith(TimeTypeConstants.AFTER_MOD):
                if mod.endswith(TimeTypeConstants.EARLY_MOD):
                    result[TimeTypeConstants.START] = start
                else:
                    result[TimeTypeConstants.START] = end
                return
            if mod == TimeTypeConstants.SINCE_MOD:
                result[TimeTypeConstants.START] = start
                return

        if not (start and end):
            return

        if start.startswith(Constants.INVALID_DATE_STRING) or end.startswith(Constants.INVALID_DATE_STRING):
            return

        result[TimeTypeConstants.START] = start
        result[TimeTypeConstants.END] = end

    def _resolve_ampm(self, values_map: Dict[str, str], key_name: str):
        if key_name not in values_map:
            return
        resolution = values_map[key_name]
        if Constants.TIMEX_KEY not in values_map:
            return
        timex = values_map[Constants.TIMEX_KEY]
        values_map.pop(key_name, None)
        values_map[key_name + Constants.AM_GROUP_NAME] = resolution

        resolution_pm = {}
        if values_map[Constants.TYPE_KEY] == Constants.SYS_DATETIME_TIME:
            resolution_pm[TimeTypeConstants.VALUE] = DateTimeFormatUtil.to_pm(
                resolution[TimeTypeConstants.VALUE])
            resolution_pm[Constants.TIMEX_KEY] = DateTimeFormatUtil.to_pm(timex)
        elif values_map[Constants.TYPE_KEY] == Constants.SYS_DATETIME_DATETIME:
            split_value = resolution[TimeTypeConstants.VALUE].split(' ')
            resolution_pm[
                TimeTypeConstants.VALUE] = f'{split_value[0]} {DateTimeFormatUtil.to_pm(split_value[1])}'
            resolution_pm[Constants.TIMEX_KEY] = DateTimeFormatUtil.all_str_to_pm(timex)
        elif values_map[Constants.TYPE_KEY] == Constants.SYS_DATETIME_TIMEPERIOD:
            if TimeTypeConstants.START in resolution:
                resolution_pm[TimeTypeConstants.START] = DateTimeFormatUtil.to_pm(
                    resolution[TimeTypeConstants.START])
            if TimeTypeConstants.END in resolution:
                resolution_pm[TimeTypeConstants.END] = DateTimeFormatUtil.to_pm(
                    resolution[TimeTypeConstants.END])
            resolution_pm[Constants.TIMEX_KEY] = DateTimeFormatUtil.all_str_to_pm(timex)
        elif values_map[Constants.TYPE_KEY] == Constants.SYS_DATETIME_DATETIMEPERIOD:
            if TimeTypeConstants.START in resolution:
                split_value = resolution[TimeTypeConstants.START].split(' ')
                resolution_pm[
                    TimeTypeConstants.START] = f'{split_value[0]} {DateTimeFormatUtil.to_pm(split_value[1])}'
            if TimeTypeConstants.END in resolution:
                split_value = resolution[TimeTypeConstants.END].split(' ')
                resolution_pm[
                    TimeTypeConstants.END] = f'{split_value[0]} {DateTimeFormatUtil.to_pm(split_value[1])}'
            resolution_pm[Constants.TIMEX_KEY] = DateTimeFormatUtil.all_str_to_pm(timex)
        values_map[key_name + Constants.PM_GROUP_NAME] = resolution_pm