from abc import abstractmethod
from typing import List, Optional, Pattern, Dict
from datetime import datetime
from collections import namedtuple

from recognizers_text.extractor import ExtractResult
from recognizers_text.meta_data import MetaData
from recognizers_date_time.date_time.constants import Constants
from recognizers_date_time.date_time.extractors import DateTimeExtractor
from recognizers_date_time.date_time.parsers import DateTimeParser, DateTimeParseResult
from recognizers_date_time.date_time.CJK.base_configs import CJKCommonDateTimeParserConfiguration
from recognizers_date_time.date_time.utilities import DateTimeOptions, RegExpUtility, ExtractResultExtension, \
    DateTimeOptionsConfiguration, DateTimeResolutionResult, MergedParserUtil

MatchedIndex = namedtuple('MatchedIndex', ['matched', 'index'])


class CJKMergedExtractorConfiguration(DateTimeOptionsConfiguration):

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
    def duration_extractor(self) -> DateTimeExtractor:
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
    def after_regex(self) -> Pattern:
        raise NotImplementedError

    @property
    @abstractmethod
    def before_regex(self) -> Pattern:
        raise NotImplementedError

    @property
    @abstractmethod
    def unspecified_date_period_regex(self) -> Pattern:
        raise NotImplementedError

    @property
    @abstractmethod
    def since_suffix_regex(self) -> Pattern:
        raise NotImplementedError

    @property
    @abstractmethod
    def since_prefix_regex(self) -> Pattern:
        raise NotImplementedError

    @property
    @abstractmethod
    def around_suffix_regex(self) -> Pattern:
        raise NotImplementedError

    @property
    @abstractmethod
    def around_prefix_regex(self) -> Pattern:
        raise NotImplementedError

    @property
    @abstractmethod
    def until_regex(self) -> Pattern:
        raise NotImplementedError

    @property
    @abstractmethod
    def equal_regex(self) -> Pattern:
        raise NotImplementedError

    @property
    @abstractmethod
    def potential_ambiguous_range_regex(self) -> Pattern:
        raise NotImplementedError

    @property
    @abstractmethod
    def ambiguous_range_modifier_prefix(self) -> Pattern:
        raise NotImplementedError

    @property
    @abstractmethod
    def ambiguity_filters_dict(self) -> Dict[Pattern, Pattern]:
        raise NotImplementedError


class BaseCJKMergedExtractor(DateTimeExtractor):
    @property
    def extractor_type_name(self) -> str:
        return Constants.SYS_DATETIME_MERGED

    def __init__(self, config: CJKMergedExtractorConfiguration, options: DateTimeOptions):
        self.config = config

    def extract(self, source: str, reference: datetime = None) -> List[ExtractResult]:
        if reference is None:
            reference = datetime.now()

        result = self.config.date_extractor.extract(source, reference)

        # The order is important, since there can be conflicts in merging
        result = self.add_to(
            result, self.config.time_extractor.extract(source, reference))
        result = self.add_to(
            result, self.config.duration_extractor.extract(source, reference))
        result = self.add_to(
            result, self.config.date_period_extractor.extract(source, reference))
        result = self.add_to(
            result, self.config.date_time_extractor.extract(source, reference))
        result = self.add_to(
            result, self.config.time_period_extractor.extract(source, reference))
        result = self.add_to(
            result, self.config.date_time_period_extractor.extract(source, reference))
        result = self.add_to(
            result, self.config.set_extractor.extract(source, reference))
        result = self.add_to(
            result, self.config.holiday_extractor.extract(source, reference))

        result = self.filter_unspecific_date_period(result)

        result = ExtractResultExtension.filter_ambiguity(result, source, self.config.ambiguity_filters_dict)

        result = self.add_mod(result, source)

        result = sorted(result, key=lambda x: x.start)

        return result

    def filter_unspecific_date_period(self, extract_results: List[ExtractResult]) -> List[ExtractResult]:
        for extract_result in extract_results:
            if self.config.unspecified_date_period_regex.search(extract_result.text) is not None:
                extract_results.remove(extract_result)

        return extract_results

    def add_mod(self, extract_results: List[ExtractResult], source: str) -> List[ExtractResult]:
        last_end = 0
        for extract_result in extract_results:
            before_str = source[last_end:extract_result.start].strip()
            after_str = source[extract_result.start + extract_result.length:].strip()

            match = RegExpUtility.match_begin(self.config.before_regex, after_str, True)
            if match:
                mod_len = match.index + match.length
                extract_result.length += mod_len
                extract_result.text = source[extract_result.start:extract_result.length + 1]

                extract_result.meta_data = self.assign_mod_metadata(extract_result.meta_data)

            match = RegExpUtility.match_begin(self.config.after_regex, after_str, True)
            if match:
                mod_len = match.index + match.length
                extract_result.length += mod_len
                extract_result.text = source[extract_result.start:extract_result.length + 1]

                extract_result.meta_data = self.assign_mod_metadata(extract_result.meta_data)

            match = RegExpUtility.match_begin(self.config.until_regex, before_str, True)
            if match:
                mod_len = len(before_str) - match.index
                extract_result.length += mod_len
                extract_result.start -= mod_len
                extract_result.text = source[extract_result.start:extract_result.length]

                extract_result.meta_data = self.assign_mod_metadata(extract_result.meta_data)

            match = RegExpUtility.match_begin(self.config.until_regex, after_str, True)
            if match:
                mod_len = len(after_str) - match.index
                extract_result.length += mod_len
                extract_result.start -= mod_len
                extract_result.text = source[extract_result.start:extract_result.length]

                extract_result.meta_data = self.assign_mod_metadata(extract_result.meta_data)

            match = RegExpUtility.match_begin(self.config.since_prefix_regex, before_str, True)
            if match and self.ambiguous_range_checker(before_str, source, extract_result):
                mod_len = len(before_str) + match.index
                extract_result.length += mod_len
                extract_result.start -= mod_len
                extract_result.text = source[extract_result.start:extract_result.length]

                extract_result.meta_data = self.assign_mod_metadata(extract_result.meta_data)

            match = RegExpUtility.match_begin(self.config.since_prefix_regex, after_str, True)
            if match and self.ambiguous_range_checker(after_str, source, extract_result):
                mod_len = len(after_str) + match.index
                extract_result.length += mod_len
                extract_result.start -= mod_len
                extract_result.text = source[extract_result.start:extract_result.length]

                extract_result.meta_data = self.assign_mod_metadata(extract_result.meta_data)

            match = RegExpUtility.match_begin(self.config.around_suffix_regex, before_str, True)
            if match:
                mod_len = len(before_str) + match.index
                extract_result.length += mod_len
                extract_result.start -= mod_len
                extract_result.text = source[extract_result.start:extract_result.length]

                extract_result.meta_data = self.assign_mod_metadata(extract_result.meta_data)

            match = RegExpUtility.match_begin(self.config.around_suffix_regex, after_str, True)
            if match:
                mod_len = len(after_str) + match.index
                extract_result.length += mod_len
                extract_result.start -= mod_len
                extract_result.text = source[extract_result.start:extract_result.length]

                extract_result.meta_data = self.assign_mod_metadata(extract_result.meta_data)

            match = RegExpUtility.match_begin(self.config.equal_regex, before_str, True)
            if match:
                mod_len = len(before_str) + match.index
                extract_result.length += mod_len
                extract_result.start -= mod_len
                extract_result.text = source[extract_result.start:extract_result.length]

                extract_result.meta_data = self.assign_mod_metadata(extract_result.meta_data)

        return extract_results

    def add_to(self, destinations: List[ExtractResult], source: List[ExtractResult]) -> List[ExtractResult]:
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

    # Avoid adding mod for ambiguity cases, such as "从" in "从 ... 到 ..." should not add mod
    # TODO: Revise PotentialAmbiguousRangeRegex to support cases like "从2015年起，哪所大学需要的分数在80到90之间"
    def ambiguous_range_checker(self, before_str: str, text: str, er: ExtractResult) -> bool:
        if RegExpUtility.match_end(self.config.ambiguous_range_modifier_prefix, text, True):
            matches = RegExpUtility.get_matches(self.config.potential_ambiguous_range_regex, text)
            if any(m.index < er.start + er.length and m.index + m.length > er.start for m in matches):
                return False
        return True

    def assign_mod_metadata(self, metadata: MetaData) -> MetaData:
        if not metadata:
            metadata = MetaData()
        metadata.has_mod = True
        return metadata


class CJKMergedParserConfiguration(CJKCommonDateTimeParserConfiguration):
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
    def since_prefix_regex(self) -> Pattern:
        raise NotImplementedError

    @property
    @abstractmethod
    def since_suffix_regex(self) -> Pattern:
        raise NotImplementedError

    @property
    @abstractmethod
    def until_regex(self) -> Pattern:
        raise NotImplementedError

    @property
    @abstractmethod
    def equal_regex(self) -> Pattern:
        raise NotImplementedError

    @property
    @abstractmethod
    def around_prefix_regex(self) -> Pattern:
        raise NotImplementedError

    @property
    @abstractmethod
    def around_suffix_regex(self) -> Pattern:
        raise NotImplementedError


class BaseCJKMergedParser(DateTimeParser):
    @property
    def parser_type_name(self) -> str:
        return Constants.SYS_DATETIME_MERGED

    def __init__(self, config: CJKMergedParserConfiguration, options):
        self.config = config
        self.options = options

    def parse(self, er: ExtractResult, reference: datetime = None) -> Optional[DateTimeParseResult]:
        if not reference:
            reference = datetime.now()

        #  push, save the MOD string
        has_inclusive_modifier = False
        has_before = False
        has_after = False
        has_until = False
        has_since = False
        has_equal = False
        has_around = False
        mod_str = ""
        mod_str_prefix = ""
        mod_str_suffix = ""

        if er.meta_data:
            if er.meta_data.has_mod:
                before_match = RegExpUtility.match_end(self.config.before_regex, er.text, True)
                after_match = RegExpUtility.match_end(self.config.after_regex, er.text, True)
                until_match = RegExpUtility.match_begin(self.config.until_regex, er.text, True)
                since_match_prefix = RegExpUtility.match_begin(self.config.since_prefix_regex, er.text, True)
                since_match_suffix = RegExpUtility.match_end(self.config.since_suffix_regex, er.text, True)
                equal_match = RegExpUtility.match_begin(self.config.equal_regex, er.text, True)
                around_match_prefix = RegExpUtility.match_begin(self.config.around_prefix_regex, er.text, True)
                around_match_suffix = RegExpUtility.match_end(self.config.around_suffix_regex, er.text, True)

                if before_match and not MergedParserUtil.is_duration_with_ago_and_later(er):
                    has_before = True
                    er.length -= before_match.length
                    if er.length > len(er.text):
                        er.text = er.text[0:er.length]
                    mod_str = before_match.value
                    if before_match.get_group(Constants.INCLUDE_GROUP_NAME):
                        has_inclusive_modifier = True

                elif after_match and not since_match_suffix and not MergedParserUtil.is_duration_with_ago_and_later(er):
                    has_after = True
                    er.length -= after_match.length
                    if er.length > len(er.text):
                        er.text = er.text[0:er.length]
                    mod_str = after_match.value
                    if after_match.get_group(Constants.INCLUDE_GROUP_NAME):
                        has_inclusive_modifier = True

                elif until_match:
                    has_until = True
                    er.start += until_match.length
                    er.length -= until_match.length
                    er.text = er.text[until_match.length]
                    mod_str = until_match.group()

                elif equal_match:
                    has_equal = True
                    er.start += equal_match.length
                    er.length -= equal_match.length
                    er.text = er.text[equal_match.length]
                    mod_str = equal_match.group()

                else:
                    if since_match_prefix:
                        has_since = True
                        er.start += since_match_prefix.length
                        er.length -= since_match_prefix.length
                        er.text = er.text[since_match_prefix.length]
                        mod_str = since_match_prefix.group()

                    if since_match_suffix:
                        has_since = True
                        er.length -= since_match_suffix.length
                        er.text = er.text[since_match_suffix.start:]
                        mod_str = since_match_suffix.group()

                    if around_match_prefix:
                        has_around = True
                        er.start += around_match_prefix.length
                        er.length -= around_match_prefix.length
                        er.text = er.text[around_match_prefix.length]
                        mod_str = around_match_prefix.group()

                    if around_match_suffix:
                        has_around = True
                        er.length -= around_match_suffix.length
                        er.text = er.text[around_match_suffix.start:]
                        mod_str = around_match_suffix.group()

        #  Parse extracted datetime mention
        pr = self.parse_result(er, reference)
        if not pr:
            return None

        # pop, restore the MOD string
        if has_before:
            pr.length += len(mod_str)
            pr.text = pr.text + mod_str
            val: DateTimeResolutionResult = pr.value

            if has_inclusive_modifier:
                val.mod = MergedParserUtil.combined_mod(val.mod, Constants.BEFORE_MOD)
            else:
                val.mod = MergedParserUtil.combined_mod(val.mod, Constants.UNTIL_MOD)
            pr.value = val

        if has_after:
            pr.length += len(mod_str)
            pr.text = pr.text + mod_str
            val: DateTimeResolutionResult = pr.value

            if has_inclusive_modifier:
                val.mod = MergedParserUtil.combined_mod(val.mod, Constants.AFTER_MOD)
            else:
                val.mod = MergedParserUtil.combined_mod(val.mod, Constants.SINCE_MOD)
            pr.value = val

        if has_until:
            pr.length += len(mod_str)
            pr.start -= len(mod_str)
            pr.text = mod_str + pr.text
            val: DateTimeResolutionResult = pr.value
            val.mod = Constants.BEFORE_MOD
            pr.value = val
            has_before = True

        if has_since:
            pr.length += len(mod_str_prefix) + len(mod_str_suffix)
            pr.start -= len(mod_str_prefix)
            pr.text = mod_str_prefix + pr.text + mod_str_suffix
            val: DateTimeResolutionResult = pr.value
            val.mod = Constants.SINCE_MOD
            pr.value = val

        if has_equal:
            pr.length += len(mod_str)
            pr.start -= len(mod_str)
            pr.text = mod_str + pr.text

        if has_around:
            pr.length += len(mod_str_prefix) + len(mod_str_suffix)
            pr.start -= len(mod_str_prefix)
            pr.text = mod_str_prefix + pr.text + mod_str_suffix
            val: DateTimeResolutionResult = pr.value
            val.mod = Constants.APPROX_MOD
            pr.value = val

        has_range_changing_mod = False
        if has_before or has_after or has_since:
            has_range_changing_mod = True

        if not pr.value:
            pr.value = DateTimeResolutionResult
            pr.value.has_range_changing_mod = has_range_changing_mod

        pr = MergedParserUtil.set_parse_result(pr, has_range_changing_mod, self.config)

        return pr

    def parse_result(self, source: ExtractResult, reference: datetime) -> DateTimeParseResult:
        result = None

        if source.type == Constants.SYS_DATETIME_DATE:
            if source.meta_data and source.meta_data.is_holiday:
                result = self.config.holiday_parser.parse(source, reference)
            else:
                result = self.config.date_parser.parse(source, reference)
        elif source.type == Constants.SYS_DATETIME_TIME:
            result = self.config.time_parser.parse(source, reference)
        elif source.type == Constants.SYS_DATETIME_DATETIME:
            result = self.config.date_time_parser.parse(source, reference)
        elif source.type == Constants.SYS_DATETIME_DATEPERIOD:
            result = self.config.date_period_parser.parse(source, reference)
        elif source.type == Constants.SYS_DATETIME_TIMEPERIOD:
            result = self.config.time_period_parser.parse(source, reference)
        elif source.type == Constants.SYS_DATETIME_DATETIMEPERIOD:
            result = self.config.date_time_period_parser.parse(source, reference)
        elif source.type == Constants.SYS_DATETIME_DURATION:
            result = self.config.duration_parser.parse(source, reference)
        elif source.type == Constants.SYS_DATETIME_SET:
            result = self.config.set_parser.parse(source, reference)
        elif source.type == Constants.SYS_DATETIME_DATETIMEALT:
            result = self.config.date_time_alt_parser.parse(source, reference)
        elif source.type == Constants.SYS_DATETIME_TIMEZONE:
            if self.config.options != 0 and DateTimeOptions.ENABLE_PREVIEW != 0:
                result = self.config.time_zone_parser.parse(source, reference)
        else:
            return None

        return result
