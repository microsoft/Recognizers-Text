from typing import Pattern
from recognizers_text.utilities import RegExpUtility

from .holiday_parser_config import FrenchHolidayParserConfiguration
from .set_parser_config import FrenchSetParserConfiguration
from .dateperiod_parser_config import FrenchDatePeriodParserConfiguration
from .timeperiod_parser_config import FrenchTimePeriodParserConfiguration
from .common_configs import FrenchCommonDateTimeParserConfiguration
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
from ...resources.french_date_time import FrenchDateTime

class FrenchMergedParserConfiguration(FrenchCommonDateTimeParserConfiguration, MergedParserConfiguration):
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
        FrenchCommonDateTimeParserConfiguration.__init__(self)

        self._before_regex = RegExpUtility.get_safe_reg_exp(FrenchDateTime.BeforeRegex)
        self._after_regex = RegExpUtility.get_safe_reg_exp(FrenchDateTime.AfterRegex)
        self._since_regex = RegExpUtility.get_safe_reg_exp(FrenchDateTime.SinceRegex)

        self._date_period_parser = BaseDatePeriodParser(FrenchDatePeriodParserConfiguration(self))
        self._time_period_parser = BaseTimePeriodParser(FrenchTimePeriodParserConfiguration(self))
        self._set_parser = BaseSetParser(FrenchSetParserConfiguration(config))
        self._holiday_parser = BaseHolidayParser(FrenchHolidayParserConfiguration(config))
