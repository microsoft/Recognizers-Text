#  Copyright (c) Microsoft Corporation. All rights reserved.
#  Licensed under the MIT License.

from enum import Enum
from recognizers_text import RegExpUtility

from ...resources.japanese_date_time import JapaneseDateTime
from ..constants import Constants
from .base_date_time_extractor import JapaneseBaseDateTimeExtractor


class TimeType(Enum):
    JapaneseTime = 1
    LessTime = 2
    DigitTime = 3


class JapaneseTimeExtractor(JapaneseBaseDateTimeExtractor):
    @property
    def extractor_type_name(self) -> str:
        return Constants.SYS_DATETIME_TIME

    def __init__(self):
        super().__init__(dict([
            (RegExpUtility.get_safe_reg_exp(
                JapaneseDateTime.TimeRegexes1), TimeType.JapaneseTime),
            (RegExpUtility.get_safe_reg_exp(
                JapaneseDateTime.TimeRegexes2), TimeType.DigitTime),
            (RegExpUtility.get_safe_reg_exp(
                JapaneseDateTime.TimeRegexes3), TimeType.LessTime)
        ]))
