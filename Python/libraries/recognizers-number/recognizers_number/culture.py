from recognizers_text.culture import BaseCultureInfo, Culture
from .number import LongFormatMode, LongFormatType

SUPPORTED_CULTURES = {
    Culture.English: LongFormatMode.DOUBLE_COMMA_DOT,
    Culture.Chinese: None,
    Culture.Spanish: LongFormatMode.DOUBLE_DOT_COMMA,
    Culture.Portuguese: LongFormatMode.DOUBLE_DOT_COMMA,
    Culture.French: LongFormatMode.DOUBLE_DOT_COMMA,
    Culture.Japanese: LongFormatMode.DOUBLE_COMMA_DOT
}


class CultureInfo(BaseCultureInfo):
    def format(self, value: object) -> str:
        result = str(value)
        result = result.replace('e', 'E')
        if '.' in result:
            result = result.rstrip('0').rstrip('.')
        if 'E-' in result:
            parts = result.split('E-')
            parts[1] = parts[1].rjust(2, '0')
            result = 'E-'.join(parts)
        if 'E+' in result:
            parts = result.split('E+')
            parts[0] = parts[0].rstrip('0')
            result = 'E+'.join(parts)
        long_format = SUPPORTED_CULTURES.get(self.code)
        if long_format:
            result = ''.join(
                map(lambda x: self.change_mark(x, long_format), result))
        return result

    def change_mark(self, source: str, long_format: LongFormatType) -> str:
        if source == '.':
            return long_format.decimals_mark
        elif source == ',':
            return long_format.thousands_mark
        return source
