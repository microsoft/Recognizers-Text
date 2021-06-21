using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Text.RegularExpressions;
using Microsoft.Recognizers.Definitions.Dutch;
using DateObject = System.DateTime;

namespace Microsoft.Recognizers.Text.DateTime.Dutch
{
    public class DutchHolidayParserConfiguration : BaseHolidayParserConfiguration
    {
        private const RegexOptions RegexFlags = RegexOptions.Singleline | RegexOptions.ExplicitCapture;

        public DutchHolidayParserConfiguration(IDateTimeOptionsConfiguration config)
            : base(config)
        {
            ThisPrefixRegex = new Regex(DateTimeDefinitions.ThisPrefixRegex, RegexFlags);
            NextPrefixRegex = new Regex(DateTimeDefinitions.NextPrefixRegex, RegexFlags);
            PreviousPrefixRegex = new Regex(DateTimeDefinitions.PreviousPrefixRegex, RegexFlags);
            this.HolidayRegexList = DutchHolidayExtractorConfiguration.HolidayRegexList;
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
                .Replace(" ", string.Empty)
                .Replace("'", string.Empty)
                .Replace("-", string.Empty);
        }

        protected override IDictionary<string, Func<int, DateObject>> InitHolidayFuncs()
        {
            return new Dictionary<string, Func<int, DateObject>>(base.InitHolidayFuncs())
            {
                { "maosbirthday", MaoBirthday },
                { "yuandan", NewYear },
                { "teachersday", DutchTeachersDay },
                { "singleday", SinglesDay },
                { "allsaintsday", HalloweenDay },
                { "youthday", YouthDay },
                { "childrenday", ChildrenDay },
                { "femaleday", FemaleDay },
                { "treeplantingday", DutchTreePlantDay },
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
                { "veteransday", DutchVeteransday },
                { "christmaseve", ChristmasEve },
                { "newyeareve", NewYearEve },
                { "oudejaarsavond", NewYearEve },
                { "easterday", EasterDay },
                { "eastermonday", EasterMonday },
                { "goodfriday", GoodFriday },
                { "kingsday", KingsDay },
                { "queensday", QueensDay },
                { "prinsjesdag", Prinsjesdag },
                { "eerstekerstdag", ChristmasDay },
                { "tweedekerstdag", SecondChristmasDay },
                { "bevrijdingsdag", Bevrijdingsdag },
                { "dodenherdenking", Dodenherdenking },
                { "dagvandearbeid", Dagvandearbeid },
                { "ascensionday", AscensionDay },
                { "whitesunday", WhiteSunday },
                { "sinterklaas", Sinterklaas },
                { "stmartinsday", StMartinsDay },
                { "driekoningen", Driekoningen },
                { "ketikoti", KetiKoti },
                { "ramadan", Ramadan },
                { "sacrifice", Sacrifice },
                { "eidalfitr", EidAlFitr },
                { "islamicnewyear", IslamicNewYear },
            };
        }

        private static DateObject NewYear(int year) => new DateObject(year, 1, 1);

        private static DateObject NewYearEve(int year) => new DateObject(year, 12, 31);

        private static DateObject ChristmasDay(int year) => new DateObject(year, 12, 25);

        private static DateObject SecondChristmasDay(int year) => new DateObject(year, 12, 26);

        private static DateObject StMartinsDay(int year) => new DateObject(year, 11, 11);

        private static DateObject ChristmasEve(int year) => new DateObject(year, 12, 24);

        private static DateObject ValentinesDay(int year) => new DateObject(year, 2, 14);

        private static DateObject WhiteLoverDay(int year) => new DateObject(year, 3, 14);

        private static DateObject FoolDay(int year) => new DateObject(year, 4, 1);

        private static DateObject GirlsDay(int year) => new DateObject(year, 3, 7);

        private static DateObject DutchTreePlantDay(int year) => DateObject.MinValue.SafeCreateFromValue(year, 3, GetDay(year, 3, 2, DayOfWeek.Wednesday));

        private static DateObject FemaleDay(int year) => new DateObject(year, 3, 8);

        private static DateObject ChildrenDay(int year) => new DateObject(year, 6, 1);

        private static DateObject YouthDay(int year) => new DateObject(year, 5, 4);

        private static DateObject TeacherDay(int year) => new DateObject(year, 10, 5);

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

        private static DateObject GoodFriday(int year) => EasterDay(year).AddDays(-2);

        private static DateObject EasterDay(int year) => HolidayFunctions.CalculateHolidayByEaster(year);

        private static DateObject EasterMonday(int year) => EasterDay(year).AddDays(1);

        private static DateObject KingsDay(int year) => new DateObject(year, 4, 27);

        private static DateObject QueensDay(int year) => new DateObject(year, 4, 30);

        private static DateObject Prinsjesdag(int year) => DateObject.MinValue.SafeCreateFromValue(year, 9, GetDay(year, 9, 2, DayOfWeek.Tuesday));

        private static DateObject Dodenherdenking(int year) => new DateObject(year, 5, 4);

        private static DateObject Bevrijdingsdag(int year) => new DateObject(year, 5, 5);

        private static DateObject Sinterklaas(int year) => new DateObject(year, 12, 5);

        private static DateObject DutchTeachersDay(int year) => new DateObject(year, 10, 5);

        private static DateObject DutchVeteransday(int year) => DateObject.MinValue.SafeCreateFromValue(year, 6, GetLastDay(year, 6, DayOfWeek.Saturday));

        private static DateObject Dagvandearbeid(int year) => new DateObject(year, 5, 1);

        private static DateObject AscensionDay(int year) => EasterDay(year).AddDays(39);

        private static DateObject WhiteSunday(int year) => EasterDay(year).AddDays(49);

        private static DateObject Driekoningen(int year) => new DateObject(year, 1, 6);

        private static DateObject KetiKoti(int year) => new DateObject(year, 7, 1);

        private static DateObject Ramadan(int year) => HolidayFunctions.IslamicHoliday(year, HolidayFunctions.IslamicHolidayType.Ramadan);

        private static DateObject Sacrifice(int year) => HolidayFunctions.IslamicHoliday(year, HolidayFunctions.IslamicHolidayType.Sacrifice);

        private static DateObject EidAlFitr(int year) => HolidayFunctions.IslamicHoliday(year, HolidayFunctions.IslamicHolidayType.EidAlFitr);

        private static DateObject IslamicNewYear(int year) => HolidayFunctions.IslamicHoliday(year, HolidayFunctions.IslamicHolidayType.NewYear);
    }
}