import regex
from typing import Pattern, List, NamedTuple

from recognizers_number.number.models import NumberMode, LongFormatMode
from recognizers_number.resources.english_numeric import EnglishNumeric
from recognizers_number.number.extractors import re_val, BaseNumberExtractor, BasePercentageExtractor
from recognizers_number.number.constants import Constants

class EnglishNumberExtractor(BaseNumberExtractor):
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
        self.__negative_number_terms=regex.compile(EnglishNumeric.NegativeNumberTermsRegex)
        self.__regexes: List[re_val] = list()
        cardinal_ex=None

        if mode is NumberMode.PURE_NUMBER:
            cardinal_ex=EnglishCardinalExtractor(EnglishNumeric.PlaceHolderPureNumber)
        elif mode is NumberMode.CURRENCY:
            self.__regexes.append(re_val(re=EnglishNumeric.CurrencyRegex, val="IntegerNum"))
        
        if cardinal_ex is None:
            cardinal_ex=EnglishCardinalExtractor()

        self.__regexes.extend(cardinal_ex.regexes)
        
        fraction_ex=EnglishFractionExtractor()
        self.__regexes.extend(fraction_ex.regexes)

class EnglishCardinalExtractor(BaseNumberExtractor):
    @property
    def regexes(self) -> List[re_val]:
        return self.__regexes
        
    @property
    def _extract_type(self) -> str:
        return Constants.SYS_NUM_CARDINAL

    @property
    def _negative_number_terms(self): pass

    def __init__(self, placeholder: str=EnglishNumeric.PlaceHolderDefault):
        self.__regexes: List[re_val] = list()

        # Add integer regexes
        integer_ex=EnglishIntegerExtractor(placeholder)
        self.__regexes.extend(integer_ex.regexes)
            
        # Add double regexes
        double_ex=EnglishDoubleExtractor(placeholder)
        self.__regexes.extend(double_ex.regexes)

class EnglishIntegerExtractor(BaseNumberExtractor):
    @property
    def regexes(self) -> List[NamedTuple('re_val', [('re', Pattern), ('val', str)])]:
        return self.__regexes

    @property
    def _extract_type(self) -> str:
        return Constants.SYS_NUM_INTEGER

    @property
    def _negative_number_terms(self): pass

    def __init__(self, placeholder: str=EnglishNumeric.PlaceHolderDefault):
        self.__regexes=[
            re_val(
                re=EnglishNumeric.NumbersWithPlaceHolder(placeholder),
                val='IntegerNum'),
            re_val(
                re=EnglishNumeric.NumbersWithSuffix,
                val='IntegerNum'),
            re_val(
                re=self._generate_format_regex(LongFormatMode.INTEGER_COMMA, placeholder),
                val='IntegerNum'),
            re_val(
                re=EnglishNumeric.RoundNumberIntegerRegexWithLocks,
                val='IntegerNum'),
            re_val(
                re=EnglishNumeric.NumbersWithDozenSuffix,
                val='IntegerNum'),
            re_val(
                re=EnglishNumeric.AllIntRegexWithLocks,
                val='IntegerEng'),
            re_val(
                re=EnglishNumeric.AllIntRegexWithDozenSuffixLocks,
                val='IntegerEng')
        ]

class EnglishDoubleExtractor(BaseNumberExtractor):
    @property
    def regexes(self) -> List[NamedTuple('re_val', [('re', Pattern), ('val', str)])]:
        return self.__regexes

    @property
    def _extract_type(self) -> str:
        return Constants.SYS_NUM_DOUBLE

    @property
    def _negative_number_terms(self): pass

    def __init__(self, placeholder):
        self.__regexes=[
            re_val(
                re=EnglishNumeric.DoubleDecimalPointRegex(placeholder),
                val='DoubleNum'),
            re_val(
                re=EnglishNumeric.DoubleWithoutIntegralRegex(placeholder),
                val='DoubleNum'),
            re_val(
                re=self._generate_format_regex(LongFormatMode.DOUBLE_COMMA_DOT, placeholder),
                val='DoubleNum'),
            re_val(
                re=EnglishNumeric.DoubleWithMultiplierRegex,
                val='DoubleNum'),
            re_val(
                re=EnglishNumeric.DoubleWithRoundNumber,
                val='DoubleNum'),
            re_val(
                re=EnglishNumeric.DoubleAllFloatRegex,
                val='DoubleEng'),
            re_val(
                re=EnglishNumeric.DoubleExponentialNotationRegex,
                val='DoublePow'),
            re_val(
                re=EnglishNumeric.DoubleCaretExponentialNotationRegex,
                val='DoublePow')
        ]

class EnglishFractionExtractor(BaseNumberExtractor):
    @property
    def regexes(self) -> List[NamedTuple('re_val', [('re', Pattern), ('val', str)])]:
        return self.__regexes

    @property
    def _extract_type(self) -> str:
        return Constants.SYS_NUM_FRACTION

    @property
    def _negative_number_terms(self): pass

    def __init__(self):
        self.__regexes=[
            re_val(
                re=EnglishNumeric.FractionNotationWithSpacesRegex,
                val='FracNum'),
            re_val(
                re=EnglishNumeric.FractionNotationRegex,
                val='FracNum'),
            re_val(
                re=EnglishNumeric.FractionNounRegex,
                val='FracEng'),
            re_val(
                re=EnglishNumeric.FractionNounWithArticleRegex,
                val='FracEng'),
            re_val(
                re=EnglishNumeric.FractionPrepositionRegex,
                val='FracEng')
        ]

class EnglishOrdinalExtractor(BaseNumberExtractor):
    @property
    def regexes(self) -> List[NamedTuple('re_val', [('re', Pattern), ('val', str)])]:
        return self.__regexes

    @property
    def _extract_type(self) -> str:
        return Constants.SYS_NUM_ORDINAL

    @property
    def _negative_number_terms(self): pass

    def __init__(self):
        self.__regexes=[
            re_val(
                re=EnglishNumeric.OrdinalSuffixRegex,
                val='OrdinalNum'),
            re_val(
                re=EnglishNumeric.OrdinalNumericRegex,
                val='OrdinalNum'),
            re_val(
                re=EnglishNumeric.OrdinalEnglishRegex,
                val='OrdEng'),
            re_val(
                re=EnglishNumeric.OrdinalRoundNumberRegex,
                val='OrdEng')
        ]

class EnglishPercentageExtractor(BasePercentageExtractor):
    def __init__(self):
        super().__init__(EnglishNumberExtractor(NumberMode.DEFAULT))

    def get_definitions(self) -> List[str]:
        return [
            EnglishNumeric.NumberWithSuffixPercentage,
            EnglishNumeric.NumberWithPrefixPercentage
        ]