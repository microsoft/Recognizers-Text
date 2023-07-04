from typing import Pattern, Dict

from recognizers_text.utilities import RegExpUtility, DefinitionLoader
from recognizers_date_time.resources.japanese_date_time import JapaneseDateTime
from recognizers_date_time.date_time.data_structures import TimeType
from recognizers_date_time.date_time.CJK.base_time import CJKTimeExtractorConfiguration


class JapaneseTimeExtractorConfiguration(CJKTimeExtractorConfiguration):

    @property
    def ambiguity_time_filters_dict(self) -> Dict[Pattern, Pattern]:
        return self._ambiguity_time_filters_dict

    @property
    def regexes(self) -> Dict[Pattern, TimeType]:
        return self._regexes

    @property
    def hour_num_regex(self) -> Pattern:
        return self._hour_num_regex

    @property
    def minute_num_regex(self) -> Pattern:
        return self._minute_num_regex

    @property
    def second_num_regex(self) -> Pattern:
        return self._second_num_regex

    @property
    def hour_CJK_regex(self) -> Pattern:
        return self._hour_CJK_regex

    @property
    def minute_CJK_regex(self) -> Pattern:
        return self._minute_CJK_regex

    @property
    def second_CJK_regex(self) -> Pattern:
        return self._second_CJK_regex

    @property
    def clock_desc_regex(self) -> Pattern:
        return self._clock_desc_regex

    @property
    def minute_desc_regex(self) -> Pattern:
        return self._minute_desc_regex

    @property
    def second_desc_regex(self) -> Pattern:
        return self._second_desc_regex

    @property
    def ban_hour_prefix_range(self) -> Pattern:
        return self._ban_hour_prefix_range

    #  e.g: 12時
    @property
    def hour_regex(self) -> Pattern:
        return self._hour_regex

    @property
    def minute_regex(self) -> Pattern:
        return self._minute_regex

    @property
    def second_regex(self) -> Pattern:
        return self._second_regex

    @property
    def half_regex(self) -> Pattern:
        return self._half_regex

    @property
    def quarter_regex(self) -> Pattern:
        return self._quarter_regex

    #  e.g: 十二五十から八|半分|瞬間
    @property
    def CJK_time_regex(self) -> Pattern:
        return self._CJK_time_regex

    # e.g: 12:23
    @property
    def digit_time_regex(self) -> Pattern:
        return self._digit_time_regex

    # e.g: 朝の9時
    @property
    def day_desc_regex(self) -> Pattern:
        return self._day_desc_regex

    @property
    def approximate_desc_prefix_regex(self) -> Pattern:
        return self._approximate_desc_prefix_regex

    @property
    def approximate_desc_suffix_regex(self) -> Pattern:
        return self._approximate_desc_suffix_regex

    def __init__(self):
        super().__init__()
        self._hour_CJK_regex = RegExpUtility.get_safe_reg_exp(JapaneseDateTime.TimeHourCJKRegex)
        self._minute_CJK_regex = RegExpUtility.get_safe_reg_exp(JapaneseDateTime.TimeMinuteCJKRegex)
        self._second_CJK_regex = RegExpUtility.get_safe_reg_exp(JapaneseDateTime.TimeSecondCJKRegex)

        self._second_num_regex = RegExpUtility.get_safe_reg_exp(JapaneseDateTime.TimeSecondNumRegex)
        self._minute_num_regex = RegExpUtility.get_safe_reg_exp(JapaneseDateTime.TimeMinuteNumRegex)
        self._hour_num_regex = RegExpUtility.get_safe_reg_exp(JapaneseDateTime.TimeHourNumRegex)

        self._clock_desc_regex = RegExpUtility.get_safe_reg_exp(JapaneseDateTime.TimeClockDescRegex)
        self._minute_desc_regex = RegExpUtility.get_safe_reg_exp(JapaneseDateTime.TimeMinuteDescRegex)
        self._second_desc_regex = RegExpUtility.get_safe_reg_exp(JapaneseDateTime.TimeSecondDescRegex)

        self._ban_hour_prefix_range = RegExpUtility.get_safe_reg_exp(JapaneseDateTime.TimeBanHourPrefixRegex)
        self._hour_regex = RegExpUtility.get_safe_reg_exp(JapaneseDateTime.TimeHourRegex)
        self._minute_regex = RegExpUtility.get_safe_reg_exp(JapaneseDateTime.TimeMinuteRegex)
        self._second_regex = RegExpUtility.get_safe_reg_exp(JapaneseDateTime.TimeSecondRegex)

        self._approximate_desc_suffix_regex = RegExpUtility.get_safe_reg_exp(
            JapaneseDateTime.TimeApproximateDescSuffixRegex
        )
        self._approximate_desc_prefix_regex = RegExpUtility.get_safe_reg_exp(
            JapaneseDateTime.TimeApproximateDescPreffixRegex
        )

        self._day_desc_regex = RegExpUtility.get_safe_reg_exp(JapaneseDateTime.TimeDayDescRegex)
        self._digit_time_regex = RegExpUtility.get_safe_reg_exp(JapaneseDateTime.TimeDigitTimeRegex)
        self._CJK_time_regex = RegExpUtility.get_safe_reg_exp(JapaneseDateTime.TimeCJKTimeRegex)
        self._quarter_regex = RegExpUtility.get_safe_reg_exp(JapaneseDateTime.TimeQuarterRegex)
        self._half_regex = RegExpUtility.get_safe_reg_exp(JapaneseDateTime.TimeHalfRegex)

        self._regexes: Dict[Pattern, TimeType] = {
            RegExpUtility.get_safe_reg_exp(JapaneseDateTime.TimeRegexes1): TimeType.CJKTime,
            RegExpUtility.get_safe_reg_exp(JapaneseDateTime.TimeRegexes2): TimeType.DigitTime,
            RegExpUtility.get_safe_reg_exp(JapaneseDateTime.TimeRegexes3): TimeType.LessTime
        }
        self._ambiguity_time_filters_dict = DefinitionLoader.load_ambiguity_filters(
            JapaneseDateTime.AmbiguityTimeFiltersDict
        )


