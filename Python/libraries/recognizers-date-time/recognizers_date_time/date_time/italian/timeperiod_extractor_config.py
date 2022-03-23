#  Copyright (c) Microsoft Corporation. All rights reserved.
#  Licensed under the MIT License.

from typing import List, Pattern

from recognizers_text.utilities import RegExpUtility
from recognizers_text.extractor import Extractor
from recognizers_number.number.italian.extractors import ItalianIntegerExtractor
from ...resources.italian_date_time import ItalianDateTime
from ..extractors import DateTimeExtractor
from ..base_timeperiod import TimePeriodExtractorConfiguration, MatchedIndex
from ..base_time import BaseTimeExtractor
from ..base_timezone import BaseTimeZoneExtractor
from .time_extractor_config import ItalianTimeExtractorConfiguration
from .base_configs import ItalianDateTimeUtilityConfiguration
from .timezone_extractor_config import ItalianTimeZoneExtractorConfiguration


class ItalianTimePeriodExtractorConfiguration(TimePeriodExtractorConfiguration):
    @property
    def check_both_before_after(self) -> bool:
        return self._check_both_before_after

    @property
    def simple_cases_regex(self) -> List[Pattern]:
        return self._simple_cases_regex

    @property
    def till_regex(self) -> Pattern:
        return self._till_regex

    @property
    def time_of_day_regex(self) -> Pattern:
        return self._time_of_day_regex

    @property
    def general_ending_regex(self) -> Pattern:
        return self._general_ending_regex

    @property
    def single_time_extractor(self) -> DateTimeExtractor:
        return self._single_time_extractor

    @property
    def integer_extractor(self) -> Extractor:
        return self._integer_extractor

    @property
    def token_before_date(self) -> str:
        return self._token_before_date

    @property
    def pure_number_regex(self) -> List[Pattern]:
        return self._pure_number_regex

    @property
    def time_zone_extractor(self) -> DateTimeExtractor:
        return self._time_zone_extractor

    def __init__(self):
        super().__init__()
        self._check_both_before_after = ItalianDateTime.CheckBothBeforeAfter
        self._single_time_extractor = BaseTimeExtractor(
            ItalianTimeExtractorConfiguration())
        self._integer_extractor = ItalianIntegerExtractor()
        self.utility_configuration = ItalianDateTimeUtilityConfiguration()

        self._simple_cases_regex: List[Pattern] = [
            RegExpUtility.get_safe_reg_exp(ItalianDateTime.PureNumFromTo),
            RegExpUtility.get_safe_reg_exp(ItalianDateTime.PureNumBetweenAnd),
            RegExpUtility.get_safe_reg_exp(ItalianDateTime.PmRegex),
            RegExpUtility.get_safe_reg_exp(ItalianDateTime.AmRegex)
        ]

        self._till_regex: Pattern = RegExpUtility.get_safe_reg_exp(
            ItalianDateTime.TillRegex)
        self._time_of_day_regex: Pattern = RegExpUtility.get_safe_reg_exp(
            ItalianDateTime.TimeOfDayRegex)
        self._general_ending_regex: Pattern = RegExpUtility.get_safe_reg_exp(
            ItalianDateTime.GeneralEndingRegex)

        self.from_regex = RegExpUtility.get_safe_reg_exp(
            ItalianDateTime.FromRegex2)
        self.connector_and_regex = RegExpUtility.get_safe_reg_exp(
            ItalianDateTime.ConnectorAndRegex)
        self.before_regex = RegExpUtility.get_safe_reg_exp(
            ItalianDateTime.BeforeRegex2)
        self._token_before_date = ItalianDateTime.TokenBeforeDate
        self._pure_number_regex = [ItalianDateTime.PureNumFromTo, ItalianDateTime.PureNumFromTo]
        self._time_zone_extractor = BaseTimeZoneExtractor(
            ItalianTimeZoneExtractorConfiguration())

    def get_from_token_index(self, source: str) -> MatchedIndex:
        match = self.from_regex.search(source)
        if match:
            return MatchedIndex(True, match.start())

        return MatchedIndex(False, -1)

    def get_between_token_index(self, source: str) -> MatchedIndex:
        match = self.before_regex.search(source)
        if match:
            return MatchedIndex(True, match.start())

        return MatchedIndex(False, -1)

    def is_connector_token(self, source: str):
        return self.connector_and_regex.match(source)
