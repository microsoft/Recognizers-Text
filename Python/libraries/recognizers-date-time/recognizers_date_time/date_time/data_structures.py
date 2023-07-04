from enum import Enum


class TimeType(Enum):
    CJKTime = 1
    LessTime = 2
    DigitTime = 3


class PeriodType(Enum):
    ShortTime = 1
    FullTime = 2


class DatePeriodTimexType(Enum):
    ByDay = 1
    ByWeek = 2
    ByFortNight = 3
    ByMonth = 4
    ByYear = 5
