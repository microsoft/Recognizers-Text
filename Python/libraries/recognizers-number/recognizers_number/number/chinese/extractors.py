from typing import List
from enum import Enum

from recognizers_number.number.extractors import ReVal, BaseNumberExtractor
from recognizers_text.utilities import RegExpUtility
from recognizers_number.number.constants import Constants
from recognizers_number.resources.chinese_numeric import ChineseNumeric
from recognizers_date_time.date_time.utilities import DateTimeOptions


class ChineseNumberExtractorMode(Enum):
    DEFAULT = 0
    EXTRACT_ALL = 1


class ChineseNumberExtractor(BaseNumberExtractor):
    @property
    def options(self):
        return self._options

    @property
    def regexes(self) -> List[ReVal]:
        return self.__regexes

    @property
    def _extract_type(self) -> str:
        return Constants.SYS_NUM

    def __init__(self, mode: ChineseNumberExtractorMode = ChineseNumberExtractorMode.DEFAULT):
        self.__regexes: List[ReVal] = list()

        cardinal_ex = ChineseCardinalExtractor(mode)
        self.__regexes.extend(cardinal_ex.regexes)

        fraction_ex = ChineseFractionExtractor()
        self.__regexes.extend(fraction_ex.regexes)

        self._options = DateTimeOptions.NONE


class ChineseCardinalExtractor(BaseNumberExtractor):
    @property
    def options(self):
        return self._options

    @property
    def regexes(self) -> List[ReVal]:
        return self.__regexes

    @property
    def _extract_type(self) -> str:
        return Constants.SYS_NUM_CARDINAL

    def __init__(self, mode: ChineseNumberExtractorMode = ChineseNumberExtractorMode.DEFAULT):
        self.__regexes: List[ReVal] = list()

        integer_ex = ChineseIntegerExtractor(mode)
        self.__regexes.extend(integer_ex.regexes)

        double_ex = ChineseDoubleExtractor()
        self.__regexes.extend(double_ex.regexes)
        self._options = DateTimeOptions.NONE


class ChineseIntegerExtractor(BaseNumberExtractor):
    @property
    def options(self):
        return self._options

    @property
    def regexes(self) -> List[ReVal]:
        return self.__regexes

    @property
    def _extract_type(self) -> str:
        return Constants.SYS_NUM_INTEGER

    def __init__(self, mode: ChineseNumberExtractorMode = ChineseNumberExtractorMode.DEFAULT):
        self.__regexes = [
            ReVal(
                re=RegExpUtility.get_safe_reg_exp(
                    ChineseNumeric.NumbersSpecialsChars),
                val='IntegerNum'),
            ReVal(
                re=RegExpUtility.get_safe_reg_exp(
                    ChineseNumeric.NumbersSpecialsCharsWithSuffix),
                val='IntegerNum'),
            ReVal(
                re=RegExpUtility.get_safe_reg_exp(
                    ChineseNumeric.DottedNumbersSpecialsChar),
                val='IntegerNum'),
            ReVal(
                re=RegExpUtility.get_safe_reg_exp(
                    ChineseNumeric.NumbersWithHalfDozen),
                val='IntegerChs'),
            ReVal(
                re=RegExpUtility.get_safe_reg_exp(
                    ChineseNumeric.NumbersWithDozen),
                val='IntegerChs'),
            ReVal(
                re=RegExpUtility.get_safe_reg_exp(
                    ChineseNumeric.HalfUnitRegex),
                val='IntegerChs')
        ]
        if mode == ChineseNumberExtractorMode.DEFAULT:
            self.__regexes.append(
                ReVal(
                    re=RegExpUtility.get_safe_reg_exp(
                        ChineseNumeric.NumbersWithAllowListRegex),
                    val='IntegerChs'
                )
            )
        elif mode == ChineseNumberExtractorMode.EXTRACT_ALL:
            self.__regexes.append(
                ReVal(
                    re=RegExpUtility.get_safe_reg_exp(
                        ChineseNumeric.NumbersAggressiveRegex),
                    val='IntegerChs'
                )
            )
        self._options = DateTimeOptions.NONE


class ChineseDoubleExtractor(BaseNumberExtractor):
    @property
    def options(self):
        return self._options

    @property
    def regexes(self) -> List[ReVal]:
        return self.__regexes

    @property
    def _extract_type(self) -> str:
        return Constants.SYS_NUM_DOUBLE

    def __init__(self):
        self.__regexes = [
            ReVal(
                re=RegExpUtility.get_safe_reg_exp(
                    ChineseNumeric.DoubleSpecialsChars),
                val='DoubleNum'),
            ReVal(
                re=RegExpUtility.get_safe_reg_exp(
                    ChineseNumeric.DoubleSpecialsCharsWithNegatives),
                val='DoubleNum'),
            ReVal(
                re=RegExpUtility.get_safe_reg_exp(
                    ChineseNumeric.SimpleDoubleSpecialsChars),
                val='DoubleNum'),
            ReVal(
                re=RegExpUtility.get_safe_reg_exp(
                    ChineseNumeric.DoubleWithMultiplierRegex),
                val='DoubleNum'),
            ReVal(
                re=RegExpUtility.get_safe_reg_exp(
                    ChineseNumeric.DoubleWithThousandsRegex),
                val='DoubleChs'),
            ReVal(
                re=RegExpUtility.get_safe_reg_exp(
                    ChineseNumeric.DoubleAllFloatRegex),
                val='DoubleChs'),
            ReVal(
                re=RegExpUtility.get_safe_reg_exp(
                    ChineseNumeric.DoubleExponentialNotationRegex),
                val='DoublePow'),
            ReVal(
                re=RegExpUtility.get_safe_reg_exp(
                    ChineseNumeric.DoubleScientificNotationRegex),
                val='DoublePow')
        ]
        self._options = DateTimeOptions.NONE


class ChineseFractionExtractor(BaseNumberExtractor):
    @property
    def options(self):
        return self._options

    @property
    def regexes(self) -> List[ReVal]:
        return self.__regexes

    @property
    def _extract_type(self) -> str:
        return Constants.SYS_NUM_FRACTION

    def __init__(self):
        self.__regexes = [
            ReVal(
                re=RegExpUtility.get_safe_reg_exp(
                    ChineseNumeric.FractionNotationSpecialsCharsRegex),
                val='FracNum'),
            ReVal(
                re=RegExpUtility.get_safe_reg_exp(
                    ChineseNumeric.FractionNotationRegex),
                val='FracNum'),
            ReVal(
                re=RegExpUtility.get_safe_reg_exp(
                    ChineseNumeric.AllFractionNumber),
                val='FracChs')
        ]
        self._options = DateTimeOptions.NONE


class ChineseOrdinalExtractor(BaseNumberExtractor):
    @property
    def options(self):
        return self._options

    @property
    def regexes(self) -> List[ReVal]:
        return self.__regexes

    @property
    def _extract_type(self) -> str:
        return Constants.SYS_NUM_ORDINAL

    def __init__(self):
        self.__regexes = [
            ReVal(
                re=RegExpUtility.get_safe_reg_exp(ChineseNumeric.OrdinalRegex),
                val='OrdinalChs'),
            ReVal(
                re=RegExpUtility.get_safe_reg_exp(
                    ChineseNumeric.OrdinalNumbersRegex),
                val='OrdinalChs')
        ]
        self._options = DateTimeOptions.NONE


class ChinesePercentageExtractor(BaseNumberExtractor):
    @property
    def options(self):
        return self._options

    @property
    def regexes(self) -> List[ReVal]:
        return self.__regexes

    @property
    def _extract_type(self) -> str:
        return Constants.SYS_NUM_PERCENTAGE

    def __init__(self):
        self.__regexes = [
            ReVal(
                re=RegExpUtility.get_safe_reg_exp(
                    ChineseNumeric.PercentagePointRegex),
                val='PerChs'),
            ReVal(
                re=RegExpUtility.get_safe_reg_exp(
                    ChineseNumeric.SimplePercentageRegex),
                val='PerChs'),
            ReVal(
                re=RegExpUtility.get_safe_reg_exp(
                    ChineseNumeric.NumbersPercentagePointRegex),
                val='PerNum'),
            ReVal(
                re=RegExpUtility.get_safe_reg_exp(
                    ChineseNumeric.NumbersPercentageWithSeparatorRegex),
                val='PerNum'),
            ReVal(
                re=RegExpUtility.get_safe_reg_exp(
                    ChineseNumeric.NumbersPercentageWithMultiplierRegex),
                val='PerNum'),
            ReVal(
                re=RegExpUtility.get_safe_reg_exp(
                    ChineseNumeric.FractionPercentagePointRegex),
                val='PerNum'),
            ReVal(
                re=RegExpUtility.get_safe_reg_exp(
                    ChineseNumeric.FractionPercentageWithSeparatorRegex),
                val='PerNum'),
            ReVal(
                re=RegExpUtility.get_safe_reg_exp(
                    ChineseNumeric.FractionPercentageWithMultiplierRegex),
                val='PerNum'),
            ReVal(
                re=RegExpUtility.get_safe_reg_exp(
                    ChineseNumeric.SimpleNumbersPercentageRegex),
                val='PerNum'),
            ReVal(
                re=RegExpUtility.get_safe_reg_exp(
                    ChineseNumeric.SimpleNumbersPercentageWithMultiplierRegex),
                val='PerNum'),
            ReVal(
                re=RegExpUtility.get_safe_reg_exp(
                    ChineseNumeric.SimpleNumbersPercentagePointRegex),
                val='PerNum'),
            ReVal(
                re=RegExpUtility.get_safe_reg_exp(
                    ChineseNumeric.IntegerPercentageRegex),
                val='PerNum'),
            ReVal(
                re=RegExpUtility.get_safe_reg_exp(
                    ChineseNumeric.IntegerPercentageWithMultiplierRegex),
                val='PerNum'),
            ReVal(
                re=RegExpUtility.get_safe_reg_exp(
                    ChineseNumeric.NumbersFractionPercentageRegex),
                val='PerNum'),
            ReVal(
                re=RegExpUtility.get_safe_reg_exp(
                    ChineseNumeric.SimpleIntegerPercentageRegex),
                val='PerNum'),
            ReVal(
                re=RegExpUtility.get_safe_reg_exp(
                    ChineseNumeric.NumbersFoldsPercentageRegex),
                val='PerSpe'),
            ReVal(
                re=RegExpUtility.get_safe_reg_exp(
                    ChineseNumeric.FoldsPercentageRegex),
                val='PerSpe'),
            ReVal(
                re=RegExpUtility.get_safe_reg_exp(
                    ChineseNumeric.SimpleFoldsPercentageRegex),
                val='PerSpe'),
            ReVal(
                re=RegExpUtility.get_safe_reg_exp(
                    ChineseNumeric.SpecialsPercentageRegex),
                val='PerSpe'),
            ReVal(
                re=RegExpUtility.get_safe_reg_exp(
                    ChineseNumeric.NumbersSpecialsPercentageRegex),
                val='PerSpe'),
            ReVal(
                re=RegExpUtility.get_safe_reg_exp(
                    ChineseNumeric.SimpleSpecialsPercentageRegex),
                val='PerSpe'),
            ReVal(
                re=RegExpUtility.get_safe_reg_exp(
                    ChineseNumeric.SpecialsFoldsPercentageRegex),
                val='PerSpe')
        ]
        self._options = DateTimeOptions.NONE
