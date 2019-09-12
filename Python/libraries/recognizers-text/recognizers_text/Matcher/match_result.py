class MatchResult:

    def __init__(self, start: int = 0, length: int = 0, ids: [] = []):
        self.__length = length
        self.__start = start
        self.__canonical_values = ids
        self.__text = ''

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

    @property
    def canonical_values(self) -> []:
        return self.__canonical_values

    @canonical_values.setter
    def canonical_values(self, canonical_values: []) -> []:
        self.__canonical_values: [] = canonical_values
