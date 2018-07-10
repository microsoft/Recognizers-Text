using System.Collections.Immutable;
using System.Text.RegularExpressions;

using Microsoft.Recognizers.Definitions.Portuguese;

namespace Microsoft.Recognizers.Text.Number.Portuguese
{
    public class NumberExtractor : BaseNumberExtractor
    {
        internal sealed override ImmutableDictionary<Regex, string> Regexes { get; }

        protected sealed override string ExtractType { get; } = Constants.SYS_NUM; // "Number";

        public NumberExtractor(NumberMode mode = NumberMode.Default, NumberOptions options = NumberOptions.None)
        {
            var builder = ImmutableDictionary.CreateBuilder<Regex, string>();

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
            var fracExtract = new FractionExtractor();
            builder.AddRange(fracExtract.Regexes);

            this.Regexes = builder.ToImmutable();
        }
    }
}