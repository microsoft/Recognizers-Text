# Copyright (c) Microsoft Corporation. All rights reserved.
# Licensed under the MIT License.

import math
from datetime import date, time, timedelta
from .time import Time
from .timex import Timex
from .date_range import DateRange
from .time_range import TimeRange
from .timex_range import TimexRange
from .timex_constants import Constants

class TimexHelpers:
    
    @staticmethod
    def expand_datetime_range(timex):
        types = timex.types
        
        if Constants.TIMEX_TYPES_DURATION in types:
            start = TimexHelpers.clone_datetime(timex)
            duration = TimexHelpers.clone_duration(timex)
            return TimexRange(start, TimexHelpers.timex_datetime_add(start, duration), duration)

        else:
            if timex.year:
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
        start = Timex()
        start.hour, start.minute, start.second = timex.hour, timex.minute, timex.second
        duration = TimexHelpers.clone_duration(start)
        return TimexRange(start, TimexHelpers.add_time(start, duration), duration)

    @staticmethod
    def timex_date_add(start, duration):
        if start.day_of_week:
            end = start.clone()
            if duration.days:
                end.day_of_week += duration.days
            return end

        if start.month is not None and start.day_of_month is not None:
            if duration.days:
                if start.year:
                    d = date(start.year, start.month, start.day_of_month)
                    d = d + timedelta(days=duration.days)
                    result = Timex()
                    result.year = d.year
                    result.month = d.month
                    result.day_of_month = d.day
                    return result
                else:
                    d = date(2001, start.month, start.day_of_month)
                    d = d + timedelta(duration.days)
                    result = Timex()
                    result.month = d.month
                    result.day_of_month = d.day_of_month
                    return result
            if duration.years:
                if start.year:
                    result = Timex()
                    result.year = start.year + duration.years
                    result.month = start.month
                    return result
            if duration.month:
                if start.month:
                    result = Timex()
                    result.year = start.year
                    result.month = start.month + duration.months
                    result.day_of_month = start.day_of_month
                    return result
        return start

    @staticmethod
    def timex_time_add(start, duration):
        if duration.hours:
            result = start.clone()
            result.hour += duration.hours
            if result.hour > 23:
                days = math.floor(result.hour / 24)
                hour = result.hour % 24
                result.hour = hour
                if result.year is not None and result.month is not None and result.day_of_month is not None:
                    d = date(result.year, result.month, result.day_of_month)
                    d = d + timedelta(days=days)
                    result.year = d.year
                    result.month = d.month
                    result.day_of_month = d.day
                    return result
                if result.day_of_week:
                    result.Day_of_week += days
                    return result
            return result

        if duration.minutes:
            result = start.clone()
            result.minute += duration.minutes
            if result.minute > 59:
                result.hour += 1
                result.minute = 0
            return result
        return start

    @staticmethod
    def timex_datetime_add(start, duration):
        return TimexHelpers.timex_time_add(
            TimexHelpers.timex_date_add(start, duration), duration)

    @staticmethod
    def date_from_timex(timex):
        return date(
            timex.year if timex.year is not None else 0,
            timex.month if timex.month is not None else 0,
            timex.day_of_month if timex.day_of_month is not None else 0)

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
        result = Timex()
        result.hour = start.hour + (duration.hours if duration.hours is not None else 0)
        result.minute = start.minute + (duration.minue if duration.minutes is not None else 0)
        result.second = start.second + (duration.second if duration.seconds is not None else 0)

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
