from typing import Pattern, List, NamedTuple
import regex

from recognizers_number.number.models import NumberMode, LongFormatMode
from recognizers_number.resources.english_numeric import EnglishNumeric
from recognizers_number.number.extractors import ReVal, BaseNumberExtractor, BasePercentageExtractor
from recognizers_number.number.constants import Constants

class EnglishNumberExtractor(BaseNumberExtractor):
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
        self.__negative_number_terms = regex.compile(EnglishNumeric.NegativeNumberTermsRegex)
        self.__regexes: List[ReVal] = list()
        cardinal_ex: EnglishCardinalExtractor = None

        if mode is NumberMode.PURE_NUMBER:
            cardinal_ex = EnglishCardinalExtractor(EnglishNumeric.PlaceHolderPureNumber)
        elif mode is NumberMode.CURRENCY:
            self.__regexes.append(ReVal(re=EnglishNumeric.CurrencyRegex, val='IntegerNum'))

        if cardinal_ex is None:
            cardinal_ex = EnglishCardinalExtractor()

        self.__regexes.extend(cardinal_ex.regexes)

        fraction_ex = EnglishFractionExtractor()
        self.__regexes.extend(fraction_ex.regexes)

class EnglishCardinalExtractor(BaseNumberExtractor):
    @property
    def regexes(self) -> List[ReVal]:
        return self.__regexes

    @property
    def _extract_type(self) -> str:
        return Constants.SYS_NUM_CARDINAL

    def __init__(self, placeholder: str = EnglishNumeric.PlaceHolderDefault):
        self.__regexes: List[ReVal] = list()

        # Add integer regexes
        integer_ex = EnglishIntegerExtractor(placeholder)
        self.__regexes.extend(integer_ex.regexes)

        # Add double regexes
        double_ex = EnglishDoubleExtractor(placeholder)
        self.__regexes.extend(double_ex.regexes)

class EnglishIntegerExtractor(BaseNumberExtractor):
    @property
    def regexes(self) -> List[NamedTuple('re_val', [('re', Pattern), ('val', str)])]:
        return self.__regexes

    @property
    def _extract_type(self) -> str:
        return Constants.SYS_NUM_INTEGER

    def __init__(self, placeholder: str = EnglishNumeric.PlaceHolderDefault):
        self.__regexes = [
            ReVal(
                re=EnglishNumeric.NumbersWithPlaceHolder(placeholder),
                val='IntegerNum'),
            ReVal(
                re=EnglishNumeric.NumbersWithSuffix,
                val='IntegerNum'),
            ReVal(
                re=self._generate_format_regex(LongFormatMode.INTEGER_COMMA, placeholder),
                val='IntegerNum'),
            ReVal(
                re=EnglishNumeric.RoundNumberIntegerRegexWithLocks,
                val='IntegerNum'),
            ReVal(
                re=EnglishNumeric.NumbersWithDozenSuffix,
                val='IntegerNum'),
            ReVal(
                re=EnglishNumeric.AllIntRegexWithLocks,
                val='IntegerEng'),
            ReVal(
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

    def __init__(self, placeholder):
        self.__regexes = [
            ReVal(
                re=EnglishNumeric.DoubleDecimalPointRegex(placeholder),
                val='DoubleNum'),
            ReVal(
                re=EnglishNumeric.DoubleWithoutIntegralRegex(placeholder),
                val='DoubleNum'),
            ReVal(
                re=self._generate_format_regex(LongFormatMode.DOUBLE_COMMA_DOT, placeholder),
                val='DoubleNum'),
            ReVal(
                re=EnglishNumeric.DoubleWithMultiplierRegex,
                val='DoubleNum'),
            ReVal(
                re=EnglishNumeric.DoubleWithRoundNumber,
                val='DoubleNum'),
            ReVal(
                re=EnglishNumeric.DoubleAllFloatRegex,
                val='DoubleEng'),
            ReVal(
                re=EnglishNumeric.DoubleExponentialNotationRegex,
                val='DoublePow'),
            ReVal(
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

    def __init__(self):
        self.__regexes = [
            ReVal(
                re=EnglishNumeric.FractionNotationWithSpacesRegex,
                val='FracNum'),
            ReVal(
                re=EnglishNumeric.FractionNotationRegex,
                val='FracNum'),
            ReVal(
                re=EnglishNumeric.FractionNounRegex,
                val='FracEng'),
            ReVal(
                re=EnglishNumeric.FractionNounWithArticleRegex,
                val='FracEng'),
            ReVal(
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

    def __init__(self):
        self.__regexes = [
            ReVal(
                re=EnglishNumeric.OrdinalSuffixRegex,
                val='OrdinalNum'),
            ReVal(
                re=EnglishNumeric.OrdinalNumericRegex,
                val='OrdinalNum'),
            ReVal(
                re=EnglishNumeric.OrdinalEnglishRegex,
                val='OrdEng'),
            ReVal(
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
