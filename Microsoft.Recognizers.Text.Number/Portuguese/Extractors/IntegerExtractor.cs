using System.Collections.Generic;
using System.Collections.Immutable;
using System.Text.RegularExpressions;

namespace Microsoft.Recognizers.Text.Number.Portuguese
{
    public class IntegerExtractor : BaseNumberExtractor
    {
        internal sealed override ImmutableDictionary<Regex, string> Regexes { get; }

        protected sealed override string ExtractType { get; } = Constants.SYS_NUM_INTEGER;

        public const string HundredsNumberIntegerRegex = @"(quatrocent[ao]s|trezent[ao]s|seiscent[ao]s|setecent[ao]s|oitocent[ao]s|novecent[ao]s|duzent[ao]s|quinhent[ao]s|cem|(?<!por\s+)(cento))";

        public const string RoundNumberIntegerRegex = @"(mil|milh[ãa]o|milh[õo]es|bilh[ãa]o|bilh[õo]es|trilh[ãa]o|trilh[õo]es|qua[td]rilh[ãa]o|qua[td]rilh[õo]es|quintilh[ãa]o|quintilh[õo]es)";

        public const string ZeroToNineIntegerRegex = @"(quatro|cinco|sete|nove|zero|tr[êe]s|seis|oito|dois|duas|um|uma)";

        public const string TenToNineteenIntegerRegex = @"(dez[ea]sseis|dez[ea]ssete|dez[ea]nove|dezoito|quatorze|catorze|quinze|treze|d[ée]z|onze|doze)";

        public const string TensNumberIntegerRegex = @"(cinquenta|quarenta|trinta|sessenta|setenta|oitenta|noventa|vinte)";

        public const string DigitsNumberRegex = @"\d|\d{1,3}(\.\d{3})";

        public static string BelowHundredsRegex => $@"(({TenToNineteenIntegerRegex}|({TensNumberIntegerRegex}(\s+e\s+{ZeroToNineIntegerRegex})?))|{ZeroToNineIntegerRegex})";

        public static string BelowThousandsRegex => $@"({HundredsNumberIntegerRegex}(\s+e\s+{BelowHundredsRegex})?|{BelowHundredsRegex})";

        public static string SupportThousandsRegex => $@"(({BelowThousandsRegex}|{BelowHundredsRegex})\s+{RoundNumberIntegerRegex}(\s+{RoundNumberIntegerRegex})?)";

        public static string SeparaIntRegex => $@"({SupportThousandsRegex}(\s+{SupportThousandsRegex})*(\s+{BelowThousandsRegex})?|{BelowThousandsRegex})";

        public static string AllIntRegex => $@"({SeparaIntRegex}|mil(\s+{BelowThousandsRegex})?)";

        public IntegerExtractor(string placeholder = @"\D|\b")
        {
            var regexes = new Dictionary<Regex, string>
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
                    new Regex(@"(((?<=\W|^)-\s*)|(?<=\b))\d+(\s+d[úu]zia(s))?(?=\b)",
                        RegexOptions.IgnoreCase | RegexOptions.Singleline),
                    "IntegerNum"
                },
                {
                    new Regex(
                        @"(((?<!\d+\s*)-\s*)|(?<=\b))\d+\s+dezena(s)?(?=\b)",
                        RegexOptions.IgnoreCase | RegexOptions.Singleline),
                    "IntegerNum"
                },
                {
                    new Regex($@"((?<=\b){AllIntRegex}(?=\b))",
                        RegexOptions.IgnoreCase | RegexOptions.Singleline),
                    "IntegerPor"
                },
                {
                new Regex($@"(?<=\b)(((meia)?\s+(d[úu]zia))|({AllIntRegex}\s+(e|com)\s+)?({AllIntRegex}\s+(d[úu]zia(s)?|dezena(s)?)))(?=\b)",
                        RegexOptions.IgnoreCase | RegexOptions.Singleline),
                    "IntegerPor"
                }
            };

            this.Regexes = regexes.ToImmutableDictionary();
        }
    }
}