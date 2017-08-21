using System;

namespace Microsoft.Recognizers.Text.DateTime
{
    public static class DateObjectExtension
    {
        public static System.DateTime Next(this System.DateTime from, DayOfWeek dayOfWeek)
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

        public static System.DateTime This(this System.DateTime from, DayOfWeek dayOfWeek)
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

        public static System.DateTime Last(this System.DateTime from, DayOfWeek dayOfWeek)
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
    }
}