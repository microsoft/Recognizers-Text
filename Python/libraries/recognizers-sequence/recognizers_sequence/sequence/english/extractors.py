from recognizers_number import CultureInfo
from recognizers_sequence import *
from recognizers_sequence.sequence.extractors import *
from recognizers_sequence.resources import *
from recognizers_text.culture import Culture
from recognizers_sequence.sequence.config import *
import regex as re


class EnglishPhoneNumberExtractorConfiguration(PhoneNumberConfiguration):

    @property
    def word_boundaries_regex(self) -> str:
        return self._WordBoundariesRegex

    @property
    def non_word_boundaries_regex(self) -> str:
        return self._NonWordBoundariesRegex

    @property
    def end_word_boundaries_regex(self) -> str:
        return self._EndWordBoundariesRegex

    @property
    def colon_prefix_check_regex(self) -> str:
        return self._ColonPrefixCheckRegex

    @property
    def colon_markers(self) -> str:
        return self._ColonMarkers

    @property
    def boundary_markers(self) -> str:
        return self._BoundaryMarkers

    @property
    def forbidden_prefix_markers(self) -> str:
        return self._ForbiddenPrefixMarkers

    @property
    def boundary_end_markers(self) -> str:
        return self._ForbiddenSuffixMarkers

    def __init__(self, culture_info: CultureInfo = None):
        if culture_info is None:
            culture_info = CultureInfo(Culture.English)
        super().__init__(culture_info)
        self._WordBoundariesRegex = BasePhoneNumbers.WordBoundariesRegex
        self._NonWordBoundariesRegex = BasePhoneNumbers.NonWordBoundariesRegex
        self._EndWordBoundariesRegex = BasePhoneNumbers.EndWordBoundariesRegex
        self._ColonPrefixCheckRegex = BasePhoneNumbers.ColonPrefixCheckRegex
        self._ForbiddenPrefixMarkers = BasePhoneNumbers.ForbiddenPrefixMarkers


class EnglishIpExtractorConfiguration(IpConfiguration):
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
        self.__ipv4_regex = RegExpUtility.get_safe_reg_exp(BaseIp.Ipv4Regex)
        self.__ipv6_regex = RegExpUtility.get_safe_reg_exp(BaseIp.Ipv6Regex)

        super().__init__(options)


class EnglishMentionExtractor(BaseMentionExtractor):
    pass


class EnglishEmailExtractor(BaseEmailExtractor):
    pass


class EnglishURLExtractorConfiguration(URLConfiguration):
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
        self.__ip_url_regex = RegExpUtility.get_safe_reg_exp(BaseURL.IpUrlRegex)
        self.__url_regex = RegExpUtility.get_safe_reg_exp(BaseURL.UrlRegex)

        super().__init__(options)


class EnglishGUIDExtractor(BaseGUIDExtractor):
    pass


class EnglishHashtagExtractor(BaseHashTagExtractor):
    pass
