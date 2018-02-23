namespace Microsoft.Recognizers.Text.Sequence
{
    class BaseSequenceParser : IParser
    {
        public virtual ParseResult Parse(ExtractResult extResult)
        {
            var result = new ParseResult
            {
                Start = extResult.Start,
                Length = extResult.Length,
                Text = extResult.Text,
                Type = extResult.Type,
                ResolutionStr = extResult.Text,
            };

            return result;
        }
    }
}
