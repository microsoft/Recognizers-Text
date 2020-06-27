using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Text.RegularExpressions;

using Microsoft.Recognizers.Definitions.French;

namespace Microsoft.Recognizers.Text.Number.French
{
    public class DoubleExtractor : BaseNumberExtractor
    {

        private const RegexOptions RegexFlags = RegexOptions.Singleline | RegexOptions.ExplicitCapture;

        private static readonly ConcurrentDictionary<string, DoubleExtractor> Instances =
            new ConcurrentDictionary<string, DoubleExtractor>();

        private DoubleExtractor(string placeholder = NumbersDefinitions.PlaceHolderDefault)
        {
            this.Regexes = new Dictionary<Regex, TypeTag>
            {
                {
                    new Regex(NumbersDefinitions.DoubleDecimalPointRegex(placeholder), RegexFlags),
                    RegexTagGenerator.GenerateRegexTag(Constants.DOUBLE_PREFIX, Constants.NUMBER_SUFFIX)
                },
                {
                    new Regex(NumbersDefinitions.DoubleWithoutIntegralRegex(placeholder), RegexFlags),
                    RegexTagGenerator.GenerateRegexTag(Constants.DOUBLE_PREFIX, Constants.NUMBER_SUFFIX)
                },
                {
                    new Regex(NumbersDefinitions.DoubleWithMultiplierRegex, RegexFlags),
                    RegexTagGenerator.GenerateRegexTag(Constants.DOUBLE_PREFIX, Constants.NUMBER_SUFFIX)
                },
                {
                    new Regex(NumbersDefinitions.DoubleWithRoundNumber, RegexFlags),
                    RegexTagGenerator.GenerateRegexTag(Constants.DOUBLE_PREFIX, Constants.NUMBER_SUFFIX)
                },
                {
                    new Regex(NumbersDefinitions.DoubleAllFloatRegex, RegexFlags),
                    RegexTagGenerator.GenerateRegexTag(Constants.DOUBLE_PREFIX, Constants.FRENCH)
                },
                {
                    new Regex(NumbersDefinitions.DoubleExponentialNotationRegex, RegexFlags),
                    RegexTagGenerator.GenerateRegexTag(Constants.DOUBLE_PREFIX, Constants.POWER_SUFFIX)
                },
                {
                    new Regex(NumbersDefinitions.DoubleCaretExponentialNotationRegex, RegexFlags),
                    RegexTagGenerator.GenerateRegexTag(Constants.DOUBLE_PREFIX, Constants.POWER_SUFFIX)
                },
                {
                    GenerateLongFormatNumberRegexes(LongFormatType.DoubleNumDotComma, placeholder, RegexFlags),
                    RegexTagGenerator.GenerateRegexTag(Constants.DOUBLE_PREFIX, Constants.NUMBER_SUFFIX)
                },
                {
                    GenerateLongFormatNumberRegexes(LongFormatType.DoubleNumNoBreakSpaceComma, placeholder, RegexFlags),
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
