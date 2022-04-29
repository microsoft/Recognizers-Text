#  Copyright (c) Microsoft Corporation. All rights reserved.
#  Licensed under the MIT License.

from typing import List, Pattern
from recognizers_text.matcher.string_matcher import StringMatcher
from ..base_timezone import TimeZoneExtractorConfiguration


class ItalianTimeZoneExtractorConfiguration(TimeZoneExtractorConfiguration):
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
