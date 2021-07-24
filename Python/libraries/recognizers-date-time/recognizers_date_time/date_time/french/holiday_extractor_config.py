#  Copyright (c) Microsoft Corporation. All rights reserved.
#  Licensed under the MIT License.

from typing import List, Pattern
from recognizers_text.utilities import RegExpUtility

from ..base_holiday import HolidayExtractorConfiguration
from ...resources.french_date_time import FrenchDateTime


class FrenchHolidayExtractorConfiguration(HolidayExtractorConfiguration):
    @property
    def year_regex(self) -> Pattern:
        return self._year_regex

    @property
    def holiday_regexes(self) -> List[Pattern]:
        return self._holiday_regexes

    def __init__(self):
        self._year_regex = RegExpUtility.get_safe_reg_exp(FrenchDateTime.YearRegex)
        self._holiday_regexes = [
            RegExpUtility.get_safe_reg_exp(FrenchDateTime.HolidayRegex1),
            RegExpUtility.get_safe_reg_exp(FrenchDateTime.HolidayRegex2),
            RegExpUtility.get_safe_reg_exp(FrenchDateTime.HolidayRegex3),
            RegExpUtility.get_safe_reg_exp(FrenchDateTime.HolidayRegex4)
        ]
