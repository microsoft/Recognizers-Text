using System.Collections.Generic;
using System.Collections.Immutable;
using System.Globalization;
using System.Text.RegularExpressions;

using Microsoft.Recognizers.Definitions.Dutch;
using Microsoft.Recognizers.Text.DateTime.Utilities;
using Microsoft.Recognizers.Text.Utilities;

namespace Microsoft.Recognizers.Text.DateTime.Dutch
{
    public class DutchTimeParserConfiguration : BaseDateTimeOptionsConfiguration, ITimeParserConfiguration
    {

        private const RegexOptions RegexFlags = RegexOptions.Singleline | RegexOptions.ExplicitCapture;

        private static readonly Regex TimeSuffixFull =
            RegexCache.Get(DateTimeDefinitions.TimeSuffixFull, RegexFlags);

        private static readonly Regex LunchRegex =
            RegexCache.Get(DateTimeDefinitions.LunchRegex, RegexFlags);

        private static readonly Regex NightRegex =
            RegexCache.Get(DateTimeDefinitions.NightRegex, RegexFlags);

        private static readonly Regex HalfTokenRegex =
            RegexCache.Get(DateTimeDefinitions.HalfTokenRegex, RegexFlags);

        private static readonly Regex QuarterTokenRegex =
            RegexCache.Get(DateTimeDefinitions.QuarterTokenRegex, RegexFlags);

        private static readonly Regex ThreeQuarterTokenRegex =
            RegexCache.Get(DateTimeDefinitions.ThreeQuarterTokenRegex, RegexFlags);

        private static readonly Regex ToTokenRegex =
            RegexCache.Get(DateTimeDefinitions.ToTokenRegex, RegexFlags);

        private static readonly Regex ToHalfTokenRegex =
            RegexCache.Get(DateTimeDefinitions.ToHalfTokenRegex, RegexFlags);

        private static readonly Regex ForHalfTokenRegex =
            RegexCache.Get(DateTimeDefinitions.ForHalfTokenRegex, RegexFlags);

        public DutchTimeParserConfiguration(ICommonDateTimeParserConfiguration config)
            : base(config)
        {
            TimeTokenPrefix = DateTimeDefinitions.TimeTokenPrefix;
            AtRegex = DutchTimeExtractorConfiguration.AtRegex;
            TimeRegexes = DutchTimeExtractorConfiguration.TimeRegexList;
            UtilityConfiguration = config.UtilityConfiguration;
            Numbers = config.Numbers;
            TimeZoneParser = config.TimeZoneParser;
        }

        public IEnumerable<Regex> TimeRegexes { get; }

        public IImmutableDictionary<string, int> Numbers { get; }

        public IDateTimeUtilityConfiguration UtilityConfiguration { get; }

        public IDateTimeParser TimeZoneParser { get; }

        public string TimeTokenPrefix { get; }

        public Regex AtRegex { get; }

        public Regex MealTimeRegex { get; }

        public void AdjustByPrefix(string prefix, ref int hour, ref int min, ref bool hasMin)
        {
            int deltaMin;

            var trimmedPrefix = prefix.Trim();

            if (HalfTokenRegex.IsMatch(trimmedPrefix))
            {
                deltaMin = -30;
            }
            else if (QuarterTokenRegex.IsMatch(trimmedPrefix))
            {
                deltaMin = 15;
            }
            else if (ThreeQuarterTokenRegex.IsMatch(trimmedPrefix))
            {
                deltaMin = 45;
            }
            else
            {
                var match = DutchTimeExtractorConfiguration.LessThanOneHour.Match(trimmedPrefix);
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
            }

            if (ToHalfTokenRegex.IsMatch(trimmedPrefix))
            {
                deltaMin = deltaMin - 30;
            }
            else if (ForHalfTokenRegex.IsMatch(trimmedPrefix))
            {
                deltaMin = -deltaMin - 30;
            }
            else if (ToTokenRegex.IsMatch(trimmedPrefix))
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

            var deltaHour = 0;
            var match = TimeSuffixFull.MatchExact(suffix, trim: true);

            if (match.Success)
            {
                var oclockStr = match.Groups["oclock"].Value;

                if (string.IsNullOrEmpty(oclockStr))
                {
                    var stringAm = match.Groups[Constants.AmGroupName].Value;
                    if (!string.IsNullOrEmpty(stringAm))
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

                    var stringPm = match.Groups[Constants.PmGroupName].Value;
                    if (!string.IsNullOrEmpty(stringPm))
                    {
                        if (hour < Constants.HalfDayHourCount)
                        {
                            deltaHour = Constants.HalfDayHourCount;
                        }

                        if (LunchRegex.IsMatch(stringPm))
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
                        else if (NightRegex.IsMatch(stringPm))
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
