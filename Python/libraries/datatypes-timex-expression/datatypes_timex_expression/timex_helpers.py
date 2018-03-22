
from .timex_range import TimexRange

class TimexHelpers:
    
    @staticmethod
    def expand_date_time_tange(timex):
        # TODO
        return TimexRange(timex, timex, timex)

    @staticmethod
    def expand_time_range(timex):
        start = Timex()
        start.hour, start.minute, start.second = timex.hour, timex.minute, timex.second
        duration = TimexHelpers.clone_duration()
        return TimexRange(start, TimexHelpers.add_time(start, duration), duration)

    @staticmethod
    def timex_date_add(timex):
        pass

    @staticmethod
    def timex_time_add(timex):
        pass
        
    @staticmethod
    def timex_datetime_add(timex):
        pass

    @staticmethod
    def date_from_timex(timex):
        pass

    @staticmethod
    def time_from_timex(timex):
        pass

    @staticmethod
    def daterange_from_timex(timex):
        pass

    @staticmethod
    def timerange_from_timex(timex):
        pass

    @staticmethod
    def add_time(start, duration):
        pass

    @staticmethod
    def clone_datetime(timex):
        pass

    @staticmethod
    def clone_cloneduration(timex):
        pass

