using System.Collections.Generic;
using System.Collections.Immutable;
using System.Text.RegularExpressions;

namespace Microsoft.Recognizers.Text.Sequence.English
{
    public class EnglishQuotedTextExtractor : BaseQuotedTextExtractor
    {
        private static Dictionary<Regex, string> regexes = new Dictionary<Regex, string>
            {
                {
                    new Regex(@"(‘([^‘’]+)’)"),
                    Constants.QUOTED_TEXT_REGEX
                },
                {
                    new Regex(@"(“([^“”]+)”)"),
                    Constants.QUOTED_TEXT_REGEX
                },
                {
                    new Regex(@"(""([^""]+)"")"),
                    Constants.QUOTED_TEXT_REGEX
                },
                {
                    new Regex(@"(\'([^\']+)\')"),
                    Constants.QUOTED_TEXT_REGEX
                },
                {
                    new Regex(@"(`([^`]+)`)"),
                    Constants.QUOTED_TEXT_REGEX
                },
            };

        internal override ImmutableDictionary<Regex, string> Regexes { get; } = regexes.ToImmutableDictionary();
    }
}
