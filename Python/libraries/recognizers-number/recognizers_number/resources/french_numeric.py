# ------------------------------------------------------------------------------
# <auto-generated>
#     This code was generated by a tool.
#     Changes to this file may cause incorrect behavior and will be lost if
#     the code is regenerated.
# </auto-generated>
#
# Copyright (c) Microsoft Corporation. All rights reserved.
# Licensed under the MIT License.
# ------------------------------------------------------------------------------

from .base_numbers import BaseNumbers
# pylint: disable=line-too-long


class FrenchNumeric:
    LangMarker = 'Fre'
    CompoundNumberLanguage = False
    MultiDecimalSeparatorCulture = True
    RoundNumberIntegerRegex = f'(cent|mille|millions?|milliards?|billions?)'
    ZeroToNineIntegerRegex = f'(une?|deux|trois|quatre|cinq|six|sept|huit|neuf)'
    TenToNineteenIntegerRegex = f'((seize|quinze|quatorze|treize|douze|onze)|dix(\\Wneuf|\\Whuit|\\Wsept)?)'
    TensNumberIntegerRegex = f'(quatre\\Wvingt(s|\\Wdix)?|soixante\\Wdix|vingt|trente|quarante|cinquante|soixante|septante|octante|huitante|nonante)'
    DigitsNumberRegex = f'\\d|\\d{{1,3}}(\\.\\d{{3}})'
    NegativeNumberTermsRegex = f'^[.]'
    NegativeNumberSignRegex = f'^({NegativeNumberTermsRegex}\\s+).*'
    HundredsNumberIntegerRegex = f'(({ZeroToNineIntegerRegex}(\\s+cent))|cent|((\\s+cent\\s)+{TensNumberIntegerRegex}))'
    BelowHundredsRegex = f'(({TenToNineteenIntegerRegex}|({TensNumberIntegerRegex}((-|(\\s+et)?\\s+)({TenToNineteenIntegerRegex}|{ZeroToNineIntegerRegex}))?))|{ZeroToNineIntegerRegex})'
    BelowThousandsRegex = f'(({HundredsNumberIntegerRegex}(\\s+{BelowHundredsRegex})?|{BelowHundredsRegex}|{TenToNineteenIntegerRegex})|cent\\s+{TenToNineteenIntegerRegex})'
    SupportThousandsRegex = f'(({BelowThousandsRegex}|{BelowHundredsRegex})\\s+{RoundNumberIntegerRegex}(\\s+{RoundNumberIntegerRegex})?)'
    SeparaIntRegex = f'({SupportThousandsRegex}(\\s+{SupportThousandsRegex})*(\\s+{BelowThousandsRegex})?|{BelowThousandsRegex})'
    AllIntRegex = f'({SeparaIntRegex}|mille(\\s+{BelowThousandsRegex})?)'

    def NumbersWithPlaceHolder(placeholder):
        return f'(((?<!\\d+\\s*)-\\s*)|(?<=\\b))\\d+(?!([,\\.]\\d+[a-zA-Z]))(?={placeholder})'
    NumbersWithSuffix = f'(((?<=\\W|^)-\\s*)|(?<=\\b))\\d+\\s*{BaseNumbers.NumberMultiplierRegex}(?=\\b)'
    RoundNumberIntegerRegexWithLocks = f'(?<=\\b)({DigitsNumberRegex})+\\s+{RoundNumberIntegerRegex}(?=\\b)'
    NumbersWithDozenSuffix = f'(((?<!\\d+\\s*)-\\s*)|(?<=\\b))\\d+\\s+douzaine(s)?(?=\\b)'
    AllIntRegexWithLocks = f'((?<=\\b){AllIntRegex}(?=\\b))'
    AllIntRegexWithDozenSuffixLocks = f'(?<=\\b)(((demi\\s+)?\\s+douzaine)|({AllIntRegex}\\s+douzaines?))(?=\\b)'
    SimpleRoundOrdinalRegex = f'(centi|[bm]illioni|milli(ardi)?)[eè]me'
    OneToNineOrdinalRegex = f'(premi[eè]re?|second[e]|tier(s|ce)|(deuxi|troisi|quatri|cinqui|sixi|septi|hui[tr]i|neuvi)[eè]me)'
    SpecialUnderHundredOrdinalRegex = f'(di[xz]i|onzi|douzi|treizi|quatorzi|quinzi|seizi|dix[\\s-](septi|huiri|neuvi))[eè]me'
    TensOrdinalRegex = f'(quatre-vingt-di[xz]i[eè]me|quatre-vingti[eè]me|huitanti[eè]me|octanti[eè]me|soixante-dixi[eè]me|septanti[eè]me|soixanti[eè]me|cinquanti[eè]me|quaranti[eè]me|trenti[eè]me|vingti[eè]me)'
    HundredOrdinalRegex = f'({AllIntRegex}(\\s+(centi[eè]me)))'
    UnderHundredOrdinalRegex = f'(((({AllIntRegex}|{TensNumberIntegerRegex})(\\W)?)?({OneToNineOrdinalRegex}|\\s+et\\s+uni[eè]me))|{SpecialUnderHundredOrdinalRegex}|{TensOrdinalRegex})'
    UnderThousandOrdinalRegex = f'((({HundredOrdinalRegex}(\\s|-)?)?{UnderHundredOrdinalRegex})|(({AllIntRegex}(\\W)?)?{SimpleRoundOrdinalRegex})|{HundredOrdinalRegex})'
    OverThousandOrdinalRegex = f'(({AllIntRegex})(-i[eè]me))'
    ComplexOrdinalRegex = f'(({OverThousandOrdinalRegex}(\\s)?)?{UnderThousandOrdinalRegex}|{OverThousandOrdinalRegex}|{UnderHundredOrdinalRegex})'
    SuffixOrdinalRegex = f'(({AllIntRegex})({SimpleRoundOrdinalRegex}))'
    ComplexRoundOrdinalRegex = f'((({SuffixOrdinalRegex}(\\s)?)?{ComplexOrdinalRegex})|{SuffixOrdinalRegex})'
    AllOrdinalRegex = f'({ComplexOrdinalRegex}|{SimpleRoundOrdinalRegex}|{ComplexRoundOrdinalRegex})'
    PlaceHolderPureNumber = f'\\b'
    PlaceHolderDefault = f'\\D|\\b'
    OrdinalSuffixRegex = f'(?<=\\b)((\\d*(11e(me)?|1[eè]re?|[02-9]e(me)?)))(?=\\b)'
    OrdinalFrenchRegex = f'(?<=\\b){AllOrdinalRegex}(?=\\b)'
    FractionNotationWithSpacesRegex = f'(((?<=\\W|^)-\\s*)|(?<=\\b))\\d+\\s+\\d+[/]\\d+(?=(\\b[^/]|$))'
    FractionNotationRegex = f'{BaseNumbers.FractionNotationRegex}'
    FractionNounRegex = f'(?<=\\b)({AllIntRegex}\\s+((et)\\s+)?)?({AllIntRegex})(\\s+((et)\\s)?)((({AllOrdinalRegex})s?|({SuffixOrdinalRegex})s?)|demi[es]?|tiers?|quarts?)(?=\\b)'
    FractionNounWithArticleRegex = f'(?<=\\b)({AllIntRegex}\\s+(et\\s+)?)?(une?)(\\s+)(({AllOrdinalRegex})|({SuffixOrdinalRegex})|(et\\s+)?demi[es]?)(?=\\b)'
    FractionPrepositionRegex = f'(?<!{BaseNumbers.CommonCurrencySymbol}\\s*)(?<=\\b)(?<numerator>({AllIntRegex})|((?<!\\.)\\d+))\\s+sur\\s+(?<denominator>({AllIntRegex})|((\\d+)(?!\\.)))(?=\\b)'
    AllPointRegex = f'((\\s+{ZeroToNineIntegerRegex})+|(\\s+{SeparaIntRegex}))'
    AllFloatRegex = f'({AllIntRegex}(\\s+(virgule|point)){AllPointRegex})'

    def DoubleDecimalPointRegex(placeholder):
        return f'(((?<!\\d+\\s*)-\\s*)|((?<=\\b)(?<!\\d+[,\\.])))\\d+[,\\.]\\d+(?!([,\\.]\\d+))(?={placeholder})'

    def DoubleWithoutIntegralRegex(placeholder):
        return f'(?<=\\s|^)(?<!(\\d+))[,\\.]\\d+(?!([,\\.]\\d+))(?={placeholder})'
    DoubleWithMultiplierRegex = f'(((?<!\\d+\\s*)-\\s*)|((?<=\\b)(?<!\\d+\\[,\\.])))\\d+[,\\.]\\d+\\s*{BaseNumbers.NumberMultiplierRegex}(?=\\b)'
    DoubleWithRoundNumber = f'(((?<!\\d+\\s*)-\\s*)|((?<=\\b)(?<!\\d+\\[,\\.])))\\d+[,\\.]\\d+\\s+{RoundNumberIntegerRegex}(?=\\b)'
    DoubleAllFloatRegex = f'((?<=\\b){AllFloatRegex}(?=\\b))'
    DoubleExponentialNotationRegex = f'(((?<!\\d+\\s*)-\\s*)|((?<=\\b)(?<!\\d+[,\\.])))(\\d+([,\\.]\\d+)?)e([+-]*[1-9]\\d*)(?=\\b)'
    DoubleCaretExponentialNotationRegex = f'(((?<!\\d+\\s*)-\\s*)|((?<=\\b)(?<!\\d+[,\\.])))(\\d+([,\\.]\\d+)?)\\^([+-]*[1-9]\\d*)(?=\\b)'
    NumberWithSuffixPercentage = f'(?<!%)({BaseNumbers.NumberReplaceToken})(\\s*)(%(?!{BaseNumbers.NumberReplaceToken})|(pourcent(s|tages?)?)\\b)'
    NumberWithPrefixPercentage = f'((?<!{BaseNumbers.NumberReplaceToken})%|pourcent|pourcent des|pourcentage de)(\\s*)({BaseNumbers.NumberReplaceToken})(?=\\s|$)'
    TillRegex = f'((?<!\\b[èe]ga(l|ux)\\s+)[àa]|--|-|—|——|~|–)'
    MoreRegex = f'(?:(bigger|greater|more|plus(\\s+(haut|grand|âgée?))?|sup[ée]rieur)(\\s+([àa]|que))?|dépassant|(au-dessus|(a\\s+)?plus|dépassant|au-delà)\\s+de|exceed(ed|ing)?|surpass(ed|ing)?|(?<!<|=)>)'
    LessRegex = f'(?:(less|plus\\s+(bas|petit|jeune)|moins|inf[ée]rieur)(\\s+([àa]|de|que))?|below|under|(?<!>|=)<)'
    EqualRegex = f'([ée]ga(l|ux)(\\s+[àa])?|(?<!<|>)=)'
    MoreOrEqualPrefix = f'((pas\\s+{LessRegex})|(au\\s+moins|[àa] partir de))'
    MoreOrEqual = f'(?:({MoreRegex}\\s+(ou)?\\s+{EqualRegex})|({EqualRegex}\\s+(ou)?\\s+{MoreRegex})|{MoreOrEqualPrefix}(\\s+(ou)?\\s+{EqualRegex})?|({EqualRegex}\\s+(ou)?\\s+)?{MoreOrEqualPrefix}|>\\s*=|≥)'
    MoreOrEqualSuffix = f'((et|ou)\\s+(((more|greater|higher|plus(\\s+grand)?|sup[ée]rieur)((?!\\s+([àa]|que))|(\\s+([àa]|que)(?!((\\s+ou\\s+[èe]ga(l|ux)\\s+[àa])?\\s*\\d+)))))|((a plus de|au-dessus de)(?!\\s+than))))'
    LessOrEqualPrefix = f'((pas\\s+{MoreRegex})|(au\\s+plus)|(jusqu\'[àa]))'
    LessOrEqual = f'(({LessRegex}\\s+(ou)?\\s+{EqualRegex})|({EqualRegex}\\s+(ou)?\\s+{LessRegex})|{LessOrEqualPrefix}(\\s+(ou)?\\s+{EqualRegex})?|({EqualRegex}\\s+(ou)?\\s+)?{LessOrEqualPrefix}|<\\s*=|≤)'
    LessOrEqualSuffix = f'((et|ou)\\s+(less|lower|plus petit|moins|inf[ée]rieur)((?!\\s+([àa]|de|que))|(\\s+([àa]|de|que)(?!(\\s*\\d+)))))'
    NumberSplitMark = f'(?![,.](?!\\d+))(?!\\s*\\b(et\\s+({LessRegex}|{MoreRegex})|mais|ou|to)\\b)'
    MoreRegexNoNumberSucceed = f'((bigger|greater|more|plus(\\s+grand)?|sup[ée]rieur)((?!\\s+([àa]|que))|\\s+(([àa]|que)(?!(\\s*\\d+))))|((au-dessus|a plus)\\s+de)(?!(\\s*\\d+)))'
    LessRegexNoNumberSucceed = f'((less|lower|plus petit|moins|inf[ée]rieur)((?!\\s+([àa]|de|que))|\\s+(([àa]|de|que)(?!(\\s*\\d+))))|(below|under)(?!(\\s*\\d+)))'
    EqualRegexNoNumberSucceed = f'([èe]ga(l|ux)((?!\\s+([àa]))|(\\s+([àa]|que)(?!(\\s*\\d+)))))'
    OneNumberRangeMoreRegex1 = f'({MoreOrEqual}|{MoreRegex})\\s*(l[ae]\\s+)?(?<number1>({NumberSplitMark}.)+)'
    OneNumberRangeMoreRegex1LB = f'(?<!pas\\s+){OneNumberRangeMoreRegex1}'
    OneNumberRangeMoreRegex2 = f'(?<number1>({NumberSplitMark}.)+)\\s*{MoreOrEqualSuffix}'
    OneNumberRangeMoreSeparateRegex = f'({EqualRegex}\\s+(?<number1>({NumberSplitMark}.)+)(\\s+ou\\s+){MoreRegexNoNumberSucceed})|({MoreRegex}\\s+(?<number1>({NumberSplitMark}.)+)(\\s+ou\\s+){EqualRegexNoNumberSucceed})'
    OneNumberRangeLessRegex1 = f'({LessOrEqual}|{LessRegex})\\s*(l[ae]\\s+)?(?<number2>({NumberSplitMark}.)+)'
    OneNumberRangeLessRegex1LB = f'(?<!pas\\s+){OneNumberRangeLessRegex1}'
    OneNumberRangeLessRegex2 = f'(?<number2>({NumberSplitMark}.)+)\\s*{LessOrEqualSuffix}'
    OneNumberRangeLessSeparateRegex = f'({EqualRegex}\\s+(?<number1>({NumberSplitMark}.)+)(\\s+ou\\s+){LessRegexNoNumberSucceed})|({LessRegex}\\s+(?<number1>({NumberSplitMark}.)+)(\\s+ou\\s+){EqualRegexNoNumberSucceed})'
    OneNumberRangeEqualRegex = f'(?<!\\b([àa]|que)\\s+ou\\s+){EqualRegex}\\s*(l[ae]\\s+)?(?<number1>({NumberSplitMark}.)+)'
    TwoNumberRangeRegex1 = f'entre\\s*(l[ae]\\s+)?(?<number1>({NumberSplitMark}.)+)\\s*et\\s*(l[ae]\\s+)?(?<number2>({NumberSplitMark}.)+)'
    TwoNumberRangeRegex2 = f'({OneNumberRangeMoreRegex1}|{OneNumberRangeMoreRegex2})\\s*(et|mais|,)\\s*({OneNumberRangeLessRegex1}|{OneNumberRangeLessRegex2})'
    TwoNumberRangeRegex3 = f'({OneNumberRangeLessRegex1}|{OneNumberRangeLessRegex2})\\s*(et|mais|,)\\s*({OneNumberRangeMoreRegex1}|{OneNumberRangeMoreRegex2})'
    TwoNumberRangeRegex4 = f'(de\\s+)?(?<number1>({NumberSplitMark}(?!\\bde\\b).)+)\\s*{TillRegex}\\s*(l[ae]\\s+)?(?<number2>({NumberSplitMark}.)+)'
    DecimalSeparatorChar = ','
    FractionMarkerToken = 'sur'
    NonDecimalSeparatorChar = '.'
    HalfADozenText = 'six'
    WordSeparatorToken = 'et'
    WrittenDecimalSeparatorTexts = [r'virgule']
    WrittenGroupSeparatorTexts = [r'point', r'points']
    WrittenIntegerSeparatorTexts = [r'et', r'-']
    WrittenFractionSeparatorTexts = [r'et', r'sur']
    HalfADozenRegex = f'(?<=\\b)demie?\\s+douzaine'
    DigitalNumberRegex = f'((?<=\\b)(cent|mille|millions?|milliards?|billions?|douzaines?)(?=\\b))|((?<=(\\d|\\b)){BaseNumbers.MultiplierLookupRegex}(?=\\b))'
    AmbiguousFractionConnectorsRegex = f'^[.]'
    CardinalNumberMap = dict([("zéro", 0),
                              ("zero", 0),
                              ("un", 1),
                              ("une", 1),
                              ("deux", 2),
                              ("trois", 3),
                              ("quatre", 4),
                              ("cinq", 5),
                              ("six", 6),
                              ("sept", 7),
                              ("huit", 8),
                              ("neuf", 9),
                              ("dix", 10),
                              ("onze", 11),
                              ("douze", 12),
                              ("douzaine", 12),
                              ("douzaines", 12),
                              ("treize", 13),
                              ("quatorze", 14),
                              ("quinze", 15),
                              ("seize", 16),
                              ("dix-sept", 17),
                              ("dix-huit", 18),
                              ("dix-huir", 18),
                              ("dix-neuf", 19),
                              ("vingt", 20),
                              ("trente", 30),
                              ("quarante", 40),
                              ("cinquante", 50),
                              ("soixante", 60),
                              ("soixante-dix", 70),
                              ("septante", 70),
                              ("quatre-vingts", 80),
                              ("quatre-vingt", 80),
                              ("quatre vingts", 80),
                              ("quatre vingt", 80),
                              ("quatre-vingt-dix", 90),
                              ("quatre-vingt dix", 90),
                              ("quatre vingt dix", 90),
                              ("quatre-vingts-dix", 90),
                              ("quatre-vingts-onze", 91),
                              ("quatre-vingt-onze", 91),
                              ("quatre-vingts-douze", 92),
                              ("quatre-vingt-douze", 92),
                              ("quatre-vingts-treize", 93),
                              ("quatre-vingt-treize", 93),
                              ("quatre-vingts-quatorze", 94),
                              ("quatre-vingt-quatorze", 94),
                              ("quatre-vingts-quinze", 95),
                              ("quatre-vingt-quinze", 95),
                              ("quatre-vingts-seize", 96),
                              ("quatre-vingt-seize", 96),
                              ("huitante", 80),
                              ("octante", 80),
                              ("nonante", 90),
                              ("cent", 100),
                              ("mille", 1000),
                              ("un million", 1000000),
                              ("million", 1000000),
                              ("millions", 1000000),
                              ("un milliard", 1000000000),
                              ("milliard", 1000000000),
                              ("milliards", 1000000000),
                              ("un mille milliards", 1000000000000),
                              ("un billion", 1000000000000)])
    OrdinalNumberMap = dict([("premier", 1),
                             ("première", 1),
                             ("premiere", 1),
                             ("unième", 1),
                             ("unieme", 1),
                             ("deuxième", 2),
                             ("deuxieme", 2),
                             ("second", 2),
                             ("seconde", 2),
                             ("troisième", 3),
                             ("demi", 2),
                             ("demie", 2),
                             ("tiers", 3),
                             ("tierce", 3),
                             ("quart", 4),
                             ("quarts", 4),
                             ("troisieme", 3),
                             ("quatrième", 4),
                             ("quatrieme", 4),
                             ("cinquième", 5),
                             ("cinquieme", 5),
                             ("sixième", 6),
                             ("sixieme", 6),
                             ("septième", 7),
                             ("septieme", 7),
                             ("huitième", 8),
                             ("huitieme", 8),
                             ("huirième", 8),
                             ("huirieme", 8),
                             ("neuvième", 9),
                             ("neuvieme", 9),
                             ("dixième", 10),
                             ("dixieme", 10),
                             ("dizième", 10),
                             ("dizieme", 10),
                             ("onzième", 11),
                             ("onzieme", 11),
                             ("douzième", 12),
                             ("douzieme", 12),
                             ("treizième", 13),
                             ("treizieme", 13),
                             ("quatorzième", 14),
                             ("quatorzieme", 14),
                             ("quinzième", 15),
                             ("quinzieme", 15),
                             ("seizième", 16),
                             ("seizieme", 16),
                             ("dix-septième", 17),
                             ("dix-septieme", 17),
                             ("dix-huitième", 18),
                             ("dix-huitieme", 18),
                             ("dix-huirième", 18),
                             ("dix-huirieme", 18),
                             ("dix-neuvième", 19),
                             ("dix-neuvieme", 19),
                             ("vingtième", 20),
                             ("vingtieme", 20),
                             ("trentième", 30),
                             ("trentieme", 30),
                             ("quarantième", 40),
                             ("quarantieme", 40),
                             ("cinquantième", 50),
                             ("cinquantieme", 50),
                             ("soixantième", 60),
                             ("soixantieme", 60),
                             ("soixante-dixième", 70),
                             ("soixante-dixieme", 70),
                             ("septantième", 70),
                             ("septantieme", 70),
                             ("quatre-vingtième", 80),
                             ("quatre-vingtieme", 80),
                             ("huitantième", 80),
                             ("huitantieme", 80),
                             ("octantième", 80),
                             ("octantieme", 80),
                             ("quatre-vingt-dixième", 90),
                             ("quatre-vingt-dixieme", 90),
                             ("nonantième", 90),
                             ("nonantieme", 90),
                             ("centième", 100),
                             ("centieme", 100),
                             ("millième", 1000),
                             ("millieme", 1000),
                             ("millionième", 1000000),
                             ("millionieme", 1000000),
                             ("milliardième", 1000000000),
                             ("milliardieme", 1000000000),
                             ("billionieme", 1000000000000),
                             ("billionième", 1000000000000),
                             ("trillionième", 1000000000000000000),
                             ("trillionieme", 1000000000000000000)])
    PrefixCardinalMap = dict([("deux", 2),
                              ("trois", 3),
                              ("quatre", 4),
                              ("cinq", 5),
                              ("six", 6),
                              ("sept", 7),
                              ("huit", 8),
                              ("neuf", 9),
                              ("dix", 10),
                              ("onze", 11),
                              ("douze", 12),
                              ("treize", 13),
                              ("quatorze", 14),
                              ("quinze", 15),
                              ("seize", 16),
                              ("dix sept", 17),
                              ("dix-sept", 17),
                              ("dix-huit", 18),
                              ("dix huit", 18),
                              ("dix-neuf", 19),
                              ("dix neuf", 19),
                              ("vingt", 20),
                              ("vingt-et-un", 21),
                              ("vingt et un", 21),
                              ("vingt-deux", 21),
                              ("vingt deux", 22),
                              ("vingt-trois", 23),
                              ("vingt trois", 23),
                              ("vingt-quatre", 24),
                              ("vingt quatre", 24),
                              ("vingt-cinq", 25),
                              ("vingt cinq", 25),
                              ("vingt-six", 26),
                              ("vingt six", 26),
                              ("vingt-sept", 27),
                              ("vingt sept", 27),
                              ("vingt-huit", 28),
                              ("vingt huit", 28),
                              ("vingt-neuf", 29),
                              ("vingt neuf", 29),
                              ("trente", 30),
                              ("quarante", 40),
                              ("cinquante", 50),
                              ("soixante", 60),
                              ("soixante-dix", 70),
                              ("soixante dix", 70),
                              ("septante", 70),
                              ("quatre-vingt", 80),
                              ("quatre vingt", 80),
                              ("huitante", 80),
                              ("octante", 80),
                              ("nonante", 90),
                              ("quatre vingt dix", 90),
                              ("quatre-vingt-dix", 90),
                              ("cent", 100),
                              ("deux cent", 200),
                              ("trois cents", 300),
                              ("quatre cents", 400),
                              ("cinq cent", 500),
                              ("six cent", 600),
                              ("sept cent", 700),
                              ("huit cent", 800),
                              ("neuf cent", 900)])
    SuffixOrdinalMap = dict([("millième", 1000),
                             ("million", 1000000),
                             ("milliardième", 1000000000000)])
    RoundNumberMap = dict([("cent", 100),
                           ("mille", 1000),
                           ("million", 1000000),
                           ("millions", 1000000),
                           ("milliard", 1000000000),
                           ("milliards", 1000000000),
                           ("billion", 1000000000000),
                           ("billions", 1000000000000),
                           ("centieme", 100),
                           ("centième", 100),
                           ("millieme", 1000),
                           ("millième", 1000),
                           ("millionième", 1000000),
                           ("millionieme", 1000000),
                           ("milliardième", 1000000000),
                           ("milliardieme", 1000000000),
                           ("billionième", 1000000000000),
                           ("billionieme", 1000000000000),
                           ("centiemes", 100),
                           ("centièmes", 100),
                           ("millièmes", 1000),
                           ("milliemes", 1000),
                           ("millionièmes", 1000000),
                           ("millioniemes", 1000000),
                           ("milliardièmes", 1000000000),
                           ("milliardiemes", 1000000000),
                           ("billionièmes", 1000000000000),
                           ("billioniemes", 1000000000000),
                           ("douzaine", 12),
                           ("douzaines", 12),
                           ("k", 1000),
                           ("m", 1000000),
                           ("g", 1000000000),
                           ("b", 1000000000),
                           ("t", 1000000000000)])
    AmbiguityFiltersDict = dict([("^[.]", "")])
    RelativeReferenceOffsetMap = dict([("", "")])
    RelativeReferenceRelativeToMap = dict([("", "")])
# pylint: enable=line-too-long
