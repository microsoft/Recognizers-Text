// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System;
using System.Text.RegularExpressions;

namespace Microsoft.Recognizers.Text.DateTime.Utilities
{
    public static class SetHandler
    {

        public static Tuple<string, int> WeekDayGroupMatchTuple(Match match)
        {
            string weekday = match.Groups["weekday"].ToString();
            int del = 1;

            return Tuple.Create<string, int>(weekday, del);
        }

        public static string WeekDayGroupMatchString(Match match)
        {
            string weekday = match.Groups["weekday"].ToString();

            return weekday;
        }

        public static DateTimeResolutionResult ResolveSet(ref DateTimeResolutionResult result, string innerTimex)
        {
            result.Timex = innerTimex;
            result.FutureValue = result.PastValue = "Set: " + innerTimex;
            result.Success = true;

            return result;
        }

    }
}
