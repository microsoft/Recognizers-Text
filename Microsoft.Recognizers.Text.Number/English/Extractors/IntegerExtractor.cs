using Microsoft.Recognizers.Text.Number.Extractors;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Text.RegularExpressions;

namespace Microsoft.Recognizers.Text.Number.English.Extractors
{
    public class IntegerExtractor : BaseNumberExtractor
    {
        internal sealed override ImmutableDictionary<Regex, string> Regexes { get; }
        protected sealed override string ExtractType { get; } = Constants.SYS_NUM_INTEGER; // "Integer";

        public const string RoundNumberIntegerRegex = @"(hundred|thousand|million|billion|trillion)";

        public const string ZeroToNineIntegerRegex = @"(three|seven|eight|four|five|zero|nine|one|two|six)";

        public const string AnIntRegex = @"(an|a)(?=\s)";

        public const string TenToNineteenIntegerRegex =
            @"(seventeen|thirteen|fourteen|eighteen|nineteen|fifteen|sixteen|eleven|twelve|ten)";

        public const string TensNumberIntegerRegex = @"(seventy|twenty|thirty|eighty|ninety|forty|fifty|sixty)";

        public static string SeparaIntRegex
            =>
                $@"((({TenToNineteenIntegerRegex}|({TensNumberIntegerRegex}(\s+(and\s+)?|\s*-\s*){ZeroToNineIntegerRegex
                    })|{TensNumberIntegerRegex}|{ZeroToNineIntegerRegex})(\s+{RoundNumberIntegerRegex})*))|(({AnIntRegex
                    }(\s+{RoundNumberIntegerRegex})+))";

        public static string AllIntRegex
            =>
                $@"(((({TenToNineteenIntegerRegex}|({TensNumberIntegerRegex}(\s+(and\s+)?|\s*-\s*){
                    ZeroToNineIntegerRegex})|{TensNumberIntegerRegex}|{ZeroToNineIntegerRegex}|{AnIntRegex})(\s+{
                    RoundNumberIntegerRegex})+)\s+(and\s+)?)*{SeparaIntRegex})";

        public IntegerExtractor(string placeholder = @"\D|\b")
        {
            var _regexes = new Dictionary<Regex, string>
            {
                {
                    new Regex($@"(((?<!\d+\s*)-\s*)|(?<=\b))\d+(?!(\.\d+[a-zA-Z]))(?={placeholder})",
                        RegexOptions.IgnoreCase | RegexOptions.Singleline)
                    , "IntegerNum"
                },
                {
                    new Regex(@"(((?<!\d+\s*)-\s*)|(?<=\b))\d+\s*(K|k|M|T|G)(?=\b)", RegexOptions.Singleline)
                    , "IntegerNum"
                },
                {
                    new Regex(@"(((?<!\d+\s*)-\s*)|(?<=\b))\d{1,3}(,\d{3})+" + $@"(?={placeholder})",
                        RegexOptions.IgnoreCase | RegexOptions.Singleline),
                    "IntegerNum"
                },
                {
                    new Regex($@"(?<=\b)\d+\s+{RoundNumberIntegerRegex}(?=\b)",
                        RegexOptions.IgnoreCase | RegexOptions.Singleline),
                    "IntegerNum"
                },
                {
                    new Regex(
                        @"(((?<!\d+\s*)-\s*)|(?<=\b))\d+\s+dozen(s)?(?=\b)",
                        RegexOptions.IgnoreCase | RegexOptions.Singleline),
                    "IntegerNum"
                },
                {
                    new Regex(
                        $@"((?<=\b){AllIntRegex}(?=\b))",
                        RegexOptions.IgnoreCase | RegexOptions.Singleline),
                    "IntegerEng"
                },
                {
                    new Regex(
                        $@"(?<=\b)(((half\s+)?a\s+dozen)|({AllIntRegex}\s+dozen(s)?))(?=\b)",
                        RegexOptions.IgnoreCase | RegexOptions.Singleline),
                    "IntegerEng"
                }
            };
            Regexes = _regexes.ToImmutableDictionary();
        }
    }
}