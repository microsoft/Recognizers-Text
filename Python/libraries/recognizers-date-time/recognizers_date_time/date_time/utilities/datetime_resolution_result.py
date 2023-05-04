from typing import List, Dict
from recognizers_date_time.date_time.utilities import TimeZoneResolutionResult


class DateTimeResolutionResult:
    def __init__(self):
        self.success: bool = False
        self.timex: str = ''
        self.is_lunar: bool = False
        self.mod: str = ''
        self.has_range_changing_mod: bool = False
        self.comment: str = ''
        self.future_resolution: Dict[str, str] = dict()
        self.past_resolution: Dict[str, str] = dict()
        self.future_value: object = None
        self.past_value: object = None
        self.sub_date_time_entities: List[object] = list()
        self.timezone_resolution: TimeZoneResolutionResult()
        self.list: List[object] = list()
