from typing import List
from datetime import datetime
import regex

from recognizers_text import ExtractResult

from ..constants import Constants
from ..utilities import Token, merge_all_tokens
from ..base_datetime import BaseDateTimeExtractor
from .datetime_extractor_config import ChineseDateTimeExtractorConfiguration

class ChineseDateTimeExtractor(BaseDateTimeExtractor):
    def __init__(self):
        super().__init__(ChineseDateTimeExtractorConfiguration())

    def extract(self, source: str, reference: datetime = None) -> List[ExtractResult]:

        if reference is None:
            reference = datetime.now()

        tokens: List[Token] = list()
        tokens.extend(self.merge_date_and_time(source, reference))
        tokens.extend(self.basic_regex_match(source))
        tokens.extend(self.time_of_today(source, reference))

        result = merge_all_tokens(tokens, source, self.extractor_type_name)
        return result

    def merge_date_and_time(self, source: str, reference: datetime) -> List[Token]:
        tokens: List[Token] = list()
        ers: List[ExtractResult] = self.config.date_point_extractor.extract(source, reference)

        if len(ers) < 1:
            return tokens

        ers.extend(self.config.time_point_extractor.extract(source, reference))

        if len(ers) < 2:
            return tokens

        ers = sorted(ers, key=lambda x: x.start)
        i = 0

        while i < len(ers)-1:
            j = i+1

            while j < len(ers) and ers[i].overlap(ers[j]):
                j += 1

            if j >= len(ers):
                break

            if ers[i].type is Constants.SYS_DATETIME_DATE and ers[j].type is Constants.SYS_DATETIME_TIME:
                middle_begin = ers[i].start + ers[i].length
                middle_end = ers[j].start

                if middle_begin > middle_end:
                    continue

                middle = source[middle_begin:middle_end].strip().lower()

                if self.config.is_connector_token(middle):
                    begin = ers[i].start
                    end = ers[j].start + ers[j].length
                    tokens.append(Token(begin, end))
                i = j + 1
                continue
            i = j

        return tokens

    def time_of_today(self, source: str, reference: datetime) -> List[Token]:
        tokens: List[Token] = list()
        ers = self.config.time_point_extractor.extract(source, reference)

        for er in ers:
            before = source[:er.start]
            inner_match = regex.search(self.config.night_regex, er.text)

            if inner_match is not None and inner_match.start() == 0:
                before = source[:er.start + len(inner_match.group())]

            if not before:
                continue

            match = regex.search(self.config.time_of_today_before_regex, before)
            if match is not None and not before[match.end():].strip():
                begin = match.start()
                end = er.start + er.length
                tokens.append(Token(begin, end))

        return tokens
