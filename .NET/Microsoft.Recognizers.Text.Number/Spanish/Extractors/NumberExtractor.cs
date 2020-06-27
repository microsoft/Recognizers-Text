using System.Collections.Concurrent;
using System.Collections.Immutable;
using System.Text.RegularExpressions;

using Microsoft.Recognizers.Definitions.Spanish;

namespace Microsoft.Recognizers.Text.Number.Spanish
{
    public class NumberExtractor : CachedNumberExtractor
    {

        private const RegexOptions RegexFlags = RegexOptions.Singleline | RegexOptions.ExplicitCapture;

        private static readonly ConcurrentDictionary<(NumberMode, NumberOptions), NumberExtractor> Instances =
            new ConcurrentDictionary<(NumberMode, NumberOptions), NumberExtractor>();

        private readonly string keyPrefix;

        private NumberExtractor(BaseNumberOptionsConfiguration config)
            : base(config.Options)
        {

            keyPrefix = string.Intern(ExtractType + "_" + config.Options + "_" + config.Mode + "_" + config.Culture);

            NegativeNumberTermsRegex = new Regex(NumbersDefinitions.NegativeNumberTermsRegex + '$', RegexFlags);

            var builder = ImmutableDictionary.CreateBuilder<Regex, TypeTag>();

            // Add Cardinal
            CardinalExtractor cardExtract = null;
            switch (config.Mode)
            {
                case NumberMode.PureNumber:
                    var purNumConfig = new BaseNumberOptionsConfiguration(config.Culture, config.Options, config.Mode,
                                                                          NumbersDefinitions.PlaceHolderPureNumber);
                    cardExtract = CardinalExtractor.GetInstance(purNumConfig);
                    break;
                case NumberMode.Currency:
                    builder.Add(
                        CurrencyRegex,
                        RegexTagGenerator.GenerateRegexTag(Constants.INTEGER_PREFIX, Constants.NUMBER_SUFFIX));
                    break;
                case NumberMode.Default:
                    break;
            }

            if (cardExtract == null)
            {
                cardExtract = CardinalExtractor.GetInstance(config);
            }

            builder.AddRange(cardExtract.Regexes);

            // Add Fraction
            var fracExtract = FractionExtractor.GetInstance(config);
            builder.AddRange(fracExtract.Regexes);

            Regexes = builder.ToImmutable();

            var ambiguityBuilder = ImmutableDictionary.CreateBuilder<Regex, Regex>();

            // Do not filter the ambiguous number cases like '$2000' in NumberWithUnit, otherwise they can't be resolved.
            if (config.Mode != NumberMode.Unit)
            {
                foreach (var item in NumbersDefinitions.AmbiguityFiltersDict)
                {
                    ambiguityBuilder.Add(new Regex(item.Key, RegexFlags), new Regex(item.Value, RegexFlags));
                }
            }

            AmbiguityFiltersDict = ambiguityBuilder.ToImmutable();
        }

        internal sealed override ImmutableDictionary<Regex, TypeTag> Regexes { get; }

        protected sealed override ImmutableDictionary<Regex, Regex> AmbiguityFiltersDict { get; }

        // "Number"
        protected sealed override string ExtractType { get; } = Constants.SYS_NUM;

        protected sealed override Regex NegativeNumberTermsRegex { get; }

        public static NumberExtractor GetInstance(BaseNumberOptionsConfiguration config)
        {
            var extractorKey = (config.Mode, config.Options);

            if (!Instances.ContainsKey(extractorKey))
            {
                var instance = new NumberExtractor(config);
                Instances.TryAdd(extractorKey, instance);
            }

            return Instances[extractorKey];
        }

        protected override object GenKey(string input)
        {
            return (keyPrefix, input);
        }
    }
}