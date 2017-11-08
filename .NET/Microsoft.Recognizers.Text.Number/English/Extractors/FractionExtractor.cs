using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Text.RegularExpressions;

using Microsoft.Recognizers.Definitions.English;

namespace Microsoft.Recognizers.Text.Number.English
{
    public class FractionExtractor : BaseNumberExtractor
    {
        internal sealed override ImmutableDictionary<Regex, string> Regexes { get; }

        protected sealed override string ExtractType { get; } = Constants.SYS_NUM_FRACTION; // "Fraction";

        private static readonly ConcurrentDictionary<string, FractionExtractor> Instances = new ConcurrentDictionary<string, FractionExtractor>();

        public static FractionExtractor GetInstance(string placeholder = "")
        {

            if (!Instances.ContainsKey(placeholder))
            {
                var instance = new FractionExtractor();
                Instances.TryAdd(placeholder, instance);
            }

            return Instances[placeholder];
        }

        private FractionExtractor()
        {
            var regexes = new Dictionary<Regex, string>
            {
                {
                    new Regex(NumbersDefinitions.FractionNotationWithSpacesRegex,
                        RegexOptions.IgnoreCase | RegexOptions.Singleline)
                    , "FracNum"
                },
                {
                    new Regex(NumbersDefinitions.FractionNotationRegex,
                        RegexOptions.IgnoreCase | RegexOptions.Singleline)
                    , "FracNum"
                },
                {
                    new Regex(
                        NumbersDefinitions.FractionNounRegex,
                        RegexOptions.IgnoreCase | RegexOptions.Singleline)
                    , "FracEng"
                },
                {
                    new Regex(
                        NumbersDefinitions.FractionNounWithArticleRegex, RegexOptions.IgnoreCase | RegexOptions.Singleline)
                    , "FracEng"
                },
                {
                    new Regex(
                        NumbersDefinitions.FractionPrepositionRegex,
                        RegexOptions.IgnoreCase | RegexOptions.Singleline)
                    , "FracEng"
                }
            };

            Regexes = regexes.ToImmutableDictionary();
        }
    }
}