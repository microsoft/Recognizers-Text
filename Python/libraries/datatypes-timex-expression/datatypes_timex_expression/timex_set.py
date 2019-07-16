from .timex import Timex


class TimexSet:
    timex: Timex

    def __init__(self, timex):
        self.timex = Timex(timex)
