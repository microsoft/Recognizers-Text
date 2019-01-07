using System.Collections.Immutable;
using System.Text.RegularExpressions;

namespace Microsoft.Recognizers.Text.Number.Chinese
{
    /// <summary>
    /// These modes only apply to ChineseNumberExtractor.
    /// The default more urilizes an allow list to avoid extracting numbers in ambiguous/undesired combinations of Chinese ideograms.
    /// ExtractAll mode is to be used in cases where extraction should be more aggressive (e.g. in Units extraction).
    /// </summary>
    public enum ChineseNumberExtractorMode
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
        public NumberExtractor(ChineseNumberExtractorMode mode = ChineseNumberExtractorMode.Default)
        {
            var builder = ImmutableDictionary.CreateBuilder<Regex, TypeTag>();

            // Add Cardinal
            var cardExtractChs = new CardinalExtractor(mode);
            builder.AddRange(cardExtractChs.Regexes);

            // Add Fraction
            var fracExtractChs = new FractionExtractor();
            builder.AddRange(fracExtractChs.Regexes);

            Regexes = builder.ToImmutable();
        }

        internal sealed override ImmutableDictionary<Regex, TypeTag> Regexes { get; }

        protected sealed override string ExtractType { get; } = Constants.SYS_NUM;
    }
}