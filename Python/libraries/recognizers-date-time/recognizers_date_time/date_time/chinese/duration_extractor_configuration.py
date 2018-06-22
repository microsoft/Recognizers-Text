from typing import Dict

from recognizers_number.culture import Culture, CultureInfo
from recognizers_number_with_unit.number_with_unit.chinese.extractors import ChineseNumberWithUnitExtractorConfiguration
from ...resources.chinese_date_time import ChineseDateTime
from ..constants import Constants

class ChineseDurationExtractorConfiguration(ChineseNumberWithUnitExtractorConfiguration):
    @property
    def suffix_list(self) -> Dict[str, str]:
        return self._suffix_list

    @property
    def prefix_list(self) -> Dict[str, str]:
        return self._prefix_list

    @property
    def ambiguous_unit_list(self) -> Dict[str, str]:
        return self._ambiguous_unit_list

    @property
    def extract_type(self) -> str:
        return self._extract_type

    def __init__(self):
        super().__init__(CultureInfo(Culture.Chinese))
        self._extract_type = Constants.SYS_DATETIME_DURATION
        self._suffix_list = ChineseDateTime.DurationSuffixList
        self._prefix_list = dict()
        self._ambiguous_unit_list = ChineseDateTime.DurationAmbiguousUnits
