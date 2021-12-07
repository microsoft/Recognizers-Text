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

public class EnglishNumeric {

    public static final String LangMarker = "Eng";

    public static final Boolean CompoundNumberLanguage = false;

    public static final Boolean MultiDecimalSeparatorCulture = true;

    public static final List<String> NonStandardSeparatorVariants = Arrays.asList("en-za", "en-na", "en-zw");

    public static final String RoundNumberIntegerRegex = "(?:hundred|thousand|million|mln|billion|bln|trillion|tln|lakh|crore)s?";

    public static final String ZeroToNineIntegerRegex = "(?:three|seven|eight|four|five|zero|nine|one|two|six)";

    public static final String TwoToNineIntegerRegex = "(?:three|seven|eight|four|five|nine|two|six)";

    public static final String NegativeNumberTermsRegex = "(?<negTerm>(minus|negative)\\s+)";

    public static final String NegativeNumberSignRegex = "^{NegativeNumberTermsRegex}.*"
            .replace("{NegativeNumberTermsRegex}", NegativeNumberTermsRegex);

    public static final String AnIntRegex = "(an?)(?=\\s)";

    public static final String TenToNineteenIntegerRegex = "(?:seventeen|thirteen|fourteen|eighteen|nineteen|fifteen|sixteen|eleven|twelve|ten)";

    public static final String TensNumberIntegerRegex = "(?:seventy|twenty|thirty|eighty|ninety|forty|fifty|sixty)";

    public static final String SeparaIntRegex = "(?:(({TenToNineteenIntegerRegex}|({TensNumberIntegerRegex}(\\s+(and\\s+)?|\\s*-\\s*){ZeroToNineIntegerRegex})|{TensNumberIntegerRegex}|{ZeroToNineIntegerRegex})(\\s+{RoundNumberIntegerRegex})*))|(({AnIntRegex}(\\s+{RoundNumberIntegerRegex})+))"
            .replace("{TenToNineteenIntegerRegex}", TenToNineteenIntegerRegex)
            .replace("{TensNumberIntegerRegex}", TensNumberIntegerRegex)
            .replace("{ZeroToNineIntegerRegex}", ZeroToNineIntegerRegex)
            .replace("{RoundNumberIntegerRegex}", RoundNumberIntegerRegex)
            .replace("{AnIntRegex}", AnIntRegex);

    public static final String AllIntRegex = "(?:((({TenToNineteenIntegerRegex}|({TensNumberIntegerRegex}(\\s+(and\\s+)?|\\s*-\\s*){ZeroToNineIntegerRegex})|{TensNumberIntegerRegex}|{ZeroToNineIntegerRegex}|{AnIntRegex})(\\s+{RoundNumberIntegerRegex})+)\\s+(and\\s+)?)*{SeparaIntRegex})"
            .replace("{TenToNineteenIntegerRegex}", TenToNineteenIntegerRegex)
            .replace("{TensNumberIntegerRegex}", TensNumberIntegerRegex)
            .replace("{ZeroToNineIntegerRegex}", ZeroToNineIntegerRegex)
            .replace("{AnIntRegex}", AnIntRegex)
            .replace("{RoundNumberIntegerRegex}", RoundNumberIntegerRegex)
            .replace("{SeparaIntRegex}", SeparaIntRegex);

    public static final String PlaceHolderPureNumber = "\\b";

    public static final String PlaceHolderDefault = "(?=\\D)|\\b";

    public static final String PlaceHolderMixed = "\\D|\\b";

    public static String NumbersWithPlaceHolder(String placeholder) {
        return "(((?<!(\\d+(\\s*(K|k|MM?|mil|G|T|B|b))?\\s*|\\p{L}))-\\s*)|(?<={placeholder}))\\d+(?!([\\.,]\\d+[a-zA-Z]))(?={placeholder})"
            .replace("{placeholder}", placeholder);
    }

    public static final String IndianNumberingSystemRegex = "(?<=\\b)((?:\\d{1,2},(?:\\d{2},)*\\d{3})(?=\\b))";

    public static final String NumbersWithSuffix = "(((?<!\\d+(\\s*{BaseNumbers.NumberMultiplierRegex})?\\s*)-\\s*)|(?<=\\b))\\d+\\s*{BaseNumbers.NumberMultiplierRegex}(?=\\b)"
            .replace("{BaseNumbers.NumberMultiplierRegex}", BaseNumbers.NumberMultiplierRegex);

    public static final String RoundNumberIntegerRegexWithLocks = "(?<=\\b)\\d+\\s+{RoundNumberIntegerRegex}(?=\\b)"
            .replace("{RoundNumberIntegerRegex}", RoundNumberIntegerRegex);

    public static final String NumbersWithDozenSuffix = "(((?<!\\d+(\\s*{BaseNumbers.NumberMultiplierRegex})?\\s*)-\\s*)|(?<=\\b))\\d+\\s+dozen(s)?(?=\\b)"
            .replace("{BaseNumbers.NumberMultiplierRegex}", BaseNumbers.NumberMultiplierRegex);

    public static final String AllIntRegexWithLocks = "((?<=\\b){AllIntRegex}(?=\\b))"
            .replace("{AllIntRegex}", AllIntRegex);

    public static final String AllIntRegexWithDozenSuffixLocks = "(?<=\\b)(((half\\s+)?a\\s+dozen)|({AllIntRegex}\\s+dozen(s)?))(?=\\b)"
            .replace("{AllIntRegex}", AllIntRegex);

    public static final String RoundNumberOrdinalRegex = "(?:hundredth|thousandth|millionth|billionth|trillionth)";

    public static final String NumberOrdinalRegex = "(?:first|second|third|fourth|fifth|sixth|seventh|eighth|nine?th|tenth|eleventh|twelfth|thirteenth|fourteenth|fifteenth|sixteenth|seventeenth|eighteenth|nineteenth|twentieth|thirtieth|fortieth|fiftieth|sixtieth|seventieth|eightieth|ninetieth)";

    public static final String RelativeOrdinalRegex = "(?<relativeOrdinal>(next|previous|current)\\s+one|(the\\s+second|next)\\s+to\\s+last|the\\s+one\\s+before\\s+the\\s+last(\\s+one)?|the\\s+last\\s+but\\s+one|(ante)?penultimate|last|next|previous|current)";

    public static final String BasicOrdinalRegex = "({NumberOrdinalRegex}|{RelativeOrdinalRegex})"
            .replace("{NumberOrdinalRegex}", NumberOrdinalRegex)
            .replace("{RelativeOrdinalRegex}", RelativeOrdinalRegex);

    public static final String SuffixBasicOrdinalRegex = "(?:(((({TensNumberIntegerRegex}(\\s+(and\\s+)?|\\s*-\\s*){ZeroToNineIntegerRegex})|{TensNumberIntegerRegex}|{ZeroToNineIntegerRegex}|{AnIntRegex})(\\s+{RoundNumberIntegerRegex})+)\\s+(and\\s+)?)*({TensNumberIntegerRegex}(\\s+|\\s*-\\s*))?{BasicOrdinalRegex})"
            .replace("{TensNumberIntegerRegex}", TensNumberIntegerRegex)
            .replace("{ZeroToNineIntegerRegex}", ZeroToNineIntegerRegex)
            .replace("{AnIntRegex}", AnIntRegex)
            .replace("{RoundNumberIntegerRegex}", RoundNumberIntegerRegex)
            .replace("{BasicOrdinalRegex}", BasicOrdinalRegex);

    public static final String SuffixRoundNumberOrdinalRegex = "(?:({AllIntRegex}\\s+){RoundNumberOrdinalRegex})"
            .replace("{AllIntRegex}", AllIntRegex)
            .replace("{RoundNumberOrdinalRegex}", RoundNumberOrdinalRegex);

    public static final String AllOrdinalRegex = "(?:{SuffixBasicOrdinalRegex}|{SuffixRoundNumberOrdinalRegex})"
            .replace("{SuffixBasicOrdinalRegex}", SuffixBasicOrdinalRegex)
            .replace("{SuffixRoundNumberOrdinalRegex}", SuffixRoundNumberOrdinalRegex);

    public static final String OrdinalSuffixRegex = "(?<=\\b)(?:(\\d*(1st|2nd|3rd|[4-90]th))|(1[1-2]th))(?=\\b)";

    public static final String OrdinalNumericRegex = "(?<=\\b)(?:\\d{1,3}(\\s*,\\s*\\d{3})*\\s*th)(?=\\b)";

    public static final String OrdinalRoundNumberRegex = "(?<!an?\\s+){RoundNumberOrdinalRegex}"
            .replace("{RoundNumberOrdinalRegex}", RoundNumberOrdinalRegex);

    public static final String OrdinalEnglishRegex = "(?<=\\b){AllOrdinalRegex}(?=\\b)"
            .replace("{AllOrdinalRegex}", AllOrdinalRegex);

    public static final String FractionNotationWithSpacesRegex = "(((?<=\\W|^)-\\s*)|(?<=\\b))\\d+\\s+\\d+[/]\\d+(?=(\\b[^/]|$))";

    public static final String FractionNotationRegex = "{BaseNumbers.FractionNotationRegex}"
            .replace("{BaseNumbers.FractionNotationRegex}", BaseNumbers.FractionNotationRegex);

    public static final String RoundMultiplierRegex = "\\b\\s*((of\\s+)?a\\s+)?(?<multiplier>{RoundNumberIntegerRegex})$"
            .replace("{RoundNumberIntegerRegex}", RoundNumberIntegerRegex);

    public static final String FractionNounRegex = "(?<=\\b)({AllIntRegex}\\s+(and\\s+)?)?(({AllIntRegex})(\\s+|\\s*-\\s*)((({AllOrdinalRegex})|({RoundNumberOrdinalRegex}))s|halves|quarters)((\\s+of\\s+a)?\\s+{RoundNumberIntegerRegex})?|(half(\\s+a)?|quarter(\\s+of\\s+a)?)\\s+{RoundNumberIntegerRegex})(?=\\b)"
            .replace("{AllIntRegex}", AllIntRegex)
            .replace("{AllOrdinalRegex}", AllOrdinalRegex)
            .replace("{RoundNumberOrdinalRegex}", RoundNumberOrdinalRegex)
            .replace("{RoundNumberIntegerRegex}", RoundNumberIntegerRegex);

    public static final String FractionNounWithArticleRegex = "(?<=\\b)((({AllIntRegex}\\s+(and\\s+)?)?(an?|one)(\\s+|\\s*-\\s*)(?!\\bfirst\\b|\\bsecond\\b)(({AllOrdinalRegex})|({RoundNumberOrdinalRegex})|(half|quarter)(((\\s+of)?\\s+a)?\\s+{RoundNumberIntegerRegex})?))|(half))(?=\\b)"
            .replace("{AllIntRegex}", AllIntRegex)
            .replace("{AllOrdinalRegex}", AllOrdinalRegex)
            .replace("{RoundNumberOrdinalRegex}", RoundNumberOrdinalRegex)
            .replace("{RoundNumberIntegerRegex}", RoundNumberIntegerRegex);

    public static final String FractionPrepositionRegex = "(?<!{BaseNumbers.CommonCurrencySymbol}\\s*)(?<=\\b)(?<numerator>({AllIntRegex})|((?<![\\.,])\\d+))\\s+(over|(?<ambiguousSeparator>in|out\\s+of))\\s+(?<denominator>({AllIntRegex})|(\\d+)(?![\\.,]))(?=\\b)"
            .replace("{AllIntRegex}", AllIntRegex)
            .replace("{BaseNumbers.CommonCurrencySymbol}", BaseNumbers.CommonCurrencySymbol);

    public static final String FractionPrepositionWithinPercentModeRegex = "(?<!{BaseNumbers.CommonCurrencySymbol}\\s*)(?<=\\b)(?<numerator>({AllIntRegex})|((?<![\\.,])\\d+))\\s+over\\s+(?<denominator>({AllIntRegex})|(\\d+)(?![\\.,]))(?=\\b)"
            .replace("{AllIntRegex}", AllIntRegex)
            .replace("{BaseNumbers.CommonCurrencySymbol}", BaseNumbers.CommonCurrencySymbol);

    public static final String AllPointRegex = "((\\s+{ZeroToNineIntegerRegex})+|(\\s+{SeparaIntRegex}))"
            .replace("{ZeroToNineIntegerRegex}", ZeroToNineIntegerRegex)
            .replace("{SeparaIntRegex}", SeparaIntRegex);

    public static final String AllFloatRegex = "{AllIntRegex}(\\s+point){AllPointRegex}"
            .replace("{AllIntRegex}", AllIntRegex)
            .replace("{AllPointRegex}", AllPointRegex);

    public static final String DoubleWithMultiplierRegex = "(((?<!\\d+(\\s*{BaseNumbers.NumberMultiplierRegex})?\\s*)-\\s*)|((?<=\\b)(?<!\\d+[\\.,])))(\\d{1,3}(,\\d{3})+(\\.\\d+)?|\\d+[\\.,]\\d+)\\s*{BaseNumbers.NumberMultiplierRegex}(?=\\b)"
            .replace("{BaseNumbers.NumberMultiplierRegex}", BaseNumbers.NumberMultiplierRegex);

    public static final String DoubleExponentialNotationRegex = "(((?<!\\d+(\\s*{BaseNumbers.NumberMultiplierRegex})?\\s*)-\\s*)|((?<=\\b)(?<!\\d+[\\.,])))(\\d+([\\.,]\\d+)?)(e|x10\\^)([+-]*[1-9]\\d*)(?=\\b)"
            .replace("{BaseNumbers.NumberMultiplierRegex}", BaseNumbers.NumberMultiplierRegex);

    public static final String DoubleCaretExponentialNotationRegex = "(((?<!\\d+(\\s*{BaseNumbers.NumberMultiplierRegex})?\\s*)-\\s*)|((?<=\\b)(?<!\\d+[\\.,])))(\\d+([\\.,]\\d+)?)\\^([+-]*[1-9]\\d*)(?=\\b)"
            .replace("{BaseNumbers.NumberMultiplierRegex}", BaseNumbers.NumberMultiplierRegex);

    public static String DoubleDecimalPointRegex(String placeholder) {
        return "(((?<!(\\d+(\\s*(K|k|MM?|mil|G|T|B|b))?\\s*|\\p{L}))-\\s*)|((?<={placeholder})(?<!\\d+[\\.,])))(\\d{1,3}(,\\d{3})+(\\.\\d+)?|\\d+[\\.,]\\d+)(?!([\\.,]\\d+))(?={placeholder})"
            .replace("{placeholder}", placeholder);
    }

    public static final String DoubleIndianDecimalPointRegex = "(?<=\\b)((?:\\d{1,2},(?:\\d{2},)*\\d{3})(?:\\.\\d{2})(?=\\b))";

    public static String DoubleWithoutIntegralRegex(String placeholder) {
        return "(?<=\\s|^)(?<!(\\d+))[\\.,]\\d+(?!([\\.,]\\d+))(?={placeholder})"
            .replace("{placeholder}", placeholder);
    }

    public static final String DoubleWithRoundNumber = "(((?<!\\d+(\\s*{BaseNumbers.NumberMultiplierRegex})?\\s*)-\\s*)|((?<=\\b)(?<!\\d+[\\.,])))(\\d{1,3}(,\\d{3})+(\\.\\d+)?|\\d+[\\.,]\\d+)\\s+{RoundNumberIntegerRegex}(?=\\b)"
            .replace("{RoundNumberIntegerRegex}", RoundNumberIntegerRegex)
            .replace("{BaseNumbers.NumberMultiplierRegex}", BaseNumbers.NumberMultiplierRegex);

    public static final String DoubleAllFloatRegex = "((?<=\\b){AllFloatRegex}(?=\\b))"
            .replace("{AllFloatRegex}", AllFloatRegex);

    public static final String ConnectorRegex = "(?<spacer>and)";

    public static final String NumberWithSuffixPercentage = "(?<!%({BaseNumbers.NumberReplaceToken})?)({BaseNumbers.NumberReplaceToken}(\\s*))?(%(?!{BaseNumbers.NumberReplaceToken})|(per\\s*cents?|percentage|cents?)\\b)"
            .replace("{BaseNumbers.NumberReplaceToken}", BaseNumbers.NumberReplaceToken);

    public static final String FractionNumberWithSuffixPercentage = "(({BaseNumbers.FractionNumberReplaceToken})\\s+of)"
            .replace("{BaseNumbers.FractionNumberReplaceToken}", BaseNumbers.FractionNumberReplaceToken);

    public static final String NumberWithPrefixPercentage = "(per\\s*cents?\\s+of)(\\s*)({BaseNumbers.NumberReplaceToken})"
            .replace("{BaseNumbers.NumberReplaceToken}", BaseNumbers.NumberReplaceToken);

    public static final String NumberWithPrepositionPercentage = "({BaseNumbers.NumberReplaceToken})\\s*(in|out\\s+of)\\s*({BaseNumbers.NumberReplaceToken})"
            .replace("{BaseNumbers.NumberReplaceToken}", BaseNumbers.NumberReplaceToken);

    public static final String TillRegex = "((?<!\\bequal\\s+)to|through|--|-|—|——|~|–)";

    public static final String MoreRegex = "(?:(bigger|greater|more|higher|larger)(\\s+than)?|above|over|beyond|exceed(ed|ing)?|surpass(ed|ing)?|(?<!<|=)>)";

    public static final String LessRegex = "(?:(less|lower|smaller|fewer)(\\s+than)?|below|under|(?<!>|=)<)";

    public static final String EqualRegex = "(equal(s|ing)?(\\s+(to|than))?|(?<!<|>)=)";

    public static final String MoreOrEqualPrefix = "((no\\s+{LessRegex})|(at\\s+least))"
            .replace("{LessRegex}", LessRegex);

    public static final String MoreOrEqual = "(?:({MoreRegex}\\s+(or)?\\s+{EqualRegex})|({EqualRegex}\\s+(or)?\\s+{MoreRegex})|{MoreOrEqualPrefix}(\\s+(or)?\\s+{EqualRegex})?|({EqualRegex}\\s+(or)?\\s+)?{MoreOrEqualPrefix}|>\\s*=|≥)"
            .replace("{MoreRegex}", MoreRegex)
            .replace("{EqualRegex}", EqualRegex)
            .replace("{LessRegex}", LessRegex)
            .replace("{MoreOrEqualPrefix}", MoreOrEqualPrefix);

    public static final String MoreOrEqualSuffix = "((and|or)\\s+(((more|greater|higher|larger|bigger)((?!\\s+than)|(\\s+than(?!((\\s+or\\s+equal\\s+to)?\\s*\\d+)))))|((over|above)(?!\\s+than))))";

    public static final String LessOrEqualPrefix = "((no\\s+{MoreRegex})|(at\\s+most)|(up\\s+to))"
            .replace("{MoreRegex}", MoreRegex);

    public static final String LessOrEqual = "(({LessRegex}\\s+(or)?\\s+{EqualRegex})|({EqualRegex}\\s+(or)?\\s+{LessRegex})|{LessOrEqualPrefix}(\\s+(or)?\\s+{EqualRegex})?|({EqualRegex}\\s+(or)?\\s+)?{LessOrEqualPrefix}|<\\s*=|≤)"
            .replace("{LessRegex}", LessRegex)
            .replace("{EqualRegex}", EqualRegex)
            .replace("{MoreRegex}", MoreRegex)
            .replace("{LessOrEqualPrefix}", LessOrEqualPrefix);

    public static final String LessOrEqualSuffix = "((and|or)\\s+(less|lower|smaller|fewer)((?!\\s+than)|(\\s+than(?!(\\s*\\d+)))))";

    public static final String NumberSplitMark = "(?![,.](?!\\d+))(?!\\s*\\b(and\\s+({LessRegex}|{MoreRegex})|but|or|to)\\b)"
            .replace("{MoreRegex}", MoreRegex)
            .replace("{LessRegex}", LessRegex);

    public static final String MoreRegexNoNumberSucceed = "((bigger|greater|more|higher|larger)((?!\\s+than)|\\s+(than(?!(\\s*\\d+))))|(above|over)(?!(\\s*\\d+)))";

    public static final String LessRegexNoNumberSucceed = "((less|lower|smaller|fewer)((?!\\s+than)|\\s+(than(?!(\\s*\\d+))))|(below|under)(?!(\\s*\\d+)))";

    public static final String EqualRegexNoNumberSucceed = "(equal(s|ing)?((?!\\s+(to|than))|(\\s+(to|than)(?!(\\s*\\d+)))))";

    public static final String OneNumberRangeMoreRegex1 = "({MoreOrEqual}|{MoreRegex})\\s*(the\\s+)?(?<number1>({NumberSplitMark}.)+)"
            .replace("{MoreOrEqual}", MoreOrEqual)
            .replace("{MoreRegex}", MoreRegex)
            .replace("{NumberSplitMark}", NumberSplitMark);

    public static final String OneNumberRangeMoreRegex1LB = "(?<!no\\s+){OneNumberRangeMoreRegex1}"
            .replace("{OneNumberRangeMoreRegex1}", OneNumberRangeMoreRegex1);

    public static final String OneNumberRangeMoreRegex2 = "(?<number1>({NumberSplitMark}.)+)\\s*{MoreOrEqualSuffix}"
            .replace("{MoreOrEqualSuffix}", MoreOrEqualSuffix)
            .replace("{NumberSplitMark}", NumberSplitMark);

    public static final String OneNumberRangeMoreSeparateRegex = "({EqualRegex}\\s+(?<number1>({NumberSplitMark}.)+)(\\s+or\\s+){MoreRegexNoNumberSucceed})|({MoreRegex}\\s+(?<number1>({NumberSplitMark}.)+)(\\s+or\\s+){EqualRegexNoNumberSucceed})"
            .replace("{EqualRegex}", EqualRegex)
            .replace("{MoreRegex}", MoreRegex)
            .replace("{EqualRegexNoNumberSucceed}", EqualRegexNoNumberSucceed)
            .replace("{MoreRegexNoNumberSucceed}", MoreRegexNoNumberSucceed)
            .replace("{NumberSplitMark}", NumberSplitMark);

    public static final String OneNumberRangeLessRegex1 = "({LessOrEqual}|{LessRegex})\\s*(the\\s+)?(?<number2>({NumberSplitMark}.)+)"
            .replace("{LessOrEqual}", LessOrEqual)
            .replace("{LessRegex}", LessRegex)
            .replace("{NumberSplitMark}", NumberSplitMark);

    public static final String OneNumberRangeLessRegex1LB = "(?<!no\\s+){OneNumberRangeLessRegex1}"
            .replace("{OneNumberRangeLessRegex1}", OneNumberRangeLessRegex1);

    public static final String OneNumberRangeLessRegex2 = "(?<number2>({NumberSplitMark}.)+)\\s*{LessOrEqualSuffix}"
            .replace("{LessOrEqualSuffix}", LessOrEqualSuffix)
            .replace("{NumberSplitMark}", NumberSplitMark);

    public static final String OneNumberRangeLessSeparateRegex = "({EqualRegex}\\s+(?<number1>({NumberSplitMark}.)+)(\\s+or\\s+){LessRegexNoNumberSucceed})|({LessRegex}\\s+(?<number1>({NumberSplitMark}.)+)(\\s+or\\s+){EqualRegexNoNumberSucceed})"
            .replace("{EqualRegex}", EqualRegex)
            .replace("{LessRegex}", LessRegex)
            .replace("{EqualRegexNoNumberSucceed}", EqualRegexNoNumberSucceed)
            .replace("{LessRegexNoNumberSucceed}", LessRegexNoNumberSucceed)
            .replace("{NumberSplitMark}", NumberSplitMark);

    public static final String OneNumberRangeEqualRegex = "(?<!\\bthan\\s+or\\s+){EqualRegex}\\s*(the\\s+)?(?<number1>({NumberSplitMark}.)+)"
            .replace("{EqualRegex}", EqualRegex)
            .replace("{NumberSplitMark}", NumberSplitMark);

    public static final String TwoNumberRangeRegex1 = "between\\s*(the\\s+)?(?<number1>({NumberSplitMark}.)+)\\s*and\\s*(the\\s+)?(?<number2>({NumberSplitMark}.)+)"
            .replace("{NumberSplitMark}", NumberSplitMark);

    public static final String TwoNumberRangeRegex2 = "({OneNumberRangeMoreRegex1}|{OneNumberRangeMoreRegex2})\\s*(and|but|,)\\s*({OneNumberRangeLessRegex1}|{OneNumberRangeLessRegex2})"
            .replace("{OneNumberRangeMoreRegex1}", OneNumberRangeMoreRegex1)
            .replace("{OneNumberRangeMoreRegex2}", OneNumberRangeMoreRegex2)
            .replace("{OneNumberRangeLessRegex1}", OneNumberRangeLessRegex1)
            .replace("{OneNumberRangeLessRegex2}", OneNumberRangeLessRegex2);

    public static final String TwoNumberRangeRegex3 = "({OneNumberRangeLessRegex1}|{OneNumberRangeLessRegex2})\\s*(and|but|,)\\s*({OneNumberRangeMoreRegex1}|{OneNumberRangeMoreRegex2})"
            .replace("{OneNumberRangeMoreRegex1}", OneNumberRangeMoreRegex1)
            .replace("{OneNumberRangeMoreRegex2}", OneNumberRangeMoreRegex2)
            .replace("{OneNumberRangeLessRegex1}", OneNumberRangeLessRegex1)
            .replace("{OneNumberRangeLessRegex2}", OneNumberRangeLessRegex2);

    public static final String TwoNumberRangeRegex4 = "(from\\s+)?(?<number1>({NumberSplitMark}(?!\\bfrom\\b).)+)\\s*{TillRegex}\\s*(the\\s+)?(?<number2>({NumberSplitMark}.)+)"
            .replace("{TillRegex}", TillRegex)
            .replace("{NumberSplitMark}", NumberSplitMark);

    public static final String AmbiguousFractionConnectorsRegex = "(\\bin\\b)";

    public static final Character DecimalSeparatorChar = '.';

    public static final String FractionMarkerToken = "over";

    public static final Character NonDecimalSeparatorChar = ',';

    public static final String HalfADozenText = "six";

    public static final String WordSeparatorToken = "and";

    public static final List<String> WrittenDecimalSeparatorTexts = Arrays.asList("point");

    public static final List<String> WrittenGroupSeparatorTexts = Arrays.asList("punto");

    public static final List<String> WrittenIntegerSeparatorTexts = Arrays.asList("and");

    public static final List<String> WrittenFractionSeparatorTexts = Arrays.asList("and");

    public static final String HalfADozenRegex = "half\\s+a\\s+dozen";

    public static final String DigitalNumberRegex = "((?<=\\b)(hundred|thousand|[mb]illion|trillion|[mbt]ln|lakh|crore|dozen(s)?)(?=\\b))|((?<=(\\d|\\b)){BaseNumbers.MultiplierLookupRegex}(?=\\b))"
            .replace("{BaseNumbers.MultiplierLookupRegex}", BaseNumbers.MultiplierLookupRegex);

    public static final ImmutableMap<String, Long> CardinalNumberMap = ImmutableMap.<String, Long>builder()
        .put("a", 1L)
        .put("zero", 0L)
        .put("an", 1L)
        .put("one", 1L)
        .put("two", 2L)
        .put("three", 3L)
        .put("four", 4L)
        .put("five", 5L)
        .put("six", 6L)
        .put("seven", 7L)
        .put("eight", 8L)
        .put("nine", 9L)
        .put("ten", 10L)
        .put("eleven", 11L)
        .put("twelve", 12L)
        .put("dozen", 12L)
        .put("dozens", 12L)
        .put("thirteen", 13L)
        .put("fourteen", 14L)
        .put("fifteen", 15L)
        .put("sixteen", 16L)
        .put("seventeen", 17L)
        .put("eighteen", 18L)
        .put("nineteen", 19L)
        .put("twenty", 20L)
        .put("thirty", 30L)
        .put("forty", 40L)
        .put("fifty", 50L)
        .put("sixty", 60L)
        .put("seventy", 70L)
        .put("eighty", 80L)
        .put("ninety", 90L)
        .put("hundred", 100L)
        .put("thousand", 1000L)
        .put("million", 1000000L)
        .put("mln", 1000000L)
        .put("billion", 1000000000L)
        .put("bln", 1000000000L)
        .put("trillion", 1000000000000L)
        .put("tln", 1000000000000L)
        .put("lakh", 100000L)
        .put("crore", 10000000L)
        .build();

    public static final ImmutableMap<String, Long> OrdinalNumberMap = ImmutableMap.<String, Long>builder()
        .put("first", 1L)
        .put("second", 2L)
        .put("secondary", 2L)
        .put("half", 2L)
        .put("third", 3L)
        .put("fourth", 4L)
        .put("quarter", 4L)
        .put("fifth", 5L)
        .put("sixth", 6L)
        .put("seventh", 7L)
        .put("eighth", 8L)
        .put("ninth", 9L)
        .put("nineth", 9L)
        .put("tenth", 10L)
        .put("eleventh", 11L)
        .put("twelfth", 12L)
        .put("thirteenth", 13L)
        .put("fourteenth", 14L)
        .put("fifteenth", 15L)
        .put("sixteenth", 16L)
        .put("seventeenth", 17L)
        .put("eighteenth", 18L)
        .put("nineteenth", 19L)
        .put("twentieth", 20L)
        .put("thirtieth", 30L)
        .put("fortieth", 40L)
        .put("fiftieth", 50L)
        .put("sixtieth", 60L)
        .put("seventieth", 70L)
        .put("eightieth", 80L)
        .put("ninetieth", 90L)
        .put("hundredth", 100L)
        .put("thousandth", 1000L)
        .put("millionth", 1000000L)
        .put("billionth", 1000000000L)
        .put("trillionth", 1000000000000L)
        .put("firsts", 1L)
        .put("halves", 2L)
        .put("thirds", 3L)
        .put("fourths", 4L)
        .put("quarters", 4L)
        .put("fifths", 5L)
        .put("sixths", 6L)
        .put("sevenths", 7L)
        .put("eighths", 8L)
        .put("ninths", 9L)
        .put("nineths", 9L)
        .put("tenths", 10L)
        .put("elevenths", 11L)
        .put("twelfths", 12L)
        .put("thirteenths", 13L)
        .put("fourteenths", 14L)
        .put("fifteenths", 15L)
        .put("sixteenths", 16L)
        .put("seventeenths", 17L)
        .put("eighteenths", 18L)
        .put("nineteenths", 19L)
        .put("twentieths", 20L)
        .put("thirtieths", 30L)
        .put("fortieths", 40L)
        .put("fiftieths", 50L)
        .put("sixtieths", 60L)
        .put("seventieths", 70L)
        .put("eightieths", 80L)
        .put("ninetieths", 90L)
        .put("hundredths", 100L)
        .put("thousandths", 1000L)
        .put("millionths", 1000000L)
        .put("billionths", 1000000000L)
        .put("trillionths", 1000000000000L)
        .build();

    public static final ImmutableMap<String, Long> RoundNumberMap = ImmutableMap.<String, Long>builder()
        .put("hundred", 100L)
        .put("thousand", 1000L)
        .put("million", 1000000L)
        .put("mln", 1000000L)
        .put("billion", 1000000000L)
        .put("bln", 1000000000L)
        .put("trillion", 1000000000000L)
        .put("tln", 1000000000000L)
        .put("lakh", 100000L)
        .put("crore", 10000000L)
        .put("hundredth", 100L)
        .put("thousandth", 1000L)
        .put("millionth", 1000000L)
        .put("billionth", 1000000000L)
        .put("trillionth", 1000000000000L)
        .put("hundredths", 100L)
        .put("thousandths", 1000L)
        .put("millionths", 1000000L)
        .put("billionths", 1000000000L)
        .put("trillionths", 1000000000000L)
        .put("dozen", 12L)
        .put("dozens", 12L)
        .put("k", 1000L)
        .put("m", 1000000L)
        .put("mm", 1000000L)
        .put("mil", 1000000L)
        .put("g", 1000000000L)
        .put("b", 1000000000L)
        .put("t", 1000000000000L)
        .build();

    public static final ImmutableMap<String, String> AmbiguityFiltersDict = ImmutableMap.<String, String>builder()
        .put("\\bone\\b", "\\b(the|this|that|which)\\s+(one)\\b")
        .build();

    public static final ImmutableMap<String, String> RelativeReferenceOffsetMap = ImmutableMap.<String, String>builder()
        .put("last", "0")
        .put("next one", "1")
        .put("current", "0")
        .put("current one", "0")
        .put("previous one", "-1")
        .put("the second to last", "-1")
        .put("the one before the last one", "-1")
        .put("the one before the last", "-1")
        .put("next to last", "-1")
        .put("penultimate", "-1")
        .put("the last but one", "-1")
        .put("antepenultimate", "-2")
        .put("next", "1")
        .put("previous", "-1")
        .build();

    public static final ImmutableMap<String, String> RelativeReferenceRelativeToMap = ImmutableMap.<String, String>builder()
        .put("last", "end")
        .put("next one", "current")
        .put("previous one", "current")
        .put("current", "current")
        .put("current one", "current")
        .put("the second to last", "end")
        .put("the one before the last one", "end")
        .put("the one before the last", "end")
        .put("next to last", "end")
        .put("penultimate", "end")
        .put("the last but one", "end")
        .put("antepenultimate", "end")
        .put("next", "current")
        .put("previous", "current")
        .build();
}
