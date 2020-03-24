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
    RoundNumberIntegerRegex = f'(cent|mille|millions|million|milliard|milliards|billion|billions)'
    ZeroToNineIntegerRegex = f'(et un|un|une|deux|trois|quatre|cinq|six|sept|huit|neuf)'
    TenToNineteenIntegerRegex = f'((seize|quinze|quatorze|treize|douze|onze)|dix(\\Wneuf|\\Whuit|\\Wsept)?)'
    TensNumberIntegerRegex = f'(quatre\\Wvingt(s|\\Wdix)?|soixante\\Wdix|vingt|trente|quarante|cinquante|soixante|septante|octante|huitante|nonante)'
    DigitsNumberRegex = f'\\d|\\d{{1,3}}(\\.\\d{{3}})'
    NegativeNumberTermsRegex = f'^[.]'
    NegativeNumberSignRegex = f'^({NegativeNumberTermsRegex}\\s+).*'
    HundredsNumberIntegerRegex = f'(({ZeroToNineIntegerRegex}(\\s+cent))|cent|((\\s+cent\\s)+{TensNumberIntegerRegex}))'
    BelowHundredsRegex = f'(({TenToNineteenIntegerRegex}|({TensNumberIntegerRegex}([-\\s]+({TenToNineteenIntegerRegex}|{ZeroToNineIntegerRegex}))?))|{ZeroToNineIntegerRegex})'
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
    SimpleRoundOrdinalRegex = f'(centi[eè]me|milli[eè]me|millioni[eè]me|milliardi[eè]me|billioni[eè]me)'
    OneToNineOrdinalRegex = f'(premier|premi[eè]re|deuxi[eè]me|second[e]|troisi[eè]me|tiers|tierce|quatri[eè]me|cinqui[eè]me|sixi[eè]me|septi[eè]me|huiti[eè]me|neuvi[eè]me)'
    SpecialUnderHundredOrdinalRegex = f'(onzi[eè]me|douzi[eè]me)'
    TensOrdinalRegex = f'(quatre-vingt-dixi[eè]me|quatre-vingti[eè]me|huitanti[eè]me|octanti[eè]me|soixante-dixi[eè]me|septanti[eè]me|soixanti[eè]me|cinquanti[eè]me|quaranti[eè]me|trenti[eè]me|vingti[eè]me)'
    HundredOrdinalRegex = f'({AllIntRegex}(\\s+(centi[eè]me\\s)))'
    UnderHundredOrdinalRegex = f'((({AllIntRegex}(\\W)?)?{OneToNineOrdinalRegex})|({TensNumberIntegerRegex}(\\W)?)?{OneToNineOrdinalRegex}|{TensOrdinalRegex}|{SpecialUnderHundredOrdinalRegex})'
    UnderThousandOrdinalRegex = f'((({HundredOrdinalRegex}(\\s)?)?{UnderHundredOrdinalRegex})|(({AllIntRegex}(\\W)?)?{SimpleRoundOrdinalRegex})|{HundredOrdinalRegex})'
    OverThousandOrdinalRegex = f'(({AllIntRegex})(i[eè]me))'
    ComplexOrdinalRegex = f'(({OverThousandOrdinalRegex}(\\s)?)?{UnderThousandOrdinalRegex}|{OverThousandOrdinalRegex}|{UnderHundredOrdinalRegex})'
    SuffixOrdinalRegex = f'(({AllIntRegex})({SimpleRoundOrdinalRegex}))'
    ComplexRoundOrdinalRegex = f'((({SuffixOrdinalRegex}(\\s)?)?{ComplexOrdinalRegex})|{SuffixOrdinalRegex})'
    AllOrdinalRegex = f'({ComplexOrdinalRegex}|{SimpleRoundOrdinalRegex}|{ComplexRoundOrdinalRegex})'
    PlaceHolderPureNumber = f'\\b'
    PlaceHolderDefault = f'\\D|\\b'
    OrdinalSuffixRegex = f'(?<=\\b)((\\d*(1er|2e|2eme|3e|3eme|4e|4eme|5e|5eme|6e|6eme|7e|7eme|8e|8eme|9e|9eme|0e|0eme))|(11e|11eme|12e|12eme))(?=\\b)'
    OrdinalFrenchRegex = f'(?<=\\b){AllOrdinalRegex}(?=\\b)'
    FractionNotationWithSpacesRegex = f'(((?<=\\W|^)-\\s*)|(?<=\\b))\\d+\\s+\\d+[/]\\d+(?=(\\b[^/]|$))'
    FractionNotationRegex = f'(((?<=\\W|^)-\\s*)|(?<=\\b))\\d+[/]\\d+(?=(\\b[^/]|$))'
    FractionNounRegex = f'(?<=\\b)({AllIntRegex}\\s+((et)\\s+)?)?({AllIntRegex})(\\s+((et)\\s)?)((({AllOrdinalRegex})s?|({SuffixOrdinalRegex})s?)|demis?|tiers?|quarts?)(?=\\b)'
    FractionNounWithArticleRegex = f'(?<=\\b)({AllIntRegex}\\s+(et\\s+)?)?(un|une)(\\s+)(({AllOrdinalRegex})|({SuffixOrdinalRegex})|(et\\s+)?demis?)(?=\\b)'
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
    NumberWithSuffixPercentage = f'(?<!%)({BaseNumbers.NumberReplaceToken})(\\s*)(%(?!{BaseNumbers.NumberReplaceToken})|(pourcentages|pourcents|pourcentage|pourcent)\\b)'
    NumberWithPrefixPercentage = f'((?<!{BaseNumbers.NumberReplaceToken})%|pourcent|pourcent des|pourcentage de)(\\s*)({BaseNumbers.NumberReplaceToken})(?=\\s|$)'
    DecimalSeparatorChar = ','
    FractionMarkerToken = 'sur'
    NonDecimalSeparatorChar = '.'
    HalfADozenText = 'six'
    WordSeparatorToken = 'et'
    WrittenDecimalSeparatorTexts = [r'virgule']
    WrittenGroupSeparatorTexts = [r'point', r'points']
    WrittenIntegerSeparatorTexts = [r'et', r'-']
    WrittenFractionSeparatorTexts = [r'et', r'sur']
    HalfADozenRegex = f'(?<=\\b)demi\\s+douzaine'
    DigitalNumberRegex = f'((?<=\\b)(cent|mille|million|millions|milliard|milliards|billions|billion|douzaine(s)?)(?=\\b))|((?<=(\\d|\\b)){BaseNumbers.MultiplierLookupRegex}(?=\\b))'
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
                              ("treize", 13),
                              ("quatorze", 14),
                              ("quinze", 15),
                              ("seize", 16),
                              ("dix-sept", 17),
                              ("dix-huit", 18),
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
                             ("deuxième", 2),
                             ("deuxieme", 2),
                             ("second", 2),
                             ("seconde", 2),
                             ("troisième", 3),
                             ("demi", 2),
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
                             ("neuvième", 9),
                             ("neuvieme", 9),
                             ("dixième", 10),
                             ("dixieme", 10),
                             ("onzième", 11),
                             ("onzieme", 11),
                             ("douzième", 12),
                             ("douzieme", 12),
                             ("treizième", 13),
                             ("treizieme", 13),
                             ("quatorzième", 14),
                             ("quatorizieme", 14),
                             ("quinzième", 15),
                             ("quinzieme", 15),
                             ("seizième", 16),
                             ("seizieme", 16),
                             ("dix-septième", 17),
                             ("dix-septieme", 17),
                             ("dix-huitième", 18),
                             ("dix-huitieme", 18),
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
