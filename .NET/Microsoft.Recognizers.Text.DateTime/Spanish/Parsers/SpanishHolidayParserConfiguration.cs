using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using Microsoft.Recognizers.Definitions.Spanish;
using DateObject = System.DateTime;

namespace Microsoft.Recognizers.Text.DateTime.Spanish
{
    public class SpanishHolidayParserConfiguration : BaseHolidayParserConfiguration
    {
        public SpanishHolidayParserConfiguration(IDateTimeOptionsConfiguration config)
            : base(config)
        {
            this.HolidayRegexList = SpanishHolidayExtractorConfiguration.HolidayRegexList;
            this.HolidayNames = DateTimeDefinitions.HolidayNames.ToImmutableDictionary();
            this.VariableHolidaysTimexDictionary = DateTimeDefinitions.VariableHolidaysTimexDictionary.ToImmutableDictionary();
        }

        public override int GetSwiftYear(string text)
        {
            var trimmedText = text.Trim();
            var swift = -10;

            if (SpanishDatePeriodParserConfiguration.NextPrefixRegexCache.IsMatch(trimmedText))
            {
                swift = 1;
            }
            else if (SpanishDatePeriodParserConfiguration.PreviousPrefixRegexCache.IsMatch(trimmedText))
            {
                swift = -1;
            }
            else if (SpanishDatePeriodParserConfiguration.ThisPrefixRegexCache.IsMatch(trimmedText))
            {
                swift = 0;
            }

            return swift;
        }

        public override string SanitizeHolidayToken(string holiday)
        {
            return holiday
                .Replace(" ", string.Empty)
                .Replace("á", "a")
                .Replace("é", "e")
                .Replace("í", "i")
                .Replace("ó", "o")
                .Replace("ú", "u");
        }

        protected override IDictionary<string, Func<int, DateObject>> InitHolidayFuncs()
        {
            return new Dictionary<string, Func<int, DateObject>>(base.InitHolidayFuncs())
            {
                { "padres", FathersDay },
                { "madres", MothersDay },
                { "acciondegracias", ThanksgivingDay },
                { "trabajador", InternationalWorkersDay },
                { "delaraza", ColumbusDay },
                { "memoria", MemorialDay },
                { "pascuas", Pascuas },
                { "navidad", ChristmasDay },
                { "nochebuena", ChristmasEve },
                { "añonuevo", NewYear },
                { "nochevieja", NewYearEve },
                { "yuandan", NewYear },
                { "maestro", TeacherDay },
                { "todoslossantos", HalloweenDay },
                { "niño", ChildrenDay },
                { "mujer", FemaleDay },
            };
        }

        private static DateObject NewYear(int year) => new DateObject(year, 1, 1);

        private static DateObject NewYearEve(int year) => new DateObject(year, 12, 31);

        private static DateObject ChristmasDay(int year) => new DateObject(year, 12, 25);

        private static DateObject ChristmasEve(int year) => new DateObject(year, 12, 24);

        private static DateObject FemaleDay(int year) => new DateObject(year, 3, 8);

        private static DateObject ChildrenDay(int year) => new DateObject(year, 6, 1);

        private static DateObject HalloweenDay(int year) => new DateObject(year, 10, 31);

        private static DateObject TeacherDay(int year) => new DateObject(year, 9, 11);

        private static DateObject Pascuas(int year) => HolidayFunctions.CalculateHolidayByEaster(year);
    }
}
