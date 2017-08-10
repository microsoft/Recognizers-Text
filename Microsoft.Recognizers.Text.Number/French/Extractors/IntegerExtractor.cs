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

        public const string ZeroToNineIntegerRegex = @"(et un|un|une|deux|trois|quatre|cinq|six|sept|huit|neuf)";

        public const string TenToNineteenIntegerRegex =
            @"(dix\Wneuf|dix\Whuit|dix\Wsept|seize|quinze|quatorze|treize|douze|onze|dix)";

        public const string TensNumberIntegerRegex = @"(octante|vingt|trente|quarante|cinquante|soixante-dix|soixante|septante|huitante|quatre-vingt-dix|nonante)";

        public const string DigitsNumberRegex = @"\d|\d{1,3}(\.\d{3})";

        public static string HundredsNumberIntegerRegex = $@"(({ZeroToNineIntegerRegex}(\s+cent))|cent|((\s+cent\s)+{TensNumberIntegerRegex}))"; // work on this one

        public static string SupportThousandsRegex => $@"(({BelowThousandsRegex}|{BelowHundredsRegex})\s+{RoundNumberIntegerRegex}(\s+{RoundNumberIntegerRegex})?)";

        public static string BelowHundredsRegex => $@"(({TenToNineteenIntegerRegex}|({TensNumberIntegerRegex}(\W+{ZeroToNineIntegerRegex})?))|{ZeroToNineIntegerRegex})"; 

        public static string BelowThousandsRegex => $@"(({HundredsNumberIntegerRegex}(\s+{BelowHundredsRegex})?|{BelowHundredsRegex}|{TenToNineteenIntegerRegex})|cent\s+{TenToNineteenIntegerRegex})";

        public static string SeparaIntRegex
            =>
                 $@"({SupportThousandsRegex}(\s+{SupportThousandsRegex})*(\s+{BelowThousandsRegex})?|{BelowThousandsRegex})";

        public static string AllIntRegex
            =>
               $@"({SeparaIntRegex}|mille(\s+{BelowThousandsRegex})?)";

        public IntegerExtractor(string placeholder = @"\D|\b")
        {
            var regexes = new Dictionary<Regex, string>
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
                    new Regex($@"(?<=\b)({DigitsNumberRegex})+\s+{RoundNumberIntegerRegex}(?=\b)",
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
                        $@"(?<=\b)(((demi\s+)?\s+douzaine)|({AllIntRegex}\s+douzaines?))(?=\b)",
                        RegexOptions.IgnoreCase | RegexOptions.Singleline),
                    "IntegerFr"
                }
            };

            Regexes = regexes.ToImmutableDictionary();
        }
    }
}
