using System.Collections.Generic;
using System.Collections.Immutable;
using System.Text.RegularExpressions;
using Microsoft.Recognizers.Definitions.Italian;
using Microsoft.Recognizers.Text.DateTime.Utilities;

namespace Microsoft.Recognizers.Text.DateTime.Italian
{
    public class ItalianTimeParserConfiguration : BaseOptionsConfiguration, ITimeParserConfiguration
    {
        private static readonly Regex LunchRegex =
            new Regex(DateTimeDefinitions.LunchRegex, RegexOptions.Singleline);

        private static readonly Regex NightRegex =
            new Regex(DateTimeDefinitions.NightRegex, RegexOptions.Singleline);

        public ItalianTimeParserConfiguration(ICommonDateTimeParserConfiguration config)
            : base(config.Options)
        {
            TimeTokenPrefix = DateTimeDefinitions.TimeTokenPrefix;
            AtRegex = ItalianTimeExtractorConfiguration.AtRegex;
            TimeRegexes = ItalianTimeExtractorConfiguration.TimeRegexList;
            UtilityConfiguration = config.UtilityConfiguration;
            Numbers = config.Numbers;
            TimeZoneParser = config.TimeZoneParser;
        }

        public string TimeTokenPrefix { get; }

        public Regex AtRegex { get; }

        public IEnumerable<Regex> TimeRegexes { get; }

        public IImmutableDictionary<string, int> Numbers { get; }

        public IDateTimeUtilityConfiguration UtilityConfiguration { get; }

        public IDateTimeParser TimeZoneParser { get; }

        public void AdjustByPrefix(string prefix, ref int hour, ref int min, ref bool hasMin)
        {
            var deltaMin = 0;
            var trimmedPrefix = prefix.Trim();

            // "it's half past 8"
            if (trimmedPrefix.EndsWith("mezza") || trimmedPrefix.EndsWith("mezzo"))
            {
                deltaMin = 30;
            }
            else if (trimmedPrefix.EndsWith("un quarto") || trimmedPrefix.EndsWith("quarto"))
            {
                deltaMin = 15;
            }
            else if (trimmedPrefix.EndsWith("tre quarti"))
            {
                deltaMin = 45;
            }
            else
            {
                var match = ItalianTimeExtractorConfiguration.LessThanOneHour.Match(trimmedPrefix);
                var minStr = match.Groups["deltamin"].Value;
                if (!string.IsNullOrWhiteSpace(minStr))
                {
                    deltaMin = int.Parse(minStr);
                }
                else
                {
                    minStr = match.Groups["deltaminnum"].Value;
                    deltaMin = Numbers[minStr];
                }
            }

            // 'to' i.e 'one to five'
            if (trimmedPrefix.StartsWith("meno"))
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
            var lowerSuffix = suffix;
            var deltaHour = 0;
            var match = ItalianTimeExtractorConfiguration.TimeSuffix.MatchExact(lowerSuffix, trim: true);

            if (match.Success)
            {
                var oclockStr = match.Groups["oclock"].Value;
                if (string.IsNullOrEmpty(oclockStr))
                {
                    var matchAmStr = match.Groups["am"].Value;
                    if (!string.IsNullOrEmpty(matchAmStr))
                    {
                        if (hour >= 12)
                        {
                            deltaHour = -12;
                        }

                        hasAm = true;
                    }

                    var matchPmStr = match.Groups["pm"].Value;
                    if (!string.IsNullOrEmpty(matchPmStr))
                    {
                        if (hour < 12)
                        {
                            deltaHour = 12;
                        }

                        if (LunchRegex.IsMatch(matchPmStr))
                        {
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
