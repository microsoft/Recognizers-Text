import regex
from enum import Enum
from typing import List, Pattern

from recognizers_text.utilities import RegExpUtility
from recognizers_text.extractor import ExtractResult
from recognizers_number.number.extractors import re_val, BaseNumberExtractor
from recognizers_number.number.constants import Constants
from recognizers_number.resources.chinese_numeric import ChineseNumeric

class ChineseNumberExtractorMode(Enum):
    DEFAULT=0,
    EXTRACT_ALL=1

class ChineseNumberExtractor(BaseNumberExtractor):
    @property
    def regexes(self) -> List[re_val]:
        return self.__regexes

    @property
    def _extract_type(self) -> str:
        return Constants.SYS_NUM

    @property
    def _negative_number_terms(self) -> Pattern: pass

    def __init__(self, mode: ChineseNumberExtractorMode = ChineseNumberExtractorMode.DEFAULT):
        self.__regexes: List[re_val] = list()

        cardinal_ex = ChineseCardinalExtractor(mode)
        self.__regexes.extend(cardinal_ex.regexes)

        fraction_ex = ChineseFractionExtractor()
        self.__regexes.extend(fraction_ex.regexes)

class ChineseCardinalExtractor(BaseNumberExtractor):
    @property
    def regexes(self) -> List[re_val]:
        return self.__regexes

    @property
    def _extract_type(self) -> str:
        return Constants.SYS_NUM_CARDINAL

    @property
    def _negative_number_terms(self) -> Pattern: pass

    def __init__(self, mode: ChineseNumberExtractorMode = ChineseNumberExtractorMode.DEFAULT):
        self.__regexes: List[re_val] = list()

        integer_ex = ChineseIntegerExtractor(mode)
        self.__regexes.extend(integer_ex.regexes)

        double_ex = ChineseDoubleExtractor()
        self.__regexes.extend(double_ex.regexes)

class ChineseIntegerExtractor(BaseNumberExtractor):
    @property
    def regexes(self) -> List[re_val]:
        return self.__regexes

    @property
    def _extract_type(self) -> str:
        return Constants.SYS_NUM_INTEGER

    @property
    def _negative_number_terms(self) -> Pattern: pass

    def __init__(self, mode: ChineseNumberExtractorMode = ChineseNumberExtractorMode.DEFAULT):
        self.__regexes=[
            re_val(
                re=RegExpUtility.get_safe_reg_exp(ChineseNumeric.NumbersSpecialsChars),
                val='IntegerNum'),
            re_val(
                re=RegExpUtility.get_safe_reg_exp(ChineseNumeric.NumbersSpecialsCharsWithSuffix),
                val='IntegerNum'),
            re_val(
                re=RegExpUtility.get_safe_reg_exp(ChineseNumeric.DottedNumbersSpecialsChar),
                val='IntegerNum'),
            re_val(
                re=RegExpUtility.get_safe_reg_exp(ChineseNumeric.NumbersWithHalfDozen),
                val='IntegerChs'),
            re_val(
                re=RegExpUtility.get_safe_reg_exp(ChineseNumeric.NumbersWithDozen),
                val='IntegerChs')
        ]
        if mode == ChineseNumberExtractorMode.DEFAULT:
            self.__regexes.append(
                re_val(
                    re=RegExpUtility.get_safe_reg_exp(ChineseNumeric.NumbersWithAllowListRegex),
                    val='IntegerChs'
                )
            )
        elif mode == ChineseNumberExtractorMode.EXTRACT_ALL:
            self.__regexes.append(
                re_val(
                    re=RegExpUtility.get_safe_reg_exp(ChineseNumeric.NumbersAggressiveRegex),
                    val='IntegerChs'
                )
            )

class ChineseDoubleExtractor(BaseNumberExtractor):
    @property
    def regexes(self) -> List[re_val]:
        return self.__regexes

    @property
    def _extract_type(self) -> str:
        return Constants.SYS_NUM_DOUBLE

    @property
    def _negative_number_terms(self) -> Pattern: pass

    def __init__(self):
        self.__regexes=[
            re_val(
                re=RegExpUtility.get_safe_reg_exp(ChineseNumeric.DoubleSpecialsChars),
                val='DoubleNum'),
            re_val(
                re=RegExpUtility.get_safe_reg_exp(ChineseNumeric.DoubleSpecialsCharsWithNegatives),
                val='DoubleNum'),
            re_val(
                re=RegExpUtility.get_safe_reg_exp(ChineseNumeric.SimpleDoubleSpecialsChars),
                val='DoubleNum'),
            re_val(
                re=RegExpUtility.get_safe_reg_exp(ChineseNumeric.DoubleWithMultiplierRegex),
                val='DoubleNum'),
            re_val(
                re=RegExpUtility.get_safe_reg_exp(ChineseNumeric.DoubleWithThousandsRegex),
                val='DoubleChs'),
            re_val(
                re=RegExpUtility.get_safe_reg_exp(ChineseNumeric.DoubleAllFloatRegex),
                val='DoubleChs'),
            re_val(
                re=RegExpUtility.get_safe_reg_exp(ChineseNumeric.DoubleExponentialNotationRegex),
                val='DoublePow'),
            re_val(
                re=RegExpUtility.get_safe_reg_exp(ChineseNumeric.DoubleScientificNotationRegex),
                val='DoublePow')
        ]

class ChineseFractionExtractor(BaseNumberExtractor):
    @property
    def regexes(self) -> List[re_val]:
        return self.__regexes

    @property
    def _extract_type(self) -> str:
        return Constants.SYS_NUM_FRACTION
    @property
    def _negative_number_terms(self) -> Pattern: pass

    def __init__(self):
        self.__regexes=[
            re_val(
                re=RegExpUtility.get_safe_reg_exp(ChineseNumeric.FractionNotationSpecialsCharsRegex),
                val='FracNum'),
            re_val(
                re=RegExpUtility.get_safe_reg_exp(ChineseNumeric.FractionNotationRegex),
                val='FracNum'),
            re_val(
                re=RegExpUtility.get_safe_reg_exp(ChineseNumeric.AllFractionNumber),
                val='FracChs')
        ]

class ChineseOrdinalExtractor(BaseNumberExtractor):
    @property
    def regexes(self) -> List[re_val]:
        return self.__regexes

    @property
    def _extract_type(self) -> str:
        return Constants.SYS_NUM_ORDINAL

    @property
    def _negative_number_terms(self) -> Pattern: pass

    def __init__(self):
        self.__regexes=[
            re_val(
                re=RegExpUtility.get_safe_reg_exp(ChineseNumeric.OrdinalRegexChs),
                val='OrdinalChs'),
            re_val(
                re=RegExpUtility.get_safe_reg_exp(ChineseNumeric.OrdinalNumbersRegex),
                val='OrdinalChs')
        ]

class ChinesePercentageExtractor(BaseNumberExtractor):
    @property
    def regexes(self) -> List[re_val]:
        return self.__regexes

    @property
    def _extract_type(self) -> str:
        return Constants.SYS_NUM_PERCENTAGE

    @property
    def _negative_number_terms(self) -> Pattern: pass

    def __init__(self):
        self.__regexes=[
            re_val(
                re=RegExpUtility.get_safe_reg_exp(ChineseNumeric.PercentagePointRegex),
                val='PerChs'),
            re_val(
                re=RegExpUtility.get_safe_reg_exp(ChineseNumeric.SimplePercentageRegex),
                val='PerChs'),
            re_val(
                re=RegExpUtility.get_safe_reg_exp(ChineseNumeric.NumbersPercentagePointRegex),
                val='PerNum'),
            re_val(
                re=RegExpUtility.get_safe_reg_exp(ChineseNumeric.NumbersPercentageWithSeparatorRegex),
                val='PerNum'),
            re_val(
                re=RegExpUtility.get_safe_reg_exp(ChineseNumeric.NumbersPercentageWithMultiplierRegex),
                val='PerNum'),
            re_val(
                re=RegExpUtility.get_safe_reg_exp(ChineseNumeric.FractionPercentagePointRegex),
                val='PerNum'),
            re_val(
                re=RegExpUtility.get_safe_reg_exp(ChineseNumeric.FractionPercentageWithSeparatorRegex),
                val='PerNum'),
            re_val(
                re=RegExpUtility.get_safe_reg_exp(ChineseNumeric.FractionPercentageWithMultiplierRegex),
                val='PerNum'),
            re_val(
                re=RegExpUtility.get_safe_reg_exp(ChineseNumeric.SimpleNumbersPercentageRegex),
                val='PerNum'),
            re_val(
                re=RegExpUtility.get_safe_reg_exp(ChineseNumeric.SimpleNumbersPercentageWithMultiplierRegex),
                val='PerNum'),
            re_val(
                re=RegExpUtility.get_safe_reg_exp(ChineseNumeric.SimpleNumbersPercentagePointRegex),
                val='PerNum'),
            re_val(
                re=RegExpUtility.get_safe_reg_exp(ChineseNumeric.IntegerPercentageRegex),
                val='PerNum'),
            re_val(
                re=RegExpUtility.get_safe_reg_exp(ChineseNumeric.IntegerPercentageWithMultiplierRegex),
                val='PerNum'),
            re_val(
                re=RegExpUtility.get_safe_reg_exp(ChineseNumeric.NumbersFractionPercentageRegex),
                val='PerNum'),
            re_val(
                re=RegExpUtility.get_safe_reg_exp(ChineseNumeric.SimpleIntegerPercentageRegex),
                val='PerNum'),
            re_val(
                re=RegExpUtility.get_safe_reg_exp(ChineseNumeric.NumbersFoldsPercentageRegex),
                val='PerSpe'),
            re_val(
                re=RegExpUtility.get_safe_reg_exp(ChineseNumeric.FoldsPercentageRegex),
                val='PerSpe'),
            re_val(
                re=RegExpUtility.get_safe_reg_exp(ChineseNumeric.SimpleFoldsPercentageRegex),
                val='PerSpe'),
            re_val(
                re=RegExpUtility.get_safe_reg_exp(ChineseNumeric.SpecialsPercentageRegex),
                val='PerSpe'),
            re_val(
                re=RegExpUtility.get_safe_reg_exp(ChineseNumeric.NumbersSpecialsPercentageRegex),
                val='PerSpe'),
            re_val(
                re=RegExpUtility.get_safe_reg_exp(ChineseNumeric.SimpleSpecialsPercentageRegex),
                val='PerSpe'),
            re_val(
                re=RegExpUtility.get_safe_reg_exp(ChineseNumeric.SpecialsFoldsPercentageRegex),
                val='PerSpe')
        ]