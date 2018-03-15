namespace Microsoft.Recognizers.Text.NumberWithUnit
{
    class BaseMergedParser : IParser
    {
        protected readonly BaseNumberWithUnitParserConfiguration config;
        private readonly NumberWithUnitParser numberWithUnitParser;

        public BaseMergedParser(BaseNumberWithUnitParserConfiguration config)
        {
            this.config = config;
            numberWithUnitParser = new NumberWithUnitParser(config);
        }

        public ParseResult Parse(ExtractResult extResult)
        {
            ParseResult pr;

            // For now only currency model recognize compound units.
            if (extResult.Type.Equals(Constants.SYS_UNIT_COMPOUND))
            {
                pr = new BaseCurrencyParser(config).Parse(extResult);
            }
            else if (extResult.Type.Equals(Constants.SYS_UNIT_CURRENCY))
            {
                pr = new BaseCurrencyParser(config).Parse(extResult);
            }
            else
            {
                pr = numberWithUnitParser.Parse(extResult);
            }

            return pr;
        }
    }
}