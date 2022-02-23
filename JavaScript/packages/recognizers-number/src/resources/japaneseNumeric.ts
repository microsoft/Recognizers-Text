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
export namespace JapaneseNumeric {
    export const LangMarker = `Jpn`;
    export const CompoundNumberLanguage = true;
    export const MultiDecimalSeparatorCulture = false;
    export const DecimalSeparatorChar = `.`;
    export const FractionMarkerToken = ``;
    export const NonDecimalSeparatorChar = ` `;
    export const HalfADozenText = ``;
    export const WordSeparatorToken = ``;
    export const ZeroChar = `零`;
    export const PairChar = `対`;
    export const RoundNumberMap: ReadonlyMap<string, number> = new Map<string, number>([["k", 1000],["m", 1000000],["g", 1000000000],["t", 1000000000000],["b", 1000000000]]);
    export const RoundNumberMapChar: ReadonlyMap<string, number> = new Map<string, number>([["十", 10],["百", 100],["千", 1000],["万", 10000],["億", 100000000],["兆", 1000000000000]]);
    export const ZeroToNineMap: ReadonlyMap<string, number> = new Map<string, number>([["0", 0],["1", 1],["2", 2],["3", 3],["4", 4],["5", 5],["6", 6],["7", 7],["8", 8],["9", 9],["零", 0],["〇", 0],["一", 1],["二", 2],["三", 3],["四", 4],["五", 5],["六", 6],["七", 7],["八", 8],["九", 9],["半", 0.5]]);
    export const FullToHalfMap: ReadonlyMap<string, string> = new Map<string, string>([["０", "0"],["１", "1"],["２", "2"],["３", "3"],["４", "4"],["５", "5"],["６", "6"],["７", "7"],["８", "8"],["９", "9"],["／", "/"],["－", "-"],["，", "'"],["、", "'"],["Ｇ", "G"],["Ｍ", "M"],["Ｔ", "T"],["Ｋ", "K"],["ｋ", "k"],["．", "."]]);
    export const UnitMap: ReadonlyMap<string, string> = new Map<string, string>([["万万", "億"],["億万", "兆"],["万億", "兆"],[" ", ""]]);
    export const RoundDirectList = [ "万","億","兆" ];
    export const TenChars = [ "十" ];
    export const AllMultiplierLookupRegex = `(${BaseNumbers.MultiplierLookupRegex}|ミリリットル(入れら)?|キロメートル|メートル|ミリメート)`;
    export const DigitalNumberRegex = `((?<=(\\d|\\b))${BaseNumbers.MultiplierLookupRegex}(?=\\b))`;
    export const ZeroToNineFullHalfRegex = `[\\d]`;
    export const DigitNumRegex = `${ZeroToNineFullHalfRegex}+`;
    export const DozenRegex = `.*ダース$`;
    export const PercentageRegex = `.+(?=パ\\s*ー\\s*セ\\s*ン\\s*ト)|.*(?=[％%])`;
    export const DoubleAndRoundRegex = `${ZeroToNineFullHalfRegex}+(\\.${ZeroToNineFullHalfRegex}+)?\\s*[万億]{1,2}(\\s*(以上))?`;
    export const FracSplitRegex = `[はと]|分\\s*の`;
    export const ZeroToNineIntegerRegex = `[〇一二三四五六七八九]`;
    export const HalfUnitRegex = `半`;
    export const NegativeNumberTermsRegex = `(マ\\s*イ\\s*ナ\\s*ス)`;
    export const NegativeNumberTermsRegexNum = `((?<!(\\d+(\\s*${BaseNumbers.NumberMultiplierRegex})?\\s*)|[-−－])[-−－])`;
    export const NegativeNumberSignRegex = `^${NegativeNumberTermsRegex}.*|^${NegativeNumberTermsRegexNum}.*`;
    export const SpeGetNumberRegex = `${ZeroToNineFullHalfRegex}|${ZeroToNineIntegerRegex}|[半対]|[分厘]`;
    export const PairRegex = `.*[対膳足]$`;
    export const RoundNumberIntegerRegex = `(十|百|千|万(?!万)|億|兆)`;
    export const AllowListRegex = `(。|，|、|（|）|”｜国|週間|時間|時|匹|キロ|トン|年|個|足|本|で|は|\\s|$|つ|月|の|と)`;
    export const NotSingleRegex = `(?<!(第|だい))(${RoundNumberIntegerRegex}+((${ZeroToNineIntegerRegex}+|${RoundNumberIntegerRegex})+|${ZeroToNineFullHalfRegex}+|十)(\\s*(以上))?)|((${ZeroToNineIntegerRegex}+|${ZeroToNineFullHalfRegex}+|十)(\\s*${RoundNumberIntegerRegex}){1,2})(\\s*([零]?(${ZeroToNineIntegerRegex}+|${ZeroToNineFullHalfRegex}+|十)(\\s*${RoundNumberIntegerRegex}){0,1}))*(\\s*(以上)?)`;
    export const SingleRegex = `((${ZeroToNineIntegerRegex}+|${ZeroToNineFullHalfRegex}+|十)(?=${AllowListRegex}))`;
    export const AllIntRegex = `(?<!(ダース))(${NotSingleRegex}|(${ZeroToNineIntegerRegex}+|${RoundNumberIntegerRegex}+))`;
    export const PlaceHolderPureNumber = `\\b`;
    export const PlaceHolderDefault = `\\D|\\b`;
    export const NumbersSpecialsChars = `(((${NegativeNumberTermsRegexNum}|${NegativeNumberTermsRegex})\\s*)?(${ZeroToNineFullHalfRegex}))+(?=\\b|\\D)(?!(([\\.．]${ZeroToNineFullHalfRegex}+)?\\s*${AllMultiplierLookupRegex}))`;
    export const NumbersSpecialsCharsWithSuffix = `(${NegativeNumberTermsRegexNum}?${ZeroToNineFullHalfRegex}+(\\s*${BaseNumbers.NumberMultiplierRegex}+)?)(?=\\b|\\D)(?!(([\\.．]${ZeroToNineFullHalfRegex}+)?\\s*${AllMultiplierLookupRegex}))`;
    export const DottedNumbersSpecialsChar = `${NegativeNumberTermsRegexNum}?${ZeroToNineFullHalfRegex}{1,3}([,，、]${ZeroToNineFullHalfRegex}{3})+`;
    export const NumbersWithHalfDozen = `半(${RoundNumberIntegerRegex}|(ダース))`;
    export const NumbersWithDozen = `(${AllIntRegex}([と]?${ZeroToNineIntegerRegex})?|${ZeroToNineFullHalfRegex}+)(ダース)`;
    export const PointRegexStr = `[\\.．・]`;
    export const AllFloatRegex = `${NegativeNumberTermsRegex}?${AllIntRegex}\\s*${PointRegexStr}\\s*[一二三四五六七八九](\\s*${ZeroToNineIntegerRegex})*`;
    export const NumbersWithAllowListRegex = `(?<!(離は))(${NegativeNumberTermsRegex}?(${NotSingleRegex}|${SingleRegex})(?!(${AllIntRegex}*([、.]${ZeroToNineIntegerRegex}+)*|${AllFloatRegex})*\\s*${PercentageRegex}+))(?!(\\s*${AllMultiplierLookupRegex}))`;
    export const NumbersAggressiveRegex = `((${AllIntRegex})(?!(${AllIntRegex}|([、.]${ZeroToNineIntegerRegex})|${AllFloatRegex}|\\s*${PercentageRegex})))`;
    export const PointRegex = `${PointRegexStr}`;
    export const DoubleSpecialsChars = `((?<!(${ZeroToNineFullHalfRegex}+[\\.．]${ZeroToNineFullHalfRegex}*))(${NegativeNumberTermsRegexNum}\\s*)?${ZeroToNineFullHalfRegex}+[\\.．,]${ZeroToNineFullHalfRegex}+(?!(${ZeroToNineFullHalfRegex}*[\\.．,]${ZeroToNineFullHalfRegex}+)))(?=\\b|\\D)(?!\\s*${AllMultiplierLookupRegex})`;
    export const DoubleRoundNumberSpecialsChars = `(?<!((${ZeroToNineIntegerRegex}|${RoundNumberIntegerRegex})+[\\.．・,](${ZeroToNineIntegerRegex}|${RoundNumberIntegerRegex})*))((${NegativeNumberTermsRegexNum}|${NegativeNumberTermsRegex})\\s*)?(${ZeroToNineIntegerRegex}|${RoundNumberIntegerRegex})+[\\.．・,](${ZeroToNineIntegerRegex}|${RoundNumberIntegerRegex})+(?!(${ZeroToNineIntegerRegex}|${RoundNumberIntegerRegex})*[\\.．・,](${ZeroToNineIntegerRegex}|${RoundNumberIntegerRegex})+)`;
    export const DoubleSpecialsCharsWithNegatives = `(?<!(${ZeroToNineFullHalfRegex}+|\\.\\.|．．))(${NegativeNumberTermsRegexNum}\\s*)?[\\.．]${ZeroToNineFullHalfRegex}+(?!${ZeroToNineFullHalfRegex}*([\\.．]${ZeroToNineFullHalfRegex}+))`;
    export const SimpleDoubleSpecialsChars = `(${NegativeNumberTermsRegexNum}\\s*)?${ZeroToNineFullHalfRegex}{1,3}([,，]${ZeroToNineFullHalfRegex}{3})+[\\.．]${ZeroToNineFullHalfRegex}+`;
    export const DoubleWithMultiplierRegex = `((${NegativeNumberTermsRegexNum}\\s*)?${ZeroToNineFullHalfRegex}+[\\.．]${ZeroToNineFullHalfRegex}+\\s*${BaseNumbers.NumberMultiplierRegex})(?=\\b|\\D)(?!${AllMultiplierLookupRegex})`;
    export const DoubleWithThousandsRegex = `(${NegativeNumberTermsRegex}{0,1}|${NegativeNumberTermsRegexNum})\\s*(${ZeroToNineFullHalfRegex}+([\\.．]${ZeroToNineFullHalfRegex}+)?\\s*[万亿萬億]{1,2})`;
    export const DoubleAllFloatRegex = `(?<!((${AllIntRegex}[.]*)|${AllFloatRegex})*)${AllFloatRegex}(?!${ZeroToNineIntegerRegex}*\\s*パーセント)`;
    export const DoubleExponentialNotationRegex = `(?<!\\d+[\\.])(${NegativeNumberTermsRegexNum}\\s*)?(\\d+([\\.]\\d+)?)e(([-+＋]*[1-9]\\d*)|[0](?!\\d+))|(?<!\\d+[\\.])(${NegativeNumberTermsRegexNum}\\s*)?(\\d+([\\.]\\d+)?)(×)?(10)?の((([-+＋]*[1-9]\\d*)|[0])[乗](?!\\d+))`;
    export const DoubleExponentialNotationKanjiRegex = `(?<!(${ZeroToNineIntegerRegex})+[\\.．・])((${NegativeNumberTermsRegexNum}|${NegativeNumberTermsRegex})\\s*)?(${ZeroToNineIntegerRegex}|[十千五百])+([\\.．・](${ZeroToNineIntegerRegex})+)?(×)?(十)?(の)((((マ\\s*イ\\s*ナ\\s*ス))*(${ZeroToNineIntegerRegex}|[十])(${ZeroToNineIntegerRegex}|[十])*[乗])(?!(${ZeroToNineIntegerRegex}|[十])+))`;
    export const DoubleScientificNotationRegex = `(?<!\\d+[\\.])(${NegativeNumberTermsRegexNum}\\s*)?(\\d+([\\.]\\d+)?)\\^([-+＋]*[1-9]\\d*)`;
    export const OrdinalNumbersRegex = `(((第|だい)(${ZeroToNineFullHalfRegex}+)(${RoundNumberIntegerRegex}+)?))|((${ZeroToNineFullHalfRegex}+|${ZeroToNineIntegerRegex}+)(${RoundNumberIntegerRegex}+)?(番目|位|等(?!級)))`;
    export const OrdinalRegex = `((${OrdinalNumbersRegex})|((第|だい)(${AllIntRegex})|((${AllIntRegex}+|${NumbersWithAllowListRegex}+)(番目|位|等))))|(最初|1等|ファースト)`;
    export const RelativeOrdinalRegex = `(?<relativeOrdinal>((最後)(から1つ前のこと|から(3|2|1)番目|(から1つ前)(のもの)|から三番目|から二番目|(から(一|1)つ前)(のもの|のこと)?|(から1つ)?(前))?|(次のもの)(前)?|(前(?=の))(のもの)?|(現在)(のこと)?|次|二位))`;
    export const AllOrdinalRegex = `(${OrdinalRegex}|${RelativeOrdinalRegex})`;
    export const AllFractionNumber = `(((${NegativeNumberTermsRegex}{0,1})|${NegativeNumberTermsRegexNum})((${ZeroToNineFullHalfRegex}+|${AllIntRegex})\\s*[はと]{0,1}\\s*)?${NegativeNumberTermsRegex}{0,1}(${ZeroToNineFullHalfRegex}+|${AllIntRegex})\\s*分\\s*の\\s*${NegativeNumberTermsRegex}{0,1}(${ZeroToNineFullHalfRegex}+|${AllIntRegex})+)|半(分|数)`;
    export const FractionNotationSpecialsCharsRegex = `(${NegativeNumberTermsRegexNum}\\s*)?${ZeroToNineFullHalfRegex}+\\s+${ZeroToNineFullHalfRegex}+[/／]${ZeroToNineFullHalfRegex}+`;
    export const FractionNotationRegex = `(${NegativeNumberTermsRegexNum}\\s*)?${ZeroToNineFullHalfRegex}+[/／]${ZeroToNineFullHalfRegex}+`;
    export const PercentagePointRegex = `(?<!${AllIntRegex})(${AllFloatRegex}|${AllIntRegex})\\s*(パ\\s*ー\\s*セ\\s*ン\\s*ト|[%])`;
    export const SimplePercentageRegex = `(${AllFloatRegex}|((${NegativeNumberTermsRegex})?(${AllIntRegex}|[百])))\\s*(パ\\s*ー\\s*セ\\s*ン\\s*ト|[%])`;
    export const NumbersPercentagePointRegex = `(?<!%)((${NegativeNumberTermsRegexNum}|${NegativeNumberTermsRegex})?(${ZeroToNineFullHalfRegex})+([\\.．](${ZeroToNineFullHalfRegex})+)?\\s*(パ\\s*ー\\s*セ\\s*ン\\s*ト|((?<!(%\\d+))%))(?!${ZeroToNineFullHalfRegex}))`;
    export const NumbersPercentageWithSeparatorRegex = `(${ZeroToNineFullHalfRegex}{1,3}[,，、]${ZeroToNineFullHalfRegex}{3})+([\\.．]${ZeroToNineFullHalfRegex}+)*\\s*(パ\\s*ー\\s*セ\\s*ン\\s*ト|[%])`;
    export const NumbersPercentageWithMultiplierRegex = `(?<!${ZeroToNineIntegerRegex})${ZeroToNineFullHalfRegex}+[\\.．]${ZeroToNineFullHalfRegex}+\\s*${BaseNumbers.NumberMultiplierRegex}\\s*(パ\\s*ー\\s*セ\\s*ン\\s*ト|[%])`;
    export const FractionPercentagePointRegex = `(?<!(${ZeroToNineFullHalfRegex}+[\\.．]))${ZeroToNineFullHalfRegex}+[\\.．]${ZeroToNineFullHalfRegex}+(?!([\\.．]${ZeroToNineFullHalfRegex}+))\\s*(パ\\s*ー\\s*セ\\s*ン\\s*ト|[%])`;
    export const FractionPercentageWithSeparatorRegex = `${ZeroToNineFullHalfRegex}{1,3}([,，、]${ZeroToNineFullHalfRegex}{3})+[\\.．]${ZeroToNineFullHalfRegex}+\\s*(パ\\s*ー\\s*セ\\s*ン\\s*ト|[%])`;
    export const FractionPercentageWithMultiplierRegex = `${ZeroToNineFullHalfRegex}+[\\.．]${ZeroToNineFullHalfRegex}+\\s*${BaseNumbers.NumberMultiplierRegex}\\s*(パ\\s*ー\\s*セ\\s*ン\\s*ト|[%])`;
    export const SimpleNumbersPercentageRegex = `(?<!${ZeroToNineIntegerRegex})${ZeroToNineFullHalfRegex}+\\s*(パ\\s*ー\\s*セ\\s*ン\\s*ト|[%])(?!([\\.．]${ZeroToNineFullHalfRegex}+))`;
    export const SimpleNumbersPercentageWithMultiplierRegex = `(?<!${ZeroToNineIntegerRegex})${ZeroToNineFullHalfRegex}+\\s*${BaseNumbers.NumberMultiplierRegex}\\s*(パ\\s*ー\\s*セ\\s*ン\\s*ト|[%])`;
    export const SimpleNumbersPercentagePointRegex = `(?!${ZeroToNineIntegerRegex})${ZeroToNineFullHalfRegex}{1,3}([,，]${ZeroToNineFullHalfRegex}{3})+\\s*(パ\\s*ー\\s*セ\\s*ン\\s*ト|[%])`;
    export const IntegerPercentageRegex = `${ZeroToNineFullHalfRegex}+\\s*(パ\\s*ー\\s*セ\\s*ン\\s*ト|[%])`;
    export const IntegerPercentageWithMultiplierRegex = `${ZeroToNineFullHalfRegex}+\\s*${BaseNumbers.NumberMultiplierRegex}\\s*(パ\\s*ー\\s*セ\\s*ン\\s*ト|[%])`;
    export const NumbersFractionPercentageRegex = `${ZeroToNineFullHalfRegex}{1,3}([,，]${ZeroToNineFullHalfRegex}{3})+\\s*(パ\\s*ー\\s*セ\\s*ン\\s*ト|[%])`;
    export const SimpleIntegerPercentageRegex = `(?<!%)((${NegativeNumberTermsRegexNum}|${NegativeNumberTermsRegex})?${ZeroToNineFullHalfRegex}+([\\.．]${ZeroToNineFullHalfRegex}+)?(\\s*)((?<!(%\\d+))%)(?!${ZeroToNineFullHalfRegex}))`;
    export const NumbersFoldsPercentageRegex = `${ZeroToNineFullHalfRegex}(([\\.．]?|\\s*)${ZeroToNineFullHalfRegex})?\\s*[の]*\\s*割引`;
    export const FoldsPercentageRegex = `${ZeroToNineIntegerRegex}(\\s*[.]?\\s*${ZeroToNineIntegerRegex})?\\s*[の]\\s*割引`;
    export const SimpleFoldsPercentageRegex = `${ZeroToNineFullHalfRegex}\\s*割(\\s*(半|(${ZeroToNineFullHalfRegex}\\s*分\\s*${ZeroToNineFullHalfRegex}\\s*厘)|${ZeroToNineFullHalfRegex}))?`;
    export const SpecialsPercentageRegex = `(${ZeroToNineIntegerRegex}|[十])\\s*割(\\s*(半|${ZeroToNineIntegerRegex}))?`;
    export const NumbersSpecialsPercentageRegex = `(${ZeroToNineFullHalfRegex}[\\.．]${ZeroToNineFullHalfRegex}|10)\\s*割`;
    export const SimpleSpecialsPercentageRegex = `${ZeroToNineIntegerRegex}\\s*[.]\\s*${ZeroToNineIntegerRegex}\\s*割`;
    export const SpecialsFoldsPercentageRegex = `半\\s*分|(?<=ダース)`;
    export const TillRegex = `(から|--|-|—|——|~)`;
    export const MoreRegex = `(大なり|を超える|大きい|高い|大きく|(?<!<|=)>)`;
    export const LessRegex = `(小なり|小さい|低い|(?<!>|=)<)`;
    export const EqualRegex = `(等しい|イコール|(?<!<|>)=)`;
    export const MoreOrEqualPrefixRegex = `(少なくとも)`;
    export const LessOrEqualPrefixRegex = `(多くて)`;
    export const MoreOrEqual = `((${MoreRegex}(か)${EqualRegex})|小さくない|以上|最低)`;
    export const MoreOrEqualSuffix = `(より(大なりイコール|小さくない))`;
    export const LessOrEqual = `((${LessRegex}\\s*(或|或者)?\\s*${EqualRegex})|(${LessRegex}(か)${EqualRegex})|大さくない|以下|最大)`;
    export const LessOrEqualSuffix = `(小なりイコール|大さくない)`;
    export const OneNumberRangeMoreRegex1 = `(?<number1>(((?!((,(?!\\d+))|。|は)).)+))\\s*((より)\\s*((${MoreOrEqual}|${MoreRegex})))|(?<number1>((?!((,(?!\\d+))|。|は)).)+)\\s*(${MoreRegex})`;
    export const OneNumberRangeMoreRegex3 = `(?<number1>((?!((,(?!\\d+))|。)).)+)\\s*(以上|最低)(?![万億]{1,2})`;
    export const OneNumberRangeMoreRegex4 = `(${MoreOrEqualPrefixRegex})\\s*(?<number1>((?!(と|は|((と)?同時に)|((と)?そして)|が|,|(,(?!\\d+))|。)).)*)`;
    export const OneNumberRangeMoreRegex5 = `(?<number1>((?!((,(?!\\d+))|。)).)+)\\s*((もしくはそれ)(以上)(?![万億]{1,2}))`;
    export const OneNumberRangeMoreSeparateRegex = `^[.]`;
    export const OneNumberRangeLessSeparateRegex = `^[.]`;
    export const OneNumberRangeLessRegex1 = `(?<number2>(((?!(((,)(?!\\d+))|。|(\\D)))|(?:[-]|(分の))).)+)\\s*(より)\\s*(${LessOrEqual}|${LessRegex})|(?<number2>((?!((,(?!\\d+))|。)).)+)\\s*(小な)`;
    export const OneNumberRangeLessRegex3 = `(?<number2>(((?!((,(?!\\d+))|。)).)+))\\s*(以下|未満)(の間)?(?![万億]{1,2})`;
    export const OneNumberRangeLessRegex4 = `(${LessOrEqual}|${LessRegex}|${LessOrEqualPrefixRegex})\\s*(?<number2>((?!(と|は|((と)?同時に)|((と)?そして)|が|の|,|(,(?!\\d+))|。)).)+)`;
    export const OneNumberRangeEqualRegex = `(((?<number1>((?!((,(?!\\d+))|。)).)+)\\s*(に)\\s*${EqualRegex})|(${EqualRegex}\\s*(?<number1>((?!((,(?!\\d+))|。)).)+)))`;
    export const TwoNumberRangeMoreSuffix = `(${MoreOrEqualPrefixRegex}\\s*(?<number1>((?!(と|は|((と)?同時に)|((と)?そして)|が|,|(,(?!\\d+))|。)).)*))(,${LessOrEqualPrefixRegex})`;
    export const TwoNumberRangeRegex1 = `(?<number1>((?!((,(?!\\d+))|。)).)+)\\s*(と|${TillRegex})\\s*(?<number2>((?!((,(?!\\d+))|。)).)+)\\s*(の間|未満)`;
    export const TwoNumberRangeRegex2 = `(${OneNumberRangeMoreRegex1}|${OneNumberRangeMoreRegex3}|${OneNumberRangeMoreRegex4})\\s*(と|((と)?同時に)|((と)?そして)|が|,)?\\s*(${OneNumberRangeLessRegex1}|${OneNumberRangeLessRegex3}|${OneNumberRangeLessRegex4})`;
    export const TwoNumberRangeRegex3 = `(${OneNumberRangeLessRegex1}|${OneNumberRangeLessRegex3}|${OneNumberRangeLessRegex4})\\s*(と|((と)?同時に)|((と)?そして)|が|,)?\\s*(${OneNumberRangeMoreRegex1}|${OneNumberRangeMoreRegex3}|${OneNumberRangeMoreRegex4})`;
    export const TwoNumberRangeRegex4 = `(?<number1>((?!((,(?!\\d+))|。)).)+)\\s*${TillRegex}\\s*(?<number2>((?!((,(?!\\d+))|。)).)+)`;
    export const AmbiguousFractionConnectorsRegex = `^[.]`;
    export const RelativeReferenceOffsetMap: ReadonlyMap<string, string> = new Map<string, string>([["前", ""],["現在", ""],["次", ""],["最後", ""],["最後から三番目", ""],["最後から二番目", ""],["最後から一つ前", ""],["最後から一つ前のもの", ""],["最後から一つ前のこと", ""],["最後から1つ前のこと", ""],["最後から1つ前のもの", ""],["最後から1つ前", ""],["現在のこと", ""],["前のもの", ""],["次のもの", ""],["最後から3番目", ""],["最後から2番目", ""]]);
    export const RelativeReferenceRelativeToMap: ReadonlyMap<string, string> = new Map<string, string>([["前", "current"],["現在", "current"],["次", "current"],["最後", "end"],["最後から三番目", "end"],["最後から二番目", "end"],["最後から一つ前", "end"],["最後から一つ前のもの", "end"],["最後から一つ前のこと", "end"],["現在のこと", "current"],["最後から1つ前のこと", "end"],["最後から1つ前のもの", "end"],["最後から1つ前", "end"],["前のもの", "current"],["次のもの", "current"],["最後から3番目", "end"],["最後から2番目", "end"]]);
}
