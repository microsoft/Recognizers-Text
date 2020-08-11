from .node import Node


class AaNode(Node):

    def __init__(self, c: [] = [], depth: int = 0, parent=[]):
        self.__word = c,
        self.__depth = depth,
        self.__parent = parent,
        self.__fail = 0

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
    def word(self) -> []:
        return self.__word

    @word.setter
    def word(self, word):
        self.__word = word

    @property
    def depth(self):
        return self.__depth

    @depth.setter
    def depth(self, depth):
        self.__depth = depth

    @property
    def parent(self):
        return self.__parent

    @parent.setter
    def parent(self, parent):
        self.__parent = parent

    @property
    def fail(self):
        return self.__fail

    @fail.setter
    def fail(self, fail):
        self.__fail = fail

    def to_string(self):
        return str(self.__word)
