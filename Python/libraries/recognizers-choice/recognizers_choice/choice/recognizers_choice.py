from enum import IntFlag
from typing import List

from recognizers_choice.choice.extractors import BooleanExtractor
from recognizers_choice.choice.english import EnglishBooleanExtractorConfiguration
from recognizers_choice.choice.models import BooleanModel
from recognizers_choice.choice.parsers import BooleanParser
from recognizers_text import Culture, Recognizer, ModelResult, Model


class ChoiceOptions(IntFlag):
    NONE = 0


def recognize_boolean(query: str,
                      culture: str,
                      options: ChoiceOptions = ChoiceOptions.NONE,
                      fallback_to_default_culture: bool = True) -> List[ModelResult]:
    recognizer = ChoiceRecognizer(culture, options)
    model = recognizer.get_boolean_model(culture, fallback_to_default_culture)
    return model.parse(query)


class ChoiceRecognizer (Recognizer[ChoiceOptions]):

    def __init__(self, target_culture: str = None, options: ChoiceOptions = ChoiceOptions.NONE, lazy_initialization: bool = False):
        if options < ChoiceOptions.NONE or options > ChoiceOptions.NONE:
            raise ValueError()
        super().__init__(target_culture, options, lazy_initialization)

    def initialize_configuration(self):
        self.register_model('BooleanModel', Culture.English, lambda options: BooleanModel(
            BooleanParser(), BooleanExtractor(EnglishBooleanExtractorConfiguration())))

    @staticmethod
    def is_valid_option(options: int) -> bool:
        return options >= 0 & options <= ChoiceOptions.NONE

    def get_boolean_model(self, culture: str = None, fallback_to_default_culture: bool = True) -> Model:
        return self.get_model('BooleanModel', culture, fallback_to_default_culture)
