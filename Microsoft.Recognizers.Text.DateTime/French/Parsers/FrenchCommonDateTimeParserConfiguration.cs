using System.Collections.Generic;
using System.Collections.Immutable;
using Microsoft.Recognizers.Text.DateTime.French.Utilities;
using Microsoft.Recognizers.Text.Number;
using Microsoft.Recognizers.Text.Number.French;
using Microsoft.Recognizers.Definitions.French;
using Microsoft.Recognizers.Definitions;

namespace Microsoft.Recognizers.Text.DateTime.French
{
    public class FrenchCommonDateTimeParserConfiguration : BaseDateParserConfiguration
    {
        public FrenchCommonDateTimeParserConfiguration()
        {
            UtilityConfiguration = new FrenchDatetimeUtilityConfiguration();

            UnitMap = DateTimeDefinitions.UnitMap.ToImmutableDictionary();
            UnitValueMap = DateTimeDefinitions.UnitValueMap.ToImmutableDictionary();
            SeasonMap = DateTimeDefinitions.SeasonMap.ToImmutableDictionary();
            CardinalMap = DateTimeDefinitions.CardinalMap.ToImmutableDictionary();
            DayOfWeek = DateTimeDefinitions.DayOfWeek.ToImmutableDictionary();
            MonthOfYear = DateTimeDefinitions.MonthOfYear.ToImmutableDictionary();
            Numbers = DateTimeDefinitions.Numbers.ToImmutableDictionary();
            DoubleNumbers = DateTimeDefinitions.DoubleNumbers.ToImmutableDictionary();

            CardinalExtractor = new CardinalExtractor();
            IntegerExtractor = new IntegerExtractor();
            OrdinalExtractor = new OrdinalExtractor();

            NumberParser = new BaseNumberParser(new FrenchNumberParserConfiguration());
            DateExtractor = new BaseDateExtractor(new FrenchDateExtractorConfiguration());
            TimeExtractor = new BaseTimeExtractor(new FrenchTimeExtractorConfiguration());
            DateTimeExtractor = new BaseDateTimeExtractor(new FrenchDateTimeExtractorConfiguration());
            DurationExtractor = new BaseDurationExtractor(new FrenchDurationExtractorConfiguration());
            DatePeriodExtractor = new BaseDatePeriodExtractor(new FrenchDatePeriodExtractorConfiguration());
            TimePeriodExtractor = new BaseTimePeriodExtractor(new FrenchTimePeriodExtractorConfiguration());
            DateTimePeriodExtractor = new BaseDateTimePeriodExtractor(new FrenchDateTimePeriodExtractorConfiguration());
            DateParser = new BaseDateParser(new FrenchDateParserConfiguration(this));
            TimeParser = new BaseTimeParser(new FrenchTimeParserConfiguration(this));
            DateTimeParser = new BaseDateTimeParser(new FrenchDateTimeParserConfiguration(this));
            DurationParser = new BaseDurationParser(new FrenchDurationParserConfiguration(this));
            UtilityConfiguration = new FrenchDatetimeUtilityConfiguration();
            DatePeriodParser = new BaseDatePeriodParser(new FrenchDatePeriodParserConfiguration(this));
            TimePeriodParser = new BaseTimePeriodParser(new FrenchTimePeriodParserConfiguration(this));
            DateTimePeriodParser = new BaseDateTimePeriodParser(new FrenchDateTimePeriodParserConfiguration(this));
        }

        public override IImmutableDictionary<string, int> DayOfMonth => BaseDateTime.DayOfMonthDictionary.ToImmutableDictionary().AddRange(DateTimeDefinitions.DayOfMonth);

        //private static ImmutableDictionary<string, string> InitUnitMap()
        //{
        //    return new Dictionary<string, string>
        //    {
        //        {"années", "Y"},
        //        {"annees", "Y"},
        //        {"mois", "MON"},
        //        {"mes", "MON"},
        //        {"semaines", "W"},
        //        {"semaine", "W"},
        //        {"journées", "D"},
        //        {"journees", "D"},
        //        {"journée", "D"},
        //        {"journee", "D"},
        //        {"heures", "H"},
        //        {"heure", "H"},
        //        {"hrs", "H"},
        //        {"hr", "H"},
        //        {"h", "H"},
        //        {"minutes", "M"},
        //        {"minute", "M"},
        //        {"mins", "M"},
        //        {"min", "M"},
        //        {"secondes", "S"},
        //        {"seconde", "S"},
        //        {"secs", "S"},
        //        {"sec", "S"}
        //    }.ToImmutableDictionary();
        //}

        //private static ImmutableDictionary<string, long> InitUnitValueMap()
        //{
        //    return new Dictionary<string, long>
        //    {
        //        {"années", 31536000},
        //        {"annees", 31536000},
        //        {"mois", 2592000},
        //        {"mes", 2592000},
        //        {"semaines", 604800},
        //        {"semaine", 604800},
        //        {"journées", 86400},
        //        {"journees", 86400},
        //        {"journée", 86400},
        //        {"journee", 86400},
        //        {"heures", 3600},
        //        {"heure", 3600},
        //        {"hrs", 3600},
        //        {"hr", 3600},
        //        {"h", 3600},
        //        {"minutes", 60},
        //        {"minute", 60},
        //        {"mins", 60},
        //        {"min", 60},
        //        {"secondes", 1},
        //        {"seconde", 1},
        //        {"secs", 1},
        //        {"sec", 1}
        //    }.ToImmutableDictionary();
        //}

        //private static ImmutableDictionary<string, string> InitSeasonMap()
        //{
        //    return new Dictionary<string, string>
        //    {
        //        {"printemps", "SP"},
        //        {"été", "SU"},
        //        {"automne", "FA"},
        //        {"hiver", "WI"}
        //    }.ToImmutableDictionary();
        //}

        //private static ImmutableDictionary<string, int> InitSeasonValueMap()
        //{
        //    return new Dictionary<string, int>
        //    {
        //        {"SP", 3},
        //        {"SU", 6},
        //        {"FA", 9},
        //        {"WI", 12}
        //    }.ToImmutableDictionary();
        //}

        //// TODO: Ordinals with dates get wonky - EDIT 
        //private static ImmutableDictionary<string, int> InitCardinalMap()
        //{
        //    return new Dictionary<string, int>
        //    {
        //        {"premier", 1},
        //        {"1er", 1},
        //        {"seconde", 2},
        //        {"2ème", 2},
        //        {"troisième", 3},
        //        {"3ème", 3},
        //        {"quatrième", 4},
        //        {"4ème", 4},
        //        {"cinqième", 5},
        //        {"5ème", 5}
        //    }.ToImmutableDictionary();
        //}

        //private static ImmutableDictionary<string, int> InitDayOfWeek()
        //{
        //    return new Dictionary<string, int>
        //    {
        //        {"lundi", 1},
        //        {"mardi", 2},
        //        {"mecredi", 3},
        //        {"jeudi", 4},
        //        {"vendredi", 5},
        //        {"samedi", 6},
        //        {"dimanche", 0},
        //        {"lun", 1},
        //        {"mar", 2},
        //        {"mer", 3},
        //        {"jeu", 4},
        //        {"ven", 5},
        //        {"sam", 6},
        //        {"dim", 0}
        //    }.ToImmutableDictionary();
        //}
        //private static ImmutableDictionary<string, int> InitMonthOfYear()
        //{
        //    return new Dictionary<string, int>
        //    {
        //        {"janvier", 1},
        //        {"février", 2},
        //        {"fevrier", 2},
        //        {"mars", 3},
        //        {"avril", 4},
        //        {"mai", 5},
        //        {"juin", 6},
        //        {"juillet", 7},
        //        {"août", 8},
        //        {"aout", 8},
        //        {"septembre", 9},
        //        {"octobre", 10},
        //        {"novembre", 11},
        //        {"decembre", 12},
        //        {"décembre", 12},
        //        {"janv", 1},
        //        {"fevr", 2},
        //        {"févr", 2},
        //        {"mars", 3},
        //        {"avril", 4},
        //        {"juin", 6},
        //        {"juil", 7},
        //        {"sep", 9},
        //        {"sept", 9},
        //        {"oct", 10},
        //        {"nov", 11},
        //        {"dec", 12},
        //        {"déc", 12},
        //        {"1", 1},
        //        {"2", 2},
        //        {"3", 3},
        //        {"4", 4},
        //        {"5", 5},
        //        {"6", 6},
        //        {"7", 7},
        //        {"8", 8},
        //        {"9", 9},
        //        {"10", 10},
        //        {"11", 11},
        //        {"12", 12},
        //        {"01", 1},
        //        {"02", 2},
        //        {"03", 3},
        //        {"04", 4},
        //        {"05", 5},
        //        {"06", 6},
        //        {"07", 7},
        //        {"08", 8},
        //        {"09", 9}
        //    }.ToImmutableDictionary();
        //}

        //private static ImmutableDictionary<string, int> InitNumbers()
        //{
        //    return new Dictionary<string, int>
        //    {
        //        {"zero", 0},
        //        {"un", 1},
        //        {"une", 1},
        //        {"deux", 2},
        //        {"trois", 3},
        //        {"quatre", 4},
        //        {"cinq", 5},
        //        {"six", 6},
        //        {"sept", 7},
        //        {"huit", 8},
        //        {"neuf", 9},
        //        {"dix", 10},
        //        {"onze", 11},
        //        {"douze", 12},
        //        {"treize", 13},
        //        {"quatorze", 14},
        //        {"quinze", 15},
        //        {"seize", 16},
        //        {"dix-sept", 17},
        //        {"dix-huit", 18},
        //        {"dix-neuf", 19},
        //        {"vingt", 20},
        //        {"vingt et un", 21},
        //        {"vingt-et-un", 21},
        //        {"vingt deux", 22},
        //        {"vingt-deux", 22},
        //        {"vingt trois", 23},
        //        {"vingt-trois", 23},
        //        {"vingt quatre", 24},
        //        {"vingt-quatre", 24},
        //        {"vingt cinq", 25},
        //        {"vingt-cinq", 25},
        //        {"vingt six", 26},
        //        {"vingt-six", 26},
        //        {"vingt sept", 27},
        //        {"vingt-sept", 27},
        //        {"vingt huit", 28},
        //        {"vingt-huit", 28},
        //        {"vingt neuf", 29},
        //        {"vingt-neuf", 29},
        //        {"trente", 30},
        //        {"trente et un", 31},
        //        {"trente-et-un", 31},
        //        {"trente deux", 32},
        //        {"trente-deux", 32},
        //        {"trente trois", 33},
        //        {"trente-trois", 33},
        //        {"trente quatre", 34},
        //        {"trente-quatre", 34},
        //        {"trente cinq", 35},
        //        {"trente-cinq", 35},
        //        {"trente six", 36},
        //        {"trente-six", 36},
        //        {"trente sept", 37},
        //        {"trente-sept", 37},
        //        {"trente huit", 38},
        //        { "trente-huit", 38},
        //        { "trente neuf", 39},
        //        { "trente-neuf", 39},
        //        {"quarante", 40},
        //    }.ToImmutableDictionary();
        //}
    }
}
