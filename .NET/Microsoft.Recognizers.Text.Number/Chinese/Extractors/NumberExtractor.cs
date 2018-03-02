using System.Collections.Immutable;
using System.Text.RegularExpressions;

namespace Microsoft.Recognizers.Text.Number.Chinese
{
    public class NumberExtractor : BaseNumberExtractor
    {
        internal sealed override ImmutableDictionary<Regex, string> Regexes { get; }

        protected sealed override string ExtractType { get; } = Constants.SYS_NUM;

        public NumberExtractor(NumberOptions options = NumberOptions.None)
        {
            var builder = ImmutableDictionary.CreateBuilder<Regex, string>();

            //Add Cardinal
            var cardExtractChs = new CardinalExtractor(options);
            builder.AddRange(cardExtractChs.Regexes);
            
            //Add Fraction
            var fracExtractChs = new FractionExtractor();
            builder.AddRange(fracExtractChs.Regexes);

            Regexes = builder.ToImmutable();
        }
    }
}