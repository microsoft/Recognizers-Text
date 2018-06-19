from typing import Dict, List, Pattern

from recognizers_text.culture import Culture
from recognizers_text.extractor import Extractor
from recognizers_text.utilities import RegExpUtility
from recognizers_number.culture import CultureInfo
from recognizers_number.number.spanish.extractors import SpanishNumberExtractor
from recognizers_number_with_unit.number_with_unit.constants import Constants
from recognizers_number_with_unit.number_with_unit.extractors import NumberWithUnitExtractorConfiguration
from recognizers_number_with_unit.resources.spanish_numeric_with_unit import SpanishNumericWithUnit

# pylint: disable=abstract-method
class SpanishNumberWithUnitExtractorConfiguration(NumberWithUnitExtractorConfiguration):
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
        return SpanishNumericWithUnit.ConnectorToken

    @property
    def compound_unit_connector_regex(self) -> Pattern:
        return self._compound_unit_connector_regex

    def __init__(self, culture_info: CultureInfo):
        if culture_info is None:
            culture_info = CultureInfo(Culture.Spanish)
        super().__init__(culture_info)
        self._unit_num_extractor = SpanishNumberExtractor()
        self._build_prefix = SpanishNumericWithUnit.BuildPrefix
        self._build_suffix = SpanishNumericWithUnit.BuildSuffix
        self._compound_unit_connector_regex = RegExpUtility.get_safe_reg_exp(SpanishNumericWithUnit.CompoundUnitConnectorRegex)
# pylint: enable=abstract-method

class SpanishAgeExtractorConfiguration(SpanishNumberWithUnitExtractorConfiguration):
    @property
    def extract_type(self) -> str:
        return Constants.SYS_UNIT_AGE

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
        self._suffix_list = SpanishNumericWithUnit.AgeSuffixList
        self._prefix_list = dict()
        self._ambiguous_unit_list = list()

class SpanishCurrencyExtractorConfiguration(SpanishNumberWithUnitExtractorConfiguration):
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
        self._suffix_list = SpanishNumericWithUnit.CurrencySuffixList
        self._prefix_list = SpanishNumericWithUnit.CurrencyPrefixList
        self._ambiguous_unit_list = SpanishNumericWithUnit.AmbiguousCurrencyUnitList

class SpanishDimensionExtractorConfiguration(SpanishNumberWithUnitExtractorConfiguration):
    @property
    def extract_type(self) -> str:
        return Constants.SYS_UNIT_DIMENSION

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
        self._suffix_list = SpanishNumericWithUnit.DimensionSuffixList
        self._prefix_list = dict()
        self._ambiguous_unit_list = SpanishNumericWithUnit.AmbiguousDimensionUnitList

class SpanishTemperatureExtractorConfiguration(SpanishNumberWithUnitExtractorConfiguration):
    @property
    def extract_type(self) -> str:
        return Constants.SYS_UNIT_TEMPERATURE

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
        self._suffix_list = SpanishNumericWithUnit.TemperatureSuffixList
        self._prefix_list = dict()
        self._ambiguous_unit_list = []
