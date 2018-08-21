using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using DateObject = System.DateTime;

namespace Microsoft.Recognizers.Text.DateTime
{
    internal class DurationParsingUtil
    {
        public static bool IsTimeDurationUnit(string unitStr)
        {
            var ret = false;
            switch (unitStr)
            {
                case "H":
                    ret = true;
                    break;
                case "M":
                    ret = true;
                    break;
                case "S":
                    ret = true;
                    break;
            }

            return ret;
        }

        public static bool IsMultipleDuration(string timex)
        {
            var dict = ResolveDurationTimex(timex);

            return dict.Count > 1;
        }

        public static bool IsDateDuration(string timex)
        {
            var dict = ResolveDurationTimex(timex);

            foreach (var unit in dict.Keys)
            {
                if (IsTimeDurationUnit(unit))
                {
                    return false;
                }
            }

            return true;
        }

        public static DateObject ShiftDateTime(string timex, DateObject referenceDateTime, bool future)
        {
            var timexUnitMap = ResolveDurationTimex(timex);
            var ret = GetShiftResult(timexUnitMap, referenceDateTime, future);
            return ret;
        }

        private static DateObject GetShiftResult(IImmutableDictionary<string, double> timexUnitMap, DateObject referenceDate, bool future)
        {
            var ret = referenceDate;
            var futureOrPast = future ? 1 : -1;

            foreach (var pair in timexUnitMap)
            {
                var unitStr = pair.Key;
                var number = pair.Value;
                switch (unitStr)
                {
                    case "H":
                        ret = ret.AddHours(number * futureOrPast);
                        break;
                    case "M":
                        ret = ret.AddMinutes(number * futureOrPast);
                        break;
                    case "S":
                        ret = ret.AddSeconds(number * futureOrPast);
                        break;
                    case "D":
                        ret = ret.AddDays(number * futureOrPast);
                        break;
                    case "W":
                        ret = ret.AddDays(7 * number * futureOrPast);
                        break;
                    case "MON":
                        ret = ret.AddMonths(Convert.ToInt32(number) * futureOrPast);
                        break;
                    case "Y":
                        ret = ret.AddYears(Convert.ToInt32(number) * futureOrPast);
                        break;
                    case "BD":
                        ret = GetNthBusinessDay(ret, Convert.ToInt32(number), future, out _);
                        break;
                    default:
                        return ret;
                }
            }

            return ret;
        }

        public static DateObject GetNthBusinessDay(DateObject startDate, int n, bool isFuture, out List<DateObject> dateList)
        {
            var date = startDate;
            dateList = new List<DateObject> {date};

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

        private static ImmutableDictionary<string, double> ResolveDurationTimex(string timexStr)
        {
            var ret = new Dictionary<string, double>();

            // Resolve duration timex, such as P21DT2H(21 days 2 hours)
            var durationStr = timexStr.Replace("P", "");
            var numberStart = 0;
            var isTime = false;

            // Resolve business days
            if (durationStr.EndsWith("BD"))
            {
                if (double.TryParse(durationStr.Substring(0, durationStr.Length - 2), out var numVal))
                {
                    ret.Add("BD", numVal);
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
                        if (!isTime && srcTimexUnit == "M")
                        {
                            srcTimexUnit = "MON";
                        }

                        ret.Add(srcTimexUnit, number);
                    }

                    numberStart = idx + 1;
                }
            }

            return ret.ToImmutableDictionary();
        }

        public enum MultiDurationType
        {
            Date = 0,
            Time,
            DateTime,
        }
    }
}
