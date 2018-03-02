using System.Collections.Immutable;
using System.Text.RegularExpressions;

namespace Microsoft.Recognizers.Text.Number.Chinese
{
    public class CardinalExtractor : BaseNumberExtractor
    {
        internal sealed override ImmutableDictionary<Regex, string> Regexes { get; }
        protected sealed override string ExtractType { get; } = Constants.SYS_NUM_CARDINAL;

        //CardinalExtractor = Int + Double
        public CardinalExtractor(NumberOptions options = NumberOptions.None)
        {
            var builder = ImmutableDictionary.CreateBuilder<Regex, string>();

            var intExtractChs = new IntegerExtractor(options);
            builder.AddRange(intExtractChs.Regexes);

            var douExtractorChs = new DoubleExtractor();
            builder.AddRange(douExtractorChs.Regexes);

            Regexes = builder.ToImmutable();
        }
    }
}