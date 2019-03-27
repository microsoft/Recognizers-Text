from typing import Pattern, List, NamedTuple

from recognizers_number.number.models import NumberMode, LongFormatMode
from recognizers_number.resources.spanish_numeric import SpanishNumeric
from recognizers_number.number.extractors import ReVal, BaseNumberExtractor, \
    BasePercentageExtractor
from recognizers_number.number.constants import Constants


class SpanishNumberExtractor(BaseNumberExtractor):
    @property
    def regexes(self) -> List[ReVal]:
        return self.__regexes

    @property
    def _extract_type(self) -> str:
        return Constants.SYS_NUM

    def __init__(self, mode: NumberMode = NumberMode.DEFAULT):
        self.__regexes: List[ReVal] = list()
        cardinal_ex: SpanishCardinalExtractor = None

        if mode is NumberMode.PURE_NUMBER:
            cardinal_ex = SpanishCardinalExtractor(
                SpanishNumeric.PlaceHolderPureNumber)
        elif mode is NumberMode.CURRENCY:
            self.__regexes.append(
                ReVal(re=SpanishNumeric.CurrencyRegex, val='IntegerNum'))

        if cardinal_ex is None:
            cardinal_ex = SpanishCardinalExtractor()

        self.__regexes.extend(cardinal_ex.regexes)

        fraction_ex = SpanishFractionExtractor()
        self.__regexes.extend(fraction_ex.regexes)


class SpanishCardinalExtractor(BaseNumberExtractor):
    @property
    def regexes(self) -> List[ReVal]:
        return self.__regexes

    @property
    def _extract_type(self) -> str:
        return Constants.SYS_NUM_CARDINAL

    def __init__(self, placeholder: str = SpanishNumeric.PlaceHolderDefault):
        self.__regexes: List[ReVal] = list()

        # Add integer regexes
        integer_ex = SpanishIntegerExtractor(placeholder)
        self.__regexes.extend(integer_ex.regexes)

        # Add double regexes
        double_ex = SpanishDoubleExtractor(placeholder)
        self.__regexes.extend(double_ex.regexes)


class SpanishIntegerExtractor(BaseNumberExtractor):
    @property
    def regexes(self) -> List[
        NamedTuple('re_val', [('re', Pattern), ('val', str)])]:
        return self.__regexes

    @property
    def _extract_type(self) -> str:
        return Constants.SYS_NUM_INTEGER

    def __init__(self, placeholder: str = SpanishNumeric.PlaceHolderDefault):
        self.__regexes = [
            ReVal(
                re=SpanishNumeric.NumbersWithPlaceHolder(placeholder),
                val='IntegerNum'),
            ReVal(
                re=SpanishNumeric.NumbersWithSuffix,
                val='IntegerNum'),
            ReVal(
                re=self._generate_format_regex(LongFormatMode.INTEGER_DOT,
                                               placeholder),
                val='IntegerNum'),
            ReVal(
                re=self._generate_format_regex(LongFormatMode.INTEGER_BLANK,
                                               placeholder),
                val='IntegerNum'),
            ReVal(
                re=self._generate_format_regex(
                    LongFormatMode.INTEGER_NO_BREAK_SPACE, placeholder),
                val='IntegerNum'),
            ReVal(
                re=SpanishNumeric.RoundNumberIntegerRegexWithLocks,
                val='IntegerNum'),
            ReVal(
                re=SpanishNumeric.NumbersWithDozenSuffix,
                val='IntegerNum'),
            ReVal(
                re=SpanishNumeric.AllIntRegexWithLocks,
                val='IntegerSpa'),
            ReVal(
                re=SpanishNumeric.AllIntRegexWithDozenSuffixLocks,
                val='IntegerSpa')
        ]


class SpanishDoubleExtractor(BaseNumberExtractor):
    @property
    def regexes(self) -> List[
        NamedTuple('re_val', [('re', Pattern), ('val', str)])]:
        return self.__regexes

    @property
    def _extract_type(self) -> str:
        return Constants.SYS_NUM_DOUBLE

    def __init__(self, placeholder: str = SpanishNumeric.PlaceHolderDefault):
        self.__regexes = [
            ReVal(
                re=SpanishNumeric.DoubleDecimalPointRegex(placeholder),
                val='DoubleNum'),
            ReVal(
                re=SpanishNumeric.DoubleWithoutIntegralRegex(placeholder),
                val='DoubleNum'),
            ReVal(
                re=SpanishNumeric.DoubleWithMultiplierRegex,
                val='DoubleNum'),
            ReVal(
                re=SpanishNumeric.DoubleWithRoundNumber,
                val='DoubleNum'),
            ReVal(
                re=SpanishNumeric.DoubleAllFloatRegex,
                val='DoubleSpa'),
            ReVal(
                re=SpanishNumeric.DoubleExponentialNotationRegex,
                val='DoublePow'),
            ReVal(
                re=SpanishNumeric.DoubleCaretExponentialNotationRegex,
                val='DoublePow'),
            ReVal(
                re=self._generate_format_regex(LongFormatMode.DOUBLE_DOT_COMMA,
                                               placeholder),
                val='DoubleNum'),
            ReVal(
                re=self._generate_format_regex(
                    LongFormatMode.DOUBLE_NO_BREAK_SPACE_COMMA,
                    placeholder),
                val='DoubleNum')
        ]


class SpanishFractionExtractor(BaseNumberExtractor):
    @property
    def regexes(self) -> List[
        NamedTuple('re_val', [('re', Pattern), ('val', str)])]:
        return self.__regexes

    @property
    def _extract_type(self) -> str:
        return Constants.SYS_NUM_FRACTION

    def __init__(self):
        self.__regexes = [
            ReVal(
                re=SpanishNumeric.FractionNotationRegex,
                val='FracNum'),
            ReVal(
                re=SpanishNumeric.FractionNotationWithSpacesRegex,
                val='FracNum'),
            ReVal(
                re=SpanishNumeric.FractionNounRegex,
                val='FracSpa'),
            ReVal(
                re=SpanishNumeric.FractionNounWithArticleRegex,
                val='FracSpa'),
            ReVal(
                re=SpanishNumeric.FractionPrepositionRegex,
                val='FracSpa')
        ]


class SpanishOrdinalExtractor(BaseNumberExtractor):
    @property
    def regexes(self) -> List[
        NamedTuple('re_val', [('re', Pattern), ('val', str)])]:
        return self.__regexes

    @property
    def _extract_type(self) -> str:
        return Constants.SYS_NUM_ORDINAL

    def __init__(self):
        self.__regexes = [
            ReVal(
                re=SpanishNumeric.OrdinalSuffixRegex,
                val='OrdinalNum'),
            ReVal(
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
