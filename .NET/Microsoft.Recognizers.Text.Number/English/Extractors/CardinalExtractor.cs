using System.Collections.Concurrent;
using System.Collections.Immutable;
using System.Text.RegularExpressions;

using Microsoft.Recognizers.Definitions.English;

namespace Microsoft.Recognizers.Text.Number.English
{
    public class CardinalExtractor : BaseNumberExtractor
    {
        internal sealed override ImmutableDictionary<Regex, string> Regexes { get; }

        protected sealed override string ExtractType { get; } = Constants.SYS_NUM_CARDINAL; //"Cardinal";

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

        private CardinalExtractor(string placeholder = NumbersDefinitions.PlaceHolderDefault)
        {
            var builder = ImmutableDictionary.CreateBuilder<Regex, string>();

            //Add Integer Regexes
            var intExtract = IntegerExtractor.GetInstance(placeholder);
            builder.AddRange(intExtract.Regexes);

            //Add Double Regexes
            var douExtract = DoubleExtractor.GetInstance(placeholder);
            builder.AddRange(douExtract.Regexes);

            Regexes = builder.ToImmutable();
        }
    }
}