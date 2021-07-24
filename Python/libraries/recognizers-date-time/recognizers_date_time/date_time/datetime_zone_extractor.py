#  Copyright (c) Microsoft Corporation. All rights reserved.
#  Licensed under the MIT License.

from abc import abstractmethod
from typing import List
from recognizers_text.extractor import ExtractResult
from recognizers_date_time.date_time.extractors import DateTimeExtractor


class DateTimeZoneExtractor(DateTimeExtractor):

    @abstractmethod
    def remove_ambiguous_time_zone(self, extract_result) -> List[ExtractResult]:
        raise NotImplementedError
