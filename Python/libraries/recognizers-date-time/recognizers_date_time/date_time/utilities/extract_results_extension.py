from typing import List, Dict, Pattern

from regex import regex

from recognizers_text.extractor import ExtractResult
from recognizers_text.utilities import RegExpUtility


class ExtractResultExtension:
    @staticmethod
    def is_overlap(er1: ExtractResult, er2: ExtractResult) -> bool:
        return not (er1.start >= er2.start + er2.length) and not (er2.start >= er1.start + er1.length)

    @staticmethod
    def is_cover(er1: ExtractResult, er2: ExtractResult) -> bool:
        return (er2.start < er1.start and er2.start + er2.length >= er1.start + er1.length) or \
               (er2.start <= er1.start and er2.start+ er2.length > er1.start + er1.length)

    @staticmethod
    def merge_all_tokens(results: List[ExtractResult]) -> List[ExtractResult]:

        results = sorted(filter(None, results), key=lambda x: x.start)
        merged_results: List[ExtractResult] = list()
        for result in results:
            should_add = True

            for index, m_token in enumerate(merged_results):
                if not should_add:
                    break

                # It is included in one of the current results
                if result.start >= m_token.start and result.end <= m_token.end:
                    should_add = False

                # If it contains overlaps
                if m_token.start < result.start < m_token.end:
                    should_add = False

                #  It includes one of the results and should replace the included one
                if result.start <= m_token.start and result.end >= m_token.end:
                    should_add = False
                    merged_results[index] = result

            if should_add:
                merged_results.append(result)

        return merged_results

    @staticmethod
    def filter_ambiguity(extract_results: List[ExtractResult], text: str,
                         ambiguity_filters_dict: Dict[Pattern, Pattern]) -> List[ExtractResult]:
        if ambiguity_filters_dict:
            for key, value in ambiguity_filters_dict.items():
                for er in reversed(extract_results):

                    if regex.match(key, er.text):
                        matches = RegExpUtility.get_matches(value, text)

                        if any(text.index(m) < er.start + er.length and text.index(m) + len(m) > er.start
                               for m in matches):
                            extract_results.remove(er)

        return extract_results



