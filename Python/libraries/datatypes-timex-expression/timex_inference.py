# Copyright (c) Microsoft Corporation. All rights reserved.
# Licensed under the MIT License.

from .timex_constants import Constants

class TimexInference:

    @staticmethod
    def infer(obj):
        
        types = set()

        if TimexInference.__is_present(obj):
            types.add(Constants.TIMEX_TYPES_PRESENT)

        if TimexInference.__is_definite(obj):
            types.add(Constants.TIMEX_TYPES_DEFINITE)

        if TimexInference.__is_date(obj):
            types.add(Constants.TIMEX_TYPES_DATE)

        if TimexInference.__is_date_range(obj):
            types.add(Constants.TIMEX_TYPES_DATERANGE)

        if TimexInference.__is_duration(obj):
            types.add(Constants.TIMEX_TYPES_DURATION)

        if TimexInference.__is_time(obj):
            types.add(Constants.TIMEX_TYPES_TIME)

        if TimexInference.__is_time_range(obj):
            types.add(Constants.TIMEX_TYPES_TIMERANGE)

        if Constants.TIMEX_TYPES_PRESENT in types:
            types.add(Constants.TIMEX_TYPES_DATE)
            types.add(Constants.TIMEX_TYPES_TIME)

        if Constants.TIMEX_TYPES_TIME in types and Constants.TIMEX_TYPES_DURATION in types:
            types.add(Constants.TIMEX_TYPES_TIMERANGE)

        if Constants.TIMEX_TYPES_DATE in types and Constants.TIMEX_TYPES_TIME in types:
            types.add(Constants.TIMEX_TYPES_DATETIME)

        if Constants.TIMEX_TYPES_DATE in types and Constants.TIMEX_TYPES_DURATION in types:
            types.add(Constants.TIMEX_TYPES_DATERANGE)

        if Constants.TIMEX_TYPES_DATETIME in types and Constants.TIMEX_TYPES_DURATION in types:
            types.add(Constants.TIMEX_TYPES_DATETIMERANGE)

        if Constants.TIMEX_TYPES_DATE in types and Constants.TIMEX_TYPES_TIMERANGE in types:
            types.add(Constants.TIMEX_TYPES_DATETIMERANGE)

        return types

    @staticmethod
    def __is_present(obj):
        return obj.now == True

    @staticmethod
    def __is_duration(obj):
        return (obj.years or obj.months or obj.weeks or 
            obj.days or obj.hours or obj.minutes or obj.seconds)

    @staticmethod
    def __is_time(obj):
        return obj.hour is not None and obj.minute is not None and obj.second is not None

    @staticmethod
    def __is_date(obj):
        return (obj.month is not None and obj.day_of_month is not None) or obj.day_of_week

    @staticmethod
    def __is_time_range(obj):
        return obj.part_of_day is not None

    @staticmethod
    def __is_date_range(obj):
        return ((obj.year is not None and obj.day_of_month is None) or
                (obj.year is not None and obj.month is not None and obj.day_of_month is None) or
                (obj.month is not None and obj.day_of_month is None) or
                obj.season or obj.week_of_year or obj.week_of_month)

    @staticmethod
    def __is_definite(obj):
        return obj.year is not None and obj.month is not None and obj.day_of_month is not None

