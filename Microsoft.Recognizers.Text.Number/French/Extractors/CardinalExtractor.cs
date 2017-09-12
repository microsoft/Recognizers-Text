﻿using System.Collections.Immutable;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace Microsoft.Recognizers.Text.Number.French
{
    public class CardinalExtractor : BaseNumberExtractor
    {
        internal sealed override ImmutableDictionary<Regex, string> Regexes { get; }

        protected sealed override string ExtractType { get; } = Constants.SYS_NUM_CARDINAL;

        private static readonly Dictionary<string, CardinalExtractor> Instances = new Dictionary<string, CardinalExtractor>();

        public static CardinalExtractor GetInstance(string placeholder = @"\D|/b") // TODO: later add NumbersDefinitions.PlaceHolderDefault
        {

            if (!Instances.ContainsKey(placeholder))
            {
                var instance = new CardinalExtractor(placeholder);
                Instances.Add(placeholder, instance);
            }

            return Instances[placeholder];
        }

        public CardinalExtractor(string placeholder = @"\D|/b")
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
