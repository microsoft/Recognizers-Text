from typing import List, Pattern

from recognizers_text.utilities import RegExpUtility
from recognizers_text.extractor import Extractor
from recognizers_number.number.french.extractors import FrenchIntegerExtractor
from ...resources.french_date_time import FrenchDateTime
from ..extractors import DateTimeExtractor
from ..base_timeperiod import TimePeriodExtractorConfiguration, MatchedIndex
from ..base_time import BaseTimeExtractor
from ..base_timezone import BaseTimeZoneExtractor
from .time_extractor_config import FrenchTimeExtractorConfiguration
from .base_configs import FrenchDateTimeUtilityConfiguration
from .timezone_extractor_config import FrenchTimeZoneExtractorConfiguration


class FrenchTimePeriodExtractorConfiguration(TimePeriodExtractorConfiguration):
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
    def timezone_extractor(self) -> DateTimeExtractor:
        return self._timezone_extractor

    def __init__(self):
        super().__init__()
        self._check_both_before_after = FrenchDateTime.CheckBothBeforeAfter
        self._single_time_extractor = BaseTimeExtractor(
            FrenchTimeExtractorConfiguration())
        self._integer_extractor = FrenchIntegerExtractor()
        self.utility_configuration = FrenchDateTimeUtilityConfiguration()

        self._simple_cases_regex: List[Pattern] = [
            RegExpUtility.get_safe_reg_exp(FrenchDateTime.PureNumFromTo),
            RegExpUtility.get_safe_reg_exp(FrenchDateTime.PureNumBetweenAnd),
            RegExpUtility.get_safe_reg_exp(FrenchDateTime.PmRegex),
            RegExpUtility.get_safe_reg_exp(FrenchDateTime.AmRegex)
        ]

        self._till_regex: Pattern = RegExpUtility.get_safe_reg_exp(
            FrenchDateTime.TillRegex)
        self._time_of_day_regex: Pattern = RegExpUtility.get_safe_reg_exp(
            FrenchDateTime.TimeOfDayRegex)
        self._general_ending_regex: Pattern = RegExpUtility.get_safe_reg_exp(
            FrenchDateTime.GeneralEndingRegex)

        self.from_regex = RegExpUtility.get_safe_reg_exp(
            FrenchDateTime.FromRegex2)
        self.connector_and_regex = RegExpUtility.get_safe_reg_exp(
            FrenchDateTime.ConnectorAndRegex)
        self.before_regex = RegExpUtility.get_safe_reg_exp(
            FrenchDateTime.BeforeRegex2)
        self._token_before_date = FrenchDateTime.TokenBeforeDate
        self._pure_number_regex = [FrenchDateTime.PureNumFromTo, FrenchDateTime.PureNumFromTo]
        self._timezone_extractor = BaseTimeZoneExtractor(
            FrenchTimeZoneExtractorConfiguration())

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
