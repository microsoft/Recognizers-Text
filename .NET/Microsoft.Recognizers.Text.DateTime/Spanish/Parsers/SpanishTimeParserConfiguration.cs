// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Globalization;
using System.Text.RegularExpressions;

using Microsoft.Recognizers.Definitions.Spanish;
using Microsoft.Recognizers.Text.DateTime.Utilities;
using Microsoft.Recognizers.Text.Utilities;

namespace Microsoft.Recognizers.Text.DateTime.Spanish
{
    public class SpanishTimeParserConfiguration : BaseDateTimeOptionsConfiguration, ITimeParserConfiguration
    {
        private const RegexOptions RegexFlags = RegexOptions.Singleline | RegexOptions.ExplicitCapture;

        private static readonly Regex HalfTokenRegex =
            new Regex(DateTimeDefinitions.HalfTokenRegex, RegexFlags);

        private static readonly Regex QuarterTokenRegex =
            new Regex(DateTimeDefinitions.QuarterTokenRegex, RegexFlags);

        private static readonly Regex PastTokenRegex =
            new Regex(DateTimeDefinitions.PastTokenRegex, RegexFlags);

        private static readonly Regex ToTokenRegex =
            new Regex(DateTimeDefinitions.ToTokenRegex, RegexFlags);

        public SpanishTimeParserConfiguration(ICommonDateTimeParserConfiguration config)
            : base(config)
        {
            TimeTokenPrefix = DateTimeDefinitions.TimeTokenPrefix;
            AtRegex = SpanishTimeExtractorConfiguration.AtRegex;
            TimeRegexes = SpanishTimeExtractorConfiguration.TimeRegexList;
            UtilityConfiguration = config.UtilityConfiguration;
            Numbers = config.Numbers;
            TimeZoneParser = config.TimeZoneParser;
        }

        public string TimeTokenPrefix { get; }

        public Regex AtRegex { get; }

        public Regex MealTimeRegex { get; }

        public IEnumerable<Regex> TimeRegexes { get; }

        public IImmutableDictionary<string, int> Numbers { get; }

        public IDateTimeUtilityConfiguration UtilityConfiguration { get; }

        public IDateTimeParser TimeZoneParser { get; }

        public void AdjustByPrefix(string prefix, ref int hour, ref int min, ref bool hasMin)
        {
            var deltaMin = 0;
            var trimmedPrefix = prefix.Trim();

            if (QuarterTokenRegex.IsMatch(trimmedPrefix))
            {
                var match = QuarterTokenRegex.Match(trimmedPrefix);
                if (match.Groups[Constants.NegativeGroupName].Success)
                {
                    deltaMin = -15;
                }
                else
                {
                    deltaMin = 15;
                }
            }
            else if (HalfTokenRegex.IsMatch(trimmedPrefix))
            {
                deltaMin = 30;
            }
            else
            {
                var match = SpanishTimeExtractorConfiguration.LessThanOneHour.Match(trimmedPrefix);
                var minStr = match.Groups["deltamin"].Value;
                if (!string.IsNullOrWhiteSpace(minStr))
                {
                    deltaMin = int.Parse(minStr, CultureInfo.InvariantCulture);
                }
                else
                {
                    minStr = match.Groups["deltaminnum"].Value;
                    Numbers.TryGetValue(minStr, out deltaMin);
                }
            }

            if (ToTokenRegex.IsMatch(trimmedPrefix))
            {
                var match = ToTokenRegex.Match(trimmedPrefix);
                if (match.Groups[Constants.NegativeGroupName].Success)
                {
                    min = -min;
                }
                else
                {
                    deltaMin = -deltaMin;
                }
            }

            min += deltaMin;
            if (min < 0)
            {
                min += 60;
                hour -= 1;
            }

            hasMin = hasMin || min != 0;
        }

        public void AdjustBySuffix(string suffix, ref int hour, ref int min, ref bool hasMin, ref bool hasAm, ref bool hasPm)
        {
            var trimedSuffix = suffix.Trim();
            AdjustByPrefix(trimedSuffix, ref hour, ref min, ref hasMin);

            var deltaHour = 0;
            var match = SpanishTimeExtractorConfiguration.TimeSuffix.MatchExact(trimedSuffix, trim: true);

            if (match.Success)
            {
                var oclockStr = match.Groups["oclock"].Value;
                if (string.IsNullOrEmpty(oclockStr))
                {
                    var matchAmStr = match.Groups[Constants.AmGroupName].Value;
                    if (!string.IsNullOrEmpty(matchAmStr))
                    {
                        if (hour >= Constants.HalfDayHourCount)
                        {
                            deltaHour = -Constants.HalfDayHourCount;
                        }

                        hasAm = true;
                    }

                    var matchPmStr = match.Groups[Constants.PmGroupName].Value;
                    if (!string.IsNullOrEmpty(matchPmStr))
                    {
                        if (hour < Constants.HalfDayHourCount)
                        {
                            deltaHour = Constants.HalfDayHourCount;
                        }

                        hasPm = true;
                    }
                }
            }

            hour = (hour + deltaHour) % 24;
        }
    }
}
