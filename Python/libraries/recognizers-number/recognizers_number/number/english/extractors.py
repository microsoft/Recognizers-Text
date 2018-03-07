import re
from typing import Pattern, List, NamedTuple

from recognizers_number.resources.english_numeric import EnglishNumeric
from recognizers_number.number.extractors import BaseNumberExtractor, re_val

class EnglishNumberExtractor(BaseNumberExtractor):
    def regexes(self) -> List[NamedTuple('re_val', [('re', Pattern), ('val', str)])]:
        return self.__regexes

    def _extract_type(self) -> str:
        return 'SYS_NUM'

    def _negative_number_terms(self) -> Pattern:
        return self.__negative_number_terms

    def __init__(self, mode):
        super()
        self.__negative_number_terms=re.compile(EnglishNumeric.NegativeNumberTermsRegex)
        self.__regexes=list()
        cardinal_ex=None

        # TODO switch mode

        if cardinal_ex is None:
            cardinal_ex=EnglishCardinalExtractor(None)
        for regex in list(cardinal_ex.regexes):
            self.__regexes.append(regex)
        
        fraction_ex=EnglishFractionExtractor()
        for regex in list(fraction_ex.regexes):
            self.__regexes.append(regex)

class EnglishCardinalExtractor(BaseNumberExtractor):
    def regexes(self) -> List[NamedTuple('re_val', [('re', Pattern), ('val', str)])]:
        return self.__regexes

    def _extract_type(self) -> str:
        return 'SYS_NUM_CARDINAL'

    def _negative_number_terms(self): pass

    def __init__(self, placeholder):
        super()
        self.__regexes: List[NamedTuple('re_val', [('re', Pattern), ('val', str)])]

        # Add integer regexes
        integer_ex=EnglishIntegerExtractor(placeholder)
        for regex in list(integer_ex.regexes):
            self.__regexes.append(regex)
            
        # Add double regexes
        double_ex=EnglishDoubleExtractor(placeholder)
        for regex in list(double_ex.regexes):
            self.__regexes.append(regex)

class EnglishIntegerExtractor(BaseNumberExtractor):
    def regexes(self) -> List[NamedTuple('re_val', [('re', Pattern), ('val', str)])]:
        return self.__regexes

    def _extract_type(self) -> str:
        return 'SYS_NUM_INTEGER'

    def _negative_number_terms(self): pass

    def __init__(self, placeholder):
        super()
        self.__regexes=list([
            re_val(
                re=EnglishNumeric.NumbersWithPlaceHolder,
                val='IntegerNum'),
            re_val(
                re=EnglishNumeric.NumbersWithSuffix,
                val='IntegerNum'),
            re_val(
                re=None,
                val='IntegerNum'),
            re_val(
                re=EnglishNumeric.RoundNumberIntegerRegexWithLocks,
                val='IntegerNum'),
            re_val(
                re=EnglishNumeric.NumbersWithDozenSuffix,
                val='IntegerNum'),
            re_val(
                re=EnglishNumeric.AllIntRegexWithLocks,
                val='IntegerEng'),
            re_val(
                re=EnglishNumeric.AllIntRegexWithDozenSuffixLocks,
                val='IntegerEng')
        ])

class EnglishDoubleExtractor(BaseNumberExtractor):
    def regexes(self) -> List[NamedTuple('re_val', [('re', Pattern), ('val', str)])]:
        return self.__regexes

    def _extract_type(self) -> str:
        return 'SYS_NUM_DOUBLE'

    def _negative_number_terms(self): pass

    def __init__(self, placeholder):
        super()
        self.__regexes=list([
            re_val(
                re=EnglishNumeric.DoubleDecimalPointRegex(placeholder),
                val='DoubleNum'),
            re_val(
                re=EnglishNumeric.DoubleWithoutIntegralRegex(placeholder),
                val='DoubleNum'),
            re_val(
                re=None,
                val='DoubleNum'),
            re_val(
                re=EnglishNumeric.DoubleWithMultiplierRegex,
                val='DoubleNum'),
            re_val(
                re=EnglishNumeric.DoubleWithRoundNumber,
                val='DoubleNum'),
            re_val(
                re=EnglishNumeric.DoubleAllFloatRegex,
                val='DoubleEng'),
            re_val(
                re=EnglishNumeric.DoubleExponentialNotationRegex,
                val='DoublePow'),
            re_val(
                re=EnglishNumeric.DoubleCaretExponentialNotationRegex,
                val='DoublePow')
        ])

class EnglishFractionExtractor(BaseNumberExtractor):
    def regexes(self) -> List[NamedTuple('re_val', [('re', Pattern), ('val', str)])]:
        return self.__regexes

    def _extract_type(self) -> str:
        return 'SYS_NUM_FRACTION'

    def _negative_number_terms(self): pass

    def __init__(self):
        super()
        self.__regexes=list([
            re_val(
                re=EnglishNumeric.FractionNotationWithSpacesRegex,
                val='FracNum'),
            re_val(
                re=EnglishNumeric.FractionNotationRegex,
                val='FracNum'),
            re_val(
                re=EnglishNumeric.FractionNounRegex,
                val='FracEng'),
            re_val(
                re=EnglishNumeric.FractionNounWithArticleRegex,
                val='FracEng'),
            re_val(
                re=EnglishNumeric.FractionPrepositionRegex,
                val='FracEng')
        ])

class EnglishOrdinalExtractor(BaseNumberExtractor):
    def regexes(self) -> List[NamedTuple('re_val', [('re', Pattern), ('val', str)])]:
        return self.__regexes

    def _extract_type(self) -> str:
        return 'SYS_NUM_ORDINAL'

    def _negative_number_terms(self): pass

    def __init__(self):
        super()
        self.__regexes=list([
            re_val(
                re=EnglishNumeric.OrdinalSuffixRegex,
                val='OrdinalNum'),
            re_val(
                re=EnglishNumeric.OrdinalNumericRegex,
                val='OrdinalNum'),
            re_val(
                re=EnglishNumeric.OrdinalEnglishRegex,
                val='OrdEng'),
            re_val(
                re=EnglishNumeric.OrdinalRoundNumberRegex,
                val='OrdEng')
        ])
