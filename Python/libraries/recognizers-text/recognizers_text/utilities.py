from typing import Pattern, Match, Union, List
import regex

class RegExpUtility:
    @staticmethod
    def get_safe_reg_exp(source: str, flags: int = regex.I | regex.S) -> Pattern:
        return regex.compile(source, flags=flags)

    @staticmethod
    def get_group(match: Match, group: str, default_val: str = '') -> str:
        return match.groupdict().get(group, default_val) or default_val

    @staticmethod
    def get_group_list(match: Match, group: str) -> List[str]:
        return match.captures(group)

class FormatUtility:
    @staticmethod
    def preprocess(source: str, lower: bool = True) -> str:
        result: str = source.lower() if lower else source
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
        return result

    @staticmethod
    def float_or_int(source: Union[float, int]) -> Union[float, int]:
        return float(source) if source % 1 else int(source)
