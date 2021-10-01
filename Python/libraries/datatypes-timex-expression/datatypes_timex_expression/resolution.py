#  Copyright (c) Microsoft Corporation. All rights reserved.
#  Licensed under the MIT License.

class Entry:
    timex: str
    type: str
    value: str
    start: str
    end: str


class Resolution:
    values: []

    def __init__(self):
        self.values = []
