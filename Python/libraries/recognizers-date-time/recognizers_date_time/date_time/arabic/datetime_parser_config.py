from typing import Pattern, Dict

from recognizers_text.utilities import RegExpUtility
from recognizers_number.number.extractors import BaseNumberExtractor
from recognizers_number.number.parsers import BaseNumberParser
from recognizers_date_time.resources.arabic_date_time import ArabicDateTime
from recognizers_date_time.date_time.extractors import DateTimeExtractor
from recognizers_date_time.date_time.parsers import DateTimeParser
from recognizers_date_time.date_time.utilities import DateTimeUtilityConfiguration
from recognizers_date_time.date_time.base_configs import BaseDateParserConfiguration
from recognizers_date_time.date_time.base_datetime import DateTimeParserConfiguration, MatchedTimex
from recognizers_date_time.date_time.constants import Constants


class ArabicDateTimeParserConfiguration(DateTimeParserConfiguration):
    @property
    def token_before_date(self) -> str:
        return self._token_before_date

    @property
    def token_before_time(self) -> str:
        return self._token_before_time

    @property
    def date_extractor(self) -> DateTimeExtractor:
        return self._date_extractor

    @property
    def time_extractor(self) -> DateTimeExtractor:
        return self._time_extractor

    @property
    def date_parser(self) -> DateTimeParser:
        return self._date_parser

    @property
    def time_parser(self) -> DateTimeParser:
        return self._time_parser

    @property
    def cardinal_extractor(self) -> BaseNumberExtractor:
        return self._cardinal_extractor

    @property
    def number_parser(self) -> BaseNumberParser:
        return self._number_parser

    @property
    def duration_extractor(self) -> DateTimeExtractor:
        return self._duration_extractor

    @property
    def duration_parser(self) -> DateTimeParser:
        return self._duration_parser

    @property
    def now_regex(self) -> Pattern:
        return self._now_regex

    @property
    def am_time_regex(self) -> Pattern:
        return self._am_time_regex

    @property
    def pm_time_regex(self) -> Pattern:
        return self._pm_time_regex

    @property
    def simple_time_of_today_after_regex(self) -> Pattern:
        return self._simple_time_of_today_after_regex

    @property
    def simple_time_of_today_before_regex(self) -> Pattern:
        return self._simple_time_of_today_before_regex

    @property
    def specific_time_of_day_regex(self) -> Pattern:
        return self._specific_time_of_day_regex

    @property
    def specific_end_of_regex(self) -> Pattern:
        return self._specific_end_of_regex

    @property
    def unspecific_end_of_regex(self) -> Pattern:
        return self._unspecific_end_of_regex

    @property
    def unit_regex(self) -> Pattern:
        return self._unit_regex

    @property
    def unit_map(self) -> Dict[str, str]:
        return self._unit_map

    @property
    def numbers(self) -> Dict[str, int]:
        return self._numbers

    @property
    def night_time_regex(self) -> Pattern:
        return self._night_time_regex

    @property
    def now_time_regex(self) -> Pattern:
        return self._now_time_regex

    @property
    def recently_time_regex(self) -> Pattern:
        return self._recently_time_regex

    @property
    def asap_time_regex(self) -> Pattern:
        return self._asap_time_regex

    @property
    def next_prefix_regex(self) -> Pattern:
        return self._next_prefix_regex

    @property
    def previous_prefix_regex(self) -> Pattern:
        return self._previous_prefix_regex

    @property
    def utility_configuration(self) -> DateTimeUtilityConfiguration:
        return self._utility_configuration

    def __init__(self, config: BaseDateParserConfiguration):
        self._token_before_date = ArabicDateTime.TokenBeforeDate
        self._token_before_time = ArabicDateTime.TokenBeforeTime
        self._date_extractor = config.date_extractor
        self._time_extractor = config.time_extractor
        self._date_parser = config.date_parser
        self._time_parser = config.time_parser
        self._now_regex = RegExpUtility.get_safe_reg_exp(
            ArabicDateTime.NowRegex)
        self._am_time_regex = RegExpUtility.get_safe_reg_exp(
            ArabicDateTime.AMTimeRegex)
        self._pm_time_regex = RegExpUtility.get_safe_reg_exp(
            ArabicDateTime.PMTimeRegex)
        self._simple_time_of_today_after_regex = RegExpUtility.get_safe_reg_exp(
            ArabicDateTime.SimpleTimeOfTodayAfterRegex)
        self._simple_time_of_today_before_regex = RegExpUtility.get_safe_reg_exp(
            ArabicDateTime.SimpleTimeOfTodayBeforeRegex)
        self._specific_time_of_day_regex = RegExpUtility.get_safe_reg_exp(
            ArabicDateTime.SpecificTimeOfDayRegex)
        self._specific_end_of_regex = RegExpUtility.get_safe_reg_exp(
            ArabicDateTime.SpecificEndOfRegex)
        self._unspecific_end_of_regex = RegExpUtility.get_safe_reg_exp(
            ArabicDateTime.UnspecificEndOfRegex)
        self._unit_regex = RegExpUtility.get_safe_reg_exp(
            ArabicDateTime.TimeUnitRegex)
        self._numbers = config.numbers
        self._cardinal_extractor = config.cardinal_extractor
        self._number_parser = config.number_parser
        self._duration_extractor = config.duration_extractor
        self._duration_parser = config.duration_parser
        self._unit_map = config.unit_map
        self._utility_configuration = config.utility_configuration
        self._night_time_regex = RegExpUtility.get_safe_reg_exp(ArabicDateTime.NightTimeRegex)
        self._now_time_regex = RegExpUtility.get_safe_reg_exp(ArabicDateTime.NowTimeRegex)
        self._recently_time_regex = RegExpUtility.get_safe_reg_exp(ArabicDateTime.RecentlyTimeRegex)
        self._asap_time_regex = RegExpUtility.get_safe_reg_exp(ArabicDateTime.AsapTimeRegex)
        self._next_prefix_regex = RegExpUtility.get_safe_reg_exp(ArabicDateTime.NextPrefixRegex)
        self._previous_prefix_regex = RegExpUtility.get_safe_reg_exp(ArabicDateTime.PreviousPrefixRegex)

    def get_hour(self, source: str, hour: int) -> int:
        source = source.strip().lower()

        if RegExpUtility.match_end(self.am_time_regex, source, False) and hour >= Constants.HALF_DAY_HOUR_COUNT:
            return hour - 12
        elif not RegExpUtility.match_end(self.am_time_regex, source, False) and hour < Constants.HALF_DAY_HOUR_COUNT and \
                not (RegExpUtility.match_end(self.pm_time_regex, source, False) and hour < Constants.QUARTER_DAY_HOUR_COUNT):
            return hour + 12

        return hour

    def get_matched_now_timex(self, source: str) -> MatchedTimex:

        if RegExpUtility.match_end(self.now_time_regex, source, True):
            return MatchedTimex(True, 'PRESENT_REF')
        elif RegExpUtility.match_end(self.recently_time_regex, source, True):
            return MatchedTimex(True, 'PAST_REF')
        elif RegExpUtility.match_end(self.asap_time_regex, source, True):
            return MatchedTimex(True, 'FUTURE_REF')

        return MatchedTimex(False, None)

    def get_swift_day(self, source: str) -> int:

        if RegExpUtility.match_begin(self.next_prefix_regex, source, True):
            return 1
        elif RegExpUtility.match_begin(self.previous_prefix_regex, source, True):
            return -1

        return 0

    def have_ambiguous_token(self, source: str, matched_text: str) -> bool:
        return False

