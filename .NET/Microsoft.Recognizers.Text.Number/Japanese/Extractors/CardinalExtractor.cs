using System.Collections.Immutable;
using System.Text.RegularExpressions;

namespace Microsoft.Recognizers.Text.Number.Japanese
{
    public class CardinalExtractor : BaseNumberExtractor
    {
        internal sealed override ImmutableDictionary<Regex, string> Regexes { get; }

        protected sealed override string ExtractType { get; } = Constants.SYS_NUM_CARDINAL;

        // CardinalExtractor = Int + Double
        public CardinalExtractor(JapaneseNumberExtractorMode mode = JapaneseNumberExtractorMode.Default)
        {
            var builder = ImmutableDictionary.CreateBuilder<Regex, string>();

            var intExtract = new IntegerExtractor(mode);
            builder.AddRange(intExtract.Regexes);

            var douExtractor = new DoubleExtractor();
            builder.AddRange(douExtractor.Regexes);

            Regexes = builder.ToImmutable();
        }
    }
}
