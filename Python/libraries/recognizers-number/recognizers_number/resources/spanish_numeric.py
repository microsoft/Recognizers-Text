# ------------------------------------------------------------------------------
# <auto-generated>
#     This code was generated by a tool.
#     Changes to this file may cause incorrect behavior and will be lost if
#     the code is regenerated.
# </auto-generated>
# ------------------------------------------------------------------------------

from .base_numbers import BaseNumbers
# pylint: disable=line-too-long
class SpanishNumeric:
    LangMarker = 'Spa'
    CompoundNumberLanguage = True
    MultiDecimalSeparatorCulture = True
    HundredsNumberIntegerRegex = f'(cuatrocient[ao]s|trescient[ao]s|seiscient[ao]s|setecient[ao]s|ochocient[ao]s|novecient[ao]s|doscient[ao]s|quinient[ao]s|(?<!por\\s+)(cien(to)?))'
    RoundNumberIntegerRegex = f'(mil millones|mil|millones|mill[oó]n|billones|bill[oó]n|trillones|trill[oó]n|cuatrillones|cuatrill[oó]n|quintillones|quintill[oó]n|sextillones|sextill[oó]n|septillones|septill[oó]n)'
    ZeroToNineIntegerRegex = f'(cuatro|cinco|siete|nueve|cero|tres|seis|ocho|dos|un[ao]?)'
    TenToNineteenIntegerRegex = f'(diecisiete|diecinueve|diecis[eé]is|dieciocho|catorce|quince|trece|diez|once|doce)'
    TwentiesIntegerRegex = f'(veinticuatro|veinticinco|veintisiete|veintinueve|veintitr[eé]s|veintis[eé]is|veintiocho|veintid[oó]s|ventiun[ao]|veinti[uú]n[oa]?|veinte)'
    TensNumberIntegerRegex = f'(cincuenta|cuarenta|treinta|sesenta|setenta|ochenta|noventa)'
    NegativeNumberTermsRegex = f'((?<!(al|lo)\\s+)menos\\s+)'
    NegativeNumberSignRegex = f'^{NegativeNumberTermsRegex}.*'
    DigitsNumberRegex = f'\\d|\\d{{1,3}}(\\.\\d{{3}})'
    BelowHundredsRegex = f'(({TenToNineteenIntegerRegex}|{TwentiesIntegerRegex}|({TensNumberIntegerRegex}(\\s+y\\s+{ZeroToNineIntegerRegex})?))|{ZeroToNineIntegerRegex})'
    BelowThousandsRegex = f'({HundredsNumberIntegerRegex}(\\s+{BelowHundredsRegex})?|{BelowHundredsRegex})'
    SupportThousandsRegex = f'(({BelowThousandsRegex}|{BelowHundredsRegex})\\s+{RoundNumberIntegerRegex}(\\s+{RoundNumberIntegerRegex})?)'
    SeparaIntRegex = f'({SupportThousandsRegex}(\\s+{SupportThousandsRegex})*(\\s+{BelowThousandsRegex})?|{BelowThousandsRegex})'
    AllIntRegex = f'({SeparaIntRegex}|mil(\\s+{BelowThousandsRegex})?)'
    PlaceHolderPureNumber = f'\\b'
    PlaceHolderDefault = f'\\D|\\b'
    NumbersWithPlaceHolder = lambda placeholder: f'(((?<!\\d+\\s*)-\\s*)|(?<=\\b))\\d+(?!([\\.,]\\d+[a-zA-Z]))(?={placeholder})'
    NumbersWithSuffix = f'(((?<=\\W|^)-\\s*)|(?<=\\b))\\d+\\s*{BaseNumbers.NumberMultiplierRegex}(?=\\b)'
    RoundNumberIntegerRegexWithLocks = f'(?<=\\b)({DigitsNumberRegex})+\\s+{RoundNumberIntegerRegex}(?=\\b)'
    NumbersWithDozenSuffix = f'(((?<=\\W|^)-\\s*)|(?<=\\b))\\d+\\s+docenas?(?=\\b)'
    AllIntRegexWithLocks = f'((?<=\\b){AllIntRegex}(?=\\b))'
    AllIntRegexWithDozenSuffixLocks = f'(?<=\\b)(((media\\s+)?\\s+docena)|({AllIntRegex}\\s+(y|con)\\s+)?({AllIntRegex}\\s+docenas?))(?=\\b)'
    SimpleRoundOrdinalRegex = f'(mil[eé]simo|millon[eé]sim[oa]|billon[eé]sim[oa]|trillon[eé]sim[oa]|cuatrillon[eé]sim[oa]|quintillon[eé]sim[oa]|sextillon[eé]sim[oa]|septillon[eé]sim[oa])'
    OneToNineOrdinalRegex = f'(primer[oa]?|segund[oa]|tercer[oa]?|cuart[oa]|quint[oa]|sext[oa]|s[eé]ptim[oa]|octav[oa]|noven[oa])'
    TensOrdinalRegex = f'(nonag[eé]sim[oa]|octog[eé]sim[oa]|septuag[eé]sim[oa]|sexag[eé]sim[oa]|quincuag[eé]sim[oa]|cuadrag[eé]sim[oa]|trig[eé]sim[oa]|vig[eé]sim[oa]|d[eé]cim[oa])'
    HundredOrdinalRegex = f'(cent[eé]sim[oa]|ducent[eé]sim[oa]|tricent[eé]sim[oa]|cuadringent[eé]sim[oa]|quingent[eé]sim[oa]|sexcent[eé]sim[oa]|septingent[eé]sim[oa]|octingent[eé]sim[oa]|noningent[eé]sim[oa])'
    SpecialUnderHundredOrdinalRegex = f'(und[eé]cim[oa]|duod[eé]cimo|decimoctav[oa])'
    UnderHundredOrdinalRegex = f'({SpecialUnderHundredOrdinalRegex}|(({TensOrdinalRegex}(\\s)?)?{OneToNineOrdinalRegex})|{TensOrdinalRegex})'
    UnderThousandOrdinalRegex = f'((({HundredOrdinalRegex}(\\s)?)?{UnderHundredOrdinalRegex})|{HundredOrdinalRegex})'
    OverThousandOrdinalRegex = f'(({AllIntRegex})([eé]sim[oa]))'
    ComplexOrdinalRegex = f'(({OverThousandOrdinalRegex}(\\s)?)?{UnderThousandOrdinalRegex}|{OverThousandOrdinalRegex})'
    SufixRoundOrdinalRegex = f'(({AllIntRegex})({SimpleRoundOrdinalRegex}))'
    ComplexRoundOrdinalRegex = f'((({SufixRoundOrdinalRegex}(\\s)?)?{ComplexOrdinalRegex})|{SufixRoundOrdinalRegex})'
    AllOrdinalRegex = f'{ComplexOrdinalRegex}|{SimpleRoundOrdinalRegex}|{ComplexRoundOrdinalRegex}'
    OrdinalSuffixRegex = f'(?<=\\b)(\\d*(1r[oa]|2d[oa]|3r[oa]|4t[oa]|5t[oa]|6t[oa]|7m[oa]|8v[oa]|9n[oa]|0m[oa]|11[vm][oa]|12[vm][oa]))(?=\\b)'
    OrdinalNounRegex = f'(?<=\\b){AllOrdinalRegex}(?=\\b)'
    SpecialFractionInteger = f'((({AllIntRegex})i?({ZeroToNineIntegerRegex})|({AllIntRegex}))a?v[oa]s?)'
    FractionNotationRegex = f'(((?<=\\W|^)-\\s*)|(?<=\\b))\\d+[/]\\d+(?=(\\b[^/]|$))'
    FractionNotationWithSpacesRegex = f'(((?<=\\W|^)-\\s*)|(?<=\\b))\\d+\\s+\\d+[/]\\d+(?=(\\b[^/]|$))'
    FractionNounRegex = f'(?<=\\b)({AllIntRegex}\\s+((y|con)\\s+)?)?({AllIntRegex})(\\s+((y|con)\\s)?)((({AllOrdinalRegex})s?|({SpecialFractionInteger})|({SufixRoundOrdinalRegex})s?)|medi[oa]s?|tercios?)(?=\\b)'
    FractionNounWithArticleRegex = f'(?<=\\b)({AllIntRegex}\\s+(y\\s+)?)?(un|un[oa])(\\s+)(({AllOrdinalRegex})|({SufixRoundOrdinalRegex})|(y\\s+)?medi[oa]s?)(?=\\b)'
    FractionPrepositionRegex = f'(?<=\\b)(?<numerator>({AllIntRegex})|((?<!\\.)\\d+))\\s+sobre\\s+(?<denominator>({AllIntRegex})|((\\d+)(?!\\.)))(?=\\b)'
    AllPointRegex = f'((\\s+{ZeroToNineIntegerRegex})+|(\\s+{AllIntRegex}))'
    AllFloatRegex = f'{AllIntRegex}(\\s+(coma|con)){AllPointRegex}'
    DoubleDecimalPointRegex = lambda placeholder: f'(((?<!\\d+\\s*)-\\s*)|((?<=\\b)(?<!\\d+[\\.,])))\\d+[\\.,]\\d+(?!([\\.,]\\d+))(?={placeholder})'
    DoubleWithoutIntegralRegex = lambda placeholder: f'(?<=\\s|^)(?<!(\\d+))[\\.,]\\d+(?!([\\.,]\\d+))(?={placeholder})'
    DoubleWithMultiplierRegex = f'(((?<!\\d+\\s*)-\\s*)|((?<=\\b)(?<!\\d+\\[\\.,])))\\d+[\\.,]\\d+\\s*{BaseNumbers.NumberMultiplierRegex}(?=\\b)'
    DoubleWithRoundNumber = f'(((?<!\\d+\\s*)-\\s*)|((?<=\\b)(?<!\\d+\\[\\.,])))\\d+[\\.,]\\d+\\s+{RoundNumberIntegerRegex}(?=\\b)'
    DoubleAllFloatRegex = f'((?<=\\b){AllFloatRegex}(?=\\b))'
    DoubleExponentialNotationRegex = f'(((?<!\\d+\\s*)-\\s*)|((?<=\\b)(?<!\\d+[\\.,])))(\\d+([\\.,]\\d+)?)e([+-]*[1-9]\\d*)(?=\\b)'
    DoubleCaretExponentialNotationRegex = f'(((?<!\\d+\\s*)-\\s*)|((?<=\\b)(?<!\\d+[\\.,])))(\\d+([\\.,]\\d+)?)\\^([+-]*[1-9]\\d*)(?=\\b)'
    NumberWithPrefixPercentage = f'(?<!%)({BaseNumbers.NumberReplaceToken})(\\s*)(%(?!{BaseNumbers.NumberReplaceToken})|(por ciento|por cien)\\b)'
    TillRegex = f'(\\ba\\b|hasta|--|-|—|——|~|–)'
    MoreRegex = f'(más\\s+alt[oa]\\s+que|(m[áa]s|mayor(es)?|por\\s+encima)(\\s+(que|de|del))?|(?<!<|=)>)'
    LessRegex = f'((menos|menor|menores|por\\s+debajo)(\\s+(que|de|del))?|más\\s+baj[oa]\\s+que|(?<!>|=)<)'
    EqualRegex = f'((igual(es)?|equivalente(s)?|equivale|equivalen|son)(\\s+(a|que|de|al|del))?|(?<!<|>)=)'
    MoreOrEqualPrefix = f'((no\\s+{LessRegex})|(por\\s+lo\\s+menos|como\\s+m[íi]nimo|al\\s+menos))'
    MoreOrEqual = f'(({MoreRegex}\\s+(o)?\\s+{EqualRegex})|({EqualRegex}\\s+(o|y)\\s+{MoreRegex})|{MoreOrEqualPrefix}(\\s+(o)\\s+{EqualRegex})?|({EqualRegex}\\s+(o)\\s+)?{MoreOrEqualPrefix}|>\\s*=)'
    MoreOrEqualSuffix = f'((\\b(y|o)\\b\\s+(m[áa]s|mayor|mayores)((?!\\s+(alt[oa]|baj[oa]|que|de|del))|(\\s+(que|de|del)(?!(\\s*\\d+)))))|como\\s+m[áa]ximo|por\\s+lo\\s+menos|al\\s+menos)'
    LessOrEqualPrefix = f'((no\\s+{MoreRegex})|(como\\s+máximo|como\\s+maximo|como\\s+mucho))'
    LessOrEqual = f'(({LessRegex}\\s+(o)?\\s+{EqualRegex})|({EqualRegex}\\s+(o)?\\s+{LessRegex})|{LessOrEqualPrefix}(\\s+(o)?\\s+{EqualRegex})?|({EqualRegex}\\s+(o)?\\s+)?{LessOrEqualPrefix}|<\\s*=)'
    LessOrEqualSuffix = f'((\\b(y|o)\\b\\s+(menos|menor|menores)((?!\\s+(alt[oa]|baj[oa]|que|de|del))|(\\s+(que|de|del)(?!(\\s*\\d+)))))|como\\s+m[íi]nimo)'
    NumberSplitMark = f'(?![,.](?!\\d+))'
    MoreRegexNoNumberSucceed = f'((m[áa]s|mayor|mayores)((?!\\s+(que|de|del))|\\s+((que|de|del)(?!(\\s*\\d+))))|(por encima)(?!(\\s*\\d+)))'
    LessRegexNoNumberSucceed = f'((menos|menor|menores)((?!\\s+(que|de|del))|\\s+((que|de|del)(?!(\\s*\\d+))))|(por debajo)(?!(\\s*\\d+)))'
    EqualRegexNoNumberSucceed = f'((igual|iguales|equivalente|equivalentes|equivale|equivalen)((?!\\s+(a|que|de|al|del))|(\\s+(a|que|de|al|del)(?!(\\s*\\d+)))))'
    OneNumberRangeMoreRegex1 = f'({MoreOrEqual}|{MoreRegex})\\s*((el|la|los|las)\\s+)?(?<number1>({NumberSplitMark}.)+)'
    OneNumberRangeMoreRegex2 = f'(?<number1>({NumberSplitMark}.)+)\\s*{MoreOrEqualSuffix}'
    OneNumberRangeMoreSeparateRegex = f'({EqualRegex}\\s+(?<number1>({NumberSplitMark}.)+)(\\s+o\\s+){MoreRegexNoNumberSucceed})|({MoreRegex}\\s+(?<number1>({NumberSplitMark}.)+)(\\s+o\\s+){EqualRegexNoNumberSucceed})'
    OneNumberRangeLessRegex1 = f'({LessOrEqual}|{LessRegex})\\s*((el|la|los|las)\\s+)?(?<number2>({NumberSplitMark}.)+)'
    OneNumberRangeLessRegex2 = f'(?<number2>({NumberSplitMark}.)+)\\s*{LessOrEqualSuffix}'
    OneNumberRangeLessSeparateRegex = f'({EqualRegex}\\s+(?<number1>({NumberSplitMark}.)+)(\\s+o\\s+){LessRegexNoNumberSucceed})|({LessRegex}\\s+(?<number1>({NumberSplitMark}.)+)(\\s+o\\s+){EqualRegexNoNumberSucceed})'
    OneNumberRangeEqualRegex = f'{EqualRegex}\\s*((el|la|los|las)\\s+)?(?<number1>({NumberSplitMark}.)+)'
    TwoNumberRangeRegex1 = f'entre\\s*((el|la|los|las)\\s+)?(?<number1>({NumberSplitMark}.)+)\\s*y\\s*((el|la|los|las)\\s+)?(?<number2>({NumberSplitMark}.)+)'
    TwoNumberRangeRegex2 = f'({OneNumberRangeMoreRegex1}|{OneNumberRangeMoreRegex2})\\s*(\\by\\b|\\be\\b|pero|,)\\s*({OneNumberRangeLessRegex1}|{OneNumberRangeLessRegex2})'
    TwoNumberRangeRegex3 = f'({OneNumberRangeLessRegex1}|{OneNumberRangeLessRegex2})\\s*(\\by\\b|\\be\\b|pero|,)\\s*({OneNumberRangeMoreRegex1}|{OneNumberRangeMoreRegex2})'
    TwoNumberRangeRegex4 = f'((de|desde)\\s+)?((el|la|los|las)\\s+)?(?<number1>({NumberSplitMark}(?!\\b(entre|de|desde|es)\\b).)+)\\s*{TillRegex}\\s*((el|la|los|las)\\s+)?(?<number2>({NumberSplitMark}.)+)'
    AmbiguousFractionConnectorsRegex = f'(\\b(en|de)\\b)'
    DecimalSeparatorChar = ','
    FractionMarkerToken = 'sobre'
    NonDecimalSeparatorChar = '.'
    HalfADozenText = 'seis'
    WordSeparatorToken = 'y'
    WrittenDecimalSeparatorTexts = [r'coma', r'con']
    WrittenGroupSeparatorTexts = [r'punto']
    WrittenIntegerSeparatorTexts = [r'y']
    WrittenFractionSeparatorTexts = [r'con']
    HalfADozenRegex = f'media\\s+docena'
    DigitalNumberRegex = f'((?<=\\b)(mil|millones|mill[oó]n|billones|bill[oó]n|trillones|trill[oó]n|docenas?)(?=\\b))|((?<=(\\d|\\b)){BaseNumbers.MultiplierLookupRegex}(?=\\b))'
    CardinalNumberMap = dict([("cero", 0),
                              ("un", 1),
                              ("una", 1),
                              ("uno", 1),
                              ("dos", 2),
                              ("tres", 3),
                              ("cuatro", 4),
                              ("cinco", 5),
                              ("seis", 6),
                              ("siete", 7),
                              ("ocho", 8),
                              ("nueve", 9),
                              ("diez", 10),
                              ("once", 11),
                              ("doce", 12),
                              ("docena", 12),
                              ("docenas", 12),
                              ("trece", 13),
                              ("catorce", 14),
                              ("quince", 15),
                              ("dieciseis", 16),
                              ("dieciséis", 16),
                              ("diecisiete", 17),
                              ("dieciocho", 18),
                              ("diecinueve", 19),
                              ("veinte", 20),
                              ("ventiuna", 21),
                              ("ventiuno", 21),
                              ("veintiun", 21),
                              ("veintiún", 21),
                              ("veintiuno", 21),
                              ("veintiuna", 21),
                              ("veintidos", 22),
                              ("veintidós", 22),
                              ("veintitres", 23),
                              ("veintitrés", 23),
                              ("veinticuatro", 24),
                              ("veinticinco", 25),
                              ("veintiseis", 26),
                              ("veintiséis", 26),
                              ("veintisiete", 27),
                              ("veintiocho", 28),
                              ("veintinueve", 29),
                              ("treinta", 30),
                              ("cuarenta", 40),
                              ("cincuenta", 50),
                              ("sesenta", 60),
                              ("setenta", 70),
                              ("ochenta", 80),
                              ("noventa", 90),
                              ("cien", 100),
                              ("ciento", 100),
                              ("doscientas", 200),
                              ("doscientos", 200),
                              ("trescientas", 300),
                              ("trescientos", 300),
                              ("cuatrocientas", 400),
                              ("cuatrocientos", 400),
                              ("quinientas", 500),
                              ("quinientos", 500),
                              ("seiscientas", 600),
                              ("seiscientos", 600),
                              ("setecientas", 700),
                              ("setecientos", 700),
                              ("ochocientas", 800),
                              ("ochocientos", 800),
                              ("novecientas", 900),
                              ("novecientos", 900),
                              ("mil", 1000),
                              ("millon", 1000000),
                              ("millón", 1000000),
                              ("millones", 1000000),
                              ("billon", 1000000000000),
                              ("billón", 1000000000000),
                              ("billones", 1000000000000),
                              ("trillon", 1000000000000000000),
                              ("trillón", 1000000000000000000),
                              ("trillones", 1000000000000000000)])
    OrdinalNumberMap = dict([("primero", 1),
                             ("primera", 1),
                             ("primer", 1),
                             ("segundo", 2),
                             ("segunda", 2),
                             ("medio", 2),
                             ("media", 2),
                             ("tercero", 3),
                             ("tercera", 3),
                             ("tercer", 3),
                             ("tercio", 3),
                             ("cuarto", 4),
                             ("cuarta", 4),
                             ("quinto", 5),
                             ("quinta", 5),
                             ("sexto", 6),
                             ("sexta", 6),
                             ("septimo", 7),
                             ("septima", 7),
                             ("octavo", 8),
                             ("octava", 8),
                             ("noveno", 9),
                             ("novena", 9),
                             ("decimo", 10),
                             ("décimo", 10),
                             ("decima", 10),
                             ("décima", 10),
                             ("undecimo", 11),
                             ("undecima", 11),
                             ("duodecimo", 12),
                             ("duodecima", 12),
                             ("decimotercero", 13),
                             ("decimotercera", 13),
                             ("decimocuarto", 14),
                             ("decimocuarta", 14),
                             ("decimoquinto", 15),
                             ("decimoquinta", 15),
                             ("decimosexto", 16),
                             ("decimosexta", 16),
                             ("decimoseptimo", 17),
                             ("decimoseptima", 17),
                             ("decimoctavo", 18),
                             ("decimoctava", 18),
                             ("decimonoveno", 19),
                             ("decimonovena", 19),
                             ("vigesimo", 20),
                             ("vigesima", 20),
                             ("trigesimo", 30),
                             ("trigesima", 30),
                             ("cuadragesimo", 40),
                             ("cuadragesima", 40),
                             ("quincuagesimo", 50),
                             ("quincuagesima", 50),
                             ("sexagesimo", 60),
                             ("sexagesima", 60),
                             ("septuagesimo", 70),
                             ("septuagesima", 70),
                             ("octogesimo", 80),
                             ("octogesima", 80),
                             ("nonagesimo", 90),
                             ("nonagesima", 90),
                             ("centesimo", 100),
                             ("centesima", 100),
                             ("ducentesimo", 200),
                             ("ducentesima", 200),
                             ("tricentesimo", 300),
                             ("tricentesima", 300),
                             ("cuadringentesimo", 400),
                             ("cuadringentesima", 400),
                             ("quingentesimo", 500),
                             ("quingentesima", 500),
                             ("sexcentesimo", 600),
                             ("sexcentesima", 600),
                             ("septingentesimo", 700),
                             ("septingentesima", 700),
                             ("octingentesimo", 800),
                             ("octingentesima", 800),
                             ("noningentesimo", 900),
                             ("noningentesima", 900),
                             ("milesimo", 1000),
                             ("milesima", 1000),
                             ("millonesimo", 1000000),
                             ("millonesima", 1000000),
                             ("billonesimo", 1000000000000),
                             ("billonesima", 1000000000000)])
    PrefixCardinalMap = dict([("dos", 2),
                              ("tres", 3),
                              ("cuatro", 4),
                              ("cinco", 5),
                              ("seis", 6),
                              ("siete", 7),
                              ("ocho", 8),
                              ("nueve", 9),
                              ("diez", 10),
                              ("once", 11),
                              ("doce", 12),
                              ("trece", 13),
                              ("catorce", 14),
                              ("quince", 15),
                              ("dieciseis", 16),
                              ("dieciséis", 16),
                              ("diecisiete", 17),
                              ("dieciocho", 18),
                              ("diecinueve", 19),
                              ("veinte", 20),
                              ("ventiuna", 21),
                              ("veintiun", 21),
                              ("veintiún", 21),
                              ("veintidos", 22),
                              ("veintitres", 23),
                              ("veinticuatro", 24),
                              ("veinticinco", 25),
                              ("veintiseis", 26),
                              ("veintisiete", 27),
                              ("veintiocho", 28),
                              ("veintinueve", 29),
                              ("treinta", 30),
                              ("cuarenta", 40),
                              ("cincuenta", 50),
                              ("sesenta", 60),
                              ("setenta", 70),
                              ("ochenta", 80),
                              ("noventa", 90),
                              ("cien", 100),
                              ("doscientos", 200),
                              ("trescientos", 300),
                              ("cuatrocientos", 400),
                              ("quinientos", 500),
                              ("seiscientos", 600),
                              ("setecientos", 700),
                              ("ochocientos", 800),
                              ("novecientos", 900)])
    SuffixOrdinalMap = dict([("milesimo", 1000),
                             ("millonesimo", 1000000),
                             ("billonesimo", 1000000000000)])
    RoundNumberMap = dict([("mil", 1000),
                           ("milesimo", 1000),
                           ("millon", 1000000),
                           ("millón", 1000000),
                           ("millones", 1000000),
                           ("millonesimo", 1000000),
                           ("billon", 1000000000000),
                           ("billón", 1000000000000),
                           ("billones", 1000000000000),
                           ("billonesimo", 1000000000000),
                           ("trillon", 1000000000000000000),
                           ("trillón", 1000000000000000000),
                           ("trillones", 1000000000000000000),
                           ("trillonesimo", 1000000000000000000),
                           ("docena", 12),
                           ("docenas", 12),
                           ("k", 1000),
                           ("m", 1000000),
                           ("g", 1000000000),
                           ("b", 1000000000),
                           ("t", 1000000000000)])
    RelativeReferenceOffsetMap = dict([("", "")])
    RelativeReferenceRelativeToMap = dict([("", "")])
# pylint: enable=line-too-long
