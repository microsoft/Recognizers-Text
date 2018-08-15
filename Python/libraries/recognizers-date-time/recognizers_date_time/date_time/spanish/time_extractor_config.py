from typing import List, Pattern

from recognizers_text.utilities import RegExpUtility
from ...resources.spanish_date_time import SpanishDateTime
from ..base_time import TimeExtractorConfiguration

class SpanishTimeExtractorConfiguration(TimeExtractorConfiguration):
    @property
    def time_regex_list(self) -> List[Pattern]:
        return self._time_regex_list

    @property
    def at_regex(self) -> Pattern:
        return self._at_regex

    @property
    def ish_regex(self) -> Pattern:
        return self._ish_regex

    def __init__(self):
        self._time_regex_list: List[Pattern] = SpanishTimeExtractorConfiguration.get_time_regex_list()
        self._at_regex: Pattern = RegExpUtility.get_safe_reg_exp(SpanishDateTime.AtRegex)
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
            RegExpUtility.get_safe_reg_exp(SpanishDateTime.TimeRegex10),
            RegExpUtility.get_safe_reg_exp(SpanishDateTime.TimeRegex11),
            RegExpUtility.get_safe_reg_exp(SpanishDateTime.TimeRegex12),
            RegExpUtility.get_safe_reg_exp(SpanishDateTime.ConnectNumRegex)
        ]