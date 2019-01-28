using System.Collections.Generic;
using System.Globalization;
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

        public static readonly Dictionary<string, int> UnitValueMap = DateTimeDefinitions.DurationUnitValueMap;

        private static readonly IParser InternalParser = new NumberWithUnitParser(new DurationParserConfiguration());

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

            // handle cases like "三年半"
            var hasHalfSuffix = false;
            if (er.Text.EndsWith("半"))
            {
                er.Length -= 1;
                er.Text = er.Text.Substring(0, er.Text.Length - 1);
                hasHalfSuffix = true;
            }

            var parseResult = InternalParser.Parse(er);
            var unitResult = parseResult.Value as UnitValue;

            if (unitResult == null)
            {
                return null;
            }

            var dateTimeParseResult = new DateTimeResolutionResult();
            var unitStr = unitResult.Unit;
            var numStr = unitResult.Number;

            if (hasHalfSuffix)
            {
                numStr = (double.Parse(numStr) + 0.5).ToString(CultureInfo.InvariantCulture);
            }

            dateTimeParseResult.Timex = "P" + (BaseDurationParser.IsLessThanDay(unitStr) ? "T" : string.Empty) + numStr + unitStr[0];
            dateTimeParseResult.FutureValue = dateTimeParseResult.PastValue = double.Parse(numStr) * UnitValueMap[unitStr];
            dateTimeParseResult.Success = true;

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