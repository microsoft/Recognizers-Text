from datetime import datetime, timedelta


class TimexDateHelpers:

    @staticmethod
    def tomorrow(date: datetime):
        return date + timedelta(days=1)

    @staticmethod
    def yesterday(date):
        return date - timedelta(days=1)

    @staticmethod
    def date_part_equal(date_x, date_y):
        return date_x == date_y

    @staticmethod
    def is_date_in_week(date, start_of_week):
        d = start_of_week
        for i in range(0, 7, 1):
            if TimexDateHelpers.date_part_equal(date, d):
                return True
            d = d + timedelta(days=1)

        return False

    @staticmethod
    def is_this_week(date, reference_date: datetime):
        start_of_week = reference_date
        while start_of_week.weekday() > 0:
            start_of_week = start_of_week - timedelta(days=1)
        return TimexDateHelpers.is_date_in_week(date, start_of_week)

    @staticmethod
    def is_next_week(date, reference_date):
        next_week_date = reference_date + timedelta(days=7)
        return TimexDateHelpers.is_this_week(date, next_week_date)

    @staticmethod
    def is_last_week(date, reference_date):
        next_week_date = reference_date - timedelta(days=7)
        return TimexDateHelpers.is_this_week(date, next_week_date)

    @staticmethod
    def week_of_year(date: datetime):
        ds = datetime(date.year, 1, 1)
        de = datetime(date.year, date.month, date.day)
        weeks = 1

        while ds < de:
            day_of_week = ds.weekday()

            if day_of_week == 6:
                weeks = weeks + 1

            ds = ds + timedelta(days=1)

        return weeks

    @staticmethod
    def fixed_format_number(n, size):
        return str(n).rjust(size, '0')

    @staticmethod
    def date_of_last_day(day: datetime, reference_date: datetime):
        result = reference_date
        result = result - timedelta(days=1)
        while result.weekday() != day:
            result = result - timedelta(days=1)

        return result

    @staticmethod
    def date_of_next_day(day: datetime, reference_date: datetime):
        result = reference_date
        result = result + timedelta(days=1)
        while result.weekday() != day:
            result = result + timedelta(days=1)

        return result

    @staticmethod
    def dates_matching_day(day, start: datetime, end: datetime):
        result = []
        d = start
        while not TimexDateHelpers.date_part_equal(d, end):
            if d.weekday() == day:
                result.append(d)
            d = d + timedelta(days=1)

        return result
