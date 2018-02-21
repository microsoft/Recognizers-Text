using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Recognizers.Text.Options;

namespace Microsoft.Recognizers.Text.Options.Parsers
{
    class OptionsParser<T> : IParser
    {
        private readonly IOptionParserConfiguration<T> config;

        public OptionsParser(IOptionParserConfiguration<T> config)
        {
            this.config = config;
        }

        public ParseResult Parse(ExtractResult extResult)
        {
            var result = new ParseResult(extResult);
            result.Value = config.Resolutions[result.Type];

            return result;
        }
    }
}
