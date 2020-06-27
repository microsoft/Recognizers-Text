from abc import ABC, abstractmethod
from typing import Dict, Pattern, Optional, List
from decimal import Decimal, getcontext
import regex
from recognizers_text import ExtractResult, re
from recognizers_text.parser import Parser, ParseResult


class SequenceParser(Parser):
    def parse(self, source: ExtractResult) -> Optional[ParseResult]:
        res = ParseResult(source)
        res.resolution_str = res.text
        return res


class BaseIpParser(SequenceParser):
    def parse(self, ext_result: ExtractResult):
        result = ParseResult(ext_result)
        result.start = ext_result.start
        result.length = ext_result.length
        result.text = ext_result.text
        result.type = ext_result.type
        result.resolution_str = self.drop_leading_zeros(ext_result.text)
        result.data = ext_result.data
        return result

    @staticmethod
    def drop_leading_zeros(text):
        result = ''
        number = ''
        for i in range(0, len(text), 1):
            c = text[i]
            if c == '.' or c == ':':

                if number != '':
                    number = number if number == '0' else number.lstrip('0')
                    number = '0' if not number else number
                    result = result + number

                result = result + text[i]
                number = ''
            else:
                number = number + str(c)
                if i == len(text) - 1:
                    number = number if number == '0' else number.lstrip('0')
                    number = '0' if not number else number
                    result = result + number

        return result
