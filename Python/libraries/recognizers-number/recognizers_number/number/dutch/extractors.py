from recognizers_number.resources.dutch_numeric import DutchNumeric
from typing import Pattern, List, NamedTuple
import regex
from recognizers_text.utilities import RegExpUtility
from recognizers_number.number.models import NumberMode, LongFormatMode
from recognizers_number.number.extractors import ReVal, ReRe, BaseNumberExtractor, BasePercentageExtractor, \
    BaseMergedNumberExtractor
from recognizers_number.number.constants import Constants


class DutchNumberExtractor(BaseNumberExtractor):
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
            DutchNumeric.NegativeNumberTermsRegex)
        self.__regexes: List[ReVal] = list()
        cardinal_ex: DutchCardinalExtractor = None

        if mode is NumberMode.PURE_NUMBER:
            cardinal_ex = DutchCardinalExtractor(
                DutchNumeric.PlaceHolderPureNumber)
        elif mode is NumberMode.CURRENCY:
            self.__regexes.append(ReVal(re=RegExpUtility.get_safe_reg_exp(
                DutchNumeric.CurrencyRegex), val='IntegerNum'))

        if cardinal_ex is None:
            cardinal_ex = DutchCardinalExtractor()

        self.__regexes.extend(cardinal_ex.regexes)

        fraction_ex = DutchFractionExtractor(mode)
        self.__regexes.extend(fraction_ex.regexes)

        ambiguity_filters_dict: List[ReRe] = list()

        if mode != NumberMode.Unit:
            for key, value in DutchNumeric.AmbiguityFiltersDict.items():
                ambiguity_filters_dict.append(ReRe(reKey=RegExpUtility.get_safe_reg_exp(key),
                                                   reVal=RegExpUtility.get_safe_reg_exp(value)))
        self.__ambiguity_filters_dict = ambiguity_filters_dict


class DutchCardinalExtractor(BaseNumberExtractor):
    @property
    def regexes(self) -> List[ReVal]:
        return self.__regexes

    @property
    def _extract_type(self) -> str:
        return Constants.SYS_NUM_CARDINAL

    def __init__(self, placeholder: str = DutchNumeric.PlaceHolderDefault):
        self.__regexes: List[ReVal] = list()

        # Add integer regexes
        integer_ex = DutchIntegerExtractor(placeholder)
        self.__regexes.extend(integer_ex.regexes)

        # Add double regexes
        double_ex = DutchDoubleExtractor(placeholder)
        self.__regexes.extend(double_ex.regexes)


class DutchIntegerExtractor(BaseNumberExtractor):
    @property
    def regexes(self) -> List[NamedTuple('re_val', [('re', Pattern), ('val', str)])]:
        return self.__regexes

    @property
    def _extract_type(self) -> str:
        return Constants.SYS_NUM_INTEGER

    def __init__(self, placeholder: str = DutchNumeric.PlaceHolderDefault):
        self.__regexes = [
            ReVal(
                re=RegExpUtility.get_safe_reg_exp(
                    DutchNumeric.NumbersWithPlaceHolder(placeholder)),
                val='IntegerNum'),
            ReVal(
                re=RegExpUtility.get_safe_reg_exp(
                    DutchNumeric.NumbersWithSuffix, regex.S),
                val='IntegerNum'),
            ReVal(
                re=RegExpUtility.get_safe_reg_exp(self._generate_format_regex(
                    LongFormatMode.INTEGER_COMMA, placeholder)),
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
                    DutchNumeric.RoundNumberIntegerRegexWithLocks),
                val='IntegerNum'),
            ReVal(
                re=RegExpUtility.get_safe_reg_exp(
                    DutchNumeric.NumbersWithDozenSuffix),
                val='IntegerNum'),
            ReVal(
                re=RegExpUtility.get_safe_reg_exp(
                    DutchNumeric.AllIntRegexWithLocks),
                val='IntegerDut'),
            ReVal(
                re=RegExpUtility.get_safe_reg_exp(
                    DutchNumeric.AllIntRegexWithDozenSuffixLocks),
                val='IntegerDut')
        ]


class DutchDoubleExtractor(BaseNumberExtractor):
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
                    DutchNumeric.DoubleDecimalPointRegex(placeholder)),
                val='DoubleNum'),
            ReVal(
                re=RegExpUtility.get_safe_reg_exp(
                    DutchNumeric.DoubleWithoutIntegralRegex(placeholder)),
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
                re=RegExpUtility.get_safe_reg_exp(self._generate_format_regex(
                    LongFormatMode.DOUBLE_NUM_BLANK_COMMA, placeholder)),
                val='DoubleNum'),
            ReVal(
                re=DutchNumeric.DoubleWithMultiplierRegex,
                val='DoubleNum'),
            ReVal(
                re=RegExpUtility.get_safe_reg_exp(
                    DutchNumeric.DoubleWithRoundNumber),
                val='DoubleNum'),
            ReVal(
                re=RegExpUtility.get_safe_reg_exp(
                    DutchNumeric.DoubleAllFloatRegex),
                val=f'Double{DutchNumeric.LangMarker}'),
            ReVal(
                re=RegExpUtility.get_safe_reg_exp(
                    DutchNumeric.DoubleExponentialNotationRegex),
                val='DoublePow'),
            ReVal(
                re=RegExpUtility.get_safe_reg_exp(
                    DutchNumeric.DoubleCaretExponentialNotationRegex),
                val='DoublePow')
        ]


class DutchFractionExtractor(BaseNumberExtractor):
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
                    DutchNumeric.FractionNotationWithSpacesRegex),
                val='FracNum'),
            ReVal(
                re=RegExpUtility.get_safe_reg_exp(
                    DutchNumeric.FractionNotationRegex),
                val='FracNum'),
            ReVal(
                re=RegExpUtility.get_safe_reg_exp(
                    DutchNumeric.FractionNounRegex),
                val=f'Frac{DutchNumeric.LangMarker}'),
            ReVal(
                re=RegExpUtility.get_safe_reg_exp(
                    DutchNumeric.FractionNounWithArticleRegex),
                val=f'Frac{DutchNumeric.LangMarker}')
        ]

        if mode != NumberMode.Unit:
            self.__regexes.append(
                ReVal(
                    re=RegExpUtility.get_safe_reg_exp(
                        DutchNumeric.FractionPrepositionRegex),
                    val=f'Frac{DutchNumeric.LangMarker}'))


class DutchOrdinalExtractor(BaseNumberExtractor):
    @property
    def regexes(self) -> List[NamedTuple('re_val', [('re', Pattern), ('val', str)])]:
        return self.__regexes

    @property
    def _extract_type(self) -> str:
        return Constants.SYS_NUM_ORDINAL

    def __init__(self):
        self.__regexes = [
            ReVal(
                re=DutchNumeric.OrdinalSuffixRegex,
                val='OrdinalNum'),
            ReVal(
                re=DutchNumeric.OrdinalNumericRegex,
                val='OrdinalNum'),
            ReVal(
                re=DutchNumeric.OrdinalDutchRegex,
                val='OrdDut'),
            ReVal(
                re=DutchNumeric.OrdinalRoundNumberRegex,
                val='OrdDut')
        ]


class DutchPercentageExtractor(BasePercentageExtractor):
    def __init__(self):
        super().__init__(DutchNumberExtractor(NumberMode.DEFAULT))

    def get_definitions(self) -> List[str]:
        return [
            DutchNumeric.NumberWithSuffixPercentage,
            DutchNumeric.NumberWithPrefixPercentage
        ]


class DutchMergedNumberExtractor(BaseMergedNumberExtractor):

    @property
    def _round_number_integer_regex_with_locks(self) -> Pattern:
        return RegExpUtility.get_safe_reg_exp(DutchNumeric.RoundNumberIntegerRegexWithLocks)

    @property
    def _connector_regex(self) -> Pattern:
        return RegExpUtility.get_safe_reg_exp(DutchNumeric.ConnectorRegex)

    def __init__(self, mode: NumberMode = NumberMode.DEFAULT):
        self._number_extractor = DutchNumberExtractor(mode)
        