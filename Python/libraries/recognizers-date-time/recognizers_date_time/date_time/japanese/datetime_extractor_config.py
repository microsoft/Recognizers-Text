from typing import Dict, Pattern

from recognizers_text.utilities import RegExpUtility, DefinitionLoader
from recognizers_date_time.date_time.extractors import DateTimeExtractor
from recognizers_date_time.date_time.utilities import DateTimeOptionsConfiguration
from recognizers_date_time.date_time.CJK import CJKDateTimeExtractorConfiguration, BaseCJKDateExtractor, \
    BaseCJKTimeExtractor, BaseCJKDurationExtractor
from recognizers_date_time.date_time.japanese.date_extractor_config import JapaneseDateExtractorConfiguration
from recognizers_date_time.date_time.japanese.time_extractor_config import JapaneseTimeExtractorConfiguration
from recognizers_date_time.date_time.japanese.duration_extractor_config import JapaneseDurationExtractorConfiguration
from recognizers_date_time.resources.japanese_date_time import JapaneseDateTime


class JapaneseDateTimeExtractorConfiguration(CJKDateTimeExtractorConfiguration):
    @property
    def preposition_regex(self) -> Pattern:
        return self._preposition_regex

    @property
    def now_regex(self) -> Pattern:
        return self._now_regex

    @property
    def night_regex(self) -> Pattern:
        return self._night_regex

    @property
    def time_of_special_day_regex(self) -> Pattern:
        return self._time_of_special_day_regex

    @property
    def time_of_day_regex(self) -> Pattern:
        return self._time_of_day_regex

    @property
    def before_regex(self) -> Pattern:
        return self._before_regex

    @property
    def after_regex(self) -> Pattern:
        return self._after_regex

    @property
    def datetime_period_unit_regex(self) -> Pattern:
        return self._datetime_period_unit_regex

    @property
    def duration_relative_duration_unit_regex(self) -> Pattern:
        return self._duration_relative_duration_unit_regex

    @property
    def ago_later_regex(self) -> Pattern:
        return self._ago_later_regex

    @property
    def connector_regex(self) -> Pattern:
        return self._connector_regex

    @property
    def duration_extractor(self) -> DateTimeExtractor:
        return self._duration_extractor

    @property
    def date_point_extractor(self) -> DateTimeExtractor:
        return self._date_point_extractor

    @property
    def time_point_extractor(self) -> DateTimeExtractor:
        return self._time_point_extractor

    @property
    def ambiguity_date_time_filters(self) -> Dict[Pattern, Pattern]:
        return self._ambiguity_date_time_filters

    def __init__(self):
        super().__init__()
        self._date_point_extractor = BaseCJKDateExtractor(JapaneseDateExtractorConfiguration())
        self._time_point_extractor = BaseCJKTimeExtractor(JapaneseTimeExtractorConfiguration())
        self._duration_extractor = BaseCJKDurationExtractor(JapaneseDurationExtractorConfiguration())

        self._preposition_regex = RegExpUtility.get_safe_reg_exp(JapaneseDateTime.PrepositionRegex)
        self._now_regex = RegExpUtility.get_safe_reg_exp(JapaneseDateTime.NowRegex)
        self._night_regex = RegExpUtility.get_safe_reg_exp(JapaneseDateTime.NightRegex)
        self._time_of_special_day_regex = RegExpUtility.get_safe_reg_exp(JapaneseDateTime.TimeOfSpecialDayRegex)
        self._time_of_day_regex = RegExpUtility.get_safe_reg_exp(JapaneseDateTime.TimeOfDayRegex)
        self._before_regex = RegExpUtility.get_safe_reg_exp(JapaneseDateTime.BeforeRegex)
        self._after_regex = RegExpUtility.get_safe_reg_exp(JapaneseDateTime.AfterRegex)
        self._datetime_period_unit_regex = RegExpUtility.get_safe_reg_exp(JapaneseDateTime.DateTimePeriodUnitRegex)
        self._duration_relative_duration_unit_regex = RegExpUtility.get_safe_reg_exp(
            JapaneseDateTime.DurationRelativeDurationUnitRegex)
        self._ago_later_regex = RegExpUtility.get_safe_reg_exp(JapaneseDateTime.AgoLaterRegex)
        self._connector_regex = RegExpUtility.get_safe_reg_exp(JapaneseDateTime.ConnectorRegex)

        self._ambiguity_date_time_filters = DefinitionLoader.load_ambiguity_filters(
            JapaneseDateTime.AmbiguityDateTimeFiltersDict
        )
