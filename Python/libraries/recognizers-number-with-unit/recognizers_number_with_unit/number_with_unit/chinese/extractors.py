from typing import Dict, List, Pattern
import regex

from recognizers_text.culture import Culture
from recognizers_text.extractor import Extractor, ExtractResult
from recognizers_text.utilities import RegExpUtility
from recognizers_number.culture import CultureInfo
from recognizers_number.number.chinese.extractors import ChineseNumberExtractor, ChineseNumberExtractorMode
from recognizers_number_with_unit.number_with_unit.constants import Constants
from recognizers_number_with_unit.number_with_unit.extractors import NumberWithUnitExtractorConfiguration, NumberWithUnitExtractor
from recognizers_number_with_unit.resources.chinese_numeric_with_unit import ChineseNumericWithUnit
from recognizers_number_with_unit.resources.base_units import BaseUnits
from recognizers_text.matcher.match_result import MatchResult


# pylint: disable=abstract-method
class ChineseNumberWithUnitExtractorConfiguration(NumberWithUnitExtractorConfiguration):
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
        return ChineseNumericWithUnit.ConnectorToken

    @property
    def compound_unit_connector_regex(self) -> Pattern:
        return self._compound_unit_connector_regex

    @property
    def non_unit_regex(self) -> Pattern:
        return self._pm_non_unit_regex

    @property
    def ambiguous_unit_number_multiplier_regex(self) -> Pattern:
        return None

    @property
    def ambiguity_filters_dict(self) -> Dict[Pattern, Pattern]:
        return None

    @property
    def extract_type(self) -> str:
        raise NotImplementedError

    @property
    def suffix_list(self) -> Dict[str, str]:
        raise NotImplementedError

    @property
    def prefix_list(self) -> Dict[str, str]:
        raise NotImplementedError

    @property
    def ambiguous_unit_list(self) -> List[str]:
        raise NotImplementedError

    @property
    def culture_info(self) -> CultureInfo:
        return self._culture_info

    def expand_half_suffix(self, source, result, numbers):
        # Expand Chinese phrase to the `half` patterns when it follows closely origin phrase.
        if self._half_unit_regex and numbers:
            match = [number for number in numbers if regex.match(self._half_unit_regex, number.text)]
            if match:
                res = []
                for er in result:
                    start = er.start
                    length = er.length
                    match_suffix = [mr for mr in match if mr.start == (start + length)]
                    if len(match_suffix) == 1:
                        mr = match_suffix[0]
                        er.length += mr.length
                        er.text += mr.text
                        er.data = [er.data, mr]
                    res.append(er)
                result = res

    def __init__(self, culture_info: CultureInfo):
        if culture_info is None:
            culture_info = CultureInfo(Culture.Chinese)
        super().__init__(culture_info)
        self._unit_num_extractor = ChineseNumberExtractor(
            ChineseNumberExtractorMode.EXTRACT_ALL)
        self._build_prefix = ChineseNumericWithUnit.BuildPrefix
        self._build_suffix = ChineseNumericWithUnit.BuildSuffix
        self._compound_unit_connector_regex = RegExpUtility.get_safe_reg_exp(
            ChineseNumericWithUnit.CompoundUnitConnectorRegex)
        self._pm_non_unit_regex = RegExpUtility.get_safe_reg_exp(
            BaseUnits.PmNonUnitRegex)
        self._half_unit_regex = RegExpUtility.get_safe_reg_exp(ChineseNumericWithUnit.HalfUnitRegex)

# pylint: enable=abstract-method


class ChineseAgeExtractorConfiguration(ChineseNumberWithUnitExtractorConfiguration):
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
        self._suffix_list = ChineseNumericWithUnit.AgeSuffixList
        self._prefix_list = dict()
        self._ambiguous_unit_list = ChineseNumericWithUnit.AgeAmbiguousValues


class ChineseCurrencyExtractorConfiguration(ChineseNumberWithUnitExtractorConfiguration):
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

    def __init__(self, culture_info: CultureInfo = Culture.Chinese):
        super().__init__(culture_info)
        self._suffix_list = ChineseNumericWithUnit.CurrencySuffixList
        self._prefix_list = ChineseNumericWithUnit.CurrencyPrefixList
        self._ambiguous_unit_list = ChineseNumericWithUnit.CurrencyAmbiguousValues


class ChineseDimensionExtractorConfiguration(ChineseNumberWithUnitExtractorConfiguration):
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
        self._suffix_list = ChineseNumericWithUnit.DimensionSuffixList
        self._prefix_list = dict()
        self._ambiguous_unit_list = ChineseNumericWithUnit.DimensionAmbiguousValues


class ChineseTemperatureExtractorConfiguration(ChineseNumberWithUnitExtractorConfiguration):
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
        self._suffix_list = ChineseNumericWithUnit.TemperatureSuffixList
        self._prefix_list = ChineseNumericWithUnit.TemperaturePrefixList
        self._ambiguous_unit_list = ChineseNumericWithUnit.TemperatureAmbiguousValues
        self._ambiguous_unit_number_multiplier_regex = RegExpUtility.get_safe_reg_exp(
            BaseUnits.AmbiguousUnitNumberMultiplierRegex)
