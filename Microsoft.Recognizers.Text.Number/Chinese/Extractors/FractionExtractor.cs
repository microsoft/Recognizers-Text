using System.Collections.Generic;
using System.Collections.Immutable;
using System.Text.RegularExpressions;

namespace Microsoft.Recognizers.Text.Number.Chinese
{
    public class FractionExtractor : BaseNumberExtractor
    {
        internal sealed override ImmutableDictionary<Regex, string> Regexes { get; }
        protected sealed override string ExtractType { get; } = Constants.SYS_NUM_FRACTION;

        public static string AllFractionNumber
            =>
                $@"{IntegerExtractor.SignSymbolRegexChs}?(({IntegerExtractor.ZeroToNineChsFullHalfRegexChs}+|{IntegerExtractor
                    .AllIntRegexChs})\s*又\s*)?{IntegerExtractor.SignSymbolRegexChs}?({IntegerExtractor
                        .ZeroToNineChsFullHalfRegexChs}+|{IntegerExtractor.AllIntRegexChs})\s*分\s*之\s*{IntegerExtractor
                            .SignSymbolRegexChs}?({IntegerExtractor.ZeroToNineChsFullHalfRegexChs}+|{IntegerExtractor
                                .AllIntRegexChs})";

        public FractionExtractor()
        {
            var regexes = new Dictionary<Regex, string>()
            {
                {
                    // -4 5/2,       ４ ６／３
                    new Regex(
                        $@"({IntegerExtractor.SignSymbolRegexNum}\s*)?{IntegerExtractor
                            .ZeroToNineChsFullHalfRegexChs}+\s+{IntegerExtractor
                                .ZeroToNineChsFullHalfRegexChs}+[/／]{IntegerExtractor.ZeroToNineChsFullHalfRegexChs}+",
                        RegexOptions.IgnoreCase | RegexOptions.Singleline)
                    , "FracNum"
                },
                // 8/3 
                {
                    new Regex(
                        $@"({IntegerExtractor.SignSymbolRegexNum}\s*)?{IntegerExtractor
                            .ZeroToNineChsFullHalfRegexChs}+[/／]{IntegerExtractor
                                .ZeroToNineChsFullHalfRegexChs}+",
                        RegexOptions.IgnoreCase | RegexOptions.Singleline)
                    , "FracNum"
                },
                //四分之六十五
                {
                    new Regex($@"{AllFractionNumber}", RegexOptions.Singleline)
                    , "FracChs"
                }
            };
            Regexes = regexes.ToImmutableDictionary();
        }
    }
}