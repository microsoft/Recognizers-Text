class Node:
    def __init__(self):
        self.__values = []
        self.__children = {}

    def __iter__(self):
        for item in self.children:
            yield item

    def __getitem__(self, item):
        if self.children is not None and item in self.children:
            return self.children[item]
        else:
            return None

    def __setitem__(self, key, value):
        if self.children is None:
            self.children = {}

        self.children[key] = value

    @property
    def end(self) -> bool:
        return self.values is not None and any(self.values)

    @property
    def values(self) -> []:
        return self.__values

    @values.setter
    def values(self, values):
        self.__values = values

    @property
    def children(self) -> {}:
        return self.__children

    @children.setter
    def children(self, children):
        self.__children = children

    def get_enumerator(self):
        return self.__children.values

    def add_value(self, value) -> None:
        if self.values is None:
            self.values = []

        self.values.append(value)
