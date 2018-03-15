from enum import IntFlag
from typing import List
from recognizers_text import Culture, Recognizer
from recognizers_text.model import Model, ModelResult

class NumberWithUnitOptions(IntFlag):
    NONE = 0

class NumberWithUnitRecognizer(Recognizer[NumberWithUnitOptions]):
    def __init__(self, target_culture: str = None, options: NumberWithUnitOptions = NumberWithUnitOptions.NONE, lazy_initialization: bool = True):
        if options < NumberWithUnitOptions.NONE or options > NumberWithUnitOptions.NONE:
            raise ValueError()
        super().__init__(target_culture, options, lazy_initialization)

    def initialize_configuration(self):
        #TODO initialization
        pass

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
