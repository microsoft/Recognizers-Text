using System.Collections.Generic;
using System.Collections.Immutable;

using DateObject = System.DateTime;

namespace Microsoft.Recognizers.Text.DateTime
{
    internal static class HolidayFunctions
    {

        // Holi an Diwali dates { year, (holy_month, holy_day, diwali_month, diwali_day) }
        // @TODO move declarations to base DateTime or implement lunar calculation
        private static readonly IDictionary<int, IEnumerable<int>> HoliDiwaliDates =
            Definitions.Hindi.DateTimeDefinitions.HoliDiwaliDates.ToImmutableDictionary();

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
                var dates = HoliDiwaliDates[year].ToImmutableList();
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

    }
}
