
from .timex_range import TimexRange

class TimexHelpers:
    
    @staticmethod
    def expand_date_time_tange(timex):
        return TimexRange(timex, timex, timex)
