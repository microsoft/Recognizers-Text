from recognizers_text.culture import Culture
from recognizers_text.extractor import Extractor
from recognizers_text.parser import Parser
from recognizers_number.culture import CultureInfo
from recognizers_number.number.portuguese.extractors import PortugueseNumberExtractor, NumberMode
from recognizers_number.number.parser_factory import AgnosticNumberParserFactory, ParserType
from recognizers_number.number.portuguese.parsers import PortugueseNumberParserConfiguration
from recognizers_number_with_unit.number_with_unit.parsers import NumberWithUnitParserConfiguration
from recognizers_number_with_unit.resources.portuguese_numeric_with_unit import PortugueseNumericWithUnit


class PortugueseNumberWithUnitParserConfiguration(NumberWithUnitParserConfiguration):
    @property
    def internal_number_parser(self) -> Parser:
        return self._internal_number_parser

    @property
    def internal_number_extractor(self) -> Extractor:
        return self._internal_number_extractor

    @property
    def connector_token(self) -> str:
        return self._connector_token

    def __init__(self, culture_info: CultureInfo):
        if culture_info is None:
            culture_info = CultureInfo(Culture.Portuguese)
        super().__init__(culture_info)
        self._internal_number_extractor = PortugueseNumberExtractor(
            NumberMode.DEFAULT)
        self._internal_number_parser = AgnosticNumberParserFactory.get_parser(
            ParserType.NUMBER, PortugueseNumberParserConfiguration(culture_info))
        self._connector_token = PortugueseNumericWithUnit.ConnectorToken


class PortugueseAgeParserConfiguration(PortugueseNumberWithUnitParserConfiguration):
    def __init__(self, culture_info: CultureInfo = None):
        super().__init__(culture_info)
        self.add_dict_to_unit_map(PortugueseNumericWithUnit.AgeSuffixList)


class PortugueseCurrencyParserConfiguration(PortugueseNumberWithUnitParserConfiguration):
    def __init__(self, culture_info: CultureInfo = None):
        super().__init__(culture_info)
        self.add_dict_to_unit_map(PortugueseNumericWithUnit.CurrencySuffixList)
        self.add_dict_to_unit_map(PortugueseNumericWithUnit.CurrencyPrefixList)


class PortugueseDimensionParserConfiguration(PortugueseNumberWithUnitParserConfiguration):
    def __init__(self, culture_info: CultureInfo = None):
        super().__init__(culture_info)
        self.add_dict_to_unit_map(
            PortugueseNumericWithUnit.InformationSuffixList)
        self.add_dict_to_unit_map(PortugueseNumericWithUnit.AreaSuffixList)
        self.add_dict_to_unit_map(PortugueseNumericWithUnit.LengthSuffixList)
        self.add_dict_to_unit_map(PortugueseNumericWithUnit.SpeedSuffixList)
        self.add_dict_to_unit_map(PortugueseNumericWithUnit.VolumeSuffixList)
        self.add_dict_to_unit_map(PortugueseNumericWithUnit.WeightSuffixList)


class PortugueseTemperatureParserConfiguration(PortugueseNumberWithUnitParserConfiguration):
    def __init__(self, culture_info: CultureInfo = None):
        super().__init__(culture_info)
        self.add_dict_to_unit_map(
            PortugueseNumericWithUnit.TemperatureSuffixList)
