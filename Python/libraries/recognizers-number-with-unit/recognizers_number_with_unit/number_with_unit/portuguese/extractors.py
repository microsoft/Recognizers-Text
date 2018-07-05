from typing import Dict, List, Pattern

from recognizers_text.culture import Culture
from recognizers_text.extractor import Extractor
from recognizers_text.utilities import RegExpUtility
from recognizers_number.culture import CultureInfo
from recognizers_number.number.portuguese.extractors import PortugueseNumberExtractor
from recognizers_number_with_unit.number_with_unit.constants import Constants
from recognizers_number_with_unit.number_with_unit.extractors import NumberWithUnitExtractorConfiguration
from recognizers_number_with_unit.resources.portuguese_numeric_with_unit import PortugueseNumericWithUnit

# pylint: disable=abstract-method
class PortugueseNumberWithUnitExtractorConfiguration(NumberWithUnitExtractorConfiguration):
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

    def __init__(self, culture_info: CultureInfo):
        if culture_info is None:
            culture_info = CultureInfo(Culture.Portuguese)
        super().__init__(culture_info)
        self._unit_num_extractor = PortugueseNumberExtractor()
        self._build_prefix = PortugueseNumericWithUnit.BuildPrefix
        self._build_suffix = PortugueseNumericWithUnit.BuildSuffix
        self._connector_token = PortugueseNumericWithUnit.ConnectorToken
        self._compound_unit_connector_regex = RegExpUtility.get_safe_reg_exp(PortugueseNumericWithUnit.CompoundUnitConnectorRegex)
# pylint: enable=abstract-method

class PortugueseAgeExtractorConfiguration(PortugueseNumberWithUnitExtractorConfiguration):
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
        self._suffix_list = PortugueseNumericWithUnit.AgeSuffixList
        self._prefix_list = dict()
        self._ambiguous_unit_list = list()

class PortugueseCurrencyExtractorConfiguration(PortugueseNumberWithUnitExtractorConfiguration):
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
        self._suffix_list = PortugueseNumericWithUnit.CurrencySuffixList
        self._prefix_list = PortugueseNumericWithUnit.CurrencyPrefixList
        self._ambiguous_unit_list = PortugueseNumericWithUnit.AmbiguousCurrencyUnitList

class PortugueseDimensionExtractorConfiguration(PortugueseNumberWithUnitExtractorConfiguration):
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
        self._suffix_list = {
            **PortugueseNumericWithUnit.InformationSuffixList,
            **PortugueseNumericWithUnit.AreaSuffixList,
            **PortugueseNumericWithUnit.LenghtSuffixList,
            **PortugueseNumericWithUnit.SpeedSuffixList,
            **PortugueseNumericWithUnit.VolumeSuffixList,
            **PortugueseNumericWithUnit.WeightSuffixList
        }
        self._prefix_list = dict()
        self._ambiguous_unit_list = PortugueseNumericWithUnit.AmbiguousDimensionUnitList

class PortugueseTemperatureExtractorConfiguration(PortugueseNumberWithUnitExtractorConfiguration):
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
        self._suffix_list = PortugueseNumericWithUnit.TemperatureSuffixList
        self._prefix_list = dict()
        self._ambiguous_unit_list = list()
