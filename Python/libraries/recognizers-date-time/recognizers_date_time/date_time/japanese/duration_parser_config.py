from typing import Dict, Pattern

from recognizers_date_time.date_time.CJK import CJKDurationParserConfiguration, CJKCommonDateTimeParserConfiguration, \
    BaseCJKDurationExtractor
from recognizers_date_time.date_time.japanese.duration_extractor_config import JapaneseDurationExtractorConfiguration
from recognizers_date_time.date_time.utilities import DateTimeExtractor
from recognizers_number.culture import CultureInfo, Culture
from recognizers_number_with_unit.number_with_unit import NumberWithUnitParser
from recognizers_number_with_unit.number_with_unit.japanese import JapaneseNumberWithUnitParserConfiguration
from recognizers_text import Parser


class JapaneseDurationParserConfiguration(CJKDurationParserConfiguration):
    @property
    def duration_extractor(self) -> DateTimeExtractor:
        return self._duration_extractor

    @property
    def internal_parser(self) -> Parser:
        return self._internal_parser

    @property
    def year_regex(self) -> Pattern:
        return self._year_regex

    @property
    def some_regex(self) -> Pattern:
        return self._some_regex

    @property
    def more_or_less_regex(self) -> Pattern:
        return self._more_or_less_regex

    @property
    def duration_unit_regex(self) -> Pattern:
        return self._duration_unit_regex

    @property
    def an_unit_regex(self) -> Pattern:
        return self._an_unit_regex

    @property
    def duration_connector_regex(self) -> Pattern:
        return self._duration_connector_regex

    @property
    def unit_map(self) -> Dict[str, str]:
        return self._unit_map

    @property
    def unit_value_map(self) -> Dict[str, float]:
        return self._unit_value_map

    class DurationParserConfiguration(JapaneseNumberWithUnitParserConfiguration):
        def __init__(self, culture_info: CultureInfo = CultureInfo(Culture.Japanese)):
            super().__init__(culture_info)
            self.bind_dictionary = \
                JapaneseDurationExtractorConfiguration.DurationExtractorConfiguration.duration_suffix_list

    def __init__(self, config: CJKCommonDateTimeParserConfiguration):
        super().__init__()
        self._internal_parser = NumberWithUnitParser(self.DurationParserConfiguration())

        self._duration_extractor = BaseCJKDurationExtractor(
            JapaneseDurationExtractorConfiguration(), False)

        self._year_regex = JapaneseDurationExtractorConfiguration().year_regex
        self._some_regex = JapaneseDurationExtractorConfiguration().some_regex
        self._more_or_less_regex = JapaneseDurationExtractorConfiguration().more_or_less_regex
        self._duration_unit_regex = JapaneseDurationExtractorConfiguration().duration_unit_regex
        self._an_unit_regex = JapaneseDurationExtractorConfiguration().an_unit_regex
        self._duration_connector_regex = JapaneseDurationExtractorConfiguration().duration_connector_regex

        self._unit_map = config.unit_map
        self._unit_value_map = config.unit_value_map

