using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Recognizers.Text.Options;
using Microsoft.Recognizers.Text.Options.Extractors;

namespace Microsoft.Recognizers.Text.Options.Parsers
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
        private readonly IOptionParserConfiguration<T> config;

        public OptionsParser(IOptionParserConfiguration<T> config)
        {
            this.config = config;
        }

        public ParseResult Parse(ExtractResult extResult)
        {
            var result = new ParseResult(extResult);
            var data = extResult.Data as OptionsExtractDataResult;
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
            var data = extResult.Data as OptionsExtractDataResult;

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
