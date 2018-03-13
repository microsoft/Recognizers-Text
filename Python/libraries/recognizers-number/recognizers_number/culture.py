from recognizers_text.culture import BaseCultureInfo
from decimal import *

class CultureInfo(BaseCultureInfo):
    def format(self, value: object) -> str:
        result = str(value)
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
        return result