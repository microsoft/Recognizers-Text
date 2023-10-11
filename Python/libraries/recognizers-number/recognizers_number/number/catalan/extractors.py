from typing import Pattern, List, NamedTuple

from recognizers_text.utilities import RegExpUtility
from recognizers_number.number.models import NumberMode, LongFormatMode
from recognizers_number.resources.catalan_numeric import CatalanNumeric
from recognizers_number.number.extractors import ReVal, ReRe, BaseNumberExtractor, BasePercentageExtractor
from recognizers_number.number.constants import Constants


class CatalanNumberExtractor(BaseNumberExtractor):
    @property
    def regexes(self) -> List[ReVal]:
        return self.__regexes

    @property
    def ambiguity_filters_dict(self) -> List[ReRe]:
        return self.__ambiguity_filters_dict

    @property
    def _extract_type(self) -> str:
        return Constants.SYS_NUM

    def __init__(self, mode: NumberMode = NumberMode.DEFAULT):
        self.__regexes: List[ReVal] = list()
        cardinal_ex: CatalanCardinalExtractor = None

        if mode is NumberMode.PURE_NUMBER:
            cardinal_ex = CatalanCardinalExtractor(
                CatalanNumeric.PlaceHolderPureNumber)
        elif mode is NumberMode.CURRENCY:
            self.__regexes.append(
                ReVal(re=CatalanNumeric.CurrencyRegex, val='IntegerNum'))

        if cardinal_ex is None:
            cardinal_ex = CatalanCardinalExtractor()

        self.__regexes.extend(cardinal_ex.regexes)

        fraction_ex = CatalanFractionExtractor(mode)
        self.__regexes.extend(fraction_ex.regexes)

        ambiguity_filters_dict: List[ReRe] = list()

        if mode != NumberMode.Unit:
            for key, value in CatalanNumeric.AmbiguityFiltersDict.items():
                ambiguity_filters_dict.append(ReRe(reKey=RegExpUtility.get_safe_reg_exp(key),
                                                   reVal=RegExpUtility.get_safe_reg_exp(value)))
        self.__ambiguity_filters_dict = ambiguity_filters_dict


class CatalanCardinalExtractor(BaseNumberExtractor):
    @property
    def regexes(self) -> List[ReVal]:
        return self.__regexes

    @property
    def _extract_type(self) -> str:
        return Constants.SYS_NUM_CARDINAL

    def __init__(self, placeholder: str = CatalanNumeric.PlaceHolderDefault):
        self.__regexes: List[ReVal] = list()

        # Add integer regexes
        integer_ex = CatalanIntegerExtractor(placeholder)
        self.__regexes.extend(integer_ex.regexes)

        # Add double regexes
        double_ex = CatalanDoubleExtractor(placeholder)
        self.__regexes.extend(double_ex.regexes)


class CatalanIntegerExtractor(BaseNumberExtractor):
    @property
    def regexes(self) -> List[
            NamedTuple('re_val', [('re', Pattern), ('val', str)])]:
        return self.__regexes

    @property
    def _extract_type(self) -> str:
        return Constants.SYS_NUM_INTEGER

    def __init__(self, placeholder: str = CatalanNumeric.PlaceHolderDefault):
        self.__regexes = [
            ReVal(
                re=CatalanNumeric.NumbersWithPlaceHolder(placeholder),
                val='IntegerNum'),
            ReVal(
                re=CatalanNumeric.NumbersWithSuffix,
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
                re=CatalanNumeric.RoundNumberIntegerRegexWithLocks,
                val='IntegerNum'),
            ReVal(
                re=CatalanNumeric.NumbersWithDozenSuffix,
                val='IntegerNum'),
            ReVal(
                re=CatalanNumeric.AllIntRegexWithLocks,
                val='IntegerCat'),
        ]


class CatalanDoubleExtractor(BaseNumberExtractor):
    @property
    def regexes(self) -> List[
            NamedTuple('re_val', [('re', Pattern), ('val', str)])]:
        return self.__regexes

    @property
    def _extract_type(self) -> str:
        return Constants.SYS_NUM_DOUBLE

    def __init__(self, placeholder: str = CatalanNumeric.PlaceHolderDefault):
        self.__regexes = [
            ReVal(
                re=CatalanNumeric.DoubleDecimalPointRegex(placeholder),
                val='DoubleNum'),
            ReVal(
                re=CatalanNumeric.DoubleWithoutIntegralRegex(placeholder),
                val='DoubleNum'),
            ReVal(
                re=CatalanNumeric.DoubleWithMultiplierRegex,
                val='DoubleNum'),
            ReVal(
                re=CatalanNumeric.DoubleWithRoundNumber,
                val='DoubleNum'),
            ReVal(
                re=CatalanNumeric.DoubleAllFloatRegex,
                val='DoubleCat'),
            ReVal(
                re=CatalanNumeric.DoubleExponentialNotationRegex,
                val='DoublePow'),
            ReVal(
                re=CatalanNumeric.DoubleCaretExponentialNotationRegex,
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


class CatalanFractionExtractor(BaseNumberExtractor):
    @property
    def regexes(self) -> List[
            NamedTuple('re_val', [('re', Pattern), ('val', str)])]:
        return self.__regexes

    @property
    def _extract_type(self) -> str:
        return Constants.SYS_NUM_FRACTION

    def __init__(self, mode):
        self.__regexes = []


class CatalanOrdinalExtractor(BaseNumberExtractor):
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
                re=CatalanNumeric.OrdinalSuffixRegex,
                val='OrdinalNum')
        ]


class CatalanPercentageExtractor(BasePercentageExtractor):
    def __init__(self):
        super().__init__(CatalanNumberExtractor())

    def get_definitions(self) -> List[str]:
        return []