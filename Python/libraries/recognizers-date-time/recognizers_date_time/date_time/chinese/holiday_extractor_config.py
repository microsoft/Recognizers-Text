from typing import List, Pattern

from recognizers_text import RegExpUtility

from ...resources.chinese_date_time import ChineseDateTime
from ..base_holiday import HolidayExtractorConfiguration

class ChineseHolidayExtractorConfiguration(HolidayExtractorConfiguration):
    @property
    def holiday_regexes(self) -> List[Pattern]:
        return self._holiday_regexes

    def __init__(self):
        self._holiday_regexes = [
            RegExpUtility.get_safe_reg_exp(ChineseDateTime.HolidayRegexList1),
            RegExpUtility.get_safe_reg_exp(ChineseDateTime.HolidayRegexList2),
            RegExpUtility.get_safe_reg_exp(ChineseDateTime.LunarHolidayRegex)
        ]
