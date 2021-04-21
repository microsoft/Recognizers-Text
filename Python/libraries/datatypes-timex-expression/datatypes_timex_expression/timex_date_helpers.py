from datetime import datetime, timedelta


class TimexDateHelpers:

    @staticmethod
    def tomorrow(reference_date: datetime) -> datetime:
        """Next day of the reference_date"""
        return reference_date + timedelta(days=1)

    @staticmethod
    def yesterday(reference_date: datetime) -> datetime:
        """Previous day of the reference_date"""
        return reference_date - timedelta(days=1)

    @staticmethod
    def date_part_equal(date_x, date_y) -> bool:
        """Are two given dates equal?"""
        return date_x == date_y

    @staticmethod
    def is_date_in_week(date: datetime, start_of_week: datetime) -> bool:
        """Is the given date in the week starting at start_of_week?"""
        diff = date - start_of_week
        return 0 <= diff.days < 7

    @staticmethod
    def is_this_week(date: datetime, reference_date: datetime) -> bool:
        """Is the given date in the current week based on reference_date?"""
        start_of_week = reference_date - timedelta(days=reference_date.weekday())
        return TimexDateHelpers.is_date_in_week(date, start_of_week)

    @staticmethod
    def is_next_week(date: datetime, reference_date: datetime) -> bool:
        """Is the given date in the next week based on reference_date?"""
        next_week_date = reference_date + timedelta(days=7)
        return TimexDateHelpers.is_this_week(date, next_week_date)

    @staticmethod
    def is_last_week(date: datetime, reference_date: datetime) -> bool:
        """Is the given date in the last week based on reference_date?"""
        last_week_date = reference_date - timedelta(days=7)
        return TimexDateHelpers.is_this_week(date, last_week_date)

    @staticmethod
    def week_of_year(date: datetime) -> int:
        """Iso week number of a given date"""
        return date.isocalendar()[1]

    @staticmethod
    def fixed_format_number(n: int, size: int) -> str:
        return str(n).rjust(size, '0')

    @staticmethod
    def date_of_last_day(day: int, reference_date: datetime) -> datetime:
        """Date of the previous given day from reference_date (e.g. previous Wednesday)"""
        day_delta = (6 - day + reference_date.weekday()) % 7 + 1
        return reference_date - timedelta(days=day_delta)

    @staticmethod
    def date_of_next_day(day: int, reference_date: datetime) -> datetime:
        """Date of the next given day from reference_date (e.g. next Wednesday)"""
        day_delta = (6 + day - reference_date.weekday()) % 7 + 1
        return reference_date + timedelta(days=day_delta)

    @staticmethod
    def dates_matching_day(day: int, start: datetime, end: datetime) -> list:
        """All dates matching the given day that are between start and end (e.g. all Wednesday between start and end)"""
        result = []
        d = start
        while not TimexDateHelpers.date_part_equal(d, end):
            if d.weekday() == day:
                result.append(d)
            d = d + timedelta(days=1)

        return result
