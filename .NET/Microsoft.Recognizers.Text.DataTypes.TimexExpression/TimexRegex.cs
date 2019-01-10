// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace Microsoft.Recognizers.Text.DataTypes.TimexExpression
{
    public static class TimexRegex
    {
        private static IDictionary<string, Regex[]> timexRegex = new Dictionary<string, Regex[]>
        {
            {
                "date", new Regex[]
                {
                    // date
                    new Regex(@"^(?<year>\d\d\d\d)-(?<month>\d\d)-(?<dayOfMonth>\d\d)$"),
                    new Regex(@"^XXXX-WXX-(?<dayOfWeek>\d)$"),
                    new Regex(@"^XXXX-(?<month>\d\d)-(?<dayOfMonth>\d\d)$"),

                    // daterange
                    new Regex(@"^(?<year>\d\d\d\d)$"),
                    new Regex(@"^(?<year>\d\d\d\d)-(?<month>\d\d)$"),
                    new Regex(@"^(?<season>SP|SU|FA|WI)$"),
                    new Regex(@"^(?<year>\d\d\d\d)-(?<season>SP|SU|FA|WI)$"),
                    new Regex(@"^(?<year>\d\d\d\d)-W(?<weekOfYear>\d\d)$"),
                    new Regex(@"^(?<year>\d\d\d\d)-W(?<weekOfYear>\d\d)-(?<weekend>WE)$"),
                    new Regex(@"^XXXX-(?<month>\d\d)$"),
                    new Regex(@"^XXXX-(?<month>\d\d)-W(?<weekOfMonth>\d\d)$"),
                    new Regex(@"^XXXX-(?<month>\d\d)-WXX-(?<weekOfMonth>\d)-(?<dayOfWeek>\d)$"),
                }
            },
            {
                "time", new Regex[]
                {
                    // time
                    new Regex(@"^T(?<hour>\d\d)$"),
                    new Regex(@"^T(?<hour>\d\d):(?<minute>\d\d)$"),
                    new Regex(@"^T(?<hour>\d\d):(?<minute>\d\d):(?<second>\d\d)$"),

                    // timerange
                    new Regex(@"^T(?<partOfDay>DT|NI|MO|AF|EV)$"),
                }
            },
            {
                "period", new Regex[]
                {
                    new Regex(@"^P(?<amount>\d*\.?\d+)(?<dateUnit>Y|M|W|D)$"),
                    new Regex(@"^PT(?<amount>\d*\.?\d+)(?<timeUnit>H|M|S)$"),
                }
            },
        };

        public static bool Extract(string name, string timex, IDictionary<string, string> result)
        {
            foreach (var entry in timexRegex[name])
            {
                if (TryExtract(entry, timex, result))
                {
                    return true;
                }
            }

            return false;
        }

        private static bool TryExtract(Regex regex, string timex, IDictionary<string, string> result)
        {
            var regexResult = regex.Match(timex);
            if (!regexResult.Success)
            {
                return false;
            }

            foreach (var groupName in regex.GetGroupNames())
            {
                result[groupName] = regexResult.Groups[groupName].Value;
            }

            return true;
        }
    }
}
