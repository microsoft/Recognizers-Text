class ResolutionStartEnd:
    def __init__(self, _start=None, _end=None):
        self.start = _start
        self.end = _end

    @property
    def _start(self):
        return self.start

    @property
    def _end(self):
        return self.end
