#  Copyright (c) Microsoft Corporation. All rights reserved.
#  Licensed under the MIT License.

from typing import List, Pattern, Dict
import regex

from recognizers_text.utilities import RegExpUtility
from ...resources.french_date_time import FrenchDateTime
from ..base_time import TimeParserConfiguration, AdjustParams
from ..base_configs import BaseDateParserConfiguration, DateTimeUtilityConfiguration
from .time_extractor_config import FrenchTimeExtractorConfiguration
from ..parsers import DateTimeParser


class FrenchTimeParserConfiguration(TimeParserConfiguration):
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
        self._time_token_prefix: str = FrenchDateTime.TimeTokenPrefix
        self._at_regex: Pattern = RegExpUtility.get_safe_reg_exp(
            FrenchDateTime.AtRegex)
        self._time_regexes: List[Pattern] = FrenchTimeExtractorConfiguration.get_time_regex_list(
        )
        self.less_than_one_hour = RegExpUtility.get_safe_reg_exp(
            FrenchDateTime.LessThanOneHour)
        self.time_suffix = RegExpUtility.get_safe_reg_exp(
            FrenchDateTime.TimeSuffix)

        self._utility_configuration = config.utility_configuration
        self._numbers: Dict[str, int] = config.numbers
        self._time_zone_parser = config.time_zone_parser

    def adjust_by_prefix(self, prefix: str, adjust: AdjustParams):
        delta_min = 0
        trimmed_prefix = prefix.strip().lower()

        # @todo Move hardcoded strings to resource YAML file.
        if trimmed_prefix.endswith('demie'):
            delta_min = 30
        elif trimmed_prefix.endswith('un quart') or trimmed_prefix.endswith('quart'):
            delta_min = 15
        elif trimmed_prefix.endswith('trois quarts'):
            delta_min = 45
        else:
            match = regex.search(self.less_than_one_hour, trimmed_prefix)
            if match:
                min_str = RegExpUtility.get_group(match, 'deltamin')
                if min_str:
                    delta_min = int(min_str)
                else:
                    min_str = RegExpUtility.get_group(
                        match, 'deltaminnum').lower()
                    delta_min = self.numbers.get(min_str)

        if trimmed_prefix.endswith('à') or 'moins' in trimmed_prefix:
            delta_min = delta_min * -1

        adjust.minute += delta_min

        if adjust.minute < 0:
            adjust.minute += 60
            adjust.hour -= 1

        adjust.has_minute = True

    def adjust_by_suffix(self, suffix: str, adjust: AdjustParams):
        suffix = suffix.strip().lower()

        delta_hour = 0
        match = regex.match(self.time_suffix, suffix)

        if match and match.group() == suffix:
            oclock_str = RegExpUtility.get_group(match, 'heures')
            if not oclock_str:
                am_str = RegExpUtility.get_group(match, 'am')
                if am_str:
                    if adjust.hour >= 12:
                        delta_hour -= 12

                    adjust.has_am = True

                pm_str = RegExpUtility.get_group(match, 'pm')
                if pm_str:
                    if adjust.hour < 12:
                        delta_hour = 12

                    adjust.has_pm = True

        adjust.hour = (adjust.hour + delta_hour) % 24
