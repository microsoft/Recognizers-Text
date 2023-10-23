from recognizers_text.culture import Culture
from recognizers_text.extractor import Extractor
from recognizers_text.parser import Parser
from recognizers_number.culture import CultureInfo
from recognizers_number.number.arabic.extractors import ArabicNumberExtractor, NumberMode
from recognizers_number.number.parser_factory import AgnosticNumberParserFactory, ParserType
from recognizers_number.number.arabic.parsers import ArabicNumberParserConfiguration
from recognizers_number_with_unit.number_with_unit.parsers import NumberWithUnitParserConfiguration
from recognizers_number_with_unit.resources.arabic_numeric_with_unit import ArabicNumericWithUnit


class ArabicNumberWithUnitParserConfiguration(NumberWithUnitParserConfiguration):
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
            culture_info = CultureInfo(Culture.Arabic)
        super().__init__(culture_info)
        self._internal_number_extractor = ArabicNumberExtractor(
            NumberMode.DEFAULT)
        self._internal_number_parser = AgnosticNumberParserFactory.get_parser(
            ParserType.NUMBER, ArabicNumberParserConfiguration(culture_info))


class ArabicCurrencyParserConfiguration(ArabicNumberWithUnitParserConfiguration):
    def __init__(self, culture_info: CultureInfo = None):
        super().__init__(culture_info)
        self.add_dict_to_unit_map(ArabicNumericWithUnit.CurrencySuffixList)
        self.add_dict_to_unit_map(ArabicNumericWithUnit.CurrencyPrefixList)
        self.currency_name_to_iso_code_map = ArabicNumericWithUnit.CurrencyNameToIsoCodeMap
        self.currency_fraction_code_list = ArabicNumericWithUnit.FractionalUnitNameToCodeMap