from typing import Pattern, List, NamedTuple
import regex

from recognizers_text.utilities import RegExpUtility
from recognizers_number.number.models import NumberMode, LongFormatMode
from recognizers_number.resources import BaseNumbers
from recognizers_number.resources.german_numeric import GermanNumeric
from recognizers_number.number.extractors import ReVal, ReRe, BaseNumberExtractor, BasePercentageExtractor
from recognizers_number.number.constants import Constants


class GermanNumberExtractor(BaseNumberExtractor):
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
            GermanNumeric.NegativeNumberTermsRegex)
        self.__regexes: List[ReVal] = list()
        cardinal_ex: GermanCardinalExtractor = None

        if mode is NumberMode.PURE_NUMBER:
            cardinal_ex = GermanCardinalExtractor(
                GermanNumeric.PlaceHolderPureNumber)
        elif mode is NumberMode.CURRENCY:
            self.__regexes.append(ReVal(re=RegExpUtility.get_safe_reg_exp(
                GermanNumeric.CurrencyRegex), val='IntegerNum'))

        if cardinal_ex is None:
            cardinal_ex = GermanCardinalExtractor()

        self.__regexes.extend(cardinal_ex.regexes)

        fraction_ex = GermanFractionExtractor(mode)
        self.__regexes.extend(fraction_ex.regexes)

        ambiguity_filters_dict: List[ReRe] = list()

        if mode != NumberMode.Unit:
            for key, value in GermanNumeric.AmbiguityFiltersDict.items():
                ambiguity_filters_dict.append(ReRe(reKey=RegExpUtility.get_safe_reg_exp(key),
                                                   reVal=RegExpUtility.get_safe_reg_exp(value)))
        self.__ambiguity_filters_dict = ambiguity_filters_dict


class GermanCardinalExtractor(BaseNumberExtractor):
    @property
    def regexes(self) -> List[ReVal]:
        return self.__regexes

    @property
    def _extract_type(self) -> str:
        return Constants.SYS_NUM_CARDINAL

    def __init__(self, placeholder: str = GermanNumeric.PlaceHolderDefault):
        self.__regexes: List[ReVal] = list()

        # Add integer regexes
        integer_ex = GermanIntegerExtractor(placeholder)
        self.__regexes.extend(integer_ex.regexes)

        # Add double regexes
        double_ex = GermanDoubleExtractor(placeholder)
        self.__regexes.extend(double_ex.regexes)


class GermanIntegerExtractor(BaseNumberExtractor):
    @property
    def regexes(self) -> List[NamedTuple('re_val', [('re', Pattern), ('val', str)])]:
        return self.__regexes

    @property
    def _extract_type(self) -> str:
        return Constants.SYS_NUM_INTEGER

    def __init__(self, placeholder: str = GermanNumeric.PlaceHolderDefault):
        self.__regexes = [
            ReVal(
                re=RegExpUtility.get_safe_reg_exp(
                    GermanNumeric.NumbersWithPlaceHolder(placeholder)),
                val='IntegerNum'),
            ReVal(
                re=RegExpUtility.get_safe_reg_exp(
                    GermanNumeric.NumbersWithSuffix, regex.S),
                val='IntegerNum'),
            ReVal(
                re=RegExpUtility.get_safe_reg_exp(self._generate_format_regex(
                    LongFormatMode.INTEGER_COMMA, placeholder)),
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
                    GermanNumeric.RoundNumberIntegerRegexWithLocks),
                val='IntegerNum'),
            ReVal(
                re=RegExpUtility.get_safe_reg_exp(
                    GermanNumeric.NumbersWithDozenSuffix),
                val='IntegerNum'),
            ReVal(
                re=RegExpUtility.get_safe_reg_exp(
                    GermanNumeric.AllIntRegexWithLocks),
                val='IntegerGer'),
            ReVal(
                re=RegExpUtility.get_safe_reg_exp(
                    GermanNumeric.AllIntRegexWithDozenSuffixLocks),
                val='IntegerGer')
        ]


class GermanDoubleExtractor(BaseNumberExtractor):
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
                    GermanNumeric.DoubleDecimalPointRegex(placeholder)),
                val='DoubleNum'),
            ReVal(
                re=RegExpUtility.get_safe_reg_exp(
                    GermanNumeric.DoubleWithoutIntegralRegex(placeholder)),
                val='DoubleNum'),
            ReVal(
                re=RegExpUtility.get_safe_reg_exp(self._generate_format_regex(
                    LongFormatMode.DOUBLE_COMMA_DOT, placeholder)),
                val='DoubleNum'),
            ReVal(
                re=RegExpUtility.get_safe_reg_exp(self._generate_format_regex(
                    LongFormatMode.DOUBLE_NO_BREAK_SPACE_DOT, placeholder)),
                val='DoubleNum'),
            ReVal(
                re=RegExpUtility.get_safe_reg_exp(
                    GermanNumeric.DoubleWithMultiplierRegex, regex.S),
                val='DoubleNum'),
            ReVal(
                re=RegExpUtility.get_safe_reg_exp(
                    GermanNumeric.DoubleWithRoundNumber),
                val='DoubleNum'),
            ReVal(
                re=RegExpUtility.get_safe_reg_exp(
                    GermanNumeric.DoubleAllFloatRegex),
                val='DoubleGer'),
            ReVal(
                re=RegExpUtility.get_safe_reg_exp(
                    GermanNumeric.DoubleExponentialNotationRegex),
                val='DoublePow'),
            ReVal(
                re=RegExpUtility.get_safe_reg_exp(
                    GermanNumeric.DoubleCaretExponentialNotationRegex),
                val='DoublePow')
        ]


class GermanFractionExtractor(BaseNumberExtractor):
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
                    GermanNumeric.FractionNotationWithSpacesRegex),
                val='FracNum'),
            ReVal(
                re=RegExpUtility.get_safe_reg_exp(
                    GermanNumeric.FractionNotationRegex),
                val='FracNum'),
            ReVal(
                re=RegExpUtility.get_safe_reg_exp(
                    GermanNumeric.FractionNounRegex),
                val='FracGer'),
            ReVal(
                re=RegExpUtility.get_safe_reg_exp(
                    GermanNumeric.FractionNounWithArticleRegex),
                val='FracGer')
        ]

        if mode != NumberMode.Unit:
            self.__regexes.append(
                ReVal(
                    re=RegExpUtility.get_safe_reg_exp(
                        GermanNumeric.FractionPrepositionRegex),
                    val='FracGer'))


class GermanOrdinalExtractor(BaseNumberExtractor):
    @property
    def regexes(self) -> List[NamedTuple('re_val', [('re', Pattern), ('val', str)])]:
        return self.__regexes

    @property
    def _extract_type(self) -> str:
        return Constants.SYS_NUM_ORDINAL

    def __init__(self):
        self.__regexes = [
            ReVal(
                re=GermanNumeric.OrdinalSuffixRegex,
                val='OrdinalNum'),
            ReVal(
                re=GermanNumeric.OrdinalNumericRegex,
                val='OrdinalNum'),
            ReVal(
                re=GermanNumeric.OrdinalGermanRegex,
                val='OrdGer'),
            ReVal(
                re=GermanNumeric.OrdinalRoundNumberRegex,
                val='OrdGer')
        ]


class GermanPercentageExtractor(BasePercentageExtractor):
    def __init__(self):
        super().__init__(GermanNumberExtractor(NumberMode.DEFAULT))

    def get_definitions(self) -> List[str]:
        return [
            GermanNumeric.NumberWithSuffixPercentage,
            GermanNumeric.NumberWithPrefixPercentage
        ]
