using System.Collections.Generic;
using System.Collections.Immutable;
using System.Text.RegularExpressions;

using Microsoft.Recognizers.Definitions.Portuguese;
using Microsoft.Recognizers.Text.DateTime.Utilities;

namespace Microsoft.Recognizers.Text.DateTime.Portuguese
{
    public class PortugueseTimeParserConfiguration : BaseOptionsConfiguration, ITimeParserConfiguration
    {
        public string TimeTokenPrefix { get; }

        public Regex AtRegex { get; }

        public Regex MealTimeRegex { get; }

        public IEnumerable<Regex> TimeRegexes { get; }

        public IImmutableDictionary<string, int> Numbers { get; }

        public IDateTimeUtilityConfiguration UtilityConfiguration { get; }

        public IDateTimeParser TimeZoneParser { get; }

        public PortugueseTimeParserConfiguration(ICommonDateTimeParserConfiguration config) : base(config.Options)
        {
            TimeTokenPrefix = DateTimeDefinitions.TimeTokenPrefix;
            AtRegex = PortugueseTimeExtractorConfiguration.AtRegex;
            TimeRegexes = PortugueseTimeExtractorConfiguration.TimeRegexList;
            UtilityConfiguration = config.UtilityConfiguration;
            Numbers = config.Numbers;
            TimeZoneParser = new BaseTimeZoneParser();
        }

        public void AdjustByPrefix(string prefix, ref int hour, ref int min, ref bool hasMin)
        {
            var deltaMin = 0;
            var trimedPrefix = prefix.Trim().ToLowerInvariant();

            if (trimedPrefix.StartsWith("quarto") || trimedPrefix.StartsWith("e um quarto") ||
                trimedPrefix.StartsWith("quinze") || trimedPrefix.StartsWith("e quinze"))
            {
                deltaMin = 15;
            }
            else if (trimedPrefix.StartsWith("menos um quarto"))
            {
                deltaMin = -15;
            }
            else if (trimedPrefix.StartsWith("meia") || trimedPrefix.StartsWith("e meia"))
            {
                deltaMin = 30;
            }
            else
            {
                var match = PortugueseTimeExtractorConfiguration.LessThanOneHour.Match(trimedPrefix);
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

            if (trimedPrefix.EndsWith("passadas") || trimedPrefix.EndsWith("pasados") ||
                trimedPrefix.EndsWith("depois das") || trimedPrefix.EndsWith("depois da") || trimedPrefix.EndsWith("depois do") ||
                trimedPrefix.EndsWith("passadas as") || trimedPrefix.EndsWith("passadas das"))
            {
                //deltaMin it's positive
            }
            else if (trimedPrefix.EndsWith("para a") || trimedPrefix.EndsWith("para as") ||
                     trimedPrefix.EndsWith("pra") || trimedPrefix.EndsWith("pras") ||
                     trimedPrefix.EndsWith("antes da") || trimedPrefix.EndsWith("antes das"))
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
            var match = PortugueseTimeExtractorConfiguration.TimeSuffix.Match(trimedSuffix);
            if (match.Success && match.Index == 0 && match.Length == trimedSuffix.Length)
            {
                var oclockStr = match.Groups["oclock"].Value;
                if (string.IsNullOrEmpty(oclockStr))
                {
                    var amStr = match.Groups[Constants.AmGroupName].Value;
                    if (!string.IsNullOrEmpty(amStr))
                    {
                        if (hour >= Constants.HalfDayHourCount)
                        {
                            deltaHour = -Constants.HalfDayHourCount;
                        }
                        hasAm = true;
                    }

                    var pmStr = match.Groups[Constants.PmGroupName].Value;
                    if (!string.IsNullOrEmpty(pmStr))
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
