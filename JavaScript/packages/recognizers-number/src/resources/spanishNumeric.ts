// ------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//
// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.
// ------------------------------------------------------------------------------

import { BaseNumbers } from "./baseNumbers";
export namespace SpanishNumeric {
    export const LangMarker = `Spa`;
    export const CompoundNumberLanguage = false;
    export const MultiDecimalSeparatorCulture = true;
    export const NonStandardSeparatorVariants = [ "es-mx","es-do","es-sv","es-gt","es-hn","es-ni","es-pa","es-pr" ];
    export const HundredsNumberIntegerRegex = `(cuatrocient[ao]s|trescient[ao]s|seiscient[ao]s|setecient[ao]s|ochocient[ao]s|novecient[ao]s|doscient[ao]s|quinient[ao]s|(?<!por\\s+)(cien(to)?))`;
    export const RoundNumberIntegerSingRegex = `(((mil\\s+)?mi|bi|cuatri|quinti|sexti|septi)ll[oó]n|mil)`;
    export const RoundNumberIntegerRegex = `(${RoundNumberIntegerSingRegex}(es)?)`;
    export const ZeroToNineIntegerRegex = `(cuatro|cinco|siete|nueve|cero|tres|seis|ocho|dos|un[ao]?)`;
    export const TwoToNineIntegerRegex = `(cuatro|cinco|siete|nueve|tres|seis|ocho|dos)`;
    export const TenToNineteenIntegerRegex = `(diecisiete|diecinueve|diecis[eé]is|dieciocho|catorce|quince|trece|diez|once|doce)`;
    export const TwentiesIntegerRegex = `(veinti(cuatro|cinco|siete|nueve|tr[eé]s|s[eé]is|ocho|d[oó]s|[uú]n[oa]?)|ventiun[ao]|veinte)`;
    export const TensNumberIntegerRegex = `(cincuenta|cuarenta|treinta|se[st]enta|ochenta|noventa)`;
    export const NegativeNumberTermsRegex = `(?<negTerm>(?<!(al|lo)\\s+)menos\\s+)`;
    export const NegativeNumberSignRegex = `^${NegativeNumberTermsRegex}.*`;
    export const DigitsNumberRegex = `\\d|\\d{1,3}(\\.\\d{3})`;
    export const BelowHundredsRegex = `((${TenToNineteenIntegerRegex}|${TwentiesIntegerRegex}|(${TensNumberIntegerRegex}(\\s+y\\s+${ZeroToNineIntegerRegex})?))|${ZeroToNineIntegerRegex})`;
    export const BelowThousandsRegex = `(${HundredsNumberIntegerRegex}(\\s+${BelowHundredsRegex})?|${BelowHundredsRegex})`;
    export const SupportThousandsRegex = `((${BelowThousandsRegex}|${BelowHundredsRegex})\\s+${RoundNumberIntegerRegex}(\\s+${RoundNumberIntegerRegex})?)`;
    export const SeparaIntRegex = `(${SupportThousandsRegex}(\\s+${SupportThousandsRegex})*(\\s+${BelowThousandsRegex})?|${BelowThousandsRegex})`;
    export const AllIntRegex = `(${SeparaIntRegex}|mil(\\s+${BelowThousandsRegex})?|${RoundNumberIntegerSingRegex})`;
    export const PlaceHolderPureNumber = `\\b`;
    export const PlaceHolderDefault = `\\D|\\b`;
    export const NumbersWithPlaceHolder = (placeholder: string) => { return `(((?<!\\d+\\s*)-\\s*)|(?<=\\b))\\d+(?!([\\.,]\\d+[a-zA-Z]))(?=${placeholder})`; }
    export const NumbersWithSuffix = `(((?<=\\W|^)-\\s*)|(?<=\\b))\\d+\\s*${BaseNumbers.NumberMultiplierRegex}(?=\\b)`;
    export const RoundNumberIntegerRegexWithLocks = `(?<=\\b)(${DigitsNumberRegex})+\\s+${RoundNumberIntegerRegex}(?=\\b)`;
    export const NumbersWithDozenSuffix = `(((?<=\\W|^)-\\s*)|(?<=\\b))\\d+\\s+(docena|dz|doz)s?(?=\\b)`;
    export const AllIntRegexWithLocks = `((?<=\\b)${AllIntRegex}(?=\\b))`;
    export const AllIntRegexWithDozenSuffixLocks = `(?<=\\b)(((media\\s+)?\\s+docena)|(${AllIntRegex}\\s+(y|con)\\s+)?(${AllIntRegex}\\s+(docena|dz|doz)s?))(?=\\b)`;
    export const SimpleRoundOrdinalRegex = `(mil[eé]simo|millon[eé]sim[oa]|billon[eé]sim[oa]|trillon[eé]sim[oa]|cuatrillon[eé]sim[oa]|quintillon[eé]sim[oa]|sextillon[eé]sim[oa]|septillon[eé]sim[oa])`;
    export const OneToNineOrdinalRegex = `(primer[oa]?|segund[oa]|tercer[oa]?|cuart[oa]|quint[oa]|sext[oa]|s[eé]ptim[oa]|octav[oa]|noven[oa])`;
    export const TensOrdinalRegex = `(nonag[eé]sim[oa]|octog[eé]sim[oa]|septuag[eé]sim[oa]|sexag[eé]sim[oa]|quincuag[eé]sim[oa]|cuadrag[eé]sim[oa]|trig[eé]sim[oa]|vig[eé]sim[oa]|d[eé]cim[oa])`;
    export const HundredOrdinalRegex = `(cent[eé]sim[oa]|ducent[eé]sim[oa]|tricent[eé]sim[oa]|cuadringent[eé]sim[oa]|quingent[eé]sim[oa]|sexcent[eé]sim[oa]|septingent[eé]sim[oa]|octingent[eé]sim[oa]|noningent[eé]sim[oa])`;
    export const SpecialUnderHundredOrdinalRegex = `(und[eé]cim[oa]|duod[eé]cim[oa]|decimoctav[oa])`;
    export const UnderHundredOrdinalRegex = `(${SpecialUnderHundredOrdinalRegex}|((${TensOrdinalRegex}(\\s)?)?${OneToNineOrdinalRegex})|${TensOrdinalRegex})`;
    export const UnderThousandOrdinalRegex = `(((${HundredOrdinalRegex}(\\s)?)?${UnderHundredOrdinalRegex})|${HundredOrdinalRegex})`;
    export const OverThousandOrdinalRegex = `((${AllIntRegex})([eé]sim[oa]))`;
    export const RelativeOrdinalRegex = `(?<relativeOrdinal>(antes\\s+de|anterior\\s+a)(l|\\s+la)\\s+[uú]ltim[ao]|((ante)?pen)?[uú]ltim[ao]s?|pr[oó]xim[ao]s?|anterior(es)?|actual(es)?|siguientes?)`;
    export const ComplexOrdinalRegex = `((${OverThousandOrdinalRegex}(\\s)?)?${UnderThousandOrdinalRegex}|${OverThousandOrdinalRegex})`;
    export const SufixRoundOrdinalRegex = `((${AllIntRegex})(${SimpleRoundOrdinalRegex}))`;
    export const ComplexRoundOrdinalRegex = `(((${SufixRoundOrdinalRegex}(\\s)?)?${ComplexOrdinalRegex})|${SufixRoundOrdinalRegex})`;
    export const AllOrdinalNumberRegex = `${ComplexOrdinalRegex}|${SimpleRoundOrdinalRegex}|${ComplexRoundOrdinalRegex}`;
    export const AllOrdinalRegex = `(?:${AllOrdinalNumberRegex}s?|${RelativeOrdinalRegex})`;
    export const OrdinalSuffixRegex = `(?<=\\b)(\\d*((1(er|r[oa])|2d[oa]|3r[oa]|4t[oa]|5t[oa]|6t[oa]|7m[oa]|8v[oa]|9n[oa]|0m[oa]|11[vm][oa]|12[vm][oa])|\\d\\.?[ºª]))(?=\\b)`;
    export const OrdinalNounRegex = `(?<=\\b)${AllOrdinalRegex}(?=\\b)`;
    export const SpecialFractionInteger = `(((${AllIntRegex})i?(${ZeroToNineIntegerRegex})|(${AllIntRegex}))a?v[oa]s?)`;
    export const FractionNotationRegex = `${BaseNumbers.FractionNotationRegex}`;
    export const FractionNotationWithSpacesRegex = `(((?<=\\W|^)-\\s*)|(?<=\\b))\\d+\\s+\\d+[/]\\d+(?=(\\b[^/]|$))`;
    export const FractionMultiplierRegex = `(?<fracMultiplier>\\s+(y|con)\\s+(medio|(un|${TwoToNineIntegerRegex})\\s+(medio|terci[oa]?|cuart[oa]|quint[oa]|sext[oa]|s[eé]ptim[oa]|octav[oa]|noven[oa]|d[eé]cim[oa])s?))`;
    export const RoundMultiplierWithFraction = `(?<multiplier>(?:(mil\\s+millones|mill[oó]n(es)?|bill[oó]n(es)?|trill[oó]n(es)?|cuatrill[oó]n(es)?|quintill[oó]n(es)?|sextill[oó]n(es)?|septill[oó]n(es)?)))(?=${FractionMultiplierRegex}?$)`;
    export const RoundMultiplierRegex = `\\b\\s*(${RoundMultiplierWithFraction}|(?<multiplier>(mil))$)`;
    export const FractionNounRegex = `(?<=\\b)(${AllIntRegex}\\s+((y|con)\\s+)?)?(${AllIntRegex}\\s+(((${AllOrdinalNumberRegex}|${SufixRoundOrdinalRegex})s|${SpecialFractionInteger})|((y|con)\\s+)?(medi[oa]s?|tercios?))|(medio|un\\s+cuarto\\s+de)\\s+${RoundNumberIntegerRegex})(?=\\b)`;
    export const FractionNounWithArticleRegex = `(?<=\\b)((${AllIntRegex}|${RoundNumberIntegerRegexWithLocks})\\s+((y|con)\\s+)?)?((un|un[oa])(\\s+)((${AllOrdinalNumberRegex})|(${SufixRoundOrdinalRegex}))|(un[ao]?\\s+)?medi[oa]s?|mitad)(?=\\b)`;
    export const FractionPrepositionRegex = `(?<!${BaseNumbers.CommonCurrencySymbol}\\s*)(?<=\\b)(?<numerator>(${AllIntRegex})|((?<!\\.)\\d+))\\s+sobre\\s+(?<denominator>(${AllIntRegex})|((\\d+)(?!\\.)))(?=\\b)`;
    export const AllPointRegex = `((\\s+${ZeroToNineIntegerRegex})+|(\\s+${AllIntRegex}))`;
    export const AllFloatRegex = `${AllIntRegex}(\\s+(coma|con))${AllPointRegex}`;
    export const DoubleDecimalPointRegex = (placeholder: string) => { return `(((?<!\\d+\\s*)-\\s*)|((?<=\\b)(?<!\\d+[\\.,])))\\d+[\\.,]\\d+(?!([\\.,]\\d+))(?=${placeholder})`; }
    export const DoubleWithoutIntegralRegex = (placeholder: string) => { return `(?<=\\s|^)(?<!(\\d+))[\\.,]\\d+(?!([\\.,]\\d+))(?=${placeholder})`; }
    export const DoubleWithMultiplierRegex = `(((?<!\\d+\\s*)-\\s*)|((?<=\\b)(?<!\\d+\\[\\.,])))\\d+[\\.,]\\d+\\s*${BaseNumbers.NumberMultiplierRegex}(?=\\b)`;
    export const DoubleWithRoundNumber = `(((?<!\\d+\\s*)-\\s*)|((?<=\\b)(?<!\\d+\\[\\.,])))\\d+[\\.,]\\d+\\s+${RoundNumberIntegerRegex}(?=\\b)`;
    export const DoubleAllFloatRegex = `((?<=\\b)${AllFloatRegex}(?=\\b))`;
    export const DoubleExponentialNotationRegex = `(((?<!\\d+\\s*)-\\s*)|((?<=\\b)(?<!\\d+[\\.,])))(\\d+([\\.,]\\d+)?)e([+-]*[1-9]\\d*)(?=\\b)`;
    export const DoubleCaretExponentialNotationRegex = `(((?<!\\d+\\s*)-\\s*)|((?<=\\b)(?<!\\d+[\\.,])))(\\d+([\\.,]\\d+)?)\\^([+-]*[1-9]\\d*)(?=\\b)`;
    export const NumberWithPrefixPercentage = `(?<!%)(${BaseNumbers.NumberReplaceToken})(\\s*)(%(?!${BaseNumbers.NumberReplaceToken})|por\\s+cien(to)?\\b)`;
    export const TillRegex = `(\\ba\\b|hasta|--|-|—|——|~|–)`;
    export const MoreRegex = `(más\\s+(alt[oa]s?|grandes)\\s+que|(m[áa]s|mayor(es)?|superior(es)?|por\\s+encima)\\b((\\s+(que|del?|al?))|(?=\\s+o\\b))|(?<!<|=)>)`;
    export const LessRegex = `((meno(s|r(es)?)|inferior(es)?|por\\s+debajo)((\\s+(que|del?|al?)|(?=\\s+o\\b)))|más\\s+baj[oa]\\s+que|(?<!>|=)<)`;
    export const EqualRegex = `((igual(es)?|equivalente(s)?|equivalen?)(\\s+(al?|que|del?))?|(?<!<|>)=)`;
    export const MoreOrEqualPrefix = `((no\\s+${LessRegex})|(por\\s+lo\\s+menos|como\\s+m[íi]nimo|al\\s+menos))`;
    export const MoreOrEqual = `((${MoreRegex}\\s+(o)?\\s+${EqualRegex})|(${EqualRegex}\\s+(o|y)\\s+${MoreRegex})|${MoreOrEqualPrefix}(\\s+(o)\\s+${EqualRegex})?|(${EqualRegex}\\s+(o)\\s+)?${MoreOrEqualPrefix}|>\\s*=)`;
    export const MoreOrEqualSuffix = `((\\b(y|o)\\b\\s+(m[áa]s|mayor(es)?|superior(es)?)((?!\\s+(alt[oa]|baj[oa]|que|del?|al?))|(\\s+(que|del?|al?)(?!(\\s*\\d+)))))|como\\s+m[íi]nimo|por\\s+lo\\s+menos|al\\s+menos)\\b`;
    export const LessOrEqualPrefix = `((no\\s+${MoreRegex})|(como\\s+(m[aá]ximo|mucho)))`;
    export const LessOrEqual = `((${LessRegex}\\s+(o)?\\s+${EqualRegex})|(${EqualRegex}\\s+(o)?\\s+${LessRegex})|${LessOrEqualPrefix}(\\s+(o)?\\s+${EqualRegex})?|(${EqualRegex}\\s+(o)?\\s+)?${LessOrEqualPrefix}|<\\s*=)`;
    export const LessOrEqualSuffix = `((\\b(y|o)\\b\\s+(meno(s|r(es)?|inferior(es)?))((?!\\s+(alt[oa]|baj[oa]|que|del?|al?))|(\\s+(que|del?|al?)(?!(\\s*\\d+)))))|como\\s+m[áa]ximo)\\b`;
    export const NumberSplitMark = `(?![,.](?!\\d+))(?!\\s*\\b(((y|e)\\s+)?(${LessRegex}|${MoreRegex}|${EqualRegex}|no|de)|pero|o|a)\\b)`;
    export const MoreRegexNoNumberSucceed = `(\\b(m[áa]s|mayor(es)?|superior(es)?)((?!\\s+(que|del?|al?))|\\s+((que|del?)(?!(\\s*\\d+))))|(por encima)(?!(\\s*\\d+)))\\b`;
    export const LessRegexNoNumberSucceed = `(\\b(meno(s|r(es)?)|inferior(es)?)((?!\\s+(que|del?|al?))|\\s+((que|del?|al?)(?!(\\s*\\d+))))|(por debajo)(?!(\\s*\\d+)))\\b`;
    export const EqualRegexNoNumberSucceed = `(\\b(igual(es)?|equivalentes?|equivalen?)((?!\\s+(al?|que|del?))|(\\s+(al?|que|del?)(?!(\\s*\\d+)))))\\b`;
    export const OneNumberRangeMoreRegex1 = `(${MoreOrEqual}|${MoreRegex})\\s*((el|las?|los)\\s+)?(?<number1>(${NumberSplitMark}.)+)`;
    export const OneNumberRangeMoreRegex1LB = `(?<!no\\s+)${OneNumberRangeMoreRegex1}`;
    export const OneNumberRangeMoreRegex2 = `(?<number1>(${NumberSplitMark}.)+)\\s*${MoreOrEqualSuffix}`;
    export const OneNumberRangeMoreSeparateRegex = `(${EqualRegex}\\s+(?<number1>(${NumberSplitMark}.)+)(\\s+o\\s+)${MoreRegexNoNumberSucceed})|(${MoreRegex}\\s+(?<number1>(${NumberSplitMark}.)+)(\\s+o\\s+)${EqualRegexNoNumberSucceed})`;
    export const OneNumberRangeLessRegex1 = `(${LessOrEqual}|${LessRegex})\\s*((el|las?|los)\\s+)?(?<number2>(${NumberSplitMark}.)+)`;
    export const OneNumberRangeLessRegex1LB = `(?<!no\\s+)${OneNumberRangeLessRegex1}`;
    export const OneNumberRangeLessRegex2 = `(?<number2>(${NumberSplitMark}.)+)\\s*${LessOrEqualSuffix}`;
    export const OneNumberRangeLessSeparateRegex = `(${EqualRegex}\\s+(?<number1>(${NumberSplitMark}.)+)(\\s+o\\s+)${LessRegexNoNumberSucceed})|(${LessRegex}\\s+(?<number1>(${NumberSplitMark}.)+)(\\s+o\\s+)${EqualRegexNoNumberSucceed})`;
    export const OneNumberRangeEqualRegex = `${EqualRegex}\\s*((el|las?|los)\\s+)?(?<number1>(${NumberSplitMark}.)+)`;
    export const TwoNumberRangeRegex1 = `\\bentre\\s*((el|las?|los)\\s+)?(?<number1>(${NumberSplitMark}.)+)\\s*y\\s*((el|las?|los)\\s+)?(?<number2>(${NumberSplitMark}.)+)`;
    export const TwoNumberRangeRegex2 = `(${OneNumberRangeMoreRegex1}|${OneNumberRangeMoreRegex2})\\s*(\\by\\b|\\be\\b|pero|,)\\s*(${OneNumberRangeLessRegex1}|${OneNumberRangeLessRegex2})`;
    export const TwoNumberRangeRegex3 = `(${OneNumberRangeLessRegex1}|${OneNumberRangeLessRegex2})\\s*(\\by\\b|\\be\\b|pero|,)\\s*(${OneNumberRangeMoreRegex1}|${OneNumberRangeMoreRegex2})`;
    export const TwoNumberRangeRegex4 = `(\\bde(sde)?\\s+)?(\\b(el|las?|los)\\s+)?\\b(?!\\s+)(?<number1>(${NumberSplitMark}(?!\\b(entre|de(sde)?|es)\\b).)+)\\b\\s*${TillRegex}\\s*((el|las?|los)\\s+)?\\b(?!\\s+)(?<number2>(${NumberSplitMark}.)+)\\b`;
    export const AmbiguousFractionConnectorsRegex = `(\\b(en|de)\\b)`;
    export const DecimalSeparatorChar = `,`;
    export const FractionMarkerToken = `sobre`;
    export const NonDecimalSeparatorChar = `.`;
    export const HalfADozenText = `seis`;
    export const WordSeparatorToken = `y`;
    export const WrittenDecimalSeparatorTexts = [ "coma","con" ];
    export const WrittenGroupSeparatorTexts = [ "punto" ];
    export const WrittenIntegerSeparatorTexts = [ "y" ];
    export const WrittenFractionSeparatorTexts = [ "con" ];
    export const OneHalfTokens = [ "un","medio" ];
    export const HalfADozenRegex = `media\\s+docena`;
    export const DigitalNumberRegex = `((?<=\\b)(mil(l[oó]n(es)?)?|bill[oó]n(es)?|trill[oó]n(es)?|(docena|dz|doz)s?)(?=\\b))|((?<=(\\d|\\b))${BaseNumbers.MultiplierLookupRegex}(?=\\b))`;
    export const CardinalNumberMap: ReadonlyMap<string, number> = new Map<string, number>([["cero", 0],["un", 1],["una", 1],["uno", 1],["dos", 2],["tres", 3],["cuatro", 4],["cinco", 5],["seis", 6],["siete", 7],["ocho", 8],["nueve", 9],["diez", 10],["once", 11],["doce", 12],["docena", 12],["docenas", 12],["dz", 12],["doz", 12],["dzs", 12],["dozs", 12],["trece", 13],["catorce", 14],["quince", 15],["dieciseis", 16],["dieciséis", 16],["diecisiete", 17],["dieciocho", 18],["diecinueve", 19],["veinte", 20],["ventiuna", 21],["ventiuno", 21],["veintiun", 21],["veintiún", 21],["veintiuno", 21],["veintiuna", 21],["veintidos", 22],["veintidós", 22],["veintitres", 23],["veintitrés", 23],["veinticuatro", 24],["veinticinco", 25],["veintiseis", 26],["veintiséis", 26],["veintisiete", 27],["veintiocho", 28],["veintinueve", 29],["treinta", 30],["cuarenta", 40],["cincuenta", 50],["sesenta", 60],["setenta", 70],["ochenta", 80],["noventa", 90],["cien", 100],["ciento", 100],["doscientas", 200],["doscientos", 200],["trescientas", 300],["trescientos", 300],["cuatrocientas", 400],["cuatrocientos", 400],["quinientas", 500],["quinientos", 500],["seiscientas", 600],["seiscientos", 600],["setecientas", 700],["setecientos", 700],["ochocientas", 800],["ochocientos", 800],["novecientas", 900],["novecientos", 900],["mil", 1000],["millon", 1000000],["millón", 1000000],["millones", 1000000],["billon", 1000000000000],["billón", 1000000000000],["billones", 1000000000000],["trillon", 1000000000000000000],["trillón", 1000000000000000000],["trillones", 1000000000000000000]]);
    export const OrdinalNumberMap: ReadonlyMap<string, number> = new Map<string, number>([["primero", 1],["primera", 1],["primer", 1],["segundo", 2],["segunda", 2],["medio", 2],["media", 2],["mitad", 2],["tercero", 3],["tercera", 3],["tercer", 3],["tercio", 3],["cuarto", 4],["cuarta", 4],["quinto", 5],["quinta", 5],["sexto", 6],["sexta", 6],["septimo", 7],["septima", 7],["séptimo", 7],["séptima", 7],["octavo", 8],["octava", 8],["noveno", 9],["novena", 9],["decimo", 10],["décimo", 10],["decima", 10],["décima", 10],["undecimo", 11],["undecima", 11],["undécimo", 11],["undécima", 11],["duodecimo", 12],["duodecima", 12],["duodécimo", 12],["duodécima", 12],["decimotercero", 13],["decimotercera", 13],["decimocuarto", 14],["decimocuarta", 14],["decimoquinto", 15],["decimoquinta", 15],["decimosexto", 16],["decimosexta", 16],["decimoseptimo", 17],["decimoseptima", 17],["decimoctavo", 18],["decimoctava", 18],["decimonoveno", 19],["decimonovena", 19],["vigesimo", 20],["vigesima", 20],["vigésimo", 20],["vigésima", 20],["trigesimo", 30],["trigesima", 30],["trigésimo", 30],["trigésima", 30],["cuadragesimo", 40],["cuadragesima", 40],["cuadragésimo", 40],["cuadragésima", 40],["quincuagesimo", 50],["quincuagesima", 50],["quincuagésimo", 50],["quincuagésima", 50],["sexagesimo", 60],["sexagesima", 60],["sexagésimo", 60],["sexagésima", 60],["septuagesimo", 70],["septuagesima", 70],["septuagésimo", 70],["septuagésima", 70],["octogesimo", 80],["octogesima", 80],["octogésimo", 80],["octogésima", 80],["nonagesimo", 90],["nonagesima", 90],["nonagésimo", 90],["nonagésima", 90],["centesimo", 100],["centesima", 100],["centésimo", 100],["centésima", 100],["ducentesimo", 200],["ducentesima", 200],["ducentésimo", 200],["ducentésima", 200],["tricentesimo", 300],["tricentesima", 300],["tricentésimo", 300],["tricentésima", 300],["cuadringentesimo", 400],["cuadringentesima", 400],["cuadringentésimo", 400],["cuadringentésima", 400],["quingentesimo", 500],["quingentesima", 500],["quingentésimo", 500],["quingentésima", 500],["sexcentesimo", 600],["sexcentesima", 600],["sexcentésimo", 600],["sexcentésima", 600],["septingentesimo", 700],["septingentesima", 700],["septingentésimo", 700],["septingentésima", 700],["octingentesimo", 800],["octingentesima", 800],["octingentésimo", 800],["octingentésima", 800],["noningentesimo", 900],["noningentesima", 900],["noningentésimo", 900],["noningentésima", 900],["milesimo", 1000],["milesima", 1000],["milésimo", 1000],["milésima", 1000],["millonesimo", 1000000],["millonesima", 1000000],["millonésimo", 1000000],["millonésima", 1000000],["billonesimo", 1000000000000],["billonesima", 1000000000000],["billonésimo", 1000000000000],["billonésima", 1000000000000],["primeros", 1],["primeras", 1],["segundos", 2],["segundas", 2],["terceros", 3],["terceras", 3],["tercios", 3],["cuartos", 4],["cuartas", 4],["quintos", 5],["quintas", 5],["sextos", 6],["sextas", 6],["septimos", 7],["septimas", 7],["séptimos", 7],["séptimas", 7],["octavos", 8],["octavas", 8],["novenos", 9],["novenas", 9],["decimos", 10],["décimos", 10],["decimas", 10],["décimas", 10],["undecimos", 11],["undecimas", 11],["undécimos", 11],["undécimas", 11],["duodecimos", 12],["duodecimas", 12],["duodécimos", 12],["duodécimas", 12],["decimoterceros", 13],["decimoterceras", 13],["decimocuartos", 14],["decimocuartas", 14],["decimoquintos", 15],["decimoquintas", 15],["decimosextos", 16],["decimosextas", 16],["decimoseptimos", 17],["decimoseptimas", 17],["decimoctavos", 18],["decimoctavas", 18],["decimonovenos", 19],["decimonovenas", 19],["vigesimos", 20],["vigesimas", 20],["vigésimos", 20],["vigésimas", 20],["trigesimos", 30],["trigesimas", 30],["trigésimos", 30],["trigésimas", 30],["cuadragesimos", 40],["cuadragesimas", 40],["cuadragésimos", 40],["cuadragésimas", 40],["quincuagesimos", 50],["quincuagesimas", 50],["quincuagésimos", 50],["quincuagésimas", 50],["sexagesimos", 60],["sexagesimas", 60],["sexagésimos", 60],["sexagésimas", 60],["septuagesimos", 70],["septuagesimas", 70],["septuagésimos", 70],["septuagésimas", 70],["octogesimos", 80],["octogesimas", 80],["octogésimos", 80],["octogésimas", 80],["nonagesimos", 90],["nonagesimas", 90],["nonagésimos", 90],["nonagésimas", 90],["centesimos", 100],["centesimas", 100],["centésimos", 100],["centésimas", 100],["ducentesimos", 200],["ducentesimas", 200],["ducentésimos", 200],["ducentésimas", 200],["tricentesimos", 300],["tricentesimas", 300],["tricentésimos", 300],["tricentésimas", 300],["cuadringentesimos", 400],["cuadringentesimas", 400],["cuadringentésimos", 400],["cuadringentésimas", 400],["quingentesimos", 500],["quingentesimas", 500],["quingentésimos", 500],["quingentésimas", 500],["sexcentesimos", 600],["sexcentesimas", 600],["sexcentésimos", 600],["sexcentésimas", 600],["septingentesimos", 700],["septingentesimas", 700],["septingentésimos", 700],["septingentésimas", 700],["octingentesimos", 800],["octingentesimas", 800],["octingentésimos", 800],["octingentésimas", 800],["noningentesimos", 900],["noningentesimas", 900],["noningentésimos", 900],["noningentésimas", 900],["milesimos", 1000],["milesimas", 1000],["milésimos", 1000],["milésimas", 1000],["millonesimos", 1000000],["millonesimas", 1000000],["millonésimos", 1000000],["millonésimas", 1000000],["billonesimos", 1000000000000],["billonesimas", 1000000000000],["billonésimos", 1000000000000],["billonésimas", 1000000000000]]);
    export const PrefixCardinalMap: ReadonlyMap<string, number> = new Map<string, number>([["dos", 2],["tres", 3],["cuatro", 4],["cinco", 5],["seis", 6],["siete", 7],["ocho", 8],["nueve", 9],["diez", 10],["once", 11],["doce", 12],["trece", 13],["catorce", 14],["quince", 15],["dieciseis", 16],["dieciséis", 16],["diecisiete", 17],["dieciocho", 18],["diecinueve", 19],["veinte", 20],["ventiuna", 21],["veintiun", 21],["veintiún", 21],["veintidos", 22],["veintitres", 23],["veinticuatro", 24],["veinticinco", 25],["veintiseis", 26],["veintisiete", 27],["veintiocho", 28],["veintinueve", 29],["treinta", 30],["cuarenta", 40],["cincuenta", 50],["sesenta", 60],["setenta", 70],["ochenta", 80],["noventa", 90],["cien", 100],["doscientos", 200],["trescientos", 300],["cuatrocientos", 400],["quinientos", 500],["seiscientos", 600],["setecientos", 700],["ochocientos", 800],["novecientos", 900]]);
    export const SuffixOrdinalMap: ReadonlyMap<string, number> = new Map<string, number>([["milesimo", 1000],["millonesimo", 1000000],["billonesimo", 1000000000000]]);
    export const RoundNumberMap: ReadonlyMap<string, number> = new Map<string, number>([["mil", 1000],["milesimo", 1000],["millon", 1000000],["millón", 1000000],["millones", 1000000],["millonesimo", 1000000],["billon", 1000000000000],["billón", 1000000000000],["billones", 1000000000000],["billonesimo", 1000000000000],["trillon", 1000000000000000000],["trillón", 1000000000000000000],["trillones", 1000000000000000000],["trillonesimo", 1000000000000000000],["docena", 12],["docenas", 12],["dz", 12],["doz", 12],["dzs", 12],["dozs", 12],["k", 1000],["m", 1000000],["g", 1000000000],["b", 1000000000],["t", 1000000000000]]);
    export const AmbiguityFiltersDict: ReadonlyMap<string, string> = new Map<string, string>([["^[.]", ""]]);
    export const RelativeReferenceOffsetMap: ReadonlyMap<string, string> = new Map<string, string>([["proxima", ""],["proximo", ""],["proximas", ""],["proximos", ""],["próxima", ""],["próximo", ""],["próximas", ""],["próximos", ""],["anterior", ""],["anteriores", ""],["actual", ""],["actuales", ""],["siguiente", ""],["siguientes", ""],["ultima", ""],["ultimo", ""],["última", ""],["último", ""],["ultimas", ""],["ultimos", ""],["últimas", ""],["últimos", ""],["penultima", ""],["penultimo", ""],["penúltima", ""],["penúltimo", ""],["penultimas", ""],["penultimos", ""],["penúltimas", ""],["penúltimos", ""],["antepenultima", ""],["antepenultimo", ""],["antepenúltima", ""],["antepenúltimo", ""],["antepenultimas", ""],["antepenultimos", ""],["antepenúltimas", ""],["antepenúltimos", ""],["antes de la ultima", ""],["antes del ultimo", ""],["antes de la última", ""],["antes del último", ""],["anterior al ultimo", ""],["anterior a la ultima", ""],["anterior al último", ""],["anterior a la última", ""]]);
    export const RelativeReferenceRelativeToMap: ReadonlyMap<string, string> = new Map<string, string>([["proxima", "current"],["proximo", "current"],["proximas", "current"],["proximos", "current"],["próxima", "current"],["próximo", "current"],["próximas", "current"],["próximos", "current"],["anterior", "current"],["anteriores", "current"],["actual", "current"],["actuales", "current"],["siguiente", "current"],["siguientes", "current"],["ultima", "end"],["ultimo", "end"],["última", "end"],["último", "end"],["ultimas", "end"],["ultimos", "end"],["últimas", "end"],["últimos", "end"],["penultima", "end"],["penultimo", "end"],["penúltima", "end"],["penúltimo", "end"],["penultimas", "end"],["penultimos", "end"],["penúltimas", "end"],["penúltimos", "end"],["antepenultima", "end"],["antepenultimo", "end"],["antepenúltima", "end"],["antepenúltimo", "end"],["antepenultimas", "end"],["antepenultimos", "end"],["antepenúltimas", "end"],["antepenúltimos", "end"],["antes de la ultima", "end"],["antes del ultimo", "end"],["antes de la última", "end"],["antes del último", "end"],["anterior al ultimo", "end"],["anterior a la ultima", "end"],["anterior al último", "end"],["anterior a la última", "end"]]);
}
