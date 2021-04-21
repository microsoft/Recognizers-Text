from datetime import datetime, timedelta

from datatypes_timex_expression import Timex, TimexInference, Constants, TimexValue, TimexHelpers, TimexDateHelpers
from datatypes_timex_expression.resolution import Resolution, Entry


class TimexResolver:
    @staticmethod
    def resolve(timex_array: [str], date: datetime = None):
        resolution = Resolution()
        for timex in timex_array:
            t = Timex(timex)
            r = TimexResolver.resolve_timex(t, date)
            resolution.values.extend(r)

        return resolution

    @staticmethod
    def resolve_timex(timex: Timex, date: datetime):
        types = timex.types if len(timex.types) != 0 else TimexInference.infer(timex)

        if Constants.TIMEX_TYPES_DATETIMERANGE in types:
            return TimexResolver.resolve_date_timerange(timex)
        if Constants.TIMEX_TYPES_DEFINITE in types and Constants.TIMEX_TYPES_TIME in types:
            return TimexResolver.resolve_definite_time(timex)
        if Constants.TIMEX_TYPES_DEFINITE in types and Constants.TIMEX_TYPES_DATERANGE in types:
            return TimexResolver.resolve_definite_daterange(timex)
        if Constants.TIMEX_TYPES_DATERANGE in types:
            return TimexResolver.resolve_date_range(timex, date)
        if Constants.TIMEX_TYPES_DEFINITE in types:
            return TimexResolver.resolve_definite(timex)
        if Constants.TIMEX_TYPES_TIMERANGE in types:
            return TimexResolver.resolve_time_range(timex)
        if Constants.TIMEX_TYPES_DATETIME in types:
            return TimexResolver.resolve_date_time(timex, date)
        if Constants.TIMEX_TYPES_DURATION in types:
            return TimexResolver.resolve_duration(timex)
        if Constants.TIMEX_TYPES_DATE in types:
            return TimexResolver.resolve_date(timex, date)
        if Constants.TIMEX_TYPES_TIME in types:
            return TimexResolver.resolve_time(timex)

        return [Entry()]

    @staticmethod
    def resolve_definite_time(timex: Timex):
        entry = Entry()
        entry.timex = timex.timex_value()
        entry.type = 'datetime'
        entry.value = f'{TimexValue.date_value(timex)} {TimexValue.time_value(timex)}'
        entry.start = None
        entry.end = None

        return [entry]

    @staticmethod
    def resolve_definite(timex: Timex):
        entry = Entry()
        entry.timex = timex.timex_value()
        entry.type = 'date'
        entry.value = TimexValue.date_value(timex)
        entry.start = None
        entry.end = None

        return [entry]

    @staticmethod
    def resolve_definite_daterange(timex: Timex):
        date_range = TimexHelpers.expand_datetime_range(timex)

        entry = Entry()
        entry.timex = timex.timex_value()
        entry.type = 'daterange'
        entry.start = TimexValue.date_value(date_range.start)
        entry.end = TimexValue.date_value(date_range.end)

        return [entry]

    @staticmethod
    def resolve_date(timex: Timex, date: datetime):
        entry1 = Entry()
        entry1.timex = timex.timex_value()
        entry1.type = 'date'
        entry1.value = TimexResolver.last_date_value(timex, date)
        entry1.start = None
        entry1.end = None

        entry2 = Entry()
        entry2.timex = timex.timex_value()
        entry2.type = 'date'
        entry2.value = TimexResolver.next_date_value(timex, date)
        entry2.start = None
        entry2.end = None

        return [entry1, entry2]

    @staticmethod
    def last_date_value(timex: Timex, date: datetime):
        if (timex.month and timex.day_of_month) is not None:
            a = TimexValue.date_value(Timex(year=date.year - 1, month=timex.month, day_of_month=timex.day_of_month))
            return a

        if timex.day_of_week is not None:
            day = Constants.DAYS['SUNDAY'] if timex.day_of_week == 6 else timex.day_of_week
            result = TimexDateHelpers.date_of_last_day(day - 1, date)
            return TimexValue.date_value(Timex(year=result.year, month=result.month, day_of_month=result.day))

        return ''

    @staticmethod
    def next_date_value(timex: Timex, date: datetime):
        if (timex.month and timex.day_of_month) is not None:
            return TimexValue.date_value(Timex(year=date.year, month=timex.month, day_of_month=timex.day_of_month))

        if timex.day_of_week is not None:
            day = Constants.DAYS['SUNDAY'] if timex.day_of_week == 6 else timex.day_of_week
            result = TimexDateHelpers.date_of_next_day(day - 1, date)
            return TimexValue.date_value(Timex(year=result.year, month=result.month, day_of_month=result.day))

        return ''

    @staticmethod
    def resolve_time(timex: Timex):
        entry = Entry()

        entry.timex = timex.timex_value()
        entry.type = 'time'
        entry.value = TimexValue.time_value(timex)
        entry.start = None
        entry.end = None

        return [entry]

    @staticmethod
    def resolve_duration(timex: Timex):
        entry = Entry()

        entry.timex = timex.timex_value()
        entry.type = 'duration'
        entry.value = TimexValue.duration_value(timex)
        entry.start = None
        entry.end = None

        return [entry]

    @staticmethod
    def year_date_range(year: int) -> (Timex, Timex):
        return TimexValue.date_value(Timex(year=year, month=1, day_of_month=1)), TimexValue.date_value(Timex(year=year+1, month=1, day_of_month=1))

    @staticmethod
    def month_date_range(year: int, month: int):
        return TimexValue.date_value(Timex(year=year, month=month, day_of_month=1)), TimexValue.date_value(Timex(year=year, month=month + 1, day_of_month=1))

    @staticmethod
    def week_date_range(year: int, week_of_year: int):
        """Returns start and end dates of the given ISO week number of a given year"""
        date_in_week = datetime(year, 1, 1)

        # From Wikipedia: The ISO 8601 definition for week 01 is the week with
        # the first Thursday of the Gregorian year (i.e. of January) in it.
        if date_in_week.weekday() <= 3:
            date_in_week -= timedelta(days=date_in_week.weekday())
        else:
            date_in_week += timedelta(days=7 - date_in_week.weekday())

        date_in_week += timedelta(days=week_of_year * 7)

        start = TimexDateHelpers.date_of_last_day(
            Constants.DAYS['MONDAY'], date_in_week)
        end = TimexDateHelpers.date_of_last_day(
            Constants.DAYS['MONDAY'], date_in_week + timedelta(days=7))

        return TimexValue.date_value(Timex(year=start.year, month=start.month, day_of_month=start.day)), \
               TimexValue.date_value(Timex(year=start.year, month=start.month, day_of_month=end.day))

    @staticmethod
    def resolve_date_range(timex: Timex, date: datetime):
        if timex.season is not None:
            entry = Entry()

            entry.timex = timex.timex_value()
            entry.type = 'daterange'
            entry.value = 'not resolved'
            entry.start = None
            entry.end = None

            return [entry]
        else:
            if (timex.year and timex.month) is not None:
                date_range = TimexResolver.month_date_range(timex.year, timex.month)
                entry = Entry()

                entry.timex = timex.timex_value()
                entry.type = 'daterange'
                entry.start = date_range[0]
                entry.end = date_range[1]
                entry.value = None

                return [entry]

            if (timex.year and timex.week_of_year) is not None:
                date_range = TimexResolver.week_date_range(
                    timex.year, timex.week_of_year)
                entry = Entry()

                entry.timex = timex.timex_value()
                entry.type = 'daterange'
                entry.start = date_range[0]
                entry.end = date_range[1]
                entry.value = None

                return [entry]

            if timex.month is not None:
                y = date.year
                last_year_date_range = TimexResolver.month_date_range(
                    y - 1, timex.month)
                this_year_date_range = TimexResolver.month_date_range(y, timex.month)

                entry1 = Entry()

                entry1.timex = timex.timex_value()
                entry1.type = 'daterange'
                entry1.start = last_year_date_range[0]
                entry1.end = last_year_date_range[1]
                entry1.value = None

                entry2 = Entry()

                entry2.timex = timex.timex_value()
                entry2.type = 'daterange'
                entry2.start = this_year_date_range[0]
                entry2.end = this_year_date_range[1]
                entry2.value = None

                return [entry1, entry2]

            if timex.year is not None:
                date_range = TimexResolver.year_date_range(timex.year)

                entry = Entry()

                entry.timex = timex.timex_value()
                entry.type = 'daterange'
                entry.start = date_range[0]
                entry.end = date_range[1]
                entry.value = None

                return [entry]

            return [Entry()]

    @staticmethod
    def part_of_day_timerange(timex: Timex):
        if timex.part_of_day == 'MO':
            return '08:00:00', '12:00:00'
        if timex.part_of_day == 'AF':
            return '12:00:00', '16:00:00'
        if timex.part_of_day == 'EV':
            return '16:00:00', '20:00:00'
        if timex.part_of_day == 'NI':
            return '20:00:00', '24:00:00'

        return 'not resolved', 'not resolved'

    @staticmethod
    def resolve_time_range(timex: Timex):
        if timex.part_of_day is not None:
            range = TimexResolver.part_of_day_timerange(timex)

            entry = Entry()

            entry.timex = timex.timex_value()
            entry.type = 'timerange'
            entry.start = range[0]
            entry.end = range[1]
            entry.value = None

            return [entry]

        else:
            range = TimexHelpers.expand_time_range(timex)
            entry = Entry()

            entry.timex = timex.timex_value()
            entry.type = 'timerange'
            entry.start = TimexValue.time_value(range.start)
            entry.end = TimexValue.time_value(range.end)
            entry.value = None

            return [entry]

    @staticmethod
    def resolve_date_time(timex: Timex, date: datetime):
        resolved_dates = TimexResolver.resolve_date(timex, date)

        for resolved in resolved_dates:
            resolved.type = 'datetime'
            resolved.value = f'{resolved.value} {TimexValue.time_value(timex)}'

        return resolved_dates

    @staticmethod
    def resolve_date_timerange(timex: Timex):
        if timex.part_of_day is not None:
            date = TimexValue.date_value(timex)
            time_range = TimexResolver.part_of_day_timerange(timex)

            entry = Entry()

            entry.timex = timex.timex_value()
            entry.type = 'datetimerange'
            entry.start = f'{date} {time_range[0]}'
            entry.end = f'{date} {time_range[1]}'
            entry.value = None

            return [entry]
        else:
            time_range = TimexHelpers.expand_datetime_range(timex)

            entry = Entry()

            entry.timex = timex.timex_value()
            entry.type = 'datetimerange'
            entry.start = f'{TimexValue.date_value(time_range.start)} {TimexValue.time_value(time_range.start)}'
            entry.end = f'{TimexValue.date_value(time_range.end)} {TimexValue.time_value(time_range.end)}'
            entry.value = None

            return [entry]
