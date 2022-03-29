#  Copyright (c) Microsoft Corporation. All rights reserved.
#  Licensed under the MIT License.

from typing import Dict, Pattern

from recognizers_number import BaseNumberExtractor, ItalianCardinalExtractor, ItalianIntegerExtractor, ItalianOrdinalExtractor, BaseNumberParser, ItalianNumberParserConfiguration

from ...resources import ItalianDateTime, BaseDateTime
from ..extractors import DateTimeExtractor
from ..parsers import DateTimeParser
from ..base_configs import BaseDateParserConfiguration, DateTimeUtilityConfiguration
from ..base_date import BaseDateExtractor, BaseDateParser
from ..base_time import BaseTimeExtractor
from ..base_duration import BaseDurationExtractor, BaseDurationParser
from ..base_dateperiod import BaseDatePeriodExtractor, BaseDatePeriodParser
from ..base_timeperiod import BaseTimePeriodExtractor, BaseTimePeriodParser
from ..base_datetime import BaseDateTimeExtractor, BaseDateTimeParser
from ..base_datetimeperiod import BaseDateTimePeriodExtractor, BaseDateTimePeriodParser
from ..base_timezone import BaseTimeZoneParser
from .base_configs import ItalianDateTimeUtilityConfiguration
from .duration_extractor_config import ItalianDurationExtractorConfiguration
from .date_extractor_config import ItalianDateExtractorConfiguration
from .time_extractor_config import ItalianTimeExtractorConfiguration
from .datetime_extractor_config import ItalianDateTimeExtractorConfiguration
from .dateperiod_extractor_config import ItalianDatePeriodExtractorConfiguration
from .timeperiod_extractor_config import ItalianTimePeriodExtractorConfiguration
from .datetimeperiod_extractor_config import ItalianDateTimePeriodExtractorConfiguration
from .duration_parser_config import ItalianDurationParserConfiguration
from .date_parser_config import ItalianDateParserConfiguration
from .time_parser_config import ItalianTimeParserConfiguration
from .datetime_parser_config import ItalianDateTimeParserConfiguration
from .dateperiod_parser_config import ItalianDatePeriodParserConfiguration
from .timeperiod_parser_config import ItalianTimePeriodParserConfiguration
from .datetimeperiod_parser_config import ItalianDateTimePeriodParserConfiguration
from .parsers import ItalianTimeParser


class ItalianCommonDateTimeParserConfiguration(BaseDateParserConfiguration):
    @property
    def time_zone_parser(self) -> DateTimeParser:
        self._time_zone_parser

    @property
    def check_both_before_after(self) -> Pattern:
        return self._check_both_before_after

    @property
    def cardinal_extractor(self) -> BaseNumberExtractor:
        return self._cardinal_extractor

    @property
    def integer_extractor(self) -> BaseNumberExtractor:
        return self._integer_extractor

    @property
    def ordinal_extractor(self) -> BaseNumberExtractor:
        return self._ordinal_extractor

    @property
    def number_parser(self) -> BaseNumberParser:
        return self._number_parser

    @property
    def date_extractor(self) -> DateTimeExtractor:
        return self._date_extractor

    @property
    def time_extractor(self) -> DateTimeExtractor:
        return self._time_extractor

    @property
    def date_time_extractor(self) -> DateTimeExtractor:
        return self._date_time_extractor

    @property
    def duration_extractor(self) -> DateTimeExtractor:
        return self._duration_extractor

    @property
    def date_period_extractor(self) -> DateTimeExtractor:
        return self._date_period_extractor

    @property
    def time_period_extractor(self) -> DateTimeExtractor:
        return self._time_period_extractor

    @property
    def date_time_period_extractor(self) -> DateTimeExtractor:
        return self._date_time_period_extractor

    @property
    def date_parser(self) -> DateTimeParser:
        return self._date_parser

    @property
    def time_parser(self) -> DateTimeParser:
        return self._time_parser

    @property
    def date_time_parser(self) -> DateTimeParser:
        return self._date_time_parser

    @property
    def duration_parser(self) -> DateTimeParser:
        return self._duration_parser

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
    def month_of_year(self) -> Dict[str, int]:
        return self._month_of_year

    @property
    def numbers(self) -> Dict[str, int]:
        return self._numbers

    @property
    def unit_value_map(self) -> Dict[str, int]:
        return self._unit_value_map

    @property
    def season_map(self) -> Dict[str, str]:
        return self._season_map

    @property
    def unit_map(self) -> Dict[str, str]:
        return self._unit_map

    @property
    def cardinal_map(self) -> Dict[str, int]:
        return self._cardinal_map

    @property
    def day_of_week(self) -> Dict[str, int]:
        return self._day_of_week

    @property
    def double_numbers(self) -> Dict[str, int]:
        return self._double_numbers

    @property
    def utility_configuration(self) -> DateTimeUtilityConfiguration:
        return self._utility_configuration

    def __init__(self):
        super().__init__()
        self._time_zone_parser = BaseTimeZoneParser()
        self._utility_configuration = ItalianDateTimeUtilityConfiguration()
        self._unit_map = ItalianDateTime.UnitMap
        self._unit_value_map = ItalianDateTime.UnitValueMap
        self._season_map = ItalianDateTime.SeasonMap
        self._cardinal_map = ItalianDateTime.CardinalMap
        self._day_of_week = ItalianDateTime.DayOfWeek
        self._month_of_year = ItalianDateTime.MonthOfYear
        self._numbers = ItalianDateTime.Numbers
        self._double_numbers = ItalianDateTime.DoubleNumbers
        self._check_both_before_after = ItalianDateTime.CheckBothBeforeAfter

        self._cardinal_extractor = ItalianCardinalExtractor()
        self._integer_extractor = ItalianIntegerExtractor()
        self._ordinal_extractor = ItalianOrdinalExtractor()

        self._day_of_month = {
            **BaseDateTime.DayOfMonthDictionary, **ItalianDateTime.DayOfMonth}
        self._number_parser = BaseNumberParser(
            ItalianNumberParserConfiguration())
        self._date_extractor = BaseDateExtractor(
            ItalianDateExtractorConfiguration())
        self._time_extractor = BaseTimeExtractor(
            ItalianTimeExtractorConfiguration())
        self._duration_extractor = BaseDurationExtractor(
            ItalianDurationExtractorConfiguration())
        self._date_period_extractor = BaseDatePeriodExtractor(
            ItalianDatePeriodExtractorConfiguration())
        self._time_period_extractor = BaseTimePeriodExtractor(
            ItalianTimePeriodExtractorConfiguration())
        self._date_time_extractor = BaseDateTimeExtractor(
            ItalianDateTimeExtractorConfiguration())
        self._date_time_period_extractor = BaseDateTimePeriodExtractor(
            ItalianDateTimePeriodExtractorConfiguration())
        self._duration_parser = BaseDurationParser(
            ItalianDurationParserConfiguration(self))
        self._date_parser = BaseDateParser(ItalianDateParserConfiguration(self))
        self._time_parser = ItalianTimeParser(
            ItalianTimeParserConfiguration(self))
        self._date_period_parser = BaseDatePeriodParser(
            ItalianDatePeriodParserConfiguration(self))
        self._time_period_parser = BaseTimePeriodParser(
            ItalianTimePeriodParserConfiguration(self))
        self._date_time_parser = BaseDateTimeParser(
            ItalianDateTimeParserConfiguration(self))
        self._date_time_period_parser = BaseDateTimePeriodParser(
            ItalianDateTimePeriodParserConfiguration(self))
