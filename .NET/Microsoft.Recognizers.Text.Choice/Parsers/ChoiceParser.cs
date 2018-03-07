using System.Collections.Generic;
using System.Linq;
using Microsoft.Recognizers.Text.Choice.Extractors;

namespace Microsoft.Recognizers.Text.Choice.Parsers
{
    public class OptionsOtherMatchParseResult
    {
        public double Score { get; set; }

        public string Text { get; set; }

        public object Value { get; set; }
    }

    public class OptionsParseDataResult
    {
        public double Score { get; set; }

        public IEnumerable<OptionsOtherMatchParseResult> OtherMatches { get; set; }
    }

    public class OptionsParser<T> : IParser
    {
        private readonly IChoiceParserConfiguration<T> config;

        public OptionsParser(IChoiceParserConfiguration<T> config)
        {
            this.config = config;
        }

        public ParseResult Parse(ExtractResult extResult)
        {
            var result = new ParseResult(extResult);
            var data = extResult.Data as ChoiceExtractDataResult;
            result.Value = config.Resolutions[result.Type];
            result.Data = new OptionsParseDataResult()
            {
                Score = data.Score,
                OtherMatches = data.OtherMatches.Select(er => TOptionsOtherMatchReuslt(er))
            };

            return result;
        }

        private OptionsOtherMatchParseResult TOptionsOtherMatchReuslt(ExtractResult extResult)
        {
            var rp = new ParseResult(extResult);
            var data = extResult.Data as ChoiceExtractDataResult;

            var result = new OptionsOtherMatchParseResult()
            {
                Text = rp.Text,
                Value = config.Resolutions[rp.Type],
                Score = data.Score
            };

            return result;
        }
    }
}
