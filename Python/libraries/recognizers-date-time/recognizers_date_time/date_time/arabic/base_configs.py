from typing import Pattern
from recognizers_text.utilities import RegExpUtility
from recognizers_date_time.resources.arabic_date_time import ArabicDateTime
from recognizers_date_time.date_time.base_date import DateTimeUtilityConfiguration


class ArabicDateTimeUtilityConfiguration(DateTimeUtilityConfiguration):
    @property
    def ago_regex(self) -> Pattern:
        return self._ago_regex

    @property
    def later_regex(self) -> Pattern:
        return self._later_regex

    @property
    def in_connector_regex(self) -> Pattern:
        return self._in_connector_regex

    @property
    def since_year_suffix_regex(self) -> Pattern:
        return self._since_year_suffix_regex

    @property
    def within_next_prefix_regex(self) -> Pattern:
        return self._within_next_prefix_regex

    @property
    def am_desc_regex(self) -> Pattern:
        return self._am_desc_regex

    @property
    def pm_desc__regex(self) -> Pattern:
        return self._pm_desc__regex

    @property
    def am_pm_desc_regex(self) -> Pattern:
        return self._am_pm_desc_regex

    @property
    def range_unit_regex(self) -> Pattern:
        return self._range_unit_regex

    @property
    def time_unit_regex(self) -> Pattern:
        return self._time_unit_regex

    @property
    def date_unit_regex(self) -> Pattern:
        return self._date_unit_regex

    @property
    def common_date_prefix_regex(self) -> Pattern:
        return self._common_date_prefix_regex

    @property
    def range_prefix_regex(self) -> Pattern:
        return self._range_prefix_regex

    @property
    def check_both_before_after(self) -> Pattern:
        return self._check_both_before_after

    def __init__(self):
        self._later_regex = RegExpUtility.get_safe_reg_exp(ArabicDateTime.LaterRegex)
        self._ago_regex = RegExpUtility.get_safe_reg_exp(ArabicDateTime.AgoRegex)
        self._in_connector_regex = RegExpUtility.get_safe_reg_exp(ArabicDateTime.InConnectorRegex)
        self._since_year_suffix_regex = RegExpUtility.get_safe_reg_exp(ArabicDateTime.SinceYearSuffixRegex)
        self._within_next_prefix_regex = RegExpUtility.get_safe_reg_exp(ArabicDateTime.WithinNextPrefixRegex)
        self._am_desc_regex = RegExpUtility.get_safe_reg_exp(ArabicDateTime.AmDescRegex)
        self._pm_desc__regex = RegExpUtility.get_safe_reg_exp(ArabicDateTime.PmDescRegex)
        self._am_pm_desc_regex = RegExpUtility.get_safe_reg_exp(ArabicDateTime.AmPmDescRegex)
        self._range_unit_regex = RegExpUtility.get_safe_reg_exp(ArabicDateTime.RangeUnitRegex)
        self._time_unit_regex = RegExpUtility.get_safe_reg_exp(ArabicDateTime.TimeUnitRegex)
        self._date_unit_regex = RegExpUtility.get_safe_reg_exp(ArabicDateTime.DateUnitRegex)
        self._common_date_prefix_regex = RegExpUtility.get_safe_reg_exp(ArabicDateTime.CommonDatePrefixRegex)
        self._range_prefix_regex = RegExpUtility.get_safe_reg_exp(ArabicDateTime.RangePrefixRegex)
        self._check_both_before_after = ArabicDateTime.CheckBothBeforeAfter
