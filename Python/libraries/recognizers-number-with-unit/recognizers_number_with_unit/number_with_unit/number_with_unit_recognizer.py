from enum import IntFlag
from typing import List
from recognizers_text import Culture, Recognizer
from recognizers_text.model import Model, ModelResult
from .models import CurrencyModel, TemperatureModel, DimensionModel, AgeModel, ExtractorParserModel
from .extractors import NumberWithUnitExtractor
from .english.extractors import (EnglishCurrencyExtractorConfiguration,
                                EnglishTemperatureExtractorConfiguration,
                                EnglishDimensionExtractorConfiguration,
                                EnglishAgeExtractorConfiguration)
from .spanish.extractors import SpanishCurrencyExtractorConfiguration
from .parsers import NumberWithUnitParser
from .english.parsers import (EnglishCurrencyParserConfiguration,
                             EnglishTemperatureParserConfiguration,
                             EnglishDimensionParserConfiguration,
                             EnglishAgeParserConfiguration)
from .spanish.parsers import SpanishCurrencyParserConfiguration

class NumberWithUnitOptions(IntFlag):
    NONE = 0

class NumberWithUnitRecognizer(Recognizer[NumberWithUnitOptions]):
    def __init__(self, target_culture: str = None,
                 options: NumberWithUnitOptions = NumberWithUnitOptions.NONE,
                 lazy_initialization: bool = True):
        if options < NumberWithUnitOptions.NONE or options > NumberWithUnitOptions.NONE:
            raise ValueError()
        super().__init__(target_culture, options, lazy_initialization)

    def initialize_configuration(self):
        #region English
        self.register_model('CurrencyModel', Culture.English, lambda options: CurrencyModel(
            dict([('EnglishCurrencyExtractorConfiguration',
                   ExtractorParserModel(NumberWithUnitExtractor(EnglishCurrencyExtractorConfiguration()),
                                       NumberWithUnitParser(EnglishCurrencyParserConfiguration()))
                   )])))
        self.register_model('TemperatureModel', Culture.English, lambda options: TemperatureModel(
            dict([('EnglishTemperatureExtractorConfiguration',
                   ExtractorParserModel(NumberWithUnitExtractor(EnglishTemperatureExtractorConfiguration()),
                                       NumberWithUnitParser(EnglishTemperatureParserConfiguration()))
                   )])))
        self.register_model('DimensionModel', Culture.English, lambda options: DimensionModel(
            dict([('EnglishDimensionExtractorConfiguration',
                   ExtractorParserModel(NumberWithUnitExtractor(EnglishDimensionExtractorConfiguration()),
                                       NumberWithUnitParser(EnglishDimensionParserConfiguration()))
                   )])))
        self.register_model('AgeModel', Culture.English, lambda options: AgeModel(
            dict([('EnglishAgeExtractorConfiguration',
                   ExtractorParserModel(NumberWithUnitExtractor(EnglishAgeExtractorConfiguration()),
                                       NumberWithUnitParser(EnglishAgeParserConfiguration()))
                   )])))
        #endregion

        #region English
        self.register_model('CurrencyModel', Culture.Spanish, lambda options: CurrencyModel(
            dict([('SpanishCurrencyExtractorConfiguration',
                   ExtractorParserModel(NumberWithUnitExtractor(SpanishCurrencyExtractorConfiguration()),
                                       NumberWithUnitParser(SpanishCurrencyParserConfiguration()))
                   )])))
        #endregion

    def get_age_model(self, culture: str = None, fallback_to_default_culture: bool = True) -> Model:
        return self.get_model('AgeModel', culture, fallback_to_default_culture)

    def get_currency_model(self, culture: str = None, fallback_to_default_culture: bool = True) -> Model:
        return self.get_model('CurrencyModel', culture, fallback_to_default_culture)

    def get_dimension_model(self, culture: str = None, fallback_to_default_culture: bool = True) -> Model:
        return self.get_model('DimensionModel', culture, fallback_to_default_culture)

    def get_temperature_model(self, culture: str = None, fallback_to_default_culture: bool = True) -> Model:
        return self.get_model('TemperatureModel', culture, fallback_to_default_culture)

    @staticmethod
    def recognize_age(query: str, culture: str, options: NumberWithUnitOptions = NumberWithUnitOptions.NONE, fallback_to_default_culture: bool = True) -> List[ModelResult]:
        recognizer = NumberWithUnitRecognizer(culture, options)
        model = recognizer.get_age_model(culture, fallback_to_default_culture)
        return model.parse(query)

    @staticmethod
    def recognize_currency(query: str, culture: str, options: NumberWithUnitOptions = NumberWithUnitOptions.NONE, fallback_to_default_culture: bool = True) -> List[ModelResult]:
        recognizer = NumberWithUnitRecognizer(culture, options)
        model = recognizer.get_currency_model(culture, fallback_to_default_culture)
        return model.parse(query)

    @staticmethod
    def recognize_dimension(query: str, culture: str, options: NumberWithUnitOptions = NumberWithUnitOptions.NONE, fallback_to_default_culture: bool = True) -> List[ModelResult]:
        recognizer = NumberWithUnitRecognizer(culture, options)
        model = recognizer.get_dimension_model(culture, fallback_to_default_culture)
        return model.parse(query)

    @staticmethod
    def recognize_temperature(query: str, culture: str, options: NumberWithUnitOptions = NumberWithUnitOptions.NONE, fallback_to_default_culture: bool = True) -> List[ModelResult]:
        recognizer = NumberWithUnitRecognizer(culture, options)
        model = recognizer.get_temperature_model(culture, fallback_to_default_culture)
        return model.parse(query)
