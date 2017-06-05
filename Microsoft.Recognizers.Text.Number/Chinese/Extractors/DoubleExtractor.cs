using System.Collections.Generic;
using System.Collections.Immutable;
using System.Text.RegularExpressions;

namespace Microsoft.Recognizers.Text.Number.Chinese
{
    public class DoubleExtractor : BaseNumberExtractor
    {
        internal sealed override ImmutableDictionary<Regex, string> Regexes { get; }
        protected sealed override string ExtractType { get; } = Constants.SYS_NUM_DOUBLE;
        public static string PointRegexChsStr => @"[点點\.．]";

        public static string AllFloatRegexChs =>
            $@"{IntegerExtractor.SignSymbolRegexChs}?{IntegerExtractor.AllIntRegexChs}\s*{PointRegexChsStr}(\s*{IntegerExtractor
                .ZeroToNineIntegerRegexChs})+";

        public DoubleExtractor()
        {
            var regexes = new Dictionary<Regex, string>()
            {
                {
                    new Regex(
                        $@"(?<!({IntegerExtractor.ZeroToNineChsFullHalfRegexChs}+[\.．]{IntegerExtractor
                            .ZeroToNineChsFullHalfRegexChs}*))({IntegerExtractor.SignSymbolRegexNum}\s*)?{IntegerExtractor
                                .ZeroToNineChsFullHalfRegexChs}+[\.．]{IntegerExtractor.ZeroToNineChsFullHalfRegexChs}+(?!{IntegerExtractor
                                    .ZeroToNineChsFullHalfRegexChs}*[\.．]{IntegerExtractor
                                        .ZeroToNineChsFullHalfRegexChs}+)",
                        RegexOptions.IgnoreCase | RegexOptions.Singleline),
                    "DoubleNum"
                },
                {
                    // (-)2.5, can avoid cases like ip address xx.xx.xx.xx
                    new Regex(
                        $@"(?<!({IntegerExtractor.ZeroToNineChsFullHalfRegexChs}+|\.\.|．．))({IntegerExtractor
                            .SignSymbolRegexNum}\s*)?[\.．]{IntegerExtractor
                                .ZeroToNineChsFullHalfRegexChs}+(?!{IntegerExtractor.ZeroToNineChsFullHalfRegexChs}*([\.．]{IntegerExtractor
                                    .ZeroToNineChsFullHalfRegexChs}+))",
                        RegexOptions.IgnoreCase | RegexOptions.Singleline),
                    "DoubleNum"
                },
                //(-).2 
                {
                    new Regex(
                        $@"({IntegerExtractor.SignSymbolRegexNum}\s*)?{IntegerExtractor
                            .ZeroToNineChsFullHalfRegexChs}" + @"{1,3}([,，]" +
                        $@"{IntegerExtractor.ZeroToNineChsFullHalfRegexChs}" + @"{3})+[\.．]" +
                        $@"{IntegerExtractor.ZeroToNineChsFullHalfRegexChs}+",
                        RegexOptions.IgnoreCase | RegexOptions.Singleline),
                    "DoubleNum"
                },
                // 1.0 K
                {
                    new Regex(
                        $@"({IntegerExtractor.SignSymbolRegexNum}\s*)?{IntegerExtractor
                            .ZeroToNineChsFullHalfRegexChs}+[\.．]{IntegerExtractor
                                .ZeroToNineChsFullHalfRegexChs}+\s*(K|k|M|G|T|Ｍ|Ｋ|ｋ|Ｇ|Ｔ)",
                        RegexOptions.Singleline),
                    "DoubleNum"
                },
                //１５.２万
                {
                    new Regex(
                        $@"{IntegerExtractor.SignSymbolRegexChs}?{IntegerExtractor.ZeroToNineChsFullHalfRegexChs}+([\.．]{IntegerExtractor
                            .ZeroToNineChsFullHalfRegexChs}+)?\s*[多几幾余]?[万亿萬億]" + @"{1,2}",
                        RegexOptions.Singleline),
                    "DoubleChs"
                },
                //四十五点三三
                {
                    new Regex(
                        $@"(?<![百佰]\s*分\s*之\s*(({IntegerExtractor.AllIntRegexChs}[点點]*)|{AllFloatRegexChs})*){AllFloatRegexChs}(?!{IntegerExtractor
                            .ZeroToNineIntegerRegexChs}*\s*[个個]\s*[百佰]\s*分\s*[点點])",
                        RegexOptions.Singleline),
                    "DoubleChs"
                },
                // 2e6, 21.2e0
                {
                    new Regex(
                        $@"(?<!{IntegerExtractor.ZeroToNineChsFullHalfRegexChs}+[\.．])({IntegerExtractor
                            .SignSymbolRegexNum}\s*)?{IntegerExtractor
                                .ZeroToNineChsFullHalfRegexChs}+([\.．]{IntegerExtractor.ZeroToNineChsFullHalfRegexChs}+)?e(([-－+＋]*[1-9１２３４５６７８９]{IntegerExtractor
                                    .ZeroToNineChsFullHalfRegexChs}*)|[0０](?!{IntegerExtractor
                                        .ZeroToNineChsFullHalfRegexChs}+))",
                        RegexOptions.IgnoreCase | RegexOptions.Singleline),
                    "DoublePow"
                },
                //2^5
                {
                    new Regex(
                        $@"(?<!{IntegerExtractor.ZeroToNineChsFullHalfRegexChs}+[\.．])({IntegerExtractor
                            .SignSymbolRegexNum}\s*)?({IntegerExtractor
                                .ZeroToNineChsFullHalfRegexChs}+([\.．]{IntegerExtractor.ZeroToNineChsFullHalfRegexChs}+)?)\^([-－+＋]*[1-9１２３４５６７８９]{IntegerExtractor
                                    .ZeroToNineChsFullHalfRegexChs}*)",
                        RegexOptions.IgnoreCase | RegexOptions.Singleline),
                    "DoublePow"
                }
            };
            Regexes = regexes.ToImmutableDictionary();
        }
    }
}