using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Text.RegularExpressions;
using DateObject = System.DateTime;

namespace Microsoft.Recognizers.Text.DateTime.Parsers
{
    public abstract class BaseHolidayParserConfiguration : IHolidayParserConfiguration
    {
        public IImmutableDictionary<string, string> VariableHolidaysTimexDictionary { get; protected set; }

        public IImmutableDictionary<string, Func<int, DateObject>> HolidayFuncDictionary { get; protected set; }

        public IImmutableDictionary<string, IEnumerable<string>> HolidayNames { get; protected set; }

        public IEnumerable<Regex> HolidayRegexList { get; protected set; }

        protected BaseHolidayParserConfiguration()
        {
            this.VariableHolidaysTimexDictionary = InitVariableHolidaysTimex().ToImmutableDictionary();
            this.HolidayFuncDictionary = InitHolidayFuncs().ToImmutableDictionary();
        }

        protected virtual IDictionary<string, Func<int, DateObject>> InitHolidayFuncs()
        {
            return new Dictionary<string, Func<int, DateObject>>
            {
                {"fathers", FathersDay},
                {"mothers", MothersDay},
                {"thanksgivingday", ThanksgivingDay},
                {"thanksgiving", ThanksgivingDay},
                {"martinlutherking", MartinLutherKingDay},
                {"washingtonsbirthday", WashingtonsBirthday},
                {"canberra", CanberraDay},
                {"labour", LabourDay},
                {"columbus", ColumbusDay},
                {"memorial", MemorialDay}
            };
        }

        protected virtual IDictionary<string, string> InitVariableHolidaysTimex()
        {
            return new Dictionary<string, string>
            {
                {"fathers", @"-06-WXX-6-3"},
                {"mothers", @"-05-WXX-7-2"},
                {"thanksgiving", @"-11-WXX-4-4"},
                {"martinlutherking", @"-01-WXX-1-3"},
                {"washingtonsbirthday", @"-02-WXX-1-3"},
                {"canberra", @"-03-WXX-1-1"},
                {"labour", @"-09-WXX-1-1"},
                {"columbus", @"-10-WXX-1-2"},
                {"memorial", @"-05-WXX-1-4"}
            };
        }

        public abstract int GetSwiftYear(string text);

        public abstract string SanitizeHolidayToken(string holiday);

        private static DateObject MothersDay(int year) => new DateObject(year, 5, GetDay(year, 5, 1, DayOfWeek.Sunday));

        private static DateObject FathersDay(int year) => new DateObject(year, 6, GetDay(year, 6, 2, DayOfWeek.Sunday));

        private static DateObject MartinLutherKingDay(int year) => new DateObject(year, 1, GetDay(year, 1, 2, DayOfWeek.Monday));

        private static DateObject WashingtonsBirthday(int year) => new DateObject(year, 2, GetDay(year, 2, 2, DayOfWeek.Monday));

        private static DateObject CanberraDay(int year) => new DateObject(year, 3, GetDay(year, 3, 0, DayOfWeek.Monday));

        private static DateObject MemorialDay(int year) => new DateObject(year, 5, GetLastDay(year, 5, DayOfWeek.Monday));

        private static DateObject LabourDay(int year) => new DateObject(year, 9, GetDay(year, 9, 0, DayOfWeek.Monday));

        private static DateObject ColumbusDay(int year) => new DateObject(year, 10, GetDay(year, 10, 1, DayOfWeek.Monday));

        private static DateObject ThanksgivingDay(int year) => new DateObject(year, 11, GetDay(year, 11, 3, DayOfWeek.Thursday));

        protected static int GetDay(int year, int month, int week, DayOfWeek dayOfWeek) =>
            (from day in Enumerable.Range(1, DateObject.DaysInMonth(year, month))
             where new DateObject(year, month, day).DayOfWeek == dayOfWeek
             select day).ElementAt(week);

        protected static int GetLastDay(int year, int month, DayOfWeek dayOfWeek) =>
            (from day in Enumerable.Range(1, DateObject.DaysInMonth(year, month))
             where new DateObject(year, month, day).DayOfWeek == dayOfWeek
             select day).Last();
    }
}
