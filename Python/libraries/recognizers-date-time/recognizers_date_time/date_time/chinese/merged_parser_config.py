#  Copyright (c) Microsoft Corporation. All rights reserved.
#  Licensed under the MIT License.

from typing import Pattern

from recognizers_text import RegExpUtility

from ...resources.chinese_date_time import ChineseDateTime, BaseDateTime
from ..parsers import DateTimeParser
from ..base_merged import MergedParserConfiguration
from .duration_parser import ChineseDurationParser
from .date_parser import ChineseDateParser
from .time_parser import ChineseTimeParser
from .dateperiod_parser import ChineseDatePeriodParser
from .timeperiod_parser import ChineseTimePeriodParser
from .datetime_parser import ChineseDateTimeParser
from .datetimeperiod_parser import ChineseDateTimePeriodParser
from .holiday_parser import ChineseHolidayParser
from .set_parser import ChineseSetParser


class ChineseMergedParserConfiguration(MergedParserConfiguration):
    @property
    def around_regex(self) -> Pattern:
        return self._around_regex

    @property
    def equal_regex(self) -> Pattern:
        return self._equal_regex

    @property
    def year_regex(self) -> Pattern:
        return self._year_regex

    @property
    def suffix_after(self) -> Pattern:
        return self._suffix_after

    @property
    def before_regex(self) -> Pattern:
        return self._before_regex

    @property
    def after_regex(self) -> Pattern:
        return self._after_regex

    @property
    def since_regex(self) -> Pattern:
        return self._since_regex

    @property
    def date_parser(self) -> DateTimeParser:
        return self._date_parser

    @property
    def holiday_parser(self) -> DateTimeParser:
        return self._holiday_parser

    @property
    def time_parser(self) -> DateTimeParser:
        return self._time_parser

    @property
    def date_time_parser(self) -> DateTimeParser:
        return self._date_time_parser

    @property
    def date_period_parser(self) -> DateTimeParser:
        return self._date_period_parser

    @property
    def time_period_parser(self) -> DateTimeParser:
        return self._time_period_parser

    @property
    def date_time_period_parser(self) -> DateTimeParser:
        return self._date_time_period_parser

    @property
    def duration_parser(self) -> DateTimeParser:
        return self._duration_parser

    @property
    def set_parser(self) -> DateTimeParser:
        return self._set_parser

    def __init__(self):
        self._equal_regex = RegExpUtility.get_safe_reg_exp(BaseDateTime.EqualRegex)
        self._year_regex = RegExpUtility.get_safe_reg_exp(
            ChineseDateTime.YearRegex
        )
        self._before_regex = RegExpUtility.get_safe_reg_exp(
            ChineseDateTime.MergedBeforeRegex)
        self._after_regex = RegExpUtility.get_safe_reg_exp(
            ChineseDateTime.MergedAfterRegex)
        self._since_regex = RegExpUtility.get_safe_reg_exp(
            ChineseDateTime.MergedAfterRegex)
        self._date_parser = ChineseDateParser()
        self._holiday_parser = ChineseHolidayParser()
        self._time_parser = ChineseTimeParser()
        self._date_time_parser = ChineseDateTimeParser()
        self._date_period_parser = ChineseDatePeriodParser()
        self._time_period_parser = ChineseTimePeriodParser()
        self._date_time_period_parser = ChineseDateTimePeriodParser()
        self._duration_parser = ChineseDurationParser()
        self._set_parser = ChineseSetParser()
        # TODO When the implementation for these properties is added, change the None values to their respective Regexps
        self._around_regex = None
        self._suffix_after = None
