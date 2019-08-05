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
export namespace EnglishNumeric {
    export const LangMarker = 'Eng';
    export const CompoundNumberLanguage = false;
    export const MultiDecimalSeparatorCulture = true;
    export const RoundNumberIntegerRegex = `(?:hundred|thousand|million|billion|trillion)`;
    export const ZeroToNineIntegerRegex = `(?:three|seven|eight|four|five|zero|nine|one|two|six)`;
    export const TwoToNineIntegerRegex = `(?:three|seven|eight|four|five|nine|two|six)`;
    export const NegativeNumberTermsRegex = `(?<negTerm>(minus|negative)\\s+)`;
    export const NegativeNumberSignRegex = `^${NegativeNumberTermsRegex}.*`;
    export const AnIntRegex = `(an?)(?=\\s)`;
    export const TenToNineteenIntegerRegex = `(?:seventeen|thirteen|fourteen|eighteen|nineteen|fifteen|sixteen|eleven|twelve|ten)`;
    export const TensNumberIntegerRegex = `(?:seventy|twenty|thirty|eighty|ninety|forty|fifty|sixty)`;
    export const SeparaIntRegex = `(?:((${TenToNineteenIntegerRegex}|(${TensNumberIntegerRegex}(\\s+(and\\s+)?|\\s*-\\s*)${ZeroToNineIntegerRegex})|${TensNumberIntegerRegex}|${ZeroToNineIntegerRegex})(\\s+${RoundNumberIntegerRegex})*))|((${AnIntRegex}(\\s+${RoundNumberIntegerRegex})+))`;
    export const AllIntRegex = `(?:(((${TenToNineteenIntegerRegex}|(${TensNumberIntegerRegex}(\\s+(and\\s+)?|\\s*-\\s*)${ZeroToNineIntegerRegex})|${TensNumberIntegerRegex}|${ZeroToNineIntegerRegex}|${AnIntRegex})(\\s+${RoundNumberIntegerRegex})+)\\s+(and\\s+)?)*${SeparaIntRegex})`;
    export const PlaceHolderPureNumber = `\\b`;
    export const PlaceHolderDefault = `\\D|\\b`;
    export const NumbersWithPlaceHolder = (placeholder: string) => {
        return `(((?<!\\d+\\s*)-\\s*)|(?<=\\b))\\d+(?!([\\.,]\\d+[a-zA-Z]))(?=${placeholder})`;
    };
    export const NumbersWithSuffix = `(((?<!\\d+\\s*)-\\s*)|(?<=\\b))\\d+\\s*${BaseNumbers.NumberMultiplierRegex}(?=\\b)`;
    export const RoundNumberIntegerRegexWithLocks = `(?<=\\b)\\d+\\s+${RoundNumberIntegerRegex}(?=\\b)`;
    export const NumbersWithDozenSuffix = `(((?<!\\d+\\s*)-\\s*)|(?<=\\b))\\d+\\s+dozen(s)?(?=\\b)`;
    export const AllIntRegexWithLocks = `((?<=\\b)${AllIntRegex}(?=\\b))`;
    export const AllIntRegexWithDozenSuffixLocks = `(?<=\\b)(((half\\s+)?a\\s+dozen)|(${AllIntRegex}\\s+dozen(s)?))(?=\\b)`;
    export const RoundNumberOrdinalRegex = `(?:hundredth|thousandth|millionth|billionth|trillionth)`;
    export const NumberOrdinalRegex = `(?:first|second|third|fourth|fifth|sixth|seventh|eighth|ninth|tenth|eleventh|twelfth|thirteenth|fourteenth|fifteenth|sixteenth|seventeenth|eighteenth|nineteenth|twentieth|thirtieth|fortieth|fiftieth|sixtieth|seventieth|eightieth|ninetieth)`;
    export const RelativeOrdinalRegex = `(?<relativeOrdinal>(next|previous|current)\\s+one|(the\\s+second|next)\\s+to\\s+last|the\\s+one\\s+before\\s+the\\s+last(\\s+one)?|the\\s+last\\s+but\\s+one|(ante)?penultimate|last|next|previous|current)`;
    export const BasicOrdinalRegex = `(${NumberOrdinalRegex}|${RelativeOrdinalRegex})`;
    export const SuffixBasicOrdinalRegex = `(?:((((${TensNumberIntegerRegex}(\\s+(and\\s+)?|\\s*-\\s*)${ZeroToNineIntegerRegex})|${TensNumberIntegerRegex}|${ZeroToNineIntegerRegex}|${AnIntRegex})(\\s+${RoundNumberIntegerRegex})+)\\s+(and\\s+)?)*(${TensNumberIntegerRegex}(\\s+|\\s*-\\s*))?${BasicOrdinalRegex})`;
    export const SuffixRoundNumberOrdinalRegex = `(?:(${AllIntRegex}\\s+)${RoundNumberOrdinalRegex})`;
    export const AllOrdinalRegex = `(?:${SuffixBasicOrdinalRegex}|${SuffixRoundNumberOrdinalRegex})`;
    export const OrdinalSuffixRegex = `(?<=\\b)(?:(\\d*(1st|2nd|3rd|[4-90]th))|(1[1-2]th))(?=\\b)`;
    export const OrdinalNumericRegex = `(?<=\\b)(?:\\d{1,3}(\\s*,\\s*\\d{3})*\\s*th)(?=\\b)`;
    export const OrdinalRoundNumberRegex = `(?<!an?\\s+)${RoundNumberOrdinalRegex}`;
    export const OrdinalEnglishRegex = `(?<=\\b)${AllOrdinalRegex}(?=\\b)`;
    export const FractionNotationWithSpacesRegex = `(((?<=\\W|^)-\\s*)|(?<=\\b))\\d+\\s+\\d+[/]\\d+(?=(\\b[^/]|$))`;
    export const FractionNotationRegex = `(((?<=\\W|^)-\\s*)|(?<![/-])(?<=\\b))\\d+[/]\\d+(?=(\\b[^/]|$))`;
    export const FractionNounRegex = `(?<=\\b)(${AllIntRegex}\\s+(and\\s+)?)?(${AllIntRegex})(\\s+|\\s*-\\s*)(((${AllOrdinalRegex})|(${RoundNumberOrdinalRegex}))s|halves|quarters)(?=\\b)`;
    export const FractionNounWithArticleRegex = `(?<=\\b)(((${AllIntRegex}\\s+(and\\s+)?)?(an?|one)(\\s+|\\s*-\\s*)(?!\\bfirst\\b|\\bsecond\\b)((${AllOrdinalRegex})|(${RoundNumberOrdinalRegex})|half|quarter))|(half))(?=\\b)`;
    export const FractionPrepositionRegex = `(?<=\\b)(?<numerator>(${AllIntRegex})|((?<![\\.,])\\d+))\\s+(over|in|out\\s+of)\\s+(?<denominator>(${AllIntRegex})|(\\d+)(?![\\.,]))(?=\\b)`;
    export const FractionPrepositionWithinPercentModeRegex = `(?<=\\b)(?<numerator>(${AllIntRegex})|((?<![\\.,])\\d+))\\s+over\\s+(?<denominator>(${AllIntRegex})|(\\d+)(?![\\.,]))(?=\\b)`;
    export const AllPointRegex = `((\\s+${ZeroToNineIntegerRegex})+|(\\s+${SeparaIntRegex}))`;
    export const AllFloatRegex = `${AllIntRegex}(\\s+point)${AllPointRegex}`;
    export const DoubleWithMultiplierRegex = `(((?<!\\d+\\s*)-\\s*)|((?<=\\b)(?<!\\d+[\\.,])))\\d+[\\.,]\\d+\\s*${BaseNumbers.NumberMultiplierRegex}(?=\\b)`;
    export const DoubleExponentialNotationRegex = `(((?<!\\d+\\s*)-\\s*)|((?<=\\b)(?<!\\d+[\\.,])))(\\d+([\\.,]\\d+)?)e([+-]*[1-9]\\d*)(?=\\b)`;
    export const DoubleCaretExponentialNotationRegex = `(((?<!\\d+\\s*)-\\s*)|((?<=\\b)(?<!\\d+[\\.,])))(\\d+([\\.,]\\d+)?)\\^([+-]*[1-9]\\d*)(?=\\b)`;
    export const DoubleDecimalPointRegex = (placeholder: string) => {
        return `(((?<!\\d+\\s*)-\\s*)|((?<=\\b)(?<!\\d+[\\.,])))\\d+[\\.,]\\d+(?!([\\.,]\\d+))(?=${placeholder})`;
    };
    export const DoubleWithoutIntegralRegex = (placeholder: string) => {
        return `(?<=\\s|^)(?<!(\\d+))[\\.,]\\d+(?!([\\.,]\\d+))(?=${placeholder})`;
    };
    export const DoubleWithRoundNumber = `(((?<!\\d+\\s*)-\\s*)|((?<=\\b)(?<!\\d+[\\.,])))\\d+[\\.,]\\d+\\s+${RoundNumberIntegerRegex}(?=\\b)`;
    export const DoubleAllFloatRegex = `((?<=\\b)${AllFloatRegex}(?=\\b))`;
    export const ConnectorRegex = `(?<spacer>and)`;
    export const NumberWithSuffixPercentage = `(?<!%)(${BaseNumbers.NumberReplaceToken})(\\s*)(%(?!${BaseNumbers.NumberReplaceToken})|(per\\s*cents?|percentage|cents?)\\b)`;
    export const FractionNumberWithSuffixPercentage = `((${BaseNumbers.FractionNumberReplaceToken})\\s+of)`;
    export const NumberWithPrefixPercentage = `(per\\s*cents?\\s+of)(\\s*)(${BaseNumbers.NumberReplaceToken})`;
    export const NumberWithPrepositionPercentage = `(${BaseNumbers.NumberReplaceToken})\\s*(in|out\\s+of)\\s*(${BaseNumbers.NumberReplaceToken})`;
    export const TillRegex = `(to|through|--|-|—|——|~|–)`;
    export const MoreRegex = `(?:(bigger|greater|more|higher|larger)(\\s+than)?|above|over|exceed(ed|ing)?|surpass(ed|ing)?|(?<!<|=)>)`;
    export const LessRegex = `(?:(less|lower|smaller|fewer)(\\s+than)?|below|under|(?<!>|=)<)`;
    export const EqualRegex = `(equal(s|ing)?(\\s+(to|than))?|(?<!<|>)=)`;
    export const MoreOrEqualPrefix = `((no\\s+${LessRegex})|(at\\s+least))`;
    export const MoreOrEqual = `(?:(${MoreRegex}\\s+(or)?\\s+${EqualRegex})|(${EqualRegex}\\s+(or)?\\s+${MoreRegex})|${MoreOrEqualPrefix}(\\s+(or)?\\s+${EqualRegex})?|(${EqualRegex}\\s+(or)?\\s+)?${MoreOrEqualPrefix}|>\\s*=)`;
    export const MoreOrEqualSuffix = `((and|or)\\s+(((more|greater|higher|larger|bigger)((?!\\s+than)|(\\s+than(?!(\\s*\\d+)))))|((over|above)(?!\\s+than))))`;
    export const LessOrEqualPrefix = `((no\\s+${MoreRegex})|(at\\s+most)|(up\\s+to))`;
    export const LessOrEqual = `((${LessRegex}\\s+(or)?\\s+${EqualRegex})|(${EqualRegex}\\s+(or)?\\s+${LessRegex})|${LessOrEqualPrefix}(\\s+(or)?\\s+${EqualRegex})?|(${EqualRegex}\\s+(or)?\\s+)?${LessOrEqualPrefix}|<\\s*=)`;
    export const LessOrEqualSuffix = `((and|or)\\s+(less|lower|smaller|fewer)((?!\\s+than)|(\\s+than(?!(\\s*\\d+)))))`;
    export const NumberSplitMark = `(?![,.](?!\\d+))`;
    export const MoreRegexNoNumberSucceed = `((bigger|greater|more|higher|larger)((?!\\s+than)|\\s+(than(?!(\\s*\\d+))))|(above|over)(?!(\\s*\\d+)))`;
    export const LessRegexNoNumberSucceed = `((less|lower|smaller|fewer)((?!\\s+than)|\\s+(than(?!(\\s*\\d+))))|(below|under)(?!(\\s*\\d+)))`;
    export const EqualRegexNoNumberSucceed = `(equal(s|ing)?((?!\\s+(to|than))|(\\s+(to|than)(?!(\\s*\\d+)))))`;
    export const OneNumberRangeMoreRegex1 = `(${MoreOrEqual}|${MoreRegex})\\s*(the\\s+)?(?<number1>(${NumberSplitMark}.)+)`;
    export const OneNumberRangeMoreRegex2 = `(?<number1>(${NumberSplitMark}.)+)\\s*${MoreOrEqualSuffix}`;
    export const OneNumberRangeMoreSeparateRegex = `(${EqualRegex}\\s+(?<number1>(${NumberSplitMark}.)+)(\\s+or\\s+)${MoreRegexNoNumberSucceed})|(${MoreRegex}\\s+(?<number1>(${NumberSplitMark}.)+)(\\s+or\\s+)${EqualRegexNoNumberSucceed})`;
    export const OneNumberRangeLessRegex1 = `(${LessOrEqual}|${LessRegex})\\s*(the\\s+)?(?<number2>(${NumberSplitMark}.)+)`;
    export const OneNumberRangeLessRegex2 = `(?<number2>(${NumberSplitMark}.)+)\\s*${LessOrEqualSuffix}`;
    export const OneNumberRangeLessSeparateRegex = `(${EqualRegex}\\s+(?<number1>(${NumberSplitMark}.)+)(\\s+or\\s+)${LessRegexNoNumberSucceed})|(${LessRegex}\\s+(?<number1>(${NumberSplitMark}.)+)(\\s+or\\s+)${EqualRegexNoNumberSucceed})`;
    export const OneNumberRangeEqualRegex = `${EqualRegex}\\s*(the\\s+)?(?<number1>(${NumberSplitMark}.)+)`;
    export const TwoNumberRangeRegex1 = `between\\s*(the\\s+)?(?<number1>(${NumberSplitMark}.)+)\\s*and\\s*(the\\s+)?(?<number2>(${NumberSplitMark}.)+)`;
    export const TwoNumberRangeRegex2 = `(${OneNumberRangeMoreRegex1}|${OneNumberRangeMoreRegex2})\\s*(and|but|,)\\s*(${OneNumberRangeLessRegex1}|${OneNumberRangeLessRegex2})`;
    export const TwoNumberRangeRegex3 = `(${OneNumberRangeLessRegex1}|${OneNumberRangeLessRegex2})\\s*(and|but|,)\\s*(${OneNumberRangeMoreRegex1}|${OneNumberRangeMoreRegex2})`;
    export const TwoNumberRangeRegex4 = `(from\\s+)?(?<number1>(${NumberSplitMark}(?!\\bfrom\\b).)+)\\s*${TillRegex}\\s*(the\\s+)?(?<number2>(${NumberSplitMark}.)+)`;
    export const AmbiguousFractionConnectorsRegex = `(\\bin\\b)`;
    export const DecimalSeparatorChar = '.';
    export const FractionMarkerToken = 'over';
    export const NonDecimalSeparatorChar = ',';
    export const HalfADozenText = 'six';
    export const WordSeparatorToken = 'and';
    export const WrittenDecimalSeparatorTexts = ["point"];
    export const WrittenGroupSeparatorTexts = ["punto"];
    export const WrittenIntegerSeparatorTexts = ["and"];
    export const WrittenFractionSeparatorTexts = ["and"];
    export const HalfADozenRegex = `half\\s+a\\s+dozen`;
    export const DigitalNumberRegex = `((?<=\\b)(hundred|thousand|[mb]illion|trillion|dozen(s)?)(?=\\b))|((?<=(\\d|\\b))${BaseNumbers.MultiplierLookupRegex}(?=\\b))`;
    export const CardinalNumberMap: ReadonlyMap<string, number> = new Map<string, number>([["a", 1], ["zero", 0], ["an", 1], ["one", 1], ["two", 2], ["three", 3], ["four", 4], ["five", 5], ["six", 6], ["seven", 7], ["eight", 8], ["nine", 9], ["ten", 10], ["eleven", 11], ["twelve", 12], ["dozen", 12], ["dozens", 12], ["thirteen", 13], ["fourteen", 14], ["fifteen", 15], ["sixteen", 16], ["seventeen", 17], ["eighteen", 18], ["nineteen", 19], ["twenty", 20], ["thirty", 30], ["forty", 40], ["fifty", 50], ["sixty", 60], ["seventy", 70], ["eighty", 80], ["ninety", 90], ["hundred", 100], ["thousand", 1000], ["million", 1000000], ["billion", 1000000000], ["trillion", 1000000000000]]);
    export const OrdinalNumberMap: ReadonlyMap<string, number> = new Map<string, number>([["first", 1], ["second", 2], ["secondary", 2], ["half", 2], ["third", 3], ["fourth", 4], ["quarter", 4], ["fifth", 5], ["sixth", 6], ["seventh", 7], ["eighth", 8], ["ninth", 9], ["tenth", 10], ["eleventh", 11], ["twelfth", 12], ["thirteenth", 13], ["fourteenth", 14], ["fifteenth", 15], ["sixteenth", 16], ["seventeenth", 17], ["eighteenth", 18], ["nineteenth", 19], ["twentieth", 20], ["thirtieth", 30], ["fortieth", 40], ["fiftieth", 50], ["sixtieth", 60], ["seventieth", 70], ["eightieth", 80], ["ninetieth", 90], ["hundredth", 100], ["thousandth", 1000], ["millionth", 1000000], ["billionth", 1000000000], ["trillionth", 1000000000000], ["firsts", 1], ["halves", 2], ["thirds", 3], ["fourths", 4], ["quarters", 4], ["fifths", 5], ["sixths", 6], ["sevenths", 7], ["eighths", 8], ["ninths", 9], ["tenths", 10], ["elevenths", 11], ["twelfths", 12], ["thirteenths", 13], ["fourteenths", 14], ["fifteenths", 15], ["sixteenths", 16], ["seventeenths", 17], ["eighteenths", 18], ["nineteenths", 19], ["twentieths", 20], ["thirtieths", 30], ["fortieths", 40], ["fiftieths", 50], ["sixtieths", 60], ["seventieths", 70], ["eightieths", 80], ["ninetieths", 90], ["hundredths", 100], ["thousandths", 1000], ["millionths", 1000000], ["billionths", 1000000000], ["trillionths", 1000000000000]]);
    export const RoundNumberMap: ReadonlyMap<string, number> = new Map<string, number>([["hundred", 100], ["thousand", 1000], ["million", 1000000], ["billion", 1000000000], ["trillion", 1000000000000], ["hundredth", 100], ["thousandth", 1000], ["millionth", 1000000], ["billionth", 1000000000], ["trillionth", 1000000000000], ["hundredths", 100], ["thousandths", 1000], ["millionths", 1000000], ["billionths", 1000000000], ["trillionths", 1000000000000], ["dozen", 12], ["dozens", 12], ["k", 1000], ["m", 1000000], ["g", 1000000000], ["b", 1000000000], ["t", 1000000000000]]);
    export const AmbiguityFiltersDict: ReadonlyMap<string, string> = new Map<string, string>([["\\bone\\b", "\\b(the|this|that|which)\\s+(one)\\b"]]);
    export const RelativeReferenceOffsetMap: ReadonlyMap<string, string> = new Map<string, string>([["last", ""], ["next one", ""], ["current", ""], ["current one", ""], ["previous one", ""], ["the second to last", ""], ["the one before the last one", ""], ["the one before the last", ""], ["next to last", ""], ["penultimate", ""], ["the last but one", ""], ["antepenultimate", ""], ["next", ""], ["previous", ""]]);
    export const RelativeReferenceRelativeToMap: ReadonlyMap<string, string> = new Map<string, string>([["last", "end"], ["next one", "current"], ["previous one", "current"], ["current", "current"], ["current one", "current"], ["the second to last", "end"], ["the one before the last one", "end"], ["the one before the last", "end"], ["next to last", "end"], ["penultimate", "end"], ["the last but one", "end"], ["antepenultimate", "end"], ["next", "current"], ["previous", "current"]]);
}
