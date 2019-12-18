from typing import Dict, Pattern

from recognizers_text import RegExpUtility

from ...resources.chinese_date_time import ChineseDateTime
from ..extractors import DateTimeExtractor
from ..constants import Constants
from ..parsers import DateTimeParser
from ..base_datetimeperiod import DateTimePeriodParserConfiguration, MatchedTimeRange
from .date_extractor import ChineseDateExtractor
from .time_extractor import ChineseTimeExtractor
from .timeperiod_extractor import ChineseTimePeriodExtractor
from .datetime_extractor import ChineseDateTimeExtractor
from .date_parser import ChineseDateParser
from .time_parser import ChineseTimeParser
from .timeperiod_parser import ChineseTimePeriodParser
from .datetime_parser import ChineseDateTimeParser


class ChineseDateTimePeriodParserConfiguration(DateTimePeriodParserConfiguration):

    @property
    def future_suffix_regex(self):
        return self._future_suffix_regex

    @property
    def within_next_prefix_regex(self):
        return self._within_next_prefix_regex

    @property
    def previous_prefix_regex(self):
        return self._previous_prefix_regex

    @property
    def cardinal_extractor(self):
        return self._cardinal_extractor

    @property
    def am_desc_regex(self):
        return self._am_desc_regex

    @property
    def pm_desc_regex(self):
        return self._pm_desc_regex

    @property
    def before_regex(self):
        return self._before_regex

    @property
    def after_regex(self):
        return self._after_regex

    @property
    def prefix_day_regex(self):
        return self._prefix_day_regex

    @property
    def token_before_date(self):
        return self._token_before_date

    @property
    def check_both_before_after(self) -> bool:
        return self._check_both_before_after

    @property
    def pure_number_from_to_regex(self) -> any:
        return None

    @property
    def pure_number_between_and_regex(self) -> any:
        return None

    @property
    def period_time_of_day_with_date_regex(self) -> any:
        return None

    @property
    def specific_time_of_day_regex(self) -> Pattern:
        return self._specific_time_of_day_regex

    @property
    def past_regex(self) -> Pattern:
        return self._past_regex

    @property
    def future_regex(self) -> Pattern:
        return self._future_regex

    @property
    def relative_time_unit_regex(self) -> Pattern:
        return self._relative_time_unit_regex

    @property
    def rest_of_date_time_regex(self) -> any:
        return None

    @property
    def numbers(self) -> any:
        return None

    @property
    def unit_map(self) -> Dict[str, str]:
        return self._unit_map

    @property
    def date_extractor(self) -> DateTimeExtractor:
        return self._date_extractor

    @property
    def time_extractor(self) -> DateTimeExtractor:
        return self._time_extractor

    @property
    def date_time_extractor(self) -> DateTimeExtractor:
        return self._date_time_extractor

    @property
    def time_period_extractor(self) -> DateTimeExtractor:
        return self._time_period_extractor

    @property
    def duration_extractor(self) -> any:
        return None

    @property
    def date_parser(self) -> DateTimeParser:
        return self._date_parser

    @property
    def time_parser(self) -> DateTimeParser:
        return self._time_parser

    @property
    def date_time_parser(self) -> DateTimeParser:
        return self._date_time_parser

    @property
    def time_period_parser(self) -> DateTimeParser:
        return self._time_period_parser

    @property
    def duration_parser(self) -> any:
        return None

    def __init__(self):
        self._after_regex = RegExpUtility.get_safe_reg_exp(ChineseDateTime.AfterRegex)
        self._before_regex = RegExpUtility.get_safe_reg_exp(ChineseDateTime.BeforeRegex)
        self._specific_time_of_day_regex = RegExpUtility.get_safe_reg_exp(
            ChineseDateTime.SpecificTimeOfDayRegex)
        self._time_of_day_regex = RegExpUtility.get_safe_reg_exp(
            ChineseDateTime.TimeOfDayRegex
        )
        self._past_regex = RegExpUtility.get_safe_reg_exp(
            ChineseDateTime.PastRegex)
        self._future_regex = RegExpUtility.get_safe_reg_exp(
            ChineseDateTime.FutureRegex)
        self._relative_time_unit_regex = RegExpUtility.get_safe_reg_exp(
            ChineseDateTime.TimeOfDayRegex)
        self._unit_map = ChineseDateTime.ParserConfigurationUnitMap
        self._date_extractor = ChineseDateExtractor()
        self._time_extractor = ChineseTimeExtractor()
        self._date_time_extractor = ChineseDateTimeExtractor()
        self._time_period_extractor = ChineseTimePeriodExtractor()
        self._date_parser = ChineseDateParser()
        self._time_parser = ChineseTimeParser()
        self._date_time_parser = ChineseDateTimeParser()
        self._time_period_parser = ChineseTimePeriodParser()
        self._check_both_before_after = None
        self._token_before_date = None
        self._prefix_day_regex = None
        self._am_desc_regex = None
        self._pm_desc_regex = None
        self._cardinal_extractor = None
        self._previous_prefix_regex = None
        self._within_next_prefix_regex = None
        self._future_suffix_regex = None

    @property
    def time_of_day_regex(self) -> Pattern:
        return self._time_of_day_regex

    def get_matched_time_range(self, trimmed_source: str):
        trimmed_source = trimmed_source.strip().lower()
        time_str = ''
        begin_hour = 0
        end_hour = 0
        end_min = 0
        swift = 0
        if trimmed_source in ['今晚']:
            swift = 0
            time_str = 'TEV'
            begin_hour = 16
            end_hour = 20
        elif trimmed_source in ['今早', '今晨']:
            swift = 0
            time_str = 'TMO'
            begin_hour = 8
            end_hour = Constants.HALF_DAY_HOUR_COUNT
        elif trimmed_source in ['明晚']:
            swift = 1
            time_str = 'TEV'
            begin_hour = 16
            end_hour = 20
        elif trimmed_source in ['明早', '明晨']:
            swift = 1
            time_str = 'TMO'
            begin_hour = 8
            end_hour = Constants.HALF_DAY_HOUR_COUNT
        elif trimmed_source in ['昨晚']:
            swift = -1
            time_str = 'TEV'
            begin_hour = 16
            end_hour = 20
        else:
            time_str = None
            return MatchedTimeRange(time_str, begin_hour, end_hour, end_min, False, swift)

        return MatchedTimeRange(time_str, begin_hour, end_hour, end_min, True, swift)

    def get_swift_prefix(self, source: str) -> int:
        return None
