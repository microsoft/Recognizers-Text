from recognizers_sequence.sequence.config import *
from recognizers_sequence.sequence.config.base_phone_number_configuration import *
from recognizers_sequence.sequence.sequence_recognizer import *
from recognizers_sequence.resources.portuguese_phone_numbers import PortuguesePhoneNumbers
from recognizers_sequence.sequence.extractors import *
from recognizers_text.culture import Culture


class PortuguesePhoneNumberExtractorConfiguration(BasePhoneNumberExtractorConfiguration):

    @property
    def false_positive_prefix_regex(self) -> str:
        return self._false_positive_prefix_regex

    def __init__(self, culture_info: CultureInfo = None):
        super().__init__(culture_info)
        self._false_positive_prefix_regex = PortuguesePhoneNumbers.FalsePositivePrefixRegex
