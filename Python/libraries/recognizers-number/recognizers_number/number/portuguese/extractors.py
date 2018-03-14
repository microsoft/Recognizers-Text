import regex
from typing import Pattern, List, NamedTuple

from recognizers_number.number.models import NumberMode, LongFormatMode
from recognizers_number.resources.portuguese_numeric import PortugueseNumeric
from recognizers_number.number.extractors import re_val, BaseNumberExtractor, BasePercentageExtractor
from recognizers_number.number.constants import Constants

class PortugueseNumberExtractor(BaseNumberExtractor):
    @property
    def regexes(self) -> List[re_val]:
        return self.__regexes

    @property
    def _extract_type(self) -> str:
        return Constants.SYS_NUM

    @property
    def _negative_number_terms(self) -> Pattern:
        return self.__negative_number_terms

    def __init__(self, mode: NumberMode=NumberMode.DEFAULT):
        self.__negative_number_terms=regex.compile(PortugueseNumeric.NegativeNumberTermsRegex)
        self.__regexes: List[re_val] = list()
        cardinal_ex:PortugueseCardinalExtractor=None

        if mode is NumberMode.PURE_NUMBER:
            cardinal_ex=PortugueseCardinalExtractor(PortugueseNumeric.PlaceHolderPureNumber)
        elif mode is NumberMode.CURRENCY:
            self.__regexes.append(re_val(re=PortugueseNumeric.CurrencyRegex, val="IntegerNum"))
        
        if cardinal_ex is None:
            cardinal_ex=PortugueseCardinalExtractor()

        self.__regexes.extend(cardinal_ex.regexes)
        
        fraction_ex=PortugueseFractionExtractor()
        self.__regexes.extend(fraction_ex.regexes)

class PortugueseCardinalExtractor(BaseNumberExtractor):
    @property
    def regexes(self) -> List[re_val]:
        return self.__regexes
        
    @property
    def _extract_type(self) -> str:
        return Constants.SYS_NUM_CARDINAL

    def __init__(self, placeholder: str=PortugueseNumeric.PlaceHolderDefault):
        self.__regexes: List[re_val] = list()

        # Add integer regexes
        integer_ex=PortugueseIntegerExtractor(placeholder)
        self.__regexes.extend(integer_ex.regexes)
            
        # Add double regexes
        double_ex=PortugueseDoubleExtractor(placeholder)
        self.__regexes.extend(double_ex.regexes)

class PortugueseIntegerExtractor(BaseNumberExtractor):
    @property
    def regexes(self) -> List[NamedTuple('re_val', [('re', Pattern), ('val', str)])]:
        return self.__regexes

    @property
    def _extract_type(self) -> str:
        return Constants.SYS_NUM_INTEGER

    def __init__(self, placeholder: str=PortugueseNumeric.PlaceHolderDefault):
        self.__regexes=[
            re_val(
                re=PortugueseNumeric.NumbersWithPlaceHolder(placeholder),
                val='IntegerNum'),
            re_val(
                re=PortugueseNumeric.NumbersWithSuffix,
                val='IntegerNum'),
            re_val(
                re=self._generate_format_regex(LongFormatMode.INTEGER_DOT, placeholder),
                val='IntegerNum'),
            re_val(
                re=PortugueseNumeric.RoundNumberIntegerRegexWithLocks,
                val='IntegerNum'),
            re_val(
                re=PortugueseNumeric.NumbersWithDozenSuffix,
                val='IntegerNum'),
            re_val(
                re=PortugueseNumeric.AllIntRegexWithLocks,
                val='IntegerPor'),
            re_val(
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
        self.__regexes=[
            re_val(
                re=PortugueseNumeric.DoubleDecimalPointRegex(placeholder),
                val='DoubleNum'),
            re_val(
                re=PortugueseNumeric.DoubleWithoutIntegralRegex(placeholder),
                val='DoubleNum'),
            re_val(
                re=PortugueseNumeric.DoubleWithMultiplierRegex,
                val='DoubleNum'),
            re_val(
                re=PortugueseNumeric.DoubleWithRoundNumber,
                val='DoubleNum'),
            re_val(
                re=PortugueseNumeric.DoubleAllFloatRegex,
                val='DoublePor'),
            re_val(
                re=PortugueseNumeric.DoubleExponentialNotationRegex,
                val='DoublePow'),
            re_val(
                re=PortugueseNumeric.DoubleCaretExponentialNotationRegex,
                val='DoublePow'),
            re_val(
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
        self.__regexes=[
            re_val(
                re=PortugueseNumeric.FractionNotationWithSpacesRegex,
                val='FracNum'),
            re_val(
                re=PortugueseNumeric.FractionNotationRegex,
                val='FracNum'),
            re_val(
                re=PortugueseNumeric.FractionNounRegex,
                val='FracPor'),
            re_val(
                re=PortugueseNumeric.FractionNounWithArticleRegex,
                val='FracPor'),
            re_val(
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
        self.__regexes=[
            re_val(
                re=PortugueseNumeric.OrdinalSuffixRegex,
                val='OrdinalNum'),
            re_val(
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