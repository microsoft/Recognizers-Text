# Copyright (c) Microsoft Corporation. All rights reserved.
# Licensed under the MIT License.

from decimal import Decimal
from copy import copy
from .timex_parsing import TimexParsing
from .timex_inference import TimexInference
from .timex_convert import TimexConvert
from .timex_relative_convert import TimexRelativeConvert
from .time import Time

class Timex:
    
    def __init__(self, timex = None):
        self.now = None
        self.years = None
        self.months = None
        self.weeks = None
        self.days = None
        self.hours = None
        self.minutes = None
        self.seconds = None
        self.year = None
        self.month = None
        self.day_of_month = None
        self.day_of_week = None
        self.season = None
        self.week_of_year = None
        self.weekend = None
        self.week_of_month = None
        self.part_of_day = None
        if timex != None:
            TimexParsing.parse_string(timex, self)

    @classmethod
    def from_date(date):
        pass

    @classmethod
    def from_date_time(datetime):
        pass

    @classmethod
    def from_time(time):
        pass

    @property
    def time_value(self):
        from .timex_format import TimexFormat
        return TimexFormat.format(self)

    @property
    def types(self):
        return TimexInference.infer(self)

    def __str__(self):
        return TimexConvert.convert_timex_to_string(self)

    def to_natural_language(self, referene_date):
        return TimexRelativeConvert.convert_timex_to_string_relative(self)

    @property
    def hour(self):
        if hasattr(self, '__time'):
            return getattr(self, '__time').hour
        else:
            return None

    @hour.setter
    def hour(self, value):
        if value != None:
            if not hasattr(self, '__time'):
                setattr(self, '__time', Time(value, 0, 0))
            else:
                getattr(self, '__time').hour = value
        else:
            if hasattr(self, '__time'):
                delattr(self, '__time')

    @property
    def minute(self):
        if hasattr(self, '__time'):
            return getattr(self, '__time').minute
        else:
            return None

    @minute.setter
    def minute(self, value):
        if value != None:
            if not hasattr(self, '__time'):
                setattr(self, '__time', Time(0, value, 0))
            else:
                getattr(self, '__time').minute = value
        else:
            if hasattr(self, '__time'):
                delattr(self, '__time')

    @property
    def second(self):
        if hasattr(self, '__time'):
            return getattr(self, '__time').second
        else:
            return None

    @second.setter
    def second(self, value):
        if value != None:
            if not hasattr(self, '__time'):
                setattr(self, '__time', Time(0, 0, value))
            else:
                getattr(self, '__time').second = value
        else:
            if hasattr(self, '__time'):
                delattr(self, '__time')

    def clone(self):
        return copy(self)

    def assign_properties(self, source):
        for key, value in source.items():
            if key == 'year':
                self.year = int(value)
            elif key == 'month':
                self.month = int(value)
            elif key == 'day_of_month':
                self.day_of_month = int(value)
            elif key == 'day_of_week':
                self.day_of_week = int(value)
            elif key == 'season':
                self.season = value
            elif key == 'week_of_year':
                self.week_of_year = int(value)
            elif key == 'weekend':
                self.weekend = True
            elif key == 'week_of_month':
                self.week_of_month = int(value)
            elif key == 'hour':
                self.hour = int(value)
            elif key == 'minute':
                self.minute = int(value)
            elif key == 'second':
                self.second = int(value)
            elif key == 'part_of_day':
                self.part_of_day = value
            elif key == 'date_unit':
                self.assign_date_duration(source)
            elif key == 'time_unit':
                self.assign_time_duration(source)

    def assign_date_duration(self, source):
        if source['date_unit'] == 'Y':
            self.years = Decimal(source['amount'])
        elif source['date_unit'] == 'M':
            self.months = Decimal(source['amount'])
        elif source['date_unit'] == 'W':
            self.weeks = Decimal(source['amount'])
        elif source['date_unit'] == 'D':
            self.days = Decimal(source['amount'])
            
    def assign_time_duration(self, source):
        if source['time_unit'] == 'H':
            self.hours = Decimal(source['amount'])
        elif source['time_unit'] == 'M':
            self.minutes = Decimal(source['amount'])
        elif source['time_unit'] == 'S':
            self.seconds = Decimal(source['amount'])
