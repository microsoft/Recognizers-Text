#  Copyright (c) Microsoft Corporation. All rights reserved.
#  Licensed under the MIT License.

from typing import Dict, Pattern, List

from recognizers_text.utilities import RegExpUtility
from recognizers_text.culture import Culture
from recognizers_text.parser import ParseResult
from recognizers_number.culture import CultureInfo
from recognizers_number.number.parsers import NumberParserConfiguration
from recognizers_number.resources.italian_numeric import ItalianNumeric


class ItalianNumberParserConfiguration(NumberParserConfiguration):
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

    @property
    def non_standard_separator_variants(self) -> List[str]:
        return self._non_standard_separator_variants

    @property
    def is_multi_decimal_separator_culture(self) -> bool:
        return self._is_multi_decimal_separator_culture

    @property
    def round_multiplier_regex(self) -> Pattern:
        return self._round_multiplier_regex

    def __init__(self, culture_info=None):
        if culture_info is None:
            culture_info = CultureInfo(Culture.Italian)

        self._culture_info = culture_info
        self._lang_marker = ItalianNumeric.LangMarker
        self._decimal_separator_char = ItalianNumeric.DecimalSeparatorChar
        self._fraction_marker_token = ItalianNumeric.FractionMarkerToken
        self._non_decimal_separator_char = ItalianNumeric.NonDecimalSeparatorChar
        self._half_a_dozen_text = ItalianNumeric.HalfADozenText
        self._word_separator_token = ItalianNumeric.WordSeparatorToken
        self._non_standard_separator_variants = []
        self._is_multi_decimal_separator_culture = ItalianNumeric.MultiDecimalSeparatorCulture

        self._written_decimal_separator_texts = ItalianNumeric.WrittenDecimalSeparatorTexts
        self._written_group_separator_texts = ItalianNumeric.WrittenGroupSeparatorTexts
        self._written_integer_separator_texts = ItalianNumeric.WrittenIntegerSeparatorTexts
        self._written_fraction_separator_texts = ItalianNumeric.WrittenFractionSeparatorTexts

        self._cardinal_number_map = ItalianNumeric.CardinalNumberMap
        self._ordinal_number_map = ItalianNumeric.OrdinalNumberMap
        self._round_number_map = ItalianNumeric.RoundNumberMap
        self._negative_number_sign_regex = RegExpUtility.get_safe_reg_exp(
            ItalianNumeric.NegativeNumberSignRegex)
        self._half_a_dozen_regex = RegExpUtility.get_safe_reg_exp(
            ItalianNumeric.HalfADozenRegex)
        self._digital_number_regex = RegExpUtility.get_safe_reg_exp(
            ItalianNumeric.DigitalNumberRegex)
        self._round_multiplier_regex = RegExpUtility.get_safe_reg_exp(
            ItalianNumeric.RoundMultiplierRegex)

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

        # The following piece of code is needed to compute the fraction pattern number+'e mezzo'
        # e.g. 'due e mezzo' ('two and a half') where the numerator is omitted in Italian.
        # It works by inserting the numerator 'un' ('a') in the list frac_words
        # so that the pattern is correctly processed.
        if len(frac_words) > 2:
            if frac_words[len(frac_words) - 1] == ItalianNumeric.OneHalfTokens[1] and \
                    frac_words[len(frac_words) - 2] == ItalianNumeric.WordSeparatorToken:
                frac_words[len(frac_words) - 2] = ItalianNumeric.WrittenFractionSeparatorTexts[0]
                frac_words.insert(len(frac_words) - 1, ItalianNumeric.OneHalfTokens[0])

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
