using System.Linq;

namespace Microsoft.Recognizers.Text.Choice
{
    public class ChoiceParser<T> : IParser
    {
        private readonly IChoiceParserConfiguration<T> config;

        public ChoiceParser(IChoiceParserConfiguration<T> config)
        {
            this.config = config;
        }

        public ParseResult Parse(ExtractResult extResult)
        {
            var result = new ParseResult(extResult);
            var data = extResult.Data as ChoiceExtractDataResult;
            result.Value = config.Resolutions[result.Type];
            result.Data = new ChoiceParseDataResult()
            {
                Score = data.Score,
                OtherMatches = data.OtherMatches.Select(er => ToOtherMatchResult(er)),
            };

            return result;
        }

        private OtherMatchParseResult ToOtherMatchResult(ExtractResult extResult)
        {
            var rp = new ParseResult(extResult);
            var data = extResult.Data as ChoiceExtractDataResult;

            var result = new OtherMatchParseResult()
            {
                Text = rp.Text,
                Value = config.Resolutions[rp.Type],
                Score = data.Score,
            };

            return result;
        }
    }
}
