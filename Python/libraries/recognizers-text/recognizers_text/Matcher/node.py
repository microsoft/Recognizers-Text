
class Node(object):
    def __init__(self):
        self.__values = set(),
        self.__children = {}

    def end(self) -> bool:
        return self.values is not None & any(self.values)

    @property
    def values(self) -> set():
        return self.__values

    @values.setter
    def values(self, values):
        self.__values = values

    @property
    def children(self) -> {}:
        return self.__children

    @children.setter
    def children(self, children) -> {}:
        self.__children = children

    def get_enumerator(self):
        pass

    def add_value(self, value: str) -> None:
        pass
