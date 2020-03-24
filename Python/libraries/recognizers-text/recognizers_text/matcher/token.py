class Token(object):
    def __init__(self, start: int, length: int, text: str):
        self.__length = length
        self.__start = start
        self.__text = text

    @property
    def text(self) -> str:
        return self.__text

    @text.setter
    def text(self, text) -> None:
        self.__text = text

    @property
    def start(self) -> int:
        return self.__start

    @start.setter
    def start(self, start) -> None:
        self.__start = start

    @property
    def length(self) -> int:
        return self.__length

    @length.setter
    def length(self, length) -> None:
        self.__length = length

    @property
    def end(self) -> int:
        return self.start + self.length
