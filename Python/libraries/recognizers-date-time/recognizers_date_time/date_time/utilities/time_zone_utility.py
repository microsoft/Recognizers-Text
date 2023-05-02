from typing import List

from recognizers_text.matcher.number_with_unit_tokenizer import NumberWithUnitTokenizer
from recognizers_text.matcher.match_strategy import MatchStrategy
from recognizers_text.extractor import ExtractResult
from recognizers_date_time.date_time.constants import Constants
from recognizers_date_time.date_time.utilities.datetime_options import DateTimeOptions
from recognizers_text.matcher.string_matcher import StringMatcher


class TimeZoneUtility:

    @staticmethod
    def merge_time_zones(original_extract_results: [ExtractResult], time_zone_ers: [ExtractResult], text: str):

        for extract_result in original_extract_results:
            for time_zone_er in time_zone_ers:

                begin = extract_result.start + extract_result.length
                end = time_zone_er.start

                if begin < end:
                    gap_text = text[begin: begin + (end - begin)]

                    if gap_text.isspace() or gap_text is None:
                        new_length = time_zone_er.start + time_zone_er.length - extract_result.start

                        extract_result.text = text[extract_result.start:new_length]
                        extract_result.length = new_length
                        extract_result.data = {Constants.SYS_DATETIME_TIMEZONE, time_zone_er}

                if extract_result.overlap(time_zone_er):
                    extract_result.data = {Constants.SYS_DATETIME_TIMEZONE, time_zone_er}

        return original_extract_results

    @staticmethod
    def should_resolve_time_zone(extract_result: ExtractResult, options):
        enable_preview = (options & DateTimeOptions.ENABLE_PREVIEW) != 0

        if not enable_preview:
            return False

        has_time_zone_data = False

        if isinstance(extract_result.data, {}):
            meta_data = extract_result.data
            if meta_data is not None and Constants.SYS_DATETIME_TIMEZONE in meta_data.keys():
                has_time_zone_data = True

        return has_time_zone_data

    @staticmethod
    def build_matcher_from_lists(*collections: List[str]) -> StringMatcher:
        matcher = StringMatcher(MatchStrategy.TrieTree, NumberWithUnitTokenizer())

        matcher_list = []
        for collection in collections:
            list(map(lambda x: matcher_list.append(x.strip().lower()), collection))

        matcher_list = TimeZoneUtility.distinct(matcher_list)

        matcher.init(matcher_list)

        return matcher

    @staticmethod
    def distinct(list1):

        unique_list = []
        for x in list1:

            if x not in unique_list:
                unique_list.append(x)

        return unique_list