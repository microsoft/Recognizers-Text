from typing import Dict, Pattern, List
import regex

from recognizers_text.utilities import RegExpUtility
from recognizers_text.culture import Culture
from recognizers_text.parser import ParseResult
from recognizers_number.culture import CultureInfo
from recognizers_number.number.parsers import NumberParserConfiguration
from recognizers_number.resources.catalan_numeric import CatalanNumeric


class CatalanNumberParserConfiguration(NumberParserConfiguration):
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
            culture_info = CultureInfo(Culture.Catalan)

        self._culture_info = culture_info
        self._lang_marker = CatalanNumeric.LangMarker
        self._decimal_separator_char = CatalanNumeric.DecimalSeparatorChar
        self._fraction_marker_token = None
        self._non_decimal_separator_char = CatalanNumeric.NonDecimalSeparatorChar
        self._half_a_dozen_text = CatalanNumeric.HalfADozenText
        self._word_separator_token = CatalanNumeric.WordSeparatorToken

        self._written_decimal_separator_texts = CatalanNumeric.WrittenDecimalSeparatorTexts
        self._written_group_separator_texts = CatalanNumeric.WrittenGroupSeparatorTexts
        self._written_integer_separator_texts = CatalanNumeric.WrittenIntegerSeparatorTexts
        self._written_fraction_separator_texts = None
        self._non_standard_separator_variants = CatalanNumeric.NonStandardSeparatorVariants
        self._is_multi_decimal_separator_culture = CatalanNumeric.MultiDecimalSeparatorCulture

        ordinal_number_map: Dict[str, int] = dict(
            CatalanNumeric.OrdinalNumberMap)
        # for prefix_key in CatalanNumeric.PrefixCardinalMap:
        #     for suffix_key in CatalanNumeric.SuffixOrdinalMap:
        #         if not prefix_key+suffix_key in ordinal_number_map:
        #             prefix_value = CatalanNumeric.PrefixCardinalMap[prefix_key]
        #             suffix_value = CatalanNumeric.SuffixOrdinalMap[suffix_key]
        #             ordinal_number_map[prefix_key +
        #                                suffix_key] = prefix_value*suffix_value
        self._cardinal_number_map = CatalanNumeric.CardinalNumberMap
        self._ordinal_number_map = ordinal_number_map
        self._round_number_map = CatalanNumeric.RoundNumberMap
        self._negative_number_sign_regex = RegExpUtility.get_safe_reg_exp(
            CatalanNumeric.NegativeNumberSignRegex)
        self._half_a_dozen_regex = RegExpUtility.get_safe_reg_exp(
            CatalanNumeric.HalfADozenRegex)
        self._digital_number_regex = RegExpUtility.get_safe_reg_exp(
            CatalanNumeric.DigitalNumberRegex)
        self._round_multiplier_regex = None

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

        # The following piece of code is needed to compute the fraction pattern number+'y medio'
        # e.g. 'cinco y medio' ('five and a half') where the numerator is omitted in Catalan.
        # It works by inserting the numerator 'un' ('a') in the list result
        # so that the pattern is correctly processed.
        # if len(result) > 2:
        #     if result[len(result) - 1] == CatalanNumeric.OneHalfTokens[1] and \
        #             result[len(result) - 2] == CatalanNumeric.WordSeparatorToken:
        #         result[len(result) - 2] = CatalanNumeric.WrittenFractionSeparatorTexts[0]
        #         result.insert(len(result) - 1, CatalanNumeric.OneHalfTokens[0])

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
