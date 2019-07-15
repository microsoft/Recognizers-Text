using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Globalization;
using System.Linq;
using Microsoft.Recognizers.Definitions.English;
using DateObject = System.DateTime;

namespace Microsoft.Recognizers.Text.DateTime.English
{
    public class EnglishHolidayParserConfiguration : BaseHolidayParserConfiguration
    {
        public EnglishHolidayParserConfiguration(IOptionsConfiguration config)
            : base(config)
        {
            this.HolidayRegexList = EnglishHolidayExtractorConfiguration.HolidayRegexList;
            this.HolidayNames = DateTimeDefinitions.HolidayNames.ToImmutableDictionary();
        }

        public override int GetSwiftYear(string text)
        {
            var trimmedText = text.Trim();
            var swift = -10;

            if (trimmedText.StartsWith("next"))
            {
                swift = 1;
            }
            else if (trimmedText.StartsWith("last"))
            {
                swift = -1;
            }
            else if (trimmedText.StartsWith("this"))
            {
                swift = 0;
            }

            return swift;
        }

        public override string SanitizeHolidayToken(string holiday)
        {
            return holiday
                .Replace("saint ", "st ")
                .Replace(" ", string.Empty)
                .Replace("'", string.Empty)
                .Replace(".", string.Empty);
        }

        // @TODO Change to auto-generate.
        protected override IDictionary<string, Func<int, DateObject>> InitHolidayFuncs()
        {
            return new Dictionary<string, Func<int, DateObject>>(base.InitHolidayFuncs())
            {
                { "maosbirthday", MaoBirthday },
                { "yuandan", NewYear },
                { "teachersday", TeacherDay },
                { "singleday", SinglesDay },
                { "allsaintsday", HalloweenDay },
                { "youthday", YouthDay },
                { "childrenday", ChildrenDay },
                { "femaleday", FemaleDay },
                { "treeplantingday", TreePlantDay },
                { "arborday", TreePlantDay },
                { "girlsday", GirlsDay },
                { "whiteloverday", WhiteLoverDay },
                { "loverday", ValentinesDay },
                { "christmas", ChristmasDay },
                { "xmas", ChristmasDay },
                { "newyear", NewYear },
                { "newyearday", NewYear },
                { "newyearsday", NewYear },
                { "inaugurationday", InaugurationDay },
                { "groundhougday", GroundhogDay },
                { "valentinesday", ValentinesDay },
                { "stpatrickday", StPatrickDay },
                { "aprilfools", FoolDay },
                { "earthday", EarthDay },
                { "stgeorgeday", StGeorgeDay },
                { "mayday", Mayday },
                { "cincodemayoday", CincoDeMayoday },
                { "baptisteday", BaptisteDay },
                { "usindependenceday", UsaIndependenceDay },
                { "independenceday", UsaIndependenceDay },
                { "bastilleday", BastilleDay },
                { "halloweenday", HalloweenDay },
                { "allhallowday", AllHallowDay },
                { "allsoulsday", AllSoulsday },
                { "guyfawkesday", GuyFawkesDay },
                { "veteransday", Veteransday },
                { "christmaseve", ChristmasEve },
                { "newyeareve", NewYearEve },
                { "easterday", EasterDay },
                { "ashwednesday", AshWednesday },
                { "palmsunday", PalmSunday },
                { "maundythursday", MaundyThursday },
                { "goodfriday", GoodFriday },
                { "eastersaturday", EasterSaturday },
                { "eastermonday", EasterMonday },
                { "ascensionday", AscensionDay },
                { "whitesunday", WhiteSunday },
                { "whitemonday", WhiteMonday },
                { "trinitysunday", TrinitySunday },
                { "corpuschristi", CorpusChristi },
            };
        }

        private static DateObject NewYear(int year) => new DateObject(year, 1, 1);

        private static DateObject NewYearEve(int year) => new DateObject(year, 12, 31);

        private static DateObject ChristmasDay(int year) => new DateObject(year, 12, 25);

        private static DateObject ChristmasEve(int year) => new DateObject(year, 12, 24);

        private static DateObject ValentinesDay(int year) => new DateObject(year, 2, 14);

        private static DateObject WhiteLoverDay(int year) => new DateObject(year, 3, 14);

        private static DateObject FoolDay(int year) => new DateObject(year, 4, 1);

        private static DateObject EarthDay(int year) => new DateObject(year, 4, 22);

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

        private static DateObject EasterDay(int year) => CalculateHolidayByEaster(year);

        private static DateObject AshWednesday(int year) => EasterDay(year).AddDays(-46);

        private static DateObject PalmSunday(int year) => EasterDay(year).AddDays(-7);

        private static DateObject MaundyThursday(int year) => EasterDay(year).AddDays(-3);

        private static DateObject GoodFriday(int year) => EasterDay(year).AddDays(-2);

        private static DateObject EasterSaturday(int year) => EasterDay(year).AddDays(-1);

        private static DateObject EasterMonday(int year) => EasterDay(year).AddDays(1);

        private static DateObject AscensionDay(int year) => EasterDay(year).AddDays(39);

        private static DateObject WhiteSunday(int year) => EasterDay(year).AddDays(49);

        private static DateObject WhiteMonday(int year) => EasterDay(year).AddDays(50);

        private static DateObject TrinitySunday(int year) => EasterDay(year).AddDays(56);

        private static DateObject CorpusChristi(int year) => EasterDay(year).AddDays(60);

        // function adopted from German implementation
        private static DateObject CalculateHolidayByEaster(int year, int days = 0)
        {
            int day = 0;
            int month = 3;

            int g = year % 19;
            int c = year / 100;
            int h = (c - (int)(c / 4) - (int)(((8 * c) + 13) / 25) + (19 * g) + 15) % 30;
            int i = h - ((int)(h / 28) * (1 - ((int)(h / 28) * (int)(29 / (h + 1)) * (int)((21 - g) / 11))));

            day = i - ((year + (int)(year / 4) + i + 2 - c + (int)(c / 4)) % 7) + 28;

            if (day > 31)
            {
                month++;
                day -= 31;
            }

            return DateObject.MinValue.SafeCreateFromValue(year, month, day).AddDays(days);
        }
    }
}
