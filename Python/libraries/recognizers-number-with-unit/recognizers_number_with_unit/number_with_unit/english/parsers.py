from recognizers_text import Culture
from recognizers_text.extractor import Extractor
from recognizers_text.parser import Parser
from recognizers_number.culture import CultureInfo
from recognizers_number.number.english.extractors import EnglishNumberExtractor, NumberMode
from recognizers_number.number.parser_factory import AgnosticNumberParserFactory, ParserType
from recognizers_number.number.english.parsers import EnglishNumberParserConfiguration
from recognizers_number_with_unit.number_with_unit.parsers import NumberWithUnitParserConfiguration
from recognizers_number_with_unit.resources.english_numeric_with_unit import EnglishNumericWithUnit

class EnglishNumberWithUnitParserConfiguration(NumberWithUnitParserConfiguration):
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
            culture_info = CultureInfo(Culture.English)
        super().__init__(culture_info)
        self._internal_number_extractor = EnglishNumberExtractor(NumberMode.DEFAULT)
        self._internal_number_parser = AgnosticNumberParserFactory.get_parser(ParserType.NUMBER, EnglishNumberParserConfiguration(culture_info))

class EnglishAgeParserConfiguration(EnglishNumberWithUnitParserConfiguration):
    def __init__(self, culture_info: CultureInfo = None):
        super().__init__(culture_info)
        self.add_dict_to_unit_map(EnglishNumericWithUnit.AgeSuffixList)

class EnglishCurrencyParserConfiguration(EnglishNumberWithUnitParserConfiguration):
    def __init__(self, culture_info: CultureInfo = None):
        super().__init__(culture_info)
        self.add_dict_to_unit_map(EnglishNumericWithUnit.CurrencySuffixList)
        self.add_dict_to_unit_map(EnglishNumericWithUnit.CurrencyPrefixList)
        self.currency_name_to_iso_code_map = EnglishNumericWithUnit.CurrencyNameToIsoCodeMap
        self.currency_fraction_code_list = EnglishNumericWithUnit.FractionalUnitNameToCodeMap

class EnglishDimensionParserConfiguration(EnglishNumberWithUnitParserConfiguration):
    def __init__(self, culture_info: CultureInfo = None):
        super().__init__(culture_info)
        self.add_dict_to_unit_map(EnglishNumericWithUnit.InformationSuffixList)
        self.add_dict_to_unit_map(EnglishNumericWithUnit.AreaSuffixList)
        self.add_dict_to_unit_map(EnglishNumericWithUnit.LenghtSuffixList)
        self.add_dict_to_unit_map(EnglishNumericWithUnit.SpeedSuffixList)
        self.add_dict_to_unit_map(EnglishNumericWithUnit.VolumeSuffixList)
        self.add_dict_to_unit_map(EnglishNumericWithUnit.WeightSuffixList)

class EnglishTemperatureParserConfiguration(EnglishNumberWithUnitParserConfiguration):
    def __init__(self, culture_info: CultureInfo = None):
        super().__init__(culture_info)
        self.add_dict_to_unit_map(EnglishNumericWithUnit.TemperatureSuffixList)
