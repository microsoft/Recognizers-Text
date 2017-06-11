using System.Collections.Generic;
using System.Collections.Immutable;
using System.Text.RegularExpressions;

namespace Microsoft.Recognizers.Text.Number.French
{
    class IntegerExtractor : BaseNumberExtractor
    {
        internal sealed override ImmutableDictionary<Regex, string> Regexes { get; }

        protected sealed override string ExtractType { get; } = Constants.SYS_NUM_INTEGER;

        public const string PostTwentyAndOneIntRegex = @"(et-un)";

        public const string RoundNumberIntegerRegex = @"(cent|mille|un-million|un-milliard|un-billion|un-billiard|un-trillion)";

        public const string ZeroToNineIntegerRegex = @"(z[eé]ro|un|duex|trois|quatre|cinq|six|sept|huit|neuf)";

        public const string AnIntRegex = @"(un|une)(?=\s)"; 

        public const string TenToNineteenIntegerRegex = @"(dix|onze|douze|treize|quatorze|quinze|seize|dix-sept|dix-huit|dix-neuf)";

        public const string TensNumberIntegerRegex = 
            @"(vingt|trente|quarante|cinquante|soixante|soixante-dix|sepante|quatre-vingts|huitante|octante|quatre-vingt-dix|nonante)";

        public static string SeparaIntRegex 
            =>
               $@"((({TenToNineteenIntegerRegex}|({TensNumberIntegerRegex}(\s+(et\s+)?|\s*-\s*){ZeroToNineIntegerRegex})
                    |{TensNumberIntegerRegex}|{ZeroToNineIntegerRegex})(\s+{RoundNumberIntegerRegex})*))
                    |(({AnIntRegex}(\s+{RoundNumberIntegerRegex})+))";

        public static string AllIntRegex
           =>
               $@"(((({TenToNineteenIntegerRegex}|({TensNumberIntegerRegex}(\s+(et\s+)?|\s*-\s*){
                    ZeroToNineIntegerRegex})|{TensNumberIntegerRegex}|{ZeroToNineIntegerRegex}|{AnIntRegex})(\s+{
                    RoundNumberIntegerRegex})+)\s+(et\s+)?)*{SeparaIntRegex})";
    

        public IntegerExtractor(string placeholder = @"\D|\b")
        {
            // ((?<=\W|^)-\s*)|(?<=\b))\d    for all of these ---> matches any non-word char (W) or match starts at beginning of line (^),
            // to (-) any white space char (s) zero or more times (*)  OR boundary match any decimal digit 

            var _regexes = new Dictionary<Regex, string>
            {
                {
                    // any decimal digit separated by ',' and any digit or digit representation ex "1,three", "2,5" no spaces  
                    new Regex($@"(((?<=\W|^)-\s*)|(?<=\b))\d+(?!(,\d+[a-zA-Z]))(?={placeholder})",
                    RegexOptions.Compiled | RegexOptions.IgnoreCase | RegexOptions.Singleline)
                    , "IntegerNum" 
                },
                {   
                    new Regex(@"(((?<=\W|^)-\s*)|(?<=\b))\d+\s*(K|k|M|T|G)(?=\b)",
                    RegexOptions.Compiled | RegexOptions.Singleline)
                    , "IntegerNum"
                },
                {
                    new Regex(@"(((?<=\W|^)-\s*)|(?<=\b))\d{1,3}(\.\d{3})+" + $@"(?={placeholder})",
                        RegexOptions.Compiled | RegexOptions.IgnoreCase | RegexOptions.Singleline),
                    "IntegerNum"
                },
                {
                    new Regex($@"(?<=\b)\d+\s+{RoundNumberIntegerRegex}(?=\b)",
                        RegexOptions.Compiled | RegexOptions.IgnoreCase | RegexOptions.Singleline),
                    "IntegerNum"
                },
                {
                    new Regex(@"(((?<=\W|^)-\s*)|(?<=\b))\d+\s+douzaines?(?=\b)",
                        RegexOptions.Compiled | RegexOptions.IgnoreCase | RegexOptions.Singleline),
                    "IntegerNum"
                },
                {
                    new Regex($@"((?<=\b){AllIntRegex}(?=\b))",
                        RegexOptions.Compiled | RegexOptions.IgnoreCase | RegexOptions.Singleline),
                    "IntegerFr"
                },
                {
                new Regex($@"(?<=\b)(((demi\s+)?-\s+douzaines)|({AllIntRegex}\s+douzaines?))(?=\b)",
                        RegexOptions.Compiled | RegexOptions.IgnoreCase | RegexOptions.Singleline),
                    "IntegerFr"
                }
            };
            Regexes = _regexes.ToImmutableDictionary();
        }
            
    }
}
