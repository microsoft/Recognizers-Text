class TimeResult:
    def __init__(self, hour: int, minute: int, second: int, low_bound: int = -1):
        self.hour = hour
        self.minute = minute
        self.second = second
        self.low_bound = low_bound
