import regex
from typing import Pattern, List, NamedTuple

from recognizers_number.number.models import NumberMode, LongFormatMode
from recognizers_number.resources.french_numeric import FrenchNumeric
from recognizers_number.number.extractors import re_val, BaseNumberExtractor, BasePercentageExtractor
from recognizers_number.number.constants import Constants

class FrenchNumberExtractor(BaseNumberExtractor):
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
        self.__negative_number_terms=regex.compile(FrenchNumeric.NegativeNumberTermsRegex)
        self.__regexes: List[re_val] = list()
        cardinal_ex:FrenchCardinalExtractor=None

        if mode is NumberMode.PURE_NUMBER:
            cardinal_ex=FrenchCardinalExtractor(FrenchNumeric.PlaceHolderPureNumber)
        elif mode is NumberMode.CURRENCY:
            self.__regexes.append(re_val(re=FrenchNumeric.CurrencyRegex, val="IntegerNum"))
        
        if cardinal_ex is None:
            cardinal_ex=FrenchCardinalExtractor()

        self.__regexes.extend(cardinal_ex.regexes)
        
        fraction_ex=FrenchFractionExtractor()
        self.__regexes.extend(fraction_ex.regexes)

class FrenchCardinalExtractor(BaseNumberExtractor):
    @property
    def regexes(self) -> List[re_val]:
        return self.__regexes
        
    @property
    def _extract_type(self) -> str:
        return Constants.SYS_NUM_CARDINAL

    def __init__(self, placeholder: str=FrenchNumeric.PlaceHolderDefault):
        self.__regexes: List[re_val] = list()

        # Add integer regexes
        integer_ex=FrenchIntegerExtractor(placeholder)
        self.__regexes.extend(integer_ex.regexes)
            
        # Add double regexes
        double_ex=FrenchDoubleExtractor(placeholder)
        self.__regexes.extend(double_ex.regexes)

class FrenchIntegerExtractor(BaseNumberExtractor):
    @property
    def regexes(self) -> List[NamedTuple('re_val', [('re', Pattern), ('val', str)])]:
        return self.__regexes

    @property
    def _extract_type(self) -> str:
        return Constants.SYS_NUM_INTEGER

    def __init__(self, placeholder: str=FrenchNumeric.PlaceHolderDefault):
        self.__regexes=[
            re_val(
                re=FrenchNumeric.NumbersWithPlaceHolder(placeholder),
                val='IntegerNum'),
            re_val(
                re=FrenchNumeric.NumbersWithSuffix,
                val='IntegerNum'),
            re_val(
                re=self._generate_format_regex(LongFormatMode.INTEGER_DOT, placeholder),
                val='IntegerNum'),
            re_val(
                re=FrenchNumeric.RoundNumberIntegerRegexWithLocks,
                val='IntegerNum'),
            re_val(
                re=FrenchNumeric.NumbersWithDozenSuffix,
                val='IntegerNum'),
            re_val(
                re=FrenchNumeric.AllIntRegexWithLocks,
                val='IntegerFr'),
            re_val(
                re=FrenchNumeric.AllIntRegexWithDozenSuffixLocks,
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
        self.__regexes=[
            re_val(
                re=FrenchNumeric.DoubleDecimalPointRegex(placeholder),
                val='DoubleNum'),
            re_val(
                re=FrenchNumeric.DoubleWithoutIntegralRegex(placeholder),
                val='DoubleNum'),
            re_val(
                re=self._generate_format_regex(LongFormatMode.DOUBLE_COMMA_DOT, placeholder),
                val='DoubleNum'),
            re_val(
                re=FrenchNumeric.DoubleWithMultiplierRegex,
                val='DoubleNum'),
            re_val(
                re=FrenchNumeric.DoubleWithRoundNumber,
                val='DoubleNum'),
            re_val(
                re=FrenchNumeric.DoubleAllFloatRegex,
                val='DoubleFr'),
            re_val(
                re=FrenchNumeric.DoubleExponentialNotationRegex,
                val='DoublePow'),
            re_val(
                re=FrenchNumeric.DoubleCaretExponentialNotationRegex,
                val='DoublePow')
        ]

class FrenchFractionExtractor(BaseNumberExtractor):
    @property
    def regexes(self) -> List[NamedTuple('re_val', [('re', Pattern), ('val', str)])]:
        return self.__regexes

    @property
    def _extract_type(self) -> str:
        return Constants.SYS_NUM_FRACTION

    def __init__(self):
        self.__regexes=[
            re_val(
                re=FrenchNumeric.FractionNotationWithSpacesRegex,
                val='FracNum'),
            re_val(
                re=FrenchNumeric.FractionNotationRegex,
                val='FracNum'),
            re_val(
                re=FrenchNumeric.FractionNounRegex,
                val='FracFr'),
            re_val(
                re=FrenchNumeric.FractionNounWithArticleRegex,
                val='FracFr'),
            re_val(
                re=FrenchNumeric.FractionPrepositionRegex,
                val='FracFr')
        ]

class FrenchOrdinalExtractor(BaseNumberExtractor):
    @property
    def regexes(self) -> List[NamedTuple('re_val', [('re', Pattern), ('val', str)])]:
        return self.__regexes

    @property
    def _extract_type(self) -> str:
        return Constants.SYS_NUM_ORDINAL

    def __init__(self):
        self.__regexes=[
            re_val(
                re=FrenchNumeric.OrdinalSuffixRegex,
                val='OrdinalNum'),
            re_val(
                re=FrenchNumeric.OrdinalFrenchRegex,
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