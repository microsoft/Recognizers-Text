// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System.Collections.Generic;

namespace Microsoft.Recognizers.DataTypes.DateTime
{
    public static class TimexParsing
    {
        public static void ParseString(string timex, Timex obj)
        {
            // a reference to the present
            if (timex == "PRESENT_REF")
            {
                obj.Now = true;
            }
            // duration
            else if (timex.StartsWith("P"))
            {
                ExtractDuration(timex, obj);
            }
            // range indicated with start and end dates and a duration
            else if (timex.StartsWith("(") && timex.EndsWith(")"))
            {
                ExtractStartEndRange(timex, obj);
            }
            // date and time and their respective ranges
            else
            {
                ExtractDateTime(timex, obj);
            }
        }

        private static void ExtractDuration(string s, Timex obj)
        {
            var extracted = new Dictionary<string, string>();
            TimexRegex.Extract("period", s, extracted);
            obj.AssignProperties(extracted);
        }

        private static void ExtractStartEndRange(string s, Timex obj)
        {
            var parts = s.Substring(1, s.Length - 2).Split(',');

            if (parts.Length == 3)
            {
                ExtractDateTime(parts[0], obj);
                ExtractDuration(parts[2], obj);
            }
        }

        private static void ExtractDateTime(string s, Timex obj)
        {
            var indexOfT = s.IndexOf('T');

            if (indexOfT == -1)
            {
                var extracted = new Dictionary<string, string>();
                TimexRegex.Extract("date", s, extracted);
                obj.AssignProperties(extracted);
            }
            else
            {
                var extracted = new Dictionary<string, string>();
                TimexRegex.Extract("date", s.Substring(0, indexOfT), extracted);
                TimexRegex.Extract("time", s.Substring(indexOfT), extracted);
                obj.AssignProperties(extracted);
            }
        }

    }
}
