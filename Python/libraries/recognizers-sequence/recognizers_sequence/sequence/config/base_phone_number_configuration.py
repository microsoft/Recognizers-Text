from recognizers_sequence.sequence.config import *
from recognizers_sequence.resources import *
from recognizers_text.culture import Culture
from recognizers_number import CultureInfo


class BasePhoneNumberExtractorConfiguration(PhoneNumberConfiguration):

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
    def false_positive_prefix_regex(self) -> str:
        return None

    @property
    def forbidden_prefix_markers(self) -> str:
        return self._ForbiddenPrefixMarkers

    @property
    def forbidden_suffix_markers(self) -> str:
        return None

    def __init__(self, culture_info: CultureInfo = None):
        if culture_info is None:
            culture_info = CultureInfo(Culture.English)
        super().__init__(culture_info)
        self._WordBoundariesRegex = BasePhoneNumbers.WordBoundariesRegex
        self._NonWordBoundariesRegex = BasePhoneNumbers.NonWordBoundariesRegex
        self._EndWordBoundariesRegex = BasePhoneNumbers.EndWordBoundariesRegex
        self._ColonPrefixCheckRegex = BasePhoneNumbers.ColonPrefixCheckRegex
        self._ForbiddenPrefixMarkers = BasePhoneNumbers.ForbiddenPrefixMarkers
