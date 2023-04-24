#  Copyright (c) Microsoft Corporation. All rights reserved.
#  Licensed under the MIT License.

from typing import Pattern

from recognizers_text import RegExpUtility

from ...resources.japanese_date_time import JapaneseDateTime, BaseDateTime
from ..parsers import DateTimeParser
from ..base_merged import MergedParserConfiguration
from .time_parser import JapaneseTimeParser
from .timeperiod_parser import JapaneseTimePeriodParser


class JapaneseMergedParserConfiguration(MergedParserConfiguration):
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
        raise NotImplementedError

    @property
    def holiday_parser(self) -> DateTimeParser:
        raise NotImplementedError

    @property
    def time_parser(self) -> DateTimeParser:
        return self._time_parser

    @property
    def date_time_parser(self) -> DateTimeParser:
        raise NotImplementedError

    @property
    def date_period_parser(self) -> DateTimeParser:
        raise NotImplementedError

    @property
    def time_period_parser(self) -> DateTimeParser:
        return self._time_period_parser

    @property
    def date_time_period_parser(self) -> DateTimeParser:
        raise NotImplementedError

    @property
    def duration_parser(self) -> DateTimeParser:
        raise NotImplementedError

    @property
    def set_parser(self) -> DateTimeParser:
        raise NotImplementedError

    def __init__(self):
        self._before_regex = RegExpUtility.get_safe_reg_exp(
            JapaneseDateTime.MergedBeforeRegex)
        self._after_regex = RegExpUtility.get_safe_reg_exp(
            JapaneseDateTime.MergedAfterRegex)
        self._equal_regex = RegExpUtility.get_safe_reg_exp(BaseDateTime.EqualRegex)
        self._year_regex = RegExpUtility.get_safe_reg_exp(
            JapaneseDateTime.YearRegex
        )
        self._since_regex = RegExpUtility.get_safe_reg_exp(
            JapaneseDateTime.MergedAfterRegex)
        self._time_parser = JapaneseTimeParser()
        self._time_period_parser = JapaneseTimePeriodParser()
        # TODO When the implementation for these properties is added, change the None values to their respective Regexps
        self._around_regex = None
        self._suffix_after = None
