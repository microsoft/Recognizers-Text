import regex
from typing import Pattern, List, NamedTuple

from recognizers_number.number.models import NumberMode, LongFormatMode
from recognizers_number.resources.spanish_numeric import SpanishNumeric
from recognizers_number.number.extractors import re_val, BaseNumberExtractor, BasePercentageExtractor
from recognizers_number.number.constants import Constants

class SpanishNumberExtractor(BaseNumberExtractor):
    @property
    def regexes(self) -> List[re_val]:
        return self.__regexes

    @property
    def _extract_type(self) -> str:
        return Constants.SYS_NUM

    def __init__(self, mode: NumberMode=NumberMode.DEFAULT):
        self.__regexes: List[re_val] = list()
        cardinal_ex:SpanishCardinalExtractor=None

        if mode is NumberMode.PURE_NUMBER:
            cardinal_ex=SpanishCardinalExtractor(SpanishNumeric.PlaceHolderPureNumber)
        elif mode is NumberMode.CURRENCY:
            self.__regexes.append(re_val(re=SpanishNumeric.CurrencyRegex, val="IntegerNum"))
        
        if cardinal_ex is None:
            cardinal_ex=SpanishCardinalExtractor()

        self.__regexes.extend(cardinal_ex.regexes)
        
        fraction_ex=SpanishFractionExtractor()
        self.__regexes.extend(fraction_ex.regexes)

class SpanishCardinalExtractor(BaseNumberExtractor):
    @property
    def regexes(self) -> List[re_val]:
        return self.__regexes
        
    @property
    def _extract_type(self) -> str:
        return Constants.SYS_NUM_CARDINAL

    def __init__(self, placeholder: str=SpanishNumeric.PlaceHolderDefault):
        self.__regexes: List[re_val] = list()

        # Add integer regexes
        integer_ex=SpanishIntegerExtractor(placeholder)
        self.__regexes.extend(integer_ex.regexes)
            
        # Add double regexes
        double_ex=SpanishDoubleExtractor(placeholder)
        self.__regexes.extend(double_ex.regexes)

class SpanishIntegerExtractor(BaseNumberExtractor):
    @property
    def regexes(self) -> List[NamedTuple('re_val', [('re', Pattern), ('val', str)])]:
        return self.__regexes

    @property
    def _extract_type(self) -> str:
        return Constants.SYS_NUM_INTEGER

    def __init__(self, placeholder: str=SpanishNumeric.PlaceHolderDefault):
        self.__regexes=[
            re_val(
                re=SpanishNumeric.NumbersWithPlaceHolder(placeholder),
                val='IntegerNum'),
            re_val(
                re=SpanishNumeric.NumbersWithSuffix,
                val='IntegerNum'),
            re_val(
                re=self._generate_format_regex(LongFormatMode.INTEGER_DOT, placeholder),
                val='IntegerNum'),
            re_val(
                re=SpanishNumeric.RoundNumberIntegerRegexWithLocks,
                val='IntegerNum'),
            re_val(
                re=SpanishNumeric.NumbersWithDozenSuffix,
                val='IntegerNum'),
            re_val(
                re=SpanishNumeric.AllIntRegexWithLocks,
                val='IntegerSpa'),
            re_val(
                re=SpanishNumeric.AllIntRegexWithDozenSuffixLocks,
                val='IntegerSpa')
        ]

class SpanishDoubleExtractor(BaseNumberExtractor):
    @property
    def regexes(self) -> List[NamedTuple('re_val', [('re', Pattern), ('val', str)])]:
        return self.__regexes

    @property
    def _extract_type(self) -> str:
        return Constants.SYS_NUM_DOUBLE

    def __init__(self, placeholder:str=SpanishNumeric.PlaceHolderDefault):
        self.__regexes=[
            re_val(
                re=SpanishNumeric.DoubleDecimalPointRegex(placeholder),
                val='DoubleNum'),
            re_val(
                re=SpanishNumeric.DoubleWithoutIntegralRegex(placeholder),
                val='DoubleNum'),
            re_val(
                re=SpanishNumeric.DoubleWithMultiplierRegex,
                val='DoubleNum'),
            re_val(
                re=SpanishNumeric.DoubleWithRoundNumber,
                val='DoubleNum'),
            re_val(
                re=SpanishNumeric.DoubleAllFloatRegex,
                val='DoubleSpa'),
            re_val(
                re=SpanishNumeric.DoubleExponentialNotationRegex,
                val='DoublePow'),
            re_val(
                re=SpanishNumeric.DoubleCaretExponentialNotationRegex,
                val='DoublePow'),
            re_val(
                re=self._generate_format_regex(LongFormatMode.DOUBLE_COMMA_DOT, placeholder),
                val='DoubleNum')
        ]

class SpanishFractionExtractor(BaseNumberExtractor):
    @property
    def regexes(self) -> List[NamedTuple('re_val', [('re', Pattern), ('val', str)])]:
        return self.__regexes

    @property
    def _extract_type(self) -> str:
        return Constants.SYS_NUM_FRACTION

    def __init__(self):
        self.__regexes=[
            re_val(
                re=SpanishNumeric.FractionNotationRegex,
                val='FracNum'),
            re_val(
                re=SpanishNumeric.FractionNotationWithSpacesRegex,
                val='FracNum'),
            re_val(
                re=SpanishNumeric.FractionNounRegex,
                val='FracSpa'),
            re_val(
                re=SpanishNumeric.FractionNounWithArticleRegex,
                val='FracSpa'),
            re_val(
                re=SpanishNumeric.FractionPrepositionRegex,
                val='FracSpa')
        ]

class SpanishOrdinalExtractor(BaseNumberExtractor):
    @property
    def regexes(self) -> List[NamedTuple('re_val', [('re', Pattern), ('val', str)])]:
        return self.__regexes

    @property
    def _extract_type(self) -> str:
        return Constants.SYS_NUM_ORDINAL

    def __init__(self):
        self.__regexes=[
            re_val(
                re=SpanishNumeric.OrdinalSuffixRegex,
                val='OrdinalNum'),
            re_val(
                re=SpanishNumeric.OrdinalNounRegex,
                val='OrdSpa')
        ]

class SpanishPercentageExtractor(BasePercentageExtractor):
    def __init__(self):
        super().__init__(SpanishNumberExtractor())

    def get_definitions(self) -> List[str]:
        return [
            SpanishNumeric.NumberWithPrefixPercentage
        ]
