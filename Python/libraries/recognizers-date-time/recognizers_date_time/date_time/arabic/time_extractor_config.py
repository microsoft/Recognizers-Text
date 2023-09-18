from typing import List, Pattern

from recognizers_text.utilities import RegExpUtility
from recognizers_date_time.resources.arabic_date_time import ArabicDateTime
from recognizers_date_time.date_time.base_time import TimeExtractorConfiguration
from recognizers_date_time.date_time.utilities import DateTimeOptions


class ArabicTimeExtractorConfiguration(TimeExtractorConfiguration):

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
    def at_regex(self) -> Pattern:
        return self._at_regex

    @property
    def ish_regex(self) -> Pattern:
        return self._ish_regex

    @property
    def time_unit_regex(self) -> Pattern:
        return self._time_unit_regex

    @property
    def connect_num_regex(self):
        return self._connect_num_regex

    @property
    def time_before_after_regex(self) -> Pattern:
        return self._time_before_after_regex

    @property
    def time_regex_list(self) -> List[Pattern]:
        return self._time_regex_list

    @property
    def duration_extractor(self):
        return self._duration_extractor

    @property
    def time_zone_extractor(self):
        return self._time_zone_extractor

    def __init__(self):
        super().__init__()

        self._time_regex_list: List[Pattern] = ArabicTimeExtractorConfiguration.get_time_regex_list(
        )
        self._at_regex: Pattern = RegExpUtility.get_safe_reg_exp(
            ArabicDateTime.AtRegex)
        self._ish_regex: Pattern = RegExpUtility.get_safe_reg_exp(
            ArabicDateTime.IshRegex)
        self._time_before_after_regex: Pattern = RegExpUtility.get_safe_reg_exp(
            ArabicDateTime.TimeBeforeAfterRegex)

        self._options = DateTimeOptions.NONE
        self._time_zone_extractor = None
        self._duration_extractor = None

        self._desc_regex = RegExpUtility.get_safe_reg_exp(ArabicDateTime.DescRegex)
        self._hour_num_regex = RegExpUtility.get_safe_reg_exp(ArabicDateTime.HourNumRegex)
        self._minute_num_regex = RegExpUtility.get_safe_reg_exp(ArabicDateTime.MinuteNumRegex)
        self._oclock_regex = RegExpUtility.get_safe_reg_exp(ArabicDateTime.OclockRegex)
        self._pm_regex = RegExpUtility.get_safe_reg_exp(ArabicDateTime.PmRegex)
        self._am_regex = RegExpUtility.get_safe_reg_exp(ArabicDateTime.AmRegex)
        self._less_than_one_hour = RegExpUtility.get_safe_reg_exp(ArabicDateTime.LessThanOneHour)
        self._written_time_regex = RegExpUtility.get_safe_reg_exp(ArabicDateTime.WrittenTimeRegex)
        self._time_prefix_regex = RegExpUtility.get_safe_reg_exp(ArabicDateTime.TimePrefix)
        self._time_suffix_regex = RegExpUtility.get_safe_reg_exp(ArabicDateTime.TimeSuffix)
        self._basic_time_regex = RegExpUtility.get_safe_reg_exp(ArabicDateTime.BasicTime)

        self._midnight_regex = RegExpUtility.get_safe_reg_exp(ArabicDateTime.MidnightRegex)
        self._midmorning_regex = RegExpUtility.get_safe_reg_exp(ArabicDateTime.MidmorningRegex)
        self._midafternoon_regex = RegExpUtility.get_safe_reg_exp(ArabicDateTime.MidafternoonRegex)
        self._midday_regex = RegExpUtility.get_safe_reg_exp(ArabicDateTime.MiddayRegex)
        self._midtime_regex = RegExpUtility.get_safe_reg_exp(ArabicDateTime.MidTimeRegex)
        self._time_unit_regex = RegExpUtility.get_safe_reg_exp(ArabicDateTime.TimeUnitRegex)
        self._connect_num_regex = RegExpUtility.get_safe_reg_exp(ArabicDateTime.ConnectNumRegex)

    @staticmethod
    def get_time_regex_list() -> List[Pattern]:
        return [
            RegExpUtility.get_safe_reg_exp(ArabicDateTime.TimeRegex1),
            RegExpUtility.get_safe_reg_exp(ArabicDateTime.TimeRegex2),
            RegExpUtility.get_safe_reg_exp(ArabicDateTime.TimeRegex3),
            RegExpUtility.get_safe_reg_exp(ArabicDateTime.TimeRegex4),
            RegExpUtility.get_safe_reg_exp(ArabicDateTime.TimeRegex5),
            RegExpUtility.get_safe_reg_exp(ArabicDateTime.TimeRegex6),
            RegExpUtility.get_safe_reg_exp(ArabicDateTime.TimeRegex7),
            RegExpUtility.get_safe_reg_exp(ArabicDateTime.TimeRegex9),
            RegExpUtility.get_safe_reg_exp(ArabicDateTime.TimeRegex10),
            RegExpUtility.get_safe_reg_exp(ArabicDateTime.TimeRegex11),
            RegExpUtility.get_safe_reg_exp(ArabicDateTime.ConnectNumRegex)
        ]
