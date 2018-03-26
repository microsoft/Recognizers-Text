
from .timex_range import TimexRange

class TimexHelpers:
    
    @staticmethod
    def expand_date_time_tange(timex):
        types = timex.types
        
        if Constants.TIMEX_TYPES_DURATION in types:
            start = clone_datetime(timex)
            duration = clone_duration(timex)
            return TimexRange(start, TimexHelpers.timex_datetime_add(start, duration), duration)

        else:
            if timex.year:
                start = Timex()
                start.year = timex.year
                range = TimexRange(start, Timex())
                if timex.month is not None:
                    range.start.month = timex.month
                    range.start.day_of_month = 1
                    range.end.year = timex.year
                    range.end.month = timex.month + 1
                    range.end.day_of_month = 1
                else:
                    range.start.month = 1
                    range.start.day_of_month = 1
                    range.end.year = timex.year + 1
                    range.end.month = 1
                    range.end.day_of_month = 1
                return range

        return TimexRange(Timex(), Timex())

    @staticmethod
    def expand_time_range(timex):
        start = Timex()
        start.hour, start.minute, start.second = timex.hour, timex.minute, timex.second
        duration = TimexHelpers.clone_duration()
        return TimexRange(start, TimexHelpers.add_time(start, duration), duration)

    @staticmethod
    def timex_date_add(start, duration):
        pass

    @staticmethod
    def timex_time_add(start, duration):
        pass
        
    @staticmethod
    def timex_datetime_add(start, duration):
        pass

    @staticmethod
    def date_from_timex(timex):
        pass

    @staticmethod
    def time_from_timex(timex):
        return Time(
            timex.hour if timex.hour is not null else 0,
            timex.minute if timex.minute is not null else 0,
            timex.second if timex.second is not null else 0)

    @staticmethod
    def daterange_from_timex(timex):
        expanded = TimexHelpers.expand_date_range(timex)
        return TimeRange(
            TimexHelpers.date_from_timex(expanded.start),
            TimexHelpers.date_from_timex(expanded.end))

    @staticmethod
    def timerange_from_timex(timex):
        expanded = TimexHelpers.expand_time_range(timex)
        return TimeRange(
            TimexHelpers.time_from_timex(expanded.start),
            TimexHelpers.time_from_timex(expanded.end))

    @staticmethod
    def add_time(start, duration):
        result = Timex()
        result.hour = start.hour + duration.hours if duration.hours is not None else 0
        result.minute = start.minute + duration.minue if duration.minutes is not None else 0
        result.second = start.second + duration.second if duration.seconds is not None else 0

    @staticmethod
    def clone_datetime(timex):
        result = timex.clone()
        result.years = None
        result.months = None
        result.weeks = None
        result.days = None
        result.hours = None
        result.minutes = None
        result.seconds = None
        return result

    @staticmethod
    def clone_cloneduration(timex):
        result = timex.clone()
        result.year = None
        result.month = None
        result.day_of_month = None
        result.day_of_week = None
        result.week_of_year = None
        result.week_of_month = None
        result.season = None
        result.hour = None
        result.minute = None
        result.second = None
        result.weekend = None
        result.part_of_day = None
        return result
