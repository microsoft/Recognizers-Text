using System.Collections.Generic;
using System.Collections.Immutable;
using System.Text.RegularExpressions;

using Microsoft.Recognizers.Definitions.Spanish;
using Microsoft.Recognizers.Text.DateTime.Utilities;

namespace Microsoft.Recognizers.Text.DateTime.Spanish
{
    public class SpanishTimeParserConfiguration : BaseOptionsConfiguration, ITimeParserConfiguration
    {
        public string TimeTokenPrefix { get; }

        public Regex AtRegex { get; }

        public Regex MealTimeRegex { get; }

        public IEnumerable<Regex> TimeRegexes { get; }

        public IImmutableDictionary<string, int> Numbers { get; }

        public IDateTimeUtilityConfiguration UtilityConfiguration { get; }

        public IDateTimeParser TimeZoneParser { get; }

        public SpanishTimeParserConfiguration(ICommonDateTimeParserConfiguration config) : base(config.Options)
        {
            TimeTokenPrefix = DateTimeDefinitions.TimeTokenPrefix;
            AtRegex = SpanishTimeExtractorConfiguration.AtRegex;
            TimeRegexes = SpanishTimeExtractorConfiguration.TimeRegexList;
            UtilityConfiguration = config.UtilityConfiguration;
            Numbers = config.Numbers;
            TimeZoneParser = new BaseTimeZoneParser();
        }

        public void AdjustByPrefix(string prefix, ref int hour, ref int min, ref bool hasMin)
        {
            var deltaMin = 0;
            var trimedPrefix = prefix.Trim().ToLowerInvariant();

            if (trimedPrefix.StartsWith("cuarto") || trimedPrefix.StartsWith("y cuarto"))
            {
                deltaMin = 15;
            }
            else if (trimedPrefix.StartsWith("menos cuarto"))
            {
                deltaMin = -15;
            }
            else if (trimedPrefix.StartsWith("media") || trimedPrefix.StartsWith("y media"))
            {
                deltaMin = 30;
            }
            else
            {
                var match = SpanishTimeExtractorConfiguration.LessThanOneHour.Match(trimedPrefix);
                var minStr = match.Groups["deltamin"].Value;
                if (!string.IsNullOrWhiteSpace(minStr))
                {
                    deltaMin = int.Parse(minStr);
                }
                else
                {
                    minStr = match.Groups["deltaminnum"].Value.ToLower();
                    Numbers.TryGetValue(minStr, out deltaMin);
                }
            }

            if (trimedPrefix.EndsWith("pasadas") || trimedPrefix.EndsWith("pasados") ||
                trimedPrefix.EndsWith("pasadas las") || trimedPrefix.EndsWith("pasados las") ||
                trimedPrefix.EndsWith("pasadas de las") || trimedPrefix.EndsWith("pasados de las"))
            {
                //deltaMin it's positive
            }
            else if (trimedPrefix.EndsWith("para la") || trimedPrefix.EndsWith("para las") ||
                     trimedPrefix.EndsWith("antes de la") || trimedPrefix.EndsWith("antes de las"))
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
            var trimedSuffix = suffix.Trim().ToLowerInvariant();
            AdjustByPrefix(trimedSuffix, ref hour, ref min, ref hasMin);

            var deltaHour = 0;
            var match = SpanishTimeExtractorConfiguration.TimeSuffix.Match(trimedSuffix);
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
