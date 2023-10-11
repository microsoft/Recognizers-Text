from typing import Dict, Pattern, List
from datetime import datetime
from recognizers_text.extractor import ExtractResult
from recognizers_number import BaseNumberExtractor, BaseNumberParser
from recognizers_number.number.catalan.parsers import CatalanNumberParserConfiguration
from recognizers_number.number.catalan.extractors import CatalanCardinalExtractor, CatalanIntegerExtractor, \
    CatalanOrdinalExtractor

from ...resources.catalan_date_time import CatalanDateTime
from ..extractors import DateTimeExtractor
from ..parsers import DateTimeParser
from ..base_configs import DateTimeUtilityConfiguration
from ..base_duration import BaseDurationExtractor, BaseDurationParser
from ..base_minimal_configs import MinimalBaseDateParserConfiguration
from ..base_date import BaseDateExtractor, DateExtractorConfiguration, BaseDateParser
from ..base_time import BaseTimeExtractor, BaseTimeParser
from ..base_timezone import BaseTimeZoneParser
from .base_configs import CatalanDateTimeUtilityConfiguration
from .date_extractor_config import CatalanDateExtractorConfiguration
from .date_parser_config import CatalanDateParserConfiguration
from .time_extractor_config import CatalanTimeExtractorConfiguration
from .time_parser_config import CatalanTimeParserConfiguration


class CatalanBaseDateExtractor(BaseDateExtractor):

    def __init__(self, config: DateExtractorConfiguration):
        super().__init__(config)

    def extract(self, source: str, reference: datetime = None) -> List[ExtractResult]:
        from ..utilities import merge_all_tokens
        if reference is None:
            reference = datetime.now()

        tokens = []
        tokens.extend(self.basic_regex_match(source))
        tokens.extend(self.implicit_date(source))
        tokens.extend(self.number_with_month(source, reference))

        result = merge_all_tokens(tokens, source, self.extractor_type_name)
        return result


class CatalanCommonDateTimeParserConfiguration(MinimalBaseDateParserConfiguration):
    @property
    def time_zone_parser(self) -> DateTimeParser:
        return self._time_zone_parser

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
    def date_parser(self) -> DateTimeParser:
        return self._date_parser

    @property
    def time_parser(self) -> DateTimeParser:
        return self._time_parser

    @property
    def month_of_year(self) -> Dict[str, int]:
        return self._month_of_year

    @property
    def numbers(self) -> Dict[str, int]:
        return self._numbers

    @property
    def day_of_month(self) -> Dict[str, int]:
        return self._day_of_month

    @property
    def day_of_week(self) -> Dict[str, int]:
        return self._day_of_week

    @property
    def unit_map(self):
        return {}

    @property
    def cardinal_map(self):
        return {}

    @property
    def utility_configuration(self) -> DateTimeUtilityConfiguration:
        return self._utility_configuration

    def __init__(self):
        MinimalBaseDateParserConfiguration.__init__(self)

        self._utility_configuration = CatalanDateTimeUtilityConfiguration()
        self._time_zone_parser = BaseTimeZoneParser()
        self._day_of_week = CatalanDateTime.DayOfWeek
        self._month_of_year = CatalanDateTime.MonthOfYear
        self._numbers = CatalanDateTime.Numbers
        self._check_both_before_after = CatalanDateTime.CheckBothBeforeAfter
        self._cardinal_extractor = CatalanCardinalExtractor()
        self._integer_extractor = CatalanIntegerExtractor()
        self._ordinal_extractor = CatalanOrdinalExtractor()
        self._number_parser = BaseNumberParser(
            CatalanNumberParserConfiguration())
        self._date_extractor = CatalanBaseDateExtractor(
            CatalanDateExtractorConfiguration())
        self._time_extractor = BaseTimeExtractor(CatalanTimeExtractorConfiguration())
        self._date_parser = BaseDateParser(
            CatalanDateParserConfiguration(self))
        self._time_parser = BaseTimeParser(CatalanTimeParserConfiguration(self))