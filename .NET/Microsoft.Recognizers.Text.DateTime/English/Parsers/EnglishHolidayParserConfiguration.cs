using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Text.RegularExpressions;

using Microsoft.Recognizers.Definitions.English;

using DateObject = System.DateTime;

namespace Microsoft.Recognizers.Text.DateTime.English
{
    public class EnglishHolidayParserConfiguration : BaseHolidayParserConfiguration
    {
        private const RegexOptions RegexFlags = RegexOptions.Singleline | RegexOptions.ExplicitCapture;

        public EnglishHolidayParserConfiguration(IDateTimeOptionsConfiguration config)
            : base(config)
        {
            ThisPrefixRegex = new Regex(DateTimeDefinitions.ThisPrefixRegex, RegexFlags);
            NextPrefixRegex = new Regex(DateTimeDefinitions.NextPrefixRegex, RegexFlags);
            PreviousPrefixRegex = new Regex(DateTimeDefinitions.PreviousPrefixRegex, RegexFlags);

            this.HolidayRegexList = EnglishHolidayExtractorConfiguration.HolidayRegexList;
            this.HolidayNames = DateTimeDefinitions.HolidayNames.ToImmutableDictionary();
        }

        public Regex ThisPrefixRegex { get; }

        public Regex NextPrefixRegex { get; }

        public Regex PreviousPrefixRegex { get; }

        public override int GetSwiftYear(string text)
        {
            var trimmedText = text.Trim();
            var swift = -10;

            if (NextPrefixRegex.IsMatch(trimmedText))
            {
                swift = 1;
            }
            else if (PreviousPrefixRegex.IsMatch(trimmedText))
            {
                swift = -1;
            }
            else if (ThisPrefixRegex.IsMatch(trimmedText))
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
                .Replace(".", string.Empty)
                .Replace("-", string.Empty);
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
                { "cincodemayoday", CincoDeMayoDay },
                { "baptisteday", BaptisteDay },
                { "usindependenceday", UsaIndependenceDay },
                { "independenceday", UsaIndependenceDay },
                { "bastilleday", BastilleDay },
                { "halloweenday", HalloweenDay },
                { "allhallowday", AllHallowDay },
                { "allsoulsday", AllSoulsDay },
                { "guyfawkesday", GuyFawkesDay },
                { "veteransday", VeteransDay },
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
                { "juneteenth", Juneteenth },
                { "ramadan", Ramadan },
                { "sacrifice", Sacrifice },
                { "eidalfitr", EidAlFitr },
                { "islamicnewyear", IslamicNewYear },
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

        private static DateObject CincoDeMayoDay(int year) => new DateObject(year, 5, 5);

        private static DateObject BaptisteDay(int year) => new DateObject(year, 6, 24);

        private static DateObject UsaIndependenceDay(int year) => new DateObject(year, 7, 4);

        private static DateObject BastilleDay(int year) => new DateObject(year, 7, 14);

        private static DateObject HalloweenDay(int year) => new DateObject(year, 10, 31);

        private static DateObject AllHallowDay(int year) => new DateObject(year, 11, 1);

        private static DateObject AllSoulsDay(int year) => new DateObject(year, 11, 2);

        private static DateObject GuyFawkesDay(int year) => new DateObject(year, 11, 5);

        private static DateObject VeteransDay(int year) => new DateObject(year, 11, 11);

        private static DateObject Juneteenth(int year) => new DateObject(year, 6, 19);

        private static DateObject EasterDay(int year) => HolidayFunctions.CalculateHolidayByEaster(year);

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

        private static DateObject Ramadan(int year) => HolidayFunctions.IslamicHoliday(year, HolidayFunctions.IslamicHolidayType.Ramadan);

        private static DateObject Sacrifice(int year) => HolidayFunctions.IslamicHoliday(year, HolidayFunctions.IslamicHolidayType.Sacrifice);

        private static DateObject EidAlFitr(int year) => HolidayFunctions.IslamicHoliday(year, HolidayFunctions.IslamicHolidayType.EidAlFitr);

        private static DateObject IslamicNewYear(int year) => HolidayFunctions.IslamicHoliday(year, HolidayFunctions.IslamicHolidayType.NewYear);
    }
}
