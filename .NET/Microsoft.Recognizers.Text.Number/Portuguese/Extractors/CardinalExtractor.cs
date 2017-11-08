using System.Collections.Immutable;
using System.Text.RegularExpressions;

using Microsoft.Recognizers.Definitions.Portuguese;

namespace Microsoft.Recognizers.Text.Number.Portuguese
{
    public class CardinalExtractor : BaseNumberExtractor // Same as Spanish.
    {
        internal sealed override ImmutableDictionary<Regex, string> Regexes { get; }

        protected sealed override string ExtractType { get; } = Constants.SYS_NUM_CARDINAL; //"Cardinal";

        public CardinalExtractor(string placeholder = NumbersDefinitions.PlaceHolderDefault)
        {
            var builder = ImmutableDictionary.CreateBuilder<Regex, string>();

            //Add Integer Regexes
            var intExtract = new IntegerExtractor(placeholder);
            builder.AddRange(intExtract.Regexes);

            //Add Double Regexes
            var douExtract = new DoubleExtractor(placeholder);
            builder.AddRange(douExtract.Regexes);

            Regexes = builder.ToImmutable();
        }
    }
}