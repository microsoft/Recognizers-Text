using System.Collections.Concurrent;
using System.Collections.Immutable;
using System.Text.RegularExpressions;

using Microsoft.Recognizers.Definitions.French;

namespace Microsoft.Recognizers.Text.Number.French
{
    public class CardinalExtractor : BaseNumberExtractor
    {
        internal sealed override ImmutableDictionary<Regex, string> Regexes { get; }

        protected sealed override string ExtractType { get; } = Constants.SYS_NUM_CARDINAL;

        private static readonly ConcurrentDictionary<string, CardinalExtractor> Instances = new ConcurrentDictionary<string, CardinalExtractor>();

        public static CardinalExtractor GetInstance(string placeholder = NumbersDefinitions.PlaceHolderDefault)
        {

            if (!Instances.ContainsKey(placeholder))
            {
                var instance = new CardinalExtractor(placeholder);
                Instances.TryAdd(placeholder, instance);
            }

            return Instances[placeholder];
        }

        public CardinalExtractor(string placeholder = NumbersDefinitions.PlaceHolderDefault)
        {
            var builder = ImmutableDictionary.CreateBuilder<Regex, string>();

            // Add Integer Regexes
            var intExtract = new IntegerExtractor(placeholder);
            builder.AddRange(intExtract.Regexes);

            // Add Double Regexes
            var douExtract = new DoubleExtractor(placeholder);
            builder.AddRange(douExtract.Regexes);

            Regexes = builder.ToImmutable();
        }
    }
}
