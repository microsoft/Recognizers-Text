from typing import List, Pattern

from recognizers_text.utilities import RegExpUtility
from recognizers_date_time.date_time.CJK import CJKHolidayExtractorConfiguration
from recognizers_date_time.resources.japanese_date_time import JapaneseDateTime


class JapaneseHolidayExtractorConfiguration(CJKHolidayExtractorConfiguration):
    @property
    def holiday_regexes(self) -> List[Pattern]:
        return self._holiday_regexes

    @property
    def lunar_holiday_regex(self) -> Pattern:
        return self._lunar_holiday_regex

    @property
    def holiday_regex_list(self) -> List[Pattern]:
        return self._holiday_regexes

    def __init__(self):
        super().__init__()
        self._lunar_holiday_regex = RegExpUtility.get_safe_reg_exp(JapaneseDateTime.LunarHolidayRegex)
        self._holiday_regex_list = [
            RegExpUtility.get_safe_reg_exp(JapaneseDateTime.HolidayRegexList1),
            RegExpUtility.get_safe_reg_exp(JapaneseDateTime.HolidayRegexList2),
            self._lunar_holiday_regex
        ]
        self._holiday_regexes = self._holiday_regex_list
