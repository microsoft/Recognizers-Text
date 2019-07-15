from enum import IntFlag

from recognizers_sequence.sequence.chinese.extractors import ChinesePhoneNumberExtractorConfiguration
from recognizers_text import Culture, Recognizer

from .english.extractors import EmailExtractor, BasePhoneNumberExtractor, EnglishPhoneNumberExtractorConfiguration
from .english.parsers import PhoneNumberParser, EmailParser
from .models import *
from .parsers import *


class SequenceOptions(IntFlag):
    NONE = 0


class SequenceRecognizer(Recognizer[SequenceOptions]):
    def __init__(self, target_culture: str = None, options: SequenceOptions = SequenceOptions.NONE,
                 lazy_initialization: bool = True):
        if options < SequenceOptions.NONE or options > SequenceOptions.NONE:
            raise ValueError()
        super().__init__(target_culture, options, lazy_initialization)

    def initialize_configuration(self):
        # region English
        self.register_model('PhoneNumberModel', Culture.English,
                            lambda options: PhoneNumberModel(PhoneNumberParser(),
                                                             BasePhoneNumberExtractor(EnglishPhoneNumberExtractorConfiguration())))
        self.register_model('EmailModel', Culture.English,
                            lambda options: EmailModel(EmailParser(), EmailExtractor()))
        # endregion

        # region Chinese
        self.register_model('PhoneNumberModel', Culture.Chinese,
                            lambda options: PhoneNumberModel(PhoneNumberParser(),
                                                             BasePhoneNumberExtractor(ChinesePhoneNumberExtractorConfiguration())))
        # endregion

    def get_phone_number_model(self, culture: str = None, fallback_to_default_culture: bool = True) -> Model:
        if culture and (culture.lower().startswith("zh-") or culture.lower().startswith("ja-")):
            return self.get_model('PhoneNumberModel', Culture.Chinese, fallback_to_default_culture)
        return self.get_model('PhoneNumberModel', culture, fallback_to_default_culture)

    def get_email_model(self, culture: str = None, fallback_to_default_culture: bool = True) -> Model:
        return self.get_model('EmailModel', culture, fallback_to_default_culture)


def recognize_phone_number(query: str, culture: str, options: SequenceOptions = SequenceOptions.NONE,
                           fallback_to_default_culture: bool = True) -> List[ModelResult]:
    recognizer = SequenceRecognizer(culture, options)
    model = recognizer.get_phone_number_model(
        culture, fallback_to_default_culture)
    return model.parse(query)


def recognize_email(query: str, culture: str, options: SequenceOptions = SequenceOptions.NONE,
                    fallback_to_default_culture: bool = True) -> List[ModelResult]:
    recognizer = SequenceRecognizer(culture, options)
    model = recognizer.get_email_model(culture, fallback_to_default_culture)
    return model.parse(query)
