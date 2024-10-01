from typing import Dict, List, Pattern

from recognizers_text.culture import Culture
from recognizers_text.extractor import Extractor
from recognizers_text.utilities import RegExpUtility
from recognizers_number.culture import CultureInfo
from recognizers_number.number.japanese.extractors import JapaneseNumberExtractor, JapaneseNumberExtractorMode
from recognizers_number_with_unit.number_with_unit.constants import Constants
from recognizers_number_with_unit.number_with_unit.extractors import NumberWithUnitExtractorConfiguration
from recognizers_number_with_unit.resources.japanese_numeric_with_unit import JapaneseNumericWithUnit
from recognizers_number_with_unit.resources.base_units import BaseUnits


# pylint: disable=abstract-method
class JapaneseNumberWithUnitExtractorConfiguration(NumberWithUnitExtractorConfiguration):

    @property
    def ambiguity_filters_dict(self) -> Dict[Pattern, Pattern]:
        return JapaneseNumericWithUnit.AmbiguityFiltersDict

    @property
    def unit_num_extractor(self) -> Extractor:
        return self._unit_num_extractor

    @property
    def build_prefix(self) -> str:
        return self._build_prefix

    @property
    def build_suffix(self) -> str:
        return self._build_suffix

    @property
    def connector_token(self) -> str:
        return self._connector_token

    @property
    def compound_unit_connector_regex(self) -> Pattern:
        return self._compound_unit_connector_regex

    @property
    def non_unit_regex(self) -> Pattern:
        return self._pm_non_unit_regex

    @property
    def half_unit_regex(self) -> Pattern:
        return self._half_unit_regex

    @property
    def ambiguous_unit_number_multiplier_regex(self) -> Pattern:
        return None

    def expand_half_suffix(self, source, result, numbers):
        pass

    def __init__(self, culture_info: CultureInfo):
        if culture_info is None:
            culture_info = CultureInfo(Culture.Japanese)
        super().__init__(culture_info)
        self._unit_num_extractor = JapaneseNumberExtractor(JapaneseNumberExtractorMode.EXTRACT_ALL)
        self._build_prefix = JapaneseNumericWithUnit.BuildPrefix
        self._build_suffix = JapaneseNumericWithUnit.BuildSuffix
        self._connector_token = JapaneseNumericWithUnit.ConnectorToken
        self._compound_unit_connector_regex = RegExpUtility.get_safe_reg_exp(
            JapaneseNumericWithUnit.CompoundUnitConnectorRegex)
        self._pm_non_unit_regex = RegExpUtility.get_safe_reg_exp(
            BaseUnits.PmNonUnitRegex)
        self._half_unit_regex = RegExpUtility.get_safe_reg_exp(JapaneseNumericWithUnit.HalfUnitRegex)


# pylint: enable=abstract-method

class JapaneseCurrencyExtractorConfiguration(JapaneseNumberWithUnitExtractorConfiguration):
    @property
    def extract_type(self) -> str:
        return Constants.SYS_UNIT_CURRENCY

    @property
    def suffix_list(self) -> Dict[str, str]:
        return self._suffix_list

    @property
    def prefix_list(self) -> Dict[str, str]:
        return self._prefix_list

    @property
    def ambiguous_unit_list(self) -> List[str]:
        return self._ambiguous_unit_list

    def __init__(self, culture_info: CultureInfo = None):
        super().__init__(culture_info)
        self._suffix_list = JapaneseNumericWithUnit.CurrencySuffixList
        self._prefix_list = JapaneseNumericWithUnit.CurrencyPrefixList
        self._ambiguous_unit_list = JapaneseNumericWithUnit.CurrencyAmbiguousValues

