import pytest
from recognizers_text import Culture
from recognizers_number_with_unit.number_with_unit import NumberWithUnitRecognizer, NumberWithUnitOptions
from recognizers_number_with_unit.number_with_unit.models import CurrencyModel, AbstractNumberWithUnitModel
from recognizers_number_with_unit.number_with_unit.extractors import NumberWithUnitExtractor
from recognizers_number_with_unit.number_with_unit.english.extractors import EnglishCurrencyExtractorConfiguration
from recognizers_number_with_unit.number_with_unit.parsers import NumberWithUnitParser
from recognizers_number_with_unit.number_with_unit.english.parsers import EnglishCurrencyParserConfiguration

class TestInitializationNumberRecognizer():
    control_model = CurrencyModel(dict([(
        NumberWithUnitExtractor(EnglishCurrencyExtractorConfiguration()),
        NumberWithUnitParser(EnglishCurrencyParserConfiguration())
    )]))
    english_culture = Culture.English
    spanish_culture = Culture.Spanish
    invalid_culture = "vo-id"

    def assert_models_equal(self, expected:AbstractNumberWithUnitModel, actual:AbstractNumberWithUnitModel):
        assert actual.model_type_name == expected.model_type_name
        assert len(actual.extractor_parser_dict) == len(expected.extractor_parser_dict)

        # deep comparison
        for actual_extractor in actual.extractor_parser_dict:
            expected_extractor = [x for x in expected.extractor_parser_dict.keys()
                                                           if type(actual_extractor) == type(x)]
            assert expected_extractor is not None
            actual_parser = actual.extractor_parser_dict[actual_extractor]
            expected_parser = expected.extractor_parser_dict[expected_extractor]
            assert type(actual_parser) is type(expected_parser)

            # configs
            assert type(actual_extractor.config) is (expected_extractor.config)
            assert type(actual_parser.config) is (expected_parser.config)
        
    def assert_models_distinct(self, expected, actual):
        assert actual.model_type_name == expected.model_type_name
        assert type(actual.extractor) is not type(expected.extractor)
        assert type(actual.parser.config) is not type(expected.parser.config)

    def test_without_culture_use_target_culture(self):
        recognizer = NumberWithUnitRecognizer(self.english_culture)
        self.assert_models_equal(self.control_model, recognizer.get_currency_model())

    def test_withOtherCulture_not_use_target_culture(self):
        recognizer = NumberWithUnitRecognizer(self.english_culture)
        self.assert_models_distinct(self.control_model, recognizer.get_currency_model(self.spanish_culture))

    def test_with_invalid_culture_use_target_culture(self):
        recognizer = NumberWithUnitRecognizer(self.spanish_culture)
        self.assert_models_equal(self.control_model, recognizer.get_currency_model(self.invalid_culture))

    def test_with_invalid_culture_and_without_fallback_throw_error(self):
        recognizer = NumberWithUnitRecognizer()
        with pytest.raises(ValueError):
            recognizer.get_currency_model(self.invalid_culture, False)
    
    def test_with_invalid_culture_as_target_and_without_fallback_throw_error(self):
        recognizer = NumberWithUnitRecognizer(self.invalid_culture)
        with pytest.raises(ValueError):
            recognizer.get_currency_model(None, False)
    
    def test_without_target_culture_and_without_culture_fallback_to_english_culture(self):
        recognizer = NumberWithUnitRecognizer()
        self.assert_models_equal(self.control_model, recognizer.get_currency_model())
    
    def test_initialization_with_int_option_resolve_options_enum(self):
        recognizer = NumberWithUnitRecognizer(self.english_culture, NumberOptions.NONE, False)
        assert (recognizer.options & NumberOptions.NONE) == NumberOptions.NONE
    
    def test_initialization_with_invalid_options_throw_error(self):
        with pytest.raises(ValueError):
            NumberWithUnitRecognizer(self.invalid_culture, -1)

if __name__ == '__main__':
    tests = TestInitializationNumberRecognizer()
    tests.test_without_culture_use_target_culture()