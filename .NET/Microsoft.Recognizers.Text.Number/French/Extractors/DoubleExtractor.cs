﻿using System.Collections.Generic;
using System.Collections.Immutable;
using System.Text.RegularExpressions;

namespace Microsoft.Recognizers.Text.Number.French
{
    public class DoubleExtractor : BaseNumberExtractor
    {
        internal sealed override ImmutableDictionary<Regex, string> Regexes { get; }

        protected sealed override string ExtractType { get; } = Constants.SYS_NUM_DOUBLE;

        public static string AllPointRegex => $@"((\s+{IntegerExtractor.ZeroToNineIntegerRegex})+|(\s+{IntegerExtractor.SeparaIntRegex}))";

        public static string AllFloatRegex => $@"{IntegerExtractor.AllIntRegex}(\s+(virgule|point)){AllPointRegex}";

        public DoubleExtractor(string placeholder = @"\D|\b")
        {
            var regexes = new Dictionary<Regex, string>
            {
                {
                    new Regex($@"(((?<!\d+\s*)-\s*)|((?<=\b)(?<!\d+,)))\d+,\d+(?!(,\d+))(?={placeholder})",
                        RegexOptions.IgnoreCase | RegexOptions.Singleline),
                    "DoubleNum"
                },
                {
                    new Regex($@"(?<=\s|^)(?<!(\d+)),\d+(?!(,\d+))(?={placeholder})",
                        RegexOptions.IgnoreCase | RegexOptions.Singleline),
                    "DoubleNum"
                },
                {
                    new Regex(@"(((?<!\d+\s*)-\s*)|((?<=\b)(?<!\d+\,)))\d+,\d+\s*(K|k|M|G|T)(?=\b)",
                        RegexOptions.Singleline),
                    "DoubleNum"
                },
                {
                    new Regex($@"(((?<!\d+\s*)-\s*)|((?<=\b)(?<!\d+\,)))\d+,\d+\s+{IntegerExtractor.RoundNumberIntegerRegex}(?=\b)",
                        RegexOptions.IgnoreCase | RegexOptions.Singleline),
                    "DoubleNum"
                },
                {
                    new Regex($@"((?<=\b){AllFloatRegex}(?=\b))",
                        RegexOptions.IgnoreCase | RegexOptions.Singleline),
                    "DoubleFr"
                },
                {
                    new Regex(@"(((?<!\d+\s*)-\s*)|((?<=\b)(?<!\d+,)))(\d+(,\d+)?)e([+-]*[1-9]\d*)(?=\b)",
                        RegexOptions.IgnoreCase | RegexOptions.Singleline),
                    "DoublePow"
                },
                {
                    new Regex(@"(((?<!\d+\s*)-\s*)|((?<=\b)(?<!\d+,)))(\d+(,\d+)?)\^([+-]*[1-9]\d*)(?=\b)",
                        RegexOptions.IgnoreCase | RegexOptions.Singleline),
                    "DoublePow"
                }
            };

            regexes.Add(GenerateLongFormatNumberRegexes(LongFormatType.DoubleNumDotComma, placeholder), "DoubleNum");
            this.Regexes = regexes.ToImmutableDictionary();
        }

    }
}
