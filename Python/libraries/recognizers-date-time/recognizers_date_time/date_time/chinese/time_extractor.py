from enum import Enum
from recognizers_text import RegExpUtility

from ...resources.chinese_date_time import ChineseDateTime
from ..constants import Constants
from .base_date_time_extractor import ChineseBaseDateTimeExtractor

class TimeType(Enum):
    ChineseTime = 1
    LessTime = 2
    DigitTime = 3

class ChineseTimeExtractor(ChineseBaseDateTimeExtractor):
    @property
    def extractor_type_name(self) -> str:
        return Constants.SYS_DATETIME_TIME

    def __init__(self):
        super().__init__(dict([
            (RegExpUtility.get_safe_reg_exp(ChineseDateTime.TimeRegexes1), TimeType.ChineseTime),
            (RegExpUtility.get_safe_reg_exp(ChineseDateTime.TimeRegexes2), TimeType.DigitTime),
            (RegExpUtility.get_safe_reg_exp(ChineseDateTime.TimeRegexes3), TimeType.LessTime)
        ]))
