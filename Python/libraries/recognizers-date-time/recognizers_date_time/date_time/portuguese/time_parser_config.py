#  Copyright (c) Microsoft Corporation. All rights reserved.
#  Licensed under the MIT License.

from typing import List, Pattern, Dict
import regex
from ..parsers import DateTimeParser

from recognizers_text.utilities import RegExpUtility
from ...resources.portuguese_date_time import PortugueseDateTime
from ..base_time import TimeParserConfiguration, AdjustParams
from ..base_configs import BaseDateParserConfiguration, DateTimeUtilityConfiguration


class PortugueseTimeParserConfiguration(TimeParserConfiguration):
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

    @property
    def time_zone_parser(self) -> DateTimeParser:
        return self._time_zone_parser

    def __init__(self, config: BaseDateParserConfiguration):
        self._time_token_prefix: str = PortugueseDateTime.TimeTokenPrefix
        self._at_regex: Pattern = RegExpUtility.get_safe_reg_exp(
            PortugueseDateTime.AtRegex)
        self._time_regexes: List[Pattern] = [
            RegExpUtility.get_safe_reg_exp(PortugueseDateTime.TimeRegex1),
            RegExpUtility.get_safe_reg_exp(PortugueseDateTime.TimeRegex2),
            RegExpUtility.get_safe_reg_exp(PortugueseDateTime.TimeRegex3),
            RegExpUtility.get_safe_reg_exp(PortugueseDateTime.TimeRegex4),
            RegExpUtility.get_safe_reg_exp(PortugueseDateTime.TimeRegex5),
            RegExpUtility.get_safe_reg_exp(PortugueseDateTime.TimeRegex6),
            RegExpUtility.get_safe_reg_exp(PortugueseDateTime.TimeRegex7),
            RegExpUtility.get_safe_reg_exp(PortugueseDateTime.TimeRegex8),
            RegExpUtility.get_safe_reg_exp(PortugueseDateTime.TimeRegex9),
            RegExpUtility.get_safe_reg_exp(PortugueseDateTime.TimeRegex11),
            RegExpUtility.get_safe_reg_exp(PortugueseDateTime.TimeRegex12),
            RegExpUtility.get_safe_reg_exp(PortugueseDateTime.ConnectNumRegex)
        ]
        self._numbers: Dict[str, int] = PortugueseDateTime.Numbers
        self._time_zone_parser = config.time_zone_parser
        self._utility_configuration = config.utility_configuration
        self.less_than_one_hour = RegExpUtility.get_safe_reg_exp(
            PortugueseDateTime.LessThanOneHour)
        self.time_suffix_full = RegExpUtility.get_safe_reg_exp(
            PortugueseDateTime.TimeSuffix)
        self.night_regex = RegExpUtility.get_safe_reg_exp(
            PortugueseDateTime.NightRegex)

    def adjust_by_prefix(self, prefix: str, adjust: AdjustParams):
        delta_min = 0
        prefix = prefix.strip().lower()
        if prefix.startswith('meia') or prefix.startswith('e meia'):
            delta_min = 30
        elif prefix.startswith('quarto') or prefix.startswith('e um quarto') \
                or prefix.startswith('quinze') or prefix.startswith('e quinze'):
            delta_min = 15
        elif prefix.startswith('menos um quarto'):
            delta_min = -15
        else:
            match = regex.search(self.less_than_one_hour, prefix)
            min_str = RegExpUtility.get_group(match, 'deltamin')
            if min_str:
                delta_min = int(min_str)
            else:
                min_str = RegExpUtility.get_group(match, 'deltaminnum').lower()
                delta_min = self.numbers[min_str]
        if prefix.endswith('para a') or prefix.endswith('para as') or prefix.endswith('pra')  \
                or prefix.endswith('pras') or prefix.endswith('antes da') or prefix.endswith('antes das'):
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
                    if regex.search(self.night_regex, pm_str):
                        if adjust.hour <= 3 or adjust.hour == 12:
                            if adjust.hour == 12:
                                adjust.hour = 0
                            delta_hour = 0
                            adjust.has_am = True
                        else:
                            adjust.has_pm = True
        adjust.hour = (adjust.hour + delta_hour) % 24
