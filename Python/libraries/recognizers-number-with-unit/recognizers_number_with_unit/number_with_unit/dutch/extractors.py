#  Copyright (c) Microsoft Corporation. All rights reserved.
#  Licensed under the MIT License.

from typing import Dict, List, Pattern

from recognizers_text.culture import Culture
from recognizers_text.extractor import Extractor
from recognizers_text.utilities import RegExpUtility, DefinitionLoader
from recognizers_number.culture import CultureInfo
from recognizers_number.number.models import NumberMode
from recognizers_number.number.dutch.extractors import DutchNumberExtractor
from recognizers_number_with_unit.number_with_unit.constants import Constants
from recognizers_number_with_unit.number_with_unit.extractors import NumberWithUnitExtractorConfiguration
from recognizers_number_with_unit.resources.dutch_numeric_with_unit import DutchNumericWithUnit
from recognizers_number_with_unit.resources.base_units import BaseUnits


# pylint: disable=abstract-method
class DutchNumberWithUnitExtractorConfiguration(NumberWithUnitExtractorConfiguration):
    @property
    def ambiguity_filters_dict(self) -> Dict[Pattern, Pattern]:
        return DefinitionLoader.load_ambiguity_filters(DutchNumericWithUnit.AmbiguityFiltersDict)

    @property
    def dimension_ambiguity_filters_dict(self) -> Dict[Pattern, Pattern]:
        return DefinitionLoader.load_ambiguity_filters(DutchNumericWithUnit.DimensionAmbiguityFiltersDict)

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

    @property
    def non_unit_regex(self) -> Pattern:
        return self._pm_non_unit_regex

    @property
    def ambiguous_unit_number_multiplier_regex(self) -> Pattern:
        return None

    def expand_half_suffix(self, source, result, numbers):
        pass

    def __init__(self, culture_info: CultureInfo):
        if culture_info is None:
            culture_info = CultureInfo(Culture.Dutch)
        super().__init__(culture_info)
        self._unit_num_extractor = DutchNumberExtractor(NumberMode.Unit)
        self._build_prefix = DutchNumericWithUnit.BuildPrefix
        self._build_suffix = DutchNumericWithUnit.BuildSuffix
        self._compound_unit_connector_regex = RegExpUtility.get_safe_reg_exp(
            DutchNumericWithUnit.CompoundUnitConnectorRegex)
        self._pm_non_unit_regex = RegExpUtility.get_safe_reg_exp(
            BaseUnits.PmNonUnitRegex)


# pylint: enable=abstract-method

class DutchAgeExtractorConfiguration(DutchNumberWithUnitExtractorConfiguration):
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
        self._suffix_list = DutchNumericWithUnit.AgeSuffixList
        self._prefix_list = dict()
        self._ambiguous_unit_list = list()


class DutchCurrencyExtractorConfiguration(DutchNumberWithUnitExtractorConfiguration):

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
        self._suffix_list = DutchNumericWithUnit.CurrencySuffixList
        self._prefix_list = DutchNumericWithUnit.CurrencyPrefixList
        self._ambiguous_unit_list = DutchNumericWithUnit.AmbiguousCurrencyUnitList


class DutchDimensionExtractorConfiguration(DutchNumberWithUnitExtractorConfiguration):

    @property
    def ambiguity_filters_dict(self) -> Dict[Pattern, Pattern]:
        return DutchNumericWithUnit.AmbiguityFiltersDict

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
            **DutchNumericWithUnit.InformationSuffixList,
            **DutchNumericWithUnit.AreaSuffixList,
            **DutchNumericWithUnit.LengthSuffixList,
            **DutchNumericWithUnit.AngleSuffixList,
            **DutchNumericWithUnit.SpeedSuffixList,
            **DutchNumericWithUnit.VolumeSuffixList,
            **DutchNumericWithUnit.WeightSuffixList
        }
        self._prefix_list = dict()
        self._ambiguous_unit_list = DutchNumericWithUnit.AmbiguousDimensionUnitList +\
            DutchNumericWithUnit.AmbiguousAngleUnitList +\
            DutchNumericWithUnit.AmbiguousLengthUnitList +\
            DutchNumericWithUnit.AmbiguousVolumeUnitList +\
            DutchNumericWithUnit.AmbiguousWeightUnitList


class DutchTemperatureExtractorConfiguration(DutchNumberWithUnitExtractorConfiguration):

    @property
    def ambiguity_filters_dict(self) -> Dict[Pattern, Pattern]:
        return DutchNumericWithUnit.AmbiguityFiltersDict

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

    @property
    def ambiguous_unit_number_multiplier_regex(self) -> Pattern:
        return self._ambiguous_unit_number_multiplier_regex

    def __init__(self, culture_info: CultureInfo = None):
        super().__init__(culture_info)
        self._suffix_list = DutchNumericWithUnit.TemperatureSuffixList
        self._prefix_list = dict()
        self._ambiguous_unit_list = DutchNumericWithUnit.AmbiguousTemperatureUnitList
        self._ambiguous_unit_number_multiplier_regex = RegExpUtility.get_safe_reg_exp(
            BaseUnits.AmbiguousUnitNumberMultiplierRegex)
