using System;
using System.Linq;
using System.Text.RegularExpressions;
using Microsoft.Recognizers.Definitions;

namespace Microsoft.Recognizers.Text.Sequence.Parsers
{
    public class CreditCardParser : BaseSequenceParser
    {

        // @TODO move regexes to base resource files

        public override ParseResult Parse(ExtractResult extResult)
        {
            var result = new ParseResult
            {
                Start = extResult.Start,
                Length = extResult.Length,
                Text = extResult.Text,
                Type = extResult.Type,
                ResolutionStr = extResult.Text,
                Value = extResult.Text,
                Issuer = extResult.Issuer,
                Validation = extResult.Validation,

            };

            return result;
        }
    }
}
