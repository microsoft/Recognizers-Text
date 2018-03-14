from enum import Enum
from typing import List, Pattern

from recognizers_text.extractor import ExtractResult
from recognizers_number.number.extractors import ReVal, BaseNumberExtractor
from recognizers_number.number.constants import Constants
from recognizers_number.resources.chinese_numeric import ChineseNumeric

class ChineseNumberExtractorMode(Enum):
    DEFAULT=0,
    EXTRACT_ALL=1

class ChineseNumberExtractor(BaseNumberExtractor):
    @property
    def regexes(self) -> List[ReVal]:
        return self.__regexes

    @property
    def _extract_type(self) -> str:
        return Constants.SYS_NUM

    @property
    def _negative_number_terms(self) -> Pattern: pass

    def __init__(self, mode: ChineseNumberExtractorMode = ChineseNumberExtractorMode.DEFAULT):
        self.__regexes: List[ReVal] = list()

        cardinal_ex = ChineseCardinalExtractor(mode)
        self.__regexes.extend(cardinal_ex.regexes)

        fraction_ex = ChineseFractionExtractor()
        self.__regexes.extend(fraction_ex.regexes)

class ChineseCardinalExtractor(BaseNumberExtractor):
    @property
    def regexes(self) -> List[ReVal]:
        return self.__regexes

    @property
    def _extract_type(self) -> str:
        return Constants.SYS_NUM_CARDINAL

    @property
    def _negative_number_terms(self) -> Pattern: pass

    def __init__(self, mode: ChineseNumberExtractorMode = ChineseNumberExtractorMode.DEFAULT):
        self.__regexes: List[ReVal] = list()

        integer_ex = ChineseIntegerExtractor(mode)
        self.__regexes.extend(integer_ex.regexes)

        double_ex = ChineseDoubleExtractor()
        self.__regexes.extend(double_ex.regexes)

class ChineseIntegerExtractor(BaseNumberExtractor):
    @property
    def regexes(self) -> List[ReVal]:
        return self.__regexes

    @property
    def _extract_type(self) -> str:
        return Constants.SYS_NUM_INTEGER

    @property
    def _negative_number_terms(self) -> Pattern: pass

    def __init__(self, mode: ChineseNumberExtractorMode = ChineseNumberExtractorMode.DEFAULT):
        self.__regexes=[
            ReVal(
                re=ChineseNumeric.NumbersSpecialsChars,
                val='IntegerNum'),
            ReVal(
                re=ChineseNumeric.NumbersSpecialsCharsWithSuffix,
                val='IntegerNum'),
            ReVal(
                re=ChineseNumeric.DottedNumbersSpecialsChar,
                val='IntegerNum'),
            ReVal(
                re=ChineseNumeric.NumbersWithHalfDozen,
                val='IntegerChs'),
            ReVal(
                re=ChineseNumeric.NumbersWithDozen,
                val='IntegerChs')
        ]
        if mode == ChineseNumberExtractorMode.DEFAULT:
            self.__regexes.append(
                ReVal(
                    re=ChineseNumeric.NumbersWithAllowListRegex,
                    val='IntegerChs'
                )
            )
        elif mode == ChineseNumberExtractorMode.EXTRACT_ALL:
            self.__regexes.append(
                ReVal(
                    re=ChineseNumeric.NumbersAggressiveRegex,
                    val='IntegerChs'
                )
            )

class ChineseDoubleExtractor(BaseNumberExtractor):
    @property
    def regexes(self) -> List[ReVal]:
        return self.__regexes

    @property
    def _extract_type(self) -> str:
        return Constants.SYS_NUM_DOUBLE

    @property
    def _negative_number_terms(self) -> Pattern: pass

    def __init__(self):
        self.__regexes=[
            ReVal(
                re=ChineseNumeric.DoubleSpecialsChars,
                val='DoubleNum'),
            ReVal(
                re=ChineseNumeric.DoubleSpecialsCharsWithNegatives,
                val='DoubleNum'),
            ReVal(
                re=ChineseNumeric.SimpleDoubleSpecialsChars,
                val='DoubleNum'),
            ReVal(
                re=ChineseNumeric.DoubleWithMultiplierRegex,
                val='DoubleNum'),
            ReVal(
                re=ChineseNumeric.DoubleWithThousandsRegex,
                val='DoubleChs'),
            ReVal(
                re=ChineseNumeric.DoubleAllFloatRegex,
                val='DoubleChs'),
            ReVal(
                re=ChineseNumeric.DoubleExponentialNotationRegex,
                val='DoublePow'),
            ReVal(
                re=ChineseNumeric.DoubleScientificNotationRegex,
                val='DoublePow')
        ]

class ChineseFractionExtractor(BaseNumberExtractor):
    @property
    def regexes(self) -> List[ReVal]:
        return self.__regexes

    @property
    def _extract_type(self) -> str:
        return Constants.SYS_NUM_FRACTION
    @property
    def _negative_number_terms(self) -> Pattern: pass

    def __init__(self):
        self.__regexes=[
            ReVal(
                re=ChineseNumeric.FractionNotationSpecialsCharsRegex,
                val='FracNum'),
            ReVal(
                re=ChineseNumeric.FractionNotationRegex,
                val='FracNum'),
            ReVal(
                re=ChineseNumeric.AllFractionNumber,
                val='FracChs')
        ]

class ChineseOrdinalExtractor(BaseNumberExtractor):
    @property
    def regexes(self) -> List[ReVal]:
        return self.__regexes

    @property
    def _extract_type(self) -> str:
        return Constants.SYS_NUM_ORDINAL

    @property
    def _negative_number_terms(self) -> Pattern: pass

    def __init__(self):
        self.__regexes=[
            ReVal(
                re=ChineseNumeric.OrdinalRegexChs,
                val='OrdinalChs'),
            ReVal(
                re=ChineseNumeric.OrdinalNumbersRegex,
                val='OrdinalChs')
        ]

class ChinesePercentageExtractor(BaseNumberExtractor):
    @property
    def regexes(self) -> List[ReVal]:
        return self.__regexes

    @property
    def _extract_type(self) -> str:
        return Constants.SYS_NUM_PERCENTAGE

    @property
    def _negative_number_terms(self) -> Pattern: pass

    def __init__(self):
        self.__regexes=[
            ReVal(
                re=ChineseNumeric.PercentagePointRegex,
                val='PerChs'),
            ReVal(
                re=ChineseNumeric.SimplePercentageRegex,
                val='PerChs'),
            ReVal(
                re=ChineseNumeric.NumbersPercentagePointRegex,
                val='PerNum'),
            ReVal(
                re=ChineseNumeric.NumbersPercentageWithSeparatorRegex,
                val='PerNum'),
            ReVal(
                re=ChineseNumeric.NumbersPercentageWithMultiplierRegex,
                val='PerNum'),
            ReVal(
                re=ChineseNumeric.FractionPercentagePointRegex,
                val='PerNum'),
            ReVal(
                re=ChineseNumeric.FractionPercentageWithSeparatorRegex,
                val='PerNum'),
            ReVal(
                re=ChineseNumeric.FractionPercentageWithMultiplierRegex,
                val='PerNum'),
            ReVal(
                re=ChineseNumeric.SimpleNumbersPercentageRegex,
                val='PerNum'),
            ReVal(
                re=ChineseNumeric.SimpleNumbersPercentageWithMultiplierRegex,
                val='PerNum'),
            ReVal(
                re=ChineseNumeric.SimpleNumbersPercentagePointRegex,
                val='PerNum'),
            ReVal(
                re=ChineseNumeric.IntegerPercentageRegex,
                val='PerNum'),
            ReVal(
                re=ChineseNumeric.IntegerPercentageWithMultiplierRegex,
                val='PerNum'),
            ReVal(
                re=ChineseNumeric.NumbersFractionPercentageRegex,
                val='PerNum'),
            ReVal(
                re=ChineseNumeric.SimpleIntegerPercentageRegex,
                val='PerNum'),
            ReVal(
                re=ChineseNumeric.NumbersFoldsPercentageRegex,
                val='PerSpe'),
            ReVal(
                re=ChineseNumeric.FoldsPercentageRegex,
                val='PerSpe'),
            ReVal(
                re=ChineseNumeric.SimpleFoldsPercentageRegex,
                val='PerSpe'),
            ReVal(
                re=ChineseNumeric.SpecialsPercentageRegex,
                val='PerSpe'),
            ReVal(
                re=ChineseNumeric.NumbersSpecialsPercentageRegex,
                val='PerSpe'),
            ReVal(
                re=ChineseNumeric.SimpleSpecialsPercentageRegex,
                val='PerSpe'),
            ReVal(
                re=ChineseNumeric.SpecialsFoldsPercentageRegex,
                val='PerSpe')
        ]