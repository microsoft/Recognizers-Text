from recognizers_text.culture import Culture
from recognizers_text.extractor import Extractor
from recognizers_text.parser import Parser
from recognizers_number.culture import CultureInfo
from recognizers_number.number.chinese.extractors import ChineseNumberExtractor, ChineseNumberExtractorMode
from recognizers_number.number.parser_factory import AgnosticNumberParserFactory, ParserType
from recognizers_number.number.chinese.parsers import ChineseNumberParserConfiguration
from recognizers_number_with_unit.number_with_unit.parsers import NumberWithUnitParserConfiguration
from recognizers_number_with_unit.resources.chinese_numeric_with_unit import ChineseNumericWithUnit

class ChineseNumberWithUnitParserConfiguration(NumberWithUnitParserConfiguration):
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
            culture_info = CultureInfo(Culture.Chinese)
        super().__init__(culture_info)
        self._internal_number_extractor = ChineseNumberExtractor(ChineseNumberExtractorMode.EXTRACT_ALL)
        self._internal_number_parser = AgnosticNumberParserFactory.get_parser(ParserType.NUMBER, ChineseNumberParserConfiguration(culture_info))
        self.currency_name_to_iso_code_map = ChineseNumericWithUnit.CurrencyNameToIsoCodeMap
        self.currency_fraction_code_list = ChineseNumericWithUnit.FractionalUnitNameToCodeMap

class ChineseAgeParserConfiguration(ChineseNumberWithUnitParserConfiguration):
    def __init__(self, culture_info: CultureInfo = None):
        super().__init__(culture_info)
        self.add_dict_to_unit_map(ChineseNumericWithUnit.AgeSuffixList)

class ChineseCurrencyParserConfiguration(ChineseNumberWithUnitParserConfiguration):
    def __init__(self, culture_info: CultureInfo = None):
        super().__init__(culture_info)
        self.add_dict_to_unit_map(ChineseNumericWithUnit.CurrencySuffixList)
        self.add_dict_to_unit_map(ChineseNumericWithUnit.CurrencyPrefixList)

class ChineseDimensionParserConfiguration(ChineseNumberWithUnitParserConfiguration):
    def __init__(self, culture_info: CultureInfo = None):
        super().__init__(culture_info)
        self.add_dict_to_unit_map(ChineseNumericWithUnit.DimensionSuffixList)

class ChineseTemperatureParserConfiguration(ChineseNumberWithUnitParserConfiguration):
    def __init__(self, culture_info: CultureInfo = None):
        super().__init__(culture_info)
        self.add_dict_to_unit_map(ChineseNumericWithUnit.TemperatureSuffixList)
        self.add_dict_to_unit_map(ChineseNumericWithUnit.TemperaturePrefixList)
