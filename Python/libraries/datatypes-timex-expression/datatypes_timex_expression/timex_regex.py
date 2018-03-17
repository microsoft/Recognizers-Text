
import re

class TimexRegex:

    timexRegex = {
        'date': [
            # date
            re.compile(r"^(?P<year>\d\d\d\d)-(?P<month>\d\d)-(?P<dayOfMonth>\d\d)$"),
            re.compile(r"^XXXX-WXX-(?P<dayOfWeek>\d)$"),
            re.compile(r"^XXXX-(?P<month>\d\d)-(?P<dayOfMonth>\d\d)$"),

            # daterange
            re.compile(r"^(?P<year>\d\d\d\d)$"),
            re.compile(r"^(?P<year>\d\d\d\d)-(?P<month>\d\d)$"),
            re.compile(r"^(?P<season>SP|SU|FA|WI)$"),
            re.compile(r"^(?P<year>\d\d\d\d)-(?P<season>SP|SU|FA|WI)$"),
            re.compile(r"^(?P<year>\d\d\d\d)-W(?P<weekOfYear>\d\d)$"),
            re.compile(r"^(?P<year>\d\d\d\d)-W(?P<weekOfYear>\d\d)-(?P<weekend>WE)$"),
            re.compile(r"^XXXX-(?P<month>\d\d)$"),
            re.compile(r"^XXXX-(?P<month>\d\d)-W(?P<weekOfMonth>\d\d)$"),
            re.compile(r"^XXXX-(?P<month>\d\d)-WXX-(?P<weekOfMonth>\d)-(?P<dayOfWeek>\d)$")        
        ],
        'time': [
            # time
            re.compile(r"^T(?P<hour>\d\d)$"),
            re.compile(r"^T(?P<hour>\d\d):(?P<minute>\d\d)$"),
            re.compile(r"^T(?P<hour>\d\d):(?P<minute>\d\d):(?P<second>\d\d)$"),

            # timerange
            re.compile(r"^T(?P<partOfDay>DT|NI|MO|AF|EV)$")
        ],
        'period': [
            re.compile(r"^P(?P<amount>\d*\.?\d+)(?P<dateUnit>Y|M|W|D)$"),
            re.compile(r"^PT(?P<amount>\d*\.?\d+)(?P<timeUnit>H|M|S)$")
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
        print('try_extract...')
        regexResult = regex.match(timex)
        if regexResult == None:
            return False
        groups = regexResult.groupdict()
        for k in groups:
            result[k] = groups[k]
        return True
