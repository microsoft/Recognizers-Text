from recognizers_sequence.sequence.config import *
from recognizers_sequence.sequence.sequence_recognizer import *
from recognizers_sequence.resources.portuguese_phone_numbers import PortuguesePhoneNumbers
from recognizers_sequence.sequence.extractors import *
from recognizers_text.culture import Culture


class PortuguesePhoneNumberExtractorConfiguration(PhoneNumberConfiguration):

    @property
    def word_boundaries_regex(self) -> str:
        return self.__word_boundaries_regex

    @word_boundaries_regex.setter
    def word_boundaries_regex(self, word_boundaries_regex):
        self.__word_boundaries_regex = word_boundaries_regex

    @property
    def non_word_boundaries_regex(self) -> str:
        return self.__non_word_boundaries_regex

    @non_word_boundaries_regex.setter
    def non_word_boundaries_regex(self, non_word_boundaries_regex):
        self.__non_word_boundaries_regex = non_word_boundaries_regex

    @property
    def end_word_boundaries_regex(self) -> str:
        return self.__end_word_boundaries_regex

    @end_word_boundaries_regex.setter
    def end_word_boundaries_regex(self, end_word_boundaries_regex):
        self.__end_word_boundaries_regex = end_word_boundaries_regex

    @property
    def colon_prefix_check_regex(self) -> str:
        return self.__colon_prefix_check_regex

    @colon_prefix_check_regex.setter
    def colon_prefix_check_regex(self, colon_prefix_check_regex):
        self.__colon_prefix_check_regex = colon_prefix_check_regex

    @property
    def forbidden_prefix_markers(self) -> str:
        return self.__forbidden_prefix_markers

    @forbidden_prefix_markers.setter
    def forbidden_prefix_markers(self, forbidden_prefix_markers):
        self.__forbidden_prefix_markers = forbidden_prefix_markers

    @property
    def false_positive_prefix_regex(self) -> str:
        return self.__false_positive_prefix_regex

    @false_positive_prefix_regex.setter
    def false_positive_prefix_regex(self, false_positive_prefix_regex):
        self.__false_positive_prefix_regex = false_positive_prefix_regex

    def __init__(self, options=None):
        super().__init__(options)
        self.__word_boundaries_regex = BasePhoneNumbers.WordBoundariesRegex
        self.__non_word_boundaries_regex = BasePhoneNumbers.NonWordBoundariesRegex
        self.__end_word_boundaries_regex = BasePhoneNumbers.EndWordBoundariesRegex
        self.__colon_prefix_check_regex = BasePhoneNumbers.ColonPrefixCheckRegex
        self.__forbidden_prefix_markers = BasePhoneNumbers.ForbiddenPrefixMarkers
        self.__false_positive_prefix_regex = PortuguesePhoneNumbers.FalsePositivePrefixRegex
