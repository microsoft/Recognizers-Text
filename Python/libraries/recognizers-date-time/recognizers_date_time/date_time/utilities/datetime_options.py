from enum import IntFlag


class DateTimeOptions(IntFlag):
    NONE = 0
    SKIP_FROM_TO_MERGE = 1
    SPLIT_DATE_AND_TIME = 2
    CALENDAR = 4
    EXTENDED_TYPES = 8
    FAIL_FAST = 2097152
    EXPERIMENTAL_MODE = 4194304
    ENABLE_PREVIEW = 8388608


class DateTimeOptionsConfiguration:
    @property
    def options(self):
        return self._options

    @property
    def dmy_date_format(self) -> bool:
        return self._dmy_date_format

    def __init__(self, options=DateTimeOptions.NONE, dmy_date_format=False):
        self._options = options
        self._dmy_date_format = dmy_date_format
