from typing import List, Pattern, Dict

from recognizers_text.utilities import RegExpUtility
from recognizers_date_time.date_time.base_time import TimeExtractorConfiguration, TimeParserConfiguration, AdjustParams
from recognizers_date_time.date_time.utilities import DateTimeUtilityConfiguration
from recognizers_date_time.resources.english_date_time import EnglishDateTime

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

class EnglishTimeParserConfiguration(TimeParserConfiguration):
    @property
    def time_token_prefix(self) -> str:
        return self._time_token_prefix

    @property
    def at_regex(self) -> Pattern:
        return self._at_regex

    @property
    def time_regexes(self) -> List[Pattern]:
        return self._time_regexes

    @property
    def numbers(self) -> Dict[str, int]:
        return self._numbers

    @property
    def utility_configuration(self) -> DateTimeUtilityConfiguration:
        return self._utility_configuration

    def __init__(self):
        self._time_token_prefix: str = EnglishDateTime.TimeTokenPrefix
        self._at_regex: Pattern = RegExpUtility.get_safe_reg_exp(EnglishDateTime.AtRegex)
        self._time_regexes: List[Pattern] = [
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
        self._numbers: Dict[str, int] = EnglishDateTime.Numbers
        self._utility_configuration: DateTimeUtilityConfiguration = None

    def adjust_by_prefix(self, prefix: str, adjust: AdjustParams):
        pass

    def adjust_by_suffix(self, prefix: str, adjust: AdjustParams):
        pass
