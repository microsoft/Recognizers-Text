from enum import IntFlag

from recognizers_text import Culture, Recognizer

class NumberWithUnitOptions(IntFlag):
    NONE = 0

class NumberWithUnitRecognizer(Recognizer[NumberWithUnitOptions]):
    pass
