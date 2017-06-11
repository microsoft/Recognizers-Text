using System.Collections.Generic;
using System.Collections.Immutable;
using System.Text.RegularExpressions;

namespace Microsoft.Recognizers.Text.Number.Chinese
{
    public class OrdinalExtractor : BaseNumberExtractor
    {
        internal sealed override ImmutableDictionary<Regex, string> Regexes { get; }
        protected sealed override string ExtractType { get; } = Constants.SYS_NUM_ORDINAL;

        public static string OrdinalRegexChs => $@"第{IntegerExtractor.AllIntRegexChs}";

        public OrdinalExtractor()
        {
            var regexes = new Dictionary<Regex, string>
            {
                //第一百五十四
                {
                    new Regex(
                        OrdinalRegexChs,
                        RegexOptions.Singleline)
                    , "OrdinalChs"
                },
                //第２５６５,  第1234
                {
                    new Regex(
                        $@"第{IntegerExtractor.ZeroToNineChsFullHalfRegexChs}+",
                        RegexOptions.Singleline)
                    , "OrdinalChs"
                }
            };
            Regexes = regexes.ToImmutableDictionary();
        }
    }
}