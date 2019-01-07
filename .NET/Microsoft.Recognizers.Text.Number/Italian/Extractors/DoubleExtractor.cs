using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Text.RegularExpressions;

using Microsoft.Recognizers.Definitions.Italian;

namespace Microsoft.Recognizers.Text.Number.Italian
{
    public class DoubleExtractor : BaseNumberExtractor
    {
        private static readonly ConcurrentDictionary<string, DoubleExtractor> Instances =
            new ConcurrentDictionary<string, DoubleExtractor>();

        private DoubleExtractor(string placeholder = NumbersDefinitions.PlaceHolderDefault)
        {
            this.Regexes = new Dictionary<Regex, TypeTag>
            {
                {
                    new Regex(NumbersDefinitions.DoubleDecimalPointRegex(placeholder), RegexOptions.Singleline),
                    RegexTagGenerator.GenerateRegexTag(Constants.DOUBLE_PREFIX, Constants.NUMBER_SUFFIX)
                },
                {
                    new Regex(NumbersDefinitions.DoubleWithoutIntegralRegex(placeholder), RegexOptions.Singleline),
                    RegexTagGenerator.GenerateRegexTag(Constants.DOUBLE_PREFIX, Constants.NUMBER_SUFFIX)
                },
                {
                    new Regex(NumbersDefinitions.DoubleWithMultiplierRegex, RegexOptions.Singleline),
                    RegexTagGenerator.GenerateRegexTag(Constants.DOUBLE_PREFIX, Constants.NUMBER_SUFFIX)
                },
                {
                    new Regex(NumbersDefinitions.DoubleWithRoundNumber, RegexOptions.Singleline),
                    RegexTagGenerator.GenerateRegexTag(Constants.DOUBLE_PREFIX, Constants.NUMBER_SUFFIX)
                },
                {
                    new Regex(NumbersDefinitions.DoubleAllFloatRegex, RegexOptions.Singleline),
                    RegexTagGenerator.GenerateRegexTag(Constants.DOUBLE_PREFIX, Constants.ITALIAN)
                },
                {
                    new Regex(NumbersDefinitions.DoubleExponentialNotationRegex, RegexOptions.Singleline),
                    RegexTagGenerator.GenerateRegexTag(Constants.DOUBLE_PREFIX, Constants.POWER_SUFFIX)
                },
                {
                    new Regex(NumbersDefinitions.DoubleCaretExponentialNotationRegex, RegexOptions.Singleline),
                    RegexTagGenerator.GenerateRegexTag(Constants.DOUBLE_PREFIX, Constants.POWER_SUFFIX)
                },
                {
                    GenerateLongFormatNumberRegexes(LongFormatType.DoubleNumDotComma, placeholder),
                    RegexTagGenerator.GenerateRegexTag(Constants.DOUBLE_PREFIX, Constants.NUMBER_SUFFIX)
                },
                {
                    GenerateLongFormatNumberRegexes(LongFormatType.DoubleNumNoBreakSpaceComma, placeholder),
                    RegexTagGenerator.GenerateRegexTag(Constants.DOUBLE_PREFIX, Constants.NUMBER_SUFFIX)
                },
            }.ToImmutableDictionary();
        }

        internal sealed override ImmutableDictionary<Regex, TypeTag> Regexes { get; }

        protected sealed override string ExtractType { get; } = Constants.SYS_NUM_DOUBLE;

        public static DoubleExtractor GetInstance(string placeholder = NumbersDefinitions.PlaceHolderDefault)
        {
            if (!Instances.ContainsKey(placeholder))
            {
                var instance = new DoubleExtractor(placeholder);
                Instances.TryAdd(placeholder, instance);
            }

            return Instances[placeholder];
        }
    }
}
