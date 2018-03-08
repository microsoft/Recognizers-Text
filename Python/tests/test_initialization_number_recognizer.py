import pytest
from recognizers_text import Culture
from recognizers_number.number import NumberModel
from recognizers_number.number import NumberRecognizer
from recognizers_number.number.english.extractors import EnglishIntegerExtractor
from recognizers_number.number import NumberOptions
from recognizers_number.number.parsers import BaseNumberParser

class TestInitializationNumberRecognizer():
    control_model = NumberModel(None, None)
    english_culture = Culture.English
    spanish_culture = Culture.Spanish
    invalid_culture = "vo-id"

    def assert_models_equal(self, expected, actual):
        assert actual.model_type_name == expected.model_type_name
        assert type(actual.extractor) is type(expected.extractor)
        assert type(actual.parser) is type(expected.parser)
        
    def assert_models_distinct(self, expected, actual):
        assert actual.model_type_name == expected.model_type_name
        assert type(actual.extractor) is not type(expected.extractor)
        assert type(actual.parser) is not type(expected.parser)

    def test_without_culture_use_target_culture(self):
        extractor = EnglishIntegerExtractor()
        pepe=extractor.extract('number one and two')
        recognizer = NumberRecognizer(self.english_culture)
        self.assert_models_equal(self.control_model, recognizer.get_number_model())

    def test_withOtherCulture_not_use_target_culture(self):
        recognizer = NumberRecognizer(self.english_culture)
        self.assert_models_distinct(self.control_model, recognizer.get_number_model(self.spanish_culture))

    def test_with_invalid_culture_use_target_culture(self):
        recognizer = NumberRecognizer(self.spanish_culture)
        self.assert_models_equal(self.control_model, recognizer.get_number_model(self.english_culture))

    def test_with_invalid_culture_and_without_fallback_throw_error(self):
        recognizer = NumberRecognizer()
        with pytest.raises(ValueError):
            recognizer.get_number_model(self.invalid_culture, False)
    
    def test_with_invalid_culture_as_target_and_without_fallback_throw_error(self):
        recognizer = NumberRecognizer(self.invalid_culture)
        with pytest.raises(ValueError):
            recognizer.get_number_model(None, False)
    
    def test_without_target_culture_and_without_culture_fallback_to_english_culture(self):
        recognizer = NumberRecognizer()
        self.assert_models_equal(self.control_model, recognizer.get_number_model())
    
    def test_initialization_with_int_option_resolve_options_enum(self):
        recognizer = NumberRecognizer(self.english_culture, NumberOptions.NONE, False)
        assert (recognizer.options & NumberOptions.NONE) == NumberOptions.NONE
    
    def test_initialization_with_invalid_options_throw_error(self):
        with pytest.raises(ValueError):
            NumberRecognizer(self.invalid_culture, -1)
