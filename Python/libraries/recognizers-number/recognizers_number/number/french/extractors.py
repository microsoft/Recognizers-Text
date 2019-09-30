from typing import Pattern, List, NamedTuple
import regex

from recognizers_text.utilities import RegExpUtility
from recognizers_number.number.models import NumberMode, LongFormatMode
from recognizers_number.resources import BaseNumbers
from recognizers_number.resources.french_numeric import FrenchNumeric
from recognizers_number.number.extractors import ReVal, ReRe, BaseNumberExtractor, BasePercentageExtractor
from recognizers_number.number.constants import Constants


class FrenchNumberExtractor(BaseNumberExtractor):
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
            FrenchNumeric.NegativeNumberTermsRegex)
        self.__regexes: List[ReVal] = list()
        cardinal_ex: FrenchCardinalExtractor = None

        if mode is NumberMode.PURE_NUMBER:
            cardinal_ex = FrenchCardinalExtractor(
                FrenchNumeric.PlaceHolderPureNumber)
        elif mode is NumberMode.CURRENCY:
            self.__regexes.append(ReVal(re=RegExpUtility.get_safe_reg_exp(
                FrenchNumeric.CurrencyRegex), val='IntegerNum'))

        if cardinal_ex is None:
            cardinal_ex = FrenchCardinalExtractor()

        self.__regexes.extend(cardinal_ex.regexes)

        fraction_ex = FrenchFractionExtractor(mode)
        self.__regexes.extend(fraction_ex.regexes)

        ambiguity_filters_dict: List[ReRe] = list()

        if mode != NumberMode.Unit:
            for key, value in FrenchNumeric.AmbiguityFiltersDict.items():
                ambiguity_filters_dict.append(ReRe(reKey=RegExpUtility.get_safe_reg_exp(key),
                                                   reVal=RegExpUtility.get_safe_reg_exp(value)))
        self.__ambiguity_filters_dict = ambiguity_filters_dict


class FrenchCardinalExtractor(BaseNumberExtractor):
    @property
    def regexes(self) -> List[ReVal]:
        return self.__regexes

    @property
    def _extract_type(self) -> str:
        return Constants.SYS_NUM_CARDINAL

    def __init__(self, placeholder: str = FrenchNumeric.PlaceHolderDefault):
        self.__regexes: List[ReVal] = list()

        # Add integer regexes
        integer_ex = FrenchIntegerExtractor(placeholder)
        self.__regexes.extend(integer_ex.regexes)

        # Add double regexes
        double_ex = FrenchDoubleExtractor(placeholder)
        self.__regexes.extend(double_ex.regexes)


class FrenchIntegerExtractor(BaseNumberExtractor):
    @property
    def regexes(self) -> List[NamedTuple('re_val', [('re', Pattern), ('val', str)])]:
        return self.__regexes

    @property
    def _extract_type(self) -> str:
        return Constants.SYS_NUM_INTEGER

    def __init__(self, placeholder: str = FrenchNumeric.PlaceHolderDefault):
        self.__regexes = [
            ReVal(
                re=RegExpUtility.get_safe_reg_exp(
                    FrenchNumeric.NumbersWithPlaceHolder(placeholder), regex.I),
                val='IntegerNum'),
            ReVal(
                re=RegExpUtility.get_safe_reg_exp(
                    FrenchNumeric.NumbersWithSuffix, regex.S),
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
                    FrenchNumeric.RoundNumberIntegerRegexWithLocks),
                val='IntegerNum'),
            ReVal(
                re=RegExpUtility.get_safe_reg_exp(
                    FrenchNumeric.NumbersWithDozenSuffix),
                val='IntegerNum'),
            ReVal(
                re=RegExpUtility.get_safe_reg_exp(
                    FrenchNumeric.AllIntRegexWithLocks),
                val='IntegerFr'),
            ReVal(
                re=RegExpUtility.get_safe_reg_exp(
                    FrenchNumeric.AllIntRegexWithDozenSuffixLocks),
                val='IntegerFr')
        ]


class FrenchDoubleExtractor(BaseNumberExtractor):
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
                    FrenchNumeric.DoubleDecimalPointRegex(placeholder)),
                val='DoubleNum'),
            ReVal(
                re=RegExpUtility.get_safe_reg_exp(
                    FrenchNumeric.DoubleWithoutIntegralRegex(placeholder)),
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
                re=RegExpUtility.get_safe_reg_exp(
                    FrenchNumeric.DoubleWithMultiplierRegex),
                val='DoubleNum'),
            ReVal(
                re=RegExpUtility.get_safe_reg_exp(
                    FrenchNumeric.DoubleWithRoundNumber),
                val='DoubleNum'),
            ReVal(
                re=RegExpUtility.get_safe_reg_exp(
                    FrenchNumeric.DoubleAllFloatRegex),
                val='DoubleFr'),
            ReVal(
                re=RegExpUtility.get_safe_reg_exp(
                    FrenchNumeric.DoubleExponentialNotationRegex),
                val='DoublePow'),
            ReVal(
                re=RegExpUtility.get_safe_reg_exp(
                    FrenchNumeric.DoubleCaretExponentialNotationRegex),
                val='DoublePow')
        ]


class FrenchFractionExtractor(BaseNumberExtractor):
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
                    FrenchNumeric.FractionNotationWithSpacesRegex),
                val='FracNum'),
            ReVal(
                re=RegExpUtility.get_safe_reg_exp(
                    FrenchNumeric.FractionNotationRegex),
                val='FracNum'),
            ReVal(
                re=RegExpUtility.get_safe_reg_exp(
                    FrenchNumeric.FractionNounRegex),
                val='FracFr'),
            ReVal(
                re=RegExpUtility.get_safe_reg_exp(
                    FrenchNumeric.FractionNounWithArticleRegex),
                val='FracFr')
        ]

        if mode != NumberMode.Unit:
            self.__regexes.append(
                ReVal(
                    re=RegExpUtility.get_safe_reg_exp(
                        FrenchNumeric.FractionPrepositionRegex),
                    val='FracFr'))


class FrenchOrdinalExtractor(BaseNumberExtractor):
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
                    FrenchNumeric.OrdinalSuffixRegex),
                val='OrdinalNum'),
            ReVal(
                re=RegExpUtility.get_safe_reg_exp(
                    FrenchNumeric.OrdinalFrenchRegex),
                val='OrdFr')
        ]


class FrenchPercentageExtractor(BasePercentageExtractor):
    def __init__(self):
        super().__init__(FrenchNumberExtractor(NumberMode.DEFAULT))

    def get_definitions(self) -> List[str]:
        return [
            FrenchNumeric.NumberWithSuffixPercentage,
            FrenchNumeric.NumberWithPrefixPercentage
        ]
