#  Copyright (c) Microsoft Corporation. All rights reserved.
#  Licensed under the MIT License.

from typing import List
from enum import Enum

from recognizers_number.number.extractors import ReVal, ReRe, BaseNumberExtractor
from recognizers_text.utilities import RegExpUtility
from recognizers_number.number.constants import Constants
from recognizers_number.resources.chinese_numeric import ChineseNumeric
from recognizers_number.number.models import NumberMode


class ChineseNumberExtractorMode(Enum):
    DEFAULT = 0
    EXTRACT_ALL = 1


class ChineseNumberExtractor(BaseNumberExtractor):
    @property
    def regexes(self) -> List[ReVal]:
        return self.__regexes

    @property
    def _extract_type(self) -> str:
        return Constants.SYS_NUM

    @property
    def ambiguity_filters_dict(self) -> List[ReRe]:
        return self.__ambiguity_filters_dict

    def __init__(self, mode: ChineseNumberExtractorMode = ChineseNumberExtractorMode.DEFAULT):
        self.__regexes: List[ReVal] = list()

        cardinal_ex = ChineseCardinalExtractor(mode)
        self.__regexes.extend(cardinal_ex.regexes)

        fraction_ex = ChineseFractionExtractor()
        self.__regexes.extend(fraction_ex.regexes)

        ambiguity_filters_dict: List[ReRe] = list()

        if mode != NumberMode.Unit:
            for key, value in ChineseNumeric.AmbiguityFiltersDict.items():
                ambiguity_filters_dict.append(ReRe(reKey=RegExpUtility.get_safe_reg_exp(key),
                                                   reVal=RegExpUtility.get_safe_reg_exp(value)))
        self.__ambiguity_filters_dict = ambiguity_filters_dict


class ChineseCardinalExtractor(BaseNumberExtractor):
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


class ChineseIntegerExtractor(BaseNumberExtractor):
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
                val=f'Integer{ChineseNumeric.LangMarker}'),
            ReVal(
                re=RegExpUtility.get_safe_reg_exp(
                    ChineseNumeric.NumbersWithDozen),
                val=f'Integer{ChineseNumeric.LangMarker}'),
            ReVal(
                re=RegExpUtility.get_safe_reg_exp(
                    ChineseNumeric.HalfUnitRegex),
                val=f'Integer{ChineseNumeric.LangMarker}')
        ]
        if mode == ChineseNumberExtractorMode.DEFAULT:
            self.__regexes.append(
                ReVal(
                    re=RegExpUtility.get_safe_reg_exp(
                        ChineseNumeric.NumbersWithAllowListRegex),
                    val=f'Integer{ChineseNumeric.LangMarker}'
                )
            )
        elif mode == ChineseNumberExtractorMode.EXTRACT_ALL:
            self.__regexes.append(
                ReVal(
                    re=RegExpUtility.get_safe_reg_exp(
                        ChineseNumeric.NumbersAggressiveRegex),
                    val=f'Integer{ChineseNumeric.LangMarker}'
                )
            )


class ChineseDoubleExtractor(BaseNumberExtractor):
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
                val=f'Double{ChineseNumeric.LangMarker}'),
            ReVal(
                re=RegExpUtility.get_safe_reg_exp(
                    ChineseNumeric.DoubleAllFloatRegex),
                val=f'Double{ChineseNumeric.LangMarker}'),
            ReVal(
                re=RegExpUtility.get_safe_reg_exp(
                    ChineseNumeric.DoubleExponentialNotationRegex),
                val='DoublePow'),
            ReVal(
                re=RegExpUtility.get_safe_reg_exp(
                    ChineseNumeric.DoubleScientificNotationRegex),
                val='DoublePow')
        ]


class ChineseFractionExtractor(BaseNumberExtractor):
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
                val=f'Frac{ChineseNumeric.LangMarker}')
        ]


class ChineseOrdinalExtractor(BaseNumberExtractor):
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
                val=f'Ordinal{ChineseNumeric.LangMarker}'),
            ReVal(
                re=RegExpUtility.get_safe_reg_exp(
                    ChineseNumeric.OrdinalNumbersRegex),
                val=f'Ordinal{ChineseNumeric.LangMarker}')
        ]


class ChinesePercentageExtractor(BaseNumberExtractor):
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
                val=f'Per{ChineseNumeric.LangMarker}'),
            ReVal(
                re=RegExpUtility.get_safe_reg_exp(
                    ChineseNumeric.SimplePercentageRegex),
                val=f'Per{ChineseNumeric.LangMarker}'),
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
