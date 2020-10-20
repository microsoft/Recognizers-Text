using System.Collections.Generic;
using System.Collections.Immutable;
using System.Globalization;
using System.Text.RegularExpressions;

using Microsoft.Recognizers.Definitions.Chinese;
using Microsoft.Recognizers.Text.NumberWithUnit;
using Microsoft.Recognizers.Text.NumberWithUnit.Chinese;
using Microsoft.Recognizers.Text.Utilities;
using DateObject = System.DateTime;

namespace Microsoft.Recognizers.Text.DateTime.Chinese
{
    public enum DurationType
    {
        /// <summary>
        /// Types of DurationType.
        /// </summary>
        WithNumber,
    }

    public class ChineseDurationExtractorConfiguration : ChineseBaseDateTimeExtractorConfiguration<DurationType>
    {

        private const RegexOptions RegexFlags = RegexOptions.Singleline | RegexOptions.ExplicitCapture;

        private static readonly IExtractor InternalExtractor = new ChineseNumberWithUnitExtractor(new DurationExtractorConfiguration());

        private static readonly Regex YearRegex = new Regex(DateTimeDefinitions.DurationYearRegex, RegexFlags);

        private static readonly Regex HalfSuffixRegex = new Regex(DateTimeDefinitions.DurationHalfSuffixRegex, RegexFlags);

        internal override ImmutableDictionary<Regex, DurationType> Regexes { get; }

        protected sealed override string ExtractType { get; } = Constants.SYS_DATETIME_DURATION; // "Duration";

        // extract by number with unit
        public override List<ExtractResult> Extract(string source, DateObject referenceTime)
        {
            // Use Unit to extract
            var retList = InternalExtractor.Extract(source);
            var res = new List<ExtractResult>();
            foreach (var ret in retList)
            {
                // filter
                var match = YearRegex.Match(ret.Text);
                if (match.Success)
                {
                    continue;
                }

                res.Add(ret);
            }

            return res;
        }

        internal class DurationExtractorConfiguration : ChineseNumberWithUnitExtractorConfiguration
        {
            public static readonly ImmutableDictionary<string, string> DurationSuffixList = DateTimeDefinitions.DurationSuffixList.ToImmutableDictionary();

            public DurationExtractorConfiguration()
                : base(new CultureInfo("zh-CN"))
            {
            }

            public override ImmutableDictionary<string, string> SuffixList => DurationSuffixList;

            public override ImmutableDictionary<string, string> PrefixList => null;

            public override string ExtractType => Constants.SYS_DATETIME_DURATION;

            public override ImmutableList<string> AmbiguousUnitList => DateTimeDefinitions.DurationAmbiguousUnits.ToImmutableList();
        }
    }
}