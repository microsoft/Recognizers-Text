using System.Collections.Generic;
using System.Collections.Immutable;
using System.Text.RegularExpressions;

namespace Microsoft.Recognizers.Text.Number.French
{
    public class IntegerExtractor : BaseNumberExtractor

    {

        internal sealed override ImmutableDictionary<Regex, string> Regexes { get; }

        protected sealed override string ExtractType { get; } = Constants.SYS_NUM_INTEGER; // "Integer";


        public const string RoundNumberIntegerRegex = @"(cent|mille|million|milliard|billion)";


        public const string ZeroToNineIntegerRegex = @"(un|une|deux|trois|quatre|cinq|six|sept|huit|neuf)";


        public const string AnIntRegex = @"(un-)(?=\s)";


        public const string TenToNineteenIntegerRegex =
            @"(dix|onze|douze|treize|quatorze|quinze|seize|dix-sept|dix-huit|dix-neuf)";


        public const string TensNumberIntegerRegex = @"(vingt|trente|quarante|cinqaunte|soixante|soixante-dix|septante|quatre-vingts|huitante|octante|quatre-vingt-dix|nonante)";


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
                    new Regex($@"(((?<!\d+\s*)-\s*)|(?<=\b))\d+(?!(\,\d+[a-zA-Z]))(?={placeholder})",
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
                        @"(((?<!\d+\s*)-\s*)|(?<=\b))\d+\s+douzaine(s)?(?=\b)",
                        RegexOptions.IgnoreCase | RegexOptions.Singleline),
                    "IntegerNum"
                },
                {
                    new Regex(
                        $@"((?<=\b){AllIntRegex}(?=\b))",
                        RegexOptions.IgnoreCase | RegexOptions.Singleline),
                    "IntegerFr"
                },
                {
                    new Regex(
                        $@"(?<=\b)(((demi\s+)?a\s+douzaine)|({AllIntRegex}\s+douzaine(s)?))(?=\b)",
                        RegexOptions.IgnoreCase | RegexOptions.Singleline),
                    "IntegerFr"
                }
            };
            Regexes = _regexes.ToImmutableDictionary();
        }
    }
}
