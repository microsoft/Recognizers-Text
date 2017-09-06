using System.Collections.Generic;
using System.Collections.Immutable;
using System.Text.RegularExpressions;

namespace Microsoft.Recognizers.Text.Number.Spanish
{
    public class CardinalExtractor : BaseNumberExtractor
    {
        internal sealed override ImmutableDictionary<Regex, string> Regexes { get; }

        protected sealed override string ExtractType { get; } = Constants.SYS_NUM_CARDINAL; //"Cardinal";

        private static readonly Dictionary<string, CardinalExtractor> Instances = new Dictionary<string, CardinalExtractor>();

        public static CardinalExtractor GetInstance(string placeholder = @"\D|\b")
        {

            if (!Instances.ContainsKey(placeholder))
            {
                var instance = new CardinalExtractor(placeholder);
                Instances.Add(placeholder, instance);
            }

            return Instances[placeholder];
        }

        private CardinalExtractor(string placeholder = @"\D|\b")
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