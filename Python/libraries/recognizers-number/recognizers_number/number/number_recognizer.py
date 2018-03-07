from recognizers_text import Recognizer

class NumberRecognizer(Recognizer):
    def __init__(self, culture: str = "", fallbackToDefaultCulture: bool = True):
        pass

    def get_number_model(self):
        raise NotImplementedError