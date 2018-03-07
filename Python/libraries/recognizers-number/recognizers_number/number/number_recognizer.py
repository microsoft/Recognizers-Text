from enum import Flag
from recognizers_text import Recognizer

class NumberOptions(Flag):
    NONE = 0

class NumberRecognizer(Recognizer[NumberOptions]):
    def __init__(self, target_culture: str = "", options: NumberOptions = 0, lazy_initialization: bool = True):
        super(NumberRecognizer, self).__init__(target_culture, options, lazy_initialization)

    def initialize_configuration(self):
        pass

    def get_number_model(self, culture: str="", fallback_to_default_culture: bool = True):
        raise NotImplementedError