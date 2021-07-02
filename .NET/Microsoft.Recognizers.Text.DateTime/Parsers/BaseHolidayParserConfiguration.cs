using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Text.RegularExpressions;
using Microsoft.Recognizers.Definitions;
using DateObject = System.DateTime;

namespace Microsoft.Recognizers.Text.DateTime
{
    public abstract class BaseHolidayParserConfiguration : BaseDateTimeOptionsConfiguration, IHolidayParserConfiguration
    {
        protected BaseHolidayParserConfiguration(IDateTimeOptionsConfiguration config)
                : base(config)
        {
            this.VariableHolidaysTimexDictionary = BaseDateTime.VariableHolidaysTimexDictionary.ToImmutableDictionary();
            this.HolidayFuncDictionary = InitHolidayFuncs().ToImmutableDictionary();
        }

        public IImmutableDictionary<string, string> VariableHolidaysTimexDictionary { get; protected set; }

        public IImmutableDictionary<string, Func<int, DateObject>> HolidayFuncDictionary { get; protected set; }

        public IImmutableDictionary<string, IEnumerable<string>> HolidayNames { get; protected set; }

        public IEnumerable<Regex> HolidayRegexList { get; protected set; }

        public abstract int GetSwiftYear(string text);

        public abstract string SanitizeHolidayToken(string holiday);

        // @TODO move all old holiday definitions to resource files
        protected static DateObject MothersDay(int year) => DateObject.MinValue.SafeCreateFromValue(year, 5, GetDay(year, 5, 1, DayOfWeek.Sunday));

        protected static DateObject FathersDay(int year) => DateObject.MinValue.SafeCreateFromValue(year, 6, GetDay(year, 6, 2, DayOfWeek.Sunday));

        protected static DateObject MemorialDay(int year) => DateObject.MinValue.SafeCreateFromValue(year, 5, GetLastDay(year, 5, DayOfWeek.Monday));

        protected static DateObject UsLabourDay(int year) => DateObject.MinValue.SafeCreateFromValue(year, 9, GetDay(year, 9, 0, DayOfWeek.Monday));

        protected static DateObject InternationalWorkersDay(int year) => new DateObject(year, 5, 1);

        protected static DateObject ColumbusDay(int year) => DateObject.MinValue.SafeCreateFromValue(year, 10, GetDay(year, 10, 1, DayOfWeek.Monday));

        protected static DateObject ThanksgivingDay(int year) => DateObject.MinValue.SafeCreateFromValue(year, 11, GetDay(year, 11, 3, DayOfWeek.Thursday));

        protected static DateObject BlackFriday(int year) => ThanksgivingDay(year).AddDays(1);

        protected static DateObject CyberMonday(int year) => ThanksgivingDay(year).AddDays(4);

        protected static int GetDay(int year, int month, int week, DayOfWeek dayOfWeek) =>
            (from day in Enumerable.Range(1, DateObject.DaysInMonth(year, month))
             where DateObject.MinValue.SafeCreateFromValue(year, month, day).DayOfWeek == dayOfWeek
             select day).ElementAt(week);

        protected static int GetLastDay(int year, int month, DayOfWeek dayOfWeek) =>
            (from day in Enumerable.Range(1, DateObject.DaysInMonth(year, month))
             where DateObject.MinValue.SafeCreateFromValue(year, month, day).DayOfWeek == dayOfWeek
             select day).Last();

        protected virtual IDictionary<string, Func<int, DateObject>> InitHolidayFuncs()
        {
            return new Dictionary<string, Func<int, DateObject>>
            {
                { "fathers", FathersDay },
                { "mothers", MothersDay },
                { "thanksgiving", ThanksgivingDay },
                { "blackfriday", BlackFriday },
                { "cybermonday", CyberMonday },
                { "martinlutherking", MartinLutherKingDay },
                { "washingtonsbirthday", WashingtonsBirthday },
                { "canberra", CanberraDay },
                { "labour", UsLabourDay },
                { "internationalworkers", InternationalWorkersDay },
                { "columbus", ColumbusDay },
                { "memorial", MemorialDay },
            };
        }

        private static DateObject MartinLutherKingDay(int year) => DateObject.MinValue.SafeCreateFromValue(year, 1, GetDay(year, 1, 2, DayOfWeek.Monday));

        private static DateObject WashingtonsBirthday(int year) => DateObject.MinValue.SafeCreateFromValue(year, 2, GetDay(year, 2, 2, DayOfWeek.Monday));

        private static DateObject CanberraDay(int year) => DateObject.MinValue.SafeCreateFromValue(year, 3, GetDay(year, 3, 0, DayOfWeek.Monday));
    }
}
