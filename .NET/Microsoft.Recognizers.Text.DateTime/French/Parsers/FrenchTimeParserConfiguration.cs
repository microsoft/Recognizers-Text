using System.Collections.Generic;
using System.Collections.Immutable;
using System.Text.RegularExpressions;

using Microsoft.Recognizers.Text.DateTime.Utilities;
using Microsoft.Recognizers.Definitions.French;

namespace Microsoft.Recognizers.Text.DateTime.French
{
    public class FrenchTimeParserConfiguration : BaseOptionsConfiguration, ITimeParserConfiguration
    {
        public string TimeTokenPrefix { get; }

        public Regex AtRegex { get; }

        public IEnumerable<Regex> TimeRegexes { get; }

        public IImmutableDictionary<string, int> Numbers { get; }

        public IDateTimeUtilityConfiguration UtilityConfiguration { get; }

        public IDateTimeParser TimeZoneParser { get; }

        public FrenchTimeParserConfiguration(ICommonDateTimeParserConfiguration config) : base(config.Options)
        {
            TimeTokenPrefix = DateTimeDefinitions.TimeTokenPrefix; 
            AtRegex = FrenchTimeExtractorConfiguration.AtRegex;
            TimeRegexes = FrenchTimeExtractorConfiguration.TimeRegexList;
            UtilityConfiguration = config.UtilityConfiguration;
            Numbers = config.Numbers;
            TimeZoneParser = new BaseTimeZoneParser();
        }

        public void AdjustByPrefix(string prefix, ref int hour, ref int min, ref bool hasMin)
        {
            var deltaMin = 0;
            var trimedPrefix = prefix.Trim().ToLowerInvariant();

            if (trimedPrefix.EndsWith("demie"))   // c'este 8 heures et demie, - "it's half past 8"
            {
                deltaMin = 30;
            }
            else if (trimedPrefix.EndsWith("un quart") || trimedPrefix.EndsWith("quart"))
            {
                deltaMin = 15;
            }
            else if (trimedPrefix.EndsWith("trois quarts"))
            {
                deltaMin = 45;
            }
            else
            {
                var match = FrenchTimeExtractorConfiguration.LessThanOneHour.Match(trimedPrefix);
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

            if (trimedPrefix.EndsWith("à")) // 'to' i.e 'one to five' = 'un à cinq'
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
            var match = FrenchTimeExtractorConfiguration.TimeSuffix.Match(trimedSuffix);
            if (match.Success && match.Index == 0 && match.Length == trimedSuffix.Length)
            {
                var oclockStr = match.Groups["heures"].Value;
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
