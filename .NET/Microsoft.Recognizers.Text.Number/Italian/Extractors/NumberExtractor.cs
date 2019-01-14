using System.Collections.Concurrent;
using System.Collections.Immutable;
using System.Text.RegularExpressions;

using Microsoft.Recognizers.Definitions.Italian;

namespace Microsoft.Recognizers.Text.Number.Italian
{
    public class NumberExtractor : BaseNumberExtractor
    {
        private static readonly ConcurrentDictionary<(NumberMode, NumberOptions), NumberExtractor> Instances =
            new ConcurrentDictionary<(NumberMode, NumberOptions), NumberExtractor>();

        private NumberExtractor(NumberMode mode, NumberOptions options)
        {
            Options = options;

            var builder = ImmutableDictionary.CreateBuilder<Regex, TypeTag>();

            CardinalExtractor cardExtract = null;
            switch (mode)
            {
                case NumberMode.PureNumber:
                    cardExtract = CardinalExtractor.GetInstance(NumbersDefinitions.PlaceHolderPureNumber);
                    break;
                case NumberMode.Currency:
                    builder.Add(
                        BaseNumberExtractor.CurrencyRegex,
                        RegexTagGenerator.GenerateRegexTag(Constants.INTEGER_PREFIX, Constants.NUMBER_SUFFIX));
                    break;
                case NumberMode.Default:
                    break;
                default:
                    break;
            }

            if (cardExtract == null)
            {
                cardExtract = CardinalExtractor.GetInstance();
            }

            builder.AddRange(cardExtract.Regexes);

            var fracExtract = FractionExtractor.GetInstance(Options);
            builder.AddRange(fracExtract.Regexes);

            Regexes = builder.ToImmutable();
        }

        internal sealed override ImmutableDictionary<Regex, TypeTag> Regexes { get; }

        protected sealed override NumberOptions Options { get; }

        protected sealed override string ExtractType { get; } = Constants.SYS_NUM;

        public static NumberExtractor GetInstance(NumberMode mode = NumberMode.Default, NumberOptions options = NumberOptions.None)
        {
            var cacheKey = (mode, options);
            if (!Instances.ContainsKey(cacheKey))
            {
                var instance = new NumberExtractor(mode, options);
                Instances.TryAdd(cacheKey, instance);
            }

            return Instances[cacheKey];
        }
    }
}
