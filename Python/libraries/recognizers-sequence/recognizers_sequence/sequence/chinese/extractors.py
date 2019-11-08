from recognizers_sequence.sequence.config import *
from recognizers_sequence.sequence.sequence_recognizer import *
from recognizers_sequence.resources.chinese_phone_numbers import ChinesePhoneNumbers
from recognizers_sequence.resources.chinese_url import ChineseURL
from recognizers_sequence.resources.chinese_ip import ChineseIp
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
        self.__word_boundaries_regex = ChinesePhoneNumbers.WordBoundariesRegex
        self.__non_word_boundaries_regex = ChinesePhoneNumbers.NonWordBoundariesRegex
        self.__end_word_boundaries_regex = ChinesePhoneNumbers.EndWordBoundariesRegex
        self.__colon_prefix_check_regex = ChinesePhoneNumbers.ColonPrefixCheckRegex
        self.__forbidden_prefix_markers = ChinesePhoneNumbers.ForbiddenPrefixMarkers
        self.__false_positive_prefix_regex = None


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


class ChineseIpExtractorConfiguration(IpConfiguration):
    @property
    def ipv4_regex(self) -> Pattern:
        return self.__ipv4_regex

    @ipv4_regex.setter
    def ipv4_regex(self, ipv4_regex):
        self.__ipv4_regex = ipv4_regex

    @property
    def ipv6_regex(self) -> Pattern:
        return self.__ipv6_regex

    @ipv6_regex.setter
    def ipv6_regex(self, ipv6_regex):
        self.__ipv6_regex = ipv6_regex

    def __init__(self, options):
        self.__ipv4_regex = RegExpUtility.get_safe_reg_exp(ChineseIp.Ipv4Regex)
        self.__ipv6_regex = RegExpUtility.get_safe_reg_exp(ChineseIp.Ipv6Regex)

        super().__init__(options)
