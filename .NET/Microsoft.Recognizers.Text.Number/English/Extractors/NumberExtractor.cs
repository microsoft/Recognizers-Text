using System.Collections.Concurrent;
using System.Collections.Immutable;
using System.Text.RegularExpressions;

using Microsoft.Recognizers.Definitions.English;

namespace Microsoft.Recognizers.Text.Number.English
{
    public class NumberExtractor : BaseNumberExtractor
    {
        internal sealed override ImmutableDictionary<Regex, string> Regexes { get; }

        protected sealed override NumberOptions Options { get; }

        protected sealed override string ExtractType { get; } = Constants.SYS_NUM; // "Number";

        protected sealed override Regex NegativeNumberTermsRegex { get; }

        private static readonly ConcurrentDictionary<string, NumberExtractor> Instances = new ConcurrentDictionary<string, NumberExtractor>();

        public static NumberExtractor GetInstance(NumberMode mode = NumberMode.Default, NumberOptions options = NumberOptions.None) {

            var placeholder = mode.ToString();

            if (!Instances.ContainsKey(placeholder))
            {
                var instance = new NumberExtractor(mode, options);
                Instances.TryAdd(placeholder, instance);
            }

            return Instances[placeholder];
        }

        private NumberExtractor(NumberMode mode, NumberOptions options)
        {
            NegativeNumberTermsRegex = new Regex(NumbersDefinitions.NegativeNumberTermsRegex + '$', RegexOptions.IgnoreCase | RegexOptions.Singleline);

            Options = options;

            var builder = ImmutableDictionary.CreateBuilder<Regex, string>();
            
            //Add Cardinal
            CardinalExtractor cardExtract = null;
            switch (mode)
            {
                case NumberMode.PureNumber:
                    cardExtract = CardinalExtractor.GetInstance(NumbersDefinitions.PlaceHolderPureNumber);
                    break;
                case NumberMode.Currency:
                    builder.Add(new Regex(NumbersDefinitions.CurrencyRegex, RegexOptions.Singleline), "IntegerNum");
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

            Regexes = builder.ToImmutable();
        }
    }
}