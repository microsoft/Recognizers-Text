from datetime import datetime
from typing import List
from recognizers_text import Culture, Recognizer
from recognizers_text.model import Model, ModelResult
from .utilities import DateTimeOptions
from .models import DateTimeModel
from .base_merged import BaseMergedExtractor, BaseMergedParser
from .english.common_configs import EnglishCommonDateTimeParserConfiguration
from .english.merged_extractor_config import EnglishMergedExtractorConfiguration
from .english.merged_parser_config import EnglishMergedParserConfiguration
from .spanish.common_configs import SpanishCommonDateTimeParserConfiguration
from .spanish.merged_extractor_config import SpanishMergedExtractorConfiguration
from .spanish.merged_parser_config import SpanishMergedParserConfiguration

class DateTimeRecognizer(Recognizer[DateTimeOptions]):
    def __init__(self, target_culture: str = None,
                 options: DateTimeOptions = DateTimeOptions.NONE,
                 lazy_initialization: bool = True):
        if options < DateTimeOptions.NONE or options > DateTimeOptions.CALENDAR:
            raise ValueError()
        super().__init__(target_culture, options, lazy_initialization)

    def initialize_configuration(self):
        self.register_model('DateTimeModel', Culture.English, lambda options: DateTimeModel(
            BaseMergedParser(EnglishMergedParserConfiguration(EnglishCommonDateTimeParserConfiguration()), options),
            BaseMergedExtractor(EnglishMergedExtractorConfiguration(), options)
        ))

        self.register_model('DateTimeModel', Culture.Chinese, lambda options: DateTimeModel(
            None,
            None
        ))

        self.register_model('DateTimeModel', Culture.Spanish, lambda options: DateTimeModel(
             BaseMergedParser(SpanishMergedParserConfiguration(SpanishCommonDateTimeParserConfiguration()), options),
             BaseMergedExtractor(SpanishMergedExtractorConfiguration(), options)
        ))

        self.register_model('DateTimeModel', Culture.French, lambda options: DateTimeModel(
            None,
            None
        ))

        self.register_model('DateTimeModel', Culture.Portuguese, lambda options: DateTimeModel(
            None,
            None
        ))

    def get_datetime_model(self, culture: str = None, fallback_to_default_culture: bool = True) -> Model:
        return self.get_model('DateTimeModel', culture, fallback_to_default_culture)

def recognize_datetime(query: str, culture: str, options: DateTimeOptions = DateTimeOptions.NONE, reference: datetime = None, fallback_to_default_culture: bool = True) -> List[ModelResult]:
    recognizer = DateTimeRecognizer(culture, options)
    model = recognizer.get_datetime_model(culture, fallback_to_default_culture)
    return model.parse(query, reference)
