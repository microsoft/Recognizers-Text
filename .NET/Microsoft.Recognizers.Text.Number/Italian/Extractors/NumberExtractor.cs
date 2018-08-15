using System.Collections.Concurrent;
using System.Collections.Immutable;
using System.Text.RegularExpressions;

using Microsoft.Recognizers.Definitions.Italian;

namespace Microsoft.Recognizers.Text.Number.Italian
{
    public class NumberExtractor : BaseNumberExtractor
    {
        internal sealed override ImmutableDictionary<Regex, TypeTag> Regexes { get; }

        protected sealed override string ExtractType { get; } = Constants.SYS_NUM;

        private static readonly ConcurrentDictionary<string, NumberExtractor> Instances = new ConcurrentDictionary<string, NumberExtractor>();

        public static NumberExtractor GetInstance(NumberMode mode = NumberMode.Default)
        {

            var placeholder = mode.ToString();

            if (!Instances.ContainsKey(placeholder))
            {
                var instance = new NumberExtractor(mode);
                Instances.TryAdd(placeholder, instance);
            }

            return Instances[placeholder];
        }

        public NumberExtractor(NumberMode mode = NumberMode.Default)
        {
            var builder = ImmutableDictionary.CreateBuilder<Regex, TypeTag>();

            CardinalExtractor cardExtract = null;
            switch(mode)
            {
                case NumberMode.PureNumber:
                    cardExtract = new CardinalExtractor(NumbersDefinitions.PlaceHolderPureNumber);
                    break;
                case NumberMode.Currency:
                    builder.Add(new Regex(NumbersDefinitions.CurrencyRegex, RegexOptions.Singleline),
                                RegexTagGenerator.GenerateRegexTag(Constants.INTEGER_PREFIX, Constants.NUMBER_SUFFIX));
                    break;
                case NumberMode.Default:
                    break;
            }

            if (cardExtract == null)
            {
                cardExtract = new CardinalExtractor();
            }

            builder.AddRange(cardExtract.Regexes);

            var fracExtract = new FractionExtractor();
            builder.AddRange(fracExtract.Regexes);

            Regexes = builder.ToImmutable();
        }
    }
}
