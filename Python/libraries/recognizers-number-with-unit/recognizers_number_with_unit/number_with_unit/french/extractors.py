from typing import Dict, List, Pattern

from recognizers_text.culture import Culture
from recognizers_text.extractor import Extractor
from recognizers_text.utilities import RegExpUtility
from recognizers_number.culture import CultureInfo
from recognizers_number.number.french.extractors import FrenchNumberExtractor
from recognizers_number_with_unit.number_with_unit.constants import Constants
from recognizers_number_with_unit.number_with_unit.extractors import NumberWithUnitExtractorConfiguration
from recognizers_number_with_unit.resources.french_numeric_with_unit import FrenchNumericWithUnit

# pylint: disable=abstract-method
class FrenchNumberWithUnitExtractorConfiguration(NumberWithUnitExtractorConfiguration):
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
            culture_info = CultureInfo(Culture.French)
        super().__init__(culture_info)
        self._unit_num_extractor = FrenchNumberExtractor()
        self._build_prefix = FrenchNumericWithUnit.BuildPrefix
        self._build_suffix = FrenchNumericWithUnit.BuildSuffix
        self._connector_token = FrenchNumericWithUnit.ConnectorToken
        self._compound_unit_connector_regex = RegExpUtility.get_safe_reg_exp(FrenchNumericWithUnit.CompoundUnitConnectorRegex)
# pylint: enable=abstract-method

class FrenchAgeExtractorConfiguration(FrenchNumberWithUnitExtractorConfiguration):
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
        self._suffix_list = FrenchNumericWithUnit.AgeSuffixList
        self._prefix_list = dict()
        self._ambiguous_unit_list = list()

class FrenchCurrencyExtractorConfiguration(FrenchNumberWithUnitExtractorConfiguration):
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
        self._suffix_list = FrenchNumericWithUnit.CurrencySuffixList
        self._prefix_list = FrenchNumericWithUnit.CurrencyPrefixList
        self._ambiguous_unit_list = FrenchNumericWithUnit.AmbiguousCurrencyUnitList

class FrenchDimensionExtractorConfiguration(FrenchNumberWithUnitExtractorConfiguration):
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
            **FrenchNumericWithUnit.InformationSuffixList,
            **FrenchNumericWithUnit.AreaSuffixList,
            **FrenchNumericWithUnit.LengthSuffixList,
            **FrenchNumericWithUnit.SpeedSuffixList,
            **FrenchNumericWithUnit.VolumeSuffixList,
            **FrenchNumericWithUnit.WeightSuffixList
        }
        self._prefix_list = dict()
        self._ambiguous_unit_list = FrenchNumericWithUnit.AmbiguousDimensionUnitList

class FrenchTemperatureExtractorConfiguration(FrenchNumberWithUnitExtractorConfiguration):
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
        self._suffix_list = FrenchNumericWithUnit.TemperatureSuffixList
        self._prefix_list = dict()
        self._ambiguous_unit_list = list()
