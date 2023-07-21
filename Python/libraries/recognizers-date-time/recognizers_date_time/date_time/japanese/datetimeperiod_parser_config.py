from typing import Dict, Pattern

from recognizers_text.utilities import RegExpUtility
from recognizers_text import Parser, Extractor
from recognizers_number.number.japanese.extractors import JapaneseCardinalExtractor
from recognizers_number.number import AgnosticNumberParserFactory, JapaneseNumberParserConfiguration, ParserType
from recognizers_date_time.date_time.constants import Constants
from recognizers_date_time.date_time.parsers import DateTimeParser
from recognizers_date_time.date_time.extractors import DateTimeExtractor
from recognizers_date_time.date_time.CJK import CJKDateTimePeriodParserConfiguration, \
    CJKCommonDateTimeParserConfiguration, MatchedTimeRegexAndSwift
from recognizers_date_time.resources.japanese_date_time import JapaneseDateTime
from recognizers_date_time.date_time.japanese.datetimeperiod_extractor_config import \
    JapaneseDateTimePeriodExtractorConfiguration
from recognizers_date_time.date_time.utilities import TimexUtil


class JapaneseDateTimePeriodParserConfiguration(CJKDateTimePeriodParserConfiguration):
    @property
    def mo_regex(self) -> Pattern:
        return self._mo_regex
    
    @property
    def mi_regex(self) -> Pattern:
        return self._mi_regex

    @property
    def af_regex(self) -> Pattern:
        return self._af_regex

    @property
    def ev_regex(self) -> Pattern:
        return self._ev_regex

    @property
    def ni_regex(self) -> Pattern:
        return self._ni_regex

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
    def duration_extractor(self) -> DateTimeExtractor:
        return self._duration_extractor

    @property
    def duration_parser(self) -> DateTimeParser:
        return self._duration_parser

    @property
    def cardinal_extractor(self) -> Extractor:
        return self._cardinal_extractor

    @property
    def cardinal_parser(self) -> Parser:
        return self._cardinal_parser

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
    def specific_time_of_day_regex(self) -> Pattern:
        return self._specific_time_of_day_regex

    @property
    def time_of_day_regex(self) -> Pattern:
        return self._time_of_day_regex

    @property
    def next_regex(self) -> Pattern:
        return self._next_regex

    @property
    def last_regex(self) -> Pattern:
        return self._last_regex

    @property
    def past_regex(self) -> Pattern:
        return self._past_regex

    @property
    def future_regex(self) -> Pattern:
        return self._future_regex

    @property
    def weekday_regex(self) -> Pattern:
        return self._weekday_regex

    @property
    def time_period_left_regex(self) -> Pattern:
        return self._time_period_left_regex

    @property
    def unit_regex(self) -> Pattern:
        return self._unit_regex

    @property
    def rest_of_date_regex(self) -> Pattern:
        return self._rest_of_date_regex

    @property
    def am_pm_desc_regex(self) -> Pattern:
        return self._am_pm_desc_regex

    @property
    def unit_map(self) -> Dict[str, str]:
        return self._unit_map

    def __init__(self, config: CJKCommonDateTimeParserConfiguration):

        super().__init__()
        self._cardinal_extractor = JapaneseCardinalExtractor()
        self._cardinal_parser = AgnosticNumberParserFactory.get_parser(
                ParserType.NUMBER, JapaneseNumberParserConfiguration())

        self._date_extractor = config.date_extractor
        self._time_extractor = config.time_extractor
        self._date_time_extractor = config.date_time_extractor
        self._time_period_extractor = config.time_period_extractor
        self._duration_extractor = config.duration_extractor
        self._duration_parser = config.duration_parser
        self._date_parser = config.date_parser
        self._time_parser = config.time_parser
        self._date_time_parser = config.date_time_parser
        self._time_period_parser = config.time_period_parser

        self._mo_regex = RegExpUtility.get_safe_reg_exp(JapaneseDateTime.DateTimePeriodMORegex)
        self._mi_regex = RegExpUtility.get_safe_reg_exp(JapaneseDateTime.DateTimePeriodMIRegex)
        self._af_regex = RegExpUtility.get_safe_reg_exp(JapaneseDateTime.DateTimePeriodAFRegex)
        self._ev_regex = RegExpUtility.get_safe_reg_exp(JapaneseDateTime.DateTimePeriodEVRegex)
        self._ni_regex = RegExpUtility.get_safe_reg_exp(JapaneseDateTime.DateTimePeriodNIRegex)

        self._specific_time_of_day_regex = JapaneseDateTimePeriodExtractorConfiguration().specific_time_of_day_regex
        self._time_of_day_regex = JapaneseDateTimePeriodExtractorConfiguration().time_of_day_regex
        self._next_regex = JapaneseDateTimePeriodExtractorConfiguration().next_regex
        self._last_regex = JapaneseDateTimePeriodExtractorConfiguration().last_regex
        self._past_regex = JapaneseDateTimePeriodExtractorConfiguration().past_regex
        self._future_regex = JapaneseDateTimePeriodExtractorConfiguration().future_regex
        self._weekday_regex = JapaneseDateTimePeriodExtractorConfiguration().weekday_regex
        self._time_period_left_regex = JapaneseDateTimePeriodExtractorConfiguration().time_period_left_regex
        self._unit_regex = JapaneseDateTimePeriodExtractorConfiguration().unit_regex
        self._rest_of_date_regex = JapaneseDateTimePeriodExtractorConfiguration().rest_of_date_regex
        self._am_pm_desc_regex = JapaneseDateTimePeriodExtractorConfiguration().am_pm_desc_regex
        self._unit_map = config.unit_map

    def get_matched_time_range(self, text: str) -> MatchedTimeRegexAndSwift:
        trimmed_text = text.strip().lower()

        begin_hour = 0
        end_hour = 0
        end_minute = 0
        swift = 0

        tod = ""

        if trimmed_text == "今晚":
            swift = 0
            tod = Constants.EVENING
        elif trimmed_text == "今早" or trimmed_text == "今晨":
            swift = 0
            tod = Constants.MORNING
        elif trimmed_text == "明晚":
            swift = 1
            tod = Constants.EVENING
        elif trimmed_text == "明早" or trimmed_text == "明晨":
            swift = 1
            tod = Constants.MORNING
        elif trimmed_text == "昨晚":
            swift = -1
            tod = Constants.EVENING

        if RegExpUtility.get_matches(self.mo_regex, trimmed_text):
            tod = Constants.MORNING
        elif RegExpUtility.get_matches(self.mi_regex, trimmed_text):
            tod = Constants.MIDDAY
        elif RegExpUtility.get_matches(self.af_regex, trimmed_text):
            tod = Constants.AFTERNOON
        elif RegExpUtility.get_matches(self.ev_regex, trimmed_text):
            tod = Constants.EVENING
        elif RegExpUtility.get_matches(self.ni_regex, trimmed_text):
            tod = Constants.NIGHT
        elif not tod:
            tod_symbol = None
            return MatchedTimeRegexAndSwift(False, tod_symbol, begin_hour, end_hour, end_minute, swift)

        parse_result = TimexUtil.resolve_time_of_day(tod)
        tod_symbol = parse_result.timex
        begin_hour = parse_result.begin_hour
        end_hour = parse_result.end_hour
        end_minute = parse_result.end_min

        return MatchedTimeRegexAndSwift(True, tod_symbol, begin_hour, end_hour, end_minute, swift)

    def get_matched_time_range_and_swift(self, text: str) -> MatchedTimeRegexAndSwift:
        return self.get_matched_time_range(text)
