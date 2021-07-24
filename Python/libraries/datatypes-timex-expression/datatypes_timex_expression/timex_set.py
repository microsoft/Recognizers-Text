#  Copyright (c) Microsoft Corporation. All rights reserved.
#  Licensed under the MIT License.

from .timex import Timex


class TimexSet:
    timex: Timex

    def __init__(self, timex):
        self.timex = Timex(timex)
