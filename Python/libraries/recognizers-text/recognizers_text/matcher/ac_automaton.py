from .aa_node import AaNode
from .abstract_matcher import AbstractMatcher
from queue import Queue
from .match_result import MatchResult


class AcAutomaton(AbstractMatcher):

    def __init__(self):
        self.__root = AaNode()

    @property
    def root(self) -> AaNode():
        return self.__root

    def insert(self, value: [], _id: str) -> None:
        node = self.root
        i = 0

        for item in value:
            child = node[item]

            if child is None:
                node[item] = AaNode(item, i, node)
                child = node[item]

            node = child
            i += 1

        node.add_value(_id)

    def init(self, values: [], ids: []) -> None:
        self.batch_insert(values, ids)
        queue = Queue()
        queue.put(self.root)

        while any(queue):
            node = queue.get()

            if node.children is not None:
                for child in node:
                    queue.put(child)

            if node == self.root:
                self.root.fail = self.root
                continue

            fail = node.parent.fail

            while fail[node.word] is None & fail != self.root:
                fail = fail.fail

            node.fail = fail[node.word] if fail[node.word] is not None else self.root

            node.fail = self.root if node.fail == node else node.fail

        list(self.root)

    def find(self, query_text: []) -> []:
        node = self.root
        i = 0

        for c in query_text:
            while node[c] is None & node != self.root:
                node = node.fail

            node = node[c] if node[c] is not None else self.root

            t = node

            while t != self.root:
                t = t.fail
                if t.end:
                    yield MatchResult(i - t.depth, t.depth + 1, t.values)

            i += 1
