from typing import Pattern
from recognizers_text.utilities import RegExpUtility

from .holiday_parser_config import EnglishHolidayParserConfiguration
from .set_parser_config import EnglishSetParserConfiguration
from ..base_date import BaseDateParser
from ..base_time import BaseTimeParser
from ..base_datetime import BaseDateTimeParser
from ..base_holiday import BaseHolidayParser
from ..base_dateperiod import BaseDatePeriodParser
from ..base_timeperiod import BaseTimePeriodParser
from ..base_datetimeperiod import BaseDateTimePeriodParser
from ..base_duration import BaseDurationParser
from ..base_set import BaseSetParser
from ..base_merged import MergedParserConfiguration
from ...resources.english_date_time import EnglishDateTime

class EnglishMergedParserConfiguration(MergedParserConfiguration):
    @property
    def before_regex(self) -> Pattern:
        return self.__before_regex

    @property
    def after_regex(self) -> Pattern:
        return self.__after_regex

    @property
    def since_regex(self) -> Pattern:
        return self.__since_regex

    @property
    def date_parser(self) -> BaseDateParser:
        return self.__date_parser

    @property
    def holiday_parser(self) -> BaseHolidayParser:
        return self.__holiday_parser

    @property
    def time_parser(self) -> BaseTimeParser:
        return self.__time_parser

    @property
    def date_time_parser(self) -> BaseDateTimeParser:
        return self.__date_time_parser

    @property
    def date_period_parser(self) -> BaseDatePeriodParser:
        return self.__date_period_parser

    @property
    def time_period_parser(self) -> BaseTimePeriodParser:
        return self.__time_period_parser

    @property
    def date_time_period_parser(self) -> BaseDateTimePeriodParser:
        return self.__date_time_period_parser

    @property
    def duration_parser(self) -> BaseDurationParser:
        return self.__duration_parser

    @property
    def set_parser(self) -> BaseSetParser:
        return self.__set_parser

    def __init__(self, config):
        self.__before_regex = RegExpUtility.get_safe_reg_exp(EnglishDateTime.BeforeRegex)
        self.__after_regex = RegExpUtility.get_safe_reg_exp(EnglishDateTime.AfterRegex)
        self.__since_regex = RegExpUtility.get_safe_reg_exp(EnglishDateTime.SinceRegex)
        self.__holiday_parser = BaseHolidayParser(EnglishHolidayParserConfiguration(config))
        self.__date_parser = config.date_parser
        self.__time_parser = config.time_parser
        self.__date_time_parser = config.date_time_parser
        self.__date_period_parser = config.date_period_parser
        self.__time_period_parser = config.time_period_parser
        self.__date_time_period_parser = config.date_time_period_parser
        self.__duration_parser = config.duration_parser
        self.__set_parser = BaseSetParser(EnglishSetParserConfiguration(config))
