from enum import Enum
from recognizers_text import RegExpUtility

from ...resources.chinese_date_time import ChineseDateTime
from ..constants import Constants
from .base_date_time_extractor import ChineseBaseDateTimeExtractor

class TimePeriodType(Enum):
    ShortTime = 1
    FullTime = 2

class ChineseTimePeriodExtractor(ChineseBaseDateTimeExtractor):
    @property
    def extractor_type_name(self) -> str:
        return Constants.SYS_DATETIME_TIMEPERIOD

    def __init__(self):
        super().__init__(dict([
            (RegExpUtility.get_safe_reg_exp(ChineseDateTime.TimePeriodRegexes1), TimePeriodType.FullTime),
            (RegExpUtility.get_safe_reg_exp(ChineseDateTime.TimePeriodRegexes2), TimePeriodType.ShortTime)
        ]))
