using System.Collections.Concurrent;
using System.Collections.Immutable;
using System.Text.RegularExpressions;

using Microsoft.Recognizers.Definitions.Portuguese;

namespace Microsoft.Recognizers.Text.Number.Portuguese
{
    public class NumberExtractor : BaseNumberExtractor
    {
        internal sealed override ImmutableDictionary<Regex, TypeTag> Regexes { get; }

        protected sealed override NumberOptions Options { get; }

        protected sealed override string ExtractType { get; } = Constants.SYS_NUM; // "Number";

        private static readonly ConcurrentDictionary<(NumberMode, NumberOptions), NumberExtractor> Instances =
            new ConcurrentDictionary<(NumberMode, NumberOptions), NumberExtractor>();

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

        private NumberExtractor(NumberMode mode = NumberMode.Default, NumberOptions options = NumberOptions.None)
        {

            Options = options;

            var builder = ImmutableDictionary.CreateBuilder<Regex, TypeTag>();

            //Add Cardinal
            CardinalExtractor cardExtract = null;
            switch (mode)
            {
                case NumberMode.PureNumber:
                    cardExtract = CardinalExtractor.GetInstance(NumbersDefinitions.PlaceHolderPureNumber);
                    break;
                case NumberMode.Currency:
                    builder.Add(new Regex(NumbersDefinitions.CurrencyRegex, RegexOptions.Singleline), RegexTagGenerator.GenerateRegexTag(Constants.FRACTION_PREFIX, Constants.NUMBER_SUFFIX));
                    break;
                case NumberMode.Default:
                    break;
            }

            if (cardExtract == null)
            {
                cardExtract = CardinalExtractor.GetInstance();
            }
            builder.AddRange(cardExtract.Regexes);

            //Add Fraction
            var fracExtract = FractionExtractor.GetInstance(Options);
            builder.AddRange(fracExtract.Regexes);

            this.Regexes = builder.ToImmutable();
        }
    }
}