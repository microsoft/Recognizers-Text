from abc import abstractmethod
from typing import List, Match
import datetime
from recognizers_text.extractor import ExtractResult

from recognizers_date_time.date_time.date_extractor import DateExtractor
from recognizers_date_time.date_time.constants import Constants
from recognizers_text.utilities import RegExpUtility


class AbstractYearExtractor(DateExtractor):

    def __init__(self, config):
        self.config = config

    @abstractmethod
    def extract(self, extract_result, text, reference: datetime = None) -> List[ExtractResult]:
        raise NotImplementedError

    def get_year_from_text(self, match: Match) -> int:
        year = Constants.INVALID_YEAR

        year_str = RegExpUtility.get_group(
            match, 'year')
        if year_str and not (str.isspace(year_str) or year_str is None):
            year = int(year_str)
            if 100 > year >= Constants.MIN_TWO_DIGIT_YEAR_PAST_NUM:
                year += 1900
            elif 0 <= year < Constants.MAX_TWO_DIGIT_YEAR_FUTURE_NUM:
                year += 2000
        else:
            first_two_year_num_str = RegExpUtility.get_group(
                match, Constants.FIRST_TWO_YEAR_NUM)

            if first_two_year_num_str and not (str.isspace(first_two_year_num_str) or first_two_year_num_str is None):

                er = ExtractResult()
                er.text = first_two_year_num_str
                er.start = match.string.index(RegExpUtility.get_group(match, Constants.FIRST_TWO_YEAR_NUM))
                er.length = len(RegExpUtility.get_group(
                    match, Constants.FIRST_TWO_YEAR_NUM))

                first_two_year_num = self.config.number_parser.parse(er).value if \
                    self.config.number_parser.parse(er).value else 0

                last_two_year_num = 0
                last_two_year_num_str = RegExpUtility.get_group(
                    match, Constants.LAST_TWO_YEAR_NUM)

                if not (str.isspace(last_two_year_num_str) or last_two_year_num_str is None):
                    er = ExtractResult()
                    er.text = last_two_year_num_str
                    er.start = match.string.index(RegExpUtility.get_group(match, Constants.LAST_TWO_YEAR_NUM))
                    er.length = len(RegExpUtility.get_group(
                        match, Constants.LAST_TWO_YEAR_NUM))

                    last_two_year_num = self.config.number_parser.parse(er).value if \
                        self.config.number_parser.parse(er).value else 0

                if (first_two_year_num < 100 and last_two_year_num == 0)\
                        or (first_two_year_num < 100 and first_two_year_num % 10 == 0
                            and len(last_two_year_num_str.strip().split(' ')) == 1):
                    year = Constants.INVALID_YEAR
                    return year

                if first_two_year_num >= 100:
                    year = first_two_year_num + last_two_year_num
                else:
                    year = (first_two_year_num * 100) + last_two_year_num

        return year
