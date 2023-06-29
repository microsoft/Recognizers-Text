from recognizers_date_time.date_time.extractors import DateTimeExtractor
from recognizers_date_time.date_time.data_structures import TimeType
from recognizers_date_time.date_time.CJK.base_time import CJKTimeParserConfiguration
from recognizers_date_time.date_time.CJK.base_configs import CJKCommonDateTimeParserConfiguration
from recognizers_date_time.date_time.japanese.time_extractor_config import JapaneseTimeExtractorConfiguration
from recognizers_date_time.date_time.utilities import TimeFunctions
from recognizers_date_time.resources.japanese_date_time import JapaneseDateTime


class JapaneseTimeParserConfiguration(CJKTimeParserConfiguration):
    @property
    def time_extractor(self) -> DateTimeExtractor:
        return self._time_extractor

    @property
    def time_func(self) -> TimeFunctions:
        return self._time_func

    @property
    def function_map(self):
        return self._function_map

    def __init__(self, config: CJKCommonDateTimeParserConfiguration):
        super().__init__()
        self._time_func = TimeFunctions(
            number_dictionary=JapaneseDateTime.TimeNumberDictionary,
            low_bound_desc=JapaneseDateTime.TimeLowBoundDesc,
            day_desc_regex=JapaneseTimeExtractorConfiguration.day_desc_regex
        )
        self._function_map = {
            TimeType.DigitTime: self.time_func.handle_digit,
            TimeType.CJKTime: self.time_func.handle_kanji,
            TimeType.LessTime: self.time_func.handle_less
        }

        self._time_extractor = config.time_extractor


