﻿// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System.Collections.Generic;

namespace Microsoft.Recognizers.Text.DataTypes.TimexExpression
{
    public static class TimexParsing
    {
        public static void ParseString(string timex, TimexProperty timexProperty)
        {
            // a reference to the present
            if (timex == "PRESENT_REF")
            {
                timexProperty.Now = true;
            }
            else if (timex.StartsWith("P", System.StringComparison.Ordinal))
            { // duration
                ExtractDuration(timex, timexProperty);
            }
            else if (timex.StartsWith("(", System.StringComparison.Ordinal) && timex.EndsWith(")", System.StringComparison.Ordinal))
            { // range indicated with start and end dates and a duration
                ExtractStartEndRange(timex, timexProperty);
            }
            else
            { // date and time and their respective ranges
                ExtractDateTime(timex, timexProperty);
            }
        }

        private static void ExtractDuration(string s, TimexProperty timexProperty)
        {
            var extracted = new Dictionary<string, string>();
            TimexRegex.Extract("period", s, extracted);
            timexProperty.AssignProperties(extracted);
        }

        private static void ExtractStartEndRange(string s, TimexProperty timexProperty)
        {
            var parts = s.Substring(1, s.Length - 2).Split(',');

            if (parts.Length == 3)
            {
                ExtractDateTime(parts[0], timexProperty);
                ExtractDuration(parts[2], timexProperty);
            }
        }

        private static void ExtractDateTime(string s, TimexProperty timexProperty)
        {
            var indexOfT = s.IndexOf('T');
            var indexOfP = s.IndexOf('P');

            // Spring timex value has a P in it, but should not be mixed up with
            // the "period" types that have P in them
            var indexOfSP = s.IndexOf("SP");

            if (indexOfT == -1)
            {
                var extracted = new Dictionary<string, string>();
                if (indexOfSP > -1 || indexOfP == -1)
                {
                    TimexRegex.Extract("date", s, extracted);
                }
                else
                {
                    TimexRegex.Extract("date", s.Substring(0, indexOfP), extracted);
                }

                timexProperty.AssignProperties(extracted);
            }
            else
            {
                var extracted = new Dictionary<string, string>();
                TimexRegex.Extract("date", s.Substring(0, indexOfT), extracted);
                if (indexOfP == -1)
                {
                    TimexRegex.Extract("time", s.Substring(indexOfT), extracted);
                }
                else
                {
                    TimexRegex.Extract("time", s.Substring(indexOfT, indexOfP - indexOfT), extracted);
                }

                timexProperty.AssignProperties(extracted);
            }
        }
    }
}
