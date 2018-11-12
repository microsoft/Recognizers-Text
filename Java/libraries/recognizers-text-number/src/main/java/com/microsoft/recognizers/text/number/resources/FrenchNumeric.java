// ------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
// ------------------------------------------------------------------------------

package com.microsoft.recognizers.text.number.resources;

import java.util.Arrays;
import java.util.List;
import java.util.Map;

import com.google.common.collect.ImmutableMap;

public class FrenchNumeric {

    public static final String LangMarker = "Fr";

    public static final String RoundNumberIntegerRegex = "(cent|mille|millions|million|milliard|milliards|billion|billions)";

    public static final String ZeroToNineIntegerRegex = "(et un|un|une|deux|trois|quatre|cinq|six|sept|huit|neuf)";

    public static final String TenToNineteenIntegerRegex = "((seize|quinze|quatorze|treize|douze|onze)|dix(\\Wneuf|\\Whuit|\\Wsept)?)";

    public static final String TensNumberIntegerRegex = "(quatre\\Wvingt(s|\\Wdix)?|soixante\\Wdix|vingt|trente|quarante|cinquante|soixante|septante|octante|huitante|nonante)";

    public static final String DigitsNumberRegex = "\\d|\\d{1,3}(\\.\\d{3})";

    public static final String NegativeNumberTermsRegex = "^[.]";

    public static final String NegativeNumberSignRegex = "^({NegativeNumberTermsRegex}\\s+).*"
            .replace("{NegativeNumberTermsRegex}", NegativeNumberTermsRegex);

    public static final String HundredsNumberIntegerRegex = "(({ZeroToNineIntegerRegex}(\\s+cent))|cent|((\\s+cent\\s)+{TensNumberIntegerRegex}))"
            .replace("{ZeroToNineIntegerRegex}", ZeroToNineIntegerRegex)
            .replace("{TensNumberIntegerRegex}", TensNumberIntegerRegex);

    public static final String BelowHundredsRegex = "(({TenToNineteenIntegerRegex}|({TensNumberIntegerRegex}([-\\s]+({TenToNineteenIntegerRegex}|{ZeroToNineIntegerRegex}))?))|{ZeroToNineIntegerRegex})"
            .replace("{TenToNineteenIntegerRegex}", TenToNineteenIntegerRegex)
            .replace("{TensNumberIntegerRegex}", TensNumberIntegerRegex)
            .replace("{ZeroToNineIntegerRegex}", ZeroToNineIntegerRegex);

    public static final String BelowThousandsRegex = "(({HundredsNumberIntegerRegex}(\\s+{BelowHundredsRegex})?|{BelowHundredsRegex}|{TenToNineteenIntegerRegex})|cent\\s+{TenToNineteenIntegerRegex})"
            .replace("{HundredsNumberIntegerRegex}", HundredsNumberIntegerRegex)
            .replace("{BelowHundredsRegex}", BelowHundredsRegex)
            .replace("{TenToNineteenIntegerRegex}", TenToNineteenIntegerRegex);

    public static final String SupportThousandsRegex = "(({BelowThousandsRegex}|{BelowHundredsRegex})\\s+{RoundNumberIntegerRegex}(\\s+{RoundNumberIntegerRegex})?)"
            .replace("{BelowThousandsRegex}", BelowThousandsRegex)
            .replace("{BelowHundredsRegex}", BelowHundredsRegex)
            .replace("{RoundNumberIntegerRegex}", RoundNumberIntegerRegex);

    public static final String SeparaIntRegex = "({SupportThousandsRegex}(\\s+{SupportThousandsRegex})*(\\s+{BelowThousandsRegex})?|{BelowThousandsRegex})"
            .replace("{SupportThousandsRegex}", SupportThousandsRegex)
            .replace("{BelowThousandsRegex}", BelowThousandsRegex);

    public static final String AllIntRegex = "({SeparaIntRegex}|mille(\\s+{BelowThousandsRegex})?)"
            .replace("{SeparaIntRegex}", SeparaIntRegex)
            .replace("{BelowThousandsRegex}", BelowThousandsRegex);

    public static String NumbersWithPlaceHolder(String placeholder) {
        return "(((?<!\\d+\\s*)-\\s*)|(?<=\\b))\\d+(?!([,\\.]\\d+[a-zA-Z]))(?={placeholder})"
			.replace("{placeholder}", placeholder);
    }

    public static final String NumbersWithSuffix = "(((?<=\\W|^)-\\s*)|(?<=\\b))\\d+\\s*{BaseNumbers.NumberMultiplierRegex}(?=\\b)"
            .replace("{BaseNumbers.NumberMultiplierRegex}", BaseNumbers.NumberMultiplierRegex);

    public static final String RoundNumberIntegerRegexWithLocks = "(?<=\\b)({DigitsNumberRegex})+\\s+{RoundNumberIntegerRegex}(?=\\b)"
            .replace("{DigitsNumberRegex}", DigitsNumberRegex)
            .replace("{RoundNumberIntegerRegex}", RoundNumberIntegerRegex);

    public static final String NumbersWithDozenSuffix = "(((?<!\\d+\\s*)-\\s*)|(?<=\\b))\\d+\\s+douzaine(s)?(?=\\b)";

    public static final String AllIntRegexWithLocks = "((?<=\\b){AllIntRegex}(?=\\b))"
            .replace("{AllIntRegex}", AllIntRegex);

    public static final String AllIntRegexWithDozenSuffixLocks = "(?<=\\b)(((demi\\s+)?\\s+douzaine)|({AllIntRegex}\\s+douzaines?))(?=\\b)"
            .replace("{AllIntRegex}", AllIntRegex);

    public static final String SimpleRoundOrdinalRegex = "(centi[eè]me|milli[eè]me|millioni[eè]me|milliardi[eè]me|billioni[eè]me)";

    public static final String OneToNineOrdinalRegex = "(premier|premi[eè]re|deuxi[eè]me|second[e]|troisi[eè]me|tiers|tierce|quatri[eè]me|cinqui[eè]me|sixi[eè]me|septi[eè]me|huiti[eè]me|neuvi[eè]me)";

    public static final String SpecialUnderHundredOrdinalRegex = "(onzi[eè]me|douzi[eè]me)";

    public static final String TensOrdinalRegex = "(quatre-vingt-dixi[eè]me|quatre-vingti[eè]me|huitanti[eè]me|octanti[eè]me|soixante-dixi[eè]me|septanti[eè]me|soixanti[eè]me|cinquanti[eè]me|quaranti[eè]me|trenti[eè]me|vingti[eè]me)";

    public static final String HundredOrdinalRegex = "({AllIntRegex}(\\s+(centi[eè]me\\s)))"
            .replace("{AllIntRegex}", AllIntRegex);

    public static final String UnderHundredOrdinalRegex = "((({AllIntRegex}(\\W)?)?{OneToNineOrdinalRegex})|({TensNumberIntegerRegex}(\\W)?)?{OneToNineOrdinalRegex}|{TensOrdinalRegex}|{SpecialUnderHundredOrdinalRegex})"
            .replace("{AllIntRegex}", AllIntRegex)
            .replace("{OneToNineOrdinalRegex}", OneToNineOrdinalRegex)
            .replace("{TensNumberIntegerRegex}", TensNumberIntegerRegex)
            .replace("{TensOrdinalRegex}", TensOrdinalRegex)
            .replace("{SpecialUnderHundredOrdinalRegex}", SpecialUnderHundredOrdinalRegex);

    public static final String UnderThousandOrdinalRegex = "((({HundredOrdinalRegex}(\\s)?)?{UnderHundredOrdinalRegex})|(({AllIntRegex}(\\W)?)?{SimpleRoundOrdinalRegex})|{HundredOrdinalRegex})"
            .replace("{HundredOrdinalRegex}", HundredOrdinalRegex)
            .replace("{UnderHundredOrdinalRegex}", UnderHundredOrdinalRegex)
            .replace("{AllIntRegex}", AllIntRegex)
            .replace("{SimpleRoundOrdinalRegex}", SimpleRoundOrdinalRegex);

    public static final String OverThousandOrdinalRegex = "(({AllIntRegex})(i[eè]me))"
            .replace("{AllIntRegex}", AllIntRegex);

    public static final String ComplexOrdinalRegex = "(({OverThousandOrdinalRegex}(\\s)?)?{UnderThousandOrdinalRegex}|{OverThousandOrdinalRegex}|{UnderHundredOrdinalRegex})"
            .replace("{OverThousandOrdinalRegex}", OverThousandOrdinalRegex)
            .replace("{UnderThousandOrdinalRegex}", UnderThousandOrdinalRegex)
            .replace("{UnderHundredOrdinalRegex}", UnderHundredOrdinalRegex);

    public static final String SuffixOrdinalRegex = "(({AllIntRegex})({SimpleRoundOrdinalRegex}))"
            .replace("{AllIntRegex}", AllIntRegex)
            .replace("{SimpleRoundOrdinalRegex}", SimpleRoundOrdinalRegex);

    public static final String ComplexRoundOrdinalRegex = "((({SuffixOrdinalRegex}(\\s)?)?{ComplexOrdinalRegex})|{SuffixOrdinalRegex})"
            .replace("{SuffixOrdinalRegex}", SuffixOrdinalRegex)
            .replace("{ComplexOrdinalRegex}", ComplexOrdinalRegex);

    public static final String AllOrdinalRegex = "({ComplexOrdinalRegex}|{SimpleRoundOrdinalRegex}|{ComplexRoundOrdinalRegex})"
            .replace("{ComplexOrdinalRegex}", ComplexOrdinalRegex)
            .replace("{SimpleRoundOrdinalRegex}", SimpleRoundOrdinalRegex)
            .replace("{ComplexRoundOrdinalRegex}", ComplexRoundOrdinalRegex);

    public static final String PlaceHolderPureNumber = "\\b";

    public static final String PlaceHolderDefault = "\\D|\\b";

    public static final String OrdinalSuffixRegex = "(?<=\\b)((\\d*(1er|2e|2eme|3e|3eme|4e|4eme|5e|5eme|6e|6eme|7e|7eme|8e|8eme|9e|9eme|0e|0eme))|(11e|11eme|12e|12eme))(?=\\b)";

    public static final String OrdinalFrenchRegex = "(?<=\\b){AllOrdinalRegex}(?=\\b)"
            .replace("{AllOrdinalRegex}", AllOrdinalRegex);

    public static final String FractionNotationWithSpacesRegex = "(((?<=\\W|^)-\\s*)|(?<=\\b))\\d+\\s+\\d+[/]\\d+(?=(\\b[^/]|$))";

    public static final String FractionNotationRegex = "(((?<=\\W|^)-\\s*)|(?<=\\b))\\d+[/]\\d+(?=(\\b[^/]|$))";

    public static final String FractionNounRegex = "(?<=\\b)({AllIntRegex}\\s+((et)\\s+)?)?({AllIntRegex})(\\s+((et)\\s)?)((({AllOrdinalRegex})s?|({SuffixOrdinalRegex})s?)|demis?|tiers?|quarts?)(?=\\b)"
            .replace("{AllIntRegex}", AllIntRegex)
            .replace("{AllOrdinalRegex}", AllOrdinalRegex)
            .replace("{SuffixOrdinalRegex}", SuffixOrdinalRegex);

    public static final String FractionNounWithArticleRegex = "(?<=\\b)({AllIntRegex}\\s+(et\\s+)?)?(un|une)(\\s+)(({AllOrdinalRegex})|({SuffixOrdinalRegex})|(et\\s+)?demis?)(?=\\b)"
            .replace("{AllIntRegex}", AllIntRegex)
            .replace("{AllOrdinalRegex}", AllOrdinalRegex)
            .replace("{SuffixOrdinalRegex}", SuffixOrdinalRegex);

    public static final String FractionPrepositionRegex = "(?<=\\b)(?<numerator>({AllIntRegex})|((?<!\\.)\\d+))\\s+sur\\s+(?<denominator>({AllIntRegex})|((\\d+)(?!\\.)))(?=\\b)"
            .replace("{AllIntRegex}", AllIntRegex);

    public static final String AllPointRegex = "((\\s+{ZeroToNineIntegerRegex})+|(\\s+{SeparaIntRegex}))"
            .replace("{ZeroToNineIntegerRegex}", ZeroToNineIntegerRegex)
            .replace("{SeparaIntRegex}", SeparaIntRegex);

    public static final String AllFloatRegex = "({AllIntRegex}(\\s+(virgule|point)){AllPointRegex})"
            .replace("{AllIntRegex}", AllIntRegex)
            .replace("{AllPointRegex}", AllPointRegex);

    public static String DoubleDecimalPointRegex(String placeholder) {
        return "(((?<!\\d+\\s*)-\\s*)|((?<=\\b)(?<!\\d+[,\\.])))\\d+[,\\.]\\d+(?!([,\\.]\\d+))(?={placeholder})"
			.replace("{placeholder}", placeholder);
    }

    public static String DoubleWithoutIntegralRegex(String placeholder) {
        return "(?<=\\s|^)(?<!(\\d+))[,\\.]\\d+(?!([,\\.]\\d+))(?={placeholder})"
			.replace("{placeholder}", placeholder);
    }

    public static final String DoubleWithMultiplierRegex = "(((?<!\\d+\\s*)-\\s*)|((?<=\\b)(?<!\\d+\\[,\\.])))\\d+[,\\.]\\d+\\s*{BaseNumbers.NumberMultiplierRegex}(?=\\b)"
            .replace("{BaseNumbers.NumberMultiplierRegex}", BaseNumbers.NumberMultiplierRegex);

    public static final String DoubleWithRoundNumber = "(((?<!\\d+\\s*)-\\s*)|((?<=\\b)(?<!\\d+\\[,\\.])))\\d+[,\\.]\\d+\\s+{RoundNumberIntegerRegex}(?=\\b)"
            .replace("{RoundNumberIntegerRegex}", RoundNumberIntegerRegex);

    public static final String DoubleAllFloatRegex = "((?<=\\b){AllFloatRegex}(?=\\b))"
            .replace("{AllFloatRegex}", AllFloatRegex);

    public static final String DoubleExponentialNotationRegex = "(((?<!\\d+\\s*)-\\s*)|((?<=\\b)(?<!\\d+[,\\.])))(\\d+([,\\.]\\d+)?)e([+-]*[1-9]\\d*)(?=\\b)";

    public static final String DoubleCaretExponentialNotationRegex = "(((?<!\\d+\\s*)-\\s*)|((?<=\\b)(?<!\\d+[,\\.])))(\\d+([,\\.]\\d+)?)\\^([+-]*[1-9]\\d*)(?=\\b)";

    public static final String NumberWithSuffixPercentage = "(?<!%)({BaseNumbers.NumberReplaceToken})(\\s*)(%(?!{BaseNumbers.NumberReplaceToken})|(pourcentages|pourcents|pourcentage|pourcent)\\b)"
            .replace("{BaseNumbers.NumberReplaceToken}", BaseNumbers.NumberReplaceToken);

    public static final String NumberWithPrefixPercentage = "((?<!{BaseNumbers.NumberReplaceToken})%|pourcent|pourcent des|pourcentage de)(\\s*)({BaseNumbers.NumberReplaceToken})(?=\\s|$)"
            .replace("{BaseNumbers.NumberReplaceToken}", BaseNumbers.NumberReplaceToken);

    public static final Character DecimalSeparatorChar = ',';

    public static final String FractionMarkerToken = "sur";

    public static final Character NonDecimalSeparatorChar = '.';

    public static final String HalfADozenText = "six";

    public static final String WordSeparatorToken = "et";

    public static final List<String> WrittenDecimalSeparatorTexts = Arrays.asList("virgule");

    public static final List<String> WrittenGroupSeparatorTexts = Arrays.asList("point", "points");

    public static final List<String> WrittenIntegerSeparatorTexts = Arrays.asList("et", "-");

    public static final List<String> WrittenFractionSeparatorTexts = Arrays.asList("et", "sur");

    public static final String HalfADozenRegex = "(?<=\\b)+demi\\s+douzaine";

    public static final String DigitalNumberRegex = "((?<=\\b)(cent|mille|million|millions|milliard|milliards|billions|billion|douzaine(s)?)(?=\\b))|((?<=(\\d|\\b)){BaseNumbers.MultiplierLookupRegex}(?=\\b))"
            .replace("{BaseNumbers.MultiplierLookupRegex}", BaseNumbers.MultiplierLookupRegex);

    public static final String AmbiguousFractionConnectorsRegex = "^[.]";

    public static final ImmutableMap<String, Long> CardinalNumberMap = ImmutableMap.<String, Long>builder()
        .put("zéro", 0L)
        .put("zero", 0L)
        .put("un", 1L)
        .put("une", 1L)
        .put("deux", 2L)
        .put("trois", 3L)
        .put("quatre", 4L)
        .put("cinq", 5L)
        .put("six", 6L)
        .put("sept", 7L)
        .put("huit", 8L)
        .put("neuf", 9L)
        .put("dix", 10L)
        .put("onze", 11L)
        .put("douze", 12L)
        .put("treize", 13L)
        .put("quatorze", 14L)
        .put("quinze", 15L)
        .put("seize", 16L)
        .put("dix-sept", 17L)
        .put("dix-huit", 18L)
        .put("dix-neuf", 19L)
        .put("vingt", 20L)
        .put("trente", 30L)
        .put("quarante", 40L)
        .put("cinquante", 50L)
        .put("soixante", 60L)
        .put("soixante-dix", 70L)
        .put("septante", 70L)
        .put("quatre-vingts", 80L)
        .put("quatre-vingt", 80L)
        .put("quatre vingts", 80L)
        .put("quatre vingt", 80L)
        .put("quatre-vingt-dix", 90L)
        .put("quatre-vingt dix", 90L)
        .put("quatre vingt dix", 90L)
        .put("quatre-vingts-dix", 90L)
        .put("quatre-vingts-onze", 91L)
        .put("quatre-vingt-onze", 91L)
        .put("quatre-vingts-douze", 92L)
        .put("quatre-vingt-douze", 92L)
        .put("quatre-vingts-treize", 93L)
        .put("quatre-vingt-treize", 93L)
        .put("quatre-vingts-quatorze", 94L)
        .put("quatre-vingt-quatorze", 94L)
        .put("quatre-vingts-quinze", 95L)
        .put("quatre-vingt-quinze", 95L)
        .put("quatre-vingts-seize", 96L)
        .put("quatre-vingt-seize", 96L)
        .put("huitante", 80L)
        .put("octante", 80L)
        .put("nonante", 90L)
        .put("cent", 100L)
        .put("mille", 1000L)
        .put("un million", 1000000L)
        .put("million", 1000000L)
        .put("millions", 1000000L)
        .put("un milliard", 1000000000L)
        .put("milliard", 1000000000L)
        .put("milliards", 1000000000L)
        .put("un mille milliards", 1000000000000L)
        .put("un billion", 1000000000000L)
        .build();

    public static final ImmutableMap<String, Long> OrdinalNumberMap = ImmutableMap.<String, Long>builder()
        .put("premier", 1L)
        .put("première", 1L)
        .put("premiere", 1L)
        .put("deuxième", 2L)
        .put("deuxieme", 2L)
        .put("second", 2L)
        .put("seconde", 2L)
        .put("troisième", 3L)
        .put("demi", 2L)
        .put("tiers", 3L)
        .put("tierce", 3L)
        .put("quart", 4L)
        .put("quarts", 4L)
        .put("troisieme", 3L)
        .put("quatrième", 4L)
        .put("quatrieme", 4L)
        .put("cinquième", 5L)
        .put("cinquieme", 5L)
        .put("sixième", 6L)
        .put("sixieme", 6L)
        .put("septième", 7L)
        .put("septieme", 7L)
        .put("huitième", 8L)
        .put("huitieme", 8L)
        .put("neuvième", 9L)
        .put("neuvieme", 9L)
        .put("dixième", 10L)
        .put("dixieme", 10L)
        .put("onzième", 11L)
        .put("onzieme", 11L)
        .put("douzième", 12L)
        .put("douzieme", 12L)
        .put("treizième", 13L)
        .put("treizieme", 13L)
        .put("quatorzième", 14L)
        .put("quatorizieme", 14L)
        .put("quinzième", 15L)
        .put("quinzieme", 15L)
        .put("seizième", 16L)
        .put("seizieme", 16L)
        .put("dix-septième", 17L)
        .put("dix-septieme", 17L)
        .put("dix-huitième", 18L)
        .put("dix-huitieme", 18L)
        .put("dix-neuvième", 19L)
        .put("dix-neuvieme", 19L)
        .put("vingtième", 20L)
        .put("vingtieme", 20L)
        .put("trentième", 30L)
        .put("trentieme", 30L)
        .put("quarantième", 40L)
        .put("quarantieme", 40L)
        .put("cinquantième", 50L)
        .put("cinquantieme", 50L)
        .put("soixantième", 60L)
        .put("soixantieme", 60L)
        .put("soixante-dixième", 70L)
        .put("soixante-dixieme", 70L)
        .put("septantième", 70L)
        .put("septantieme", 70L)
        .put("quatre-vingtième", 80L)
        .put("quatre-vingtieme", 80L)
        .put("huitantième", 80L)
        .put("huitantieme", 80L)
        .put("octantième", 80L)
        .put("octantieme", 80L)
        .put("quatre-vingt-dixième", 90L)
        .put("quatre-vingt-dixieme", 90L)
        .put("nonantième", 90L)
        .put("nonantieme", 90L)
        .put("centième", 100L)
        .put("centieme", 100L)
        .put("millième", 1000L)
        .put("millieme", 1000L)
        .put("millionième", 1000000L)
        .put("millionieme", 1000000L)
        .put("milliardième", 1000000000L)
        .put("milliardieme", 1000000000L)
        .put("billionieme", 1000000000000L)
        .put("billionième", 1000000000000L)
        .put("trillionième", 1000000000000000000L)
        .put("trillionieme", 1000000000000000000L)
        .build();

    public static final ImmutableMap<String, Long> PrefixCardinalMap = ImmutableMap.<String, Long>builder()
        .put("deux", 2L)
        .put("trois", 3L)
        .put("quatre", 4L)
        .put("cinq", 5L)
        .put("six", 6L)
        .put("sept", 7L)
        .put("huit", 8L)
        .put("neuf", 9L)
        .put("dix", 10L)
        .put("onze", 11L)
        .put("douze", 12L)
        .put("treize", 13L)
        .put("quatorze", 14L)
        .put("quinze", 15L)
        .put("seize", 16L)
        .put("dix sept", 17L)
        .put("dix-sept", 17L)
        .put("dix-huit", 18L)
        .put("dix huit", 18L)
        .put("dix-neuf", 19L)
        .put("dix neuf", 19L)
        .put("vingt", 20L)
        .put("vingt-et-un", 21L)
        .put("vingt et un", 21L)
        .put("vingt-deux", 21L)
        .put("vingt deux", 22L)
        .put("vingt-trois", 23L)
        .put("vingt trois", 23L)
        .put("vingt-quatre", 24L)
        .put("vingt quatre", 24L)
        .put("vingt-cinq", 25L)
        .put("vingt cinq", 25L)
        .put("vingt-six", 26L)
        .put("vingt six", 26L)
        .put("vingt-sept", 27L)
        .put("vingt sept", 27L)
        .put("vingt-huit", 28L)
        .put("vingt huit", 28L)
        .put("vingt-neuf", 29L)
        .put("vingt neuf", 29L)
        .put("trente", 30L)
        .put("quarante", 40L)
        .put("cinquante", 50L)
        .put("soixante", 60L)
        .put("soixante-dix", 70L)
        .put("soixante dix", 70L)
        .put("septante", 70L)
        .put("quatre-vingt", 80L)
        .put("quatre vingt", 80L)
        .put("huitante", 80L)
        .put("octante", 80L)
        .put("nonante", 90L)
        .put("quatre vingt dix", 90L)
        .put("quatre-vingt-dix", 90L)
        .put("cent", 100L)
        .put("deux cent", 200L)
        .put("trois cents", 300L)
        .put("quatre cents", 400L)
        .put("cinq cent", 500L)
        .put("six cent", 600L)
        .put("sept cent", 700L)
        .put("huit cent", 800L)
        .put("neuf cent", 900L)
        .build();

    public static final ImmutableMap<String, Long> SuffixOrdinalMap = ImmutableMap.<String, Long>builder()
        .put("millième", 1000L)
        .put("million", 1000000L)
        .put("milliardième", 1000000000000L)
        .build();

    public static final ImmutableMap<String, Long> RoundNumberMap = ImmutableMap.<String, Long>builder()
        .put("cent", 100L)
        .put("mille", 1000L)
        .put("million", 1000000L)
        .put("millions", 1000000L)
        .put("milliard", 1000000000L)
        .put("milliards", 1000000000L)
        .put("billion", 1000000000000L)
        .put("billions", 1000000000000L)
        .put("centieme", 100L)
        .put("centième", 100L)
        .put("millieme", 1000L)
        .put("millième", 1000L)
        .put("millionième", 1000000L)
        .put("millionieme", 1000000L)
        .put("milliardième", 1000000000L)
        .put("milliardieme", 1000000000L)
        .put("billionième", 1000000000000L)
        .put("billionieme", 1000000000000L)
        .put("centiemes", 100L)
        .put("centièmes", 100L)
        .put("millièmes", 1000L)
        .put("milliemes", 1000L)
        .put("millionièmes", 1000000L)
        .put("millioniemes", 1000000L)
        .put("milliardièmes", 1000000000L)
        .put("milliardiemes", 1000000000L)
        .put("billionièmes", 1000000000000L)
        .put("billioniemes", 1000000000000L)
        .put("douzaine", 12L)
        .put("douzaines", 12L)
        .put("k", 1000L)
        .put("m", 1000000L)
        .put("g", 1000000000L)
        .put("b", 1000000000L)
        .put("t", 1000000000000L)
        .build();
}
