from recognizers_text.culture import Culture
from recognizers_text.extractor import Extractor
from recognizers_text.parser import Parser
from recognizers_number.culture import CultureInfo
from recognizers_number.number.catalan.extractors import CatalanNumberExtractor, NumberMode
from recognizers_number.number.parser_factory import AgnosticNumberParserFactory, ParserType
from recognizers_number.number.catalan.parsers import CatalanNumberParserConfiguration
from recognizers_number_with_unit.number_with_unit.parsers import NumberWithUnitParserConfiguration
from recognizers_number_with_unit.resources.catalan_numeric_with_unit import CatalanNumericWithUnit


class CatalanNumberWithUnitParserConfiguration(NumberWithUnitParserConfiguration):
    @property
    def internal_number_parser(self) -> Parser:
        return self._internal_number_parser

    @property
    def internal_number_extractor(self) -> Extractor:
        return self._internal_number_extractor

    @property
    def connector_token(self) -> str:
        return None

    def __init__(self, culture_info: CultureInfo):
        if culture_info is None:
            culture_info = CultureInfo(Culture.Catalan)
        super().__init__(culture_info)
        self._internal_number_extractor = CatalanNumberExtractor(
            NumberMode.DEFAULT)
        self._internal_number_parser = AgnosticNumberParserFactory.get_parser(
            ParserType.NUMBER, CatalanNumberParserConfiguration(culture_info))


class CatalanCurrencyParserConfiguration(CatalanNumberWithUnitParserConfiguration):
    def __init__(self, culture_info: CultureInfo = None):
        super().__init__(culture_info)
        self.add_dict_to_unit_map(CatalanNumericWithUnit.CurrencySuffixList)
        self.add_dict_to_unit_map(CatalanNumericWithUnit.CurrencyPrefixList)
        self.currency_name_to_iso_code_map = CatalanNumericWithUnit.CurrencyNameToIsoCodeMap
        self.currency_fraction_code_list = CatalanNumericWithUnit.FractionalUnitNameToCodeMap