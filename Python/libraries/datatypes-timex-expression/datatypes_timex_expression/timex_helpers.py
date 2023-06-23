#  Copyright (c) Microsoft Corporation. All rights reserved.
#  Licensed under the MIT License.

from datetime import date, timedelta, datetime
from typing import List

from math import floor

from .timex_inference import TimexInference

from .date_range import DateRange
from .time import Time
from .time_range import TimeRange
from .timex_constants import Constants
from .timex_range import TimexRange


class TimexHelpers:

    @staticmethod
    def expand_datetime_range(timex):
        from datatypes_timex_expression import Timex

        types = timex.types if len(timex.types) != 0 else TimexInference.infer(timex)

        if Constants.TIMEX_TYPES_DURATION in types:
            start = TimexHelpers.clone_datetime(timex)
            duration = TimexHelpers.clone_duration(timex)
            return TimexRange(start, TimexHelpers.timex_datetime_add(start, duration), duration)

        else:
            if timex.year is not None:
                start = Timex()
                start.year = timex.year
                result = TimexRange(start, Timex())
                if timex.month is not None:
                    result.start.month = timex.month
                    result.start.day_of_month = 1
                    result.end.year = timex.year
                    result.end.month = timex.month + 1
                    result.end.day_of_month = 1
                else:
                    result.start.month = 1
                    result.start.day_of_month = 1
                    result.end.year = timex.year + 1
                    result.end.month = 1
                    result.end.day_of_month = 1
                return result

        return TimexRange(Timex(), Timex())

    @staticmethod
    def expand_time_range(timex):
        from datatypes_timex_expression import TimexCreator

        from datatypes_timex_expression import Timex
        if not (Constants.TIMEX_TYPES_TIMERANGE in timex.types):
            raise TypeError
        if timex.part_of_day is not None:
            if timex.part_of_day == 'DT':
                timex = Timex(TimexCreator.DAYTIME)
            elif timex.part_of_day == 'MO':
                timex = Timex(TimexCreator.MORNING)
            elif timex.part_of_day == 'AF':
                timex = Timex(TimexCreator.AFTERNOON)
            elif timex.part_of_day == 'EV':
                timex = Timex(TimexCreator.EVENING)
            elif timex.part_of_day == 'NI':
                timex = Timex(TimexCreator.NIGHT)
            else:
                raise TypeError

        start = Timex(hour=timex.hour, minute=timex.minute, second=timex.second)
        duration = TimexHelpers.clone_duration(timex)

        return TimexRange(start, TimexHelpers.add_time(start, duration))

    @staticmethod
    def timex_date_add(start, duration):
        from datatypes_timex_expression import Timex
        duration_days = duration.days
        if duration.days is None and duration.weeks is not None:
            duration_days = 7 * duration.weeks

        if start.day_of_week:
            end = start.clone()
            if duration.days:
                end.day_of_week += duration.days
            return end

        if start.month is not None and start.day_of_month is not None:
            if duration_days:
                if start.year:
                    d = date(start.year, start.month, start.day_of_month)
                    d = d + timedelta(days=int(duration_days))
                    result = Timex()
                    result.year = d.year
                    result.month = d.month
                    result.day_of_month = d.day
                    return result
                else:
                    d = date(2001, start.month, start.day_of_month)
                    d = d + timedelta(int(duration_days))
                    result = Timex()
                    result.month = d.month
                    result.day_of_month = d.day
                    return result
            if duration.years:
                if start.year:
                    result = Timex()
                    result.year = start.year + duration.years
                    result.month = start.month
                    result.day_of_month = start.day_of_month
                    return result
            if duration.months:
                if start.month:
                    result = Timex()
                    result.year = start.year
                    result.month = start.month + duration.months
                    result.day_of_month = start.day_of_month
                    return result
        return start

    @staticmethod
    def timex_time_add(start, duration):
        if duration.hours is not None:
            result = start.clone()
            result.hour = result.hour + int(duration.hours)
            if result.hour > 23:
                days = floor(result.hour / 24)
                hour = result.hour % 24
                result.hour = hour

                if (result.year and result.month and result.day_of_month) is not None:
                    d = datetime(result.year, result.month, result.day_of_month, 0, 0, 0)
                    d = d + timedelta(days=float(days))

                    result.year = d.year
                    result.month = d.month
                    result.day_of_month = d.day

                    return result
                if result.day_of_week is not None:
                    result.day_of_week += int(days)
                    return result
            return result

        if duration.minutes is not None:
            result = start.clone()
            result.minute += int(duration.minutes)

            if result.minute > 50:
                result.hour = result.hour + 1
                result.minute = 0

            return result
        return start

    @staticmethod
    def timex_datetime_add(start, duration):
        a = TimexHelpers.timex_date_add(start, duration)
        b = TimexHelpers.timex_time_add(a, duration)
        return b

    @staticmethod
    def date_from_timex(timex):
        return date(
            int(timex.year) if timex.year is not None else 2001,
            int(timex.month) if timex.month is not None else 1,
            int(timex.day_of_month) if timex.day_of_month is not None else 1
        )

    @staticmethod
    def time_from_timex(timex):
        return Time(
            timex.hour if timex.hour is not None else 0,
            timex.minute if timex.minute is not None else 0,
            timex.second if timex.second is not None else 0)

    @staticmethod
    def daterange_from_timex(timex):
        expanded = TimexHelpers.expand_datetime_range(timex)
        return DateRange(
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
        from datatypes_timex_expression import Timex
        result = Timex()
        result.hour = start.hour + (duration.hours if duration.hours is not None else 0)
        result.minute = start.minute + (duration.minue if duration.minutes is not None else 0)
        result.second = start.second + (duration.second if duration.seconds is not None else 0)
        return result

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
    def clone_duration(timex):
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

    @staticmethod
    def is_time_duration_timex(timex: str) -> bool:
        return timex.startswith(f"{Constants.GENERAL_PERIOD_PREFIX}{Constants.TIME_TIMEX_PREFIX}")

    @staticmethod
    def get_duration_timex_without_prefix(timex: str) -> str:
        #  Remove "PT" prefix for TimeDuration, Remove "P" prefix for DateDuration
        if TimexHelpers.is_time_duration_timex(timex):
            return timex[2:]
        else:
            return timex[1:]

    @staticmethod
    def generate_compound_duration_timex(timex_list: List[str]) -> str:
        is_time_duration_already_exist = False
        timex_builder = Constants.GENERAL_PERIOD_PREFIX

        #  The Time Duration component occurs first time
        for timex_component in timex_list:
            if not is_time_duration_already_exist and TimexHelpers.is_time_duration_timex(timex_component):
                timex_builder += f"{Constants.TIME_TIMEX_PREFIX}" \
                                 f"{TimexHelpers.get_duration_timex_without_prefix(timex_component)}"
                is_time_duration_already_exist = True
            else:
                timex_builder += f"{TimexHelpers.get_duration_timex_without_prefix(timex_component)}"

        return timex_builder
