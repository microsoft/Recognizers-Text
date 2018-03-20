
import re

class TimexRegex:

    timexRegex = {
        'date': [
            # date
            re.compile(r"^(?P<year>\d\d\d\d)-(?P<month>\d\d)-(?P<day_of_month>\d\d)$"),
            re.compile(r"^XXXX-WXX-(?P<day_of_week>\d)$"),
            re.compile(r"^XXXX-(?P<month>\d\d)-(?P<day_of_month>\d\d)$"),

            # daterange
            re.compile(r"^(?P<year>\d\d\d\d)$"),
            re.compile(r"^(?P<year>\d\d\d\d)-(?P<month>\d\d)$"),
            re.compile(r"^(?P<season>SP|SU|FA|WI)$"),
            re.compile(r"^(?P<year>\d\d\d\d)-(?P<season>SP|SU|FA|WI)$"),
            re.compile(r"^(?P<year>\d\d\d\d)-W(?P<week_of_year>\d\d)$"),
            re.compile(r"^(?P<year>\d\d\d\d)-W(?P<week_of_year>\d\d)-(?P<weekend>WE)$"),
            re.compile(r"^XXXX-(?P<month>\d\d)$"),
            re.compile(r"^XXXX-(?P<month>\d\d)-W(?P<week_of_month>\d\d)$"),
            re.compile(r"^XXXX-(?P<month>\d\d)-WXX-(?P<week_of_month>\d)-(?P<day_of_week>\d)$")        
        ],
        'time': [
            # time
            re.compile(r"^T(?P<hour>\d\d)$"),
            re.compile(r"^T(?P<hour>\d\d):(?P<minute>\d\d)$"),
            re.compile(r"^T(?P<hour>\d\d):(?P<minute>\d\d):(?P<second>\d\d)$"),

            # timerange
            re.compile(r"^T(?P<part_of_day>DT|NI|MO|AF|EV)$")
        ],
        'period': [
            re.compile(r"^P(?P<amount>\d*\.?\d+)(?P<date_unit>Y|M|W|D)$"),
            re.compile(r"^PT(?P<amount>\d*\.?\d+)(?P<time_unit>H|M|S)$")
        ]
    }

    @staticmethod
    def extract(name, timex, result):
        for entry in TimexRegex.timexRegex[name]:
            if TimexRegex.try_extract(entry, timex, result):
                return True
        return False

    @staticmethod
    def try_extract(regex, timex, result):
        regexResult = regex.match(timex)
        if regexResult == None:
            return False
        groups = regexResult.groupdict()
        for k in groups:
            result[k] = groups[k]
        return True
