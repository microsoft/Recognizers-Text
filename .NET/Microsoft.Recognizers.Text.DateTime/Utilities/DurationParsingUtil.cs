using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;

using DateObject = System.DateTime;

namespace Microsoft.Recognizers.Text.DateTime
{
    internal class DurationParsingUtil
    {
        /// <summary>
        /// Represents multi duration.
        /// </summary>
        public enum MultiDurationType
        {
            /// <summary>
            /// Represents a date
            /// </summary>
            Date = 0,

            /// <summary>
            /// Represents a time
            /// </summary>
            Time,

            /// <summary>
            /// Represents the time of the date
            /// </summary>
            DateTime,
        }

        public static bool IsTimeDurationUnit(string unitStr)
        {
            bool result;

            switch (unitStr)
            {
                case "H":
                    result = true;
                    break;
                case "M":
                    result = true;
                    break;
                case "S":
                    result = true;
                    break;
                default:
                    result = false;
                    break;
            }

            return result;
        }

        public static bool IsMultipleDuration(string timex)
        {
            var dict = ResolveDurationTimex(timex);

            return dict.Count > 1;
        }

        public static bool IsDateDuration(string timex)
        {
            var dict = ResolveDurationTimex(timex);

            return dict.Keys.All(unit => !IsTimeDurationUnit(unit));
        }

        public static DateObject ShiftDateTime(string timex, DateObject referenceDateTime, bool future)
        {
            var timexUnitMap = ResolveDurationTimex(timex);
            var result = GetShiftResult(timexUnitMap, referenceDateTime, future);
            return result;
        }

        public static DateObject GetNthBusinessDay(DateObject startDate, int n, bool isFuture, out List<DateObject> dateList)
        {
            var date = startDate;
            dateList = new List<DateObject> { date };

            for (var i = 0; i < n; i++)
            {
                date = GetNextBusinessDay(date, isFuture);
                dateList.Add(date);
            }

            if (!isFuture)
            {
                dateList.Reverse();
            }

            return date;
        }

        // By design it currently does not take holidays into account
        public static DateObject GetNextBusinessDay(DateObject startDate, bool isFuture = true)
        {
            var dateIncrement = isFuture ? 1 : -1;
            var date = startDate.AddDays(dateIncrement);

            while (date.DayOfWeek == DayOfWeek.Saturday || date.DayOfWeek == DayOfWeek.Sunday)
            {
                date = date.AddDays(dateIncrement);
            }

            return date;
        }

        private static DateObject GetShiftResult(IImmutableDictionary<string, double> timexUnitMap, DateObject referenceDate, bool future)
        {
            var result = referenceDate;
            var futureOrPast = future ? 1 : -1;

            foreach (var pair in timexUnitMap)
            {
                var unitStr = pair.Key;
                var number = pair.Value;
                switch (unitStr)
                {
                    case "H":
                        result = result.AddHours(number * futureOrPast);
                        break;
                    case "M":
                        result = result.AddMinutes(number * futureOrPast);
                        break;
                    case "S":
                        result = result.AddSeconds(number * futureOrPast);
                        break;
                    case Constants.TimexDay:
                        result = result.AddDays(number * futureOrPast);
                        break;
                    case Constants.TimexWeek:
                        result = result.AddDays(7 * number * futureOrPast);
                        break;
                    case Constants.TimexMonthFull:
                        result = result.AddMonths(Convert.ToInt32(number) * futureOrPast);
                        break;
                    case Constants.TimexYear:
                        result = result.AddYears(Convert.ToInt32(number) * futureOrPast);
                        break;
                    case Constants.TimexBusinessDay:
                        result = GetNthBusinessDay(result, Convert.ToInt32(number), future, out _);
                        break;
                    default:
                        return result;
                }
            }

            return result;
        }

        private static ImmutableDictionary<string, double> ResolveDurationTimex(string timexStr)
        {
            var ret = new Dictionary<string, double>();

            // Resolve duration timex, such as P21DT2H (21 days 2 hours)
            var durationStr = timexStr.Replace(Constants.GeneralPeriodPrefix, string.Empty);
            var numberStart = 0;
            var isTime = false;

            // Resolve business days
            if (durationStr.EndsWith(Constants.TimexBusinessDay))
            {
                if (double.TryParse(durationStr.Substring(0, durationStr.Length - 2), out var numVal))
                {
                    ret.Add(Constants.TimexBusinessDay, numVal);
                }

                return ret.ToImmutableDictionary();
            }

            for (var idx = 0; idx < durationStr.Length; idx++)
            {
                if (char.IsLetter(durationStr[idx]))
                {
                    if (durationStr[idx] == 'T')
                    {
                        isTime = true;
                    }
                    else
                    {
                        var numStr = durationStr.Substring(numberStart, idx - numberStart);
                        if (!double.TryParse(numStr, out var number))
                        {
                            return new Dictionary<string, double>().ToImmutableDictionary();
                        }

                        var srcTimexUnit = durationStr.Substring(idx, 1);
                        if (!isTime && srcTimexUnit == Constants.TimexMonth)
                        {
                            srcTimexUnit = Constants.TimexMonthFull;
                        }

                        ret.Add(srcTimexUnit, number);
                    }

                    numberStart = idx + 1;
                }
            }

            return ret.ToImmutableDictionary();
        }
    }
}
