from datetime import datetime, timedelta

from datatypes_timex_expression import Timex, TimexInference, Constants, TimexValue, TimexHelpers, TimexDateHelpers
from datatypes_timex_expression.resolution import Resolution, Entry


class TimexResolver:

    def resolve(self, timex_array: [str], date: datetime = None):
        resolution = Resolution()
        for timex in timex_array:
            t = Timex(timex)
            r = self.resolve_timex(t, date)
            resolution.values().extend(r)

        return resolution

    def resolve_timex(self, timex: Timex, date: datetime):
        types = timex.types() if len(timex.types()) != 0 else TimexInference.infer(timex)

        if Constants.TIMEX_TYPES_DATETIMERANGE in types:
            return self.resolve_date_timerange(timex)
        if Constants.TIMEX_TYPES_DEFINITE in types and Constants.TIMEX_TYPES_TIME in types:
            return self.resolve_definite_time(timex)
        if Constants.TIMEX_TYPES_DEFINITE in types and Constants.TIMEX_TYPES_DATERANGE in types:
            return self.resolve_definite_daterange(timex)
        if Constants.TIMEX_TYPES_DATERANGE in types:
            return self.resolve_date_range(timex, date)
        if Constants.TIMEX_TYPES_DATETIME in types:
            return self.resolve_date_time(timex, date)
        if Constants.TIMEX_TYPES_DURATION in types:
            return self.resolve_duration(timex)
        if Constants.TIMEX_TYPES_DATE in types:
            return self.resolve_date(timex, date)
        if Constants.TIMEX_TYPES_TIME in types:
            return self.resolve_time(timex)

        return [Entry()]

    @staticmethod
    def resolve_definite_time(timex: Timex):
        entry = Entry()
        entry.timex = timex.timex_value()
        entry.type = 'datetime'
        entry.value = f'{TimexValue.date_value(timex)} {TimexValue.time_value(timex)}'

    @staticmethod
    def resolve_definite(timex: Timex):
        entry = Entry()
        entry.timex = timex.timex_value()
        entry.type = 'date'
        entry.value = TimexValue.date_value(timex)

        return [entry]

    @staticmethod
    def resolve_definite_daterange(timex: Timex):
        date_range = TimexHelpers.expand_datetime_range(timex)

        entry = Entry()
        entry.timex = timex.timex_value()
        entry.type = 'daterange'
        entry.start = TimexValue.date_value(date_range.start)
        entry.end = TimexValue.date_value(date_range.end)

    def resolve_date(self, timex: Timex, date: datetime):
        entry1 = Entry()
        entry1.timex = timex.timex_value()
        entry1.type = 'date'
        entry1.value = self.last_date_value(timex, date)

        entry2 = Entry()
        entry2.timex = timex.timex_value()
        entry2.type = 'date'
        entry2.value = self.next_date_value(timex, date)

        return [entry1, entry2]

    @staticmethod
    def last_date_value(timex: Timex, date: datetime):
        if (timex.month and timex.day_of_month) is not None:
            return TimexValue.date_value(Timex(date.year - 1, timex.month, timex.day_of_month))

        if timex.day_of_month is not None:
            day = Constants.DAYS['SUNDAY'] if timex.day_of_week == 7 else timex.day_of_week
            result = TimexDateHelpers.date_of_last_day(day, date)
            return TimexValue.date_value(Timex(result.year, result.month, result.day))

        return ''

    @staticmethod
    def next_date_value(timex: Timex, date: datetime):
        if (timex.month and timex.day_of_month) is not None:
            return TimexValue.date_value(Timex(date.year, timex.month, timex.day_of_month))

        if timex.day_of_month is not None:
            day = Constants.DAYS['SUNDAY'] if timex.day_of_week == 7 else timex.day_of_week
            result = TimexDateHelpers.date_of_next_day(day, date)
            return TimexValue.date_value(Timex(result.year, result.month, result.day))

        return ''

    @staticmethod
    def resolve_time(timex: Timex):
        entry = Entry()

        entry.timex = timex.timex_value()
        entry.type = 'time'
        entry.value = TimexValue.time_value(timex)

        return [entry]

    @staticmethod
    def resolve_duration(timex: Timex):
        entry = Entry()

        entry.timex = timex.timex_value()
        entry.type = 'duration'
        entry.value = TimexValue.duration_value(timex)

        return [entry]

    @staticmethod
    def year_date_range(year: int) -> (Timex, Timex):
        return TimexValue.date_value(Timex(year, 1, 1)), TimexValue.date_value(Timex(year + 1, 1, 1))

    @staticmethod
    def month_date_range(year: int, month: int):
        return TimexValue.date_value(Timex(year, month, 1)), TimexValue.date_value(Timex(year, month + 1, 1))

    @staticmethod
    def week_date_range(year: int, week_of_year: int):
        date_in_week = datetime(year, 1, 1) + \
            timedelta(days=(week_of_year-1)*7)
        start = TimexDateHelpers.date_of_last_day(
            Constants.DAYS['MONDAY'], date_in_week)
        end = TimexDateHelpers.date_of_last_day(
            Constants.DAYS['MONDAY'], date_in_week + timedelta(days=7))

        return TimexValue.date_value(Timex(start.year, start.month, start.day)), TimexValue.date_value(
            Timex(end.year, end.month, end.day))

    def resolve_date_range(self, timex: Timex, date: datetime):
        if timex.season is not None:
            entry = Entry()

            entry.timex = timex.timex_value()
            entry.type = 'daterange'
            entry.value = 'not resolved'

            return [entry]
        else:
            if (timex.year and timex.month) is not None:
                date_range = self.month_date_range(timex.year, timex.month)
                entry = Entry()

                entry.timex = timex.timex_value()
                entry.type = 'daterange'
                entry.start = date_range[0]
                entry.end = date_range[1]

                return [entry]

            if (timex.year and timex.week_of_year) is not None:
                date_range = self.week_date_range(
                    timex.year, timex.week_of_year)
                entry = Entry()

                entry.timex = timex.timex_value()
                entry.type = 'daterange'
                entry.start = date_range[0]
                entry.end = date_range[1]

                return [entry]

            if timex.month is not None:
                y = date.year
                last_year_date_range = self.month_date_range(
                    y - 1, timex.month)
                this_year_date_range = self.month_date_range(y, timex.month)

                entry1 = Entry()

                entry1.timex = timex.timex_value()
                entry1.type = 'duration'
                entry1.start = last_year_date_range[0]
                entry1.end = last_year_date_range[1]

                entry2 = Entry()

                entry2.timex = timex.timex_value()
                entry2.type = 'duration'
                entry2.start = this_year_date_range[0]
                entry2.end = this_year_date_range[1]

                return [entry1, entry2]

            if timex.year is not None:
                date_range = self.year_date_range(timex.year)

                entry = Entry()

                entry.timex = timex.timex_value()
                entry.type = 'daterange'
                entry.start = date_range[0]
                entry.end = date_range[1]

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

    def resolve_time_range(self, timex: Timex):
        if timex.part_of_day is not None:
            range = self.part_of_day_timerange(timex)

            entry = Entry()

            entry.timex = timex.timex_value()
            entry.type = 'timerange'
            entry.start = range[0]
            entry.end = range[1]

            return [entry]

        else:
            range = TimexHelpers.expand_time_range(timex)
            entry = Entry()

            entry.timex = timex.timex_value()
            entry.type = 'daterange'
            entry.start = TimexValue.time_value(range.start)
            entry.end = TimexValue.time_value(range.end)

            return [entry]

    def resolve_date_time(self, timex: Timex, date: datetime):
        resolved_dates = self.resolve_date(timex, date)

        for resolved in resolved_dates:
            resolved.type = 'datetime'
            resolved.value = f'{resolved.value} {TimexValue.time_value(timex)}'

        return resolved_dates

    def resolve_date_timerange(self, timex: Timex):
        if timex.part_of_day is not None:
            date = TimexValue.date_value(timex)
            time_range = self.part_of_day_timerange(timex)

            entry = Entry()

            entry.timex = timex.timex_value()
            entry.type = 'datetimerange'
            entry.start = f'{date} {time_range[0]}'
            entry.end = f'{date} {time_range[1]}'

            return [entry]
        else:
            time_range = TimexHelpers.expand_datetime_range(timex)

            entry = Entry()

            entry.timex = timex.timex_value()
            entry.type = 'datetimerange'
            entry.start = f'{TimexValue.date_value(time_range.start)} {TimexValue.time_value(time_range.start)}'
            entry.end = f'{TimexValue.date_value(time_range.end)} {TimexValue.time_value(time_range.end)}'

            return [entry]
