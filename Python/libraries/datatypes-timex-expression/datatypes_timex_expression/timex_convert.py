#  Copyright (c) Microsoft Corporation. All rights reserved.
#  Licensed under the MIT License.

from .english import *


class TimexConvert:

    @staticmethod
    def convert_timex_to_string(timex):
        return convert_timex_to_string(timex)

    @staticmethod
    def convert_timex_set_to_string(timex):
        return convert_timex_set_to_string(timex)
