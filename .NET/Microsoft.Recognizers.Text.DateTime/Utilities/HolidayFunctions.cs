using System.Collections.Generic;
using System.Collections.Immutable;
using System.Globalization;

using DateObject = System.DateTime;

namespace Microsoft.Recognizers.Text.DateTime
{
    internal static class HolidayFunctions
    {

        // Holi an Diwali dates { year, (holy_month, holy_day, diwali_month, diwali_day) }
        // @TODO move declarations to base DateTime or implement lunar calculation
        private static readonly IDictionary<int, IEnumerable<int>> HoliDiwaliRakshabandhanBaisakhiDates =
            Definitions.Hindi.DateTimeDefinitions.HoliDiwaliRakshabandhanBaisakhiDates.ToImmutableDictionary();

        public enum IslamicHolidayType
        {
            /// <summary>Ramadan</summary>
            Ramadan = 0,

            /// <summary>Eid al-Adha (Feast of the Sacrifice)</summary>
            Sacrifice,

            /// <summary>Eid al-Fitr (Festival of Breaking the Fast)</summary>
            EidAlFitr,

            /// <summary>Islamic New Year</summary>
            NewYear,
        }

        public static DateObject CalculateHolidayByEaster(int year, int days = 0)
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

        public static DateObject CalculateAdventDate(int year, int days = 0)
        {
            DateObject xmas = new DateObject(year, 12, 25);
            int weekday = (int)xmas.DayOfWeek;

            DateObject result;

            if (weekday == 0)
            {
                result = xmas.AddDays(-7 - days);
            }
            else
            {
                result = xmas.AddDays(-weekday - days);
            }

            return result;
        }

        // Holi and Diwali follow the lunar calendar
        // their dates have been included in the dictionary HoliDiwaliDates
        public static DateObject CalculateHoliDiwaliDate(int year, bool isHoli)
        {
            int day = 1;
            int month = 1;
            if (year >= 1900 && year < 2100)
            {
                var dates = HoliDiwaliRakshabandhanBaisakhiDates[year].ToImmutableList();
                if (isHoli)
                {
                    month = dates[0];
                    day = dates[1];
                }
                else
                {
                    month = dates[2];
                    day = dates[3];
                }
            }

            return DateObject.MinValue.SafeCreateFromValue(year, month, day);
        }

        // Rakshabandhan and Vaishakhi also follow the lunar calendar
        // their dates have been included in the dictionary HoliDiwaliDates
        public static DateObject CalculateRakshaBandhanVaishakhiDate(int year, bool isRakshabandhan)
        {
            int day = 1;
            int month = 1;
            if (year >= 1900 && year < 2100)
            {
                var dates = HoliDiwaliRakshabandhanBaisakhiDates[year].ToImmutableList();
                if (isRakshabandhan)
                {
                    month = dates[4];
                    day = dates[5];
                }
                else
                {
                    month = dates[6];
                    day = dates[7];
                }
            }

            return DateObject.MinValue.SafeCreateFromValue(year, month, day);
        }

        // Calculates the exact gregorian date for the given holiday using only gregorian year and exact hijri date
        public static DateObject IslamicHoliday(int year, IslamicHolidayType holidayType)
        {
            int y = 0;
            int m = 0;
            int d = 0;

            int hijriDay = 1;
            int hijriMonth = 1;
            int hijriYear = 1;

            var gregorian = new GregorianCalendar();
            var hijri = new HijriCalendar();

            switch (holidayType)
            {
                case IslamicHolidayType.Ramadan:
                    hijriDay = 1;
                    hijriMonth = 9;
                    break;
                case IslamicHolidayType.Sacrifice:
                    hijriDay = 10;
                    hijriMonth = 12;
                    break;
                case IslamicHolidayType.EidAlFitr:
                    hijriDay = 1;
                    hijriMonth = 10;
                    break;
                case IslamicHolidayType.NewYear:
                    hijriDay = 1;
                    hijriMonth = 1;
                    break;
            }

            for (hijriYear = 1; hijriYear <= 9999; hijriYear++)
            {
                var hijriDate = new DateObject(hijriYear, hijriMonth, hijriDay, hijri);
                y = gregorian.GetYear(hijriDate);
                m = gregorian.GetMonth(hijriDate);
                d = gregorian.GetDayOfMonth(hijriDate);

                if (y == year)
                {
                    break;
                }
            }

            return DateObject.MinValue.SafeCreateFromValue(y, m, d);
        }
    }
}
