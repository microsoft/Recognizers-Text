#  Copyright (c) Microsoft Corporation. All rights reserved.
#  Licensed under the MIT License.

from typing import List, Pattern
from recognizers_text.utilities import RegExpUtility
from ...resources.spanish_date_time import SpanishDateTime
from ..base_time import TimeExtractorConfiguration
from ..base_timezone import BaseTimeZoneExtractor
from .timezone_extractor_config import SpanishTimeZoneExtractorConfiguration


class SpanishTimeExtractorConfiguration(TimeExtractorConfiguration):
    @property
    def time_zone_extractor(self):
        return self._time_zone_extractor

    @property
    def options(self):
        return self._options

    @property
    def dmy_date_format(self) -> bool:
        return self._dmy_date_format

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
        self._time_regex_list: List[Pattern] = SpanishTimeExtractorConfiguration.get_time_regex_list(
        )
        self._at_regex: Pattern = RegExpUtility.get_safe_reg_exp(
            SpanishDateTime.AtRegex)
        self._time_before_after_regex: Pattern = RegExpUtility.get_safe_reg_exp(
            SpanishDateTime.TimeBeforeAfterRegex)
        self._time_zone_extractor = self._timezone_extractor = BaseTimeZoneExtractor(
            SpanishTimeZoneExtractorConfiguration())
        # TODO When the implementation for these properties is added, change the None values to the respective Regexps
        self._ish_regex: Pattern = None

    @staticmethod
    def get_time_regex_list() -> List[Pattern]:
        return [
            RegExpUtility.get_safe_reg_exp(SpanishDateTime.TimeRegex1),
            RegExpUtility.get_safe_reg_exp(SpanishDateTime.TimeRegex2),
            RegExpUtility.get_safe_reg_exp(SpanishDateTime.TimeRegex3),
            RegExpUtility.get_safe_reg_exp(SpanishDateTime.TimeRegex4),
            RegExpUtility.get_safe_reg_exp(SpanishDateTime.TimeRegex5),
            RegExpUtility.get_safe_reg_exp(SpanishDateTime.TimeRegex6),
            RegExpUtility.get_safe_reg_exp(SpanishDateTime.TimeRegex7),
            RegExpUtility.get_safe_reg_exp(SpanishDateTime.TimeRegex8),
            RegExpUtility.get_safe_reg_exp(SpanishDateTime.TimeRegex9),
            RegExpUtility.get_safe_reg_exp(SpanishDateTime.TimeRegex11),
            RegExpUtility.get_safe_reg_exp(SpanishDateTime.TimeRegex12),
            RegExpUtility.get_safe_reg_exp(SpanishDateTime.ConnectNumRegex)
        ]
