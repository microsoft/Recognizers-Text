
from .timex_regex import TimexRegex

class TimexParsing:
    @staticmethod
    def parse_string(timex, obj):
        # a reference to the present
        if timex == 'PRESENT_REF':
            obj.Now = True
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
        if 'dateUnit' in extracted:
            obj[{ Y: 'years', M: 'months', W: 'weeks', D: 'days' }[extracted.dateUnit]] = extracted.amount
        elif 'timeout' in extracted:
            obj[{ H: 'hours', M: 'minutes', S: 'seconds' }[extracted.timeUnit]] = extracted.amount

    @staticmethod
    def extract_start_end_range(s, obj):
        parts = s[1:s.Length - 2].split(',')

        if len(parts) == 3:
            TimexParsing.extract_date_time(parts[0], obj)
            TimexParsing.extract_duration(parts[2], obj)

    @staticmethod
    def extract_date_time(s, obj):
        indexOfT = s.find('T')
        if indexOfT == -1:
            TimexRegex.extract('date', s, obj)
        else:
            TimexRegex.extract('date', s[0:indexOfT], obj)
            TimexRegex.extract('time', s[indexOfT:], obj)
