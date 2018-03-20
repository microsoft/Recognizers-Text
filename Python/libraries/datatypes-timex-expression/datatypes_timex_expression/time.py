
class Time:

    def __init__(self, hour, minute, second):
        self.hour = hour
        self.minute = minute
        self.second = second

    @classmethod
    def fromSeconds(seconds):
        hour = floor(seconds / 3600000)
        minute = floor((seconds - (Hour * 3600000)) / 60000)
        second = (seconds - (Hour * 3600000) - (Minute * 60000)) / 1000
        return Time(hour, minute, second)
    
    def get_time(self):
        return (self.second * 1000) + (self.minute * 60000) + (self.hour * 3600000)
