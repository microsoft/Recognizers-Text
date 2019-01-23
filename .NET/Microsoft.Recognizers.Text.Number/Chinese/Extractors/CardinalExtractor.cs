using System.Collections.Immutable;
using System.Text.RegularExpressions;

namespace Microsoft.Recognizers.Text.Number.Chinese
{
    public class CardinalExtractor : BaseNumberExtractor
    {
        // CardinalExtractor = Int + Double
        public CardinalExtractor(ChineseNumberExtractorMode mode = ChineseNumberExtractorMode.Default)
        {
            var builder = ImmutableDictionary.CreateBuilder<Regex, TypeTag>();

            var intExtractChs = new IntegerExtractor(mode);
            builder.AddRange(intExtractChs.Regexes);

            var douExtractorChs = new DoubleExtractor();
            builder.AddRange(douExtractorChs.Regexes);

            Regexes = builder.ToImmutable();
        }

        internal sealed override ImmutableDictionary<Regex, TypeTag> Regexes { get; }

        protected sealed override string ExtractType { get; } = Constants.SYS_NUM_CARDINAL;
    }
}