using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Text.RegularExpressions;

using Microsoft.Recognizers.Definitions.Chinese;

namespace Microsoft.Recognizers.Text.Number.Chinese
{
    public class OrdinalExtractor : BaseNumberExtractor
    {
        internal sealed override ImmutableDictionary<Regex, string> Regexes { get; }

        protected sealed override string ExtractType { get; } = Constants.SYS_NUM_ORDINAL;

        private static readonly ConcurrentDictionary<string, OrdinalExtractor> Instances = new ConcurrentDictionary<string, OrdinalExtractor>();

        public static OrdinalExtractor GetInstance(string placeholder = "")
        {

            if (!Instances.ContainsKey(placeholder))
            {
                var instance = new OrdinalExtractor();
                Instances.TryAdd(placeholder, instance);
            }

            return Instances[placeholder];
        }

        public OrdinalExtractor()
        {
            var regexes = new Dictionary<Regex, string>
            {
                {
                    //第一百五十四
                    new Regex(NumbersDefinitions.OrdinalRegexChs, RegexOptions.Singleline)
                    , "OrdinalChs"
                },
                {
                    //第２５６５,  第1234
                    new Regex(NumbersDefinitions.OrdinalNumbersRegex, RegexOptions.Singleline)
                    , "OrdinalChs"
                }
            };

            Regexes = regexes.ToImmutableDictionary();
        }
    }
}