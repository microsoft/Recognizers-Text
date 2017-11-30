using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using DateObject = System.DateTime;

namespace Microsoft.Recognizers.Text.DateTime
{
    class DurationParsingUtil
    {
        public static bool isTimeDuration(string unitStr)
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

            return (dict.Count > 1);
        }

        public static DateObject ShiftDateTime(string timex, DateObject referenceDateTime, bool future)
        {
            var timexUnitMap = ResolveDurationTimex(timex);
            var ret = GetShiftResult(timexUnitMap, referenceDateTime, future);
            return ret;
        }
        private static DateObject GetShiftResult(IImmutableDictionary<string, double> timexUnitMap, System.DateTime referenceDate, bool future)
        {
            System.DateTime ret = referenceDate;
            int futureOrPast = future ? 1 : -1;

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
                    default:
                        return ret;
                }
            }

            return ret;
        }

        private static ImmutableDictionary<string, double> ResolveDurationTimex(string timexStr)
        {
            var ret = new Dictionary<string, double>();
            // resolve duration timex, such as P21DT2H(21 days 2 hours)
            var durationStr = timexStr.Replace("P", "");
            var numberStart = 0;
            var isTime = false;
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
                        double number = 0;
                        if (!double.TryParse(numStr, out number))
                        {
                            return (new Dictionary<string, double>()).ToImmutableDictionary();
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
