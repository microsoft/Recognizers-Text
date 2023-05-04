from datetime import datetime, timedelta
from recognizers_date_time.date_time.utilities import DateUtils


class HolidayFunctions:

    @staticmethod
    def calculate_holiday_by_easter(year: int, days: int = 0) -> datetime:

        month = 3

        g = year % 19
        c = year / 100

        h = (c - int(c / 4) - int(((8 * c) + 13) / 25) + (19 * g) + 15) % 30
        i = h - (int(h / 28) * (1 - (int(h / 28) * int(29 / (h + 1)) * int((21 - g) / 11))))
        day = i - ((year + int(year / 4) + i + 2 - c + int(c / 4)) % 7) + 28

        if day > 31:
            month += 1
            day -= 31

        return DateUtils.safe_create_from_min_value(year, month, int(day)) + timedelta(days=days)
