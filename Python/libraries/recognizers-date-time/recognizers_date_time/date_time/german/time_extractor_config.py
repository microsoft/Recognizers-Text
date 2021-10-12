#  Copyright (c) Microsoft Corporation. All rights reserved.
#  Licensed under the MIT License.

from typing import List, Pattern

from recognizers_text.utilities import RegExpUtility
from ...resources.german_date_time import GermanDateTime
from ..base_time import TimeExtractorConfiguration
from ..base_timezone import BaseTimeZoneExtractor
from ..extractors import DateTimeExtractor
from .timezone_extractor_config import GermanTimeZoneExtractorConfiguration


class GermanTimeExtractorConfiguration(TimeExtractorConfiguration):
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
    def time_zone_extractor(self) -> DateTimeExtractor:
        return self._time_zone_extractor

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
    def less_than_one_hour(self) -> Pattern:
        return self._less_than_one_hour

    @property
    def written_time_regex(self) -> Pattern:
        return self._written_time_regex

    @property
    def time_prefix(self) -> Pattern:
        return self._time_prefix

    @property
    def time_suffix(self) -> Pattern:
        return self._time_suffix

    @property
    def basic_time(self) -> Pattern:
        return self._basic_time

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
        self._desc_regex = RegExpUtility.get_safe_reg_exp(
            GermanDateTime.DescRegex
        )
        self._hour_num_regex = RegExpUtility.get_safe_reg_exp(
            GermanDateTime.HourNumRegex
        )
        self._minute_num_regex = RegExpUtility.get_safe_reg_exp(
            GermanDateTime.MinuteNumRegex
        )
        self._oclock_regex = RegExpUtility.get_safe_reg_exp(
            GermanDateTime.OclockRegex
        )
        self._pm_regex = RegExpUtility.get_safe_reg_exp(
            GermanDateTime.PmRegex
        )
        self._am_regex = RegExpUtility.get_safe_reg_exp(
            GermanDateTime.AmRegex
        )
        self._less_than_one_hour = RegExpUtility.get_safe_reg_exp(
            GermanDateTime.LessThanOneHour
        )
        self._written_time_regex = RegExpUtility.get_safe_reg_exp(
            GermanDateTime.WrittenTimeRegex
        )
        self._time_prefix = RegExpUtility.get_safe_reg_exp(
            GermanDateTime.TimePrefix
        )
        self._time_suffix = RegExpUtility.get_safe_reg_exp(
            GermanDateTime.TimeSuffix
        )
        self._basic_time = RegExpUtility.get_safe_reg_exp(
            GermanDateTime.BasicTime
        )
        self._midnight_regex = RegExpUtility.get_safe_reg_exp(
            GermanDateTime.MidnightRegex
        )
        self._midmorning_regex = RegExpUtility.get_safe_reg_exp(
            GermanDateTime.MidmorningRegex
        )
        self._midafternoon_regex = RegExpUtility.get_safe_reg_exp(
            GermanDateTime.MidafternoonRegex
        )
        self._midday_regex = RegExpUtility.get_safe_reg_exp(
            GermanDateTime.MiddayRegex
        )
        self._midtime_regex = RegExpUtility.get_safe_reg_exp(
            GermanDateTime.MidTimeRegex
        )
        self._time_unit_regex = RegExpUtility.get_safe_reg_exp(
            GermanDateTime.TimeUnitRegex
        )
        self._time_regex_list: List[Pattern] = [
            RegExpUtility.get_safe_reg_exp(GermanDateTime.TimeRegex1),
            RegExpUtility.get_safe_reg_exp(GermanDateTime.TimeRegex2),
            RegExpUtility.get_safe_reg_exp(GermanDateTime.TimeRegex3),
            RegExpUtility.get_safe_reg_exp(GermanDateTime.TimeRegex4),
            RegExpUtility.get_safe_reg_exp(GermanDateTime.TimeRegex5),
            RegExpUtility.get_safe_reg_exp(GermanDateTime.TimeRegex6),
            RegExpUtility.get_safe_reg_exp(GermanDateTime.TimeRegex7),
            RegExpUtility.get_safe_reg_exp(GermanDateTime.TimeRegex8),
            RegExpUtility.get_safe_reg_exp(GermanDateTime.TimeRegex9),
            RegExpUtility.get_safe_reg_exp(GermanDateTime.TimeRegex10),
            # RegExpUtility.get_safe_reg_exp(GermanDateTime.TimeRegex11),
            RegExpUtility.get_safe_reg_exp(GermanDateTime.ConnectNumRegex)
        ]
        self._at_regex: Pattern = RegExpUtility.get_safe_reg_exp(
            GermanDateTime.AtRegex)
        self._ish_regex: Pattern = RegExpUtility.get_safe_reg_exp(
            GermanDateTime.IshRegex)
        self._time_before_after_regex: Pattern = RegExpUtility.get_safe_reg_exp(
            GermanDateTime.TimeBeforeAfterRegex)
        self._time_zone_extractor = BaseTimeZoneExtractor(
            GermanTimeZoneExtractorConfiguration())
