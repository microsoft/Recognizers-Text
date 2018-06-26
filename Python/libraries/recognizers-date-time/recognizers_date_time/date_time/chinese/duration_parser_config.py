from recognizers_number import CultureInfo, Culture
from recognizers_number_with_unit import ChineseNumberWithUnitParserConfiguration

from ...resources.chinese_date_time import ChineseDateTime
from ..base_duration import DurationParserConfiguration

class ChineseDurationParserConfiguration(DurationParserConfiguration):
    @property
    def unit_value_map(self) -> any:
        return self._unit_value_map

    def __init__(self):
        self._unit_value_map = ChineseDateTime.DurationUnitValueMap

class ChineseDurationNumberWithUnitParserConfiguration(ChineseNumberWithUnitParserConfiguration):
    def __init__(self):
        super().__init__(CultureInfo(Culture.Chinese))
        self.add_dict_to_unit_map(ChineseDateTime.DurationSuffixList)
