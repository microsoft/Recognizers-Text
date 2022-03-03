#  Copyright (c) Microsoft Corporation. All rights reserved.
#  Licensed under the MIT License.

from typing import Dict, Pattern

from recognizers_number import BaseNumberExtractor, BaseNumberParser
from recognizers_number.number.portuguese.parsers import PortugueseNumberParserConfiguration
from recognizers_number.number.portuguese.extractors import PortugueseCardinalExtractor, PortugueseIntegerExtractor, PortugueseOrdinalExtractor

from ...resources.portuguese_date_time import BaseDateTime, PortugueseDateTime
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

from .base_configs import PortugueseDateTimeUtilityConfiguration
from .date_extractor_config import PortugueseDateExtractorConfiguration
from .date_parser_config import PortugueseDateParserConfiguration
from .time_extractor_config import PortugueseTimeExtractorConfiguration
from .time_parser_config import PortugueseTimeParserConfiguration
from .parsers import PortugueseTimeParser
from .duration_extractor_config import PortugueseDurationExtractorConfiguration
from .duration_parser_config import PortugueseDurationParserConfiguration
from .dateperiod_extractor_config import PortugueseDatePeriodExtractorConfiguration
from .dateperiod_parser_config import PortugueseDatePeriodParserConfiguration
from .timeperiod_extractor_config import PortugueseTimePeriodExtractorConfiguration
from .timeperiod_parser_config import PortugueseTimePeriodParserConfiguration
from .datetime_extractor_config import PortugueseDateTimeExtractorConfiguration
from .datetime_parser_config import PortugueseDateTimeParserConfiguration
from .datetimeperiod_extractor_config import PortugueseDateTimePeriodExtractorConfiguration
from .datetimeperiod_parser_config import PortugueseDateTimePeriodParserConfiguration
from ..base_timezone import BaseTimeZoneParser


class PortugueseCommonDateTimeParserConfiguration(BaseDateParserConfiguration):
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
    def time_zone_parser(self) -> BaseTimeZoneParser:
        return self._time_zone_parser

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
    def day_of_month(self) -> Dict[str, int]:
        return self._day_of_month

    @property
    def double_numbers(self) -> Dict[str, int]:
        return self._double_numbers

    @property
    def utility_configuration(self) -> DateTimeUtilityConfiguration:
        return self._utility_configuration

    def __init__(self):
        BaseDateParserConfiguration.__init__(self)

        self._utility_configuration = PortugueseDateTimeUtilityConfiguration()
        self._unit_map = PortugueseDateTime.UnitMap
        self._unit_value_map = PortugueseDateTime.UnitValueMap
        self._season_map = PortugueseDateTime.SeasonMap
        self._cardinal_map = PortugueseDateTime.CardinalMap
        self._day_of_week = PortugueseDateTime.DayOfWeek
        self._month_of_year = PortugueseDateTime.MonthOfYear
        self._numbers = PortugueseDateTime.Numbers
        self._double_numbers = PortugueseDateTime.DoubleNumbers
        self._cardinal_extractor = PortugueseCardinalExtractor()
        self._integer_extractor = PortugueseIntegerExtractor()
        self._ordinal_extractor = PortugueseOrdinalExtractor()
        self._check_both_before_after = PortugueseDateTime.CheckBothBeforeAfter
        self._time_zone_parser = BaseTimeZoneParser()
        self._number_parser = BaseNumberParser(
            PortugueseNumberParserConfiguration())
        self._date_extractor = BaseDateExtractor(
            PortugueseDateExtractorConfiguration())
        self._time_extractor = BaseTimeExtractor(
            PortugueseTimeExtractorConfiguration())
        self._duration_extractor = BaseDurationExtractor(
            PortugueseDurationExtractorConfiguration())
        self._date_period_extractor = BaseDatePeriodExtractor(
            PortugueseDatePeriodExtractorConfiguration())
        self._time_period_extractor = BaseTimePeriodExtractor(
            PortugueseTimePeriodExtractorConfiguration())
        self._date_time_extractor = BaseDateTimeExtractor(
            PortugueseDateTimeExtractorConfiguration())
        self._date_time_period_extractor = BaseDateTimePeriodExtractor(
            PortugueseDateTimePeriodExtractorConfiguration())
        self._duration_parser = BaseDurationParser(
            PortugueseDurationParserConfiguration(self))
        self._date_parser = BaseDateParser(
            PortugueseDateParserConfiguration(self))
        self._time_parser = PortugueseTimeParser(
            PortugueseTimeParserConfiguration(self))
        self._date_period_parser = BaseDatePeriodParser(
            PortugueseDatePeriodParserConfiguration(self))
        self._time_period_parser = BaseTimePeriodParser(
            PortugueseTimePeriodParserConfiguration(self))
        self._date_time_parser = BaseDateTimeParser(
            PortugueseDateTimeParserConfiguration(self))
        self._date_time_period_parser = BaseDateTimePeriodParser(
            PortugueseDateTimePeriodParserConfiguration(self))
