import pytest
from recognizers_text import Culture
from recognizers_choice.choice.models import BooleanModel
from recognizers_choice.choice.parsers import BooleanParser
from recognizers_choice.choice.extractors import BooleanExtractor
from recognizers_choice.choice.recognizers_choice import ChoiceRecognizer, ChoiceOptions
from recognizers_choice.choice.english.boolean import EnglishBooleanExtractorConfiguration


class TestInitializationChoiceRecognizer():
    control_model: BooleanModel = BooleanModel(
        BooleanParser(), BooleanExtractor(
            EnglishBooleanExtractorConfiguration()))
    english_culture = Culture.English
    spanish_culture = Culture.Spanish
    invalid_culture = "vo-id"

    @staticmethod
    def assert_model_equal(expected, actual):
        assert actual.model_type_name == expected.model_type_name
        assert isinstance(actual.extractor, type(expected.extractor))
        assert isinstance(actual.parser, type(expected.parser))

    @staticmethod
    def assert_models_distinct(expected, actual):
        assert actual.model_type_name == expected.model_type_name
        assert not isinstance(actual.extractor, type(expected.extractor))
        assert not isinstance(actual.parser, type(expected.parser))

    def test_without_culture_use_target_culture(self):
        recognizer = ChoiceRecognizer(self.english_culture)
        self.assert_model_equal(
            self.control_model,
            recognizer.get_boolean_model())

    # This test doesn't apply. Kept as documentation of purpose. Not marked as 'Ignore' to avoid permanent warning
    # due to design.
    def test_with_other_culture_not_use_target_culture(self):
        pass

    def test_with_invalid_culture_use_target_culture(self):
        recognizer = ChoiceRecognizer(self.spanish_culture)
        self.assert_model_equal(
            self.control_model,
            recognizer.get_boolean_model(
                self.invalid_culture))

    def test_with_invalid_culture_and_without_fallback_throw_error(self):
        recognizer = ChoiceRecognizer()
        with pytest.raises(ValueError):
            recognizer.get_boolean_model(self.invalid_culture, False)

    def test_with_invalid_culture_as_target_and_without_fallback_throw_error(
            self):
        recognizer = ChoiceRecognizer(self.invalid_culture)
        with pytest.raises(ValueError):
            recognizer.get_boolean_model(None, False)

    def test_without_target_culture_and_without_culture_fallback_to_english_culture(
            self):
        recognizer = ChoiceRecognizer()
        self.assert_model_equal(
            self.control_model,
            recognizer.get_boolean_model())

    def test_initialization_with_int_option_resolve_options_enum(self):
        recognizer = ChoiceRecognizer(
            self.english_culture, ChoiceOptions.NONE, False)
        assert (recognizer.options & ChoiceOptions.NONE) == ChoiceOptions.NONE

    def test_initialization_with_invalid_options_throw_error(self):
        with pytest.raises(ValueError):
            ChoiceRecognizer(self.invalid_culture, -1)
