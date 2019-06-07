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
            var trimmedText = text.Trim().ToLowerInvariant();
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
                .Replace(" ", string.Empty)
                .Replace("'", string.Empty);
        }

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

        private static DateObject EasterDay(int year) => GetEasterYearList().Contains(year) ? GetEasterDateList().Where(d => d.Year == year).First() : DateObject.MinValue;

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

        private static IList<int> GetEasterYearList() => GetEasterDateList().Select(d => d.Year).ToList();

        // easter sunday: 1900 ~ 2099, following the Paschal Full Moon (PFM) date of lunar year calendar.
        private static IList<DateObject> GetEasterDateList()
        {
            return new List<DateObject>
            {
                DateObject.Parse("1900-04-15", new CultureInfo("en-us")),
                DateObject.Parse("1901-04-07", new CultureInfo("en-us")),
                DateObject.Parse("1902-03-30", new CultureInfo("en-us")),
                DateObject.Parse("1903-04-12", new CultureInfo("en-us")),
                DateObject.Parse("1904-04-03", new CultureInfo("en-us")),
                DateObject.Parse("1905-04-23", new CultureInfo("en-us")),
                DateObject.Parse("1906-04-15", new CultureInfo("en-us")),
                DateObject.Parse("1907-03-31", new CultureInfo("en-us")),
                DateObject.Parse("1908-04-19", new CultureInfo("en-us")),
                DateObject.Parse("1909-04-11", new CultureInfo("en-us")),
                DateObject.Parse("1910-03-27", new CultureInfo("en-us")),
                DateObject.Parse("1911-04-16", new CultureInfo("en-us")),
                DateObject.Parse("1912-04-07", new CultureInfo("en-us")),
                DateObject.Parse("1913-03-23", new CultureInfo("en-us")),
                DateObject.Parse("1914-04-12", new CultureInfo("en-us")),
                DateObject.Parse("1915-04-04", new CultureInfo("en-us")),
                DateObject.Parse("1916-04-23", new CultureInfo("en-us")),
                DateObject.Parse("1917-04-08", new CultureInfo("en-us")),
                DateObject.Parse("1918-03-31", new CultureInfo("en-us")),
                DateObject.Parse("1919-04-20", new CultureInfo("en-us")),
                DateObject.Parse("1920-04-04", new CultureInfo("en-us")),
                DateObject.Parse("1921-03-27", new CultureInfo("en-us")),
                DateObject.Parse("1922-04-16", new CultureInfo("en-us")),
                DateObject.Parse("1923-04-01", new CultureInfo("en-us")),
                DateObject.Parse("1924-04-20", new CultureInfo("en-us")),
                DateObject.Parse("1925-04-12", new CultureInfo("en-us")),
                DateObject.Parse("1926-04-04", new CultureInfo("en-us")),
                DateObject.Parse("1927-04-17", new CultureInfo("en-us")),
                DateObject.Parse("1928-04-08", new CultureInfo("en-us")),
                DateObject.Parse("1929-03-31", new CultureInfo("en-us")),
                DateObject.Parse("1930-04-20", new CultureInfo("en-us")),
                DateObject.Parse("1931-04-05", new CultureInfo("en-us")),
                DateObject.Parse("1932-03-27", new CultureInfo("en-us")),
                DateObject.Parse("1933-04-16", new CultureInfo("en-us")),
                DateObject.Parse("1934-04-01", new CultureInfo("en-us")),
                DateObject.Parse("1935-04-21", new CultureInfo("en-us")),
                DateObject.Parse("1936-04-12", new CultureInfo("en-us")),
                DateObject.Parse("1937-03-28", new CultureInfo("en-us")),
                DateObject.Parse("1938-04-17", new CultureInfo("en-us")),
                DateObject.Parse("1939-04-09", new CultureInfo("en-us")),
                DateObject.Parse("1940-03-24", new CultureInfo("en-us")),
                DateObject.Parse("1941-04-13", new CultureInfo("en-us")),
                DateObject.Parse("1942-04-05", new CultureInfo("en-us")),
                DateObject.Parse("1943-04-25", new CultureInfo("en-us")),
                DateObject.Parse("1944-04-09", new CultureInfo("en-us")),
                DateObject.Parse("1945-04-01", new CultureInfo("en-us")),
                DateObject.Parse("1946-04-21", new CultureInfo("en-us")),
                DateObject.Parse("1947-04-06", new CultureInfo("en-us")),
                DateObject.Parse("1948-03-28", new CultureInfo("en-us")),
                DateObject.Parse("1949-04-17", new CultureInfo("en-us")),
                DateObject.Parse("1950-04-09", new CultureInfo("en-us")),
                DateObject.Parse("1951-03-25", new CultureInfo("en-us")),
                DateObject.Parse("1952-04-13", new CultureInfo("en-us")),
                DateObject.Parse("1953-04-05", new CultureInfo("en-us")),
                DateObject.Parse("1954-04-18", new CultureInfo("en-us")),
                DateObject.Parse("1955-04-10", new CultureInfo("en-us")),
                DateObject.Parse("1956-04-01", new CultureInfo("en-us")),
                DateObject.Parse("1957-04-21", new CultureInfo("en-us")),
                DateObject.Parse("1958-04-06", new CultureInfo("en-us")),
                DateObject.Parse("1959-03-29", new CultureInfo("en-us")),
                DateObject.Parse("1960-04-17", new CultureInfo("en-us")),
                DateObject.Parse("1961-04-02", new CultureInfo("en-us")),
                DateObject.Parse("1962-04-22", new CultureInfo("en-us")),
                DateObject.Parse("1963-04-14", new CultureInfo("en-us")),
                DateObject.Parse("1964-03-29", new CultureInfo("en-us")),
                DateObject.Parse("1965-04-18", new CultureInfo("en-us")),
                DateObject.Parse("1966-04-10", new CultureInfo("en-us")),
                DateObject.Parse("1967-03-26", new CultureInfo("en-us")),
                DateObject.Parse("1968-04-14", new CultureInfo("en-us")),
                DateObject.Parse("1969-04-06", new CultureInfo("en-us")),
                DateObject.Parse("1970-03-29", new CultureInfo("en-us")),
                DateObject.Parse("1971-04-11", new CultureInfo("en-us")),
                DateObject.Parse("1972-04-02", new CultureInfo("en-us")),
                DateObject.Parse("1973-04-22", new CultureInfo("en-us")),
                DateObject.Parse("1974-04-14", new CultureInfo("en-us")),
                DateObject.Parse("1975-03-30", new CultureInfo("en-us")),
                DateObject.Parse("1976-04-18", new CultureInfo("en-us")),
                DateObject.Parse("1977-04-10", new CultureInfo("en-us")),
                DateObject.Parse("1978-03-26", new CultureInfo("en-us")),
                DateObject.Parse("1979-04-15", new CultureInfo("en-us")),
                DateObject.Parse("1980-04-06", new CultureInfo("en-us")),
                DateObject.Parse("1981-04-19", new CultureInfo("en-us")),
                DateObject.Parse("1982-04-11", new CultureInfo("en-us")),
                DateObject.Parse("1983-04-03", new CultureInfo("en-us")),
                DateObject.Parse("1984-04-22", new CultureInfo("en-us")),
                DateObject.Parse("1985-04-07", new CultureInfo("en-us")),
                DateObject.Parse("1986-03-30", new CultureInfo("en-us")),
                DateObject.Parse("1987-04-19", new CultureInfo("en-us")),
                DateObject.Parse("1988-04-03", new CultureInfo("en-us")),
                DateObject.Parse("1989-03-26", new CultureInfo("en-us")),
                DateObject.Parse("1990-04-15", new CultureInfo("en-us")),
                DateObject.Parse("1991-03-31", new CultureInfo("en-us")),
                DateObject.Parse("1992-04-19", new CultureInfo("en-us")),
                DateObject.Parse("1993-04-11", new CultureInfo("en-us")),
                DateObject.Parse("1994-04-03", new CultureInfo("en-us")),
                DateObject.Parse("1995-04-16", new CultureInfo("en-us")),
                DateObject.Parse("1996-04-07", new CultureInfo("en-us")),
                DateObject.Parse("1997-03-30", new CultureInfo("en-us")),
                DateObject.Parse("1998-04-12", new CultureInfo("en-us")),
                DateObject.Parse("1999-04-04", new CultureInfo("en-us")),
                DateObject.Parse("2000-04-23", new CultureInfo("en-us")),
                DateObject.Parse("2001-04-15", new CultureInfo("en-us")),
                DateObject.Parse("2002-03-31", new CultureInfo("en-us")),
                DateObject.Parse("2003-04-20", new CultureInfo("en-us")),
                DateObject.Parse("2004-04-11", new CultureInfo("en-us")),
                DateObject.Parse("2005-03-27", new CultureInfo("en-us")),
                DateObject.Parse("2006-04-16", new CultureInfo("en-us")),
                DateObject.Parse("2007-04-08", new CultureInfo("en-us")),
                DateObject.Parse("2008-03-23", new CultureInfo("en-us")),
                DateObject.Parse("2009-04-12", new CultureInfo("en-us")),
                DateObject.Parse("2010-04-04", new CultureInfo("en-us")),
                DateObject.Parse("2011-04-24", new CultureInfo("en-us")),
                DateObject.Parse("2012-04-08", new CultureInfo("en-us")),
                DateObject.Parse("2013-03-31", new CultureInfo("en-us")),
                DateObject.Parse("2014-04-20", new CultureInfo("en-us")),
                DateObject.Parse("2015-04-05", new CultureInfo("en-us")),
                DateObject.Parse("2016-03-27", new CultureInfo("en-us")),
                DateObject.Parse("2017-04-16", new CultureInfo("en-us")),
                DateObject.Parse("2018-04-01", new CultureInfo("en-us")),
                DateObject.Parse("2019-04-21", new CultureInfo("en-us")),
                DateObject.Parse("2020-04-12", new CultureInfo("en-us")),
                DateObject.Parse("2021-04-04", new CultureInfo("en-us")),
                DateObject.Parse("2022-04-17", new CultureInfo("en-us")),
                DateObject.Parse("2023-04-09", new CultureInfo("en-us")),
                DateObject.Parse("2024-03-31", new CultureInfo("en-us")),
                DateObject.Parse("2025-04-20", new CultureInfo("en-us")),
                DateObject.Parse("2026-04-05", new CultureInfo("en-us")),
                DateObject.Parse("2027-03-28", new CultureInfo("en-us")),
                DateObject.Parse("2028-04-16", new CultureInfo("en-us")),
                DateObject.Parse("2029-04-01", new CultureInfo("en-us")),
                DateObject.Parse("2030-04-21", new CultureInfo("en-us")),
                DateObject.Parse("2031-04-13", new CultureInfo("en-us")),
                DateObject.Parse("2032-03-28", new CultureInfo("en-us")),
                DateObject.Parse("2033-04-17", new CultureInfo("en-us")),
                DateObject.Parse("2034-04-09", new CultureInfo("en-us")),
                DateObject.Parse("2035-03-25", new CultureInfo("en-us")),
                DateObject.Parse("2036-04-13", new CultureInfo("en-us")),
                DateObject.Parse("2037-04-05", new CultureInfo("en-us")),
                DateObject.Parse("2038-04-25", new CultureInfo("en-us")),
                DateObject.Parse("2039-04-10", new CultureInfo("en-us")),
                DateObject.Parse("2040-04-01", new CultureInfo("en-us")),
                DateObject.Parse("2041-04-21", new CultureInfo("en-us")),
                DateObject.Parse("2042-04-06", new CultureInfo("en-us")),
                DateObject.Parse("2043-03-29", new CultureInfo("en-us")),
                DateObject.Parse("2044-04-17", new CultureInfo("en-us")),
                DateObject.Parse("2045-04-09", new CultureInfo("en-us")),
                DateObject.Parse("2046-03-25", new CultureInfo("en-us")),
                DateObject.Parse("2047-04-14", new CultureInfo("en-us")),
                DateObject.Parse("2048-04-05", new CultureInfo("en-us")),
                DateObject.Parse("2049-04-18", new CultureInfo("en-us")),
                DateObject.Parse("2050-04-10", new CultureInfo("en-us")),
                DateObject.Parse("2051-04-02", new CultureInfo("en-us")),
                DateObject.Parse("2052-04-21", new CultureInfo("en-us")),
                DateObject.Parse("2053-04-06", new CultureInfo("en-us")),
                DateObject.Parse("2054-03-29", new CultureInfo("en-us")),
                DateObject.Parse("2055-04-18", new CultureInfo("en-us")),
                DateObject.Parse("2056-04-02", new CultureInfo("en-us")),
                DateObject.Parse("2057-04-22", new CultureInfo("en-us")),
                DateObject.Parse("2058-04-14", new CultureInfo("en-us")),
                DateObject.Parse("2059-03-30", new CultureInfo("en-us")),
                DateObject.Parse("2060-04-18", new CultureInfo("en-us")),
                DateObject.Parse("2061-04-10", new CultureInfo("en-us")),
                DateObject.Parse("2062-03-26", new CultureInfo("en-us")),
                DateObject.Parse("2063-04-15", new CultureInfo("en-us")),
                DateObject.Parse("2064-04-06", new CultureInfo("en-us")),
                DateObject.Parse("2065-03-29", new CultureInfo("en-us")),
                DateObject.Parse("2066-04-11", new CultureInfo("en-us")),
                DateObject.Parse("2067-04-03", new CultureInfo("en-us")),
                DateObject.Parse("2068-04-22", new CultureInfo("en-us")),
                DateObject.Parse("2069-04-14", new CultureInfo("en-us")),
                DateObject.Parse("2070-03-30", new CultureInfo("en-us")),
                DateObject.Parse("2071-04-19", new CultureInfo("en-us")),
                DateObject.Parse("2072-04-10", new CultureInfo("en-us")),
                DateObject.Parse("2073-03-26", new CultureInfo("en-us")),
                DateObject.Parse("2074-04-15", new CultureInfo("en-us")),
                DateObject.Parse("2075-04-07", new CultureInfo("en-us")),
                DateObject.Parse("2076-04-19", new CultureInfo("en-us")),
                DateObject.Parse("2077-04-11", new CultureInfo("en-us")),
                DateObject.Parse("2078-04-03", new CultureInfo("en-us")),
                DateObject.Parse("2079-04-23", new CultureInfo("en-us")),
                DateObject.Parse("2080-04-07", new CultureInfo("en-us")),
                DateObject.Parse("2081-03-30", new CultureInfo("en-us")),
                DateObject.Parse("2082-04-19", new CultureInfo("en-us")),
                DateObject.Parse("2083-04-04", new CultureInfo("en-us")),
                DateObject.Parse("2084-03-26", new CultureInfo("en-us")),
                DateObject.Parse("2085-04-15", new CultureInfo("en-us")),
                DateObject.Parse("2086-03-31", new CultureInfo("en-us")),
                DateObject.Parse("2087-04-20", new CultureInfo("en-us")),
                DateObject.Parse("2088-04-11", new CultureInfo("en-us")),
                DateObject.Parse("2089-04-03", new CultureInfo("en-us")),
                DateObject.Parse("2090-04-16", new CultureInfo("en-us")),
                DateObject.Parse("2091-04-08", new CultureInfo("en-us")),
                DateObject.Parse("2092-03-30", new CultureInfo("en-us")),
                DateObject.Parse("2093-04-12", new CultureInfo("en-us")),
                DateObject.Parse("2094-04-04", new CultureInfo("en-us")),
                DateObject.Parse("2095-04-24", new CultureInfo("en-us")),
                DateObject.Parse("2096-04-15", new CultureInfo("en-us")),
                DateObject.Parse("2097-03-31", new CultureInfo("en-us")),
                DateObject.Parse("2098-04-20", new CultureInfo("en-us")),
                DateObject.Parse("2099-04-12", new CultureInfo("en-us")),
            };
        }
    }
}
