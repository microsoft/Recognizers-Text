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


class GermanNumeric:
    LangMarker = 'Ger'
    CompoundNumberLanguage = False
    MultiDecimalSeparatorCulture = True
    RoundNumberIntegerRegex = f'(?:hundert|tausend|million|billion|trillion|hunderttausend|zehn millionen)'
    ZeroToNineIntegerRegex = f'(?:drei|sieben|acht|vier|fünf|null|neun|eins|zwei|sechs)'
    TwoToNineIntegerRegex = f'(?:drei|sieben|acht|vier|fünf|neun|zwei|sechs)'
    NegativeNumberTermsRegex = f'(?<negTerm>(minus|negative)\\s+)'
    NegativeNumberSignRegex = f'^{NegativeNumberTermsRegex}.*'
    AnIntRegex = f'(an?)(?=\\s)'
    TenToNineteenIntegerRegex = f'(?:siebzehn|dreizehn|vierzehn|achtzehn|neunzehn|fünfzehn|sechszehn|elf|zwölf|zehn)'
    TensNumberIntegerRegex = f'(?:siebzig|zwanzig|dreißig|achtzig|neunzig|vierzig|fünfzig|sechszig)'
    SeparaIntRegex = f'(?:(({TenToNineteenIntegerRegex}|({TensNumberIntegerRegex}(\\s+(und\\s+)?|\\s*-\\s*){ZeroToNineIntegerRegex})|{TensNumberIntegerRegex}|{ZeroToNineIntegerRegex})(\\s+{RoundNumberIntegerRegex})*))|(({AnIntRegex}(\\s+{RoundNumberIntegerRegex})+))'
    AllIntRegex = f'(?:((({TenToNineteenIntegerRegex}|({TensNumberIntegerRegex}(\\s+(und\\s+)?|\\s*-\\s*){ZeroToNineIntegerRegex})|{TensNumberIntegerRegex}|{ZeroToNineIntegerRegex}|{AnIntRegex})(\\s+{RoundNumberIntegerRegex})+)\\s+(und\\s+)?)*{SeparaIntRegex})'
    PlaceHolderPureNumber = f'\\b'
    PlaceHolderDefault = f'\\D|\\b'

    def NumbersWithPlaceHolder(placeholder):
        return f'(((?<!\\d+(\\s*(K|k|MM?|mil|G|T|B|b))?\\s*)-\\s*)|(?<=\\b))\\d+(?!([\\.,]\\d+[a-zA-Z]))(?={placeholder})'
    IndianNumberingSystemRegex = f'(?<=\\b)((?:\\d{{1,2}},(?:\\d{{2}},)*\\d{{3}})(?=\\b))'
    NumbersWithSuffix = f'(((?<!\\d+(\\s*{BaseNumbers.NumberMultiplierRegex})?\\s*)-\\s*)|(?<=\\b))\\d+\\s*{BaseNumbers.NumberMultiplierRegex}(?=\\b)'
    RoundNumberIntegerRegexWithLocks = f'(?<=\\b)\\d+\\s+{RoundNumberIntegerRegex}(?=\\b)'
    NumbersWithDozenSuffix = f'(((?<!\\d+(\\s*{BaseNumbers.NumberMultiplierRegex})?\\s*)-\\s*)|(?<=\\b))\\d+\\s+dutzend(e)?(?=\\b)'
    AllIntRegexWithLocks = f'((?<=\\b){AllIntRegex}(?=\\b))'
    AllIntRegexWithDozenSuffixLocks = f'(?<=\\b)(((halb\\s+)?a\\s+dutzend)|({AllIntRegex}\\s+dutzend(e)?))(?=\\b)'
    RoundNumberOrdinalRegex = f'(?:hundertste|tausenste|millionste|billionste|trillionste)'
    NumberOrdinalRegex = f'(?:erstens|zweitens|drittens|viertens|fünftens|sechstens|siebentens|achtens|neuntens|zehntens|elftens|zwölftens|dreizehntens|vierzehntens|fünfzehntens|sechszehtens|siebzehntens|achtzehntens|neunzehntens|zwanzigst|dreißigst|vierzigst|fünfzigst|sechszigst|siebzigst|achtzigst|neunzigst)'
    RelativeOrdinalRegex = f'(?<relativeOrdinal>(nächster|vorheriger|aktueller)\\s+one|(the\\s+second|next)\\s+to\\s+last|the\\s+one\\s+before\\s+the\\s+last(\\s+one)?|the\\s+last\\s+but\\s+one|(ante)?penultimate|letzter|nächster|vorheriger|aktueller)'
    BasicOrdinalRegex = f'({NumberOrdinalRegex}|{RelativeOrdinalRegex})'
    SuffixBasicOrdinalRegex = f'(?:(((({TensNumberIntegerRegex}(\\s+(und\\s+)?|\\s*-\\s*){ZeroToNineIntegerRegex})|{TensNumberIntegerRegex}|{ZeroToNineIntegerRegex}|{AnIntRegex})(\\s+{RoundNumberIntegerRegex})+)\\s+(und\\s+)?)*({TensNumberIntegerRegex}(\\s+|\\s*-\\s*))?{BasicOrdinalRegex})'
    SuffixRoundNumberOrdinalRegex = f'(?:({AllIntRegex}\\s+){RoundNumberOrdinalRegex})'
    AllOrdinalRegex = f'(?:{SuffixBasicOrdinalRegex}|{SuffixRoundNumberOrdinalRegex})'
    OrdinalSuffixRegex = f'(?<=\\b)(?:(\\d*(1st|2nd|3rd|[4-90]th))|(1[1-2]th))(?=\\b)'
    OrdinalNumericRegex = f'(?<=\\b)(?:\\d{{1,3}}(\\s*,\\s*\\d{{3}})*\\s*th)(?=\\b)'
    OrdinalRoundNumberRegex = f'(?<!an?\\s+){RoundNumberOrdinalRegex}'
    OrdinalGermanRegex = f'(?<=\\b){AllOrdinalRegex}(?=\\b)'
    FractionNotationWithSpacesRegex = f'(((?<=\\W|^)-\\s*)|(?<=\\b))\\d+\\s+\\d+[/]\\d+(?=(\\b[^/]|$))'
    FractionNotationRegex = f'(((?<=\\W|^)-\\s*)|(?<![/-])(?<=\\b))\\d+[/]\\d+(?=(\\b[^/]|$))'
    FractionNounRegex = f'(?<=\\b)({AllIntRegex}\\s+(und\\s+)?)?({AllIntRegex})(\\s+|\\s*-\\s*)((({AllOrdinalRegex})|({RoundNumberOrdinalRegex}))s|halves|quarters)(?=\\b)'
    FractionNounWithArticleRegex = f'(?<=\\b)((({AllIntRegex}\\s+(und\\s+)?)?(an?|eins)(\\s+|\\s*-\\s*)(?!\\berster\\b|\\bzweiter\\b)(({AllOrdinalRegex})|({RoundNumberOrdinalRegex})|halb|viertel))|(halb))(?=\\b)'
    FractionPrepositionRegex = f'(?<!{BaseNumbers.CommonCurrencySymbol}\\s*)(?<=\\b)(?<numerator>({AllIntRegex})|((?<![\\.,])\\d+))\\s+(over|in|out\\s+of)\\s+(?<denominator>({AllIntRegex})|(\\d+)(?![\\.,]))(?=\\b)'
    FractionPrepositionWithinPercentModeRegex = f'(?<!{BaseNumbers.CommonCurrencySymbol}\\s*)(?<=\\b)(?<numerator>({AllIntRegex})|((?<![\\.,])\\d+))\\s+over\\s+(?<denominator>({AllIntRegex})|(\\d+)(?![\\.,]))(?=\\b)'
    AllPointRegex = f'((\\s+{ZeroToNineIntegerRegex})+|(\\s+{SeparaIntRegex}))'
    AllFloatRegex = f'{AllIntRegex}(\\s+punkt){AllPointRegex}'
    DoubleWithMultiplierRegex = f'(((?<!\\d+(\\s*{BaseNumbers.NumberMultiplierRegex})?\\s*)-\\s*)|((?<=\\b)(?<!\\d+[\\.,])))\\d+[\\.,]\\d+\\s*{BaseNumbers.NumberMultiplierRegex}(?=\\b)'
    DoubleExponentialNotationRegex = f'(((?<!\\d+(\\s*{BaseNumbers.NumberMultiplierRegex})?\\s*)-\\s*)|((?<=\\b)(?<!\\d+[\\.,])))(\\d+([\\.,]\\d+)?)e([+-]*[1-9]\\d*)(?=\\b)'
    DoubleCaretExponentialNotationRegex = f'(((?<!\\d+(\\s*{BaseNumbers.NumberMultiplierRegex})?\\s*)-\\s*)|((?<=\\b)(?<!\\d+[\\.,])))(\\d+([\\.,]\\d+)?)\\^([+-]*[1-9]\\d*)(?=\\b)'

    def DoubleDecimalPointRegex(placeholder):
        return f'(((?<!\\d+(\\s*(K|k|MM?|mil|G|T|B|b))?\\s*)-\\s*)|((?<=\\b)(?<!\\d+[\\.,])))\\d+[\\.,]\\d+(?!([\\.,]\\d+))(?={placeholder})'
    DoubleIndianDecimalPointRegex = f'(?<=\\b)((?:\\d{{1,2}},(?:\\d{{2}},)*\\d{{3}})(?:\\.\\d{{2}})(?=\\b))'

    def DoubleWithoutIntegralRegex(placeholder):
        return f'(?<=\\s|^)(?<!(\\d+))[\\.,]\\d+(?!([\\.,]\\d+))(?={placeholder})'
    DoubleWithRoundNumber = f'(((?<!\\d+(\\s*{BaseNumbers.NumberMultiplierRegex})?\\s*)-\\s*)|((?<=\\b)(?<!\\d+[\\.,])))\\d+[\\.,]\\d+\\s+{RoundNumberIntegerRegex}(?=\\b)'
    DoubleAllFloatRegex = f'((?<=\\b){AllFloatRegex}(?=\\b))'
    ConnectorRegex = f'(?<spacer>und)'
    NumberWithSuffixPercentage = f'(?<!%)({BaseNumbers.NumberReplaceToken})(\\s*)(%(?!{BaseNumbers.NumberReplaceToken})|(per\\s*cents?|prozent|cents?)\\b)'
    FractionNumberWithSuffixPercentage = f'(({BaseNumbers.FractionNumberReplaceToken})\\s+of)'
    NumberWithPrefixPercentage = f'(per\\s*cents?\\s+von)(\\s*)({BaseNumbers.NumberReplaceToken})'
    NumberWithPrepositionPercentage = f'({BaseNumbers.NumberReplaceToken})\\s*(in|out\\s+of)\\s*({BaseNumbers.NumberReplaceToken})'
    TillRegex = f'(bis|bis zu|--|-|—|——|~|–)'
    MoreRegex = f'(?:(größer|höher|mehr)(\\s+als)?|über|darüber(hinaus)?|(?<!<|=)>)'
    LessRegex = f'(?:(weniger|winziger|kleiner|wenig)(\\s+als)?|darunter|unter|(?<!>|=)<)'
    EqualRegex = f'(gleich(\\s+(als|zu))?|(?<!<|>)=)'
    MoreOrEqualPrefix = f'((nicht\\s+{LessRegex})|(als\\s+letzte(r)?))'
    MoreOrEqual = f'(?:({MoreRegex}\\s+(oder)?\\s+{EqualRegex})|({EqualRegex}\\s+(oder)?\\s+{MoreRegex})|{MoreOrEqualPrefix}(\\s+(oder)?\\s+{EqualRegex})?|({EqualRegex}\\s+(oder)?\\s+)?{MoreOrEqualPrefix}|>\\s*=|≥)'
    MoreOrEqualSuffix = f'((und|oder)\\s+(((mehr|größer|höher)((?!\\s+als)|(\\s+als(?!(\\s*\\d+)))))|((über|darüber)(?!\\s+als))))'
    LessOrEqualPrefix = f'((nicht\\s+{MoreRegex})|(at\\s+viele)|(bis\\s+zu))'
    LessOrEqual = f'(({LessRegex}\\s+(oder)?\\s+{EqualRegex})|({EqualRegex}\\s+(oder)?\\s+{LessRegex})|{LessOrEqualPrefix}(\\s+(oder)?\\s+{EqualRegex})?|({EqualRegex}\\s+(oder)?\\s+)?{LessOrEqualPrefix}|<\\s*=|≤)'
    LessOrEqualSuffix = f'((und|oder)\\s+(weniger|geringer|kleiner|winziger)((?!\\s+als)|(\\s+als(?!(\\s*\\d+)))))'
    NumberSplitMark = f'(?![,.](?!\\d+))'
    MoreRegexNoNumberSucceed = f'((größer|mehr|höhrer|breiter)((?!\\s+als)|\\s+(als(?!(\\s*\\d+))))|(darüber|über)(?!(\\s*\\d+)))'
    LessRegexNoNumberSucceed = f'((kleiner|weniger|winziger)((?!\\s+als)|\\s+(als(?!(\\s*\\d+))))|(unter|darunter)(?!(\\s*\\d+)))'
    EqualRegexNoNumberSucceed = f'(gleich(s|ing)?((?!\\s+(zu|als))|(\\s+(zu|als)(?!(\\s*\\d+)))))'
    OneNumberRangeMoreRegex1 = f'({MoreOrEqual}|{MoreRegex})\\s*(der\\s+)?(?<number1>({NumberSplitMark}.)+)'
    OneNumberRangeMoreRegex2 = f'(?<number1>({NumberSplitMark}.)+)\\s*{MoreOrEqualSuffix}'
    OneNumberRangeMoreSeparateRegex = f'({EqualRegex}\\s+(?<number1>({NumberSplitMark}.)+)(\\s+or\\s+){MoreRegexNoNumberSucceed})|({MoreRegex}\\s+(?<number1>({NumberSplitMark}.)+)(\\s+oder\\s+){EqualRegexNoNumberSucceed})'
    OneNumberRangeLessRegex1 = f'({LessOrEqual}|{LessRegex})\\s*(the\\s+)?(?<number2>({NumberSplitMark}.)+)'
    OneNumberRangeLessRegex2 = f'(?<number2>({NumberSplitMark}.)+)\\s*{LessOrEqualSuffix}'
    OneNumberRangeLessSeparateRegex = f'({EqualRegex}\\s+(?<number1>({NumberSplitMark}.)+)(\\s+or\\s+){LessRegexNoNumberSucceed})|({LessRegex}\\s+(?<number1>({NumberSplitMark}.)+)(\\s+oder\\s+){EqualRegexNoNumberSucceed})'
    OneNumberRangeEqualRegex = f'{EqualRegex}\\s*(the\\s+)?(?<number1>({NumberSplitMark}.)+)'
    TwoNumberRangeRegex1 = f'zwischen\\s*(der\\s+)?(?<number1>({NumberSplitMark}.)+)\\s*und\\s*(der\\s+)?(?<number2>({NumberSplitMark}.)+)'
    TwoNumberRangeRegex2 = f'({OneNumberRangeMoreRegex1}|{OneNumberRangeMoreRegex2})\\s*(und|aber|,)\\s*({OneNumberRangeLessRegex1}|{OneNumberRangeLessRegex2})'
    TwoNumberRangeRegex3 = f'({OneNumberRangeLessRegex1}|{OneNumberRangeLessRegex2})\\s*(und|aber|,)\\s*({OneNumberRangeMoreRegex1}|{OneNumberRangeMoreRegex2})'
    TwoNumberRangeRegex4 = f'(von\\s+)?(?<number1>({NumberSplitMark}(?!\\bvon\\b).)+)\\s*{TillRegex}\\s*(the\\s+)?(?<number2>({NumberSplitMark}.)+)'
    AmbiguousFractionConnectorsRegex = f'(\\bin\\b)'
    DecimalSeparatorChar = '.'
    FractionMarkerToken = 'over'
    NonDecimalSeparatorChar = ','
    HalfADozenText = 'sechs'
    WordSeparatorToken = 'und'
    WrittenDecimalSeparatorTexts = [r'punkt']
    WrittenGroupSeparatorTexts = [r'punkt']
    WrittenIntegerSeparatorTexts = [r'und']
    WrittenFractionSeparatorTexts = [r'und']
    HalfADozenRegex = f'halbes\\s+a\\s+dutzend'
    DigitalNumberRegex = f'((?<=\\b)(hundert|tausend|[mb]illion|trillion|hundertausend|zehn millionen|dutzend(e)?)(?=\\b))|((?<=(\\d|\\b)){BaseNumbers.MultiplierLookupRegex}(?=\\b))'
    CardinalNumberMap = dict([("ein", 1),
                              ("null", 0),
                              ("eine", 1),
                              ("eins", 1),
                              ("zwei", 2),
                              ("drei", 3),
                              ("vier", 4),
                              ("fünf", 5),
                              ("sechs", 6),
                              ("sieben", 7),
                              ("acht", 8),
                              ("neun", 9),
                              ("zehn", 10),
                              ("elf", 11),
                              ("zwölf", 12),
                              ("dutzend", 12),
                              ("dutzende", 12),
                              ("dreizehn", 13),
                              ("vierzehn", 14),
                              ("fünfzehn", 15),
                              ("sechszehn", 16),
                              ("siebzehn", 17),
                              ("achtzehn", 18),
                              ("neunzehn", 19),
                              ("zwanzig", 20),
                              ("dreißig", 30),
                              ("vierzig", 40),
                              ("fünfzig", 50),
                              ("sechszig", 60),
                              ("siebzig", 70),
                              ("achtzig", 80),
                              ("neunzig", 90),
                              ("hundert", 100),
                              ("tausend", 1000),
                              ("million", 1000000),
                              ("billion", 1000000000),
                              ("trillion", 1000000000000),
                              ("hunderttausend", 100000),
                              ("zehn millionen", 10000000)])
    OrdinalNumberMap = dict([("erstens", 1),
                             ("zweitens", 2),
                             ("drittens", 2),
                             ("viertens", 2),
                             ("fünftens", 3),
                             ("sechstens", 4),
                             ("siebentens", 7),
                             ("achtens", 8),
                             ("neuntens", 9),
                             ("zehntens", 10),
                             ("elftens", 11),
                             ("zwölftens", 12),
                             ("dreizehntens", 13),
                             ("vierzehntens", 14),
                             ("fühnzehntens", 15),
                             ("sechszehntens", 16),
                             ("siebzehntens", 17),
                             ("achtzehntens", 18),
                             ("neunzehntens", 19),
                             ("zwanzigstens", 20),
                             ("dreißigstens", 30),
                             ("vierzigstens", 40),
                             ("fünzigstens", 50),
                             ("sechzigstens", 60),
                             ("siebzigstens", 70),
                             ("achtzigstens", 80),
                             ("neunzigstens", 90),
                             ("hundertstens", 100),
                             ("tausendstens", 1000),
                             ("millionstens", 1000000),
                             ("billionstens", 1000000000),
                             ("trillionstens", 1000000000000),
                             ("erste", 1),
                             ("zweite", 2),
                             ("halbe", 2),
                             ("dritte", 2),
                             ("vierte", 2),
                             ("viertel", 2),
                             ("fünfte", 3),
                             ("sechste", 4),
                             ("siebente", 7),
                             ("achte", 8),
                             ("neunte", 9),
                             ("zehnte", 10),
                             ("elfte", 11),
                             ("zwölfte", 12),
                             ("dreizehnte", 13),
                             ("vierzehnte", 14),
                             ("fühnzehnte", 15),
                             ("sechszehnte", 16),
                             ("siebzehnte", 17),
                             ("achtzehnte", 18),
                             ("neunzehnte", 19),
                             ("zwanzigste", 20),
                             ("dreißigste", 30),
                             ("vierzigste", 40),
                             ("fünzigste", 50),
                             ("sechzigste", 60),
                             ("siebzigste", 70),
                             ("achtzigste", 80),
                             ("neunzigste", 90),
                             ("hundertste", 100),
                             ("tausendste", 1000),
                             ("millionste", 1000000),
                             ("billionste", 1000000000),
                             ("trillionste", 1000000000000),
                             ("erster", 1),
                             ("zweiter", 2),
                             ("dritter", 3),
                             ("vierter", 4),
                             ("vierteln", 4),
                             ("fünfter", 5),
                             ("sechster", 6),
                             ("siebenter", 7),
                             ("achter", 8),
                             ("neunter", 9),
                             ("zehnter", 10),
                             ("elfter", 11),
                             ("zwölfter", 12),
                             ("dreizehnter", 13),
                             ("vierzehnter", 14),
                             ("fünfzehnter", 15),
                             ("sechszehnter", 16),
                             ("siebzehnter", 17),
                             ("achtzehner", 18),
                             ("neunzehnter", 19),
                             ("zwanzigster", 20),
                             ("dreißigster", 30),
                             ("vierzigster", 40),
                             ("fünfzigster", 50),
                             ("sechszigster", 60),
                             ("siebzigster", 70),
                             ("achtzigster", 80),
                             ("neunzigster", 90),
                             ("hunderster", 100),
                             ("tausendster", 1000),
                             ("millionster", 1000000),
                             ("billionster", 1000000000),
                             ("trillionster", 1000000000000)])
    RoundNumberMap = dict([("hundert", 100),
                           ("tausend", 1000),
                           ("million", 1000000),
                           ("billion", 1000000000),
                           ("trillion", 1000000000000),
                           ("hunderttausend", 100000),
                           ("zehn millionen", 10000000),
                           ("hundertste", 100),
                           ("tausenste", 1000),
                           ("millionste", 1000000),
                           ("billionste", 1000000000),
                           ("trillionste", 1000000000000),
                           ("hundertster", 100),
                           ("tausenster", 1000),
                           ("millionster", 1000000),
                           ("billionster", 1000000000),
                           ("trillionster", 1000000000000),
                           ("dutzend", 12),
                           ("dutzende", 12),
                           ("k", 1000),
                           ("m", 1000000),
                           ("mm", 1000000),
                           ("mil", 1000000),
                           ("g", 1000000000),
                           ("b", 1000000000),
                           ("t", 1000000000000)])
    AmbiguityFiltersDict = dict([("\\bone\\b", "\\b(der|die|das|wer)\\s+(ein)\\b")])
    RelativeReferenceOffsetMap = dict([("letztes", "0"),
                                       ("letzte", "0"),
                                       ("letzter", "0"),
                                       ("nächstes", "1"),
                                       ("nächste", "1"),
                                       ("nächster", "1"),
                                       ("aktuelles", "0"),
                                       ("aktuelle", "0"),
                                       ("aktueller", "0"),
                                       ("vorheriges", "-1"),
                                       ("vorherige", "-1"),
                                       ("vorheriger", "-1"),
                                       ("zweitletztes", "-1"),
                                       ("zweitletzter", "-1"),
                                       ("zweitletzte", "-1"),
                                       ("vorvorletztes", "-2"),
                                       ("vorvorletzte", "-2"),
                                       ("vorvorletzter", "-2"),
                                       ("drittletztes", "-2"),
                                       ("drittletzte", "-2"),
                                       ("drittletzter", "-2")
                                       ])
    RelativeReferenceRelativeToMap = dict([("letzer", "end"),
                                           ("nächster", "aktueller"),
                                           ("letzter", "aktueller"),
                                           ("aktuelles", "aktueller"),
                                           ("aktuelle", "aktueller"),
                                           ("zweitletzter", "ende"),
                                           ("drittletzte", "ende"),
                                           ("nächstes", "aktuell"),
                                           ("vorheriger", "aktueller")])
# pylint: enable=line-too-long
