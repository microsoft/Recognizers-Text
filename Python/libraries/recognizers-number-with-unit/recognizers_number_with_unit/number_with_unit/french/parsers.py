from recognizers_text.culture import Culture
from recognizers_text.extractor import Extractor
from recognizers_text.parser import Parser
from recognizers_number.culture import CultureInfo
from recognizers_number.number.french.extractors import FrenchNumberExtractor, NumberMode
from recognizers_number.number.parser_factory import AgnosticNumberParserFactory, ParserType
from recognizers_number.number.french.parsers import FrenchNumberParserConfiguration
from recognizers_number_with_unit.number_with_unit.parsers import NumberWithUnitParserConfiguration
from recognizers_number_with_unit.resources.french_numeric_with_unit import FrenchNumericWithUnit


class FrenchNumberWithUnitParserConfiguration(NumberWithUnitParserConfiguration):
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
            culture_info = CultureInfo(Culture.French)
        super().__init__(culture_info)
        self._internal_number_extractor = FrenchNumberExtractor(
            NumberMode.DEFAULT)
        self._internal_number_parser = AgnosticNumberParserFactory.get_parser(
            ParserType.NUMBER, FrenchNumberParserConfiguration(culture_info))
        self._connector_token = FrenchNumericWithUnit.ConnectorToken


class FrenchAgeParserConfiguration(FrenchNumberWithUnitParserConfiguration):
    def __init__(self, culture_info: CultureInfo = None):
        super().__init__(culture_info)
        self.add_dict_to_unit_map(FrenchNumericWithUnit.AgeSuffixList)


class FrenchCurrencyParserConfiguration(FrenchNumberWithUnitParserConfiguration):
    def __init__(self, culture_info: CultureInfo = None):
        super().__init__(culture_info)
        self.add_dict_to_unit_map(FrenchNumericWithUnit.CurrencySuffixList)
        self.add_dict_to_unit_map(FrenchNumericWithUnit.CurrencyPrefixList)


class FrenchDimensionParserConfiguration(FrenchNumberWithUnitParserConfiguration):
    def __init__(self, culture_info: CultureInfo = None):
        super().__init__(culture_info)
        self.add_dict_to_unit_map(FrenchNumericWithUnit.InformationSuffixList)
        self.add_dict_to_unit_map(FrenchNumericWithUnit.AreaSuffixList)
        self.add_dict_to_unit_map(FrenchNumericWithUnit.LengthSuffixList)
        self.add_dict_to_unit_map(FrenchNumericWithUnit.SpeedSuffixList)
        self.add_dict_to_unit_map(FrenchNumericWithUnit.VolumeSuffixList)
        self.add_dict_to_unit_map(FrenchNumericWithUnit.WeightSuffixList)


class FrenchTemperatureParserConfiguration(FrenchNumberWithUnitParserConfiguration):
    def __init__(self, culture_info: CultureInfo = None):
        super().__init__(culture_info)
        self._connector_token = None
        self.add_dict_to_unit_map(FrenchNumericWithUnit.TemperatureSuffixList)
