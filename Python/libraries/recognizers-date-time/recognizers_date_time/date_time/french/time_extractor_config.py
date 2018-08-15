from typing import List, Pattern

from recognizers_text.utilities import RegExpUtility
from ...resources.french_date_time import FrenchDateTime
from ..base_time import TimeExtractorConfiguration

class FrenchTimeExtractorConfiguration(TimeExtractorConfiguration):
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
        self._time_regex_list: List[Pattern] = FrenchTimeExtractorConfiguration.get_time_regex_list()
        self._at_regex: Pattern = RegExpUtility.get_safe_reg_exp(FrenchDateTime.AtRegex)
        self._ish_regex: Pattern = RegExpUtility.get_safe_reg_exp(FrenchDateTime.IshRegex)

    @staticmethod
    def get_time_regex_list() -> List[Pattern]:
        return [
            RegExpUtility.get_safe_reg_exp(FrenchDateTime.TimeRegex1),
            RegExpUtility.get_safe_reg_exp(FrenchDateTime.TimeRegex2),
            RegExpUtility.get_safe_reg_exp(FrenchDateTime.TimeRegex3),
            RegExpUtility.get_safe_reg_exp(FrenchDateTime.TimeRegex4),
            RegExpUtility.get_safe_reg_exp(FrenchDateTime.TimeRegex5),
            RegExpUtility.get_safe_reg_exp(FrenchDateTime.TimeRegex6),
            RegExpUtility.get_safe_reg_exp(FrenchDateTime.TimeRegex7),
            RegExpUtility.get_safe_reg_exp(FrenchDateTime.TimeRegex8),
            RegExpUtility.get_safe_reg_exp(FrenchDateTime.TimeRegex9),
            RegExpUtility.get_safe_reg_exp(FrenchDateTime.TimeRegex10),
            RegExpUtility.get_safe_reg_exp(FrenchDateTime.ConnectNumRegex)
        ]
