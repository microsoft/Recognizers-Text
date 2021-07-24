#  Copyright (c) Microsoft Corporation. All rights reserved.
#  Licensed under the MIT License.

from enum import IntFlag
from typing import List
from recognizers_text import Culture, Recognizer, Model
from recognizers_number.number.models import NumberMode, NumberModel, OrdinalModel, PercentModel, ModelResult
from recognizers_number.number.parser_factory import ParserType, AgnosticNumberParserFactory
from recognizers_number.number.english.extractors import EnglishNumberExtractor, EnglishOrdinalExtractor, EnglishPercentageExtractor
from recognizers_number.number.english.parsers import EnglishNumberParserConfiguration
from recognizers_number.number.spanish.extractors import SpanishNumberExtractor, SpanishOrdinalExtractor, SpanishPercentageExtractor
from recognizers_number.number.chinese.extractors import ChineseNumberExtractor, ChineseOrdinalExtractor, ChinesePercentageExtractor
from recognizers_number.number.chinese.parsers import ChineseNumberParserConfiguration
from recognizers_number.number.japanese.extractors import JapaneseNumberExtractor, JapaneseOrdinalExtractor, JapanesePercentageExtractor
from recognizers_number.number.japanese.parsers import JapaneseNumberParserConfiguration
from recognizers_number.number.spanish.parsers import SpanishNumberParserConfiguration
from recognizers_number.number.portuguese.extractors import PortugueseNumberExtractor, PortugueseOrdinalExtractor, PortuguesePercentageExtractor
from recognizers_number.number.portuguese.parsers import PortugueseNumberParserConfiguration
from recognizers_number.number.french.extractors import FrenchNumberExtractor, FrenchOrdinalExtractor, FrenchPercentageExtractor
from recognizers_number.number.french.parsers import FrenchNumberParserConfiguration
from recognizers_number.number.german.extractors import GermanNumberExtractor, GermanOrdinalExtractor, GermanPercentageExtractor
from recognizers_number.number.german.parsers import GermanNumberParserConfiguration


class NumberOptions(IntFlag):
    NONE = 0


class NumberRecognizer(Recognizer[NumberOptions]):
    def __init__(self, target_culture: str = None, options: NumberOptions = NumberOptions.NONE, lazy_initialization: bool = True):
        if options < NumberOptions.NONE or options > NumberOptions.NONE:
            raise ValueError()
        super().__init__(target_culture, options, lazy_initialization)

    def initialize_configuration(self):
        # region English
        self.register_model('NumberModel', Culture.English, lambda options: NumberModel(
            AgnosticNumberParserFactory.get_parser(
                ParserType.NUMBER, EnglishNumberParserConfiguration()),
            EnglishNumberExtractor(NumberMode.PURE_NUMBER)
        ))
        self.register_model('OrdinalModel', Culture.English, lambda options: OrdinalModel(
            AgnosticNumberParserFactory.get_parser(
                ParserType.ORDINAL, EnglishNumberParserConfiguration()),
            EnglishOrdinalExtractor()
        ))
        self.register_model('PercentModel', Culture.English, lambda options: PercentModel(
            AgnosticNumberParserFactory.get_parser(
                ParserType.PERCENTAGE, EnglishNumberParserConfiguration()),
            EnglishPercentageExtractor()
        ))
        # endregion

        # region German
        # self.register_model('NumberModel', Culture.German, lambda options: NumberModel(
        #    AgnosticNumberParserFactory.get_parser(
        #        ParserType.NUMBER, GermanNumberParserConfiguration()),
        #    GermanNumberExtractor(NumberMode.PURE_NUMBER)
        # ))
        # self.register_model('OrdinalModel', Culture.German, lambda options: OrdinalModel(
        #    AgnosticNumberParserFactory.get_parser(
        #        ParserType.ORDINAL, GermanNumberParserConfiguration()),
        #    GermanOrdinalExtractor()
        # ))
        # self.register_model('PercentModel', Culture.German, lambda options: PercentModel(
        #    AgnosticNumberParserFactory.get_parser(
        #        ParserType.PERCENTAGE, GermanNumberParserConfiguration()),
        #    GermanPercentageExtractor()
        # ))
        # endregion

        # region Chinese
        self.register_model('NumberModel', Culture.Chinese, lambda options: NumberModel(
            AgnosticNumberParserFactory.get_parser(
                ParserType.NUMBER, ChineseNumberParserConfiguration()),
            ChineseNumberExtractor()
        ))
        self.register_model('OrdinalModel', Culture.Chinese, lambda options: OrdinalModel(
            AgnosticNumberParserFactory.get_parser(
                ParserType.ORDINAL, ChineseNumberParserConfiguration()),
            ChineseOrdinalExtractor()
        ))
        self.register_model('PercentModel', Culture.Chinese, lambda options: PercentModel(
            AgnosticNumberParserFactory.get_parser(
                ParserType.PERCENTAGE, ChineseNumberParserConfiguration()),
            ChinesePercentageExtractor()
        ))
        # endregion

        # region Japanese
        self.register_model('NumberModel', Culture.Japanese, lambda options: NumberModel(
            AgnosticNumberParserFactory.get_parser(
                ParserType.NUMBER, JapaneseNumberParserConfiguration()),
            JapaneseNumberExtractor()
        ))
        self.register_model('OrdinalModel', Culture.Japanese, lambda options: OrdinalModel(
            AgnosticNumberParserFactory.get_parser(
                ParserType.ORDINAL, JapaneseNumberParserConfiguration()),
            JapaneseOrdinalExtractor()
        ))
        self.register_model('PercentModel', Culture.Japanese, lambda options: PercentModel(
            AgnosticNumberParserFactory.get_parser(
                ParserType.PERCENTAGE, JapaneseNumberParserConfiguration()),
            JapanesePercentageExtractor()
        ))
        # endregion

        # region Spanish
        self.register_model('NumberModel', Culture.Spanish, lambda options: NumberModel(
            AgnosticNumberParserFactory.get_parser(
                ParserType.NUMBER, SpanishNumberParserConfiguration()),
            SpanishNumberExtractor(NumberMode.PURE_NUMBER)
        ))
        self.register_model('OrdinalModel', Culture.Spanish, lambda options: OrdinalModel(
            AgnosticNumberParserFactory.get_parser(
                ParserType.ORDINAL, SpanishNumberParserConfiguration()),
            SpanishOrdinalExtractor()
        ))
        self.register_model('PercentModel', Culture.Spanish, lambda options: PercentModel(
            AgnosticNumberParserFactory.get_parser(
                ParserType.PERCENTAGE, SpanishNumberParserConfiguration()),
            SpanishPercentageExtractor()
        ))
        # endregion

        # region Portuguese
        self.register_model('NumberModel', Culture.Portuguese, lambda options: NumberModel(
            AgnosticNumberParserFactory.get_parser(
                ParserType.NUMBER, PortugueseNumberParserConfiguration()),
            PortugueseNumberExtractor(NumberMode.PURE_NUMBER)
        ))
        self.register_model('OrdinalModel', Culture.Portuguese, lambda options: OrdinalModel(
            AgnosticNumberParserFactory.get_parser(
                ParserType.ORDINAL, PortugueseNumberParserConfiguration()),
            PortugueseOrdinalExtractor()
        ))
        self.register_model('PercentModel', Culture.Portuguese, lambda options: PercentModel(
            AgnosticNumberParserFactory.get_parser(
                ParserType.PERCENTAGE, PortugueseNumberParserConfiguration()),
            PortuguesePercentageExtractor()
        ))
        # endregion

        # region French
        self.register_model('NumberModel', Culture.French, lambda options: NumberModel(
            AgnosticNumberParserFactory.get_parser(
                ParserType.NUMBER, FrenchNumberParserConfiguration()),
            FrenchNumberExtractor(NumberMode.PURE_NUMBER)
        ))
        self.register_model('OrdinalModel', Culture.French, lambda options: OrdinalModel(
            AgnosticNumberParserFactory.get_parser(
                ParserType.ORDINAL, FrenchNumberParserConfiguration()),
            FrenchOrdinalExtractor()
        ))
        self.register_model('PercentModel', Culture.French, lambda options: PercentModel(
            AgnosticNumberParserFactory.get_parser(
                ParserType.PERCENTAGE, FrenchNumberParserConfiguration()),
            FrenchPercentageExtractor()
        ))
        # endregion

    def get_number_model(self, culture: str = None, fallback_to_default_culture: bool = True) -> Model:
        return self.get_model('NumberModel', culture, fallback_to_default_culture)

    def get_ordinal_model(self, culture: str = None, fallback_to_default_culture: bool = True) -> Model:
        return self.get_model('OrdinalModel', culture, fallback_to_default_culture)

    def get_percentage_model(self, culture: str = None, fallback_to_default_culture: bool = True) -> Model:
        return self.get_model('PercentModel', culture, fallback_to_default_culture)


def recognize_number(query: str, culture: str, options: NumberOptions = NumberOptions.NONE, fallback_to_default_culture: bool = True) -> List[ModelResult]:
    recognizer = NumberRecognizer(culture, options)
    model = recognizer.get_number_model(culture, fallback_to_default_culture)
    return model.parse(query)


def recognize_ordinal(query: str, culture: str, options: NumberOptions = NumberOptions.NONE, fallback_to_default_culture: bool = True) -> List[ModelResult]:
    recognizer = NumberRecognizer(culture, options)
    model = recognizer.get_ordinal_model(culture, fallback_to_default_culture)
    return model.parse(query)


def recognize_percentage(query: str, culture: str, options: NumberOptions = NumberOptions.NONE, fallback_to_default_culture: bool = True) -> List[ModelResult]:
    recognizer = NumberRecognizer(culture, options)
    model = recognizer.get_percentage_model(
        culture, fallback_to_default_culture)
    return model.parse(query)
