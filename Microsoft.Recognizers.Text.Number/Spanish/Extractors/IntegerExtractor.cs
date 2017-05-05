using Microsoft.Recognizers.Text.Number.Extractors;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Text.RegularExpressions;

namespace Microsoft.Recognizers.Text.Number.Spanish.Extractors
{
    public class IntegerExtractor : BaseNumberExtractor
    {
        internal override sealed ImmutableDictionary<Regex, string> Regexes { get; }

        protected override sealed string ExtractType { get; } = Constants.SYS_NUM_INTEGER;

        public const string HundredsNumberIntegerRegex = @"(cuatrocient[ao]s|trescient[ao]s|seiscient[ao]s|setecient[ao]s|ochocient[ao]s|novecient[ao]s|doscient[ao]s|quinient[ao]s|(?<!por\s+)(cien(to)?))";

        public const string RoundNumberIntegerRegex = @"(mil millones|mil|millones|mill[oó]n|billones|bill[oó]n|trillones|trill[oó]n|cuatrillones|cuatrill[oó]n|quintillones|quintill[oó]n|sextillones|sextill[oó]n|septillones|septill[oó]n)";

        public const string ZeroToNineIntegerRegex = @"(cuatro|cinco|siete|nueve|cero|tres|seis|ocho|dos|un[ao]?)";

        public const string TenToNineteenIntegerRegex = @"(diecisiete|diecinueve|diecis[eé]is|dieciocho|catorce|quince|trece|diez|once|doce)";

        public const string TwentiesIntegerRegex = @"(veinticuatro|veinticinco|veintisiete|veintinueve|veintitr[eé]s|veintis[eé]is|veintiocho|veintid[oó]s|ventiun[ao]|veinti[uú]n[oa]?|veinte)";

        public const string TensNumberIntegerRegex = @"(cincuenta|cuarenta|treinta|sesenta|setenta|ochenta|noventa)";

        public const string DigitsNumberRegex = @"\d|\d{1,3}(\.\d{3})";

        public static string BelowHundredsRegex => $@"(({TenToNineteenIntegerRegex}|{TwentiesIntegerRegex}|({TensNumberIntegerRegex}(\s+y\s+{ZeroToNineIntegerRegex})?))|{ZeroToNineIntegerRegex})";

        public static string BelowThousandsRegex => $@"({HundredsNumberIntegerRegex}(\s+{BelowHundredsRegex})?|{BelowHundredsRegex})";

        public static string SupportThousandsRegex => $@"(({BelowThousandsRegex}|{BelowHundredsRegex})\s+{RoundNumberIntegerRegex}(\s+{RoundNumberIntegerRegex})?)";

        public static string SeparaIntRegex => $@"({SupportThousandsRegex}(\s+{SupportThousandsRegex})*(\s+{BelowThousandsRegex})?|{BelowThousandsRegex})";

        public static string AllIntRegex => $@"({SeparaIntRegex}|mil(\s+{BelowThousandsRegex})?)";

        public IntegerExtractor(string placeholder = @"\D|\b")
        {
            var _regexes = new Dictionary<Regex, string>
            {
                {
                    new Regex($@"(((?<=\W|^)-\s*)|(?<=\b))\d+(?!(,\d+[a-zA-Z]))(?={placeholder})",
                    RegexOptions.IgnoreCase | RegexOptions.Singleline)
                    , "IntegerNum"
                },
                {
                    new Regex(@"(((?<=\W|^)-\s*)|(?<=\b))\d+\s*(k|M|T|G)(?=\b)",
                        RegexOptions.Singleline)
                    , "IntegerNum"
                },
                {
                    new Regex(@"(((?<=\W|^)-\s*)|(?<=\b))\d{1,3}(\.\d{3})+" + $@"(?={placeholder})",
                        RegexOptions.IgnoreCase | RegexOptions.Singleline),
                    "IntegerNum"
                },
                {
                    new Regex($@"(?<=\b)({DigitsNumberRegex})+\s+{RoundNumberIntegerRegex}(?=\b)",
                        RegexOptions.IgnoreCase | RegexOptions.Singleline),
                    "IntegerNum"
                },
                {
                    new Regex(@"(((?<=\W|^)-\s*)|(?<=\b))\d+\s+docenas?(?=\b)",
                        RegexOptions.IgnoreCase | RegexOptions.Singleline),
                    "IntegerNum"
                },
                {
                    new Regex($@"((?<=\b){AllIntRegex}(?=\b))",
                        RegexOptions.IgnoreCase | RegexOptions.Singleline),
                    "IntegerSpa"
                },
                {
                new Regex($@"(?<=\b)(((media\s+)?\s+docena)|({AllIntRegex}\s+(y|con)\s+)?({AllIntRegex}\s+docenas?))(?=\b)",
                        RegexOptions.IgnoreCase | RegexOptions.Singleline),
                    "IntegerSpa"
                }
            };

            this.Regexes = _regexes.ToImmutableDictionary();
        }
    }
}