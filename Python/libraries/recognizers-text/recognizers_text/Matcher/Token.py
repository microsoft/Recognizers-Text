class Token(object):
    def __init__(self, s: int, l: int, t: str):
        self.__length = s,
        self.__start = l,
        self.__text = t

    @property
    def text(self) -> str:
        return self.__text

    @text.setter
    def text(self, text) -> str:
        self.__text = text

    @property
    def start(self) -> int:
        return self.__start

    @start.setter
    def start(self, start) -> int:
        self.__start = start

    @property
    def length(self) -> int:
        return self.__length

    @length.setter
    def length(self, length) -> int:
        self.__length = length

    @property
    def end(self) -> int:
        return self.start + self.length
