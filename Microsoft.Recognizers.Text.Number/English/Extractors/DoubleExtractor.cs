using System.Collections.Generic;
using System.Collections.Immutable;
using System.Text.RegularExpressions;

namespace Microsoft.Recognizers.Text.Number.English
{
    public class DoubleExtractor : BaseNumberExtractor
    {
        internal sealed override ImmutableDictionary<Regex, string> Regexes { get; }
        protected sealed override string ExtractType { get; } = Constants.SYS_NUM_DOUBLE; // "Double";

        public static string AllPointRegex
            => $@"((\s+{IntegerExtractor.ZeroToNineIntegerRegex})+|(\s+{IntegerExtractor.SeparaIntRegex}))";

        public static string AllFloatRegex => $@"{IntegerExtractor.AllIntRegex}(\s+point){AllPointRegex}";

        public DoubleExtractor(string placeholder = @"\D|\b")
        {
            var _regexes = new Dictionary<Regex, string>
            {
                {
                    new Regex($@"(((?<!\d+\s*)-\s*)|((?<=\b)(?<!\d+\.)))\d+\.\d+(?!(\.\d+))(?={placeholder})",
                        RegexOptions.IgnoreCase | RegexOptions.Singleline),
                    "DoubleNum"
                },
                {
                    new Regex($@"(?<=\s|^)(?<!(\d+))\.\d+(?!(\.\d+))(?={placeholder})",
                        RegexOptions.IgnoreCase | RegexOptions.Singleline),
                    "DoubleNum"
                },
                {
                    new Regex(@"(((?<!\d+\s*)-\s*)|((?<=\b)(?<!\d+\.)))\d{1,3}(,\d{3})+\.\d+" + $@"(?={placeholder})",
                        RegexOptions.IgnoreCase | RegexOptions.Singleline),
                    "DoubleNum"
                },
                {
                    new Regex(@"(((?<!\d+\s*)-\s*)|((?<=\b)(?<!\d+\.)))\d+\.\d+\s*(K|k|M|G|T|B|b)(?=\b)",
                        RegexOptions.Singleline),
                    "DoubleNum"
                },
                {
                    new Regex(
                        $@"(((?<!\d+\s*)-\s*)|((?<=\b)(?<!\d+\.)))\d+\.\d+\s+{IntegerExtractor.RoundNumberIntegerRegex}(?=\b)",
                        RegexOptions.IgnoreCase | RegexOptions.Singleline),
                    "DoubleNum"
                },
                {
                    new Regex($@"((?<=\b){AllFloatRegex}(?=\b))", RegexOptions.IgnoreCase | RegexOptions.Singleline),
                    "DoubleEng"
                },
                {
                    new Regex(@"(((?<!\d+\s*)-\s*)|((?<=\b)(?<!\d+\.)))(\d+(\.\d+)?)e([+-]*[1-9]\d*)(?=\b)",
                        RegexOptions.IgnoreCase | RegexOptions.Singleline),
                    "DoublePow"
                },
                {
                    new Regex(@"(((?<!\d+\s*)-\s*)|((?<=\b)(?<!\d+\.)))(\d+(\.\d+)?)\^([+-]*[1-9]\d*)(?=\b)",
                        RegexOptions.IgnoreCase | RegexOptions.Singleline),
                    "DoublePow"
                }
            };
            Regexes = _regexes.ToImmutableDictionary();
        }
    }
}