#  Copyright (c) Microsoft Corporation. All rights reserved.
#  Licensed under the MIT License.

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
from .chinese.merged_extractor import ChineseMergedExtractor
from .chinese.merged_parser import ChineseMergedParser
from .french.common_configs import FrenchCommonDateTimeParserConfiguration
from .french.merged_extractor_config import FrenchMergedExtractorConfiguration
from .french.merged_parser_config import FrenchMergedParserConfiguration
from .portuguese.common_configs import PortugueseCommonDateTimeParserConfiguration
from .portuguese.merged_extractor_config import PortugueseMergedExtractorConfiguration
from .portuguese.merged_parser_config import PortugueseMergedParserConfiguration
from .italian.common_configs import ItalianCommonDateTimeParserConfiguration
from .italian.merged_extractor_config import ItalianMergedExtractorConfiguration
from .italian.merged_parser_config import ItalianMergedParserConfiguration
from .german.common_configs import GermanCommonDateTimeParserConfiguration
from .german.merged_extractor_config import GermanMergedExtractorConfiguration
from .german.merged_parser_config import GermanMergedParserConfiguration
from .dutch.common_configs import DutchCommonDateTimeParserConfiguration
from .dutch.merged_extractor_config import DutchMergedExtractorConfiguration
from .dutch.merged_parser_config import DutchMergedParserConfiguration


class DateTimeRecognizer(Recognizer[DateTimeOptions]):
    def __init__(self, target_culture: str = None,
                 options: DateTimeOptions = DateTimeOptions.NONE,
                 lazy_initialization: bool = True):
        if options < DateTimeOptions.NONE or options > DateTimeOptions.CALENDAR:
            raise ValueError()
        super().__init__(target_culture, options, lazy_initialization)

    def initialize_configuration(self):
        self.register_model('DateTimeModel', Culture.English, lambda options: DateTimeModel(
            BaseMergedParser(EnglishMergedParserConfiguration(
                EnglishCommonDateTimeParserConfiguration()), options),
            BaseMergedExtractor(EnglishMergedExtractorConfiguration(), options)
        ))

        self.register_model('DateTimeModel', Culture.EnglishOthers, lambda options: DateTimeModel(
            BaseMergedParser(EnglishMergedParserConfiguration(
                EnglishCommonDateTimeParserConfiguration(dmyDateFormat=True)), options),
            BaseMergedExtractor(EnglishMergedExtractorConfiguration(dmyDateFormat=True), options)
        ))

        self.register_model('DateTimeModel', Culture.Chinese, lambda options: DateTimeModel(
            ChineseMergedParser(),
            ChineseMergedExtractor(options)
        ))

        self.register_model('DateTimeModel', Culture.Spanish, lambda options: DateTimeModel(
            BaseMergedParser(SpanishMergedParserConfiguration(
                SpanishCommonDateTimeParserConfiguration()), options),
            BaseMergedExtractor(SpanishMergedExtractorConfiguration(), options)
        ))

        self.register_model('DateTimeModel', Culture.SpanishMexican, lambda options: DateTimeModel(
            BaseMergedParser(SpanishMergedParserConfiguration(
                SpanishCommonDateTimeParserConfiguration()), options),
            BaseMergedExtractor(SpanishMergedExtractorConfiguration(), options)
        ))

        self.register_model('DateTimeModel', Culture.French, lambda options: DateTimeModel(
            BaseMergedParser(FrenchMergedParserConfiguration(
                FrenchCommonDateTimeParserConfiguration()), options),
            BaseMergedExtractor(FrenchMergedExtractorConfiguration(), options)
        ))

        self.register_model('DateTimeModel', Culture.Portuguese, lambda options: DateTimeModel(
            BaseMergedParser(PortugueseMergedParserConfiguration(
                PortugueseCommonDateTimeParserConfiguration()), options),
            BaseMergedExtractor(PortugueseMergedExtractorConfiguration(), options)
        ))

        self.register_model('DateTimeModel', Culture.Italian, lambda options: DateTimeModel(
            BaseMergedParser(ItalianMergedParserConfiguration(
                ItalianCommonDateTimeParserConfiguration()), options),
            BaseMergedExtractor(ItalianMergedExtractorConfiguration(), options)
        ))

        self.register_model('DateTimeModel', Culture.German, lambda options: DateTimeModel(
            BaseMergedParser(GermanMergedParserConfiguration(
                GermanCommonDateTimeParserConfiguration()), options),
            BaseMergedExtractor(GermanMergedExtractorConfiguration(), options)
        ))

        self.register_model('DateTimeModel', Culture.Dutch, lambda options: DateTimeModel(
            BaseMergedParser(DutchMergedParserConfiguration(
                DutchCommonDateTimeParserConfiguration()), options),
            BaseMergedExtractor(DutchMergedExtractorConfiguration(), options)
        ))

    def get_datetime_model(self, culture: str = None, fallback_to_default_culture: bool = True) -> Model:
        return self.get_model('DateTimeModel', culture, fallback_to_default_culture)


def recognize_datetime(query: str, culture: str, options: DateTimeOptions = DateTimeOptions.NONE,
                       reference: datetime = None, fallback_to_default_culture: bool = True) -> List[ModelResult]:
    recognizer = DateTimeRecognizer(culture, options)
    model = recognizer.get_datetime_model(culture, fallback_to_default_culture)
    return model.parse(query, reference)
