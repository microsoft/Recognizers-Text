//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
//
//     Generation parameters:
//     - DataFilename: Patterns\Swedish\Swedish-Numbers.yaml
//     - Language: Swedish
//     - ClassName: NumbersDefinitions
// </auto-generated>
//
// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.
// ------------------------------------------------------------------------------

namespace Microsoft.Recognizers.Definitions.Swedish
{
    using System;
    using System.Collections.Generic;

    public static class NumbersDefinitions
    {
      public const string LangMarker = @"Swe";
      public const bool CompoundNumberLanguage = true;
      public const bool MultiDecimalSeparatorCulture = true;
      public const string RoundNumberIntegerRegex = @"(?#RoundNumberIntegerRegex)(hundra|tusen|miljon(er)?|miljard(er)?|biljon(er)?|biljard(er)?|triljon(er)?)";
      public const string ZeroToNineIntegerRegex = @"(?#ZeroToNineIntegerRegex)(tre|sju|åtta|fyra|fem|noll|nio|ett|en|två|sex)";
      public const string TwoToNineIntegerRegex = @"(?#TwoToNineIntegerRegex)(tre|sju|åtta|fyra|fem|nio|två|sex)";
      public const string NegativeNumberTermsRegex = @"(?#NegativeNumberTermsRegex)(?<negTerm>((minus|negativ(t)?)\s+))";
      public static readonly string NegativeNumberSignRegex = $@"(?#NegativeNumberSignRegex)^({NegativeNumberTermsRegex}).*";
      public const string AnIntRegex = @"(?#AnIntRegex)(e(n|tt))(?=\s)";
      public const string TenToNineteenIntegerRegex = @"(?#TenToNineteenIntegerRegex)(sjutton|tretton|fjorton|arton|nitton|femton|sexton|elva|tolv|tio)";
      public const string TensNumberIntegerRegex = @"(?#TensNumberIntegerRegex)(sjuttio|tjugo|trettio|åttio|nittio|fyrtio|femtio|sextio)";
      public static readonly string SeparaIntRegex = $@"(?#SeparaIntRegex)((({TenToNineteenIntegerRegex}|({TensNumberIntegerRegex}(\s+(och\s+)?|\s*-\s*)?{ZeroToNineIntegerRegex})|{TensNumberIntegerRegex}|{ZeroToNineIntegerRegex})(\s*{RoundNumberIntegerRegex})*))|(({AnIntRegex}(\s*{RoundNumberIntegerRegex})+))";
      public static readonly string AllIntRegex = $@"(?#AllIntRegex)(((({TenToNineteenIntegerRegex}|({TensNumberIntegerRegex}(\s+(och\s+)?|\s*-\s*)?{ZeroToNineIntegerRegex})|{TensNumberIntegerRegex}|({ZeroToNineIntegerRegex}|{AnIntRegex}))?(\s*{RoundNumberIntegerRegex})))*{SeparaIntRegex})";
      public const string PlaceHolderPureNumber = @"(?#PlaceHolderPureNumber)\b";
      public const string PlaceHolderDefault = @"(?#PlaceHolderDefault)\D|\b";
      public static readonly Func<string, string> NumbersWithPlaceHolder = (placeholder) => $@"(?#NumbersWithPlaceHolder)(((?<!\d+\s*)-\s*)|(?<=\b))\d+(?!([\.,]\d+[a-zA-Z]))(?={placeholder})";
      public static readonly string NumbersWithSuffix = $@"(?#NumbersWithSuffix)(((?<!\d+\s*)-\s*)|(?<=\b))\d+\s*{BaseNumbers.NumberMultiplierRegex}(?=\b)";
      public static readonly string RoundNumberIntegerRegexWithLocks = $@"(?#RoundNumberIntegerRegexWithLocks)(?<=\b)\d+\s*{RoundNumberIntegerRegex}(?=\b)";
      public const string NumbersWithDozenSuffix = @"(?#NumbersWithDozenSuffix)(((?<!\d+\s*)-\s*)|(?<=\b))\d+\s+dussin(?=\b)";
      public static readonly string AllIntRegexWithLocks = $@"(?#AllIntRegexWithLocks)((?<=\b){AllIntRegex}(?=\b))";
      public static readonly string AllIntRegexWithDozenSuffixLocks = $@"(?#AllIntRegexWithDozenSuffixLocks)(?<=\b)(((ett\s+)?halvt\s+dussin)|({AllIntRegex}\s+dussin))(?=\b)";
      public const string RoundNumberOrdinalRegex = @"(?#RoundNumberOrdinalRegex)(hundrade|tusende|miljonte|miljardte|biljonte|biljardte|triljonte|triljardte)";
      public const string NumberOrdinalRegex = @"(?#NumberOrdinalRegex)(först(e|a)|andr(a|e)|tredje|fjärde|femte|sjätte|sjunde|åttonde|nionde|tioende|elfte|tolfte|trettonde|fjortonde|femtonde|sextonde|sjuttonde|artonde|nittonde|tjugonde|trettionde|fyrtionde|femtionde|sextionde|sjuttionde|åttionde|nittionde)";
      public const string RelativeOrdinalRegex = @"(?#RelativeOrdinalRegex)(?<relativeOrdinal>(\bnäst(a|e)|\bföregående|\bnäst\s+sist(a|e)|\bsist(a|e)|\bnuvarande|\b(före|efter)\s+nuvarande|\bförr(a|e)|\btredje\s+från\s+slutet|\bsenaste|\btidigare|\bföre\s+den\s+sist(a|e)|\b(innan|efter|före)\s+sist(a|e)))";
      public static readonly string BasicOrdinalRegex = $@"(?#BasicOrdinalRegex)({NumberOrdinalRegex}|{RelativeOrdinalRegex})";
      public static readonly string SuffixBasicOrdinalRegex = $@"(?#SuffixBasicOrdinalRegex)((((({TensNumberIntegerRegex}(\s+(och\s+)?|\s*-?\s*){ZeroToNineIntegerRegex})|{TensNumberIntegerRegex}|{ZeroToNineIntegerRegex}|{AnIntRegex})(\s*{RoundNumberIntegerRegex})+)\s*(och\s+)?)*({TensNumberIntegerRegex}(\s+|\s*-?\s*))?{BasicOrdinalRegex})";
      public static readonly string SuffixRoundNumberOrdinalRegex = $@"(?#SuffixRoundNumberOrdinalRegex)(({AllIntRegex}\s*){RoundNumberOrdinalRegex})";
      public static readonly string AllOrdinalRegex = $@"(?#AllOrdinalRegex)({SuffixBasicOrdinalRegex}|{SuffixRoundNumberOrdinalRegex})";
      public const string OrdinalSuffixRegex = @"(?#OrdinalSuffixRegex)(?<=\b)(?:(\d*(1:(e|a)|2:(a|e)|3:e|4:e|5:e|6:e|7:e|8:e|9:e|0:e))|(11:e|12:e))(?=\b)";
      public const string OrdinalNumericRegex = @"(?#OrdinalNumericRegex)(?<=\b)(?:\d{1,3}(\s*,\s*\d{3})*\s*(:(e|a)))(?=\b)";
      public static readonly string OrdinalRoundNumberRegex = $@"(?#OrdinalRoundNumberRegex)(?<!(en|ett)\s+)?{RoundNumberOrdinalRegex}";
      public static readonly string OrdinalSwedishRegex = $@"(?#OrdinalSwedishRegex)(?<=\b){AllOrdinalRegex}(?=\b)";
      public const string RoundNumberFractionSwedishRegex = @"(?#RoundNumberFractionSwedishRegex)(hundradel(s|ar)?|tusendel(s|ar)?|miljon(te)?del(s|ar)?|miljarddel(s|ar)?|biljon(te)?del(s|ar)?|biljarddel(s|ar)?|triljon(te)?del(s|ar)?|triljarddel(s|ar)?)";
      public const string FractionNotationWithSpacesRegex = @"(?#FractionNotationWithSpacesRegex)(((?<=\W|^)-\s*)|(?<=\b))\d+\s+\d+[/]\d+(?=(\b[^/]|$))";
      public static readonly string FractionNotationRegex = $@"(?#FractionNotationRegex)'{BaseNumbers.FractionNotationRegex}'";
      public static readonly string FractionNounRegex = $@"(?#FractionNounRegex)(?<=\b)({AllIntRegex}\s+(och\s+)?)?({AllIntRegex})(\s*|\s*-\s*)((({AllOrdinalRegex})|({RoundNumberFractionSwedishRegex}))((de)?l(s|ar)?)?|halvor|kvart(ar|s))(?=\b)";
      public static readonly string FractionNounWithArticleRegex = $@"(?#FractionNounWithArticleRegex)(?<=\b)((({AllIntRegex}\s+(och\s+)?)?(en|ett)?(\s+|\s*-\s*)(?!\bförsta\b|\bandra\b)(({AllOrdinalRegex})|({RoundNumberFractionSwedishRegex})|halv(t)?|kvart(s)?))|(halva|hälften))(?=\b)";
      public const string FractionOverRegex = @"(?#FractionOverRegex)(genom|delat\s+(med|på)|delad\s+(med|på)|dividerat\s+(med|på)|dividerad\s+(med|på)|(ut)?av|på)";
      public static readonly string FractionPrepositionRegex = $@"(?#FractionPrepositionRegex)(?<!{BaseNumbers.CommonCurrencySymbol}\s*)(?<=\b)(?<numerator>({AllIntRegex})|((?<![\.,])\d+))\s+{FractionOverRegex}\s+(?<denominator>({AllIntRegex})|(\d+)(?![\.,]))(?=\b)";
      public static readonly string FractionPrepositionWithinPercentModeRegex = $@"(?#FractionPrepositionWithinPercentModeRegex)(?<!{BaseNumbers.CommonCurrencySymbol}\s*)(?<=\b)(?<numerator>({AllIntRegex})|((?<![\.,])\d+))\s+genom\s+(?<denominator>({AllIntRegex})|(\d+)(?![\.,]))(?=\b)";
      public static readonly string AllPointRegex = $@"(?#AllPointRegex)((\s+{ZeroToNineIntegerRegex})+|(\s+{SeparaIntRegex}))";
      public static readonly string AllFloatRegex = $@"(?#AllFloatRegex)'{AllIntRegex}(\s+komma){AllPointRegex}'";
      public static readonly string DoubleWithMultiplierRegex = $@"(?#DoubleWithMultiplierRegex)(((?<!\d+\s*)-\s*)|((?<=\b)(?<!\d+[\.,])))\d+[\.,]\d+\s*{BaseNumbers.NumberMultiplierRegex}(?=\b)";
      public const string DoubleExponentialNotationRegex = @"(?#DoubleExponentialNotationRegex)(((?<!\d+\s*)-\s*)|((?<=\b)(?<!\d+[\.,])))(\d+([\.,]\d+)?)e([+-]*[1-9]\d*)(?=\b)";
      public const string DoubleCaretExponentialNotationRegex = @"(?#DoubleCaretExponentialNotationRegex)(((?<!\d+\s*)-\s*)|((?<=\b)(?<!\d+[\.,])))(\d+([\.,]\d+)?)\^([+-]*[1-9]\d*)(?=\b)";
      public static readonly Func<string, string> DoubleDecimalPointRegex = (placeholder) => $@"(?#DoubleDecimalPointRegex)(((?<!\d+\s*)-\s*)|((?<=\b)(?<!\d+[\.,])))\d+[\.,]\d+(?!([\.,]\d+))(?={placeholder})";
      public static readonly Func<string, string> DoubleWithoutIntegralRegex = (placeholder) => $@"(?#DoubleWithoutIntegralRegex)(?<=\s|^)(?<!(\d+))[\.,]\d+(?!([\.,]\d+))(?={placeholder})";
      public static readonly string DoubleWithRoundNumber = $@"(?#DoubleWithRoundNumber)(((?<!\d+\s*)-\s*)|((?<=\b)(?<!\d+[\.,])))\d+[\.,]\d+\s+{RoundNumberIntegerRegex}(?=\b)";
      public static readonly string DoubleAllFloatRegex = $@"(?#DoubleAllFloatRegex)((?<=\b){AllFloatRegex}(?=\b))";
      public const string ConnectorRegex = @"(?#ConnectorRegex)(?<spacer>och)";
      public static readonly string NumberWithSuffixPercentage = $@"(?#NumberWithSuffixPercentage)(?<!%)({BaseNumbers.NumberReplaceToken})(\s*)(%(?!{BaseNumbers.NumberReplaceToken})|(procent|procentenheter)\b)";
      public static readonly string FractionNumberWithSuffixPercentage = $@"(?#FractionNumberWithSuffixPercentage)(({BaseNumbers.FractionNumberReplaceToken})\s+av)";
      public static readonly string NumberWithPrefixPercentage = $@"(?#NumberWithPrefixPercentage)(procent\s+av)(\s*)({BaseNumbers.NumberReplaceToken})";
      public static readonly string NumberWithPrepositionPercentage = $@"(?#NumberWithPrepositionPercentage)({BaseNumbers.NumberReplaceToken})\s*(ut\s+av)\s*({BaseNumbers.NumberReplaceToken})";
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
      public const string AmbiguousFractionConnectorsRegex = @"^[.]";
      public const char DecimalSeparatorChar = ',';
      public const string FractionMarkerToken = @"av";
      public const char NonDecimalSeparatorChar = '.';
      public const string HalfADozenText = @"sex";
      public const string HalfATjogText = @"tio";
      public const string WordSeparatorToken = @"och";
      public static readonly string[] WrittenDecimalSeparatorTexts = { @"komma" };
      public static readonly string[] WrittenGroupSeparatorTexts = { @"punkt" };
      public static readonly string[] WrittenIntegerSeparatorTexts = { @"och" };
      public static readonly string[] WrittenFractionSeparatorTexts = { @"och" };
      public const string HalfADozenRegex = @"ett\s+halvt\s+dussin";
      public static readonly string DigitalNumberRegex = $@"((?<=\b)(hundra|tusen|miljon|miljoner|miljard|miljarder|biljon|biljoner|triljon|triljoner|biljard|biljarder|dussin|tjog)(?=\b))|((?<=(\d|\b)){BaseNumbers.MultiplierLookupRegex}(?=\b))";
      public static readonly Dictionary<string, long> CardinalNumberMap = new Dictionary<string, long>
        {
            { @"ingen", 0 },
            { @"inga", 0 },
            { @"noll", 0 },
            { @"en", 1 },
            { @"ett", 1 },
            { @"två", 2 },
            { @"tre", 3 },
            { @"fyra", 4 },
            { @"fem", 5 },
            { @"sex", 6 },
            { @"sju", 7 },
            { @"åtta", 8 },
            { @"nio", 9 },
            { @"tio", 10 },
            { @"elva", 11 },
            { @"tolv", 12 },
            { @"dussin", 12 },
            { @"dussintal", 12 },
            { @"dussintals", 12 },
            { @"tretton", 13 },
            { @"fjorton", 14 },
            { @"femton", 15 },
            { @"sexton", 16 },
            { @"sjutton", 17 },
            { @"arton", 18 },
            { @"nitton", 19 },
            { @"tjugo", 20 },
            { @"tjog", 20 },
            { @"tjogvis", 20 },
            { @"trettio", 30 },
            { @"tretti", 30 },
            { @"fyrtio", 40 },
            { @"femtio", 50 },
            { @"sextio", 60 },
            { @"sjuttio", 70 },
            { @"åttio", 80 },
            { @"nittio", 90 },
            { @"hundra", 100 },
            { @"tusen", 1000 },
            { @"miljon", 1000000 },
            { @"miljoner", 1000000 },
            { @"miljard", 1000000000 },
            { @"miljarder", 1000000000 },
            { @"biljon", 1000000000000 },
            { @"biljoner", 1000000000000 },
            { @"biljard", 1000000000000000 },
            { @"biljarder", 1000000000000000 },
            { @"triljon", 1000000000000000000 },
            { @"triljoner", 1000000000000000000 }
        };
      public static readonly Dictionary<string, long> OrdinalNumberMap = new Dictionary<string, long>
        {
            { @"första", 1 },
            { @"förste", 1 },
            { @"etta", 1 },
            { @"ettan", 1 },
            { @"andra", 2 },
            { @"andre", 2 },
            { @"sekundära", 2 },
            { @"sekundäre", 2 },
            { @"tvåa", 2 },
            { @"tvåan", 2 },
            { @"halva", 2 },
            { @"halvan", 2 },
            { @"halvt", 2 },
            { @"halv", 2 },
            { @"halvor", 2 },
            { @"hälft", 2 },
            { @"hälften", 2 },
            { @"tredje", 3 },
            { @"tertiära", 3 },
            { @"tertiäre", 3 },
            { @"fjärde", 4 },
            { @"kvart", 4 },
            { @"kvarten", 4 },
            { @"kvarts", 4 },
            { @"femte", 5 },
            { @"sjätte", 6 },
            { @"sjunde", 7 },
            { @"åttonde", 8 },
            { @"nionde", 9 },
            { @"tionde", 10 },
            { @"elfte", 11 },
            { @"tolfte", 12 },
            { @"trettonde", 13 },
            { @"fjortonde", 14 },
            { @"femtonde", 15 },
            { @"sextonde", 16 },
            { @"sjuttonde", 17 },
            { @"artonde", 18 },
            { @"nittonde", 19 },
            { @"tjugonde", 20 },
            { @"trettionde", 30 },
            { @"fyrtionde", 40 },
            { @"femtionde", 50 },
            { @"sextionde", 60 },
            { @"sjuttionde", 70 },
            { @"åttionde", 80 },
            { @"nittionde", 90 },
            { @"hundrade", 100 },
            { @"tusende", 1000 },
            { @"miljonte", 1000000 },
            { @"miljardte", 1000000000 },
            { @"biljonte", 1000000000000 },
            { @"biljardte", 1000000000000000 },
            { @"triljonte", 1000000000000000000 },
            { @"förstadelar", 1 },
            { @"förstedelar", 1 },
            { @"andradelar", 2 },
            { @"andredelar", 2 },
            { @"tredjedelar", 3 },
            { @"tredjedel", 3 },
            { @"tredjedels", 3 },
            { @"fjärdedelar", 4 },
            { @"fjärdedel", 4 },
            { @"fjärdedels", 4 },
            { @"kvartar", 4 },
            { @"femtedelar", 5 },
            { @"femtedel", 5 },
            { @"femtedels", 5 },
            { @"sjättedelar", 6 },
            { @"sjättedel", 6 },
            { @"sjättedels", 6 },
            { @"sjundedelar", 7 },
            { @"sjundedel", 7 },
            { @"sjundedels", 7 },
            { @"åttondelar", 8 },
            { @"åttondedelar", 8 },
            { @"åttondel", 8 },
            { @"åttondedel", 8 },
            { @"åttondels", 8 },
            { @"åttondedels", 8 },
            { @"niondelar", 9 },
            { @"niondedelar", 9 },
            { @"niondel", 9 },
            { @"niondedel", 9 },
            { @"niondels", 9 },
            { @"niondedels", 9 },
            { @"tiondelar", 10 },
            { @"tiondedelar", 10 },
            { @"tiondel", 10 },
            { @"tiondedel", 10 },
            { @"tiondels", 10 },
            { @"tiondedels", 10 },
            { @"elftedelar", 11 },
            { @"elftedel", 11 },
            { @"elftedels", 11 },
            { @"tolftedelar", 12 },
            { @"tolftedel", 12 },
            { @"tolftedels", 12 },
            { @"trettondelar", 13 },
            { @"trettondedelar", 13 },
            { @"trettondel", 13 },
            { @"trettondedel", 13 },
            { @"trettondels", 13 },
            { @"trettondedels", 13 },
            { @"fjortondelar", 14 },
            { @"fjortondedelar", 14 },
            { @"fjortondel", 14 },
            { @"fjortondedel", 14 },
            { @"fjortondels", 14 },
            { @"fjortondedels", 14 },
            { @"femtondelar", 15 },
            { @"femtondedelar", 15 },
            { @"femtondel", 15 },
            { @"femtondedel", 15 },
            { @"femtondels", 15 },
            { @"femtondedels", 15 },
            { @"sextondelar", 16 },
            { @"sextondedelar", 16 },
            { @"sextondel", 16 },
            { @"sextondedel", 16 },
            { @"sextondels", 16 },
            { @"sextondedels", 16 },
            { @"sjuttondelar", 17 },
            { @"sjuttondedelar", 17 },
            { @"sjuttondel", 17 },
            { @"sjuttondedel", 17 },
            { @"sjuttondels", 17 },
            { @"sjuttondedels", 17 },
            { @"artondelar", 18 },
            { @"artondedelar", 18 },
            { @"artondel", 18 },
            { @"artondedel", 18 },
            { @"artondels", 18 },
            { @"artondedels", 18 },
            { @"nittondelar", 19 },
            { @"nittondedelar", 19 },
            { @"nittondel", 19 },
            { @"nittondedel", 19 },
            { @"nittondels", 19 },
            { @"nittondedels", 19 },
            { @"tjugondelar", 20 },
            { @"tjugondedelar", 20 },
            { @"tjugondel", 20 },
            { @"tjugondedel", 20 },
            { @"tjugondels", 20 },
            { @"tjugondedels", 20 },
            { @"trettiondelar", 30 },
            { @"trettiondedelar", 30 },
            { @"trettiondel", 30 },
            { @"trettiondedel", 30 },
            { @"trettiondels", 30 },
            { @"trettiondedels", 30 },
            { @"fyrtiondelar", 40 },
            { @"fyrtiondedelar", 40 },
            { @"fyrtiondel", 40 },
            { @"fyrtiondedel", 40 },
            { @"fyrtiondels", 40 },
            { @"fyrtiondedels", 40 },
            { @"femtiondelar", 50 },
            { @"femtiondedelar", 50 },
            { @"femtiondel", 50 },
            { @"femtiondedel", 50 },
            { @"femtiondels", 50 },
            { @"femtiondedels", 50 },
            { @"sextiondelar", 60 },
            { @"sextiondedelar", 60 },
            { @"sextiondedels", 60 },
            { @"sextiondels", 60 },
            { @"sextiondel", 60 },
            { @"sextiondedel", 60 },
            { @"sjuttiondelar", 70 },
            { @"sjuttiondedelar", 70 },
            { @"sjuttiondel", 70 },
            { @"sjuttiondedel", 70 },
            { @"sjuttiondels", 70 },
            { @"sjuttiondedels", 70 },
            { @"åttiondelar", 80 },
            { @"åttiondedelar", 80 },
            { @"åttiondel", 80 },
            { @"åttiondedel", 80 },
            { @"åttiondels", 80 },
            { @"åttiondedels", 80 },
            { @"nittiondelar", 90 },
            { @"nittiondedelar", 90 },
            { @"nittiondel", 90 },
            { @"nittiondedel", 90 },
            { @"nittiondels", 90 },
            { @"nittiondedels", 90 },
            { @"hundradelar", 100 },
            { @"hundradedelar", 100 },
            { @"hundradel", 100 },
            { @"hundradedel", 100 },
            { @"hundradels", 100 },
            { @"hundradedels", 100 },
            { @"tusendelar", 1000 },
            { @"tusendedelar", 1000 },
            { @"tusendel", 1000 },
            { @"tusendedel", 1000 },
            { @"tusendels", 1000 },
            { @"tusendedels", 1000 },
            { @"miljondelar", 1000000 },
            { @"miljontedelar", 1000000 },
            { @"miljondel", 1000000 },
            { @"miljontedel", 1000000 },
            { @"miljontedels", 1000000 },
            { @"miljondels", 1000000 },
            { @"miljarddelar", 1000000000 },
            { @"miljarddel", 1000000000 },
            { @"miljarddels", 1000000000 },
            { @"biljondelar", 1000000000000 },
            { @"biljondel", 1000000000000 },
            { @"biljontedel", 1000000000000 },
            { @"biljondels", 1000000000000 },
            { @"biljarddelar", 1000000000000000 },
            { @"biljarddel", 1000000000000000 },
            { @"biljarddels", 1000000000000000 },
            { @"triljondelar", 1000000000000000000 },
            { @"triljontedelar", 1000000000000000000 },
            { @"triljontedels", 1000000000000000000 },
            { @"triljondels", 1000000000000000000 },
            { @"triljondel", 1000000000000000000 }
        };
      public static readonly Dictionary<string, long> RoundNumberMap = new Dictionary<string, long>
        {
            { @"hundra", 100 },
            { @"tusen", 1000 },
            { @"miljon", 1000000 },
            { @"milj", 1000000 },
            { @"miljoner", 1000000 },
            { @"miljard", 1000000000 },
            { @"miljarder", 1000000000 },
            { @"biljon", 1000000000000 },
            { @"biljoner", 1000000000000 },
            { @"biljard", 1000000000000000 },
            { @"bijarder", 1000000000000000 },
            { @"triljon", 1000000000000000000 },
            { @"triljoner", 1000000000000000000 },
            { @"hundrade", 100 },
            { @"tusende", 1000 },
            { @"miljonte", 1000000 },
            { @"miljardte", 1000000000 },
            { @"biljonte", 1000000000000 },
            { @"biljardte", 1000000000000000 },
            { @"triljonte", 1000000000000000000 },
            { @"hundratals", 100 },
            { @"tusentals", 1000 },
            { @"miljontals", 1000000 },
            { @"miljardtals", 1000000000 },
            { @"biljontals", 1000000000000 },
            { @"biljardtals", 1000000000000000 },
            { @"triljontals", 1000000000000000000 },
            { @"dussin", 12 },
            { @"tjog", 20 },
            { @"dussintals", 12 },
            { @"k", 1000 },
            { @"m", 1000000 },
            { @"g", 1000000000 },
            { @"b", 1000000000 },
            { @"t", 1000000000000 }
        };
      public static readonly Dictionary<string, string> AmbiguityFiltersDict = new Dictionary<string, string>
        {
            { @"\ben\b", @"\b(en)\s+(en)\b" },
            { @"m", @"\dm\b" }
        };
      public static readonly Dictionary<string, string> RelativeReferenceOffsetMap = new Dictionary<string, string>
        {
            { @"sista", @"0" },
            { @"siste", @"0" },
            { @"senaste", @"0" },
            { @"nästa", @"1" },
            { @"näste", @"1" },
            { @"efter nuvarande", @"1" },
            { @"nuvarande", @"0" },
            { @"föregående", @"-1" },
            { @"före nuvarande", @"-1" },
            { @"förra", @"-1" },
            { @"tidigare", @"-1" },
            { @"näst sista", @"-1" },
            { @"näst siste", @"-1" },
            { @"före den sista", @"-1" },
            { @"före den siste", @"-1" },
            { @"före sista", @"-1" },
            { @"före siste", @"-1" },
            { @"innan siste", @"-1" },
            { @"innan sista", @"-1" },
            { @"efter sista", @"-1" },
            { @"efter siste", @"-1" },
            { @"tredje från slutet", @"-2" }
        };
      public static readonly Dictionary<string, string> RelativeReferenceRelativeToMap = new Dictionary<string, string>
        {
            { @"sista", @"end" },
            { @"siste", @"end" },
            { @"senaste", @"end" },
            { @"nästa", @"current" },
            { @"näste", @"current" },
            { @"efter nuvarande", @"current" },
            { @"nuvarande", @"current" },
            { @"föregående", @"current" },
            { @"före nuvarande", @"current" },
            { @"förra", @"current" },
            { @"tidigare", @"current" },
            { @"näst sista", @"end" },
            { @"näst siste", @"end" },
            { @"före den sista", @"end" },
            { @"före den siste", @"end" },
            { @"före siste", @"end" },
            { @"före sista", @"end" },
            { @"innan siste", @"end" },
            { @"innan sista", @"end" },
            { @"efter sista", @"end" },
            { @"efter siste", @"end" },
            { @"tredje från slutet", @"end" }
        };
    }
}