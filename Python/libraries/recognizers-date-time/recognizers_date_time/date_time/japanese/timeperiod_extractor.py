#  Copyright (c) Microsoft Corporation. All rights reserved.
#  Licensed under the MIT License.

from enum import Enum
from recognizers_text import RegExpUtility

from ...resources.japanese_date_time import JapaneseDateTime
from ..constants import Constants
from .base_date_time_extractor import JapaneseBaseDateTimeExtractor


class TimePeriodType(Enum):
    ShortTime = 1
    FullTime = 2


class JapaneseTimePeriodExtractor(JapaneseBaseDateTimeExtractor):
    @property
    def extractor_type_name(self) -> str:
        return Constants.SYS_DATETIME_TIMEPERIOD

    def __init__(self):
        super().__init__(dict([
            (RegExpUtility.get_safe_reg_exp(
                JapaneseDateTime.TimePeriodRegexes1), TimePeriodType.FullTime),
            (RegExpUtility.get_safe_reg_exp(
                JapaneseDateTime.TimePeriodRegexes2), TimePeriodType.ShortTime),
            (RegExpUtility.get_safe_reg_exp(
                JapaneseDateTime.TimeOfDayRegex), TimePeriodType.ShortTime)
        ]))
