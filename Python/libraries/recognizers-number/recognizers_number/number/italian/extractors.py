#  Copyright (c) Microsoft Corporation. All rights reserved.
#  Licensed under the MIT License.

from typing import Pattern, List, NamedTuple
import regex

from recognizers_text.utilities import RegExpUtility
from recognizers_number.number.models import NumberMode, LongFormatMode
from recognizers_number.resources import BaseNumbers
from recognizers_number.resources.italian_numeric import ItalianNumeric
from recognizers_number.number.extractors import ReVal, ReRe, BaseNumberExtractor, BasePercentageExtractor
from recognizers_number.number.constants import Constants


class ItalianNumberExtractor(BaseNumberExtractor):
    @property
    def regexes(self) -> List[ReVal]:
        return self.__regexes

    @property
    def ambiguity_filters_dict(self) -> List[ReRe]:
        return self.__ambiguity_filters_dict

    @property
    def _extract_type(self) -> str:
        return Constants.SYS_NUM

    @property
    def _negative_number_terms(self) -> Pattern:
        return self.__negative_number_terms

    def __init__(self, mode: NumberMode = NumberMode.DEFAULT):
        self.__negative_number_terms = RegExpUtility.get_safe_reg_exp(
            ItalianNumeric.NegativeNumberTermsRegex)
        self.__regexes: List[ReVal] = list()
        cardinal_ex: ItalianCardinalExtractor = None

        if mode is NumberMode.PURE_NUMBER:
            cardinal_ex = ItalianCardinalExtractor(
                ItalianNumeric.PlaceHolderPureNumber)
        elif mode is NumberMode.CURRENCY:
            self.__regexes.append(ReVal(re=RegExpUtility.get_safe_reg_exp(
                ItalianNumeric.CurrencyRegex), val='IntegerNum'))

        if cardinal_ex is None:
            cardinal_ex = ItalianCardinalExtractor()

        self.__regexes.extend(cardinal_ex.regexes)

        fraction_ex = ItalianFractionExtractor(mode)
        self.__regexes.extend(fraction_ex.regexes)

        ambiguity_filters_dict: List[ReRe] = list()

        if mode != NumberMode.Unit:
            for key, value in ItalianNumeric.AmbiguityFiltersDict.items():
                ambiguity_filters_dict.append(ReRe(reKey=RegExpUtility.get_safe_reg_exp(key),
                                                   reVal=RegExpUtility.get_safe_reg_exp(value)))
        self.__ambiguity_filters_dict = ambiguity_filters_dict


class ItalianCardinalExtractor(BaseNumberExtractor):
    @property
    def regexes(self) -> List[ReVal]:
        return self.__regexes

    @property
    def _extract_type(self) -> str:
        return Constants.SYS_NUM_CARDINAL

    def __init__(self, placeholder: str = ItalianNumeric.PlaceHolderDefault):
        self.__regexes: List[ReVal] = list()

        # Add integer regexes
        integer_ex = ItalianIntegerExtractor(placeholder)
        self.__regexes.extend(integer_ex.regexes)

        # Add double regexes
        double_ex = ItalianDoubleExtractor(placeholder)
        self.__regexes.extend(double_ex.regexes)


class ItalianIntegerExtractor(BaseNumberExtractor):
    @property
    def regexes(self) -> List[NamedTuple('re_val', [('re', Pattern), ('val', str)])]:
        return self.__regexes

    @property
    def _extract_type(self) -> str:
        return Constants.SYS_NUM_INTEGER

    def __init__(self, placeholder: str = ItalianNumeric.PlaceHolderDefault):
        self.__regexes = [
            ReVal(
                re=RegExpUtility.get_safe_reg_exp(
                    ItalianNumeric.NumbersWithPlaceHolder(placeholder), regex.I),
                val='IntegerNum'),
            ReVal(
                re=RegExpUtility.get_safe_reg_exp(
                    ItalianNumeric.NumbersWithSuffix, regex.S),
                val='IntegerNum'),
            ReVal(
                re=RegExpUtility.get_safe_reg_exp(self._generate_format_regex(
                    LongFormatMode.INTEGER_DOT, placeholder), regex.V1),
                val='IntegerNum'),
            ReVal(
                re=RegExpUtility.get_safe_reg_exp(self._generate_format_regex(
                    LongFormatMode.INTEGER_BLANK, placeholder)),
                val='IntegerNum'),
            ReVal(
                re=RegExpUtility.get_safe_reg_exp(self._generate_format_regex(
                    LongFormatMode.INTEGER_NO_BREAK_SPACE, placeholder)),
                val='IntegerNum'),
            ReVal(
                re=RegExpUtility.get_safe_reg_exp(
                    ItalianNumeric.RoundNumberIntegerRegexWithLocks),
                val='IntegerNum'),
            ReVal(
                re=RegExpUtility.get_safe_reg_exp(
                    ItalianNumeric.NumbersWithDozenSuffix),
                val='IntegerNum'),
            ReVal(
                re=RegExpUtility.get_safe_reg_exp(
                    ItalianNumeric.AllIntRegexWithLocks),
                val=f'Integer{ItalianNumeric.LangMarker}'),
            ReVal(
                re=RegExpUtility.get_safe_reg_exp(
                    ItalianNumeric.AllIntRegexWithDozenSuffixLocks),
                val=f'Integer{ItalianNumeric.LangMarker}')
        ]


class ItalianDoubleExtractor(BaseNumberExtractor):
    @property
    def regexes(self) -> List[NamedTuple('re_val', [('re', Pattern), ('val', str)])]:
        return self.__regexes

    @property
    def _extract_type(self) -> str:
        return Constants.SYS_NUM_DOUBLE

    def __init__(self, placeholder):
        self.__regexes = [
            ReVal(
                re=RegExpUtility.get_safe_reg_exp(
                    ItalianNumeric.DoubleDecimalPointRegex(placeholder)),
                val='DoubleNum'),
            ReVal(
                re=RegExpUtility.get_safe_reg_exp(
                    ItalianNumeric.DoubleWithoutIntegralRegex(placeholder)),
                val='DoubleNum'),
            ReVal(
                re=RegExpUtility.get_safe_reg_exp(self._generate_format_regex(
                    LongFormatMode.DOUBLE_DOT_COMMA, placeholder)),
                val='DoubleNum'),
            ReVal(
                re=RegExpUtility.get_safe_reg_exp(self._generate_format_regex(
                    LongFormatMode.DOUBLE_NO_BREAK_SPACE_COMMA, placeholder)),
                val='DoubleNum'),
            ReVal(
                re=ItalianNumeric.DoubleWithMultiplierRegex,
                val='DoubleNum'),
            ReVal(
                re=RegExpUtility.get_safe_reg_exp(
                    ItalianNumeric.DoubleWithRoundNumber),
                val='DoubleNum'),
            ReVal(
                re=RegExpUtility.get_safe_reg_exp(
                    ItalianNumeric.DoubleAllFloatRegex),
                val=f'Double{ItalianNumeric.LangMarker}'),
            ReVal(
                re=RegExpUtility.get_safe_reg_exp(
                    ItalianNumeric.DoubleExponentialNotationRegex),
                val='DoublePow'),
            ReVal(
                re=RegExpUtility.get_safe_reg_exp(
                    ItalianNumeric.DoubleCaretExponentialNotationRegex),
                val='DoublePow')
        ]


class ItalianFractionExtractor(BaseNumberExtractor):
    @property
    def regexes(self) -> List[NamedTuple('re_val', [('re', Pattern), ('val', str)])]:
        return self.__regexes

    @property
    def _extract_type(self) -> str:
        return Constants.SYS_NUM_FRACTION

    def __init__(self, mode):
        self.__regexes = [
            ReVal(
                re=RegExpUtility.get_safe_reg_exp(
                    ItalianNumeric.FractionNotationWithSpacesRegex),
                val='FracNum'),
            ReVal(
                re=RegExpUtility.get_safe_reg_exp(
                    ItalianNumeric.FractionNotationRegex),
                val='FracNum'),
            ReVal(
                re=RegExpUtility.get_safe_reg_exp(
                    ItalianNumeric.FractionNounRegex),
                val=f'Frac{ItalianNumeric.LangMarker}'),
            ReVal(
                re=RegExpUtility.get_safe_reg_exp(
                    ItalianNumeric.FractionNounWithArticleRegex),
                val=f'Frac{ItalianNumeric.LangMarker}')
        ]

        if mode != NumberMode.Unit:
            self.__regexes.append(
                ReVal(
                    re=RegExpUtility.get_safe_reg_exp(
                        ItalianNumeric.FractionPrepositionRegex),
                    val=f'Frac{ItalianNumeric.LangMarker}'))


class ItalianOrdinalExtractor(BaseNumberExtractor):
    @property
    def regexes(self) -> List[NamedTuple('re_val', [('re', Pattern), ('val', str)])]:
        return self.__regexes

    @property
    def _extract_type(self) -> str:
        return Constants.SYS_NUM_ORDINAL

    def __init__(self):
        self.__regexes = [
            ReVal(
                re=RegExpUtility.get_safe_reg_exp(
                    ItalianNumeric.OrdinalSuffixRegex),
                val='OrdinalNum'),
            ReVal(
                re=RegExpUtility.get_safe_reg_exp(
                    ItalianNumeric.OrdinalItalianRegex),
                val=f'Ord{ItalianNumeric.LangMarker}')
        ]


class ItalianPercentageExtractor(BasePercentageExtractor):
    def __init__(self):
        super().__init__(ItalianNumberExtractor(NumberMode.DEFAULT))

    def get_definitions(self) -> List[str]:
        return [
            ItalianNumeric.NumberWithSuffixPercentage,
            ItalianNumeric.NumberWithPrefixPercentage
        ]
