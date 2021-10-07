#  Copyright (c) Microsoft Corporation. All rights reserved.
#  Licensed under the MIT License.

from enum import IntFlag


class NumberOptions(IntFlag):
    NONE = 0
    PERCENTAGE_MODE = 1
    SUPPRESS_EXTENDED_TYPES = 2097152
    EXPERIMENTAL_MODE = 4194304
    ENABLE_PREVIEW = 8388608
