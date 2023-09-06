from .base_numbers import BaseNumbers
# pylint: disable=line-too-long


class CatalanNumeric:
    LangMarker = 'Cat'
    CompoundNumberLanguage = False
    MultiDecimalSeparatorCulture = True
    NonStandardSeparatorVariants = []
    RoundNumberIntegerRegex = f'((?:cents|milers|milions|mil milions|bilió)s?|mil)'
    ZeroToNineIntegerRegex = f'(?:tres|set|vuit|quatre|cinc|zero|nou|un|dos|sis)'
    TwoToNineIntegerRegex = f'(?:tres|set|vuit|quatre|cinc|nou|dos|sis)'
    NegativeNumberTermsRegex = f'(?<negTerm>(menys|negatiu)\\s+)'
    NegativeNumberSignRegex = f'^{NegativeNumberTermsRegex}.*'
    TenToNineteenIntegerRegex = f'(?:disset|tretze|catorze|divuit anys|dinou|quinze|setze|onze|dotze|deu)'
    TensNumberIntegerRegex = f'(?:setanta|vint|trenta|vuitanta|noranta|quaranta|cinquanta|seixanta)'
    TwentiesIntegerRegex = f'(vint(\\s?-\\s?|\\s)i(\\s?-\\s?|\\s)({TwoToNineIntegerRegex}|u(n)?))'
    BelowHundredsRegex = f'(({TenToNineteenIntegerRegex}|{TwentiesIntegerRegex}|({TensNumberIntegerRegex}((\\s?-\\s?|\\s)({TwoToNineIntegerRegex}|u))?)|{ZeroToNineIntegerRegex}))'
    HundredsNumberIntegerRegex = f'(({TwoToNineIntegerRegex}(\\s?-\\s?|\\s))?cent(s?))'
    BelowThousandsRegex = f'({HundredsNumberIntegerRegex}(\\s+{BelowHundredsRegex})?|{BelowHundredsRegex})'
    SupportThousandsRegex = f'(({BelowThousandsRegex}|{BelowHundredsRegex})\\s+{RoundNumberIntegerRegex}(\\s+{RoundNumberIntegerRegex})?)'
    SeparaIntRegex = f'({SupportThousandsRegex}(\\s+{SupportThousandsRegex})*(\\s+{BelowThousandsRegex})?|{BelowThousandsRegex})'
    AllIntRegex = f'({SeparaIntRegex}|mil(\\s+{BelowThousandsRegex})?|{RoundNumberIntegerRegex})'
    PlaceHolderPureNumber = f'\\b'
    PlaceHolderDefault = f'(?=\\D)|\\b'
    PlaceHolderMixed = f'\\D|\\b'
    DigitsNumberRegex = f'\\d|\\d{{1,3}}(\\.\\d{{3}})'

    def NumbersWithPlaceHolder(placeholder):
        return f'(((?<!\\d+\\s*)-\\s*)|(?<=\\b))\\d+(?!([\\.,]\\d+[a-zA-Z]))(?={placeholder})'

    NumbersWithSuffix = f'(((?<=\\W|^)-\\s*)|(?<=\\b))\\d+\\s*{BaseNumbers.NumberMultiplierRegex}(?=\\b)'
    RoundNumberIntegerRegexWithLocks = f'(?<=\\b)({DigitsNumberRegex})+\\s+{RoundNumberIntegerRegex}(?=\\b)'
    NumbersWithDozenSuffix = f'(((?<=\\W|^)-\\s*)|(?<=\\b))\\d+\\s+(dotzena)s?(?=\\b)'
    AllIntRegexWithLocks = f'((?<=\\b){AllIntRegex}(?=\\b))'
    RoundNumberOrdinalRegex = f'(?:centèsim|mil·lèssim|milionèsima|mil milions|bilionsè)'
    NumberOrdinalRegex = f'(?:primer|segon[a]?|tercer|quart|cinquè|cinquena|sisè|sisena|setè|setena|vuitè|vuitena|novè|desè|desena|onzè|dotzè|tretzè|catorzè|quinzena|setzè|dissetè|divuitè|dinou|vintè|thirtieth|trent[èe]|cinquantè|seixanta|setanta|vuitanta|noranta)'
    SuffixRoundNumberOrdinalRegex = f'(?:({AllIntRegex}\\s+){RoundNumberOrdinalRegex})'
    AllOrdinalNumberRegex = f'(?:{SuffixRoundNumberOrdinalRegex})'
    AllOrdinalRegex = f'(?:{AllOrdinalNumberRegex})'
    OrdinalSuffixRegex = f'(?<=\\b)(?:(\\d*(1r|2n|3r|4t|[5-99][èe])))(?=\\b)'
    AllPointRegex = f'((\\s+{ZeroToNineIntegerRegex})+|(\\s+{SeparaIntRegex}))'
    AllFloatRegex = f'{AllIntRegex}(\\s+coma){AllPointRegex}'

    def DoubleDecimalPointRegex(placeholder):
        return f'(((?<!\\d+\\s*)-\\s*)|((?<=\\b)(?<!\\d+[\\.,])))\\d+[\\.,]\\d+(?!([\\.,]\\d+))(?={placeholder})'

    def DoubleWithoutIntegralRegex(placeholder):
        return f'(?<=\\s|^)(?<!(\\d+))[\\.,]\\d+(?!([\\.,]\\d+))(?={placeholder})'

    DoubleWithMultiplierRegex = f'(((?<!\\d+\\s*)-\\s*)|((?<=\\b)(?<!\\d+\\[\\.,])))\\d+[\\.,]\\d+\\s*{BaseNumbers.NumberMultiplierRegex}(?=\\b)'
    DoubleWithRoundNumber = f'(((?<!\\d+\\s*)-\\s*)|((?<=\\b)(?<!\\d+\\[\\.,])))\\d+[\\.,]\\d+\\s+{RoundNumberIntegerRegex}(?=\\b)'
    DoubleAllFloatRegex = f'((?<=\\b){AllFloatRegex}(?=\\b))'
    DoubleExponentialNotationRegex = f'(((?<!\\d+\\s*)-\\s*)|((?<=\\b)(?<!\\d+[\\.,])))(\\d+([\\.,]\\d+)?)e([+-]*[1-9]\\d*)(?=\\b)'
    DoubleCaretExponentialNotationRegex = f'(((?<!\\d+\\s*)-\\s*)|((?<=\\b)(?<!\\d+[\\.,])))(\\d+([\\.,]\\d+)?)\\^([+-]*[1-9]\\d*)(?=\\b)'
    EqualRegex = f'(igual a)'
    AmbiguousFractionConnectorsRegex = f'(\\b(en|a)\\b)'
    DecimalSeparatorChar = ','
    NonDecimalSeparatorChar = '.'
    HalfADozenText = 'sis'
    WordSeparatorToken = 'i'
    WrittenDecimalSeparatorTexts = [r'coma']
    WrittenGroupSeparatorTexts = [r'punt(o)?']
    WrittenIntegerSeparatorTexts = [r'i']
    HalfADozenRegex = f'mitja\\s+dotzena'
    DigitalNumberRegex = f'((?<=\\b)(cent|milers|mil|milions|mil milions|bili[óo])(?=\\b))|((?<=(\\d|\\b)){BaseNumbers.MultiplierLookupRegex}(?=\\b))'
    CardinlaNumberMap = dict([("zero", 0),
                              ("una", 1),
                              ("un", 1),
                              ("u", 1),
                              ("dos", 2),
                              ("tres", 3),
                              ("quatre", 4),
                              ("cinc", 5),
                              ("sis", 6),
                              ("set", 7),
                              ("vuit", 8),
                              ("nou", 9),
                              ("deu", 10),
                              ("onze", 11),
                              ("dotze", 12),
                              ("dotzena", 12),
                              ("dotzenes", 12),
                              ("tretze", 13),
                              ("catorze", 14),
                              ("quinze", 15),
                              ("setze", 16),
                              ("disset", 17),
                              ("divuit", 18),
                              ("dinnou", 19),
                              ("dinou", 19),
                              ("vint", 20),
                              ("trenta", 30),
                              ("quaranta", 40),
                              ("cinquanta", 50),
                              ("seixanta", 60),
                              ("setanta", 70),
                              ("vuitanta", 80),
                              ("noranta", 90),
                              ("cent", 100),
                              ("dos-cents", 200),
                              ("doscents", 200),
                              ("dos cents", 200),
                              ("tres-cents", 300),
                              ("trescents", 300),
                              ("tres cents", 300),
                              ("quatre-cents", 400),
                              ("quatre cents", 400),
                              ("quatrecents", 400),
                              ("cinc-cents", 500),
                              ("cinc cents", 500),
                              ("cinccents", 500),
                              ("sis-cents", 600),
                              ("sis cents", 600),
                              ("siscents", 600),
                              ("set-cents", 700),
                              ("set cents", 700),
                              ("setcents", 700),
                              ("vuit-cents", 800),
                              ("vuit cents", 800),
                              ("vuitcents", 800),
                              ("nou-cents", 900),
                              ("nou cents", 900),
                              ("noucents", 900),
                              ("mil", 1000),
                              ("milions", 1000000),
                              ("bilió", 1000000000000),
                              ("centenars", 100),
                              ("milers", 1000),
                              ("milers de milions", 1000000000),
                              ("bilions", 1000000000000)])
    OrdinalNumberMap = dict([("primer", 1),
                             ("segon", 2),
                             ("secundari", 2),
                             ("la meitat", 2),
                             ("tercer", 3),
                             ("quart", 4),
                             ("cinquè", 5),
                             ("sisè", 6),
                             ("setè", 7),
                             ("vuitè", 8),
                             ("novè", 9),
                             ("desè", 10),
                             ("onzè", 11),
                             ("dotzè", 12),
                             ("tretzè", 13),
                             ("catorzè", 14),
                             ("quinzè", 15),
                             ("setze", 16),
                             ("disset", 17),
                             ("divuitena", 18),
                             ("dinovena", 19),
                             ("vintè", 20),
                             ("trenta", 30),
                             ("quaranta", 40),
                             ("cinquantè", 50),
                             ("seixanta", 60),
                             ("setanta", 70),
                             ("80è", 80),
                             ("noranta", 90),
                             ("centèsima", 100),
                             ("milè", 1000),
                             ("milionèsima", 1000000),
                             ("bil·lionèsima", 1000000000000)])
    RoundNumberMap = dict([("cent", 100),
                           ("mil", 1000),
                           ("milions", 1000000),
                           ("mln", 1000000),
                           ("mil milions", 1000000000),
                           ("bln", 1000000000),
                           ("bilió", 1000000000000),])
    AmbiguityFiltersDict = dict([("^[.]", "")])
# pylint: enable=line-too-long