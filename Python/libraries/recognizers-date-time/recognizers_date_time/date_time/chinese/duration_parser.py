from recognizers_number_with_unit import NumberWithUnitParser

from ..base_duration import BaseDurationParser
from .duration_parser_config import ChineseDurationParserConfiguration, ChineseDurationNumberWithUnitParserConfiguration

class ChineseDurationParser(BaseDurationParser):
    def __init__(self):
        super().__init__(ChineseDurationParserConfiguration())
        self._internal_parser = NumberWithUnitParser(ChineseDurationNumberWithUnitParserConfiguration())
