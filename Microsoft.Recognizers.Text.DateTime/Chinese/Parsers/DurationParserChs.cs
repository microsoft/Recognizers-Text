using System.Collections.Generic;
using System.Globalization;
using DateObject = System.DateTime;
using Microsoft.Recognizers.Text.NumberWithUnit;
using Microsoft.Recognizers.Text.NumberWithUnit.Chinese.Parsers;
using Microsoft.Recognizers.Text.DateTime.Parsers;
using Microsoft.Recognizers.Text.DateTime.Utilities;
using static Microsoft.Recognizers.Text.DateTime.Chinese.Extractors.DurationExtractorChs;

namespace Microsoft.Recognizers.Text.DateTime.Chinese.Parsers
{
    public class DurationParserChs : IDateTimeParser
    {
        public static readonly string ParserName = Constants.SYS_DATETIME_DURATION; //"Duration";

        private static readonly IParser InternalParser = new NumberWithUnitParser(new DurationParserConfiguration());

        internal class DurationParserConfiguration : ChineseNumberWithUnitParserConfiguration
        {
            public DurationParserConfiguration() : base(new CultureInfo("zh-CN"))
            {
                this.BindDictionary(DurationExtractorConfiguration.DurationSuffixList);
            }
        }

        public static readonly Dictionary<string, int> UnitValueMap = new Dictionary<string, int>
        {
            {"Y", 31536000},
            {"Mon", 2592000},
            {"W", 604800},
            {"D", 86400},
            {"H", 3600},
            {"M", 60},
            {"S", 1}
        };
        
        private readonly IFullDateTimeParserConfiguration config;

        public DurationParserChs(IFullDateTimeParserConfiguration configuration)
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
            var parseResult = InternalParser.Parse(er);
            var unitResult = parseResult.Value as UnitValue;
            var dtParseResult = new DTParseResult();
            var unitStr = unitResult.Unit;
            var numStr = unitResult.Number;
            dtParseResult.Timex = "P" + (BaseDurationParser.IsLessThanDay(unitStr) ? "T" : "") + numStr + unitStr[0];
            dtParseResult.FutureValue = dtParseResult.PastValue = double.Parse(numStr)*UnitValueMap[unitStr];
            dtParseResult.Success = true;
            if (dtParseResult.Success)
            {
                dtParseResult.FutureResolution = new Dictionary<string, string>
                {
                    {TimeTypeConstants.DURATION, dtParseResult.FutureValue.ToString()}
                };
                dtParseResult.PastResolution = new Dictionary<string, string>
                {
                    {TimeTypeConstants.DURATION, dtParseResult.PastValue.ToString()}
                };
            }
            var ret = new DateTimeParseResult
            {
                Text = er.Text,
                Start = er.Start,
                Length = er.Length,
                Type = er.Type,
                Data = er.Data,
                Value = dtParseResult,
                TimexStr = dtParseResult.Timex,
                ResolutionStr = ""
            };
            return ret;
        }
    }
}