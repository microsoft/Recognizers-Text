using System.Text.RegularExpressions;
using Microsoft.Recognizers.Text.Number.English;

namespace Microsoft.Recognizers.Text.DateTime.English
{
    public class EnglishDurationExtractorConfiguration : IDurationExtractorConfiguration
    {
        public static readonly Regex UnitRegex =
            new Regex(
                @"(?<unit>years|year|months|month|weeks|week|days|day|hours|hour|hrs|hr|h|minutes|minute|mins|min|seconds|second|secs|sec)\b",
                RegexOptions.IgnoreCase | RegexOptions.Singleline);

        public static readonly Regex FollowedUnit = new Regex($@"^\s*{UnitRegex}",
            RegexOptions.IgnoreCase | RegexOptions.Singleline);

        public static readonly Regex NumberCombinedWithUnit =
            new Regex($@"\b(?<num>\d+(\.\d*)?){UnitRegex}", RegexOptions.IgnoreCase | RegexOptions.Singleline);

        public static readonly Regex AnUnitRegex = new Regex($@"(((?<half>half\s+)*(an|a))|(an|a))\s+{UnitRegex}",
            RegexOptions.IgnoreCase | RegexOptions.Singleline);

        public static readonly Regex AllRegex = new Regex(@"\b(?<all>all\s+(?<unit>year|month|week|day))\b",
            RegexOptions.IgnoreCase | RegexOptions.Singleline);

        public static readonly Regex HalfRegex = new Regex(@"\b(?<half>half\s+(?<unit>year|month|week|day|hour))\b",
            RegexOptions.IgnoreCase | RegexOptions.Singleline);

        public EnglishDurationExtractorConfiguration()
        {
            CardinalExtractor = new CardinalExtractor();
        }

        public IExtractor CardinalExtractor { get; }

        Regex IDurationExtractorConfiguration.FollowedUnit => FollowedUnit;

        Regex IDurationExtractorConfiguration.NumberCombinedWithUnit => NumberCombinedWithUnit;

        Regex IDurationExtractorConfiguration.AnUnitRegex => AnUnitRegex;

        Regex IDurationExtractorConfiguration.AllRegex => AllRegex;

        Regex IDurationExtractorConfiguration.HalfRegex => HalfRegex;
    }
}