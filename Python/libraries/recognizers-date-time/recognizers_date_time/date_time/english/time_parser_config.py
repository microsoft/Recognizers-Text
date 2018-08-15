from typing import List, Pattern, Dict
import regex

from recognizers_text.utilities import RegExpUtility
from ...resources.english_date_time import EnglishDateTime
from ..base_time import TimeParserConfiguration, AdjustParams
from ..base_configs import BaseDateParserConfiguration, DateTimeUtilityConfiguration

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

    def __init__(self, config: BaseDateParserConfiguration):
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
        self._utility_configuration = config.utility_configuration
        self.less_than_one_hour = RegExpUtility.get_safe_reg_exp(EnglishDateTime.LessThanOneHour)
        self.time_suffix_full = RegExpUtility.get_safe_reg_exp(EnglishDateTime.TimeSuffixFull)
        self.lunch_regex = RegExpUtility.get_safe_reg_exp(EnglishDateTime.LunchRegex)
        self.night_regex = RegExpUtility.get_safe_reg_exp(EnglishDateTime.NightRegex)
        self.ish_regex = RegExpUtility.get_safe_reg_exp(EnglishDateTime.IshRegex)

    def adjust_by_prefix(self, prefix: str, adjust: AdjustParams):
        delta_min = 0
        prefix = prefix.strip().lower()
        if prefix.startswith('half'):
            delta_min = 30
        elif prefix.startswith('a quarter') or prefix.startswith('quarter'):
            delta_min = 15
        elif prefix.startswith('three quarter'):
            delta_min = 45
        else:
            match = regex.search(self.less_than_one_hour, prefix)
            min_str = RegExpUtility.get_group(match, 'deltamin')
            if min_str:
                delta_min = int(min_str)
            else:
                min_str = RegExpUtility.get_group(match, 'deltaminnum').lower()
                delta_min = self.numbers[min_str]
        if prefix.endswith('to'):
            delta_min = delta_min * -1
        adjust.minute += delta_min
        if adjust.minute < 0:
            adjust.minute += 60
            adjust.hour -= 1
        adjust.has_minute = True

    def adjust_by_suffix(self, suffix: str, adjust: AdjustParams):
        suffix = suffix.strip().lower()
        delta_hour = 0
        match = regex.search(self.time_suffix_full, suffix)
        if match is not None and match.start() == 0 and match.group() == suffix:
            oclock_str = RegExpUtility.get_group(match, 'oclock')
            if not oclock_str:
                am_str = RegExpUtility.get_group(match, 'am')
                if am_str:
                    if adjust.hour >= 12:
                        delta_hour -= 12
                    else:
                        adjust.has_am = True
                pm_str = RegExpUtility.get_group(match, 'pm')
                if pm_str:
                    if adjust.hour < 12:
                        delta_hour = 12
                    if regex.search(self.lunch_regex, pm_str):
                        # for hour >= 10 and < 12
                        if adjust.hour >= 10 and adjust.hour <= 12:
                            delta_hour = 0
                            if adjust.hour == 12:
                                adjust.has_pm = True
                            else:
                                adjust.has_am = True
                        else:
                            adjust.has_pm = True
                    elif regex.search(self.night_regex, pm_str):
                        if adjust.hour <= 3 or adjust.hour == 12:
                            if adjust.hour == 12:
                                adjust.hour = 0
                            delta_hour = 0
                            adjust.has_am = True
                        else:
                            adjust.has_pm = True
        adjust.hour = (adjust.hour + delta_hour) % 24
