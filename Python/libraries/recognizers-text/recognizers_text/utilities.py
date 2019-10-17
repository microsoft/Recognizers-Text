import re
from typing import Pattern, Union, List, Match
import regex
from emoji import UNICODE_EMOJI


class StringUtility:
    @staticmethod
    def is_emoji(letter):
        return letter in UNICODE_EMOJI

    @staticmethod
    def remove_unicode_matches(string: Pattern):
        py_regex = re.sub('\\\\u.{4}[\\|\\\\]', '', string.pattern)
        return re.sub('\\\\u', '\\\\U', py_regex)

    @staticmethod
    def index_of(string, token, position):
        try:
            ret = string.index(token, position)
        except:
            ret = 1
        return ret


class RegExpUtility:
    @staticmethod
    def get_safe_reg_exp(source: str, flags: int = regex.I | regex.S) -> Pattern:
        return regex.compile(source, flags=flags)

    @staticmethod
    def get_group(match: Match, group: str, default_val: str = '') -> str:
        if match is None:
            return None
        return match.groupdict().get(group, default_val) or default_val

    @staticmethod
    def get_group_list(match: Match, group: str) -> List[str]:
        return match.captures(group)

    @staticmethod
    def get_matches(regexp: Pattern, source: str) -> []:
        py_regex = StringUtility.remove_unicode_matches(regexp)
        matches = list(regex.finditer(py_regex, source))
        return list(filter(None, map(lambda m: m.group().lower(), matches)))


class QueryProcessor:
    @staticmethod
    def preprocess(source: str, case_sensitive: bool = False, recode: bool = True) -> str:

        result: str = source

        if recode:
            result = result.replace('０', '0')
            result = result.replace('１', '1')
            result = result.replace('２', '2')
            result = result.replace('３', '3')
            result = result.replace('４', '4')
            result = result.replace('５', '5')
            result = result.replace('６', '6')
            result = result.replace('７', '7')
            result = result.replace('８', '8')
            result = result.replace('９', '9')
            result = result.replace('：', ':')
            result = result.replace('－', '-')
            result = result.replace('，', ',')
            result = result.replace('／', '/')
            result = result.replace('Ｇ', 'G')
            result = result.replace('Ｍ', 'M')
            result = result.replace('Ｔ', 'T')
            result = result.replace('Ｋ', 'K')
            result = result.replace('ｋ', 'k')
            result = result.replace('．', '.')
            result = result.replace('（', '(')
            result = result.replace('）', ')')
            result = result.replace('％', '%')
            result = result.replace('、', ',')

        if not case_sensitive:
            result = result.lower()
        else:
            result = QueryProcessor.to_lower_term_sensitive(result)

        return result

    tokens = '(kB|K[Bb]|K|M[Bb]|M|G[Bb]|G|B)'
    expression = f'(?<=(\\s|\\d))' + tokens + '\\b'
    special_tokens_regex = RegExpUtility.get_safe_reg_exp(expression, regex.S)

    @staticmethod
    def to_lower_term_sensitive(input_str: str) -> str:

        str_chars = list(input_str.lower())

        matches = QueryProcessor.special_tokens_regex.finditer(input_str)
        for match in matches:
            QueryProcessor.apply_reverse(
                match.start(), str_chars, match.group())

        return ''.join(str_chars)

    @staticmethod
    def apply_reverse(idx: int, string_chars, value: str):
        for i in range(len(value)):
            string_chars[idx + i] = value[i]

    @staticmethod
    def float_or_int(source: Union[float, int]) -> Union[float, int]:
        return float(source) if source % 1 else int(source)
