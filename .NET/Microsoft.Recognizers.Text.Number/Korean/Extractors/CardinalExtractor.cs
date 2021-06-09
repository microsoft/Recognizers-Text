using System.Collections.Immutable;
using System.Text.RegularExpressions;
using Microsoft.Recognizers.Text.Number.Config;

namespace Microsoft.Recognizers.Text.Number.Korean
{
    public class CardinalExtractor : BaseNumberExtractor
    {
        // CardinalExtractor = Int + Double
        public CardinalExtractor(BaseNumberOptionsConfiguration config, CJKNumberExtractorMode mode = CJKNumberExtractorMode.Default)
        {
            var builder = ImmutableDictionary.CreateBuilder<Regex, TypeTag>();

            var intExtract = new IntegerExtractor(config, mode);
            builder.AddRange(intExtract.Regexes);

            var douExtractor = new DoubleExtractor(config);
            builder.AddRange(douExtractor.Regexes);

            Regexes = builder.ToImmutable();
        }

        internal sealed override ImmutableDictionary<Regex, TypeTag> Regexes { get; }

        protected sealed override string ExtractType { get; } = Constants.SYS_NUM_CARDINAL;
    }
}
