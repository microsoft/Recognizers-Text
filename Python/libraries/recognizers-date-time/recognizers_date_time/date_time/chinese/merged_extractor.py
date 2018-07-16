from typing import List
from datetime import datetime
import regex

from recognizers_text import RegExpUtility

from ..base_merged import BaseMergedExtractor
from ..utilities import DateTimeOptions, ExtractResult
from .merged_extractor_config import ChineseMergedExtractorConfiguration

class ChineseMergedExtractor(BaseMergedExtractor):
    def __init__(self, options: DateTimeOptions):
        super().__init__(ChineseMergedExtractorConfiguration(), options)
        self.day_of_month_regex = RegExpUtility.get_safe_reg_exp('^\\d{1,2}号', regex.I)

    def extract(self, source: str, reference: datetime = None) -> List[ExtractResult]:
        if reference is None:
            reference = datetime.now()

        result: List[ExtractResult] = list()
        result = self.add_to(result, self.config.date_extractor.extract(source, reference), source)
        result = self.add_to(result, self.config.time_extractor.extract(source, reference), source)
        result = self.add_to(result, self.config.duration_extractor.extract(source, reference), source)
        result = self.add_to(result, self.config.date_period_extractor.extract(source, reference), source)
        result = self.add_to(result, self.config.date_time_extractor.extract(source, reference), source)
        result = self.add_to(result, self.config.time_period_extractor.extract(source, reference), source)
        result = self.add_to(result, self.config.date_time_period_extractor.extract(source, reference), source)
        result = self.add_to(result, self.config.set_extractor.extract(source, reference), source)
        result = self.add_to(result, self.config.holiday_extractor.extract(source, reference), source)
        result = self.add_mod(result, source)

        result = sorted(result, key=lambda x: x.start)

        return result

    def add_to(self, destination: List[ExtractResult], source: List[ExtractResult], text: str) -> List[ExtractResult]:
        for value in source:
            is_found = False
            rm_index = -1
            rm_len = 1

            for index, dest in enumerate(destination):
                if dest.overlap(value):
                    is_found = True
                    if value.length > dest.length:
                        rm_index = index
                        j = index + 1
                        while j < len(destination) and destination[j].overlap(value):
                            rm_len = rm_len + 1
                            j = j + 1
                    break

            if not is_found:
                destination.append(value)
            elif rm_index >= 0:
                del destination[rm_index:rm_index + rm_len]
                destination = self.move_overlap(destination, value)
                destination.insert(rm_index, value)
        return destination

    def move_overlap(self, destination: List[ExtractResult], source: ExtractResult) -> List[ExtractResult]:
        duplicated: List[int] = list()
        for index, dest in enumerate(destination):
            includes_text = dest.text in source.text
            same_boundary = source.start == dest.start or source.start + source.length == dest.start + dest.length
            if includes_text and same_boundary:
                duplicated.append(index)

        for index in duplicated:
            del destination[index]
        return destination

    def add_mod(self, ers: List[ExtractResult], source: str) -> List[ExtractResult]:
        return list(filter(lambda x: self.filter_item(x, source), ers))

    def filter_item(self, value: ExtractResult, source: str) -> bool:
        value_end = value.start + value.length

        if value_end != len(source):
            last_char = source[value_end]
            if value.text.endswith('周') and value_end < len(source) and last_char == '岁':
                return True

        if regex.search(self.day_of_month_regex, value.text):
            return True

        return True
