using System.Collections.Immutable;
using System.Text.RegularExpressions;

using Microsoft.Recognizers.Text.Number.Config;

namespace Microsoft.Recognizers.Text.Number.Chinese
{

    public class NumberExtractor : BaseNumberExtractor
    {
        public NumberExtractor(BaseNumberOptionsConfiguration config, CJKNumberExtractorMode mode = CJKNumberExtractorMode.Default)
        {
            var builder = ImmutableDictionary.CreateBuilder<Regex, TypeTag>();

            // Add Cardinal
            var cardExtractChs = new CardinalExtractor(config, mode);
            builder.AddRange(cardExtractChs.Regexes);

            // Add Fraction
            var fracExtractChs = new FractionExtractor(config);
            builder.AddRange(fracExtractChs.Regexes);

            Regexes = builder.ToImmutable();
        }

        internal sealed override ImmutableDictionary<Regex, TypeTag> Regexes { get; }

        protected sealed override string ExtractType { get; } = Constants.SYS_NUM;
    }
}