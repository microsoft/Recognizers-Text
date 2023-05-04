from datetime import datetime

from datedelta import datedelta
from recognizers_date_time.date_time.constants import TimeTypeConstants, Constants
from recognizers_date_time.date_time.parsers import DateTimeParseResult
from recognizers_date_time.date_time.utilities import DateTimeResolutionResult, TimexUtil, DateUtils


class DateContext:
    year: int = Constants.INVALID_YEAR

    # This method is to ensure the begin date is less than the end date. As DateContext only supports common Year as
    # context, so it subtracts one year from beginDate.
    # @TODO problematic in other usages.
    @staticmethod
    def swift_date_object(begin_date: datetime, end_date: datetime) -> datetime:
        if begin_date > end_date:
            begin_date = begin_date - datedelta(years=1)
        return begin_date

    def process_date_entity_parsing_result(self, original_result: DateTimeParseResult) -> DateTimeParseResult:
        if not self.is_empty():
            original_result.timex_str = TimexUtil.set_timex_with_context(original_result.timex_str, self)
            original_result.value = self.process_date_entity_resolution(original_result.value)

        return original_result

    def process_date_entity_resolution(self, resolution_result: DateTimeResolutionResult) -> DateTimeResolutionResult:
        if not self.is_empty():
            resolution_result.timex = TimexUtil.set_timex_with_context(resolution_result.timex, self)
            resolution_result.future_value = self.__set_date_with_context(resolution_result.future_value)
            resolution_result.past_value = self.__set_date_with_context(resolution_result.past_value)
        return resolution_result

    def process_date_period_entity_resolution(self, resolution_result: DateTimeResolutionResult) -> DateTimeResolutionResult:
        if not self.is_empty():
            resolution_result.timex = TimexUtil.set_timex_with_context(resolution_result.timex, self)
            resolution_result.future_value = self.__set_date_range_with_context(resolution_result.future_value)
            resolution_result.past_value = self.__set_date_range_with_context(resolution_result.past_value)
        return resolution_result

    def is_empty(self) -> bool:
        return self.year == Constants.INVALID_YEAR

    def __set_date_with_context(self, original_date, year=-1) -> datetime:
        if not DateUtils.is_valid_datetime(original_date):
            return original_date
        value = DateUtils.safe_create_from_min_value(year=self.year if year == -1 else year, month=original_date.month, day=original_date.day)
        return value

    def __set_date_range_with_context(self, original_date_range):
        start_date = self.__set_date_with_context(original_date_range[0])
        end_date = self.__set_date_with_context(original_date_range[1])
        result = {
            TimeTypeConstants.START_DATE: start_date,
            TimeTypeConstants.END_DATE: end_date
        }

        return result

    # This method is to ensure the year of begin date is same with the end date in no year situation.
    def sync_year(self, pr1, pr2):
        if self.is_empty():
            if DateUtils.is_Feb_29th_datetime(pr1.value.future_value):
                future_year = pr1.value.future_value.year
                past_year = pr1.value.past_value.year
                pr2.value = self.sync_year_resolution(pr2.value, future_year, past_year)
            elif DateUtils.is_Feb_29th_datetime(pr2.value.future_value):
                future_year = pr2.value.future_value.year
                past_year = pr2.value.past_value.year
                pr1.value = self.sync_year_resolution(pr1.value, future_year, past_year)
        return pr1, pr2

    def sync_year_resolution(self, resolution_result, future_year, past_year):
        resolution_result.future_value = self.__set_date_with_context(resolution_result.future_value, future_year)
        resolution_result.past_value = self.__set_date_with_context(resolution_result.past_value, past_year)
        return resolution_result