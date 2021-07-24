#  Copyright (c) Microsoft Corporation. All rights reserved.
#  Licensed under the MIT License.

from typing import List
from datetime import datetime
import regex

from recognizers_text import RegExpUtility, ExtractResult
from recognizers_number_with_unit import NumberWithUnitExtractor
from ...resources.chinese_date_time import ChineseDateTime
from ..constants import Constants
from .base_date_time_extractor import ChineseBaseDateTimeExtractor
from .duration_extractor_config import ChineseDurationExtractorConfiguration


class ChineseDurationExtractor(ChineseBaseDateTimeExtractor):
    @property
    def extractor_type_name(self) -> str:
        return Constants.SYS_DATETIME_DURATION

    def __init__(self):
        super().__init__(None)
        self.extractor = NumberWithUnitExtractor(
            ChineseDurationExtractorConfiguration())
        self.year_regex = RegExpUtility.get_safe_reg_exp(
            ChineseDateTime.DurationYearRegex)
        self.half_suffix_regex = RegExpUtility.get_safe_reg_exp(
            ChineseDateTime.DurationHalfSuffixRegex)

    def extract(self, source: str, reference: datetime = None) -> List[ExtractResult]:

        if reference is None:
            reference = datetime.now()

        result: List[ExtractResult] = list()
        for er_result in self.extractor.extract(source):
            # filter
            if regex.search(self.year_regex, er_result.text):
                continue

            # match suffix
            suffix = source[er_result.start + er_result.length:]
            result.append(er_result)

        return result
