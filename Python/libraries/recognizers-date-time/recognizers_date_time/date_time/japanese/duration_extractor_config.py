from typing import Dict, Pattern, List

from recognizers_number.culture import CultureInfo, Culture
from recognizers_text.extractor import Extractor
from recognizers_text.utilities import RegExpUtility, DefinitionLoader
from recognizers_date_time.date_time.constants import Constants
from recognizers_date_time.date_time.CJK import CJKDurationExtractorConfiguration
from recognizers_date_time.resources.japanese_date_time import JapaneseDateTime
from recognizers_number_with_unit.number_with_unit import NumberWithUnitExtractor
from recognizers_number_with_unit.number_with_unit.japanese import JapaneseNumberWithUnitExtractorConfiguration


class JapaneseDurationExtractorConfiguration(CJKDurationExtractorConfiguration):

    @property
    def year_regex(self) -> Pattern:
        return self._year_regex
    
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
    def all_regex(self) -> Pattern:
        return self._all_regex

    @property
    def half_regex(self) -> Pattern:
        return self._half_regex

    @property
    def relative_duration_unit_regex(self) -> Pattern:
        return self._relative_duration_unit_regex

    @property
    def during_regex(self) -> Pattern:
        return self._during_regex

    @property
    def some_regex(self) -> Pattern:
        return self._some_regex

    @property
    def more_or_less_regex(self) -> Pattern:
        return self._more_or_less_regex

    @property
    def internal_extractor(self) -> Extractor:
        return self._internal_extractor

    @property
    def unit_map(self) -> Dict[str, str]:
        return self._unit_map

    @property
    def unit_value_map(self) -> Dict[str, float]:
        return self._unit_value_map

    @property
    def ambiguity_duration_filters_dict(self) -> Dict[Pattern, Pattern]:
        return self._ambiguity_duration_filters_dict

    class DurationExtractorConfiguration(JapaneseNumberWithUnitExtractorConfiguration):
        @property
        def duration_suffix_list(self) -> Dict[str, str]:
            return JapaneseDateTime.DurationSuffixList

        @property
        def extract_type(self) -> str:
            return Constants.SYS_DATETIME_DURATION

        @property
        def suffix_list(self) -> Dict[str, str]:
            return self.duration_suffix_list

        @property
        def prefix_list(self) -> Dict[str, str]:
            return {}

        @property
        def ambiguous_unit_list(self) -> List[str]:
            return JapaneseDateTime.DurationAmbiguousUnits

        def __init__(self, culture_info: CultureInfo = CultureInfo(Culture.Japanese)):
            super().__init__(culture_info)

    def __init__(self, merge: bool = True):
        super().__init__()
        self.merge = merge
        self._year_regex = RegExpUtility.get_safe_reg_exp(JapaneseDateTime.DurationYearRegex)
        self._duration_unit_regex = RegExpUtility.get_safe_reg_exp(JapaneseDateTime.DurationUnitRegex)
        self._an_unit_regex = RegExpUtility.get_safe_reg_exp(JapaneseDateTime.AnUnitRegex)
        self._duration_connector_regex = RegExpUtility.get_safe_reg_exp(JapaneseDateTime.DurationConnectorRegex)
        self._all_regex = RegExpUtility.get_safe_reg_exp(JapaneseDateTime.DurationAllRegex)
        self._half_regex = RegExpUtility.get_safe_reg_exp(JapaneseDateTime.DurationHalfRegex)
        self._relative_duration_unit_regex = RegExpUtility.get_safe_reg_exp(
            JapaneseDateTime.DurationRelativeDurationUnitRegex)
        self._during_regex = RegExpUtility.get_safe_reg_exp(JapaneseDateTime.DurationDuringRegex)
        self._some_regex = RegExpUtility.get_safe_reg_exp(JapaneseDateTime.DurationSomeRegex)
        self._more_or_less_regex = RegExpUtility.get_safe_reg_exp(JapaneseDateTime.DurationMoreOrLessRegex)

        self._internal_extractor = NumberWithUnitExtractor(self.DurationExtractorConfiguration())

        self._unit_map = JapaneseDateTime.ParserConfigurationUnitMap
        self._unit_value_map = JapaneseDateTime.DurationUnitValueMap
        self._ambiguity_duration_filters_dict = DefinitionLoader.load_ambiguity_filters(
            JapaneseDateTime.AmbiguityDurationFiltersDict
        )


