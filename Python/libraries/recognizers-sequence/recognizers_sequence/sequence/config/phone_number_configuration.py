#  Copyright (c) Microsoft Corporation. All rights reserved.
#  Licensed under the MIT License.

from abc import ABC, abstractmethod


class PhoneNumberConfiguration(ABC):
    @property
    @abstractmethod
    def word_boundaries_regex(self) -> str:
        raise NotImplementedError

    @property
    @abstractmethod
    def non_word_boundaries_regex(self) -> str:
        raise NotImplementedError

    @property
    @abstractmethod
    def end_word_boundaries_regex(self) -> str:
        raise NotImplementedError

    @property
    @abstractmethod
    def colon_prefix_check_regex(self) -> str:
        raise NotImplementedError

    @property
    @abstractmethod
    def false_positive_prefix_regex(self) -> str:
        raise NotImplementedError

    @property
    @abstractmethod
    def forbidden_prefix_markers(self) -> str:
        raise NotImplementedError

    def __init__(self, options):
        self.options = options
