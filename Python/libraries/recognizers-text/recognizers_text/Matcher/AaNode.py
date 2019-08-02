from typing import TypeVar
from .Node import Node


class AaNode(Node):
    T = TypeVar('T')

    def __init__(self, c: [T] = [], depth: int = 0, parent=[T]):
        self.__word = c,
        self.__depth = depth,
        self.__parent = parent,
        self.__fail = 0

    def __getitem__(self, c: [T]):
        pass

    def __setitem__(self, c: [T], value):
        pass

    @property
    def word(self) -> [T]:
        return self.__word

    @word.setter
    def word(self, word) -> [T]:
        self.__word = word

    @property
    def depth(self) -> int:
        return self.__depth

    @depth.setter
    def depth(self, depth) -> int:
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

    @depth.setter
    def fail(self, fail):
        self.__fail = fail

    def get_enumerator(self):
        pass

    def to_string(self):
        pass
