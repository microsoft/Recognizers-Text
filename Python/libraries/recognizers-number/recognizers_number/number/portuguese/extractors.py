from typing import Pattern, List, NamedTuple
import regex

from recognizers_number.number.models import NumberMode, LongFormatMode
from recognizers_number.resources.portuguese_numeric import PortugueseNumeric
from recognizers_number.number.extractors import ReVal, BaseNumberExtractor, BasePercentageExtractor
from recognizers_number.number.constants import Constants

class PortugueseNumberExtractor(BaseNumberExtractor):
    @property
    def regexes(self) -> List[ReVal]:
        return self.__regexes

    @property
    def _extract_type(self) -> str:
        return Constants.SYS_NUM

    @property
    def _negative_number_terms(self) -> Pattern:
        return self.__negative_number_terms

    def __init__(self, mode: NumberMode = NumberMode.DEFAULT):
        self.__negative_number_terms = regex.compile(PortugueseNumeric.NegativeNumberTermsRegex)
        self.__regexes: List[ReVal] = list()
        cardinal_ex: PortugueseCardinalExtractor = None

        if mode is NumberMode.PURE_NUMBER:
            cardinal_ex = PortugueseCardinalExtractor(PortugueseNumeric.PlaceHolderPureNumber)
        elif mode is NumberMode.CURRENCY:
            self.__regexes.append(ReVal(re=PortugueseNumeric.CurrencyRegex, val='IntegerNum'))

        if cardinal_ex is None:
            cardinal_ex = PortugueseCardinalExtractor()

        self.__regexes.extend(cardinal_ex.regexes)

        fraction_ex = PortugueseFractionExtractor()
        self.__regexes.extend(fraction_ex.regexes)

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
                re=PortugueseNumeric.NumbersWithPlaceHolder(placeholder),
                val='IntegerNum'),
            ReVal(
                re=PortugueseNumeric.NumbersWithSuffix,
                val='IntegerNum'),
            ReVal(
                re=self._generate_format_regex(LongFormatMode.INTEGER_DOT, placeholder),
                val='IntegerNum'),
            ReVal(
                re=PortugueseNumeric.RoundNumberIntegerRegexWithLocks,
                val='IntegerNum'),
            ReVal(
                re=PortugueseNumeric.NumbersWithDozenSuffix,
                val='IntegerNum'),
            ReVal(
                re=PortugueseNumeric.AllIntRegexWithLocks,
                val='IntegerPor'),
            ReVal(
                re=PortugueseNumeric.AllIntRegexWithDozenSuffixLocks,
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
                re=PortugueseNumeric.DoubleDecimalPointRegex(placeholder),
                val='DoubleNum'),
            ReVal(
                re=PortugueseNumeric.DoubleWithoutIntegralRegex(placeholder),
                val='DoubleNum'),
            ReVal(
                re=PortugueseNumeric.DoubleWithMultiplierRegex,
                val='DoubleNum'),
            ReVal(
                re=PortugueseNumeric.DoubleWithRoundNumber,
                val='DoubleNum'),
            ReVal(
                re=PortugueseNumeric.DoubleAllFloatRegex,
                val='DoublePor'),
            ReVal(
                re=PortugueseNumeric.DoubleExponentialNotationRegex,
                val='DoublePow'),
            ReVal(
                re=PortugueseNumeric.DoubleCaretExponentialNotationRegex,
                val='DoublePow'),
            ReVal(
                re=self._generate_format_regex(LongFormatMode.DOUBLE_DOT_COMMA, placeholder),
                val='DoubleNum')
        ]

class PortugueseFractionExtractor(BaseNumberExtractor):
    @property
    def regexes(self) -> List[NamedTuple('re_val', [('re', Pattern), ('val', str)])]:
        return self.__regexes

    @property
    def _extract_type(self) -> str:
        return Constants.SYS_NUM_FRACTION

    def __init__(self):
        self.__regexes = [
            ReVal(
                re=PortugueseNumeric.FractionNotationWithSpacesRegex,
                val='FracNum'),
            ReVal(
                re=PortugueseNumeric.FractionNotationRegex,
                val='FracNum'),
            ReVal(
                re=PortugueseNumeric.FractionNounRegex,
                val='FracPor'),
            ReVal(
                re=PortugueseNumeric.FractionNounWithArticleRegex,
                val='FracPor'),
            ReVal(
                re=PortugueseNumeric.FractionPrepositionRegex,
                val='FracPor')
        ]

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
                re=PortugueseNumeric.OrdinalSuffixRegex,
                val='OrdinalNum'),
            ReVal(
                re=PortugueseNumeric.OrdinalEnglishRegex,
                val='OrdinalPor')
        ]

class PortuguesePercentageExtractor(BasePercentageExtractor):
    def __init__(self):
        super().__init__(PortugueseNumberExtractor(NumberMode.DEFAULT))

    def get_definitions(self) -> List[str]:
        return [
            PortugueseNumeric.NumberWithSuffixPercentage
        ]
