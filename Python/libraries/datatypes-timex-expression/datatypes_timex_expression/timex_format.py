
from .timex_constants import Constants
from .timex_helpers import TimexHelpers
from .timex_date_helpers import TimexDateHelpers

class TimexFormat:

    @staticmethod
    def format(timex):

        types = timex.types

        if Constants.TIMEX_TYPES_PRESENT in types:
            return 'PRESENT_REF'

        if ((Constants.TIMEX_TYPES_DATETIMERANGE in types or
            Constants.TIMEX_TYPES_DATERANGE in types or
            Constants.TIMEX_TYPES_TIMERANGE in types) and
            Constants.TIMEX_TYPES_DURATION in types):
            range = TimexHelpers.expand_date_time_tange(timex)
            return '({},{},{})'.format(
                TimexFormat.format(range.start),
                TimexFormat.format(range.end),
                TimexFormat.format(range.duration))

        if Constants.TIMEX_TYPES_DATETIMERANGE in types:
            return '{}{}'.format(
                TimexFormat.format_date(timex),
                TimexFormat.format_time_range(timex))

        if Constants.TIMEX_TYPES_DATERANGE in types:
            return TimexFormat.format_date_range(timex)

        if Constants.TIMEX_TYPES_TIMERANGE in types:
            return TimexFormat.format_time_range(timex)
        
        if Constants.TIMEX_TYPES_DATETIME in types:
            return '{}{}'.format(
                TimexFormat.format_date(timex),
                TimexFormat.format_time(timex))
        
        if Constants.TIMEX_TYPES_DURATION in types:
            return TimexFormat.format_duration(timex)

        if Constants.TIMEX_TYPES_DATE in types:
            return TimexFormat.format_date(timex)

        if Constants.TIMEX_TYPES_TIME in types:
            return TimexFormat.format_time(timex)

        return ''

    @staticmethod
    def format_duration(timex):
        if timex.years != None:
            return 'P{}Y'.format(timex.years)

        if timex.months != None:
            return 'P{}M'.format(timex.months)

        if timex.weeks != None:
            return 'P{}W'.format(timex.weeks)

        if timex.days != None:
            return 'P{}D'.format(timex.days)

        if timex.hours != None:
            return 'PT{}H'.format(timex.hours)

        if timex.minutes != None:
            return 'PT{}M'.format(timex.minutes)

        if timex.seconds != None:
            return 'PT{}S'.format(timex.seconds)

        return ''

    @staticmethod
    def format_time(timex):
        if timex.minute == 0 and timex.second == 0:
            return 'T{}'.format(TimexDateHelpers.fixed_format_number(timex.hour, 2))

        if timex.second == 0:
            return 'T{}:{}'.format(
                TimexDateHelpers.fixed_format_number(timex.hour, 2),
                TimexDateHelpers.fixed_format_number(timex.minute, 2))

        return 'T{}:{}:{}'.format(
            TimexDateHelpers.fixed_format_number(timex.hour, 2),
            TimexDateHelpers.fixed_format_number(timex.minute, 2),
            TimexDateHelpers.fixed_format_number(timex.second, 2))
            
    @staticmethod
    def format_date(timex):
        if timex.year != None and timex.month != None and timex.day_of_month != None:
            return '{}-{}-{}'.format(
                TimexDateHelpers.fixed_format_number(timex.Year, 4),
                TimexDateHelpers.fixed_format_number(timex.Month, 2),
                TimexDateHelpers.fixed_format_number(timex.DayOfMonth, 2))

        if timex.month != None and timex.day_of_month != None:
            return 'XXXX-{}-{}'.format(
                TimexDateHelpers.fixed_format_number(timex.month, 2),
                TimexDateHelpers.fixed_format_number(timex.day_of_month, 2))

        if timex.day_of_week != None:
            return 'XXXX-WXX-{}'.format(timex.day_of_week)

        return ''

    @staticmethod
    def format_date_range(timex):
        pass

    @staticmethod
    def format_time_range(timex):
        if timex.part_of_day != None:
            return 'T{}'.format(timex.part_of_day)

        return ''