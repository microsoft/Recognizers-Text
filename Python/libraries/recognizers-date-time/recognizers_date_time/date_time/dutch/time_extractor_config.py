#  Copyright (c) Microsoft Corporation. All rights reserved.
#  Licensed under the MIT License.

from typing import List, Pattern

from recognizers_text.utilities import RegExpUtility
from ...resources.dutch_date_time import DutchDateTime
from ..base_time import TimeExtractorConfiguration
from ..utilities import DateTimeOptions
from ..base_timezone import BaseTimeZoneExtractor
from .timezone_extractor_config import DutchTimeZoneExtractorConfiguration


class DutchTimeExtractorConfiguration(TimeExtractorConfiguration):
    @property
    def time_zone_extractor(self):
        return self._time_zone_extractor

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
    def desc_regex(self) -> Pattern:
        return self._desc_regex

    @property
    def hour_num_regex(self) -> Pattern:
        return self._hour_num_regex

    @property
    def minute_num_regex(self) -> Pattern:
        return self._minute_num_regex

    @property
    def oclock_regex(self) -> Pattern:
        return self._oclock_regex

    @property
    def pm_regex(self) -> Pattern:
        return self._pm_regex

    @property
    def am_regex(self) -> Pattern:
        return self._am_regex

    @property
    def less_than_one_hour_regex(self) -> Pattern:
        return self._less_than_one_hour

    @property
    def written_time_regex(self) -> Pattern:
        return self._written_time_regex

    @property
    def time_prefix_regex(self) -> Pattern:
        return self._time_prefix_regex

    @property
    def time_suffix_regex(self) -> Pattern:
        return self._time_suffix_regex

    @property
    def basic_time_regex(self) -> Pattern:
        return self._basic_time_regex

    @property
    def midnight_regex(self) -> Pattern:
        return self._midnight_regex

    @property
    def midmorning_regex(self) -> Pattern:
        return self._midmorning_regex

    @property
    def midafternoon_regex(self) -> Pattern:
        return self._midafternoon_regex

    @property
    def midday_regex(self) -> Pattern:
        return self._midday_regex

    @property
    def midtime_regex(self) -> Pattern:
        return self._midtime_regex

    @property
    def time_unit_regex(self) -> Pattern:
        return self._time_unit_regex

    def __init__(self):
        super().__init__()
        self._time_regex_list: List[Pattern] = DutchTimeExtractorConfiguration.get_time_regex_list(
        )
        self._at_regex: Pattern = RegExpUtility.get_safe_reg_exp(
            DutchDateTime.AtRegex)
        self._ish_regex: Pattern = RegExpUtility.get_safe_reg_exp(
            DutchDateTime.IshRegex)
        self._time_before_after_regex: Pattern = RegExpUtility.get_safe_reg_exp(
            DutchDateTime.TimeBeforeAfterRegex)
        self._options = DateTimeOptions.NONE
        self._time_zone_extractor = BaseTimeZoneExtractor(
            DutchTimeZoneExtractorConfiguration())
        self._desc_regex = RegExpUtility.get_safe_reg_exp(DutchDateTime.DescRegex)
        self._hour_num_regex = RegExpUtility.get_safe_reg_exp(DutchDateTime.HourNumRegex)
        self._minute_num_regex = RegExpUtility.get_safe_reg_exp(DutchDateTime.MinuteNumRegex)
        self._oclock_regex = RegExpUtility.get_safe_reg_exp(DutchDateTime.OclockRegex)
        self._pm_regex = RegExpUtility.get_safe_reg_exp(DutchDateTime.PmRegex)
        self._am_regex = RegExpUtility.get_safe_reg_exp(DutchDateTime.AmRegex)
        self._less_than_one_hour = RegExpUtility.get_safe_reg_exp(DutchDateTime.LessThanOneHour)
        self._written_time_regex = RegExpUtility.get_safe_reg_exp(DutchDateTime.WrittenTimeRegex)
        self._time_prefix_regex = RegExpUtility.get_safe_reg_exp(DutchDateTime.TimePrefix)
        self._time_suffix_regex = RegExpUtility.get_safe_reg_exp(DutchDateTime.TimeSuffix)
        self._basic_time_regex = RegExpUtility.get_safe_reg_exp(DutchDateTime.BasicTime)
        self._midnight_regex = RegExpUtility.get_safe_reg_exp(DutchDateTime.MidnightRegex)
        self._midmorning_regex = RegExpUtility.get_safe_reg_exp(DutchDateTime.MidmorningRegex)
        self._midafternoon_regex = RegExpUtility.get_safe_reg_exp(DutchDateTime.MidafternoonRegex)
        self._midday_regex = RegExpUtility.get_safe_reg_exp(DutchDateTime.MiddayRegex)
        self._midtime_regex = RegExpUtility.get_safe_reg_exp(DutchDateTime.MidTimeRegex)
        self._time_unit_regex = RegExpUtility.get_safe_reg_exp(DutchDateTime.TimeUnitRegex)

    @staticmethod
    def get_time_regex_list() -> List[Pattern]:
        return [
            RegExpUtility.get_safe_reg_exp(DutchDateTime.TimeRegex1),
            RegExpUtility.get_safe_reg_exp(DutchDateTime.TimeRegex2),
            RegExpUtility.get_safe_reg_exp(DutchDateTime.TimeRegex3),
            RegExpUtility.get_safe_reg_exp(DutchDateTime.TimeRegex4),
            RegExpUtility.get_safe_reg_exp(DutchDateTime.TimeRegex5),
            RegExpUtility.get_safe_reg_exp(DutchDateTime.TimeRegex6),
            RegExpUtility.get_safe_reg_exp(DutchDateTime.TimeRegex7),
            RegExpUtility.get_safe_reg_exp(DutchDateTime.TimeRegex8),
            RegExpUtility.get_safe_reg_exp(DutchDateTime.TimeRegex9),
            RegExpUtility.get_safe_reg_exp(DutchDateTime.TimeRegex10),
            RegExpUtility.get_safe_reg_exp(DutchDateTime.TimeRegex11),
            RegExpUtility.get_safe_reg_exp(DutchDateTime.TimeRegex12),
            RegExpUtility.get_safe_reg_exp(DutchDateTime.ConnectNumRegex)
        ]
