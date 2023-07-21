from typing import Dict

from recognizers_date_time.date_time.base_timezone import BaseTimeZoneParser
from recognizers_date_time.date_time.utilities.datetime_utility_config import DateTimeUtilityConfiguration
from recognizers_number import BaseNumberExtractor

from recognizers_number.number.japanese.parsers import JapaneseNumberParserConfiguration
from recognizers_number.number.japanese.extractors import JapaneseIntegerExtractor, JapaneseOrdinalExtractor, \
    JapaneseCardinalExtractor
from recognizers_number.number.cjk_parsers import CJKNumberParser

from recognizers_date_time.resources.japanese_date_time import JapaneseDateTime
from recognizers_date_time.date_time.CJK import CJKCommonDateTimeParserConfiguration, BaseCJKDateTimeExtractor, \
    BaseCJKDateTimePeriodExtractor, BaseCJKDurationExtractor, BaseCJKHolidayExtractor, BaseCJKSetExtractor, \
    BaseCJKDateTimeParser, BaseCJKDateTimePeriodParser, BaseCJKDurationParser, BaseCJKHolidayParser, \
    BaseCJKSetParser, BaseCJKTimeExtractor, BaseCJKTimePeriodExtractor, BaseCJKTimeParser, \
    BaseCJKTimePeriodParser, BaseCJKDateParser, BaseCJKDateExtractor, BaseCJKDatePeriodExtractor, \
    BaseCJKDatePeriodParser
from recognizers_date_time.date_time.japanese.datetime_extractor_config import JapaneseDateTimeExtractorConfiguration
from recognizers_date_time.date_time.japanese.datetimeperiod_extractor_config import \
    JapaneseDateTimePeriodExtractorConfiguration
from recognizers_date_time.date_time.japanese.duration_extractor_config import JapaneseDurationExtractorConfiguration
from recognizers_date_time.date_time.japanese.set_extractor_config import JapaneseSetExtractorConfiguration
from recognizers_date_time.date_time.japanese.holiday_extractor_config import JapaneseHolidayExtractorConfiguration

from recognizers_date_time.date_time.japanese.datetime_parser_config import JapaneseDateTimeParserConfiguration
from recognizers_date_time.date_time.japanese.datetimeperiod_parser_config import \
    JapaneseDateTimePeriodParserConfiguration
from recognizers_date_time.date_time.japanese.duration_parser_config import JapaneseDurationParserConfiguration
from recognizers_date_time.date_time.japanese.set_parser_config import JapaneseSetParserConfiguration
from recognizers_date_time.date_time.japanese.holiday_parser_config import JapaneseHolidayParserConfiguration
from recognizers_date_time.date_time.japanese.time_extractor_config import JapaneseTimeExtractorConfiguration
from recognizers_date_time.date_time.japanese.timeperiod_extractor_config import \
    JapaneseTimePeriodExtractorConfiguration
from recognizers_date_time.date_time.japanese.timeperiod_parser_config import JapaneseTimePeriodParserConfiguration
from recognizers_date_time.date_time.japanese.time_parser_config import JapaneseTimeParserConfiguration
from recognizers_date_time.date_time.japanese.date_extractor_config import JapaneseDateExtractorConfiguration
from recognizers_date_time.date_time.japanese.dateperiod_extractor_config import \
    JapaneseDatePeriodExtractorConfiguration
from recognizers_date_time.date_time.japanese.date_parser_config import JapaneseDateParserConfiguration
from recognizers_date_time.date_time.japanese.dateperiod_parser_config import JapaneseDatePeriodParserConfiguration
from recognizers_date_time.date_time.extractors import DateTimeExtractor
from recognizers_date_time.date_time.parsers import DateTimeParser


class JapaneseCommonDateTimeParserConfiguration(CJKCommonDateTimeParserConfiguration):

    @property
    def cardinal_extractor(self) -> BaseNumberExtractor:
        return self._cardinal_extractor

    @property
    def integer_extractor(self) -> JapaneseIntegerExtractor:
        return self._integer_extractor

    @property
    def ordinal_extractor(self) -> BaseNumberExtractor:
        return self._ordinal_extractor

    @property
    def number_parser(self) -> CJKNumberParser:
        return self._number_parser

    @property
    def date_extractor(self) -> BaseCJKDateExtractor:
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
    def set_extractor(self) -> DateTimeExtractor:
        return self._set_extractor

    @property
    def holiday_extractor(self) -> DateTimeExtractor:
        return self._holiday_extractor

    @property
    def date_parser(self) -> BaseCJKDateParser:
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
    def set_parser(self) -> DateTimeParser:
        return self._set_parser

    @property
    def holiday_parser(self) -> DateTimeParser:
        return self._holiday_parser

    @property
    def date_time_alt_parser(self) -> DateTimeParser:
        return None

    @property
    def time_zone_parser(self) -> DateTimeParser:
        return self._time_zone_parser

    @property
    def month_of_year(self) -> Dict[str, int]:
        return self._month_of_year

    @property
    def numbers(self) -> Dict[str, int]:
        return None

    @property
    def unit_value_map(self) -> Dict[str, int]:
        return self._unit_value_map

    @property
    def season_map(self) -> Dict[str, str]:
        return self._season_map

    @property
    def special_year_prefixes_map(self) -> Dict[str, str]:
        return None

    @property
    def unit_map(self) -> Dict[str, str]:
        return self._unit_map

    @property
    def cardinal_map(self) -> Dict[str, int]:
        return self._cardinal_map

    @property
    def day_of_month(self) -> Dict[str, int]:
        return self._day_of_month

    @property
    def day_of_week(self) -> Dict[str, int]:
        return self._day_of_week

    @property
    def double_numbers(self) -> Dict[str, int]:
        return None

    @property
    def written_decades(self) -> Dict[str, int]:
        return None

    @property
    def special_decade_cases(self) -> Dict[str, int]:
        return None

    @property
    def utility_configuration(self) -> DateTimeUtilityConfiguration:
        return None

    def __init__(self):
        super().__init__()

        self._unit_map = JapaneseDateTime.ParserConfigurationUnitMap
        self._unit_value_map = JapaneseDateTime.DurationUnitValueMap
        self._cardinal_map = JapaneseDateTime.ParserConfigurationCardinalMap
        self._day_of_month = JapaneseDateTime.ParserConfigurationDayOfMonth
        self._day_of_week = JapaneseDateTime.ParserConfigurationDayOfWeek
        self._month_of_year = JapaneseDateTime.ParserConfigurationMonthOfYear

        self._integer_extractor = JapaneseIntegerExtractor()
        self._cardinal_extractor = JapaneseCardinalExtractor()
        self._ordinal_extractor = JapaneseOrdinalExtractor()
        self._number_parser = CJKNumberParser(JapaneseNumberParserConfiguration())

        # Do not change order. The order of initialization can lead to side effects
        self._date_extractor = BaseCJKDateExtractor(
            JapaneseDateExtractorConfiguration())
        self._time_extractor = BaseCJKTimeExtractor(JapaneseTimeExtractorConfiguration())
        self._date_time_extractor = BaseCJKDateTimeExtractor(JapaneseDateTimeExtractorConfiguration())
        self._duration_extractor = BaseCJKDurationExtractor(JapaneseDurationExtractorConfiguration())
        self._date_period_extractor = BaseCJKDatePeriodExtractor(
            JapaneseDatePeriodExtractorConfiguration())
        self._time_period_extractor = BaseCJKTimePeriodExtractor(JapaneseTimePeriodExtractorConfiguration())
        self._date_time_period_extractor = BaseCJKDateTimePeriodExtractor(
            JapaneseDateTimePeriodExtractorConfiguration())
        self._holiday_extractor = BaseCJKHolidayExtractor(JapaneseHolidayExtractorConfiguration())
        self._set_extractor = BaseCJKSetExtractor(JapaneseSetExtractorConfiguration())

        self._duration_parser = BaseCJKDurationParser(JapaneseDurationParserConfiguration(self))
        self._date_parser = BaseCJKDateParser(JapaneseDateParserConfiguration(self))
        self._time_parser = BaseCJKTimeParser(JapaneseTimeParserConfiguration(self))
        self._date_time_parser = BaseCJKDateTimeParser(JapaneseDateTimeParserConfiguration(self))
        self._date_period_parser = BaseCJKDatePeriodParser(JapaneseDatePeriodParserConfiguration(self))
        self._time_period_parser = BaseCJKTimePeriodParser(JapaneseTimePeriodParserConfiguration(self))
        self._date_time_period_parser = BaseCJKDateTimePeriodParser(JapaneseDateTimePeriodParserConfiguration(self))
        self._holiday_parser = BaseCJKHolidayParser(JapaneseHolidayParserConfiguration(self))
        self._set_parser = BaseCJKSetParser(JapaneseSetParserConfiguration(self))

        self._season_map = JapaneseDateTime.ParserConfigurationSeasonMap
        self._time_zone_parser = BaseTimeZoneParser()
