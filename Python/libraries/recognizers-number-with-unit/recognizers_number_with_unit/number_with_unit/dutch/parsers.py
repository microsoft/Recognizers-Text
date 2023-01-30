#  Copyright (c) Microsoft Corporation. All rights reserved.
#  Licensed under the MIT License.

from recognizers_text import Culture
from recognizers_text.extractor import Extractor
from recognizers_text.parser import Parser
from recognizers_number.culture import CultureInfo
from recognizers_number.number.dutch.extractors import DutchNumberExtractor, NumberMode
from recognizers_number.number.parser_factory import AgnosticNumberParserFactory, ParserType
from recognizers_number.number.dutch.parsers import DutchNumberParserConfiguration
from recognizers_number_with_unit.number_with_unit.parsers import NumberWithUnitParserConfiguration
from recognizers_number_with_unit.resources.dutch_numeric_with_unit import DutchNumericWithUnit


class DutchNumberWithUnitParserConfiguration(NumberWithUnitParserConfiguration):
    @property
    def internal_number_parser(self) -> Parser:
        return self._internal_number_parser

    @property
    def internal_number_extractor(self) -> Extractor:
        return self._internal_number_extractor

    @property
    def connector_token(self) -> str:
        return ''

    def __init__(self, culture_info: CultureInfo):
        if culture_info is None:
            culture_info = CultureInfo(Culture.Dutch)
        super().__init__(culture_info)
        self._internal_number_extractor = DutchNumberExtractor(
            NumberMode.DEFAULT)
        self._internal_number_parser = AgnosticNumberParserFactory.get_parser(
            ParserType.NUMBER, DutchNumberParserConfiguration(culture_info))


class DutchAgeParserConfiguration(DutchNumberWithUnitParserConfiguration):
    def __init__(self, culture_info: CultureInfo = None):
        super().__init__(culture_info)
        self.add_dict_to_unit_map(DutchNumericWithUnit.AgeSuffixList)


class DutchCurrencyParserConfiguration(DutchNumberWithUnitParserConfiguration):
    def __init__(self, culture_info: CultureInfo = None):
        super().__init__(culture_info)
        self.add_dict_to_unit_map(DutchNumericWithUnit.CurrencySuffixList)
        self.add_dict_to_unit_map(DutchNumericWithUnit.CurrencyPrefixList)
        self.currency_name_to_iso_code_map = DutchNumericWithUnit.CurrencyNameToIsoCodeMap
        self.currency_fraction_code_list = DutchNumericWithUnit.FractionalUnitNameToCodeMap


class DutchDimensionParserConfiguration(DutchNumberWithUnitParserConfiguration):
    def __init__(self, culture_info: CultureInfo = None):
        super().__init__(culture_info)
        self.add_dict_to_unit_map(DutchNumericWithUnit.InformationSuffixList)
        self.add_dict_to_unit_map(DutchNumericWithUnit.AreaSuffixList)
        self.add_dict_to_unit_map(DutchNumericWithUnit.LengthSuffixList)
        self.add_dict_to_unit_map(DutchNumericWithUnit.SpeedSuffixList)
        self.add_dict_to_unit_map(DutchNumericWithUnit.AngleSuffixList)
        self.add_dict_to_unit_map(DutchNumericWithUnit.VolumeSuffixList)
        self.add_dict_to_unit_map(DutchNumericWithUnit.WeightSuffixList)


class DutchTemperatureParserConfiguration(DutchNumberWithUnitParserConfiguration):
    def __init__(self, culture_info: CultureInfo = None):
        super().__init__(culture_info)
        self.add_dict_to_unit_map(DutchNumericWithUnit.TemperatureSuffixList)
