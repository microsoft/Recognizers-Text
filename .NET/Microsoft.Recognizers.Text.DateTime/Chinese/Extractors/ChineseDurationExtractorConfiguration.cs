using System.Collections.Generic;
using System.Collections.Immutable;
using System.Globalization;
using System.Linq;
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

        private static readonly IExtractor InternalExtractor = new NumberWithUnitExtractor(new DurationExtractorConfiguration());

        private static readonly Regex YearRegex = RegexCache.Get(DateTimeDefinitions.DurationYearRegex, RegexFlags);

        private static readonly Regex HalfSuffixRegex = RegexCache.Get(DateTimeDefinitions.DurationHalfSuffixRegex, RegexFlags);

        private static readonly Regex DurationUnitRegex = RegexCache.Get(DateTimeDefinitions.DurationUnitRegex, RegexFlags);

        private static readonly Regex DurationConnectorRegex = RegexCache.Get(DateTimeDefinitions.DurationConnectorRegex, RegexFlags);

        private static readonly Dictionary<string, string> UnitMap = DateTimeDefinitions.ParserConfigurationUnitMap.ToDictionary(k => k.Key, k => k.Value.Substring(0, 1) + k.Value.Substring(1).ToLower());

        private static readonly Dictionary<string, long> UnitValueMap = DateTimeDefinitions.DurationUnitValueMap;

        private readonly bool merge;

        public ChineseDurationExtractorConfiguration(bool merge = true)
        {
            this.merge = merge;
        }

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

            if (this.merge)
            {
                res = MergeMultipleDuration(source, res);
            }

            return res;
        }

        private List<ExtractResult> MergeMultipleDuration(string text, List<ExtractResult> extractorResults)
        {
            if (extractorResults.Count <= 1)
            {
                return extractorResults;
            }

            var unitMap = UnitMap;
            var unitValueMap = UnitValueMap;
            var unitRegex = DurationUnitRegex;
            List<ExtractResult> ret = new List<ExtractResult>();

            var firstExtractionIndex = 0;
            var timeUnit = 0;
            var totalUnit = 0;
            while (firstExtractionIndex < extractorResults.Count)
            {
                string curUnit = null;
                var unitMatch = unitRegex.Match(extractorResults[firstExtractionIndex].Text);

                if (unitMatch.Success && unitMap.ContainsKey(unitMatch.Groups["unit"].ToString()))
                {
                    curUnit = unitMatch.Groups["unit"].ToString();
                    totalUnit++;
                    if (DurationParsingUtil.IsTimeDurationUnit(unitMap[curUnit]))
                    {
                        timeUnit++;
                    }
                }

                if (string.IsNullOrEmpty(curUnit))
                {
                    firstExtractionIndex++;
                    continue;
                }

                var secondExtractionIndex = firstExtractionIndex + 1;
                while (secondExtractionIndex < extractorResults.Count)
                {
                    var valid = false;
                    var midStrBegin = extractorResults[secondExtractionIndex - 1].Start + extractorResults[secondExtractionIndex - 1].Length ?? 0;
                    var midStrEnd = extractorResults[secondExtractionIndex].Start ?? 0;
                    if (midStrBegin > midStrEnd)
                    {
                        return extractorResults;
                    }

                    var midStr = text.Substring(midStrBegin, midStrEnd - midStrBegin);
                    var match = DurationConnectorRegex.Match(midStr);
                    if (match.Success)
                    {
                        unitMatch = unitRegex.Match(extractorResults[secondExtractionIndex].Text);
                        if (unitMatch.Success && unitMap.ContainsKey(unitMatch.Groups["unit"].ToString()))
                        {
                            var nextUnitStr = unitMatch.Groups["unit"].ToString();
                            if (unitValueMap[UnitMap[nextUnitStr]] != unitValueMap[UnitMap[curUnit]])
                            {
                                valid = true;
                                if (unitValueMap[UnitMap[nextUnitStr]] < unitValueMap[UnitMap[curUnit]])
                                {
                                    curUnit = nextUnitStr;
                                }
                            }

                            totalUnit++;
                            if (DurationParsingUtil.IsTimeDurationUnit(unitMap[nextUnitStr]))
                            {
                                timeUnit++;
                            }
                        }
                    }

                    if (!valid)
                    {
                        break;
                    }

                    secondExtractionIndex++;
                }

                if (secondExtractionIndex - 1 > firstExtractionIndex)
                {
                    var node = new ExtractResult();
                    node.Start = extractorResults[firstExtractionIndex].Start;
                    node.Length = extractorResults[secondExtractionIndex - 1].Start + extractorResults[secondExtractionIndex - 1].Length - node.Start;
                    node.Text = text.Substring(node.Start ?? 0, node.Length ?? 0);
                    node.Type = extractorResults[firstExtractionIndex].Type;

                    // Add multiple duration type to extract result
                    string type = Constants.MultipleDuration_DateTime; // Default type
                    if (timeUnit == totalUnit)
                    {
                        type = Constants.MultipleDuration_Time;
                    }
                    else if (timeUnit == 0)
                    {
                        type = Constants.MultipleDuration_Date;
                    }

                    node.Data = type;

                    ret.Add(node);

                    timeUnit = 0;
                    totalUnit = 0;
                }
                else
                {
                    ret.Add(extractorResults[firstExtractionIndex]);
                }

                firstExtractionIndex = secondExtractionIndex;
            }

            return ret;
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