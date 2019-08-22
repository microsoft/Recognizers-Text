from abc import abstractmethod
from typing import List, Pattern
import datetime
from recognizers_text.extractor import ExtractResult
from recognizers_date_time.date_time.base_date import DateExtractorConfiguration
from recognizers_date_time.date_time.date_extractor import DateExtractor
from recognizers_date_time.date_time.constants import Constants
from recognizers_text.utilities import RegExpUtility


class AbstractYearExtractor(DateExtractor):

    def __init__(self, config: DateExtractorConfiguration):
        self.config = config

    @property
    @abstractmethod
    def extract(self, extract_result, text, reference: datetime = None) -> List[ExtractResult]:
        raise NotImplementedError

    def get_year_from_text(self, match: Pattern) -> int:
        year = Constants.InvalidYear

        year_str = RegExpUtility.get_group(
            match, 'year')
        if not (str.isspace(year_str) or year_str is None):
            year = year_str
            if year < 100 and year >= Constants.MinTwoDigitYearPastNum:
                year += 1900
            elif year >= 0 and year < Constants.MaxTwoDigitYearFutureNum:
                year += 2000
        else:
            first_two_year_num_str = RegExpUtility.get_group(
                match, 'firsttwoyearnum')

            if not (str.isspace(first_two_year_num_str) or first_two_year_num_str is None):

                er = ExtractResult
                er.text = first_two_year_num_str
                er.start = RegExpUtility.get_group(
                    match, 'firsttwoyearnum').index()
                er.length = len(RegExpUtility.get_group(
                    match, 'firsttwoyearnum'))

            first_two_year_num = self.config.number_parser.parse(er).value if \
                self.config.number_parser.parse(er).value else 0

            last_two_year_num = 0
            last_two_year_num_str = RegExpUtility.get_group(
                match, 'lasttwoyearnum')

            if not (str.isspace(last_two_year_num_str) or last_two_year_num_str is None):
                er = ExtractResult
                er.text = last_two_year_num_str
                er.start = RegExpUtility.get_group(
                    match, 'lasttwoyearnum').index()
                er.length = len(RegExpUtility.get_group(
                    match, 'lasttwoyearnum'))

                last_two_year_num = self.config.number_parser.parse(er).value if \
                    self.config.number_parser.parse(er).value else 0

            if (first_two_year_num < 100 and last_two_year_num == 0)\
                    or (first_two_year_num < 100 and first_two_year_num % 10 == 0
                        and len(last_two_year_num_str.strip().split(' ')) == 1):
                year = Constants.InvalidYear
            return year

            if first_two_year_num >= 100:
                year = first_two_year_num + last_two_year_num
            else:
                year = (first_two_year_num * 100) + last_two_year_num

        return year
