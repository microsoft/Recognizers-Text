from typing import List, Pattern
from recognizers_text.utilities import RegExpUtility

from recognizers_date_time.date_time.base_holiday import HolidayExtractorConfiguration
from recognizers_date_time.resources.arabic_date_time import ArabicDateTime


class ArabicHolidayExtractorConfiguration(HolidayExtractorConfiguration):

    @property
    def year_regex(self) -> Pattern:
        return self._year_regex

    @property
    def holiday_regexes(self) -> List[Pattern]:
        return self._holiday_regexes

    def __init__(self):
        self._year_regex = RegExpUtility.get_safe_reg_exp(ArabicDateTime.YearRegex)
        self._holiday_regexes = [
            RegExpUtility.get_safe_reg_exp(ArabicDateTime.HolidayRegex)
        ]
