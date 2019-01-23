using System.Linq;

namespace Microsoft.Recognizers.Text.Choice
{
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
                OtherMatches = data.OtherMatches.Select(er => TOptionsOtherMatchReuslt(er)),
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
                Score = data.Score,
            };

            return result;
        }
    }
}
