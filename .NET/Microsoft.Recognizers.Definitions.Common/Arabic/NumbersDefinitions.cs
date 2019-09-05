//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
//
//     Generation parameters:
//     - DataFilename: Patterns\Arabic\Arabic-Numbers.yaml
//     - Language: Arabic
//     - ClassName: NumbersDefinitions
// </auto-generated>
//
// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.
// ------------------------------------------------------------------------------

namespace Microsoft.Recognizers.Definitions.Arabic
{
    using System;
    using System.Collections.Generic;

    public static class NumbersDefinitions
    {
      public const string LangMarker = @"Ara";
      public const bool CompoundNumberLanguage = false;
      public const bool MultiDecimalSeparatorCulture = true;
      public const string RoundNumberIntegerRegex = @"(ال)?(?:مائة|ألف|مليون|مليار|تريليون)";
      public const string ZeroToNineIntegerRegex = @"(ال)?(?:ثلاثة|سبعة|ثلاث|سبع|ثامن|ثمانية|أربعة|أربع|خمس|خمسة|صفر|تسع|تسعة|واحد|واحدة|اثنين|اثنتين|اثنان|اثنتين|ست|ستة)";
      public const string TwoToNineIntegerRegex = @"(ال)?(?:ثلاث|ثلاثة|سبع|سبعة|ثمان|ثمانية|أربع|أربعة|خمس|خمسة|تسع|تسعة|اثنان|اثنتان|اثتنين|اثنتان|ست|ستة)";
      public const string NegativeNumberTermsRegex = @"(?<negTerm>(minus|negative)\s+)";
      public static readonly string NegativeNumberSignRegex = $@"^{NegativeNumberTermsRegex}.*";
      public const string AnIntRegex = @"(an?)(?=\s)";
      public const string TenToNineteenIntegerRegex = @"(?:سبعة عشر|ثلاثة عشر|أربعة عشر|ثمانية عشر|تسعة عشر|خمسة عشر|ستة عشر|حادية عشر|ثاينة عشر|عشرة)";
      public const string TensNumberIntegerRegex = @"(?:سبعون|سبعين|عشرون|عشرين|ثلاثون|ثلاثين|ثمانون|ثمانين|تسعون|تسعين|أربعون|اربعين|خمسون|خمسين|ستون|ستين)";
      public static readonly string SeparaIntRegex = $@"(?:(({TenToNineteenIntegerRegex}|({TensNumberIntegerRegex}(\s+(and\s+)?|\s*-\s*){ZeroToNineIntegerRegex})|{TensNumberIntegerRegex}|{ZeroToNineIntegerRegex})(\s+{RoundNumberIntegerRegex})*))|(({AnIntRegex}(\s+{RoundNumberIntegerRegex})+))";
      public static readonly string AllIntRegex = $@"(?:((({TenToNineteenIntegerRegex}|({TensNumberIntegerRegex}(\s+(and\s+)?|\s*-\s*){ZeroToNineIntegerRegex})|{TensNumberIntegerRegex}|{ZeroToNineIntegerRegex}|{AnIntRegex})(\s+{RoundNumberIntegerRegex})+)\s+(and\s+)?)*{SeparaIntRegex})";
      public const string PlaceHolderPureNumber = @"\b";
      public const string PlaceHolderDefault = @"\D|\b";
      public static readonly Func<string, string> NumbersWithPlaceHolder = (placeholder) => $@"(((?<!\d+\s*)-\s*)|(?<=\b))\d+(?!([\.,]\d+[a-zA-Z]))(?={placeholder})";
      public static readonly string NumbersWithSuffix = $@"(((?<!\d+\s*)-\s*)|(?<=\b))\d+\s*{BaseNumbers.NumberMultiplierRegex}(?=\b)";
      public static readonly string RoundNumberIntegerRegexWithLocks = $@"(?<=\b)\d+\s+{RoundNumberIntegerRegex}(?=\b)";
      public const string NumbersWithDozenSuffix = @"(((?<!\d+\s*)-\s*)|(?<=\b))\d+\s+dozen(s)?(?=\b)";
      public static readonly string AllIntRegexWithLocks = $@"((?<=\b){AllIntRegex}(?=\b))";
      public static readonly string AllIntRegexWithDozenSuffixLocks = $@"(?<=\b)(((half\s+)?a\s+dozen)|({AllIntRegex}\s+dozen(s)?))(?=\b)";
      public const string RoundNumberOrdinalRegex = @"(?:واحدة? بالمئة|واحدة? بالمائة| واحدة? من مائة|واحدة? على مائة|واحدة? من ألف| واحدة? على ألف|واحدة? من مليون|واحدة? على مليون|واحدة? بالمليون|واحدة? من مليار|واحدة? بالمليار|واحدة? بالتريليون|واحدة? من تريليون)";
      public const string NumberOrdinalRegex = @"(?:أولى?|ثانية?|ثالثة?|رابعة?|خامسة?|سادسة?|سابعة?|ثامنة?|تاسعة?|عاشرة?|حادية? عشرة?|ثانية? عشرة?|ثالثة? عشرة?|رابعة? عشرة?|خامسة? عشرة?|ساجسة? عشرة?|سابعة? عشرة?|ثمانية? عشرة?|تاسعة? عشرة?|عشرون|ثلاثون|أربعون|خمسون|ستون|سبعون|ثمانون|تسعون)";
      public const string RelativeOrdinalRegex = @"(?<relativeOrdinal>(تالي|سابق|حالي)\s+one|(the\s+second|next)\s+to\s+last|the\s+one\s+before\s+the\s+last(\s+one)?|the\s+last\s+but\s+one|(ante)?penultimate|last|next|previous|current)";
      public static readonly string BasicOrdinalRegex = $@"({NumberOrdinalRegex}|{RelativeOrdinalRegex})";
      public static readonly string SuffixBasicOrdinalRegex = $@"(?:(((({TensNumberIntegerRegex}(\s+(and\s+)?|\s*-\s*){ZeroToNineIntegerRegex})|{TensNumberIntegerRegex}|{ZeroToNineIntegerRegex}|{AnIntRegex})(\s+{RoundNumberIntegerRegex})+)\s+(and\s+)?)*({TensNumberIntegerRegex}(\s+|\s*-\s*))?{BasicOrdinalRegex})";
      public static readonly string SuffixRoundNumberOrdinalRegex = $@"(?:({AllIntRegex}\s+){RoundNumberOrdinalRegex})";
      public static readonly string AllOrdinalRegex = $@"(?:{SuffixBasicOrdinalRegex}|{SuffixRoundNumberOrdinalRegex})";
      public const string OrdinalNumericRegex = @"(?<=\b)(?:\d{1,3}(\s*,\s*\d{3})*\s*th)(?=\b)";
      public static readonly string OrdinalRoundNumberRegex = $@"(?<!an?\s+){RoundNumberOrdinalRegex}";
      public static readonly string OrdinalEnglishRegex = $@"(?<=\b){AllOrdinalRegex}(?=\b)";
      public const string FractionNotationWithSpacesRegex = @"(((?<=\W|^)-\s*)|(?<=\b))\d+\s+\d+[/]\d+(?=(\b[^/]|$))";
      public const string FractionNotationRegex = @"(((?<=\W|^)-\s*)|(?<![/-])(?<=\b))\d+[/]\d+(?=(\b[^/]|$))";
      public static readonly string FractionNounRegex = $@"(?<=\b)({AllIntRegex}\s+(and\s+)?)?({AllIntRegex})(\s+|\s*-\s*)((({AllOrdinalRegex})|({RoundNumberOrdinalRegex}))s|halves|quarters)(?=\b)";
      public static readonly string FractionNounWithArticleRegex = $@"(?<=\b)((({AllIntRegex}\s+(and\s+)?)?(an?|one)(\s+|\s*-\s*)(?!\bfirst\b|\bsecond\b)(({AllOrdinalRegex})|({RoundNumberOrdinalRegex})|half|quarter))|(half))(?=\b)";
      public static readonly string FractionPrepositionRegex = $@"(?<!{BaseNumbers.CommonCurrencySymbol}\s*)(?<=\b)(?<numerator>({AllIntRegex})|((?<![\.,])\d+))\s+(over|in|out\s+of)\s+(?<denominator>({AllIntRegex})|(\d+)(?![\.,]))(?=\b)";
      public static readonly string FractionPrepositionWithinPercentModeRegex = $@"(?<!{BaseNumbers.CommonCurrencySymbol}\s*)(?<=\b)(?<numerator>({AllIntRegex})|((?<![\.,])\d+))\s+over\s+(?<denominator>({AllIntRegex})|(\d+)(?![\.,]))(?=\b)";
      public static readonly string AllPointRegex = $@"((\s+{ZeroToNineIntegerRegex})+|(\s+{SeparaIntRegex}))";
      public static readonly string AllFloatRegex = $@"{AllIntRegex}(\s+point){AllPointRegex}";
      public static readonly string DoubleWithMultiplierRegex = $@"(((?<!\d+\s*)-\s*)|((?<=\b)(?<!\d+[\.,])))\d+[\.,]\d+\s*{BaseNumbers.NumberMultiplierRegex}(?=\b)";
      public const string DoubleExponentialNotationRegex = @"(((?<!\d+\s*)-\s*)|((?<=\b)(?<!\d+[\.,])))(\d+([\.,]\d+)?)e([+-]*[1-9]\d*)(?=\b)";
      public const string DoubleCaretExponentialNotationRegex = @"(((?<!\d+\s*)-\s*)|((?<=\b)(?<!\d+[\.,])))(\d+([\.,]\d+)?)\^([+-]*[1-9]\d*)(?=\b)";
      public static readonly Func<string, string> DoubleDecimalPointRegex = (placeholder) => $@"(((?<!\d+\s*)-\s*)|((?<=\b)(?<!\d+[\.,])))\d+[\.,]\d+(?!([\.,]\d+))(?={placeholder})";
      public static readonly Func<string, string> DoubleWithoutIntegralRegex = (placeholder) => $@"(?<=\s|^)(?<!(\d+))[\.,]\d+(?!([\.,]\d+))(?={placeholder})";
      public static readonly string DoubleWithRoundNumber = $@"(((?<!\d+\s*)-\s*)|((?<=\b)(?<!\d+[\.,])))\d+[\.,]\d+\s+{RoundNumberIntegerRegex}(?=\b)";
      public static readonly string DoubleAllFloatRegex = $@"((?<=\b){AllFloatRegex}(?=\b))";
      public const string ConnectorRegex = @"(?<spacer>and)";
      public static readonly string NumberWithSuffixPercentage = $@"(?<!%)({BaseNumbers.NumberReplaceToken})(\s*)(%(?!{BaseNumbers.NumberReplaceToken})|(per\s*cents?|percentage|cents?)\b)";
      public static readonly string FractionNumberWithSuffixPercentage = $@"(({BaseNumbers.FractionNumberReplaceToken})\s+of)";
      public static readonly string NumberWithPrefixPercentage = $@"(per\s*cents?\s+of)(\s*)({BaseNumbers.NumberReplaceToken})";
      public static readonly string NumberWithPrepositionPercentage = $@"({BaseNumbers.NumberReplaceToken})\s*(in|out\s+of)\s*({BaseNumbers.NumberReplaceToken})";
      public const string TillRegex = @"(to|through|--|-|—|——|~|–)";
      public const string MoreRegex = @"(?:(bigger|greater|more|higher|larger)(\s+than)?|above|over|exceed(ed|ing)?|surpass(ed|ing)?|(?<!<|=)>)";
      public const string LessRegex = @"(?:(less|lower|smaller|fewer)(\s+than)?|below|under|(?<!>|=)<)";
      public const string EqualRegex = @"(equal(s|ing)?(\s+(to|than))?|(?<!<|>)=)";
      public static readonly string MoreOrEqualPrefix = $@"((no\s+{LessRegex})|(at\s+least))";
      public static readonly string MoreOrEqual = $@"(?:({MoreRegex}\s+(or)?\s+{EqualRegex})|({EqualRegex}\s+(or)?\s+{MoreRegex})|{MoreOrEqualPrefix}(\s+(or)?\s+{EqualRegex})?|({EqualRegex}\s+(or)?\s+)?{MoreOrEqualPrefix}|>\s*=)";
      public const string MoreOrEqualSuffix = @"((and|or)\s+(((more|greater|higher|larger|bigger)((?!\s+than)|(\s+than(?!(\s*\d+)))))|((over|above)(?!\s+than))))";
      public static readonly string LessOrEqualPrefix = $@"((no\s+{MoreRegex})|(at\s+most)|(up\s+to))";
      public static readonly string LessOrEqual = $@"(({LessRegex}\s+(or)?\s+{EqualRegex})|({EqualRegex}\s+(or)?\s+{LessRegex})|{LessOrEqualPrefix}(\s+(or)?\s+{EqualRegex})?|({EqualRegex}\s+(or)?\s+)?{LessOrEqualPrefix}|<\s*=)";
      public const string LessOrEqualSuffix = @"((and|or)\s+(less|lower|smaller|fewer)((?!\s+than)|(\s+than(?!(\s*\d+)))))";
      public const string NumberSplitMark = @"(?![,.](?!\d+))";
      public const string MoreRegexNoNumberSucceed = @"((bigger|greater|more|higher|larger)((?!\s+than)|\s+(than(?!(\s*\d+))))|(above|over)(?!(\s*\d+)))";
      public const string LessRegexNoNumberSucceed = @"((less|lower|smaller|fewer)((?!\s+than)|\s+(than(?!(\s*\d+))))|(below|under)(?!(\s*\d+)))";
      public const string EqualRegexNoNumberSucceed = @"(equal(s|ing)?((?!\s+(to|than))|(\s+(to|than)(?!(\s*\d+)))))";
      public static readonly string OneNumberRangeMoreRegex1 = $@"({MoreOrEqual}|{MoreRegex})\s*(the\s+)?(?<number1>({NumberSplitMark}.)+)";
      public static readonly string OneNumberRangeMoreRegex2 = $@"(?<number1>({NumberSplitMark}.)+)\s*{MoreOrEqualSuffix}";
      public static readonly string OneNumberRangeMoreSeparateRegex = $@"({EqualRegex}\s+(?<number1>({NumberSplitMark}.)+)(\s+or\s+){MoreRegexNoNumberSucceed})|({MoreRegex}\s+(?<number1>({NumberSplitMark}.)+)(\s+or\s+){EqualRegexNoNumberSucceed})";
      public static readonly string OneNumberRangeLessRegex1 = $@"({LessOrEqual}|{LessRegex})\s*(the\s+)?(?<number2>({NumberSplitMark}.)+)";
      public static readonly string OneNumberRangeLessRegex2 = $@"(?<number2>({NumberSplitMark}.)+)\s*{LessOrEqualSuffix}";
      public static readonly string OneNumberRangeLessSeparateRegex = $@"({EqualRegex}\s+(?<number1>({NumberSplitMark}.)+)(\s+or\s+){LessRegexNoNumberSucceed})|({LessRegex}\s+(?<number1>({NumberSplitMark}.)+)(\s+or\s+){EqualRegexNoNumberSucceed})";
      public static readonly string OneNumberRangeEqualRegex = $@"{EqualRegex}\s*(the\s+)?(?<number1>({NumberSplitMark}.)+)";
      public static readonly string TwoNumberRangeRegex1 = $@"between\s*(the\s+)?(?<number1>({NumberSplitMark}.)+)\s*and\s*(the\s+)?(?<number2>({NumberSplitMark}.)+)";
      public static readonly string TwoNumberRangeRegex2 = $@"({OneNumberRangeMoreRegex1}|{OneNumberRangeMoreRegex2})\s*(and|but|,)\s*({OneNumberRangeLessRegex1}|{OneNumberRangeLessRegex2})";
      public static readonly string TwoNumberRangeRegex3 = $@"({OneNumberRangeLessRegex1}|{OneNumberRangeLessRegex2})\s*(and|but|,)\s*({OneNumberRangeMoreRegex1}|{OneNumberRangeMoreRegex2})";
      public static readonly string TwoNumberRangeRegex4 = $@"(from\s+)?(?<number1>({NumberSplitMark}(?!\bfrom\b).)+)\s*{TillRegex}\s*(the\s+)?(?<number2>({NumberSplitMark}.)+)";
      public const string AmbiguousFractionConnectorsRegex = @"(\bin\b)";
      public const char DecimalSeparatorChar = '.';
      public const string FractionMarkerToken = @"over";
      public const char NonDecimalSeparatorChar = ',';
      public const string HalfADozenText = @"six";
      public const string WordSeparatorToken = @"and";
      public static readonly string[] WrittenDecimalSeparatorTexts = { @"point" };
      public static readonly string[] WrittenGroupSeparatorTexts = { @"punto" };
      public static readonly string[] WrittenIntegerSeparatorTexts = { @"and" };
      public static readonly string[] WrittenFractionSeparatorTexts = { @"and" };
      public const string HalfADozenRegex = @"نصف?\sدستة";
      public static readonly string DigitalNumberRegex = $@"((?<=\b)(hundred|thousand|[mb]illion|trillion|dozen(s)?)(?=\b))|((?<=(\d|\b)){BaseNumbers.MultiplierLookupRegex}(?=\b))";
      public static readonly Dictionary<string, long> CardinalNumberMap = new Dictionary<string, long>
        {
            { @"واحد", 1 },
            { @"صفر", 0 },
            { @"اثنان", 2 },
            { @"ثلاثة", 3 },
            { @"أربعة", 4 },
            { @"خمسة", 5 },
            { @"ستة", 6 },
            { @"سبعة", 7 },
            { @"ثمانية", 8 },
            { @"تسعة", 9 },
            { @"عشرة", 10 },
            { @"إحدى عشر", 11 },
            { @"اثنى عشر", 12 },
            { @"دستة", 12 },
            { @"دستات", 12 },
            { @"ثلاثة عشر", 13 },
            { @"أربعة عشر", 14 },
            { @"خمسة عشر", 15 },
            { @"ستة عشر", 16 },
            { @"سبعة عشر", 17 },
            { @"ثمانية عشر", 18 },
            { @"تسعة عشر", 19 },
            { @"عشرون", 20 },
            { @"ثلاثون", 30 },
            { @"أربعون", 40 },
            { @"خمسون", 50 },
            { @"ستون", 60 },
            { @"سبعون", 70 },
            { @"ثمانون", 80 },
            { @"تسعون", 90 },
            { @"مائة", 100 },
            { @"ألف", 1000 },
            { @"مليون", 1000000 },
            { @"مليار", 1000000000 },
            { @"تريليون", 1000000000000 }
        };
      public static readonly Dictionary<string, long> OrdinalNumberMap = new Dictionary<string, long>
        {
            { @"أول", 1 },
            { @"ثاني", 2 },
            { @"ثان", 2 },
            { @"نصف", 2 },
            { @"ثلث", 3 },
            { @"ربع", 4 },
            { @"خمس", 5 },
            { @"سدس", 6 },
            { @"سبع", 7 },
            { @"ثمن", 8 },
            { @"تسع", 9 },
            { @"عشر", 10 },
            { @"واحد من إحدى عشر", 11 },
            { @"واحد من إثنى عشر", 12 },
            { @"واحد من ثلاثة عشر", 13 },
            { @"واحد من أربعة عشر", 14 },
            { @"واحد من خمسة عشر", 15 },
            { @"واحد من ستة عشر", 16 },
            { @"واحد من سبعة عشر", 17 },
            { @"واحد من ثمانية عشر", 18 },
            { @"واحد من تسعة عشر", 19 },
            { @"واحد من عشرين", 20 },
            { @"واحد من ثلاثين", 30 },
            { @"واحد من أربعين", 40 },
            { @"واحد من خمسين", 50 },
            { @"واحد من ستين", 60 },
            { @"واحد من سبعين", 70 },
            { @"واحد من ثمانين", 80 },
            { @"واحد من تسعين", 90 },
            { @"واحد من مائة", 100 },
            { @"واحد من ألف", 1000 },
            { @"واحد من مليون", 1000000 },
            { @"واحد من مليار", 1000000000 },
            { @"واحد من تريليون", 1000000000000 },
            { @"أوائل", 1 },
            { @"أنصاف", 2 },
            { @"أثلاث", 3 },
            { @"أرباع", 4 },
            { @"أخماس", 5 },
            { @"أسداس", 6 },
            { @"أسباع", 7 },
            { @"أثمان", 8 },
            { @"أتساع", 9 },
            { @"أعشار", 10 },
            { @"عشرينات", 20 },
            { @"ثلاثينات", 30 },
            { @"أربعينات", 40 },
            { @"خمسينات", 50 },
            { @"ستينات", 60 },
            { @"سبعينات", 70 },
            { @"ثمانينات", 80 },
            { @"تسعينات", 90 },
            { @"مئات", 100 },
            { @"ألوف", 1000 },
            { @"ملايين", 1000000 },
            { @"مليارات", 1000000000 },
            { @"بليونات", 1000000000000 }
        };
      public static readonly Dictionary<string, long> RoundNumberMap = new Dictionary<string, long>
        {
            { @"مائة", 100 },
            { @"ألف", 1000 },
            { @"مليون", 1000000 },
            { @"مليار", 1000000000 },
            { @"تريليون", 1000000000000 },
            { @"hundredth", 100 },
            { @"thousandth", 1000 },
            { @"millionth", 1000000 },
            { @"billionth", 1000000000 },
            { @"trillionth", 1000000000000 },
            { @"hundredths", 100 },
            { @"thousandths", 1000 },
            { @"millionths", 1000000 },
            { @"billionths", 1000000000 },
            { @"trillionths", 1000000000000 },
            { @"dozen", 12 },
            { @"dozens", 12 },
            { @"k", 1000 },
            { @"m", 1000000 },
            { @"g", 1000000000 },
            { @"b", 1000000000 },
            { @"t", 1000000000000 }
        };
      public static readonly Dictionary<string, string> AmbiguityFiltersDict = new Dictionary<string, string>
        {
            { @"\bone\b", @"\b(the|this|that|which)\s+(one)\b" }
        };
      public static readonly Dictionary<string, string> RelativeReferenceOffsetMap = new Dictionary<string, string>
        {
            { @"last", @"0" },
            { @"next one", @"1" },
            { @"current", @"0" },
            { @"current one", @"0" },
            { @"previous one", @"-1" },
            { @"the second to last", @"-1" },
            { @"the one before the last one", @"-1" },
            { @"the one before the last", @"-1" },
            { @"next to last", @"-1" },
            { @"penultimate", @"-1" },
            { @"the last but one", @"-1" },
            { @"antepenultimate", @"-2" },
            { @"next", @"1" },
            { @"previous", @"-1" }
        };
      public static readonly Dictionary<string, string> RelativeReferenceRelativeToMap = new Dictionary<string, string>
        {
            { @"last", @"end" },
            { @"next one", @"current" },
            { @"previous one", @"current" },
            { @"current", @"current" },
            { @"current one", @"current" },
            { @"the second to last", @"end" },
            { @"the one before the last one", @"end" },
            { @"the one before the last", @"end" },
            { @"next to last", @"end" },
            { @"penultimate", @"end" },
            { @"the last but one", @"end" },
            { @"antepenultimate", @"end" },
            { @"next", @"current" },
            { @"previous", @"current" }
        };
    }
}