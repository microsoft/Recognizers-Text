using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Globalization;
using System.Text.RegularExpressions;

using Microsoft.Recognizers.Definitions.Portuguese;
using Microsoft.Recognizers.Text.DateTime.Utilities;
using Microsoft.Recognizers.Text.Utilities;

namespace Microsoft.Recognizers.Text.DateTime.Portuguese
{
    public class PortugueseTimeParserConfiguration : BaseDateTimeOptionsConfiguration, ITimeParserConfiguration
    {
        public PortugueseTimeParserConfiguration(ICommonDateTimeParserConfiguration config)
            : base(config)
        {
            TimeTokenPrefix = DateTimeDefinitions.TimeTokenPrefix;
            AtRegex = PortugueseTimeExtractorConfiguration.AtRegex;
            TimeRegexes = PortugueseTimeExtractorConfiguration.TimeRegexList;
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

            // @TODO move hardcoded values to resources file
            if (trimmedPrefix.StartsWith("quarto", StringComparison.Ordinal) || trimmedPrefix.StartsWith("e um quarto", StringComparison.Ordinal) ||
                trimmedPrefix.StartsWith("quinze", StringComparison.Ordinal) || trimmedPrefix.StartsWith("e quinze", StringComparison.Ordinal))
            {
                deltaMin = 15;
            }
            else if (trimmedPrefix.StartsWith("menos um quarto", StringComparison.Ordinal))
            {
                deltaMin = -15;
            }
            else if (trimmedPrefix.StartsWith("meia", StringComparison.Ordinal) || trimmedPrefix.StartsWith("e meia", StringComparison.Ordinal))
            {
                deltaMin = 30;
            }
            else
            {
                var match = PortugueseTimeExtractorConfiguration.LessThanOneHour.Match(trimmedPrefix);
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

            if (trimmedPrefix.EndsWith("passadas", StringComparison.Ordinal) || trimmedPrefix.EndsWith("pasados", StringComparison.Ordinal) ||
                trimmedPrefix.EndsWith("depois das", StringComparison.Ordinal) || trimmedPrefix.EndsWith("depois da", StringComparison.Ordinal) || trimmedPrefix.EndsWith("depois do", StringComparison.Ordinal) ||
                trimmedPrefix.EndsWith("passadas as", StringComparison.Ordinal) || trimmedPrefix.EndsWith("passadas das", StringComparison.Ordinal))
            {
                // deltaMin it's positive
            }
            else if (trimmedPrefix.EndsWith("para a", StringComparison.Ordinal) || trimmedPrefix.EndsWith("para as", StringComparison.Ordinal) ||
                     trimmedPrefix.EndsWith("pra", StringComparison.Ordinal) || trimmedPrefix.EndsWith("pras", StringComparison.Ordinal) ||
                     trimmedPrefix.EndsWith("antes da", StringComparison.Ordinal) || trimmedPrefix.EndsWith("antes das", StringComparison.Ordinal))
            {
                deltaMin = -deltaMin;
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
            var trimmedSuffix = suffix.Trim();
            AdjustByPrefix(trimmedSuffix, ref hour, ref min, ref hasMin);

            var deltaHour = 0;
            var match = PortugueseTimeExtractorConfiguration.TimeSuffix.MatchExact(trimmedSuffix, trim: true);

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
