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
