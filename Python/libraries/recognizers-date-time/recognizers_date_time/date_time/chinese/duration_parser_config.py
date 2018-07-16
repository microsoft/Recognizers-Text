from recognizers_number import CultureInfo, Culture
from recognizers_number_with_unit import ChineseNumberWithUnitParserConfiguration

from ...resources.chinese_date_time import ChineseDateTime
from ..base_duration import DurationParserConfiguration

class ChineseDurationParserConfiguration(DurationParserConfiguration):
    @property
    def cardinal_extractor(self) -> any:
        return None

    @property
    def number_parser(self) -> any:
        return None

    @property
    def followed_unit(self) -> any:
        return None

    @property
    def suffix_and_regex(self) -> any:
        return None

    @property
    def number_combined_with_unit(self) -> any:
        return None

    @property
    def an_unit_regex(self) -> any:
        return None

    @property
    def all_date_unit_regex(self) -> any:
        return None

    @property
    def half_date_unit_regex(self) -> any:
        return None

    @property
    def inexact_number_unit_regex(self) -> any:
        return None

    @property
    def unit_map(self) -> any:
        return None

    @property
    def unit_value_map(self) -> any:
        return self._unit_value_map

    @property
    def double_numbers(self) -> any:
        return None

    def __init__(self):
        self._unit_value_map = ChineseDateTime.DurationUnitValueMap

class ChineseDurationNumberWithUnitParserConfiguration(ChineseNumberWithUnitParserConfiguration):
    def __init__(self):
        super().__init__(CultureInfo(Culture.Chinese))
        self.add_dict_to_unit_map(ChineseDateTime.DurationSuffixList)
