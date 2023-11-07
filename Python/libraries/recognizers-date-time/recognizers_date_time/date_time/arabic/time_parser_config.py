from typing import List, Pattern, Dict
import regex

from recognizers_text.utilities import RegExpUtility
from recognizers_date_time.resources.arabic_date_time import ArabicDateTime
from recognizers_date_time.date_time.base_time import TimeParserConfiguration, AdjustParams
from recognizers_date_time.date_time.base_configs import BaseDateParserConfiguration, DateTimeUtilityConfiguration
from recognizers_date_time.date_time.arabic.time_extractor_config import ArabicTimeExtractorConfiguration
from recognizers_date_time.date_time.parsers import DateTimeParser
from recognizers_date_time.date_time.constants import Constants


class ArabicTimeParserConfiguration(TimeParserConfiguration):

    @property
    def time_suffix_full(self) -> Pattern:
        return self._time_suffix_full

    @property
    def lunch_regex(self) -> Pattern:
        return self._lunch_regex

    @property
    def night_regex(self) -> Pattern:
        return self._night_regex

    @property
    def half_token_regex(self) -> Pattern:
        return self._half_token_regex

    @property
    def quarter_token_regex(self) -> Pattern:
        return self._quarter_token_regex

    @property
    def three_quarter_token_regex(self) -> Pattern:
        return self._three_quarter_token_regex

    @property
    def to_token_regex(self) -> Pattern:
        return self._to_token_regex

    @property
    def to_half_token_regex(self) -> Pattern:
        return self._to_half_token_regex

    @property
    def for_half_token_regex(self) -> Pattern:
        return self._for_half_token_regex

    @property
    def time_token_prefix(self) -> str:
        return self._time_token_prefix

    @property
    def at_regex(self) -> Pattern:
        return self._at_regex

    @property
    def meal_time_regex(self) -> Pattern:
        return self._meal_time_regex

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

        self._time_token_prefix = ArabicDateTime.TimeTokenPrefix
        self._at_regex: Pattern = RegExpUtility.get_safe_reg_exp(ArabicDateTime.AtRegex)
        self._time_regexes = ArabicTimeExtractorConfiguration.get_time_regex_list()
        self.less_than_one_hour = RegExpUtility.get_safe_reg_exp(ArabicDateTime.LessThanOneHour)
        self.time_suffix = RegExpUtility.get_safe_reg_exp(ArabicDateTime.TimeSuffix)
        self.time_suffix_full_regex = RegExpUtility.get_safe_reg_exp(ArabicDateTime.TimeSuffixFull)

        self._half_token_regex = RegExpUtility.get_safe_reg_exp(
            ArabicDateTime.HalfTokenRegex)
        self._quarter_token_regex = RegExpUtility.get_safe_reg_exp(
            ArabicDateTime.QuarterTokenRegex)
        self._three_quarter_token_regex = RegExpUtility.get_safe_reg_exp(
            ArabicDateTime.ThreeQuarterTokenRegex)

        self._night_regex = RegExpUtility.get_safe_reg_exp(
            ArabicDateTime.NightRegex)
        self._lunch_regex = RegExpUtility.get_safe_reg_exp(ArabicDateTime.LunchRegex)
        self._to_token_regex = RegExpUtility.get_safe_reg_exp(ArabicDateTime.ToTokenRegex)
        self._to_half_token_regex = RegExpUtility.get_safe_reg_exp(ArabicDateTime.ToHalfTokenRegex)
        self._for_half_token_regex = RegExpUtility.get_safe_reg_exp(ArabicDateTime.ForHalfTokenRegex)
        self._meal_time_regex = RegExpUtility.get_safe_reg_exp(ArabicDateTime.MealTimeRegex)
        self._time_suffix_full = RegExpUtility.get_safe_reg_exp(ArabicDateTime.TimeSuffixFull)

        self._utility_configuration = config.utility_configuration
        self._numbers: Dict[str, int] = config.numbers
        self._time_zone_parser = config.time_zone_parser

    def adjust_by_prefix(self, prefix: str, adjust: AdjustParams):
        delta_min = 0
        trimmed_prefix = prefix.strip().lower()

        if regex.search(self.half_token_regex, trimmed_prefix):
            delta_min = 30
        elif regex.search(self.quarter_token_regex, trimmed_prefix):
            delta_min = 15
        elif regex.search(self.three_quarter_token_regex, trimmed_prefix):
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

        if regex.search(self.to_half_token_regex, trimmed_prefix):
            delta_min = delta_min - 30
        elif regex.search(self.for_half_token_regex, trimmed_prefix):
            delta_min = -delta_min - 30
        elif regex.search(self.to_token_regex, trimmed_prefix):
            delta_min = delta_min * -1

        adjust.minute += delta_min

        if adjust.minute < 0:
            adjust.minute += 60
            adjust.hour -= 1

        adjust.has_minute = True

    def adjust_by_suffix(self, suffix: str, adjust: AdjustParams):
        suffix = suffix.strip().lower()
        delta_hour = 0
        match = regex.search(self.time_suffix_full_regex, suffix)
        if match is not None and match.start() == 0 and match.group() == suffix:
            oclock_str = RegExpUtility.get_group(match, 'oclock')
            if not oclock_str:
                am_str = RegExpUtility.get_group(match, Constants.AM_GROUP_NAME)
                if am_str:
                    if adjust.hour >= Constants.HALF_DAY_HOUR_COUNT:
                        delta_hour -= Constants.HALF_DAY_HOUR_COUNT
                    else:
                        adjust.has_am = True
                pm_str = RegExpUtility.get_group(match, Constants.PM_GROUP_NAME)

                if pm_str:
                    if adjust.hour < Constants.HALF_DAY_HOUR_COUNT:
                        delta_hour = Constants.HALF_DAY_HOUR_COUNT
                    if regex.search(self.lunch_regex, pm_str):
                        if adjust.hour >= 10 and adjust.hour <= Constants.HALF_DAY_HOUR_COUNT:
                            delta_hour = 0
                            if (adjust.hour == Constants.HALF_DAY_HOUR_COUNT):
                                adjust.has_pm = True
                            else:
                                adjust.has_am = True
                        else:
                            adjust.has_pm = True

                    elif regex.search(self.night_regex, pm_str):
                        if adjust.hour <= 3 or adjust.hour == Constants.HALF_DAY_HOUR_COUNT:
                            if adjust.hour == Constants.HALF_DAY_HOUR_COUNT:
                                adjust.hour = 0
                            delta_hour = 0
                            adjust.has_am = True
                        else:
                            adjust.has_pm = True
                    else:
                        adjust.has_pm = True

        adjust.hour = (adjust.hour + delta_hour) % 24
