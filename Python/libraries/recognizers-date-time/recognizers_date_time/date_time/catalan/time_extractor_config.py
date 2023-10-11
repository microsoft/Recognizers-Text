from typing import List, Pattern
from recognizers_text.utilities import RegExpUtility
from ...resources.catalan_date_time import CatalanDateTime
from ..base_time import TimeExtractorConfiguration
from ..base_timezone import BaseTimeZoneExtractor


class CatalanTimeExtractorConfiguration(TimeExtractorConfiguration):
    @property
    def time_zone_extractor(self):
        return None

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
        self._time_regex_list: List[Pattern] = CatalanTimeExtractorConfiguration.get_time_regex_list(
        )
        self._at_regex: Pattern = RegExpUtility.get_safe_reg_exp(
            CatalanDateTime.AtRegex)
        self._time_before_after_regex: Pattern = RegExpUtility.get_safe_reg_exp(f'^[.]')
        self._ish_regex: Pattern = None

    @staticmethod
    def get_time_regex_list() -> List[Pattern]:
        return [
            RegExpUtility.get_safe_reg_exp(CatalanDateTime.TimeRegex1),
            RegExpUtility.get_safe_reg_exp(CatalanDateTime.TimeRegex2),
            RegExpUtility.get_safe_reg_exp(CatalanDateTime.TimeRegex3),
            RegExpUtility.get_safe_reg_exp(CatalanDateTime.TimeRegex4),
            RegExpUtility.get_safe_reg_exp(CatalanDateTime.TimeRegex5),
            RegExpUtility.get_safe_reg_exp(CatalanDateTime.TimeRegex6),
            RegExpUtility.get_safe_reg_exp(CatalanDateTime.TimeRegex7),
            RegExpUtility.get_safe_reg_exp(CatalanDateTime.TimeRegex8),
            RegExpUtility.get_safe_reg_exp(CatalanDateTime.TimeRegex9),
            RegExpUtility.get_safe_reg_exp(CatalanDateTime.TimeRegex11),
            RegExpUtility.get_safe_reg_exp(CatalanDateTime.TimeRegex12),
            RegExpUtility.get_safe_reg_exp(CatalanDateTime.ConnectNumRegex)
        ]