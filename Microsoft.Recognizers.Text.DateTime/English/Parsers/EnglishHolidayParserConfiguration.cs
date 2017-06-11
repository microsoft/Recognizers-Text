using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using DateObject = System.DateTime;

namespace Microsoft.Recognizers.Text.DateTime.English
{
    public class EnglishHolidayParserConfiguration : BaseHolidayParserConfiguration
    {
        public EnglishHolidayParserConfiguration() : base()
        {
            this.HolidayRegexList = EnglishHolidayExtractorConfiguration.HolidayRegexList;
            this.HolidayNames = InitHolidayNames().ToImmutableDictionary();
        }

        private IDictionary<string, IEnumerable<string>> InitHolidayNames()
        {
            return new Dictionary<string, IEnumerable<string>>
            {
                { "fathers", new string[]{ "fatherday", "fathersday" } },
                { "mothers", new string[]{ "motherday", "mothersday" } },
                { "thanksgiving", new string[]{ "thanksgivingday" } },
                { "martinlutherking", new string[]{ "martinlutherkingday", "martinlutherkingjrday" } },
                { "washingtonsbirthday", new string[]{ "washingtonsbirthday", "washingtonbirthday" } },
                { "canberra", new string[]{ "canberraday" } },
                { "labour", new string[]{ "labourday", "laborday" } },
                { "columbus", new string[]{ "columbusday" } },
                { "memorial", new string[]{ "memorialday" } },
                { "yuandan", new string[]{ "yuandan" } },
            };
        }

        protected override IDictionary<string, Func<int, DateObject>> InitHolidayFuncs()
        {
            return new Dictionary<string, Func<int, DateObject>>(base.InitHolidayFuncs())
            {
                {"yuandan", Yuandan},
                {"chinesenational", ChineseNationalDay},
            };
        }
        
        private static DateObject Yuandan(int year) => new DateObject(year, 1, 1);
        private static DateObject ChineseNationalDay(int year) => new DateObject(year, 10, 1);
        private static DateObject WorkDay(int year) => new DateObject(year, 5, 1);
        private static DateObject ChristmasDay(int year) => new DateObject(year, 12, 25);
        private static DateObject LoverDay(int year) => new DateObject(year, 2, 14);
        private static DateObject WhiteLoverDay(int year) => new DateObject(year, 3, 14);
        private static DateObject ChineseMilbuildday(int year) => new DateObject(year, 8, 1);
        private static DateObject FoolDay(int year) => new DateObject(year, 4, 1);
        private static DateObject GirlsDay(int year) => new DateObject(year, 3, 7);
        private static DateObject TreePlantDay(int year) => new DateObject(year, 3, 12);
        private static DateObject FemaleDay(int year) => new DateObject(year, 3, 8);
        private static DateObject ChildrenDay(int year) => new DateObject(year, 6, 1);
        private static DateObject YouthDay(int year) => new DateObject(year, 5, 4);
        private static DateObject TeacherDay(int year) => new DateObject(year, 9, 10);
        private static DateObject AllSaintsDay(int year) => new DateObject(year, 10, 31);
        private static DateObject SinglesDay(int year) => new DateObject(year, 11, 11);
        private static DateObject MaoBirthday(int year) => new DateObject(year, 12, 26);
        private static DateObject InaugurationDay(int year) => new DateObject(year, 1, 20);
        private static DateObject GroundhogDay(int year) => new DateObject(year, 2, 2);
        private static DateObject StPatrickDay(int year) => new DateObject(year, 3, 17);
        private static DateObject StGeorgeDay(int year) => new DateObject(year, 4, 23);
        private static DateObject Mayday(int year) => new DateObject(year, 5, 1);
        private static DateObject CincoDeMayoday(int year) => new DateObject(year, 5, 5);
        private static DateObject BaptisteDay(int year) => new DateObject(year, 6, 24);
        private static DateObject USAIndependenceDay(int year) => new DateObject(year, 7, 4);
        private static DateObject BastilleDay(int year) => new DateObject(year, 7, 14);
        private static DateObject HalloweenDay(int year) => new DateObject(year, 10, 31);
        private static DateObject AllHallowDay(int year) => new DateObject(year, 11, 1);
        private static DateObject AllSoulsday(int year) => new DateObject(year, 11, 2);
        private static DateObject GuyFawkesDay(int year) => new DateObject(year, 11, 5);
        private static DateObject Veteransday(int year) => new DateObject(year, 11, 11);


        public override int GetSwiftYear(string text)
        {
            var trimedText = text.Trim().ToLowerInvariant();
            var swift = -10;
            if (trimedText.StartsWith("next"))
            {
                swift = 1;
            }
            else if (trimedText.StartsWith("last"))
            {
                swift = -1;
            }
            else if (trimedText.StartsWith("this"))
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
