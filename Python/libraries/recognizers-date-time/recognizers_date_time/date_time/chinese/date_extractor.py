from typing import List
from datetime import datetime
import regex
from ...resources.chinese_date_time import ChineseDateTime

from recognizers_text import ExtractResult, RegExpUtility, MetaData
from ..utilities import merge_all_tokens, Token, get_tokens_from_regex
from ..base_date import BaseDateExtractor
from .date_extractor_config import ChineseDateExtractorConfiguration
from .duration_extractor import ChineseDurationExtractor


class ChineseDateExtractor(BaseDateExtractor):
    before_regex = RegExpUtility.get_safe_reg_exp(
        ChineseDateTime.BeforeRegex)
    after_regex = RegExpUtility.get_safe_reg_exp(
        ChineseDateTime.AfterRegex)
    date_time_period_unit_regex = RegExpUtility.get_safe_reg_exp(
        ChineseDateTime.DateTimePeriodUnitRegex)

    def __init__(self):
        super().__init__(ChineseDateExtractorConfiguration())
        self.duration_extractor = ChineseDurationExtractor()

    def extract(self, source: str, reference: datetime = None) -> List[ExtractResult]:
        if reference is None:
            reference = datetime.now()

        tokens = self.basic_regex_match(source)
        tokens.extend(self.implicit_date(source))
        tokens.extend(self.duration_with_ago_and_later(source, reference))

        result = merge_all_tokens(tokens, source, self.extractor_type_name)
        return result

    def duration_with_ago_and_later(self, source: str, reference: datetime) -> List[Token]:
        ret: List[Token] = list()
        duration_er = self.duration_extractor.extract(source, reference)

        for er in duration_er:
            if not regex.search(self.date_time_period_unit_regex, er.text):
                pos = er.start + er.length
                if pos < len(source):
                    suffix = source[pos]
                    before_match = RegExpUtility.get_matches(self.before_regex, suffix)
                    after_match = RegExpUtility.get_matches(self.after_regex, suffix)

                    if (before_match and suffix.startswith(before_match[0])) \
                            or (after_match and suffix.startswith(after_match[0])):
                        meta_data = MetaData()
                        meta_data.is_duration_with_ago_and_later = True
                        ret.append(Token(er.start, pos + 1, meta_data))

        return ret

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
