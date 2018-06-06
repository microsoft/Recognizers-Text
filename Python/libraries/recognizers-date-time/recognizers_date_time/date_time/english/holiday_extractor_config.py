from typing import List, Pattern
from recognizers_text.utilities import RegExpUtility

from ..base_holiday import HolidayExtractorConfiguration
from ...resources.english_date_time import EnglishDateTime

class EnglishHolidayExtractorConfiguration(HolidayExtractorConfiguration):
    @property
    def holiday_regexes(self) -> List[Pattern]:
        return self._holiday_regexes

    def __init__(self):
        self._holiday_regexes = [
            RegExpUtility.get_safe_reg_exp(EnglishDateTime.HolidayRegex1),
            RegExpUtility.get_safe_reg_exp(EnglishDateTime.HolidayRegex2),
            RegExpUtility.get_safe_reg_exp(EnglishDateTime.HolidayRegex3)
        ]
