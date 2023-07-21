from typing import Pattern, Dict

from recognizers_date_time.date_time.parsers import DateTimeParser
from recognizers_date_time.date_time.base_datetime import MatchedTimex
from recognizers_date_time.date_time.extractors import DateTimeExtractor
from recognizers_date_time.date_time.CJK import CJKSetParserConfiguration, CJKCommonDateTimeParserConfiguration
from recognizers_date_time.resources.japanese_date_time import JapaneseDateTime
from recognizers_date_time.date_time.japanese.set_extractor_config import JapaneseSetExtractorConfiguration
from recognizers_text import RegExpUtility


class JapaneseSetParserConfiguration(CJKSetParserConfiguration):

    @property
    def day_type_regex(self) -> Pattern:
        return self._day_type_regex

    @property
    def week_type_regex(self) -> Pattern:
        return self._week_type_regex

    @property
    def bi_week_type_regex(self) -> Pattern:
        return self._bi_week_type_regex

    @property
    def month_type_regex(self) -> Pattern:
        return self._month_type_regex

    @property
    def year_type_regex(self) -> Pattern:
        return self._year_type_regex

    @property
    def unit_map(self) -> Dict[str, str]:
        return self._unit_map

    @property
    def each_prefix_regex(self) -> Pattern:
        return self._each_prefix_regex

    @property
    def each_unit_regex(self) -> Pattern:
        return self._each_unit_regex

    @property
    def each_day_regex(self) -> Pattern:
        return self._each_day_regex

    @property
    def each_date_unit_regex(self) -> Pattern:
        return self._each_date_unit_regex

    @property
    def duration_extractor(self) -> DateTimeExtractor:
        return self._duration_extractor

    @property
    def duration_parser(self) -> DateTimeParser:
        return self._duration_parser

    @property
    def time_extractor(self) -> DateTimeExtractor:
        return self._time_extractor

    @property
    def time_parser(self) -> DateTimeParser:
        return self._time_parser

    @property
    def time_period_extractor(self) -> DateTimeExtractor:
        return self._time_period_extractor

    @property
    def time_period_parser(self) -> DateTimeParser:
        return self._time_period_parser

    @property
    def date_extractor(self) -> DateTimeExtractor:
        return self._date_extractor

    @property
    def date_parser(self) -> DateTimeParser:
        return self._date_parser

    @property
    def date_time_extractor(self) -> DateTimeExtractor:
        return self._date_time_extractor

    @property
    def date_time_parser(self) -> DateTimeParser:
        return self._date_time_parser

    def __init__(self, config: CJKCommonDateTimeParserConfiguration):
        super().__init__()
        self._day_type_regex = RegExpUtility.get_safe_reg_exp(JapaneseDateTime.DayTypeRegex)
        self._week_type_regex = RegExpUtility.get_safe_reg_exp(JapaneseDateTime.WeekTypeRegex)
        self._bi_week_type_regex = RegExpUtility.get_safe_reg_exp(JapaneseDateTime.BiWeekTypeRegex)
        self._month_type_regex = RegExpUtility.get_safe_reg_exp(JapaneseDateTime.MonthTypeRegex)
        self._year_type_regex = RegExpUtility.get_safe_reg_exp(JapaneseDateTime.YearTypeRegex)

        self._duration_parser = config.duration_parser
        self._time_parser = config.time_parser
        self._time_period_parser = config.time_period_parser
        self._date_parser = config.date_parser
        self._date_time_parser = config.date_time_parser

        self._duration_extractor = config.duration_extractor
        self._time_extractor = config.time_extractor
        self._time_period_extractor = config.time_period_extractor
        self._date_extractor = config.date_extractor
        self._date_time_extractor = config.date_time_extractor

        self._each_prefix_regex = JapaneseSetExtractorConfiguration().each_prefix_regex
        self._each_unit_regex = JapaneseSetExtractorConfiguration().each_unit_regex
        self._each_day_regex = JapaneseSetExtractorConfiguration().each_day_regex
        self._each_date_unit_regex = JapaneseSetExtractorConfiguration().each_date_unit_regex
        self._unit_map = config.unit_map

    def get_matched_unit_timex(self, text: str) -> MatchedTimex:
        trimmed_text = text.strip().lower()

        if RegExpUtility.get_matches(self._day_type_regex, trimmed_text):
            timex = 'P1D'
        elif RegExpUtility.get_matches(self._week_type_regex, trimmed_text):
            timex = 'P1W'
        elif RegExpUtility.get_matches(self._bi_week_type_regex, trimmed_text):
            timex = 'P2W'
        elif RegExpUtility.get_matches(self._month_type_regex, trimmed_text):
            timex = 'P1M'
        elif RegExpUtility.get_matches(self._year_type_regex, trimmed_text):
            timex = 'P1Y'
        else:
            return MatchedTimex(False, None)

        return MatchedTimex(True, timex)
