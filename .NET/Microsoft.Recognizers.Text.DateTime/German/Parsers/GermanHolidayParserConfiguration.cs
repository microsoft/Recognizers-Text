using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using DateObject = System.DateTime;

using Microsoft.Recognizers.Definitions.German;

namespace Microsoft.Recognizers.Text.DateTime.German
{
    public class GermanHolidayParserConfiguration : BaseHolidayParserConfiguration
    {
        public GermanHolidayParserConfiguration() : base()
        {
            this.HolidayRegexList = GermanHolidayExtractorConfiguration.HolidayRegexList;
            this.HolidayNames = DateTimeDefinitions.HolidayNames.ToImmutableDictionary();
        }

        protected override IDictionary<string, Func<int, DateObject>> InitHolidayFuncs()
        {
            return new Dictionary<string, Func<int, DateObject>>(base.InitHolidayFuncs())
            {
                {"maosbirthday", MaoBirthday},
                {"yuandan", NewYear},
                {"teachersday", TeacherDay},
                {"singleday", SinglesDay},
                {"allsaintsday", HalloweenDay},
                {"youthday", YouthDay},
                {"childrenday", ChildrenDay},
                {"femaleday", FemaleDay},
                {"treeplantingday", TreePlantDay},
                {"arborday", TreePlantDay},
                {"girlsday", GirlsDay},
                {"whiteloverday", WhiteLoverDay},
                {"loverday", ValentinesDay},
                {"christmas", ChristmasDay},
                {"xmas", ChristmasDay},
                {"newyear", NewYear},
                {"newyearday", NewYear},
                {"newyearsday", NewYear},
                {"inaugurationday", InaugurationDay},
                {"groundhougday", GroundhogDay},
                {"valentinesday", ValentinesDay},
                {"stpatrickday", StPatrickDay},
                {"aprilfools", FoolDay},
                {"stgeorgeday", StGeorgeDay},
                {"mayday", Mayday},
                {"cincodemayoday", CincoDeMayoday},
                {"baptisteday", BaptisteDay},
                {"usindependenceday", UsaIndependenceDay},
                {"independenceday", UsaIndependenceDay},
                {"bastilleday", BastilleDay},
                {"halloweenday", HalloweenDay},
                {"allhallowday", AllHallowDay},
                {"allsoulsday", AllSoulsday},
                {"guyfawkesday", GuyFawkesDay},
                {"veteransday", Veteransday},
                {"christmaseve", ChristmasEve},
                {"newyeareve", NewYearEve},
                {"easterday", EasterDay}
            };
        }

        private static DateObject NewYear(int year) => new DateObject(year, 1, 1);
        private static DateObject NewYearEve(int year) => new DateObject(year, 12, 31);
        private static DateObject ChristmasDay(int year) => new DateObject(year, 12, 25);
        private static DateObject ChristmasEve(int year) => new DateObject(year, 12, 24);
        private static DateObject ValentinesDay(int year) => new DateObject(year, 2, 14);
        private static DateObject WhiteLoverDay(int year) => new DateObject(year, 3, 14);
        private static DateObject FoolDay(int year) => new DateObject(year, 4, 1);
        private static DateObject GirlsDay(int year) => new DateObject(year, 3, 7);
        private static DateObject TreePlantDay(int year) => new DateObject(year, 3, 12);
        private static DateObject FemaleDay(int year) => new DateObject(year, 3, 8);
        private static DateObject ChildrenDay(int year) => new DateObject(year, 6, 1);
        private static DateObject YouthDay(int year) => new DateObject(year, 5, 4);
        private static DateObject TeacherDay(int year) => new DateObject(year, 9, 10);
        private static DateObject SinglesDay(int year) => new DateObject(year, 11, 11);
        private static DateObject MaoBirthday(int year) => new DateObject(year, 12, 26);
        private static DateObject InaugurationDay(int year) => new DateObject(year, 1, 20);
        private static DateObject GroundhogDay(int year) => new DateObject(year, 2, 2);
        private static DateObject StPatrickDay(int year) => new DateObject(year, 3, 17);
        private static DateObject StGeorgeDay(int year) => new DateObject(year, 4, 23);
        private static DateObject Mayday(int year) => new DateObject(year, 5, 1);
        private static DateObject CincoDeMayoday(int year) => new DateObject(year, 5, 5);
        private static DateObject BaptisteDay(int year) => new DateObject(year, 6, 24);
        private static DateObject UsaIndependenceDay(int year) => new DateObject(year, 7, 4);
        private static DateObject BastilleDay(int year) => new DateObject(year, 7, 14);
        private static DateObject HalloweenDay(int year) => new DateObject(year, 10, 31);
        private static DateObject AllHallowDay(int year) => new DateObject(year, 11, 1);
        private static DateObject AllSoulsday(int year) => new DateObject(year, 11, 2);
        private static DateObject GuyFawkesDay(int year) => new DateObject(year, 11, 5);
        private static DateObject Veteransday(int year) => new DateObject(year, 11, 11);
        private static DateObject EasterDay(int year) => DateObject.MinValue;

        public override int GetSwiftYear(string text)
        {
            var trimedText = text.Trim().ToLowerInvariant();
            var swift = -10;
            if (trimedText.StartsWith("nächster") || trimedText.StartsWith("nächstes") || trimedText.StartsWith("nächsten") || trimedText.StartsWith("nächste"))
            {
                swift = 1;
            }
            else if (trimedText.StartsWith("letzter") || trimedText.StartsWith("letztes") || trimedText.StartsWith("letzten") || trimedText.StartsWith("letzte"))
            {
                swift = -1;
            }
            else if (trimedText.StartsWith("dieser") || trimedText.StartsWith("dieses") || trimedText.StartsWith("diesen") || trimedText.StartsWith("diese"))
            {
                swift = 0;
            }
            return swift;
        }

        public override string SanitizeHolidayToken(string holiday)
        {
            return holiday
                .Replace(" ", "")
                .Replace("'", "");
        }
    }
}
