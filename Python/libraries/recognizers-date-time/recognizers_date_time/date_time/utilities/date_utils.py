from enum import IntEnum
from datetime import datetime, timedelta
import calendar

from datedelta import datedelta

from recognizers_date_time.date_time.constants import Constants


class DayOfWeek(IntEnum):
    MONDAY = 1
    TUESDAY = 2
    WEDNESDAY = 3
    THURSDAY = 4
    FRIDAY = 5
    SATURDAY = 6
    SUNDAY = 7


class DateUtils:
    min_value = datetime(1, 1, 1, 0, 0, 0, 0)

    # Generate future/past date for cases without specific year like "Feb 29th"
    @staticmethod
    def generate_dates(no_year: bool, reference: datetime, year: int, month: int, day: int) -> list:
        future_date = DateUtils.safe_create_from_min_value(year, month, day)
        past_date = DateUtils.safe_create_from_min_value(year, month, day)
        future_year = year
        past_year = year
        if no_year:
            if DateUtils.is_Feb_29th(year, month, day):
                if DateUtils.is_leap_year(year):
                    if future_date < reference:
                        future_date = DateUtils.safe_create_from_min_value(future_year + 4, month, day)
                    else:
                        past_date = DateUtils.safe_create_from_min_value(past_year - 4, month, day)
                else:
                    past_year = past_year >> 2 << 2
                    if not DateUtils.is_leap_year(past_year):
                        past_year -= 4

                    future_year = past_year + 4
                    if not DateUtils.is_leap_year(future_year):
                        future_year += 4

                    future_date = DateUtils.safe_create_from_min_value(future_year, month, day)
                    past_date = DateUtils.safe_create_from_min_value(past_year, month, day)
            else:
                if future_date < reference and DateUtils.is_valid_date(year, month, day):
                    future_date = DateUtils.safe_create_from_min_value(year + 1, month, day)

                if past_date >= reference and DateUtils.is_valid_date(year, month, day):
                    past_date = DateUtils.safe_create_from_min_value(year - 1, month, day)
        return [future_date, past_date]

    @staticmethod
    def int_try_parse(value):
        try:
            return int(value), True
        except ValueError:
            return value, False

    @staticmethod
    def safe_create_from_value(seed: datetime, year: int, month: int, day: int,
                               hour: int = 0, minute: int = 0, second: int = 0) -> datetime:
        if DateUtils.is_valid_date(year, month, day) and DateUtils.is_valid_time(hour, minute, second):
            return datetime(year, month, day, hour, minute, second)

        return seed

    @staticmethod
    def safe_create_from_min_value(year: int, month: int, day: int,
                                   hour: int = 0, minute: int = 0, second: int = 0) -> datetime:
        return DateUtils.safe_create_from_value(DateUtils.min_value, year, month, day, hour, minute, second)

    @staticmethod
    def safe_create_from_min_value_date_time(date: datetime, time: datetime = None) -> datetime:
        return DateUtils.safe_create_from_value(DateUtils.min_value, date.year, date.month, date.day,
                                                time.hour if time else 0, time.minute if time else 0,
                                                time.second if time else 0)

    @staticmethod
    def is_valid_date(year: int, month: int, day: int) -> bool:
        try:
            datetime(year, month, day)
            return True
        except ValueError:
            return False

    @staticmethod
    def is_valid_datetime(date: datetime) -> bool:
        return date != DateUtils.min_value

    @staticmethod
    def is_valid_time(hour: int, minute: int, second: int) -> bool:
        return 0 <= hour < 24 and 0 <= minute < 60 and second >= 0 and minute < 60

    @staticmethod
    def this(from_date: datetime, day_of_week: DayOfWeek) -> datetime:
        start = from_date.isoweekday()
        target = day_of_week if day_of_week >= int(
            DayOfWeek.MONDAY) else int(DayOfWeek.SUNDAY)
        result = from_date + timedelta(days=target - start)
        return result

    @staticmethod
    def next(from_date: datetime, day_of_week: DayOfWeek) -> datetime:
        return DateUtils.this(from_date, day_of_week) + timedelta(weeks=1)

    @staticmethod
    def last(from_date: datetime, day_of_week: DayOfWeek) -> datetime:
        return DateUtils.this(from_date, day_of_week) - timedelta(weeks=1)

    @staticmethod
    def safe_create_date_resolve_overflow(year: int, month: int, day: int) -> datetime:
        if month > 12:
            year = year + month // 12
            month = month % 12
        return DateUtils.safe_create_from_min_value(year, month, day)

    @staticmethod
    def total_hours(from_date: datetime, to_date: datetime) -> int:
        return round((to_date - from_date).total_seconds() / 3600)

    @staticmethod
    def day_of_year(seed: datetime) -> int:
        return seed.timetuple().tm_yday

    @staticmethod
    def last_day_of_month(year: int, month: int) -> int:
        return calendar.monthrange(year, month)[1]

    @staticmethod
    def week_of_year(date: datetime) -> int:
        return date.isocalendar()[1]

    @staticmethod
    def is_leap_year(year) -> bool:
        return (year % 4 == 0) and (year % 100 != 0) or (year % 400 == 0)

    @staticmethod
    def is_Feb_29th(year, month, day):
        return month == 2 and day == 29

    @staticmethod
    def is_Feb_29th_datetime(date: datetime):
        return date.month == 2 and date.day == 29

    @staticmethod
    def get_last_day(year: int, month: int) -> datetime:
        month += 1

        if month == 13:
            year += 1
            month = 1
        first_day_of_next_month = DateUtils.safe_create_from_min_value(year, month, 1)

        return first_day_of_next_month - datedelta(days=1)

    @staticmethod
    def day_of_week(day):
        dayOfWeek = {
            'sunday': 0,
            'monday': 1,
            'tuesday': 2,
            'wednesday': 3,
            'thursday': 4,
            'friday': 5,
            'saturday': 6,
        }
        return dayOfWeek.get(day)

    @staticmethod
    def get_first_thursday(year: int, month: int = Constants.INVALID_MONTH) -> datetime:
        target_month = month

        if month == Constants.INVALID_MONTH:
            target_month = 1

        first_day = DateUtils.safe_create_from_min_value(year, target_month, 1)
        first_thursday = DateUtils.this(first_day, DayOfWeek.THURSDAY)

        # Thursday falls into previous year or previous month
        if first_thursday.month != target_month:
            first_thursday = first_day + datedelta(days=Constants.WEEK_DAY_COUNT)

            return first_thursday

    @staticmethod
    def get_last_thursday(year: int, month: int = Constants.INVALID_MONTH) -> datetime:
        target_month = month

        if month == Constants.INVALID_MONTH:
            target_month = 12

        last_day = DateUtils.get_last_day(year, target_month)
        last_thursday = DateUtils.this(last_day, DayOfWeek.THURSDAY)

        # Thursday falls into next year or next month
        if last_thursday.month != target_month:
            last_thursday = last_thursday - datedelta(days=Constants.WEEK_DAY_COUNT)

        return last_thursday
