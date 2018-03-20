
from .timex_regex import TimexRegex

class TimexParsing:
    @staticmethod
    def parse_string(timex, obj):
        # a reference to the present
        if timex == 'PRESENT_REF':
            obj.now = True
        elif timex.startswith('P'):
            # duration
            TimexParsing.extract_duration(timex, obj)
        elif timex.startswith('(') and timex.endswith(')'):
            # range indicated with start and end dates and a duration
            TimexParsing.extract_start_end_range(timex, obj)
        else:
            # date and time and their respective ranges
            TimexParsing.extract_date_time(timex, obj)

    @staticmethod
    def extract_duration(s, obj):
        extracted = {}
        TimexRegex.extract('period', s, extracted)
        obj.assign_properties(extracted)

    @staticmethod
    def extract_start_end_range(s, obj):
        parts = s[1:len(s)-1].split(',')
        if len(parts) == 3:
            TimexParsing.extract_date_time(parts[0], obj)
            TimexParsing.extract_duration(parts[2], obj)

    @staticmethod
    def extract_date_time(s, obj):
        indexOfT = s.find('T')
        if indexOfT == -1:
            extracted = {}
            TimexRegex.extract('date', s, extracted)
            obj.assign_properties(extracted)
        else:
            extracted = {}
            TimexRegex.extract('date', s[0:indexOfT], extracted)
            TimexRegex.extract('time', s[indexOfT:], extracted)
            obj.assign_properties(extracted)
