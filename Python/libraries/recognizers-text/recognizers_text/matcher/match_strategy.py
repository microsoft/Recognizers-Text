#  Copyright (c) Microsoft Corporation. All rights reserved.
#  Licensed under the MIT License.

from enum import Enum


class MatchStrategy(Enum):

    AcAutomaton = 0

    TrieTree = 1
