using System.Collections.Immutable;
using System.Text.RegularExpressions;

namespace Microsoft.Recognizers.Text.Number.Japanese
{
    // These modes can be applied to JapaneseNumberExtractor.
    // The default more urilizes an allow list to avoid extracting numbers in ambiguous/undesired combinations of Japanese ideograms.
    // --> such as "西九条" is a place name in Japanese, should not be extracted.
    // ExtractAll mode is to be used in cases where extraction should be more aggressive (e.g. in Units extraction).
    public enum JapaneseNumberExtractorMode
    {
        /// <summary>
        /// Number extraction with an allow list that filters what numbers to extract.
        /// </summary>
        Default,

        /// <summary>
        /// Extract all number-related terms aggressively.
        /// </summary>
        ExtractAll,
    }

    public class NumberExtractor : BaseNumberExtractor
    {
        public NumberExtractor(JapaneseNumberExtractorMode mode = JapaneseNumberExtractorMode.Default)
        {
            var builder = ImmutableDictionary.CreateBuilder<Regex, TypeTag>();

            // Add Cardinal
            var cardExtract = new CardinalExtractor(mode);
            builder.AddRange(cardExtract.Regexes);

            // Add Fraction
            var fracExtract = new FractionExtractor();
            builder.AddRange(fracExtract.Regexes);

            Regexes = builder.ToImmutable();
        }

        internal sealed override ImmutableDictionary<Regex, TypeTag> Regexes { get; }

        protected sealed override string ExtractType { get; } = Constants.SYS_NUM;
    }
}
