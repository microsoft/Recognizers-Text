from ..timex_set import TimexSet
from ..timex import Timex

from .timex_constants import EnglishConstants
from ..timex_inference import *


def english_convert_date(timex: Timex):
    if timex.day_of_week is not None:
        return EnglishConstants.DAYS[timex.day_of_week - 1]

    month = EnglishConstants.MONTHS[timex.month - 1]
    date = str(timex.day_of_month)
    abbreviation = EnglishConstants.DATE_ABBREVIATION[int(
        str(date[len(date) - 1]))]

    if timex.year is not None:
        return f'{date}{abbreviation} {month} {timex.year}'

    return f'{date}{abbreviation} {month}'


def convert_time(timex: Timex):
    if timex.hour == 0 and timex.minute == 0 and timex.second == 0:
        return 'midnight'
    if timex.hour == 12 and timex.minute == 0 and timex.second == 0:
        return 'midday'

    hour = '12' if timex.hour == 0 else str(
        timex.hour - 12) if timex.hour > 12 else str(timex.hour)
    minute = '' if timex.minute == 0 and timex.second == 0 else ':' + \
        str(timex.minute).rjust(2, '0')
    second = '' if timex.second == 0 else ':' + str(timex.second).rjust(2, '0')
    period = 'AM' if timex.hour < 12 else 'PM'

    return f'{hour}{minute}{second}{period}'


def convert_duration_property_to_string(value, prop, include_single_count):
    if value == 1:
        return f'1 {prop}' if include_single_count else prop
    else:
        return f'{value} {prop}s'


def convert_timex_duration_to_string(timex: Timex, include_single_count):
    if timex.years is not None:
        return convert_duration_property_to_string(timex.years, 'year', include_single_count)
    if timex.months is not None:
        return convert_duration_property_to_string(timex.months, 'month', include_single_count)
    if timex.weeks is not None:
        return convert_duration_property_to_string(timex.weeks, 'week', include_single_count)
    if timex.days is not None:
        return convert_duration_property_to_string(timex.days, 'day', include_single_count)
    if timex.hours is not None:
        return convert_duration_property_to_string(timex.hours, 'hour', include_single_count)
    if timex.minutes is not None:
        return convert_duration_property_to_string(timex.minutes, 'minute', include_single_count)
    if timex.seconds is not None:
        return convert_duration_property_to_string(timex.seconds, 'second', include_single_count)
    return ''


def convert_duration(timex: Timex):
    return convert_timex_duration_to_string(timex, True)


def convert_date_range(timex: Timex):
    season = EnglishConstants.SEASONS[timex.season] if timex.season is not None else ''
    year = str(timex.year) if timex.year is not None else ''
    if timex.week_of_year is not None and timex.weekend is not None:
        raise NotImplementedError

    if timex.month is not None:
        month = str(EnglishConstants.MONTHS[timex.month - 1])
        if timex.week_of_month is not None:
            return str(EnglishConstants.WEEKS[timex.week_of_month - 1]) + "week of " + month
        else:
            return str(month).strip() + str(year).strip()
    return f'{season.strip()} {year.strip()}'


def convert_time_range(timex: Timex):
    return EnglishConstants.DAY_PARTS[timex.part_of_day]


def convert_date_time(timex: Timex):
    return f'{convert_time(timex)} {english_convert_date(timex)}'


def convert_date(timex):
    if timex.day_of_week is not None:
        return EnglishConstants.DAYS[timex.day_of_week - 1]

    month = EnglishConstants.MONTHS[timex.month - 1]
    date = str(timex.day_of_month)
    abbreviation = EnglishConstants.DATE_ABBREVIATION[int(date)]

    if timex.year is not None:
        return f'{date}{abbreviation} {month} {timex.year}'
    return f'{date}{abbreviation} {month}'


def convert_date_time_range(timex: Timex):
    if Constants.TIMEX_TYPES_TIMERANGE in timex.types:
        return f'{convert_date(timex)} {convert_time_range(timex)}'
    return ''


def convert_timex_to_string(timex: Timex):
    types = timex.types if len(timex.types) != 0 else TimexInference.infer(timex)

    if Constants.TIMEX_TYPES_PRESENT in types:
        return 'now'
    if Constants.TIMEX_TYPES_DATETIMERANGE in types:
        return convert_date_time_range(timex)
    if Constants.TIMEX_TYPES_DATERANGE in types:
        return convert_date_range(timex)
    if Constants.TIMEX_TYPES_DURATION in types:
        return convert_duration(timex)
    if Constants.TIMEX_TYPES_TIMERANGE in types:
        return convert_time_range(timex)
    if Constants.TIMEX_TYPES_DATETIME in types:
        return convert_date_time(timex)
    if Constants.TIMEX_TYPES_DATE in types:
        return english_convert_date(timex)
    if Constants.TIMEX_TYPES_TIME in types:
        return convert_time(timex)

    return ''


def convert_timex_set_to_string(timex_set: TimexSet):
    timex = timex_set.timex

    if Constants.TIMEX_TYPES_DURATION in timex.types():
        return 'every ' + convert_timex_duration_to_string(timex, False)
    else:
        return 'every ' + convert_timex_to_string(timex)
