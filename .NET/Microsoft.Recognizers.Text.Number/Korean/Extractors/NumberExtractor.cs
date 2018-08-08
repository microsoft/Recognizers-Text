using System.Collections.Immutable;
using System.Text.RegularExpressions;

namespace Microsoft.Recognizers.Text.Number.Korean
{
    public class NumberExtractor : BaseNumberExtractor
    {
        internal sealed override ImmutableDictionary<Regex, string> Regexes { get; }

        protected sealed override string ExtractType { get; } = Constants.SYS_NUM;

        public NumberExtractor(KoreanNumberExtractorMode mode = KoreanNumberExtractorMode.Default)
        {
            var builder = ImmutableDictionary.CreateBuilder<Regex, string>();

            // Add Cardinal
            var cardExtract = new CardinalExtractor(mode);
            builder.AddRange(cardExtract.Regexes);

            // Add Fraction
            var fracExtract = new FractionExtractor();
            builder.AddRange(fracExtract.Regexes);

            Regexes = builder.ToImmutable();
        }
    }

    // These modes can be applied to KoreanNumberExtractor.
    // The default more urilizes an allow list to avoid extracting numbers in ambiguous/undesired combinations of Korean ideograms.
    // --> such as "십이지장(十二指腸)" is organ name(duodenum, part of small intestine) in Korean, should not be extracted.
    // ExtractAll mode is to be used in cases where extraction should be more aggressive (e.g. in Units extraction).
    public enum KoreanNumberExtractorMode
    {
        Default, // Number extraction with an allow list that filters what numbers to extract.
        ExtractAll, // Extract all number-related terms aggressively.
    }
}
