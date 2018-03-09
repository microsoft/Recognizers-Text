from enum import IntFlag
from typing import List
from recognizers_text import Culture, Recognizer, Model
from recognizers_number.number.models import NumberMode, NumberModel, OrdinalModel, PercentModel, ModelResult
from recognizers_number.number.english.extractors import EnglishNumberExtractor
from recognizers_number.number.english.parsers import EnglishNumberParserConfiguration
from recognizers_number.number.spanish.parsers import SpanishNumberParserConfiguration
from recognizers_number.number.parser_factory import ParserType, AgnosticNumberParserFactory

class NumberOptions(IntFlag):
    NONE = 0

class NumberRecognizer(Recognizer[NumberOptions]):
    def __init__(self, target_culture: str=None, options: NumberOptions=NumberOptions.NONE, lazy_initialization: bool=True):
        if options < NumberOptions.NONE or options > NumberOptions.NONE:
            raise ValueError()
        super().__init__(target_culture, options, lazy_initialization)

    def initialize_configuration(self):
        #region English
        self.register_model("NumberModel", Culture.English, lambda options: NumberModel(
            AgnosticNumberParserFactory.get_parser(ParserType.NUMBER, EnglishNumberParserConfiguration()),
            EnglishNumberExtractor(NumberMode.PURE_NUMBER)
        ))
        self.register_model("OrdinalModel", Culture.English, lambda options: OrdinalModel(
            AgnosticNumberParserFactory.get_parser(ParserType.ORDINAL, EnglishNumberParserConfiguration()),
            None #TODO implement EnglishOrdinalExtractor
        ))
        self.register_model("PercentModel", Culture.English, lambda options: PercentModel(
            AgnosticNumberParserFactory.get_parser(ParserType.PERCENTAGE, EnglishNumberParserConfiguration()),
            None #TODO implement EnglishPercentageExtractor
        ))
        #endregion

        #region Spanish
        self.register_model("NumberModel", Culture.Spanish, lambda options: NumberModel(
            AgnosticNumberParserFactory.get_parser(ParserType.NUMBER, SpanishNumberParserConfiguration()),
            None #TODO implement SpanishNumberExtractor
        ))
        self.register_model("OrdinalModel", Culture.Spanish, lambda options: OrdinalModel(
            AgnosticNumberParserFactory.get_parser(ParserType.ORDINAL, SpanishNumberParserConfiguration()),
            None #TODO implement SpanishOrdinalExtractor
        ))
        self.register_model("PercentModel", Culture.Spanish, lambda options: PercentModel(
            AgnosticNumberParserFactory.get_parser(ParserType.PERCENTAGE, SpanishNumberParserConfiguration()),
            None #TODO implement SpanishPercentageExtractor
        ))
        #endregion
        

    def get_number_model(self, culture: str=None, fallback_to_default_culture: bool=True) -> Model:
        return self.get_model("NumberModel", culture, fallback_to_default_culture)

    @staticmethod
    def recognize_number(query: str, culture: str, options: NumberOptions = NumberOptions.NONE, fallback_to_default_culture: bool = True) -> List[ModelResult]:
        pass

    @staticmethod
    def recognize_ordinal(query: str, culture: str, options: NumberOptions = NumberOptions.NONE, fallback_to_default_culture: bool = True) -> List[ModelResult]:
        pass

    @staticmethod
    def recognize_percentage(query: str, culture: str, options: NumberOptions = NumberOptions.NONE, fallback_to_default_culture: bool = True) -> List[ModelResult]:
        pass