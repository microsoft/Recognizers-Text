namespace Microsoft.Recognizers.Text.Number.Chinese
{
    public class ChineseNumberRangeParserConfiguration :INumberRangeParserConfiguration
    {
        public IExtractor NumberExtractor { get; }

        public IExtractor OrdinalExtractor { get; }

        public IParser NumberParser { get; }

        public ChineseNumberRangeParserConfiguration()
        {
            NumberExtractor = new NumberExtractor();
            OrdinalExtractor = new OrdinalExtractor();
            NumberParser =  new ChineseNumberParser(new ChineseNumberParserConfiguration());
        }
    }
}
