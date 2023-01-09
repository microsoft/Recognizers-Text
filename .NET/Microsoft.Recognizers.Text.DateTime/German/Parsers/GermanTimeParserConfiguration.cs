// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Globalization;
using System.Text.RegularExpressions;

using Microsoft.Recognizers.Definitions.German;
using Microsoft.Recognizers.Text.DateTime.Utilities;
using Microsoft.Recognizers.Text.Utilities;

namespace Microsoft.Recognizers.Text.DateTime.German
{
    public class GermanTimeParserConfiguration : BaseDateTimeOptionsConfiguration, ITimeParserConfiguration
    {

        private const RegexOptions RegexFlags = RegexOptions.Singleline | RegexOptions.ExplicitCapture;

        private static readonly Regex TimeSuffixFull =
            new Regex(DateTimeDefinitions.TimeSuffixFull, RegexFlags);

        private static readonly Regex LunchRegex =
            new Regex(DateTimeDefinitions.LunchRegex, RegexFlags);

        private static readonly Regex NightRegex =
            new Regex(DateTimeDefinitions.NightRegex, RegexFlags);

        private static readonly Regex HalfTokenRegex =
            new Regex(DateTimeDefinitions.HalfTokenRegex, RegexFlags);

        private static readonly Regex QuarterToTokenRegex =
            new Regex(DateTimeDefinitions.QuarterToTokenRegex, RegexFlags);

        private static readonly Regex QuarterPastTokenRegex =
            new Regex(DateTimeDefinitions.QuarterPastTokenRegex, RegexFlags);

        private static readonly Regex ThreeQuarterToTokenRegex =
            new Regex(DateTimeDefinitions.ThreeQuarterToTokenRegex, RegexFlags);

        private static readonly Regex ThreeQuarterPastTokenRegex =
            new Regex(DateTimeDefinitions.ThreeQuarterPastTokenRegex, RegexFlags);

        public GermanTimeParserConfiguration(ICommonDateTimeParserConfiguration config)
            : base(config)
        {
            TimeTokenPrefix = DateTimeDefinitions.TimeTokenPrefix;
            AtRegex = GermanTimeExtractorConfiguration.AtRegex;
            TimeRegexes = GermanTimeExtractorConfiguration.TimeRegexList;
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

            if (HalfTokenRegex.IsMatch(trimmedPrefix))
            {
                deltaMin = -30;
            }
            else if (QuarterToTokenRegex.IsMatch(trimmedPrefix))
            {
                deltaMin = -15;
            }
            else if (QuarterPastTokenRegex.IsMatch(trimmedPrefix))
            {
                deltaMin = 15;
            }
            else if (ThreeQuarterToTokenRegex.IsMatch(trimmedPrefix))
            {
                deltaMin = -45;
            }
            else if (ThreeQuarterPastTokenRegex.IsMatch(trimmedPrefix))
            {
                deltaMin = 45;
            }
            else
            {
                var match = GermanTimeExtractorConfiguration.LessThanOneHour.Match(trimmedPrefix);
                var minStr = match.Groups["deltamin"].Value;
                if (!string.IsNullOrWhiteSpace(minStr))
                {
                    deltaMin = int.Parse(minStr, CultureInfo.InvariantCulture);
                }
                else
                {
                    minStr = match.Groups["deltaminnum"].Value;
                    deltaMin = Numbers[minStr];
                }

                // @TODO move hardcoded values to resources file

                if (trimmedPrefix.EndsWith("vor", StringComparison.Ordinal))
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

            hasMin = true;
        }

        public void AdjustBySuffix(string suffix, ref int hour, ref int min, ref bool hasMin, ref bool hasAm, ref bool hasPm)
        {

            var deltaHour = 0;
            var match = TimeSuffixFull.MatchExact(suffix, trim: true);

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
                        else
                        {
                            hasAm = true;
                        }
                    }

                    var matchPmStr = match.Groups[Constants.PmGroupName].Value;
                    if (!string.IsNullOrEmpty(matchPmStr))
                    {
                        if (hour < Constants.HalfDayHourCount)
                        {
                            deltaHour = Constants.HalfDayHourCount;
                        }

                        if (LunchRegex.IsMatch(matchPmStr))
                        {
                            // for hour>=10, <12
                            if (hour >= 10 && hour <= Constants.HalfDayHourCount)
                            {
                                deltaHour = 0;
                                if (hour == Constants.HalfDayHourCount)
                                {
                                    hasPm = true;
                                }
                                else
                                {
                                    hasAm = true;
                                }
                            }
                            else
                            {
                                hasPm = true;
                            }
                        }
                        else if (NightRegex.IsMatch(matchPmStr))
                        {
                            // For hour <=3 or ==12, we treat it as am, for example 1 in the night (midnight) == 1am
                            if (hour <= 3 || hour == Constants.HalfDayHourCount)
                            {
                                if (hour == Constants.HalfDayHourCount)
                                {
                                    hour = 0;
                                }

                                deltaHour = 0;
                                hasAm = true;
                            }
                            else
                            {
                                hasPm = true;
                            }
                        }
                        else
                        {
                            hasPm = true;
                        }
                    }
                }
            }

            hour = (hour + deltaHour) % 24;
        }
    }
}
