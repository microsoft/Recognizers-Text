#  Copyright (c) Microsoft Corporation. All rights reserved.
#  Licensed under the MIT License.

class Constants:
    TIMEX_TYPES_PRESENT = 'present'
    TIMEX_TYPES_DEFINITE = 'definite'
    TIMEX_TYPES_DATE = 'date'
    TIMEX_TYPES_DATETIME = 'datetime'
    TIMEX_TYPES_DATERANGE = 'daterange'
    TIMEX_TYPES_DURATION = 'duration'
    TIMEX_TYPES_TIME = 'time'
    TIMEX_TYPES_TIMERANGE = 'timerange'
    TIMEX_TYPES_DATETIMERANGE = 'datetimerange'
    DAYS: {str, int} = {
        'MONDAY': 0,
        'TUESDAY': 1,
        'WEDNESDAY': 2,
        'THURSDAY': 3,
        'FRIDAY': 4,
        'SATURDAY': 5,
        'SUNDAY': 6
    }
    GENERAL_PERIOD_PREFIX: str = "P"
    TIME_TIMEX_PREFIX: str = "T"
