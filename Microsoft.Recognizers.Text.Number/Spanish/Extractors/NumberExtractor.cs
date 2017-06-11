using System.Collections.Immutable;
using System.Text.RegularExpressions;

namespace Microsoft.Recognizers.Text.Number.Spanish
{
    public class NumberExtractor : BaseNumberExtractor
    {
        internal override sealed ImmutableDictionary<Regex, string> Regexes { get; }

        protected override sealed string ExtractType { get; } = Constants.SYS_NUM; // "Number";

        public NumberExtractor(NumberMode mode = NumberMode.Default)
        {
            var builder = ImmutableDictionary.CreateBuilder<Regex, string>();

            //Add Cardinal
            CardinalExtractor cardExtract = null;
            switch (mode)
            {
                case NumberMode.PureNumber:
                    cardExtract = new CardinalExtractor(@"\b");
                    break;
                case NumberMode.Currency:
                    builder.Add(new Regex(@"(((?<=\W|^)-\s*)|(?<=\b))\d+\s*(B|b|m|t|g)(?=\b)", RegexOptions.Singleline),
                        "IntegerNum");
                    break;
                case NumberMode.Default:
                    break;
            }
            if (cardExtract == null)
            {
                cardExtract = new CardinalExtractor();
            }
            builder.AddRange(cardExtract.Regexes);

            //Add Fraction
            var fracExtract = new FractionExtractor();
            builder.AddRange(fracExtract.Regexes);

            this.Regexes = builder.ToImmutable();
        }
    }
}