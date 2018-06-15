from typing import Dict, List, Pattern

from recognizers_text.culture import Culture
from recognizers_text.extractor import Extractor
from recognizers_text.utilities import RegExpUtility
from recognizers_number.culture import CultureInfo
from recognizers_number.number.english.extractors import EnglishNumberExtractor
from recognizers_number_with_unit.number_with_unit.constants import Constants
from recognizers_number_with_unit.number_with_unit.extractors import NumberWithUnitExtractorConfiguration
from recognizers_number_with_unit.resources.english_numeric_with_unit import EnglishNumericWithUnit

# pylint: disable=abstract-method
class EnglishNumberWithUnitExtractorConfiguration(NumberWithUnitExtractorConfiguration):
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
        return ''

    @property
    def compound_unit_connector_regex(self) -> Pattern:
        return self._compound_unit_connector_regex

    def __init__(self, culture_info: CultureInfo):
        if culture_info is None:
            culture_info = CultureInfo(Culture.English)
        super().__init__(culture_info)
        self._unit_num_extractor = EnglishNumberExtractor()
        self._build_prefix = EnglishNumericWithUnit.BuildPrefix
        self._build_suffix = EnglishNumericWithUnit.BuildSuffix
        self._compound_unit_connector_regex = RegExpUtility.get_safe_reg_exp(EnglishNumericWithUnit.CompoundUnitConnectorRegex)
# pylint: enable=abstract-method

class EnglishAgeExtractorConfiguration(EnglishNumberWithUnitExtractorConfiguration):
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
        self._suffix_list = EnglishNumericWithUnit.AgeSuffixList
        self._prefix_list = dict()
        self._ambiguous_unit_list = list()

class EnglishCurrencyExtractorConfiguration(EnglishNumberWithUnitExtractorConfiguration):
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
        self._suffix_list = EnglishNumericWithUnit.CurrencySuffixList
        self._prefix_list = EnglishNumericWithUnit.CurrencyPrefixList
        self._ambiguous_unit_list = EnglishNumericWithUnit.AmbiguousCurrencyUnitList

class EnglishDimensionExtractorConfiguration(EnglishNumberWithUnitExtractorConfiguration):
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
            **EnglishNumericWithUnit.InformationSuffixList,
            **EnglishNumericWithUnit.AreaSuffixList,
            **EnglishNumericWithUnit.LenghtSuffixList,
            **EnglishNumericWithUnit.SpeedSuffixList,
            **EnglishNumericWithUnit.VolumeSuffixList,
            **EnglishNumericWithUnit.WeightSuffixList
        }
        self._prefix_list = dict()
        self._ambiguous_unit_list = EnglishNumericWithUnit.AmbiguousDimensionUnitList

class EnglishTemperatureExtractorConfiguration(EnglishNumberWithUnitExtractorConfiguration):
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
        self._suffix_list = EnglishNumericWithUnit.TemperatureSuffixList
        self._prefix_list = dict()
        self._ambiguous_unit_list = EnglishNumericWithUnit.AmbiguousTemperatureUnitList
