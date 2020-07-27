using System;
using System.Collections.Generic;

namespace Microsoft.Recognizers.Text.NumberWithUnit
{
    public class BaseMergedUnitParser : IParser
    {
        private readonly NumberWithUnitParser numberWithUnitParser;

        public BaseMergedUnitParser(BaseNumberWithUnitParserConfiguration config, NumberWithUnitOptions options = NumberWithUnitOptions.None)
        {
            this.Config = config;
            numberWithUnitParser = new NumberWithUnitParser(config);
            this.Options = options;
        }

        public virtual NumberWithUnitOptions Options { get; } = NumberWithUnitOptions.None;

        protected BaseNumberWithUnitParserConfiguration Config { get; private set; }

        public ParseResult Parse(ExtractResult extResult)
        {
            ParseResult pr;

            // For now only currency model recognizes compound units.
            if (extResult.Type.Equals(Constants.SYS_UNIT_CURRENCY, StringComparison.Ordinal))
            {
                pr = new BaseCurrencyParser(Config).Parse(extResult);
            }
            else if ((this.Options & NumberWithUnitOptions.EnableCompoundTypes) != 0 && extResult.Type.Equals(Constants.SYS_UNIT_DIMENSION, StringComparison.Ordinal) && extResult.Data is List<ExtractResult>)
            {
                pr = new BaseMergedDimensionsParser(Config).Parse(extResult);
            }
            else
            {
                pr = numberWithUnitParser.Parse(extResult);
            }

            return pr;
        }
    }
}