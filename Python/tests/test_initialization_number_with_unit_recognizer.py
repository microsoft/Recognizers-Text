import pytest
from recognizers_text import Culture
from recognizers_number_with_unit.number_with_unit import NumberWithUnitRecognizer, NumberWithUnitOptions
from recognizers_number_with_unit.number_with_unit.models import DimensionModel, AbstractNumberWithUnitModel, ExtractorParserModel
from recognizers_number_with_unit.number_with_unit.extractors import NumberWithUnitExtractor
from recognizers_number_with_unit.number_with_unit.english.extractors import EnglishDimensionExtractorConfiguration
from recognizers_number_with_unit.number_with_unit.parsers import NumberWithUnitParser
from recognizers_number_with_unit.number_with_unit.english.parsers import EnglishDimensionParserConfiguration

class TestInitializationNumberWithUnitRecognizer():
    control_model = DimensionModel(
        [ ExtractorParserModel(NumberWithUnitExtractor(EnglishDimensionExtractorConfiguration()), NumberWithUnitParser(EnglishDimensionParserConfiguration())) ]
        )
    english_culture = Culture.English
    spanish_culture = Culture.Spanish
    invalid_culture = "vo-id"

    def assert_models_equal(self, expected:AbstractNumberWithUnitModel, actual:AbstractNumberWithUnitModel):
        assert actual.model_type_name == expected.model_type_name
        assert len(actual.extractor_parser) == len(expected.extractor_parser)

        # deep comparison
        for actual_item, expected_item in zip(actual.extractor_parser, expected.extractor_parser):
            assert type(actual_item.parser) is type(expected_item.parser)

            # configs
            assert type(actual_item.extractor.config) is type(expected_item.extractor.config)
            assert type(actual_item.parser.config) is type(expected_item.parser.config)
        
    def assert_models_distinct(self, expected, actual):
        assert actual.model_type_name == expected.model_type_name
        assert len(actual.extractor_parser) == len(expected.extractor_parser)

        # deep comparison
        any_config_is_different = False
        for actual_item, expected_item in zip(actual.extractor_parser, expected.extractor_parser):
            assert type(actual_item.parser) is type(expected_item.parser)

            # configs
            any_config_is_different = any_config_is_different or type(actual_item.extractor.config) is not type(expected_item.extractor.config)
            any_config_is_different = any_config_is_different or type(actual_item.parser.config) is not type(expected_item.parser.config)

        assert any_config_is_different

    def test_without_culture_use_target_culture(self):
        recognizer = NumberWithUnitRecognizer(self.english_culture)
        self.assert_models_equal(self.control_model, recognizer.get_dimension_model())

    def test_withOtherCulture_not_use_target_culture(self):
        recognizer = NumberWithUnitRecognizer(self.english_culture)
        self.assert_models_distinct(self.control_model, recognizer.get_dimension_model(self.spanish_culture))

    def test_with_invalid_culture_use_target_culture(self):
        recognizer = NumberWithUnitRecognizer(self.spanish_culture)
        self.assert_models_equal(self.control_model, recognizer.get_dimension_model(self.invalid_culture))

    def test_with_invalid_culture_and_without_fallback_throw_error(self):
        recognizer = NumberWithUnitRecognizer()
        with pytest.raises(ValueError):
            recognizer.get_dimension_model(self.invalid_culture, False)
    
    def test_with_invalid_culture_as_target_and_without_fallback_throw_error(self):
        recognizer = NumberWithUnitRecognizer(self.invalid_culture)
        with pytest.raises(ValueError):
            recognizer.get_dimension_model(None, False)
    
    def test_without_target_culture_and_without_culture_fallback_to_english_culture(self):
        recognizer = NumberWithUnitRecognizer()
        self.assert_models_equal(self.control_model, recognizer.get_dimension_model())
    
    def test_initialization_with_int_option_resolve_options_enum(self):
        recognizer = NumberWithUnitRecognizer(self.english_culture, NumberWithUnitOptions.NONE, False)
        assert (recognizer.options & NumberWithUnitOptions.NONE) == NumberWithUnitOptions.NONE
    
    def test_initialization_with_invalid_options_throw_error(self):
        with pytest.raises(ValueError):
            NumberWithUnitRecognizer(self.invalid_culture, -1)

if __name__ == '__main__':
    tests = TestInitializationNumberWithUnitRecognizer()
    tests.test_withOtherCulture_not_use_target_culture()
