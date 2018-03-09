from enum import Enum

from recognizers_number.number.parsers import NumberParserConfiguration, BaseNumberParser, BasePercentageParser
from recognizers_number.number.constants import Constants

class ParserType(Enum):
    NUMBER=0
    CARDINAL=1
    DOUBLE=2
    FRACTION=3
    INTEGER=4
    ORDINAL=5
    PERCENTAGE=6

class AgnosticNumberParserFactory:
    @staticmethod
    def get_parser(parser_type: ParserType, language_config: NumberParserConfiguration) -> BaseNumberParser:
        parser = BaseNumberParser(language_config)

        if parser_type is ParserType.CARDINAL:
            parser.supported_types = [Constants.SYS_NUM_CARDINAL, Constants.SYS_NUM_INTEGER, Constants.SYS_NUM_DOUBLE]
        elif parser_type is ParserType.DOUBLE:
            parser.supported_types = [Constants.SYS_NUM_DOUBLE]
        elif parser_type is ParserType.FRACTION:
            parser.supported_types = [Constants.SYS_NUM_FRACTION]
        elif parser_type is ParserType.INTEGER:
            parser.supported_types = [Constants.SYS_NUM_INTEGER]
        elif parser_type is ParserType.ORDINAL:
            parser.supported_types = [Constants.SYS_NUM_ORDINAL]
        elif parser_type is ParserType.PERCENTAGE:
            parser = BasePercentageParser(language_config)
        
        return parser