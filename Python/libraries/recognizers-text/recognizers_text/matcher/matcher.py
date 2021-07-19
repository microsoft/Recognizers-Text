#  Copyright (c) Microsoft Corporation. All rights reserved.
#  Licensed under the MIT License.

from abc import ABC, abstractmethod


class Matcher(ABC):

    @abstractmethod
    def init(self, values: [], ids: []) -> None:
        raise NotImplementedError

    @abstractmethod
    def find(self, query_text: []) -> []:
        raise NotImplementedError
