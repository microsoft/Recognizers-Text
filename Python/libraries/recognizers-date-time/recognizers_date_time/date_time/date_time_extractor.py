#  Copyright (c) Microsoft Corporation. All rights reserved.
#  Licensed under the MIT License.

from abc import abstractmethod
from typing import List
from recognizers_text.extractor import ExtractResult
from recognizers_text.extractor import Extractor


class DateTimeExtractor(Extractor):

    @abstractmethod
    def extract(self, text) -> List[ExtractResult]:
        raise NotImplementedError
