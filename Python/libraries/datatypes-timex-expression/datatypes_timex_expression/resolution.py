class Entry:
    timex: str
    type: str
    value: str
    start: str
    end: str


class Resolution:

    def __init__(self):
        self.values = [Entry]

    @property
    def values(self) -> []:
        return self.__values

    @values.setter
    def values(self, value):
        self.__values = value
