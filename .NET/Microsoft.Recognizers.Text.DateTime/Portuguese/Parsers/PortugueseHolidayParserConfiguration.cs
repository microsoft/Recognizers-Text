using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using Microsoft.Recognizers.Definitions.Portuguese;
using DateObject = System.DateTime;

namespace Microsoft.Recognizers.Text.DateTime.Portuguese
{
    public class PortugueseHolidayParserConfiguration : BaseHolidayParserConfiguration
    {
        public PortugueseHolidayParserConfiguration(IOptionsConfiguration config)
            : base(config)
        {
            this.HolidayRegexList = PortugueseHolidayExtractorConfiguration.HolidayRegexList;
            this.HolidayNames = DateTimeDefinitions.HolidayNames.ToImmutableDictionary();
            this.VariableHolidaysTimexDictionary = DateTimeDefinitions.VariableHolidaysTimexDictionary.ToImmutableDictionary();
        }

        public override int GetSwiftYear(string text)
        {
            var trimmedText = text.Trim().ToLowerInvariant();
            var swift = -10;

            if (PortugueseDatePeriodParserConfiguration.NextPrefixRegex.IsMatch(trimmedText))
            {
                swift = 1;
            }

            if (PortugueseDatePeriodParserConfiguration.PreviousPrefixRegex.IsMatch(trimmedText))
            {
                swift = -1;
            }
            else if (PortugueseDatePeriodParserConfiguration.ThisPrefixRegex.IsMatch(trimmedText))
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
                .Replace("ú", "u")
                .Replace("ê", "e")
                .Replace("ô", "o")
                .Replace("ü", "u")
                .Replace("ã", "a")
                .Replace("õ", "o")
                .Replace("ç", "c");
        }

        protected override IDictionary<string, Func<int, DateObject>> InitHolidayFuncs()
        {
            return new Dictionary<string, Func<int, DateObject>>(base.InitHolidayFuncs())
            {
                { "pai", FathersDay },
                { "mae", MothersDay },
                { "acaodegracas", ThanksgivingDay },
                { "trabalho", LabourDay },
                { "pascoa", Easter },
                { "natal", ChristmasDay },
                { "vesperadenatal", ChristmasEve },
                { "anonovo", NewYear },
                { "versperadeanonovo", NewYearEve },
                { "yuandan", NewYear },
                { "professor", TeacherDay },
                { "todosossantos", HalloweenDay },
                { "crianca", ChildrenDay },
                { "mulher", FemaleDay },
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

        private static DateObject Easter(int year) => DateObject.MinValue;
    }
}
