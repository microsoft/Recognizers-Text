using System.Collections.Immutable;
using System.Text.RegularExpressions;

using Microsoft.Recognizers.Text.Number.Config;

namespace Microsoft.Recognizers.Text.Number.Chinese
{

    public class CardinalExtractor : BaseNumberExtractor
    {

        // CardinalExtractor = Int + Double
        public CardinalExtractor(BaseNumberOptionsConfiguration config, CJKNumberExtractorMode mode = CJKNumberExtractorMode.Default)
        {

            var builder = ImmutableDictionary.CreateBuilder<Regex, TypeTag>();

            var intExtractChs = new IntegerExtractor(config, mode);
            builder.AddRange(intExtractChs.Regexes);

            var douExtractorChs = new DoubleExtractor(config);
            builder.AddRange(douExtractorChs.Regexes);

            Regexes = builder.ToImmutable();
        }

        internal sealed override ImmutableDictionary<Regex, TypeTag> Regexes { get; }

        protected sealed override string ExtractType { get; } = Constants.SYS_NUM_CARDINAL;
    }
}