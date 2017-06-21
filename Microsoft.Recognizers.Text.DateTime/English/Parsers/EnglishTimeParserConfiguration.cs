using System.Collections.Generic;
using System.Collections.Immutable;
using System.Text.RegularExpressions;

namespace Microsoft.Recognizers.Text.DateTime.English
{
    public class EnglishTimeParserConfiguration : ITimeParserConfiguration
    {
        public string TimeTokenPrefix { get; }

        public Regex AtRegex { get; }

        public Regex UnitRegex { get; }

        public IEnumerable<Regex> TimeRegexes { get; }

        public IImmutableDictionary<string, int> Numbers { get; }

        public IExtractor CardinalExtractor { get; }

        public IParser NumberParser { get; }

        public IExtractor DurationExtractor { get; }

        public IParser DurationParser { get; }

        public IImmutableDictionary<string, string> UnitMap { get; }

        public EnglishTimeParserConfiguration(ICommonDateTimeParserConfiguration config)
        {
            TimeTokenPrefix = "at ";
            AtRegex = EnglishTimeExtractorConfiguration.AtRegex;
            UnitRegex = EnglishTimeExtractorConfiguration.UnitRegex;
            TimeRegexes = EnglishTimeExtractorConfiguration.TimeRegexList;
            Numbers = config.Numbers;
            CardinalExtractor = config.CardinalExtractor;
            NumberParser = config.NumberParser;
            DurationExtractor = config.DurationExtractor;
            DurationParser = config.DurationParser;
            UnitMap = config.UnitMap;
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

        public bool ContainsAgoString(string text)
        {
            string agoString = "ago";
            return text.TrimStart().StartsWith(agoString);
        }

        public bool ContainsLaterString(string text)
        {
            string laterString = "later";
            return text.TrimStart().StartsWith(laterString);
        }
    }
}
