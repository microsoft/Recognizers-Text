from typing import Pattern, Dict

from recognizers_date_time.date_time.data_structures import PeriodType
from recognizers_text.utilities import RegExpUtility, DefinitionLoader
from recognizers_date_time.resources.japanese_date_time import JapaneseDateTime
from recognizers_date_time.date_time.CJK.base_timeperiod import CJKTimePeriodExtractorConfiguration
from recognizers_date_time.date_time.japanese.time_extractor_config import JapaneseTimeExtractorConfiguration


class JapaneseTimePeriodExtractorConfiguration(CJKTimePeriodExtractorConfiguration):

    @property
    def regexes(self) -> Dict[Pattern, PeriodType]:
        return self._regexes

    @property
    def ambiguity_time_period_filters_dict(self) -> Dict[Pattern, Pattern]:
        return self._ambiguity_time_period_filters_dict

    @property
    def time_period_connect_words(self) -> Pattern:
        return self._time_period_connect_words

    @property
    def CJK_time_regex(self) -> Pattern:
        return self._CJK_time_regex

    @property
    def left_CJK_time_regex(self) -> Pattern:
        return self._left_CJK_time_regex

    @property
    def right_CJK_time_regex(self) -> Pattern:
        return self._right_CJK_time_regex

    @property
    def digit_time_regex(self) -> Pattern:
        return self._digit_time_regex

    @property
    def left_digit_time_regex(self) -> Pattern:
        return self._left_digit_time_regex

    @property
    def right_digit_time_regex(self) -> Pattern:
        return self._right_digit_time_regex

    @property
    def short_left_CJK_time_regex(self) -> Pattern:
        return self._short_left_CJK_time_regex

    @property
    def short_left_digit_time_regex(self) -> Pattern:
        return self._short_left_digit_time_regex

    def __init__(self):
        super().__init__()
        self._short_left_digit_time_regex = RegExpUtility.get_safe_reg_exp(
            JapaneseDateTime.TimePeriodShortLeftDigitTimeRegex
        )
        self._short_left_CJK_time_regex = RegExpUtility.get_safe_reg_exp(
            JapaneseDateTime.TimePeriodShortLeftCJKTimeRegex
        )
        self._right_digit_time_regex = RegExpUtility.get_safe_reg_exp(JapaneseDateTime.TimePeriodRightDigitTimeRegex)
        self._left_digit_time_regex = RegExpUtility.get_safe_reg_exp(JapaneseDateTime.TimePeriodLeftDigitTimeRegex)
        self._digit_time_regex = JapaneseTimeExtractorConfiguration.digit_time_regex
        self._right_CJK_time_regex = RegExpUtility.get_safe_reg_exp(JapaneseDateTime.TimePeriodRightCJKTimeRegex)
        self._left_CJK_time_regex = RegExpUtility.get_safe_reg_exp(JapaneseDateTime.TimePeriodShortLeftCJKTimeRegex)
        self._CJK_time_regex = JapaneseTimeExtractorConfiguration.CJK_time_regex
        self._time_period_connect_words = RegExpUtility.get_safe_reg_exp(
            JapaneseDateTime.TimePeriodTimePeriodConnectWords
        )
        self._ambiguity_time_period_filters_dict = DefinitionLoader.load_ambiguity_filters(
            JapaneseDateTime.AmbiguityTimePeriodFiltersDict
        )
        self._regexes: Dict[Pattern, PeriodType] = {
            RegExpUtility.get_safe_reg_exp(JapaneseDateTime.TimePeriodRegexes1): PeriodType.FullTime,
            RegExpUtility.get_safe_reg_exp(JapaneseDateTime.TimePeriodRegexes2): PeriodType.ShortTime,
            RegExpUtility.get_safe_reg_exp(JapaneseDateTime.TimeOfDayRegex): PeriodType.ShortTime
        }



