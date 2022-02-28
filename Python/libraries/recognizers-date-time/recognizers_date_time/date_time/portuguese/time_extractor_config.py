#  Copyright (c) Microsoft Corporation. All rights reserved.
#  Licensed under the MIT License.

from typing import List, Pattern

from recognizers_text.utilities import RegExpUtility
from ...resources.portuguese_date_time import PortugueseDateTime
from ..base_time import TimeExtractorConfiguration
from ..base_timezone import BaseTimeZoneExtractor
from ..extractors import DateTimeExtractor
from .timezone_extractor_config import PortugueseTimeZoneExtractorConfiguration


class PortugueseTimeExtractorConfiguration(TimeExtractorConfiguration):
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

    @property
    def pm_regex(self) -> Pattern:
        return self._pm_regex

    @property
    def am_regex(self) -> Pattern:
        return self._am_regex

    def __init__(self):
        super().__init__()
        self._desc_regex = RegExpUtility.get_safe_reg_exp(
            PortugueseDateTime.DescRegex
        )
        self._hour_num_regex = RegExpUtility.get_safe_reg_exp(
            PortugueseDateTime.HourNumRegex
        )
        self._minute_num_regex = RegExpUtility.get_safe_reg_exp(
            PortugueseDateTime.MinuteNumRegex
        )
        self._oclock_regex = RegExpUtility.get_safe_reg_exp(
            PortugueseDateTime.OclockRegex
        )
        self._pm_regex = RegExpUtility.get_safe_reg_exp(
            PortugueseDateTime.PmRegex
        )
        self._am_regex = RegExpUtility.get_safe_reg_exp(
            PortugueseDateTime.AmRegex
        )
        self._less_than_one_hour = RegExpUtility.get_safe_reg_exp(
            PortugueseDateTime.LessThanOneHour
        )
        self._written_time_regex = RegExpUtility.get_safe_reg_exp(
            PortugueseDateTime.WrittenTimeRegex
        )
        self._time_prefix = RegExpUtility.get_safe_reg_exp(
            PortugueseDateTime.TimePrefix
        )
        self._time_suffix = RegExpUtility.get_safe_reg_exp(
            PortugueseDateTime.TimeSuffix
        )
        self._basic_time = RegExpUtility.get_safe_reg_exp(
            PortugueseDateTime.BasicTime
        )
        self._midnight_regex = RegExpUtility.get_safe_reg_exp(
            PortugueseDateTime.MidnightRegex
        )
        self._midmorning_regex = RegExpUtility.get_safe_reg_exp(
            PortugueseDateTime.MidmorningRegex
        )
        self._midafternoon_regex = RegExpUtility.get_safe_reg_exp(
            PortugueseDateTime.MidafternoonRegex
        )
        self._midday_regex = RegExpUtility.get_safe_reg_exp(
            PortugueseDateTime.MiddayRegex
        )
        self._midtime_regex = RegExpUtility.get_safe_reg_exp(
            PortugueseDateTime.MidTimeRegex
        )
        self._time_unit_regex = RegExpUtility.get_safe_reg_exp(
            PortugueseDateTime.TimeUnitRegex
        )
        self._time_regex_list: List[Pattern] = [
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
        self._at_regex: Pattern = RegExpUtility.get_safe_reg_exp(
            PortugueseDateTime.AtRegex)
        self._ish_regex: Pattern = None
        self._time_before_after_regex: Pattern = RegExpUtility.get_safe_reg_exp(
            PortugueseDateTime.TimeBeforeAfterRegex)
        self._time_zone_extractor = BaseTimeZoneExtractor(
            PortugueseTimeZoneExtractorConfiguration())
