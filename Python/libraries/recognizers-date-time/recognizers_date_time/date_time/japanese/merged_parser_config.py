from typing import Pattern

from .common_configs import JapaneseCommonDateTimeParserConfiguration
from recognizers_date_time.date_time.CJK.base_merged import CJKMergedParserConfiguration
from recognizers_date_time.date_time.japanese import JapaneseMergedExtractorConfiguration


class JapaneseMergedParserConfiguration(JapaneseCommonDateTimeParserConfiguration, CJKMergedParserConfiguration):

    @property
    def before_regex(self) -> Pattern:
        return self._before_regex

    @property
    def after_regex(self) -> Pattern:
        return self._after_regex

    @property
    def since_prefix_regex(self) -> Pattern:
        return self._since_prefix_regex

    @property
    def since_suffix_regex(self) -> Pattern:
        return self._since_suffix_regex

    @property
    def around_prefix_regex(self) -> Pattern:
        return self._around_prefix_regex

    @property
    def around_suffix_regex(self) -> Pattern:
        return self._around_suffix_regex

    @property
    def equal_regex(self) -> Pattern:
        return self._equal_regex

    @property
    def until_regex(self) -> Pattern:
        return self._until_regex

    def __init__(self, config):
        JapaneseCommonDateTimeParserConfiguration.__init__(self)
        self._before_regex = JapaneseMergedExtractorConfiguration().before_regex
        self._after_regex = JapaneseMergedExtractorConfiguration().after_regex
        self._since_prefix_regex = JapaneseMergedExtractorConfiguration().since_prefix_regex
        self._since_suffix_regex = JapaneseMergedExtractorConfiguration().since_suffix_regex
        self._around_prefix_regex = JapaneseMergedExtractorConfiguration().around_prefix_regex
        self._around_suffix_regex = JapaneseMergedExtractorConfiguration().around_suffix_regex
        self._equal_regex = JapaneseMergedExtractorConfiguration().equal_regex
        self._until_regex = JapaneseMergedExtractorConfiguration().until_regex
