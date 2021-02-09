import re
import unicodedata
from typing import Pattern, Union, List, Match, Dict
import regex
from emoji import UNICODE_EMOJI
from multipledispatch import dispatch


class StringUtility:
    @staticmethod
    def is_emoji(letter):
        return letter in UNICODE_EMOJI["en"]

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


class ConditionalMatch:

    def __init__(self, match: Match, success: bool):
        self._match = match,
        self._success = success

    @property
    def match(self) -> Pattern:
        return self._match

    @match.setter
    def match(self, value):
        self._match = value

    @property
    def success(self) -> bool:
        return self._success

    @success.setter
    def success(self, value):
        self._success = value

    @property
    def index(self) -> int:
        return self.match[0].string.index(self.match[0].group())

    @property
    def length(self) -> int:
        return len(self.match[0].group()) or 0

    @dispatch()
    def group(self):
        return self.match[0].group()

    @dispatch(str)
    def group(self, grp):
        return self.match[0].group(grp)

    @property
    def value(self) -> str:
        return self.match[0].string

    @property
    def groups(self):
        return self.match[0].groups()

    def get_group(self, group: str, default_val: str = '') -> str:
        if self is None:
            return None
        return self.match[0].groupdict().get(group, default_val) or default_val


class RegExpUtility:
    @staticmethod
    def get_safe_reg_exp(source: str, flags: int = regex.I | regex.S) -> Pattern:
        return regex.compile(source, flags=flags)

    @staticmethod
    def get_group(match, group: str, default_val: str = '') -> str:
        if match is None:
            return None
        try:
            return match.groupdict().get(group, default_val) or default_val
        except:
            return match.match[0].groupdict().get(group, default_val) or default_val

    @staticmethod
    def get_group_list(match: Match, group: str) -> List[str]:
        return match.captures(group)

    @staticmethod
    def get_matches(regexp: Pattern, source: str) -> []:
        py_regex = StringUtility.remove_unicode_matches(regexp)
        matches = list(regex.finditer(py_regex, source))
        return list(filter(None, map(lambda m: m.group().lower(), matches)))

    @staticmethod
    def match_begin(regex: Pattern, text: str, trim: bool):
        match = regex.search(text)

        if match is None:
            return None

        str_before = text[0: text.index(match.group())]

        if trim:
            str_before = str_before.strip()

        return ConditionalMatch(match, match and (str.isspace(str_before) or str_before == ''))

    @staticmethod
    def match_end(regexp: Pattern, text: str, trim: bool):
        match = regex.search(regexp, text)

        if match is None:
            return None

        srt_after = text[text.index(match.group()) + (match.end() - match.start()):]

        if trim:
            srt_after = srt_after.strip()

        success = match and (str.isspace(srt_after) or srt_after == '')

        return ConditionalMatch(match, success)

    @staticmethod
    def is_exact_match(regex: Pattern, text: str, trim: bool):
        match = regex.match(text)

        length = len(text.strip()) if trim else len(text)

        return match and len(match.group()) == length

    @staticmethod
    def exact_match(regexp: Pattern, text: str, trim: bool):
        match = regexp.search(text)

        length = len(text.strip()) if trim else len(text)

        return ConditionalMatch(match, match and len(match.group()) == length)


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

    tokens = '(kB|K[Bb]?|M[BbM]?|G[Bb]?|B)'
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

    @staticmethod
    def remove_diacritics(query: str) -> str:
        if not query:
            return None

        # NFD indicates that a Unicode string is normalized using full canonical decomposition.
        chars = ''.join((c for c in unicodedata.normalize('NFD', query) if unicodedata.category(c) != 'Mn'))

        # NFC indicates that a Unicode string is normalized using full canonical decomposition,
        # followed by the replacement of sequences with their primary composites, if possible.
        return str(unicodedata.normalize('NFC', chars)).lower()


class DefinitionLoader:

    @staticmethod
    def load_ambiguity_filters(filters: Dict[str, str]) -> Dict[Pattern, Pattern]:

        ambiguity_filters_dict = dict()

        for k, v in filters.items():

            if not "null" == k:
                ambiguity_filters_dict[RegExpUtility.get_safe_reg_exp(k)] = RegExpUtility.get_safe_reg_exp(v)

        return ambiguity_filters_dict


def flatten(result):
    return [item for sublist in result for item in sublist]
