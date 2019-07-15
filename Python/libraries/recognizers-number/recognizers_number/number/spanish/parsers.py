from typing import Dict, Pattern, List
import regex

from recognizers_text.utilities import RegExpUtility
from recognizers_text.culture import Culture
from recognizers_text.parser import ParseResult
from recognizers_number.culture import CultureInfo
from recognizers_number.number.parsers import NumberParserConfiguration
from recognizers_number.resources.spanish_numeric import SpanishNumeric


class SpanishNumberParserConfiguration(NumberParserConfiguration):
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
            culture_info = CultureInfo(Culture.Spanish)

        self._culture_info = culture_info
        self._lang_marker = SpanishNumeric.LangMarker
        self._decimal_separator_char = SpanishNumeric.DecimalSeparatorChar
        self._fraction_marker_token = SpanishNumeric.FractionMarkerToken
        self._non_decimal_separator_char = SpanishNumeric.NonDecimalSeparatorChar
        self._half_a_dozen_text = SpanishNumeric.HalfADozenText
        self._word_separator_token = SpanishNumeric.WordSeparatorToken

        self._written_decimal_separator_texts = SpanishNumeric.WrittenDecimalSeparatorTexts
        self._written_group_separator_texts = SpanishNumeric.WrittenGroupSeparatorTexts
        self._written_integer_separator_texts = SpanishNumeric.WrittenIntegerSeparatorTexts
        self._written_fraction_separator_texts = SpanishNumeric.WrittenFractionSeparatorTexts

        ordinal_number_map: Dict[str, int] = dict(
            SpanishNumeric.OrdinalNumberMap)
        for prefix_key in SpanishNumeric.PrefixCardinalMap:
            for suffix_key in SpanishNumeric.SuffixOrdinalMap:
                if not prefix_key+suffix_key in ordinal_number_map:
                    prefix_value = SpanishNumeric.PrefixCardinalMap[prefix_key]
                    suffix_value = SpanishNumeric.SuffixOrdinalMap[suffix_key]
                    ordinal_number_map[prefix_key +
                                       suffix_key] = prefix_value*suffix_value
        self._cardinal_number_map = SpanishNumeric.CardinalNumberMap
        self._ordinal_number_map = ordinal_number_map
        self._round_number_map = SpanishNumeric.RoundNumberMap
        self._negative_number_sign_regex = RegExpUtility.get_safe_reg_exp(
            SpanishNumeric.NegativeNumberSignRegex)
        self._half_a_dozen_regex = RegExpUtility.get_safe_reg_exp(
            SpanishNumeric.HalfADozenRegex)
        self._digital_number_regex = RegExpUtility.get_safe_reg_exp(
            SpanishNumeric.DigitalNumberRegex)

    def normalize_token_set(self, tokens: List[str], context: ParseResult) -> List[str]:
        result: List[str] = list()
        for token in tokens:
            temp_word = regex.sub(r'^s+', '', token)
            temp_word = regex.sub(r's+$', '', temp_word)
            if temp_word in self.ordinal_number_map:
                result.append(temp_word)
                continue
            if temp_word.endswith('avo') or temp_word.endswith('ava'):
                oringinal_temp_word = temp_word
                new_length = len(temp_word)
                temp_word = oringinal_temp_word[0:new_length-3]
                if temp_word in self.cardinal_number_map:
                    result.append(temp_word)
                    continue
                else:
                    temp_word = oringinal_temp_word[0:new_length-2]
                    if temp_word in self.cardinal_number_map:
                        result.append(temp_word)
                        continue
            result.append(token)

        return result

    def resolve_composite_number(self, number_str: str) -> int:
        if number_str in self.ordinal_number_map:
            return self.ordinal_number_map[number_str]
        if number_str in self.cardinal_number_map:
            return self.cardinal_number_map[number_str]

        value = 0
        final_value = 0
        str_builder = ''
        last_good_char = 0

        number_str_len = len(number_str)
        i = 0
        while i < number_str_len:
            str_builder += number_str[i]
            if (str_builder in self.cardinal_number_map and
                    self.cardinal_number_map[str_builder] > 0):
                last_good_char = i
                value = self.cardinal_number_map[str_builder]
            if i+1 == number_str_len:
                final_value += value
                str_builder = ''
                last_good_char += 1
                i = last_good_char
                value = 0
            else:
                i += 1

        return final_value
