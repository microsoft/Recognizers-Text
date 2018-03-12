from recognizers_text.culture import BaseCultureInfo

class CultureInfo(BaseCultureInfo):
    def format(self, value: object):
        value = super().format(value)
        return value