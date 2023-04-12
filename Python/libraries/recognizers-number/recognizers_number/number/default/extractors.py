from typing import Pattern, List, NamedTuple
import regex

from recognizers_text.utilities import RegExpUtility
from recognizers_number.number.models import NumberMode, LongFormatMode
from recognizers_number.resources.default_numeric import DefaultNumeric
from recognizers_number.number.extractors import ReVal, ReRe, BaseNumberExtractor, BasePercentageExtractor, BaseMergedNumberExtractor
from recognizers_number.number.constants import Constants


class DefaultNumberExtractor(BaseNumberExtractor):
    @property
    def _extract_type(self) -> str:
        return Constants.SYS_NUM

    @property
    def regexes(self) -> List[ReVal]:
        return self.__regexes

    def __init__(self, mode: NumberMode = NumberMode.DEFAULT):
        self.__regexes: List[ReVal] = list()
        cardinal_ex: DefaultCardinalExtractor = None
        
        if mode is NumberMode.PURE_NUMBER:
            cardinal_ex = DefaultCardinalExtractor(
                DefaultNumeric.PlaceHolderPureNumber)
        
        if cardinal_ex is None:
            cardinal_ex = DefaultCardinalExtractor()

        self.__regexes.extend(cardinal_ex.regexes)

        fraction_ex = DefaultFractionExtractor(mode)
        self.__regexes.extend(fraction_ex.regexes)
        

class DefaultCardinalExtractor(BaseNumberExtractor):
    @property
    def regexes(self) -> List[ReVal]:
        return self.__regexes

    @property
    def _extract_type(self) -> str:
        return Constants.SYS_NUM_CARDINAL
    
    def __init__(self, placeholder: str = DefaultNumeric.PlaceHolderDefault):
        self.__regexes: List[ReVal] = list()

        # Add integer regexes
        integer_ex = DefaultIntegerExtractor(placeholder)
        self.__regexes.extend(integer_ex.regexes)

        # Add double regexes
        double_ex = DefaultDoubleExtractor(placeholder)
        self.__regexes.extend(double_ex.regexes)
        

class DefaultIntegerExtractor(BaseNumberExtractor):
    @property
    def regexes(self) -> List[ReVal]:
        return self.__regexes

    @property
    def _extract_type(self) -> str:
        return Constants.SYS_NUM_INTEGER
    
    def __init__(self, placeholder: str = DefaultNumeric.PlaceHolderDefault):
        self.__regexes = [
            ReVal(
                re=RegExpUtility.get_safe_reg_exp(
                    DefaultNumeric.NumbersWithPlaceHolder(placeholder)),
                val='IntegerNum'),
            ReVal(
                re=RegExpUtility.get_safe_reg_exp(
                    DefaultNumeric.NumbersWithSuffix, regex.S),
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
                val='IntegerNum')
        ]
        
class DefaultDoubleExtractor(BaseNumberExtractor):
    @property
    def regexes(self) -> List[ReVal]:
        return self.__regexes

    @property
    def _extract_type(self) -> str:
        return Constants.SYS_NUM_DOUBLE

    def __init__(self, placeholder):
        self.__regexes = [
            ReVal(
                re=RegExpUtility.get_safe_reg_exp(
                    DefaultNumeric.DoubleDecimalPointRegex(placeholder)),
                val='DoubleNum'),
            ReVal(
                re=RegExpUtility.get_safe_reg_exp(
                    DefaultNumeric.DoubleWithoutIntegralRegex(placeholder)),
                val='DoubleNum'),
            ReVal(
                re=RegExpUtility.get_safe_reg_exp(self._generate_format_regex(
                    LongFormatMode.DOUBLE_COMMA_DOT, placeholder)),
                val='DoubleNum'),
            ReVal(
                re=RegExpUtility.get_safe_reg_exp(self._generate_format_regex(
                    LongFormatMode.DOUBLE_NO_BREAK_SPACE_DOT, placeholder)),
                val='DoubleNum'),
            ReVal(
                re=RegExpUtility.get_safe_reg_exp(
                    DefaultNumeric.DoubleWithMultiplierRegex, regex.S),
                val='DoubleNum'),
            ReVal(
                re=RegExpUtility.get_safe_reg_exp(
                    DefaultNumeric.DoubleExponentialNotationRegex),
                val='DoublePow'),
            ReVal(
                re=RegExpUtility.get_safe_reg_exp(
                    DefaultNumeric.DoubleCaretExponentialNotationRegex),
                val='DoublePow')
        ]
        

class DefaultFractionExtractor(BaseNumberExtractor):
    @property
    def regexes(self) -> List[ReVal]:
        return self.__regexes

    @property
    def _extract_type(self) -> str:
        return Constants.SYS_NUM_FRACTION

    def __init__(self, mode):
        self.__regexes = [
            ReVal(
                re=RegExpUtility.get_safe_reg_exp(
                    DefaultNumeric.FractionNotationWithSpacesRegex),
                val='FracNum'),
            ReVal(
                re=RegExpUtility.get_safe_reg_exp(
                    DefaultNumeric.FractionNotationRegex),
                val='FracNum')
        ]


class DefaultOrdinalExtractor(BaseNumberExtractor):
    @property
    def regexes(self) -> List[ReVal]:
        return self.__regexes

    @property
    def _extract_type(self) -> str:
        return Constants.SYS_NUM_ORDINAL

    def __init__(self):
        self.__regexes = [
            ReVal(
                re=DefaultNumeric.OrdinalSuffixRegex,
                val='OrdinalNum'),
            ReVal(
                re=DefaultNumeric.OrdinalNumericRegex,
                val='OrdinalNum')
        ]


class DefaultPercentageExtractor(BasePercentageExtractor):
    @property
    def get_definitions(self) -> List[str]:
        pass

    def __init__(self):
        super().__init__(DefaultNumberExtractor(NumberMode.DEFAULT))
