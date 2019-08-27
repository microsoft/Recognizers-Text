from abc import abstractmethod
from typing import Pattern
from recognizers_date_time.date_time.date_time_extractor import DateTimeExtractor


class DateExtractor(DateTimeExtractor):

    @abstractmethod
    def get_year_from_text(self, match: Pattern) -> int:
        raise NotImplementedError
