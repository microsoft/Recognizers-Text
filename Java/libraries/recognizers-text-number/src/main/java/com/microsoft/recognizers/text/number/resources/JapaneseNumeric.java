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

package com.microsoft.recognizers.text.number.resources;

import java.util.Arrays;
import java.util.List;
import java.util.Map;

import com.google.common.collect.ImmutableMap;

public class JapaneseNumeric {

    public static final String LangMarker = "Jpn";

    public static final Boolean CompoundNumberLanguage = true;

    public static final Boolean MultiDecimalSeparatorCulture = false;

    public static final Character DecimalSeparatorChar = '.';

    public static final String FractionMarkerToken = "";

    public static final Character NonDecimalSeparatorChar = ' ';

    public static final String HalfADozenText = "";

    public static final String WordSeparatorToken = "";

    public static final Character ZeroChar = '零';

    public static final Character PairChar = '対';

    public static final ImmutableMap<String, Long> RoundNumberMap = ImmutableMap.<String, Long>builder()
        .put("k", 1000L)
        .put("m", 1000000L)
        .put("g", 1000000000L)
        .put("t", 1000000000000L)
        .put("b", 1000000000L)
        .build();

    public static final ImmutableMap<Character, Long> RoundNumberMapChar = ImmutableMap.<Character, Long>builder()
        .put('十', 10L)
        .put('百', 100L)
        .put('千', 1000L)
        .put('万', 10000L)
        .put('億', 100000000L)
        .put('兆', 1000000000000L)
        .build();

    public static final ImmutableMap<Character, Double> ZeroToNineMap = ImmutableMap.<Character, Double>builder()
        .put('零', 0D)
        .put('〇', 0D)
        .put('一', 1D)
        .put('二', 2D)
        .put('三', 3D)
        .put('四', 4D)
        .put('五', 5D)
        .put('六', 6D)
        .put('七', 7D)
        .put('八', 8D)
        .put('九', 9D)
        .put('0', 0D)
        .put('1', 1D)
        .put('2', 2D)
        .put('3', 3D)
        .put('4', 4D)
        .put('5', 5D)
        .put('6', 6D)
        .put('7', 7D)
        .put('8', 8D)
        .put('9', 9D)
        .put('半', 0.5D)
        .build();

    public static final ImmutableMap<Character, Character> FullToHalfMap = ImmutableMap.<Character, Character>builder()
        .put('０', '0')
        .put('１', '1')
        .put('２', '2')
        .put('３', '3')
        .put('４', '4')
        .put('５', '5')
        .put('６', '6')
        .put('７', '7')
        .put('８', '8')
        .put('９', '9')
        .put('／', '/')
        .put('－', '-')
        .put('，', '\'')
        .put('、', '\'')
        .put('Ｇ', 'G')
        .put('Ｍ', 'M')
        .put('Ｔ', 'T')
        .put('Ｋ', 'K')
        .put('ｋ', 'k')
        .put('．', '.')
        .build();

    public static final ImmutableMap<String, String> UnitMap = ImmutableMap.<String, String>builder()
        .put("万万", "億")
        .put("億万", "兆")
        .put("万億", "兆")
        .put(" ", "")
        .build();

    public static final List<Character> RoundDirectList = Arrays.asList('万', '億', '兆');

    public static final List<Character> TenChars = Arrays.asList('十');

    public static final String AllMultiplierLookupRegex = "({BaseNumbers.MultiplierLookupRegex}|ミリリットル(入れら)?|キロメートル|メートル|ミリメート)"
            .replace("{BaseNumbers.MultiplierLookupRegex}", BaseNumbers.MultiplierLookupRegex);

    public static final String DigitalNumberRegex = "((?<=(\\d|\\b)){BaseNumbers.MultiplierLookupRegex}(?=\\b))"
            .replace("{BaseNumbers.MultiplierLookupRegex}", BaseNumbers.MultiplierLookupRegex);

    public static final String ZeroToNineFullHalfRegex = "[\\d]";

    public static final String DigitNumRegex = "{ZeroToNineFullHalfRegex}+"
            .replace("{ZeroToNineFullHalfRegex}", ZeroToNineFullHalfRegex);

    public static final String DozenRegex = ".*ダース$";

    public static final String PercentageRegex = ".+(?=パ\\s*ー\\s*セ\\s*ン\\s*ト)|.*(?=[％%])";

    public static final String DoubleAndRoundRegex = "{ZeroToNineFullHalfRegex}+(\\.{ZeroToNineFullHalfRegex}+)?\\s*[万億]{1,2}(\\s*(以上))?"
            .replace("{ZeroToNineFullHalfRegex}", ZeroToNineFullHalfRegex);

    public static final String FracSplitRegex = "[はと]|分\\s*の";

    public static final String ZeroToNineIntegerRegex = "[〇一二三四五六七八九]";

    public static final String NegativeNumberTermsRegex = "(マ\\s*イ\\s*ナ\\s*ス)";

    public static final String NegativeNumberTermsRegexNum = "((?<!(\\d+(\\s*{BaseNumbers.NumberMultiplierRegex})?\\s*)|[-−－])[-−－])"
            .replace("{BaseNumbers.NumberMultiplierRegex}", BaseNumbers.NumberMultiplierRegex);

    public static final String NegativeNumberSignRegex = "^{NegativeNumberTermsRegex}.*|^{NegativeNumberTermsRegexNum}.*"
            .replace("{NegativeNumberTermsRegex}", NegativeNumberTermsRegex)
            .replace("{NegativeNumberTermsRegexNum}", NegativeNumberTermsRegexNum);

    public static final String SpeGetNumberRegex = "{ZeroToNineFullHalfRegex}|{ZeroToNineIntegerRegex}|[半対]|[分厘]"
            .replace("{ZeroToNineFullHalfRegex}", ZeroToNineFullHalfRegex)
            .replace("{ZeroToNineIntegerRegex}", ZeroToNineIntegerRegex);

    public static final String PairRegex = ".*[対膳足]$";

    public static final String RoundNumberIntegerRegex = "(十|百|千|万(?!万)|億|兆)";

    public static final String AllowListRegex = "(。|，|、|（|）|”｜国|週間|時間|時|匹|キロ|トン|年|個|足|本|で|は|\\s|$|つ|月|の|と)";

    public static final String NotSingleRegex = "(?<!(第|だい))(({RoundNumberIntegerRegex}+(({ZeroToNineIntegerRegex}+|{RoundNumberIntegerRegex})+|{ZeroToNineFullHalfRegex}+|十)\\s*(以上)?))|(({ZeroToNineIntegerRegex}+|{ZeroToNineFullHalfRegex}+|十)\\s*({RoundNumberIntegerRegex}\\s*){1,2})\\s*(([零]?({ZeroToNineIntegerRegex}+|{ZeroToNineFullHalfRegex}+|十)\\s*{RoundNumberIntegerRegex}{0,1})\\s*)*\\s*(\\s*(以上)?)"
            .replace("{ZeroToNineIntegerRegex}", ZeroToNineIntegerRegex)
            .replace("{ZeroToNineFullHalfRegex}", ZeroToNineFullHalfRegex)
            .replace("{RoundNumberIntegerRegex}", RoundNumberIntegerRegex);

    public static final String SingleRegex = "(({ZeroToNineIntegerRegex}+|{ZeroToNineFullHalfRegex}+|十)(?={AllowListRegex}))"
            .replace("{ZeroToNineIntegerRegex}", ZeroToNineIntegerRegex)
            .replace("{ZeroToNineFullHalfRegex}", ZeroToNineFullHalfRegex)
            .replace("{AllowListRegex}", AllowListRegex);

    public static final String AllIntRegex = "(?<!(ダース))((((({ZeroToNineIntegerRegex}|[十百千])\\s*{RoundNumberIntegerRegex}*)|(({ZeroToNineFullHalfRegex}\\s*{RoundNumberIntegerRegex})|{RoundNumberIntegerRegex})){1,2}|({RoundNumberIntegerRegex}+))(\\s*[以上]+)?)"
            .replace("{ZeroToNineIntegerRegex}", ZeroToNineIntegerRegex)
            .replace("{ZeroToNineFullHalfRegex}", ZeroToNineFullHalfRegex)
            .replace("{RoundNumberIntegerRegex}", RoundNumberIntegerRegex);

    public static final String PlaceHolderPureNumber = "\\b";

    public static final String PlaceHolderDefault = "\\D|\\b";

    public static final String NumbersSpecialsChars = "((({NegativeNumberTermsRegexNum}|{NegativeNumberTermsRegex})\\s*)?({ZeroToNineFullHalfRegex}))+(?=\\b|\\D)(?!(([\\.．]{ZeroToNineFullHalfRegex}+)?\\s*{AllMultiplierLookupRegex}))"
            .replace("{NegativeNumberTermsRegexNum}", NegativeNumberTermsRegexNum)
            .replace("{NegativeNumberTermsRegex}", NegativeNumberTermsRegex)
            .replace("{ZeroToNineFullHalfRegex}", ZeroToNineFullHalfRegex)
            .replace("{AllMultiplierLookupRegex}", AllMultiplierLookupRegex);

    public static final String NumbersSpecialsCharsWithSuffix = "({NegativeNumberTermsRegexNum}?{ZeroToNineFullHalfRegex}+(\\s*{BaseNumbers.NumberMultiplierRegex}+)?)(?=\\b|\\D)(?!(([\\.．]{ZeroToNineFullHalfRegex}+)?\\s*{AllMultiplierLookupRegex}))"
            .replace("{NegativeNumberTermsRegexNum}", NegativeNumberTermsRegexNum)
            .replace("{ZeroToNineFullHalfRegex}", ZeroToNineFullHalfRegex)
            .replace("{BaseNumbers.NumberMultiplierRegex}", BaseNumbers.NumberMultiplierRegex)
            .replace("{AllMultiplierLookupRegex}", AllMultiplierLookupRegex);

    public static final String DottedNumbersSpecialsChar = "{NegativeNumberTermsRegexNum}?{ZeroToNineFullHalfRegex}{1,3}([,，、]{ZeroToNineFullHalfRegex}{3})+"
            .replace("{NegativeNumberTermsRegexNum}", NegativeNumberTermsRegexNum)
            .replace("{ZeroToNineFullHalfRegex}", ZeroToNineFullHalfRegex);

    public static final String NumbersWithHalfDozen = "半({RoundNumberIntegerRegex}|(ダース))"
            .replace("{RoundNumberIntegerRegex}", RoundNumberIntegerRegex);

    public static final String NumbersWithDozen = "({AllIntRegex}([と]?{ZeroToNineIntegerRegex})?|{ZeroToNineFullHalfRegex}+)(ダース)"
            .replace("{AllIntRegex}", AllIntRegex)
            .replace("{ZeroToNineIntegerRegex}", ZeroToNineIntegerRegex)
            .replace("{ZeroToNineFullHalfRegex}", ZeroToNineFullHalfRegex);

    public static final String PointRegexStr = "[\\.．・]";

    public static final String AllFloatRegex = "{NegativeNumberTermsRegex}?{AllIntRegex}\\s*{PointRegexStr}\\s*[一二三四五六七八九](\\s*{ZeroToNineIntegerRegex})*"
            .replace("{NegativeNumberTermsRegex}", NegativeNumberTermsRegex)
            .replace("{AllIntRegex}", AllIntRegex)
            .replace("{PointRegexStr}", PointRegexStr)
            .replace("{ZeroToNineIntegerRegex}", ZeroToNineIntegerRegex);

    public static final String NumbersWithAllowListRegex = "(?<!(離は))({NegativeNumberTermsRegex}?({NotSingleRegex}|{SingleRegex})(?!({AllIntRegex}*([、.]{ZeroToNineIntegerRegex}+)*|{AllFloatRegex})*\\s*{PercentageRegex}+))(?!(\\s*{AllMultiplierLookupRegex}))"
            .replace("{NegativeNumberTermsRegex}", NegativeNumberTermsRegex)
            .replace("{NotSingleRegex}", NotSingleRegex)
            .replace("{SingleRegex}", SingleRegex)
            .replace("{AllIntRegex}", AllIntRegex)
            .replace("{ZeroToNineIntegerRegex}", ZeroToNineIntegerRegex)
            .replace("{AllFloatRegex}", AllFloatRegex)
            .replace("{PercentageRegex}", PercentageRegex)
            .replace("{AllMultiplierLookupRegex}", AllMultiplierLookupRegex);

    public static final String NumbersAggressiveRegex = "(({AllIntRegex})(?!({AllIntRegex}|([、.]{ZeroToNineIntegerRegex})|{AllFloatRegex}|\\s*{PercentageRegex})))"
            .replace("{AllIntRegex}", AllIntRegex)
            .replace("{AllFloatRegex}", AllFloatRegex)
            .replace("{NegativeNumberTermsRegex}", NegativeNumberTermsRegex)
            .replace("{ZeroToNineIntegerRegex}", ZeroToNineIntegerRegex)
            .replace("{PercentageRegex}", PercentageRegex);

    public static final String PointRegex = "{PointRegexStr}"
            .replace("{PointRegexStr}", PointRegexStr);

    public static final String DoubleSpecialsChars = "((?<!({ZeroToNineFullHalfRegex}+[\\.．]{ZeroToNineFullHalfRegex}*))({NegativeNumberTermsRegexNum}\\s*)?{ZeroToNineFullHalfRegex}+[\\.．,]{ZeroToNineFullHalfRegex}+(?!({ZeroToNineFullHalfRegex}*[\\.．,]{ZeroToNineFullHalfRegex}+)))(?=\\b|\\D)(?!\\s*{AllMultiplierLookupRegex})"
            .replace("{ZeroToNineFullHalfRegex}", ZeroToNineFullHalfRegex)
            .replace("{NegativeNumberTermsRegexNum}", NegativeNumberTermsRegexNum)
            .replace("{AllMultiplierLookupRegex}", AllMultiplierLookupRegex);

    public static final String DoubleRoundNumberSpecialsChars = "(?<!(({ZeroToNineIntegerRegex}|{RoundNumberIntegerRegex})+[\\.．・,]({ZeroToNineIntegerRegex}|{RoundNumberIntegerRegex})*))(({NegativeNumberTermsRegexNum}|{NegativeNumberTermsRegex})\\s*)?({ZeroToNineIntegerRegex}|{RoundNumberIntegerRegex})+[\\.．・,]({ZeroToNineIntegerRegex}|{RoundNumberIntegerRegex})+(?!({ZeroToNineIntegerRegex}|{RoundNumberIntegerRegex})*[\\.．・,]({ZeroToNineIntegerRegex}|{RoundNumberIntegerRegex})+)"
            .replace("{NegativeNumberTermsRegexNum}", NegativeNumberTermsRegexNum)
            .replace("{NegativeNumberTermsRegex}", NegativeNumberTermsRegex)
            .replace("{ZeroToNineIntegerRegex}", ZeroToNineIntegerRegex)
            .replace("{RoundNumberIntegerRegex}", RoundNumberIntegerRegex);

    public static final String DoubleSpecialsCharsWithNegatives = "(?<!({ZeroToNineFullHalfRegex}+|\\.\\.|．．))({NegativeNumberTermsRegexNum}\\s*)?[\\.．]{ZeroToNineFullHalfRegex}+(?!{ZeroToNineFullHalfRegex}*([\\.．]{ZeroToNineFullHalfRegex}+))"
            .replace("{ZeroToNineFullHalfRegex}", ZeroToNineFullHalfRegex)
            .replace("{NegativeNumberTermsRegexNum}", NegativeNumberTermsRegexNum);

    public static final String SimpleDoubleSpecialsChars = "({NegativeNumberTermsRegexNum}\\s*)?{ZeroToNineFullHalfRegex}{1,3}([,，]{ZeroToNineFullHalfRegex}{3})+[\\.．]{ZeroToNineFullHalfRegex}+"
            .replace("{NegativeNumberTermsRegexNum}", NegativeNumberTermsRegexNum)
            .replace("{ZeroToNineFullHalfRegex}", ZeroToNineFullHalfRegex);

    public static final String DoubleWithMultiplierRegex = "(({NegativeNumberTermsRegexNum}\\s*)?{ZeroToNineFullHalfRegex}+[\\.．]{ZeroToNineFullHalfRegex}+\\s*{BaseNumbers.NumberMultiplierRegex})(?=\\b|\\D)(?!{AllMultiplierLookupRegex})"
            .replace("{NegativeNumberTermsRegexNum}", NegativeNumberTermsRegexNum)
            .replace("{ZeroToNineFullHalfRegex}", ZeroToNineFullHalfRegex)
            .replace("{BaseNumbers.NumberMultiplierRegex}", BaseNumbers.NumberMultiplierRegex)
            .replace("{AllMultiplierLookupRegex}", AllMultiplierLookupRegex);

    public static final String DoubleWithThousandsRegex = "({NegativeNumberTermsRegex}{0,1}|{NegativeNumberTermsRegexNum})\\s*({ZeroToNineFullHalfRegex}+([\\.．]{ZeroToNineFullHalfRegex}+)?\\s*[万亿萬億]{1,2})"
            .replace("{NegativeNumberTermsRegex}", NegativeNumberTermsRegex)
            .replace("{NegativeNumberTermsRegexNum}", NegativeNumberTermsRegexNum)
            .replace("{ZeroToNineFullHalfRegex}", ZeroToNineFullHalfRegex);

    public static final String DoubleAllFloatRegex = "(?<!(({AllIntRegex}[.]*)|{AllFloatRegex})*){AllFloatRegex}(?!{ZeroToNineIntegerRegex}*\\s*パーセント)"
            .replace("{AllIntRegex}", AllIntRegex)
            .replace("{AllFloatRegex}", AllFloatRegex)
            .replace("{ZeroToNineIntegerRegex}", ZeroToNineIntegerRegex);

    public static final String DoubleExponentialNotationRegex = "(?<!\\d+[\\.])({NegativeNumberTermsRegexNum}\\s*)?(\\d+([\\.]\\d+)?)e(([-+＋]*[1-9]\\d*)|[0](?!\\d+))|(?<!\\d+[\\.])({NegativeNumberTermsRegexNum}\\s*)?(\\d+([\\.]\\d+)?)(×)?(10)?の((([-+＋]*[1-9]\\d*)|[0])[乗](?!\\d+))"
            .replace("{NegativeNumberTermsRegexNum}", NegativeNumberTermsRegexNum);

    public static final String DoubleExponentialNotationKanjiRegex = "(?<!({ZeroToNineIntegerRegex})+[\\.．・])(({NegativeNumberTermsRegexNum}|{NegativeNumberTermsRegex})\\s*)?({ZeroToNineIntegerRegex}|[十千五百])+([\\.．・]({ZeroToNineIntegerRegex})+)?(×)?(十)?(の)((((マ\\s*イ\\s*ナ\\s*ス))*({ZeroToNineIntegerRegex}|[十])({ZeroToNineIntegerRegex}|[十])*[乗])(?!({ZeroToNineIntegerRegex}|[十])+))"
            .replace("{ZeroToNineIntegerRegex}", ZeroToNineIntegerRegex)
            .replace("{NegativeNumberTermsRegexNum}", NegativeNumberTermsRegexNum)
            .replace("{NegativeNumberTermsRegex}", NegativeNumberTermsRegex);

    public static final String DoubleScientificNotationRegex = "(?<!\\d+[\\.])({NegativeNumberTermsRegexNum}\\s*)?(\\d+([\\.]\\d+)?)\\^([-+＋]*[1-9]\\d*)"
            .replace("{NegativeNumberTermsRegexNum}", NegativeNumberTermsRegexNum);

    public static final String OrdinalNumbersRegex = "(((第|だい)({ZeroToNineFullHalfRegex}+)({RoundNumberIntegerRegex}+)?))|(({ZeroToNineFullHalfRegex}+|{ZeroToNineIntegerRegex}+)({RoundNumberIntegerRegex}+)?(番目|位|等(?!級)))"
            .replace("{ZeroToNineFullHalfRegex}", ZeroToNineFullHalfRegex)
            .replace("{ZeroToNineIntegerRegex}", ZeroToNineIntegerRegex)
            .replace("{RoundNumberIntegerRegex}", RoundNumberIntegerRegex);

    public static final String OrdinalRegex = "(({OrdinalNumbersRegex})|((第|だい)({AllIntRegex})|(({AllIntRegex}+|{NumbersWithAllowListRegex}+)(番目|位|等))))|(最初|1等|ファースト)"
            .replace("{NumbersWithAllowListRegex}", NumbersWithAllowListRegex)
            .replace("{AllIntRegex}", AllIntRegex)
            .replace("{OrdinalNumbersRegex}", OrdinalNumbersRegex)
            .replace("{RoundNumberIntegerRegex}", RoundNumberIntegerRegex);

    public static final String RelativeOrdinalRegex = "(?<relativeOrdinal>((最後)(から1つ前のこと|から(3|2|1)番目|(から1つ前)(のもの)|から三番目|から二番目|(から(一|1)つ前)(のもの|のこと)?|(から1つ)?(前))?|(次のもの)(前)?|(前(?=の))(のもの)?|(現在)(のこと)?|次|二位))";

    public static final String AllOrdinalRegex = "({OrdinalRegex}|{RelativeOrdinalRegex})"
            .replace("{OrdinalRegex}", OrdinalRegex)
            .replace("{RelativeOrdinalRegex}", RelativeOrdinalRegex);

    public static final String AllFractionNumber = "((({NegativeNumberTermsRegex}{0,1})|{NegativeNumberTermsRegexNum})(({ZeroToNineFullHalfRegex}+|{AllIntRegex})\\s*[はと]{0,1}\\s*)?{NegativeNumberTermsRegex}{0,1}({ZeroToNineFullHalfRegex}+|{AllIntRegex})\\s*分\\s*の\\s*{NegativeNumberTermsRegex}{0,1}({ZeroToNineFullHalfRegex}+|{AllIntRegex})+)|半(分|数)"
            .replace("{NegativeNumberTermsRegex}", NegativeNumberTermsRegex)
            .replace("{ZeroToNineFullHalfRegex}", ZeroToNineFullHalfRegex)
            .replace("{NegativeNumberTermsRegexNum}", NegativeNumberTermsRegexNum)
            .replace("{AllIntRegex}", AllIntRegex);

    public static final String FractionNotationSpecialsCharsRegex = "({NegativeNumberTermsRegexNum}\\s*)?{ZeroToNineFullHalfRegex}+\\s+{ZeroToNineFullHalfRegex}+[/／]{ZeroToNineFullHalfRegex}+"
            .replace("{NegativeNumberTermsRegexNum}", NegativeNumberTermsRegexNum)
            .replace("{ZeroToNineFullHalfRegex}", ZeroToNineFullHalfRegex);

    public static final String FractionNotationRegex = "({NegativeNumberTermsRegexNum}\\s*)?{ZeroToNineFullHalfRegex}+[/／]{ZeroToNineFullHalfRegex}+"
            .replace("{NegativeNumberTermsRegexNum}", NegativeNumberTermsRegexNum)
            .replace("{ZeroToNineFullHalfRegex}", ZeroToNineFullHalfRegex);

    public static final String PercentagePointRegex = "(?<!{AllIntRegex})({AllFloatRegex}|{AllIntRegex})\\s*(パ\\s*ー\\s*セ\\s*ン\\s*ト|[%])"
            .replace("{AllIntRegex}", AllIntRegex)
            .replace("{AllFloatRegex}", AllFloatRegex);

    public static final String SimplePercentageRegex = "({AllFloatRegex}|(({NegativeNumberTermsRegex})?({AllIntRegex}|[百])))\\s*(パ\\s*ー\\s*セ\\s*ン\\s*ト|[%])"
            .replace("{NegativeNumberTermsRegex}", NegativeNumberTermsRegex)
            .replace("{ZeroToNineIntegerRegex}", ZeroToNineIntegerRegex)
            .replace("{AllFloatRegex}", AllFloatRegex)
            .replace("{AllIntRegex}", AllIntRegex);

    public static final String NumbersPercentagePointRegex = "(?<!%)(({NegativeNumberTermsRegexNum}|{NegativeNumberTermsRegex})?({ZeroToNineFullHalfRegex})+([\\.．]({ZeroToNineFullHalfRegex})+)?\\s*(パ\\s*ー\\s*セ\\s*ン\\s*ト|((?<!(%\\d+))%))(?!{ZeroToNineFullHalfRegex}))"
            .replace("{NegativeNumberTermsRegexNum}", NegativeNumberTermsRegexNum)
            .replace("{NegativeNumberTermsRegex}", NegativeNumberTermsRegex)
            .replace("{ZeroToNineFullHalfRegex}", ZeroToNineFullHalfRegex);

    public static final String NumbersPercentageWithSeparatorRegex = "({ZeroToNineFullHalfRegex}{1,3}[,，、]{ZeroToNineFullHalfRegex}{3})+([\\.．]{ZeroToNineFullHalfRegex}+)*\\s*(パ\\s*ー\\s*セ\\s*ン\\s*ト|[%])"
            .replace("{ZeroToNineIntegerRegex}", ZeroToNineIntegerRegex)
            .replace("{ZeroToNineFullHalfRegex}", ZeroToNineFullHalfRegex);

    public static final String NumbersPercentageWithMultiplierRegex = "(?<!{ZeroToNineIntegerRegex}){ZeroToNineFullHalfRegex}+[\\.．]{ZeroToNineFullHalfRegex}+\\s*{BaseNumbers.NumberMultiplierRegex}\\s*(パ\\s*ー\\s*セ\\s*ン\\s*ト|[%])"
            .replace("{ZeroToNineIntegerRegex}", ZeroToNineIntegerRegex)
            .replace("{ZeroToNineFullHalfRegex}", ZeroToNineFullHalfRegex)
            .replace("{BaseNumbers.NumberMultiplierRegex}", BaseNumbers.NumberMultiplierRegex);

    public static final String FractionPercentagePointRegex = "(?<!({ZeroToNineFullHalfRegex}+[\\.．])){ZeroToNineFullHalfRegex}+[\\.．]{ZeroToNineFullHalfRegex}+(?!([\\.．]{ZeroToNineFullHalfRegex}+))\\s*(パ\\s*ー\\s*セ\\s*ン\\s*ト|[%])"
            .replace("{ZeroToNineFullHalfRegex}", ZeroToNineFullHalfRegex);

    public static final String FractionPercentageWithSeparatorRegex = "{ZeroToNineFullHalfRegex}{1,3}([,，、]{ZeroToNineFullHalfRegex}{3})+[\\.．]{ZeroToNineFullHalfRegex}+\\s*(パ\\s*ー\\s*セ\\s*ン\\s*ト|[%])"
            .replace("{ZeroToNineFullHalfRegex}", ZeroToNineFullHalfRegex);

    public static final String FractionPercentageWithMultiplierRegex = "{ZeroToNineFullHalfRegex}+[\\.．]{ZeroToNineFullHalfRegex}+\\s*{BaseNumbers.NumberMultiplierRegex}\\s*(パ\\s*ー\\s*セ\\s*ン\\s*ト|[%])"
            .replace("{ZeroToNineFullHalfRegex}", ZeroToNineFullHalfRegex)
            .replace("{BaseNumbers.NumberMultiplierRegex}", BaseNumbers.NumberMultiplierRegex);

    public static final String SimpleNumbersPercentageRegex = "(?<!{ZeroToNineIntegerRegex}){ZeroToNineFullHalfRegex}+\\s*(パ\\s*ー\\s*セ\\s*ン\\s*ト|[%])(?!([\\.．]{ZeroToNineFullHalfRegex}+))"
            .replace("{ZeroToNineIntegerRegex}", ZeroToNineIntegerRegex)
            .replace("{ZeroToNineFullHalfRegex}", ZeroToNineFullHalfRegex);

    public static final String SimpleNumbersPercentageWithMultiplierRegex = "(?<!{ZeroToNineIntegerRegex}){ZeroToNineFullHalfRegex}+\\s*{BaseNumbers.NumberMultiplierRegex}\\s*(パ\\s*ー\\s*セ\\s*ン\\s*ト|[%])"
            .replace("{ZeroToNineIntegerRegex}", ZeroToNineIntegerRegex)
            .replace("{ZeroToNineFullHalfRegex}", ZeroToNineFullHalfRegex)
            .replace("{BaseNumbers.NumberMultiplierRegex}", BaseNumbers.NumberMultiplierRegex);

    public static final String SimpleNumbersPercentagePointRegex = "(?!{ZeroToNineIntegerRegex}){ZeroToNineFullHalfRegex}{1,3}([,，]{ZeroToNineFullHalfRegex}{3})+\\s*(パ\\s*ー\\s*セ\\s*ン\\s*ト|[%])"
            .replace("{ZeroToNineIntegerRegex}", ZeroToNineIntegerRegex)
            .replace("{ZeroToNineFullHalfRegex}", ZeroToNineFullHalfRegex);

    public static final String IntegerPercentageRegex = "{ZeroToNineFullHalfRegex}+\\s*(パ\\s*ー\\s*セ\\s*ン\\s*ト|[%])"
            .replace("{ZeroToNineFullHalfRegex}", ZeroToNineFullHalfRegex);

    public static final String IntegerPercentageWithMultiplierRegex = "{ZeroToNineFullHalfRegex}+\\s*{BaseNumbers.NumberMultiplierRegex}\\s*(パ\\s*ー\\s*セ\\s*ン\\s*ト|[%])"
            .replace("{ZeroToNineFullHalfRegex}", ZeroToNineFullHalfRegex)
            .replace("{BaseNumbers.NumberMultiplierRegex}", BaseNumbers.NumberMultiplierRegex);

    public static final String NumbersFractionPercentageRegex = "{ZeroToNineFullHalfRegex}{1,3}([,，]{ZeroToNineFullHalfRegex}{3})+\\s*(パ\\s*ー\\s*セ\\s*ン\\s*ト|[%])"
            .replace("{ZeroToNineFullHalfRegex}", ZeroToNineFullHalfRegex);

    public static final String SimpleIntegerPercentageRegex = "(?<!%)(({NegativeNumberTermsRegexNum}|{NegativeNumberTermsRegex})?{ZeroToNineFullHalfRegex}+([\\.．]{ZeroToNineFullHalfRegex}+)?(\\s*)((?<!(%\\d+))%)(?!{ZeroToNineFullHalfRegex}))"
            .replace("{NegativeNumberTermsRegexNum}", NegativeNumberTermsRegexNum)
            .replace("{NegativeNumberTermsRegex}", NegativeNumberTermsRegex)
            .replace("{ZeroToNineFullHalfRegex}", ZeroToNineFullHalfRegex);

    public static final String NumbersFoldsPercentageRegex = "{ZeroToNineFullHalfRegex}(([\\.．]?|\\s*){ZeroToNineFullHalfRegex})?\\s*[の]*\\s*割引"
            .replace("{ZeroToNineFullHalfRegex}", ZeroToNineFullHalfRegex);

    public static final String FoldsPercentageRegex = "{ZeroToNineIntegerRegex}(\\s*[.]?\\s*{ZeroToNineIntegerRegex})?\\s*[の]\\s*割引"
            .replace("{ZeroToNineIntegerRegex}", ZeroToNineIntegerRegex);

    public static final String SimpleFoldsPercentageRegex = "{ZeroToNineFullHalfRegex}\\s*割(\\s*(半|({ZeroToNineFullHalfRegex}\\s*分\\s*{ZeroToNineFullHalfRegex}\\s*厘)|{ZeroToNineFullHalfRegex}))?"
            .replace("{ZeroToNineFullHalfRegex}", ZeroToNineFullHalfRegex);

    public static final String SpecialsPercentageRegex = "({ZeroToNineIntegerRegex}|[十])\\s*割(\\s*(半|{ZeroToNineIntegerRegex}))?"
            .replace("{ZeroToNineIntegerRegex}", ZeroToNineIntegerRegex);

    public static final String NumbersSpecialsPercentageRegex = "({ZeroToNineFullHalfRegex}[\\.．]{ZeroToNineFullHalfRegex}|10)\\s*割"
            .replace("{ZeroToNineFullHalfRegex}", ZeroToNineFullHalfRegex);

    public static final String SimpleSpecialsPercentageRegex = "{ZeroToNineIntegerRegex}\\s*[.]\\s*{ZeroToNineIntegerRegex}\\s*割"
            .replace("{ZeroToNineIntegerRegex}", ZeroToNineIntegerRegex);

    public static final String SpecialsFoldsPercentageRegex = "半\\s*分|(?<=ダース)";

    public static final String TillRegex = "(から|--|-|—|——|~)";

    public static final String MoreRegex = "(大なり|を超える|大きい|高い|大きく|(?<!<|=)>)";

    public static final String LessRegex = "(小なり|小さい|低い|(?<!>|=)<)";

    public static final String EqualRegex = "(等しい|イコール|(?<!<|>)=)";

    public static final String MoreOrEqualPrefixRegex = "(少なくとも)";

    public static final String LessOrEqualPrefixRegex = "(多くて)";

    public static final String MoreOrEqual = "(({MoreRegex}(か){EqualRegex})|小さくない|以上|最低)"
            .replace("{MoreRegex}", MoreRegex)
            .replace("{EqualRegex}", EqualRegex);

    public static final String MoreOrEqualSuffix = "(より(大なりイコール|小さくない))";

    public static final String LessOrEqual = "(({LessRegex}\\s*(或|或者)?\\s*{EqualRegex})|({LessRegex}(か){EqualRegex})|大さくない|以下|最大)"
            .replace("{LessRegex}", LessRegex)
            .replace("{EqualRegex}", EqualRegex);

    public static final String LessOrEqualSuffix = "(小なりイコール|大さくない)";

    public static final String OneNumberRangeMoreRegex1 = "(?<number1>(((?!((,(?!\\d+))|。|は)).)+))\\s*((より)\\s*(({MoreOrEqual}|{MoreRegex})))|(?<number1>((?!((,(?!\\d+))|。|は)).)+)\\s*({MoreRegex})"
            .replace("{MoreOrEqual}", MoreOrEqual)
            .replace("{MoreRegex}", MoreRegex);

    public static final String OneNumberRangeMoreRegex3 = "(?<number1>((?!((,(?!\\d+))|。)).)+)\\s*(以上|最低)(?![万億]{1,2})";

    public static final String OneNumberRangeMoreRegex4 = "({MoreOrEqualPrefixRegex})\\s*(?<number1>((?!(と|は|((と)?同時に)|((と)?そして)|が|,|(,(?!\\d+))|。)).)*)"
            .replace("{MoreOrEqual}", MoreOrEqual)
            .replace("{MoreRegex}", MoreRegex)
            .replace("{MoreOrEqualPrefixRegex}", MoreOrEqualPrefixRegex);

    public static final String OneNumberRangeMoreRegex5 = "(?<number1>((?!((,(?!\\d+))|。)).)+)\\s*((もしくはそれ)(以上)(?![万億]{1,2}))";

    public static final String OneNumberRangeMoreSeparateRegex = "^[.]";

    public static final String OneNumberRangeLessSeparateRegex = "^[.]";

    public static final String OneNumberRangeLessRegex1 = "(?<number2>(((?!(((,)(?!\\d+))|。|(\\D)))|(?:[-]|(分の))).)+)\\s*(より)\\s*({LessOrEqual}|{LessRegex})|(?<number2>((?!((,(?!\\d+))|。)).)+)\\s*(小な)"
            .replace("{LessOrEqual}", LessOrEqual)
            .replace("{LessRegex}", LessRegex);

    public static final String OneNumberRangeLessRegex3 = "(?<number2>(((?!((,(?!\\d+))|。)).)+))\\s*(以下|未満)(の間)?(?![万億]{1,2})";

    public static final String OneNumberRangeLessRegex4 = "({LessOrEqual}|{LessRegex}|{LessOrEqualPrefixRegex})\\s*(?<number2>((?!(と|は|((と)?同時に)|((と)?そして)|が|の|,|(,(?!\\d+))|。)).)+)"
            .replace("{LessOrEqual}", LessOrEqual)
            .replace("{LessRegex}", LessRegex)
            .replace("{LessOrEqualPrefixRegex}", LessOrEqualPrefixRegex);

    public static final String OneNumberRangeEqualRegex = "(((?<number1>((?!((,(?!\\d+))|。)).)+)\\s*(に)\\s*{EqualRegex})|({EqualRegex}\\s*(?<number1>((?!((,(?!\\d+))|。)).)+)))"
            .replace("{EqualRegex}", EqualRegex);

    public static final String TwoNumberRangeMoreSuffix = "({MoreOrEqualPrefixRegex}\\s*(?<number1>((?!(と|は|((と)?同時に)|((と)?そして)|が|,|(,(?!\\d+))|。)).)*))(,{LessOrEqualPrefixRegex})"
            .replace("{MoreOrEqualPrefixRegex}", MoreOrEqualPrefixRegex)
            .replace("{LessOrEqualPrefixRegex}", LessOrEqualPrefixRegex);

    public static final String TwoNumberRangeRegex1 = "(?<number1>((?!((,(?!\\d+))|。)).)+)\\s*(と|{TillRegex})\\s*(?<number2>((?!((,(?!\\d+))|。)).)+)\\s*(の間|未満)"
            .replace("{TillRegex}", TillRegex);

    public static final String TwoNumberRangeRegex2 = "({OneNumberRangeMoreRegex1}|{OneNumberRangeMoreRegex3}|{OneNumberRangeMoreRegex4})\\s*(と|((と)?同時に)|((と)?そして)|が|,)?\\s*({OneNumberRangeLessRegex1}|{OneNumberRangeLessRegex3}|{OneNumberRangeLessRegex4})"
            .replace("{OneNumberRangeMoreRegex1}", OneNumberRangeMoreRegex1)
            .replace("{OneNumberRangeMoreRegex3}", OneNumberRangeMoreRegex3)
            .replace("{OneNumberRangeMoreRegex4}", OneNumberRangeMoreRegex4)
            .replace("{OneNumberRangeLessRegex1}", OneNumberRangeLessRegex1)
            .replace("{OneNumberRangeLessRegex3}", OneNumberRangeLessRegex3)
            .replace("{OneNumberRangeLessRegex4}", OneNumberRangeLessRegex4);

    public static final String TwoNumberRangeRegex3 = "({OneNumberRangeLessRegex1}|{OneNumberRangeLessRegex3}|{OneNumberRangeLessRegex4})\\s*(と|((と)?同時に)|((と)?そして)|が|,)?\\s*({OneNumberRangeMoreRegex1}|{OneNumberRangeMoreRegex3}|{OneNumberRangeMoreRegex4})"
            .replace("{OneNumberRangeMoreRegex1}", OneNumberRangeMoreRegex1)
            .replace("{OneNumberRangeMoreRegex3}", OneNumberRangeMoreRegex3)
            .replace("{OneNumberRangeMoreRegex4}", OneNumberRangeMoreRegex4)
            .replace("{OneNumberRangeLessRegex1}", OneNumberRangeLessRegex1)
            .replace("{OneNumberRangeLessRegex3}", OneNumberRangeLessRegex3)
            .replace("{OneNumberRangeLessRegex4}", OneNumberRangeLessRegex4);

    public static final String TwoNumberRangeRegex4 = "(?<number1>((?!((,(?!\\d+))|。)).)+)\\s*{TillRegex}\\s*(?<number2>((?!((,(?!\\d+))|。)).)+)"
            .replace("{TillRegex}", TillRegex);

    public static final String AmbiguousFractionConnectorsRegex = "^[.]";

    public static final ImmutableMap<String, String> RelativeReferenceOffsetMap = ImmutableMap.<String, String>builder()
        .put("前", "-1")
        .put("現在", "0")
        .put("次", "1")
        .put("最後", "0")
        .put("最後から三番目", "-2")
        .put("最後から二番目", "-1")
        .put("最後から一つ前", "-1")
        .put("最後から一つ前のもの", "-1")
        .put("最後から一つ前のこと", "-1")
        .put("最後から1つ前のこと", "-1")
        .put("最後から1つ前のもの", "-1")
        .put("最後から1つ前", "-1")
        .put("現在のこと", "0")
        .put("前のもの", "-1")
        .put("次のもの", "1")
        .put("最後から3番目", "-2")
        .put("最後から2番目", "-1")
        .build();

    public static final ImmutableMap<String, String> RelativeReferenceRelativeToMap = ImmutableMap.<String, String>builder()
        .put("前", "current")
        .put("現在", "end")
        .put("次", "current")
        .put("最後", "end")
        .put("最後から三番目", "end")
        .put("最後から二番目", "end")
        .put("最後から一つ前", "end")
        .put("最後から一つ前のもの", "end")
        .put("最後から一つ前のこと", "end")
        .put("現在のこと", "current")
        .put("最後から1つ前のこと", "end")
        .put("最後から1つ前のもの", "end")
        .put("最後から1つ前", "end")
        .put("前のもの", "current")
        .put("次のもの", "current")
        .put("最後から3番目", "end")
        .put("最後から2番目", "end")
        .build();
}
