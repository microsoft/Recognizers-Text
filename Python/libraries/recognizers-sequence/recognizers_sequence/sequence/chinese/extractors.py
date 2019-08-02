from recognizers_sequence.sequence.config import PhoneNumberConfiguration, URLConfiguration
from recognizers_sequence.sequence.sequence_recognizer import *
from recognizers_sequence.resources.chinese_phone_numbers import ChinesePhoneNumbers
from recognizers_sequence.resources.chinese_url import ChineseURL
from recognizers_sequence.sequence.extractors import *
from recognizers_text.culture import Culture


class ChinesePhoneNumberExtractorConfiguration(PhoneNumberConfiguration):

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

    def __init__(self, options=None):
        super().__init__(options)
        self.__word_boundaries_regex = ChinesePhoneNumbers.WordBoundariesRegex
        self.__non_word_boundaries_regex = ChinesePhoneNumbers.NonWordBoundariesRegex
        self.__end_word_boundaries_regex = ChinesePhoneNumbers.EndWordBoundariesRegex


class ChineseURLExtractorConfiguration(URLConfiguration):
    @property
    def ip_url_regex(self) -> Pattern:
        return self.__ip_url_regex

    @ip_url_regex.setter
    def ip_url_regex(self, ip_url_regex):
        self.__ip_url_regex = ip_url_regex

    @property
    def url_regex(self) -> Pattern:
        return self.__url_regex

    @url_regex.setter
    def url_regex(self, url_regex):
        self.__url_regex = url_regex

    def __init__(self, options):
        self.__url_regex = RegExpUtility.get_safe_reg_exp(ChineseURL.UrlRegex)
        self.__ip_url_regex = RegExpUtility.get_safe_reg_exp(ChineseURL.IpUrlRegex)

        super().__init__(options)
