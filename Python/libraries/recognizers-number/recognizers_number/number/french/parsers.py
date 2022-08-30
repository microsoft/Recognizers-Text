#  Copyright (c) Microsoft Corporation. All rights reserved.
#  Licensed under the MIT License.

from typing import Dict, Pattern, List
import regex

from recognizers_text.utilities import RegExpUtility
from recognizers_text.culture import Culture
from recognizers_text.parser import ParseResult
from recognizers_number.culture import CultureInfo
from recognizers_number.number.parsers import NumberParserConfiguration
from recognizers_number.resources.french_numeric import FrenchNumeric


class FrenchNumberParserConfiguration(NumberParserConfiguration):
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
            culture_info = CultureInfo(Culture.French)

        self._culture_info = culture_info
        self._lang_marker = FrenchNumeric.LangMarker
        self._decimal_separator_char = FrenchNumeric.DecimalSeparatorChar
        self._fraction_marker_token = FrenchNumeric.FractionMarkerToken
        self._non_decimal_separator_char = FrenchNumeric.NonDecimalSeparatorChar
        self._half_a_dozen_text = FrenchNumeric.HalfADozenText
        self._word_separator_token = FrenchNumeric.WordSeparatorToken
        self._non_standard_separator_variants = []
        self._is_multi_decimal_separator_culture = FrenchNumeric.MultiDecimalSeparatorCulture

        self._written_decimal_separator_texts = FrenchNumeric.WrittenDecimalSeparatorTexts
        self._written_group_separator_texts = FrenchNumeric.WrittenGroupSeparatorTexts
        self._written_integer_separator_texts = FrenchNumeric.WrittenIntegerSeparatorTexts
        self._written_fraction_separator_texts = FrenchNumeric.WrittenFractionSeparatorTexts

        self._cardinal_number_map = FrenchNumeric.CardinalNumberMap
        self._ordinal_number_map = FrenchNumeric.OrdinalNumberMap
        self._round_number_map = FrenchNumeric.RoundNumberMap
        self._negative_number_sign_regex = RegExpUtility.get_safe_reg_exp(
            FrenchNumeric.NegativeNumberSignRegex)
        self._half_a_dozen_regex = RegExpUtility.get_safe_reg_exp(
            FrenchNumeric.HalfADozenRegex)
        self._digital_number_regex = RegExpUtility.get_safe_reg_exp(
            FrenchNumeric.DigitalNumberRegex)
        self._round_multiplier_regex = RegExpUtility.get_safe_reg_exp(
            FrenchNumeric.RoundMultiplierRegex)

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

        # The following piece of code is needed to compute the fraction pattern number+'et demi'
        # e.g. 'deux et demi' ('two and a half') where the numerator is omitted in French.
        # It works by inserting the numerator 'un' ('a') in the list frac_words
        #
        if len(frac_words) > 2:
            if frac_words[len(frac_words) - 1] == FrenchNumeric.OneHalfTokens[1] and \
                    frac_words[len(frac_words) - 2] == FrenchNumeric.WordSeparatorToken:
                frac_words.insert(len(frac_words) - 1, FrenchNumeric.OneHalfTokens[0])

        return frac_words

    def resolve_composite_number(self, number_str: str) -> int:
        if number_str in self.ordinal_number_map:
            return self.ordinal_number_map[number_str]
        if number_str in self.cardinal_number_map:
            return self.cardinal_number_map[number_str]

        value = 0
        final_value = 0
        str_builder = ''
        last_good_char = 0
        i = 0
        while i < len(number_str):
            str_builder += number_str[i]
            if (str_builder in self.cardinal_number_map
                    and self.cardinal_number_map[str_builder] > value):
                last_good_char = i
                value = self.cardinal_number_map[str_builder]

            if (i + 1) == len(number_str):
                final_value += value
                str_builder = ''
                i = last_good_char
                last_good_char += 1
                value = 0
            i += 1

        return final_value
