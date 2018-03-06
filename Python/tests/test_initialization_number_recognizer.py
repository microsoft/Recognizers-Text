from recognizers_text import Culture
from recognizers_number.number import NumberModel
from recognizers_number.number import NumberRecognizer

class TestInitializationNumberRecognizer():
    control_model = NumberModel()
    english_culture = Culture.English
    spanish_culture = Culture.Spanish
    invalid_culture = "vo-id"

    def test_without_culture_use_target_culture(self):
       recognizer = NumberRecognizer(self.english_culture)
       
       assert recognizer.get_number_model() == self.control_model

    def test_withOtherCulture_not_use_target_culture(self):
        recognizer = NumberRecognizer(self.english_culture)

        assert recognizer.get_number_model(self.spanish_culture) != self.control_model

    def test_with_invalid_culture_use_target_culture(self):
        recognizer = NumberRecognizer(self.spanish_culture)
        assert recognizer.get_number_model(self.english_culture) == self.control_model

    def test_with_invalid_culture_and_without_fallback_throw_error(self):
        recognizer = NumberRecognizer()
        #t.throws(() => { recognizer.get_number_model(self.invalid_culture, false) })
    
    def test_with_invalid_culture_as_target_and_without_fallback_throw_error(self):
        recognizer = NumberRecognizer(self.invalid_culture)
        #t.throws(() => { recognizer.get_number_model(null, false) })
    
    def test_without_target_culture_and_without_culture_fallback_to_english_culture(self):
        recognizer = NumberRecognizer()
        assert recognizer.get_number_model() == self.control_model
    
    def test_initialization_with_int_option_resolve_options_enum(self):
        recognizer = NumberRecognizer(self.english_culture, 0)
        #assert (recognizer.Options & NumberOptions.None) === NumberOptions.None)
    
    def test_initialization_with_invalid_options_throw_error(self):
        pass
        #t.throws(() => { (NumberRecognizer(self.invalid_culture, -1)})
    