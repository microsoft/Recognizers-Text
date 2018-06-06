from typing import List, Pattern

from recognizers_text.utilities import RegExpUtility
from ...resources.english_date_time import EnglishDateTime
from ..base_time import TimeExtractorConfiguration

class EnglishTimeExtractorConfiguration(TimeExtractorConfiguration):
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
        self._time_regex_list: List[Pattern] = [
            RegExpUtility.get_safe_reg_exp(EnglishDateTime.TimeRegex1),
            RegExpUtility.get_safe_reg_exp(EnglishDateTime.TimeRegex2),
            RegExpUtility.get_safe_reg_exp(EnglishDateTime.TimeRegex3),
            RegExpUtility.get_safe_reg_exp(EnglishDateTime.TimeRegex4),
            RegExpUtility.get_safe_reg_exp(EnglishDateTime.TimeRegex5),
            RegExpUtility.get_safe_reg_exp(EnglishDateTime.TimeRegex6),
            RegExpUtility.get_safe_reg_exp(EnglishDateTime.TimeRegex7),
            RegExpUtility.get_safe_reg_exp(EnglishDateTime.TimeRegex8),
            RegExpUtility.get_safe_reg_exp(EnglishDateTime.TimeRegex9),
            RegExpUtility.get_safe_reg_exp(EnglishDateTime.ConnectNumRegex)
        ]
        self._at_regex: Pattern = RegExpUtility.get_safe_reg_exp(EnglishDateTime.AtRegex)
        self._ish_regex: Pattern = RegExpUtility.get_safe_reg_exp(EnglishDateTime.IshRegex)
