from re import Pattern
from typing import List, Optional

import regex

from recognizers_text.utilities import RegExpUtility
from recognizers_number.number.extractors import ReVal, ReRe, BaseNumberExtractor, BasePercentageExtractor
from recognizers_number.number.models import NumberMode, LongFormatMode
from recognizers_number.number.number_options import NumberOptions
from recognizers_number.resources.arabic_numeric import ArabicNumeric
from recognizers_number.number.constants import Constants


class ArabicNumberExtractor(BaseNumberExtractor):

    @property
    def ambiguity_filters_dict(self) -> List[ReRe]:
        return self.__ambiguity_filters_dict

    @property
    def ambiguous_fraction_connectors(self) -> Pattern:
        return self._ambiguous_fraction_connectors

    @property
    def regexes(self) -> List[ReVal]:
        return self.__regexes

    @property
    def relative_reference(self) -> Pattern:
        return self._relative_reference

    @property
    def _extract_type(self) -> str:
        return Constants.SYS_NUM

    @property
    def _negative_number_terms(self) -> Pattern:
        return self.__negative_number_terms

    def __init__(self, mode: NumberMode = NumberMode.DEFAULT):
        self._ambiguous_fraction_connectors = (
            RegExpUtility.get_safe_reg_exp(ArabicNumeric.AmbiguousFractionConnectorsRegex))
        self._relative_reference = RegExpUtility.get_safe_reg_exp(ArabicNumeric.RelativeOrdinalRegex)
        self.__negative_number_terms = RegExpUtility.get_safe_reg_exp(ArabicNumeric.NegativeNumberTermsRegex)
        self.__regexes: List[ReVal] = list()

        # Add Cardinal
        cardinal_ex: Optional[ArabicCardinalExtractor] = None
        if mode is NumberMode.PURE_NUMBER:
            cardinal_ex = ArabicCardinalExtractor(ArabicNumeric.PlaceHolderDefault)
        elif mode is NumberMode.CURRENCY:
            self.__regexes.append(
                ReVal(re=RegExpUtility.get_safe_reg_exp(ArabicNumeric.CurrencyRegex), val='IntegerNum'))

        if not cardinal_ex:
            cardinal_ex = ArabicCardinalExtractor()

        self.__regexes.extend(cardinal_ex.regexes)

        # Add Fraction
        fraction_ex = ArabicFractionExtractor(mode)
        self.regexes.extend(fraction_ex.regexes)

        # Do not filter the ambiguous number cases like 'that one' in NumberWithUnit, otherwise they can't be resolved.
        ambiguity_filters_dict: List[ReRe] = list()

        if mode is not NumberMode.Unit:
            for key, value in ArabicNumeric.AmbiguityFiltersDict.items():
                ambiguity_filters_dict.append(
                    ReRe(reKey=RegExpUtility.get_safe_reg_exp(key), reVal=RegExpUtility.get_safe_reg_exp(value))
                )
        self.__ambiguity_filters_dict = ambiguity_filters_dict


class ArabicCardinalExtractor(BaseNumberExtractor):

    @property
    def regexes(self) -> List[ReVal]:
        return self.__regexes

    @property
    def _extract_type(self) -> str:
        return Constants.SYS_NUM_CARDINAL

    def __init__(self, placeholder: str = ArabicNumeric.PlaceHolderDefault):
        self.__regexes: List[ReVal] = list()

        # Add Integer Regexes
        int_extract = ArabicIntegerExtractor(placeholder)
        self.__regexes.extend(int_extract.regexes)

        # Add Double Regexes
        dou_extract = ArabicDoubleExtractor(placeholder)
        self.__regexes.extend(dou_extract.regexes)


class ArabicIntegerExtractor(BaseNumberExtractor):

    @property
    def regexes(self) -> List[ReVal]:
        return self.__regexes

    @property
    def _extract_type(self) -> str:
        return Constants.SYS_NUM_INTEGER

    def __init__(self, placeholder: str = ArabicNumeric.PlaceHolderDefault):
        self.__regexes = [
            ReVal(
                re=RegExpUtility.get_safe_reg_exp(
                    ArabicNumeric.NumbersWithPlaceHolder(placeholder)
                ),
                val='IntegerNum'
            ),
            ReVal(
                re=RegExpUtility.get_safe_reg_exp(
                    ArabicNumeric.NumbersWithSuffix, regex.S
                ),
                val='IntegerNum'
            ),
            ReVal(
                re=RegExpUtility.get_safe_reg_exp(
                    ArabicNumeric.RoundNumberIntegerRegexWithLocks
                ),
                val='IntegerNum'
            ),
            ReVal(
                re=RegExpUtility.get_safe_reg_exp(
                    ArabicNumeric.NumbersWithDozenSuffix
                ),
                val='IntegerNum'
            ),
            ReVal(
                re=RegExpUtility.get_safe_reg_exp(
                    ArabicNumeric.AllIntRegexWithLocks
                ),
                val='IntegerNum'
            ),
            ReVal(
                re=RegExpUtility.get_safe_reg_exp(
                    ArabicNumeric.AllIntRegexWithDozenSuffixLocks
                ),
                val='IntegerNum'
            ),
            ReVal(
                re=RegExpUtility.get_safe_reg_exp(
                    self._generate_format_regex(LongFormatMode.INTEGER_COMMA, placeholder)
                ),
                val='IntegerNum'
            ),
            ReVal(
                re=RegExpUtility.get_safe_reg_exp(
                    self._generate_format_regex(LongFormatMode.INTEGER_DOT, placeholder)
                ),
                val='IntegerNum'
            ),
            ReVal(
                re=RegExpUtility.get_safe_reg_exp(
                    self._generate_format_regex(LongFormatMode.INTEGER_BLANK, placeholder)
                ),
                val='IntegerNum'
            ),
            ReVal(
                re=RegExpUtility.get_safe_reg_exp(
                    self._generate_format_regex(LongFormatMode.INTEGER_NO_BREAK_SPACE, placeholder)
                ),
                val='IntegerNum'
            ),
        ]


class ArabicDoubleExtractor(BaseNumberExtractor):

    @property
    def regexes(self) -> List[ReVal]:
        return self.__regexes

    @property
    def _extract_type(self) -> str:
        return Constants.SYS_NUM_DOUBLE

    def __init__(self, placeholder: str):
        self.__regexes = [
            ReVal(
                re=RegExpUtility.get_safe_reg_exp(
                    ArabicNumeric.DoubleDecimalPointRegex(placeholder)
                ),
                val='DoubleNum'
            ),
            ReVal(
                re=RegExpUtility.get_safe_reg_exp(
                    ArabicNumeric.DoubleWithoutIntegralRegex(placeholder)
                ),
                val='DoubleNum'
            ),
            ReVal(
                re=RegExpUtility.get_safe_reg_exp(
                    ArabicNumeric.DoubleWithMultiplierRegex
                ),
                val='DoubleNum'
            ),
            ReVal(
                re=RegExpUtility.get_safe_reg_exp(
                    ArabicNumeric.DoubleWithRoundNumber
                ),
                val='DoubleNum'
            ),
            ReVal(
                re=RegExpUtility.get_safe_reg_exp(
                    ArabicNumeric.DoubleAllFloatRegex
                ),
                val='DoubleNum'
            ),
            ReVal(
                re=RegExpUtility.get_safe_reg_exp(
                    ArabicNumeric.DoubleExponentialNotationRegex
                ),
                val='DoubleNum'
            ),
            ReVal(
                re=RegExpUtility.get_safe_reg_exp(
                    ArabicNumeric.DoubleCaretExponentialNotationRegex
                ),
                val='DoubleNum'
            ),
            ReVal(
                re=RegExpUtility.get_safe_reg_exp(
                    self._generate_format_regex(LongFormatMode.DOUBLE_COMMA_DOT, placeholder)
                ),
                val='DoubleNum'
            ),
            ReVal(
                re=RegExpUtility.get_safe_reg_exp(
                    self._generate_format_regex(LongFormatMode.DOUBLE_NUM_BLANK_DOT, placeholder)
                ),
                val='DoubleNum'
            ),
            ReVal(
                re=RegExpUtility.get_safe_reg_exp(
                    self._generate_format_regex(LongFormatMode.DOUBLE_NO_BREAK_SPACE_DOT, placeholder)
                ),
                val='DoubleNum'
            ),
            ReVal(
                re=RegExpUtility.get_safe_reg_exp(
                    ArabicNumeric.DoubleWithThousandMarkRegex(placeholder)
                ),
                val='DoubleNum'
            ),
        ]


class ArabicFractionExtractor(BaseNumberExtractor):
    @property
    def regexes(self) -> List[ReVal]:
        return self.__regexes

    @property
    def _extract_type(self) -> str:
        return Constants.SYS_NUM_FRACTION

    def __init__(self, mode):
        self.__regexes = [
            ReVal(
                re=RegExpUtility.get_safe_reg_exp(ArabicNumeric.FractionNotationWithSpacesRegex),
                val='FracNum'
            ),
            ReVal(
                re=RegExpUtility.get_safe_reg_exp(ArabicNumeric.FractionNotationWithSpacesRegex2),
                val='FracNum'
            ),
            ReVal(
                re=RegExpUtility.get_safe_reg_exp(ArabicNumeric.FractionNotationRegex),
                val='FracNum'
            ),
            ReVal(
                re=RegExpUtility.get_safe_reg_exp(ArabicNumeric.FractionNounRegex),
                val=f'Frac{ArabicNumeric.LangMarker}'
            ),
            ReVal(
                re=RegExpUtility.get_safe_reg_exp(ArabicNumeric.FractionNounWithArticleRegex),
                val=f'Frac{ArabicNumeric.LangMarker}'
            ),
            ReVal(
                re=RegExpUtility.get_safe_reg_exp(ArabicNumeric.FractionWithOrdinalPrefix),
                val=f'Frac{ArabicNumeric.LangMarker}'
            ),
            ReVal(
                re=RegExpUtility.get_safe_reg_exp(ArabicNumeric.FractionWithPartOfPrefix),
                val=f'Frac{ArabicNumeric.LangMarker}'
            )
        ]

        # Not add FractionPrepositionRegex when the mode is Unit to avoid wrong recognize cases like "$1000 over 3"
        if mode is not NumberMode.Unit:
            if NumberOptions.PERCENTAGE_MODE:
                self.__regexes.append(
                    ReVal(
                        re=RegExpUtility.get_safe_reg_exp(ArabicNumeric.FractionPrepositionWithinPercentModeRegex),
                        val=f'Frac{ArabicNumeric.LangMarker}'
                    )
                )
            else:
                self.__regexes.append(
                    ReVal(
                        re=RegExpUtility.get_safe_reg_exp(ArabicNumeric.FractionPrepositionRegex),
                        val=f'Frac{ArabicNumeric.LangMarker}'
                    )
                )


class ArabicOrdinalExtractor(BaseNumberExtractor):

    @property
    def ambiguous_fraction_connectors(self):
        return self._ambiguous_fraction_connectors

    @property
    def regexes(self) -> List[ReVal]:
        return self.__regexes

    @property
    def relative_reference(self):
        return self._relative_reference

    @property
    def _extract_type(self) -> str:
        return Constants.SYS_NUM_ORDINAL

    def __init__(self):
        self._ambiguous_fraction_connectors = (
            RegExpUtility.get_safe_reg_exp(ArabicNumeric.AmbiguousFractionConnectorsRegex))
        self._relative_reference = RegExpUtility.get_safe_reg_exp(ArabicNumeric.RelativeOrdinalRegex)

        self.__regexes: List[ReVal] = [
            ReVal(
                re=RegExpUtility.get_safe_reg_exp(ArabicNumeric.OrdinalNumericRegex),
                val='OrdinalNum'
            ),
            ReVal(
                re=RegExpUtility.get_safe_reg_exp(ArabicNumeric.OrdinalEnglishRegex),
                val='OrdinalNum'
            ),
            ReVal(
                re=RegExpUtility.get_safe_reg_exp(ArabicNumeric.OrdinalRoundNumberRegex),
                val='OrdinalNum'
            ),
        ]


class ArabicPercentageExtractor(BasePercentageExtractor):

    def get_definitions(self) -> List[str]:
        regex_strs = [
            ArabicNumeric.NumberWithSuffixPercentage,
            ArabicNumeric.NumberWithPrefixPercentage
        ]

        if NumberOptions.PERCENTAGE_MODE:
            regex_strs.append(ArabicNumeric.FractionNumberWithSuffixPercentage)
            regex_strs.append(ArabicNumeric.NumberWithPrepositionPercentage)

        return regex_strs

    def __init__(self):
        super().__init__(ArabicNumberExtractor(NumberMode.DEFAULT))
