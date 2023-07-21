from typing import Dict, Pattern

from recognizers_number import BaseNumberParser, BaseNumberExtractor
from recognizers_text.utilities import RegExpUtility
from recognizers_date_time.date_time.parsers import DateTimeParser
from recognizers_date_time.date_time.base_datetime import MatchedTimex
from recognizers_date_time.date_time.constants import Constants
from recognizers_date_time.date_time.extractors import DateTimeExtractor
from recognizers_date_time.date_time.CJK import CJKDateTimeParserConfiguration,CJKCommonDateTimeParserConfiguration
from recognizers_date_time.date_time.japanese.datetime_extractor_config import JapaneseDateTimeExtractorConfiguration
from recognizers_date_time.resources.japanese_date_time import JapaneseDateTime


class JapaneseDateTimeParserConfiguration(CJKDateTimeParserConfiguration):
    @property
    def lunar_regex(self) -> Pattern:
        return self._lunar_regex

    @property
    def lunar_holiday_regex(self) -> Pattern:
        return self._lunar_holiday_regex

    @property
    def simple_am_regex(self) -> Pattern:
        return self._simple_am_regex

    @property
    def simple_pm_regex(self) -> Pattern:
        return self._simple_pm_regex

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
    def date_extractor(self) -> DateTimeExtractor:
        return self._date_extractor

    @property
    def time_extractor(self) -> DateTimeExtractor:
        return self._time_extractor

    @property
    def duration_extractor(self) -> DateTimeExtractor:
        return self._duration_extractor

    @property
    def date_parser(self) -> DateTimeParser:
        return self._date_parser

    @property
    def duration_parser(self) -> DateTimeParser:
        return self._duration_parser

    @property
    def time_parser(self) -> DateTimeParser:
        return self._time_parser

    @property
    def integer_extractor(self) -> BaseNumberExtractor:
        return self._integer_extractor

    @property
    def number_parser(self) -> BaseNumberParser:
        return self._number_parser

    @property
    def unit_map(self) -> Dict[str, str]:
        return self._unit_map

    @property
    def now_regex(self) -> Pattern:
        return self._now_regex

    @property
    def time_of_special_day_regex(self) -> Pattern:
        return self._time_of_special_day_regex

    @property
    def datetime_period_unit_regex(self) -> Pattern:
        return self._datetime_period_unit_regex

    @property
    def before_regex(self) -> Pattern:
        return self._before_regex

    @property
    def after_regex(self) -> Pattern:
        return self._after_regex

    @property
    def duration_relative_duration_unit_regex(self) -> Pattern:
        return self._duration_relative_duration_unit_regex

    @property
    def ago_later_regex(self) -> Pattern:
        return self._ago_later_regex

    def __init__(self, config: CJKCommonDateTimeParserConfiguration = None):
        super().__init__()
        self._integer_extractor = config.integer_extractor
        self._date_extractor = config.date_extractor
        self._time_extractor = config.time_extractor
        self._duration_extractor = config.duration_extractor

        self._date_parser = config.date_parser
        self._duration_parser = config.duration_parser
        self._time_parser = config.time_parser

        self._number_parser = config.number_parser

        self._unit_map = JapaneseDateTime.ParserConfigurationUnitMap
        self._now_regex = JapaneseDateTimeExtractorConfiguration().now_regex
        self._time_of_special_day_regex = JapaneseDateTimeExtractorConfiguration().time_of_special_day_regex
        self._datetime_period_unit_regex = JapaneseDateTimeExtractorConfiguration().datetime_period_unit_regex
        self._before_regex = JapaneseDateTimeExtractorConfiguration().before_regex
        self._after_regex = JapaneseDateTimeExtractorConfiguration().after_regex
        self._duration_relative_duration_unit_regex = \
            JapaneseDateTimeExtractorConfiguration().duration_relative_duration_unit_regex
        self._ago_later_regex = JapaneseDateTimeExtractorConfiguration().ago_later_regex

        self._lunar_regex = RegExpUtility.get_safe_reg_exp(JapaneseDateTime.LunarRegex)
        self._lunar_holiday_regex = RegExpUtility.get_safe_reg_exp(JapaneseDateTime.LunarHolidayRegex)
        self._simple_am_regex = RegExpUtility.get_safe_reg_exp(JapaneseDateTime.DateTimeSimpleAmRegex)
        self._simple_pm_regex = RegExpUtility.get_safe_reg_exp(JapaneseDateTime.DateTimeSimplePmRegex)
        self._now_time_regex = RegExpUtility.get_safe_reg_exp(JapaneseDateTime.NowTimeRegex)
        self._recently_time_regex = RegExpUtility.get_safe_reg_exp(JapaneseDateTime.RecentlyTimeRegex)
        self._asap_time_regex = RegExpUtility.get_safe_reg_exp(JapaneseDateTime.AsapTimeRegex)

    def get_matched_now_timex(self, source: str) -> MatchedTimex:
        source = source.strip().lower()
        if RegExpUtility.match_end(self.now_time_regex, source, False):
            timex = 'PRESENT_REF'
        elif RegExpUtility.exact_match(self.recently_time_regex, source, False):
            timex = 'PAST_REF'
        elif RegExpUtility.exact_match(self.asap_time_regex, source, False):
            timex = 'FUTURE_REF'
        else:
            return MatchedTimex(False, None)

        return MatchedTimex(True, timex)

    def get_swift_day(self, source: str) -> int:
        source = source.strip().lower()
        swift = 0

        if (
                source == '今天' or
                source == '今日' or
                source == '最近'
        ):
            swift = 0
        elif (
                source.startswith('明')
        ):
            swift = 1
        elif (
                source.startswith('昨')
        ):
            swift = -1
        elif (
            source == '大后天' or
            source == '大後天'
        ):
            swift = 3
        elif (
            source == '大前天'
        ):
            swift = -3
        elif (
            source == '后天' or
            source == '後天'
        ):
            swift = 2
        elif (
            source == '前天'
        ):
            swift = -2

        return swift

    def adjust_by_time_of_day(self, source: str, hour: int, swift: int) -> None:
        # TODO: THIS DOESN'T DO ANYTHING
        if source == '今晚':
            if hour < Constants.HALF_DAY_HOUR_COUNT:
                hour += Constants.HALF_DAY_HOUR_COUNT
        elif source == '今早':
            if hour >= Constants.HALF_DAY_HOUR_COUNT:
                hour -= Constants.HALF_DAY_HOUR_COUNT
        elif source == '明晚':
            swift = 1
            if hour < Constants.HALF_DAY_HOUR_COUNT:
                hour += Constants.HALF_DAY_HOUR_COUNT
        elif source == '明晨':
            swift = -1
            if hour >= Constants.HALF_DAY_HOUR_COUNT:
                hour -= Constants.HALF_DAY_HOUR_COUNT
        elif "昨晚":
            swift = -1
            if hour < Constants.HALF_DAY_HOUR_COUNT:
                hour += Constants.HALF_DAY_HOUR_COUNT
