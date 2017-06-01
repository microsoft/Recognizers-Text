using Microsoft.Recognizers.Text.DateTime.Parsers;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Text.RegularExpressions;
using Microsoft.Recognizers.Text.DateTime.English.Extractors;

namespace Microsoft.Recognizers.Text.DateTime.English.Parsers
{
    public class EnglishTimeParserConfiguration : ITimeParserConfiguration
    {
        public string TimeTokenPrefix { get; }

        public Regex AtRegex { get; }

        public IEnumerable<Regex> TimeRegexes { get; }

        public IImmutableDictionary<string, int> Numbers { get; }

        public EnglishTimeParserConfiguration(ICommonDateTimeParserConfiguration config)
        {
            TimeTokenPrefix = "at ";
            AtRegex = EnglishTimeExtractorConfiguration.AtRegex;
            TimeRegexes = EnglishTimeExtractorConfiguration.TimeRegexList;
            Numbers = config.Numbers;
        }

        public void AdjustByPrefix(string prefix, ref int hour, ref int min, ref bool hasMin)
        {
            var deltaMin = 0;
            var trimedPrefix = prefix.Trim().ToLowerInvariant();

            if (trimedPrefix.StartsWith("half"))
            {
                deltaMin = 30;
            }
            else if (trimedPrefix.StartsWith("a quarter") || trimedPrefix.StartsWith("quarter"))
            {
                deltaMin = 15;
            }
            else if (trimedPrefix.StartsWith("three quarter"))
            {
                deltaMin = 45;
            }
            else
            {
                var match = EnglishTimeExtractorConfiguration.LessThanOneHour.Match(trimedPrefix);
                var minStr = match.Groups["deltamin"].Value;
                if (!string.IsNullOrWhiteSpace(minStr))
                {
                    deltaMin = int.Parse(minStr);
                }
                else
                {
                    minStr = match.Groups["deltaminnum"].Value.ToLower();
                    deltaMin = Numbers[minStr];
                }
            }

            if (trimedPrefix.EndsWith("to"))
            {
                deltaMin = -deltaMin;
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
            var trimedSuffix = suffix.Trim().ToLowerInvariant();
            var deltaHour = 0;
            var match = EnglishTimeExtractorConfiguration.TimeSuffix.Match(trimedSuffix);
            if (match.Success && match.Index == 0 && match.Length == trimedSuffix.Length)
            {
                var oclockStr = match.Groups["oclock"].Value;
                if (string.IsNullOrEmpty(oclockStr))
                {
                    var amStr = match.Groups["am"].Value;
                    if (!string.IsNullOrEmpty(amStr))
                    {
                        if (hour >= 12)
                        {
                            deltaHour = -12;
                        }
                        hasAm = true;
                    }

                    var pmStr = match.Groups["pm"].Value;
                    if (!string.IsNullOrEmpty(pmStr))
                    {
                        if (hour < 12)
                        {
                            deltaHour = 12;
                        }
                        hasPm = true;
                    }
                }
            }

            hour = (hour + deltaHour) % 24;
        }
    }
}
