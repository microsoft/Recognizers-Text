#  Copyright (c) Microsoft Corporation. All rights reserved.
#  Licensed under the MIT License.

from recognizers_text import Extractor
from recognizers_number import JapaneseIntegerExtractor

from ..parsers import DateTimeParser
from ..base_timeperiod import TimePeriodParserConfiguration
from .time_parser import JapaneseTimeParser


class JapaneseTimePeriodParserConfiguration(TimePeriodParserConfiguration):
    @property
    def time_extractor(self) -> any:
        return None

    @property
    def time_parser(self) -> DateTimeParser:
        return self._time_parser

    @property
    def integer_extractor(self) -> Extractor:
        return self._integer_extractor

    @property
    def pure_number_from_to_regex(self) -> any:
        return None

    @property
    def pure_number_between_and_regex(self) -> any:
        return None

    @property
    def time_of_day_regex(self) -> any:
        return None

    @property
    def till_regex(self) -> any:
        return None

    @property
    def numbers(self) -> any:
        return None

    @property
    def utility_configuration(self) -> any:
        return None

    def __init__(self):
        self._time_parser = JapaneseTimeParser()
        self._integer_extractor = JapaneseIntegerExtractor()

    def get_matched_timex_range(self, source: str):
        return None
