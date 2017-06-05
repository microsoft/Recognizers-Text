using System.Collections.Immutable;
using System.Text.RegularExpressions;

namespace Microsoft.Recognizers.Text.Number.Spanish
{
    public class CardinalExtractor : BaseNumberExtractor
    {
        internal override sealed ImmutableDictionary<Regex, string> Regexes { get; }

        protected override sealed string ExtractType { get; } = Constants.SYS_NUM_CARDINAL; //"Cardinal";

        public CardinalExtractor(string placeholder = @"\D|\b")
        {
            var builder = ImmutableDictionary.CreateBuilder<Regex, string>();

            //Add Integer Regexes
            var intExtract = new IntegerExtractor(placeholder);
            builder.AddRange(intExtract.Regexes);

            //Add Double Regexes
            var douExtract = new DoubleExtractor(placeholder);
            builder.AddRange(douExtract.Regexes);

            this.Regexes = builder.ToImmutable();
        }
    }
}