from datetime import datetime, timedelta

from .timex import Timex
from .timex_date_helpers import TimexDateHelpers
from .timex_constants import Constants


class TimexCreator:
    # The following constants are consistent with the Recognizer results

    MONDAY: str = "XXXX-WXX-1"

    TUESDAY: str = "XXXX-WXX-2"

    WEDNESDAY: str = "XXXX-WXX-3"

    THURSDAY: str = "XXXX-WXX-4"

    FRIDAY: str = "XXXX-WXX-5"

    SATURDAY: str = "XXXX-WXX-6"

    SUNDAY: str = "XXXX-WXX-7"

    MORNING: str = "(T08,T12,PT4H)"

    AFTERNOON: str = "(T12,T16,PT4H)"

    EVENING: str = "(T16,T20,PT4H)"

    DAYTIME: str = "(T08,T18,PT10H)"

    NIGHT: str = "(T20,T24,PT10H)"

    @staticmethod
    def today(date=None):
        Timex.from_date(
            datetime.now() if date is None else date).timex_value()

    @staticmethod
    def tomorrow(date=None):
        d = datetime.now() if date is None else date
        d = d.now() + timedelta(1)
        return Timex.from_date(d).timex_value()

    @staticmethod
    def yesterday(date=None):
        d = datetime.now() if date is None else date
        d = d - timedelta(1)
        return Timex.from_date(d).timex_value()

    @staticmethod
    def week_from_today(date=None):
        d = datetime.now() if date is None else date
        t = Timex.from_date(d)
        t.days = 7
        return t.timex_value()

    @staticmethod
    def week_back_today(date=None):
        d = datetime.now() if date is None else date
        d = d - timedelta(7)
        t = Timex.from_date(d)
        t.days = 7
        return t.timex_value()

    @staticmethod
    def this_week(date=None):
        d = datetime.now() if date is None else date
        d = d - timedelta(7)
        start = TimexDateHelpers.date_of_next_day(Constants.DAYS['MONDAY'], d)
        t = Timex.from_date(start)
        t.days = 7
        return t.timex_value()

    @staticmethod
    def next_week(date=None):
        d = datetime.now() if date is None else date
        start = TimexDateHelpers.date_of_next_day(Constants.DAYS['MONDAY'], d)
        t = Timex.from_date(start)
        t.days = 7
        return t.timex_value()

    @staticmethod
    def last_week(date=None):
        d = datetime if date is None else date
        start = TimexDateHelpers.date_of_next_day(Constants.DAYS['MONDAY'], d)
        start = start - timedelta(7)
        t = Timex.from_date(start)
        t.days = 7
        return t.timex_value()

    @staticmethod
    def next_weeks_from_today(n, date=None):
        d = datetime.now() if date is None else date
        start = TimexDateHelpers.date_of_next_day(Constants.DAYS['MONDAY'], d)
        t = Timex.from_date(start)
        t.days = n * 7
        return t.timex_value()
