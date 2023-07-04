from recognizers_date_time.date_time.constants import Constants
from recognizers_date_time.date_time.extractors import DateTimeExtractor
from recognizers_date_time.date_time.parsers import DateTimeParser
from recognizers_date_time.date_time.CJK.base_timeperiod import CJKTimePeriodParserConfiguration
from recognizers_date_time.date_time.CJK.base_configs import CJKCommonDateTimeParserConfiguration
from recognizers_date_time.date_time.japanese.time_extractor_config import JapaneseTimeExtractorConfiguration
from recognizers_date_time.date_time.utilities import TimeFunctions, TimexUtil
from recognizers_date_time.resources.japanese_date_time import JapaneseDateTime


class JapaneseTimePeriodParserConfiguration(CJKTimePeriodParserConfiguration):

    @property
    def time_extractor(self) -> DateTimeExtractor:
        return self._time_extractor

    @property
    def time_parser(self) -> DateTimeParser:
        return self._time_parser

    @property
    def time_func(self) -> TimeFunctions:
        return self._time_func

    def __init__(self, config: CJKCommonDateTimeParserConfiguration):
        super().__init__()
        self._time_parser = config.time_parser
        self._time_extractor = config.time_extractor
        self._time_func = TimeFunctions(
            number_dictionary=JapaneseDateTime.TimeNumberDictionary,
            low_bound_desc=JapaneseDateTime.TimeLowBoundDesc,
            day_desc_regex=JapaneseTimeExtractorConfiguration.day_desc_regex
        )

    def get_matched_timex_range(self, text: str) -> dict:
        trimmed_text = text.strip()
        begin_hour = 0
        end_hour = 0
        end_min = 0

        if any(trimmed_text.endswith(o) for o in JapaneseDateTime.MorningTermList):
            time_of_day = Constants.MORNING
        elif any(trimmed_text.endswith(o) for o in JapaneseDateTime.MidDayTermList):
            time_of_day = Constants.MID_DAY
        elif any(trimmed_text.endswith(o) for o in JapaneseDateTime.AfternoonTermList):
            time_of_day = Constants.AFTERNOON
        elif any(trimmed_text.endswith(o) for o in JapaneseDateTime.EveningTermList):
            time_of_day = Constants.EVENING
        elif any(trimmed_text == o for o in JapaneseDateTime.DaytimeTermList):
            time_of_day = Constants.DAYTIME
        elif any(trimmed_text.endswith(o) for o in JapaneseDateTime.NightTermList):
            time_of_day = Constants.NIGHT
        elif any(trimmed_text.endswith(o) for o in JapaneseDateTime.BusinessHourTermList):
            time_of_day = Constants.BUSINESS_HOUR
        else:
            timex = None
            matched = False

            return {'matched': matched, 'timex': timex, 'begin_hour': begin_hour,
                    'end_hour': end_hour, 'end_min': end_min}

        parse_result = TimexUtil.parse_time_of_day(time_of_day)
        timex = parse_result.timex
        begin_hour = parse_result.begin_hour
        end_hour = parse_result.end_hour
        end_min = parse_result.end_min

        #  Modify time period if "early"/"late" is present
        if any(trimmed_text.endswith(o) for o in JapaneseDateTime.EarlyHourTermList):
            end_hour = begin_hour + Constants.HALF_MID_DAY_DURATION_HOUR_COUNT
            #  Handling special case: night ends with 23:59.
            if end_min == 59:
                end_min = 0

        if any(trimmed_text.endswith(o) for o in JapaneseDateTime.LateHourTermList):
            begin_hour = begin_hour + Constants.HALF_MID_DAY_DURATION_HOUR_COUNT

        matched = True
        return {'matched': matched, 'timex': timex, 'begin_hour': begin_hour,
                'end_hour': end_hour, 'end_min': end_min}
