from typing import Dict, Pattern, List

from recognizers_text.utilities import RegExpUtility
from recognizers_text.culture import Culture
from recognizers_text.parser import ParseResult
from recognizers_number.culture import CultureInfo
from recognizers_number.number.parsers import NumberParserConfiguration
from recognizers_number.resources.english_numeric import EnglishNumeric

class EnglishNumberParserConfiguration(NumberParserConfiguration):
    @property
    def cardinal_number_map(self) -> Dict[str, int]:
        return self._cardinal_number_map

    @property
    def ordinal_number_map(self) -> Dict[str, int]:
        return self._ordinal_number_map

    @property
    def round_number_map(self) -> Dict[str, int]:
        return self._round_number_map

    @property
    def culture_info(self):
        return self._culture_info

    @property
    def digital_number_regex(self) -> Pattern:
        return self._digital_number_regex

    @property
    def fraction_marker_token(self) -> str:
        return self._fraction_marker_token

    @property
    def negative_number_sign_regex(self) -> Pattern:
        return self._negative_number_sign_regex

    @property
    def half_a_dozen_regex(self) -> Pattern:
        return self._half_a_dozen_regex

    @property
    def half_a_dozen_text(self) -> str:
        return self._half_a_dozen_text

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
    def word_separator_token(self) -> str:
        return self._word_separator_token

    @property
    def written_decimal_separator_texts(self) -> List[str]:
        return self._written_decimal_separator_texts

    @property
    def written_group_separator_texts(self) -> List[str]:
        return self._written_group_separator_texts

    @property
    def written_integer_separator_texts(self) -> List[str]:
        return self._written_integer_separator_texts

    @property
    def written_fraction_separator_texts(self) -> List[str]:
        return self._written_fraction_separator_texts

    def __init__(self, culture_info=None):
        if culture_info is None:
            culture_info = CultureInfo(Culture.English)

        self._culture_info = culture_info
        self._lang_marker = EnglishNumeric.LangMarker
        self._decimal_separator_char = EnglishNumeric.DecimalSeparatorChar
        self._fraction_marker_token = EnglishNumeric.FractionMarkerToken
        self._non_decimal_separator_char = EnglishNumeric.NonDecimalSeparatorChar
        self._half_a_dozen_text = EnglishNumeric.HalfADozenText
        self._word_separator_token = EnglishNumeric.WordSeparatorToken

        self._written_decimal_separator_texts = EnglishNumeric.WrittenDecimalSeparatorTexts
        self._written_group_separator_texts = EnglishNumeric.WrittenGroupSeparatorTexts
        self._written_integer_separator_texts = EnglishNumeric.WrittenIntegerSeparatorTexts
        self._written_fraction_separator_texts = EnglishNumeric.WrittenFractionSeparatorTexts

        self._cardinal_number_map = EnglishNumeric.CardinalNumberMap
        self._ordinal_number_map = EnglishNumeric.OrdinalNumberMap
        self._round_number_map = EnglishNumeric.RoundNumberMap
        self._negative_number_sign_regex = RegExpUtility.get_safe_reg_exp(EnglishNumeric.NegativeNumberSignRegex)
        self._half_a_dozen_regex = RegExpUtility.get_safe_reg_exp(EnglishNumeric.HalfADozenRegex)
        self._digital_number_regex = RegExpUtility.get_safe_reg_exp(EnglishNumeric.DigitalNumberRegex)

    def normalize_token_set(self, tokens: List[str], context: ParseResult) -> List[str]:
        frac_words: List[str] = list()
        tokens_len = len(tokens)
        i = 0
        while i < tokens_len:
            if i < tokens_len - 2 and tokens[i + 1] == '-':
                frac_words.append(tokens[i] + tokens[i + 1] + tokens[i + 2])
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
