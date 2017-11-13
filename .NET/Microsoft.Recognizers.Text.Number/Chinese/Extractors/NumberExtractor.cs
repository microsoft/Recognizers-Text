using System.Collections.Immutable;
using System.Text.RegularExpressions;

namespace Microsoft.Recognizers.Text.Number.Chinese
{
    public class NumberExtractor : BaseNumberExtractor
    {
        internal sealed override ImmutableDictionary<Regex, string> Regexes { get; }
        protected sealed override string ExtractType { get; } = Constants.SYS_NUM;

        public NumberExtractor(ChineseNumberMode mode = ChineseNumberMode.Default)
        {
            var builder = ImmutableDictionary.CreateBuilder<Regex, string>();

            //Add Cardinal
            var cardExtractChs = new CardinalExtractor(mode);
            builder.AddRange(cardExtractChs.Regexes);
            
            //Add Fraction
            var fracExtractChs = new FractionExtractor();
            builder.AddRange(fracExtractChs.Regexes);

            Regexes = builder.ToImmutable();
        }
    }

    public enum ChineseNumberMode
    {
        //for number with white list
        Default,
        //for number without white list
        ExtractAll,
    }
}