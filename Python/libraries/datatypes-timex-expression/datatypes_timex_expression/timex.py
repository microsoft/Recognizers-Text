
from .timex_parsing import TimexParsing

class Timex:
    
    def __init__(self, timex):
        TimexParsing.parse_string(timex, self)

