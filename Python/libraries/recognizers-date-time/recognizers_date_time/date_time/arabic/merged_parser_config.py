from typing import Pattern

from recognizers_text.utilities import RegExpUtility

from recognizers_date_time.date_time.arabic.holiday_parser_config import ArabicHolidayParserConfiguration
from recognizers_date_time.date_time.arabic.set_parser_config import ArabicSetParserConfiguration
from recognizers_date_time.date_time.arabic.dateperiod_parser_config import ArabicDatePeriodParserConfiguration
from recognizers_date_time.date_time.arabic.timeperiod_parser_config import ArabicTimePeriodParserConfiguration
from recognizers_date_time.date_time.arabic.common_configs import ArabicCommonDateTimeParserConfiguration
from recognizers_date_time.date_time.base_date import BaseDateParser
from recognizers_date_time.date_time.base_time import BaseTimeParser
from recognizers_date_time.date_time.base_datetime import BaseDateTimeParser
from recognizers_date_time.date_time.base_holiday import BaseHolidayParser
from recognizers_date_time.date_time.base_dateperiod import BaseDatePeriodParser
from recognizers_date_time.date_time.base_timeperiod import BaseTimePeriodParser
from recognizers_date_time.date_time.base_datetimeperiod import BaseDateTimePeriodParser
from recognizers_date_time.date_time.base_duration import BaseDurationParser
from recognizers_date_time.date_time.base_set import BaseSetParser
from recognizers_date_time.date_time.base_merged import MergedParserConfiguration
from recognizers_date_time.resources.arabic_date_time import ArabicDateTime, BaseDateTime


class ArabicMergedParserConfiguration(ArabicCommonDateTimeParserConfiguration, MergedParserConfiguration):
    @property
    def around_regex(self) -> Pattern:
        return self._around_regex

    @property
    def equal_regex(self) -> Pattern:
        return self._equal_regex

    @property
    def year_regex(self) -> Pattern:
        return self._year_regex

    @property
    def suffix_after(self) -> Pattern:
        return self._suffix_after

    @property
    def before_regex(self) -> Pattern:
        return self._before_regex

    @property
    def after_regex(self) -> Pattern:
        return self._after_regex

    @property
    def since_regex(self) -> Pattern:
        return self._since_regex

    @property
    def date_parser(self) -> BaseDateParser:
        return self._date_parser

    @property
    def holiday_parser(self) -> BaseHolidayParser:
        return self._holiday_parser

    @property
    def time_parser(self) -> BaseTimeParser:
        return self._time_parser

    @property
    def date_time_parser(self) -> BaseDateTimeParser:
        return self._date_time_parser

    @property
    def date_period_parser(self) -> BaseDatePeriodParser:
        return self._date_period_parser

    @property
    def time_period_parser(self) -> BaseTimePeriodParser:
        return self._time_period_parser

    @property
    def date_time_period_parser(self) -> BaseDateTimePeriodParser:
        return self._date_time_period_parser

    @property
    def duration_parser(self) -> BaseDurationParser:
        return self._duration_parser

    @property
    def set_parser(self) -> BaseSetParser:
        return self._set_parser

    def __init__(self, config):
        ArabicCommonDateTimeParserConfiguration.__init__(self)
        self._suffix_after = RegExpUtility.get_safe_reg_exp(
            ArabicDateTime.SuffixAfterRegex)
        self._year_regex = RegExpUtility.get_safe_reg_exp(ArabicDateTime.YearRegex)
        self._equal_regex = RegExpUtility.get_safe_reg_exp(BaseDateTime.EqualRegex)
        self._around_regex = RegExpUtility.get_safe_reg_exp(ArabicDateTime.AroundRegex)
        self._before_regex = RegExpUtility.get_safe_reg_exp(
            ArabicDateTime.BeforeRegex)
        self._after_regex = RegExpUtility.get_safe_reg_exp(
            ArabicDateTime.AfterRegex)
        self._since_regex = RegExpUtility.get_safe_reg_exp(
            ArabicDateTime.SinceRegex)

        self._date_period_parser = BaseDatePeriodParser(
            ArabicDatePeriodParserConfiguration(self))
        self._time_period_parser = BaseTimePeriodParser(
            ArabicTimePeriodParserConfiguration(self))
        self._set_parser = BaseSetParser(ArabicSetParserConfiguration(self))
        self._holiday_parser = BaseHolidayParser(ArabicHolidayParserConfiguration(self))
