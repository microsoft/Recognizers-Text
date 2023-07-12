from typing import List, Pattern
import regex

from recognizers_text.extractor import ExtractResult
from recognizers_text.utilities import RegExpUtility
from recognizers_text.matcher.string_matcher import MatchResult
from recognizers_date_time.resources.base_date_time import BaseDateTime


class MatchedIndex:
    def __init__(self, matched: bool, index: int):
        self.matched = matched
        self.index = index


class MatchingUtil:

    invalid_day_number_prefix = RegExpUtility.get_safe_reg_exp(
        BaseDateTime.InvalidDayNumberPrefix)

    @staticmethod
    def is_invalid_day_number_prefix(prefix: str) -> bool:
        return MatchingUtil.invalid_day_number_prefix.search(prefix)

    @staticmethod
    def pre_process_text_remove_superfluous_words(text: str, matcher: Pattern):
        superfluous_word_matches = MatchingUtil.remove_sub_matches(matcher.find(text))

        bias = 0

        for match in superfluous_word_matches:
            text = text[match.start - bias: match.length]

            bias += match.length

        return text, superfluous_word_matches

    @staticmethod
    def post_process_recover_superfluous_words(extract_results: List[ExtractResult], superfluous_word_matches,
                                               origin_text: str):
        for match in superfluous_word_matches:
            for extract_result in extract_results:

                extract_result_end = extract_result.start + extract_result.length
                if extract_result.start < match.start <= extract_result_end:
                    extract_result.length += len(match)

                if match.start <= extract_result.start:
                    extract_result.start += len(match)

        for extract_result in extract_results:
            extract_result.text = origin_text[extract_result.start: extract_result.start + extract_result.length]

        return extract_results

    @staticmethod
    def remove_sub_matches(match_results: List[MatchResult]):
        match_list = list(match_results)

        match_list = (list(filter(lambda item: not any(list(filter(
            lambda ritem: (ritem.start < item.start and ritem.end >= item.end) or (
                ritem.start <= item.start and ritem.end > item.end), match_list))), match_list)))

        return match_list

    @staticmethod
    def get_ago_later_index(source: str, regexp: Pattern, in_suffix) -> MatchedIndex:
        result = MatchedIndex(matched=False, index=-1)
        trimmed_source = source.strip().lower()
        match = RegExpUtility.match_begin(regexp, trimmed_source, True) if in_suffix else\
            RegExpUtility.match_end(regexp, trimmed_source, True)

        if match and match.success:
            result.index = source.lower().find(match.group()) + (match.length if in_suffix else 0)
            result.matched = True

        return result

    @staticmethod
    def contains_ago_later_index(source: str, regexp: Pattern, in_suffix) -> bool:
        return MatchingUtil.get_ago_later_index(source, regexp, in_suffix).matched

    @staticmethod
    def get_term_index(source: str, regexp: Pattern) -> MatchedIndex:
        result = MatchedIndex(matched=False, index=-1)
        referenced_match = regex.search(
            regexp, source.strip().lower().split(' ').pop())

        if referenced_match:
            result = MatchedIndex(matched=True, index=len(
                source) - source.lower().rfind(referenced_match.group()))

        return result

    @staticmethod
    def contains_term_index(source: str, regexp: Pattern) -> bool:
        return MatchingUtil.get_term_index(source, regexp).matched