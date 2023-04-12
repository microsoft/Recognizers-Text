from typing import Dict, Pattern, List

from recognizers_text.utilities import RegExpUtility
from recognizers_text.culture import Culture
from recognizers_text.parser import ParseResult
from recognizers_number.culture import CultureInfo
from recognizers_number.number.parsers import NumberParserConfiguration
from recognizers_number.resources.default_numeric import DefaultNumeric


class DefaultNumberParserConfiguration(NumberParserConfiguration):
    @property
    def cardinal_number_map(self) -> Dict[str, int]:
        return {}

    @property
    def ordinal_number_map(self) -> Dict[str, int]:
        return {}

    @property
    def round_number_map(self) -> Dict[str, int]:
        return self._round_number_map

    @property
    def digital_number_regex(self) -> Pattern:
        return RegExpUtility.get_safe_reg_exp("")

    @property
    def fraction_marker_token(self) -> str:
        return ""

    @property
    def negative_number_sign_regex(self) -> Pattern:
        return RegExpUtility.get_safe_reg_exp("Jumanji")

    @property
    def half_a_dozen_regex(self) -> Pattern:
        return RegExpUtility.get_safe_reg_exp("")

    @property
    def half_a_dozen_text(self) -> str:
        return ""

    @property
    def word_separator_token(self) -> str:
        return ""

    @property
    def written_decimal_separator_texts(self) -> List[str]:
        return []

    @property
    def written_group_separator_texts(self) -> List[str]:
        return []

    @property
    def written_integer_separator_texts(self) -> List[str]:
        return []

    @property
    def written_fraction_separator_texts(self) -> List[str]:
        return []

    @property
    def non_standard_separator_variants(self) -> List[str]:
        return []

    @property
    def round_multiplier_regex(self) -> Pattern:
        return

    @property
    def culture_info(self):
        return self._culture_info

    @property
    def lang_marker(self) -> str:
        return self._lang_marker

    @property
    def non_decimal_separator_char(self) -> str:
        return self._non_decimal_separator_char

    @property
    def decimal_separator_char(self) -> str:
        return self._decimal_separator_char

    @property
    def is_multi_decimal_separator_culture(self) -> bool:
        return self._is_multi_decimal_separator_culture

    def __init__(self, culture_info=None):
        if culture_info is None:
            culture_info = CultureInfo(Culture.Default)

        self._culture_info = culture_info
        self._lang_marker = DefaultNumeric.LangMarker
        self._decimal_separator_char = DefaultNumeric.DecimalSeparatorChar
        self._non_decimal_separator_char = DefaultNumeric.NonDecimalSeparatorChar
        self._is_multi_decimal_separator_culture = DefaultNumeric.MultiDecimalSeparatorCulture
        self._round_number_map = DefaultNumeric.RoundNumberMap

    def normalize_token_set(self, tokens: List[str], context: ParseResult) -> List[str]:
        frac_words: List[str] = list()
        tokens_len = len(tokens)
        i = 0
        while i < tokens_len:
            if '-' in tokens[i]:
                splited_tokens = tokens[i].split('-')
                if len(splited_tokens) == 2 and splited_tokens[1] in self.ordinal_number_map:
                    frac_words.append(splited_tokens[0])
                    frac_words.append(splited_tokens[1])
                else:
                    frac_words.append(tokens[i])
            elif i < tokens_len - 2 and tokens[i + 1] == '-':
                if tokens[i + 2] in self.ordinal_number_map:
                    frac_words.append(tokens[i])
                    frac_words.append(tokens[i + 2])
                else:
                    frac_words.append(
                        tokens[i] + tokens[i + 1] + tokens[i + 2])
                i += 2
            else:
                frac_words.append(tokens[i])
            i += 1

        return frac_words

    def resolve_composite_number(self, number_str: str) -> int:
        if '-' in number_str:
            numbers = number_str.split('-')
            ret = 0
            for num in numbers:
                if num in self.ordinal_number_map:
                    ret += self.ordinal_number_map[num]
                elif num in self.cardinal_number_map:
                    ret += self.cardinal_number_map[num]
            return ret
        elif number_str in self.ordinal_number_map:
            return self.ordinal_number_map[number_str]
        elif number_str in self.cardinal_number_map:
            return self.cardinal_number_map[number_str]

        return 0
