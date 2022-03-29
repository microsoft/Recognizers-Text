#  Copyright (c) Microsoft Corporation. All rights reserved.
#  Licensed under the MIT License.

from typing import List, Pattern
from recognizers_text.utilities import RegExpUtility

from ..base_holiday import HolidayExtractorConfiguration
from ...resources.italian_date_time import ItalianDateTime


class ItalianHolidayExtractorConfiguration(HolidayExtractorConfiguration):
    @property
    def year_regex(self) -> Pattern:
        return self._year_regex

    @property
    def holiday_regexes(self) -> List[Pattern]:
        return self._holiday_regexes

    def __init__(self):
        self._year_regex = RegExpUtility.get_safe_reg_exp(ItalianDateTime.YearRegex)
        self._holiday_regexes = [
            RegExpUtility.get_safe_reg_exp(ItalianDateTime.HolidayRegex1),
            RegExpUtility.get_safe_reg_exp(ItalianDateTime.HolidayRegex2),
            RegExpUtility.get_safe_reg_exp(ItalianDateTime.HolidayRegex3)
        ]
