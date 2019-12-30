from typing import List, Pattern
from recognizers_text.matcher.string_matcher import StringMatcher
from ..base_timezone import TimeZoneExtractorConfiguration
from ...resources.english_time_zone import TimeZoneDefinitions
from ..utilities import TimeZoneUtility, RegExpUtility


class FrenchTimeZoneExtractorConfiguration(TimeZoneExtractorConfiguration):
    @property
    def timezone_matcher(self):
        return self._timezone_matcher

    @property
    def direct_utc_regex(self) -> Pattern:
        return self._direct_utc_regex

    @property
    def location_time_suffix_regex(self) -> Pattern:
        return self._location_time_suffix_regex

    @property
    def location_matcher(self) -> StringMatcher:
        return self._location_matcher

    @property
    def ambiguous_timezone_list(self) -> List[str]:
        return self._ambiguous_timezone_list

    def __init__(self):
        super().__init__()
        self._direct_utc_regex = RegExpUtility.get_safe_reg_exp(TimeZoneDefinitions.DirectUtcRegex)
        self._abbreviations_list = list(TimeZoneDefinitions.AbbreviationsList)
        self._full_name_list = list(TimeZoneDefinitions.FullNameList)
        self._timezone_matcher = TimeZoneUtility.build_matcher_from_lists(self._full_name_list, self._abbreviations_list)
        self._location_time_suffix_regex = RegExpUtility.get_safe_reg_exp(TimeZoneDefinitions.LocationTimeSuffixRegex)
        self._location_matcher = StringMatcher()
        self._ambiguous_timezone_list = list(TimeZoneDefinitions.AmbiguousTimezoneList)
