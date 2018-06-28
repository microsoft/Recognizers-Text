from ..extractors import DateTimeExtractor
from ..base_merged import MergedExtractorConfiguration
from ..base_holiday import BaseHolidayExtractor
from .duration_extractor import ChineseDurationExtractor
from .time_extractor import ChineseTimeExtractor
from .date_extractor import ChineseDateExtractor
from .datetime_extractor import ChineseDateTimeExtractor
from .timeperiod_extractor import ChineseTimePeriodExtractor
from .dateperiod_extractor import ChineseDatePeriodExtractor
from .datetimeperiod_extractor import ChineseDateTimePeriodExtractor
from .set_extractor import ChineseSetExtractor
from .holiday_extractor_config import ChineseHolidayExtractorConfiguration

class ChineseMergedExtractorConfiguration(MergedExtractorConfiguration):
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
    def date_period_extractor(self) -> DateTimeExtractor:
        return self._date_period_extractor

    @property
    def time_period_extractor(self) -> DateTimeExtractor:
        return self._time_period_extractor

    @property
    def date_time_period_extractor(self) -> DateTimeExtractor:
        return self._date_time_period_extractor

    @property
    def holiday_extractor(self) -> DateTimeExtractor:
        return self._holiday_extractor

    @property
    def duration_extractor(self) -> DateTimeExtractor:
        return self._duration_extractor

    @property
    def set_extractor(self) -> DateTimeExtractor:
        return self._set_extractor

    @property
    def integer_extractor(self) -> any:
        return None

    @property
    def after_regex(self) -> any:
        return None

    @property
    def since_regex(self) -> any:
        return None

    @property
    def before_regex(self) -> any:
        return None

    @property
    def from_to_regex(self) -> any:
        return None

    @property
    def single_ambiguous_month_regex(self) -> any:
        return None

    @property
    def preposition_suffix_regex(self) -> any:
        return None

    @property
    def number_ending_pattern(self) -> any:
        return None

    @property
    def filter_word_regex_list(self) -> any:
        return None

    def __init__(self):
        self._date_extractor = ChineseDateExtractor()
        self._time_extractor = ChineseTimeExtractor()
        self._date_time_extractor = ChineseDateTimeExtractor()
        self._date_period_extractor = ChineseDatePeriodExtractor()
        self._time_period_extractor = ChineseTimePeriodExtractor()
        self._date_time_period_extractor = ChineseDateTimePeriodExtractor()
        self._holiday_extractor = BaseHolidayExtractor(ChineseHolidayExtractorConfiguration())
        self._duration_extractor = ChineseDurationExtractor()
        self._set_extractor = ChineseSetExtractor()
