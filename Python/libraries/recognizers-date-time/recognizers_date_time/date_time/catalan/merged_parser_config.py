from typing import Pattern

from recognizers_text.utilities import RegExpUtility
from .common_configs import CatalanCommonDateTimeParserConfiguration
from ..base_date import BaseDateParser
from ..base_time import BaseTimeParser
from ..base_minimal_merged import MinimalMergedParserConfiguration
from ...resources.catalan_date_time import CatalanDateTime, BaseDateTime
from ..parsers import DateTimeParser


class CatalanMergedParserConfiguration(CatalanCommonDateTimeParserConfiguration, MinimalMergedParserConfiguration):
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
    def date_parser(self) -> BaseDateParser:
        return self._date_parser

    @property
    def time_parser(self) -> BaseTimeParser:
        return self._time_parser

    @property
    def time_zone_parser(self) -> DateTimeParser:
        return self._time_zone_parser

    def __init__(self, config):
        CatalanCommonDateTimeParserConfiguration.__init__(self)
        self._time_zone_parser = config.time_zone_parser
        self._equal_regex = RegExpUtility.get_safe_reg_exp(BaseDateTime.EqualRegex)
        self._suffix_after = RegExpUtility.get_safe_reg_exp(f'^[.]')
        self._year_regex = RegExpUtility.get_safe_reg_exp(
            CatalanDateTime.YearRegex)
        self._around_regex = RegExpUtility.get_safe_reg_exp(f'^[.]')
        self._before_regex = RegExpUtility.get_safe_reg_exp(f'^[.]')
        self._after_regex = RegExpUtility.get_safe_reg_exp(f'^[.]')
        self._since_regex = RegExpUtility.get_safe_reg_exp(f'^[.]')
