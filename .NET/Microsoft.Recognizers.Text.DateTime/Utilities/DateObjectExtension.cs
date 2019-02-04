using System;
using DateObject = System.DateTime;

namespace Microsoft.Recognizers.Text.DateTime
{
    public static class DateObjectExtension
    {
        public static DateObject Next(this DateObject from, DayOfWeek dayOfWeek)
        {
            var start = (int)from.DayOfWeek;
            var target = (int)dayOfWeek;

            if (start == 0)
            {
                start = 7;
            }

            if (target == 0)
            {
                target = 7;
            }

            return from.AddDays(target - start + 7);
        }

        public static DateObject Upcoming(this DateObject from, DayOfWeek dayOfWeek)
        {
            var start = (int)from.DayOfWeek;
            var target = (int)dayOfWeek;

            if (start == 0)
            {
                start = 7;
            }

            if (target == 0)
            {
                target = 7;
            }

            if (start < target)
            {
                return This(from, dayOfWeek);
            }
            else
            {
                return Next(from, dayOfWeek);
            }
        }

        public static DateObject This(this DateObject from, DayOfWeek dayOfWeek)
        {
            var start = (int)from.DayOfWeek;
            var target = (int)dayOfWeek;

            if (start == 0)
            {
                start = 7;
            }

            if (target == 0)
            {
                target = 7;
            }

            return from.AddDays(target - start);
        }

        public static DateObject Last(this DateObject from, DayOfWeek dayOfWeek)
        {
            var start = (int)from.DayOfWeek;
            var target = (int)dayOfWeek;

            if (start == 0)
            {
                start = 7;
            }

            if (target == 0)
            {
                target = 7;
            }

            return from.AddDays(target - start - 7);
        }

        public static DateObject Past(this DateObject from, DayOfWeek dayOfWeek)
        {
            var start = (int)from.DayOfWeek;
            var target = (int)dayOfWeek;

            if (start == 0)
            {
                start = 7;
            }

            if (target == 0)
            {
                target = 7;
            }

            if (start > target)
            {
                return This(from, dayOfWeek);
            }
            else
            {
                return Last(from, dayOfWeek);
            }
        }

        public static DateObject SafeCreateFromValue(this DateObject datetime, int year, int month, int day)
        {
            if (IsValidDate(year, month, day))
            {
                datetime = datetime.AddYears(year - datetime.Year);
                datetime = datetime.AddMonths(month - datetime.Month);
                datetime = datetime.AddDays(day - datetime.Day);
            }

            return datetime;
        }

        public static DateObject SafeCreateFromValue(this DateObject datetime, int year, int month, int day, int hour, int minute, int second)
        {
            if (IsValidDate(year, month, day) && IsValidTime(hour, minute, second))
            {
                datetime = datetime.SafeCreateFromValue(year, month, day);
                datetime = datetime.AddHours(hour - datetime.Hour);
                datetime = datetime.AddMinutes(minute - datetime.Minute);
                datetime = datetime.AddSeconds(second - datetime.Second);
            }

            return datetime;
        }

        public static bool IsValidDate(int year, int month, int day)
        {
            if (year < 1 || year > 9999)
            {
                return false;
            }

            int[] validDays =
            {
                31,
                (year % 4 == 0 && year % 100 != 0) || year % 400 == 0 ? 29 : 28,
                31,
                30,
                31,
                30,
                31,
                31,
                30,
                31,
                30,
                31,
            };

            return month >= 1 && month <= 12 && day >= 1 && day <= validDays[month - 1];
        }

        public static bool IsValidTime(int hour, int minute, int second)
        {
            return hour >= 0 && hour <= 23 && minute >= 0 && minute <= 59 && second >= 0 && second <= 59;
        }

        public static bool IsDefaultValue(this DateObject datetime)
        {
            return datetime == default(DateObject);
        }
    }
}