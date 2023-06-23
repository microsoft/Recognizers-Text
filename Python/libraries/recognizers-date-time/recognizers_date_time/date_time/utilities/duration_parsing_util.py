from typing import Dict, List
from datetime import datetime, timedelta

from recognizers_date_time.date_time.constants import Constants


class DurationParsingUtil:

    @staticmethod
    def is_time_duration_unit(uni_str: str):

        if uni_str == Constants.UNIT_H:
            result = True
        elif uni_str == Constants.UNIT_M:
            result = True
        elif uni_str == Constants.UNIT_S:
            result = True
        else:
            result = False

        return result

    @staticmethod
    def shift_date_time(timex: str, reference: datetime, future: bool) -> datetime:

        timex_unit_map = DurationParsingUtil.resolve_duration_timex(timex)
        result = DurationParsingUtil.get_shift_result(timex_unit_map, reference, future)

        return result

    @staticmethod
    def resolve_duration_timex(timex_str: str) -> Dict[str, float]:
        result = {}

        # Resolve duration timex, such as P21DT2H (21 days 2 hours)
        duration_str = timex_str.replace(Constants.GENERAL_PERIOD_PREFIX, "")
        number_start = 0
        is_time = False

        # Resolve business days
        if duration_str.endswith(Constants.TIMEX_BUSINESS_DAY):
            try:
                num_val = float(duration_str[0:-2])
                result[Constants.TIMEX_BUSINESS_DAY] = num_val
                return result
            except ValueError:
                pass

        for idx, char in enumerate(duration_str):
            if char.isalpha():
                if char == Constants.TIME_TIMEX_PREFIX:
                    is_time = True
                else:
                    num_str = duration_str[number_start:idx]
                    try:
                        number = float(num_str)
                        src_timex_unit = duration_str[idx:idx+1]
                        if not is_time and src_timex_unit == Constants.TIMEX_MONTH:
                            src_timex_unit = Constants.TIMEX_MONTH_FULL
                        result[src_timex_unit] = number
                    except ValueError:
                        return {}
                number_start = idx + 1
        return result

    @staticmethod
    def get_shift_result(timex_unit_map: Dict[str, float], reference: datetime, future: bool) -> datetime:
        result = reference
        future_or_past = 1 if future else -1

        # timexUnitMap needs to be an ordered collection because the result depends on the order of the shifts.
        # For example "1 month 21 days later" produces different results depending on whether the day or month shift is applied first
        # (when the reference month and the following month have different numbers of days).
        for unit_str, number in timex_unit_map.items():
            if unit_str == "H":
                result += timedelta(hours=number*future_or_past)
            elif unit_str == "M":
                result += timedelta(minutes=number*future_or_past)
            elif unit_str == "S":
                result += timedelta(seconds=number*future_or_past)
            elif unit_str == "H":
                result += timedelta(hours=number*future_or_past)
            elif unit_str == Constants.TIMEX_DAY:
                result += timedelta(days=number*future_or_past)
            elif unit_str == Constants.TIMEX_WEEK:
                result += timedelta(days=7*number * future_or_past)
            elif unit_str == Constants.TIMEX_MONTH_FULL:
                result.replace(month=int(number*future_or_past))
            elif unit_str == Constants.TIMEX_YEAR:
                result.replace(year=int(number*future_or_past))
            elif unit_str == Constants.TIMEX_BUSINESS_DAY:
                result = DurationParsingUtil.get_nth_business_day(result, int(number), future)
        return result

    @staticmethod
    def get_nth_business_day(start_date: datetime, n: int, is_future: bool) -> List[datetime]:
        date_list = []
        date = start_date
        for i in range(n):
            date = DurationParsingUtil.get_next_business_day(date, is_future)
            date_list.append(date)

        if not is_future:
            date_list.reverse()
        return date_list

    @staticmethod
    def get_next_business_day(start_date: datetime, is_future: bool) -> datetime:
        date_increment = 1 if is_future else -1
        date = start_date + timedelta(days=date_increment)

        # if Saturday or Sunday
        while date.weekday() == 5 or date.weekday() == 6:
            date += timedelta(days=date_increment)
        return date

    @staticmethod
    def is_date_duration(timex: str) -> bool:
        resolved_timex = DurationParsingUtil.resolve_duration_timex(timex)
        return len(resolved_timex) > 1
