import regex
from typing import Pattern, List, NamedTuple

from recognizers_text.utilities import RegExpUtility
from recognizers_number.number.models import NumberMode, LongFormatMode
from recognizers_number.resources import BaseNumbers
from recognizers_number.resources.portuguese_numeric import PortugueseNumeric
from recognizers_number.number.extractors import ReVal, ReRe, BaseNumberExtractor, BasePercentageExtractor
from recognizers_number.number.constants import Constants


class PortugueseNumberExtractor(BaseNumberExtractor):
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
            PortugueseNumeric.NegativeNumberTermsRegex)
        self.__regexes: List[ReVal] = list()
        cardinal_ex: PortugueseCardinalExtractor = None

        if mode is NumberMode.PURE_NUMBER:
            cardinal_ex = PortugueseCardinalExtractor(
                PortugueseNumeric.PlaceHolderPureNumber)
        elif mode is NumberMode.CURRENCY:
            self.__regexes.append(ReVal(re=RegExpUtility.get_safe_reg_exp(
                PortugueseNumeric.CurrencyRegex), val='IntegerNum'))

        if cardinal_ex is None:
            cardinal_ex = PortugueseCardinalExtractor()

        self.__regexes.extend(cardinal_ex.regexes)

        fraction_ex = PortugueseFractionExtractor(mode)
        self.__regexes.extend(fraction_ex.regexes)

        ambiguity_filters_dict: List[ReRe] = list()

        if mode != NumberMode.Unit:
            for key, value in PortugueseNumeric.AmbiguityFiltersDict.items():
                ambiguity_filters_dict.append(ReRe(reKey=RegExpUtility.get_safe_reg_exp(key),
                                                   reVal=RegExpUtility.get_safe_reg_exp(value)))
        self.__ambiguity_filters_dict = ambiguity_filters_dict


class PortugueseCardinalExtractor(BaseNumberExtractor):
    @property
    def regexes(self) -> List[ReVal]:
        return self.__regexes

    @property
    def _extract_type(self) -> str:
        return Constants.SYS_NUM_CARDINAL

    def __init__(self, placeholder: str = PortugueseNumeric.PlaceHolderDefault):
        self.__regexes: List[ReVal] = list()

        # Add integer regexes
        integer_ex = PortugueseIntegerExtractor(placeholder)
        self.__regexes.extend(integer_ex.regexes)

        # Add double regexes
        double_ex = PortugueseDoubleExtractor(placeholder)
        self.__regexes.extend(double_ex.regexes)


class PortugueseIntegerExtractor(BaseNumberExtractor):
    @property
    def regexes(self) -> List[NamedTuple('re_val', [('re', Pattern), ('val', str)])]:
        return self.__regexes

    @property
    def _extract_type(self) -> str:
        return Constants.SYS_NUM_INTEGER

    def __init__(self, placeholder: str = PortugueseNumeric.PlaceHolderDefault):
        self.__regexes = [
            ReVal(
                re=RegExpUtility.get_safe_reg_exp(
                    PortugueseNumeric.NumbersWithPlaceHolder(placeholder)),
                val='IntegerNum'),
            ReVal(
                re=RegExpUtility.get_safe_reg_exp(
                    PortugueseNumeric.NumbersWithSuffix, regex.S),
                val='IntegerNum'),
            ReVal(
                re=RegExpUtility.get_safe_reg_exp(self._generate_format_regex(
                    LongFormatMode.INTEGER_DOT, placeholder)),
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
                    PortugueseNumeric.RoundNumberIntegerRegexWithLocks),
                val='IntegerNum'),
            ReVal(
                re=RegExpUtility.get_safe_reg_exp(
                    PortugueseNumeric.NumbersWithDozenSuffix),
                val='IntegerNum'),
            ReVal(
                re=RegExpUtility.get_safe_reg_exp(
                    PortugueseNumeric.AllIntRegexWithLocks),
                val='IntegerPor'),
            ReVal(
                re=RegExpUtility.get_safe_reg_exp(
                    PortugueseNumeric.AllIntRegexWithDozenSuffixLocks),
                val='IntegerPor')
        ]


class PortugueseDoubleExtractor(BaseNumberExtractor):
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
                    PortugueseNumeric.DoubleDecimalPointRegex(placeholder)),
                val='DoubleNum'),
            ReVal(
                re=RegExpUtility.get_safe_reg_exp(
                    PortugueseNumeric.DoubleWithoutIntegralRegex(placeholder)),
                val='DoubleNum'),
            ReVal(
                re=RegExpUtility.get_safe_reg_exp(
                    PortugueseNumeric.DoubleWithMultiplierRegex),
                val='DoubleNum'),
            ReVal(
                re=RegExpUtility.get_safe_reg_exp(
                    PortugueseNumeric.DoubleWithRoundNumber),
                val='DoubleNum'),
            ReVal(
                re=RegExpUtility.get_safe_reg_exp(
                    PortugueseNumeric.DoubleAllFloatRegex),
                val='DoublePor'),
            ReVal(
                re=RegExpUtility.get_safe_reg_exp(
                    PortugueseNumeric.DoubleExponentialNotationRegex),
                val='DoublePow'),
            ReVal(
                re=RegExpUtility.get_safe_reg_exp(
                    PortugueseNumeric.DoubleCaretExponentialNotationRegex),
                val='DoublePow'),
            ReVal(
                re=RegExpUtility.get_safe_reg_exp(self._generate_format_regex(
                    LongFormatMode.DOUBLE_DOT_COMMA, placeholder)),
                val='DoubleNum'),
            ReVal(
                re=RegExpUtility.get_safe_reg_exp(self._generate_format_regex(
                    LongFormatMode.DOUBLE_NO_BREAK_SPACE_COMMA, placeholder)),
                val='DoubleNum')
        ]


class PortugueseFractionExtractor(BaseNumberExtractor):
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
                    PortugueseNumeric.FractionNotationWithSpacesRegex),
                val='FracNum'),
            ReVal(
                re=RegExpUtility.get_safe_reg_exp(
                    PortugueseNumeric.FractionNotationRegex),
                val='FracNum'),
            ReVal(
                re=RegExpUtility.get_safe_reg_exp(
                    PortugueseNumeric.FractionNounRegex),
                val='FracPor'),
            ReVal(
                re=RegExpUtility.get_safe_reg_exp(
                    PortugueseNumeric.FractionNounWithArticleRegex),
                val='FracPor')
        ]

        if mode != NumberMode.Unit:
            self.__regexes.append(ReVal(
                re=RegExpUtility.get_safe_reg_exp(
                    PortugueseNumeric.FractionPrepositionRegex),
                val='FracPor'))


class PortugueseOrdinalExtractor(BaseNumberExtractor):
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
                    PortugueseNumeric.OrdinalSuffixRegex),
                val='OrdinalNum'),
            ReVal(
                re=RegExpUtility.get_safe_reg_exp(
                    PortugueseNumeric.OrdinalEnglishRegex),
                val='OrdinalPor')
        ]


class PortuguesePercentageExtractor(BasePercentageExtractor):
    def __init__(self):
        super().__init__(PortugueseNumberExtractor(NumberMode.DEFAULT))

    def get_definitions(self) -> List[str]:
        return [
            PortugueseNumeric.NumberWithSuffixPercentage
        ]
