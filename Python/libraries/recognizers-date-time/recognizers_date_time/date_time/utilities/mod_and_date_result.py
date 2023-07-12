import datetime
from typing import List

from datedelta import datedelta

from recognizers_date_time.date_time.utilities import DurationParsingUtil
from recognizers_date_time.date_time import Constants


class ModAndDateResult:

    def __init__(self):
        self.date_list = None
        self.end_date = None
        self.begin_date = None
        self.mod = None

    def mod_and_date_result(self, begin_date: datetime, end_date: datetime, mod: str = '',
                            date_list: [datetime] = None):
        self.begin_date = begin_date
        self.end_date = end_date
        self.mod = mod
        self.date_list = date_list

    def get_mod_and_date(self, begin_date: datetime, end_date: datetime, reference: datetime, timex: str, future: bool):
        begin_date_result = begin_date
        end_date_result = end_date

        is_business_day = timex.endswith(Constants.TIMEX_BUSINESS_DAY)
        business_day_count = 0
        date_list: List[datetime] = None

        if is_business_day:
            business_day_count = timex[1: len(timex) - 3]

        if future:
            mod = Constants.AFTER_MOD

            # For future the beginDate should add 1 first
            if is_business_day:
                begin_date_result = DurationParsingUtil.get_next_business_day(reference, future)
                end_date_result = DurationParsingUtil.get_nth_business_day(begin_date_result, business_day_count - 1,
                                                                           future)
                end_date_result = end_date_result + datedelta(days=1)
                return self.mod_and_date_result(begin_date_result, end_date_result, mod, date_list)
            else:
                begin_date_result = reference + datedelta(days=1)
                end_date_result = DurationParsingUtil.shift_date_time(timex, begin_date_result, future)
                return self.mod_and_date_result(begin_date_result, end_date_result, mod, None)
        else:
            mod = Constants.BEFORE_MOD

            if is_business_day:
                begin_date_result = DurationParsingUtil.get_nth_business_day(end_date_result, business_day_count - 1,
                                                                             future)
                end_date_result = DurationParsingUtil.get_next_business_day(end_date_result, future)
                end_date_result = end_date_result + datedelta(days=1)
                return self.mod_and_date_result(begin_date_result, end_date_result, mod, date_list)
            else:
                begin_date_result = DurationParsingUtil.shift_date_time(timex, end_date_result, future)
                return self.mod_and_date_result(begin_date_result, end_date_result, mod, None)
