from recognizers_text.culture import BaseCultureInfo, Culture
from .number import LongFormatMode
from decimal import *


supported_cultures = {
    Culture.English: LongFormatMode.DOUBLE_COMMA_DOT,
    Culture.Chinese: None,
    Culture.Spanish: LongFormatMode.DOUBLE_DOT_COMMA,
    Culture.French: LongFormatMode.DOUBLE_DOT_COMMA,
}

class CultureInfo(BaseCultureInfo):
    def format(self, value: object) -> str:
        result = str(value)
        result = result.replace('e', 'E')
        if '.' in result:
            result = result.rstrip('0').rstrip('.')
        if 'E-' in result:
            p = result.split('E-')
            p[1] = p[1].rjust(2, '0')
            result = 'E-'.join(p)
        if 'E+' in result:
            p = result.split('E+')
            p[0] = p[0].rstrip('0')
            result = 'E+'.join(p)
        long_format = supported_cultures.get(self.code)
        if long_format:
            result = long_format.thousands_mark.join(list(map(lambda x: x.replace(".", long_format.decimals_mark),  result.split(","))))
        return result