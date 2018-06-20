from enum import IntFlag
from typing import List
from recognizers_text import Culture, Recognizer
from recognizers_text.model import Model, ModelResult
from .models import CurrencyModel, TemperatureModel, DimensionModel, AgeModel, ExtractorParserModel
from .extractors import NumberWithUnitExtractor, BaseMergedUnitExtractor
from .parsers import NumberWithUnitParser, BaseMergedUnitParser
from .english.extractors import (EnglishCurrencyExtractorConfiguration,
                                 EnglishTemperatureExtractorConfiguration,
                                 EnglishDimensionExtractorConfiguration,
                                 EnglishAgeExtractorConfiguration)
from .english.parsers import (EnglishCurrencyParserConfiguration,
                              EnglishTemperatureParserConfiguration,
                              EnglishDimensionParserConfiguration,
                              EnglishAgeParserConfiguration)
from .chinese.extractors import (ChineseCurrencyExtractorConfiguration,
                                 ChineseTemperatureExtractorConfiguration,
                                 ChineseDimensionExtractorConfiguration,
                                 ChineseAgeExtractorConfiguration)
from .chinese.parsers import (ChineseCurrencyParserConfiguration,
                              ChineseTemperatureParserConfiguration,
                              ChineseDimensionParserConfiguration,
                              ChineseAgeParserConfiguration)
from .spanish.extractors import (SpanishCurrencyExtractorConfiguration,
                                 SpanishTemperatureExtractorConfiguration,
                                 SpanishDimensionExtractorConfiguration,
                                 SpanishAgeExtractorConfiguration)
from .spanish.parsers import (SpanishCurrencyParserConfiguration,
                              SpanishTemperatureParserConfiguration,
                              SpanishDimensionParserConfiguration,
                              SpanishAgeParserConfiguration)
from .french.extractors import (FrenchCurrencyExtractorConfiguration,
                                FrenchTemperatureExtractorConfiguration,
                                FrenchDimensionExtractorConfiguration,
                                FrenchAgeExtractorConfiguration)
from .french.parsers import (FrenchCurrencyParserConfiguration,
                             FrenchTemperatureParserConfiguration,
                             FrenchDimensionParserConfiguration,
                             FrenchAgeParserConfiguration)
from .portuguese.extractors import (PortugueseCurrencyExtractorConfiguration,
                                    PortugueseTemperatureExtractorConfiguration,
                                    PortugueseDimensionExtractorConfiguration,
                                    PortugueseAgeExtractorConfiguration)
from .portuguese.parsers import (PortugueseCurrencyParserConfiguration,
                                 PortugueseTemperatureParserConfiguration,
                                 PortugueseDimensionParserConfiguration,
                                 PortugueseAgeParserConfiguration)

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
            [ExtractorParserModel(BaseMergedUnitExtractor(EnglishCurrencyExtractorConfiguration()), BaseMergedUnitParser(EnglishCurrencyParserConfiguration()))]
            ))
        self.register_model('TemperatureModel', Culture.English, lambda options: TemperatureModel(
            [ExtractorParserModel(NumberWithUnitExtractor(EnglishTemperatureExtractorConfiguration()), NumberWithUnitParser(EnglishTemperatureParserConfiguration()))]
            ))
        self.register_model('DimensionModel', Culture.English, lambda options: DimensionModel(
            [ExtractorParserModel(NumberWithUnitExtractor(EnglishDimensionExtractorConfiguration()), NumberWithUnitParser(EnglishDimensionParserConfiguration()))]
            ))
        self.register_model('AgeModel', Culture.English, lambda options: AgeModel(
            [ExtractorParserModel(NumberWithUnitExtractor(EnglishAgeExtractorConfiguration()), NumberWithUnitParser(EnglishAgeParserConfiguration()))]
            ))
        #endregion

        #region Chinese
        self.register_model('CurrencyModel', Culture.Chinese, lambda options: CurrencyModel([
            ExtractorParserModel(
                BaseMergedUnitExtractor(ChineseCurrencyExtractorConfiguration()),
                BaseMergedUnitParser(ChineseCurrencyParserConfiguration())),
            ExtractorParserModel(
                NumberWithUnitExtractor(EnglishCurrencyExtractorConfiguration()),
                NumberWithUnitParser(EnglishCurrencyParserConfiguration()))
            ]))
        self.register_model('TemperatureModel', Culture.Chinese, lambda options: TemperatureModel([
            ExtractorParserModel(
                NumberWithUnitExtractor(ChineseTemperatureExtractorConfiguration()),
                NumberWithUnitParser(ChineseTemperatureParserConfiguration())),
            ExtractorParserModel(
                NumberWithUnitExtractor(EnglishTemperatureExtractorConfiguration()),
                NumberWithUnitParser(EnglishTemperatureParserConfiguration()))
            ]))
        self.register_model('DimensionModel', Culture.Chinese, lambda options: DimensionModel([
            ExtractorParserModel(
                NumberWithUnitExtractor(ChineseDimensionExtractorConfiguration()),
                NumberWithUnitParser(ChineseDimensionParserConfiguration())),
            ExtractorParserModel(
                NumberWithUnitExtractor(EnglishDimensionExtractorConfiguration()),
                NumberWithUnitParser(EnglishDimensionParserConfiguration()))
            ]))
        self.register_model('AgeModel', Culture.Chinese, lambda options: AgeModel([
            ExtractorParserModel(
                NumberWithUnitExtractor(ChineseAgeExtractorConfiguration()),
                NumberWithUnitParser(ChineseAgeParserConfiguration())),
            ExtractorParserModel(
                NumberWithUnitExtractor(EnglishAgeExtractorConfiguration()),
                NumberWithUnitParser(EnglishAgeParserConfiguration()))
            ]))
        #endregion

        #region French
        self.register_model('CurrencyModel', Culture.French, lambda options: CurrencyModel([
            ExtractorParserModel(
                NumberWithUnitExtractor(FrenchCurrencyExtractorConfiguration()),
                NumberWithUnitParser(FrenchCurrencyParserConfiguration()))
            ]))
        self.register_model('TemperatureModel', Culture.French, lambda options: TemperatureModel([
            ExtractorParserModel(
                NumberWithUnitExtractor(FrenchTemperatureExtractorConfiguration()),
                NumberWithUnitParser(FrenchTemperatureParserConfiguration()))
            ]))
        self.register_model('DimensionModel', Culture.French, lambda options: DimensionModel([
            ExtractorParserModel(
                NumberWithUnitExtractor(FrenchDimensionExtractorConfiguration()),
                NumberWithUnitParser(FrenchDimensionParserConfiguration()))
            ]))
        self.register_model('AgeModel', Culture.French, lambda options: AgeModel([
            ExtractorParserModel(
                NumberWithUnitExtractor(FrenchAgeExtractorConfiguration()),
                NumberWithUnitParser(FrenchAgeParserConfiguration()))
            ]))
        #endregion

        #region Portuguese
        self.register_model('CurrencyModel', Culture.Portuguese, lambda options: CurrencyModel([
            ExtractorParserModel(
                NumberWithUnitExtractor(PortugueseCurrencyExtractorConfiguration()),
                NumberWithUnitParser(PortugueseCurrencyParserConfiguration()))
            ]))
        self.register_model('TemperatureModel', Culture.Portuguese, lambda options: TemperatureModel([
            ExtractorParserModel(
                NumberWithUnitExtractor(PortugueseTemperatureExtractorConfiguration()),
                NumberWithUnitParser(PortugueseTemperatureParserConfiguration()))
            ]))
        self.register_model('DimensionModel', Culture.Portuguese, lambda options: DimensionModel([
            ExtractorParserModel(
                NumberWithUnitExtractor(PortugueseDimensionExtractorConfiguration()),
                NumberWithUnitParser(PortugueseDimensionParserConfiguration()))
            ]))
        self.register_model('AgeModel', Culture.Portuguese, lambda options: AgeModel([
            ExtractorParserModel(
                NumberWithUnitExtractor(PortugueseAgeExtractorConfiguration()),
                NumberWithUnitParser(PortugueseAgeParserConfiguration()))
            ]))
        #endregion

        #region Spanish
        self.register_model('CurrencyModel', Culture.Spanish, lambda options: CurrencyModel(
            [ExtractorParserModel(NumberWithUnitExtractor(SpanishCurrencyExtractorConfiguration()), NumberWithUnitParser(SpanishCurrencyParserConfiguration()))]
            ))
        self.register_model('TemperatureModel', Culture.Spanish, lambda options: TemperatureModel(
            [ExtractorParserModel(NumberWithUnitExtractor(SpanishTemperatureExtractorConfiguration()), NumberWithUnitParser(SpanishTemperatureParserConfiguration()))]
            ))
        self.register_model('DimensionModel', Culture.Spanish, lambda options: DimensionModel(
            [ExtractorParserModel(NumberWithUnitExtractor(SpanishDimensionExtractorConfiguration()), NumberWithUnitParser(SpanishDimensionParserConfiguration()))]
            ))
        self.register_model('AgeModel', Culture.Spanish, lambda options: AgeModel(
            [ExtractorParserModel(NumberWithUnitExtractor(SpanishAgeExtractorConfiguration()), NumberWithUnitParser(SpanishAgeParserConfiguration()))]
            ))
        #endregion

    def get_age_model(self, culture: str = None, fallback_to_default_culture: bool = True) -> Model:
        return self.get_model('AgeModel', culture, fallback_to_default_culture)

    def get_currency_model(self, culture: str = None, fallback_to_default_culture: bool = True) -> Model:
        return self.get_model('CurrencyModel', culture, fallback_to_default_culture)

    def get_dimension_model(self, culture: str = None, fallback_to_default_culture: bool = True) -> Model:
        return self.get_model('DimensionModel', culture, fallback_to_default_culture)

    def get_temperature_model(self, culture: str = None, fallback_to_default_culture: bool = True) -> Model:
        return self.get_model('TemperatureModel', culture, fallback_to_default_culture)

def recognize_age(query: str, culture: str, options: NumberWithUnitOptions = NumberWithUnitOptions.NONE, fallback_to_default_culture: bool = True) -> List[ModelResult]:
    recognizer = NumberWithUnitRecognizer(culture, options)
    model = recognizer.get_age_model(culture, fallback_to_default_culture)
    return model.parse(query)

def recognize_currency(query: str, culture: str, options: NumberWithUnitOptions = NumberWithUnitOptions.NONE, fallback_to_default_culture: bool = True) -> List[ModelResult]:
    recognizer = NumberWithUnitRecognizer(culture, options)
    model = recognizer.get_currency_model(culture, fallback_to_default_culture)
    return model.parse(query)

def recognize_dimension(query: str, culture: str, options: NumberWithUnitOptions = NumberWithUnitOptions.NONE, fallback_to_default_culture: bool = True) -> List[ModelResult]:
    recognizer = NumberWithUnitRecognizer(culture, options)
    model = recognizer.get_dimension_model(culture, fallback_to_default_culture)
    return model.parse(query)

def recognize_temperature(query: str, culture: str, options: NumberWithUnitOptions = NumberWithUnitOptions.NONE, fallback_to_default_culture: bool = True) -> List[ModelResult]:
    recognizer = NumberWithUnitRecognizer(culture, options)
    model = recognizer.get_temperature_model(culture, fallback_to_default_culture)
    return model.parse(query)
