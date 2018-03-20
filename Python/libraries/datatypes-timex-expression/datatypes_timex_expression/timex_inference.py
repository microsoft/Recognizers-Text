
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
        return (obj.years != None or obj.months != None or obj.weeks != None or 
            obj.days != None or obj.hours != None or obj.minutes != None or obj.seconds != None)

    @staticmethod
    def __is_time(obj):
        return obj.hour != None and obj.minute != None and obj.second != None

    @staticmethod
    def __is_date(obj):
        return (obj.month != None and obj.day_of_month != None) or obj.day_of_week != None

    @staticmethod
    def __is_time_range(obj):
        return obj.part_of_day != None

    @staticmethod
    def __is_date_range(obj):
        return ((obj.year != None and obj.day_of_month == None) or
                (obj.year != None and obj.month != None and obj.day_of_month == None) or
                (obj.month != None and obj.day_of_month == None) or
                obj.season != None or obj.week_of_year != None or obj.week_of_month != None)

    @staticmethod
    def __is_definite(obj):
        return obj.year != None and obj.month != None and obj.day_of_month != None

