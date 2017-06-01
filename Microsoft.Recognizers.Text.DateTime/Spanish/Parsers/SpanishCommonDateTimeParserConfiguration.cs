using Microsoft.Recognizers.Text.DateTime.Extractors;
using Microsoft.Recognizers.Text.DateTime.Parsers;
using Microsoft.Recognizers.Text.DateTime.Spanish.Extractors;
using Microsoft.Recognizers.Text.Number.Parsers;
using Microsoft.Recognizers.Text.Number.Spanish.Extractors;
using Microsoft.Recognizers.Text.Number.Spanish.Parsers;
using System.Collections.Generic;
using System.Collections.Immutable;

namespace Microsoft.Recognizers.Text.DateTime.Spanish.Parsers
{
    public class SpanishCommonDateTimeParserConfiguration : BaseDateParserConfiguration
    {
        public SpanishCommonDateTimeParserConfiguration()
        {
            UnitMap = InitUnitMap();
            UnitValueMap = InitUnitValueMap();
            SeasonMap = InitSeasonMap();
            CardinalMap = InitCardinalMap();
            DayOfWeek = InitDayOfWeek();
            MonthOfYear = InitMonthOfYear();
            Numbers = InitNumbers();
            CardinalExtractor = new CardinalExtractor();
            IntegerExtractor = new IntegerExtractor();
            OrdinalExtractor = new OrdinalExtractor();
            NumberParser = new BaseNumberParser(new SpanishNumberParserConfiguration());
            DateExtractor = new BaseDateExtractor(new SpanishDateExtractorConfiguration());
            TimeExtractor = new BaseTimeExtractor(new SpanishTimeExtractorConfiguration());
            DateTimeExtractor = new BaseDateTimeExtractor(new SpanishDateTimeExtractorConfiguration());
            DurationExtractor = new BaseDurationExtractor(new SpanishDurationExtractorConfiguration());
            DateParser = new BaseDateParser(new SpanishDateParserConfiguration(this));
            TimeParser = new BaseTimeParser(new SpanishTimeParserConfiguration(this));
            DateTimeParser = new BaseDateTimeParser(new SpanishDateTimeParserConfiguration(this));
            DurationParser = new BaseDurationParser(new SpanishDurationParserConfiguration(this));
        }

        private static ImmutableDictionary<string, string> InitUnitMap()
        {
            return new Dictionary<string, string>
            {
                {"años", "Y"},
                {"año", "Y"},
                {"meses", "MON"},
                {"mes", "MON"},
                {"semanas", "W"},
                {"semana", "W"},
                {"dias", "D"},
                {"dia", "D"},
                {"días", "D"},
                {"día", "D"},
                {"horas", "H"},
                {"hora", "H"},
                {"hrs", "H"},
                {"hr", "H"},
                {"h", "H"},
                {"minutos", "M"},
                {"minuto", "M"},
                {"mins", "M"},
                {"min", "M"},
                {"segundos", "S"},
                {"segundo", "S"},
                {"segs", "S"},
                {"seg", "S"}
            }.ToImmutableDictionary();
        }
        private static ImmutableDictionary<string, long> InitUnitValueMap()
        {
            return new Dictionary<string, long>
            {
                {"años", 31536000},
                {"año", 31536000},
                {"meses", 2592000},
                {"mes", 2592000},
                {"semanas", 604800},
                {"semana", 604800},
                {"dias", 86400},
                {"dia", 86400},
                {"días", 86400},
                {"día", 86400},
                {"horas", 3600},
                {"hora", 3600},
                {"hrs", 3600},
                {"hr", 3600},
                {"h", 3600},
                {"minutos", 60},
                {"minuto", 60},
                {"mins", 60},
                {"min", 60},
                {"segundos", 1},
                {"segundo", 1},
                {"segs", 1},
                {"seg", 1}
            }.ToImmutableDictionary();
        }
        private static ImmutableDictionary<string, string> InitSeasonMap()
        {
            return new Dictionary<string, string>
            {
                {"primavera", "SP"},
                {"verano", "SU"},
                {"otoño", "FA"},
                {"invierno", "WI"}
            }.ToImmutableDictionary();
        }
        private static ImmutableDictionary<string, int> InitSeasonValueMap()
        {
            return new Dictionary<string, int>
            {
                {"SP", 3},
                {"SU", 6},
                {"FA", 9},
                {"WI", 12}
            }.ToImmutableDictionary();
        }
        private static ImmutableDictionary<string, int> InitCardinalMap()
        {
            return new Dictionary<string, int>
            {
                {"primer", 1},
                {"primero", 1},
                {"primera", 1},
                {"1er", 1},
                {"1ro", 1},
                {"1ra", 1},
                {"segundo", 2},
                {"segunda", 2},
                {"2do", 2},
                {"2da", 2},
                {"tercer", 3},
                {"tercero", 3},
                {"tercera", 3},
                {"3er", 3},
                {"3ro", 3},
                {"3ra", 3},
                {"cuarto", 4},
                {"cuarta", 4},
                {"4to", 4},
                {"4ta", 4},
                {"quinto", 5},
                {"quinta", 5},
                {"5to", 5},
                {"5ta", 5}
            }.ToImmutableDictionary();
        }
      
        private static ImmutableDictionary<string, int> InitDayOfWeek()
        {
            return new Dictionary<string, int>
            {
                {"lunes", 1},
                {"martes", 2},
                {"miercoles", 3},
                {"miércoles", 3},
                {"jueves", 4},
                {"viernes", 5},
                {"sabado", 6},
                {"domingo", 0},
                {"lu", 1},
                {"ma", 2},
                {"mi", 3},
                {"ju", 4},
                {"vi", 5},
                {"sa", 6},
                {"do", 0}
            }.ToImmutableDictionary();
        }
        private static ImmutableDictionary<string, int> InitMonthOfYear()
        {
            return new Dictionary<string, int>
            {
                {"enero", 1},
                {"febrero", 2},
                {"marzo", 3},
                {"abril", 4},
                {"mayo", 5},
                {"junio", 6},
                {"julio", 7},
                {"abosto", 8},
                {"septiembre", 9},
                {"setiembre", 9},
                {"octubre", 10},
                {"noviembre", 11},
                {"diciembre", 12},
                {"ene", 1},
                {"feb", 2},
                {"mar", 3},
                {"abr", 4},
                {"may", 5},
                {"jun", 6},
                {"jul", 7},
                {"ago", 8},
                {"sept", 9},
                {"set", 9},
                {"oct", 10},
                {"nov", 11},
                {"dic", 12},
                {"1", 1},
                {"2", 2},
                {"3", 3},
                {"4", 4},
                {"5", 5},
                {"6", 6},
                {"7", 7},
                {"8", 8},
                {"9", 9},
                {"10", 10},
                {"11", 11},
                {"12", 12},
                {"01", 1},
                {"02", 2},
                {"03", 3},
                {"04", 4},
                {"05", 5},
                {"06", 6},
                {"07", 7},
                {"08", 8},
                {"09", 9}
            }.ToImmutableDictionary();
        }
        private static ImmutableDictionary<string, int> InitNumbers()
        {
            return new Dictionary<string, int>
            {
                {"cero", 0},
                {"un", 1},
                {"una", 1},
                {"uno", 1},
                {"dos", 2},
                {"tres", 3},
                {"cuatro", 4},
                {"cinco", 5},
                {"seis", 6},
                {"siete", 7},
                {"ocho", 8},
                {"nueve", 9},
                {"diez", 10},
                {"once", 11},
                {"doce", 12},
                {"docena", 12},
                {"docenas", 12},
                {"trece", 13},
                {"catorce", 14},
                {"quince", 15},
                {"dieciseis", 16},
                {"dieciséis", 16},
                {"diecisiete", 17},
                {"dieciocho", 18},
                {"diecinueve", 19},
                {"veinte", 20},
                {"ventiuna", 21 },
                {"ventiuno", 21 },
                {"veintiun", 21 },
                {"veintiún", 21 },
                {"veintiuno", 21 },
                {"veintiuna", 21 },
                {"veintidos", 22 },
                {"veintidós", 22 },
                {"veintitres", 23 },
                {"veintitrés", 23 },
                {"veinticuatro", 24 },
                {"veinticinco", 25 },
                {"veintiseis", 26 },
                {"veintiséis", 26 },
                {"veintisiete", 27 },
                {"veintiocho", 28 },
                {"veintinueve", 29 },
                {"treinta", 30 },
            }.ToImmutableDictionary();
        }
    }
}
