using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Globalization;
using System.Linq;
using System.Text.RegularExpressions;
using Microsoft.Recognizers.Definitions.Chinese;
using Microsoft.Recognizers.Text.NumberWithUnit;
using Microsoft.Recognizers.Text.NumberWithUnit.Chinese;
using static Microsoft.Recognizers.Text.DateTime.Chinese.ChineseDurationExtractorConfiguration;
using DateObject = System.DateTime;

namespace Microsoft.Recognizers.Text.DateTime.Chinese
{
    public class ChineseDurationParserConfiguration : IDateTimeParser
    {

        public static readonly string ParserName = Constants.SYS_DATETIME_DURATION; // "Duration";

        public static readonly ImmutableDictionary<string, long> UnitValueMap = DateTimeDefinitions.DurationUnitValueMap.ToImmutableDictionary();

        private const RegexOptions RegexFlags = RegexOptions.Singleline | RegexOptions.ExplicitCapture;

        private static readonly Regex DurationUnitRegex = new Regex(DateTimeDefinitions.DurationUnitRegex, RegexFlags);

        private static readonly IParser InternalParser = new NumberWithUnitParser(new DurationParserConfiguration());

        private static readonly IDateTimeExtractor DurationExtractor = new ChineseDurationExtractorConfiguration(false);

        private static readonly Dictionary<string, string> UnitMap =
            DateTimeDefinitions.ParserConfigurationUnitMap.ToDictionary(k => k.Key, k => k.Value.Substring(0, 1) + k.Value.Substring(1).ToLowerInvariant());

        private readonly IFullDateTimeParserConfiguration config;

        public ChineseDurationParserConfiguration(IFullDateTimeParserConfiguration configuration)
        {
            config = configuration;
        }

        public ParseResult Parse(ExtractResult extResult)
        {
            return this.Parse(extResult, DateObject.Now);
        }

        public DateTimeParseResult Parse(ExtractResult er, DateObject refDate)
        {
            var referenceTime = refDate;

            var dateTimeParseResult = ParseMergedDuration(er.Text, referenceTime);
            if (!dateTimeParseResult.Success)
            {
                var parseResult = InternalParser.Parse(er);
                var unitResult = parseResult.Value as UnitValue;

                if (unitResult == null)
                {
                    return null;
                }

                var unitStr = unitResult.Unit;
                var numStr = unitResult.Number;

                dateTimeParseResult.Timex = TimexUtility.GenerateDurationTimex(double.Parse(numStr, CultureInfo.InvariantCulture), unitStr, BaseDurationParser.IsLessThanDay(unitStr));
                dateTimeParseResult.FutureValue = dateTimeParseResult.PastValue = double.Parse(numStr, CultureInfo.InvariantCulture) * UnitValueMap[unitStr];
                dateTimeParseResult.Success = true;
            }

            if (dateTimeParseResult.Success)
            {
                dateTimeParseResult.FutureResolution = new Dictionary<string, string>
                    {
                        { TimeTypeConstants.DURATION, dateTimeParseResult.FutureValue.ToString() },
                    };

                dateTimeParseResult.PastResolution = new Dictionary<string, string>
                    {
                        { TimeTypeConstants.DURATION, dateTimeParseResult.PastValue.ToString() },
                    };
            }

            var ret = new DateTimeParseResult
            {
                Text = er.Text,
                Start = er.Start,
                Length = er.Length,
                Type = er.Type,
                Data = er.Data,
                Value = dateTimeParseResult,
                TimexStr = dateTimeParseResult.Timex,
                ResolutionStr = string.Empty,
            };

            return ret;
        }

        public List<DateTimeParseResult> FilterResults(string query, List<DateTimeParseResult> candidateResults)
        {
            return candidateResults;
        }

        private DateTimeResolutionResult ParseMergedDuration(string text, DateObject referenceTime)
        {
            var ret = new DateTimeResolutionResult();
            var durationExtractor = DurationExtractor;

            // DurationExtractor without parameter will not extract merged duration
            var ers = durationExtractor.Extract(text, referenceTime);

            // only handle merged duration cases like "1 month 21 days"
            if (ers.Count <= 1)
            {
                ret.Success = false;
                return ret;
            }

            var start = ers[0].Start ?? 0;
            if (start != 0)
            {
                var beforeStr = text.Substring(0, start - 1);
                if (!string.IsNullOrWhiteSpace(beforeStr))
                {
                    return ret;
                }
            }

            var end = ers[ers.Count - 1].Start + ers[ers.Count - 1].Length ?? 0;
            if (end != text.Length)
            {
                var afterStr = text.Substring(end);
                if (!string.IsNullOrWhiteSpace(afterStr))
                {
                    return ret;
                }
            }

            var prs = new List<DateTimeParseResult>();
            var timexDict = new Dictionary<string, string>();

            // insert timex into a dictionary
            foreach (var er in ers)
            {
                var unitRegex = DurationUnitRegex;
                var unitMatch = unitRegex.Match(er.Text);
                if (unitMatch.Success)
                {
                    var pr = (DateTimeParseResult)Parse(er);
                    if (pr.Value != null)
                    {
                        timexDict.Add(UnitMap[unitMatch.Groups["unit"].Value], pr.TimexStr);
                        prs.Add(pr);
                    }
                }
            }

            // sort the timex using the granularity of the duration, "P1M23D" for "1 month 23 days" and "23 days 1 month"
            if (prs.Count == ers.Count)
            {
                ret.Timex = TimexUtility.GenerateCompoundDurationTimex(timexDict, UnitValueMap);

                double value = 0;
                foreach (var pr in prs)
                {
                    value += double.Parse(((DateTimeResolutionResult)pr.Value).FutureValue.ToString(), CultureInfo.InvariantCulture);
                }

                ret.FutureValue = ret.PastValue = value;
            }

            ret.Success = true;
            return ret;
        }

        internal class DurationParserConfiguration : ChineseNumberWithUnitParserConfiguration
        {
            public DurationParserConfiguration()
                : base(new CultureInfo(Culture.Chinese))
            {
                this.BindDictionary(DurationExtractorConfiguration.DurationSuffixList);
            }
        }
    }
}