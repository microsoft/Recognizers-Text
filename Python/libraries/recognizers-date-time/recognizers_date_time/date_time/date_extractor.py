from abc import abstractmethod
from typing import Match
from recognizers_date_time.date_time.date_time_extractor import DateTimeExtractor


class DateExtractor(DateTimeExtractor):

    @abstractmethod
    def get_year_from_text(self, match: Match) -> int:
        raise NotImplementedError
