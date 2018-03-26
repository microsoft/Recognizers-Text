from datetime import datetime
import regex

from recognizers_text.utilities import RegExpUtility
from recognizers_number.number.parsers import BaseNumberParser
from recognizers_number.number.english.parsers import EnglishNumberParserConfiguration
from recognizers_number.number.english.extractors import EnglishCardinalExtractor, EnglishIntegerExtractor, EnglishOrdinalExtractor
from recognizers_date_time.date_time.utilities import DateTimeResolutionResult
from recognizers_date_time.date_time.base_time import BaseTimeParser, BaseTimeExtractor
from recognizers_date_time.date_time.base_date import DateParserConfiguration, BaseDateExtractor, BaseDateParser
from recognizers_date_time.date_time.base_datetime import BaseDateTimeExtractor, BaseDateTimeParser
from recognizers_date_time.date_time.base_duration import BaseDurationExtractor, BaseDurationParser
from recognizers_date_time.date_time.base_dateperiod import BaseDatePeriodExtractor, BaseDatePeriodParser
from recognizers_date_time.date_time.base_timeperiod import BaseTimePeriodExtractor, BaseTimePeriodParser
from recognizers_date_time.date_time.base_datetimeperiod import BaseDateTimePeriodExtractor, BaseDateTimePeriodParser
from recognizers_date_time.date_time.english.base_configs import EnglishDateTimeUtilityConfiguration
from recognizers_date_time.date_time.english.time_configs import EnglishTimeParserConfiguration, EnglishTimeExtractorConfiguration
from recognizers_date_time.date_time.english.date_configs import EnglishDateExtractorConfiguration
from recognizers_date_time.date_time.english.duration_configs import EnglishDurationExtractorConfiguration, EnglishDurationParserConfiguration
from recognizers_date_time.date_time.english.timeperiod_configs import EnglishTimePeriodExtractorConfiguration
from recognizers_date_time.date_time.english.dateperiod_configs import EnglishDatePeriodExtractorConfiguration, EnglishDatePeriodParserConfiguration
from recognizers_date_time.resources.english_date_time import BaseDateTime, EnglishDateTime


class EnglishTimeParser(BaseTimeParser):
    def __init__(self, config: EnglishTimeParserConfiguration):
        super().__init__(config)

    def internal_parser(self, source: str, reference: datetime) -> DateTimeResolutionResult:
        inner_result = self.parse_basic_regex_match(source, reference)
        if not inner_result.success:
            inner_result = self.parse_ish(source, reference)
        return inner_result

    def parse_ish(self, source: str, reference: datetime) -> DateTimeResolutionResult:
        result = DateTimeResolutionResult()
        source = source.strip().lower()
        match = regex.search(self.config.ish_regex, source)
        if match is None or match.group() != source:
            return result

        hour_str = RegExpUtility.get_group(match, 'hour')
        hour = 12
        if hour_str:
            hour = int(hour_str)

        result.timex = f'T{hour:02d}'
        result.future_value = datetime(reference.year, reference.month, reference.day, hour, 0, 0)
        result.past_value = result.future_value
        result.success = True
        return result

class EnglishCommonDateTimeParserConfiguration(DateParserConfiguration):

    @property
    def utility_configuration(self):
        return self._utility_configuration

    @property
    def unit_map(self):
        return self._unit_map

    @property
    def unit_value_map(self):
        return self._unit_value_map

    @property
    def season_map(self):
        return self._season_map

    @property
    def cardinal_map(self):
        return self._cardinal_map

    @property
    def day_of_week(self):
        return self._day_of_week

    @property
    def month_of_year(self):
        return self._month_of_year

    @property
    def numbers(self):
        return self._numbers

    @property
    def double_numbers(self):
        return self._double_numbers

    @property
    def cardinal_extractor(self):
        return self._cardinal_extractor

    @property
    def integer_extractor(self):
        return self._integer_extractor

    @property
    def ordinal_extractor(self):
        return self._ordinal_extractor

    @property
    def day_of_month(self):
        return self._day_of_month

    @property
    def number_parser(self):
        return self._number_parser

    @property
    def date_extractor(self):
        return self._date_extractor

    @property
    def time_extractor(self):
        return self._time_extractor

    @property
    def date_time_extractor(self):
        return self._date_time_extractor

    @property
    def duration_extractor(self):
        return self._duration_extractor

    @property
    def date_period_extractor(self):
        return self._date_period_extractor

    @property
    def time_period_extractor(self):
        return self._time_period_extractor

    @property
    def date_time_period_extractor(self):
        return self._date_time_period_extractor

    @property
    def duration_parser(self):
        return self._duration_parser

    @property
    def date_parser(self):
        return self._date_parser

    @property
    def time_parser(self):
        return self._time_parser

    @property
    def date_time_parser(self):
        return self._date_time_parser

    @property
    def date_period_parser(self):
        return self._date_period_parser

    @property
    def time_period_parser(self):
        return self._time_period_parser

    @property
    def date_time_period_parser(self):
        return self._date_time_period_parser

    def __init__(self):
        self._utility_configuration = EnglishDateTimeUtilityConfiguration()
        self._unit_map = EnglishDateTime.UnitMap
        self._unit_value_map = EnglishDateTime.UnitValueMap
        self._season_map = EnglishDateTime.SeasonMap
        self._cardinal_map = EnglishDateTime.CardinalMap
        self._day_of_week = EnglishDateTime.DayOfWeek
        self._month_of_year = EnglishDateTime.MonthOfYear
        self._numbers = EnglishDateTime.Numbers
        self._double_numbers = EnglishDateTime.DoubleNumbers
        self._cardinal_extractor = EnglishCardinalExtractor()
        self._integer_extractor = EnglishIntegerExtractor()
        self._ordinal_extractor = EnglishOrdinalExtractor()
        self._day_of_month = {**BaseDateTime.DayOfMonthDictionary, **EnglishDateTime.DayOfMonth}
        self._number_parser = BaseNumberParser(EnglishNumberParserConfiguration())
        self._date_extractor = BaseDateExtractor(EnglishDateExtractorConfiguration())
        self._time_extractor = BaseTimeExtractor(EnglishTimeExtractorConfiguration())
        self._date_time_extractor = None #BaseDateTimeExtractor(EnglishDateTimeExtractorConfiguration())
        self._duration_extractor = BaseDurationExtractor(EnglishDurationExtractorConfiguration())
        self._date_period_extractor = BaseDatePeriodExtractor(EnglishDatePeriodExtractorConfiguration())
        self._time_period_extractor = BaseTimePeriodExtractor(EnglishTimePeriodExtractorConfiguration())
        self._date_time_period_extractor = None #BaseDateTimePeriodExtractor(EnglishDateTimePeriodExtractorConfiguration())
        self._duration_parser = BaseDurationParser(EnglishDurationParserConfiguration()) #BaseDurationParser(EnglishDurationParserConfiguration(self))
        self._date_parser = None #BaseDateParser(EnglishDateParserConfiguration(self))
        self._time_parser = None #EnglishTimeParser(EnglishTimeParserConfiguration(self))
        self._date_time_parser = None #BaseDateTimeParser(EnglishDateTimeParserConfiguration(self))
        self._date_period_parser = BaseDatePeriodParser(EnglishDatePeriodParserConfiguration(self))
        self._time_period_parser = None #BaseTimePeriodParser(EnglishTimePeriodParserConfiguration(self))
        self._date_time_period_parser = None #BaseDateTimePeriodParser(EnglishDateTimePeriodParserConfiguration(self))