#  Copyright (c) Microsoft Corporation. All rights reserved.
#  Licensed under the MIT License.

from typing import List, Pattern

from recognizers_text.utilities import RegExpUtility
from ...resources.italian_date_time import ItalianDateTime
from ..base_time import TimeExtractorConfiguration
from ..utilities import DateTimeOptions
from ..base_timezone import BaseTimeZoneExtractor
from .timezone_extractor_config import ItalianTimeZoneExtractorConfiguration


class ItalianTimeExtractorConfiguration(TimeExtractorConfiguration):
    @property
    def time_zone_extractor(self):
        return self._time_zone_extractor

    @property
    def time_regex_list(self) -> List[Pattern]:
        return self._time_regex_list

    @property
    def at_regex(self) -> Pattern:
        return self._at_regex

    @property
    def ish_regex(self) -> Pattern:
        return self._ish_regex

    @property
    def time_before_after_regex(self) -> Pattern:
        return self._time_before_after_regex

    def __init__(self):
        super().__init__()
        self._time_regex_list: List[Pattern] = ItalianTimeExtractorConfiguration.get_time_regex_list(
        )
        self._at_regex: Pattern = RegExpUtility.get_safe_reg_exp(
            ItalianDateTime.AtRegex)
        self._ish_regex: Pattern = RegExpUtility.get_safe_reg_exp(
            ItalianDateTime.IshRegex)
        self._time_before_after_regex: Pattern = RegExpUtility.get_safe_reg_exp(
            ItalianDateTime.TimeBeforeAfterRegex)
        self._options = DateTimeOptions.NONE
        self._time_zone_extractor = BaseTimeZoneExtractor(
            ItalianTimeZoneExtractorConfiguration())

    @staticmethod
    def get_time_regex_list() -> List[Pattern]:
        return [
            RegExpUtility.get_safe_reg_exp(ItalianDateTime.TimeRegex1),
            RegExpUtility.get_safe_reg_exp(ItalianDateTime.TimeRegex2),
            RegExpUtility.get_safe_reg_exp(ItalianDateTime.TimeRegex3),
            RegExpUtility.get_safe_reg_exp(ItalianDateTime.TimeRegex4),
            RegExpUtility.get_safe_reg_exp(ItalianDateTime.TimeRegex5),
            RegExpUtility.get_safe_reg_exp(ItalianDateTime.TimeRegex6),
            RegExpUtility.get_safe_reg_exp(ItalianDateTime.TimeRegex7),
            RegExpUtility.get_safe_reg_exp(ItalianDateTime.TimeRegex8),
            RegExpUtility.get_safe_reg_exp(ItalianDateTime.TimeRegex9),
            RegExpUtility.get_safe_reg_exp(ItalianDateTime.TimeRegex10),
            RegExpUtility.get_safe_reg_exp(ItalianDateTime.ConnectNumRegex)
        ]
