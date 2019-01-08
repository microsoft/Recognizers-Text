namespace Microsoft.Recognizers.Text.NumberWithUnit
{
    public class BaseMergedUnitParser : IParser
    {
        private readonly NumberWithUnitParser numberWithUnitParser;

        public BaseMergedUnitParser(BaseNumberWithUnitParserConfiguration config)
        {
            this.Config = config;
            numberWithUnitParser = new NumberWithUnitParser(config);
        }

        protected BaseNumberWithUnitParserConfiguration Config { get; private set; }

        public ParseResult Parse(ExtractResult extResult)
        {
            ParseResult pr;

            // For now only currency model recognizes compound units.
            if (extResult.Type.Equals(Constants.SYS_UNIT_CURRENCY))
            {
                pr = new BaseCurrencyParser(Config).Parse(extResult);
            }
            else
            {
                pr = numberWithUnitParser.Parse(extResult);
            }

            return pr;
        }
    }
}